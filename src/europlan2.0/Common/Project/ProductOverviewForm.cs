using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class ProductOverviewForm : Form {

		private bool gridContentChanged = false;
		private bool showHeat = true;
		private bool updateOngoing = false;
		private PlannedProduct productToEdit = null;
		
		public ProductOverviewForm(bool heat) {
			updateOngoing = true;
			InitializeComponent();
			showHeat = heat;
			UpdateControl();
			updateOngoing = false;
		}

		private void UpdateControl() {
			updateOngoing = true;
			List<ProductOverviewWrapper> wrapperList = new List<ProductOverviewWrapper>();
			ProductOverviewWrapper wrapper;
			if (Project.Instance != null) {
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							wrapper = new ProductOverviewWrapper(pp);
							if ((showHeat && pp.RequestedHeatLoad > 0) || (!showHeat && pp.RequestedCoolLoad > 0)) {
								wrapperList.Add(wrapper);
							}
						}
					}
				}
			}

			druckverlustHeatDataGridViewTextBoxColumn.Visible = showHeat;
			heatNetLoadDataGridViewTextBoxColumn.Visible = showHeat;
			heatRestDataGridViewTextBoxColumn.Visible = showHeat;
			druckverlustCoolDataGridViewTextBoxColumn.Visible = !showHeat;
			coolNetLoadDataGridViewTextBoxColumn.Visible = !showHeat;
			coolRestDataGridViewTextBoxColumn.Visible = !showHeat;

			productOverviewWrapperBindingSource.DataSource = wrapperList;
			productOverviewWrapperBindingSource.ResetBindings(false);
			updateOngoing = false;
		}

		private void ProductOverviewForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProductOverviewForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();

		}

		private void ProductOverviewForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProductOverviewForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		public bool UnsavedChanges {
			get { return gridContentChanged; }
		}

		public PlannedProduct ProductToEdit {
			get { return productToEdit; }
		}

		private void dgvProductOverview_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (!updateOngoing) {
				gridContentChanged = true;
				UpdateControl();
			}
		}

		private void dgvProductOverview_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvProductOverview.Columns.Count &&
				  this.dgvProductOverview.Columns[e.ColumnIndex] == this.editColumn &&
				  e.RowIndex >= 0 && e.RowIndex < this.dgvProductOverview.Rows.Count) {
				PlannedProduct pp = (this.dgvProductOverview.Rows[e.RowIndex].DataBoundItem as ProductOverviewWrapper).PlannedProduct;
				if (pp != null) {
					productToEdit = pp;
				}
				this.Close();
			}
		}

		private void dgvProductOverview_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.dgvProductOverview.Rows[i];
				if (!(row.DataBoundItem as ProductOverviewWrapper).NrOfCircuitsEditable) {
					row.Cells[nrOfCircuitsDataGridViewTextBoxColumn.Index].Style.BackColor = SystemColors.Control;
					row.Cells[nrOfCircuitsDataGridViewTextBoxColumn.Index].ReadOnly = true;
				} else {
					row.Cells[nrOfCircuitsDataGridViewTextBoxColumn.Index].Style.BackColor = row.DefaultCellStyle.BackColor;
					row.Cells[nrOfCircuitsDataGridViewTextBoxColumn.Index].ReadOnly = false;
				}
			}
		}

		private void dgvProductOverview_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Delete) {

				DataGridViewCell cell = this.dgvProductOverview.SelectedCells[0];
				DataGridViewColumn col = cell.OwningColumn;

				if (col == this.nrOfCircuitsDataGridViewTextBoxColumn) {
					e.IsInputKey = false;
					cell.Value = null;
				}

			}
		}

		private void dgvProductOverview_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvProductOverview.Columns.Count &&
				  this.dgvProductOverview.Columns[e.ColumnIndex] == this.nrOfCircuitsDataGridViewTextBoxColumn &&
				  e.RowIndex >= 0 && e.RowIndex < this.dgvProductOverview.Rows.Count) {
				ProductOverviewWrapper wrapper = (this.dgvProductOverview.Rows[e.RowIndex].DataBoundItem as ProductOverviewWrapper);
				if (wrapper.NrOfCircuitsModified) {
					e.Paint(e.CellBounds, DataGridViewPaintParts.All);
					e.Graphics.FillEllipse(Brushes.Red, e.CellBounds.X + 6, e.CellBounds.Y + (e.CellBounds.Height / 2) - 4, (float)7, (float)7);
					e.Handled = true;
				}
			}
		}
	}
}