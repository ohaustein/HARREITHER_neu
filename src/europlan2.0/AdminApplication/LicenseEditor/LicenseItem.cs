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
			if (this.license.ValidUntil < DateTime.Today.AddDays(7)) {
				//this.Font = new System.Drawing.Font(this.Font, System.Drawing.FontStyle.Bold);
				this.ForeColor = System.Drawing.Color.Red;
			} else {
				this.ForeColor = System.Drawing.Color.Black;
			}
			this.dateItem = new ListViewSubItem(this, this.license.ValidUntil.ToShortDateString());
			this.SubItems.Add(this.dateItem);
			this.license.NameChanged += new EventHandler(license_NameChanged);
		}

		private void license_NameChanged(object sender, EventArgs e) {
			this.Text = license.DisplayName;
			this.dateItem.Text = license.ValidUntil.ToShortDateString();
			if (this.license.ValidUntil < DateTime.Today.AddDays(7)) {
				this.ForeColor = System.Drawing.Color.Red;
			} else {
				this.ForeColor = System.Drawing.Color.Black;
			}
		}

		public LicenseTemplate License {
			get { return this.license; }
		}

		public override int GetHashCode() {
			return this.license.GetHashCode();
	}

		public override bool Equals(object obj) {
			LicenseItem otherLicenseItem = obj as LicenseItem;
			if (otherLicenseItem == null) {
				return false;
			}
			return this.license.Equals(otherLicenseItem.license);
		}
	}
}
