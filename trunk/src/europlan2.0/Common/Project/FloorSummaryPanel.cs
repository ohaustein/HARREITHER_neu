using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class FloorSummaryPanel : UserControl, IEditorUserControl {
		
		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Floor floor;
		
		public FloorSummaryPanel() {
			InitializeComponent();
		}

		public void UpdateControl() {
			if (this.Tag != null) {
				this.floor = this.Tag as Floor;
				this.floorRoomsSource.DataSource = this.floor.Rooms;
			}
			List<DataGridViewColumn> selectedCols = null;
			Room selectedRoom = null;
			if (this.gridRooms.SelectedRows.Count > 0) {
				selectedRoom = this.gridRooms.SelectedRows[0].DataBoundItem as Room;
			} else if (this.gridRooms.SelectedCells.Count > 0) {
				selectedCols = new List<DataGridViewColumn>();
				foreach (DataGridViewCell cell in this.gridRooms.SelectedCells) {
					if (selectedRoom == null) {
						selectedRoom = cell.OwningRow.DataBoundItem as Room;
					}
					if (selectedRoom != null && selectedRoom == cell.OwningRow.DataBoundItem) {
						selectedCols.Add(cell.OwningColumn);
					}
				}
			}

			this.lblFloorName.Text = floor.Name;
			this.floorRoomsSource.ResetBindings(false);

			if (selectedRoom != null) {
				foreach (DataGridViewRow row in this.gridRooms.Rows) {
					if (row.DataBoundItem == selectedRoom) {
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

		private void gridRooms_CellClick(object sender, DataGridViewCellEventArgs e) {
			//this.gridRooms.EditMode = (e.ColumnIndex == -1 ? DataGridViewEditMode.EditOnKeystroke : DataGridViewEditMode.EditOnEnter);
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridRooms.Columns.Count &&
					this.gridRooms.Columns[e.ColumnIndex] == this.colView &&
					e.RowIndex >= 0 && e.RowIndex < this.gridRooms.Rows.Count) {
				Room r = this.gridRooms.Rows[e.RowIndex].DataBoundItem as Room;
				if (r != null) {
					if (TreeSelectionRequested != null) {
						TreeSelectionRequested(this, r);
					}
				}
			}

		}

		private void gridRooms_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
			if (e.RowIndex >= 0 && e.RowIndex < this.gridRooms.Rows.Count &&
					this.gridRooms.Rows[e.RowIndex].DataBoundItem == null) {
				e.PaintCells(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.Focus | DataGridViewPaintParts.SelectionBackground);
				e.PaintHeader(DataGridViewPaintParts.All);
				e.Handled = true;
			}
		}

		private void gridRooms_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridRooms.Columns.Count &&
					(this.gridRooms.Columns[e.ColumnIndex] == this.nameDataGridViewTextBoxColumn) &&
					e.RowIndex >= 0 && e.RowIndex < this.gridRooms.Rows.Count) {
				if (ProjectStructureChanged != null) {
					ProjectStructureChanged(this);
				}
			}
		}

		private void gridRooms_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			gridRooms.AllowUserToAddRows = true;
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(this);
			}
		}

		private void gridRooms_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			gridRooms.AllowUserToAddRows = false;
		}

		private void btnAddDistributor_Click(object sender, EventArgs e) {
			// TODO: check if regulator circuit is available
			NewDistributor form = new NewDistributor();
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK) {
				if (form.Distributor != null) {
					this.floor.Distributors.Add(form.Distributor);
					if (ProjectChanged != null) {
						ProjectChanged(this);
					}
				}
			}
		}
	}
}
