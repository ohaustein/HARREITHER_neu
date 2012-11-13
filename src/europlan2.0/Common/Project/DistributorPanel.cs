using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class DistributorPanel : UserControl, IEditorUserControl {

        private event ProjectStructureChangedHandler projectStructureChanged;
        public event ProjectStructureChangedHandler ProjectStructureChanged {
            add { this.projectStructureChanged += value; }
            remove { this.projectStructureChanged -= value; }
        }
        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }
        private event TreeSelectionRequestedHandler treeSelectionRequested;
        public event TreeSelectionRequestedHandler TreeSelectionRequested {
            add { this.treeSelectionRequested += value; }
            remove { this.treeSelectionRequested -= value; }
        }

		private Distributor distributor;

		public DistributorPanel() {
			InitializeComponent();

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.label1.Text = EuroplanRes.DistributorPanel_Verteilerdaten; //"Verteilerdaten"
			this.label2.Text = EuroplanRes.DistributorPanel_Nummer; //"Nummer:"
			this.label3.Text = EuroplanRes.DistributorPanel_Bezeichnung; //"Bezeichnung:"
			this.label4.Text = EuroplanRes.DistributorPanel_Regelkreis; //"Regelkreis:"
			this.label5.Text = EuroplanRes.DistributorPanel_MaximaleHeizkreise; //"max. Heizkreise:"
			this.label6.Text = EuroplanRes.DistributorPanel_Geschosse; //"Geschoße, die diesen Verteiler auch nutzen können:"
			this.chkEinbauschrank.Text = EuroplanRes.DistributorPanel_Einbauschrank; //"Einbauschrank"
			this.chkFlansch.Text = EuroplanRes.DistributorPanel_Flanschkugelhaehne; //"Flanschkugelhähne"
			this.label9.Text = EuroplanRes.DistributorPanel_Verteilertyp; //"Verteilertyp:"
			this.label10.Text = EuroplanRes.DistributorPanel_Anschlusshollaender; //"Anschlußholländer:"
			this.label11.Text = EuroplanRes.DistributorPanel_Zubehoer; //"Zubehör:"
			this.chkAnschluss.Text = EuroplanRes.DistributorPanel_LangeAnschlussboegen; //"Lange Anschlussbögen"
			this.label7.Text = EuroplanRes.DistributorPanel_ZusaetzlicheHeizkreise; //"zus. Heizkreise:"
			this.label8.Text = EuroplanRes.DistributorPanel_ZusaetzlicheStellantriebe; //"zus. Stellantriebe:"
			this.label12.Text = EuroplanRes.DistributorPanel_Heizsyteme; //"Heizsysteme, die standard- mäßig an diesen Verteiler angeschlossen werden sollen:"
			this.label13.Text = EuroplanRes.DistributorPanel_ZugewieseneHeizkreise; //"zugewiesene Heizkreise: "
			this.lblCircuits.Text = EuroplanRes.DistributorPanel_Aktiv.Replace("%VALUE%", "7"); //"7 (aktiv)"
			this.btnGraphicalPosition.Text = EuroplanRes.DistributorPanel_GraphicalPosition; //"Grafische Positionierung"
		}

		public void UpdateControl(bool resetUserInterface) {
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
				this.lstSystems.Items.Add(EuroplanRes.DistributorPanel_Fussboden, distributor.UseForFloor);
				this.lstSystems.Items.Add(EuroplanRes.DistributorPanel_Wand, distributor.UseForWall);
				this.lstSystems.Items.Add(EuroplanRes.DistributorPanel_Decke, distributor.UseForCeiling);
				this.cmbCircuit.SelectedItem = distributor.RegulatorCircuit;
				this.cmbAnschlussHollaender.SelectedItem = distributor.AnschlussHollaender;

				this.btnGraphicalPosition.Enabled = distributor.AssociatedFloor.AssociatedPlanId != null && distributor.AssociatedFloor.AssociatedPlanId != "";

				UpdateCircuitsLabel();

				this.UpdateErrorMessages();
			}
		}

		private void UpdateErrorMessages() {
			this.lstError.Items.Clear();
			string[] messages = this.distributor.ErrorMessageArray;
			foreach (string message in messages) {
				if (!string.IsNullOrEmpty(message)) {
					ListViewItem item = new ListViewItem(message);
					item.ForeColor = Color.Red;
					this.lstError.Items.Add(item);
				}
			}
			if (lstError.Items.Count > 0) {
				this.lstError.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				int height = this.lstError.Items[this.lstError.Items.Count - 1].Position.Y + this.lstError.Items[this.lstError.Items.Count - 1].Bounds.Height + 5;
				this.lstError.Height = height;
				this.lstError.Visible = true;
			} else {
				this.lstError.Visible = false;
			}
		}

		public bool AllowLeave() {
			return true;
		}

		private void txtName_TextChanged(object sender, EventArgs e) {
			distributor.Name = this.txtName.Text;
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void cmbCircuit_SelectedIndexChanged(object sender, EventArgs e) {
			distributor.RegulatorCircuit = this.cmbCircuit.SelectedItem as RegulatorCircuit;
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void numMaxCircuits_ValueChanged(object sender, EventArgs e) {
			distributor.MaxCircuits = (int)this.numMaxCircuits.Value;
			numAdditionalCircuits.Maximum = distributor.MaxCircuits;
			UpdateCircuitsLabel();
			this.UpdateErrorMessages();
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}


		private void numAdditionalCircuits_ValueChanged(object sender, EventArgs e) {
			distributor.AdditionalCircuits = (int)this.numAdditionalCircuits.Value;
			this.numZusStellantriebe.Maximum = (int)this.numAdditionalCircuits.Value;
			UpdateCircuitsLabel();
			this.UpdateErrorMessages();
			this.UpdateCircuitsLabel();
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void numZusStellantriebe_ValueChanged(object sender, EventArgs e) {
			distributor.ZusaetzlicheStellantriebe = (int)this.numZusStellantriebe.Value;
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void UpdateCircuitsLabel() {
			int plannedCircuits = this.distributor.PlannedCircuits;
			lblCircuits.Text = EuroplanRes.DistributorPanel_Aktiv.Replace("%VALUE%", plannedCircuits.ToString());
			if (numAdditionalCircuits.Value > 0) {
				lblCircuits.Text += " " + EuroplanRes.DistributorPanel_Zusaetzlich.Replace("%VALUE%", numAdditionalCircuits.Value.ToString()).Replace("%SUM%", (plannedCircuits + numAdditionalCircuits.Value).ToString());
			}
		}

		private void listFloors_ItemCheck(object sender, ItemCheckEventArgs e) {
			Floor floor = this.listFloors.Items[e.Index] as Floor;
			if (e.NewValue == CheckState.Checked) {
				if (!this.distributor.AdditionalFloorIds.Contains(floor.Id)) {
					if (floor.AssociatedPlan != null) {
						DistributorPositionerForm form = new DistributorPositionerForm(this.distributor, floor);
						form.ShowDialog();
						if (form.UnsavedChanges) {
							if (this.projectChanged != null) {
								this.projectChanged(null);
							}
						}
						form.Dispose();
						bool success = false;
						foreach (Distributor.GraphicalRepresentation gp in distributor.GraphicalRepresentations) {
							if (gp.floorId == floor.Id) {
								this.distributor.AdditionalFloorIds.Add(floor.Id);
								success = true;
							}
						}
						if (!success) {
							e.NewValue = e.CurrentValue;
						}
					} else {
						this.distributor.AdditionalFloorIds.Add(floor.Id);
					}
				}
			} else {
				if (this.distributor.AdditionalFloorIds.Contains(floor.Id)) {
					Distributor.GraphicalRepresentation toDelete = new Distributor.GraphicalRepresentation();
					foreach (Distributor.GraphicalRepresentation gp in distributor.GraphicalRepresentations) {
						if (gp.floorId == floor.Id) {
							toDelete = gp;
						}
					}
					if (distributor.GraphicalRepresentations.Contains(toDelete)) {
						distributor.GraphicalRepresentations.Remove(toDelete);
					}
					this.distributor.AdditionalFloorIds.Remove(floor.Id);
				}
			}
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void chkEinbauschrank_CheckedChanged(object sender, EventArgs e) {
			distributor.EinbauSchrank = this.chkEinbauschrank.Checked;
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void cmbAnschlussHollaender_SelectedIndexChanged(object sender, EventArgs e) {
			distributor.AnschlussHollaender = (Distributor.AnschlussHollaenderEnum)this.cmbAnschlussHollaender.SelectedItem;
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void chkFlansch_CheckedChanged(object sender, EventArgs e) {
			distributor.FlanschKugelHaehne = this.chkFlansch.Checked;
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void chkAnschluss_CheckedChanged(object sender, EventArgs e) {
			distributor.LangeAnschlussboegen = this.chkAnschluss.Checked;
			if (this.projectChanged != null) {
				this.projectChanged(null);
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
			if (this.projectChanged != null) {
				this.projectChanged(null);
			}
		}

		private void btnGraphicalPosition_Click(object sender, EventArgs e) {
			DistributorPositionerForm form = new DistributorPositionerForm(this.distributor, this.distributor.AssociatedFloor);
			form.DrawOtherDistributorsInPlan = true;
			form.ShowDialog();
			if (form.UnsavedChanges) {
				if (this.projectChanged != null) {
					this.projectChanged(null);
				}
			}
			form.Dispose();
		}

	}
}
