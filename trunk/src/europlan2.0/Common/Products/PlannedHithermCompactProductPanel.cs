using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class PlannedHithermCompactProductPanel : UserControl, IEditorUserControl {
		private PlannedProduct product = null;

		public PlannedHithermCompactProductPanel() {
			InitializeComponent();

			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par);
			this.cmbType.Items.Add(Product.ProductType.WH);
			this.cmbType.Items.Add(Product.ProductType.DH);
			this.cmbType.Items.Add(Product.ProductType.DSH);
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
			WALLS = 131072,
			TYPE = 262144,
		}


		private string errorMsg = null;

		public void UpdateControl() {
			this.product = this.Tag as PlannedProduct;
			this.tabs.SelectedTab = this.pageInput;
			this.connectionPipePanel.Update(this.product);
			this.chkStellAntriebe.Checked = this.product.Product.StellMotore;
			if (this.product != null) {
				(this.product.Product as HithermCompactProduct).ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
		private int ignoreType = 0;

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
				ignoreType++;

				HithermCompactProduct hcp = this.product.Product as HithermCompactProduct;

				int selectedCircuit = (this.dgvRegisters.SelectedCells.Count > 0 &&
					this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem is HithermCompactRegister) ?
					(this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem as HithermCompactRegister).Heizkreis : -1;

				bool showHeat = this.product.Product.AssociatedRoom.HeatLoad > 0 || this.product.Product.AssociatedRoom.CoolLoad <= 0;
				bool showCool = this.product.Product.AssociatedRoom.CoolLoad > 0;
				bool showHeatCircuit = selectedCircuit >= 0 && showHeat;
				bool showCoolCircuit = selectedCircuit >= 0 && showCool;

				lblQHeat.Visible = showHeat;
				lblQHeatUnit.Visible = showHeat;
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
				lblQCoolDiff.Visible = showCool;
				lblQCoolDiffUnit.Visible = showCool;
				lblQCoolRest.Visible = showCool;
				lblQCoolRestUnit.Visible = showCool;
				lblAvgqCool.Visible = showCoolCircuit;
				lblAvgqCoolUnit.Visible = showCoolCircuit;
				lblDurchflussCool.Visible = showCoolCircuit;
				lblDurchflussCoolUnit.Visible = showCoolCircuit;
				lblDruckverlustCool.Visible = showCoolCircuit;
				lblDruckverlustCoolUnit.Visible = showCoolCircuit;

				if ((skipFields & FieldEnum.TYPE) == FieldEnum.NONE) {
					this.cmbType.SelectedItem = hcp.HithermCompactType;
				}

				bool showArea = hcp.HithermCompactType == Product.ProductType.DH || hcp.HithermCompactType == Product.ProductType.FBH;
				if (showArea) {
					this.grpPowerArea.Height = 121;
				} else {
					this.grpPowerArea.Height = 98;
				}
				this.lblAreaTxt.Visible = showArea;
				this.numArea.Visible = showArea;
				this.lblAreaUnit.Visible = showArea;
				this.numAreaPercentage.Visible = showArea;
				this.lblAreaPercentage.Visible = showArea;

				this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
				this.numHeatLoadPercentage.MaxValue = (decimal)(hcp.AssociatedRoom.NormalizedHeatLoad <= 0 ? 0 : this.product.NecessaryHeatLoad * 100 / hcp.AssociatedRoom.NormalizedHeatLoad);

				switch (hcp.HithermCompactType) {
					case Product.ProductType.FBH:
						this.numArea.MaxValue = (decimal)hcp.AvailableFloorArea;
						this.numAreaPercentage.MaxValue = (decimal)(hcp.AvailableFloorArea * 100 / hcp.AssociatedRoom.Area);
						break;
					case Product.ProductType.DH:
						this.numArea.MaxValue = (decimal)hcp.AvailableCeilingArea;
						this.numAreaPercentage.MaxValue = (decimal)(hcp.AvailableCeilingArea * 100 / hcp.AssociatedRoom.Area);
						break;
					default:
						//this.numArea.MaxValue = (decimal)0;
						//this.numAreaPercentage.MaxValue = (decimal)0;
						break;
				}

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
				this.numCoolLoadPercentage.MaxValue = (decimal)(hcp.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / hcp.AssociatedRoom.NormalizedCoolLoad);
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
				float plannedArea = (float)(this.product.PlannedArea.HasValue ? Math.Round(this.product.PlannedArea.Value, 2) : 0);
				if ((skipFields & FieldEnum.AREA) == FieldEnum.NONE) {
					if (hcp.HithermCompactType == Product.ProductType.FBH) {
						this.numArea.Value = Math.Round((decimal)plannedArea, 2);
					} else if (hcp.HithermCompactType == Product.ProductType.DH) {
						this.numArea.Value = Math.Round((decimal)plannedArea, 2);
					}
				}
				if ((skipFields & FieldEnum.AREA_PERCENTAGE) == FieldEnum.NONE) {
					if (hcp.HithermCompactType == Product.ProductType.FBH) {
						if (hcp.AssociatedRoom.Area <= 0) {
							this.numAreaPercentage.Value = 100;
						} else {
							this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / hcp.AssociatedRoom.Area), 2);
						}
					} else if (hcp.HithermCompactType == Product.ProductType.DH) {
						if (hcp.AssociatedRoom.Area <= 0) {
							this.numAreaPercentage.Value = 100;
						} else {
							this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / hcp.AssociatedRoom.Area), 2);
						}
					}
				}

				if ((skipFields & FieldEnum.REGISTER) == FieldEnum.NONE) {
					List<HithermCompactRegister> allRegisters = new List<HithermCompactRegister>();
					foreach (HithermCompactCircuit c in hcp.PlannedCircuits) {
						foreach (HithermCompactRegister r in c.Registers) {
							allRegisters.Add(r);
						}
					}
					this.hithermCompactRegisterBindingSource.DataSource = allRegisters;
				}

				//    // General
				double qDiffHeat = this.product.PlannedHeatLoad - this.product.RequestedHeatLoad;
				double qDiffCool = this.product.PlannedCoolLoad - this.product.RequestedCoolLoad;

				lblRest.Text = "Rest (" + this.product.Product.AssociatedRoom.ToString() + ")";
				lblQHeat.Text = Math.Round(this.product.PlannedHeatLoad, 0).ToString();
				lblQHeatDiff.Text = Math.Round(qDiffHeat, 0).ToString();
				lblQHeatRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenHeatLoad, 0).ToString();
				lblQCool.Text = Math.Round(this.product.PlannedCoolLoad, 0).ToString();
				lblQCoolDiff.Text = (qDiffCool > 0 ? "+" : "") + Math.Round(qDiffCool, 0).ToString();
				lblQCoolRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenCoolLoad, 0).ToString();
				lblCoveredArea.Text = Math.Round(hcp.PlannedWallArea, 2).ToString();
				lblNecessaryWaermestromdichte.Text = (hcp.PlannedWallArea > 0) ? Math.Round(this.product.RequestedHeatLoad / hcp.PlannedWallArea, 2).ToString() : "--";
				lblNecessaryArea.Text = (hcp.PlannedHeatLoad > 0 && hcp.PlannedWallArea > 0) ? Math.Round(this.product.RequestedHeatLoad / (hcp.PlannedHeatLoad / hcp.PlannedWallArea), 2).ToString() : "--";
				if (selectedCircuit >= 0) {
					HithermCompactCircuit hc = hcp.GetCircuitForRegister(this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem as HithermCompactRegister);
					if (hc != null) {
						lblHk.Text = "Heizkreis " + selectedCircuit.ToString() + ":";
						lblAvgqHeat.Text = Math.Round(hc.C_QHeatPerSqm, 2).ToString();
						lblDurchflussHeat.Text = Math.Round(hc.C_DurchflussHeat, 2).ToString();
						lblDruckverlustHeat.Text = Math.Round(hc.C_DruckverlustHeat, 2).ToString();
						//lblTempHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_FloorTempHeat, 2).ToString();
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
					}
				} else {
				}

				if (hcp.PlannedConnection == null) {
					this.txtDistributor.Text = "";
				} else {
					this.txtDistributor.Text = hcp.PlannedConnection.ToString();
				}

				if ((skipFields & FieldEnum.WALLS) == FieldEnum.NONE) {
					this.hithermWallGrid1.Walls = Project.Instance.HithermCompactWalls;
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
				ignoreType--;
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
						pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
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
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
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
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
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

			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.COOL_LOAD);
			if (form.DialogResult == DialogResult.OK && this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
			form.Dispose();
		}

		private void connectionPipePanel1_GridContentChanged(object sender) {
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.NONE);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnConnectionPipes_Click(object sender, EventArgs e) {
			ConnectionPipesForm form = new ConnectionPipesForm(this.product);
			form.ShowDialog();
			if (form.UnsavedChanges) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
			e.Row.Cells[plannedProductDataGridViewTextBoxColumn.Index].Value = this.product;
			e.Row.Cells[heizkreisDataGridViewTextBoxColumn.Index].Value = 1;
			if (Project.Instance.SerializeableHithermCompactWalls.Count > 0) {
				e.Row.Cells[wallDataGridViewTextBoxColumn.Index].Value = Project.Instance.SerializeableHithermCompactWalls[0];
			} else if (Project.Instance.HithermCompactWalls.Count > 0) {
				e.Row.Cells[wallDataGridViewTextBoxColumn.Index].Value = Project.Instance.HithermCompactWalls[0];
			}
		}

		HithermCompactRegister deletingRegister = null;
		private void dgvRegisters_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			dgvRegisters.AllowUserToAddRows = false;
			this.deletingRegister = e.Row.DataBoundItem as HithermCompactRegister;
		}

		private void dgvRegisters_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			dgvRegisters.AllowUserToAddRows = true;
			(this.product.Product as HithermCompactProduct).RemoveRegisterFromCircuit(this.deletingRegister);
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.REGISTER);
			if (ProjectChanged != null) {
				ProjectChanged(this);
			}
		}

		private void dgvRegisters_UserAddedRow(object sender, DataGridViewRowEventArgs e) {
			//this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			//this.errorMsg = this.product.Product.LastErrorMessage;
			//this.UpdateControl(FieldEnum.REGISTER);
			if (this.product != null && newRow != null && newRow.DataBoundItem is HithermCompactRegister) {
				(this.product.Product as HithermCompactProduct).AddRegisterToCircuit(newRow.DataBoundItem as HithermCompactRegister, (int)newRow.Cells[this.heizkreisDataGridViewTextBoxColumn.Index].Value);
				newRow = null;
			}
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void dgvRegisters_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (this.product != null) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.REGISTER);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void dgvRegisters_SelectionChanged(object sender, EventArgs e) {
			if (this.ignoreRegisters == 0) {
				this.UpdateControl(FieldEnum.REGISTER);
			}
		}

		private void chkStellAntriebe_CheckedChanged(object sender, EventArgs e) {
			this.product.Product.StellMotore = this.chkStellAntriebe.Checked;
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void dgvRegisters_CellEnter(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex == wallDataGridViewTextBoxColumn.Index && e.RowIndex >= 0) {
				Rectangle rect = dgvRegisters.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
				btnSelectWall.Location = new Point(rect.X + rect.Width - btnSelectWall.Width - 1, rect.Y);
				btnSelectWall.Height = rect.Height - 1;
				btnSelectWall.Show();
			}
		}

		private void dgvRegisters_CellLeave(object sender, DataGridViewCellEventArgs e) {
			btnSelectWall.Hide();
		}

		private void btnSelectWall_Click(object sender, EventArgs e) {
			SelectHithermWallForm form = new SelectHithermWallForm();
			form.SelectedWall = dgvRegisters.CurrentCell.Value as HithermWall;
			if (form.ShowDialog().Equals(DialogResult.OK)) {
				HithermWall wall = form.SelectedWall;
				DataGridViewCell cell = dgvRegisters.CurrentCell;
				if (cell.Value != wall) {
					cell.Value = wall;
					int col = dgvRegisters.SelectedCells.Count > 0 ? dgvRegisters.SelectedCells[0].ColumnIndex : -1;
					int row = dgvRegisters.SelectedCells.Count > 0 ? dgvRegisters.SelectedCells[0].RowIndex : -1;
					hithermCompactRegisterBindingSource.ResetBindings(false);
					if (col > -1) {
						dgvRegisters.Rows[row].Cells[col].Selected = true;
					}
				}
			}
			form.Dispose();
		}

		private void numArea_ValueChanged(object sender, EventArgs e) {
			if (ignoreArea == 0) {
				ignoreAreaPercentage++;
				if (this.product.Product.Type == Product.ProductType.FBH) {
					(this.product.Product as HithermCompactProduct).PlannedFloorArea = (float)this.numArea.Value;
					this.numAreaPercentage.Value = (decimal)(this.product.Product as HithermCompactProduct).PlannedFloorAreaPercentage;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
					this.UpdateControl(FieldEnum.AREA);
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
				} else if (this.product.Product.Type == Product.ProductType.DH) {
					(this.product.Product as HithermCompactProduct).PlannedCeilingArea = (float)this.numArea.Value;
					this.numAreaPercentage.Value = (decimal)(this.product.Product as HithermCompactProduct).PlannedCeilingAreaPercentage;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
					this.UpdateControl(FieldEnum.AREA);
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
				}
				ignoreAreaPercentage--;
			}
		}

		private void numAreaPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaPercentage == 0) {
				ignoreArea++;
				if (this.product.Product.Type == Product.ProductType.FBH) {
					(this.product.Product as HithermCompactProduct).PlannedFloorAreaPercentage = (float)this.numAreaPercentage.Value;
					this.numArea.Value = (decimal)this.product.PlannedArea;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
					this.UpdateControl(FieldEnum.AREA_PERCENTAGE);
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
				} else if (this.product.Product.Type == Product.ProductType.DH) {
					(this.product.Product as HithermCompactProduct).PlannedCeilingAreaPercentage = (float)this.numAreaPercentage.Value;
					this.numArea.Value = (decimal)this.product.PlannedArea;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
					this.UpdateControl(FieldEnum.AREA_PERCENTAGE);
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
				}
				ignoreArea--;
			}
		}

		private void cmbType_SelectedValueChanged(object sender, EventArgs e) {
			if (ignoreType == 0) {
				if (this.cmbType.SelectedItem is Product.ProductType && this.product.Product.Type != (Product.ProductType)this.cmbType.SelectedItem) {
					(this.product.Product as HithermCompactProduct).HithermCompactType = (Product.ProductType)this.cmbType.SelectedItem;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
					this.UpdateControl(FieldEnum.TYPE);
					if (this.ProjectStructureChanged != null) {
						this.ProjectStructureChanged(this);
					}
				}
			}
		}

		private void hithermWallGrid1_WallChanged(object sender, HithermWallGrid.WallEventArgs e) {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void dgvRegisters_DataError(object sender, DataGridViewDataErrorEventArgs e) {
			Console.WriteLine(e.ToString());
		}
	}
}
