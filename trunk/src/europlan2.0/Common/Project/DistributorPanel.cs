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
				this.numAdditionalCircuits.Value = distributor.AdditionalCircuits;
				this.numZusStellantriebe.Value = distributor.ZusaetzlicheStellantriebe;
				this.chkFlansch.Checked = distributor.FlanschKugelHaehne;
				this.chkEinbauschrank.Checked = distributor.EinbauSchrank;
				this.chkAnschluss.Checked = distributor.LangeAnschlussboegen;
				Project project = Project.Instance;
				this.cmbCircuit.Items.Clear();
				foreach (RegulatorCircuit circuit in project.RegulatorCircuits) {
					this.cmbCircuit.Items.Add(circuit);
				}
				//this.cmbDistributorType.Items.Clear();
				//foreach (Distributor.DistributorTypeEnum item in Enum.GetValues(typeof(Distributor.DistributorTypeEnum))) {
				//    this.cmbDistributorType.Items.Add(item);
				//}
				this.cmbAnschlussHollaender.Items.Clear();
				foreach (Distributor.AnschlussHollaenderEnum item in Enum.GetValues(typeof(Distributor.AnschlussHollaenderEnum))) {
					this.cmbAnschlussHollaender.Items.Add(item);
				}
				this.listFloors.Items.Clear();
				foreach (Floor floor in project.Floors) {
					if (!floor.Distributors.Contains(this.distributor)) {
						this.listFloors.Items.Add(floor, this.distributor.AdditionalFloorIds.Contains(floor.Id));						
					}
				}
				this.lstSystems.Items.Clear();
				this.lstSystems.Items.Add("Fußboden", distributor.UseForFloor);
				this.lstSystems.Items.Add("Wand", distributor.UseForWall);
				this.lstSystems.Items.Add("Decke", distributor.UseForCeiling);
				this.cmbCircuit.SelectedItem = distributor.RegulatorCircuit;
				//this.cmbDistributorType.SelectedItem = distributor.DistributorType;
				this.cmbAnschlussHollaender.SelectedItem = distributor.AnschlussHollaender;

				UpdateCircuitsLabel();
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
			numAdditionalCircuits.Maximum = distributor.MaxCircuits;
			UpdateCircuitsLabel();
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}


		private void numAdditionalCircuits_ValueChanged(object sender, EventArgs e) {
			distributor.AdditionalCircuits = (int)this.numAdditionalCircuits.Value;
			UpdateCircuitsLabel();
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void UpdateCircuitsLabel() {
			int plannedCircuits = this.distributor.PlannedCircuits;			
			lblCircuits.Text = plannedCircuits + " (aktiv)";
			if (numAdditionalCircuits.Value > 0) {
				lblCircuits.Text += " + " + numAdditionalCircuits.Value + " (zus.) = " + (plannedCircuits + numAdditionalCircuits.Value);
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

		private void chkEinbauschrank_CheckedChanged(object sender, EventArgs e) {
			distributor.EinbauSchrank = this.chkEinbauschrank.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		//private void cmbDistributorType_SelectedIndexChanged(object sender, EventArgs e) {
		//    distributor.DistributorType = (Distributor.DistributorTypeEnum)this.cmbDistributorType.SelectedItem;
		//    if (ProjectChanged != null) {
		//        ProjectChanged(null);
		//    }
		//}

		private void cmbAnschlussHollaender_SelectedIndexChanged(object sender, EventArgs e) {
			distributor.AnschlussHollaender = (Distributor.AnschlussHollaenderEnum)this.cmbAnschlussHollaender.SelectedItem;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void chkFlansch_CheckedChanged(object sender, EventArgs e) {
			distributor.FlanschKugelHaehne = this.chkFlansch.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void chkAnschluss_CheckedChanged(object sender, EventArgs e) {
			distributor.LangeAnschlussboegen = this.chkAnschluss.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numZusStellantriebe_ValueChanged(object sender, EventArgs e) {
			distributor.ZusaetzlicheStellantriebe = (int)this.numZusStellantriebe.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void lstSystems_ItemCheck(object sender, ItemCheckEventArgs e) {
			switch (e.Index) {
				case 0:
					distributor.UseForFloor = e.NewValue == CheckState.Checked;
					break;
				case 1:
					distributor.UseForWall = e.NewValue == CheckState.Checked;
					break;
				case 2:
					distributor.UseForCeiling = e.NewValue == CheckState.Checked;
					break;
			}
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

	}
}
