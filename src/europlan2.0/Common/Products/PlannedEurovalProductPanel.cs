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
		private Button roomSelectionButton;

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

			roomSelectionButton = new Button();
			roomSelectionButton.Size = new Size(30, 20);
			roomSelectionButton.Text = "...";
             
            dgvConnectionPipes.Controls.Add(roomSelectionButton);
            roomSelectionButton.Hide();
            roomSelectionButton.Click += new EventHandler(roomSelectionButton_Click);
			
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
			ROOM_TEMERATURE_BELOW_HEAT = 256,
			ROOM_TEMERATURE_BELOW_COOL = 512,
			RIM_LENGTH = 1024,
			CORNERS = 2048,
			LAY_DISTANCE = 4096,
			RIM_TYPE = 8192,
			CALCULATION_TYPE = 16384
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

			// pipe type items
			this.PipeType.Items.Clear();
			this.PipeType.Items.Add(ConnectionPipe.PipeTypeEnum.PT_EUROVAL);
			if (this.product != null && this.product.Product != null &&
				this.product.Product.PlannedConnection != null) {
				PlannedProduct connectedProduct = this.product.Product.PlannedConnection.OtherProduct;
				if (connectedProduct != null && connectedProduct.Product.DefaultPipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
					this.PipeType.Items.Add(ConnectionPipe.PipeTypeEnum.PT_21MM);
				}
			}

			// verlegeart items
			this.Verlegeart.Items.Clear();
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV35);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV30);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV25);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV20);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV15);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV10);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_EV5);
			this.Verlegeart.Items.Add(ConnectionPipe.VerlegeartEnum.VA_A5);

			// insulation items
			this.Insulation.Items.Clear();
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_NONE);
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_VL);
			this.Insulation.Items.Add(ConnectionPipe.InsulationEnum.IN_VL_RL);

			if (this.product != null) {
				this.connectionPipeBindingSource.DataSource = this.product.Product.PlannedConnectionPipes;
				this.connectionPipeBindingSource.ResetBindings(false);
				(this.product.Product as EurovalProduct).ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out errorMsg);
			} else {
				this.connectionPipeBindingSource.DataSource = null;
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
				ignoreRim++;
				ignoreCalculationType++;

				EurovalProduct evProduct = this.product.Product as EurovalProduct;

				bool showHeat = this.product.RequestedHeatLoad > 0;
				bool showCool = this.product.RequestedCoolLoad > 0;
				bool showRim = evProduct.PlannedAreaRim > 0;
				bool showResidence = true;

				this.lblQSollHeat.Visible = showHeat;
				this.lblQkSollHeat.Visible = showHeat;
				this.lblQfbhHeat.Visible = showHeat;
				this.lblQRestHeat.Visible = showHeat;
				this.lblRimVaHeat.Visible = showHeat && showRim;
				this.lblRimBHeat.Visible = showHeat && showRim;
				this.lblRimTfbHeat.Visible = showHeat && showRim;
				this.lblRimQHeat.Visible = showHeat && showRim;
				this.lblResidenceVaHeat.Visible = showHeat && showResidence;
				this.lblResidenceAHeat.Visible = showHeat && showResidence;
				this.lblResidenceTfbHeat.Visible = showHeat && showResidence;
				this.lblResidenceQHeat.Visible = showHeat && showResidence;
				this.lblConnectionAHeat.Visible = showHeat;
				this.lblConnectionQHeat.Visible = showHeat;
				this.lblCircuitCountHeat.Visible = showHeat;
				this.lblPipeLengthHeat.Visible = showHeat;
				this.lblMhHeat.Visible = showHeat;
				this.lblDeltaPHeat.Visible = showHeat;
				this.lblSpreizungHeat.Visible = showHeat;

				this.lblQSollCool.Visible = showCool;
				this.lblQkSollCool.Visible = showCool;
				this.lblQfbhCool.Visible = showCool;
				this.lblQRestCool.Visible = showCool;
				this.lblRimVaCool.Visible = showCool && showRim;
				this.lblRimBCool.Visible = showCool && showRim;
				this.lblRimTfbCool.Visible = showCool && showRim;
				this.lblRimQCool.Visible = showCool && showRim;
				this.lblResidenceVaCool.Visible = showCool && showResidence;
				this.lblResidenceACool.Visible = showCool && showResidence;
				this.lblResidenceTfbCool.Visible = showCool && showResidence;
				this.lblResidenceQCool.Visible = showCool && showResidence;
				this.lblConnectionACool.Visible = showCool;
				this.lblConnectionQCool.Visible = showCool;
				this.lblCircuitCountCool.Visible = showCool;
				this.lblPipeLengthCool.Visible = showCool;
				this.lblMhCool.Visible = showCool;
				this.lblDeltaPCool.Visible = showCool;
				this.lblSpreizungCool.Visible = showCool;

				this.rbCalculateHeat.Enabled = this.product.RequestedHeatLoad > 0;
				this.rbCalculateCool.Enabled = this.product.RequestedCoolLoad > 0;
				this.rbCalculateBoth.Enabled = this.product.RequestedHeatLoad > 0 && this.product.RequestedCoolLoad > 0;
				this.numCorners.Enabled = evProduct.PlannedAreaRim > 0;
				this.cmbRimType.Enabled = evProduct.PlannedAreaRim > 0;

				this.numArea.MaxValue = (decimal)evProduct.AvailableFloorArea;
				this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
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

				if ((skipFields & FieldEnum.CALCULATION_TYPE) == FieldEnum.NONE) {
					this.rbCalculateHeat.Checked = this.product.CalculateHeat && !this.product.CalculateCool;
					this.rbCalculateCool.Checked = this.product.CalculateCool && !this.product.CalculateHeat;
					this.rbCalculateBoth.Checked = this.product.CalculateHeat && this.product.CalculateCool;
				}

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
					this.lblRimTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatRim, 1).ToString();
					this.lblRimTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolRim, 1).ToString();
					this.lblRimQHeat.Text = evProduct.PlannedHeatLoadRim.ToString();
					this.lblRimQCool.Text = evProduct.PlannedCoolLoadRim.ToString();
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
					this.lblResidenceTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatResidence, 1).ToString();
					this.lblResidenceTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolResidence, 1).ToString();
					this.lblResidenceQHeat.Text = evProduct.PlannedHeatLoadResidence.ToString();
					this.lblResidenceQCool.Text = evProduct.PlannedCoolLoadResidence.ToString();
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
				this.lblCircuitCountHeat.Text = evProduct.PlannedCircuits.ToString();
				this.lblCircuitCountCool.Text = evProduct.PlannedCircuits.ToString();
				this.lblPipeLengthHeat.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
				this.lblPipeLengthCool.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
				this.lblMhHeat.Text = Math.Round(evProduct.PlannedMhHeat, 1).ToString();
				this.lblMhCool.Text = Math.Round(evProduct.PlannedMhCool, 1).ToString();
				this.lblDeltaPHeat.Text = Math.Round(evProduct.PlannedDeltaRhoHeat, 1).ToString();
				this.lblDeltaPCool.Text = Math.Round(evProduct.PlannedDeltaRhoCool, 1).ToString(); ;
				this.lblSpreizungHeat.Text = Math.Round(evProduct.PlannedSpreizungHeat, 1).ToString();
				this.lblSpreizungCool.Text = Math.Round(evProduct.PlannedSpreizungCool, 1).ToString();

				if (evProduct.PlannedConnection == null) {
					this.txtDistributor.Text = "";
				} else {
					this.txtDistributor.Text = evProduct.PlannedConnection.ToString();
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
				ignoreRim--;
				ignoreCorners--;
				ignoreLayDistance--;
				ignoreRim--;
				ignoreCalculationType--;
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
				(this.product.Product as EurovalProduct).PlannedFloorAreaPercentage = (float)this.numAreaPercentage.Value;
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
				(this.product.Product as EurovalProduct).PlannedFloorArea = (float)this.numArea.Value;
				this.numAreaPercentage.Value = (decimal)(this.product.Product as EurovalProduct).PlannedFloorAreaPercentage;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
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
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
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
			form.SelectedConstruction = (this.product.Product as EurovalProduct).PlannedFloorConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as EurovalProduct).PlannedFloorConstruction = form.SelectedConstruction;
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
			form.SelectedConstruction = (this.product.Product as EurovalProduct).PlannedInsulationConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as EurovalProduct).PlannedInsulationConstruction = form.SelectedConstruction;
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
				(this.product.Product as EurovalProduct).PlannedRoomTemperatureBelowHeat = (float)this.numRoomTemperatureBelowHeat.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_HEAT);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numRoomTemperatureBelowCool_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowCool == 0) {
				(this.product.Product as EurovalProduct).PlannedRoomTemperatureBelowCool = (float)this.numRoomTemperatureBelowCool.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_COOL);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numRim_ValueChanged(object sender, EventArgs e) {
			if (ignoreRim == 0) {
				(this.product.Product as EurovalProduct).PlannedRimLength = (float)this.numRim.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.RIM_LENGTH);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void numCorners_ValueChanged(object sender, EventArgs e) {
			if (ignoreCorners == 0) {
				(this.product.Product as EurovalProduct).PlannedRimCorners = (int)this.numCorners.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.CORNERS);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void cmbLayDistance_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreLayDistance == 0) {
				(this.product.Product as EurovalProduct).RequestedLayDistance = (this.cmbLayDistance.SelectedItem as LayDistanceItem).layDistance;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.LAY_DISTANCE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void cmbRimType_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreRimType == 0) {
				(this.product.Product as EurovalProduct).RequestedRimType = (this.cmbRimType.SelectedItem as RimTypeItem).rimType;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
				this.UpdateControl(FieldEnum.RIM_TYPE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}

		}

		private void rbCalculationType_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationType == 0) {
				this.product.CalculateHeat = this.rbCalculateHeat.Checked || this.rbCalculateBoth.Checked;
				this.product.CalculateCool = this.rbCalculateCool.Checked || this.rbCalculateBoth.Checked;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
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
			SelectConnectionForProductForm form = new SelectConnectionForProductForm(Project.Instance.Floors[0]);
			form.SelectedConnection = (this.product.Product as EurovalProduct).PlannedConnection;
			if (form.ShowDialog() == DialogResult.OK) {
				(this.product.Product as EurovalProduct).PlannedConnection = form.SelectedConnection;
			}
			form.Dispose();
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, out this.errorMsg);
			this.UpdateControl(FieldEnum.COOL_LOAD);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void dgvConnectionPipes_CellParsing(object sender, DataGridViewCellParsingEventArgs e) {
			if (e.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) {
				this.product = this.Tag as PlannedProduct;
				foreach (Floor f in Project.Instance.Floors) {
					if (f.Rooms.Contains(this.product.Product.AssociatedRoom)) {
						foreach (Room r in f.Rooms) {
							if (r.ToString().Equals(e.Value)) {
								e.Value = r;
								e.ParsingApplied = true;
								return;
							}
						}
					}
				}
			} else if (e.ColumnIndex == productDataGridViewComboBoxColumn.DisplayIndex) {
				this.product = this.Tag as PlannedProduct;
				foreach (Floor f in Project.Instance.Floors) {
					if (f.Rooms.Contains(this.product.Product.AssociatedRoom)) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct p in r.PlannedProducts) {
								if (p.ToString().Equals(e.Value)) {
									e.Value = p;
									e.ParsingApplied = true;
									return;
								}
							}
						}
					}
				}
			}
		}

		private void dgvConnectionPipes_DataError(object sender, DataGridViewDataErrorEventArgs e) {
			string test = e.Exception.ToString();
		}

		private void dgvConnectionPipes_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}


		void roomSelectionButton_Click(object sender, EventArgs e) {
			if (dgvConnectionPipes.CurrentCell.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) {
				List<Room> rooms = new List<Room>();
				foreach (Floor f in Project.Instance.Floors) {
					if (f.Rooms.Contains(this.product.Product.AssociatedRoom)) {
						foreach (Room r in f.Rooms) {
							if (!this.product.Product.AssociatedRoom.Equals(r)) {
								rooms.Add(r);
							}
						}
					}
				}
				SelectRoomForm form = new SelectRoomForm(rooms);
				if (form.ShowDialog().Equals(DialogResult.OK)) {
					Room room = form.SelectedRoom;
					if (dgvConnectionPipes.CurrentCell.Value != room) {
						dgvConnectionPipes.CurrentCell.Value = room;
						this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[productDataGridViewComboBoxColumn.DisplayIndex].Value = null;
					}
				}
				form.Dispose();
			} else if (dgvConnectionPipes.CurrentCell.ColumnIndex == productDataGridViewComboBoxColumn.DisplayIndex) {
				List<PlannedProduct> products = new List<PlannedProduct>();

				Room r = this.dgvConnectionPipes.Rows[dgvConnectionPipes.CurrentCell.RowIndex].Cells[roomDataGridViewComboBoxColumn.DisplayIndex].Value as Room;
				// product items
				if (r != null) {
					foreach (PlannedProduct p in r.PlannedProducts) {
						products.Add(p);
					}

					SelectPlannedProduct form = new SelectPlannedProduct(products);
					if (form.ShowDialog().Equals(DialogResult.OK)) {
						PlannedProduct product = form.SelectedPlannedProduct;
						dgvConnectionPipes.CurrentCell.Value = product;
					}
					form.Dispose();
				}
			}
		}

		private void dgvConnectionPipes_CellEnter(object sender, DataGridViewCellEventArgs e) {
			if (((e.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) || (e.ColumnIndex == productDataGridViewComboBoxColumn.DisplayIndex)) && e.RowIndex >= 0) {
				Rectangle rect = dgvConnectionPipes.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int width = dgvConnectionPipes.CurrentCell.Size.Width;
                roomSelectionButton.Location = new Point(rect.X + width - roomSelectionButton.Width, rect.Y);
				roomSelectionButton.Height = dgvConnectionPipes.Rows[e.RowIndex].Height;
				roomSelectionButton.Show();
			}
		}

		private void dgvConnectionPipes_CellLeave(object sender, DataGridViewCellEventArgs e) {
			roomSelectionButton.Hide();
		}

		private void dgvConnectionPipes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Delete && this.dgvConnectionPipes.SelectedCells.Count == 1 &&
				this.dgvConnectionPipes.SelectedRows.Count == 0 && this.dgvConnectionPipes.SelectedCells[0].Value != null) {
				DataGridViewCell cell = this.dgvConnectionPipes.SelectedCells[0];
				cell.Value = null;
				if (cell.ColumnIndex == roomDataGridViewComboBoxColumn.DisplayIndex) {
					this.dgvConnectionPipes.Rows[cell.RowIndex].Cells[productDataGridViewComboBoxColumn.DisplayIndex].Value = null;
				}
			}
		}

		private void dgvConnectionPipes_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

	}
}
