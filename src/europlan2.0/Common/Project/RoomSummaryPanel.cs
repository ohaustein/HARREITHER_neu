using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WW.Math;

namespace Europlan.Common {
	public partial class RoomSummaryPanel : UserControl, IEditorUserControl {
		
		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Room room;
		private bool updateOngoing = false;
		
		public RoomSummaryPanel() {
			InitializeComponent();

			this.SetLanguage();
			room = null;
		}

		private void SetLanguage() {
			this.label1.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.label2.Text = EuroplanRes.Unit_GradCelsius; //"°C";
			this.label3.Text = EuroplanRes.Unit_Watt; //"W";
			this.label4.Text = EuroplanRes.Unit_Watt; //"W";
			this.label5.Text = EuroplanRes.Unit_Watt; //"W";
			this.label6.Text = EuroplanRes.Unit_Watt; //"W";

			this.btnWhatIsNext.Text = EuroplanRes.General_WieGehtsWeiter; //"Wie geht\'s weiter?";
			this.colEdit.HeaderText = EuroplanRes.General_BearbeitenCol; //"Bearbeiten";

			this.lblName.Text = EuroplanRes.RoomSummaryPanel_Name; //"Name:";
			this.lblArea.Text = EuroplanRes.RoomSummaryPanel_Flaeche; //"Fläche:";
			this.lblTemperature.Text = EuroplanRes.RoomSummaryPanel_Raumtemperatur; //"Norminnentemperatur:";
			this.lblHeat.Text = EuroplanRes.RoomSummaryPanel_Heizlast; //"Heizlast:";
			this.lblNormHeat.Text = EuroplanRes.RoomSummaryPanel_HeizlastBereinigt; //"Heizlast (bereinigt):";
			this.lblCool.Text = EuroplanRes.RoomSummaryPanel_Kuehllast; //"Kühllast:";
			this.lblNormCool.Text = EuroplanRes.RoomSummaryPanel_KuehllastBereinigt; //"Kühllast (bereinigt):";
			this.lblRoomData.Text = EuroplanRes.RoomSummaryPanel_Raumdaten; //"Raumdaten:";
			this.grpBoxSystems.Text = EuroplanRes.RoomSummaryPanel_Heizsysteme; //"Heizsysteme";
			this.btnDelete.Text = EuroplanRes.RoomSummaryPanel_Loeschen; //"Löschen";
			this.btnAdd.Text = EuroplanRes.RoomSummaryPanel_Hinzufuegen; //"Hinzufügen";
			this.colType.HeaderText = EuroplanRes.RoomSummaryPanel_Typ; //"Typ";
			this.colSystem.HeaderText = EuroplanRes.RoomSummaryPanel_System; //"System";
			this.colComment.HeaderText = EuroplanRes.RoomSummaryPanel_Bemerkung; //"Bemerkung";
			this.colFloorArea.HeaderText = EuroplanRes.RoomSummaryPanel_FbhFlaeche; //"FBH-\nFläche\n(m²)";
			this.colPlannedArea.HeaderText = EuroplanRes.RoomSummaryPanel_Heizflaeche; //"Heiz-\nfläche\n(m²)";
			this.colPlannedHeatLoad.HeaderText = EuroplanRes.RoomSummaryPanel_Heizleistung; //"PHeiz\n(W)";
			this.colPlannedCoolLoad.HeaderText = EuroplanRes.RoomSummaryPanel_Kuehlleistung; //"PKühl\n(W)";
			this.btnGeometry.Text = EuroplanRes.RoomSummaryPanel_Raumgeometrie_Erfassen; //"Raumgeometrie erfassen";
			this.btnCeilingGeometry.Text = EuroplanRes.RoomSummaryPanel_Deckengeometrie_Erfassen; //"Deckengeometrie erfassen";
			this.chkCeilingGeometry.Text = EuroplanRes.RoomSummaryPanel_Deckengeometrie_Gesondert_Erfassen; //"Deckengeometrie gesondert erfassen"
		}


