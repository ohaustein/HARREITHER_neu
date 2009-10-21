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

		public bool unsavedChanges = false;
		
		public ConnectionPipesForm(PlannedProduct product) {
			InitializeComponent();
			connectionPipePanel.Update(product);
			lblConnectionPipe.Text = "Anbindeleitungen durch " + product.Node.Text + " in " + product.Product.AssociatedRoom.ToString();
		}

		private void ConnectionPipesForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ConnectionPipesForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void ConnectionPipesForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ConnectionPipesForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		private void connectionPipePanel_GridContentChanged(object sender) {
			unsavedChanges = true;
		}

	}
}