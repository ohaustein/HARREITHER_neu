using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class RegulatorCircuitsSummaryPanel : UserControl, IEditorUserControl {

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
		
		public RegulatorCircuitsSummaryPanel() {
			InitializeComponent();

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.btnNext.Text = EuroplanRes.General_Weiter; //"Weiter"

			this.idDataGridViewTextBoxColumn.HeaderText = EuroplanRes.RegulatoryCircuitSummaryPanel_Nummer; //"Nr."
			this.idDataGridViewTextBoxColumn.ToolTipText = EuroplanRes.RegulatoryCircuitSummaryPanel_NummerLang; //"Eindeutige Regelkreisnummer"
			this.nameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.RegulatoryCircuitSummaryPanel_Bezeichnung; //"Bezeichnung"
			this.nameDataGridViewTextBoxColumn.ToolTipText = EuroplanRes.RegulatoryCircuitSummaryPanel_BezeichnungLang; //"Bezeichnung des Regelkreises"
			this.heatFlowTemperatureDataGridViewTextBoxColumn.HeaderText = EuroplanRes.RegulatoryCircuitSummaryPanel_VorlauftemperaturHeiz; //"TvHeiz (°C)"
			this.heatFlowTemperatureDataGridViewTextBoxColumn.ToolTipText = EuroplanRes.RegulatoryCircuitSummaryPanel_VorlauftemperaturHeizLang; //"Vorlauftemperatur im Heizbetrieb"
			this.coolFlowTemperatureDataGridViewTextBoxColumn.HeaderText = EuroplanRes.RegulatoryCircuitSummaryPanel_VorlauftemperaturKuehl; //"TvKühl (°C)"
			this.coolFlowTemperatureDataGridViewTextBoxColumn.ToolTipText = EuroplanRes.RegulatoryCircuitSummaryPanel_VorlauftemperaturKuehlLang; //"Vorlauftemperatur im Kühlbetrieb"
			this.label1.Text = EuroplanRes.RegulatoryCircuitSummaryPanel_Regelkreise; //"Regelkreise"
		}

		public void UpdateControl(bool resetUserInterface) {
			regulatoryCircuitsSource.DataSource = Project.Instance.RegulatorCircuits;
			regulatoryCircuitsSource.ResetBindings(false);
			coolFlowTemperatureDataGridViewTextBoxColumn.Visible = Project.Instance.CalculateCoolLoad;
		}

		public bool AllowLeave() {
			if (Project.Instance.CalculateCoolLoad) {
				double dewPoint = Math.Round(EN1264.Instance.TaupunktTemperatur(((double)Project.Instance.RelativeHumidity) / 100, (double)Project.Instance.InsideTemperatureForCooling), 2);
				foreach (RegulatorCircuit circuit in Project.Instance.RegulatorCircuits) {
					if (dewPoint > circuit.CoolFlowTemperature) {
						DialogResult result = MessageBox.Show(EuroplanRes.FacilityDetailsSummaryPanel_KuehltemperaturZuNiedrigText, EuroplanRes.FacilityDetailsSummaryPanel_KuehltemperaturZuNiedrigTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
						return result.Equals(DialogResult.Yes) ? false : true;
					}
				}
			}
			return true;
		}

		private void regulatoryCircuitsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridRegulatoryCircuits.Columns.Count &&
					e.RowIndex >= 0 && e.RowIndex < this.gridRegulatoryCircuits.Rows.Count) {
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void regulatoryCircuitsGrid_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

		private void btnNext_Click(object sender, EventArgs e) {
            if (this.treeSelectionRequested != null) {
                this.treeSelectionRequested(this, typeof(FloorListSummaryPanel));
			}
		}

		private void gridRegulatoryCircuits_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			RegulatorCircuit rc = e.Row.DataBoundItem as RegulatorCircuit;
			if (rc != null) {
				if (rc.ConnectedDistributors.Count > 0) {
					if (MessageBox.Show(EuroplanRes.RegulatoryCircuitSummaryPanel_VerteilerLoeschenText, EuroplanRes.RegulatoryCircuitSummaryPanel_VerteilerLoeschenTitel, MessageBoxButtons.OKCancel) == DialogResult.OK) {
						foreach (Floor floor in Project.Instance.Floors) {
							List<Distributor> delDists = new List<Distributor>();
							foreach (Distributor dist in floor.Distributors) {
								if (dist.RegulatorCircuit == rc) {
									delDists.Add(dist);
									List<PlannedProduct> connectedProducts = dist.PlannedConnectedProducts;
									foreach (PlannedProduct pp in connectedProducts) {
										pp.Product.PlannedConnection = null;
									}
									foreach (PlannedProduct pp in connectedProducts) {
										pp.ConfigureProduct(false);
									}
								}
							}
							foreach (Distributor dist in delDists) {
								floor.Distributors.Remove(dist);
							}
						}
					} else {
						e.Cancel = true;
					}
				}
			}
		}

	}
}
