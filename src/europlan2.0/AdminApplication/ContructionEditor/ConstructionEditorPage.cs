using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.AdminApplication {
	public partial class ConstructionEditorPage : UserControl {

		private ConstructionScopeEnum filter = ConstructionScopeEnum.All;
		private ConstructionListWrapper wrapper;

		public ConstructionEditorPage() {
			InitializeComponent();
			this.wrapper = new ConstructionListWrapper();
			this.constructionsWrapperBindingSource.DataSource = this.wrapper;
			this.constructionsWrapperBindingSource.ResetBindings(false);
		}

		private void constructionsGrid_CellClick(object sender, DataGridViewCellEventArgs e) {
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
	}
}
