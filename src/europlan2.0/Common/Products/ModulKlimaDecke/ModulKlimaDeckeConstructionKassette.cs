using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WW.Math;
using System.Drawing;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	public class ModulKlimaDeckeConstructionKassette : ModulKlimaDeckeConstruction {

		public enum RasterMass {
			Raster_1050_450,
			Raster_625,
			Raster_600
		}

		private double schienenBreiteX = 0.05; // meter
		private double schienenBreiteY = 0.05 - 0.0001; // meter
		private double schienenAbstandX = 0.4; // meter
		private double schienenAbstandY = 1.0 + 0.0001; // meter
		private double offsetX = 0; // meter
		private double offsetY = 0; // meter
		protected List<Polygon2D> schienenY = new List<Polygon2D>();

		public ModulKlimaDeckeConstructionKassette() {
		}

		[XmlIgnore]
		public override List<Point2D> CeilingCoordinates {
			get {
				if (this.Planner == null || this.Planner.Product == null || this.Planner.Product.AssociatedRoom == null || this.Planner.Product.AssociatedRoom.CeilingCoordinatesToUse == null) {
					return null;
				}
				return this.Planner.Product.AssociatedRoom.CeilingCoordinatesToUse;
			}
		}

		public override void RecalculateSchienen() {
			if (this.Planner == null || this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.Planner.ConnectedPlanPanel == null ||
				this.Planner.ConnectedPlanPanel.Plan == null) {
				return;
			}
			Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);

			Point2D tmp;
			double maxX = double.MinValue;
			double minX = double.MaxValue;
			double maxY = double.MinValue;
			double minY = double.MaxValue;
			foreach (Point2D point in this.CeilingCoordinates) {
				tmp = matrix.Transform(point);
				if (tmp.X > maxX) {
					maxX = tmp.X;
				}
				if (tmp.X < minX) {
					minX = tmp.X;
				}
				if (tmp.Y > maxY) {
					maxY = tmp.Y;
				}
				if (tmp.Y < minY) {
					minY = tmp.Y;
				}
			}
			matrix = matrix.GetInverse();

			this.schienen.Clear();
			this.schienenY.Clear();
			this.possibleLanes.Clear();

			double measure = this.Planner.ConnectedPlanPanel.Plan.Measure.Value;
			double increment = (schienenBreiteX + schienenAbstandX) * measure;
			double curPos = (minX + maxX - schienenBreiteX * measure) / 2.0 + (offsetX * measure);
			while (curPos > minX) {
				curPos -= increment;
			}
			//curPos = curPos - increment;
			while (curPos < maxX) {
				Polygon2D schiene = new Polygon2D();
				schiene.Add(matrix.Transform(new Point2D(curPos, minY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + schienenBreiteX * measure, minY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + schienenBreiteX * measure, maxY)));
				schiene.Add(matrix.Transform(new Point2D(curPos, maxY)));
				this.schienen.Add(schiene);
				curPos += increment;
			}

			increment = (schienenBreiteY + schienenAbstandY) * measure;
			curPos = (minY + maxY - schienenBreiteY * measure) / 2.0 + (offsetY * measure);
			while (curPos > minY) {
				curPos -= increment;
			}
			while (curPos < maxY) {
				Polygon2D schiene = new Polygon2D();
				schiene.Add(matrix.Transform(new Point2D(minX, curPos)));
				schiene.Add(matrix.Transform(new Point2D(minX, curPos + schienenBreiteY * measure)));
				schiene.Add(matrix.Transform(new Point2D(maxX, curPos + schienenBreiteY * measure)));
				schiene.Add(matrix.Transform(new Point2D(maxX, curPos)));
				this.schienenY.Add(schiene);
				curPos += increment;
			}

			Polygon2D room = new Polygon2D(this.CeilingCoordinates);
			if (room.IsClockwise()) {
				room = room.GetReverse();
			}

			for (int i = 0; i < this.schienen.Count - 1; i++) {
				Polygon2D schieneLeft = this.schienen[i];
				Polygon2D schieneRight = this.schienen[i + 1];
				Polygon2D lane = new Polygon2D();
				lane.Add(schieneLeft[1]);
				lane.Add(schieneLeft[2]);
				lane.Add(schieneRight[3]);
				lane.Add(schieneRight[0]);
				if (lane.IsClockwise()) {
					lane = lane.GetReverse();
				}

				this.possibleLanes.Add(new PossibleModulLane(GetPossibleModuleAreasInLane(lane), i));
			}
		}

		private List<PossibleModulLaneArea> GetPossibleModuleAreasInLane(Polygon2D lane) {
			Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);

			Polygon2D tmp = new Polygon2D();
			double startPointX = double.MaxValue;
			double endPointX = double.MinValue;
			int start = -1;
			int i = 0;
			foreach (Point2D point in this.CeilingCoordinates) {
				Point2D pointTf = matrix.Transform(point);
				if (pointTf.X < startPointX) {
					startPointX = pointTf.X;
					start = i;
				}
				if (pointTf.X > endPointX) {
					endPointX = pointTf.X;
				}
				tmp.Add(matrix.Transform(point));
				i++;
			}
			Polygon2D room = new Polygon2D();
			if (tmp.IsClockwise()) {
				for (i = tmp.Count; i > 0; i--) {
					room.Add(tmp[(start + i) % tmp.Count]);
				}
			} else {
				for (i = 0; i < tmp.Count; i++) {
					room.Add(tmp[(start + i) % tmp.Count]);
				}
			}

			Point2D p = matrix.Transform(lane[0]);
			Line2D borderRight = new Line2D(p, p - matrix.Transform(lane[1]));
			p = matrix.Transform(lane[3]);
			Line2D borderLeft = new Line2D(p, p - matrix.Transform(lane[2]));

			Segment2D roomBorder;
			Nullable<Point2D> intersection = null;
			if (startPointX > borderLeft.Origin.X || endPointX < borderRight.Origin.X) {
				// no possible module areas in lane found
				return new List<PossibleModulLaneArea>();
			}

			bool inside = false;
			List<double> bordersTop = new List<double>();
			List<double> bordersBottom = new List<double>();
			List<CompareablePair<double>> removes = new List<CompareablePair<double>>();
			List<double> lp = new List<double>();
			bool enteredLeft = true;
			for (i = 0; i < room.Count; i++) {
				roomBorder = new Segment2D(room[i], room[(i + 1) % room.Count]);
				if (roomBorder.Start.X < roomBorder.End.X) {
					this.CheckLeftBorder(borderLeft, roomBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
					this.CheckRightBorder(borderRight, roomBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
				} else {
					this.CheckRightBorder(borderRight, roomBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
					this.CheckLeftBorder(borderLeft, roomBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
				}
				if (inside) {
					lp.Add(room[i == room.Count - 1 ? 0 : i + 1].Y);
				}
			}

			if (this.Planner.Product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
				foreach (List<Point2D> unusedArea in this.Planner.Product.AssociatedRoom.CeilingUnusedAreaCoordinates) {
					tmp = new Polygon2D();
					startPointX = double.MaxValue;
					endPointX = double.MinValue;
					i = 0;
					foreach (Point2D unusedPoint in unusedArea) {
						Point2D pointTf = matrix.Transform(unusedPoint);
						if (pointTf.X < startPointX) {
							startPointX = pointTf.X;
							start = i;
						}
						if (pointTf.X > endPointX) {
							endPointX = pointTf.X;
						}
						tmp.Add(matrix.Transform(unusedPoint));
						i++;
					}

					if (startPointX <= borderRight.Origin.X && endPointX >= borderLeft.Origin.X) {
						Polygon2D unused = new Polygon2D();
						if (!tmp.IsClockwise()) {
							for (i = tmp.Count; i > 0; i--) {
								unused.Add(tmp[(start + i) % tmp.Count]);
							}
						} else {
							for (i = 0; i < tmp.Count; i++) {
								unused.Add(tmp[(start + i) % tmp.Count]);
							}
						}

						Segment2D unusedBorder;
						inside = false;
						for (i = 0; i < unused.Count; i++) {
							unusedBorder = new Segment2D(unused[i], unused[(i + 1) % unused.Count]);
							if (unusedBorder.Start.X < unusedBorder.End.X) {
								this.CheckLeftBorder(borderLeft, unusedBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
								this.CheckRightBorder(borderRight, unusedBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
							} else {
								this.CheckRightBorder(borderRight, unusedBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
								this.CheckLeftBorder(borderLeft, unusedBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
							}
							if (inside) {
								lp.Add(unused[i == unused.Count - 1 ? 0 : i + 1].Y);
							}
						}
					}
				}
			}

			foreach (Polygon2D schieneY in this.schienenY) {
				tmp = new Polygon2D();
				startPointX = double.MaxValue;
				endPointX = double.MinValue;
				i = 0;
				foreach (Point2D point in schieneY) {
					Point2D pointTf = matrix.Transform(point);
					if (pointTf.X < startPointX) {
						startPointX = pointTf.X;
						start = i;
					}
					if (pointTf.X > endPointX) {
						endPointX = pointTf.X;
					}
					tmp.Add(matrix.Transform(point));
					i++;
				}

				if (startPointX <= borderRight.Origin.X && endPointX >= borderLeft.Origin.X) {
					Polygon2D unused = new Polygon2D();
					if (!tmp.IsClockwise()) {
						for (i = tmp.Count; i > 0; i--) {
							unused.Add(tmp[(start + i) % tmp.Count]);
						}
					} else {
						for (i = 0; i < tmp.Count; i++) {
							unused.Add(tmp[(start + i) % tmp.Count]);
						}
					}

					Segment2D unusedBorder;
					inside = false;
					for (i = 0; i < unused.Count; i++) {
						unusedBorder = new Segment2D(unused[i], unused[(i + 1) % unused.Count]);
						if (unusedBorder.Start.X < unusedBorder.End.X) {
							this.CheckLeftBorder(borderLeft, unusedBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
							this.CheckRightBorder(borderRight, unusedBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
						} else {
							this.CheckRightBorder(borderRight, unusedBorder, ref inside, bordersTop, removes, ref lp, ref enteredLeft);
							this.CheckLeftBorder(borderLeft, unusedBorder, ref inside, bordersBottom, removes, ref lp, ref enteredLeft);
						}
						if (inside) {
							lp.Add(unused[i == unused.Count - 1 ? 0 : i + 1].Y);
						}
					}
				}
			}

			bordersTop.Sort();
			bordersBottom.Sort();
			removes.Sort();

			List<PossibleModulLaneArea> possibleAreas = new List<PossibleModulLaneArea>();
			matrix = matrix.GetInverse();
			for (i = 0; i < bordersTop.Count; i++) {
				double top = bordersTop[i];
				double bottom;
				//Polygon2D area;
				foreach (CompareablePair<double> remove in removes) {
					if (remove.value1 > bordersTop[i] && remove.value1 < bordersBottom[i]) {
						bottom = remove.value2;
						possibleAreas.Add(new PossibleModulLaneArea(
							matrix.Transform(new Point2D(borderLeft.Origin.X, top)),
							matrix.Transform(new Point2D(borderLeft.Origin.X, bottom)),
							matrix.Transform(new Point2D(borderRight.Origin.X, bottom)),
							matrix.Transform(new Point2D(borderRight.Origin.X, top)),
							top, bottom));
						top = remove.value1;
					}
				}
				bottom = bordersBottom[i];
				possibleAreas.Add(new PossibleModulLaneArea(
					matrix.Transform(new Point2D(borderLeft.Origin.X, top)),
					matrix.Transform(new Point2D(borderLeft.Origin.X, bottom)),
					matrix.Transform(new Point2D(borderRight.Origin.X, bottom)),
					matrix.Transform(new Point2D(borderRight.Origin.X, top)),
					top, bottom));

			}

			/*if (this.Planner.Product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
				foreach (
			}*/

			return possibleAreas;
		}

		private void CheckLeftBorder(Line2D borderLeft, Segment2D roomBorder, ref bool inside, List<double> bordersTop, List<CompareablePair<double>> removes, ref List<double> possiblePoints, ref bool enteredLeft) {
			Nullable<Point2D> intersection = Line2D.GetIntersection(borderLeft, roomBorder);
			if (intersection.HasValue) {
				possiblePoints.Add(intersection.Value.Y);
				if (inside) {
					if (enteredLeft) {
						removes.Add(new CompareablePair<double>(GetMax(possiblePoints), GetMin(possiblePoints)));
					} else {
						bordersTop.Add(GetMin(possiblePoints));
					}
					possiblePoints = new List<double>();
					inside = false;
				} else {
					inside = true;
					enteredLeft = true;
				}
			}
		}

		private void CheckRightBorder(Line2D borderRight, Segment2D roomBorder, ref bool inside, List<double> bordersBottom, List<CompareablePair<double>> removes, ref List<double> possiblePoints, ref bool enteredLeft) {
			Nullable<Point2D> intersection = Line2D.GetIntersection(borderRight, roomBorder);
			if (intersection.HasValue) {
				possiblePoints.Add(intersection.Value.Y);
				if (inside) {
					if (enteredLeft) {
						bordersBottom.Add(GetMax(possiblePoints));
					} else {
						removes.Add(new CompareablePair<double>(GetMax(possiblePoints), GetMin(possiblePoints)));
					}
					possiblePoints = new List<double>();
					inside = false;
				} else {
					inside = true;
					enteredLeft = false;
				}
			}
		}

		private double GetMin(List<double> values) {
			double min = double.MaxValue;
			foreach (double val in values) {
				if (val < min) {
					min = val;
				}
			}
			return min;
		}

		private double GetMax(List<double> values) {
			double max = double.MinValue;
			foreach (double val in values) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}

		[XmlIgnore]
		public double SchienenBreiteX {
			get { return this.schienenBreiteX; }
			set {
				this.schienenBreiteX = value;
				this.RecalculateSchienen();
			}
		}

		public double SchienenAbstandX {
			get { return this.schienenAbstandX; }
			set {
				this.schienenAbstandX = value;
				this.RecalculateSchienen();
			}
		}

		[XmlIgnore]
		public double SchienenBreiteY {
			get { return this.schienenBreiteY; }
			set {
				this.schienenBreiteY = value;
				this.RecalculateSchienen();
			}
		}

		public double SchienenAbstandY {
			get { return this.schienenAbstandY; }
			set {
				this.schienenAbstandY = value;
				this.RecalculateSchienen();
			}
		}

		public double OffsetX {
			get { return this.offsetX; }
			set {
				this.offsetX = value;
				double increment = this.schienenBreiteX + this.schienenAbstandX;
				while (this.offsetX >= increment) {
					this.offsetX -= increment;
				}
				while (this.offsetX < 0) {
					this.offsetX += increment;
				}
				this.RecalculateSchienen();
			}
		}

		public double OffsetY {
			get { return this.offsetY; }
			set {
				this.offsetY = value;
				double increment = this.schienenBreiteY + this.schienenAbstandY;
				while (this.offsetY >= increment) {
					this.offsetY -= increment;
				}
				while (this.offsetY < 0) {
					this.offsetY += increment;
				}
				this.RecalculateSchienen();
			}
		}

		protected GraphicsPath GetProductAreaPath() {
			List<PointF> transformedPoints = new List<PointF>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Point2D point in this.CeilingCoordinates) {
				Point2D tmp = additionalTransformation.TransformTo2D(point);
				transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(transformedPoints.ToArray());
			return path;
		}

		protected GraphicsPath GetCeilingPath() {
			List<PointF> transformedPoints = new List<PointF>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Point2D point in this.Planner.Product.AssociatedRoom.CeilingCoordinatesToUse) {
				Point2D tmp = additionalTransformation.TransformTo2D(point);
				transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(transformedPoints.ToArray());
			return path;
		}

		private List<Polygon2D> GetSchienen(bool forDrawing) {
			if (!forDrawing) {
				return this.schienen;
			}

			Polygon2D schieneForDrawing;
			List<Polygon2D> schienenForDrawing = new List<Polygon2D>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Polygon2D schiene in this.schienen) {
				schieneForDrawing = new Polygon2D();
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[0]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[1]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[2]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[3]));
				schienenForDrawing.Add(schieneForDrawing);
			}
			return schienenForDrawing;
		}

		private List<Polygon2D> GetSchienenY(bool forDrawing) {
			if (!forDrawing) {
				return this.schienenY;
			}

			Polygon2D schieneForDrawing;
			List<Polygon2D> schienenForDrawing = new List<Polygon2D>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Polygon2D schiene in this.schienenY) {
				schieneForDrawing = new Polygon2D();
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[0]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[1]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[2]));
				schieneForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)schiene[3]));
				schienenForDrawing.Add(schieneForDrawing);
			}
			return schienenForDrawing;
		}

		private List<Polygon2D> GetPossibleAreas(bool forDrawing) {
			List<Polygon2D> possibleAreas = new List<Polygon2D>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (PossibleModulLane possibleLane in this.possibleLanes) {
				foreach (PossibleModulLaneArea possibleArea in possibleLane.Areas) {
					if (forDrawing) {
						Polygon2D a = new Polygon2D();
						a.Add(additionalTransformation.TransformTo2D((Point3D)possibleArea.Area[0]));
						a.Add(additionalTransformation.TransformTo2D((Point3D)possibleArea.Area[1]));
						a.Add(additionalTransformation.TransformTo2D((Point3D)possibleArea.Area[2]));
						a.Add(additionalTransformation.TransformTo2D((Point3D)possibleArea.Area[3]));
						possibleAreas.Add(a);
					} else {
						possibleAreas.Add(possibleArea.Area);
					}
				}
			}
			return possibleAreas;
		}

		public override void Paint(Graphics g, ModulKlimaDeckePlanner.KlimaDeckeMode mode, bool drawBeplankung) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.CeilingCoordinates.Count < 3 ||
				this.Planner.ConnectedPlanPanel == null ||
				this.Planner.ConnectedPlanPanel.Plan == null ||
				this.Planner.ConnectedPlanPanel.Plan.Measure == null) {
				return;
			}
			double measure = this.Planner.ConnectedPlanPanel.Plan.Measure.Value;
			Matrix4D additionalTransformation = this.AdditionalTransformation;

			/*double minX, maxX, minY, maxY;*/
			GraphicsPath roomPath = this.GetProductAreaPath(/*out minX, out maxX, out minY, out maxY*/);
			g.Clip = new Region(roomPath);

			Color c = Color.Gray;
			Pen p = new Pen(c);
			Brush b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, c, Color.FromArgb(0, c));

			//g.FillPath(new SolidBrush(Color.FromArgb(128, Color.Yellow)), roomPath);

			foreach (Polygon2D schiene in this.GetSchienen(true)) {
				PointF[] poly = new PointF[schiene.Count];
				int i = 0;
				foreach (Point2D point in schiene) {
					poly[i++] = new PointF((float)point.X, (float)point.Y);
				}
				//g.DrawPolygon(p, poly);
				//g.FillPolygon(b, poly);
				g.FillPolygon(new SolidBrush(Color.FromArgb(127, Color.Gray)), poly);
			}

			foreach (Polygon2D schieneY in this.GetSchienenY(true)) {
				PointF[] poly = new PointF[schieneY.Count];
				int i = 0;
				foreach (Point2D point in schieneY) {
					poly[i++] = new PointF((float)point.X, (float)point.Y);
				}
				g.DrawPolygon(p, poly);
				g.FillPolygon(b, poly);
			}

			if (mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION) {
				c = Color.FromArgb(128, 0, 240, 0);
				p = new Pen(c);
				b = new SolidBrush(Color.FromArgb(64, c));
				//Region r = new Region();
				//r.MakeInfinite();
				//g.Clip = r;
				foreach (Polygon2D area in this.GetPossibleAreas(true)) {
					PointF[] poly = new PointF[area.Count];
					int i = 0;
					foreach (Point2D point in area) {
						poly[i++] = new PointF((float)point.X, (float)point.Y);
					}
					g.DrawPolygon(p, poly);
					g.FillPolygon(b, poly);
				}
			}
		}

		public override void PaintDxf(DxfModel model, DxfLayer layer, bool drawBeplankung) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.CeilingCoordinates.Count < 3 ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			double measure = this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Polygon2D clipRegion = new Polygon2D();
			foreach (Point2D point in this.CeilingCoordinates) {
				clipRegion.Add(point);
			}
			if (clipRegion.IsClockwise()) {
				clipRegion.Reverse();
			}

			Color c = Color.Gray;

			DxfHatch hatch = new DxfHatch();
			hatch.Color = c;
			DxfHatch hatchY = new DxfHatch();
			hatchY.Color = c;

			foreach (Polygon2D schiene in this.GetSchienen(false)) {
				List<Point2D> dxfPoints = new List<Point2D>();
				foreach (Point2D point in schiene) {
					dxfPoints.Add(point);
				}
				Polygon2D clipped = new Polygon2D(dxfPoints);
				if (clipped.IsClockwise()) {
					clipped.Reverse();
				}
				List<Polygon2D> list1 = new List<Polygon2D>();
				list1.Add(clipRegion);
				List<Polygon2D> list2 = new List<Polygon2D>();
				list2.Add(clipped);

				IList<Polygon2D> clippedPolygons = Polygon2D.GetIntersection(list1, list2);
				foreach (Polygon2D polygon in clippedPolygons) {
					DxfPolyline2D polyLine = new DxfPolyline2D(c, polygon);
					polyLine.Closed = true;
					polyLine.Layer = layer;
					model.Entities.Add(polyLine);

					DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath();
					boundaryPath.Type = BoundaryPathType.Polyline;
					boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(polygon.ToArray());
					boundaryPath.PolylineData.Closed = true;
					hatch.BoundaryPaths.Add(boundaryPath);
				}
			}

			foreach (Polygon2D schieneY in this.GetSchienenY(false)) {
				List<Point2D> dxfPoints = new List<Point2D>();
				foreach (Point2D point in schieneY) {
					dxfPoints.Add(point);
				}
				Polygon2D clipped = new Polygon2D(dxfPoints);
				if (clipped.IsClockwise()) {
					clipped.Reverse();
				}
				List<Polygon2D> list1 = new List<Polygon2D>();
				list1.Add(clipRegion);
				List<Polygon2D> list2 = new List<Polygon2D>();
				list2.Add(clipped);

				IList<Polygon2D> clippedPolygons = Polygon2D.GetIntersection(list1, list2);
				foreach (Polygon2D polygon in clippedPolygons) {
					DxfPolyline2D polyLine = new DxfPolyline2D(c, polygon);
					polyLine.Closed = true;
					polyLine.Layer = layer;
					model.Entities.Add(polyLine);

					DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath();
					boundaryPath.Type = BoundaryPathType.Polyline;
					boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(polygon.ToArray());
					boundaryPath.PolylineData.Closed = true;
					hatchY.BoundaryPaths.Add(boundaryPath);
				}
			}

			hatch.Pattern = new DxfPattern();
			DxfPattern.Line patternLine = new DxfPattern.Line();
			patternLine.Angle = Math.PI / 4d;
			patternLine.Offset = new Vector2D(0.02 * measure, -0.02d * measure);
			hatch.Pattern.Lines.Add(patternLine);
			patternLine = new DxfPattern.Line();
			patternLine.Angle = 3d * Math.PI / 4d;
			patternLine.Offset = new Vector2D(0.02 * measure, 0.02d * measure);
			hatch.Pattern.Lines.Add(patternLine);

			hatch.Layer = layer;
			model.Entities.Add(hatch);

			hatchY.Pattern = new DxfPattern();
			patternLine = new DxfPattern.Line();
			patternLine.Angle = Math.PI / 4d;
			patternLine.Offset = new Vector2D(0.02 * measure, -0.02d * measure);
			hatchY.Pattern.Lines.Add(patternLine);
			patternLine = new DxfPattern.Line();
			patternLine.Angle = 3d * Math.PI / 4d;
			patternLine.Offset = new Vector2D(0.02 * measure, 0.02d * measure);
			hatchY.Pattern.Lines.Add(patternLine);

			hatchY.Layer = layer;
			model.Entities.Add(hatchY);
		}

		public override bool HitTest(Point2D planPoint, Point pointInControl) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.CeilingCoordinates == null ||
				this.CeilingCoordinates.Count < 3 ||
				this.Planner.ConnectedPlanPanel == null ||
				this.Planner.ConnectedPlanPanel.Plan == null ||
				this.Planner.ConnectedPlanPanel.Plan.Measure == null) {
				return false;
			}

			GraphicsPath roomPath = this.GetProductAreaPath();
			if (!roomPath.IsVisible(pointInControl)) {
				return false;
			}

			List<Polygon2D> schienen = this.GetSchienen(false);
			foreach (Polygon2D schiene in schienen) {
				if (schiene.IsInside(planPoint)) {
					return true;
				}
			}
			return false;
		}

		Point2D startPlanPoint;
		Point startPointInControl;
		double startOffsetX;
		double startOffsetY;

		public override void StartDrag(Point2D planPoint, Point pointInControl) {
			startPlanPoint = planPoint;
			startPointInControl = pointInControl;
			startOffsetX = this.offsetX;
			startOffsetY = this.offsetY;
		}

		public override void MoveDrag(Point2D planPoint, Point pointInControl) {
			/*double dist = planPoint.X - startPlanPoint.X;
			this.Offset = startOffset + (dist / this.Planner.ConnectedPlanPanel.Plan.Measure.Value);*/

			Vector2D move = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0).Transform(planPoint - startPlanPoint);
			this.OffsetX = startOffsetX + (move.X / this.Planner.ConnectedPlanPanel.Plan.Measure.Value);
			this.OffsetY = startOffsetY + (move.Y / this.Planner.ConnectedPlanPanel.Plan.Measure.Value);

			this.Planner.ConnectedPlanPanel.InvalidateGraphics();
		}

		public override void EndDrag(Point2D planPoint, Point pointInControl) {
		}

		[XmlIgnore]
		public override Cursor PickCursor {
			get { return Cursors.NoMove2D; }
		}

		[XmlIgnore]
		public override double RotationRelativeToPlan {
			get {
				if (this.Planner.ConnectedPlanPanel.Plan is ImagePlan) {
					return this.rotation + (this.Planner.ConnectedPlanPanel.Plan as ImagePlan).Rotation + 90.0;
				} else if (this.Planner.ConnectedPlanPanel.Plan is CadPlan) {
					return -this.rotation + 90.0;
				}
				return this.rotation + 90.0;
			}
			set {
				if (this.Planner.ConnectedPlanPanel.Plan is ImagePlan) {
					this.Rotation = value - (this.Planner.ConnectedPlanPanel.Plan as ImagePlan).Rotation + 90.0;
				} else if (this.Planner.ConnectedPlanPanel.Plan is CadPlan) {
					this.Rotation = -value - 90.0;
				} else {
					this.Rotation = value - 90.0;
				}
			}
		}
	}
}
