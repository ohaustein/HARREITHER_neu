using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class FloorListSummaryPanel : UserControl, IEditorUserControl {

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

		public FloorListSummaryPanel() {
			InitializeComponent();

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.nameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.FloorListSummaryPanel_Bezeichnung; //"Bezeichnung"
			this.nameDataGridViewTextBoxColumn.ToolTipText = EuroplanRes.FloorListSummaryPanel_BezeichnungLang; //"Bezeichnung des Geschoﬂes"
			this.label1.Text = EuroplanRes.FloorListSummaryPanel_Geschosse; //"Geschoﬂe"
			this.colView.HeaderText = EuroplanRes.General_BearbeitenCol; //"Bearbeiten"
			this.btnWhatIsNext.Text = EuroplanRes.General_WieGehtsWeiter; //"Wie geht\'s weiter?"
		}

		public void UpdateControl(bool resetUserInterface) {
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
                    if (this.treeSelectionRequested != null) {
                        this.treeSelectionRequested(this, f);
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
				if (this.projectStructureChanged != null) {
					this.projectStructureChanged(this);
				}
			}
		}

		private void gridFloors_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			gridFloors.AllowUserToAddRows = true;
			if (this.projectStructureChanged != null) {
				this.projectStructureChanged(this);
			}
		}

		private void gridFloors_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			gridFloors.AllowUserToAddRows = false;
		}

		private void btnWhatIsNext_Click(object sender, EventArgs e) {
			MessageBox.Show(EuroplanRes.FloorListSummaryPanel_WieGehtsWeiterText, EuroplanRes.FloorListSummaryPanel_WieGehtsWeiterTitel);
		}

	}
}
