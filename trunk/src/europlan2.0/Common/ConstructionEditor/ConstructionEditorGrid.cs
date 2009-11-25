using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.Common {
	public partial class ConstructionEditorGrid : UserControl {

		private ConstructionScopeEnum filter = ConstructionScopeEnum.All;
		private ConstructionListWrapper wrapper;
		private bool adminMode = false;

		public ConstructionEditorGrid() {
			InitializeComponent();
			this.wrapper = new ConstructionListWrapper(Configuration.ConfigurationType.UserConfiguration); // TODO
			this.constructionsWrapperBindingSource.DataSource = this.wrapper;
			this.constructionsWrapperBindingSource.ResetBindings(false);
		}

		public Configuration.ConfigurationType Type {
			get { return this.adminMode ? Configuration.ConfigurationType.AdminConfiguration : Configuration.ConfigurationType.UserConfiguration; }
			set {
				if (!(value == Configuration.ConfigurationType.AdminConfiguration || value == Configuration.ConfigurationType.UserConfiguration)) {
					throw new Exception("Type must either be AdminConfiguration or UserConfiguration");
				}
				this.adminMode = value == Configuration.ConfigurationType.AdminConfiguration;
				this.wrapper = new ConstructionListWrapper(this.adminMode ? Configuration.ConfigurationType.AdminConfiguration : Configuration.ConfigurationType.UserConfiguration);
				this.constructionsWrapperBindingSource.DataSource = this.wrapper;
				this.constructionsWrapperBindingSource.ResetBindings(false);
			}
		}

		private void gridConstructions_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridConstructions.Columns.Count &&
					this.gridConstructions.Columns[e.ColumnIndex] == this.colEdit &&
					e.RowIndex >= 0 && e.RowIndex < this.gridConstructions.Rows.Count) {
				Construction c = this.gridConstructions.Rows[e.RowIndex].DataBoundItem as Construction;
				if (c != null) {
					ConstructionEditorForm cef = new ConstructionEditorForm(c);
					if (!this.adminMode) {
						if (c.Type != null && !c.Type.UserDefined) {
							cef.ReadOnly = true;
						}
					}
					cef.ShowDialog();
					this.constructionsWrapperBindingSource.ResetBindings(false);
				}
			}
		}

		public ConstructionScopeEnum Filter {
			get { return this.filter; }
			set {
				this.filter = value;
				if (value == ConstructionScopeEnum.All) {
					this.constructionsWrapperBindingSource.Filter = null;
				} else {
					this.constructionsWrapperBindingSource.Filter = ((int)this.filter).ToString();
				}
				this.constructionsWrapperBindingSource.ResetBindings(false);
			}
		}

		public void AddConstruction(Construction c) {
			this.wrapper.Add(c);
			this.constructionsWrapperBindingSource.ResetBindings(false);
		}

		private void btnNew_Click(object sender, EventArgs e) {
			//if (this.adminMode) {
				//this.cmsNewAdmin.Show(this.btnNew, new Point(0, this.btnNew.Height));
			//} else {
				this.cmsNew.Show(this.btnNew, new Point(0, this.btnNew.Height));
			//}
		}

		private void btnView_Click(object sender, EventArgs e) {
			this.cmsView.Show(this.btnView, new Point(0, this.btnView.Height));
		}

		private void cmsViewItem_Click(object sender, EventArgs e) {
			int i = 0;
			string selected = "";
			if (this.tsmiFloorConstruction.Checked) {
				selected += ", FB";
				i++;
			}
			if (this.tsmiInsulationConstruction.Checked) {
				selected += ", WD";
				i++;
			}
			if (this.tsmiCeilingConstruction.Checked) {
				selected += ", DK";
				i++;
			}
			if (i == 0) {
				selected = "keine";
				this.Filter = ConstructionScopeEnum.UnknownConstruction;
			} else if (i == 3) {
				selected = "alle";
				this.Filter = ConstructionScopeEnum.All;
			} else {
				selected = selected.Substring(2);
				ConstructionScopeEnum filter = ConstructionScopeEnum.UnknownConstruction;
				if (this.tsmiFloorConstruction.Checked) {
					filter = filter | ConstructionScopeEnum.FloorConstruction;
				}
				if (this.tsmiInsulationConstruction.Checked) {
					filter = filter | ConstructionScopeEnum.InsulationConstruction;
				}
				if (this.tsmiCeilingConstruction.Checked) {
					filter = filter | ConstructionScopeEnum.CeilingConstruction;
				}
				this.Filter = filter;
			}
			this.btnView.Text = "Angezeigte Konstruktionen (" + selected + ")";
		}

		private void tsmiNewConstruction_Click(object sender, EventArgs e) {
			Construction c = null;
			if (sender == this.tsmiNewFloorConstructionEstrich) {
				c = new FloorConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(this.adminMode ? ConstructionTypeManager.CT_STD_ESTRICH : ConstructionTypeManager.CT_USER_ESTRICH);
			} else if (sender == this.tsmiNewInsulationConstruction) {
				c = new InsulationConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(this.adminMode ? ConstructionTypeManager.CT_STD_DAEMM : ConstructionTypeManager.CT_USER_DAEMM);
			} else if (sender == this.tsmiNewCeilingConstruction) {
				c = new CeilingConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(this.adminMode ? ConstructionTypeManager.CT_STD_DECKE : ConstructionTypeManager.CT_USER_DECKE);
			} else if (sender == this.tsmiNewFloorConstructionTrocken) {
				c = new FloorConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(this.adminMode ? ConstructionTypeManager.CT_STD_TROCKEN : ConstructionTypeManager.CT_USER_TROCKEN);
			} else if (sender == this.tsmiNewFloorConstructionStahl) {
				c = new FloorConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(this.adminMode ? ConstructionTypeManager.CT_STD_STAHL : ConstructionTypeManager.CT_USER_STAHL);
			} else if (sender == this.tsmiNewFloorConstructionTrockenEstrich) {
				c = new FloorConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(this.adminMode ? ConstructionTypeManager.CT_STD_TRK_ESTRICH : ConstructionTypeManager.CT_USER_TRK_ESTRICH);
			}
			if (c != null) {
				ConstructionEditorForm form = new ConstructionEditorForm(c);
				form.ShowDialog();
				this.AddConstruction(c);
			}
		}

		private void tsmiNewConstructionAdmin_Click(object sender, EventArgs e) {
			Construction c = null;
			if (sender == this.tsmiNewFloorConstructionScreedAdmin) {
				c = new FloorConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH);
			} else if (sender == this.tsmiNewFloorConstructionDryAdmin) {
				c = new FloorConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TROCKEN);
			} else if (sender == this.tsmiNewInsulationConstructionAdmin) {
				c = new InsulationConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_DAEMM);
			} else if (sender == this.tsmiNewCeilingConstructionAdmin) {
				c = new CeilingConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_DECKE);
			}
			if (c != null) {
				ConstructionEditorForm form = new ConstructionEditorForm(c);
				form.ShowDialog();
				this.AddConstruction(c);
			}
		}

		private void gridConstructions_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			e.Cancel = true;
			bool refresh = false;
			if (e.Row.DataBoundItem is Construction) {
				Construction c = e.Row.DataBoundItem as Construction;
				if (c.Type == null || c.Type.UserDefined) {
					this.wrapper.Remove(c);
					refresh = true;
				} else {
					if (this.adminMode) {
						if (c.VersionCount == 0) {
							this.wrapper.Remove(c);
						} else {
							c.HasBeenDeleted = true;
						}
						refresh = true;
					}
				}
			} else {
				this.wrapper.Remove(e.Row.DataBoundItem);
				refresh = true;
			}
			if (refresh) {
				this.constructionsWrapperBindingSource.ResetBindings(false);
			}
		}

		private void gridConstructions_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.gridConstructions.Rows[i];
				if (row.DataBoundItem != null) {
					row.ReadOnly = !this.adminMode && (row.DataBoundItem as Construction).Type != null && !(row.DataBoundItem as Construction).Type.UserDefined;
					if (row.ReadOnly) {
						row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
					} else {
						row.DefaultCellStyle.ForeColor = SystemColors.ControlText;
					}
				}
			}
		}

		/*private void gridConstructions_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
			e.PaintCells(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.Focus | DataGridViewPaintParts.SelectionBackground);
			e.PaintCells(e.ClipBounds, DataGridViewPaintParts.All);
			e.PaintHeader(DataGridViewPaintParts.All);
		}

		private void gridConstructions_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
			if (e.PaintParts == DataGridViewPaintParts.All && e.ColumnIndex == this.colEdit.Index) {
				if (e.RowIndex >= 0 && e.RowIndex < this.gridConstructions.Rows.Count &&
					(this.gridConstructions.Rows[e.RowIndex].DataBoundItem == null || !(this.gridConstructions.Rows[e.RowIndex].DataBoundItem as Construction).Type.UserDefined)) {
					e.Handled = true;
				}
			}
		}*/
	}
}
