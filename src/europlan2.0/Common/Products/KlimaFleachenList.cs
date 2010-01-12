using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class KlimaFlaechenList {

		private double lengthVerbindeleitungen;
		private List<KlimaFlaechenModul> list = new List<KlimaFlaechenModul>();

		public double LengthVerbindeleitungen {
			get { return lengthVerbindeleitungen; }
			set { lengthVerbindeleitungen = value; }
		}

		public List<KlimaFlaechenModul> List {
			get { return list; }
			set { list = value; }
		}

		public double Druckverlust(double massenstrom) {
			double druckverlust = 0;
			foreach (KlimaFlaechenModul modul in this.list) {
				druckverlust += modul.Druckverlust(massenstrom);
			}
			druckverlust += EN1264.Instance.DruckverlustRohr(massenstrom, Product.rundrohr21mmInnenA, EurovalProduct.ConfigRho, Product.rundrohr21mmInnenD, EurovalProduct.ConfigV, 0.000004, lengthVerbindeleitungen);
			return druckverlust;
		}

		public double ModulArea {
			get {
				double area = 0;
				foreach (KlimaFlaechenModul modul in this.list) {
					area += modul.Area;
				}
				return area;
			}
		}

		public double EquivalentPipeLength {
			get {
				return ModulArea * 10 + lengthVerbindeleitungen;
			}
		}
	}
}
