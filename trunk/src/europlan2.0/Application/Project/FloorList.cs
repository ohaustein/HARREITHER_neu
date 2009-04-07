using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {
	public class FloorList : List<Floor> , IGuiRepresentation, IClipboard {

		public Type AssociatedPanelType {
			get { return typeof(FloorListSummaryPanel); }
		}

		public System.Drawing.Icon AssociatedIcon {
			get { return null; }
		}


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
			if (data == typeof(Floor).ToString()) {
				return true;
			}
			return false;
		}

		public string SupportedPasteFormat {
			get { return typeof(Floor).ToString(); }
		}

		public object Copy() {
			throw new Exception("Copy not supported");
		}

		public void Paste(object o) {
			if (o.GetType() == typeof(Floor)) {
				this.Add(o as Floor);
			} else {
				throw new Exception("Paste of this type not supported");
			}
		}

	}
}
