using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class LicenseItem : ListViewItem {

		private License license;

		public LicenseItem(License license) {
			this.license = license;
			this.Text = (license.LicensedTo == null || license.LicensedTo.Length == 0) ? "neue Lizenz" : license.LicensedTo; // TODO
		}

		public License License {
			get { return this.license; }
		}
	}
}
