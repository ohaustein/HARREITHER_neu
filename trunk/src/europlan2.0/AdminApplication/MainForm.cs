using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;
using System.IO;

namespace Europlan.AdminApplication {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		private void btnNewLicense_Click(object sender, EventArgs e) {
			LicenseTemplate newLicense = new LicenseTemplate();
			foreach (string module in LicenseEditor.availableModules) {
				newLicense.SetModuleEnabled(module, false);
			}
			LicenseItem newItem = new LicenseItem(newLicense);
			this.lstLicenses.Items.Add(newItem);
			/*foreach (LicenseItem item in this.lstLicenses.Items) {
				item.Selected = item == newItem;
			}*/
			newItem.Selected = true;

		}

		private void lstLicenses_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			LicenseItem item = e.Item as LicenseItem;
			if (item == null) {
				return;
			}
			if (this.licenseEditor1.License == item.License) {
				if (!e.IsSelected) {
					this.licenseEditor1.License = null;
					this.btnSaveLicense.Enabled = false;
				}
			} else if (e.IsSelected) {
				this.licenseEditor1.License = item.License;
				this.btnSaveLicense.Enabled = true;
			}
		}

		private void btnSaveLicense_Click(object sender, EventArgs e) {
			License lic = this.licenseEditor1.License.CreateLicense();
			if (lic != null) {
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.DefaultExt = ".lic";
				if (dialog.ShowDialog() == DialogResult.OK) {
					using (Stream s = new FileStream(dialog.FileName, FileMode.Create)) {
						lic.SaveLicense(s);
					}
				}
			}
		}
	}
}