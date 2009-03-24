using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Licensing {
	public class LicensedModule {
		private string name = null;
		private bool enabled = false;

		public LicensedModule() {
		}

		public LicensedModule(string name) {
			this.name = name;
		}

		public LicensedModule(string name, bool enabled) {
			this.name = name;
			this.enabled = enabled;
		}

		[XmlAttribute("name")]
		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}

		[XmlAttribute("enabled")]
		public bool Enabled {
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		public override string ToString() {
			return this.name + ":" + (this.enabled ? "1" : "0");
		}
	}
}
