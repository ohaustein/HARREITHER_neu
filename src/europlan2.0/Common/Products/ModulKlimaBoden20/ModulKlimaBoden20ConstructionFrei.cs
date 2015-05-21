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
	public class ModulKlimaBoden20ConstructionFrei : ModulKlimaBoden20Construction {

		public ModulKlimaBoden20ConstructionFrei() {
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
			// nothing to do as there are no staffeln in this construction
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

			GraphicsPath roomPath = this.GetProductAreaPath();

			System.Drawing.Color c = System.Drawing.Color.Gray;
			Pen p = new Pen(c);
			Brush b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, c, System.Drawing.Color.FromArgb(0, c));

			if (mode == ModulKlimaBodenPlanner.KlimaBodenMode.KDM_CONSTRUCTION) {
				c = System.Drawing.Color.FromArgb(128, 0, 240, 0);
				p = new Pen(c);
				b = new SolidBrush(System.Drawing.Color.FromArgb(64, c));
				Region r = new Region();
				r.MakeInfinite();
				g.Clip = r;
			}
		}

		public override void PaintDxf(DxfModel model, DxfLayer layer) {
		}

		public override bool HitTest(Point2D planPoint, Point pointInControl) {
			return false;
		}

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

		public override List<Polygon2D> Staffeln {
			get { return new List<Polygon2D>(); }
		}
	}
}
