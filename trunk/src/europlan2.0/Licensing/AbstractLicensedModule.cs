using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Licensing {
	public abstract class AbstractLicensedModule {
		protected string name = null;
		protected bool enabled = false;

		public AbstractLicensedModule() {
		}

		public AbstractLicensedModule(string name) {
			this.name = name;
		}

		public AbstractLicensedModule(string name, bool enabled) {
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
