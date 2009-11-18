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
					p.UsedForQuickDimensioning = false;
					p.AssociatedRoom = this.room;
					PlannedProduct pp = new PlannedProduct(p);
					if (p.Type == Product.ProductType.FBH) {
						double plannedFloorArea = this.room.Area;
						//double necessaryHeatLoad = this.room.NormalizedHeatLoad;
						foreach (PlannedProduct plannedP in this.room.PlannedProducts) {
							plannedFloorArea -= plannedP.Product.PlannedFloorArea;
							//necessaryHeatLoad -= plannedP.PlannedHeatLoad;
						}
						if (plannedFloorArea < 0) {
							plannedFloorArea = 0;
						}
						p.PlannedFloorArea = (float)plannedFloorArea;
						pp.ConfigureProductDefault();

					} else if (p.Type == Product.ProductType.DH) {
						double plannedCeilingArea = this.room.Area;
						foreach (PlannedProduct plannedP in this.room.PlannedProducts) {
							plannedCeilingArea -= plannedP.Product.PlannedCeilingArea;
						}
						if (plannedCeilingArea < 0) {
							plannedCeilingArea = 0;
						}
						p.PlannedCeilingArea = (float)plannedCeilingArea;
						pp.ConfigureProductDefault();
					} else {
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


		private void btnDelete_Click(object sender, EventArgs e) {
			int rowIndex = -1;
			if (dgvProducts.CurrentCell != null) {
				rowIndex = dgvProducts.CurrentCell.RowIndex;
			}
			if (rowIndex >= 0) {
				DataGridViewRow row = dgvProducts.Rows[rowIndex];
				PlannedProduct product = row.DataBoundItem as PlannedProduct;
				if (product.Node != null) {
					if (MessageBox.Show("Wollen Sie das System " + product.Node.Text + " wirklich löschen.", "Heizsystem löschen", MessageBoxButtons.YesNo) == DialogResult.Yes) {

						PlannedProduct connectedProduct = this.room.GetFloor().FindConnectedProduct(product);
						if (connectedProduct != null) {
							connectedProduct.Product.PlannedConnection = null;
							string err;
							connectedProduct.Product.ConfigureProduct(connectedProduct.RequestedHeatLoad, connectedProduct.RequestedCoolLoad, connectedProduct.CalculateHeat, connectedProduct.CalculateCool, out err);
						}
						this.room.PlannedProducts.Remove(product);
						if (this.ProjectStructureChanged != null) {
							this.ProjectStructureChanged(this);
						}

						List<PlannedProduct> products = this.room.PlannedProducts;
						Dictionary<Product.ProductType, int> productCounter = new Dictionary<Product.ProductType, int>();
						foreach (PlannedProduct p in products) {
							if (!productCounter.ContainsKey(p.PlannedProductType)) {
								productCounter.Add(p.PlannedProductType, 1);
							} else {
								productCounter[p.PlannedProductType] = productCounter[p.PlannedProductType] + 1;
							}
							p.Node.Text = p.PlannedProductType.ToString() + productCounter[p.PlannedProductType] + ": " + p.System;
						}
					}
					this.UpdateControl();
				}
			}
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
				PlannedProduct connectedProduct = this.room.GetFloor().FindConnectedProduct(this.deletedProduct);
				if (connectedProduct != null) {
					connectedProduct.Product.PlannedConnection = null;
					string err;
					connectedProduct.Product.ConfigureProduct(connectedProduct.RequestedHeatLoad, connectedProduct.RequestedCoolLoad, connectedProduct.CalculateHeat, connectedProduct.CalculateCool, out err);
				}
				if (this.ProjectStructureChanged != null) {
					this.ProjectStructureChanged(this);
				}
			}
			this.deletedProduct = null;

			List<PlannedProduct> products = this.room.PlannedProducts;
			Dictionary<Product.ProductType, int> productCounter = new Dictionary<Product.ProductType, int>();
			foreach (PlannedProduct p in products) {
				if (!productCounter.ContainsKey(p.PlannedProductType)) {
					productCounter.Add(p.PlannedProductType, 1);
				} else {
					productCounter[p.PlannedProductType] = productCounter[p.PlannedProductType] + 1;
				}
				p.Node.Text = p.PlannedProductType.ToString() + productCounter[p.PlannedProductType] + ": " + p.System;
			}
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

		private void dgvProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void dgvProducts_SelectionChanged(object sender, EventArgs e) {
			int rowIndex = -1;
			if (dgvProducts.CurrentCell != null) {
				rowIndex = dgvProducts.CurrentCell.RowIndex;
			}
			if (rowIndex >= 0) {
				DataGridViewRow row = dgvProducts.Rows[rowIndex];
				PlannedProduct product = row.DataBoundItem as PlannedProduct;
				btnDelete.Enabled = product.Node != null ? true : false;
			}
		}

		private void btnWhatIsNext_Click(object sender, EventArgs e) {
			MessageBox.Show("Klicken Sie auf einen der Buttons in der Spalte\n'Bearbeiten' um das entsprechende Heizsystem zu öffnen, oder\n klicken Sie in der Projekthierarchie auf das gewünschte Heizsystem.", "Wie geht's weiter?");
		}

	}
}
