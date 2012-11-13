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
using Europlan.Common;

namespace Europlan.Application {
	public partial class LicenseForm : Form {

		private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));

		private int fullHeight;
		private bool restartRequired = false;

		public LicenseForm() {
			InitializeComponent();

			this.SetLanguage();

			this.fullHeight = this.Height;
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.LicenseForm_Titel; //"Lizenz"
			this.btnImport.Text = EuroplanRes.LicenseForm_Importieren; //"&Lizenz importieren"
			this.btnOk.Text = EuroplanRes.General_Schliessen; //"&Schließen"
			this.label1.Text = EuroplanRes.LicenseForm_LizensiertFuer; //"Lizenziert für:"
			this.lblHardwareId.Text = EuroplanRes.LicenseForm_HardwareId; //"Hardware ID:"
			this.lblHeader.Text = EuroplanRes.LicenseForm_Seitenkopf; //"Seitenkopf:"
			this.lblLicenseDateInvalid.Text = EuroplanRes.LicenseForm_GueltigkeitAbgelaufen; //"Die Gültigkeit der Lizenz ist abgelaufen. Bitte kontaktieren sie Kontakt-Name unter lizenz@dummy.at oder +43-0000-LIZENZ um eine neue Lizenz anzufordern."
			this.lblLicenseInvalidUnknown.Text = EuroplanRes.LicenseForm_NichtGueltig; //"Die Lizenz ist nicht gültig. Falls Sie bereits eine gültige Lizenz besitzen importieren Sie diese bitte über die Schaltfläche 'Lizenz importieren'. Falls Sie noch keine gültige Lizenz besitzen kontatkieren sie Kontakt-Name unter lizenz@dummy.at oder +43-0000-LIZENZ um eine Lizenz anzufordern. Zum Anfordern einer Lizenz müssen Sie die hier angegebene Hardware ID bekannt geben."
			this.lblLicenseMissing.Text = EuroplanRes.LicenseForm_KeineLizenzGefunden; //"Es wurde keine Lizenz gefunden. Falls Sie bereits eine Lizenz besitzen importieren Sie diese bitte über die Schaltfläche 'Lizenz importieren'. Falls Sie noch keine Lizenz besitzen kontatkieren sie Kontakt-Name unter lizenz@dummy.at oder +43-0000-LIZENZ um eine Lizenz anzufordern. Zum Anfordern einer Lizenz müssen Sie die hier angegebene Hardware ID bekannt geben."
			this.lblLicenseSignatureInvalid.Text = EuroplanRes.LicenseForm_LizenzModifiziert; //"Die Lizenz ist ungültig da sie von nicht authorisierter Stelle modifiziert wurde. Bitte verwenden Sie Ihre original Lizenz oder kontatkieren sie Kontakt-Name unter lizenz@dummy.at oder +43-0000-LIZENZ um eine neue Lizenz anzufordern."
			this.lblLicenseSystemInvalid.Text = EuroplanRes.LicenseForm_RechnerNichtGueltig; //"Die Lizenz ist auf dem aktuellen Rechner nicht gültig. Bitte kontatkieren sie Kontakt-Name unter lizenz@dummy.at oder +43-0000-LIZENZ um eine Lizenz für diesen Rechner anzufordern. Zum Anfordern einer Lizenz. müssen Sie die hier angegebene Hardware ID bekannt geben."
			this.lblModules.Text = EuroplanRes.LicenseForm_InsallierteLizenzen; //"Installierte Lizenzen:"
			this.lblValidUntil.Text = EuroplanRes.LicenseForm_GueltigBis; //"Gültig bis:"
		}

		public bool RestartRequired {
			get {
				return restartRequired;
			}
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
						this.lstModule.Items.Add(new ListViewItem(module.DisplayName));
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
			dialog.Filter = EuroplanRes.License_Datei + " (*.epl)|*.epl";
			dialog.Multiselect = false;
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				ImportLicenseResultEnum importResult = LicenseManager.Instance.ImportLicense(dialog.FileName);
				if (importResult == ImportLicenseResultEnum.LICENSE_IMPORTED) {
					MessageBox.Show(EuroplanRes.License_ImportiertText, EuroplanRes.License_ImportiertTitel, MessageBoxButtons.OK, MessageBoxIcon.Information);
					this.restartRequired = true;
					this.Close();
				} else if (importResult == ImportLicenseResultEnum.LICENSE_TEMPORARY_IMPORTED) {
					string message = EuroplanRes.License_TemporaerImportiertText;
					message = message.Replace("%LICENSEPATH%", LicenseManager.Instance.LincensePath);
					MessageBox.Show(message, EuroplanRes.License_TemporaerImportiertTitel, MessageBoxButtons.OK, MessageBoxIcon.Information);
					this.Close();
				} else if (importResult == ImportLicenseResultEnum.LICENSE_NOT_FOUND) {
					MessageBox.Show(EuroplanRes.License_NichtGefundenText, EuroplanRes.License_NichtGefundenTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (importResult == ImportLicenseResultEnum.LICENSE_NOT_READABLE) {
					MessageBox.Show(EuroplanRes.License_NichtLesbarText, EuroplanRes.License_NichtLesbarTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (importResult == ImportLicenseResultEnum.LICENSE_SIGNATURE_NOT_VALID) {
					MessageBox.Show(EuroplanRes.License_SignaturNichtGueltigText, EuroplanRes.License_SignaturNichtGueltigTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (importResult == ImportLicenseResultEnum.LICENSE_SYSTEM_NOT_VALID) {
					MessageBox.Show(EuroplanRes.License_RechnerNichtGueltigText, EuroplanRes.License_RechnerNichtGueltigTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (importResult == ImportLicenseResultEnum.LICENSE_DATE_NOT_VALID) {
					MessageBox.Show(EuroplanRes.License_BereitsAbgelaufenText, EuroplanRes.License_BereitsAbgelaufenTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else {
					MessageBox.Show(EuroplanRes.License_NichtGueltigText, EuroplanRes.License_NichtGueltigTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btnOk_Click(object sender, EventArgs e) {

		}
	}
}