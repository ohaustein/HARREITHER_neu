using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;
using System.Collections;

namespace Europlan.AdminApplication {
	public partial class MaterialMapper : UserControl {

		public MaterialMapper() {
			InitializeComponent();
			InitializeUncategorizedMaterialListView();
			cmbRootCategories.SelectedIndex = 0;
		}

		private void InitializeUncategorizedMaterialListView() {
			listUncategorizedMaterials.Items.Clear();
			Configuration config = Configuration.AdminTemplate;
			foreach (Material material in config.Materials) {
				if (material.Category == null) {
					//string[] mat = new string[] { material.Name, material.PartNumber, material.Denomination.Value.ToString(), material.Unit, material.Price.ToString("0.00") };
					string[] mat = new string[] { material.Name, material.PartNumber };
					ListViewItem item = new ListViewItem(mat);
					item.Tag = material;
					listUncategorizedMaterials.Items.Add(item);
				}
			}
			listUncategorizedMaterials.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
			if (splitContainer.SplitterDistance < 220) {
				splitContainer.SplitterDistance = 220;
			}
		}

		private void cmbRootCategories_SelectedIndexChanged(object sender, EventArgs e) {
			UpdateCategoryList();
		}

		private void UpdateCategoryList() {
			List<Category> list = new List<Category>();
			foreach (Category category in Configuration.AdminTemplate.Categories) {
				if (category.Type == (CategoryType)cmbRootCategories.SelectedIndex) {
					list.Add(category);
				}
			}
			list.Sort();
			listCategories.Items.Clear();
			foreach (Category category in list) {
				listCategories.Items.Add(category);
			}
			if (listCategories.Items.Count != 0) {
				listCategories.SelectedIndex = 0;
			} else {
				txtCategoryName.Text = "";
				txtCategoryName.Enabled = false;
				btnDown.Enabled = false;
				btnUp.Enabled = false;
				btnRemove.Enabled = false;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			Category category = new Category();
			category.Name = "Kategorie";
			category.Order = listCategories.Items.Count + 1;
			category.Type = (CategoryType)cmbRootCategories.SelectedIndex;
			Configuration.AdminTemplate.Categories.Add(category);
			UpdateCategoryList();
		}

		private void listCategories_SelectedIndexChanged(object sender, EventArgs e) {
			Category selectedCategory = (listCategories.SelectedItem as Category);

			if (selectedCategory != null) {
				txtCategoryName.Text = selectedCategory.Name;
				txtCategoryName.Enabled = true;
				btnRemove.Enabled = true;
				btnUp.Enabled = selectedCategory.Order > 1;
				btnDown.Enabled = selectedCategory.Order < listCategories.Items.Count;
			}
		}

		private void btnRemove_Click(object sender, EventArgs e) {
			Category selectedCategory = (listCategories.SelectedItem as Category);
			Configuration.AdminTemplate.Categories.Remove(selectedCategory);
			UpdateCategoryList();
		}

		private void txtCategoryName_TextChanged(object sender, EventArgs e) {
			Category selectedCategory = (listCategories.SelectedItem as Category);
			if (selectedCategory != null) {
				selectedCategory.Name = txtCategoryName.Text;
				listCategories.SelectedItem = selectedCategory;
				listCategories.Items[listCategories.SelectedIndex] = selectedCategory;
			}
		}

		private void btnUp_Click(object sender, EventArgs e) {
			Category cat1 = listCategories.Items[listCategories.SelectedIndex - 1] as Category;
			Category cat2 = listCategories.Items[listCategories.SelectedIndex] as Category;
			cat1.Order++;
			cat2.Order--;
			listCategories.Items[listCategories.SelectedIndex - 1] = cat2;
			listCategories.Items[listCategories.SelectedIndex] = cat1;
			listCategories.SelectedIndex--;
		}

		private void btnDown_Click(object sender, EventArgs e) {
			Category cat1 = listCategories.Items[listCategories.SelectedIndex] as Category;
			Category cat2 = listCategories.Items[listCategories.SelectedIndex + 1] as Category;
			cat1.Order++;
			cat2.Order--;
			listCategories.Items[listCategories.SelectedIndex + 1] = cat1;
			listCategories.Items[listCategories.SelectedIndex] = cat2;
			listCategories.SelectedIndex++;
		}
	}
}
