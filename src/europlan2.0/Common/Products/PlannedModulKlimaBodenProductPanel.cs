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

			this.cmbCircuits.Items.Clear();
			this.cmbCircuits.Items.Add("Automatisch");
			this.cmbCircuits.Items.Add("1");
			this.cmbCircuits.Items.Add("2");
			this.cmbCircuits.Items.Add("3");
			this.cmbCircuits.Items.Add("4");
			this.cmbCircuits.Items.Add("5");
			this.cmbCircuits.Items.Add("6");
			this.cmbCircuits.Items.Add("7");
			this.cmbCircuits.Items.Add("8");
			this.cmbCircuits.Items.Add("9");
			this.cmbCircuits.Items.Add("10");
			this.cmbCircuits.Items.Add("11");
			this.cmbCircuits.Items.Add("12");
		}

		#region IEditorUserControl Members
		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private enum FieldEnum {
			NONE                       = 0,
			HEAT_LOAD                  = 0x1,
			COOL_LOAD                  = 0x2,
			HEAT_LOAD_PERCENTAGE       = 0x4,
			COOL_LOAD_PERCENTAGE       = 0x8,
			AREA                       = 0x10,
			AREA_PERCENTAGE            = 0x20,
			AREA_REDUCED               = 0x40,
			AREA_UNHEATED              = 0x80,
			ROOM_TEMERATURE_BELOW_HEAT = 0x100,
			ROOM_TEMERATURE_BELOW_COOL = 0x200,
			//MODULES                  = 0x400,
			//LAY_DISTANCE             = 0x800,
			//RIM_TYPE                 = 0x1000,
			//CALCULATION_TYPE         = 0x2000,
			CIRCUITS                   = 0x4000,
			MODULES_DICHT              = 0x8000,
			MODULES_MODULIEREND        = 0x10000,
			MODULES_SONTIGE            = 0x20000,
			MODULES_VERBINDELEITUNG    = 0x40000
		}


		private string errorMsg = null;

		public void UpdateControl() {
			this.product = this.Tag as PlannedProduct;
			this.tabs.SelectedTab = this.pageInput;
			this.connectionPipePanel.Update(this.product);
			this.chkStellAntriebe.Checked = this.product.Product.StellMotore;
			if (this.product != null) {
				(this.product.Product as ModulKlimaBodenProduct).ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool,false);
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
		private int ignoreModulesDicht = 0;
		private int ignoreModulesModulierend = 0;
		private int ignoreModulesSonstige = 0;
		private int ignoreVerbindeleitungen = 0;

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
				ignoreModulesDicht++;
				ignoreModulesModulierend++;
				ignoreModulesSonstige++;
				ignoreVerbindeleitungen++;

				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;

				bool showHeat = this.product.Product.AssociatedRoom.HeatLoad > 0;
				bool showCool = this.product.Product.AssociatedRoom.CoolLoad > 0;

				lblQHeat.Visible = showHeat;
				lblQHeatUnit.Visible = showHeat;
				lblQAnbHeat.Visible = showHeat;
				lblQAnbHeatUnit.Visible = showHeat;
				lblQHeatDiff.Visible = showHeat;
				lblQHeatDiffUnit.Visible = showHeat;
				lblQHeatRest.Visible = showHeat;
				lblQHeatRestUnit.Visible = showHeat;
				lblAvgqHeat.Visible = showHeat;
				lblAvgqHeatUnit.Visible = showHeat;
				lblDurchflussHeat.Visible = showHeat;
				lblDurchflussHeatUnit.Visible = showHeat;
				lblDruckverlustHeat.Visible = showHeat;
				lblDruckverlustHeatUnit.Visible = showHeat;
				lblTempHeat.Visible = showHeat;
				lblTempHeatUnit.Visible = showHeat;
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
				lblTempCool.Visible = showCool;
				lblTempCoolUnit.Visible = showCool;

				this.numArea.MaxValue = (decimal)mbProduct.AvailableFloorArea;
				this.numAreaPercentage.MaxValue = (decimal)(mbProduct.AvailableFloorArea * 100 / mbProduct.AssociatedRoom.Area);
				this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
				this.numAreaUnheated.MaxValue = (decimal)this.product.PlannedArea;
				this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
				this.numHeatLoadPercentage.MaxValue = (decimal)(mbProduct.AssociatedRoom.NormalizedHeatLoad <= 0 ? 0 : this.product.NecessaryHeatLoad * 100 / mbProduct.AssociatedRoom.NormalizedHeatLoad);
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
				this.numCoolLoadPercentage.MaxValue = (decimal)(mbProduct.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / mbProduct.AssociatedRoom.NormalizedCoolLoad);
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
					this.numArea.Value = Math.Round((decimal)plannedArea, 2);
				}
				if ((skipFields & FieldEnum.AREA_PERCENTAGE) == FieldEnum.NONE) {
					if (mbProduct.AssociatedRoom.Area <= 0) {
						this.numAreaPercentage.Value = 100;
					} else {
						this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / mbProduct.AssociatedRoom.Area), 2);
					}
				}
				if ((skipFields & FieldEnum.AREA_REDUCED) == FieldEnum.NONE) {
					this.numAreaReduced.Value = Math.Round((decimal)mbProduct.PlannedAreaReduced, 2);
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

				if ((skipFields & FieldEnum.CIRCUITS) == FieldEnum.NONE) {
					this.cmbCircuits.SelectedIndex = (mbProduct.RequestedCircuits.HasValue ? mbProduct.RequestedCircuits.Value : 0);
				}

				this.numDicht.Value = mbProduct.RequestedModulesDicht;
				this.numModulierend.Value = mbProduct.RequestedModulesModulierend;
				this.numSonstige.Value = mbProduct.RequestedModulesSonstige;
				this.numVerbindeleitungen.Value = (decimal)mbProduct.RequestedSonstigeVerbindeLeitung;

			    // General
			    double qDiffHeat = this.product.PlannedHeatLoad - this.product.RequestedHeatLoad;
			    double qDiffCool = this.product.PlannedCoolLoad - this.product.RequestedCoolLoad;

				//lblHk.Text = "Heizkreis " + (lstCircuits.SelectedIndex + 1) + ":";
				lblRest.Text = "Rest (" + this.product.Product.AssociatedRoom.ToString() + ")";
				lblQHeat.Text = Math.Round(this.product.PlannedHeatLoad, 2).ToString();
				lblQAnbHeat.Text = Math.Round(this.product.Product.PlannedHeatLoadAnbindung, 0).ToString();
				lblQHeatDiff.Text = Math.Round(qDiffHeat, 2).ToString();
				lblQHeatRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenHeatLoad, 2).ToString();
				lblAvgqHeat.Text = Math.Round(mbProduct.PlannedHeatLoadPerSqM, 2).ToString();
				lblDurchflussHeat.Text = Math.Round(mbProduct.PlannedMaxDurchflussHeat, 2).ToString();
				lblDruckverlustHeat.Text = Math.Round(mbProduct.PlannedDeltaRhoHeat, 2).ToString();
				lblTempHeat.Text = Math.Round(mbProduct.PlannedFloorTemperatureHeat, 2).ToString();
				lblQCool.Text = Math.Round(this.product.PlannedCoolLoad, 2).ToString();
				lblQAnbCool.Text = Math.Round(this.product.Product.PlannedCoolLoadAnbindung, 0).ToString();
				lblQCoolDiff.Text = (qDiffCool > 0 ? "+" : "") + Math.Round(qDiffCool, 2).ToString();
				lblQCoolRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenCoolLoad, 2).ToString();
				lblAvgqCool.Text = Math.Round(mbProduct.PlannedCoolLoadPerSqM, 2).ToString();
				lblDurchflussCool.Text = Math.Round(mbProduct.PlannedMaxDurchflussCool, 2).ToString();
				lblDruckverlustCool.Text = Math.Round(mbProduct.PlannedDeltaRhoCool, 2).ToString();
				lblTempCool.Text = Math.Round(mbProduct.PlannedFloorTemperatureCool, 2).ToString();
				double availableArea = Math.Round(this.product.Product.PlannedNetArea, 2);
				double coveredArea = Math.Round((this.product.Product as ModulKlimaBodenProduct).CoveredFloorArea, 2);
				double heatArea = Math.Round((this.product.Product as ModulKlimaBodenProduct).PlannedModulArea, 2);
				double anbArea = Math.Round((this.product.Product as ModulKlimaBodenProduct).PlannedRemoveArea, 2);
				lblAvailableArea.Text = availableArea.ToString();
				lblCoveredArea.Text = coveredArea.ToString();
				lblHeatArea.Text = heatArea.ToString();
				lblAnbArea.Text = anbArea.ToString();
				lblRestArea.Text = Math.Round(availableArea - anbArea - coveredArea, 2).ToString();
				lblCircuitCount.Text = mbProduct.PlannedCircuitCount.ToString();

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

				if (mbProduct.PlannedConnection == null) {
					this.txtDistributor.Text = "";
				} else {
					this.txtDistributor.Text = mbProduct.PlannedConnection.ToString();
				}

				if (mbProduct.PlannedModulArea > mbProduct.PlannedNetArea) {
					this.lblAreaWarning.Text = "Die verplanten Module nehmen mehr Fläche in Anspruch als für dieses System zur Verfügung steht (" + Math.Round(mbProduct.PlannedModulArea, 1).ToString() + "m² > " + Math.Round(mbProduct.PlannedNetArea, 1).ToString() + "m²)\n";
				} else {
					this.lblAreaWarning.Text = "";
				}

				this.lstError.Items.Clear();
				string[] messages;
				if (this.errorMsg != null) {
					 messages = this.errorMsg.Split('\n');
					foreach (string message in messages) {
						if (!string.IsNullOrEmpty(message)) {
							ListViewItem item = new ListViewItem(message);
							item.ForeColor = Color.Red;
							//item.Font = new Font(item.Font, FontStyle.Bold);
							this.lstError.Items.Add(item);
						}
					}
				}
				string notifications = this.product.Product.NotificationMessage;
				if (notifications != null) {
					messages = notifications.Split('\n');
					foreach (string message in messages) {
						if (!string.IsNullOrEmpty(message)) {
							ListViewItem item = new ListViewItem(message);
							item.ForeColor = Color.Orange;
							this.lstError.Items.Add(item);
						}
					}
				}
				notifications = ModulKlimaBodenProduct.GlobalNotificationMessage;
				if (notifications != null) {
					messages = notifications.Split('\n');
					foreach (string message in messages) {
						if (!string.IsNullOrEmpty(message)) {
							ListViewItem item = new ListViewItem(message);
							item.ForeColor = Color.Orange;
							this.lstError.Items.Add(item);
						}
					}
				}
				if (lstError.Items.Count > 0) {
					this.lstError.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
					int height = this.lstError.Items[this.lstError.Items.Count - 1].Position.Y + this.lstError.Items[this.lstError.Items.Count - 1].Bounds.Height + 5;
					this.lstError.Height = height;
					this.lstError.Visible = true;
				} else {
					this.lstError.Visible = false;
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
				ignoreModulesDicht--;
				ignoreModulesModulierend--;
				ignoreModulesSonstige--;
				ignoreVerbindeleitungen--;
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
						this.errorMsg = this.product.Product.LastErrorMessage;
					}
				}
			}
			return true;
		}
		#endregion

		private void chkCoverHeatLoad_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCoverHeatLoad == 0) {
				ignoreHeatLoad++;
				ignoreHeatLoadPercentage++;
				this.product.CoverHeatLoad = this.chkCoverHeatLoad.Checked;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.NONE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreHeatLoad--;
				ignoreHeatLoadPercentage--;
			}
		}

		private void chkCoverCoolLoad_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCoverCoolLoad == 0) {
				ignoreCoolLoad++;
				ignoreCoolLoadPercentage++;
				this.product.CoverCoolLoad = this.chkCoverCoolLoad.Checked;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.NONE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreCoolLoad--;
				ignoreCoolLoadPercentage--;
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

		private void numAreaPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaPercentage == 0) {
				ignoreArea++;
				(this.product.Product as ModulKlimaBodenProduct).PlannedFloorAreaPercentage = (float)this.numAreaPercentage.Value;
				this.numArea.Value = (decimal)this.product.PlannedArea;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.PlannedAreaReduced = (float)this.numAreaReduced.Value;
				if (mbProduct.PlannedAreaReduced + mbProduct.PlannedAreaUnheated > mbProduct.PlannedFloorArea) {
					mbProduct.PlannedAreaUnheated = mbProduct.PlannedFloorArea - mbProduct.PlannedAreaReduced;
				}
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.PlannedAreaUnheated = (float)this.numAreaUnheated.Value;
				if (mbProduct.PlannedAreaReduced + mbProduct.PlannedAreaUnheated > mbProduct.PlannedFloorArea) {
					mbProduct.PlannedAreaReduced = mbProduct.PlannedFloorArea - mbProduct.PlannedAreaUnheated;
				}
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.AREA_UNHEATED);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				ignoreAreaReduced--;
			}
		}

		private void btnFloorConstruction_Click(object sender, EventArgs e) {
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.FloorConstruction,
				new List<ConstructionType>(new ConstructionType[] {
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_STAHL),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_STAHL),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TRK_ESTRICH),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TRK_ESTRICH) }));
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

		private void numRoomTemperatureBelowHeat_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowHeat == 0) {
				(this.product.Product as ModulKlimaBodenProduct).PlannedRoomTemperatureBelowHeat = (float)this.numRoomTemperatureBelowHeat.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_HEAT);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numRoomTemperatureBelowCool_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowCool == 0) {
				(this.product.Product as ModulKlimaBodenProduct).PlannedRoomTemperatureBelowCool = (float)this.numRoomTemperatureBelowCool.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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

		private void numDicht_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreModulesDicht == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedModulesDicht = (int)this.numDicht.Value;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.MODULES_DICHT);
			}
		}

		private void numModulierend_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreModulesModulierend == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedModulesModulierend = (int)this.numModulierend.Value;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.MODULES_MODULIEREND);
			}
		}

		private void numSonstige_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreModulesSonstige == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedModulesSonstige = (int)this.numSonstige.Value;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.MODULES_SONTIGE);
			}
		}

		private void numVerbindeleitungen_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreVerbindeleitungen == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedSonstigeVerbindeLeitung = (double)this.numVerbindeleitungen.Value;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.MODULES_VERBINDELEITUNG);
			}
		}

		private void cmbCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.ignoreCircuits == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedCircuits = this.cmbCircuits.SelectedIndex == 0 ? (Nullable<int>)null : (Nullable<int>)this.cmbCircuits.SelectedIndex;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUITS);
			}
		}

		private void chkStellAntriebe_CheckedChanged(object sender, EventArgs e) {
			this.product.Product.StellMotore = this.chkStellAntriebe.Checked;
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void lstError_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			e.Item.Focused = false;
			e.Item.Selected = false;
		}

	}
}
