using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class SystemItem : ListViewItem {

		private LicensedSystemTemplate system;

		public SystemItem(LicensedSystemTemplate system) {
			this.system = system;
			this.Text = this.system.Id;
		}

		public LicensedSystemTemplate System {
			get { return this.system; }
		}
	}
}
