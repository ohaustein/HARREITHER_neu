using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.IO;
using System.Globalization;
using Europlan.Licensing;

namespace Europlan.Application {
	public partial class LicenseForm : Form {

		public LicenseForm() {
			InitializeComponent();
		}

		private void LicenseForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["LicenseForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			if (LicenseManager.Instance.LicenseFound) {
				this.txtLicensedTo.Text = LicenseManager.Instance.License.LicensedTo;
				this.txtHeader.Text = LicenseManager.Instance.License.Header;
				this.txtValidUntil.Text = LicenseManager.Instance.License.ValidUntil.ToShortDateString();
				this.lstModule.Items.Clear();
				foreach (LicensedModule module in LicenseManager.Instance.License.Modules) {
					if (module.Enabled) {
						this.lstModule.Items.Add(new ListViewItem(module.Name));
					}
				}
				if (!LicenseManager.Instance.License.IsSignatureValid) {
					this.txtValid.Text = "Lizenz ist nicht gültig, da Sie keine korrekte Signatur enthält";
				} else if (!LicenseManager.Instance.License.IsSystemValid) {
					this.txtValid.Text = "Lizenz ist auf dem aktuellen Rechner nicht gültig";
				} else if (!LicenseManager.Instance.License.IsDateValid) {
					this.txtValid.Text = "Lizenz ist abgelaufen";
				} else if (!LicenseManager.Instance.License.IsValid) {
					this.txtValid.Text = "Lizenz ist nicht gültig";
				} else {
					this.txtValid.Text = "OK";
				}

			} else {
				this.txtLicensedTo.Text = "-";
				this.txtHeader.Text = "-";
				this.txtValidUntil.Text = "-";
				this.lstModule.Items.Clear();
				this.lstModule.Items.Add(new ListViewItem("-"));
				this.txtValid.Text = "keine Lizenz gefunden";
			}
		}

		private void LicenseForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["LicenseForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void btnImport_Click(object sender, EventArgs e) {
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.DefaultExt = "epl";
			dialog.Filter = "Europlan 2.0 Lizenz (*.epl)|*.epl";
			dialog.Multiselect = false;
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				LicenseManager.Instance.ImportLicense(dialog.FileName);
			}
		}
	}
}