using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Licensing {
	public class LicensedModuleTemplate : AbstractLicensedModule {
		public LicensedModuleTemplate() : base() {
		}

		public LicensedModuleTemplate(string name) : base(name) {
		}

		public LicensedModuleTemplate(string name, bool enabled) : base(name, enabled) {
		}

		public LicensedModule CreateModule() {
			LicensedModule module = new LicensedModule();
			module.Name = this.name; ;
			module.Enabled = this.enabled;
			return module;
		}
	}
}
