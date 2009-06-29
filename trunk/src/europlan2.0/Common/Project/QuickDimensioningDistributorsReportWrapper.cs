using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class QuickDimensioningDistributorsReportWrapper {
		private QuickDimensioningDistributor distributor;
		private Product product;

		public QuickDimensioningDistributorsReportWrapper(QuickDimensioningDistributor distributor, Product product) {
			this.distributor = distributor;
			this.product = product;
		}

		public string Distributor {
			get { return this.distributor.Name; }
		}

		public string RoomId {
			get { return this.product.AssociatedRoom.Id; }
		}

		public string RoomName {
			get { return this.product.AssociatedRoom.Name; }
		}

		public string ProductName {
			get { return this.product.Name; }
		}

		public int Amount {
			get {
				if (!this.product.QuickDimensioningConnectedDistributors.ContainsKey(this.distributor.Id)) {
					return 0;
				}
				return this.product.QuickDimensioningConnectedDistributors[this.distributor.Id];
			}
		}
	}
}
