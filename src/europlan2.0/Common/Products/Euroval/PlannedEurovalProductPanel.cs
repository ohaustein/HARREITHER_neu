using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {
	public partial class PlannedEurovalProductPanel : UserControl, IEditorUserControl {
		
		private PlannedProduct product = null;
		private bool gridContentChanged = false;
		private bool updateOngoing = false;

		public class LayDistanceItem {
			public Nullable<EurovalProduct.EurovalLayDistance> layDistance;
			public string name;

			public LayDistanceItem(Nullable<EurovalProduct.EurovalLayDistance> layDistance, string name) {
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

		public class RimTypeItem {
			public Nullable<EurovalProduct.EurovalRimType> rimType;
			public string name;

			public RimTypeItem(Nullable<EurovalProduct.EurovalRimType> layDistance, string name) {
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

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.cmbLayDistance.Items.Clear();
			this.cmbRimType.Items.Clear();
			this.cmbCircuits.Items.Clear();

			this.btnRestwaerme.Text = EuroplanRes.PlannedProductPanel_RestwaermeUebernehmen;
			this.btnRestkaelte.Text = EuroplanRes.PlannedProductPanel_RestkaelteUebernehmen;

			this.lblAreaUnheated.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblAreaReducedUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²"
			this.lblAreaPercentage.Text = EuroplanRes.Unit_Prozent; //"%"
			this.lblCoolLoadPercentage.Text = EuroplanRes.Unit_Prozent; //"%"
			this.lblHeatLoadPercentage.Text = EuroplanRes.Unit_Prozent; //"%"
			this.lblCoolLoadUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.lblHeatLoadUnit.Text = EuroplanRes.Unit_Watt; //"W"
			this.label27.Text = EuroplanRes.Unit_GradCelsius; //"°C"
			this.label7.Text = EuroplanRes.Unit_GradCelsius; //"°C"
			this.label9.Text = EuroplanRes.Unit_Meter; //"m"

			string wattUnit = EuroplanRes.Unit_Watt;
			this.lblCoolLoadTotal.Text = "(0 " + wattUnit + ")";
			this.lblHeatLoadTotal.Text = "(0 " + wattUnit + ")";

			this.pageInput.Text = EuroplanRes.PlannedProductPanel_EingabedatenSeite; //"Eingabedaten"
			this.chkCoverCoolLoad.Text = EuroplanRes.PlannedProductPanel_KuehllastDecken; //"Kühllast decken"
			this.lblCoolLoadTxt.Text = EuroplanRes.PlannedProductPanel_GewuenschteKuehlleistung; //"gewünschte Kühlleistung:"
			this.chkCoverHeatLoad.Text = EuroplanRes.PlannedProductPanel_WaermebedarfDecken; //"Wärmebedarf decken"
			this.lblHeatLoadTxt.Text = EuroplanRes.PlannedProductPanel_GewuenschteHeizleistung; //"gewünschte Heizleistung:"
			this.label28.Text = EuroplanRes.PlannedProductPanel_TemperaturUnterhalbKuehl; //"Temperatur unterhalb (Kühlbetrieb):"
			this.label8.Text = EuroplanRes.PlannedProductPanel_TemperaturUnterhalbHeiz; //"Temperatur unterhalb (Heizbetrieb):"
			this.lblFloorConstruction.Text = EuroplanRes.PlannedProductPanel_Fussbodenkonstruktion; //"Fußbodenkonstruktion:"
			this.lblInsulationConstruction.Text = EuroplanRes.PlannedProductPanel_Daemmkonstruktion; //"Wärmedämmkonstruktion:"
			this.lblDistributor.Text = EuroplanRes.PlannedProductPanel_Verteileranschluss; //"Verteileranschluß:"
			this.pageCircuit.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenSeite; //"Anbindeleitungen"
			this.groupBox10.Text = EuroplanRes.PlannedProductPanel_AnbindeleitugenGruppe; //"Anbindeleitungen"
			this.chkStellAntriebe.Text = EuroplanRes.PlannedProductPanel_Stellantriebe; //"Stellantrieb(e) verwenden"
			this.pageConstruction.Text = EuroplanRes.PlannedProductPanel_AuslegungSeite; //"Auslegung"
			this.lblAreaTxt.Text = EuroplanRes.PlannedProductPanel_GesamteFlaeche; //"gesamte Fläche:"

			this.lblAreaUnheatedTxt.Text = EuroplanRes.PlannedEurovalProductPanel_UnbeheizteFlaeche; //"unbeheizte/ungekühlte Fläche:"
			this.lblAreaReducedTxt.Text = EuroplanRes.PlannedEurovalProductPanel_ReduzierteFlaeche; //"Fläche mit red. Heiz-/Kühlleistung:"
			this.chkAnhydritEstrich.Text = EuroplanRes.PlannedEurovalProductPanel_Anhydritestrich; //"Anhydritestrich"
			this.chkClip.Text = EuroplanRes.PlannedEurovalProductPanel_Clipschiene; //"Clipschiene mit Klebeband"
			this.cbSeparateCircuit.Text = EuroplanRes.PlannedEurovalProductPanel_EigenerHeizkreis; //"Eigener Heizkreis für dieses Fußbodenheizsystem"
			this.label12.Text = EuroplanRes.PlannedEurovalProductPanel_EckenErklaerung; //"(positive Ecken vergößern, negative verringern die Randzonenfläche)"
			this.label11.Text = EuroplanRes.PlannedEurovalProductPanel_AnzahlEcken; //"Anzahl der Ecken:"
			this.label10.Text = EuroplanRes.PlannedEurovalProductPanel_LaengeRandzone; //"Länge der Randzone:"
			this.groupBox8.Text = EuroplanRes.PlannedEurovalProductPanel_TatsaechlicheAuslegung; //"tatsächliche Auslegung"
			this.label29.Text = EuroplanRes.PlannedEurovalProductPanel_AnzahlHeizkreise; //"Anzahl Heizkreise:"
			this.rbCalculateBoth.Text = EuroplanRes.PlannedEurovalProductPanel_NachHeizUndKuehlbetrieb; //"nach Heiz- und Kühlbetrieb"
			this.rbCalculateCool.Text = EuroplanRes.PlannedEurovalProductPanel_NachKuehlbetrieb; //"nach Kühlbetrieb"
			this.rbCalculateHeat.Text = EuroplanRes.PlannedEurovalProductPanel_NachHeizbetrieb; //"nach Heizbetrieb"
			this.label14.Text = EuroplanRes.PlannedEurovalProductPanel_VerlegeartRZ; //"Verlegeart Randzone:"
			this.label13.Text = EuroplanRes.PlannedEurovalProductPanel_VerlegeartAZ; //"Verlegeart Aufenthaltszone:"
			this.grpResults.Text = EuroplanRes.PlannedEurovalProductPanel_Berechnungsergebnisse; //"Berechnungsergebnisse"
			this.btnConnectionPipes.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenBearbeiten; //"Anbindeleitungen bearbeiten"
			this.label41.Text = EuroplanRes.PlannedEurovalProductPanel_Spreizung; //"Spreizung\nK"
			this.label33.Text = EuroplanRes.PlannedEurovalProductPanel_Kuehlen; //"Kühlen:"
			this.label34.Text = EuroplanRes.PlannedEurovalProductPanel_DeltaP; //"DeltaP\nmbar"
			this.label35.Text = EuroplanRes.PlannedEurovalProductPanel_Mg; //"mh\nkg/h"
			this.label36.Text = EuroplanRes.PlannedEurovalProductPanel_Rohrlaenge; //"Rorhl.\nm"
			this.label37.Text = EuroplanRes.PlannedEurovalProductPanel_AnzahlHK; //"Anzahl\nHK"
			this.label38.Text = EuroplanRes.PlannedEurovalProductPanel_Heizen; //"Heizen:"
			this.label26.Text = EuroplanRes.PlannedEurovalProductPanel_Aufenthaltszone; //"Aufenthaltszone"
			this.label25.Text = EuroplanRes.PlannedEurovalProductPanel_Randzone; //"Randzone"
			this.label19.Text = EuroplanRes.PlannedEurovalProductPanel_Q; //"Q\nW"
			this.label20.Text = EuroplanRes.PlannedEurovalProductPanel_Tfb; //"tfb\n°C"
			this.label21.Text = EuroplanRes.PlannedEurovalProductPanel_A; //"A\nm²"
			this.label22.Text = EuroplanRes.PlannedEurovalProductPanel_VA; //"VA"
			this.label23.Text = EuroplanRes.PlannedEurovalProductPanel_QAnb; //"QAnb\nW"
			this.label24.Text = EuroplanRes.PlannedEurovalProductPanel_AAnb; //"AAnb\nm²"
			this.label15.Text = EuroplanRes.PlannedEurovalProductPanel_Q; //"Q\nW"
			this.label16.Text = EuroplanRes.PlannedEurovalProductPanel_Tfb; //"tfb\n°C"
			this.label17.Text = EuroplanRes.PlannedEurovalProductPanel_B; //"B\ncm"
			this.label18.Text = EuroplanRes.PlannedEurovalProductPanel_VA; //"VA"
			this.label6.Text = EuroplanRes.PlannedEurovalProductPanel_Kuehlen; //"Kühlen:"
			this.label5.Text = EuroplanRes.PlannedEurovalProductPanel_QRest; //"QRest\nW"
			this.label4.Text = EuroplanRes.PlannedEurovalProductPanel_QFbh; //"QFBH\nW"
			this.label3.Text = EuroplanRes.PlannedEurovalProductPanel_QSollProQuadratmeter; //"qSoll\nW/m²"
			this.label2.Text = EuroplanRes.PlannedEurovalProductPanel_QSoll; //"QSoll\nW"
			this.label1.Text = EuroplanRes.PlannedEurovalProductPanel_Heizen; //"Heizen:"
			this.pageCorrections.Text = EuroplanRes.PlannedEurovalProductPanel_ErweiterteKorrekturenSeite; //"erweiterte Korrekturen"

			this.lblCalculateMode.Text = EuroplanRes.PlannedProductPanel_Verwendungszweck;
			this.rbHeat.Text = EuroplanRes.PlannedProductPanel_Heizen;
			this.rbCool.Text = EuroplanRes.PlannedProductPanel_Kuehlen;
			this.rbHeatAndCool.Text = EuroplanRes.PlannedProductPanel_HeizenUndKuehlen;

			this.cmbLayDistance.Items.Add(new LayDistanceItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.EurovalLayDistance.EV35, EuroplanRes.EurovalProduct_EV35/*"EV35"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.EurovalLayDistance.EV30, EuroplanRes.EurovalProduct_EV30/*"EV30"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.EurovalLayDistance.EV25, EuroplanRes.EurovalProduct_EV25/*"EV25"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.EurovalLayDistance.EV20, EuroplanRes.EurovalProduct_EV20/*"EV20"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.EurovalLayDistance.EV15, EuroplanRes.EurovalProduct_EV15/*"EV15"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.EurovalLayDistance.EV10, EuroplanRes.EurovalProduct_EV10/*"EV10"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(EurovalProduct.EurovalLayDistance.EV5, EuroplanRes.EurovalProduct_EV5/*"EV5"*/));

			this.cmbRimType.Items.Add(new RimTypeItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV15_60, EuroplanRes.EurovalProduct_EV15_60/*"EV15/60"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV15_120, EuroplanRes.EurovalProduct_EV15_120/*"EV15/120"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV15_180, EuroplanRes.EurovalProduct_EV15_180/*"EV15/180"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV10_55, EuroplanRes.EurovalProduct_EV10_55/*"EV10/55"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV10_110, EuroplanRes.EurovalProduct_EV10_110/*"EV10/110"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV10_165, EuroplanRes.EurovalProduct_EV10_165/*"EV10/165"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV5_40, EuroplanRes.EurovalProduct_EV5_40/*"EV5/40"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV5_80, EuroplanRes.EurovalProduct_EV5_80/*"EV5/80"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(EurovalProduct.EurovalRimType.EV5_120, EuroplanRes.EurovalProduct_EV5_120/*"EV5/120"*/));

			this.cmbCircuits.Items.Add(EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/);
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
			SEPARATE_CIRCUIT = 65536,
			CORRECTIONS = 131072,
			LAYOUT_TYPE = 262144
		}

		private string errorMsg = null;

		private bool cmbLayDistanceContainsAutomatic = true;
		private bool cmbRimTypeContainsAutomatic = true;
		private bool cmbRimTypeContainsNone = false;
		private bool cmbCircuitsContainsAutomatic = true;

		public void UpdateControl(bool resetUserInterface) {
			this.product = this.Tag as PlannedProduct;
			if (resetUserInterface) {
				this.tabs.SelectedTab = this.pageInput;
			}
			this.extendedCorrectionsGrid.Product = this.product.Product;
			this.connectionPipePanel.Update(this.product);
			this.chkStellAntriebe.Checked = this.product.Product.StellMotore;
			if (this.product != null) {
				(this.product.Product as EurovalProduct).ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
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
		private int ignoreCorrections = 0;
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
				ignoreRim++;
				ignoreCorners++;
				ignoreLayDistance++;
				ignoreRimType++;
				ignoreCalculationType++;
				ignoreCircuits++;
				ignoreSeparateCircuit++;
				ignoreCorrections++;
				ignoreCalculationMode++;

				EurovalProduct evProduct = this.product.Product as EurovalProduct;

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
						if (evProduct.PlannedAreaGraphical.Count > 0 &&
							evProduct.PlannedFloorArea == evProduct.AssociatedRoom.Area &&
							evProduct.PlannedAreaReduced == 0 &&
							evProduct.PlannedAreaUnheated == 0 &&
							evProduct.PlannedRimLength == 0 &&
							evProduct.PlannedRimCorners == 0 &&
							evProduct.PlannedCorrections == false) {
							graphicalMode = true;
							this.product.Product.GraphicalMode = true;
						} else {
							graphicalMode = false;
							this.product.Product.GraphicalMode = false;
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

				bool showHeat = this.product.RequestedHeatLoad > 0 && evProduct.PlannedLayDistance != EurovalProduct.EurovalLayDistance.NONE;
				bool showCool = this.product.RequestedCoolLoad > 0 && evProduct.PlannedLayDistance != EurovalProduct.EurovalLayDistance.NONE;
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

				this.rbCalculateHeat.Enabled = evProduct.CalculateMode == Product.CalculateModeEnum.HEAT || evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
				this.rbCalculateCool.Enabled = evProduct.CalculateMode == Product.CalculateModeEnum.HEAT || evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
				this.rbCalculateBoth.Enabled = evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
				this.numCorners.Enabled = evProduct.PlannedRimLength > 0;
				this.cmbRimType.Enabled = evProduct.PlannedRimLength > 0;

				if (evProduct.CalculateMode == Product.CalculateModeEnum.HEAT) {
					rbHeat.Checked = true;
				} else if (evProduct.CalculateMode == Product.CalculateModeEnum.COOL) {
					rbCool.Checked = true;
				} else if (evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL) {
					rbHeatAndCool.Checked = true;
				}

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

				bool newCmbCircuitsContainsAutomatic = !evProduct.ManualMode;
				bool newCmbLayDistanceContainsAutomatic = !evProduct.ManualMode;
				bool newCmbRimTypeContainsAutomatic = !evProduct.ManualMode && cmbRimType.Enabled;
				bool newCmbRimTypeContainsNone = !this.cmbRimType.Enabled;

				if (this.cmbCircuitsContainsAutomatic != newCmbCircuitsContainsAutomatic) {
					this.cmbCircuitsContainsAutomatic = newCmbCircuitsContainsAutomatic;
					if (this.cmbCircuitsContainsAutomatic) {
						this.cmbCircuits.Items.Insert(0, EuroplanRes.EurovalProduct_Automatisch);
					} else {
						this.cmbCircuits.Items.RemoveAt(0);
					}
				}

				if (this.cmbLayDistanceContainsAutomatic != newCmbLayDistanceContainsAutomatic) {
					this.cmbLayDistanceContainsAutomatic = newCmbLayDistanceContainsAutomatic;
					if (this.cmbLayDistanceContainsAutomatic) {
						this.cmbLayDistance.Items.Insert(0, new LayDistanceItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
					} else {
						this.cmbLayDistance.Items.RemoveAt(0);
					}
				}

				if (this.cmbRimTypeContainsAutomatic != newCmbRimTypeContainsAutomatic) {
					this.cmbRimTypeContainsAutomatic = newCmbRimTypeContainsAutomatic;
					if (this.cmbRimTypeContainsAutomatic) {
						if (this.cmbRimTypeContainsNone) {
							this.cmbRimTypeContainsNone = false;
							newCmbRimTypeContainsNone = false;
							this.cmbRimType.Items.RemoveAt(0);
						}
						this.cmbRimType.Items.Insert(0, new RimTypeItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
					} else {
						this.cmbRimType.Items.RemoveAt(0);
						this.cmbRimTypeContainsNone = false;
					}
				}
				if (this.cmbRimTypeContainsNone != newCmbRimTypeContainsNone) {
					this.cmbRimTypeContainsNone = newCmbRimTypeContainsNone;
					if (this.cmbRimTypeContainsNone) {
						this.cmbRimType.Items.Insert(0, new RimTypeItem(null, ""));
					} else {
						this.cmbRimType.Items.RemoveAt(0);
					}
				}

				this.numArea.MaxValue = (decimal)evProduct.AvailableFloorArea;
				if (evProduct.AssociatedRoom.Area > 0) {
					this.numAreaPercentage.MaxValue = (decimal)(evProduct.AvailableFloorArea * 100 / evProduct.AssociatedRoom.Area);
				} else {
					this.numAreaPercentage.MaxValue = 100;
				}
				this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
				this.numAreaUnheated.MaxValue = (decimal)this.product.PlannedArea;
				this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
				this.numHeatLoadPercentage.MaxValue = (decimal)(evProduct.AssociatedRoom.NormalizedHeatLoad <= 0 ? 0 : this.product.NecessaryHeatLoad * 100 / evProduct.AssociatedRoom.NormalizedHeatLoad);
				if ((evProduct.CalculateMode == Product.CalculateModeEnum.HEAT || 
					evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL)) {
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
				this.numCoolLoadPercentage.MaxValue = (decimal)(evProduct.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / evProduct.AssociatedRoom.NormalizedCoolLoad);
				if ((evProduct.CalculateMode == Product.CalculateModeEnum.COOL || 
					evProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL)) {
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
				this.txtFloorConstruction.Text = (evProduct.PlannedFloorConstruction == null ? "" : evProduct.PlannedFloorConstruction.Id + ": " + evProduct.PlannedFloorConstruction.LocalizedName);
				this.txtInsulationConstruction.Text = (evProduct.PlannedInsulationConstruction == null ? "" : evProduct.PlannedInsulationConstruction.Id + ": " + evProduct.PlannedInsulationConstruction.LocalizedName);
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_HEAT) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowHeat.Value = Math.Round((decimal)evProduct.PlannedRoomTemperatureBelowHeat, 2);
				}
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_COOL) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowCool.Value = Math.Round((decimal)evProduct.PlannedRoomTemperatureBelowCool, 2);
				}
				if ((skipFields & FieldEnum.RIM_LENGTH) == FieldEnum.NONE) {
					this.numRim.Value = Math.Round((decimal)evProduct.PlannedRimLength, 2);
				}
				if ((skipFields & FieldEnum.CORNERS) == FieldEnum.NONE) {
					this.numCorners.Value = (decimal)evProduct.PlannedRimCorners;
				}

				if ((skipFields & FieldEnum.LAY_DISTANCE) == FieldEnum.NONE) {
					if (evProduct.RequestedLayDistance == null && !this.cmbLayDistanceContainsAutomatic) {
						evProduct.RequestedLayDistance = EurovalProduct.EurovalLayDistance.EV35;
					}
					this.cmbLayDistance.SelectedItem = new LayDistanceItem(evProduct.RequestedLayDistance, "");
				}
				if ((skipFields & FieldEnum.RIM_TYPE) == FieldEnum.NONE) {
					if (evProduct.RequestedRimType == null && !this.cmbRimTypeContainsAutomatic && !this.cmbRimTypeContainsNone) {
						evProduct.RequestedRimType = EurovalProduct.EurovalRimType.EV15_60;
					}
					this.cmbRimType.SelectedItem = new RimTypeItem(this.cmbRimType.Enabled ? evProduct.RequestedRimType : null, "");
				}
				if ((skipFields & FieldEnum.CIRCUIT_COUNT) == FieldEnum.NONE) {
					if (evProduct.RequestedCircuits != null) {
						this.cmbCircuits.SelectedIndex = evProduct.RequestedCircuits.Value - 1 + (this.cmbCircuitsContainsAutomatic ? 1 : 0);
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
				this.lblQRestHeat.Text = Math.Round(qRestHeat, 2).ToString("+0.00;-0.00");
				this.lblQRestCool.Text = Math.Round(qRestCool, 2).ToString("+0.00;-0.00");

				// Randzone
				if (complete && evProduct.PlannedRimType.HasValue) {
					switch (evProduct.PlannedRimLayDistance) {
						case EurovalProduct.EurovalLayDistance.EV5:
							this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV5; //"EV5"
                            this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV5; //"EV5"
                            this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV5 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV5"
                            break;
						case EurovalProduct.EurovalLayDistance.EV10:
							this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV10; //"EV10"
                            this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV10; //"EV10"
							this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV10 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV10"
                            break;
						case EurovalProduct.EurovalLayDistance.EV15:
							this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV15; //"EV15"
                            this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV15; //"EV15"
							this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV15 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV15"
                            break;
						case EurovalProduct.EurovalLayDistance.EV20:
							this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV20; //"EV20"
                            this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV20; //"EV20"
							this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV20 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV20"
                            break;
						case EurovalProduct.EurovalLayDistance.EV25:
							this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV25; //"EV25"
                            this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV25; //"EV25"
							this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV25 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV25"
                            break;
						case EurovalProduct.EurovalLayDistance.EV30:
							this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV30; //"EV30"
                            this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV30; //"EV30"
							this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV30 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV30"
                            break;
						case EurovalProduct.EurovalLayDistance.EV35:
							this.lblRimVaHeat.Text = EuroplanRes.EurovalProduct_EV35; //"EV35"
                            this.lblRimVaCool.Text = EuroplanRes.EurovalProduct_EV35; //"EV35"
							this.lblRimVa.Text = EuroplanRes.EurovalProduct_EV35 + "/" + evProduct.PlannedRimWidth.ToString(); //"EV35"
                            break;
						default:
							this.lblRimVaHeat.Text = "--";
                            this.lblRimVaCool.Text = "--";
                            this.lblRimVa.Text = "--";
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
                    this.lblRimVa.Text = "--";
                    this.lblRimBHeat.Text = "";
					this.lblRimBCool.Text = "";
					this.lblRimTfbHeat.Text = "";
					this.lblRimTfbCool.Text = "";
					this.lblRimQHeat.Text = "";
					this.lblRimQCool.Text = "";
				}

				// Aufenthaltszone
				if (complete && evProduct.PlannedLayDistance.HasValue) {
					switch (evProduct.PlannedLayDistance) {
						case EurovalProduct.EurovalLayDistance.EV5:
							this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV5; //"EV5"
							this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV5; //"EV5"
                            this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV5; //"EV5"
							break;
						case EurovalProduct.EurovalLayDistance.EV10:
							this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV10; //"EV10"
							this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV10; //"EV10"
                            this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV10; //"EV10"
                            break;
						case EurovalProduct.EurovalLayDistance.EV15:
							this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV15; //"EV15"
							this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV15; //"EV15"
                            this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV15; //"EV15"
                            break;
						case EurovalProduct.EurovalLayDistance.EV20:
							this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV20; //"EV20"
							this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV20; //"EV20"
                            this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV20; //"EV20"
                            break;
						case EurovalProduct.EurovalLayDistance.EV25:
							this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV25; //"EV25"
							this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV25; //"EV25"
                            this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV25; //"EV25"
                            break;
						case EurovalProduct.EurovalLayDistance.EV30:
							this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV30; //"EV30"
							this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV30; //"EV30"
                            this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV30; //"EV30"
                            break;
						case EurovalProduct.EurovalLayDistance.EV35:
							this.lblResidenceVaHeat.Text = EuroplanRes.EurovalProduct_EV35; //"EV35"
							this.lblResidenceVaCool.Text = EuroplanRes.EurovalProduct_EV35; //"EV35"
                            this.lblResidenceVa.Text = EuroplanRes.EurovalProduct_EV35; //"EV35"
                            break;
						default:
							this.lblResidenceVaHeat.Text = "--";
							this.lblResidenceVaCool.Text = "--";
                            this.lblResidenceVa.Text = "--";
                            break;
					}
					this.lblResidenceAHeat.Text = Math.Round(evProduct.PlannedAreaResidenceHeated, 1).ToString();
					this.lblResidenceACool.Text = Math.Round(evProduct.PlannedAreaResidenceHeated, 1).ToString();
					this.lblResidenceTfbHeat.Text = Math.Round(evProduct.PlannedFloorTemperatureHeatResidence, 1).ToString();
					this.lblResidenceTfbCool.Text = Math.Round(evProduct.PlannedFloorTemperatureCoolResidence, 1).ToString();
					this.lblResidenceQHeat.Text = Math.Round(evProduct.PlannedHeatLoadResidence, 0).ToString();
					this.lblResidenceQCool.Text = Math.Round(evProduct.PlannedCoolLoadResidence, 0).ToString();
				} else {
					this.lblResidenceVaHeat.Text = "";
					this.lblResidenceVaCool.Text = "";
                    this.lblResidenceVa.Text = "--";
                    this.lblResidenceAHeat.Text = "";
					this.lblResidenceACool.Text = "";
					this.lblResidenceTfbHeat.Text = "";
					this.lblResidenceTfbCool.Text = "";
					this.lblResidenceQHeat.Text = "";
					this.lblResidenceQCool.Text = "";
				}

				// anbindung
				this.lblConnectionAHeat.Text = Math.Round(evProduct.PlannedRemoveArea, 1).ToString();
				this.lblConnectionACool.Text = Math.Round(evProduct.PlannedRemoveArea, 1).ToString();
				this.lblConnectionQHeat.Text = Math.Round(evProduct.PlannedHeatLoadAnbindung, 0).ToString();
				this.lblConnectionQCool.Text = Math.Round(evProduct.PlannedCoolLoadAnbindung, 0).ToString();

				// heizkreis
				this.lblCircuitCountHeat.Text = evProduct.PlannedCircuitCount.ToString();
				this.lblCircuitCountCool.Text = evProduct.PlannedCircuitCount.ToString();
                this.lblCircuitCount.Text = (complete ? evProduct.PlannedCircuitCount.ToString() : "--");
                this.lblPipeLengthHeat.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
				this.lblPipeLengthCool.Text = Math.Round(evProduct.PlannedPipeLengthPerCircuit, 1).ToString();
				this.lblMhHeat.Text = Math.Round(evProduct.PlannedMaxMhHeat, 1).ToString();
				this.lblMhCool.Text = Math.Round(evProduct.PlannedMaxMhCool, 1).ToString();
				this.lblDeltaPHeat.Text = Math.Round(evProduct.PlannedDeltaRhoHeat, 1).ToString();
				this.lblDeltaPCool.Text = Math.Round(evProduct.PlannedDeltaRhoCool, 1).ToString(); ;
				this.lblSpreizungHeat.Text = Math.Round(evProduct.PlannedSpreizungHeat, 1).ToString();
				this.lblSpreizungCool.Text = Math.Round(evProduct.PlannedSpreizungCool, 1).ToString();

				if (evProduct.PlannedProductIsConnection) {
					this.txtDistributor.Text = EuroplanRes.PlannedEurovalProductPanel_KeinEigenerHK; //"kein eigener Heizkreis"
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
				notifications = EurovalProduct.GlobalNotificationMessage;
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
					int height = this.lstError.Items[this.lstError.Items.Count - 1].Position.Y + this.lstError.Items[this.lstError.Items.Count - 1].Bounds.Height + 20;
					this.lstError.Height = height;
					this.lstError.Visible = true;
				} else {
					this.lstError.Visible = false;
				}

				if ((skipFields & FieldEnum.CORRECTIONS) == FieldEnum.NONE) {
					this.extendedCorrectionsGrid.UpdateControl(true, true);
				}

				if (graphicalMode) {
					if (this.tabs.TabPages.Contains(pageCorrections)) {
						this.tabs.TabPages.Remove(pageCorrections);
					}
					this.numArea.Enabled = false;
					this.numAreaPercentage.Enabled = false;
					this.numAreaReduced.Enabled = false;
					this.numAreaUnheated.Enabled = false;
					this.numRim.Enabled = false;
					this.numCorners.Enabled = false;
					this.cmbLayDistance.Enabled = false;
					this.cmbRimType.Enabled = false;
					this.cmbCircuits.Enabled = false;
					this.btnGraphical.Enabled = true;
				} else {
					if (!this.tabs.TabPages.Contains(pageCorrections)) {
						this.tabs.TabPages.Add(pageCorrections);
					}
					this.numArea.Enabled = true;
					this.numAreaPercentage.Enabled = true;
					this.numAreaReduced.Enabled = true;
					this.numAreaUnheated.Enabled = true;
					this.numRim.Enabled = true;
					this.numCorners.Enabled = true;
					this.btnGraphical.Enabled = false;
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
				ignoreCorrections--;
				ignoreCalculationMode--;
			}
			updateOngoing = false;
		}

		public bool AllowLeave() {
			bool allow =  this.extendedCorrectionsGrid.AllowLeave();
			if (!allow) {
				return false;
			}
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
				(this.product.Product as EurovalProduct).PlannedFloorAreaPercentage = (float)this.numAreaPercentage.Value;
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
				(this.product.Product as EurovalProduct).PlannedFloorArea = (float)this.numArea.Value;
				this.numAreaPercentage.Value = (decimal)(this.product.Product as EurovalProduct).PlannedFloorAreaPercentage;
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
				EurovalProduct evProduct = this.product.Product as EurovalProduct;
				evProduct.PlannedAreaReduced = (float)this.numAreaReduced.Value;
				if (evProduct.PlannedAreaReduced + evProduct.PlannedAreaUnheated > evProduct.PlannedFloorArea) {
					evProduct.PlannedAreaUnheated = evProduct.PlannedFloorArea - evProduct.PlannedAreaReduced;
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
				EurovalProduct evProduct = this.product.Product as EurovalProduct;
				evProduct.PlannedAreaUnheated = (float)this.numAreaUnheated.Value;
				if (evProduct.PlannedAreaReduced + evProduct.PlannedAreaUnheated > evProduct.PlannedFloorArea) {
					evProduct.PlannedAreaReduced = evProduct.PlannedFloorArea - evProduct.PlannedAreaUnheated;
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
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TROCKEN),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TROCKEN)}));
			form.SelectedConstruction = (this.product.Product as EurovalProduct).PlannedFloorConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as EurovalProduct).PlannedFloorConstruction = form.SelectedConstruction;
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
			form.SelectedConstruction = (this.product.Product as EurovalProduct).PlannedInsulationConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as EurovalProduct).PlannedInsulationConstruction = form.SelectedConstruction;
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
				(this.product.Product as EurovalProduct).PlannedRoomTemperatureBelowHeat = (float)this.numRoomTemperatureBelowHeat.Value;
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
				(this.product.Product as EurovalProduct).PlannedRoomTemperatureBelowCool = (float)this.numRoomTemperatureBelowCool.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_COOL);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void numRim_ValueChanged(object sender, EventArgs e) {
			if (ignoreRim == 0) {
				(this.product.Product as EurovalProduct).PlannedRimLength = (float)this.numRim.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.RIM_LENGTH);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void numCorners_ValueChanged(object sender, EventArgs e) {
			if (ignoreCorners == 0) {
				(this.product.Product as EurovalProduct).PlannedRimCorners = (int)this.numCorners.Value;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CORNERS);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void cmbLayDistance_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreLayDistance == 0) {
				(this.product.Product as EurovalProduct).RequestedLayDistance = (this.cmbLayDistance.SelectedItem as LayDistanceItem).layDistance;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.LAY_DISTANCE);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void cmbRimType_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreRimType == 0) {
				(this.product.Product as EurovalProduct).RequestedRimType = (this.cmbRimType.SelectedItem as RimTypeItem).rimType;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.RIM_TYPE);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}

		}

		private void cmbCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreCircuits == 0) {
				if ((this.product.Product as EurovalProduct).PlannedCorrections) {
					if (MessageBox.Show(EuroplanRes.PlannedEurovalProductPanel_HKAnzahlAendernText/*"Wenn Sie die Anzahl der Heizkreise ändern, werden die erweiterten Korrekturen zurückgesetzt. Wollen sie das wirklich machen?"*/, EuroplanRes.PlannedEurovalProductPanel_HKAnzahlAendernTitel/*"Bestätigen"*/, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
						this.UpdateControl(FieldEnum.NONE);
						return;
					} else {
						(this.product.Product as EurovalProduct).PlannedCorrections = false;
					}
				}
				if (this.cmbCircuits.SelectedIndex >= (this.cmbCircuitsContainsAutomatic ? 1 : 0)) {
					(this.product.Product as EurovalProduct).RequestedCircuits = this.cmbCircuits.SelectedIndex + (this.cmbCircuitsContainsAutomatic ? 0 : 1);
				} else {
					(this.product.Product as EurovalProduct).RequestedCircuits = null;
				}
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CIRCUIT_COUNT);
				if (this.projectChanged != null) {
					this.projectChanged(this);
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
				if (this.projectChanged != null) {
					this.projectChanged(this);
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

		private void cbSeparateCircuit_CheckedChanged(object sender, EventArgs e) {
			if (ignoreSeparateCircuit == 0) {
				if (this.product.Product.PlannedConnectedProducts.Count != 0 && !this.cbSeparateCircuit.Checked) {
					if (MessageBox.Show(EuroplanRes.Warning_SystemsConnectedButNoSeparateCircuitText, EuroplanRes.Warning_SystemsConnectedButNoSeparateCircuitTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) {
						return;
					}

					while (this.product.Product.PlannedConnectedProducts.Count > 0) {
						PlannedProduct pp = Project.Instance.GetPlannedProduct(this.product.Product.PlannedConnectedProducts[0]);
						SelectConnectionForProductForm.UnconnectProduct(pp);
						SelectConnectionForProductForm.ConnectProduct(pp, this.product.Product.PlannedConnection.Distributor);
						pp.ConfigureProduct(false);
					}
					this.product.ConfigureProduct(false);

				}

				if (!this.cbSeparateCircuit.Checked && this.product.Product.PlannedConnection != null && this.product.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					SelectConnectionForProductForm.UnconnectProduct(this.product);
				}

				this.product.Product.PlannedProductIsConnection = !this.cbSeparateCircuit.Checked;
				if (this.cbSeparateCircuit.Checked && this.product.Product.PlannedConnection == null) {
					// find a distributor in the floor of the room which can be used for connection
					foreach (Distributor dist in this.product.Product.AssociatedRoom.AssociatedFloor.Distributors) {
						if (((dist.UseForFloor && this.product.Product.Type == Product.ProductType.FBH) ||
							(dist.UseForWall && this.product.Product.Type == Product.ProductType.WH) ||
							(dist.UseForCeiling && (this.product.Product.Type == Product.ProductType.DH || this.product.Product.Type == Product.ProductType.DSH))) &&
							dist.MaxCircuits - dist.PlannedCircuits - dist.AdditionalCircuits > 0) {
							this.product.Product.PlannedConnection = new ProductConnection(dist);
							break;
						}
					}
					if (this.product.Product.PlannedConnection == null) {
						// find a distributor in another floor that can be used for connection
						foreach (Floor f in Project.Instance.Floors) {
							foreach (Distributor dist in f.Distributors) {
								if (((dist.UseForFloor && this.product.Product.Type == Product.ProductType.FBH) ||
									(dist.UseForWall && this.product.Product.Type == Product.ProductType.WH) ||
									(dist.UseForCeiling && (this.product.Product.Type == Product.ProductType.DH || this.product.Product.Type == Product.ProductType.DSH))) &&
									dist.AdditionalFloors.Contains(this.product.Product.AssociatedRoom.AssociatedFloor) &&
									dist.MaxCircuits - dist.PlannedCircuits - dist.AdditionalCircuits > 0) {
									this.product.Product.PlannedConnection = new ProductConnection(dist);
									break;
								}
							}
							if (this.product.Product.PlannedConnection != null) {
								break;
							}
						}
					}
				}

				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.SEPARATE_CIRCUIT);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void chkClip_CheckedChanged(object sender, EventArgs e) {
			(this.product.Product as EurovalProduct).UseClipSchieneKlebeband = chkClip.Checked;
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

		private void chkAnhydritEstrich_CheckedChanged(object sender, EventArgs e) {
			(this.product.Product as EurovalProduct).UseAnhydritEstrich = chkAnhydritEstrich.Checked;
			if (chkAnhydritEstrich.Checked) {
				chkClip.Checked = true;
			}
			if (this.projectChanged != null) {
				this.projectChanged(this);
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

		private void extendedCorrectionsGrid_CorrectionsChanged(object sender, EventArgs e) {
			if (this.ignoreCorrections == 0) {
				this.product.Product.PlannedProductIsConnection = !this.cbSeparateCircuit.Checked;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CORRECTIONS);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void extendedCorrectionsGrid_CorrectionsEnabledChanged(object sender, EventArgs e) {
			if (this.ignoreCorrections == 0) {
				this.product.Product.PlannedProductIsConnection = !this.cbSeparateCircuit.Checked;
				this.product.Product.ConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				this.errorMsg = this.product.Product.LastErrorMessage;
				this.UpdateControl(FieldEnum.CORRECTIONS);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void tabs_Deselecting(object sender, TabControlCancelEventArgs e) {
			if (e.TabPage == this.pageCorrections) {
				e.Cancel = !this.extendedCorrectionsGrid.AllowLeave();
			}
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
					this.product.CalculateHeat = true;
					this.product.CalculateCool = false;
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
					this.product.CalculateHeat = false;
					this.product.CalculateCool = true;
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
					this.product.CalculateHeat = true;
					this.product.CalculateCool = true;
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
								EurovalProduct evProduct = this.product.Product as EurovalProduct;
								evProduct.ClearGraphicalRepresentation();
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
								(this.product.Product as EurovalProduct).ResetProduct();
							}
						}
					}
				}
				this.product.Product.GraphicalMode = rbLayoutGraphical.Checked;
				this.UpdateControl(FieldEnum.LAYOUT_TYPE);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

		private void btnGraphical_Click(object sender, EventArgs e) {
			if (this.product != null) {
				Europlan.Common.Products.EurovalPlannerForm form = new Europlan.Common.Products.EurovalPlannerForm(this.product);
				form.ShowDialog();
				if (form.Changed && this.projectChanged != null) {
					this.projectChanged(this);
				}
				this.UpdateControl(FieldEnum.NONE);
			}
		}

		private void btnGraphicalAnbindleitungen_Click(object sender, EventArgs e) {
			ConnectionPlannerForm form = new ConnectionPlannerForm(this.product.Product, false);
			form.ShowDialog();
		}
	}
}
