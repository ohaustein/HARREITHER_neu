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
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_250_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_300_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_50_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_100_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_150_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_200_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_250_10);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermRegister.RegisterTypeEnum.HIT_300_10);
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

				if ((skipFields & FieldEnum.REGISTER) == FieldEnum.NONE) {
					List<HithermRegister> allRegisters = new List<HithermRegister>();
					foreach (HithermCircuit c in hp.PlannedCircuits) {
						foreach (HithermRegister r in c.Registers) {
							allRegisters.Add(r);
						}
					}
					this.hithermRegisterBindingSource.DataSource = allRegisters;
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

		private void numRoomTemperatureBelowHeat_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowHeat == 0) {
				(this.product.Product as ModulKlimaBodenProduct).PlannedRoomTemperatureBelowHeat = (float)this.numRoomTemperatureBelowHeat.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_HEAT);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numRoomTemperatureBelowCool_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowCool == 0) {
				(this.product.Product as ModulKlimaBodenProduct).PlannedRoomTemperatureBelowCool = (float)this.numRoomTemperatureBelowCool.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_COOL);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
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

		private void dgvRegisters_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) {
			e.Row.Cells[PlannedProduct.DisplayIndex].Value = this.product;
			e.Row.Cells[heizkreisDataGridViewTextBoxColumn.DisplayIndex].Value = 1;
		}
		
	}
}
