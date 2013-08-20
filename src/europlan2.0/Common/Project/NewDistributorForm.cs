using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class NewDistributorForm : Form {

		private Distributor distributor = null;

		public NewDistributorForm() {
			InitializeComponent();

			this.SetLanguage();

			Inititalize();
		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"&Abbrechen"
			this.btnOk.Text = EuroplanRes.General_Ok; //"&OK"

			this.label1.Text = EuroplanRes.NewDistributorForm_NeuerVerteiler; //"Neuer Verteiler"
			this.label2.Text = EuroplanRes.NewDistributorForm_Nummer; //"Nummer:"
			this.label3.Text = EuroplanRes.NewDistributorForm_Bezeichnung; //"Bezeichnung:"
			this.label4.Text = EuroplanRes.NewDistributorForm_Regelkreis; //"Regelkreis:"
			this.Text = EuroplanRes.NewDistributorForm_VerteilerAnlegen; //"Verteiler anlegen"
		}

		private void Inititalize() {
			Project project = Project.Instance;
			List<string> distributorIds = new List<string>();
			foreach (Floor floor in project.Floors) {
				foreach (Distributor distributor in floor.Distributors) {
					distributorIds.Add(distributor.Id);
				}
			}
			int count = distributorIds.Count + 1;
			while (distributorIds.Contains("VT" + String.Format("{0:00}", count))) {
				count++;
			}
			txtId.Text = "VT" + String.Format("{0:00}", count);
			foreach (RegulatorCircuit circuit in project.RegulatorCircuits) {
				cmbCircuit.Items.Add(circuit);
			}
			if (cmbCircuit.Items.Count > 0) {
				cmbCircuit.SelectedIndex = 0;
			}
		}

		private void NewDistributor_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewDistributorForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
			if (this.DialogResult == DialogResult.OK) {
                if ((this.cmbCircuit.SelectedItem as RegulatorCircuit).UsedForTichelmann) {
                    MessageBox.Show(EuroplanRes.NewDistributorForm_RegelkreisTichelmannText, EuroplanRes.NewDistributorForm_RegelkreisTichelmannTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                } else if (txtId.Text == "") {
					MessageBox.Show(EuroplanRes.NewDistributorForm_UngueltigeVerteilernummerText, EuroplanRes.NewDistributorForm_UngueltigeVerteilernummerTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					e.Cancel = true;
				} else {
					Project project = Project.Instance;
					List<string> distributorIds = new List<string>();
					foreach (Floor floor in project.Floors) {
						foreach (Distributor d in floor.Distributors) {
							distributorIds.Add(d.Id);
						}
					}
					if (distributorIds.Contains(txtId.Text)) {
						MessageBox.Show(EuroplanRes.NewDistributorForm_UngueltigeVerteilernummerText, EuroplanRes.NewDistributorForm_UngueltigeVerteilernummerTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						e.Cancel = true;
					}
				}
				if (!e.Cancel && txtName.Text == "") {
					MessageBox.Show(EuroplanRes.NewDistributorForm_FehlendeBezeichnungText, EuroplanRes.NewDistributorForm_FehlendeBezeichnungTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					e.Cancel = true;
				}
				if (!e.Cancel) {
					this.distributor = new Distributor();
					this.distributor.Id = txtId.Text;
					this.distributor.Name = txtName.Text;
					this.distributor.RegulatorCircuit = cmbCircuit.SelectedItem as RegulatorCircuit;
				}
			}
		}

		public Distributor Distributor {
			get { return distributor; }
		}

		private void NewDistributorForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewDistributorForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}
	}
}