using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class KonstruktionenWrapper {

		private string konstruktion;
		private List<PlannedProduct> products;

		public KonstruktionenWrapper(string konstruktion, List<PlannedProduct> products) {
			this.konstruktion = konstruktion;
			this.products = products;
		}

		public string Konstruktion {
			get { return konstruktion; }
		}

		public string ProductsInRooms {
			get {
				string temp = "";
				foreach (PlannedProduct pp in products) {
					temp += pp.InternalName + " in " + pp.Product.AssociatedRoom.Id + " (" + pp.Product.AssociatedRoom.Name + ")\n";
				}
				return temp.Trim();
			}
		}

	}

}
