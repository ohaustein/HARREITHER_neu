using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {
	[Serializable]
	public class RoomList : List<Room> , IClipboard {

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
			if (data == typeof(Room).ToString()) {
				return true;
			}
			return false;
		}

		public string SupportedPasteFormat {
			get { return typeof(Room).ToString(); }
		}

		public object Copy() {
			throw new Exception("Copy not supported");
		}

		public void Paste(object o) {
			if (o.GetType() == typeof(Room)) {
				this.Add(o as Room);
			} else {
				throw new Exception("Paste of this type not supported");
			}
		}

	}
}
