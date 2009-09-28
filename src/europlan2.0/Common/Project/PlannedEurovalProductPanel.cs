using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class PlannedEurovalProductPanel : UserControl, IEditorUserControl {
		private PlannedProduct product = null;

		private class LayDistanceItem {
			public Nullable<EurovalProduct.LayDistance> layDistance;
			public string name;

			public LayDistanceItem(Nullable<EurovalProduct.LayDistance> layDistance, string name) {
				this.layDistance = layDistance;
				this.name = name;
			}

			public override string ToString() {
				return this.name;
			}

			public override bool Equals(object obj) {
				return obj is LayDistanceItem && (obj as LayDistanceItem).layDistance == this.layDistance;
			}

			public override int GetHashCode() {
				return (this.layDistance == null ? 0 : this.layDistance.GetHashCode());
			}
		}

		private class RimTypeItem {
			public Nullable<EurovalProduct.RimType> rimType;
			public string name;

			public RimTypeItem(Nullable<EurovalProduct.RimType> layDistance, string name) {
				this.rimType = layDistance;
				this.name = name;
			}

			public override string ToString() {
				return this.name;
			}

			public override bool Equals(object obj) {
				return obj is RimTypeItem && (obj as RimTypeItem).rimType == this.rimType;
			}

			public override int GetHashCode() {
				return (this.rimType == null ? 0 : this.rimType.GetHashCode());
			}
		}

		public PlannedEurovalProductPanel() {
			InitializeComponent();

			this.cmbLayDistance.Items.Clear();
			this.cmbLayDistance.Items.Add(new LayDistanceItem(null, "Automatisch"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.LayDistance.EV35, "EV35"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.LayDistance.EV30, "EV30"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.LayDistance.EV25, "EV25"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.LayDistance.EV20, "EV20"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.LayDistance.EV15, "EV15"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.LayDistance.EV10, "EV10"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.LayDistance.EV5, "EV5"));

			this.cmbRimType.Items.Clear();
			this.cmbRimType.Items.Add(new RimTypeItem(null, "Automatisch"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV15_60, "EV15/60"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV15_120, "EV15/120"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV15_180, "EV15/180"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV10_55, "EV10/55"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV10_110, "EV10/110"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV10_165, "EV10/165"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV5_40, "EV5/40"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV5_80, "EV5/80"));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.RimType.EV5_120, "EV5/120"));
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
			ROOM_TEMERATURE_BELOW = 256,
			RIM_LENGTH = 512,
			CORNERS = 1024,
			LAY_DISTANCE = 2048,
			RIM_TYPE = 4096
		}

		public void UpdateControl() {
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
		private int ignoreRoomTemperatureBelow = 0;
		private int ignoreRim = 0;
		private int ignoreCorners = 0;
		private int ignoreLayDistance = 0;
		private int ignoreRimType = 0;

		private void UpdateControl(FieldEnum skipFields) {
			this.product = this.Tag as PlannedProduct;
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
				ignoreRoomTemperatureBelow++;
				ignoreRim++;
				ignoreCorners++;
				ignoreLayDistance++;
				ignoreRim++;

				EurovalProduct evProduct = this.product.Product as EurovalProduct;
				this.numArea.MaxValue = (decimal)evProduct.AvailableFloorArea;
				this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
				this.numAreaUnheated.MaxValue = (decimal)this.product.PlannedArea;
				if (this.product.NecessaryHeatLoad > 0) {
					this.chkCoverHeatLoad.Enabled = true;
					this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
					this.numCoolLoad.MaxValue = (decimal)this.product.NecessaryCoolLoad;
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
						this.numHeatLoadPercentage.Value = Math.Round((decimal)(this.product.RequestedHeatLoad * 100 / this.product.NecessaryHeatLoad), 2);
					}
				} else {
					this.numHeatLoad.Enabled = false;
					this.numHeatLoad.Text = "";
					this.numHeatLoadPercentage.Enabled = false;
					this.numHeatLoadPercentage.Text = "";
					this.chkCoverHeatLoad.Enabled = false;
					this.chkCoverHeatLoad.Checked = false;
				}
				if (this.product.NecessaryCoolLoad > 0) {
					// TODO checkbox
					if ((skipFields & FieldEnum.COOL_LOAD) == FieldEnum.NONE) {
						this.numCoolLoad.Value = Math.Round((decimal)this.product.RequestedCoolLoad, 2);
					}
					if ((skipFields & FieldEnum.COOL_LOAD_PERCENTAGE) == FieldEnum.NONE) {
						this.numCoolLoadPercentage.Value = Math.Round((decimal)(this.product.RequestedCoolLoad * 100 / this.product.NecessaryCoolLoad), 2);
					}
				} else {
					this.numCoolLoad.Enabled = false;
					this.numCoolLoad.Text = "";
					this.numCoolLoadPercentage.Enabled = false;
					this.numCoolLoadPercentage.Text = "";
					this.chkCoverCoolLoad.Enabled = false;
					this.chkCoverCoolLoad.Checked = false;
				}
				this.lblHeatLoadTotal.Text = "(" + Math.Round(this.product.NecessaryHeatLoad, 2) + " W)";
				this.lblCoolLoadTotal.Text = "(" + Math.Round(this.product.NecessaryCoolLoad, 2) + " W)";
				float plannedArea = (float)(this.product.PlannedArea.HasValue ? Math.Round(this.product.PlannedArea.Value, 2) : 0);
				if ((skipFields & FieldEnum.AREA) == FieldEnum.NONE) {
					this.numArea.Value = Math.Round((decimal)plannedArea, 2);
				}
				if ((skipFields & FieldEnum.AREA_PERCENTAGE) == FieldEnum.NONE) {
					if (evProduct.AvailableFloorArea <= 0) {
						this.numAreaPercentage.Value = 100;
					} else {
						this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / evProduct.AvailableFloorArea), 2);
					}
				}
				if ((skipFields & FieldEnum.AREA_REDUCED) == FieldEnum.NONE) {
					this.numAreaReduced.Value = Math.Round((decimal)evProduct.PlannedAreaReduced, 2);
				}
				if ((skipFields & FieldEnum.AREA_UNHEATED) == FieldEnum.NONE) {
					this.numAreaUnheated.Value = Math.Round((decimal)evProduct.PlannedAreaUnheated, 2);
				}
				this.txtFloorConstruction.Text = (evProduct.PlannedFloorConstruction == null ? "" : evProduct.PlannedFloorConstruction.Id + ": " + evProduct.PlannedFloorConstruction.Name);
				this.txtInsulationConstruction.Text = (evProduct.PlannedInsulationConstruction == null ? "" : evProduct.PlannedInsulationConstruction.Name);
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW) == FieldEnum.NONE) {
					this.numRoomTemperatureBelow.Value = Math.Round((decimal)evProduct.PlannedRoomTemperatureBelow, 2);
				}
				if ((skipFields & FieldEnum.RIM_LENGTH) == FieldEnum.NONE) {
					this.numRim.Value = Math.Round((decimal)evProduct.PlannedRim, 2);
				}
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW) == FieldEnum.NONE) {
					this.numCorners.Value = (decimal)evProduct.PlannedCornersRim;
				}

				if ((skipFields & FieldEnum.LAY_DISTANCE) == FieldEnum.NONE) {
					this.cmbLayDistance.SelectedItem = new LayDistanceItem(evProduct.RequestedLayDistance, "");
				}
				if ((skipFields & FieldEnum.RIM_TYPE) == FieldEnum.NONE) {
					this.cmbRimType.SelectedItem = new RimTypeItem(evProduct.RequestedRimType, "");
				}

				// general
				this.lblQSollHeat.Text = Math.Round(this.product.RequestedHeatLoad, 2).ToString();
				this.lblQSollCool.Text = Math.Round(this.product.RequestedCoolLoad, 2).ToString();
				this.lblQkSollHeat.Text = Math.Round(this.product.RequestedHeatLoadPerSqM, 2).ToString();
				this.lblQkSollCool.Text = this.product.PlannedArea.HasValue ? Math.Round(this.product.RequestedCoolLoad / this.product.PlannedArea.Value, 2).ToString() : "0";
				this.lblQfbhHeat.Text = Math.Round(this.product.PlannedHeatLoad, 2).ToString();
				this.lblQfbhCool.Text = Math.Round(this.product.PlannedCoolLoad, 2).ToString();
				double qRestHeat = this.product.PlannedHeatLoad - this.product.RequestedHeatLoad;
				double qRestCool = this.product.PlannedCoolLoad - this.product.RequestedCoolLoad;
				this.lblQRestHeat.Text = (qRestHeat > 0 ? "+" : "") + Math.Round(qRestHeat, 2).ToString();
				this.lblQRestCool.Text = (qRestCool > 0 ? "+" : "") + Math.Round(qRestCool, 2).ToString();

				// randzone
				if (evProduct.PlannedRimType.HasValue) {
					switch (evProduct.PlannedLayDistanceRim) {
						case EurovalProduct.LayDistance.EV5:
							this.lblRimVaHeat.Text = "EV5";
							this.lblRimVaCool.Text = "EV5";
							break;
						case EurovalProduct.LayDistance.EV10:
							this.lblRimVaHeat.Text = "EV10";
							this.lblRimVaCool.Text = "EV10";
							break;
						case EurovalProduct.LayDistance.EV15:
							this.lblRimVaHeat.Text = "EV15";
							this.lblRimVaCool.Text = "EV15";
							break;
						case EurovalProduct.LayDistance.EV20:
							this.lblRimVaHeat.Text = "EV20";
							this.lblRimVaCool.Text = "EV20";
							break;
						case EurovalProduct.LayDistance.EV25:
							this.lblRimVaHeat.Text = "EV25";
							this.lblRimVaCool.Text = "EV25";
							break;
						case EurovalProduct.LayDistance.EV30:
							this.lblRimVaHeat.Text = "EV30";
							this.lblRimVaCool.Text = "EV30";
							break;
						case EurovalProduct.LayDistance.EV35:
							this.lblRimVaHeat.Text = "EV35";
							this.lblRimVaCool.Text = "EV35";
							break;
						default:
							this.lblRimVaHeat.Text = "???";
							this.lblRimVaCool.Text = "???";
							break;
					}
					this.lblRimBHeat.Text = evProduct.PlannedRimWidth.ToString();
					this.lblRimBCool.Text = evProduct.PlannedRimWidth.ToString();
					this.lblRimTfbHeat.Text = "???";
					this.lblRimTfbCool.Text = "???";
					this.lblRimQHeat.Text = evProduct.PlannedHeatLoadRim.ToString();
					this.lblRimQCool.Text = "0";
				} else {
					this.lblRimVaHeat.Text = "";
					this.lblRimVaCool.Text = "";
					this.lblRimBHeat.Text = "";
					this.lblRimBCool.Text = "";
					this.lblRimTfbHeat.Text = "";
					this.lblRimTfbCool.Text = "";
					this.lblRimQHeat.Text = "";
					this.lblRimQCool.Text = "";
				}

				// aufenthaltszone
				if (evProduct.PlannedLayDistance.HasValue) {
					switch (evProduct.PlannedLayDistance) {
						case EurovalProduct.LayDistance.EV5:
							this.lblResidenceVaHeat.Text = "EV5";
							this.lblResidenceVaCool.Text = "EV5";
							break;
						case EurovalProduct.LayDistance.EV10:
							this.lblResidenceVaHeat.Text = "EV10";
							this.lblResidenceVaCool.Text = "EV10";
							break;
						case EurovalProduct.LayDistance.EV15:
							this.lblResidenceVaHeat.Text = "EV15";
							this.lblResidenceVaCool.Text = "EV15";
							break;
						case EurovalProduct.LayDistance.EV20:
							this.lblResidenceVaHeat.Text = "EV20";
							this.lblResidenceVaCool.Text = "EV20";
							break;
						case EurovalProduct.LayDistance.EV25:
							this.lblResidenceVaHeat.Text = "EV25";
							this.lblResidenceVaCool.Text = "EV25";
							break;
						case EurovalProduct.LayDistance.EV30:
							this.lblResidenceVaHeat.Text = "EV30";
							this.lblResidenceVaCool.Text = "EV30";
							break;
						case EurovalProduct.LayDistance.EV35:
							this.lblResidenceVaHeat.Text = "EV35";
							this.lblResidenceVaCool.Text = "EV35";
							break;
						default:
							this.lblResidenceVaHeat.Text = "???";
							this.lblResidenceVaCool.Text = "???";
							break;
					}
					this.lblResidenceAHeat.Text = evProduct.PlannedAreaResidence.ToString();
					this.lblResidenceACool.Text = evProduct.PlannedAreaResidence.ToString();
					this.lblResidenceTfbHeat.Text = "???";
					this.lblResidenceTfbCool.Text = "???";
					this.lblResidenceQHeat.Text = evProduct.PlannedHeatLoadResidence.ToString();
					this.lblResidenceQCool.Text = "0";
				} else {
					this.lblResidenceVaHeat.Text = "";
					this.lblResidenceVaCool.Text = "";
					this.lblResidenceAHeat.Text = "";
					this.lblResidenceACool.Text = "";
					this.lblResidenceTfbHeat.Text = "";
					this.lblResidenceTfbCool.Text = "";
					this.lblResidenceQHeat.Text = "";
					this.lblResidenceQCool.Text = "";
				}

				// anbindung
				this.lblConnectionAHeat.Text = "";
				this.lblConnectionACool.Text = "";
				this.lblConnectionQHeat.Text = "";
				this.lblConnectionQCool.Text = "";

				// heizkreis
				this.lblCircuitCountHeat.Text = "";
				this.lblCircuitCountCool.Text = "";
				this.lblPipeLengthHeat.Text = Math.Round(evProduct.PlannedPipeLength, 1).ToString();
				this.lblPipeLengthCool.Text = Math.Round(evProduct.PlannedPipeLength, 1).ToString();
				this.lblMhHeat.Text = "";
				this.lblMhCool.Text = "";
				this.lblDeltaPHeat.Text = Math.Round(evProduct.PlannedDeltaRho, 1).ToString();
				this.lblDeltaPCool.Text = "";
				this.lblSpreizungHeat.Text = "5";
				this.lblSpreizungCool.Text = "0";

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
				ignoreRoomTemperatureBelow--;
				ignoreRim--;
				ignoreCorners--;
				ignoreLayDistance--;
				ignoreRim--;
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
						pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad);
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

		private void numHeatLoadPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreHeatLoadPercentage == 0) {
				ignoreHeatLoad++;
				this.product.RequestedHeatLoadPercentage = (float)this.numHeatLoadPercentage.Value;
				this.numHeatLoad.Value = (decimal)this.product.RequestedHeatLoad;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.HEAT_LOAD_PERCENTAGE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreHeatLoad--;
			}
		}

		private void numHeatLoad_ValueChanged(object sender, EventArgs e) {
			if (ignoreHeatLoad == 0) {
				ignoreHeatLoadPercentage++;
				this.product.RequestedHeatLoad = (double)this.numHeatLoad.Value;
				this.numHeatLoadPercentage.Value = (decimal)this.product.RequestedHeatLoadPercentage;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.HEAT_LOAD);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreHeatLoadPercentage--;
			}
		}

		private void numAreaPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaPercentage == 0) {
				ignoreArea++;
				(this.product.Product as EurovalProduct).PlannedFloorAreaPercentage = (float)this.numAreaPercentage.Value;
				this.numArea.Value = (decimal)this.product.PlannedArea;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.AREA_PERCENTAGE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreArea--;
			}
		}

		private void numArea_ValueChanged(object sender, EventArgs e) {
			if (ignoreArea == 0) {
				ignoreAreaPercentage++;
				(this.product.Product as EurovalProduct).PlannedFloorArea = (float)this.numArea.Value;
				this.numAreaPercentage.Value = (decimal)(this.product.Product as EurovalProduct).PlannedFloorAreaPercentage;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.AREA);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreAreaPercentage--;
			}
		}

		private void numAreaReduced_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaReduced == 0) {
				ignoreAreaUnheated++;
				EurovalProduct evProduct = this.product.Product as EurovalProduct;
				evProduct.PlannedAreaReduced = (float)this.numAreaReduced.Value;
				if (evProduct.PlannedAreaReduced + evProduct.PlannedAreaUnheated > evProduct.PlannedFloorArea) {
					evProduct.PlannedAreaUnheated = evProduct.PlannedFloorArea - evProduct.PlannedAreaReduced;
				}
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.AREA_REDUCED);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreAreaUnheated--;
			}
		}

		private void numAreaUnheated_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaUnheated == 0) {
				ignoreAreaReduced++;
				EurovalProduct evProduct = this.product.Product as EurovalProduct;
				evProduct.PlannedAreaUnheated = (float)this.numAreaUnheated.Value;
				if (evProduct.PlannedAreaReduced + evProduct.PlannedAreaUnheated > evProduct.PlannedFloorArea) {
					evProduct.PlannedAreaReduced = evProduct.PlannedFloorArea - evProduct.PlannedAreaUnheated;
				}
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.AREA_UNHEATED);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreAreaReduced--;
			}
		}

		private void btnFloorConstruction_Click(object sender, EventArgs e) {
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.FloorConstruction);
			form.SelectedConstruction = (this.product.Product as EurovalProduct).PlannedFloorConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as EurovalProduct).PlannedFloorConstruction = form.SelectedConstruction;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
					this.UpdateControl(FieldEnum.NONE);
				}
			}
			form.Dispose();
		}

		private void btnInsulationConstruction_Click(object sender, EventArgs e) {
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.InsulationConstruction);
			form.SelectedConstruction = (this.product.Product as EurovalProduct).PlannedInsulationConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as EurovalProduct).PlannedInsulationConstruction = form.SelectedConstruction;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
					this.UpdateControl(FieldEnum.NONE);
				}
			}
			form.Dispose();
		}

		private void numRoomTemperatureBelow_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelow == 0) {
				(this.product.Product as EurovalProduct).PlannedRoomTemperatureBelow = (float)this.numRoomTemperatureBelow.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numRim_ValueChanged(object sender, EventArgs e) {
			if (ignoreRim == 0) {
				(this.product.Product as EurovalProduct).PlannedRim = (float)this.numRim.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.RIM_LENGTH);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numCorners_ValueChanged(object sender, EventArgs e) {
			if (ignoreCorners == 0) {
				(this.product.Product as EurovalProduct).PlannedCornersRim = (int)this.numCorners.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.CORNERS);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void cmbLayDistance_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreLayDistance == 0) {
				(this.product.Product as EurovalProduct).RequestedLayDistance = (this.cmbLayDistance.SelectedItem as LayDistanceItem).layDistance;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.LAY_DISTANCE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void cmbRimType_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreRimType == 0) {
				(this.product.Product as EurovalProduct).RequestedRimType = (this.cmbRimType.SelectedItem as RimTypeItem).rimType;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad);
				this.UpdateControl(FieldEnum.RIM_TYPE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}

		}
	}
}
