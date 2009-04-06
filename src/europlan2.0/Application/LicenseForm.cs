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
using System.Threading;

namespace Europlan.Application {
	public partial class LicenseForm : Form {

		private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));

		private int fullHeight;

		public LicenseForm() {
			InitializeComponent();
			this.fullHeight = this.Height;
		}

		private void LicenseForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["LicenseForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.txtHardwareId.Text = new HardwareId(true).IdString;
			this.UpdateLicenseInfo();
		}

		private void UpdateLicenseInfo() {
			this.lblLicenseDateInvalid.Visible = false;
			this.lblLicenseMissing.Visible = false;
			this.lblLicenseSignatureInvalid.Visible = false;
			this.lblLicenseSystemInvalid.Visible = false;
			this.lblLicenseInvalidUnknown.Visible = false;
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
					this.lblLicenseSignatureInvalid.Visible = true;
					this.Height = this.fullHeight;
				} else if (!LicenseManager.Instance.License.IsSystemValid) {
					this.lblLicenseSystemInvalid.Visible = true;
					this.Height = this.fullHeight;
				} else if (!LicenseManager.Instance.License.IsDateValid) {
					this.lblLicenseDateInvalid.Visible = true;
					this.Height = this.fullHeight;
				} else if (!LicenseManager.Instance.License.IsValid) {
					this.lblLicenseInvalidUnknown.Visible = true;
					this.Height = this.fullHeight;
				} else {
					this.Height = this.fullHeight - this.lblLicenseInvalidUnknown.Height;
				}
			} else {
				this.txtLicensedTo.Text = "-";
				this.txtHeader.Text = "-";
				this.txtValidUntil.Text = "-";
				this.lstModule.Items.Clear();
				this.lstModule.Items.Add(new ListViewItem("-"));
				this.lblLicenseMissing.Visible = true;
				this.Height = this.fullHeight;
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
			dialog.Filter = resources.GetString("LicenseFileDescription", Thread.CurrentThread.CurrentUICulture) + " (*.epl)|*.epl";
			dialog.Multiselect = false;
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				ImportLicenseResultEnum importResult = LicenseManager.Instance.ImportLicense(dialog.FileName);
				if (importResult == ImportLicenseResultEnum.LICENSE_IMPORTED) {
					MessageBox.Show(resources.GetString("LicenseImportedMessage", Thread.CurrentThread.CurrentUICulture), resources.GetString("LicenseImportOkTitle", Thread.CurrentThread.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information);
					this.Close();
					//this.UpdateLicenseInfo();
				} else if (importResult == ImportLicenseResultEnum.LICENSE_TEMPORARY_IMPORTED) {
					MessageBox.Show(string.Format(resources.GetString("LicenseTemporaryImportedMessage", Thread.CurrentThread.CurrentUICulture), LicenseManager.Instance.LincensePath), resources.GetString("LicenseImportOkTitle", Thread.CurrentThread.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information);
					this.Close();
				} else if (importResult == ImportLicenseResultEnum.LICENSE_NOT_FOUND) {
					MessageBox.Show(resources.GetString("LicenseNotFoundMessage", Thread.CurrentThread.CurrentUICulture), resources.GetString("LicenseImportFailedTitle", Thread.CurrentThread.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (importResult == ImportLicenseResultEnum.LICENSE_NOT_READABLE) {
					MessageBox.Show(resources.GetString("LicenseNotReadableMessage", Thread.CurrentThread.CurrentUICulture), resources.GetString("LicenseImportFailedTitle", Thread.CurrentThread.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (importResult == ImportLicenseResultEnum.LICENSE_SIGNATURE_NOT_VALID) {
					MessageBox.Show(resources.GetString("LicenseSignatureNotValidMessage", Thread.CurrentThread.CurrentUICulture), resources.GetString("LicenseImportFailedTitle", Thread.CurrentThread.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (importResult == ImportLicenseResultEnum.LICENSE_SYSTEM_NOT_VALID) {
					MessageBox.Show(resources.GetString("LicenseSystemNotValidMessage", Thread.CurrentThread.CurrentUICulture), resources.GetString("LicenseImportFailedTitle", Thread.CurrentThread.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (importResult == ImportLicenseResultEnum.LICENSE_DATE_NOT_VALID) {
					MessageBox.Show(resources.GetString("LicenseDateNotValidMessage", Thread.CurrentThread.CurrentUICulture), resources.GetString("LicenseImportFailedTitle", Thread.CurrentThread.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else {
					MessageBox.Show(resources.GetString("LicenseNotValidMessage", Thread.CurrentThread.CurrentUICulture), resources.GetString("LicenseImportFailedTitle", Thread.CurrentThread.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btnOk_Click(object sender, EventArgs e) {

		}
	}
}