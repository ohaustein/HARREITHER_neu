using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	
	public partial class SelectDistributorForm : Form {

		private List<Distributor> distributors;
		private Distributor selectedDistributor = null;

		public SelectDistributorForm(List<Distributor> distributors) {
			InitializeComponent();

			this.SetLanguage();

			this.distributors = distributors;
			gridDistributors.DataSource = distributors;
		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"Abbrechen"
			this.btnOk.Text = EuroplanRes.General_Ok; //"OK"

			this.idDataGridViewTextBoxColumn.HeaderText = EuroplanRes.SelectDistributorForm_Nummer; //"Id"
			this.nameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.SelectDistributorForm_Bezeichnung; //"Name"
			this.Text = EuroplanRes.SelectDistributorForm_Titel; //"SelectDistributorForm"

		}

		private void SelectDistributorForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["SelectDistributorForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void SelectDistributorForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["SelectDistributorForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();

			selectedDistributor = null;
			if (this.gridDistributors.SelectedRows.Count > 0) {
				selectedDistributor = this.gridDistributors.SelectedRows[0].DataBoundItem as Distributor;
			} else if (this.gridDistributors.SelectedCells.Count > 0) {
				selectedDistributor = this.gridDistributors.SelectedCells[0].OwningRow.DataBoundItem as Distributor;
			}			
		}

		public Distributor SelectedDistributor {
			get { return selectedDistributor; }
		}

	}
}