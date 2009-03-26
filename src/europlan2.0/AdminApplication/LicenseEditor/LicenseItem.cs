using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class LicenseItem : ListViewItem {

		private LicenseTemplate license;
		private ListViewSubItem dateItem;

		public LicenseItem(LicenseTemplate license) {
			this.license = license;
			this.Text = license.DisplayName;
			this.dateItem = new ListViewSubItem(this, this.license.ValidUntil.ToShortDateString());
			this.SubItems.Add(this.dateItem);
			this.license.NameChanged += new EventHandler(license_NameChanged);
		}

		private void license_NameChanged(object sender, EventArgs e) {
			this.Text = license.DisplayName;
			this.dateItem.Text = license.ValidUntil.ToShortDateString();
		}

		public LicenseTemplate License {
			get { return this.license; }
		}
	}
}
