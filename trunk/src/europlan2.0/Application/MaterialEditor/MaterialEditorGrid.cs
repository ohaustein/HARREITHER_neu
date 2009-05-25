using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.Application.ContructionEditor {
	public partial class MaterialEditorGrid : UserControl {

		private Nullable<CategoryType> filter;
		private MaterialListWrapper wrapper;

		public MaterialEditorGrid() {
			InitializeComponent();
			this.wrapper = new MaterialListWrapper(Configuration.ConfigurationType.UserConfiguration);
			this.materialsWrapperBindingSource.DataSource = this.wrapper;
			this.materialsWrapperBindingSource.ResetBindings(false);
		}

		public Nullable<CategoryType> Filter {
			get { return this.filter; }
			set {
				this.filter = value;
				this.materialsWrapperBindingSource.Filter = (filter == null ? null : ((int)filter).ToString());
				this.materialsWrapperBindingSource.ResetBindings(false);
			}
		}

		public bool AllowToAdd {
			get { return this.gridMaterials.AllowUserToAddRows; }
			set { this.gridMaterials.AllowUserToAddRows = value; }
		}

		private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			Console.WriteLine("added");
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.gridMaterials.Rows[i];
				if (row.DataBoundItem != null) {
					row.ReadOnly = !(row.DataBoundItem as Material).UserDefined;
					if (row.ReadOnly) {
						row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
					} else {
						row.DefaultCellStyle.ForeColor = SystemColors.ControlText;
					}
				}
			}
		}

		private void gridMaterials_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			e.Cancel = true;
			if (e.Row.DataBoundItem is Material && (e.Row.DataBoundItem as Material).UserDefined) {
				this.wrapper.Remove(e.Row.DataBoundItem);
				this.materialsWrapperBindingSource.ResetBindings(false);
			}
		}

		public void Cleanup() {
			if (this.gridMaterials.SelectedCells.Count > 0) {
				if (this.gridMaterials.SelectedCells[0].OwningRow.DataBoundItem == null) {
					this.gridMaterials.CancelEdit();
				} else {
					this.gridMaterials.EndEdit();
				}
			}
		}
	}
}
