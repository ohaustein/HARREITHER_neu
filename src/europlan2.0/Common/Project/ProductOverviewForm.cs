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

		private ComboBox layDistanceCombo;
		private ComboBox rimTypeCombo;
		
		public ProductOverviewForm(bool heat) {
			updateOngoing = true;
			InitializeComponent();

			this.SetLanguage();

			layDistanceCombo = new ComboBox();
			layDistanceCombo.Size = new Size(30, 20);
			layDistanceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			dgvProductOverview.Controls.Add(layDistanceCombo);
			layDistanceCombo.Hide();
			layDistanceCombo.SelectedValueChanged += new EventHandler(layDistanceCombo_SelectedValueChanged);

			rimTypeCombo = new ComboBox();
			rimTypeCombo.Size = new Size(30, 20);
			rimTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			dgvProductOverview.Controls.Add(rimTypeCombo);
			rimTypeCombo.Hide();
			rimTypeCombo.SelectedValueChanged += new EventHandler(rimTypeCombo_SelectedValueChanged);

			showHeat = heat;
			UpdateControl();
			updateOngoing = false;
		}

		private void SetLanguage() {
			this.roomIdDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Raumnr; //"Raumnr."
			this.roomNameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Raumname; //"Raumname"
			this.teilSystemDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Teilsystem; //"Teil-\nsystem"
			this.systemNameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_System; //"System"
			this.nrOfCircuitsDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Heizkreise; //"Heiz-\nkreis(e)"
			this.RimType.HeaderText = EuroplanRes.ProductOverviewForm_VerlegeabstandRz; //"Verlegeabstand\nRZ"
			this.LayDistance.HeaderText = EuroplanRes.ProductOverviewForm_VerlegeabstandAz; //"Verlegeabstand\nAZ"
			this.pipeLengthDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Rohrlaenge; //"Rohrlänge\nm"
			this.totalAreaDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Flaeche; //"Fläche\nm²"
			this.druckverlustHeatDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_DruckverlustHeiz; //"Druckverlust\nmbar"
			this.heatNetLoadDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Normwaerme; //"Normwärme\nW"
			this.heatRestDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Restwaerme; //"Restwärme\nW"
			this.druckverlustCoolDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_DruckverlustKuehl; //"Druckverlust\nmbar"
			this.coolNetLoadDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Kuehllast; //"Kühllast\nW"
			this.coolRestDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Restkuellast; //"Rest\nW"
			this.okDataGridViewCheckBoxColumn.HeaderText = EuroplanRes.ProductOverviewForm_Ok; //"Ok"
			this.editColumn.HeaderText = EuroplanRes.General_BearbeitenCol; //"Bearbeiten"
			this.Text = EuroplanRes.ProductOverviewForm_Titel; //"Übersicht"

		}

		public void ReloadGrid() {
			int col = dgvProductOverview.SelectedCells.Count > 0 ? dgvProductOverview.SelectedCells[0].ColumnIndex : -1;
			int row = dgvProductOverview.SelectedCells.Count > 0 ? dgvProductOverview.SelectedCells[0].RowIndex : -1;
			productOverviewWrapperBindingSource.ResetBindings(false);
			if (col > -1) {
				dgvProductOverview.Rows[row].Cells[col].Selected = true;
			}
		}

		private void layDistanceCombo_SelectedValueChanged(object sender, EventArgs e) {
			Nullable<ProductOverviewWrapper.LayDistanceEnum> layDistance = null;
			if (layDistanceCombo.SelectedItem != null) {
				layDistance = (layDistanceCombo.SelectedItem as ProductOverviewWrapper.LayDistanceItem).layDistance;
			}
			this.dgvProductOverview.Rows[dgvProductOverview.CurrentCell.RowIndex].Cells[LayDistance.Index].Value = layDistance;
			ReloadGrid();
		}

		private void rimTypeCombo_SelectedValueChanged(object sender, EventArgs e) {
			Nullable<ProductOverviewWrapper.RimTypeEnum> rimType = null;
			if (rimTypeCombo.SelectedItem != null) {
				rimType = (rimTypeCombo.SelectedItem as ProductOverviewWrapper.RimTypeItem).rimType;
			}
			this.dgvProductOverview.Rows[dgvProductOverview.CurrentCell.RowIndex].Cells[RimType.Index].Value = rimType;
			ReloadGrid();
		}

		private void dgvProductOverview_CellEnter(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex == LayDistance.Index && e.RowIndex >= 0) {
				if ((this.dgvProductOverview.Rows[e.RowIndex].DataBoundItem as ProductOverviewWrapper).LayDistanceEditable) {

					this.layDistanceCombo.SelectedValueChanged -= new EventHandler(layDistanceCombo_SelectedValueChanged);

					DataGridViewCell layDistanceCell = dgvProductOverview.Rows[e.RowIndex].Cells[LayDistance.Index];
					layDistanceCombo.Items.Clear();
					ProductOverviewWrapper.LayDistanceEnumConverter conv = new ProductOverviewWrapper.LayDistanceEnumConverter();

					if (!(this.dgvProductOverview.Rows[e.RowIndex].DataBoundItem as ProductOverviewWrapper).ManualMode) {
						layDistanceCombo.Items.Add(new ProductOverviewWrapper.LayDistanceItem(null, EuroplanRes.EurovalProduct_Automatisch /*"Automatisch"*/));
					}

					DataGridViewRow selectedRow = dgvProductOverview.Rows[e.RowIndex];

					layDistanceCombo.Items.Add(new ProductOverviewWrapper.LayDistanceItem(ProductOverviewWrapper.LayDistanceEnum.EV35, conv.ConvertToString(ProductOverviewWrapper.LayDistanceEnum.EV35)));
					layDistanceCombo.Items.Add(new ProductOverviewWrapper.LayDistanceItem(ProductOverviewWrapper.LayDistanceEnum.EV30, conv.ConvertToString(ProductOverviewWrapper.LayDistanceEnum.EV30)));
					layDistanceCombo.Items.Add(new ProductOverviewWrapper.LayDistanceItem(ProductOverviewWrapper.LayDistanceEnum.EV25, conv.ConvertToString(ProductOverviewWrapper.LayDistanceEnum.EV25)));
					layDistanceCombo.Items.Add(new ProductOverviewWrapper.LayDistanceItem(ProductOverviewWrapper.LayDistanceEnum.EV20, conv.ConvertToString(ProductOverviewWrapper.LayDistanceEnum.EV20)));
					layDistanceCombo.Items.Add(new ProductOverviewWrapper.LayDistanceItem(ProductOverviewWrapper.LayDistanceEnum.EV15, conv.ConvertToString(ProductOverviewWrapper.LayDistanceEnum.EV15)));
					layDistanceCombo.Items.Add(new ProductOverviewWrapper.LayDistanceItem(ProductOverviewWrapper.LayDistanceEnum.EV10, conv.ConvertToString(ProductOverviewWrapper.LayDistanceEnum.EV10)));
					layDistanceCombo.Items.Add(new ProductOverviewWrapper.LayDistanceItem(ProductOverviewWrapper.LayDistanceEnum.EV5, conv.ConvertToString(ProductOverviewWrapper.LayDistanceEnum.EV5)));

					layDistanceCombo.SelectedItem = new ProductOverviewWrapper.LayDistanceItem((Nullable<ProductOverviewWrapper.LayDistanceEnum>)layDistanceCell.Value, "");
					this.layDistanceCombo.SelectedValueChanged += new EventHandler(layDistanceCombo_SelectedValueChanged);

					Rectangle rect = dgvProductOverview.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					layDistanceCombo.Location = new Point(rect.X, rect.Y);
					layDistanceCombo.Size = new Size(rect.Width, rect.Height);
					layDistanceCombo.Show();
				}
			} else if (e.ColumnIndex == RimType.Index && e.RowIndex >= 0) {
				if ((this.dgvProductOverview.Rows[e.RowIndex].DataBoundItem as ProductOverviewWrapper).RimTypeEditable) {

					this.rimTypeCombo.SelectedValueChanged -= new EventHandler(rimTypeCombo_SelectedValueChanged);

					DataGridViewCell rimTypeCell = dgvProductOverview.Rows[e.RowIndex].Cells[RimType.Index];
					rimTypeCombo.Items.Clear();
					ProductOverviewWrapper.RimTypeEnumConverter conv = new ProductOverviewWrapper.RimTypeEnumConverter();

					if (!(this.dgvProductOverview.Rows[e.RowIndex].DataBoundItem as ProductOverviewWrapper).ManualMode) {
						rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(null, EuroplanRes.EurovalProduct_Automatisch /*"Automatisch"*/));
					}

					DataGridViewRow selectedRow = dgvProductOverview.Rows[e.RowIndex];

					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV15_60, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV15_60)));
					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV15_120, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV15_120)));
					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV15_180, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV15_180)));
					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV10_55, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV10_55)));
					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV10_110, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV10_110)));
					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV10_165, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV10_165)));
					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV5_40, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV5_40)));
					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV5_80, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV5_80)));
					rimTypeCombo.Items.Add(new ProductOverviewWrapper.RimTypeItem(ProductOverviewWrapper.RimTypeEnum.EV5_120, conv.ConvertToString(ProductOverviewWrapper.RimTypeEnum.EV5_120)));

					rimTypeCombo.SelectedItem = new ProductOverviewWrapper.RimTypeItem((Nullable<ProductOverviewWrapper.RimTypeEnum>)rimTypeCell.Value, "");
					this.rimTypeCombo.SelectedValueChanged += new EventHandler(rimTypeCombo_SelectedValueChanged);

					Rectangle rect = dgvProductOverview.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
					rimTypeCombo.Location = new Point(rect.X, rect.Y);
					rimTypeCombo.Size = new Size(rect.Width, rect.Height);
					rimTypeCombo.Show();
				}
			}
		}

		private void dgvProductOverview_CellLeave(object sender, DataGridViewCellEventArgs e) {
			layDistanceCombo.Hide();
			rimTypeCombo.Hide();
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

				if (!(row.DataBoundItem as ProductOverviewWrapper).RimTypeEditable) {
					row.Cells[RimType.Index].Style.BackColor = SystemColors.Control;
					row.Cells[RimType.Index].ReadOnly = true;
				} else {
					row.Cells[RimType.Index].Style.BackColor = row.DefaultCellStyle.BackColor;
					row.Cells[RimType.Index].ReadOnly = true;
				}

				if (!(row.DataBoundItem as ProductOverviewWrapper).LayDistanceEditable) {
					row.Cells[LayDistance.Index].Style.BackColor = SystemColors.Control;
					row.Cells[LayDistance.Index].ReadOnly = true;
				} else {
					row.Cells[LayDistance.Index].Style.BackColor = row.DefaultCellStyle.BackColor;
					row.Cells[LayDistance.Index].ReadOnly = true;
				}

			}
		}

		private void dgvProductOverview_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Delete) {

				DataGridViewCell cell = this.dgvProductOverview.SelectedCells[0];
				DataGridViewColumn col = cell.OwningColumn;

				if (!(cell.OwningRow.DataBoundItem as ProductOverviewWrapper).ManualMode) {
					if (col == this.nrOfCircuitsDataGridViewTextBoxColumn) {
						e.IsInputKey = false;
						cell.Value = null;
					}
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
			} else if (e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvProductOverview.Columns.Count &&
				  this.dgvProductOverview.Columns[e.ColumnIndex] == this.LayDistance &&
				  e.RowIndex >= 0 && e.RowIndex < this.dgvProductOverview.Rows.Count) {
				ProductOverviewWrapper wrapper = (this.dgvProductOverview.Rows[e.RowIndex].DataBoundItem as ProductOverviewWrapper);
				if (wrapper.LayDistanceModified) {
					e.Paint(e.CellBounds, DataGridViewPaintParts.All);
					e.Graphics.FillEllipse(Brushes.Red, e.CellBounds.X + 6, e.CellBounds.Y + (e.CellBounds.Height / 2) - 4, (float)7, (float)7);
					e.Handled = true;
				}
			} else if (e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvProductOverview.Columns.Count &&
				  this.dgvProductOverview.Columns[e.ColumnIndex] == this.RimType &&
				  e.RowIndex >= 0 && e.RowIndex < this.dgvProductOverview.Rows.Count) {
				ProductOverviewWrapper wrapper = (this.dgvProductOverview.Rows[e.RowIndex].DataBoundItem as ProductOverviewWrapper);
				if (wrapper.RimTypeModified) {
					e.Paint(e.CellBounds, DataGridViewPaintParts.All);
					e.Graphics.FillEllipse(Brushes.Red, e.CellBounds.X + 6, e.CellBounds.Y + (e.CellBounds.Height / 2) - 4, (float)7, (float)7);
					e.Handled = true;
				}
			}
		}

	}
}