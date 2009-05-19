using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.AdminApplication.ContructionEditor {
	public partial class MaterialEditorGrid : UserControl {

		private Nullable<CategoryType> filter;
		private MaterialListWrapper wrapper;

		public MaterialEditorGrid() {
			InitializeComponent();
			this.wrapper = new MaterialListWrapper();
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
			get { return this.dataGridView1.AllowUserToAddRows; }
			set { this.dataGridView1.AllowUserToAddRows = value; }
		}

		private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			Console.WriteLine("added");
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.dataGridView1.Rows[i];
				if (row.DataBoundItem != null) {
					row.ReadOnly = !(row.DataBoundItem as Material).UserDefined;
					row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
				}
			}
		}
	}
}
