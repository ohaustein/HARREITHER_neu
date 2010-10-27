using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WW.Math;
using System.Drawing;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Europlan.Common {
	public class ModulKlimaDeckeConstructionGlatt : ModulKlimaDeckeConstruction {
		private double rotation = 0;
		private double schienenBreite = 0.1; // meter
		private double schienenAbstand = 0.5; // meter
		private double offset = 0; // meter

		private List<Polygon2D> schienen = new List<Polygon2D>();
		private List<List<PossibleModulRowArea>> possibleRows = new List<List<PossibleModulRowArea>>();

		public ModulKlimaDeckeConstructionGlatt() {
		}

		public void RecalculateSchienen() {
			if (this.Planner == null || this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.Planner.Product.AssociatedRoom.RoomCoordinates == null ||
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
			foreach (Point2D point in this.Planner.Product.AssociatedRoom.RoomCoordinates) {
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
			this.possibleRows.Clear();

			double measure = this.Planner.ConnectedPlanPanel.Plan.Measure.Value;
			double increment = (schienenBreite + schienenAbstand) * measure;
			double curPos = (minX + maxX - schienenBreite * measure) / 2.0 + (offset * measure);
			while (curPos > minX) {
				curPos -= increment;
			}
			//curPos = curPos - increment;
			while (curPos < maxX) {
				Polygon2D schiene = new Polygon2D();
				schiene.Add(matrix.Transform(new Point2D(curPos, minY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + schienenBreite * measure, minY)));
				schiene.Add(matrix.Transform(new Point2D(curPos + schienenBreite * measure, maxY)));
				schiene.Add(matrix.Transform(new Point2D(curPos, maxY)));
				this.schienen.Add(schiene);
				curPos += increment;
			}

			Polygon2D room = new Polygon2D(this.Planner.Product.AssociatedRoom.RoomCoordinates);
			if (room.IsClockwise()) {
				room = room.GetReverse();
			}

			for (int i = 0; i < this.schienen.Count - 1; i++) {
				Polygon2D schieneLeft = this.schienen[i];
				Polygon2D schieneRight = this.schienen[i + 1];
				Polygon2D row = new Polygon2D();
				row.Add(schieneLeft[1]);
				row.Add(schieneLeft[2]);
				row.Add(schieneRight[3]);
				row.Add(schieneRight[0]);
				if (row.IsClockwise()) {
					row = row.GetReverse();
				}
				//List<Polygon2D> rowList = Polygon2D.GetIntersection(new Polygon2D[] { room }, new Polygon2D[] { row });
				//Console.WriteLine(rowList.ToString());

				this.possibleRows.Add(GetPossibleModuleAreasInRow(row));
			}
		}

		private List<PossibleModulRowArea> GetPossibleModuleAreasInRow(Polygon2D row) {
			Matrix3D matrix = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0);

			Polygon2D tmp = new Polygon2D();
			Point2D startPoint = new Point2D(double.MaxValue, 0);
			int start = -1;
			int i = 0;
			foreach (Point2D point in this.Planner.Product.AssociatedRoom.RoomCoordinates) {
				Point2D pointTf = matrix.Transform(point);
				if (pointTf.X < startPoint.X) {
					startPoint = pointTf;
					start = i;
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

			Point2D p = matrix.Transform(row[0]);
			Line2D borderRight = new Line2D(p, p - matrix.Transform(row[1]));
			p = matrix.Transform(row[3]);
			Line2D borderLeft = new Line2D(p, p - matrix.Transform(row[2]));
			/*Polygon2D rowTf = new Polygon2D();
			foreach (Point2D point in row) {
				rowTf.Add(matrix.Transform(point));
			}*/

			Segment2D roomBorder;
			Nullable<Point2D> intersection = null;
			if (startPoint.X > borderLeft.Origin.X) {
				// no possible module areas in row found
				return new List<PossibleModulRowArea>();
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
					lp.Add(room[i + 1].Y);
				}
			}

			bordersTop.Sort();
			bordersBottom.Sort();
			removes.Sort();

			List<PossibleModulRowArea> possibleAreas = new List<PossibleModulRowArea>();
			matrix = matrix.GetInverse();
			for (i = 0; i < bordersTop.Count; i++) {
				double top = bordersTop[i];
				double bottom;
				Polygon2D area;
				foreach (CompareablePair<double> remove in removes) {
					if (remove.value1 > bordersTop[i] && remove.value1 < bordersBottom[i]) {
						bottom = remove.value2;
						area = new Polygon2D();
						area.Add(matrix.Transform(new Point2D(borderLeft.Origin.X, top)));
						area.Add(matrix.Transform(new Point2D(borderLeft.Origin.X, bottom)));
						area.Add(matrix.Transform(new Point2D(borderRight.Origin.X, bottom)));
						area.Add(matrix.Transform(new Point2D(borderRight.Origin.X, top)));
						possibleAreas.Add(new PossibleModulRowArea(area));
						top = remove.value1;
					}
				}
				bottom = bordersBottom[i];
				area = new Polygon2D();
				area.Add(matrix.Transform(new Point2D(borderLeft.Origin.X, top)));
				area.Add(matrix.Transform(new Point2D(borderLeft.Origin.X, bottom)));
				area.Add(matrix.Transform(new Point2D(borderRight.Origin.X, bottom)));
				area.Add(matrix.Transform(new Point2D(borderRight.Origin.X, top)));
				possibleAreas.Add(new PossibleModulRowArea(area));
			}

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

		public double Rotation {
			get { return this.rotation; }
			set {
				this.rotation = value;
				this.RecalculateSchienen();
			}
		}

		[XmlIgnore]
		public double RotationRelativeToPlan {
			get {
				if (this.Planner.ConnectedPlanPanel.Plan is ImagePlan) {
					return this.rotation + (this.Planner.ConnectedPlanPanel.Plan as ImagePlan).Rotation;
				} else if (this.Planner.ConnectedPlanPanel.Plan is CadPlan) {
					return -this.rotation;
				}
				return this.rotation;
			}
			set {
				if (this.Planner.ConnectedPlanPanel.Plan is ImagePlan) {
					this.Rotation = value - (this.Planner.ConnectedPlanPanel.Plan as ImagePlan).Rotation;
				} else if (this.Planner.ConnectedPlanPanel.Plan is CadPlan) {
					this.Rotation = -value;
				} else {
					this.Rotation = value;
				}
			}
		}

		public double SchienenBreite {
			get { return this.schienenBreite; }
			set {
				this.schienenBreite = value;
				this.RecalculateSchienen();
			}
		}

		public double SchienenAbstand {
			get { return this.schienenAbstand; }
			set {
				this.schienenAbstand = value;
				this.RecalculateSchienen();
			}
		}

		public double Offset {
			get { return this.offset; }
			set {
				this.offset = value;
				double increment = this.schienenBreite + this.schienenAbstand;
				while (this.offset >= increment) {
					this.offset -= increment;
				}
				while (this.offset < 0) {
					this.offset += increment;
				}
				this.RecalculateSchienen();
			}
		}

		private GraphicsPath GetRoomPath() {
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

		private List<Polygon2D> GetPossibleAreas(bool forDrawing) {
			List<Polygon2D> possibleAreas = new List<Polygon2D>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (List<PossibleModulRowArea> possibleRow in this.possibleRows) {
				foreach (PossibleModulRowArea possibleArea in possibleRow) {
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

		public override void Paint(Graphics g) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.Planner.Product.AssociatedRoom.RoomCoordinates == null ||
				this.Planner.Product.AssociatedRoom.RoomCoordinates.Count < 3 ||
				this.Planner.ConnectedPlanPanel == null ||
				this.Planner.ConnectedPlanPanel.Plan == null ||
				this.Planner.ConnectedPlanPanel.Plan.Measure == null) {
				return;
			}
			double measure = this.Planner.ConnectedPlanPanel.Plan.Measure.Value;
			Matrix4D additionalTransformation = this.AdditionalTransformation;

			/*double minX, maxX, minY, maxY;*/
			GraphicsPath roomPath = this.GetRoomPath(/*out minX, out maxX, out minY, out maxY*/);
			g.Clip = new Region(roomPath);

			Color c = Color.Red;
			Pen p = new Pen(c);
			Brush b = new HatchBrush(HatchStyle.DiagonalCross, c, Color.FromArgb(0, c));

			//g.FillPath(new SolidBrush(Color.FromArgb(128, Color.Yellow)), roomPath);

			foreach (Polygon2D schiene in this.GetSchienen(true)) {
				PointF[] poly = new PointF[schiene.Count];
				int i = 0;
				foreach (Point2D point in schiene) {
					poly[i++] = new PointF((float)point.X, (float)point.Y);
				}
				g.DrawPolygon(p, poly);
				g.FillPolygon(b, poly);
			}

			c = Color.FromArgb(128, 0, 255, 0);
			p = new Pen(c);
			b = new SolidBrush(Color.FromArgb(64, c));
			g.Clip.MakeInfinite();
			Region r = new Region();
			r.MakeInfinite();
			g.Clip = r;
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

		public override bool HitTest(Point2D planPoint, Point pointInControl) {
			if (this.Planner == null ||
				this.Planner.Product == null ||
				this.Planner.Product.AssociatedRoom == null ||
				this.Planner.Product.AssociatedRoom.RoomCoordinates == null ||
				this.Planner.Product.AssociatedRoom.RoomCoordinates.Count < 3 ||
				this.Planner.ConnectedPlanPanel == null ||
				this.Planner.ConnectedPlanPanel.Plan == null ||
				this.Planner.ConnectedPlanPanel.Plan.Measure == null) {
				return false;
			}

			GraphicsPath roomPath = this.GetRoomPath();
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
		double startOffset;

		public override void StartDrag(Point2D planPoint, Point pointInControl) {
			startPlanPoint = planPoint;
			startPointInControl = pointInControl;
			startOffset = this.offset;
		}

		public override void MoveDrag(Point2D planPoint, Point pointInControl) {
			/*double dist = planPoint.X - startPlanPoint.X;
			this.Offset = startOffset + (dist / this.Planner.ConnectedPlanPanel.Plan.Measure.Value);*/

			Vector2D move = Transformation3D.Rotate(-this.Rotation * Math.PI / 180.0).Transform(planPoint - startPlanPoint);
			this.Offset = startOffset + (move.X / this.Planner.ConnectedPlanPanel.Plan.Measure.Value);

			this.Planner.ConnectedPlanPanel.InvalidateGraphics();
		}

		public override void EndDrag(Point2D planPoint, Point pointInControl) {
		}

		[XmlIgnore]
		public override Cursor PickCursor {
			get { return Cursors.NoMove2D; }
		}
	}
}
