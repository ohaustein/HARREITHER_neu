using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Licensing {
	public class LicensedSystem {
		private HardwareId id = null;

		public LicensedSystem() {
			this.id = new HardwareId();
		}

		public LicensedSystem(string id) {
			this.id = new HardwareId(id);
		}

		[XmlAttribute("id")]
		public string Id {
			get { return this.id.IdString; }
			set { this.id = new HardwareId(value); }
		}

		public override string ToString() {
			return this.id.IdString;
		}

		public bool MatchesCurrentSystem {
			get { return new HardwareId().Equals(this.id); }
		}
	}
}