		public void UpdateControl(bool resetUserInterface) {
			updateOngoing = true;

			if (resetUserInterface) {
				this.chkCeilingGeometry.Checked = false;
			}
			
			if (this.Tag != null) {
				this.room = this.Tag as Room;
				this.lblRoomName.Text = this.room.Id + " - " + this.room.Name;
				this.lblAreaValue.Text = this.room.Area.ToString("0.0") + EuroplanRes.Unit_Quadratmeter;
				this.lblHeatLoadValue.Text = this.room.NormalizedHeatLoad.ToString("0") + EuroplanRes.Unit_Watt + " " + EuroplanRes.RoomSummaryPanel_Bereinigt;
				this.lblCoolLoadValue.Text = this.room.NormalizedCoolLoad.ToString("0") + EuroplanRes.Unit_Watt + " " + EuroplanRes.RoomSummaryPanel_Bereinigt;
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

				this.btnGeometry.Enabled = this.room.AssociatedFloor.AssociatedPlanId != null ? true : false;
				this.chkCeilingGeometry.Visible = this.btnGeometry.Enabled && this.room.RoomCoordinates != null && this.room.RoomCoordinates.Count > 0;
				if (this.room.CeilingCoordinates != null && this.room.CeilingCoordinates.Count > 0) {
					this.chkCeilingGeometry.Checked = true;
				}
				this.btnCeilingGeometry.Visible = this.chkCeilingGeometry.Checked;
			}
			updateOngoing = false;
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
				MessageBox.Show(EuroplanRes.RoomSummaryPanel_EingabefehlerText, EuroplanRes.RoomSummaryPanel_EingabefehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MessageBox.Show(EuroplanRes.RoomSummaryPanel_EingabefehlerText, EuroplanRes.RoomSummaryPanel_EingabefehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MessageBox.Show(EuroplanRes.RoomSummaryPanel_EingabefehlerText, EuroplanRes.RoomSummaryPanel_EingabefehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MessageBox.Show(EuroplanRes.RoomSummaryPanel_EingabefehlerText, EuroplanRes.RoomSummaryPanel_EingabefehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MessageBox.Show(EuroplanRes.RoomSummaryPanel_EingabefehlerText, EuroplanRes.RoomSummaryPanel_EingabefehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MessageBox.Show(EuroplanRes.RoomSummaryPanel_EingabefehlerText, EuroplanRes.RoomSummaryPanel_EingabefehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					pp.CalculateHeat = p.DefaultCalculateMode == Product.CalculateModeEnum.HEAT || p.DefaultCalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
					pp.CalculateCool = p.DefaultCalculateMode == Product.CalculateModeEnum.COOL || p.DefaultCalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
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
					} else if (p.Type == Product.ProductType.DH) {
						double plannedCeilingArea = this.room.Area;
						foreach (PlannedProduct plannedP in this.room.PlannedProducts) {
							plannedCeilingArea -= plannedP.Product.PlannedCeilingArea;
						}
						if (plannedCeilingArea < 0) {
							plannedCeilingArea = 0;
						}
						p.PlannedCeilingArea = (float)plannedCeilingArea;
					}

					foreach (Distributor dist in this.room.AssociatedFloor.Distributors) {
						if (((dist.UseForFloor && p.Type == Product.ProductType.FBH) ||
							(dist.UseForWall && p.Type == Product.ProductType.WH) ||
							(dist.UseForCeiling && (p.Type == Product.ProductType.DH || p.Type == Product.ProductType.DSH))) && 
							dist.MaxCircuits - dist.PlannedCircuits - dist.AdditionalCircuits > 0) {
							p.PlannedConnection = new ProductConnection(dist);
							break;
						}
					}
					if (p.PlannedConnection == null) {
						foreach (Floor f in Project.Instance.Floors) {
							foreach (Distributor dist in f.Distributors) {
								if (((dist.UseForFloor && p.Type == Product.ProductType.FBH) ||
									(dist.UseForWall && p.Type == Product.ProductType.WH) ||
									(dist.UseForCeiling && (p.Type == Product.ProductType.DH || p.Type == Product.ProductType.DSH))) &&
									dist.AdditionalFloors.Contains(p.AssociatedRoom.AssociatedFloor) &&
									dist.MaxCircuits - dist.PlannedCircuits - dist.AdditionalCircuits > 0) {
									p.PlannedConnection = new ProductConnection(dist);
									break;
								}
							}
							if (p.PlannedConnection != null) {
								break;
							}
						}
					}
					ProductWithInsulationConstruction pwic = p as ProductWithInsulationConstruction;
					if (pwic != null) {
						pwic.PlannedInsulationConstruction = this.room.GetFloor().LastInsulationConstruction;	
					}

					pp.Product.CalculateMode = pp.Product.DefaultCalculateMode;
					
					pp.ConfigureProduct(false);
					this.room.PlannedProducts.Add(pp);
					this.UpdateControl(true);
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
					string message = EuroplanRes.RoomSummaryPanel_LoeschenBestaetigenText;
					message = message.Replace("%SYSTEM%", product.Node.Text);
					if (MessageBox.Show(message, EuroplanRes.RoomSummaryPanel_LoeschenBestaetigenTitel, MessageBoxButtons.YesNo) == DialogResult.Yes) {

						List<PlannedProduct> connectedProducts = this.room.GetFloor().FindConnectedProduct(product);
						foreach (PlannedProduct connectedProduct in connectedProducts) {
							SelectConnectionForProductForm.UnconnectProduct(connectedProduct);
							connectedProduct.Product.PlannedConnection = null;
							connectedProduct.Product.ConfigureProduct(connectedProduct.RequestedHeatLoad, connectedProduct.RequestedCoolLoad, connectedProduct.CalculateHeat, connectedProduct.CalculateCool, false);
						}
						SelectConnectionForProductForm.UnconnectProduct(product);
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
					this.UpdateControl(true);
				}
			}
		}

		private PlannedProduct deletedProduct = null;

		private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			if (e.Row.DataBoundItem is PlannedProduct) {
				if ((e.Row.DataBoundItem as PlannedProduct).Product == null) {
					e.Cancel = true;
				} else {
					string message = EuroplanRes.RoomSummaryPanel_LoeschenBestaetigenText;
					message = message.Replace("%SYSTEM%", (e.Row.DataBoundItem as PlannedProduct).Node.Text);
					if (MessageBox.Show(message, EuroplanRes.RoomSummaryPanel_LoeschenBestaetigenTitel, MessageBoxButtons.YesNo) == DialogResult.Yes) {
						this.deletedProduct = e.Row.DataBoundItem as PlannedProduct;
					} else {
						e.Cancel = true;
					}
				}
			}
		}

		private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			if (this.deletedProduct != null) {
				List<PlannedProduct> connectedProducts = this.room.GetFloor().FindConnectedProduct(this.deletedProduct);
				foreach (PlannedProduct connectedProduct in connectedProducts) {
					SelectConnectionForProductForm.UnconnectProduct(connectedProduct);
					connectedProduct.Product.PlannedConnection = null;
					connectedProduct.Product.ConfigureProduct(connectedProduct.RequestedHeatLoad, connectedProduct.RequestedCoolLoad, connectedProduct.CalculateHeat, connectedProduct.CalculateCool, false);
				}
				SelectConnectionForProductForm.UnconnectProduct(this.deletedProduct);
				this.room.PlannedProducts.Remove(this.deletedProduct);
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
			MessageBox.Show(EuroplanRes.RoomSummaryPanel_WieGehtsWeiterText, EuroplanRes.RoomSummaryPanel_WieGehtsWeiterTitel);
		}

		private void btnGeometry_Click(object sender, EventArgs e) {
			openGeometryPicker(room.RoomCoordinates, room.RoomUnusedAreaCoordinates , true, false);
			UpdateControl(false);
		}

		private void btnCeilingGeometry_Click(object sender, EventArgs e) {
			openGeometryPicker(room.CeilingCoordinates, room.CeilingUnusedAreaCoordinates, false, true);
			UpdateControl(false);
		}

		private void openGeometryPicker(List<Point2D> coordinates, List<List<Point2D>> unusedCoordinates, bool calculateRoomArea, bool isCeiling) {
			Plan plan = this.room.AssociatedPlan;
			if (plan != null) {
				RoomPickerForm form = new RoomPickerForm(this.room, isCeiling);
				if (room.PlanSettingX.HasValue &&
					room.PlanSettingY.HasValue &&
					room.PlanSettingScale.HasValue &&
					room.PlanSettingAngle.HasValue) {
					form.Panel.SetPlanTransformations(room.PlanSettingScale.Value, room.PlanSettingX.Value, room.PlanSettingY.Value, room.PlanSettingAngle.Value);
				}
				form.RoomCoordinates = coordinates;
				form.UnusedCoordinates = unusedCoordinates;
				form.ShowDialog();
				if (form.UnsavedChanges) {
					double x,y,scale,angle;
					form.Panel.GetPlanTransformations(out scale, out x, out y, out angle);
					room.PlanSettingScale = scale;
					room.PlanSettingX = x;
					room.PlanSettingY = y;
					room.PlanSettingAngle = angle;
					if (calculateRoomArea) {
						// TODO - unbeheizte flächen...
						room.Area = (float)Math.Round(Plan.PolygonArea(form.RoomCoordinates.ToArray()) / Math.Pow(plan.Measure.Value, 2.0), 2);
					}
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
				}
				form.Dispose();
			} else {
				MessageBox.Show(EuroplanRes.RoomSummaryPanel_MeasureTitle, EuroplanRes.RoomSummaryPanel_MeasureCaption);
			}
		}

		private void chkCeilingGeometry_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (!this.chkCeilingGeometry.Checked) {
					this.room.CeilingCoordinates.Clear();
					this.room.CeilingUnusedAreaCoordinates.Clear();
				} else {
					this.room.CeilingCoordinates.Clear();
					this.room.CeilingUnusedAreaCoordinates.Clear();
					this.room.CeilingCoordinates.AddRange(this.room.RoomCoordinates);
				}
				UpdateControl(false);
			}
		}

	}
}
