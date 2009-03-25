using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Licensing {
	public class LicensedModule : AbstractLicensedModule {
		public LicensedModule() : base() {
		}

		public LicensedModule(string name) : base(name) {
		}

		public LicensedModule(string name, bool enabled) : base(name, enabled) {
		}
	}
}
