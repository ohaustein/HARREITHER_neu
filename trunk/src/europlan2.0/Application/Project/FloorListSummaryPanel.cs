using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class FloorListSummaryPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		public FloorListSummaryPanel() {
			InitializeComponent();
		}

		private void btnImport_Click(object sender, EventArgs e) {
			BuildingDataImportManager.Instance.ImportBuildingData();
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(null);
			}
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
			projectFloorsSource.DataSource = Project.Instance.Floors;
			projectFloorsSource.ResetBindings(false);
		}

		public void UpdateControl() {

			projectFloorsSource.DataSource = Project.Instance.Floors;
			projectFloorsSource.ResetBindings(false);
		}

		public bool AllowLeave() {
			return true;
		}

		private void gridFloors_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridFloors.Columns.Count &&
			  (this.gridFloors.Columns[e.ColumnIndex] == this.nameDataGridViewTextBoxColumn) &&
			  e.RowIndex >= 0 && e.RowIndex < this.gridFloors.Rows.Count) {
				if (ProjectStructureChanged != null) {
					ProjectStructureChanged(this);
				}
			}
		}

		private void gridFloors_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(this);
			}
		}

		private void gridFloors_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridFloors.Columns.Count &&
			  this.gridFloors.Columns[e.ColumnIndex] == this.colView &&
			  e.RowIndex >= 0 && e.RowIndex < this.gridFloors.Rows.Count) {
				Floor f = this.gridFloors.Rows[e.RowIndex].DataBoundItem as Floor;
				if (f != null) {
					if (TreeSelectionRequested != null) {
						TreeSelectionRequested(this, f);
					}
				}
			}
		}



	}
}
