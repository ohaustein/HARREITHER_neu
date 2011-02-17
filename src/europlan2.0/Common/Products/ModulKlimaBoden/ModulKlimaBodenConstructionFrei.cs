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
	public class ModulKlimaBodenConstructionFrei : ModulKlimaBodenConstruction {

		public ModulKlimaBodenConstructionFrei() {
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

		public override void RecalculateSchienen() {
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
			//g.Clip = new Region(roomPath);

			Color c = Color.Gray;
			Pen p = new Pen(c);
			Brush b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, c, Color.FromArgb(0, c));

			//g.FillPath(new SolidBrush(Color.FromArgb(128, Color.Yellow)), roomPath);

			if (mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_CONSTRUCTION) {
				c = Color.FromArgb(128, 0, 240, 0);
				p = new Pen(c);
				b = new SolidBrush(Color.FromArgb(64, c));
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
		}

		public override void PaintDxf(DxfModel model, DxfLayer layer) {
		}

		public override bool HitTest(Point2D planPoint, Point pointInControl) {
			return false;
		}

		Point2D startPlanPoint;
		Point startPointInControl;

		public override void StartDrag(Point2D planPoint, Point pointInControl) {
		}

		public override void MoveDrag(Point2D planPoint, Point pointInControl) {
		}

		public override void EndDrag(Point2D planPoint, Point pointInControl) {
		}

		[XmlIgnore]
		public override Cursor PickCursor {
			get { return Cursors.NoMove2D; }
		}
	}
}
