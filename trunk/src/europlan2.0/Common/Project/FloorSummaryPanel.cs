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
			//this.gridRooms.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			this.gridRooms.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
			this.gridRooms.CellPainting += new DataGridViewCellPaintingEventHandler(gridRooms_CellPainting);
			this.gridRooms.Paint += new PaintEventHandler(gridRooms_Paint);
			this.gridRooms.ColumnWidthChanged += new DataGridViewColumnEventHandler(gridRooms_ColumnWidthChanged);
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
			this.btnRemoveDistributor.Enabled = floor.Distributors.Count > 0;

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
			RoomCoolTemperature.Visible = Project.Instance.CalculateCoolLoad;
			RoomRelativeHumidity.Visible = Project.Instance.CalculateCoolLoad;
			CoolLoad.Visible = Project.Instance.CalculateCoolLoad;
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
					e.RowIndex >= 0 && e.RowIndex < this.gridRooms.Rows.Count) {
				if (this.gridRooms.Columns[e.ColumnIndex] == this.nameDataGridViewTextBoxColumn && ProjectStructureChanged != null) {
					ProjectStructureChanged(this);
				}
				if (ProjectChanged != null) {
					ProjectChanged(this);
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
			if (Project.Instance != null && Project.Instance.RegulatorCircuits.Count > 0) {
				NewDistributorForm form = new NewDistributorForm();
				DialogResult result = form.ShowDialog();
				if (result == DialogResult.OK) {
					if (form.Distributor != null) {
						this.floor.Distributors.Add(form.Distributor);
						if (ProjectStructureChanged != null) {
							ProjectStructureChanged(this);
						}
					}
				}
				form.Dispose();
			} else {
				MessageBox.Show("Ein Verteiler benötigt einen Regelkreis, an den er angeschlossen werden kann. Bitte legen Sie unter 'Regelkreise' zumindest einen Regelkreis an", "Kein Regelkreis vorhanden", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void btnRemoveDistributor_Click(object sender, EventArgs e) {
			if (Project.Instance != null) {
				SelectDistributorForm form = new SelectDistributorForm(floor.Distributors);
				if (form.ShowDialog() == DialogResult.OK) {
					Distributor toDelete = form.SelectedDistributor;
					if (toDelete != null) {
						bool usedForQuickDimensioning = false;
						foreach (Floor f in Project.Instance.Floors) {
							foreach (Room r in f.Rooms) {
								foreach (Product p in r.UsedProductsForQuickDimensioning) {
									if (p.QuickDimensioningConnectedDistributors.ContainsKey(toDelete.Id)) {
										usedForQuickDimensioning = true;
									}
								}
							}
						}
						if (usedForQuickDimensioning) {
							if (MessageBox.Show("Die Zuordnung von Heizkreisen and diesen Verteiler geht in der Flächenaufstellung verloren, wenn der Verteiler gelöscht wird. Trotzdem löschen?", "Verteiler löschen?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
								foreach (Floor f in Project.Instance.Floors) {
									foreach (Room r in f.Rooms) {
										foreach (Product p in r.UsedProductsForQuickDimensioning) {
											if (p.QuickDimensioningConnectedDistributors.ContainsKey(toDelete.Id)) {
												p.QuickDimensioningConnectedDistributors.Remove(toDelete.Id);
											}
										}
									}
								}
								floor.Distributors.Remove(toDelete);
								if (ProjectStructureChanged != null) {
									ProjectStructureChanged(this);
								}
							}
						} else {
							floor.Distributors.Remove(toDelete);
							if (ProjectStructureChanged != null) {
								ProjectStructureChanged(this);
							}
						}
					}
				}
				form.Dispose();
			}
		}


		void gridRooms_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e) {
			if (e.Column.DisplayIndex >= 3 && e.Column.DisplayIndex <= 6) {
				this.gridRooms.InvalidateCell(3, -1);
				this.gridRooms.InvalidateCell(4, -1);
				this.gridRooms.InvalidateCell(5, -1);
				this.gridRooms.InvalidateCell(6, -1);
			}
			if (e.Column.DisplayIndex >= 7 && e.Column.DisplayIndex <= 9) {
				this.gridRooms.InvalidateCell(7, -1);
				this.gridRooms.InvalidateCell(8, -1);
				this.gridRooms.InvalidateCell(9, -1);
			}
		}

		void gridRooms_Paint(object sender, PaintEventArgs e) {
			Rectangle r1 = this.gridRooms.GetCellDisplayRectangle(3, -1, true); //get the column header cell
			Rectangle r2 = this.gridRooms.GetCellDisplayRectangle(4, -1, true); //get the column header cell
			Rectangle r3 = this.gridRooms.GetCellDisplayRectangle(5, -1, true); //get the column header cell
			Rectangle r4 = this.gridRooms.GetCellDisplayRectangle(6, -1, true); //get the column header cell

			r1.X += 1;
			r1.Y += 1;
			r1.Width = r1.Width + r2.Width + r3.Width + r4.Width - 4;
			r1.Height = r1.Height / 2 - 2;
			//e.Graphics.FillRectangle(new SolidBrush(this.gridRooms.ColumnHeadersDefaultCellStyle.BackColor), r1);
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			
			e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			//e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), new Point(r1.X, r1.Y + 2), new Point(r1.X + r1.Width, r1.Y + 2));
			//e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), new Point(r1.X, r1.Y + r1.Height - 7), new Point(r1.X + r1.Width, r1.Y + r1.Height - 7));
			e.Graphics.DrawString("Heizbetrieb",
				this.gridRooms.ColumnHeadersDefaultCellStyle.Font,
				new SolidBrush(this.gridRooms.ColumnHeadersDefaultCellStyle.ForeColor),
				r1,
				format);
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X, r1.Y + r1.Height - 5), new Point(r1.X + r1.Width, r1.Y + r1.Height - 5));
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X, r1.Y), new Point(r1.X, r1.Y + (r1.Height * 2)));
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X + r1.Width, r1.Y), new Point(r1.X + r1.Width, r1.Y + (r1.Height * 2)));

			r1 = this.gridRooms.GetCellDisplayRectangle(7, -1, true); //get the column header cell
			r2 = this.gridRooms.GetCellDisplayRectangle(8, -1, true); //get the column header cell
			r3 = this.gridRooms.GetCellDisplayRectangle(9, -1, true); //get the column header cell

			r1.X += 1;
			r1.Y += 1;
			r1.Width = r1.Width + r2.Width + r3.Width - 4;
			r1.Height = r1.Height / 2 - 2;
			//e.Graphics.FillRectangle(new SolidBrush(this.gridRooms.ColumnHeadersDefaultCellStyle.BackColor), r1);
			format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawString("Kühlbetrieb",
				this.gridRooms.ColumnHeadersDefaultCellStyle.Font,
				new SolidBrush(this.gridRooms.ColumnHeadersDefaultCellStyle.ForeColor),
				r1,
				format);
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X, r1.Y + r1.Height - 5), new Point(r1.X + r1.Width, r1.Y + r1.Height - 5));
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X, r1.Y), new Point(r1.X, r1.Y + (r1.Height * 2)));
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X + r1.Width, r1.Y), new Point(r1.X + r1.Width, r1.Y + (r1.Height * 2)));

		}

		void gridRooms_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
			if (e.RowIndex == -1 && e.ColumnIndex > -1) {
				e.PaintBackground(e.CellBounds, false);

				Rectangle r2 = e.CellBounds;
				r2.Y += e.CellBounds.Height / 2;
				r2.Height = e.CellBounds.Height / 2;
				e.PaintContent(r2);
				e.Handled = true;
			}
		}

		private void btnWhatIsNext_Click(object sender, EventArgs e) {
			MessageBox.Show("Klicken Sie auf einen der Buttons in der Spalte\n'Bearbeiten' um den entsprechenden Raum zu öffnen, oder\n klicken Sie in der Projekthierarchie auf den gewünschten Raum.", "Wie geht's weiter?");
		}
	}
}
