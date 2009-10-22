using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class ConnectionPipesForm : Form {

		private bool unsavedChanges = false;
		private PlannedProduct product;
		
		public ConnectionPipesForm(PlannedProduct product) {
			this.product = product;
			InitializeComponent();
			connectionPipePanel.Update(product);
			UpdateLabels();
			lblConnectionPipe.Text = "Anbindeleitungen durch " + product.Node.Text + " in " + product.Product.AssociatedRoom.ToString();
		}

		private void UpdateLabels() {
			if (product != null) {
				List<ConnectionPipe> pipes = product.Product.PlannedConnectionPipesThroughThisProduct;
				double areaTotal = 0;
				double heatLoadTotal = 0;
				double coolLoadTotal = 0;
				foreach (ConnectionPipe pipe in pipes) {
					areaTotal += pipe.AreaTotal;
					heatLoadTotal += pipe.HeatLoadTotal;
					coolLoadTotal += pipe.CoolLoadTotal;
				}
				lblConnenctionPipeArea.Text = Math.Round(areaTotal, 2) + "m²";
				lblProductArea.Text = product.Product.PlannedNetArea + "m²";
				lblHeatLoad.Text = Math.Round(heatLoadTotal, 0) + "W";
				lblCoolLoad.Text = Math.Round(coolLoadTotal, 0) + "W";
			}
		}

		private void ConnectionPipesForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ConnectionPipesForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void ConnectionPipesForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ConnectionPipesForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		private void connectionPipePanel_GridContentChanged(object sender) {
			unsavedChanges = true;
		}

		private void btnLengthAssistant_Click(object sender, EventArgs e) {
			ConnectionPipe pipe = connectionPipePanel.SelectedConnectionPipe;
			if (pipe != null) {
				LengthAssistantForm form = new LengthAssistantForm(pipe);
				if (form.ShowDialog() == DialogResult.OK) {

				}
				form.Dispose();
			} else {
				MessageBox.Show("Keine Anbindeleitung ausgewählt.", "Keine Anbindeleitung ausgewählt.");
			}
		}

	}
}