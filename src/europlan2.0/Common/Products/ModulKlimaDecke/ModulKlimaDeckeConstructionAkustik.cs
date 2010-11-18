using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math;
using System.Windows.Forms;
using System.Xml.Serialization;

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
		}

	}
}
