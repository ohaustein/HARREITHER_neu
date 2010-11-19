using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math;
using System.Windows.Forms;
using System.Xml.Serialization;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class ModulKlimaDeckeConstructionAkustik : ModulKlimaDeckeConstructionGlatt {
		private double randfries = 0.2; // meter

		public ModulKlimaDeckeConstructionAkustik() {

		}

		public double Randfries {
			get { return this.randfries; }
			set { this.randfries = value; }
		}

		public override void Paint(Graphics g, ModulKlimaDeckePlanner.KlimaDeckeMode mode) {
			base.Paint(g, mode);
			Region region = new Region(this.GetCeilingPath());
			region.Exclude(this.GetProductAreaPath());
			g.Clip = new Region();
			g.FillRegion(new SolidBrush(Color.FromArgb(127, Color.Red)), region);
		}

		public override List<Point2D> CeilingCoordinates {
			get {
				Polygon2D coords = new Polygon2D(this.Planner.Product.AssociatedRoom.CeilingCoordinatesToUse);
				if (this.Planner == null || this.Planner.Product == null ||
					this.Planner.Product.AssociatedRoom == null ||
					this.Planner.Product.AssociatedRoom.AssociatedPlan == null ||
					this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
					return coords;
				}

				if (!coords.IsClockwise()) {
					coords.Reverse();
				}
				coords.RightSet(this.Planner.Product.AssociatedRoom.AssociatedPlan.Measure.Value * this.randfries);
				return coords;
			}
		}

	}
}
