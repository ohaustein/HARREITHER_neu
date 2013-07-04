using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class NewHeatingSystemForm : Form {
		public NewHeatingSystemForm() {
			InitializeComponent();

			this.SetLanguage();

			Licensing.License license = Licensing.LicenseManager.Instance.License;
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEuroval)) {
				this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(EurovalProduct)));
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdJumboval)) {
				this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(JumbovalProduct)));
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEcotherm)) {
				this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(EcothermProduct)));
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHitherm)) {
				this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(HithermProduct)));
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHithermCompact)) {
				this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(HithermCompactProduct)));
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaBoden)) {
				this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(ModulKlimaBodenProduct)));
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaDecke)) {
				this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(ModulKlimaDeckeProduct)));
			}
			if (this.lstHeatingSystems.Items.Count > 0) {
				this.lstHeatingSystems.Items[0].Selected = true;
			}
		}

		private void SetLanguage() {
			this.btnOk.Text = EuroplanRes.General_Ok;
			this.button2.Text = EuroplanRes.General_Abbrechen;

			this.Text = EuroplanRes.NewHeatingSystemForm_Titel; //"Bitte wählen Sie das gewünschte Heizungssystem"
		}

		private class HeatingSystemItem : ListViewItem {
			private Type productType;

			public HeatingSystemItem(Type productType) {
				this.productType = productType;
				object[] attributes = productType.GetCustomAttributes(typeof(ProductNameAttribute), true);

				this.Name = productType.FullName;
				if (attributes.Length > 0) {
					this.Text = (attributes[0] as ProductNameAttribute).FullName;
				} else {
					this.Text = productType.Name;
				}
			}

			public Type ProductType {
				get { return this.productType; }
			}
		}

		public Type SelectedProductType {
			get {
				if (this.lstHeatingSystems.SelectedItems.Count > 0) {
					return (this.lstHeatingSystems.SelectedItems[0] as HeatingSystemItem).ProductType;
				} else {
					return null;
				}
			}
		}

		private void NewHeatingSystemForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.lstHeatingSystems.SelectedItems.Count == 0 && this.DialogResult == DialogResult.OK) {
				MessageBox.Show(EuroplanRes.NewHeatingSystemForm_KeinSystemText, EuroplanRes.NewHeatingSystemForm_KeinSystemTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				e.Cancel = true;
			}
			SettingsKey settings = SettingsFile.Settings["NewHeatingSystemForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void NewHeatingSystemForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewHeatingSystemForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void lstHeatingSystems_DoubleClick(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}