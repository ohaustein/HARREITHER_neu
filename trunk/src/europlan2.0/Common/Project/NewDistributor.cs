using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class NewDistributor : Form {

		private Distributor distributor = null;

		public NewDistributor() {
			InitializeComponent();
			Inititalize();
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
			while (distributorIds.Contains("VT" + count)) {
				count++;
			}
			txtId.Text = "VT" + count;
			foreach (RegulatorCircuit circuit in project.RegulatorCircuits) {
				cmbCircuit.Items.Add(circuit);
			}
			if (cmbCircuit.Items.Count > 0) {
				cmbCircuit.SelectedIndex = 0;
			}
		}

		private void NewDistributor_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult == DialogResult.OK) {
				if (txtId.Text == "") {
					MessageBox.Show("Bitte geben Sie eine eindeutige Verteilernummer ein.", "Ungültige Verteilernummer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
						MessageBox.Show("Bitte geben Sie eine eindeutige Verteilernummer ein.", "Ungültige Verteilernummer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						e.Cancel = true;
					}
				}
				if (txtName.Text == "") {
					MessageBox.Show("Bitte geben Sie eine Bezeichnung für den Verteiler ein.", "Fehlender Bezeichner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
	}
}