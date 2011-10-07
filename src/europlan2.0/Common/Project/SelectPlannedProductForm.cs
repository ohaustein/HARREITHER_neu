using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class SelectPlannedProduct : Form {
		public SelectPlannedProduct(List<PlannedProduct> products) {
			InitializeComponent();

			this.SetLanguage();

			foreach (PlannedProduct product in products) {
				this.lstPlannedProducts.Items.Add(new PlannedProductItem(product));
			}
			if (this.lstPlannedProducts.Items.Count > 0) {
				this.lstPlannedProducts.Items[0].Selected = true;
			}
		}

		private void SetLanguage() {
			this.btnOk.Text = EuroplanRes.General_Ok; //"OK";
			this.button2.Text = EuroplanRes.General_Abbrechen; //"Abbrechen";

			this.colName.Text = EuroplanRes.SelectPlannedProductForm_Name;

			this.Text = EuroplanRes.SelectPlannedProductForm_Titel; //"Bitte wählen Sie das gewünschte Teilsystem";
		}

		private class PlannedProductItem : ListViewItem {
			private PlannedProduct product;

			public PlannedProductItem(PlannedProduct product) {
				this.product = product;
				this.Text = product.ToString();
			}

			public PlannedProduct PlannedProduct {
				get { return this.product; }
			}
		}

		public PlannedProduct SelectedPlannedProduct {
			get {
				if (this.lstPlannedProducts.SelectedItems.Count > 0) {
					return (this.lstPlannedProducts.SelectedItems[0] as PlannedProductItem).PlannedProduct;
				} else {
					return null;
				}
			}
		}

		private void SelectPlannedProduct_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.lstPlannedProducts.SelectedItems.Count == 0 && this.DialogResult == DialogResult.OK) {
				MessageBox.Show(EuroplanRes.SelectPlannedProductForm_KeinSystemText, EuroplanRes.SelectPlannedProductForm_KeinSystemTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				e.Cancel = true;
			}
			SettingsKey settings = SettingsFile.Settings["SelectPlannedProduct"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void SelectPlannedProduct_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["SelectPlannedProduct"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

	}
}