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

		private bool gridContentChanged = false;
		private bool updateOngoing = false;

		public PlannedModulKlimaBodenProductPanel() {
			InitializeComponent();

			this.SetLanguage();

		}

		private void SetLanguage() {
			this.btnRestwaerme.Text = EuroplanRes.PlannedProductPanel_RestwaermeUebernehmen;
			this.btnRestkaelte.Text = EuroplanRes.PlannedProductPanel_RestkaelteUebernehmen;

			this.label27.Text = EuroplanRes.Unit_GradCelsius; //"°C"
			this.label7.Text = EuroplanRes.Unit_GradCelsius; //"°C"
			this.lblTempCoolUnit.Text = EuroplanRes.Unit_GradCelsius; //"°C"
			this.lblTempHeatUnit.Text = EuroplanRes.Unit_GradCelsius; //"°C"
			this.lblDruckverlustCoolUnit.Text = EuroplanRes.Unit_Mbar; //"mbar"
			this.lblDruckverlustHeatUnit.Text = EuroplanRes.Unit_Mbar; //"mbar"
			this.lblDurchflussCoolUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h"
			this.lblDurchflussHeatUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h"
			this.lblAvgqCoolUnit.Text = EuroplanRes.Unit_WattProQm; //"W/m²"
			this.lblAvgqHeatUnit.Text = EuroplanRes.Unit_WattProQm; //"W/m²"
			this.lblHeatLoadPercentage.Text = EuroplanRes.Unit_Prozent; //"%"
			this.lblCoolLoadPercentage.Text = EuroplanRes.Unit_Prozent; //"%"
			this.lblAreaPercentage.Text = EuroplanRes.Unit_Prozent; //"%"
			this.lblAreaReducedUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblAreaUnheated.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblHeatAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblRestAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblAnbAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblCoveredAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblAvailableAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblVerbindeleitungenUnit.Text = EuroplanRes.Unit_Meter; //"m"
			this.lblCoolLoadUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblHeatLoadUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblQAnbCoolUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblQAnbHeatUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblQCoolRestUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblQCoolDiffUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblQCoolUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblQHeatRestUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblQHeatDiffUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblQHeatUnit.Text = EuroplanRes.Unit_Watt; //"W"

			this.lblCoolLoadTotal.Text = "(0 " + EuroplanRes.Unit_Watt + ")";
			this.lblHeatLoadTotal.Text = "(0 " + EuroplanRes.Unit_Watt + ")";
			this.lblRest.Text = EuroplanRes.PlannedHithermProductPanel_Rest + " ()";

			this.lblAreaTxt.Text = EuroplanRes.PlannedProductPanel_GesamteFlaeche; //"gesamte Fläche:"
			this.chkCoverCoolLoad.Text = EuroplanRes.PlannedProductPanel_KuehllastDecken; //"Kühllast decken"
			this.lblCoolLoadTxt.Text = EuroplanRes.PlannedProductPanel_GewuenschteKuehlleistung; //"gewünschte Kühlleistung:"
			this.chkCoverHeatLoad.Text = EuroplanRes.PlannedProductPanel_WaermebedarfDecken; //"Wärmebedarf decken"
			this.lblHeatLoadTxt.Text = EuroplanRes.PlannedProductPanel_GewuenschteHeizleistung; //"gewünschte Heizleistung:"
			this.label28.Text = EuroplanRes.PlannedProductPanel_TemperaturUnterhalbKuehl; //"Temperatur unterhalb (Kühlbetrieb):"
			this.label8.Text = EuroplanRes.PlannedProductPanel_TemperaturUnterhalbHeiz; //"Temperatur unterhalb (Heizbetrieb):"
			this.lblInsulationConstruction.Text = EuroplanRes.PlannedProductPanel_Daemmkonstruktion; //"Wärmedämmkonstruktion:"
			this.lblFloorConstruction.Text = EuroplanRes.PlannedProductPanel_Fussbodenkonstruktion; //"Fußbodenkonstruktion:"
			this.pageInput.Text = EuroplanRes.PlannedProductPanel_EingabedatenSeite; //"Eingabedaten"
			this.pageCircuit.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenSeite; //"Anbindeleitungen"
			this.groupBox10.Text = EuroplanRes.PlannedProductPanel_AnbindeleitugenGruppe; //"Anbindeleitungen"
			this.chkStellAntriebe.Text = EuroplanRes.PlannedProductPanel_Stellantriebe; //"Stellantrieb(e) verwenden"
			this.lblDistributor.Text = EuroplanRes.PlannedProductPanel_Verteileranschluss; //"Verteileranschluß:"
			this.pageConstruction.Text = EuroplanRes.PlannedProductPanel_AuslegungSeite; //"Auslegung"
			this.btnConnectionPipes.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenBearbeiten; //"Anbindeleitungen bearbeiten"

			this.lblAreaReducedText.Text = EuroplanRes.PlannedModulProductPanel_FlaecheReduziert; //"Fläche mit red. Heiz-/Kühlleistung:"
			this.lblAreaUnheatedTxt.Text = EuroplanRes.PlannedModulProductPanel_FlaecheUnbeheizt; //"unbeheizte/ungekühlte Fläche:"
			this.label3.Text = EuroplanRes.PlannedModulProductPanel_FlaecheBeheizt; //"Beheizte Fläche:"
			this.label45.Text = EuroplanRes.PlannedModulProductPanel_LeistungAnbindung; //"Leistung Anbindeleitungen:"
			this.label38.Text = EuroplanRes.PlannedModulProductPanel_FlaecheUebrig; //"Übrige Fläche:"
			this.label43.Text = EuroplanRes.PlannedModulProductPanel_FlaecheAnbindung; //"Fläche Anbindeleitungen:"
			this.label42.Text = EuroplanRes.PlannedModulProductPanel_FlaecheBelegt; //"Belegte Fläche:"
			this.label41.Text = EuroplanRes.PlannedModulProductPanel_FlaecheVerfuegbar; //"Verfügbare Fläche:"
			this.label32.Text = EuroplanRes.PlannedModulProductPanel_ZusaetzlicheInformationen; //"Zus. Informationen:"
			this.label16.Text = EuroplanRes.PlannedModulProductPanel_DifferenzLeistung; //"Differenz zur erwarteten Leistung:"
			this.label17.Text = EuroplanRes.PlannedModulProductPanel_ErreichteLeistung; //"Erreichte Leistung:"
			this.label13.Text = EuroplanRes.PlannedModulProductPanel_Oberflaechentemperatur; //"Oberflächentemperatur:"
			this.label11.Text = EuroplanRes.PlannedModulProductPanel_Kuehlbetrieb; //"Kühlbetrieb"
			this.label10.Text = EuroplanRes.PlannedModulProductPanel_Heizbetrieb; //"Heizbetrieb"
			this.label4.Text = EuroplanRes.PlannedModulProductPanel_Berechnungsergebnisse; //"Berechnungsergebnisse:"
			this.label9.Text = EuroplanRes.PlannedModulProductPanel_Druckverlust; //"Druckverlust:"
			this.label6.Text = EuroplanRes.PlannedModulProductPanel_Wassermenge; //"Wassermenge:"
			this.label5.Text = EuroplanRes.PlannedModulProductPanel_DurchschnittlicheWaermestromdichte; //"Durchschn. Wärmestromdichte:"

			this.lblCalculateMode.Text = EuroplanRes.PlannedProductPanel_Verwendungszweck;
			this.rbHeat.Text = EuroplanRes.PlannedProductPanel_Heizen;
			this.rbCool.Text = EuroplanRes.PlannedProductPanel_Kuehlen;
			this.rbHeatAndCool.Text = EuroplanRes.PlannedProductPanel_HeizenUndKuehlen;

			this.lblCircuitCountDescr.Text = EuroplanRes.PlannedModulKlimaBodenProductPanel_AnzahlHeizkreise; //"Anzahl Heizkreise:"
			this.lblCircuits.Text = EuroplanRes.PlannedModulKlimaBodenProductPanel_AnzahlHeizkreise; //"Anzahl Heizkreise:"
			this.lblVerbindeleitung.Text = EuroplanRes.PlannedModulKlimaBodenProductPanel_SonstigeAnbindeleitung; //"Anbindeleitung der sonstigen Module:"
			this.lblSonstige.Text = EuroplanRes.PlannedModulKlimaBodenProductPanel_SonstigeModule; //"Sonstige Module:"
			this.lblModulierend.Text = EuroplanRes.PlannedModulKlimaBodenProductPanel_ModulierendeModule; //"Module in modulierender Belegung:"
			this.lblDicht.Text = EuroplanRes.PlannedModulKlimaBodenProductPanel_DichteModule; //"Module in dichter Belegung:"

			this.cmbCircuits.Items.Clear();
			this.cmbCircuits.Items.Add(EuroplanRes.PlannedModulKlimaBodenProductPanel_Automatisch/*"Automatisch"*/);
			for (int i = 1; i <= 12; i++) {
				this.cmbCircuits.Items.Add(i.ToString());
			}

			this.rbLayoutTable.Text = EuroplanRes.PlannedProductPanel_Tabellarisch;
			this.rbLayoutGraphical.Text = EuroplanRes.PlannedProductPanel_Grafisch;
			this.lblLayoutType.Text = EuroplanRes.PlannedProductPanel_Auslegungsart;
			this.btnGraphicalAnbindleitungen.Text = EuroplanRes.PlannedProductPanel_GrafischeAnbindeleitungen;
			this.btnGraphical.Text = EuroplanRes.PlannedProductPanel_GrafischeAuslegung;
		}

		#region IEditorUserControl Members
        private event ProjectStructureChangedHandler projectStructureChanged;
        public event ProjectStructureChangedHandler ProjectStructureChanged {
            add { this.projectStructureChanged += value; }
            remove { this.projectStructureChanged -= value; }
        }
        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }
        private event TreeSelectionRequestedHandler treeSelectionRequested;
        public event TreeSelectionRequestedHandler TreeSelectionRequested {
            add { this.treeSelectionRequested += value; }
            remove { this.treeSelectionRequested -= value; }
        }

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
			MODULES_VERBINDELEITUNG    = 0x40000,
			LAYOUT_TYPE	               = 0x80000
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
				ignoreModulesDicht++;
				ignoreModulesModulierend++;
				ignoreModulesSonstige++;
				ignoreVerbindeleitungen++;
				ignoreCalculationMode++;

				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;

				if (this.product.Product.AssociatedRoom.AssociatedPlan != null && this.product.Product.AssociatedRoom.RoomCoordinates.Count > 0) {
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
					if (this.product.Product.AssociatedRoom.AssociatedPlan != null && this.product.Product.AssociatedRoom.RoomCoordinates.Count > 0) {
						if (mbProduct.ContainsModules) {
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
					this.btnGraphical.Enabled = true;
					this.numDicht.Enabled = false;
					this.numModulierend.Enabled = false;
					this.numSonstige.Enabled = false;
					this.numVerbindeleitungen.Enabled = false;
					this.cmbCircuits.Enabled = false;
					(this.product.Product as ModulKlimaBodenProduct).PlannedFloorAreaPercentage = 100;
				} else {
					this.numArea.Enabled = true;
					this.numAreaPercentage.Enabled = true;
					this.numAreaUnheated.Enabled = true;
					this.btnGraphical.Enabled = false;
					this.numDicht.Enabled = true;
					this.numModulierend.Enabled = true;
					this.numSonstige.Enabled = true;
					this.numVerbindeleitungen.Enabled = true;
					this.cmbCircuits.Enabled = true;
				}

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
				if (mbProduct.AssociatedRoom.Area > 0) {
					this.numAreaPercentage.MaxValue = (decimal)(mbProduct.AvailableFloorArea * 100 / mbProduct.AssociatedRoom.Area);
				} else {
					this.numAreaPercentage.MaxValue = 100;
				}

				if (this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT) {
					rbHeat.Checked = true;
				} else if (this.product.Product.CalculateMode == Product.CalculateModeEnum.COOL) {
					rbCool.Checked = true;
				} else if (this.product.Product.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL) {
					rbHeatAndCool.Checked = true;
				}

				this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
				this.numAreaUnheated.MaxValue = (decimal)this.product.PlannedArea;
				this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
				this.numHeatLoadPercentage.MaxValue = (decimal)(mbProduct.AssociatedRoom.NormalizedHeatLoad <= 0 ? 0 : this.product.NecessaryHeatLoad * 100 / mbProduct.AssociatedRoom.NormalizedHeatLoad);
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
				this.numCoolLoadPercentage.MaxValue = (decimal)(mbProduct.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / mbProduct.AssociatedRoom.NormalizedCoolLoad);
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
				this.txtFloorConstruction.Text = (mbProduct.PlannedFloorConstruction == null ? "" : mbProduct.PlannedFloorConstruction.Id + ": " + mbProduct.PlannedFloorConstruction.LocalizedName);
				this.txtInsulationConstruction.Text = (mbProduct.PlannedInsulationConstruction == null ? "" : mbProduct.PlannedInsulationConstruction.Id + ": " + mbProduct.PlannedInsulationConstruction.LocalizedName);
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

				lblRest.Text = EuroplanRes.PlannedModulProductPanel_Rest + " (" + this.product.Product.AssociatedRoom.ToString() + ")";
				lblQHeat.Text = Math.Round(this.product.PlannedHeatLoad, 2).ToString();
				lblQAnbHeat.Text = Math.Round(this.product.Product.PlannedHeatLoadAnbindung, 0).ToString();
				lblQHeatDiff.Text = Math.Round(qDiffHeat).ToString("+0;-0");
				lblQHeatRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenHeatLoad, 2).ToString("+0.00;-0.00");
				lblAvgqHeat.Text = Math.Round(mbProduct.PlannedHeatLoadPerSqM, 2).ToString();
				lblDurchflussHeat.Text = Math.Round(mbProduct.PlannedMaxDurchflussHeat, 2).ToString();
                lblDruckverlustHeat.Text = Math.Round(mbProduct.PlannedDeltaRhoInklVentilHeat, 2).ToString();
				lblTempHeat.Text = Math.Round(mbProduct.PlannedFloorTemperatureHeat, 2).ToString();
				lblQCool.Text = Math.Round(this.product.PlannedCoolLoad, 2).ToString();
				lblQAnbCool.Text = Math.Round(this.product.Product.PlannedCoolLoadAnbindung, 0).ToString();
				lblQCoolDiff.Text = Math.Round(qDiffCool, 0).ToString("+0;-0");
				lblQCoolRest.Text = Math.Round(this.product.Product.AssociatedRoom.OpenCoolLoad, 2).ToString("+0.00;-0.00");
				lblAvgqCool.Text = Math.Round(mbProduct.PlannedCoolLoadPerSqM, 2).ToString();
				lblDurchflussCool.Text = Math.Round(mbProduct.PlannedMaxDurchflussCool, 2).ToString();
                lblDruckverlustCool.Text = Math.Round(mbProduct.PlannedDeltaRhoInklVentilCool, 2).ToString();
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

				if (mbProduct.PlannedConnection == null) {
					this.txtDistributor.Text = "";
				} else {
					this.txtDistributor.Text = mbProduct.PlannedConnection.ToString();
				}

				if (mbProduct.PlannedModulArea > mbProduct.PlannedNetArea) {
					string warning = EuroplanRes.PlannedModulProductPanel_FlaecheWarnung;
					warning = warning.Replace("%VALUE%", Math.Round(mbProduct.PlannedModulArea, 1).ToString());
					warning = warning.Replace("%DEFAULT%", Math.Round(mbProduct.PlannedNetArea, 1).ToString());
					warning = warning + "\n";
					this.lblAreaWarning.Text = warning;
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
				ignoreCalculationMode--;
			}
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TRK_ESTRICH),
                    ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_COMPACT_PLATTE),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_COMPACT_PLATTE)}));
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
			if (this.projectChanged != null) {
				this.projectChanged(this);
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
					this.product.Product.AssociatedRoom.GetFloor().LastInsulationConstruction = form.SelectedConstruction;
				}
			}
			form.Dispose();
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

		private void numRoomTemperatureBelowHeat_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowHeat == 0) {
				(this.product.Product as ModulKlimaBodenProduct).PlannedRoomTemperatureBelowHeat = (float)this.numRoomTemperatureBelowHeat.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_HEAT);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void numRoomTemperatureBelowCool_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowCool == 0) {
				(this.product.Product as ModulKlimaBodenProduct).PlannedRoomTemperatureBelowCool = (float)this.numRoomTemperatureBelowCool.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_COOL);
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
			SelectConnectionForProductForm form = new SelectConnectionForProductForm(this.product, this.product.Product.AssociatedRoom.AssociatedFloor, false);
			form.ShowDialog();

			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.errorMsg = this.product.Product.LastErrorMessage;
			this.UpdateControl(FieldEnum.COOL_LOAD);
			if (form.DialogResult == DialogResult.OK && this.projectChanged != null) {
				this.projectChanged(this);
			}
			form.Dispose();
		}

		private void connectionPipePanel1_GridContentChanged(object sender) {
			gridContentChanged = true;
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

		private void btnConnectionPipes_Click(object sender, EventArgs e) {
			ConnectionPipesForm form = new ConnectionPipesForm(this.product);
			form.ShowDialog();
			if (form.UnsavedChanges) {
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.NONE);
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void numModulierend_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreModulesModulierend == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedModulesModulierend = (int)this.numModulierend.Value;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.MODULES_MODULIEREND);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void numSonstige_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreModulesSonstige == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedModulesSonstige = (int)this.numSonstige.Value;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.MODULES_SONTIGE);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void numVerbindeleitungen_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreVerbindeleitungen == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedSonstigeVerbindeLeitung = (double)this.numVerbindeleitungen.Value;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.MODULES_VERBINDELEITUNG);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void cmbCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.ignoreCircuits == 0) {
				ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
				mbProduct.RequestedCircuits = this.cmbCircuits.SelectedIndex == 0 ? (Nullable<int>)null : (Nullable<int>)this.cmbCircuits.SelectedIndex;
				mbProduct.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUITS);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void chkStellAntriebe_CheckedChanged(object sender, EventArgs e) {
			this.product.Product.StellMotore = this.chkStellAntriebe.Checked;
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
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
					if (this.projectChanged != null) {
						this.projectChanged(this);
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
					if (this.projectChanged != null) {
						this.projectChanged(this);
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
					if (this.projectChanged != null) {
						this.projectChanged(this);
					}
				}
			}
		}

		private void btnRestwaerme_Click(object sender, EventArgs e) {
			this.product.RestwaermeUebernehmen();
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.UpdateControl(FieldEnum.NONE);
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

		private void btnRestkaelte_Click(object sender, EventArgs e) {
			this.product.RestkaelteUebernehmen();
			this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			this.UpdateControl(FieldEnum.NONE);
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

		private void btnGraphical_Click(object sender, EventArgs e) {
			if (this.product != null) {
				Europlan.Common.Products.ModulKlimaBodenPlannerForm form = new Europlan.Common.Products.ModulKlimaBodenPlannerForm(this.product);
				form.ShowDialog();
				if (form.Changed && this.projectChanged != null) {
					this.projectChanged(this);
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
								ModulKlimaBodenProduct mbProduct = this.product.Product as ModulKlimaBodenProduct;
								mbProduct.ClearGraphicalRepresentation();
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
								this.product.Product.PlannedCircuits.Add(new ModulBodenCircuit());
							}
						}
						(this.product.Product as ModulKlimaBodenProduct).PlannedFloorAreaPercentage = 100;
					}
				}
				this.product.Product.GraphicalMode = rbLayoutGraphical.Checked;
				this.UpdateControl(FieldEnum.LAYOUT_TYPE);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void btnGraphicalAnbindleitungen_Click(object sender, EventArgs e) {
			ConnectionPlannerForm form = new ConnectionPlannerForm(this.product.Product, false);
			form.ShowDialog();
		}
	}
}
