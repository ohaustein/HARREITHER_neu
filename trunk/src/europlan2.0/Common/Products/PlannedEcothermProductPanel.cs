using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class PlannedEcothermProductPanel : UserControl, IEditorUserControl {
		private PlannedProduct product = null;


		private class LayDistanceItem {
			public Nullable<EcothermProduct.EcothermLayDistance> layDistance;
			public string name;

			public LayDistanceItem(Nullable<EcothermProduct.EcothermLayDistance> layDistance, string name) {
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
			public Nullable<EcothermProduct.EcothermRimType> rimType;
			public string name;

			public RimTypeItem(Nullable<EcothermProduct.EcothermRimType> layDistance, string name) {
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

		public PlannedEcothermProductPanel() {
			InitializeComponent();

			this.cmbLayDistance.Items.Clear();
			this.cmbLayDistance.Items.Add(new LayDistanceItem(null, "Automatisch"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EcothermProduct.EcothermLayDistance.EV35, "EV35"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EcothermProduct.EcothermLayDistance.EV30, "EV30"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EcothermProduct.EcothermLayDistance.EV25, "EV25"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EcothermProduct.EcothermLayDistance.EV20, "EV20"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EcothermProduct.EcothermLayDistance.EV15, "EV15"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EcothermProduct.EcothermLayDistance.EV10, "EV10"));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EcothermProduct.EcothermLayDistance.EV5, "EV5"));

			this.cmbRimType.Items.Clear();
			this.cmbRimType.Items.Add(new RimTypeItem(null, "Automatisch"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV15_60, "EV15/60"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV15_120, "EV15/120"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV15_180, "EV15/180"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV10_55, "EV10/55"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV10_110, "EV10/110"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV10_165, "EV10/165"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV5_40, "EV5/40"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV5_80, "EV5/80"));
			this.cmbRimType.Items.Add(new RimTypeItem(EcothermProduct.EcothermRimType.EV5_120, "EV5/120"));

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
			RIM_LENGTH = 1024,
			CORNERS = 2048,
			LAY_DISTANCE = 4096,
			RIM_TYPE = 8192,
			CALCULATION_TYPE = 16384,
			CIRCUIT_COUNT = 32768,
			SEPARATE_CIRCUIT = 65536
		}

		/*private class ComboItem {
			private string name;
			private object value;

			public ComboItem(string name, object value) {
				this.name = name;
				this.value = value;
			}

			public string Name {
				get { return this.name; }
				set { this.name = value; }
			}

			public object Value {
				get { return this.value; }
				set { this.value = value; }
			}

			public override string ToString() {
				return this.name;
			}

			public override int GetHashCode() {
				return this.value == null ? 0 : this.value.GetHashCode();
			}

			public override bool Equals(object obj) {
				return this.value == null ? obj == null : this.value.Equals(obj);
			}
		}*/

		private string errorMsg = null;

		public void UpdateControl() {
			this.product = this.Tag as PlannedProduct;
			this.tabs.SelectedTab = this.pageInput;
			this.connectionPipePanel.Update(this.product);
			this.chkStellAntriebe.Checked = this.product.Product.StellMotore;
			if (this.product != null) {
				(this.product.Product as EcothermProduct).ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
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
		private int ignoreRim = 0;
		private int ignoreCorners = 0;
		private int ignoreLayDistance = 0;
		private int ignoreRimType = 0;
		private int ignoreCalculationType = 0;
		private int ignoreCircuits = 0;
		private int ignoreSeparateCircuit = 0;

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
				ignoreRim++;
				ignoreCorners++;
				ignoreLayDistance++;
				ignoreRimType++;
				ignoreCalculationType++;
				ignoreCircuits++;
				ignoreSeparateCircuit++;

				EcothermProduct evProduct = this.product.Product as EcothermProduct;

				bool showHeat = this.product.Product.AssociatedRoom.HeatLoad > 0 && evProduct.PlannedLayDistance != EcothermProduct.EcothermLayDistance.NONE;
				bool showCool = this.product.Product.AssociatedRoom.CoolLoad > 0 && evProduct.PlannedLayDistance != EcothermProduct.EcothermLayDistance.NONE;
				bool showRim = evProduct.PlannedAreaRim > 0;
				bool showResidence = true;
				bool complete = this.product.Product.PlannedCalculationComplete;

				this.lblQSollHeat.Visible = showHeat;
				this.lblQkSollHeat.Visible = showHeat;
				this.lblQfbhHeat.Visible = showHeat;
				this.lblQRestHeat.Visible = showHeat;
				this.lblRimVaHeat.Visible = showHeat && showRim && complete;
				this.lblRimBHeat.Visible = showHeat && showRim && complete;
				this.lblRimTfbHeat.Visible = showHeat && showRim && complete;
				this.lblRimQHeat.Visible = showHeat && showRim && complete;
				this.lblResidenceVaHeat.Visible = showHeat && showResidence && complete;
				this.lblResidenceAHeat.Visible = showHeat && showResidence && complete;
				this.lblResidenceTfbHeat.Visible = showHeat && showResidence && complete;
				this.lblResidenceQHeat.Visible = showHeat && showResidence && complete;
				this.lblConnectionAHeat.Visible = showHeat;
				this.lblConnectionQHeat.Visible = showHeat;
				this.lblCircuitCountHeat.Visible = showHeat && complete;
				this.lblPipeLengthHeat.Visible = showHeat && complete;
				this.lblMhHeat.Visible = showHeat && complete;
				this.lblDeltaPHeat.Visible = showHeat && complete;
				this.lblSpreizungHeat.Visible = showHeat && complete;

				this.lblQSollCool.Visible = showCool;
				this.lblQkSollCool.Visible = showCool;
				this.lblQfbhCool.Visible = showCool;
				this.lblQRestCool.Visible = showCool;
				this.lblRimVaCool.Visible = showCool && showRim && complete;
				this.lblRimBCool.Visible = showCool && showRim && complete;
				this.lblRimTfbCool.Visible = showCool && showRim && complete;
				this.lblRimQCool.Visible = showCool && showRim && complete;
				this.lblResidenceVaCool.Visible = showCool && showResidence && complete;
				this.lblResidenceACool.Visible = showCool && showResidence && complete;
				this.lblResidenceTfbCool.Visible = showCool && showResidence && complete;
				this.lblResidenceQCool.Visible = showCool && showResidence && complete;
				this.lblConnectionACool.Visible = showCool;
				this.lblConnectionQCool.Visible = showCool;
				this.lblCircuitCountCool.Visible = showCool && complete;
				this.lblPipeLengthCool.Visible = showCool && complete;
				this.lblMhCool.Visible = showCool && complete;
				this.lblDeltaPCool.Visible = showCool && complete;
				this.lblSpreizungCool.Visible = showCool && complete;

				this.rbCalculateHeat.Enabled = this.product.RequestedHeatLoad > 0;
				this.rbCalculateCool.Enabled = this.product.RequestedCoolLoad > 0;
				this.rbCalculateBoth.Enabled = this.product.RequestedHeatLoad > 0 && this.product.RequestedCoolLoad > 0;
				this.numCorners.Enabled = evProduct.PlannedRimLength > 0;
				this.cmbRimType.Enabled = evProduct.PlannedAreaRim > 0;

				// disable the following controls if the product is a connection
				this.numRim.Enabled = !evProduct.PlannedProductIsConnection;
				this.numCorners.Enabled = this.numCorners.Enabled && !evProduct.PlannedProductIsConnection;
				this.rbCalculateHeat.Enabled = this.rbCalculateHeat.Enabled && !evProduct.PlannedProductIsConnection;
				this.rbCalculateCool.Enabled = this.rbCalculateCool.Enabled && !evProduct.PlannedProductIsConnection;
				this.rbCalculateBoth.Enabled = this.rbCalculateBoth.Enabled && !evProduct.PlannedProductIsConnection;
				this.btnDistributor.Enabled = !evProduct.PlannedProductIsConnection;
				this.cmbLayDistance.Enabled = !evProduct.PlannedProductIsConnection;
				this.cmbRimType.Enabled = this.cmbRimType.Enabled && !evProduct.PlannedProductIsConnection;
				this.cmbCircuits.Enabled = !evProduct.PlannedProductIsConnection;

				this.numArea.MaxValue = (decimal)evProduct.AvailableFloorArea;
				this.numAreaPercentage.MaxValue = (decimal)(evProduct.AvailableFloorArea * 100 / evProduct.AssociatedRoom.Area);
				this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
				this.numAreaUnheated.MaxValue = (decimal)this.product.PlannedArea;
				this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
				this.numHeatLoadPercentage.MaxValue = (decimal)(evProduct.AssociatedRoom.NormalizedHeatLoad <= 0 ? 0 : this.product.NecessaryHeatLoad * 100 / evProduct.AssociatedRoom.NormalizedHeatLoad);
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
				this.numCoolLoadPercentage.MaxValue = (decimal)(evProduct.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / evProduct.AssociatedRoom.NormalizedCoolLoad);
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
					if (evProduct.AssociatedRoom.Area <= 0) {
						this.numAreaPercentage.Value = 100;
					} else {
						this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / evProduct.AssociatedRoom.Area), 2);
					}
				}
				if ((skipFields & FieldEnum.AREA_REDUCED) == FieldEnum.NONE) {
					this.numAreaReduced.Value = Math.Round((decimal)evProduct.PlannedAreaReduced, 2);
				}
				if ((skipFields & FieldEnum.AREA_UNHEATED) == FieldEnum.NONE) {
					this.numAreaUnheated.Value = Math.Round((decimal)evProduct.PlannedAreaUnheated, 2);
				}
				this.txtFloorConstruction.Text = (evProduct.PlannedFloorConstruction == null ? "" : evProduct.PlannedFloorConstruction.Id + ": " + evProduct.PlannedFloorConstruction.Name);
				this.txtInsulationConstruction.Text = (evProduct.PlannedInsulationConstruction == null ? "" : evProduct.PlannedInsulationConstruction.Id + ": " + evProduct.PlannedInsulationConstruction.Name);
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_HEAT) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowHeat.Value = Math.Round((decimal)evProduct.PlannedRoomTemperatureBelowHeat, 2);
				}
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_COOL) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowCool.Value = Math.Round((decimal)evProduct.PlannedRoomTemperatureBelowCool, 2);
				}
				if ((skipFields & FieldEnum.RIM_LENGTH) == FieldEnum.NONE) {
					this.numRim.Value = Math.Round((decimal)evProduct.PlannedRimLength, 2);
				}
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_HEAT) == FieldEnum.NONE) {
					this.numCorners.Value = (decimal)evProduct.PlannedRimCorners;
				}

				if ((skipFields & FieldEnum.LAY_DISTANCE) == FieldEnum.NONE) {
					this.cmbLayDistance.SelectedItem = new LayDistanceItem(evProduct.RequestedLayDistance, "");
				}
				if ((skipFields & FieldEnum.RIM_TYPE) == FieldEnum.NONE) {
					this.cmbRimType.SelectedItem = new RimTypeItem(evProduct.RequestedRimType, "");
				}
				if ((skipFields & FieldEnum.CIRCUIT_COUNT) == FieldEnum.NONE) {
					if (evProduct.RequestedCircuits != null) {
						this.cmbCircuits.SelectedIndex = evProduct.RequestedCircuits.Value;
					} else {
						this.cmbCircuits.SelectedIndex = 0;
					}
				}

				if ((skipFields & FieldEnum.CALCULATION_TYPE) == FieldEnum.NONE) {
					this.rbCalculateHeat.Checked = this.product.CalculateHeat && !this.product.CalculateCool;
					this.rbCalculateCool.Checked = this.product.CalculateCool && !this.product.CalculateHeat;
					this.rbCalculateBoth.Checked = this.product.CalculateHeat && this.product.CalculateCool;
				}

				if ((skipFields & FieldEnum.SEPARATE_CIRCUIT) == FieldEnum.NONE) {
					this.cbSeparateCircuit.Checked = !evProduct.PlannedProductIsConnection;
				}

				this.chkClip.Checked = evProduct.UseClipSchieneKlebeband;
				this.chkAnhydritEstrich.Checked = evProduct.UseAnhydritEstrich;

				// General
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

				// Randzone
				if (evProduct.PlannedRimType.HasValue) {
					switch (evProduct.PlannedRimLayDistance) {
						case EcothermProduct.EcothermLayDistance.EV5:
							this.lblRimVaHeat.Text = "EV5";
							this.lblRimVaCool.Text = "EV5";
							break;
						case EcothermProduct.EcothermLayDistance.EV10:
							this.lblRimVaHeat.Text = "EV10";
							this.lblRimVaCool.Text = "EV10";
							break;
						case EcothermProduct.EcothermLayDistance.EV15:
							this.lblRimVaHeat.Text = "EV15";
							this.lblRimVaCool.Text = "EV15";
							break;
						case EcothermProduct.EcothermLayDistance.EV20:
							this.lblRimVaHeat.Text = "EV20";
							this.lblRimVaCool.Text = "EV20";
							break;
						case EcothermProduct.EcothermLayDistance.EV25:
							this.lblRimVaHeat.Text = "EV25";
							this.lblRimVaCool.Text = "EV25";
							break;
						case EcothermProduct.EcothermLayDistance.EV30:
							this.lblRimVaHeat.Text = "EV30";
							this.lblRimVaCool.Text = "EV30";
							break;
						case EcothermProduct.EcothermLayDistance.EV35:
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
					this.lblRimTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatRim, 1).ToString();
					this.lblRimTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolRim, 1).ToString();
					this.lblRimQHeat.Text = Math.Round(evProduct.PlannedHeatLoadRim, 0).ToString();
					this.lblRimQCool.Text = Math.Round(evProduct.PlannedCoolLoadRim, 0).ToString();
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

				// Aufenthaltszone
				if (evProduct.PlannedLayDistance.HasValue) {
					switch (evProduct.PlannedLayDistance) {
						case EcothermProduct.EcothermLayDistance.EV5:
							this.lblResidenceVaHeat.Text = "EV5";
							this.lblResidenceVaCool.Text = "EV5";
							break;
						case EcothermProduct.EcothermLayDistance.EV10:
							this.lblResidenceVaHeat.Text = "EV10";
							this.lblResidenceVaCool.Text = "EV10";
							break;
						case EcothermProduct.EcothermLayDistance.EV15:
							this.lblResidenceVaHeat.Text = "EV15";
							this.lblResidenceVaCool.Text = "EV15";
							break;
						case EcothermProduct.EcothermLayDistance.EV20:
							this.lblResidenceVaHeat.Text = "EV20";
							this.lblResidenceVaCool.Text = "EV20";
							break;
						case EcothermProduct.EcothermLayDistance.EV25:
							this.lblResidenceVaHeat.Text = "EV25";
							this.lblResidenceVaCool.Text = "EV25";
							break;
						case EcothermProduct.EcothermLayDistance.EV30:
							this.lblResidenceVaHeat.Text = "EV30";
							this.lblResidenceVaCool.Text = "EV30";
							break;
						case EcothermProduct.EcothermLayDistance.EV35:
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
					this.lblResidenceTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatResidence, 1).ToString();
					this.lblResidenceTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolResidence, 1).ToString();
					this.lblResidenceQHeat.Text = Math.Round(evProduct.PlannedHeatLoadResidence, 0).ToString();
					this.lblResidenceQCool.Text = Math.Round(evProduct.PlannedCoolLoadResidence, 0).ToString();
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
				this.lblConnectionAHeat.Text = evProduct.PlannedRemoveArea.ToString();
				this.lblConnectionACool.Text = evProduct.PlannedRemoveArea.ToString();
				this.lblConnectionQHeat.Text = Math.Round(evProduct.PlannedHeatLoadAnbindung, 0).ToString();
				this.lblConnectionQCool.Text = Math.Round(evProduct.PlannedCoolLoadAnbindung, 0).ToString();

				// heizkreis
				this.lblCircuitCountHeat.Text = evProduct.PlannedCircuitCount.ToString();
				this.lblCircuitCountCool.Text = evProduct.PlannedCircuitCount.ToString();
				this.lblPipeLengthHeat.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
				this.lblPipeLengthCool.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
				this.lblMhHeat.Text = Math.Round(evProduct.PlannedMaxMhHeat, 1).ToString();
				this.lblMhCool.Text = Math.Round(evProduct.PlannedMaxMhCool, 1).ToString();
				this.lblDeltaPHeat.Text = Math.Round(evProduct.PlannedDeltaRhoHeat, 1).ToString();
				this.lblDeltaPCool.Text = Math.Round(evProduct.PlannedDeltaRhoCool, 1).ToString(); ;
				this.lblSpreizungHeat.Text = Math.Round(evProduct.PlannedSpreizungHeat, 1).ToString();
				this.lblSpreizungCool.Text = Math.Round(evProduct.PlannedSpreizungCool, 1).ToString();

				if (evProduct.PlannedProductIsConnection) {
					this.txtDistributor.Text = "kein eigener Heizkreis";
				} else {
					if (evProduct.PlannedConnection == null) {
						this.txtDistributor.Text = "";
					} else {
						this.txtDistributor.Text = evProduct.PlannedConnection.ToString();
					}
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
				notifications = EcothermProduct.GlobalNotificationMessage;
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
				ignoreRim--;
				ignoreCorners--;
				ignoreLayDistance--;
				ignoreRimType--;
				ignoreCalculationType--;
				ignoreCircuits--;
				ignoreSeparateCircuit--;
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
				(this.product.Product as EcothermProduct).PlannedFloorAreaPercentage = (float)this.numAreaPercentage.Value;
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
				(this.product.Product as EcothermProduct).PlannedFloorArea = (float)this.numArea.Value;
				this.numAreaPercentage.Value = (decimal)(this.product.Product as EcothermProduct).PlannedFloorAreaPercentage;
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
				EcothermProduct evProduct = this.product.Product as EcothermProduct;
				evProduct.PlannedAreaReduced = (float)this.numAreaReduced.Value;
				if (evProduct.PlannedAreaReduced + evProduct.PlannedAreaUnheated > evProduct.PlannedFloorArea) {
					evProduct.PlannedAreaUnheated = evProduct.PlannedFloorArea - evProduct.PlannedAreaReduced;
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
				EcothermProduct evProduct = this.product.Product as EcothermProduct;
				evProduct.PlannedAreaUnheated = (float)this.numAreaUnheated.Value;
				if (evProduct.PlannedAreaReduced + evProduct.PlannedAreaUnheated > evProduct.PlannedFloorArea) {
					evProduct.PlannedAreaReduced = evProduct.PlannedFloorArea - evProduct.PlannedAreaUnheated;
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
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TROCKEN),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TROCKEN)}));
			form.SelectedConstruction = (this.product.Product as EcothermProduct).PlannedFloorConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as EcothermProduct).PlannedFloorConstruction = form.SelectedConstruction;
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
			form.SelectedConstruction = (this.product.Product as EcothermProduct).PlannedInsulationConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as EcothermProduct).PlannedInsulationConstruction = form.SelectedConstruction;
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
				(this.product.Product as EcothermProduct).PlannedRoomTemperatureBelowHeat = (float)this.numRoomTemperatureBelowHeat.Value;
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
				(this.product.Product as EcothermProduct).PlannedRoomTemperatureBelowCool = (float)this.numRoomTemperatureBelowCool.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_COOL);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numRim_ValueChanged(object sender, EventArgs e) {
			if (ignoreRim == 0) {
				(this.product.Product as EcothermProduct).PlannedRimLength = (float)this.numRim.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.RIM_LENGTH);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numCorners_ValueChanged(object sender, EventArgs e) {
			if (ignoreCorners == 0) {
				(this.product.Product as EcothermProduct).PlannedRimCorners = (int)this.numCorners.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CORNERS);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void cmbLayDistance_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreLayDistance == 0) {
				(this.product.Product as EcothermProduct).RequestedLayDistance = (this.cmbLayDistance.SelectedItem as LayDistanceItem).layDistance;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.LAY_DISTANCE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void cmbRimType_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreRimType == 0) {
				(this.product.Product as EcothermProduct).RequestedRimType = (this.cmbRimType.SelectedItem as RimTypeItem).rimType;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.RIM_TYPE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}

		}

		private void cmbCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreCircuits == 0) {
				if (this.cmbCircuits.SelectedIndex > 0) {
					(this.product.Product as EcothermProduct).RequestedCircuits = this.cmbCircuits.SelectedIndex;
				} else {
					(this.product.Product as EcothermProduct).RequestedCircuits = null;
				}
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUIT_COUNT);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void rbCalculationType_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationType == 0) {
				this.product.CalculateHeat = this.rbCalculateHeat.Checked || this.rbCalculateBoth.Checked;
				this.product.CalculateCool = this.rbCalculateCool.Checked || this.rbCalculateBoth.Checked;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CALCULATION_TYPE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numHeatLoad_Leave(object sender, EventArgs e) {
			if (this.product.RequestedCoolLoad == 0 && !this.rbCalculateHeat.Checked) {
				this.rbCalculateHeat.Checked = true;
			}
			if (this.product.RequestedHeatLoad == 0 && this.product.RequestedCoolLoad > 0 && !this.rbCalculateCool.Checked) {
				this.rbCalculateCool.Checked = true;
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

		private void cbSeparateCircuit_CheckedChanged(object sender, EventArgs e) {
			if (ignoreSeparateCircuit == 0) {
				this.product.Product.PlannedProductIsConnection = !this.cbSeparateCircuit.Checked;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.SEPARATE_CIRCUIT);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void chkClip_CheckedChanged(object sender, EventArgs e) {
			(this.product.Product as EcothermProduct).UseClipSchieneKlebeband = chkClip.Checked;
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void chkAnhydritEstrich_CheckedChanged(object sender, EventArgs e) {
			(this.product.Product as EcothermProduct).UseAnhydritEstrich = chkAnhydritEstrich.Checked;
			if (chkAnhydritEstrich.Checked) {
				chkClip.Checked = true;
			}
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
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
