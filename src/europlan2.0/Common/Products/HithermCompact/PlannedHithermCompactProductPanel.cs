using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {
	public partial class PlannedHithermCompactProductPanel : UserControl, IEditorUserControl {
		private PlannedProduct product = null;

		private bool gridContentChanged = false;
		private bool updateOngoing = false;

		public PlannedHithermCompactProductPanel() {
			InitializeComponent();

			this.SetLanguage();

			this.UpdateRegisterItems(false);

			this.cmbType.Items.Add(Product.ProductType.WH);
			this.cmbType.Items.Add(Product.ProductType.DH);
			//this.cmbType.Items.Add(Product.ProductType.DSH);
		}

		private void SetLanguage() {
			this.btnRestwaerme.Text = EuroplanRes.PlannedProductPanel_RestwaermeUebernehmen;
			this.btnRestkaelte.Text = EuroplanRes.PlannedProductPanel_RestkaelteUebernehmen;

			this.lblAreaPercentage.Text = EuroplanRes.Unit_Prozent; //"%";
			this.lblCoolLoadPercentage.Text = EuroplanRes.Unit_Prozent; //"%";
			this.lblHeatLoadPercentage.Text = EuroplanRes.Unit_Prozent; //"%";
			this.lblAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblRestAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblAvailableAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblNecessaryAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblCoveredAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblNecessaryWaermestromdichteUnit.Text = EuroplanRes.Unit_WattProQm; //"W/m²";
			this.lblAvgqCoolUnit.Text = EuroplanRes.Unit_WattProQm; //"W/m²";
			this.lblAvgqHeatUnit.Text = EuroplanRes.Unit_WattProQm; //"W/m²";
			this.lblDruckverlustCoolUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblDruckverlustHeatUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblDurchflussCoolUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblDurchflussHeatUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblCoolLoadUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblHeatLoadUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQCoolRestUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQCoolDiffUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQCoolUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQHeatRestUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQHeatDiffUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQHeatUnit.Text = EuroplanRes.Unit_Watt; //"W";

			string wattUnit = EuroplanRes.Unit_Watt;
			this.lblCoolLoadTotal.Text = "(0 " + wattUnit + ")";
			this.lblHeatLoadTotal.Text = "(0 " + wattUnit + ")";
			this.lblRest.Text = EuroplanRes.PlannedHithermProductPanel_Rest + " ()";
			this.lblHk.Text = EuroplanRes.PlannedHithermProductPanel_Heizkreis + " 1:";

			this.chkCoverCoolLoad.Text = EuroplanRes.PlannedProductPanel_KuehllastDecken; //"Kühllast decken";
			this.lblCoolLoadTxt.Text = EuroplanRes.PlannedProductPanel_GewuenschteKuehlleistung; //"gewünschte Kühlleistung:";
			this.chkCoverHeatLoad.Text = EuroplanRes.PlannedProductPanel_WaermebedarfDecken; //"Wärmebedarf decken";
			this.lblHeatLoadTxt.Text = EuroplanRes.PlannedProductPanel_GewuenschteHeizleistung; //"gewünschte Heizleistung:";
			this.pageInput.Text = EuroplanRes.PlannedProductPanel_EingabedatenSeite; //"Eingabedaten";
			this.pageCircuit.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenSeite; //"Anbindeleitungen";
			this.groupBox10.Text = EuroplanRes.PlannedProductPanel_AnbindeleitugenGruppe; //"Anbindeleitungen";
			this.lblDistributor.Text = EuroplanRes.PlannedProductPanel_Verteileranschluss; //"Verteileranschluß:";
			this.chkStellAntriebe.Text = EuroplanRes.PlannedProductPanel_Stellantriebe; //"Stellantrieb(e) verwenden";
			this.pageAuslegung.Text = EuroplanRes.PlannedProductPanel_AuslegungSeite; //"Auslegung";
			this.btnConnectionPipes.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenBearbeiten; //"Anbindeleitungen bearbeiten";
			this.lblAreaTxt.Text = EuroplanRes.PlannedProductPanel_GesamteFlaeche; //"gesamte Fläche:";

			this.lblCalculateMode.Text = EuroplanRes.PlannedProductPanel_Verwendungszweck;
			this.rbHeat.Text = EuroplanRes.PlannedProductPanel_Heizen;
			this.rbCool.Text = EuroplanRes.PlannedProductPanel_Kuehlen;
			this.rbHeatAndCool.Text = EuroplanRes.PlannedProductPanel_HeizenUndKuehlen;

			this.label7.Text = EuroplanRes.PlannedHithermProductPanel_Typ; //"Typ:";
			this.pageConstructions.Text = EuroplanRes.PlannedHithermProductPanel_KonstruktionenSeite; //"Konstruktionen";
			this.lblRestAreaTitle.Text = EuroplanRes.PlannedHithermProductPanel_UebrigeFlaeche; //"Übrige Fläche:";
			this.lblAvailableAreaTitle.Text = EuroplanRes.PlannedHithermProductPanel_VerfuegbareFlaeche; //"Verfügbare Fläche:";
			this.lblNecessaryAreaTitle.Text = EuroplanRes.PlannedHithermProductPanel_BenoetigteFlaeche; //"benötigte Fläche:";
			this.lblNecessaryWaermestromdichteTitle.Text = EuroplanRes.PlannedHithermProductPanel_BenoetigteWaermestromdichte; //"benötigte Wärmestromd.:";
			this.lblCoveredAreaTitle.Text = EuroplanRes.PlannedHithermProductPanel_BelegteFlaeche; //"Belegte Fläche:";
			this.registerTypeDataGridViewTextBoxColumn.HeaderText = EuroplanRes.PlannedHithermProductPanel_RegisterTyp; //"Register-\ntyp";
			this.wallDataGridViewTextBoxColumn.HeaderText = EuroplanRes.PlannedHithermProductPanel_Konstruktion; //"Konstr.";
			this.heizkreisDataGridViewTextBoxColumn.HeaderText = EuroplanRes.PlannedHithermProductPanel_HeizkreisAbkuerzung; //"HK";
			this.pipeHorizontalDataGridViewTextBoxColumn.HeaderText = EuroplanRes.PlannedHithermProductPanel_LeitungWaagrecht; //"Leitung\nwaagr.\n(m)";
			this.pipeVerticalDataGridViewTextBoxColumn.HeaderText = EuroplanRes.PlannedHithermProductPanel_LeitungSenkrecht; //"Leitung\nsenkr.\n(m)";
			this.label32.Text = EuroplanRes.PlannedHithermProductPanel_ZusaetzlicheInformationen; //"Zus. Informationen:";
			this.label16.Text = EuroplanRes.PlannedHithermProductPanel_DifferenzLeistung; //"Differenz zur erwarteten Leistung:";
			this.label17.Text = EuroplanRes.PlannedHithermProductPanel_ErreichteLeistung; //"Erreichte Leistung:";
			this.lblCool.Text = EuroplanRes.PlannedHithermProductPanel_Kuehlbetrieb; //"Kühlbetrieb";
			this.lblHeat.Text = EuroplanRes.PlannedHithermProductPanel_Heizbetrieb; //"Heizbetrieb";
			this.label4.Text = EuroplanRes.PlannedHithermProductPanel_Berechnungsergebnisse; //"Berechnungsergebnisse:";
			this.label9.Text = EuroplanRes.PlannedHithermProductPanel_Druckverlust; //"Druckverlust:";
			this.label6.Text = EuroplanRes.PlannedHithermProductPanel_Wassermenge; //"Wassermenge:";
			this.label5.Text = EuroplanRes.PlannedHithermProductPanel_DurchnittlicheWaermestromdichte; //"Durchschn. Wärmestromdichte:";

			this.registerCountDataGridViewTextBoxColumn.HeaderText = EuroplanRes.PlannedHithermCompactProductPanel_AnzahlRegister;
		}

		private void UpdateRegisterItems(bool ceiling) {
			this.registerTypeDataGridViewTextBoxColumn.Items.Clear();
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par);
			this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par);
			//if (!ceiling) {
				this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds);
				this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds);
				this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds);
				this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds);
				this.registerTypeDataGridViewTextBoxColumn.Items.Add(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds);
			//}
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
			LAYOUT_TYPE = 524288
		}


		private string errorMsg = null;

		public void UpdateControl(bool resetUserInterface) {
			this.product = this.Tag as PlannedProduct;
			if (resetUserInterface) {
				this.tabs.SelectedTab = this.pageInput;
			}
			this.connectionPipePanel.Update(this.product);
			this.chkStellAntriebe.Checked = this.product.Product.StellMotore;
			if (this.product != null) {
				this.UpdateRegisterItems((this.product.Product as HithermCompactProduct).HithermCompactType == Product.ProductType.DH);
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
		private int ignoreCalculationMode = 0;

		private void UpdateControl(FieldEnum skipFields) {
			updateOngoing = true;
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
				ignoreCalculationMode++;

				HithermCompactProduct hcp = this.product.Product as HithermCompactProduct;
				bool graphicalMode = false;
				if (this.product.Product.GraphicalMode.HasValue) {
					graphicalMode = this.product.Product.GraphicalMode.Value;
				} else {
					if (this.product.Product.AssociatedRoom.AssociatedPlan != null && this.product.Product.AssociatedRoom.RoomCoordinates.Count > 0) {
						if (hcp.PlannedRegisterArea > 0) {
							graphicalMode = false;
							this.product.Product.GraphicalMode = false;
						} else {
							graphicalMode = true;
							this.product.Product.GraphicalMode = true;
						}
					} else {
						graphicalMode = false;
						this.product.Product.GraphicalMode = false;
					}
				}
				if ((skipFields & FieldEnum.LAYOUT_TYPE) == FieldEnum.NONE) {
					if (graphicalMode) {
						this.rbLayoutGraphical.Checked = true;
					} else {
						this.rbLayoutTable.Checked = true;
					}
				}
				dgvRegisters.Enabled = !graphicalMode;
				this.btnGrafischeAuslegung.Enabled = graphicalMode;
				int selectedCircuit = (this.dgvRegisters.SelectedCells.Count > 0 &&
					this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem is HithermCompactRegister) ?
					(this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem as HithermCompactRegister).Heizkreis : -1;

				//bool showHeat = this.product.Product.AssociatedRoom.HeatLoad > 0 || this.product.Product.AssociatedRoom.CoolLoad <= 0;
				//bool showCool = this.product.Product.AssociatedRoom.CoolLoad > 0;
				bool showHeat = this.product.RequestedHeatLoad > 0;
				bool showCool = this.product.RequestedCoolLoad > 0;
				bool showHeatCircuit = selectedCircuit >= 0 && showHeat;
				bool showCoolCircuit = selectedCircuit >= 0 && showCool;
				bool showRestArea = hcp.HithermCompactType == Product.ProductType.FBH || hcp.HithermCompactType == Product.ProductType.DH;

				lblHeat.Visible = showHeat;
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
				lblCool.Visible = showCool;
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

				int xDiff = this.lblNecessaryArea.Top - this.lblNecessaryWaermestromdichte.Top;
				if (showRestArea) {
					this.lblCoveredAreaTitle.Top = this.lblAvailableAreaUnit.Top + xDiff;
					this.lblCoveredArea.Top = this.lblAvailableArea.Top + xDiff;
					this.lblCoveredAreaUnit.Top = this.lblAvailableAreaUnit.Top + xDiff;
					this.lblNecessaryWaermestromdichteTitle.Top = this.lblRestAreaTitle.Top + xDiff;
					this.lblNecessaryWaermestromdichte.Top = this.lblRestArea.Top + xDiff;
					this.lblNecessaryWaermestromdichteUnit.Top = this.lblRestAreaUnit.Top + xDiff;
					this.lblNecessaryAreaTitle.Top = this.lblNecessaryWaermestromdichteTitle.Top + xDiff;
					this.lblNecessaryArea.Top = this.lblNecessaryWaermestromdichte.Top + xDiff;
					this.lblNecessaryAreaUnit.Top = this.lblNecessaryWaermestromdichteUnit.Top + xDiff;
					this.lblAvailableAreaTitle.Visible = true;
					this.lblAvailableArea.Visible = true;
					this.lblAvailableAreaUnit.Visible = true;
					this.lblRestAreaTitle.Visible = true;
					this.lblRestArea.Visible = true;
					this.lblRestAreaUnit.Visible = true;
					this.lineInfo.Height = 135;
				} else {
					this.lblAvailableAreaTitle.Visible = false;
					this.lblAvailableArea.Visible = false;
					this.lblAvailableAreaUnit.Visible = false;
					this.lblRestAreaTitle.Visible = false;
					this.lblRestArea.Visible = false;
					this.lblRestAreaUnit.Visible = false;
					this.lblCoveredAreaTitle.Top = this.lblAvailableAreaUnit.Top;
					this.lblCoveredArea.Top = this.lblAvailableArea.Top;
					this.lblCoveredAreaUnit.Top = this.lblAvailableAreaUnit.Top;
					this.lblNecessaryWaermestromdichteTitle.Top = this.lblCoveredAreaTitle.Top + xDiff;
					this.lblNecessaryWaermestromdichte.Top = this.lblCoveredArea.Top + xDiff;
					this.lblNecessaryWaermestromdichteUnit.Top = this.lblCoveredAreaUnit.Top + xDiff;
					this.lblNecessaryAreaTitle.Top = this.lblNecessaryWaermestromdichteTitle.Top + xDiff;
					this.lblNecessaryArea.Top = this.lblNecessaryWaermestromdichte.Top + xDiff;
					this.lblNecessaryAreaUnit.Top = this.lblNecessaryWaermestromdichteUnit.Top + xDiff;
					this.lineInfo.Height = 100;
				}

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

				if (this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT) {
					rbHeat.Checked = true;
				} else if (this.product.Product.CalculateMode == Product.CalculateModeEnum.COOL) {
					rbCool.Checked = true;
				} else if (this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL) {
					rbHeatAndCool.Checked = true;
				}

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

				if ((this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT ||
					this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL)) {
					this.chkCoverHeatLoad.Enabled = true;
					this.btnRestwaerme.Enabled = true;
					if ((skipFields & (FieldEnum.HEAT_LOAD | FieldEnum.HEAT_LOAD_PERCENTAGE)) == FieldEnum.NONE) {
						if (this.product.CoverHeatLoad) {
							this.chkCoverHeatLoad.Checked = true;
							this.numHeatLoad.Enabled = false;
							this.numHeatLoadPercentage.Enabled = false;
						} else {
							this.chkCoverHeatLoad.Checked = false;
							this.numHeatLoad.Enabled = this.product.NecessaryHeatLoad > 0;
							this.numHeatLoadPercentage.Enabled = this.product.NecessaryHeatLoad > 0;
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
					this.btnRestwaerme.Enabled = false;
					this.chkCoverHeatLoad.Checked = false;
				}
				this.numCoolLoad.MaxValue = (decimal)this.product.NecessaryCoolLoad;
				this.numCoolLoadPercentage.MaxValue = (decimal)(hcp.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / hcp.AssociatedRoom.NormalizedCoolLoad);
				if ((this.product.Product.CalculateMode == Product.CalculateModeEnum.COOL ||
					this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL)) {
					this.chkCoverCoolLoad.Enabled = true;
					this.btnRestkaelte.Enabled = true;
					if ((skipFields & (FieldEnum.COOL_LOAD | FieldEnum.COOL_LOAD_PERCENTAGE)) == FieldEnum.NONE) {
						if (this.product.CoverCoolLoad) {
							this.chkCoverCoolLoad.Checked = true;
							this.numCoolLoad.Enabled = false;
							this.numCoolLoadPercentage.Enabled = false;
						} else {
							this.chkCoverCoolLoad.Checked = false;
							this.numCoolLoad.Enabled = this.product.NecessaryCoolLoad > 0;
							this.numCoolLoadPercentage.Enabled = this.product.NecessaryCoolLoad > 0;
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
					this.btnRestkaelte.Enabled = false;
					this.chkCoverCoolLoad.Checked = false;
				}
				this.lblHeatLoadTotal.Text = "(" + this.product.Product.AssociatedRoom.NormalizedHeatLoad.ToString() + " " + EuroplanRes.Unit_Watt + ")";
				this.lblCoolLoadTotal.Text = "(" + this.product.Product.AssociatedRoom.NormalizedCoolLoad.ToString() + " " + EuroplanRes.Unit_Watt + ")";
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

				lblRest.Text = EuroplanRes.PlannedHithermProductPanel_Rest + " (" + this.product.Product.AssociatedRoom.ToString() + ")";
				lblQHeat.Text = Math.Round(this.product.PlannedHeatLoad, 0).ToString();
				lblQHeatDiff.Text = Math.Round(qDiffHeat, 0).ToString("+0;-0");
				lblQHeatRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenHeatLoad, 0).ToString("+0;-0");
				lblQCool.Text = Math.Round(this.product.PlannedCoolLoad, 0).ToString();
				lblQCoolDiff.Text = Math.Round(qDiffCool, 0).ToString("+0;-0");
				lblQCoolRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenCoolLoad, 0).ToString("+0;-0");
				double area = hcp.PlannedRegisterArea;
				lblCoveredArea.Text = Math.Round(area, 2).ToString();
				lblAvailableArea.Text = Math.Round(this.product.PlannedArea.HasValue ? this.product.PlannedArea.Value : 0, 2).ToString();
				lblRestArea.Text = Math.Round((this.product.PlannedArea.HasValue ? this.product.PlannedArea.Value : 0) - area, 2).ToString();
				lblNecessaryWaermestromdichte.Text = (area > 0) ? Math.Round(this.product.RequestedHeatLoad / area, 2).ToString() : "--";
				lblNecessaryArea.Text = (hcp.PlannedHeatLoad > 0 && area > 0) ? Math.Round(this.product.RequestedHeatLoad / (hcp.PlannedHeatLoad / area), 2).ToString() : "--";
				if (selectedCircuit >= 0 && this.dgvRegisters.SelectedCells.Count > 0) {
					HithermCompactCircuit hc = hcp.GetCircuitForRegister(this.dgvRegisters.Rows[this.dgvRegisters.SelectedCells[0].RowIndex].DataBoundItem as HithermCompactRegister);
					if (hc != null) {
						lblHk.Text = EuroplanRes.PlannedHithermProductPanel_Heizkreis + " " + selectedCircuit.ToString() + ":";
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
					this.hithermWallGrid1.UpdateGrid();
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
				notifications = HithermCompactProduct.GlobalNotificationMessage;
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
				ignoreRegisters--;
				ignoreType--;
				ignoreCalculationMode--;
			}
			// TODO
			updateOngoing = false;
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
			if (gridContentChanged) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				gridContentChanged = false;
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
			if (this.product.Product.Connections != null && this.product.Product.Connections.Count > 0) {
				if (MessageBox.Show("Für dieses Produkt wurden bereits grafische Anbindeleitungen verplant, wenn Sie den Verteileranschluss ändern werden die grafischen Anbindeleitungen gelöscht", "Grafische Anbindeleitungen löschen", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) {
					return;
				}
				this.product.Product.Connections.Clear();
				List<ConnectionPipe> pipesToDelete = new List<ConnectionPipe>();
				foreach (ConnectionPipe pipe in this.product.Product.PlannedConnectionPipes) {
					pipesToDelete.Add(pipe);
				}
				foreach (ConnectionPipe pipe in pipesToDelete) {
					this.product.Product.PlannedConnectionPipes.Remove(pipe);
				}
			}
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
			gridContentChanged = true;
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
			this.newRow = e.Row;
			//e.Row.Cells[registerAreaDataGridViewTextBoxColumn.Index].Value = HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std;
			if (registerTypeDataGridViewTextBoxColumn.Items.Count > 0) {
				e.Row.Cells[registerTypeDataGridViewTextBoxColumn.Index].Value = registerTypeDataGridViewTextBoxColumn.Items[0];
			}
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
			SelectHithermWallForm form = new SelectHithermWallForm(true);
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
					//if ((Product.ProductType)this.cmbType.SelectedItem == Product.ProductType.DH) {
					//    if (MessageBox.Show(EuroplanRes.PlannedHithermCompactProductPanel_TypAendernTextDh/*"Wenn Sie den Typ auf Dachschräge ändern, werden alle Register in Parapetauslegung gelöscht"*/, EuroplanRes.PlannedHithermCompactProductPanel_TypAendernTitel/*"Bestätigen"*/, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) {
					//        this.cmbType.SelectedItem = (this.product.Product as HithermCompactProduct).HithermCompactType;
					//        return ;
					//    }
					//    List<HithermCompactRegister> removeRegisters = new List<HithermCompactRegister>();
					//    foreach (HithermCompactCircuit hcc in (this.product.Product as HithermCompactProduct).PlannedCircuits) {
					//        foreach (HithermCompactRegister hcr in hcc.Registers){
					//            if (hcr.IsParapet) {
					//                removeRegisters.Add(hcr);
					//            }
					//        }
					//    }
					//    foreach (HithermCompactRegister hcr in removeRegisters) {
					//        (this.product.Product as HithermCompactProduct).RemoveRegisterFromCircuit(hcr);
					//    }
					//}
					(this.product.Product as HithermCompactProduct).HithermCompactType = (Product.ProductType)this.cmbType.SelectedItem;
					if ((this.product.Product as HithermCompactProduct).HithermCompactType == Product.ProductType.FBH) {
						(this.product.Product as HithermCompactProduct).PlannedFloorArea = this.product.Product.AvailableFloorArea;
					} else if ((this.product.Product as HithermCompactProduct).HithermCompactType == Product.ProductType.DH) {
						(this.product.Product as HithermCompactProduct).PlannedCeilingArea = this.product.Product.AvailableCeilingArea;
					}
					this.UpdateRegisterItems((this.product.Product as HithermCompactProduct).HithermCompactType == Product.ProductType.DH);
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

		private void lstError_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			e.Item.Focused = false;
			e.Item.Selected = false;
		}

		private void tabs_Deselecting(object sender, TabControlCancelEventArgs e) {
			if (e.TabPage == this.pageCircuit && gridContentChanged) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.NONE);
				gridContentChanged = false;
			}
		}

		private void rbHeat_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationMode == 0) {
				if (this.rbHeat.Checked) {
					this.product.Product.CalculateMode = Product.CalculateModeEnum.HEAT;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.UpdateControl(FieldEnum.NONE);
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
				}
			}
		}

		private void rbCool_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationMode == 0) {
				if (this.rbCool.Checked) {
					this.product.Product.CalculateMode = Product.CalculateModeEnum.COOL;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.UpdateControl(FieldEnum.NONE);
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
				}
			}
		}

		private void rbHeatAndCool_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationMode == 0) {
				if (this.rbHeatAndCool.Checked) {
					this.product.Product.CalculateMode = Product.CalculateModeEnum.HEAT_AND_COOL;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.UpdateControl(FieldEnum.NONE);
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
				}
			}
		}

		private void btnRestwaerme_Click(object sender, EventArgs e) {
			this.product.RestwaermeUebernehmen();
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.UpdateControl(FieldEnum.NONE);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnRestkaelte_Click(object sender, EventArgs e) {
			this.product.RestkaelteUebernehmen();
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.UpdateControl(FieldEnum.NONE);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnGrafischeAuslegung_Click(object sender, EventArgs e) {
			HithermCompactPlannerForm form = new HithermCompactPlannerForm(this.product.Product as HithermCompactProduct);
			form.ShowDialog();
			this.UpdateControl(FieldEnum.LAYOUT_TYPE);
			form.Dispose();
			// TODO
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void rbGraphical_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing && (sender as RadioButton).Checked) {
				if (this.product.Product.GraphicalMode.HasValue && this.product.Product.GraphicalMode.Value != rbLayoutGraphical.Checked) {
					// change from graphical to table based
					if (this.product.Product.GraphicalMode.Value) {
						if (!this.product.Product.AllowToSwitchMode) {
							DialogResult result = MessageBox.Show(EuroplanRes.PlannedEurovalProductPanel_Auslegung_Aendern_Grafisch, EuroplanRes.PlannedEurovalProductPanel_Auslegung_Aendern_Titel, MessageBoxButtons.YesNo);
							if (result == DialogResult.No) {
								this.UpdateControl(FieldEnum.NONE);
								return;
							} else {
								HithermCompactProduct hp = this.product.Product as HithermCompactProduct;
								hp.Connections = null;
								foreach (HithermCompactCircuit c in hp.PlannedCircuits) {
									c.Links = null;
									foreach (HithermCompactRegister register in c.Registers) {
										register.ClearGraphicalRepresentation();
									}
								}
							}
						}
						// change from table based to graphical  
					} else {
						if (!this.product.Product.AllowToSwitchMode) {
							DialogResult result = MessageBox.Show(EuroplanRes.PlannedEurovalProductPanel_Auslegung_Aendern_Tabellarisch, EuroplanRes.PlannedEurovalProductPanel_Auslegung_Aendern_Titel, MessageBoxButtons.YesNo);
							if (result == DialogResult.No) {
								this.UpdateControl(FieldEnum.NONE);
								return;
							} else {
								(this.product.Product as HithermProduct).ResetProduct();
							}
						}
					}
				}
				this.product.Product.GraphicalMode = rbLayoutGraphical.Checked;
				this.UpdateControl(FieldEnum.LAYOUT_TYPE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}
	}
}
