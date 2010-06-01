using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ModulDeckeSubArea {

		private List<KlimaFlaechenList> rows = new List<KlimaFlaechenList>();

		public ModulDeckeSubArea() {
			// A subarea needs to have at least one row so add this row by default,
			// if this subarea is deserialized this row will be deleted again in FinalizeLoading
			this.rows.Add(new KlimaFlaechenList());
		}

		public List<KlimaFlaechenList> Rows {
			get { return rows; }
			set { rows = value; }
		}

		public double Druckverlust(double durchfluss) {
			double druckverlust = 0;
			foreach (KlimaFlaechenList row in this.rows) {
				double rowDruckverlust = row.Druckverlust(durchfluss / this.HeatArea * row.HeatArea);
				if (rowDruckverlust > druckverlust) {
					druckverlust = rowDruckverlust;
				}
			}
			return druckverlust;
		}

		public double CoveredArea {
			get {
				double area = 0;
				foreach (KlimaFlaechenList row in this.rows) {
					area += row.CoveredArea;
				}
				return area;
			}
		}

		public double HeatArea {
			get {
				double area = 0;
				foreach (KlimaFlaechenList row in this.rows) {
					area += row.HeatArea;
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

		internal void FinalizeLoading() {
			// If this subarea is deserialized remove the row that was added by default
			if (this.rows.Count > 0) {
				this.rows.RemoveAt(0);
			}
		}
	}
}
