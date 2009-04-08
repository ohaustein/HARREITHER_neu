using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
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
				this.txtName.Text = floor.Name;
				this.floorRoomsSource.DataSource = this.floor.Rooms;
				this.floorRoomsSource.ResetBindings(false);
			}		
		}

		public bool AllowLeave() {
			return true;
		}

		private void txtName_TextChanged(object sender, EventArgs e) {
			this.floor.Name = this.txtName.Text;
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(this);
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
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(this);
			}
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

	}
}
