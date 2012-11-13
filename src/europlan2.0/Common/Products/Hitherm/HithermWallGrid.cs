using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {
	public partial class HithermWallGrid : UserControl {

		private bool showCompact = false;

		public HithermWallGrid() {
			InitializeComponent();

			this.SetLanguage();

			if (Project.Instance != null) {
				this.tempBehindCoolDataGridViewTextBoxColumn.Visible = Project.Instance.CalculateCoolLoad;

				this.hithermWallBindingSource.DataSource = Project.Instance.HithermWalls;
				this.hithermWallBindingSource.ResetBindings(false);
			}
		}

		private void SetLanguage() {
			this.idDataGridViewTextBoxColumn.HeaderText = EuroplanRes.General_NummerCol; //"Nummer"
			this.constructionDataGridViewTextBoxColumn.HeaderText = EuroplanRes.HithermWallGrid_Basiskonstruktion; //"Basis-\nKonstr."
			this.nameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.General_BezeichnungCol; //"Bezeichnung"
			this.Deckschicht.HeaderText = EuroplanRes.HithermWallGrid_Deckschicht; //"Decksch.\nR\n(m²K/W)"
			this.kValueDataGridViewTextBoxColumn.HeaderText = EuroplanRes.HithermWallGrid_UWert; //"U-Wert\n(W/m²K)"
			this.bereinigenDataGridViewCheckBoxColumn.HeaderText = EuroplanRes.HithermWallGrid_WaermebedarfBereinigen; //"Wärme\nBedarf\nberein."
			this.additionalInsulationDataGridViewTextBoxColumn.HeaderText = EuroplanRes.HithermWallGrid_ZusaetzlicheDaemmung; //"zus. Dämmg\nR\n(m²K/W)"
			this.tempBehindHeatDataGridViewTextBoxColumn.HeaderText = EuroplanRes.HithermWallGrid_TemperaturHeiz; //"Temp.\nHeiz\n(°C)"
			this.tempBehindCoolDataGridViewTextBoxColumn.HeaderText = EuroplanRes.HithermWallGrid_TemperaturKuehl; //"Temp.\nKühl\n(°C)"
		}

		public class WallEventArgs : EventArgs {
			public HithermWall wall;
			public WallEventArgs(HithermWall wall) {
				this.wall = wall;
			}
		}
		public event EventHandler<WallEventArgs> WallChanged;
		public event EventHandler<WallEventArgs> WallAdded;
		public event EventHandler<WallEventArgs> WallRemoved;

		public bool ShowCompact {
			get { return this.showCompact; }
			set {
				if (value != showCompact) {
					this.showCompact = value;
					if (Project.Instance != null) {
						this.hithermWallBindingSource.DataSource = (showCompact ? Project.Instance.HithermCompactWalls : Project.Instance.HithermWalls);
						this.hithermWallBindingSource.ResetBindings(false);
					}
				}
			}
		}

		public void UpdateGrid() {
			if (Project.Instance != null) {
				this.hithermWallBindingSource.DataSource = (showCompact ? Project.Instance.HithermCompactWalls : Project.Instance.HithermWalls);
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
				btnSelectConstruction.Location = new Point(rect.X + rect.Width - btnSelectConstruction.Width - 1, rect.Y);
				btnSelectConstruction.Height = rect.Height - 1;
				btnSelectConstruction.Show();
			}
		}

		private void dgvWalls_CellLeave(object sender, DataGridViewCellEventArgs e) {
			btnSelectConstruction.Hide();
		}

		private void btnSelectConstruction_Click(object sender, EventArgs e) {
			SelectHithermWallConstructionForm form = new SelectHithermWallConstructionForm(this.showCompact);
			DataGridViewCell cell = dgvWalls.Rows[dgvWalls.CurrentCell.RowIndex].Cells[this.Construction.Index];
			form.SelectedConstruction = cell.Value as WallConstruction;
			if (form.ShowDialog().Equals(DialogResult.OK)) {
				WallConstruction construction = form.SelectedConstruction;
				if (cell.Value != construction) {
					cell.Value = construction;
					int col = dgvWalls.SelectedCells.Count > 0 ? dgvWalls.SelectedCells[0].ColumnIndex : -1;
					int row = dgvWalls.SelectedCells.Count > 0 ? dgvWalls.SelectedCells[0].RowIndex : -1;
					hithermWallBindingSource.ResetBindings(false);
					if (col > -1) {
						dgvWalls.Rows[row].Cells[col].Selected = true;
					}
				}
			}
			form.Dispose();
		}

		private void dgvWalls_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) {
			e.Row.Cells[this.additionalInsulationDataGridViewTextBoxColumn.Index].Value = 0.0;
			ConstructionListWrapper clw = new ConstructionListWrapper(Configuration.ConfigurationType.UserConfiguration);
			clw.ConstructionScopeFilter = ConstructionScopeEnum.WallConstruction;
			WallConstruction newConstruction = null;
			foreach (WallConstruction wc in clw) {
				if (!this.showCompact && wc.IsHithermWall) {
					newConstruction = wc;
					break;
				} else if (this.showCompact && wc.IsHithermCompactWall) {
					newConstruction = wc;
					break;
				}
			}
			e.Row.Cells[this.Construction.Index].Value = newConstruction;
			e.Row.Cells[this.Deckschicht.Index].Value = 0.0;
			e.Row.Cells[this.kValueDataGridViewTextBoxColumn.Index].Value = 0.0;
			e.Row.Cells[this.nameDataGridViewTextBoxColumn.Index].Value = EuroplanRes.HithermWallGrid_NeueWandkonstruktion; //"Neue Wandkonstruktion"
			int maxId = 0;
			int newId;
			string prefix = this.showCompact ? "UCW" : "USW";
			if (Project.Instance != null) {
				foreach (HithermWall hw in Project.Instance.HithermWalls) {
					if (hw.Id != null && hw.Id.StartsWith(prefix)) {
						if (Int32.TryParse(hw.Id.Substring(3), out newId) && newId > maxId) {
							maxId = newId;
						}
					}
				}
				foreach (HithermWall hw in Project.Instance.HithermCompactWalls) {
					if (hw.Id != null && hw.Id.StartsWith(prefix)) {
						if (Int32.TryParse(hw.Id.Substring(3), out newId) && newId > maxId) {
							maxId = newId;
						}
					}
				}
			}
			maxId++;
			e.Row.Cells[this.idDataGridViewTextBoxColumn.Index].Value = prefix + maxId.ToString("00");
			e.Row.Cells[this.tempBehindCoolDataGridViewTextBoxColumn.Index].Value = 30.0;
			e.Row.Cells[this.tempBehindHeatDataGridViewTextBoxColumn.Index].Value = -16.0;
		}

		private void dgvWalls_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (Project.Instance != null) {
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							if (!this.showCompact && pp.Product is HithermProduct) {
								HithermProduct hp = pp.Product as HithermProduct;
								hp.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
							} else if (this.showCompact && pp.Product is HithermCompactProduct) {
								HithermCompactProduct hp = pp.Product as HithermCompactProduct;
								hp.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
							}
						}
					}
				}
			}
			if (this.WallChanged != null) {
				this.WallChanged(this, new WallEventArgs(this.dgvWalls.Rows[e.RowIndex].DataBoundItem as HithermWall));
			}
		}

		private void dgvWalls_UserAddedRow(object sender, DataGridViewRowEventArgs e) {
			if (this.WallAdded != null) {
				this.WallAdded(this, new WallEventArgs(e.Row.DataBoundItem as HithermWall));
			}
		}

		private HithermWall deletingWall = null;

		private void dgvWalls_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			if (Project.Instance != null) {
				HithermWall newWall = null;
				List<HithermWall> allWalls = this.showCompact ? Project.Instance.HithermCompactWalls : Project.Instance.HithermWalls;
				if (allWalls.Count > 0) {
					newWall = allWalls[0];
				}
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							if (!showCompact && pp.Product is HithermProduct) {
								HithermProduct hp = pp.Product as HithermProduct;
								foreach (HithermCircuit hc in hp.PlannedCircuits) {
									foreach (HithermRegister hr in hc.Registers) {
										if (hr.Wall == deletingWall) {
											hr.Wall = newWall;
										}
									}
								}
								hp.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
							} else if (showCompact && pp.Product is HithermCompactProduct) {
								HithermCompactProduct hcp = pp.Product as HithermCompactProduct;
								foreach (HithermCompactCircuit hcc in hcp.PlannedCircuits) {
									foreach (HithermCompactRegister hcr in hcc.Registers) {
										if (hcr.Wall == deletingWall) {
											hcr.Wall = newWall;
										}
									}
								}
								hcp.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
							}
						}
					}
				}
			}
			deletingWall = null;
			if (this.WallRemoved != null) {
				this.WallRemoved(this, new WallEventArgs(e.Row.DataBoundItem as HithermWall));
			}
		}

		private void dgvWalls_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			deletingWall = e.Row.DataBoundItem as HithermWall;
		}
	}
}
