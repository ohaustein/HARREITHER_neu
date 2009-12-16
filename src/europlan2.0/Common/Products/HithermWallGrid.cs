using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class HithermWallGrid : UserControl {
		public HithermWallGrid() {
			InitializeComponent();

			/*List<HithermWall> walls = new List<HithermWall>();

			ConstructionListWrapper wrapper = new ConstructionListWrapper(Configuration.ConfigurationType.UserConfiguration);
			wrapper.ConstructionScopeFilter = ConstructionScopeEnum.WallConstruction;
			//int i = 1;
			foreach (WallConstruction wc in wrapper) {
				HithermWall w = new HithermWall(wc.Id, wc.Name, wc, null, null, false, null, -16, 30, true);
				walls.Add(w);
			}

			this.tempBehindCoolDataGridViewTextBoxColumn.Visible = Project.Instance.CalculateCoolLoad;

			this.hithermWallBindingSource.DataSource = walls;*/
		}

		public List<HithermWall> Walls {
			get { return this.hithermWallBindingSource.DataSource as List<HithermWall>; }
			set {
				this.hithermWallBindingSource.DataSource = value;
				this.hithermWallBindingSource.ResetBindings(false);
			}
		}

		private void dgvWalls_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.dgvWalls.Rows[i];
				if (row.DataBoundItem != null) {
					row.ReadOnly = (row.DataBoundItem as HithermWall).DefaultWall;
					if (row.ReadOnly) {
						row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
					} else {
						row.DefaultCellStyle.ForeColor = SystemColors.ControlText;
					}
				}
			}
		}

		private void dgvWalls_CellEnter(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex == constructionDataGridViewTextBoxColumn.Index && e.RowIndex >= 0 && !dgvWalls.Rows[e.RowIndex].ReadOnly) {
				Rectangle rect = dgvWalls.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
				//int width = dataGridView1.CurrentCell.Size.Width;
				btnSelectConstruction.Location = new Point(rect.X + rect.Width - btnSelectConstruction.Width - 1, rect.Y);
				btnSelectConstruction.Height = rect.Height - 1;
				btnSelectConstruction.Show();
			}
		}

		private void dgvWalls_CellLeave(object sender, DataGridViewCellEventArgs e) {
			btnSelectConstruction.Hide();
		}

		private void btnSelectConstruction_Click(object sender, EventArgs e) {
			SelectHithermWallConstructionForm form = new SelectHithermWallConstructionForm();
			if (form.ShowDialog().Equals(DialogResult.OK)) {
				WallConstruction construction = form.SelectedConstruction;
				DataGridViewCell cell = dgvWalls.Rows[dgvWalls.CurrentCell.RowIndex].Cells[this.Construction.Index];
				if (cell.Value != construction) {
					cell.Value = construction;
					hithermWallBindingSource.ResetBindings(false);
				}
			}
			form.Dispose();
		}

		private void dgvWalls_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) {
			e.Row.Cells[this.additionalInsulationDataGridViewTextBoxColumn.Index].Value = 0.0;
			ConstructionListWrapper clw = new ConstructionListWrapper(Configuration.ConfigurationType.UserConfiguration);
			clw.ConstructionScopeFilter = ConstructionScopeEnum.WallConstruction;
			e.Row.Cells[this.Construction.Index].Value = clw.Count > 0 ? clw[0] : null;
			e.Row.Cells[this.Deckschicht.Index].Value = 0.0;
			e.Row.Cells[this.kValueDataGridViewTextBoxColumn.Index].Value = 0.0;
			e.Row.Cells[this.nameDataGridViewTextBoxColumn.Index].Value = "Neue Wandkonstruktion";
			int maxId = 0;
			int newId;
			foreach (HithermWall hw in this.hithermWallBindingSource.DataSource as List<HithermWall>) {
				if (hw.Id != null && hw.Id.StartsWith("USW")) {
					if (Int32.TryParse(hw.Id.Substring(3), out newId) && newId > maxId) {
						maxId = newId;
					}
				}
			}
			maxId++;
			e.Row.Cells[this.idDataGridViewTextBoxColumn.Index].Value = "USW" + maxId.ToString("00");
			e.Row.Cells[this.tempBehindCoolDataGridViewTextBoxColumn.Index].Value = 30.0;
			e.Row.Cells[this.tempBehindHeatDataGridViewTextBoxColumn.Index].Value = -16.0;
		}
	}
}
