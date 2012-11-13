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

		private Category selectedCategory = null;
		ListViewSorter uncategorizedSorter = new ListViewSorter();
		ListViewSorter categorizedSorter = new ListViewSorter();

		public MaterialMapper() {
			InitializeComponent();
			InitializeUncategorizedMaterialListView();
			InitializeCategorizedMaterialListView();
			cmbRootCategories.SelectedIndex = 0;
		}

		private void InitializeUncategorizedMaterialListView() {
			listUncategorizedMaterials.Items.Clear();
			Configuration config = Configuration.AdminTemplate;
			foreach (Material material in config.Materials) {
				if (material.Category == null && !material.Additional) {
					string[] mat = new string[] { material.PartNumber, material.Name };
					ListViewItem item = new ListViewItem(mat);
					item.Tag = material;
					listUncategorizedMaterials.Items.Add(item);
				}
			}
			listUncategorizedMaterials.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			listUncategorizedMaterials.ListViewItemSorter = uncategorizedSorter;
			if (!(listUncategorizedMaterials.ListViewItemSorter is ListViewSorter))
				return;
			uncategorizedSorter = (ListViewSorter)listUncategorizedMaterials.ListViewItemSorter;
		}

		private void InitializeCategorizedMaterialListView() {
			listCategorizedMaterials.Items.Clear();
			if (selectedCategory != null) {
				foreach (Material material in selectedCategory.Materials) {
					if (!material.Additional) {
						string[] mat = new string[] { material.PartNumber, material.Name };
						ListViewItem item = new ListViewItem(mat);
						item.Tag = material;
						listCategorizedMaterials.Items.Add(item);
					}
				}
			}
			listCategorizedMaterials.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			listCategorizedMaterials.ListViewItemSorter = categorizedSorter;
			if (!(listCategorizedMaterials.ListViewItemSorter is ListViewSorter))
				return;
			categorizedSorter = (ListViewSorter)listCategorizedMaterials.ListViewItemSorter;
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
				selectedCategory = null;
				InitializeCategorizedMaterialListView();
				UpdateButtons();
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
			selectedCategory = (listCategories.SelectedItem as Category);

			if (selectedCategory != null) {
				txtCategoryName.Text = selectedCategory.Name;
				txtCategoryName.Enabled = true;
				btnRemove.Enabled = true;
				btnUp.Enabled = selectedCategory.Order > 1;
				btnDown.Enabled = selectedCategory.Order < listCategories.Items.Count;
			}
			InitializeCategorizedMaterialListView();
			UpdateButtons();
		}

		private void btnRemove_Click(object sender, EventArgs e) {
			Category selectedCategory = (listCategories.SelectedItem as Category);
			if (selectedCategory != null) {
				Configuration.AdminTemplate.Categories.Remove(selectedCategory);
				foreach (Material material in selectedCategory.Materials) {
					material.Category = null;
				}
				UpdateCategoryList();
				InitializeUncategorizedMaterialListView();
				UpdateButtons();
			}
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

		private void listUncategorizedMaterials_SelectedIndexChanged(object sender, EventArgs e) {
			UpdateButtons();
		}

		private void listCategorizedMaterials_SelectedIndexChanged(object sender, EventArgs e) {
			UpdateButtons();
		}

		private void UpdateButtons() {
			if (listUncategorizedMaterials.SelectedItems.Count != 0 && selectedCategory != null) {
				btnCategorize.Enabled = true;
			} else {
				btnCategorize.Enabled = false;
			}
			if (listCategorizedMaterials.SelectedItems.Count != 0 && selectedCategory != null) {
				btnUncategorize.Enabled = true;
			} else {
				btnUncategorize.Enabled = false;
			}
		}

		private void btnCategorize_Click(object sender, EventArgs e) {
			if ((selectedCategory != null) && (listUncategorizedMaterials.SelectedItems.Count != 0)) {
				List<ListViewItem> items = new List<ListViewItem>();
				foreach (ListViewItem item in listUncategorizedMaterials.SelectedItems) {
					items.Add(item);
				}
				listCategorizedMaterials.BeginUpdate();
				listUncategorizedMaterials.BeginUpdate();
				foreach (ListViewItem item in items) {
					if (item.Tag != null && item.Tag is Material) {
						Material material = item.Tag as Material;
						material.Category = selectedCategory;
						selectedCategory.Materials.Add(material);
						listUncategorizedMaterials.Items.Remove(item);						
						listCategorizedMaterials.Items.Add(item);
					}
				}
				listCategorizedMaterials.EndUpdate();
				listUncategorizedMaterials.EndUpdate();
				UpdateButtons();
			}
		}

		private void btnUncategorize_Click(object sender, EventArgs e) {
			if (selectedCategory != null) {
				List<ListViewItem> items = new List<ListViewItem>();
				foreach (ListViewItem item in listCategorizedMaterials.SelectedItems) {
					items.Add(item);
				}
				listCategorizedMaterials.BeginUpdate();
				listUncategorizedMaterials.BeginUpdate();
				foreach (ListViewItem item in items) {
					if (item.Tag != null && item.Tag is Material) {
						Material material = item.Tag as Material;
						material.Category = null;
						selectedCategory.Materials.Remove(material);
						listCategorizedMaterials.Items.Remove(item);
						listUncategorizedMaterials.Items.Add(item);
					}
				}
				listCategorizedMaterials.EndUpdate();
				listUncategorizedMaterials.EndUpdate();
				UpdateButtons();
			}
		}

		private void listUncategorizedMaterials_ColumnClick(object sender, ColumnClickEventArgs e) {
			if (uncategorizedSorter.LastSort == e.Column) {
				if (listUncategorizedMaterials.Sorting == SortOrder.Ascending)
					listUncategorizedMaterials.Sorting = SortOrder.Descending;
				else
					listUncategorizedMaterials.Sorting = SortOrder.Ascending;
			} else {
				listUncategorizedMaterials.Sorting = SortOrder.Descending;
			}
			uncategorizedSorter.ByColumn = e.Column;

			listUncategorizedMaterials.Sort();
		}

		private void listCategorizedMaterials_ColumnClick(object sender, ColumnClickEventArgs e) {
			if (categorizedSorter.LastSort == e.Column) {
				if (listCategorizedMaterials.Sorting == SortOrder.Ascending)
					listCategorizedMaterials.Sorting = SortOrder.Descending;
				else
					listCategorizedMaterials.Sorting = SortOrder.Ascending;
			} else {
				listCategorizedMaterials.Sorting = SortOrder.Descending;
			}
			categorizedSorter.ByColumn = e.Column;

			listCategorizedMaterials.Sort();
		}
	}

	public class ListViewSorter : System.Collections.IComparer {
		public int Compare(object o1, object o2) {
			if (!(o1 is ListViewItem))
				return (0);
			if (!(o2 is ListViewItem))
				return (0);

			ListViewItem lvi1 = (ListViewItem)o2;
			string str1 = lvi1.SubItems[ByColumn].Text;
			ListViewItem lvi2 = (ListViewItem)o1;
			string str2 = lvi2.SubItems[ByColumn].Text;

			int result;
			if (lvi1.ListView.Sorting == SortOrder.Ascending)
				result = String.Compare(str1, str2);
			else
				result = String.Compare(str2, str1);

			LastSort = ByColumn;

			return (result);
		}


		public int ByColumn {
			get { return Column; }
			set { Column = value; }
		}
		int Column = 0;

		public int LastSort {
			get { return LastColumn; }
			set { LastColumn = value; }
		}
		int LastColumn = 0;
	} 
}
