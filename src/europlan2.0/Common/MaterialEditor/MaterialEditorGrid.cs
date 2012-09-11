using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;
using System.Threading;

namespace Europlan.Common {
	public partial class MaterialEditorGrid : UserControl {

		public class MaterialCategoryGridItem {
			private Category itemCategory;

			public MaterialCategoryGridItem(Category itemCategory) {
				this.itemCategory = itemCategory;
			}

			public Category ItemCategory {
				get { return this.itemCategory; }
			}

			public string ItemName {
				get {
					if (this.itemCategory == null) {
						return "";
					}
					CategoryTypeEnumConverter tc = new CategoryTypeEnumConverter();
					return tc.ConvertToString(this.itemCategory.Type) + " - " + this.itemCategory.LocalizedName;
				}
			}
		}

		private MaterialListWrapper wrapper;
		private bool admin = false;

		public MaterialEditorGrid() {
			InitializeComponent();
			this.SetLanguage();
			this.wrapper = new MaterialListWrapper(Configuration.ConfigurationType.UserConfiguration);
			this.materialsWrapperBindingSource.DataSource = this.wrapper;
			this.materialsWrapperBindingSource.ResetBindings(false);
			List<MaterialCategoryGridItem> categories = new List<MaterialCategoryGridItem>();
			categories.Add(new MaterialCategoryGridItem(null));
			foreach (Category category in Configuration.AdminTemplate.Categories) {
				categories.Add(new MaterialCategoryGridItem(category));
			}
			materialCategoryGridItemBindingSource.DataSource = categories;
		}

		private void SetLanguage() {
			this.idDataGridViewTextBoxColumn.HeaderText = EuroplanRes.General_Nummer; // "Nr.";
			this.partNumberDataGridViewTextBoxColumn.HeaderText = EuroplanRes.MaterialEditorGrid_BestellnummerCol; //"Bestellnr.";
			this.nameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.General_BezeichnungCol; //"Bezeichnung";
			this.unitDataGridViewTextBoxColumn.HeaderText = EuroplanRes.MaterialEditorGrid_EinheitCol; //"Einheit";
			this.denominationDataGridViewTextBoxColumn.HeaderText = EuroplanRes.MaterialEditorGrid_VerpackungseinheitCol; //"Verpackungs-\neinheit";
			this.priceDataGridViewTextBoxColumn.HeaderText = EuroplanRes.MaterialEditorGrid_PreisCol; //"Preis pro Einheit";
			this.Category.HeaderText = EuroplanRes.MaterialEditorGrid_KategorieCol; //"Kategorie";
		}

		public bool Admin {
			get { return this.admin; }
			set {
				if (this.admin != value) {
					this.admin = value;
					this.Category.Visible = this.admin;
					MaterialListWrapper oldWrapper = this.wrapper;
					this.wrapper = new MaterialListWrapper(admin ? Configuration.ConfigurationType.AdminConfiguration : Configuration.ConfigurationType.UserConfiguration);
					this.wrapper.Admin = this.admin;
					this.materialsWrapperBindingSource.DataSource = this.wrapper;
					this.materialsWrapperBindingSource.ResetBindings(false);
					this.wrapper.FilterCategory = oldWrapper.FilterCategory;
					this.wrapper.ShowOnlyAdditional = oldWrapper.ShowOnlyAdditional;
				}
			}
		}

		public bool ShowOnlyAdditional {
			get { return this.wrapper.ShowOnlyAdditional; }
			set { this.wrapper.ShowOnlyAdditional = value; }
		}

		public Nullable<CategoryType> Filter {
			get { return this.wrapper.FilterCategory; }
			set { this.wrapper.FilterCategory = value; }
			/*get { return this.filter; }
			set {
				this.filter = value;
				this.materialsWrapperBindingSource.Filter = (filter == null ? null : ((int)filter).ToString());
				this.materialsWrapperBindingSource.ResetBindings(false);
			}*/
		}

		public bool AllowToAdd {
			get { return this.gridMaterials.AllowUserToAddRows; }
			set { this.gridMaterials.AllowUserToAddRows = value; }
		}

		private void gridMaterials_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.gridMaterials.Rows[i];
				if (row.DataBoundItem != null) {
					row.ReadOnly = !(row.DataBoundItem as Material).UserDefined && !this.admin;
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

		private void gridMaterials_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex == this.Category.Index && e.RowIndex >= 0) {
				Material material = this.gridMaterials.Rows[e.RowIndex].DataBoundItem as Material;

				foreach (Category category in Configuration.AdminTemplate.Categories) {
					if (category.Materials.Contains(material)) {
						category.Materials.Remove(material);
					}
				}
				if (material.Category != null) {
					material.Category.Materials.Add(material);
				}
			}
		}
	}
}
