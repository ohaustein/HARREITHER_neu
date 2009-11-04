using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class KlimaFleachenList : List<KlimaFlaechenModul> {

		private double lengthVerbindeleitungen;
		public double LengthVerbindeleitungen {
			get { return lengthVerbindeleitungen; }
			set { lengthVerbindeleitungen = value; }
		}

		public double Druckverlust(double durchfluss) {
			double druckverlust = 0;
			foreach (KlimaFlaechenModul modul in this) {
				druckverlust += modul.Druckverlust(durchfluss);
			}
			druckverlust += EN1264.Instance.DruckverlustRohr(durchfluss, Product.rundrohr21mmInnenA, EurovalProduct.ConfigRho, Product.rundrohr21mmInnenD, EurovalProduct.ConfigV, 0.000004, lengthVerbindeleitungen);
			return druckverlust;
		}

		public double ModulArea {
			get {
				double area = 0;
				foreach (KlimaFlaechenModul modul in this) {
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
