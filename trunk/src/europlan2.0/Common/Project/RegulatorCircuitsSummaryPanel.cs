using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class RegulatorCircuitsSummaryPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;
		
		public RegulatorCircuitsSummaryPanel() {
			InitializeComponent();
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
						DialogResult result = MessageBox.Show("Bei mindestens einem Regelkreis ist die Kühltemperatur niedriger als der Taupunkt gemäß den eingegebenen Projektdaten. Wollen Sie die Eingaben korrigieren?", "Eingabefehler", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
						return result.Equals(DialogResult.Yes) ? false : true;
					}
				}
			}
			return true;
		}

		private void regulatoryCircuitsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridRegulatoryCircuits.Columns.Count &&
					e.RowIndex >= 0 && e.RowIndex < this.gridRegulatoryCircuits.Rows.Count) {
				if (ProjectChanged != null) {
					ProjectChanged(this);
				}
			}
		}

		private void regulatoryCircuitsGrid_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(this);
			}
		}

		private void btnNext_Click(object sender, EventArgs e) {
			if (TreeSelectionRequested != null) {
				TreeSelectionRequested(this, typeof(FloorListSummaryPanel));
			}
		}

	}
}
