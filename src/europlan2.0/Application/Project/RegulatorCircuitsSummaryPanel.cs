using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class RegulatorCircuitsSummaryPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;
		
		public RegulatorCircuitsSummaryPanel() {
			InitializeComponent();
		}

		public void UpdateControl() {
			regulatoryCircuitsSource.DataSource = Project.Instance.RegulatorCircuits;
			regulatoryCircuitsSource.ResetBindings(false);
		}

		public bool AllowLeave() {
			return true;
		}

		private void regulatoryCircuitsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridRegulatoryCircuits.Columns.Count &&
					(this.gridRegulatoryCircuits.Columns[e.ColumnIndex] == this.nameDataGridViewTextBoxColumn) &&
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

	}
}
