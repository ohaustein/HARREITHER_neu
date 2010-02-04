using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {

	public delegate void RequiredMaterialGridContentChangedHandler(object sender);

	public partial class RequiredMaterialGrid : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;
		public event RequiredMaterialGridContentChangedHandler GridContentChanged;

		private CategoryType type;
		private DataGridViewGrouper grouper = null;

		public RequiredMaterialGrid() {
			InitializeComponent();
			UpdateControl(true);
		}

		public void UpdateControl(bool resetUserInterface) {
			List<RequiredMaterialWrapper> wrapperList = new List<RequiredMaterialWrapper>();
			RequiredMaterialWrapper wrapper = null;
			if (Project.Instance != null) {
				foreach (Category category in Project.Instance.Config.Categories) {
					if (category.Type == this.type) {
						foreach (Material material in category.Materials) {
							wrapper = new RequiredMaterialWrapper(material);
							wrapperList.Add(wrapper);
						}
					}
				}
			}
			wrapperList.Sort(delegate (RequiredMaterialWrapper w1, RequiredMaterialWrapper w2) {
				if (w1.Category.Order.CompareTo(w2.Category.Order) == 0) {
					if (w1.PartNumber.CompareTo(w2.PartNumber) == 0) {
						return w1.Name.CompareTo(w2.Name);
					} else {
						return w1.PartNumber.CompareTo(w2.PartNumber);
					}
				} else {
					return w1.Category.Order.CompareTo(w2.Category.Order);
				}
			});
			requiredMaterialWrapperBindingSource.DataSource = wrapperList;
			requiredMaterialWrapperBindingSource.ResetBindings(false);
			if (grouper == null) {
				grouper = new DataGridViewGrouper(dgvRequiredMaterial);
				grouper.SetGroupOn("Category");
				grouper.ShowCount = false;
				grouper.SortOrder = SortOrder.None;
			}
			if (wrapperList.Count > 0) {
				dgvRequiredMaterial.CurrentCell = dgvRequiredMaterial[1, 1];
			}
		}

		public bool AllowLeave() {
			return true;
		}
		
		public CategoryType CategoryType {
			get { return type; }
			set { type = value;	}
		}

		private void dgvRequiredMaterial_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (this.GridContentChanged != null) {
				this.GridContentChanged(this);
			}
		}

		private void dgvRequiredMaterial_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Delete && this.dgvRequiredMaterial.SelectedCells.Count == 1 &&
				this.dgvRequiredMaterial.SelectedRows.Count == 0/* && this.gridRooms.SelectedCells[0].Value != null */) {
				DataGridViewCell cell = this.dgvRequiredMaterial.SelectedCells[0];
				if (!cell.ReadOnly) {
					e.IsInputKey = false;
					this.dgvRequiredMaterial.BeginEdit(true);
					if (!(dgvRequiredMaterial.Rows[cell.RowIndex].DataBoundItem is GroupingSource.GroupRow)) {
						cell.Value = null;
					}
					this.dgvRequiredMaterial.EndEdit();
				}
			}
		}

		private void dgvRequiredMaterial_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
			if ((e.RowIndex > 0) && 
				((e.ColumnIndex == nameDataGridViewTextBoxColumn.Index) || 
				(e.ColumnIndex == calculatedAmountDataGridViewTextBoxColumn.Index) ||
				e.ColumnIndex == unitDataGridViewTextBoxColumn.Index ||
				e.ColumnIndex == partNumberDataGridViewTextBoxColumn.Index)) {
				DataGridViewRow row = dgvRequiredMaterial.Rows[e.RowIndex];
				if ((row.Cells[CanBeCalculated.Index].Value != null) && ((bool)row.Cells[CanBeCalculated.Index].Value == false)) {
					e.CellStyle.ForeColor = Color.Red;
				}
			}
		}

	}

}
