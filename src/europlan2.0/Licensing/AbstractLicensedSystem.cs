using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Licensing {
	public class AbstractLicensedSystem {
		private HardwareId id = null;

		public AbstractLicensedSystem() {
			this.id = new HardwareId();
		}

		public AbstractLicensedSystem(string id) {
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
