using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class PlannedModulKlimaDeckeProductPanel : UserControl, IEditorUserControl {
		private PlannedProduct product = null;

		private bool gridContentChanged = false;
		private bool updateOngoing = false;

		public PlannedModulKlimaDeckeProductPanel() {
			InitializeComponent();

			this.SetLanguage();

			this.cmbType.Items.Add(Product.ProductType.WH);
			this.cmbType.Items.Add(Product.ProductType.DH);
		}

		private void SetLanguage() {
			this.btnRestwaerme.Text = EuroplanRes.PlannedProductPanel_RestwaermeUebernehmen;
			this.btnRestkaelte.Text = EuroplanRes.PlannedProductPanel_RestkaelteUebernehmen;

			this.lblAreaPercentage.Text = EuroplanRes.Unit_Prozent; //"%";
			this.lblCoolLoadPercentage.Text = EuroplanRes.Unit_Prozent; //"%";
			this.lblHeatLoadPercentage.Text = EuroplanRes.Unit_Prozent; //"%";
			this.lblAreaUnheated.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblRestAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblAnbAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblCoveredAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblAvailableAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.label3.Text = EuroplanRes.Unit_Meter; //"m";
			this.lblCoolLoadUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblHeatLoadUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQAnbCoolUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQAnbHeatUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQCoolRestUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQCoolDiffUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQCoolUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQHeatRestUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQHeatDiffUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.lblQHeatUnit.Text = EuroplanRes.Unit_Watt; //"W";
			this.label27.Text = EuroplanRes.Unit_GradCelsius; //"°C";
			this.label7.Text = EuroplanRes.Unit_GradCelsius; //"°C";
			this.lblDruckverlustCoolUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblDruckverlustHeatUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblDurchflussCoolUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblDurchflussHeatUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblAvgqCoolUnit.Text = EuroplanRes.Unit_WattProQm; //"W/m²";
			this.lblAvgqHeatUnit.Text = EuroplanRes.Unit_WattProQm; //"W/m²";

			this.lblCoolLoadTotal.Text = "(0 " + EuroplanRes.Unit_Watt + ")";
			this.lblHeatLoadTotal.Text = "(0 " + EuroplanRes.Unit_Watt + ")";
			this.lblRest.Text = EuroplanRes.PlannedModulProductPanel_Rest + " ()";
			this.lblHk.Text = EuroplanRes.PlannedModulKlimaDeckeProductPanel_Heizkreis + " 1:";

			this.btnRemoveSubarea.Text = EuroplanRes.General_Minus;
			this.btnAddSubarea.Text = EuroplanRes.General_Plus;
			this.btnRemoveRow.Text = EuroplanRes.General_Minus;
			this.btnAddRow.Text = EuroplanRes.General_Plus;
			this.btnRemoveHk.Text = EuroplanRes.General_Minus;
			this.btnAddHk.Text = EuroplanRes.General_Plus;

			this.chkCoverCoolLoad.Text = EuroplanRes.PlannedProductPanel_KuehllastDecken; //"Kühllast decken";
			this.lblCoolLoadTxt.Text = EuroplanRes.PlannedProductPanel_GewuenschteKuehlleistung; //"gewünschte Kühlleistung:";
			this.chkCoverHeatLoad.Text = EuroplanRes.PlannedProductPanel_WaermebedarfDecken; //"Wärmebedarf decken";
			this.lblHeatLoadTxt.Text = EuroplanRes.PlannedProductPanel_GewuenschteHeizleistung; //"gewünschte Heizleistung:";
			this.label28.Text = EuroplanRes.PlannedProductPanel_TemperaturOberhalbKuehl; //"Temperatur oberhalb (Kühlbetrieb):";
			this.label8.Text = EuroplanRes.PlannedProductPanel_TemperaturOberhalbHeiz; //"Temperatur oberhalb (Heizbetrieb):";
			this.lblInsulationConstruction.Text = EuroplanRes.PlannedProductPanel_Daemmkonstruktion; //"Wärmedämmkonstruktion:";
			this.lblCeilingConstruction.Text = EuroplanRes.PlannedProductPanel_Deckenkonstruktion; //"Deckenkonstruktion:";
			this.pageInput.Text = EuroplanRes.PlannedProductPanel_EingabedatenSeite; //"Eingabedaten";
			this.pageCircuit.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenSeite; //"Anbindeleitungen";
			this.groupBox10.Text = EuroplanRes.PlannedProductPanel_AnbindeleitugenGruppe; //"Anbindeleitungen";
			this.chkStellAntriebe.Text = EuroplanRes.PlannedProductPanel_Stellantriebe; //"Stellantrieb(e) verwenden";
			this.lblDistributor.Text = EuroplanRes.PlannedProductPanel_Verteileranschluss; //"Verteileranschluß:";
			this.pageConstruction.Text = EuroplanRes.PlannedProductPanel_AuslegungSeite; //"Auslegung";
			this.btnConnectionPipes.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenBearbeiten; //"Anbindeleitungen bearbeiten";
			this.lblAreaTxt.Text = EuroplanRes.PlannedProductPanel_GesamteFlaeche; //"gesamte Fläche:";

			this.label4.Text = EuroplanRes.PlannedModulProductPanel_Berechnungsergebnisse; //"Berechnungsergebnisse:";
			this.label9.Text = EuroplanRes.PlannedModulProductPanel_Druckverlust; //"Druckverlust:";
			this.label6.Text = EuroplanRes.PlannedModulProductPanel_Wassermenge; //"Wassermenge:";
			this.label5.Text = EuroplanRes.PlannedModulProductPanel_DurchschnittlicheWaermestromdichte; //"Durchschn. Wärmestromdichte:";
			this.label11.Text = EuroplanRes.PlannedModulProductPanel_Kuehlbetrieb; //"Kühlbetrieb";
			this.label10.Text = EuroplanRes.PlannedModulProductPanel_Heizbetrieb; //"Heizbetrieb";
			this.lblRestAreaText.Text = EuroplanRes.PlannedModulProductPanel_FlaecheUebrig; //"Übrige Fläche:";
			this.lblAnbAreaText.Text = EuroplanRes.PlannedModulProductPanel_FlaecheAnbindung; //"Fläche Anbindeleitungen:";
			this.lblCoveredAreaText.Text = EuroplanRes.PlannedModulProductPanel_FlaecheBelegt; //"Belegte Fläche:";
			this.lblAvailableAreaText.Text = EuroplanRes.PlannedModulProductPanel_FlaecheVerfuegbar; //"Verfügbare Fläche:";
			this.lblAdditionalInfo.Text = EuroplanRes.PlannedModulProductPanel_ZusaetzlicheInformationen; //"Zus. Informationen:";
			this.label16.Text = EuroplanRes.PlannedModulProductPanel_DifferenzLeistung; //"Differenz zur erwarteten Leistung:";
			this.label17.Text = EuroplanRes.PlannedModulProductPanel_ErreichteLeistung; //"Erreichte Leistung:";
			this.lblAreaUnheatedTxt.Text = EuroplanRes.PlannedModulProductPanel_FlaecheUnbeheizt; //"unbeheizte/ungekühlte Fläche:";
			this.label45.Text = EuroplanRes.PlannedModulProductPanel_LeistungAnbindung; //"Leistung Anbindeleitungen:";

			this.lblCalculateMode.Text = EuroplanRes.PlannedProductPanel_Verwendungszweck;
			this.rbHeat.Text = EuroplanRes.PlannedProductPanel_Heizen;
			this.rbCool.Text = EuroplanRes.PlannedProductPanel_Kuehlen;
			this.rbHeatAndCool.Text = EuroplanRes.PlannedProductPanel_HeizenUndKuehlen;

			this.label2.Text = EuroplanRes.PlannedModulKlimaDeckeProductPanel_SummeVerbindungsleitungen; //"Summe Verbindungsleitungen:";
			this.label12.Text = EuroplanRes.PlannedModulKlimaDeckeProductPanel_Heizkreis; //"Heizkreise";
			this.label15.Text = EuroplanRes.PlannedModulKlimaDeckeProductPanel_Typ; //"Typ:";
			this.label13.Text = EuroplanRes.PlannedModulKlimaDeckeProductPanel_Teilflaechen; //"Teilflächen";
			this.label14.Text = EuroplanRes.PlannedModulKlimaDeckeProductPanel_ErklaerungVerbindeleitungen; //"(Pro Winkel zusätzlich 1m Verbindungsleitung)";
			this.label1.Text = EuroplanRes.PlannedModulKlimaDeckeProductPanel_ParalleleReihen; //"Parallele Reihen im Heizkreis";
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
			ROWS = 65536,
			SUBAREA = 131072,
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
				(this.product.Product as ModulKlimaDeckeProduct).ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
			}
			this.UpdateControl(FieldEnum.NONE);
		}

		private ModulDeckeCircuit selectedCircuit = null;
		private ModulDeckeSubArea selectedSubArea = null;
		private KlimaFlaechenList selectedRow = null;

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
		private int ignoreSubArea = 0;
		private int ignoreType = 0;
		private int ignoreCalculationMode = 0;
		private int ignoreLayoutType = 0;

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
				ignoreRows++;
				ignoreModules++;
				ignoreLengthVerbindungen++;
				ignoreSubArea++;
				ignoreType++;
				ignoreCalculationMode++;
				ignoreLayoutType++;

				ModulKlimaDeckeProduct mdProduct = this.product.Product as ModulKlimaDeckeProduct;

				//bool showHeat = this.product.Product.AssociatedRoom.HeatLoad > 0;
				//bool showCool = this.product.Product.AssociatedRoom.CoolLoad > 0;
				bool showHeat = this.product.RequestedHeatLoad > 0;
				bool showCool = this.product.RequestedCoolLoad > 0;

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
				//lblTempHeat.Visible = showHeat;
				//lblTempHeatUnit.Visible = showHeat;
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
				//lblTempCool.Visible = showCool;
				//lblTempCoolUnit.Visible = showCool;

				if (this.product.Product.AssociatedRoom.AssociatedPlan != null && this.product.Product.AssociatedRoom.CeilingCoordinatesToUse.Count > 0) {
					this.rbLayoutTable.Enabled = true;
					this.rbLayoutGraphical.Enabled = true;
				} else {
					this.rbLayoutTable.Enabled = false;
					this.rbLayoutGraphical.Enabled = false;
				}

				bool graphicalMode = false;
				if (this.product.Product.GraphicalMode.HasValue) {
					graphicalMode = this.product.Product.GraphicalMode.Value;
				} else {
					if (this.product.Product.AssociatedRoom.AssociatedPlan != null && this.product.Product.AssociatedRoom.CeilingCoordinatesToUse.Count > 0) {
						if (mdProduct.ContainsModules) {
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

				if (graphicalMode) {
					this.numArea.Enabled = false;
					this.numAreaPercentage.Enabled = false;
					this.numAreaUnheated.Enabled = false;
					this.dgvModules.Enabled = false;
					this.btnGraphical.Enabled = true;
					(this.product.Product as ModulKlimaDeckeProduct).PlannedCeilingAreaPercentage = 100;
				} else {
					this.numArea.Enabled = true;
					this.numAreaPercentage.Enabled = true;
					this.numAreaUnheated.Enabled = true;
					this.dgvModules.Enabled = true;
					this.btnGraphical.Enabled = false;
				}

				this.numArea.MaxValue = (decimal)mdProduct.AvailableCeilingArea;
				if (mdProduct.AssociatedRoom.Area > 0) {
					this.numAreaPercentage.MaxValue = (decimal)(mdProduct.AvailableCeilingArea * 100 / mdProduct.AssociatedRoom.Area);
				} else {
					this.numAreaPercentage.MaxValue = 100;
				}

				//this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
				this.numAreaUnheated.MaxValue = (decimal)this.product.PlannedArea;
				this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
				this.numHeatLoadPercentage.MaxValue = (decimal)(mdProduct.AssociatedRoom.NormalizedHeatLoad <= 0 ? 0 : this.product.NecessaryHeatLoad * 100 / mdProduct.AssociatedRoom.NormalizedHeatLoad);

				if ((skipFields & FieldEnum.TYPE) == FieldEnum.NONE) {
					this.cmbType.SelectedItem = mdProduct.ModulType;
				}

				bool showArea = mdProduct.ModulType == Product.ProductType.DH || mdProduct.ModulType == Product.ProductType.FBH;
				if (showArea) {
					this.grpPowerArea.Height = 145;
				} else {
					this.grpPowerArea.Height = 94;
				}
				this.lblAreaTxt.Visible = showArea;
				this.numArea.Visible = showArea;
				this.lblAreaUnit.Visible = showArea;
				this.numAreaPercentage.Visible = showArea;
				this.lblAreaPercentage.Visible = showArea;
				this.lblAreaUnheated.Visible = showArea;
				this.numAreaUnheated.Visible = showArea;
				this.lblAreaUnheatedTxt.Visible = showArea;
				this.lblAdditionalInfo.Visible = showArea;
				this.grpAdditionalInfo1.Visible = showArea;
				this.grpAdditionalInfo2.Visible = showArea;
				this.lblAvailableArea.Visible = showArea;
				this.lblAvailableAreaText.Visible = showArea;
				this.lblAvailableAreaUnit.Visible = showArea;
				this.lblCoveredArea.Visible = showArea;
				this.lblCoveredAreaText.Visible = showArea;
				this.lblCoveredAreaUnit.Visible = showArea;
				this.lblAnbArea.Visible = showArea;
				this.lblAnbAreaText.Visible = showArea;
				this.lblAnbAreaUnit.Visible = showArea;
				this.lblRestArea.Visible = showArea;
				this.lblRestAreaText.Visible = showArea;
				this.lblRestAreaUnit.Visible = showArea;

				switch (mdProduct.ModulType) {
					case Product.ProductType.FBH:
						this.numArea.MaxValue = (decimal)mdProduct.AvailableFloorArea;
						this.numAreaPercentage.MaxValue = (decimal)(mdProduct.AvailableFloorArea * 100 / mdProduct.AssociatedRoom.Area);
						break;
					case Product.ProductType.DH:
						this.numArea.MaxValue = (decimal)mdProduct.AvailableCeilingArea;
						if (mdProduct.AssociatedRoom.Area > 0) {
							this.numAreaPercentage.MaxValue = (decimal)(mdProduct.AvailableCeilingArea * 100 / mdProduct.AssociatedRoom.Area);
						} else {
							this.numAreaPercentage.MaxValue = 100;
						}
						break;
					default:
						//this.numArea.MaxValue = (decimal)0;
						//this.numAreaPercentage.MaxValue = (decimal)0;
						break;
				}

				if (this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT) {
					rbHeat.Checked = true;
				} else if (this.product.Product.CalculateMode == Product.CalculateModeEnum.COOL) {
					rbCool.Checked = true;
				} else if (this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL) {
					rbHeatAndCool.Checked = true;
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
				this.numCoolLoadPercentage.MaxValue = (decimal)(mdProduct.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / mdProduct.AssociatedRoom.NormalizedCoolLoad);
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
					this.numArea.Value = Math.Round((decimal)plannedArea, 2);
				}
				if ((skipFields & FieldEnum.AREA_PERCENTAGE) == FieldEnum.NONE) {
					if (mdProduct.AssociatedRoom.Area <= 0) {
						this.numAreaPercentage.Value = 100;
					} else {
						this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / mdProduct.AssociatedRoom.Area), 2);
					}
				}
				if ((skipFields & FieldEnum.AREA_UNHEATED) == FieldEnum.NONE) {
					this.numAreaUnheated.Value = Math.Round((decimal)mdProduct.PlannedAreaUnheated, 2);
				}
				this.txtFloorConstruction.Text = (mdProduct.PlannedCeilingConstruction == null ? "" : mdProduct.PlannedCeilingConstruction.Id + ": " + mdProduct.PlannedCeilingConstruction.LocalizedName);
				this.txtInsulationConstruction.Text = (mdProduct.PlannedInsulationConstruction == null ? "" : mdProduct.PlannedInsulationConstruction.Id + ": " + mdProduct.PlannedInsulationConstruction.LocalizedName);
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_HEAT) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowHeat.Value = Math.Round((decimal)mdProduct.PlannedRoomTemperatureBelowHeat, 2);
				}
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_COOL) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowCool.Value = Math.Round((decimal)mdProduct.PlannedRoomTemperatureBelowCool, 2);
				}
				if ((skipFields & FieldEnum.CIRCUITS) == FieldEnum.NONE) {
					if (this.product.Product.PlannedCircuits.Count == 0) {
						this.product.Product.PlannedCircuits.Add(new ModulDeckeCircuit());
					}
					this.lstCircuits.Items.Clear();
					int count = 1;
					foreach (Circuit c in this.product.Product.PlannedCircuits) {
						lstCircuits.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_HeizkreisAbkuerzung + count++);
					}
					if (lstCircuits.Items.Count > 0) {
						lstCircuits.SelectedIndex = 0;
					}
					if (graphicalMode) {
						btnAddHk.Enabled = false;
						btnRemoveHk.Enabled = false;
					} else {
						btnAddHk.Enabled = lstCircuits.Items.Count < 12;
						btnRemoveHk.Enabled = lstCircuits.Items.Count > 1;
					}
				}
				if (this.lstCircuits.SelectedIndex < 0) {
					this.lstCircuits.SelectedIndex = 0;
				}
				this.selectedCircuit = mdProduct.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit;
				if ((skipFields & FieldEnum.SUBAREA) == FieldEnum.NONE) {
					if (this.selectedCircuit.SubAreas.Count == 0) {
						this.selectedCircuit.SubAreas.Add(new ModulDeckeSubArea());
					}
					this.lstSubarea.Items.Clear();
					int count = 1;
					foreach (ModulDeckeSubArea sa in this.selectedCircuit.SubAreas) {
						lstSubarea.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Teilflaeche + " " + count++);
					}
					if (lstSubarea.Items.Count > 0) {
						lstSubarea.SelectedIndex = 0;
					}
					if (graphicalMode) {
						btnAddSubarea.Enabled = false;
						btnRemoveSubarea.Enabled = false;
					} else {
						btnAddSubarea.Enabled = lstSubarea.Items.Count < 12;
						btnRemoveSubarea.Enabled = lstSubarea.Items.Count > 1;
					}
				}
				if (this.lstSubarea.SelectedIndex < 0) {
					this.lstSubarea.SelectedIndex = 0;
				}
				this.selectedSubArea = this.selectedCircuit.SubAreas[this.lstSubarea.SelectedIndex];
				if ((skipFields & FieldEnum.ROWS) == FieldEnum.NONE) {
					if (this.selectedSubArea.Rows.Count == 0) {
						this.selectedSubArea.Rows.Add(new KlimaFlaechenList());
					}
					this.lstRows.Items.Clear();
					int count = 1;
					foreach (KlimaFlaechenList row in this.selectedSubArea.Rows) {
					    lstRows.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Reihe + " " + count++);
					}
					if (lstRows.Items.Count > 0) {
						lstRows.SelectedIndex = 0;
					}
					//btnAddRow.Enabled = lstCircuits.SelectedIndex >= 0 && lstRows.Items.Count < ModulKlimaDeckeProduct.ConfigMaxModulesInParallel;
					if (graphicalMode) {
						btnAddRow.Enabled = false;
						btnRemoveRow.Enabled = false;
						numLength.Enabled = false;
					} else {
						btnAddRow.Enabled = true;
						btnRemoveRow.Enabled = lstRows.Items.Count > 1;
						numLength.Enabled = lstRows.SelectedIndex >= 0;
					}
				}
				if (this.lstRows.SelectedIndex < 0) {
					this.lstSubarea.SelectedIndex = 0;
				}
				this.selectedRow = this.selectedSubArea.Rows[this.lstRows.SelectedIndex];
				if ((skipFields & FieldEnum.MODULES) == FieldEnum.NONE) {
					dgvModules.Row = this.selectedRow.List;
					if (graphicalMode) {
						numLength.Enabled = false;
					} else {
						numLength.Enabled = this.selectedRow.List.Count > 0;
					}
				}
				if ((skipFields & FieldEnum.LENGTH_VERBINDUNGEN) == FieldEnum.NONE) {
					this.numLength.Value = (decimal)this.selectedRow.LengthVerbindeleitungen;
				}

			    // General
			    double qDiffHeat = this.product.PlannedHeatLoad - this.product.RequestedHeatLoad;
			    double qDiffCool = this.product.PlannedCoolLoad - this.product.RequestedCoolLoad;

				lblHk.Text = EuroplanRes.PlannedModulKlimaDeckeProductPanel_Heizkreis + " " + (lstCircuits.SelectedIndex + 1) + ":";
				lblRest.Text = EuroplanRes.PlannedModulProductPanel_Rest + " (" + this.product.Product.AssociatedRoom.ToString() + ")";
				lblQHeat.Text = Math.Round(this.product.PlannedHeatLoad, 2).ToString();
				lblQAnbHeat.Text = Math.Round(this.product.Product.PlannedHeatLoadAnbindung, 0).ToString();
				lblQHeatDiff.Text = Math.Round(qDiffHeat, 0).ToString("+0;-0");
				lblQHeatRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenHeatLoad, 2).ToString("+0.00;-0.00");
				lblAvgqHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_QHeatPerSqm, 2).ToString();
				lblDurchflussHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_DurchflussHeat, 2).ToString();
				lblDruckverlustHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_DruckverlustHeat, 2).ToString();
				//lblTempHeat.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_FloorTempHeat, 2).ToString();
				lblQCool.Text = Math.Round(this.product.PlannedCoolLoad, 2).ToString();
				lblQAnbCool.Text = Math.Round(this.product.Product.PlannedCoolLoadAnbindung, 0).ToString();
				lblQCoolDiff.Text = Math.Round(qDiffCool, 0).ToString("+0;-0");
				lblQCoolRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenCoolLoad, 2).ToString("+0.00;-0.00");
				lblAvgqCool.Text = (-1.0 * Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_QCoolPerSqm, 2)).ToString();
				lblDurchflussCool.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_DurchflussCool, 2).ToString();
				lblDruckverlustCool.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_DruckverlustCool, 2).ToString();
				//lblTempCool.Text = Math.Round((this.product.Product.PlannedCircuits[lstCircuits.SelectedIndex] as ModulDeckeCircuit).C_FloorTempCool, 2).ToString();
				if (mdProduct.ModulType == Product.ProductType.DH || mdProduct.ModulType == Product.ProductType.FBH) {
					double availableArea;
					if (mdProduct.ModulType == Product.ProductType.DH) {
						availableArea = Math.Round(this.product.Product.PlannedCeilingArea, 2);
					} else {
						availableArea = Math.Round(this.product.Product.PlannedFloorArea, 2);
					}
					double coveredArea = Math.Round((this.product.Product as ModulKlimaDeckeProduct).CoveredArea, 2);
					double anbArea = Math.Round((this.product.Product as ModulKlimaDeckeProduct).PlannedRemoveArea, 2);
					lblAvailableArea.Text = availableArea.ToString();
					lblCoveredArea.Text = coveredArea.ToString();
					lblAnbArea.Text = anbArea.ToString();
					lblRestArea.Text = Math.Round(availableArea - anbArea - coveredArea, 2).ToString();
				}

				if (mdProduct.PlannedConnection == null) {
					this.txtDistributor.Text = "";
				} else {
					this.txtDistributor.Text = mdProduct.PlannedConnection.ToString();
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
				notifications = ModulKlimaDeckeProduct.GlobalNotificationMessage;
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
				ignoreRows--;
				ignoreModules--;
				ignoreLengthVerbindungen--;
				ignoreSubArea--;
				ignoreType--;
				ignoreCalculationMode--;
				ignoreLayoutType--;
			}
			updateOngoing = false;
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
				(this.product.Product as ModulKlimaDeckeProduct).PlannedCeilingAreaPercentage = (float)this.numAreaPercentage.Value;
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
				(this.product.Product as ModulKlimaDeckeProduct).PlannedCeilingArea = (float)this.numArea.Value;
				this.numAreaPercentage.Value = (decimal)(this.product.Product as ModulKlimaDeckeProduct).PlannedCeilingAreaPercentage;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
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
				ModulKlimaDeckeProduct mdProduct = this.product.Product as ModulKlimaDeckeProduct;
				mdProduct.PlannedAreaUnheated = (float)this.numAreaUnheated.Value;
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
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.CeilingConstruction, null);
			form.SelectedConstruction = (this.product.Product as ModulKlimaDeckeProduct).PlannedCeilingConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as ModulKlimaDeckeProduct).PlannedCeilingConstruction = form.SelectedConstruction;
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
			form.SelectedConstruction = (this.product.Product as ModulKlimaDeckeProduct).PlannedInsulationConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as ModulKlimaDeckeProduct).PlannedInsulationConstruction = form.SelectedConstruction;
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
					this.UpdateControl(FieldEnum.NONE);
					this.product.Product.AssociatedRoom.GetFloor().LastInsulationConstruction = form.SelectedConstruction;
				}
			}
			form.Dispose();
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void numRoomTemperatureBelowHeat_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowHeat == 0) {
				(this.product.Product as ModulKlimaDeckeProduct).PlannedRoomTemperatureBelowHeat = (float)this.numRoomTemperatureBelowHeat.Value;
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
				(this.product.Product as ModulKlimaDeckeProduct).PlannedRoomTemperatureBelowCool = (float)this.numRoomTemperatureBelowCool.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_COOL);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}


		private void btnDistributor_Click(object sender, EventArgs e) {
			if (this.product.Product.Connections != null && this.product.Product.Connections.Count > 0) {
				if (MessageBox.Show(EuroplanRes.PlannedEcothermProductPanel_GrafischeAnbindeleitungLoeschenText, EuroplanRes.PlannedEcothermProductPanel_GrafischeAnbindeleitungLoeschenTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) {
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
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.NONE);
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnAddHk_Click(object sender, EventArgs e) {
			this.product.Product.PlannedCircuits.Add(new ModulDeckeCircuit());
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.NONE);
			lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnRemoveHk_Click(object sender, EventArgs e) {
			if (lstCircuits.Items.Count > 1 && lstCircuits.SelectedIndex >= 0) {
				this.product.Product.PlannedCircuits.RemoveAt(lstCircuits.SelectedIndex);
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.NONE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void lstCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreCircuits == 0) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUITS);
			}
		}

		private void btnAddSubarea_Click(object sender, EventArgs e) {
			this.selectedCircuit.SubAreas.Add(new ModulDeckeSubArea());
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.CIRCUITS);
			lstSubarea.SelectedIndex = lstSubarea.Items.Count - 1;
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnRemoveSubarea_Click(object sender, EventArgs e) {
			if (lstSubarea.Items.Count > 1 && lstSubarea.SelectedIndex >= 0) {
				this.selectedCircuit.SubAreas.RemoveAt(lstSubarea.SelectedIndex);
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUITS);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void lstSubarea_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreSubArea == 0) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.SUBAREA);
			}
		}

		private void btnAddRow_Click(object sender, EventArgs e) {
			if (this.selectedSubArea.Rows.Count > 0) {
				this.selectedSubArea.Rows.Add(new KlimaFlaechenList(this.selectedSubArea.Rows[0]));
			} else {
			this.selectedSubArea.Rows.Add(new KlimaFlaechenList());
			}
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.SUBAREA);
			lstRows.SelectedIndex = lstRows.Items.Count - 1;
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void btnRemoveRow_Click(object sender, EventArgs e) {
			if (lstRows.Items.Count > 1 && lstRows.SelectedIndex >= 0) {
				this.selectedSubArea.Rows.RemoveAt(lstRows.SelectedIndex);
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.SUBAREA);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void lstRows_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreRows == 0) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.SUBAREA | FieldEnum.ROWS);
			}
		}

		private void dgvModules_GridContentChanged(object sender) {
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.SUBAREA | FieldEnum.ROWS | FieldEnum.MODULES);
			if (rbLayoutGraphical.Checked) {
				numLength.Enabled = false;
			} else {
				numLength.Enabled = this.selectedRow.List.Count > 0;
			}
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

		private void numLength_ValueChanged(object sender, EventArgs e) {
			if (ignoreLengthVerbindungen == 0) {
				this.selectedRow.LengthVerbindeleitungen = (double)this.numLength.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUITS | FieldEnum.SUBAREA | FieldEnum.ROWS | FieldEnum.MODULES | FieldEnum.LENGTH_VERBINDUNGEN);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
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

		private void cmbType_SelectedValueChanged(object sender, EventArgs e) {
			if (ignoreType == 0) {
				if (this.cmbType.SelectedItem is Product.ProductType && this.product.Product.Type != (Product.ProductType)this.cmbType.SelectedItem) {
					(this.product.Product as ModulKlimaDeckeProduct).ModulType = (Product.ProductType)this.cmbType.SelectedItem;
					if ((this.product.Product as ModulKlimaDeckeProduct).ModulType == Product.ProductType.FBH) {
						(this.product.Product as ModulKlimaDeckeProduct).PlannedFloorArea = this.product.Product.AvailableFloorArea;
					} else if ((this.product.Product as ModulKlimaDeckeProduct).ModulType == Product.ProductType.DH) {
						(this.product.Product as ModulKlimaDeckeProduct).PlannedCeilingArea = this.product.Product.AvailableCeilingArea;
					}
					this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.errorMsg = this.product.Product.LastErrorMessage;
					this.UpdateControl(FieldEnum.TYPE);
					if (this.ProjectStructureChanged != null) {
						this.ProjectStructureChanged(this);
					}
				}
			}
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

		private void button1_Click(object sender, EventArgs e) {
			if (this.product != null) {
				Europlan.Common.Products.ModulKlimaDeckePlannerForm form = new Europlan.Common.Products.ModulKlimaDeckePlannerForm(this.product);
				form.ShowDialog();
				if (form.Changed && this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				this.UpdateControl(FieldEnum.NONE);
			}
		}

		private void rbGraphical_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing && (sender as RadioButton).Checked) {
				if (this.product.Product.GraphicalMode.HasValue && this.product.Product.GraphicalMode.Value != rbLayoutGraphical.Checked) {
					// change from graphical to table based
					if (this.product.Product.GraphicalMode.Value) {
						if (!this.product.Product.AllowToSwitchMode) {
							DialogResult result = MessageBox.Show(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Auslegung_Aendern_Grafisch, EuroplanRes.PlannedModulKlimaDeckeProductPanel_Auslegung_Aendern_Titel, MessageBoxButtons.YesNo);
							if (result == DialogResult.No) {
								this.UpdateControl(FieldEnum.NONE);
								return;
							} else {
								ModulKlimaDeckeProduct mdProduct = this.product.Product as ModulKlimaDeckeProduct;
								mdProduct.ClearGraphicalRepresentation();
							}
						}
					// change from table based to graphical  
					} else {
						if (!this.product.Product.AllowToSwitchMode) {
							DialogResult result = MessageBox.Show(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Auslegung_Aendern_Tabellarisch, EuroplanRes.PlannedModulKlimaDeckeProductPanel_Auslegung_Aendern_Titel, MessageBoxButtons.YesNo);
							if (result == DialogResult.No) {
								this.UpdateControl(FieldEnum.NONE);
								return;
							} else {
								this.product.Product.PlannedCircuits.Clear();
								this.product.Product.PlannedCircuits.Add(new ModulDeckeCircuit());
							}
						}
						(this.product.Product as ModulKlimaDeckeProduct).PlannedCeilingAreaPercentage = 100;
					}
				}
				this.product.Product.GraphicalMode = rbLayoutGraphical.Checked;
				this.UpdateControl(FieldEnum.LAYOUT_TYPE);
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
			}
		}

		private void btnGraphicalAnbindleitungen_Click(object sender, EventArgs e) {
			ConnectionPlannerForm form = new ConnectionPlannerForm(this.product.Product, true);
			form.ShowDialog();
		}
		
	}
}
