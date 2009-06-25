using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class DistributorPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Distributor distributor;

		public DistributorPanel() {
			InitializeComponent();
		}

		public void UpdateControl() {
			if (this.Tag != null) {
				this.distributor = this.Tag as Distributor;
				this.lblId.Text = distributor.Id;
				this.txtName.Text = distributor.Name;
				this.numMaxCircuits.Value = distributor.MaxCircuits;
				Project project = Project.Instance;
				this.cmbCircuit.Items.Clear();
				foreach (RegulatorCircuit circuit in project.RegulatorCircuits) {
					this.cmbCircuit.Items.Add(circuit);
				}
				this.listFloors.Items.Clear();
				foreach (Floor floor in project.Floors) {
					if (!floor.Distributors.Contains(this.distributor)) {
						this.listFloors.Items.Add(floor, this.distributor.AdditionalFloorIds.Contains(floor.Id));						
					}
				}
				this.cmbCircuit.SelectedItem = distributor.RegulatorCircuit;
			}
		}

		public bool AllowLeave() {
			return true;
		}

		private void txtName_TextChanged(object sender, EventArgs e) {
			distributor.Name = this.txtName.Text;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void cmbCircuit_SelectedIndexChanged(object sender, EventArgs e) {
			distributor.RegulatorCircuit = this.cmbCircuit.SelectedItem as RegulatorCircuit;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numMaxCircuits_ValueChanged(object sender, EventArgs e) {
			distributor.MaxCircuits = (int)this.numMaxCircuits.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void listFloors_ItemCheck(object sender, ItemCheckEventArgs e) {
			Floor floor = this.listFloors.Items[e.Index] as Floor;
			if (e.NewValue == CheckState.Checked) {
				if (!this.distributor.AdditionalFloorIds.Contains(floor.Id)) {
					this.distributor.AdditionalFloorIds.Add(floor.Id);
				}
			} else {
				// TODO: check if distributor is also planned in a floor
				if (this.distributor.AdditionalFloorIds.Contains(floor.Id)) {
					this.distributor.AdditionalFloorIds.Remove(floor.Id);
				}
			}
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

	}
}
