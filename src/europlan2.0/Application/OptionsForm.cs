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

namespace Europlan.Application {
	public partial class OptionsForm : Form {

		private bool restartRequired = false;

		public OptionsForm() {
			InitializeComponent();
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
			foreach (string s in directories) {
				try {
					DirectoryInfo langDirectory = new DirectoryInfo(s);
					cmbLanguage.Items.Add(CultureInfo.GetCultureInfo(langDirectory.Name));
				} catch (Exception) {
				}
			}
			cmbLanguage.SelectedItem = CultureInfo.GetCultureInfo((string)settings.GetSetting("Language", "de"));
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
	}
}