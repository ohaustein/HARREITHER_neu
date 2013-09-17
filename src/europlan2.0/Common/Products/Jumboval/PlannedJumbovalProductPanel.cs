using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {
	public partial class PlannedJumbovalProductPanel : UserControl, IEditorUserControl {
		
		private PlannedProduct product = null;
		private bool gridContentChanged = false;
		private bool updateOngoing = false;

		public class LayDistanceItem {
			public Nullable<JumbovalProduct.JumbovalLayDistance> layDistance;
			public string name;

			public LayDistanceItem(Nullable<JumbovalProduct.JumbovalLayDistance> layDistance, string name) {
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
			public Nullable<JumbovalProduct.JumbovalRimType> rimType;
			public string name;

			public RimTypeItem(Nullable<JumbovalProduct.JumbovalRimType> layDistance, string name) {
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

		public PlannedJumbovalProductPanel() {
			InitializeComponent();

			this.SetLanguage();

            this.cmbType.Items.Add(Product.ProductType.FBH);
            this.cmbType.Items.Add(Product.ProductType.DH);
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
            this.lblEstrichueberdeckungUnit.Text = EuroplanRes.Unit_Zentimeter; //"cm"
            this.lblSchienenabstandUnit.Text = EuroplanRes.Unit_Meter; //"m"
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
            this.lblEstrichueberdeckung.Text = EuroplanRes.PlannedProductPanel_Estrichueberdeckung; //"Estrichüberdeckung:"
			this.lblInsulationConstruction.Text = EuroplanRes.PlannedProductPanel_Daemmkonstruktion; //"Wärmedämmkonstruktion:"
            this.lblSchienenabstand.Text = EuroplanRes.PlannedProductPanel_Schienenabstand; //"Estrichüberdeckung:"
            this.lblDistributor.Text = EuroplanRes.PlannedProductPanel_Verteileranschluss; //"Verteileranschluß:"
			this.pageCircuit.Text = EuroplanRes.PlannedProductPanel_AnbindeleitungenSeite; //"Anbindeleitungen"
			this.groupBox10.Text = EuroplanRes.PlannedProductPanel_AnbindeleitugenGruppe; //"Anbindeleitungen"
			this.chkStellAntriebe.Text = EuroplanRes.PlannedProductPanel_Stellantriebe; //"Stellantrieb(e) verwenden"
			this.pageConstruction.Text = EuroplanRes.PlannedProductPanel_AuslegungSeite; //"Auslegung"
			this.lblAreaTxt.Text = EuroplanRes.PlannedProductPanel_GesamteFlaeche; //"gesamte Fläche:"

            this.label30.Text = EuroplanRes.PlannedHithermProductPanel_Typ; //"Typ:"
			this.lblAreaUnheatedTxt.Text = EuroplanRes.PlannedEurovalProductPanel_UnbeheizteFlaeche; //"unbeheizte/ungekühlte Fläche:"
			this.lblAreaReducedTxt.Text = EuroplanRes.PlannedEurovalProductPanel_ReduzierteFlaeche; //"Fläche mit red. Heiz-/Kühlleistung:"
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
			this.cmbLayDistance.Items.Add(new LayDistanceItem(JumbovalProduct.JumbovalLayDistance.JV20, EuroplanRes.JumbovalProduct_JV20/*"JV20"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(JumbovalProduct.JumbovalLayDistance.JV30, EuroplanRes.JumbovalProduct_JV30/*"JV30"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(JumbovalProduct.JumbovalLayDistance.JV40, EuroplanRes.JumbovalProduct_JV40/*"JV40"*/));
			this.cmbLayDistance.Items.Add(new LayDistanceItem(JumbovalProduct.JumbovalLayDistance.JV50, EuroplanRes.JumbovalProduct_JV50/*"JV50"*/));

			this.cmbRimType.Items.Add(new RimTypeItem(null, EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/));
			this.cmbRimType.Items.Add(new RimTypeItem(JumbovalProduct.JumbovalRimType.JV20_80, EuroplanRes.JumbovalProduct_JV20_80/*"JV20/80"*/));
            this.cmbRimType.Items.Add(new RimTypeItem(JumbovalProduct.JumbovalRimType.JV20_100, EuroplanRes.JumbovalProduct_JV20_100/*"JV20/100"*/));

			this.cmbCircuits.Items.Add(EuroplanRes.EurovalProduct_Automatisch/*"Automatisch"*/);
            this.cmbCircuits.Items.Add(EuroplanRes.JumbovalProduct_Manuell);
			/*for (int i = 1; i <= 12; i++) {
				this.cmbCircuits.Items.Add(i.ToString());
			}*/

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
			LAYOUT_TYPE = 262144,
            ESTRICH_UEBERDECKUNG = 524288,
            SCHIENENABSTAND = 1048576,
            TYPE = 2097152
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
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(UpdateControl_ConfigureProductFinished);
                this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
            } else {
                this.UpdateControl(FieldEnum.NONE);
            }
		}

        private void UpdateControl_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(UpdateControl_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
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
        private int ignoreEstrichueberdeckung = 0;
        private int ignoreSchienenabstand = 0;
        private int ignoreType = 0;

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
                ignoreEstrichueberdeckung++;
                ignoreSchienenabstand++;
                ignoreType++;

				JumbovalProduct jvProduct = this.product.Product as JumbovalProduct;

				// at the moment, no graphical mode is available
				this.rbLayoutGraphical.Enabled = false;
				this.rbLayoutTable.Enabled = true;

				/*if (this.product.Product.AssociatedRoom.AssociatedPlan != null && this.product.Product.AssociatedRoom.RoomCoordinates.Count > 0) {
					this.rbLayoutTable.Enabled = true;
					this.rbLayoutGraphical.Enabled = true;
				} else {
					this.rbLayoutTable.Enabled = false;
					this.rbLayoutGraphical.Enabled = false;
				}*/

				bool graphicalMode = false;
				/*if (this.product.Product.GraphicalMode.HasValue) {
					graphicalMode = this.product.Product.GraphicalMode.Value;
				} else {
					if (this.product.Product.AssociatedRoom.AssociatedPlan != null && this.product.Product.AssociatedRoom.RoomCoordinates.Count > 0) {
						if (jvProduct.PlannedAreaGraphical.Count > 0 &&
							jvProduct.PlannedFloorArea == jvProduct.AssociatedRoom.Area &&
							jvProduct.PlannedAreaReduced == 0 &&
							jvProduct.PlannedAreaUnheated == 0 &&
							jvProduct.PlannedRimLength == 0 &&
							jvProduct.PlannedRimCorners == 0 &&
							jvProduct.PlannedCorrections == false) {
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
				}*/

                this.numSchienenabstand.MinValue = (decimal)JumbovalProduct.ConfigSchienenabstandMin;
                this.numSchienenabstand.MaxValue = (decimal)JumbovalProduct.ConfigSchienenabstandMax;

				if ((skipFields & FieldEnum.LAYOUT_TYPE) == FieldEnum.NONE) {
					if (graphicalMode) {
						this.rbLayoutGraphical.Checked = true;
					} else {
						this.rbLayoutTable.Checked = true;
					}
				}

                if ((skipFields & FieldEnum.TYPE) == FieldEnum.NONE) {
                    this.cmbType.SelectedItem = jvProduct.JumbovalType;
                }

				bool showHeat = this.product.RequestedHeatLoad > 0 && jvProduct.PlannedLayDistance != JumbovalProduct.JumbovalLayDistance.NONE;
				bool showCool = this.product.RequestedCoolLoad > 0 && jvProduct.PlannedLayDistance != JumbovalProduct.JumbovalLayDistance.NONE;
				bool showRim = jvProduct.PlannedAreaRim > 0;
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

				this.rbCalculateHeat.Enabled = jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT || jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
				this.rbCalculateCool.Enabled = jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT || jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
				this.rbCalculateBoth.Enabled = jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL;
				this.numCorners.Enabled = jvProduct.PlannedRimLength > 0;
				this.cmbRimType.Enabled = jvProduct.PlannedRimLength > 0;

				if (jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT) {
					rbHeat.Checked = true;
				} else if (jvProduct.CalculateMode == Product.CalculateModeEnum.COOL) {
					rbCool.Checked = true;
				} else if (jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL) {
					rbHeatAndCool.Checked = true;
				}

				// disable the following controls if the product is a connection
				this.numRim.Enabled = !jvProduct.PlannedProductIsConnection;
				this.numCorners.Enabled = this.numCorners.Enabled && !jvProduct.PlannedProductIsConnection;
				this.rbCalculateHeat.Enabled = this.rbCalculateHeat.Enabled && !jvProduct.PlannedProductIsConnection;
				this.rbCalculateCool.Enabled = this.rbCalculateCool.Enabled && !jvProduct.PlannedProductIsConnection;
				this.rbCalculateBoth.Enabled = this.rbCalculateBoth.Enabled && !jvProduct.PlannedProductIsConnection;
				this.btnDistributor.Enabled = !jvProduct.PlannedProductIsConnection;
				this.cmbLayDistance.Enabled = !jvProduct.PlannedProductIsConnection;
				this.cmbRimType.Enabled = this.cmbRimType.Enabled && !jvProduct.PlannedProductIsConnection;
				this.cmbCircuits.Enabled = !jvProduct.PlannedProductIsConnection;

                this.numCircuits.Enabled = !jvProduct.PlannedProductIsConnection && (!cmbCircuitsContainsAutomatic || cmbCircuits.SelectedIndex > 0);

				bool newCmbCircuitsContainsAutomatic = !jvProduct.ManualMode;
				bool newCmbLayDistanceContainsAutomatic = !jvProduct.ManualMode;
				bool newCmbRimTypeContainsAutomatic = !jvProduct.ManualMode && cmbRimType.Enabled;
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

                switch (jvProduct.JumbovalType) {
                    case Product.ProductType.FBH:
                        this.numArea.MaxValue = (decimal)jvProduct.AvailableFloorArea;
                        this.numAreaPercentage.MaxValue = (decimal)(jvProduct.AvailableFloorArea * 100 / jvProduct.AssociatedRoom.Area);
                        break;
                    case Product.ProductType.DH:
                        this.numArea.MaxValue = (decimal)jvProduct.AvailableCeilingArea;
                        this.numAreaPercentage.MaxValue = (decimal)(jvProduct.AvailableCeilingArea * 100 / jvProduct.AssociatedRoom.Area);
                        break;
                    default:
                        break;
                }
				if (jvProduct.AssociatedRoom.Area > 0) {
					this.numAreaPercentage.MaxValue = (decimal)(jvProduct.AvailableFloorArea * 100 / jvProduct.AssociatedRoom.Area);
				} else {
					this.numAreaPercentage.MaxValue = 100;
				}
				this.numAreaReduced.MaxValue = (decimal)this.product.PlannedArea;
				this.numAreaUnheated.MaxValue = (decimal)this.product.PlannedArea;
				this.numHeatLoad.MaxValue = (decimal)this.product.NecessaryHeatLoad;
				this.numHeatLoadPercentage.MaxValue = (decimal)(jvProduct.AssociatedRoom.NormalizedHeatLoad <= 0 ? 0 : this.product.NecessaryHeatLoad * 100 / jvProduct.AssociatedRoom.NormalizedHeatLoad);
				if ((jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT || 
					jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL)) {
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
				this.numCoolLoadPercentage.MaxValue = (decimal)(jvProduct.AssociatedRoom.NormalizedCoolLoad <= 0 ? 0 : this.product.NecessaryCoolLoad * 100 / jvProduct.AssociatedRoom.NormalizedCoolLoad);
				if ((jvProduct.CalculateMode == Product.CalculateModeEnum.COOL || 
					jvProduct.CalculateMode == Product.CalculateModeEnum.HEAT_AND_COOL)) {
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
                    if (jvProduct.JumbovalType == Product.ProductType.FBH) {
                        if (jvProduct.AssociatedRoom.Area <= 0) {
                            this.numAreaPercentage.Value = 100;
                        } else {
                            this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / jvProduct.AssociatedRoom.Area), 2);
                        }
                    } else if (jvProduct.JumbovalType == Product.ProductType.DH) {
                        if (jvProduct.AssociatedRoom.Area <= 0) {
                            this.numAreaPercentage.Value = 100;
                        } else {
                            this.numAreaPercentage.Value = Math.Round((decimal)(plannedArea * 100 / jvProduct.AssociatedRoom.Area), 2);
                        }
                    }
                }
				if ((skipFields & FieldEnum.AREA_REDUCED) == FieldEnum.NONE) {
					this.numAreaReduced.Value = Math.Round((decimal)jvProduct.PlannedAreaReduced, 2);
				}
				if ((skipFields & FieldEnum.AREA_UNHEATED) == FieldEnum.NONE) {
					this.numAreaUnheated.Value = Math.Round((decimal)jvProduct.PlannedAreaUnheated, 2);
				}
				this.txtFloorConstruction.Text = (jvProduct.PlannedFloorConstruction == null ? "" : jvProduct.PlannedFloorConstruction.Id + ": " + jvProduct.PlannedFloorConstruction.LocalizedName);
                if ((skipFields & FieldEnum.ESTRICH_UEBERDECKUNG) == FieldEnum.NONE) {
                    this.numEstrichueberdeckung.Value = Math.Round(((decimal)jvProduct.Estrichueberdeckung) * 100);
                }
				this.txtInsulationConstruction.Text = (jvProduct.PlannedInsulationConstruction == null ? "" : jvProduct.PlannedInsulationConstruction.Id + ": " + jvProduct.PlannedInsulationConstruction.LocalizedName);
                if ((skipFields & FieldEnum.SCHIENENABSTAND) == FieldEnum.NONE) {
                    this.numSchienenabstand.MinValue = (decimal)JumbovalProduct.ConfigSchienenabstandMin;
                    this.numSchienenabstand.MaxValue = (decimal)JumbovalProduct.ConfigSchienenabstandMax;
                    this.numSchienenabstand.Value = Math.Round((decimal)jvProduct.CheckedSchienenabstand, 2);
                }
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_HEAT) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowHeat.Value = Math.Round((decimal)jvProduct.PlannedRoomTemperatureBelowHeat, 2);
				}
				if ((skipFields & FieldEnum.ROOM_TEMERATURE_BELOW_COOL) == FieldEnum.NONE) {
					this.numRoomTemperatureBelowCool.Value = Math.Round((decimal)jvProduct.PlannedRoomTemperatureBelowCool, 2);
				}
				if ((skipFields & FieldEnum.RIM_LENGTH) == FieldEnum.NONE) {
					this.numRim.Value = Math.Round((decimal)jvProduct.PlannedRimLength, 2);
				}
				if ((skipFields & FieldEnum.CORNERS) == FieldEnum.NONE) {
					this.numCorners.Value = (decimal)jvProduct.PlannedRimCorners;
				}

				if ((skipFields & FieldEnum.LAY_DISTANCE) == FieldEnum.NONE) {
					if (jvProduct.RequestedLayDistance == null && !this.cmbLayDistanceContainsAutomatic) {
						jvProduct.RequestedLayDistance = JumbovalProduct.JumbovalLayDistance.JV30;
					}
					this.cmbLayDistance.SelectedItem = new LayDistanceItem(jvProduct.RequestedLayDistance, "");
				}
				if ((skipFields & FieldEnum.RIM_TYPE) == FieldEnum.NONE) {
					if (jvProduct.RequestedRimType == null && !this.cmbRimTypeContainsAutomatic && !this.cmbRimTypeContainsNone) {
						jvProduct.RequestedRimType = JumbovalProduct.JumbovalRimType.JV20_80;
					}
					this.cmbRimType.SelectedItem = new RimTypeItem(this.cmbRimType.Enabled ? jvProduct.RequestedRimType : null, "");
				}
				if ((skipFields & FieldEnum.CIRCUIT_COUNT) == FieldEnum.NONE) {
					if (jvProduct.RequestedCircuits != null) {
						this.cmbCircuits.SelectedIndex = (this.cmbCircuitsContainsAutomatic ? 1 : 0);
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
					this.cbSeparateCircuit.Checked = !jvProduct.PlannedProductIsConnection;
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
				this.lblQRestHeat.Text = Math.Round(qRestHeat, 2).ToString("+0.00;-0.00");
				this.lblQRestCool.Text = Math.Round(qRestCool, 2).ToString("+0.00;-0.00");

				// Randzone
				if (complete && jvProduct.PlannedRimType.HasValue) {
					switch (jvProduct.PlannedRimLayDistance) {
						case JumbovalProduct.JumbovalLayDistance.JV20:
							this.lblRimVaHeat.Text = EuroplanRes.JumbovalProduct_JV20; //"JV20"
                            this.lblRimVaCool.Text = EuroplanRes.JumbovalProduct_JV20; //"JV20"
                            this.lblRimVa.Text = EuroplanRes.JumbovalProduct_JV20 + "/" + jvProduct.PlannedRimWidth.ToString(); //"JV20"
                            break;
						case JumbovalProduct.JumbovalLayDistance.JV30:
                            this.lblRimVaHeat.Text = EuroplanRes.JumbovalProduct_JV30; //"JV30"
                            this.lblRimVaCool.Text = EuroplanRes.JumbovalProduct_JV30; //"JV30"
                            this.lblRimVa.Text = EuroplanRes.JumbovalProduct_JV30 + "/" + jvProduct.PlannedRimWidth.ToString(); //"JV30"
                            break;
						case JumbovalProduct.JumbovalLayDistance.JV40:
                            this.lblRimVaHeat.Text = EuroplanRes.JumbovalProduct_JV40; //"JV40"
                            this.lblRimVaCool.Text = EuroplanRes.JumbovalProduct_JV40; //"JV40"
                            this.lblRimVa.Text = EuroplanRes.JumbovalProduct_JV40 + "/" + jvProduct.PlannedRimWidth.ToString(); //"JV40"
                            break;
						case JumbovalProduct.JumbovalLayDistance.JV50:
                            this.lblRimVaHeat.Text = EuroplanRes.JumbovalProduct_JV50; //"JV50"
                            this.lblRimVaCool.Text = EuroplanRes.JumbovalProduct_JV50; //"JV50"
                            this.lblRimVa.Text = EuroplanRes.JumbovalProduct_JV50 + "/" + jvProduct.PlannedRimWidth.ToString(); //"JV50"
                            break;
						default:
							this.lblRimVaHeat.Text = "--";
                            this.lblRimVaCool.Text = "--";
                            this.lblRimVa.Text = "--";
                            break;
					}
					this.lblRimBHeat.Text = jvProduct.PlannedRimWidth.ToString();
					this.lblRimBCool.Text = jvProduct.PlannedRimWidth.ToString();
					this.lblRimTfbHeat.Text = Math.Round(jvProduct.PlannedFloorTemperatureHeatRim, 1).ToString();
					this.lblRimTfbCool.Text = Math.Round(jvProduct.PlannedFloorTemperatureCoolRim, 1).ToString();
					this.lblRimQHeat.Text = Math.Round(jvProduct.PlannedHeatLoadRim, 0).ToString();
					this.lblRimQCool.Text = Math.Round(jvProduct.PlannedCoolLoadRim, 0).ToString();
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
				if (complete && jvProduct.PlannedLayDistance.HasValue) {
					switch (jvProduct.PlannedLayDistance) {
						case JumbovalProduct.JumbovalLayDistance.JV20:
                            this.lblResidenceVaHeat.Text = EuroplanRes.JumbovalProduct_JV20; //"JV20"
                            this.lblResidenceVaCool.Text = EuroplanRes.JumbovalProduct_JV20; //"JV20"
                            this.lblResidenceVa.Text = EuroplanRes.JumbovalProduct_JV20; //"JV20"
							break;
						case JumbovalProduct.JumbovalLayDistance.JV30:
                            this.lblResidenceVaHeat.Text = EuroplanRes.JumbovalProduct_JV30; //"JV30"
                            this.lblResidenceVaCool.Text = EuroplanRes.JumbovalProduct_JV30; //"JV30"
                            this.lblResidenceVa.Text = EuroplanRes.JumbovalProduct_JV30; //"JV30"
                            break;
						case JumbovalProduct.JumbovalLayDistance.JV40:
                            this.lblResidenceVaHeat.Text = EuroplanRes.JumbovalProduct_JV40; //"JV40"
                            this.lblResidenceVaCool.Text = EuroplanRes.JumbovalProduct_JV40; //"JV40"
                            this.lblResidenceVa.Text = EuroplanRes.JumbovalProduct_JV40; //"JV40"
                            break;
						case JumbovalProduct.JumbovalLayDistance.JV50:
                            this.lblResidenceVaHeat.Text = EuroplanRes.JumbovalProduct_JV50; //"JV50"
                            this.lblResidenceVaCool.Text = EuroplanRes.JumbovalProduct_JV50; //"JV50"
                            this.lblResidenceVa.Text = EuroplanRes.JumbovalProduct_JV50; //"JV50"
                            break;
						default:
							this.lblResidenceVaHeat.Text = "--";
							this.lblResidenceVaCool.Text = "--";
                            this.lblResidenceVa.Text = "--";
                            break;
					}
					this.lblResidenceAHeat.Text = Math.Round(jvProduct.PlannedAreaResidenceHeated, 1).ToString();
					this.lblResidenceACool.Text = Math.Round(jvProduct.PlannedAreaResidenceHeated, 1).ToString();
					this.lblResidenceTfbHeat.Text = Math.Round(jvProduct.PlannedFloorTemperatureHeatResidence, 1).ToString();
					this.lblResidenceTfbCool.Text = Math.Round(jvProduct.PlannedFloorTemperatureCoolResidence, 1).ToString();
					this.lblResidenceQHeat.Text = Math.Round(jvProduct.PlannedHeatLoadResidence, 0).ToString();
					this.lblResidenceQCool.Text = Math.Round(jvProduct.PlannedCoolLoadResidence, 0).ToString();
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
				this.lblConnectionAHeat.Text = Math.Round(jvProduct.PlannedRemoveArea, 1).ToString();
				this.lblConnectionACool.Text = Math.Round(jvProduct.PlannedRemoveArea, 1).ToString();
				this.lblConnectionQHeat.Text = Math.Round(jvProduct.PlannedHeatLoadAnbindung, 0).ToString();
				this.lblConnectionQCool.Text = Math.Round(jvProduct.PlannedCoolLoadAnbindung, 0).ToString();

				// heizkreis
				this.lblCircuitCountHeat.Text = jvProduct.PlannedCircuitCount.ToString();
				this.lblCircuitCountCool.Text = jvProduct.PlannedCircuitCount.ToString();
                //this.lblCircuitCount.Text = (complete ? jvProduct.PlannedCircuitCount.ToString() : "--");
                this.numCircuits.Value = jvProduct.PlannedCircuitCount;
                this.lblPipeLengthHeat.Text = Math.Round(jvProduct.PlannedPipeLengthPerCircuit, 1).ToString();
				this.lblPipeLengthCool.Text = Math.Round(jvProduct.PlannedPipeLengthPerCircuit, 1).ToString();
				this.lblMhHeat.Text = Math.Round(jvProduct.PlannedMaxMhHeat, 1).ToString();
				this.lblMhCool.Text = Math.Round(jvProduct.PlannedMaxMhCool, 1).ToString();
                this.lblDeltaPHeat.Text = Math.Round(jvProduct.PlannedDeltaRhoInklVentilHeat, 1).ToString();
                this.lblDeltaPCool.Text = Math.Round(jvProduct.PlannedDeltaRhoInklVentilCool, 1).ToString(); ;
				this.lblSpreizungHeat.Text = Math.Round(jvProduct.PlannedSpreizungHeat, 1).ToString();
				this.lblSpreizungCool.Text = Math.Round(jvProduct.PlannedSpreizungCool, 1).ToString();

				if (jvProduct.PlannedProductIsConnection) {
					this.txtDistributor.Text = EuroplanRes.PlannedEurovalProductPanel_KeinEigenerHK; //"kein eigener Heizkreis"
				} else {
					if (jvProduct.PlannedConnection == null) {
						this.txtDistributor.Text = "";
					} else {
						this.txtDistributor.Text = jvProduct.PlannedConnection.ToString();
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
				notifications = JumbovalProduct.GlobalNotificationMessage;
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
                    this.numCircuits.Enabled = false;
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
                ignoreEstrichueberdeckung--;
                ignoreSchienenabstand--;
                ignoreType--;
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
						pp.Product.StartConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
					}
				}
			}
			if (gridContentChanged) {
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
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
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(ChkCoverHeatLoad_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
				ignoreHeatLoad--;
				ignoreHeatLoadPercentage--;
			}
		}

        private void ChkCoverHeatLoad_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(ChkCoverHeatLoad_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.NONE);
        }

		private void chkCoverCoolLoad_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCoverCoolLoad == 0) {
				ignoreCoolLoad++;
				ignoreCoolLoadPercentage++;
				this.product.CoverCoolLoad = this.chkCoverCoolLoad.Checked;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(ChkCoverCoolLoad_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
				ignoreCoolLoad--;
				ignoreCoolLoadPercentage--;
			}
		}

        private void ChkCoverCoolLoad_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(ChkCoverCoolLoad_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.NONE);
        }

		private void numHeatLoadPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreHeatLoadPercentage == 0) {
				ignoreHeatLoad++;
				this.product.RequestedHeatLoadPercentage = (float)this.numHeatLoadPercentage.Value;
				this.numHeatLoad.Value = (decimal)this.product.RequestedHeatLoad;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumHeatLoadPercentage_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
				ignoreHeatLoad--;
			}
		}

        private void NumHeatLoadPercentage_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumHeatLoadPercentage_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.HEAT_LOAD_PERCENTAGE);
        }

		private void numCoolLoadPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreCoolLoadPercentage == 0) {
				ignoreCoolLoad++;
				this.product.RequestedCoolLoadPercentage = (float)this.numCoolLoadPercentage.Value;
				this.numCoolLoad.Value = (decimal)this.product.RequestedCoolLoad;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumCoolLoadPercentage_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
				ignoreCoolLoad--;
			}
		}

        private void NumCoolLoadPercentage_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumCoolLoadPercentage_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.COOL_LOAD_PERCENTAGE);
        }

		private void numHeatLoad_ValueChanged(object sender, EventArgs e) {
			if (ignoreHeatLoad == 0) {
				ignoreHeatLoadPercentage++;
				this.product.RequestedHeatLoad = (double)this.numHeatLoad.Value;
				this.numHeatLoadPercentage.Value = (decimal)this.product.RequestedHeatLoadPercentage;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumHeatLoad_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
				ignoreHeatLoadPercentage--;
			}
		}

        private void NumHeatLoad_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumHeatLoad_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.HEAT_LOAD);
        }

		private void numCoolLoad_ValueChanged(object sender, EventArgs e) {
			if (ignoreCoolLoad == 0) {
				ignoreCoolLoadPercentage++;
				this.product.RequestedCoolLoad = (double)this.numCoolLoad.Value;
				this.numCoolLoadPercentage.Value = (decimal)this.product.RequestedCoolLoadPercentage;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumCoolLoad_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
				ignoreCoolLoadPercentage--;
			}
		}

        private void NumCoolLoad_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumCoolLoad_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.COOL_LOAD);
        }

		private void numAreaPercentage_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaPercentage == 0) {
				(this.product.Product as JumbovalProduct).PlannedFloorAreaPercentage = (float)this.numAreaPercentage.Value;
                ignoreArea++;
                this.numArea.Value = (decimal)this.product.PlannedArea;
                ignoreArea--;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumAreaPercentage_ConfigureProductFinished);
                this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
                if (this.projectChanged != null) {
                    this.projectChanged(this);
                }
            }
		}

        private void NumAreaPercentage_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumAreaPercentage_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.AREA_PERCENTAGE);
        }

		private void numArea_ValueChanged(object sender, EventArgs e) {
			if (ignoreArea == 0) {
				(this.product.Product as JumbovalProduct).PlannedFloorArea = (float)this.numArea.Value;
                ignoreAreaPercentage++;
                this.numAreaPercentage.Value = (decimal)(this.product.Product as JumbovalProduct).PlannedFloorAreaPercentage;
                ignoreAreaPercentage--;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumArea_ConfigureProductFinished);
                this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
                if (this.projectChanged != null) {
                    this.projectChanged(this);
                }
            }
		}

        private void NumArea_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumArea_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.AREA);
        }

		private void numAreaReduced_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaReduced == 0) {
				JumbovalProduct jvProduct = this.product.Product as JumbovalProduct;
				jvProduct.PlannedAreaReduced = (float)this.numAreaReduced.Value;
				if (jvProduct.PlannedAreaReduced + jvProduct.PlannedAreaUnheated > jvProduct.PlannedFloorArea) {
					jvProduct.PlannedAreaUnheated = jvProduct.PlannedFloorArea - jvProduct.PlannedAreaReduced;
				}
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumAreaReduced_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
                if (this.projectChanged != null) {
                    this.projectChanged(this);
                }
            }
		}

        private void NumAreaReduced_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumAreaReduced_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.AREA_REDUCED);
        }

		private void numAreaUnheated_ValueChanged(object sender, EventArgs e) {
			if (ignoreAreaUnheated == 0) {
				JumbovalProduct jvProduct = this.product.Product as JumbovalProduct;
				jvProduct.PlannedAreaUnheated = (float)this.numAreaUnheated.Value;
				if (jvProduct.PlannedAreaReduced + jvProduct.PlannedAreaUnheated > jvProduct.PlannedFloorArea) {
					jvProduct.PlannedAreaReduced = jvProduct.PlannedFloorArea - jvProduct.PlannedAreaUnheated;
				}
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumAreaUnheated_ConfigureProductFinished);
                this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
                if (this.projectChanged != null) {
                    this.projectChanged(this);
                }
            }
		}

        private void NumAreaUnheated_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumAreaUnheated_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.AREA_UNHEATED);
        }

        private void numEstrichueberdeckung_ValueChanged(object sender, EventArgs e) {
            if (ignoreEstrichueberdeckung == 0) {
                JumbovalProduct jvProduct = this.product.Product as JumbovalProduct;
                jvProduct.Estrichueberdeckung = (float)this.numEstrichueberdeckung.Value / 100;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumEstrichueberdeckung_ConfigureProductFinished);
                this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
                if (this.projectChanged != null) {
                    this.projectChanged(this);
                }
            }
        }

        private void NumEstrichueberdeckung_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumEstrichueberdeckung_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.ESTRICH_UEBERDECKUNG);
        }

        private void numSchienenabstand_ValueChanged(object sender, EventArgs e) {
            if (ignoreSchienenabstand == 0) {
                JumbovalProduct jvProduct = this.product.Product as JumbovalProduct;
                jvProduct.Schienenabstand = (float)this.numSchienenabstand.Value;
                this.UpdateControl(FieldEnum.SCHIENENABSTAND);
                if (this.projectChanged != null) {
                    this.projectChanged(this);
                }
            }
        }

        private void btnFloorConstruction_Click(object sender, EventArgs e) {
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.FloorConstruction,
				new List<ConstructionType>(new ConstructionType[] {
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TROCKEN),
					ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TROCKEN),
                    ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_BETON),
                    ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_BETON)}));
			form.SelectedConstruction = (this.product.Product as JumbovalProduct).PlannedFloorConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as JumbovalProduct).PlannedFloorConstruction = form.SelectedConstruction;
                    this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(BtnFloorConstruction_ConfigureProductFinished);
					this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				}
			}
			form.Dispose();
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

        private void BtnFloorConstruction_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(BtnFloorConstruction_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.NONE);
        }

		private void btnInsulationConstruction_Click(object sender, EventArgs e) {
			SelectConstructionForm form = new SelectConstructionForm(ConstructionScopeEnum.InsulationConstruction, null);
			form.SelectedConstruction = (this.product.Product as JumbovalProduct).PlannedInsulationConstruction;
			if (form.ShowDialog() == DialogResult.OK) {
				if (form.SelectedConstruction != null) {
					(this.product.Product as JumbovalProduct).PlannedInsulationConstruction = form.SelectedConstruction;
                    this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(BtnInsulationConstruction_ConfigureProductFinished);
					this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					this.product.Product.AssociatedRoom.GetFloor().LastInsulationConstruction = form.SelectedConstruction;
				}
			}
			form.Dispose();
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

        private void BtnInsulationConstruction_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(BtnInsulationConstruction_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.NONE);
        }

		private void numRoomTemperatureBelowHeat_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowHeat == 0) {
				(this.product.Product as JumbovalProduct).PlannedRoomTemperatureBelowHeat = (float)this.numRoomTemperatureBelowHeat.Value;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumroomTemperatureBelowHeat_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void NumroomTemperatureBelowHeat_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumroomTemperatureBelowHeat_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_HEAT);
        }

		private void numRoomTemperatureBelowCool_ValueChanged(object sender, EventArgs e) {
			if (ignoreRoomTemperatureBelowCool == 0) {
				(this.product.Product as JumbovalProduct).PlannedRoomTemperatureBelowCool = (float)this.numRoomTemperatureBelowCool.Value;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumRoomTemperatureBelowCool_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void NumRoomTemperatureBelowCool_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumRoomTemperatureBelowCool_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.ROOM_TEMERATURE_BELOW_COOL);
        }

		private void numRim_ValueChanged(object sender, EventArgs e) {
			if (ignoreRim == 0) {
				(this.product.Product as JumbovalProduct).PlannedRimLength = (float)this.numRim.Value;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumRim_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void NumRim_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumRim_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.RIM_LENGTH);
        }

		private void numCorners_ValueChanged(object sender, EventArgs e) {
			if (ignoreCorners == 0) {
				(this.product.Product as JumbovalProduct).PlannedRimCorners = (int)this.numCorners.Value;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumCorners_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void NumCorners_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumCorners_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.CORNERS);
        }

		private void cmbLayDistance_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreLayDistance == 0) {
				(this.product.Product as JumbovalProduct).RequestedLayDistance = (this.cmbLayDistance.SelectedItem as LayDistanceItem).layDistance;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(CmbLayDistance_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void CmbLayDistance_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(CmbLayDistance_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.LAY_DISTANCE);
        }

		private void cmbRimType_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreRimType == 0) {
				(this.product.Product as JumbovalProduct).RequestedRimType = (this.cmbRimType.SelectedItem as RimTypeItem).rimType;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(CmbRimType_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}

		}

        private void CmbRimType_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(CmbRimType_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.RIM_TYPE);
        }

		private void cmbCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (ignoreCircuits == 0) {
				if ((this.product.Product as JumbovalProduct).PlannedCorrections) {
					if (MessageBox.Show(EuroplanRes.PlannedEurovalProductPanel_HKAnzahlAendernText/*"Wenn Sie die Anzahl der Heizkreise ändern, werden die erweiterten Korrekturen zurückgesetzt. Wollen sie das wirklich machen?"*/, EuroplanRes.PlannedEurovalProductPanel_HKAnzahlAendernTitel/*"Bestätigen"*/, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
						this.UpdateControl(FieldEnum.NONE);
						return;
					} else {
						(this.product.Product as JumbovalProduct).PlannedCorrections = false;
					}
				}
                if (this.product.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.TICHELMANN && (this.product.Product.PlannedFloorArea + this.product.Product.PlannedCeilingArea) > JumbovalProduct.ConfigAutomaticCalcWarningArea && this.cmbCircuitsContainsAutomatic && this.cmbCircuits.SelectedIndex == 0) {
                    if (MessageBox.Show(EuroplanRes.PlannedJumbovalProductPanel_WarnungGrosserRaum, EuroplanRes.PlannedJumbovalProductPanel_WarnungGrosserRaumTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
                        this.cmbCircuits.SelectedIndex = 1;
                    }
                }
				/*if (this.cmbCircuits.SelectedIndex >= (this.cmbCircuitsContainsAutomatic ? 1 : 0)) {
					(this.product.Product as JumbovalProduct).RequestedCircuits = this.cmbCircuits.SelectedIndex + (this.cmbCircuitsContainsAutomatic ? 0 : 1);
				} else {
					(this.product.Product as JumbovalProduct).RequestedCircuits = null;
				}*/
                if (this.cmbCircuitsContainsAutomatic) {
                    (this.product.Product as JumbovalProduct).RequestedCircuits = (this.cmbCircuits.SelectedIndex == 0 ? null : (Nullable<int>)this.numCircuits.Value);
                } else {
                    (this.product.Product as JumbovalProduct).RequestedCircuits = (int)this.numCircuits.Value;
                }
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(CmbCircuits_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void CmbCircuits_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(CmbCircuits_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.CIRCUIT_COUNT);
        }

		private void rbCalculationType_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationType == 0) {
				this.product.CalculateHeat = this.rbCalculateHeat.Checked || this.rbCalculateBoth.Checked;
				this.product.CalculateCool = this.rbCalculateCool.Checked || this.rbCalculateBoth.Checked;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(RbCalculationType_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void RbCalculationType_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(RbCalculationType_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.CALCULATION_TYPE);
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
			SelectConnectionForProductForm form = new SelectConnectionForProductForm(this.product, this.product.Product.AssociatedRoom.AssociatedFloor, true);
			form.ShowDialog();

            this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(BtnDistributor_ConfigureProductFinished);
			this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			if (form.DialogResult == DialogResult.OK && this.projectChanged != null) {
				this.projectChanged(this);
			}
			form.Dispose();
		}

        private void BtnDistributor_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(BtnDistributor_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.COOL_LOAD);
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
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(BtnConnectionPipes_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
			form.Dispose();
		}

        private void BtnConnectionPipes_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(BtnConnectionPipes_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.NONE);
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

                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(CbSeparateCircuit_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void CbSeparateCircuit_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(CbSeparateCircuit_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.SEPARATE_CIRCUIT);
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
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(ExtendedCorrectionsGrid_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void ExtendedCorrectionsGrid_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(ExtendedCorrectionsGrid_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.CORRECTIONS);
        }

		private void extendedCorrectionsGrid_CorrectionsEnabledChanged(object sender, EventArgs e) {
			if (this.ignoreCorrections == 0) {
				this.product.Product.PlannedProductIsConnection = !this.cbSeparateCircuit.Checked;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(ExtendedCorrectionsGridEnabled_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
			}
		}

        private void ExtendedCorrectionsGridEnabled_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(ExtendedCorrectionsGridEnabled_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.CORRECTIONS);
        }

		private void tabs_Deselecting(object sender, TabControlCancelEventArgs e) {
			if (e.TabPage == this.pageCorrections) {
				e.Cancel = !this.extendedCorrectionsGrid.AllowLeave();
			}
			if (e.TabPage == this.pageCircuit && gridContentChanged) {
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(tabsDeselecting_ConfigureProductFinished);
				this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
				gridContentChanged = false;
			}
		}

        private void tabsDeselecting_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(tabsDeselecting_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.NONE);
        }

		private void rbHeat_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationMode == 0) {
				if (this.rbHeat.Checked) {
					this.product.Product.CalculateMode = Product.CalculateModeEnum.HEAT;
					this.product.CalculateHeat = true;
					this.product.CalculateCool = false;
                    this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(RbHeat_ConfigureProductFinished);
					this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					if (this.projectChanged != null) {
						this.projectChanged(this);
					}
				}
			}
		}

        private void RbHeat_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(RbHeat_ConfigureProductFinished);
            this.UpdateControl(FieldEnum.NONE);
        }

		private void rbCool_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationMode == 0) {
				if (this.rbCool.Checked) {
					this.product.Product.CalculateMode = Product.CalculateModeEnum.COOL;
					this.product.CalculateHeat = false;
					this.product.CalculateCool = true;
                    this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(RbCool_ConfigureProductFinished);
					this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					if (this.projectChanged != null) {
						this.projectChanged(this);
					}
				}
			}
		}

        private void RbCool_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(RbCool_ConfigureProductFinished);
            this.UpdateControl(FieldEnum.NONE);
        }

		private void rbHeatAndCool_CheckedChanged(object sender, EventArgs e) {
			if (ignoreCalculationMode == 0) {
				if (this.rbHeatAndCool.Checked) {
					this.product.Product.CalculateMode = Product.CalculateModeEnum.HEAT_AND_COOL;
					this.product.CalculateHeat = true;
					this.product.CalculateCool = true;
                    this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(RbHeatAndCool_ConfigureProductFinished);
					this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
					if (this.projectChanged != null) {
						this.projectChanged(this);
					}
				}
			}
		}

        private void RbHeatAndCool_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(RbHeatAndCool_ConfigureProductFinished);
            this.UpdateControl(FieldEnum.NONE);
        }

		private void btnRestwaerme_Click(object sender, EventArgs e) {
			this.product.RestwaermeUebernehmen();
            this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(BtnRestwaerme_ConfigureProductFinished);
			this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

        private void BtnRestwaerme_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(BtnRestwaerme_ConfigureProductFinished);
            this.UpdateControl(FieldEnum.NONE);
        }

		private void btnRestkaelte_Click(object sender, EventArgs e) {
			this.product.RestkaelteUebernehmen();
            this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(BtnRestkaelte_ConfigureProductFinished);
			this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

        private void BtnRestkaelte_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(BtnRestkaelte_ConfigureProductFinished);
            this.UpdateControl(FieldEnum.NONE);
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
								JumbovalProduct jvProduct = this.product.Product as JumbovalProduct;
								jvProduct.ClearGraphicalRepresentation();
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
								(this.product.Product as JumbovalProduct).ResetProduct();
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
			/*if (this.product != null) {
				Europlan.Common.Products.EurovalPlannerForm form = new Europlan.Common.Products.EurovalPlannerForm(this.product);
				form.ShowDialog();
				if (form.Changed && this.projectChanged != null) {
					this.projectChanged(this);
				}
				this.UpdateControl(FieldEnum.NONE);
			}*/
		}

		private void btnGraphicalAnbindleitungen_Click(object sender, EventArgs e) {
			ConnectionPlannerForm form = new ConnectionPlannerForm(this.product.Product, false);
			form.ShowDialog();
		}

        private void cmbType_SelectedValueChanged(object sender, EventArgs e) {
            if (ignoreType == 0) {
                if (this.cmbType.SelectedItem is Product.ProductType && this.product.Product.Type != (Product.ProductType)this.cmbType.SelectedItem) {
                    (this.product.Product as JumbovalProduct).JumbovalType = (Product.ProductType)this.cmbType.SelectedItem;
                    if ((this.product.Product as JumbovalProduct).JumbovalType == Product.ProductType.FBH) {
                        (this.product.Product as JumbovalProduct).PlannedFloorArea = this.product.Product.AvailableFloorArea;
                    } else if ((this.product.Product as JumbovalProduct).JumbovalType == Product.ProductType.DH) {
                        (this.product.Product as JumbovalProduct).PlannedCeilingArea = this.product.Product.AvailableCeilingArea;
                    }
                    this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(CmbType_ConfigureProductFinished);
                    this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
                    if (this.projectStructureChanged != null) {
                        this.projectStructureChanged(this);
                    }
                }
            }
        }

        private void CmbType_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(CmbType_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.TYPE);
        }

        private void numCircuits_ValueChanged(object sender, EventArgs e) {
            if (this.cmbCircuits.SelectedIndex > 0 || !this.cmbLayDistanceContainsAutomatic) {
                (this.product.Product as JumbovalProduct).RequestedCircuits = (int)numCircuits.Value;
                this.product.Product.ConfigureProductFinished += new EventHandler<Product.ConfigureProductFinishedArgs>(NumCircuits_ConfigureProductFinished);
                this.product.Product.StartConfigureProduct(this.product.RequestedHeatLoad, this.product.RequestedCoolLoad, this.product.CalculateHeat, this.product.CalculateCool, false);
                if (this.projectChanged != null) {
                    this.projectChanged(this);
                }
            }
        }

        private void NumCircuits_ConfigureProductFinished(object sender, Product.ConfigureProductFinishedArgs e) {
            this.product.Product.ConfigureProductFinished -= new EventHandler<Product.ConfigureProductFinishedArgs>(NumCircuits_ConfigureProductFinished);
            this.errorMsg = this.product.Product.LastErrorMessage;
            this.UpdateControl(FieldEnum.CIRCUIT_COUNT);
        }
	}
}
