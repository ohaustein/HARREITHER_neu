using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	[Serializable]
	public class DistributorList : List<Distributor> , IClipboard {

		public bool SupportsCut {
			get { return false; }
		}

		public bool SupportsCopy {
			get { return false; }
		}

		public string DataFormat {
			get { return this.GetType().ToString(); }
		}

		public bool SupportsPaste(string data) {
			if (data == typeof(Distributor).ToString()) {
				return true;
			}
			return false;
		}

		public string SupportedPasteFormat {
			get { return typeof(Distributor).ToString(); }
		}

		public object Copy() {
			throw new Exception("Copy not supported");
		}

		public void Paste(object o) {
			if (o.GetType() == typeof(Distributor)) {
				this.Add(o as Distributor);
			} else {
				throw new Exception("Paste of this type not supported");
			}
		}

	}
}
