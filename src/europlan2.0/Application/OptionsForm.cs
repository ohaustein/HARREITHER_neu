using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.IO;
using System.Globalization;
using Europlan.Common;

namespace Europlan.Application {
	public partial class OptionsForm : Form {

		private bool restartRequired = false;

		public OptionsForm() {
			InitializeComponent();

			this.megCeiling.Filter = CategoryType.Ceiling;
			this.megDistributor.Filter = CategoryType.Distributor;
			this.megFloor.Filter = CategoryType.Floor;
			this.megGeneral.Filter = CategoryType.General;
			this.megInsulation.Filter = CategoryType.Insulation;
			this.megWall.Filter = CategoryType.Wall;

			this.SetLanguague();
            this.cmbPlanUnit.SelectedIndex = Product.ConfigPlanMeasure;
        }

		public OptionsForm(bool standardWerte) {
			InitializeComponent();

			this.megCeiling.Filter = CategoryType.Ceiling;
			this.megDistributor.Filter = CategoryType.Distributor;
			this.megFloor.Filter = CategoryType.Floor;
			this.megGeneral.Filter = CategoryType.General;
			this.megInsulation.Filter = CategoryType.Insulation;
			this.megWall.Filter = CategoryType.Wall;

			this.SetLanguague();
			this.cmbPlanUnit.SelectedIndex = Product.ConfigPlanMeasure;

			if (standardWerte) {
				this.tabDefaultSystemParameters.Show();
			}
		}

		private void SetLanguague() {
			this.Text = EuroplanRes.OptionsForm_Titel; //"Einstellungen"
			btnCancel.Text = EuroplanRes.General_Abbrechen; //"&Abbrechen"
			btnOk.Text = EuroplanRes.General_Ok; //"&OK"
			button1.Text = EuroplanRes.OptionsForm_Aendern; //"Ändern"
			button2.Text = EuroplanRes.OptionsForm_Loeschen; //"Löschen"
			label1.Text = EuroplanRes.OptionsForm_Logo; //"Logo für Ausdrucke:"
			lblLanguage.Text = EuroplanRes.OptionsForm_Sprache; //"Sprache:"
			tabConstructions.Text = EuroplanRes.OptionsForm_Konstruktionen; //"Konstruktionen"
			tabGeneral.Text = EuroplanRes.OptionsForm_Allgemein; //"Allgemein"
			tabMaterials.Text = EuroplanRes.OptionsForm_Artikelstamm; //"Artikelstamm"
			tabPageCeiling.Text = EuroplanRes.OptionsForm_Decke; //"Decke"
			tabPageDistributor.Text = EuroplanRes.OptionsForm_Verteiler; //"Verteiler"
			tabPageFloor.Text = EuroplanRes.OptionsForm_Fussboden; //"Fußboden"
			tabPageGeneral.Text = EuroplanRes.OptionsForm_Allgmein; //"Allgemein"
			tabPageInsulation.Text = EuroplanRes.OptionsForm_Daemmung; //"Dämmung"
			tabPageWall.Text = EuroplanRes.OptionsForm_Wand; //"Wand"
			tabDefaultSystemParameters.Text = EuroplanRes.OptionsForm_StandardSystemparameter;
            cbOrthoRasterung.Text = EuroplanRes.OptionsForm_OrthoRasterung;
            cbAutoSave.Text = Properties.Resources.OptionsForm_AutomatischSichern;
            this.cbAutoSave.Text = EuroplanRes.OptionsForm_AutomatischSichern;

			this.lblPlanUnit.Text = EuroplanRes.SystemParametersPanel_PlaeneEinheit;
			this.cmbPlanUnit.Items.Clear();
			this.cmbPlanUnit.Items.Add(EuroplanRes.SystemParametersPanel_PlaeneEinheitM);
			this.cmbPlanUnit.Items.Add(EuroplanRes.SystemParametersPanel_PlaeneEinheitCM);
			this.cmbPlanUnit.Items.Add(EuroplanRes.SystemParametersPanel_PlaeneEinheitMM);
		}

		public bool RestartRequired {
			get {
				return restartRequired;
			}
		}

		private void OptionsForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["OptionsForm"];
			this.Location = settings.GetPoint("Location", this.Location);

			string executablePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
			string[] directories = Directory.GetDirectories(executablePath);
			bool germanFound = false;
			foreach (string s in directories) {
				try {
					DirectoryInfo langDirectory = new DirectoryInfo(s);
					FileInfo fi = new FileInfo(Path.Combine(s, "Common.resources.dll"));
					if (fi.Exists) {
						if (langDirectory.Name.Equals("de", StringComparison.InvariantCultureIgnoreCase)) {
							germanFound = true;
						}
						cmbLanguage.Items.Add(CultureInfo.GetCultureInfo(langDirectory.Name));
					}
				} catch (Exception) {
				}
			}
			if (!germanFound) {
				cmbLanguage.Items.Add(CultureInfo.GetCultureInfo("de"));
				
			}
			string partnerLogo = Configuration.UserTemplate.PartnerLogo;
			if (partnerLogo != "" && File.Exists(partnerLogo)) {
				this.pictureBox1.Image = Image.FromFile(partnerLogo);
			}
			cmbLanguage.SelectedItem = CultureInfo.GetCultureInfo((string)settings.GetSetting("Language", "de"));
			cmbPlanUnit.SelectedIndex = Product.ConfigPlanMeasure;
            cbOrthoRasterung.Checked = Product.ConfigActivateOrthoRasterung;
            cbAutoSave.Checked = Product.ConfigAutoSave;
		}

		private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["OptionsForm"];
			settings.StorePoint("Location", this.Location);
			if (DialogResult == DialogResult.OK) {
				if (cmbLanguage.SelectedItem.ToString() != settings.GetSetting("Language", "de")) {
					settings.StoreSetting("Language", cmbLanguage.SelectedItem.ToString());
					restartRequired = true;
				}
			}
			SettingsFile.Update();
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			Configuration.UserTemplate.Reset();
		}

		private void btnOk_Click(object sender, EventArgs e) {
			Product.ConfigPlanMeasure = cmbPlanUnit.SelectedIndex;
            Product.ConfigActivateOrthoRasterung = cbOrthoRasterung.Checked;
            Product.ConfigAutoSave = cbAutoSave.Checked;
			Configuration.UserTemplate.Save();
		}

		private void button1_Click(object sender, EventArgs e) {
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.DefaultExt = "jpg";
			dialog.Filter = EuroplanRes.OptionsForm_LogoFilter + "|*.BMP;*.JPG;*.PNG";
			dialog.Multiselect = false;
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				string partnerLogo = dialog.FileName;
				this.pictureBox1.Image = Image.FromFile(partnerLogo);
				Configuration.UserTemplate.PartnerLogo = partnerLogo;
			}
		}

		private void button2_Click(object sender, EventArgs e) {
			pictureBox1.Image = null;
			Configuration.UserTemplate.PartnerLogo = "";
		}
	}
}