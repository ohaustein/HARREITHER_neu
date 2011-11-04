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
	public class ModulKlimaBodenConstructionStaffeln : ModulKlimaBodenConstruction {
		private double staffelnBreite = 0.045; // meter
		private double staffelnAbstand = 0.5; // meter
		private double offset = 0; // meter
		private List<Polygon2D> staffeln = new List<Polygon2D>();

		public ModulKlimaBodenConstructionStaffeln() {
		}

		[XmlIgnore]
		public override List<Point2D> RoomCoordinates {
			get {
				if (this.Planner == null || this.Planner.Product == null || this.Planner.Product.AssociatedRoom == null || this.Planner.Product.AssociatedRoom.RoomCoordinates == null) {
					return null;
				}
				return this.Planner.Product.AssociatedRoom.RoomCoordinates;
			}
		}

		public override void RecalculateStaffeln() {
			if (this.Planner == null || this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.RoomCoordinates == null ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan == null) {
				return;
			}
			Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);

			Point2D tmp;
			double maxX = double.MinValue;
			double minX = double.MaxValue;
			double maxY = double.MinValue;
			double minY = double.MaxValue;
			foreach (Point2D point in this.RoomCoordinates) {
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

			this.staffeln.Clear();

			double measure = this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double increment = (staffelnBreite + staffelnAbstand) * measure;
			double curPos = (minX + maxX - staffelnBreite * measure) / 2.0 + (offset * measure);
			double curYPos = (minY + maxY) / 2.0;
			double curBeplankungsPos = 0;
			while (curPos > minX) {
				curPos -= increment;
			}
			//curPos = curPos - increment;
			List<int> ignoreLanes = new List<int>();
			while (curPos < maxX) {
				Polygon2D schiene = new Polygon2D();
				schiene.Add(matrix.Transform(new Point2D(curPos, minY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + staffelnBreite * measure, minY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + staffelnBreite * measure, maxY)));
				schiene.Add(matrix.Transform(new Point2D(curPos, maxY)));
				this.staffeln.Add(schiene);
				curBeplankungsPos += (staffelnBreite + staffelnAbstand);
				curPos += increment;
			}

			Polygon2D room = new Polygon2D(this.RoomCoordinates);
			if (room.IsClockwise()) {
				room = room.GetReverse();
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

		public double StaffelnBreite {
			get { return this.staffelnBreite; }
			set {
				this.staffelnBreite = value;
				this.RecalculateStaffeln();
			}
		}

		public double StaffelnAbstand {
			get { return this.staffelnAbstand; }
			set {
				this.staffelnAbstand = value;
				this.RecalculateStaffeln();
			}
		}

		[XmlIgnore]
		public double StaffelnAchsabstand {
			get { return this.staffelnAbstand + this.staffelnBreite; }
			set {
				this.staffelnAbstand = value - this.staffelnBreite;
				if (this.staffelnAbstand < 0) {
					this.staffelnAbstand = 0;
				}
			}
		}

		public double Offset {
			get { return this.offset; }
			set {
				this.offset = value;
				double increment = this.staffelnBreite + this.staffelnAbstand;
				while (this.offset >= increment) {
					this.offset -= increment;
				}
				while (this.offset < 0) {
					this.offset += increment;
				}
				this.RecalculateStaffeln();
			}
		}

		protected GraphicsPath GetProductAreaPath() {
			List<PointF> transformedPoints = new List<PointF>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Point2D point in this.RoomCoordinates) {
				Point2D tmp = additionalTransformation.TransformTo2D(point);
				transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(transformedPoints.ToArray());
			return path;
		}

		protected GraphicsPath GetRoomPath() {
			List<PointF> transformedPoints = new List<PointF>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Point2D point in this.Planner.Product.AssociatedRoom.RoomCoordinates) {
				Point2D tmp = additionalTransformation.TransformTo2D(point);
				transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(transformedPoints.ToArray());
			return path;
		}

		private List<Polygon2D> GetStaffeln(bool forDrawing) {
			if (!forDrawing) {
				return this.staffeln;
			}

			Polygon2D staffelForDrawing;
			List<Polygon2D> staffelnForDrawing = new List<Polygon2D>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Polygon2D staffel in this.staffeln) {
				staffelForDrawing = new Polygon2D();
				staffelForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)staffel[0]));
				staffelForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)staffel[1]));
				staffelForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)staffel[2]));
				staffelForDrawing.Add(additionalTransformation.TransformTo2D((Point3D)staffel[3]));
				staffelnForDrawing.Add(staffelForDrawing);
			}
			return staffelnForDrawing;
		}

		/*private List<Polygon2D> GetPossibleAreas(bool forDrawing) {
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
		}*/

		public override void Paint(Graphics g, ModulKlimaBodenPlanner.KlimaBodenMode mode) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.RoomCoordinates == null ||
				this.RoomCoordinates.Count < 3 ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			double measure = this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
			Matrix4D additionalTransformation = this.AdditionalTransformation;

			/*double minX, maxX, minY, maxY;*/
			GraphicsPath roomPath = this.GetProductAreaPath(/*out minX, out maxX, out minY, out maxY*/);
			g.Clip = new Region(roomPath);

			System.Drawing.Color c = System.Drawing.Color.Gray;
			Pen p = new Pen(c);
			Brush b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, c, System.Drawing.Color.FromArgb(0, c));

			//g.FillPath(new SolidBrush(Color.FromArgb(128, Color.Yellow)), roomPath);

			foreach (Polygon2D staffel in this.GetStaffeln(true)) {
				PointF[] poly = new PointF[staffel.Count];
				int i = 0;
				foreach (Point2D point in staffel) {
					poly[i++] = new PointF((float)point.X, (float)point.Y);
				}
				g.DrawPolygon(p, poly);
				g.FillPolygon(b, poly);
			}

			if (mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_CONSTRUCTION) {
				c = System.Drawing.Color.FromArgb(128, 0, 240, 0);
				p = new Pen(c);
				b = new SolidBrush(System.Drawing.Color.FromArgb(64, c));
				Region r = new Region();
				r.MakeInfinite();
				g.Clip = r;
				/*foreach (Polygon2D area in this.GetPossibleAreas(true)) {
					PointF[] poly = new PointF[area.Count];
					int i = 0;
					foreach (Point2D point in area) {
						poly[i++] = new PointF((float)point.X, (float)point.Y);
					}
					g.DrawPolygon(p, poly);
					g.FillPolygon(b, poly);
				}*/
			}
		}

		public override void PaintDxf(DxfModel model, DxfLayer layer) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.RoomCoordinates == null ||
				this.RoomCoordinates.Count < 3 ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			double measure = this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Polygon2D clipRegion = new Polygon2D();
			foreach (Point2D point in this.RoomCoordinates) {
				clipRegion.Add(point);
			}
			if (clipRegion.IsClockwise()) {
				clipRegion.Reverse();
			}

			EntityColor c = EntityColor.CreateFromRgb(System.Drawing.Color.Gray.ToArgb());

			DxfHatch hatch = new DxfHatch();
			hatch.Color = c;
			
			foreach (Polygon2D schiene in this.GetStaffeln(false)) {
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
		}

		public override bool HitTest(Point2D planPoint, Point pointInControl) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.RoomCoordinates == null ||
				this.RoomCoordinates.Count < 3 ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan == null ||
				this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return false;
			}

			GraphicsPath roomPath = this.GetProductAreaPath();
			if (!roomPath.IsVisible(pointInControl)) {
				return false;
			}

			List<Polygon2D> schienen = this.GetStaffeln(false);
			foreach (Polygon2D schiene in schienen) {
				if (schiene.IsInside(planPoint)) {
					return true;
				}
			}
			return false;
		}

		Point2D startPlanPoint;
		Point startPointInControl;
		double startOffset;

		public override void StartDrag(Point2D planPoint, Point pointInControl) {
			startPlanPoint = planPoint;
			startPointInControl = pointInControl;
			startOffset = this.offset;
		}

		public override void MoveDrag(Point2D planPoint, Point pointInControl) {
			Vector2D move = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0).Transform(planPoint - startPlanPoint);
			this.Offset = startOffset + (move.X / this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value);

			if (this.Planner.ConnectedPlanPanel != null) {
				this.Planner.ConnectedPlanPanel.InvalidateGraphics();
			}
		}

		public override void EndDrag(Point2D planPoint, Point pointInControl) {
		}

		[XmlIgnore]
		public override Cursor PickCursor {
			get { return Cursors.NoMove2D; }
		}

		[XmlIgnore]
		public override List<Polygon2D> Staffeln {
			get { return this.staffeln; }
		}
	}
}
