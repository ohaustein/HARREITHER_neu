using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class RoomSummaryPanel : UserControl, IEditorUserControl {
		
		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Room room;
		
		public RoomSummaryPanel() {
			InitializeComponent();
			room = null;
		}


		public void UpdateControl() {
			if (this.Tag != null) {
				this.room = this.Tag as Room;
				this.lblRoomName.Text = this.room.Id + " - " + this.room.Name;
				this.lblAreaValue.Text = this.room.Area.ToString("0.0") + " m²";
				this.lblHeatLoadValue.Text = this.room.NormalizedHeatLoad.ToString("0") + " W (bereinigt)";
				this.lblCoolLoadValue.Text = this.room.NormalizedCoolLoad.ToString("0") + " W (bereinigt)";
				List<PlannedProduct> plannedProducts = new List<PlannedProduct>();
				foreach (PlannedProduct plannedProduct in this.room.PlannedProducts) {
					plannedProducts.Add(plannedProduct);
				}
				plannedProducts.Add(new PlannedProduct(this.room));
				plannedProductWrapperBindingSource.DataSource = plannedProducts;
				plannedProductWrapperBindingSource.ResetBindings(false);
				//this.txtName.Text = room.Name
				//this.txtArea.Value = (decimal)room.Area;
				//this.txtTemperature.Text = room.RoomTemperature.ToString();
				//this.txtHeat.Text = room.HeatLoad.ToString();
				//this.txtNormHeat.Text = room.NormalizedHeatLoad.ToString();
				//this.txtCool.Text = room.CoolLoad.ToString();
				//this.txtNormCool.Text = room.NormalizedCoolLoad.ToString();
			}
		}

		public bool AllowLeave() {
			return true;
		}

		private void txtName_TextChanged(object sender, EventArgs e) {
			this.room.Name = this.txtName.Text;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void txtArea_TextChanged(object sender, EventArgs e) {
			try {
				this.room.Area = (float)this.txtArea.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtTemperature.Text = room.RoomHeatTemperature.ToString();
			}
		}

		private void txtTemperature_TextChanged(object sender, EventArgs e) {
			try {
				this.room.RoomHeatTemperature = (int)this.txtTemperature.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtTemperature.Text = room.RoomHeatTemperature.ToString();
			}
		}

		private void txtHeat_TextChanged(object sender, EventArgs e) {
			try {
				this.room.HeatLoad = (int)this.txtHeat.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtHeat.Text = room.HeatLoad.ToString();
			}
		}

		private void txtNormHeat_TextChanged(object sender, EventArgs e) {
			try {
				this.room.NormalizedHeatLoad = (int)this.txtNormHeat.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtNormHeat.Text = room.NormalizedHeatLoad.ToString();
			}
		}

		private void txtCool_TextChanged(object sender, EventArgs e) {
			try {
				this.room.CoolLoad = (int)this.txtCool.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtCool.Text = room.CoolLoad.ToString();
			}
		}

		private void txtNormCool_TextChanged(object sender, EventArgs e) {
			try {
				this.room.NormalizedCoolLoad = (int)this.txtNormCool.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtNormCool.Text = room.NormalizedCoolLoad.ToString();
			}
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			NewHeatingSystemForm form = new NewHeatingSystemForm();
			if (form.ShowDialog() == DialogResult.OK) {
				Type productType = form.SelectedProductType;
				if (productType != null) {
					Product p = productType.GetConstructor(new Type[0]).Invoke(new object[0]) as Product;
					p.AssociatedRoom = this.room;
					PlannedProduct pp = new PlannedProduct(p);
					if (p.Type == Product.ProductType.FBH) {
						p.PlannedFloorArea = this.room.Area;
						double necessaryHeatLoad = this.room.NormalizedHeatLoad;
						foreach (PlannedProduct plannedP in this.room.PlannedProducts) {
							if (plannedP.PlannedArea.HasValue) {
								p.PlannedFloorArea -= plannedP.PlannedArea.Value;
							}
							necessaryHeatLoad -= plannedP.PlannedHeatLoad;
						}
						if (p.PlannedFloorArea < 0) {
							p.PlannedFloorArea = 0;
						}
						pp.ConfigureProductDefault();

					}
					this.room.PlannedProducts.Add(pp);
					this.UpdateControl();
					if (this.ProjectStructureChanged != null) {
						this.ProjectStructureChanged(this);
					}
				}
			}
			form.Dispose();
		}

		private PlannedProduct deletedProduct = null;

		private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			if (e.Row.DataBoundItem is PlannedProduct) {
				if ((e.Row.DataBoundItem as PlannedProduct).Product == null) {
					e.Cancel = true;
				} else {
					this.deletedProduct = e.Row.DataBoundItem as PlannedProduct;
				}
			}
		}

		private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			if (this.deletedProduct != null) {
				this.room.PlannedProducts.Remove(this.deletedProduct);
				if (this.ProjectStructureChanged != null) {
					this.ProjectStructureChanged(this);
				}
			}
			this.deletedProduct = null;
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvProducts.Columns.Count &&
			  this.dgvProducts.Columns[e.ColumnIndex] == this.colEdit &&
			  e.RowIndex >= 0 && e.RowIndex < this.dgvProducts.Rows.Count) {
				PlannedProduct pp = this.dgvProducts.Rows[e.RowIndex].DataBoundItem as PlannedProduct;
				if (pp != null) {
					if (TreeSelectionRequested != null) {
						TreeSelectionRequested(this, pp);
					}
				}
			}

		}

		private void dgvProducts_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
			if (e.ColumnIndex == this.dgvProducts.Columns.Count - 1 &&
			  e.RowIndex >= 0 && e.RowIndex < this.dgvProducts.Rows.Count &&
			  (this.dgvProducts.Rows[e.RowIndex].DataBoundItem as PlannedProduct).Product == null) {
				e.PaintBackground(e.ClipBounds, true);
				e.Handled = true;
			}
		}


	}
}
