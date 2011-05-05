using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class KlimaFlaechenList {

		private double lengthVerbindeleitungen;
		private List<KlimaFlaechenModul> list = new List<KlimaFlaechenModul>();
		private List<KlimaFlaechenModulVerbindung> verbindungen = null;

		public KlimaFlaechenList() {
		}

		public KlimaFlaechenList(KlimaFlaechenList otherList) {
			this.lengthVerbindeleitungen = otherList.lengthVerbindeleitungen;
			foreach (KlimaFlaechenModul module in otherList.list) {
				this.list.Add(new KlimaFlaechenModul(module));
			}
		}

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

		public List<KlimaFlaechenModulVerbindung> Links {
			get { return this.verbindungen; }
			set { this.verbindungen = value; }
		}

		//[XmlIgnore]
		public double GetHeatArea(bool floor) {
			//get {
				double area = 0;
				foreach (KlimaFlaechenModul modul in this.list) {
					area += modul.GetHeatArea(floor);
				}
				return area;
			//}
			}

		//[XmlIgnore]
		public double GetCoveredArea(bool floor) {
			//get {
				double area = 0;
				foreach (KlimaFlaechenModul modul in this.list) {
					area += modul.GetCoveredArea(floor);
				}
				return area;
			//}
			}

		//[XmlIgnore]
		public double GetEquivalentPipeLength(bool floor) {
			//get {
				return GetHeatArea(floor) * 10 + lengthVerbindeleitungen;
			//}
		}

		public bool ContainsModul(KlimaFlaechenModul modul) {
			return this.list != null && this.list.Contains(modul);
		}

		public int CountModules() {
			return this.list.Count;
		}
	}
}
