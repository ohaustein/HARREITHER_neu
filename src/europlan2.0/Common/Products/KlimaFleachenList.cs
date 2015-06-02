using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class KlimaFlaechenList {

		private double lengthVerbindeleitungen;
		private List<KlimaFlaechenModul> list = new List<KlimaFlaechenModul>();
		private List<KlimaFlaechenModulVerbindung> verbindungen = new List<KlimaFlaechenModulVerbindung>();

		public KlimaFlaechenList() {
		}

		public KlimaFlaechenList(KlimaFlaechenList otherList) {
			this.lengthVerbindeleitungen = otherList.lengthVerbindeleitungen;
            this._sonstigeVerbindeleitung = otherList._sonstigeVerbindeleitung;
			foreach (KlimaFlaechenModul module in otherList.list) {
				this.list.Add(new KlimaFlaechenModul(module));
			}
		}

        private double _sonstigeVerbindeleitung;

        public double SonstigeVerbindeleitung
        {
            get { return _sonstigeVerbindeleitung; }
            set { _sonstigeVerbindeleitung = value; }
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

		public double GetHeatArea(bool floor) {
			double area = 0;
			foreach (KlimaFlaechenModul modul in this.list) {
                area += modul.GetHeatArea(floor);
			}
			return area + 0.02 * this.SonstigeVerbindeleitung;                   
		}

		public double GetCoveredArea(bool floor) {
			double area = 0;
			foreach (KlimaFlaechenModul modul in this.list) {
                area += modul.GetCoveredArea(floor) + modul.GetModulationArea() + 0.055 * SonstigeVerbindeleitung;
			}
            return area;
		}

		public double GetEquivalentPipeLength(bool floor) {
			return GetHeatArea(floor) * 10 + lengthVerbindeleitungen;
		}

		public bool ContainsModul(KlimaFlaechenModul modul) {
			return this.list != null && this.list.Contains(modul);
		}

		public int CountModules() {
			return this.list.Count;
		}

		#region Graphical Materials
		public int GetRequiredWinkel() {
			int result = 0;
			foreach (KlimaFlaechenModulVerbindung link in this.Links) {
				result += link.GetRequiredWinkel();
			}
			return result;
		}
		#endregion Graphical Materials
	}
}
