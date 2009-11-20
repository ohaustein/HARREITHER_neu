using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ModulDeckeSubArea {

		private List<KlimaFlaechenList> rows = new List<KlimaFlaechenList>();

		public ModulDeckeSubArea() {
			this.rows.Add(new KlimaFlaechenList());
		}

		public List<KlimaFlaechenList> Rows {
			get { return rows; }
			set { rows = value; }
		}

		public double Druckverlust(double durchfluss) {
			double druckverlust = 0;
			foreach (KlimaFlaechenList row in this.rows) {
				double rowDruckverlust = row.Druckverlust(durchfluss / this.rows.Count);
				if (rowDruckverlust > druckverlust) {
					druckverlust = rowDruckverlust;
				}
			}
			return druckverlust;
		}

		public double ModulArea {
			get {
				double area = 0;
				foreach (KlimaFlaechenList row in this.rows) {
					area += row.ModulArea;
				}
				return area;
			}
		}

		public double EquivalentPipeLength {
			get {
				double length = 0;
				foreach (KlimaFlaechenList row in rows) {
					double rowLength = row.EquivalentPipeLength;
					if (rowLength > length) {
						length = rowLength;
					}
				}
				return length;
			}
		}
	}
}
