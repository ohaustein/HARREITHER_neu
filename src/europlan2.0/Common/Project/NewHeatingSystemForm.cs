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
			// TODO: enable again
			//foreach (Type t in this.GetType().Assembly.GetTypes()) {
			//    if (t.IsSubclassOf(typeof(Product))) {
			//        this.lstHeatingSystems.Items.Add(new HeatingSystemItem(t));
			//    }
			//}
			this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(EurovalProduct)));
			this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(EcothermProduct)));
			this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(HithermProduct)));
			this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(ModulKlimaBodenProduct)));
			this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(ModulKlimaDeckeProduct)));
			this.lstHeatingSystems.Items.Add(new HeatingSystemItem(typeof(HithermCompactProduct)));
			if (this.lstHeatingSystems.Items.Count > 0) {
				this.lstHeatingSystems.Items[0].Selected = true;
			}
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
				MessageBox.Show("Bitte wählen Sie ein Heizungssystem aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
	}
}