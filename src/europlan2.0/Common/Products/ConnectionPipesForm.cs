using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Threading;

namespace Europlan.Common {
	public partial class ConnectionPipesForm : Form {

		private bool unsavedChanges = false;
		private PlannedProduct product;
		private System.Resources.ResourceManager resources = EuroplanRes.ResourceManager;

		public ConnectionPipesForm(PlannedProduct product) {
			this.product = product;
			InitializeComponent();
			this.SetLanguage();
			connectionPipePanel.Update(product);
			UpdateLabels();
			string lblText = EuroplanRes.ConnectionPipesForm_AnbindeleitungDurch;
			lblText = lblText.Replace("%SYSTEM%", product.Node.Text);
			lblText = lblText.Replace("%RAUM%", product.Product.AssociatedRoom.ToString());
			lblConnectionPipe.Text = lblText;//"Anbindeleitungen durch " + product.Node.Text + " in " + product.Product.AssociatedRoom.ToString();
		}

		private void SetLanguage() {
			this.button2.Text = EuroplanRes.General_Schliessen; //"Schließen";
			this.label1.Text = EuroplanRes.ConnectionPipesForm_Nettoflaeche; //"Nettofläche des Heizsystems:";
			this.label2.Text = EuroplanRes.ConnectionPipesForm_Anbindeflaeche; //"Durch Anbindeleitungen belegte Fläche:";
			this.label3.Text = EuroplanRes.ConnectionPipesForm_Anbindekuehlleistung; //"Kühlleistung durch Anbindeleitungen:";
			this.label4.Text = EuroplanRes.ConnectionPipesForm_Anbindeheizflaeche; //"Heizleistung durch Anbindeleitungen:";
			this.btnLengthAssistant.Text = EuroplanRes.ConnectionPipesForm_Laengenassistent; //"&Längenassistent";
			this.Text = EuroplanRes.ConnectionPipesForm_Anbindeleitungen; //"Anbindeleitungen";
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
				lblConnenctionPipeArea.Text = Math.Round(areaTotal, 2) + EuroplanRes.General_Quadratmeter; //"m²";
				lblProductArea.Text = product.Product.PlannedNetArea + EuroplanRes.General_Quadratmeter; //"m²";
				lblHeatLoad.Text = Math.Round(heatLoadTotal, 0) + EuroplanRes.General_Watt; //"W";
				lblCoolLoad.Text = Math.Round(coolLoadTotal, 0) + EuroplanRes.General_Watt; //"W";
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
			UpdateLabels();
		}

		private void btnLengthAssistant_Click(object sender, EventArgs e) {
			ConnectionPipe pipe = connectionPipePanel.SelectedConnectionPipe;
			if (pipe != null) {
				LengthAssistantForm form = new LengthAssistantForm(pipe);
				if (form.ShowDialog() == DialogResult.OK) {
					pipe.Vorlauf = form.Vorlauf;
					pipe.Ruecklauf = form.Ruecklauf;
					pipe.Verlegeart = form.Verlegeart;
					unsavedChanges = true;
					UpdateLabels();
					connectionPipePanel.ReloadGrid();
				}
				form.Dispose();
			} else {
				MessageBox.Show(EuroplanRes.ConnectionPipesForm_KeineAnbindeleitungText /*"Bitte wählen Sie eine Anbindeleitung aus."*/, EuroplanRes.ConnectionPipesForm_KeineAnbindeleitungTitel /*"Keine Anbindeleitung ausgewählt"*/);
			}
		}

	}
}