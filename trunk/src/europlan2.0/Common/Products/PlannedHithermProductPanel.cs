using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class PlannedHithermProductPanel : UserControl, IEditorUserControl {
		private PlannedProduct product = null;

		public PlannedHithermProductPanel() {
			InitializeComponent();

			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_50_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_100_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_150_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_200_10);
			//this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_250_10);
			//this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_300_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_50_5);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_100_5);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_150_5);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_200_5);
			//this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_250_5);
			//this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_300_5);
		}

		#region IEditorUserControl Members
		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private enum FieldEnum {
			NONE = 0,
			HEAT_LOAD = 1,
			COOL_LOAD = 2,
			HEAT_LOAD_PERCENTAGE = 4,
			COOL_LOAD_PERCENTAGE = 8,
			AREA = 16,
			AREA_PERCENTAGE = 32,
			AREA_REDUCED = 64,
			AREA_UNHEATED = 128,
			ROOM_TEMERATURE_BELOW_HEAT = 256,
			ROOM_TEMERATURE_BELOW_COOL = 512,
			MODULES = 1024,
			//LAY_DISTANCE = 4096,
			//RIM_TYPE = 8192,
			//CALCULATION_TYPE = 16384,
			CIRCUITS = 32768,
			REGISTER = 65536,
		}


		private string errorMsg = null;

		public void UpdateControl() {
			this.product = this.Tag as PlannedProduct;
			this.tabs.SelectedTab = this.pageInput;
			this.connectionPipePanel.Update(this.product);
			if (this.product != null) {
				(this.product.Product as HithermProduct).ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out errorMsg);
			}
			this.UpdateControl(FieldEnum.NONE);
		}

		private int ignoreCoverHeatLoad = 0;
		private int ignoreHeatLoad = 0;
		private int ignoreHeatLoadPercentage = 0;
		private int ignoreCoverCoolLoad = 0;
		private int ignoreCoolLoad = 0;
		private int ignoreCoolLoadPercentage = 0;
		private int ignoreArea = 0;
		private int ignoreAreaPercentage = 0;
		private int ignoreAreaReduced = 0;
		private int ignoreAreaUnheated = 0;
		private int ignoreRoomTemperatureBelowHeat = 0;
		private int ignoreRoomTemperatureBelowCool = 0;
		private int ignoreCircuits = 0;
		private int ignoreLengthVerbindungen = 0;
		private int ignoreRegisters = 0;

		private void UpdateControl(FieldEnum skipFields) {
			if (this.product != null) {
				ignoreCoverHeatLoad++;
				ignoreHeatLoad++;
				ignoreHeatLoadPercentage++;
				ignoreCoverCoolLoad++;
				ignoreCoolLoad++;
				ignoreCoolLoadPercentage++;
				ignoreArea++;
				ignoreAreaPercentage++;
				ignoreAreaReduced++;
				ignoreAreaUnheated++;
				ignoreRoomTemperatureBelowHeat++;
				ignoreRoomTemperatureBelowCool++;
				ignoreCircuits++;
				ignoreLengthVerbindungen++;
				ignoreRegisters++;

				HithermProduct hp = this.product.Product as HithermProduct;

				int selectedCircuit = (this.dgvRegisters.SelectedCells.Count > 0 &&
					this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem is HithermRegister) ?
					(this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem as HithermRegister).Heizkreis : -1;

				bool showHeat = true;
				bool showHeatCircuit = selectedCircuit >= 0;
				bool showCool = false;

				lblQHeat.Visible = showHeat;
				lblQHeatUnit.Visible = showHeat;
				lblQAnbHeat.Visible = showHeat;
				lblQAnbHeatUnit.Visible = showHeat;
				lblQHeatDiff.Visible = showHeat;
				lblQHeatDiffUnit.Visible = showHeat;
				lblQHeatRest.Visible = showHeat;
				lblQHeatRestUnit.Visible = showHeat;
				lblAvgqHeat.Visible = showHeatCircuit;
				lblAvgqHeatUnit.Visible = showHeatCircuit;
				lblDurchflussHeat.Visible = showHeatCircuit;
				lblDurchflussHeatUnit.Visible = showHeatCircuit;
				lblDruckverlustHeat.Visible = showHeatCircuit;
				lblDruckverlustHeatUnit.Visible = showHeatCircuit;
				lblQCool.Visible = showCool;
				lblQCoolUnit.Visible = showCool;
				lblQAnbCool.Visible = showCool;
				lblQAnbCoolUnit.Visible = showCool;
				lblQCoolDiff.Visible = showCool;
				lblQCoolDiffUnit.Visible = showCool;
				lblQCoolRest.Visible = showCool;
				lblQCoolRestUnit.Visible = showCool;
				lblAvgqCool.Visible = showCool;
				lblAvgqCoolUnit.Visible = showCool;
				lblDurchflussCool.Visible = showCool;
				lblDurchflussCoolUnit.Visible = showCool;
				lblDruckverlustCool.Visible = showCool;
				lblDruckverlustCoolUnit.Visible = showCool;

				this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
				this.numHeatLoadPercentage.MaxValue = (decimal)(hp.AssociatedRoom.NormalizedHeatLoad <= 0 ? 0 : this.product.NecessaryHeatLoad * 100 / hp.AssociatedRoom.NormalizedHeatLoad);

				if (this.product.NecessaryHeatLoad > 0) {
					this.chkCoverHeatLoad.Enabled = true;
					if ((skipFields & (FieldEnum.HEAT_LOAD | FieldEnum.HEAT_LOAD_PERCENTAGE)) == FieldEnum.NONE) {
						if (this.product.CoverHeatLoad) {
							this.chkCoverHeatLoad.Checked = true;
							this.numHeatLoad.Enabled = false;
							this.numHeatLoadPercentage.Enabled = false;
						} else {
							this.chkCoverHeatLoad.Checked = false;
							this.numHeatLoad.Enabled = true;
							this.numHeatLoadPercentage.Enabled = true;
						}
					}
					if ((skipFields & FieldEnum.HEAT_LOAD) == FieldEnum.NONE) {
						this.numHeatLoad.Value = Math.Round((decimal)this.product.RequestedHeatLoad, 2);
					}
					if ((skipFields & FieldEnum.HEAT_LOAD_PERCENTAGE) == FieldEnum.NONE) {
						this.numHeatLoadPercentage.Value = Math.Round((decimal)(this.product.RequestedHeatLoadPercentage), 2);
					}
				} else {
					this.numHeatLoad.Enabled = false;
					this.numHeatLoad.Text = "";
					this.numHeatLoadPercentage.Enabled = false;
					this.numHeatLoadPercentage.Text = "";
					this.chkCoverHeatLoad.Enabled = false;
					this.chkCoverHeatLoad.Checked = false;
				}
				this.numCoolLoad.MaxValue = (decimal)this.product.NecessaryCoolLoad;
				this.numCoolLoadPercentage.MaxValue = (decimal)(hp.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / hp.AssociatedRoom.NormalizedCoolLoad);
				if (this.product.NecessaryCoolLoad > 0) {
					this.chkCoverCoolLoad.Enabled = true;
					if ((skipFields & (FieldEnum.COOL_LOAD | FieldEnum.COOL_LOAD_PERCENTAGE)) == FieldEnum.NONE) {
						if (this.product.CoverCoolLoad) {
							this.chkCoverCoolLoad.Checked = true;
							this.numCoolLoad.Enabled = false;
							this.numCoolLoadPercentage.Enabled = false;
						} else {
							this.chkCoverCoolLoad.Checked = false;
							this.numCoolLoad.Enabled = true;
							this.numCoolLoadPercentage.Enabled = true;
						}
					}
					if ((skipFields & FieldEnum.COOL_LOAD) == FieldEnum.NONE) {
						this.numCoolLoad.Value = Math.Round((decimal)this.product.RequestedCoolLoad, 2);
					}
					if ((skipFields & FieldEnum.COOL_LOAD_PERCENTAGE) == FieldEnum.NONE) {
						this.numCoolLoadPercentage.Value = Math.Round((decimal)(this.product.RequestedCoolLoadPercentage), 2);
					}
				} else {
					this.numCoolLoad.Enabled = false;
					this.numCoolLoad.Text = "";
					this.numCoolLoadPercentage.Enabled = false;
					this.numCoolLoadPercentage.Text = "";
					this.chkCoverCoolLoad.Enabled = false;
					this.chkCoverCoolLoad.Checked = false;
				}
				this.lblHeatLoadTotal.Text = "(" + this.product.Product.AssociatedRoom.NormalizedHeatLoad.ToString() + " W)";
				this.lblCoolLoadTotal.Text = "(" + this.product.Product.AssociatedRoom.NormalizedCoolLoad.ToString() + " W)";

				if ((skipFields & FieldEnum.REGISTER) == FieldEnum.NONE) {
					List<HithermRegister> allRegisters = new List<HithermRegister>();
					foreach (HithermCircuit c in hp.PlannedCircuits) {
						foreach (HithermRegister r in c.Registers) {
							allRegisters.Add(r);
						}
					}
					this.hithermRegisterBindingSource.DataSource = allRegisters;
				}

				//    // General
				double qDiffHeat = this.product.PlannedHeatLoad - this.product.RequestedHeatLoad;
				double qDiffCool = this.product.PlannedCoolLoad - this.product.RequestedCoolLoad;

				lblRest.Text = "Rest (" + this.product.Product.AssociatedRoom.ToString() + ")";
				lblQHeat.Text = Math.Round(this.product.PlannedHeatLoad, 2).ToString();
				lblQAnbHeat.Text = Math.Round(this.product.Product.PlannedHeatLoadAnbindung, 0).ToString();
				lblQHeatDiff.Text = Math.Round(qDiffHeat, 2).ToString();
				lblQHeatRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenHeatLoad, 2).ToString();
				if (selectedCircuit >= 0) {
					HithermCircuit hc = hp.GetCircuitForRegister(this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem as HithermRegister);
					lblHk.Text = "Heizkreis " + selectedCircuit.ToString() + ":";
					lblAvgqHeat.Text = Math.Round(hc.C_QHeatPerSqm, 2).ToString();
					lblDurchflussHeat.Text = Math.Round(hc.C_DurchflussHeat, 2).ToString();
					lblDruckverlustHeat.Text = Math.Round(hc.C_DruckverlustHeat, 2).ToString();
					//lblTempHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_FloorTempHeat, 2).ToString();
					lblQCool.Text = Math.Round(this.product.PlannedCoolLoad, 2).ToString();
					lblQAnbCool.Text = Math.Round(this.product.Product.PlannedCoolLoadAnbindung, 0).ToString();
					lblQCoolDiff.Text = (qDiffCool > 0 ? "+" : "") + Math.Round(qDiffCool, 2).ToString();
					lblQCoolRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenCoolLoad, 2).ToString();
					lblAvgqCool.Text = (-1.0 * Math.Round(hc.C_QCoolPerSqm, 2)).ToString();
					lblDurchflussCool.Text = Math.Round(hc.C_DurchflussCool, 2).ToString();
					lblDruckverlustCool.Text = Math.Round(hc.C_DruckverlustCool, 2).ToString();
					//lblTempCool.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_FloorTempCool, 2).ToString();
					/*double availableArea = Math.Round(this.product.Product.PlannedNetArea, 2);
					double coveredArea = Math.Round(hp.CoveredCeilingArea, 2);
					double anbArea = Math.Round(hp.PlannedRemoveArea, 2);
					lblAvailableArea.Text = availableArea.ToString();
					lblCoveredArea.Text = coveredArea.ToString();
					lblAnbArea.Text = anbArea.ToString();
					lblRestArea.Text = Math.Round(availableArea - anbArea - coveredArea, 2).ToString();*/
				} else {
				}

				if (hp.PlannedConnection == null) {
					this.txtDistributor.Text = "";
				} else {
					this.txtDistributor.Text = hp.PlannedConnection.ToString();
				}

				if (this.errorMsg != null) {
					this.lblError.Text = this.errorMsg;
					this.lblError.Visible = true;
				} else {
					this.lblError.Visible = false;
				}

				ignoreCoverHeatLoad--;
				ignoreHeatLoad--;
				ignoreHeatLoadPercentage--;
				ignoreCoverCoolLoad--;
				ignoreCoolLoad--;
				ignoreCoolLoadPercentage--;
				ignoreArea--;
				ignoreAreaPercentage--;
				ignoreAreaReduced--;
				ignoreAreaUnheated--;
				ignoreRoomTemperatureBelowHeat--;
				ignoreRoomTemperatureBelowCool--;
				ignoreCircuits--;
				ignoreLengthVerbindungen--;
				ignoreRegisters--;
			}
			// TODO
		}

		public bool AllowLeave() {
			List<PlannedProduct> plannedProducts = this.product.Product.AssociatedRoom.PlannedProducts;
			for (int i = plannedProducts.Count - 1; i >= 0; i-- ) {
				PlannedProduct pp = plannedProducts[i];
				if (pp != this.product) {
					bool reconfigure = false;
					if (pp.RequestedHeatLoad > pp.NecessaryHeatLoad) {
						pp.RequestedHeatLoad = pp.NecessaryHeatLoad;
						reconfigure = true;
					}
					if (pp.RequestedCoolLoad > pp.NecessaryCoolLoad) {
						pp.RequestedCoolLoad = pp.NecessaryCoolLoad;
						reconfigure = true;
					}
					if (reconfigure || pp.CoverHeatLoad || pp.CoverCoolLoad) {
						string errorMsg;
						pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, out errorMsg);
					}
				}
			}
			return true;
		}
		#endregion

		private void chkCoverHeatLoad_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCoverHeatLoad == 0) {
				if (this.chkCoverHeatLoad.Checked) {
					this.numHeatLoad.Enabled = false;
					this.numHeatLoadPercentage.Enabled = false;
					this.numHeatLoadPercentage.Value = 100;
				} else {
					this.numHeatLoad.Enabled = true;
					this.numHeatLoadPercentage.Enabled = true;
					this.numHeatLoadPercentage.Value = 100;
				}
				this.product.CoverHeatLoad = this.chkCoverHeatLoad.Checked;
			}
		}

		private void chkCoverCoolLoad_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCoverCoolLoad == 0) {
				if (this.chkCoverCoolLoad.Checked) {
					this.numCoolLoad.Enabled = false;
					this.numCoolLoadPercentage.Enabled = false;
					this.numCoolLoadPercentage.Value = 100;
				} else {
					this.numCoolLoad.Enabled = true;
					this.numCoolLoadPercentage.Enabled = true;
					this.numCoolLoadPercentage.Value = 100;
				}
				this.product.CoverCoolLoad = this.chkCoverCoolLoad.Checked;
			}
		}

		private void numHeatLoadPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreHeatLoadPercentage == 0) {
				ignoreHeatLoad++;
				this.product.RequestedHeatLoadPercentage = (float)this.numHeatLoadPercentage.Value;
				this.numHeatLoad.Value = (decimal)this.product.RequestedHeatLoad;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.HEAT_LOAD_PERCENTAGE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreHeatLoad--;
			}
		}

		private void numCoolLoadPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreCoolLoadPercentage == 0) {
				ignoreCoolLoad++;
				this.product.RequestedCoolLoadPercentage = (float)this.numCoolLoadPercentage.Value;
				this.numCoolLoad.Value = (decimal)this.product.RequestedCoolLoad;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.COOL_LOAD_PERCENTAGE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreCoolLoad--;
			}
		}

		private void numHeatLoad_ValueChanged(object sender, EventArgs e) {
			if (ignoreHeatLoad == 0) {
				ignoreHeatLoadPercentage++;
				this.product.RequestedHeatLoad = (double)this.numHeatLoad.Value;
				this.numHeatLoadPercentage.Value = (decimal)this.product.RequestedHeatLoadPercentage;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.HEAT_LOAD);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreHeatLoadPercentage--;
			}
		}

		private void numCoolLoad_ValueChanged(object sender, EventArgs e) {
			if (ignoreCoolLoad == 0) {
				ignoreCoolLoadPercentage++;
				this.product.RequestedCoolLoad = (double)this.numCoolLoad.Value;
				this.numCoolLoadPercentage.Value = (decimal)this.product.RequestedCoolLoadPercentage;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.COOL_LOAD);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreCoolLoadPercentage--;
			}
		}

		private void btnFloorConstruction_Click(object sender, EventArgs e) {
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.FloorConstruction, null);
			form.SelectedConstruction = (this.product.Product as ModulKlimaBodenProduct).PlannedFloorConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as ModulKlimaBodenProduct).PlannedFloorConstruction = form.SelectedConstruction;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
					this.UpdateControl(FieldEnum.NONE);
				}
			}
			form.Dispose();
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnInsulationConstruction_Click(object sender, EventArgs e) {
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.InsulationConstruction, null);
			form.SelectedConstruction = (this.product.Product as ModulKlimaBodenProduct).PlannedInsulationConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as ModulKlimaBodenProduct).PlannedInsulationConstruction = form.SelectedConstruction;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
					this.UpdateControl(FieldEnum.NONE);
				}
			}
			form.Dispose();
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnDistributor_Click(object sender, EventArgs e) {
			SelectConnectionForProductForm form = new SelectConnectionForProductForm(this.product, this.product.Product.AssociatedRoom.AssociatedFloor);
			//form.SelectedConnection = (this.product.Product as EurovalProduct).PlannedConnection;
			//if (form.ShowDialog() == DialogResult.OK) {
			//	(this.product.Product as EurovalProduct).PlannedConnection = form.SelectedConnection;
			//}
			form.ShowDialog();

			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
			this.UpdateControl(FieldEnum.COOL_LOAD);
			if (form.DialogResult == DialogResult.OK && this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
			form.Dispose();
		}

		private void connectionPipePanel1_GridContentChanged(object sender) {
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
			this.UpdateControl(FieldEnum.NONE);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnConnectionPipes_Click(object sender, EventArgs e) {
			ConnectionPipesForm form = new ConnectionPipesForm(this.product);
			form.ShowDialog();
			if (form.UnsavedChanges) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.NONE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
			form.Dispose();
		}

		DataGridViewRow newRow = null;
		private void dgvRegisters_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) {
			Console.WriteLine("default values");
			this.newRow = e.Row;
			e.Row.Cells[PlannedProduct.DisplayIndex].Value = this.product;
			e.Row.Cells[heizkreisDataGridViewTextBoxColumn.DisplayIndex].Value = 1;
		}

		HithermRegister deletingRegister = null;
		private void dgvRegisters_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			dgvRegisters.AllowUserToAddRows = false;
			this.deletingRegister = e.Row.DataBoundItem as HithermRegister;
		}

		private void dgvRegisters_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			dgvRegisters.AllowUserToAddRows = true;
			(this.product.Product as HithermProduct).RemoveRegisterFromCircuit(this.deletingRegister);
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
			if (ProjectChanged != null) {
				ProjectChanged(this);
			}
		}

		private void dgvRegisters_UserAddedRow(object sender, DataGridViewRowEventArgs e) {
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
			if (this.product != null && newRow != null && newRow.DataBoundItem is HithermRegister) {
				(this.product.Product as HithermProduct).AddRegisterToCircuit(newRow.DataBoundItem as HithermRegister, (int)newRow.Cells[this.heizkreisDataGridViewTextBoxColumn.Index].Value);
				newRow = null;
			}
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void dgvRegisters_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (this.product != null) {
				this.UpdateControl(FieldEnum.REGISTER);
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void dgvRegisters_SelectionChanged(object sender, EventArgs e) {
			this.UpdateControl(FieldEnum.REGISTER);
		}
		
	}
}
