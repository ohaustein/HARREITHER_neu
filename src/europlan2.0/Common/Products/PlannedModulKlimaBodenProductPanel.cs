using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class PlannedModulKlimaBodenProductPanel : UserControl, IEditorUserControl {
		private PlannedProduct product = null;

		public PlannedModulKlimaBodenProductPanel() {
			InitializeComponent();
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
			LENGTH_VERBINDUNGEN = 2048,
			//LAY_DISTANCE = 4096,
			//RIM_TYPE = 8192,
			//CALCULATION_TYPE = 16384,
			CIRCUITS = 32768,
			ROWS = 65536
		}


		private string errorMsg = null;

		public void UpdateControl() {
			this.product = this.Tag as PlannedProduct;
			this.tabs.SelectedTab = this.pageInput;
			this.connectionPipePanel.Update(this.product);
			if (this.product != null) {
				(this.product.Product as ModulKlimaBodenProduct).ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out errorMsg);
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
		private int ignoreRows = 0;
		private int ignoreModules = 0;
		private int ignoreLengthVerbindungen = 0;

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
				ignoreRows++;
				ignoreModules++;
				ignoreLengthVerbindungen++;

				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;

				bool showHeat = this.product.RequestedHeatLoad > 0;
				bool showCool = this.product.RequestedCoolLoad > 0;
			//    bool showRim = evProduct.PlannedAreaRim > 0;
			//    bool showResidence = true;

				lblQHeat.Visible = showHeat;
				lblQHeatDiff.Visible = showHeat;
				lblQHeatRest.Visible = showHeat;
				lblAvgqHeat.Visible = showHeat;
				lblDurchflussHeat.Visible = showHeat;
				lblDruckverlustHeat.Visible = showHeat;
				lblTempHeat.Visible = showHeat;
				lblQCool.Visible = showCool;
				lblQCoolDiff.Visible = showCool;
				lblQCoolRest.Visible = showCool;
				lblAvgqCool.Visible = showCool;
				lblDurchflussCool.Visible = showCool;
				lblDruckverlustCool.Visible = showCool;
				lblTempCool.Visible = showCool;

			//    this.lblQSollHeat.Visible = showHeat;
			//    this.lblQkSollHeat.Visible = showHeat;
			//    this.lblQfbhHeat.Visible = showHeat;
			//    this.lblQRestHeat.Visible = showHeat;
			//    this.lblRimVaHeat.Visible = showHeat && showRim;
			//    this.lblRimBHeat.Visible = showHeat && showRim;
			//    this.lblRimTfbHeat.Visible = showHeat && showRim;
			//    this.lblRimQHeat.Visible = showHeat && showRim;
			//    this.lblResidenceVaHeat.Visible = showHeat && showResidence;
			//    this.lblResidenceAHeat.Visible = showHeat && showResidence;
			//    this.lblResidenceTfbHeat.Visible = showHeat && showResidence;
			//    this.lblResidenceQHeat.Visible = showHeat && showResidence;
			//    this.lblConnectionAHeat.Visible = showHeat;
			//    this.lblConnectionQHeat.Visible = showHeat;
			//    this.lblCircuitCountHeat.Visible = showHeat;
			//    this.lblPipeLengthHeat.Visible = showHeat;
			//    this.lblMhHeat.Visible = showHeat;
			//    this.lblDeltaPHeat.Visible = showHeat;
			//    this.lblSpreizungHeat.Visible = showHeat;

			//    this.lblQSollCool.Visible = showCool;
			//    this.lblQkSollCool.Visible = showCool;
			//    this.lblQfbhCool.Visible = showCool;
			//    this.lblQRestCool.Visible = showCool;
			//    this.lblRimVaCool.Visible = showCool && showRim;
			//    this.lblRimBCool.Visible = showCool && showRim;
			//    this.lblRimTfbCool.Visible = showCool && showRim;
			//    this.lblRimQCool.Visible = showCool && showRim;
			//    this.lblResidenceVaCool.Visible = showCool && showResidence;
			//    this.lblResidenceACool.Visible = showCool && showResidence;
			//    this.lblResidenceTfbCool.Visible = showCool && showResidence;
			//    this.lblResidenceQCool.Visible = showCool && showResidence;
			//    this.lblConnectionACool.Visible = showCool;
			//    this.lblConnectionQCool.Visible = showCool;
			//    this.lblCircuitCountCool.Visible = showCool;
			//    this.lblPipeLengthCool.Visible = showCool;
			//    this.lblMhCool.Visible = showCool;
			//    this.lblDeltaPCool.Visible = showCool;
			//    this.lblSpreizungCool.Visible = showCool;

			//    this.rbCalculateHeat.Enabled = this.product.RequestedHeatLoad > 0;
			//    this.rbCalculateCool.Enabled = this.product.RequestedCoolLoad > 0;
			//    this.rbCalculateBoth.Enabled = this.product.RequestedHeatLoad > 0 && this.product.RequestedCoolLoad > 0;
			//    this.numCorners.Enabled = evProduct.PlannedRimLength > 0;
			//    this.cmbRimType.Enabled = evProduct.PlannedAreaRim > 0;

			//    // disable the following controls if the product is a connection
			//    this.numRim.Enabled = !evProduct.PlannedProductIsConnection;
			//    this.numCorners.Enabled = this.numCorners.Enabled && !evProduct.PlannedProductIsConnection;
			//    this.rbCalculateHeat.Enabled = this.rbCalculateHeat.Enabled && !evProduct.PlannedProductIsConnection;
			//    this.rbCalculateCool.Enabled = this.rbCalculateCool.Enabled && !evProduct.PlannedProductIsConnection;
			//    this.rbCalculateBoth.Enabled = this.rbCalculateBoth.Enabled && !evProduct.PlannedProductIsConnection;
			//    this.btnDistributor.Enabled = !evProduct.PlannedProductIsConnection;
			//    this.cmbLayDistance.Enabled = !evProduct.PlannedProductIsConnection;
			//    this.cmbRimType.Enabled = this.cmbRimType.Enabled && !evProduct.PlannedProductIsConnection;
			//    this.cmbCircuits.Enabled = !evProduct.PlannedProductIsConnection;

			    this.numArea.MaxValue = (decimal)mbProduct.AvailableFloorArea;
			//    this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
			    this.numAreaUnheated.MaxValue = (decimal)this.product.PlannedArea;
			    this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
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
				this.numCoolLoad.MaxValue = (decimal)this.product.NecessaryCoolLoad;
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
					if (mbProduct.AvailableFloorArea <= 0) {
						this.numAreaPercentage.Value = 100;
					} else {
						this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / mbProduct.AvailableFloorArea), 2);
					}
				}
				if ((skipFields & FieldEnum.AREA_UNHEATED) == FieldEnum.NONE) {
					this.numAreaUnheated.Value = Math.Round((decimal)mbProduct.PlannedAreaUnheated, 2);
				}
				this.txtFloorConstruction.Text = (mbProduct.PlannedFloorConstruction == null ? "" : mbProduct.PlannedFloorConstruction.Id + ": " + mbProduct.PlannedFloorConstruction.Name);
				this.txtInsulationConstruction.Text = (mbProduct.PlannedInsulationConstruction == null ? "" : mbProduct.PlannedInsulationConstruction.Id + ": " + mbProduct.PlannedInsulationConstruction.Name);
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_HEAT) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowHeat.Value = Math.Round((decimal)mbProduct.PlannedRoomTemperatureBelowHeat, 2);
				}
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_COOL) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowCool.Value = Math.Round((decimal)mbProduct.PlannedRoomTemperatureBelowCool, 2);
				}
			//    if ((skipFields & FieldEnum.RIM_LENGTH) == FieldEnum.NONE) {
			//        this.numRim.Value = Math.Round((decimal)evProduct.PlannedRimLength, 2);
			//    }
			//    if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_HEAT) == FieldEnum.NONE) {
			//        this.numCorners.Value = (decimal)evProduct.PlannedRimCorners;
			//    }

			//    if ((skipFields & FieldEnum.LAY_DISTANCE) == FieldEnum.NONE) {
			//        this.cmbLayDistance.SelectedItem = new LayDistanceItem(evProduct.RequestedLayDistance, "");
			//    }
			//    if ((skipFields & FieldEnum.RIM_TYPE) == FieldEnum.NONE) {
			//        this.cmbRimType.SelectedItem = new RimTypeItem(evProduct.RequestedRimType, "");
			//    }
			    if ((skipFields & FieldEnum.CIRCUITS) == FieldEnum.NONE) {
					if (this.product.Product.PlannedCircuits.Count == 0) {
						this.product.Product.PlannedCircuits.Add(new ModulBodenCircuit());
					}
			        this.lstCircuits.Items.Clear();
					int count = 1;
					foreach (Circuit c in this.product.Product.PlannedCircuits) {
						lstCircuits.Items.Add("HK" + count++);
					}
					if (lstCircuits.Items.Count > 0) {
					    lstCircuits.SelectedIndex = 0;
					}
					btnRemoveHk.Enabled = lstCircuits.Items.Count > 1;
			    }
				if ((skipFields & FieldEnum.ROWS) == FieldEnum.NONE) {
					this.lstRows.Items.Clear();
					if (lstCircuits.SelectedIndex >= 0) {
						int count = 1;
						ModulBodenCircuit circuit = (this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit);
						if (circuit.Rows.Count == 0) {
							circuit.Rows.Add(new KlimaFlaechenList());
						}
						foreach (KlimaFlaechenList row in circuit.Rows) {
						    lstRows.Items.Add("Reihe " + count++);
						}
						if (lstRows.Items.Count > 0) {
							lstRows.SelectedIndex = 0;
						}
					}
					btnAddRow.Enabled = lstCircuits.SelectedIndex >= 0 && lstRows.Items.Count < ModulKlimaBodenProduct.ConfigMaxModulesInParallel;
					btnRemoveRow.Enabled = lstRows.Items.Count > 1;
					numLength.Enabled = lstRows.SelectedIndex >= 0;
				}
				if ((skipFields & FieldEnum.MODULES) == FieldEnum.NONE) {
					dgvModules.Row = (this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).Rows[lstRows.SelectedIndex].List;
				}
				if ((skipFields & FieldEnum.LENGTH_VERBINDUNGEN) == FieldEnum.NONE) {
					this.numLength.Value = (decimal)(this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).Rows[lstRows.SelectedIndex].LengthVerbindeleitungen;
				}


			//    if ((skipFields & FieldEnum.CALCULATION_TYPE) == FieldEnum.NONE) {
			//        this.rbCalculateHeat.Checked = this.product.CalculateHeat && !this.product.CalculateCool;
			//        this.rbCalculateCool.Checked = this.product.CalculateCool && !this.product.CalculateHeat;
			//        this.rbCalculateBoth.Checked = this.product.CalculateHeat && this.product.CalculateCool;
			//    }

			//    if ((skipFields & FieldEnum.SEPARATE_CIRCUIT) == FieldEnum.NONE) {
			//        this.cbSeparateCircuit.Checked = !evProduct.PlannedProductIsConnection;
			//    }

			//    // General
			    double qDiffHeat = this.product.PlannedHeatLoad - this.product.RequestedHeatLoad;
			    double qDiffCool = this.product.PlannedCoolLoad - this.product.RequestedCoolLoad;

				lblHk.Text = "Heizkreis " + (lstCircuits.SelectedIndex + 1) + ":";
				lblRest.Text = "Rest (" + this.product.Product.AssociatedRoom.ToString() + ")";
				lblQHeat.Text = Math.Round(this.product.PlannedHeatLoad, 2).ToString();
				lblQHeatDiff.Text = Math.Round(qDiffHeat, 2).ToString();
				lblQHeatRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenHeatLoad, 2).ToString();
				lblAvgqHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).C_QHeatPerSqm, 2).ToString();
				lblDurchflussHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).C_DurchflussHeat, 2).ToString();
				lblDruckverlustHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).C_DruckverlustHeat, 2).ToString();
				lblTempHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).C_FloorTempHeat, 2).ToString();
				lblQCool.Text = Math.Round(this.product.PlannedCoolLoad, 2).ToString();
				lblQCoolDiff.Text = (qDiffCool > 0 ? "+" : "") + Math.Round(qDiffCool, 2).ToString();
				lblQCoolRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenCoolLoad, 2).ToString();
				lblAvgqCool.Text = (-1.0 * Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).C_QCoolPerSqm, 2)).ToString();
				lblDurchflussCool.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).C_DurchflussCool, 2).ToString();
				lblDruckverlustCool.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).C_DruckverlustCool, 2).ToString();
				lblTempCool.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).C_FloorTempCool, 2).ToString();


			//    this.lblQSollHeat.Text = Math.Round(this.product.RequestedHeatLoad, 2).ToString();
			//    this.lblQSollCool.Text = Math.Round(this.product.RequestedCoolLoad, 2).ToString();
			//    this.lblQkSollHeat.Text = Math.Round(this.product.RequestedHeatLoadPerSqM, 2).ToString();
			//    this.lblQkSollCool.Text = this.product.PlannedArea.HasValue ? Math.Round(this.product.RequestedCoolLoad / this.product.PlannedArea.Value, 2).ToString() : "0";
			//    this.lblQfbhHeat.Text = Math.Round(this.product.PlannedHeatLoad, 2).ToString();
			//    this.lblQfbhCool.Text = Math.Round(this.product.PlannedCoolLoad, 2).ToString();
			//    double qRestHeat = this.product.PlannedHeatLoad - this.product.RequestedHeatLoad;
			//    double qRestCool = this.product.PlannedCoolLoad - this.product.RequestedCoolLoad;
			//    this.lblQRestHeat.Text = (qRestHeat > 0 ? "+" : "") + Math.Round(qRestHeat, 2).ToString();
			//    this.lblQRestCool.Text = (qRestCool > 0 ? "+" : "") + Math.Round(qRestCool, 2).ToString();


			//        this.lblRimBHeat.Text = evProduct.PlannedRimWidth.ToString();
			//        this.lblRimBCool.Text = evProduct.PlannedRimWidth.ToString();
			//        this.lblRimTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatRim, 1).ToString();
			//        this.lblRimTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolRim, 1).ToString();
			//        this.lblRimQHeat.Text = Math.Round(evProduct.PlannedHeatLoadRim, 0).ToString();
			//        this.lblRimQCool.Text = Math.Round(evProduct.PlannedCoolLoadRim, 0).ToString();
			//    } else {
			//        this.lblRimVaHeat.Text = "";
			//        this.lblRimVaCool.Text = "";
			//        this.lblRimBHeat.Text = "";
			//        this.lblRimBCool.Text = "";
			//        this.lblRimTfbHeat.Text = "";
			//        this.lblRimTfbCool.Text = "";
			//        this.lblRimQHeat.Text = "";
			//        this.lblRimQCool.Text = "";
			//    }

			//    // Aufenthaltszone
			//    if (evProduct.PlannedLayDistance.HasValue) {
			//        switch (evProduct.PlannedLayDistance) {
			//            case EurovalProduct.LayDistance.EV5:
			//                this.lblResidenceVaHeat.Text = "EV5";
			//                this.lblResidenceVaCool.Text = "EV5";
			//                break;
			//            case EurovalProduct.LayDistance.EV10:
			//                this.lblResidenceVaHeat.Text = "EV10";
			//                this.lblResidenceVaCool.Text = "EV10";
			//                break;
			//            case EurovalProduct.LayDistance.EV15:
			//                this.lblResidenceVaHeat.Text = "EV15";
			//                this.lblResidenceVaCool.Text = "EV15";
			//                break;
			//            case EurovalProduct.LayDistance.EV20:
			//                this.lblResidenceVaHeat.Text = "EV20";
			//                this.lblResidenceVaCool.Text = "EV20";
			//                break;
			//            case EurovalProduct.LayDistance.EV25:
			//                this.lblResidenceVaHeat.Text = "EV25";
			//                this.lblResidenceVaCool.Text = "EV25";
			//                break;
			//            case EurovalProduct.LayDistance.EV30:
			//                this.lblResidenceVaHeat.Text = "EV30";
			//                this.lblResidenceVaCool.Text = "EV30";
			//                break;
			//            case EurovalProduct.LayDistance.EV35:
			//                this.lblResidenceVaHeat.Text = "EV35";
			//                this.lblResidenceVaCool.Text = "EV35";
			//                break;
			//            default:
			//                this.lblResidenceVaHeat.Text = "???";
			//                this.lblResidenceVaCool.Text = "???";
			//                break;
			//        }
			//        this.lblResidenceAHeat.Text = evProduct.PlannedAreaResidence.ToString();
			//        this.lblResidenceACool.Text = evProduct.PlannedAreaResidence.ToString();
			//        this.lblResidenceTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatResidence, 1).ToString();
			//        this.lblResidenceTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolResidence, 1).ToString();
			//        this.lblResidenceQHeat.Text = Math.Round(evProduct.PlannedHeatLoadResidence, 0).ToString();
			//        this.lblResidenceQCool.Text = Math.Round(evProduct.PlannedCoolLoadResidence, 0).ToString();
			//    } else {
			//        this.lblResidenceVaHeat.Text = "";
			//        this.lblResidenceVaCool.Text = "";
			//        this.lblResidenceAHeat.Text = "";
			//        this.lblResidenceACool.Text = "";
			//        this.lblResidenceTfbHeat.Text = "";
			//        this.lblResidenceTfbCool.Text = "";
			//        this.lblResidenceQHeat.Text = "";
			//        this.lblResidenceQCool.Text = "";
			//    }

			//    // anbindung
			//    this.lblConnectionAHeat.Text = evProduct.PlannedRemoveArea.ToString();
			//    this.lblConnectionACool.Text = evProduct.PlannedRemoveArea.ToString();
			//    this.lblConnectionQHeat.Text = Math.Round(evProduct.PlannedHeatLoadAnbindung, 0).ToString();
			//    this.lblConnectionQCool.Text = Math.Round(evProduct.PlannedCoolLoadAnbindung, 0).ToString();

			//    // heizkreis
			//    this.lblCircuitCountHeat.Text = evProduct.PlannedCircuits.ToString();
			//    this.lblCircuitCountCool.Text = evProduct.PlannedCircuits.ToString();
			//    this.lblPipeLengthHeat.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
			//    this.lblPipeLengthCool.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
			//    this.lblMhHeat.Text = Math.Round(evProduct.PlannedMhHeat, 1).ToString();
			//    this.lblMhCool.Text = Math.Round(evProduct.PlannedMhCool, 1).ToString();
			//    this.lblDeltaPHeat.Text = Math.Round(evProduct.PlannedDeltaRhoHeat, 1).ToString();
			//    this.lblDeltaPCool.Text = Math.Round(evProduct.PlannedDeltaRhoCool, 1).ToString(); ;
			//    this.lblSpreizungHeat.Text = Math.Round(evProduct.PlannedSpreizungHeat, 1).ToString();
			//    this.lblSpreizungCool.Text = Math.Round(evProduct.PlannedSpreizungCool, 1).ToString();

			//    if (evProduct.PlannedProductIsConnection) {
			//        this.txtDistributor.Text = "kein eigener Heizkreis";
			//    } else {
			//        if (evProduct.PlannedConnection == null) {
			//            this.txtDistributor.Text = "";
			//        } else {
			//            this.txtDistributor.Text = evProduct.PlannedConnection.ToString();
			//        }
			//    }

			//    if (this.errorMsg != null) {
			//        this.lblError.Text = this.errorMsg;
			//        this.lblError.Visible = true;
			//    } else {
			//        this.lblError.Visible = false;
			//    }

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
				ignoreRows--;
				ignoreModules--;
				ignoreLengthVerbindungen--;
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

		private void numAreaPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaPercentage == 0) {
				ignoreArea++;
				(this.product.Product as ModulKlimaBodenProduct).PlannedFloorAreaPercentage = (float)this.numAreaPercentage.Value;
				this.numArea.Value = (decimal)this.product.PlannedArea;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
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
				(this.product.Product as ModulKlimaBodenProduct).PlannedFloorArea = (float)this.numArea.Value;
				this.numAreaPercentage.Value = (decimal)(this.product.Product as ModulKlimaBodenProduct).PlannedFloorAreaPercentage;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.AREA);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreAreaPercentage--;
			}
		}

		private void numAreaUnheated_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaUnheated == 0) {
				ignoreAreaReduced++;
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.PlannedAreaUnheated = (float)this.numAreaUnheated.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.AREA_UNHEATED);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreAreaReduced--;
			}
		}

		private void btnFloorConstruction_Click(object sender, EventArgs e) {
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.FloorConstruction);
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
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.InsulationConstruction);
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

		private void btnAddHk_Click(object sender, EventArgs e) {
			this.product.Product.PlannedCircuits.Add(new ModulBodenCircuit());
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
			this.UpdateControl(FieldEnum.NONE);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnRemoveHk_Click(object sender, EventArgs e) {
			if (lstCircuits.Items.Count > 1 && lstCircuits.SelectedIndex >= 0) {
				this.product.Product.PlannedCircuits.RemoveAt(lstCircuits.SelectedIndex);
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.NONE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void lstCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreCircuits == 0) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.CIRCUITS);
			}

		}

		private void btnAddRow_Click(object sender, EventArgs e) {
			ModulBodenCircuit circuit = (this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit);
			circuit.Rows.Add(new KlimaFlaechenList());
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
			this.UpdateControl(FieldEnum.CIRCUITS);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnRemoveRow_Click(object sender, EventArgs e) {
			if (lstRows.Items.Count > 1 && lstRows.SelectedIndex >= 0) {
				ModulBodenCircuit circuit = (this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit);
				circuit.Rows.RemoveAt(lstRows.SelectedIndex);
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.CIRCUITS);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void lstRows_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreRows == 0) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.ROWS);
			}
		}

		private void dgvModules_GridContentChanged(object sender) {
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
			this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.ROWS | FieldEnum.MODULES);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void numLength_ValueChanged(object sender, EventArgs e) {
			if (ignoreLengthVerbindungen == 0) {
				(this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulBodenCircuit).Rows[lstRows.SelectedIndex].LengthVerbindeleitungen = (double)this.numLength.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.ROWS | FieldEnum.MODULES | FieldEnum.LENGTH_VERBINDUNGEN);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
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
		
	}
}
