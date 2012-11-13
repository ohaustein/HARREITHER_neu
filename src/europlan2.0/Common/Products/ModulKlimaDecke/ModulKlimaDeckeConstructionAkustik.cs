using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math;
using System.Windows.Forms;
using System.Xml.Serialization;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;

namespace Europlan.Common {
	public class ModulKlimaDeckeConstructionAkustik : ModulKlimaDeckeConstructionGlatt {
		private double randfries = 0.2; // meter

		public ModulKlimaDeckeConstructionAkustik() {

		}

		public double Randfries {
			get { return this.randfries; }
			set { this.randfries = value; }
		}

		public override void Paint(Graphics g, ModulKlimaDeckePlanner.KlimaDeckeMode mode, bool drawBeplankung) {
			base.Paint(g, mode, drawBeplankung);
			Region region = new Region(this.GetCeilingPath());
			region.Exclude(this.GetProductAreaPathForAkustik());
			g.Clip = new Region();
			g.FillRegion(new HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(127, Color.Red), Color.FromArgb(10, Color.Red)), region);
		}

		protected GraphicsPath GetProductAreaPathForAkustik() {
			List<PointF> transformedPoints = new List<PointF>();
			Matrix4D additionalTransformation = this.AdditionalTransformation;
			foreach (Point2D point in this.CeilingCoordinatesAkustik) {
				Point2D tmp = additionalTransformation.TransformTo2D(point);
				transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
			}
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(transformedPoints.ToArray());
			return path;
		}

		[XmlIgnore]
		public List<Point2D> CeilingCoordinatesAkustik {
			get {
				if (this.Product == null || this.Product.AssociatedRoom == null || this.Product.AssociatedRoom.CeilingCoordinatesToUse == null) {
					return null;
				}
				Polygon2D coords = new Polygon2D(this.Product.AssociatedRoom.CeilingCoordinatesToUse);
				if (this.Product.AssociatedRoom.AssociatedPlan == null ||
					this.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
					return coords;
				}

				if (!coords.IsClockwise()) {
					coords.Reverse();
				}
				coords.RightSet(this.Product.AssociatedRoom.AssociatedPlan.Measure.Value * this.randfries);
				return coords;
			}
		}

	}
}
