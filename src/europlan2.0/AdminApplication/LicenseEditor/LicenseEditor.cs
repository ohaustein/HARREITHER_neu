using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;
using System.ComponentModel;

namespace Europlan.AdminApplication {
	public partial class LicenseEditor : UserControl {

		private LicenseTemplate license = null;

		public LicenseEditor() {
			InitializeComponent();
			this.UpdateGui(true);
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LicenseTemplate License {
			get { return this.license; }
			set {
				this.license = value;
				this.UpdateGui(false);
			}
		}

		private void UpdateGui(bool initial) {
			this.Enabled = this.license != null;
			if (this.license == null) {
				txtLicensedTo.Text = "";
				txtEmail.Text = "";
				dtpValidUntil.Value = DateTime.Today;
				txtHeader.Text = "";
				lstModules.Items.Clear();
				if (!initial) {
					licenseTemplateBindingSource.DataSource = null;
					licenseTemplateBindingSource.ResetBindings(false);
				}
			} else {
				txtLicensedTo.Text = this.license.LicensedTo;
				txtEmail.Text = this.license.Email;
				dtpValidUntil.Value = this.license.ValidUntil;
				txtHeader.Text = this.license.Header;
				this.UpdateModulesEnablement();
				this.RefreshSystemList();
				licenseTemplateBindingSource.DataSource = this.license.Systems;
				licenseTemplateBindingSource.ResetBindings(false);
			}
			foreach (DataGridViewRow row in this.dataGridView1.Rows) {
				string idString = row.Cells[this.idDataGridViewTextBoxColumn.Index].Value as string;
				HardwareId hwId = (idString == null) ? null : new HardwareId(idString);
				if (hwId == null || hwId.IsValid) {
					row.Cells[this.idDataGridViewTextBoxColumn.Index].ErrorText = null;
				} else {
					row.Cells[this.idDataGridViewTextBoxColumn.Index].ErrorText = "Keine gültige Hardware ID";
				}
			}
			
		}

		private void UpdateModulesEnablement() {
			lstModules.Items.Clear();
			if (this.license != null) {
				foreach (LicensedModuleTemplate module in license.Modules) {
					lstModules.Items.Add(new ModuleItem(module));
				}
			}
		}

		private void txtLicensedTo_TextChanged(object sender, EventArgs e) {
			if (this.license != null) {
				this.license.LicensedTo = txtLicensedTo.Text;
			}
		}

		private void txtEmail_TextChanged(object sender, EventArgs e) {
			if (this.license != null) {
				this.license.Email = txtEmail.Text;
			}
		}

		private void dtpValidUntil_ValueChanged(object sender, EventArgs e) {
			if (this.license != null) {
				this.license.ValidUntil = dtpValidUntil.Value;
			}
		}

		private void txtHeader_TextChanged(object sender, EventArgs e) {
			if (this.license != null) {
				this.license.Header = txtHeader.Text;
			}
		}

		private void lstModules_ItemChecked(object sender, ItemCheckedEventArgs e) {
			if (this.license != null) {
				ModuleItem item = e.Item as ModuleItem;
				if (item != null) {
					item.Module.Enabled = item.Checked;
				}
			}
		}

		private void RefreshSystemList() {
		}

		private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
			if (dataGridView1.Columns[e.ColumnIndex] == this.idDataGridViewTextBoxColumn) {
				if (new HardwareId((string)e.FormattedValue).IsValid) {
					dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = null;
				} else {
					dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Keine gültige Hardware ID";
				}
			}
		}

	}
}
