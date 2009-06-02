using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
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
			List<DataGridViewColumn> selectedCols = null;
			Floor selectedFloor = null;
			if (this.gridFloors.SelectedRows.Count > 0) {
				selectedFloor = this.gridFloors.SelectedRows[0].DataBoundItem as Floor;
			} else if (this.gridFloors.SelectedCells.Count > 0) {
				selectedCols = new List<DataGridViewColumn>();
				foreach (DataGridViewCell cell in this.gridFloors.SelectedCells) {
					if (selectedFloor == null) {
						selectedFloor = cell.OwningRow.DataBoundItem as Floor;
					}
					if (selectedFloor != null && selectedFloor == cell.OwningRow.DataBoundItem) {
						selectedCols.Add(cell.OwningColumn);
					}
				}
			}

			projectFloorsSource.DataSource = Project.Instance.Floors;
			projectFloorsSource.ResetBindings(false);

			if (selectedFloor != null) {
				foreach (DataGridViewRow row in this.gridFloors.Rows) {
					if (row.DataBoundItem == selectedFloor) {
						if (selectedCols == null) {
							row.Selected = true;
						} else {
							foreach (DataGridViewColumn col in selectedCols) {
								if (row.Cells[col.Index].Visible) {
									row.Cells[col.Index].Selected = true;
								}
							}
						}
					}
				}
			}
		}

		public bool AllowLeave() {
			return true;
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

		private void gridFloors_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
			if (e.RowIndex >= 0 && e.RowIndex < this.gridFloors.Rows.Count &&
					this.gridFloors.Rows[e.RowIndex].DataBoundItem == null) {
				e.PaintCells(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.Focus | DataGridViewPaintParts.SelectionBackground);
				e.PaintHeader(DataGridViewPaintParts.All);
				e.Handled = true;
			}
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
			gridFloors.AllowUserToAddRows = true;
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(this);
			}
		}

		private void gridFloors_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			gridFloors.AllowUserToAddRows = false;
		}
	}
}
