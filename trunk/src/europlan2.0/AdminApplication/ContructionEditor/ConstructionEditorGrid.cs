using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.AdminApplication {
	public partial class ConstructionEditorGrid : UserControl {

		private ConstructionScopeEnum filter = ConstructionScopeEnum.All;
		private ConstructionListWrapper wrapper;

		public ConstructionEditorGrid() {
			InitializeComponent();
			this.wrapper = new ConstructionListWrapper();
			this.constructionsWrapperBindingSource.DataSource = this.wrapper;
			this.constructionsWrapperBindingSource.ResetBindings(false);
		}

		private void gridConstructions_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridConstructions.Columns.Count &&
					this.gridConstructions.Columns[e.ColumnIndex] == this.colEdit &&
					e.RowIndex >= 0 && e.RowIndex < this.gridConstructions.Rows.Count) {
				Construction c = this.gridConstructions.Rows[e.RowIndex].DataBoundItem as Construction;
				if (c != null) {
					ConstructionEditorForm cef = new ConstructionEditorForm(c);
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
			this.cmsNew.Show(this.btnNew, new Point(0, this.btnNew.Height));
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
			if (i == 0) {
				selected = "keine";
				this.Filter = ConstructionScopeEnum.UnknownConstruction;
			} else if (i == 2) {
				selected = "alle";
				this.Filter = ConstructionScopeEnum.All;
			} else {
				selected = selected.Substring(2);
				if (this.tsmiFloorConstruction.Checked) {
					this.Filter = ConstructionScopeEnum.FloorConstruction;
				} else {
					this.Filter = ConstructionScopeEnum.InsulationConstruction;
				}
			}
			this.btnView.Text = "Angezeigte Konstruktionen (" + selected + ")";
		}

		private void tsmiNewConstruction_Click(object sender, EventArgs e) {
			Construction c = null;
			if (sender == this.tsmiNewFloorConstruction) {
				c = new FloorConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH);
			} else if (sender == this.tsmiNewInsulationConstruction) {
				c = new InsulationConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_DAEMM);
			} else if (sender == this.tsmiNewWallConstruction) {
				// TODO
			}
			if (c != null) {
				ConstructionEditorForm form = new ConstructionEditorForm(c);
				form.ShowDialog();
				this.AddConstruction(c);
			}
		}

		private void gridConstructions_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			Console.WriteLine("deleting");
			e.Cancel = true;
			this.wrapper.Remove(e.Row.DataBoundItem);
			this.constructionsWrapperBindingSource.ResetBindings(false);
		}

		private void gridConstructions_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			Console.WriteLine("deleted");
		}
	}
}
