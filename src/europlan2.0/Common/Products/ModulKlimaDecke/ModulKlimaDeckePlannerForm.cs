using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using WW.Math;

namespace Europlan.Common.Products {
	public partial class ModulKlimaDeckePlannerForm : Form {

		private ModulKlimaDeckeConstructionGlatt glatt = null;
		private ModulKlimaDeckeConstructionAkustik akustik = null;
		private ModulKlimaDeckeConstructionKassette kassette = null;

		private int ignoreRotation = 0;
		private bool newVisible = false;
		private PlannedProduct plannedProduct;

		private bool changed = false;
		private bool updateOngoing = false;

		public ModulKlimaDeckePlannerForm(PlannedProduct plannedProduct) {
			updateOngoing = true;
			InitializeComponent();
			this.SetLanguage();
			switch (Product.ConfigPlanMeasureEnum) {
				case Product.PlanMeasureEnum.PM_CENTIMETER:
					this.lblBeplankungBreiteUnit.Text = Europlan.Common.EuroplanRes.Unit_Zentimeter;
					this.lblBeplankungLaengeUnit.Text = Europlan.Common.EuroplanRes.Unit_Zentimeter;
					this.numBeplankungBreite.EditType = NumericBox.NumericEditType.BEPLANKUNG_CM;
					this.numBeplankungLaenge.EditType = NumericBox.NumericEditType.BEPLANKUNG_CM;
					break;

				case Product.PlanMeasureEnum.PM_MILLIMETER:
					this.lblBeplankungBreiteUnit.Text = Europlan.Common.EuroplanRes.Unit_Millimeter;
					this.lblBeplankungLaengeUnit.Text = Europlan.Common.EuroplanRes.Unit_Millimeter;
					this.numBeplankungBreite.EditType = NumericBox.NumericEditType.BEPLANKUNG_MM;
					this.numBeplankungLaenge.EditType = NumericBox.NumericEditType.BEPLANKUNG_MM;
					break;

				case Product.PlanMeasureEnum.PM_METER:
				default:
					this.lblBeplankungBreiteUnit.Text = Europlan.Common.EuroplanRes.Unit_Meter;
					this.lblBeplankungLaengeUnit.Text = Europlan.Common.EuroplanRes.Unit_Meter;
					this.numBeplankungBreite.EditType = NumericBox.NumericEditType.BEPLANKUNG_M;
					this.numBeplankungLaenge.EditType = NumericBox.NumericEditType.BEPLANKUNG_M;
					break;
			}
			this.plannedProduct = plannedProduct;
            this.btnShowPlanBg.Checked = Product.ShowPlanInBackground;
            this.cbAutomaticOrientation.Checked = this.modulKlimaDeckePlanner.AutomaticOrientation;
			this.cbAutomaticRows.Checked = this.modulKlimaDeckePlanner.AutomaticRows;
			ModulKlimaDeckeProduct product = plannedProduct.Product as ModulKlimaDeckeProduct;

			ModulKlimaDeckeProduct.ModulCeilingConstructionEnum defaultConstrType = (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum)ModulKlimaDeckeProduct.ConfigModulCeilingConstruction;

			this.modulKlimaDeckePlanner.Product = product;
			if (product.GraphConstruction == null) {
				product.GraphConstruction = (defaultConstrType == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE ? (ModulKlimaDeckeConstruction)new ModulKlimaDeckeConstructionKassette() : (ModulKlimaDeckeConstruction)new ModulKlimaDeckeConstructionGlatt());
				//product.GraphConstruction.Planner = this.modulKlimaDeckePlanner;
				product.GraphConstruction.Product = product;
				product.GraphConstruction.PlanPanel = this.planPanel;
				product.GraphConstruction.RotationRelativeToPlan = 0;
			}
			this.glatt = product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
			this.akustik = product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
			this.kassette = product.GraphConstruction as ModulKlimaDeckeConstructionKassette;

			if (this.glatt == null) {
				this.glatt = new ModulKlimaDeckeConstructionGlatt();
				//this.glatt.Planner = this.modulKlimaDeckePlanner;
				this.glatt.Product = product;
				this.glatt.PlanPanel = this.planPanel;
				this.glatt.RotationRelativeToPlan = 0;
			} else {
				//this.glatt.Planner = this.modulKlimaDeckePlanner;
				this.glatt.PlanPanel = this.planPanel;
			}
			this.glatt.RecalculateSchienen();

			if (this.akustik == null) {
				this.akustik = new ModulKlimaDeckeConstructionAkustik();
				//this.akustik.Planner = this.modulKlimaDeckePlanner;
				this.akustik.Product = product;
				this.akustik.PlanPanel = this.planPanel;
				this.akustik.RotationRelativeToPlan = 0;
			} else {
				//this.akustik.Planner = this.modulKlimaDeckePlanner;
				this.akustik.PlanPanel = this.planPanel;
			}
			this.numRandfries.Value = (decimal)Math.Round(this.akustik.Randfries * 100.0);
			this.akustik.RecalculateSchienen();

			if (this.kassette == null) {
				this.kassette = new ModulKlimaDeckeConstructionKassette();
				//this.kassette.Planner = this.modulKlimaDeckePlanner;
				this.kassette.Product = product;
				this.kassette.PlanPanel = this.planPanel;
				this.kassette.RotationRelativeToPlan = 0;
			} else {
				//this.kassette.Planner = this.modulKlimaDeckePlanner;
				this.kassette.PlanPanel = this.planPanel;
			}
			this.kassette.RecalculateSchienen();


			if (product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL) {
				this.rbCProfil.Checked = true;
				this.rbHolzstaffel.Checked = false;
				this.rbKassettendecke.Checked = false;
				//this.rbGlatt.Visible = true;
				//this.rbAkustik.Visible = true;
				//this.rbKassetten.Visible = false;
				this.grpBeplankung.Enabled = true;
				this.grpCeilingContruction.Visible = true;
			} else if (product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL) {
				this.rbCProfil.Checked = false;
				this.rbHolzstaffel.Checked = true;
				this.rbKassettendecke.Checked = false;
				//this.rbGlatt.Visible = true;
				//this.rbAkustik.Visible = true;
				//this.rbKassetten.Visible = false;
				this.grpBeplankung.Enabled = true;
				this.grpCeilingContruction.Visible = true;
			} else if (product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE) {
				this.rbCProfil.Checked = false;
				this.rbHolzstaffel.Checked = false;
				this.rbKassettendecke.Checked = true;
				//this.rbGlatt.Visible = false;
				//this.rbAkustik.Visible = false;
				//this.rbKassetten.Visible = true;
				this.grpBeplankung.Enabled = false;
				this.grpCeilingContruction.Visible = false;
			}

			if (product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
				this.rbGlatt.Checked = true;
				ignoreBeplankung++;
				if (this.glatt.Beplankung.HasValue) {
					this.numBeplankungLaenge.Value = (decimal)(this.glatt.Beplankung.Value.X * Product.ConfigPlanMeasureMultiplier);
					this.numBeplankungBreite.Value = (decimal)(this.glatt.Beplankung.Value.Y * Product.ConfigPlanMeasureMultiplier);
					this.cbBeplankung.Checked = true;
				} else {
					this.cbBeplankung.Checked = false;
					this.numBeplankungLaenge.Text = "";
					this.numBeplankungBreite.Text = "";
				}
				ignoreBeplankung--;
			} else if (product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
				this.rbAkustik.Checked = true;
				ignoreBeplankung++;
				if (this.akustik.Beplankung.HasValue) {
					this.numBeplankungLaenge.Value = (decimal)(this.akustik.Beplankung.Value.X * Product.ConfigPlanMeasureMultiplier);
					this.numBeplankungBreite.Value = (decimal)(this.akustik.Beplankung.Value.Y * Product.ConfigPlanMeasureMultiplier);
					this.cbBeplankung.Checked = true;
				} else {
					this.cbBeplankung.Checked = false;
					this.numBeplankungLaenge.Text = "";
					this.numBeplankungBreite.Text = "";
				}
				ignoreBeplankung--;
			} else if (product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
				//this.rbKassetten.Checked = true;
			}

			if (this.cbBeplankung.Checked) {
				this.numBeplankungBreite.Enabled = true;
				this.numBeplankungLaenge.Enabled = true;
				this.numBeplankungLaenge.ReadOnly = false;
				this.numBeplankungBreite.ReadOnly = false;
			} else {
				this.numBeplankungBreite.Enabled = false;
				this.numBeplankungLaenge.Enabled = false;
				this.numBeplankungLaenge.ReadOnly = true;
				this.numBeplankungBreite.ReadOnly = true;
			}

			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbOrientation.SelectedIndex = 0;
			this.cmbSelectedModuleOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			this.cmbSelectedModuleOrientation.Items.Add(KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
			this.cmbSelectedModuleOrientation.SelectedIndex = -1;
			this.cmbSelectedModuleOrientation.Enabled = false;

			this.cmbModulType.Items.Clear();
			this.cmbSelectedModuleType.Items.Clear();

			if (product.GraphConstruction is ModulKlimaDeckeConstructionGlatt || product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
				this.cmbModulType.SelectedIndex = 2;
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
				this.cmbSelectedModuleType.SelectedIndex = -1;
			} else if (product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
				if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_1050_450) {
					this.rb1050.Checked = true;
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
					this.cmbSelectedModuleType.SelectedIndex = -1;
				} else if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_625) {
					this.rb625.Checked = true;
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbSelectedModuleType.SelectedIndex = -1;
				} else if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_600) {
					this.rb600.Checked = true;
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbSelectedModuleType.SelectedIndex = -1;
				}
			}

			this.cmbSelectedModuleType.Enabled = false;
			this.lblTypError.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_KeinModul;

			this.UpdateLists(true, true, true, false);

			if (product.ContainsModules) {
				this.tabs.SelectedTab = this.pageLayout;
			} else {
				this.tabs.SelectedTab = this.pageConstruction;
			}
			updateOngoing = false;

            this.btnMoveBeplankung.Enabled = ((product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) && (product.GraphConstruction as ModulKlimaDeckeConstructionGlatt).Beplankung.HasValue);
			
			this.UpdateToolbar(this.tabs.SelectedTab);
			this.UpdateButtons();
			this.UpdateControls();
			this.CalculateAndUpdate();

			this.connectionPlanner.Product = product;
		}

		private void SetLanguage() {
			this.btnZoomIn.Text = Europlan.Common.EuroplanRes.Plan_Heranzoomen;
			this.btnZoomIn.ToolTipText = Europlan.Common.EuroplanRes.Plan_Heranzoomen;
			this.btnZoomOut.Text = Europlan.Common.EuroplanRes.Plan_Herauszoomen;
			this.btnZoomOut.ToolTipText = Europlan.Common.EuroplanRes.Plan_Herauszoomen;
			this.btnMove.Text = Europlan.Common.EuroplanRes.Plan_Verschieben;
			this.btnMove.ToolTipText = Europlan.Common.EuroplanRes.Plan_Verschieben;
			this.btnConstruction.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_UnterkonstruktionAusrichten;
			this.btnConstruction.ToolTipText = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_UnterkonstruktionAusrichten;
			this.btnAddModules.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_ModuleHinzufuegen;
			this.btnAddModules.ToolTipText = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_ModuleHinzufuegen;
			this.btnSelectModule.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_ModuleAuswaehlen;
			this.btnSelectModule.ToolTipText = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_ModuleAuswaehlen;
			this.btnAddConnections.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_VerbindeleitungHinzufuegen;
			this.btnDeleteConnection.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_VerbindeleitungLoeschen;
            this.btnShowBeplankung.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_BeplankungAnzeigen;
            this.btnMoveBeplankung.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_BeplankungVerschieben;
            this.btnShowPlanBg.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_PlanAnzeigen;
			this.btnAddAnbindeleitungen.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_AnbindeleitungenHinzufuegen;
			this.btnSelectAnbindeleitungen.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_AnbindeleitungenAendern;
			this.pageConstruction.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_KonstruktionEinrichten;
			this.grpUnterkonstruktion.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Unterkonstuktion;
			this.rbHolzstaffel.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckeProduct_Holzstaffel;
			this.rbCProfil.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckeProduct_CProfil;
			this.rbKassettendecke.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckeProduct_Kassettendecke;
			this.grpBeplankung.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Beplankung;
			this.lblBeplankungBreiteUnit.Text = Europlan.Common.EuroplanRes.Unit_Millimeter;
			this.lblBeplankungLaengeUnit.Text = Europlan.Common.EuroplanRes.Unit_Millimeter;
			this.lblBeplankungBreite.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Breite;
			this.lblBeplankungLanege.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Laenge;
			this.cbBeplankung.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_BeplankungBekannt;
			this.grpCeilingContruction.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Deckenkonstruktion;
			this.rbKassetten.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckeProduct_Kassettendecke;
			this.rbAkustik.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_AkustikdeckeRandfries;
			this.rbGlatt.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Glatt;
			this.grpConstructionParameter.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Konstruktionsparameter;
			this.lblRotation.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Ausrichtung;
			this.lblRandfriesUnit.Text = Europlan.Common.EuroplanRes.Unit_Zentimeter;
			this.lblRandfries.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Randfries;
			this.grpModulSerie.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_KlimamodulSerie;
			this.rbSerie40.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Serie40;
			this.rbSerie30.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Serie30;
			this.grpRasterMass.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Rastermass;
			this.pageLayout.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_ModuleAuslegen;
			this.label9.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.label18.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_DifferenzZurErwartetenLeistung;
			this.lblColor.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_FarbeHK;
			this.label14.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_ProWinkel1mVerbindungsleitung;
			this.label8.Text = Europlan.Common.EuroplanRes.Unit_Meter;
			this.label7.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_SummeVerbindungsleitungen;
			this.label13.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Teilflaechen;
			this.label1.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_ParalleleReihen;
			this.label12.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Heizkreise;
			this.grpSelectedModules.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_AusgewaehlteModule;
			this.btnInvertDirection.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_DurchstroemungsrichtungUmdrehen;
			this.label4.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Ausrichtung;
			this.label5.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Typ;
			this.grpSelection.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_AusgewaehltesModul;
			this.btnMoveRow.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Verschieben;
			this.btnMoveSubarea.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Verschieben;
			this.lblReihe.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Reihe;
			this.lblSubarea.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Teilflaeche;
			this.lblHk.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Heizkreis2;
			this.grpNewModules.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_NeueModule;
			this.lblNewModules.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_NeueModule2;
			this.label3.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Ausrichtung;
			this.label2.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Typ;
			this.grpAutomatic.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_AutomatischeAnpassungen;
			this.cbAutomaticRows.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_VorhandeneReihenNutzen;
			this.cbAutomaticOrientation.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_AutomatischAusrichten;
			this.pageCalculations.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Berechnungsergebnisse;
			this.lblQAnbCoolUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQAnbHeatUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.label45.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_LeistungAnbindeleitungen;
			this.lblQCoolRestUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQCoolDiffUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQCoolUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQHeatRestUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQHeatDiffUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.lblQHeatUnit.Text = Europlan.Common.EuroplanRes.Unit_Watt;
			this.label16.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_DifferenzZurErwartetenLeistung;
			this.label17.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_ErreichteLeistung;
			this.label11.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Kuehlbetrieb;
			this.label10.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Heizbetrieb;
			this.label6.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_Berechnungsergebnisse2;
			this.cbVlFlexible.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_VorlaufFlexibel;
			this.cbRlFlexible.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_RuecklaufFlexibel;
			this.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_Titel;
            this.grpSchienenBreite.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_StaffelnCProfil;
            this.lblSchienenBreite.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_StaffelBreite;
            this.lblSchienenBreiteUnit.Text = Europlan.Common.EuroplanRes.Unit_Zentimeter;
            this.btnShowPlanBg.Text = Europlan.Common.EuroplanRes.ProductPlannerForm_PlanImHintergrundAnzeigen;
        }

		private void UpdateControls() {
			updateOngoing = true;
			if (this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE) {
				rbKassettendecke.Checked = true;
				this.grpCeilingContruction.Visible = false;
				this.grpBeplankung.Enabled = false;
                this.grpSchienenBreite.Visible = false;
			} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL) {
				rbCProfil.Checked = true;
				this.grpCeilingContruction.Visible = true;
				this.grpBeplankung.Enabled = true;
                this.grpSchienenBreite.Visible = true;
            } else if (this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction == ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL) {
				rbHolzstaffel.Checked = true;
				this.grpCeilingContruction.Visible = true;
				this.grpBeplankung.Enabled = true;
                this.grpSchienenBreite.Visible = true;
            }
			if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
				rbAkustik.Checked = true;
			} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
				rbGlatt.Checked = true;
			} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
				//rbKassetten.Checked = true;
			}
			if (!this.rbKassettendecke.Checked && this.rbGlatt.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = false;
				this.numRandfries.Visible = false;
				this.lblRandfriesUnit.Visible = false;
				this.grpModulSerie.Visible = true;
				this.grpRasterMass.Visible = false;
				this.numRotation.Value = (decimal)this.glatt.RotationRelativeToPlan;
                this.numSchienenBreite.Value = (decimal)this.glatt.SchienenBreite * 100;
				if (Math.Round(this.glatt.SchienenAbstand, 2) == 0.3) {
					if (!this.rbSerie30.Checked) {
						this.rbSerie30.Checked = true;
					}
				} else {
					if (!this.rbSerie40.Checked) {
						this.rbSerie40.Checked = true;
					}
				}
				ignoreRotation--;
			} else if (!this.rbKassettendecke.Checked && this.rbAkustik.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = true;
				this.numRandfries.Visible = true;
				this.lblRandfriesUnit.Visible = true;
				this.grpModulSerie.Visible = true;
				this.grpRasterMass.Visible = false;
				this.numRotation.Value = (decimal)this.akustik.RotationRelativeToPlan;
                this.numSchienenBreite.Value = (decimal)this.akustik.SchienenBreite * 100;
				if (Math.Round(this.akustik.SchienenAbstand, 2) == 0.3) {
					if (!this.rbSerie30.Checked) {
						this.rbSerie30.Checked = true;
					}
				} else {
					if (!this.rbSerie40.Checked) {
						this.rbSerie40.Checked = true;
					}
				}
				ignoreRotation--;
			} else if (this.rbKassettendecke.Checked) {
				ignoreRotation++;
				this.lblRandfries.Visible = false;
				this.numRandfries.Visible = false;
				this.lblRandfriesUnit.Visible = false;
				this.grpModulSerie.Visible = false;
				this.grpRasterMass.Visible = true;
				this.numRotation.Value = (decimal)this.kassette.RotationRelativeToPlan;
				if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_1050_450) {
					this.rb1050.Checked = true;
				} else if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_625) {
					this.rb625.Checked = true;
				} else if (this.kassette.Raster == ModulKlimaDeckeConstructionKassette.RasterMass.Raster_600) {
					this.rb600.Checked = true;
				}	
				ignoreRotation--;
			} else {
			}
			this.cbRlFlexible.Visible = this.rbKassettendecke.Checked;
			this.cbVlFlexible.Visible = this.rbKassettendecke.Checked;
			updateOngoing = false;
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(0.9, null);
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(1.1, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnConstruction_Click(object sender, EventArgs e) {
			if (!btnConstruction.Checked) {
				this.SetProductPlanner();
                this.glatt.Mode = ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_SCHIENEN;
                this.akustik.Mode = ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_SCHIENEN;
                this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

        private void btnMoveBeplankung_Click(object sender, EventArgs e) {
            if (!btnMoveBeplankung.Checked) {
                this.SetProductPlanner();
                this.glatt.Mode = ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_BEPLANKUNG;
                this.akustik.Mode = ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_BEPLANKUNG;
                this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION;
                this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
                this.UpdateButtons();
            }
        }

		private void btnAddModules_Click(object sender, EventArgs e) {
			if (!btnAddModules.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void btnSelectModule_Click(object sender, EventArgs e) {
			if (!btnSelectModule.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE;
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.UpdateButtons();
			}
		}

		private void btnAddConnections_Click(object sender, EventArgs e) {
			if (!this.btnAddConnections.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_ADD_CONNECTION;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnDeleteConnection_Click(object sender, EventArgs e) {
			if (!this.btnDeleteConnection.Checked) {
				this.SetProductPlanner();
				this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_DEL_CONNECTION;
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnAddAnbindeleitungen_Click(object sender, EventArgs e) {
			if (!this.btnAddAnbindeleitungen.Checked) {
				this.SetConnectionPlanner();
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION;
				this.UpdateButtons();
			}
		}

		private void btnSelectAnbindeleitungen_Click(object sender, EventArgs e) {
			if (!this.btnSelectAnbindeleitungen.Checked) {
				this.SetConnectionPlanner();
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnConstruction.Checked = false;
                this.btnMoveBeplankung.Checked = false;
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_CONSTRUCTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = (this.glatt.Mode == ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_SCHIENEN);
                this.btnMoveBeplankung.Checked = (this.glatt.Mode == ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_BEPLANKUNG);
				this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
                this.btnMoveBeplankung.Checked = false;
                this.btnAddModules.Checked = true;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
                this.btnMoveBeplankung.Checked = false;
                this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = true;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
                this.btnMoveBeplankung.Checked = false;
                this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = true;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.modulKlimaDeckePlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_DEL_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
                this.btnMoveBeplankung.Checked = false;
                this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = true;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.connectionPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
                this.btnMoveBeplankung.Checked = false;
                this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = true;
				this.btnSelectAnbindeleitungen.Checked = false;
			} else if (this.planPanel.ProductPlanner == this.connectionPlanner && (this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
                this.btnMoveBeplankung.Checked = false;
                this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = true;
			} else {
				this.btnMove.Checked = false;
				this.btnConstruction.Checked = false;
                this.btnMoveBeplankung.Checked = false;
                this.btnAddModules.Checked = false;
				this.btnSelectModule.Checked = false;
				this.btnAddConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnAddAnbindeleitungen.Checked = false;
				this.btnSelectAnbindeleitungen.Checked = false;
			}
			this.grpSelectedModules.Visible = this.btnSelectModule.Checked;
			this.grpSelection.Visible = this.btnSelectModule.Checked;
			this.grpNewModules.Visible = this.btnAddModules.Checked;
			this.grpAutomatic.Visible = this.btnAddModules.Checked;
			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				if (!this.newVisible) {
					this.newVisible = true;
					this.ignoreListChange++;
					lstCircuits.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_NeuerHK);
					lstSubarea.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_NeueTeilflaeche);
					lstRows.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_NeueReihen);
					this.ignoreListChange--;
				}
			} else {
				if (this.newVisible) {
					this.newVisible = false;
					this.ignoreListChange++;
					this.lstCircuits.Items.RemoveAt(this.lstCircuits.Items.Count - 1);
					this.lstSubarea.Items.RemoveAt(this.lstSubarea.Items.Count - 1);
					this.lstRows.Items.RemoveAt(this.lstRows.Items.Count - 1);
					this.ignoreListChange--;
				}
			}
			this.UpdateLists(true, true, true, false);
		}

		private KlimaFlaechenList GetSelectedRow() {
			return this.modulKlimaDeckePlanner.HighlightRow;
		}

		private ModulDeckeCircuit GetSelectedCircuit() {
			return this.modulKlimaDeckePlanner.HighlightCircuit;
		}

		private void rbGlatt_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (rbGlatt.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction != this.glatt) {
					this.modulKlimaDeckePlanner.Product.GraphConstruction = this.glatt;
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void rbAkustik_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (rbAkustik.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction != this.akustik) {
					this.modulKlimaDeckePlanner.Product.GraphConstruction = this.akustik;
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		/*private void rbKassetten_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (rbKassetten.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction != this.kassette) {
					this.modulKlimaDeckePlanner.Product.GraphConstruction = this.kassette;
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}*/

		private void rbKassettendecke_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (this.rbKassettendecke.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction != ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.KASSETTENDECKE) {
                    this.btnMoveBeplankung.Enabled = false;
                    if (this.btnMoveBeplankung.Checked) {
                        this.btnMoveBeplankung.Checked = false;
                        this.btnConstruction.Checked = true;
                        this.glatt.Mode = ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_SCHIENEN;
                        this.akustik.Mode = ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_SCHIENEN;
                    }
					this.modulKlimaDeckePlanner.Product.GraphConstruction = this.kassette;
					this.UpdateModuleTypesInCombobox(this.kassette.Raster);
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void rbCProfil_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (this.rbCProfil.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction != ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL) {
                    this.btnMoveBeplankung.Enabled = false;
                    this.akustik.ContructionType = ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL;
					this.glatt.ContructionType = ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.C_PROFIL;
					if (this.rbAkustik.Checked) {
						this.modulKlimaDeckePlanner.Product.GraphConstruction = this.akustik;
                        this.btnMoveBeplankung.Enabled = this.akustik.Beplankung.HasValue;
					} else {
						this.modulKlimaDeckePlanner.Product.GraphConstruction = this.glatt;
                        this.btnMoveBeplankung.Enabled = this.glatt.Beplankung.HasValue;
					}
					this.modulKlimaDeckePlanner.Product.GraphConstruction.RecalculateSchienen();
					this.UpdateModuleTypesInCombobox(this.glatt.SchienenAbstand);
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void rbHolzstaffel_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (this.rbHolzstaffel.Checked && this.modulKlimaDeckePlanner.Product.GraphConstruction.CeilingConstruction != ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL) {
                    this.btnMoveBeplankung.Enabled = false;
                    this.akustik.ContructionType = ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL;
					this.glatt.ContructionType = ModulKlimaDeckeProduct.ModulCeilingConstructionEnum.HOLZSTAFFEL;
					if (this.rbAkustik.Checked) {
						this.modulKlimaDeckePlanner.Product.GraphConstruction = this.akustik;
                        this.btnMoveBeplankung.Enabled = this.akustik.Beplankung.HasValue;
                    } else {
						this.modulKlimaDeckePlanner.Product.GraphConstruction = this.glatt;
                        this.btnMoveBeplankung.Enabled = this.glatt.Beplankung.HasValue;
                    }
					this.modulKlimaDeckePlanner.Product.GraphConstruction.RecalculateSchienen();
					this.UpdateModuleTypesInCombobox(this.glatt.SchienenAbstand);
					this.changed = true;
					this.UpdateControls();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void numAusrichtung_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreRotation == 0) {
					this.changed = true;
					if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
						ModulKlimaDeckeConstructionAkustik akustik = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
						akustik.RotationRelativeToPlan = (double)this.numRotation.Value;
						this.planPanel.InvalidateGraphics();
					} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
						ModulKlimaDeckeConstructionGlatt glatt = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
						glatt.RotationRelativeToPlan = (double)this.numRotation.Value;
						this.planPanel.InvalidateGraphics();
					} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionKassette) {
						ModulKlimaDeckeConstructionKassette kassette = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionKassette;
						kassette.RotationRelativeToPlan = (double)this.numRotation.Value;
						this.planPanel.InvalidateGraphics();
					}
				}
			}
		}

		private decimal smallRotate = (decimal)0.5;
		private decimal largeRotate = 5;

		private void btnRotate_Click(object sender, EventArgs e) {
			this.changed = true;
			decimal rotation = 0;
			if (sender == this.btnCcwLarge) {
				rotation = -largeRotate;
			} else if (sender == this.btnCcwSmall) {
				rotation = -smallRotate;
			} else if (sender == btnCwLarge) {
				rotation = largeRotate;
			} else if (sender == btnCwSmall) {
				rotation = smallRotate;
			}
			decimal value = this.numRotation.Value + rotation;
			while (value < 0) {
				value += 360;
			}
			while (value >= 360) {
				value -= 360;
			}
			this.numRotation.Value = value;
		}

		private void btnHorizontal_Click(object sender, EventArgs e) {
			this.changed = true;
			this.numRotation.Value = 90;
		}

		private void btnVertical_Click(object sender, EventArgs e) {
			this.changed = true;
			this.numRotation.Value = 0;
		}

		private TabPage previousTab = null;

		private void tabs_Selecting(object sender, TabControlCancelEventArgs e) {
			if ((previousTab == this.pageLayout || previousTab == this.pageCalculations) && e.TabPage == this.pageConstruction) {
				if (this.modulKlimaDeckePlanner.Product.ContainsModules) {
					if (MessageBox.Show(Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_KonstruktionAendernText, Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_KonstruktionAendernTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
						e.Cancel = true;
					} else {
						this.modulKlimaDeckePlanner.Product.PlannedCircuits.Clear();
						this.modulKlimaDeckePlanner.Product.PlannedCircuits.Add(new ModulDeckeCircuit());
					}
				}
			}
			if (!e.Cancel) {
				this.UpdateToolbar(e.TabPage);
				this.planPanel.InvalidateGraphics();
			}
		}

		private void UpdateToolbar(TabPage tabPage) {
			if (tabPage == this.pageLayout) {
				this.btnConstruction.Visible = false;
                this.btnMoveBeplankung.Visible = false;
				this.btnAddModules.Visible = true;
				this.btnSelectModule.Visible = true;
				this.btnAddConnections.Visible = true;
				this.btnDeleteConnection.Visible = true;
				this.btnAddAnbindeleitungen.Visible = true;
				this.btnSelectAnbindeleitungen.Visible = true;
				this.sepAnbindeleitungen.Visible = true;
				if (!this.btnAddModules.Checked && !this.btnSelectModule.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
					this.UpdateButtons();
				}
			} else if (tabPage == this.pageConstruction) {
				this.btnAddModules.Visible = false;
				this.btnSelectModule.Visible = false;
				this.btnConstruction.Visible = true;
                this.btnMoveBeplankung.Visible = true;
                this.btnAddConnections.Visible = false;
				this.btnDeleteConnection.Visible = false;
				this.btnAddAnbindeleitungen.Visible = false;
				this.btnSelectAnbindeleitungen.Visible = false;
				this.sepAnbindeleitungen.Visible = false;
				if (!this.btnConstruction.Checked && !this.btnMove.Checked) {
					this.planPanel.Mode = PlanMode.PM_MOVE;
					this.modulKlimaDeckePlanner.Mode = ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
					this.UpdateButtons();
				}
			}
		}

		private void tabs_Deselected(object sender, TabControlEventArgs e) {
			this.previousTab = e.TabPage;
		}

		private void modulKlimaBodenPlanner_ListsNeedUpdate(object sender, ModulKlimaDeckePlanner.ListNeedsUpdateEventArgs e) {
			this.UpdateLists(true, true, true, e.selectLastCircuit);
		}

		private void UpdateLists(bool updateCircuits, bool updateSubareas, bool updateRows, bool selectLastCircuit) {
			List<Circuit> circuits = (this.modulKlimaDeckePlanner.Product.ContainsModules ? this.modulKlimaDeckePlanner.Product.PlannedCircuits : new List<Circuit>());

			int dec = this.newVisible ? 1 : 0;
			this.newVisible = this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_LAYOUT_ADD_AREA && this.modulKlimaDeckePlanner.Product.PlannedCircuits.Count < 12;
			ignoreListChange++;

			if (updateCircuits) {
				int circuitsCount = circuits.Count;
				int tmp = (lstCircuits.SelectedIndex == lstCircuits.Items.Count - dec ? circuitsCount : lstCircuits.SelectedIndex);
				lstCircuits.BeginUpdate();
				lstCircuits.Items.Clear();
				for (int i = 1; i <= circuitsCount; i++) {
					lstCircuits.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_HeizkreisAbkuerzung + i.ToString());
				}
				if (this.newVisible) {
					lstCircuits.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_NeuerHK);
				}
				if (selectLastCircuit) {
					if (this.newVisible && lstCircuits.Items.Count > 1) {
						lstCircuits.SelectedIndex = lstCircuits.Items.Count - 2;
					} else {
						lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
					}
				} else {
					if (tmp < lstCircuits.Items.Count && tmp >= 0) {
						lstCircuits.SelectedIndex = tmp;
					} else {
						if (this.newVisible) {
							if (tmp < 0 && lstCircuits.Items.Count > 1) {
								lstCircuits.SelectedIndex = lstCircuits.Items.Count - 2;
							} else {
								lstCircuits.SelectedIndex = lstCircuits.Items.Count - 1;
							}
						} else {
							lstCircuits.SelectedIndex = -1;
						}
					}
				}
				lstCircuits.EndUpdate();
			}

			if (updateSubareas) {
				int subAreasCount = (this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec && this.lstCircuits.SelectedIndex >= 0 ? (circuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas.Count : 0);
				int tmp = (lstSubarea.SelectedIndex == lstSubarea.Items.Count - dec ? subAreasCount : lstSubarea.SelectedIndex);
				lstSubarea.BeginUpdate();
				lstSubarea.Items.Clear();
				for (int i = 1; i <= subAreasCount; i++) {
					lstSubarea.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Teilflaeche + " " + i.ToString());
				}
				if (this.newVisible) {
					lstSubarea.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_NeueTeilflaeche);
				}
				if (tmp < lstSubarea.Items.Count) {
					if (this.newVisible && tmp < 0 && lstCircuits.SelectedIndex < lstCircuits.Items.Count - dec && lstCircuits.SelectedIndex >= 0) {
						tmp = lstSubarea.Items.Count - 1;
					}
					lstSubarea.SelectedIndex = tmp;
				} else {
					lstSubarea.SelectedIndex = -1;
				}
				lstSubarea.EndUpdate();
			}

			if (updateRows) {
				int rowsCount = (this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec && this.lstSubarea.SelectedIndex >= 0 ? (circuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex].Rows.Count : 0);
				int tmp = (lstRows.SelectedIndex == lstRows.Items.Count - dec ? rowsCount : lstRows.SelectedIndex);
				lstRows.BeginUpdate();
				lstRows.Items.Clear();
				for (int i = 1; i <= rowsCount; i++) {
					lstRows.Items.Add(EuroplanRes.PlannedModulKlimaDeckeProductPanel_Reihe + i.ToString());
				}
				if (this.newVisible) {
					lstRows.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_NeueReihen);
				}
				if (tmp < lstRows.Items.Count) {
					if (this.newVisible && tmp < 0 && lstSubarea.SelectedIndex < lstSubarea.Items.Count - dec && lstSubarea.SelectedIndex >= 0) {
						tmp = lstRows.Items.Count - 1;
					}
					lstRows.SelectedIndex = tmp;
				} else {
					lstRows.SelectedIndex = -1;
				}
				lstRows.EndUpdate();
			}

			ignoreListChange--;
			this.lstCircuits_SelectedIndexChanged(this.lstRows, EventArgs.Empty);
		}

		private int ignoreListChange = 0;
		private void lstCircuits_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.ignoreListChange == 0) {
				this.ignoreListChange++;
				int dec = this.newVisible ? 1 : 0;
				if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
					if (this.lstSubarea.SelectedIndex < 0 || sender == this.lstCircuits) {
						if (this.newVisible) {
							this.lstSubarea.SelectedIndex = this.lstSubarea.Items.Count - 1;
						} else {
							this.lstSubarea.SelectedIndex = -1;
						}
					}
				} else {
					this.lstSubarea.SelectedIndex = -1;
				}

				if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec) {
					if (this.lstRows.SelectedIndex < 0 || sender == this.lstSubarea) {
						if (this.newVisible) {
							this.lstRows.SelectedIndex = this.lstRows.Items.Count - 1;
						} else {
							this.lstRows.SelectedIndex = -1;
						}
					}
				} else {
					this.lstRows.SelectedIndex = -1;
				}

				if (sender != lstRows) {
					this.UpdateLists(false, sender == lstCircuits, true, false);
				}

				if (this.lstRows.SelectedIndex >= 0 && this.lstRows.SelectedIndex < this.lstRows.Items.Count - dec) {
					this.modulKlimaDeckePlanner.HighlightRow = (this.lstRows.Items.Count - dec > this.lstRows.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex].Rows[this.lstRows.SelectedIndex] : null);
					/*List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightRow != null) {
						foreach (KlimaFlaechenModul modul in this.modulKlimaBodenPlanner.HighlightRow.List) {
							modules.Add(modul);
						}
					}*/
					this.UpdateSelectedModules();
				} else if (this.lstSubarea.SelectedIndex >= 0 && this.lstSubarea.SelectedIndex < this.lstSubarea.Items.Count - dec) {
					this.modulKlimaDeckePlanner.HighlightSubArea = (this.lstSubarea.Items.Count - dec > this.lstSubarea.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit).SubAreas[this.lstSubarea.SelectedIndex] : null);
					/*List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightSubArea != null) {
						foreach (KlimaFlaechenList row in this.modulKlimaBodenPlanner.HighlightSubArea.Rows) {
							foreach (KlimaFlaechenModul modul in row.List) {
								modules.Add(modul);
							}
						}
					}*/
					this.UpdateSelectedModules();
				} else if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
					this.modulKlimaDeckePlanner.HighlightCircuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit) : null);
					/*List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
					if (this.modulKlimaBodenPlanner.HighlightCircuit != null) {
						foreach (ModulDeckeSubArea subArea in this.modulKlimaBodenPlanner.HighlightCircuit.SubAreas) {
							foreach (KlimaFlaechenList row in subArea.Rows) {
								foreach (KlimaFlaechenModul modul in row.List) {
									modules.Add(modul);
								}
							}
						}
					}*/
					this.UpdateSelectedModules();
				} else {
					this.modulKlimaDeckePlanner.HighlightCircuit = null;
					this.UpdateSelectedModules();
				}

				numLength.Enabled = GetSelectedRow() != null && GetSelectedRow().List.Count > 0;

				if (GetSelectedRow() != null) {
					this.numLength.Value = (decimal)GetSelectedRow().LengthVerbindeleitungen;
				}

				if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
					ModulDeckeCircuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit) : null);
					if (circuit != null) {
						btnColor.BackColor = circuit.CircuitColor;
						btnColor.Enabled = true;
					} else {
						btnColor.BackColor = Color.Transparent;
						btnColor.Enabled = false;
					}
				} else {
					btnColor.BackColor = Color.Transparent;
					btnColor.Enabled = false;
				}

				this.ignoreListChange--;
			}
		}

		private void cmbModulType_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.cmbModulType.SelectedItem is KlimaFlaechenModul.ModulTypeEnum) {
				this.modulKlimaDeckePlanner.ModuleTypeToAdd = (KlimaFlaechenModul.ModulTypeEnum)this.cmbModulType.SelectedItem;
			}
			if (this.cmbOrientation.SelectedItem is KlimaFlaechenModul.ModulOrientationEnum) {
				this.modulKlimaDeckePlanner.StartingOrientation = (KlimaFlaechenModul.ModulOrientationEnum)this.cmbOrientation.SelectedItem;
			}
		}

		private void modulKlimaBodenPlanner_ModuleSelected(object sender, ModulKlimaDeckePlanner.ModuleSelectedEventArgs e) {
			this.UpdateSelectedModules();
		}

		private ModulDeckeSubArea subAreaToMove = null;
		private KlimaFlaechenList rowToMove = null;

		private void UpdateSelectedModules() {
			this.ignoreModuleOrientationChange++;
			this.ignoreModuleTypeChange++;
			this.ignoreModuleFlexibleChange++;

			if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.modulKlimaDeckePlanner.Mode == ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_PICK_MODULE) {
				if (this.modulKlimaDeckePlanner.HighlightCircuit == null && this.modulKlimaDeckePlanner.HighlightSubArea == null && this.modulKlimaDeckePlanner.HighlightRow == null) {
					this.lstCircuits.SelectedIndex = -1;
				}
			}
			List<KlimaFlaechenModul> module = this.modulKlimaDeckePlanner.GetAllSelectedModules();

			this.btnInvertDirection.Enabled = module != null && module.Count > 0;

			if (module != null && module.Count > 0) {
				Nullable<KlimaFlaechenModul.ModulTypeEnum> typ = null;
				bool typOk = true;
				Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation = null;
				bool orientationOk = true;
				foreach (KlimaFlaechenModul modul in module) {
					if (!typ.HasValue) {
						typ = modul.ModulType;
					}
					if (!orientation.HasValue) {
						orientation = modul.Orientation;
					}
					if (typ != modul.ModulType) {
						typOk = false;
					}
					if (orientation != modul.Orientation) {
						orientationOk = false;
					}
					if (!typOk && !orientationOk) {
						break;
					}
				}
				if (typOk && typ.HasValue) {
					this.cmbSelectedModuleType.SelectedItem = typ.Value;
				} else {
					this.cmbSelectedModuleType.SelectedIndex = -1;
				}
				if (orientationOk && orientation.HasValue) {
					this.cmbSelectedModuleOrientation.SelectedItem = orientation.Value;
				} else {
					this.cmbSelectedModuleOrientation.SelectedIndex = -1;
				}

				if (typ.HasValue) {
					if (typOk) {
						this.lblTypError.Text = "";
					} else {
						this.lblTypError.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_VerschiedeneModulgroessen;
					}
					if (orientationOk) {
						this.lblOrientationError.Text = "";
						if (this.cmbSelectedModuleOrientation.Items.Count == 3) {
							this.cmbSelectedModuleOrientation.Items.RemoveAt(2);
						}
					} else {
						this.lblOrientationError.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_VerschiedeneAusrichtungen;
						if (this.cmbSelectedModuleOrientation.Items.Count == 2) {
							this.cmbSelectedModuleOrientation.Items.Add(Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_AlleUmdrehen);
						}
					}
					this.cmbSelectedModuleType.Enabled = true;
					this.cmbSelectedModuleOrientation.Enabled = true;
				} else {
					this.lblTypError.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_KeinModul;
					this.cmbSelectedModuleType.Enabled = false;
					this.cmbSelectedModuleOrientation.Enabled = false;
				}

				{ // update flexible checkboxes
					bool vlFlexible = false;
					bool vlNonFlexible = false;
					bool rlFlexible = false;
					bool rlNonFlexible = false;
					KlimaFlaechenModulVerbindung link;
					KlimaFlaechenSubAreaVerbindung saLink;
					int tmp;
					bool invertYAxis = this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.InvertYAxis;
					ModulDeckeCircuit c;
					foreach (KlimaFlaechenModul modul in module) {
						c = this.modulKlimaDeckePlanner.Product.GetCircuitForModul(modul, out tmp);
						link = modul.GetInputLink(c, invertYAxis);
						if (link != null) {
							if (link.IsFlexible) {
								vlFlexible = true;
							} else {
								vlNonFlexible = true;
							}
						} else {
							saLink = modul.GetSubareaInputLink(c, invertYAxis);
							if (saLink != null) {
								if (saLink.isEndFlexible(modul)) {
									vlFlexible = true;
								} else {
									vlNonFlexible = true;
								}
							}
						}
						link = modul.GetOutputLink(c, invertYAxis);
						if (link != null) {
							if (link.IsFlexible) {
								rlFlexible = true;
							} else {
								rlNonFlexible = true;
							}
						} else {
							saLink = modul.GetSubareaOutputLink(c, invertYAxis);
							if (saLink != null) {
								if (saLink.isStartFlexible(modul)) {
									rlFlexible = true;
								} else {
									rlNonFlexible = true;
								}
							}
						}
					}
					if (!vlFlexible && !vlNonFlexible) {
						this.cbVlFlexible.Enabled = false;
						this.cbVlFlexible.CheckState = CheckState.Indeterminate;
					} else {
						this.cbVlFlexible.Enabled = true;
						if (vlFlexible && vlNonFlexible) {
							this.cbVlFlexible.CheckState = CheckState.Indeterminate;
						} else if (vlFlexible) {
							this.cbVlFlexible.CheckState = CheckState.Checked;
						} else {
							this.cbVlFlexible.CheckState = CheckState.Unchecked;
						}
					}
					if (!rlFlexible && !rlNonFlexible) {
						this.cbRlFlexible.Enabled = false;
						this.cbRlFlexible.CheckState = CheckState.Indeterminate;
					} else {
						this.cbRlFlexible.Enabled = true;
						if (rlFlexible && rlNonFlexible) {
							this.cbRlFlexible.CheckState = CheckState.Indeterminate;
						} else if (rlFlexible) {
							this.cbRlFlexible.CheckState = CheckState.Checked;
						} else {
							this.cbRlFlexible.CheckState = CheckState.Unchecked;
						}
					}
				}

			} else {
				this.lblTypError.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_KeinModul;
				this.cmbSelectedModuleType.SelectedIndex = -1;
				this.cmbSelectedModuleOrientation.SelectedIndex = -1;
				this.cmbSelectedModuleType.Enabled = false;
				this.cmbSelectedModuleOrientation.Enabled = false;
				this.cbVlFlexible.Enabled = false;
				this.cbRlFlexible.Enabled = false;
				this.cbVlFlexible.CheckState = CheckState.Indeterminate;
				this.cbRlFlexible.CheckState = CheckState.Indeterminate;
			}
			this.ignoreModuleOrientationChange--;
			this.ignoreModuleTypeChange--;
			this.ignoreModuleFlexibleChange--;

			//List<KlimaFlaechenModul> module = this.modulKlimaBodenPlanner.GetAllSelectedModules();
			if (module.Count == 0) {
				this.llHk.Enabled = false;
				this.llHk.Tag = null;
				this.llHk.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_KeinHK;
				this.llSubarea.Enabled = false;
				this.llSubarea.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_KeineTeilflaeche;
				this.llSubarea.Tag = null;
				this.llReihe.Enabled = false;
				this.llReihe.Text = Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_KeineReihe;
				this.llReihe.Tag = null;
			} else {
				ModulDeckeCircuit circuit = null;
				bool circuitOk = true;
				ModulDeckeSubArea subArea = null;
				bool subAreaOk = true;
				KlimaFlaechenList row = null;
				bool rowOk = true;
				int circuitIndex = 0;
				int subAreaIndex = 0;
				int rowIndex = 0;
				foreach (KlimaFlaechenModul modul in module) {
					ModulDeckeCircuit curCircuit = this.modulKlimaDeckePlanner.Product.GetCircuitForModul(modul, out circuitIndex);
					if (circuit != null && circuit != curCircuit) {
						circuitOk = false;
						subAreaOk = false;
						rowOk = false;
					}
					if (curCircuit == null) {
						circuitOk = false;
						subAreaOk = false;
						rowOk = false;
					} else {
						ModulDeckeSubArea curSubArea = curCircuit.GetSubareaForModul(modul, out subAreaIndex);
						if (subArea != null && subArea != curSubArea) {
							subAreaOk = false;
							rowOk = false;
						}
						if (curSubArea == null) {
							subAreaOk = false;
							rowOk = false;
						} else {
							KlimaFlaechenList curRow = curSubArea.GetRowForModul(modul, out rowIndex);
							if (row != null && row != curRow) {
								rowOk = false;
							}
							if (curRow == null) {
								rowOk = false;
							} else {
								row = curRow;
							}
							subArea = curSubArea;
						}
						circuit = curCircuit;
					}
				}
				if (circuitOk) {
					this.llHk.Tag = circuitIndex;
					circuitIndex++;
					this.llHk.Text = Europlan.Common.EuroplanRes.PlannedModulKlimaDeckeProductPanel_HeizkreisAbkuerzung + circuitIndex.ToString();
					this.llHk.Enabled = true;
				} else {
					this.llHk.Tag = null;
					this.llHk.Enabled = false;
					this.llHk.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_VerschiendeHK;
				}
				if (subAreaOk) {
					this.subAreaToMove = subArea;
					this.llSubarea.Tag = subAreaIndex;
					subAreaIndex++;
					this.llSubarea.Text = Europlan.Common.EuroplanRes.PlannedModulKlimaDeckeProductPanel_Teilflaeche + " " + subAreaIndex.ToString();
					this.llSubarea.Enabled = true;
				} else {
					this.subAreaToMove = null;
					this.llSubarea.Tag = null;
					this.llSubarea.Enabled = false;
					this.llSubarea.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_VerschiendeHK;
				}
				if (rowOk) {
					this.rowToMove = row;
					this.llReihe.Tag = rowIndex;
					rowIndex++;
					this.llReihe.Text = Europlan.Common.EuroplanRes.PlannedModulKlimaDeckeProductPanel_Reihe + " " + rowIndex.ToString();
					this.llReihe.Enabled = true;
				} else {
					this.rowToMove = null;
					this.llReihe.Tag = null;
					this.llReihe.Enabled = false;
					this.llReihe.Text = Europlan.Common.EuroplanRes.ModulKlimaBodenPlannerForm_VerschiendeHK;
				}
			}
			btnMoveSubarea.Enabled = this.subAreaToMove != null;
			btnMoveRow.Enabled = this.rowToMove != null;
		}

		private int ignoreModuleTypeChange = 0;
		private int ignoreModuleOrientationChange = 0;
		private int ignoreModuleFlexibleChange = 0;

		private void cmbSelectedModuleType_SelectedIndexChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreModuleTypeChange == 0) {
					this.changed = true;
					if (this.modulKlimaDeckePlanner.Product == null || this.modulKlimaDeckePlanner.Product.AssociatedRoom == null ||
						this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan == null || this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.Measure == null) {
						return;
					}
					List<KlimaFlaechenModul> modules = this.modulKlimaDeckePlanner.GetAllSelectedModules();
					bool allChanged = true;
					bool nonChanged = true;
					if (this.cmbSelectedModuleType.SelectedItem is KlimaFlaechenModul.ModulTypeEnum) {
						foreach (KlimaFlaechenModul modul in modules) {
							PossibleModulLane lane = this.modulKlimaDeckePlanner.Product.GraphConstruction.PossibleLanes[modul.GraphLane];
							if (lane.ModuleChangesPossible(modul, (KlimaFlaechenModul.ModulTypeEnum)this.cmbSelectedModuleType.SelectedItem, modul.GraphPositionInLan, this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.Measure.Value, this.modulKlimaDeckePlanner.Product)) {
								modul.ModulType = (KlimaFlaechenModul.ModulTypeEnum)this.cmbSelectedModuleType.SelectedItem;
								nonChanged = false;
							} else {
								allChanged = false;
							}
						}
					}
					this.UpdateSelectedModules();
					this.planPanel.InvalidateGraphics();
					if (nonChanged) {
						MessageBox.Show(Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_NichtGenugPlatzText, Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_NichtGenugPlatzTitel, MessageBoxButtons.OK, MessageBoxIcon.Information);
					} else if (!allChanged) {
						MessageBox.Show(Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_NichtGenugPlatz2Text, Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_NichtGenugPlatz2Titel, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					this.CalculateAndUpdate();
				}
			}
		}

		private void cmbSelectedModuleOrientation_SelectedIndexChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreModuleOrientationChange == 0) {
					List<KlimaFlaechenModulVerbindung> linksToUpdate = new List<KlimaFlaechenModulVerbindung>();
					Dictionary<KlimaFlaechenModulVerbindung, KlimaFlaechenList> linksToDelete = new Dictionary<KlimaFlaechenModulVerbindung, KlimaFlaechenList>();
					Dictionary<KlimaFlaechenSubAreaVerbindung, ModulDeckeCircuit> saLinksToDelete = new Dictionary<KlimaFlaechenSubAreaVerbindung, ModulDeckeCircuit>();
					this.changed = true;
					int tmp;
					List<KlimaFlaechenModul> modules = this.modulKlimaDeckePlanner.GetAllSelectedModules();
					ModulDeckeCircuit c;
					bool invertYAxis = this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.InvertYAxis;
					double measure = this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
					KlimaFlaechenModul next, prev;
					KlimaFlaechenModulVerbindung link;
					KlimaFlaechenSubAreaVerbindung saLink;
					ModulDeckeSubArea sa;
					KlimaFlaechenList row;
					if (cmbSelectedModuleOrientation.SelectedIndex == 2) {
						foreach (KlimaFlaechenModul modul in modules) {
							modul.Orientation = (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
							c = this.modulKlimaDeckePlanner.Product.GetCircuitForModul(modul, out tmp);
							link = modul.GetOutputLink(c, invertYAxis);
							next = (link == null ? null : link.End);
							if (next != null) {
								if (modules.Contains(next) && link.Vertices.Count == 2) {
									if (!linksToUpdate.Contains(link)) {
										linksToUpdate.Add(link);
									}
								} else if (!linksToDelete.ContainsKey(link)) {
									sa = c.GetSubareaForModul(modul, out tmp);
									row = sa.GetRowForModul(modul, out tmp);
									linksToDelete.Add(link, row);
								}
							}
							link = modul.GetInputLink(c, invertYAxis);
							prev = (link == null ? null : link.Start);
							if (prev != null) {
								if (modules.Contains(prev) && link.Vertices.Count == 2) {
									if (!linksToUpdate.Contains(link)) {
										linksToUpdate.Add(link);
									}
								} else if (!linksToDelete.ContainsKey(link)) {
									sa = c.GetSubareaForModul(modul, out tmp);
									row = sa.GetRowForModul(modul, out tmp);
									linksToDelete.Add(link, row);
								}
							}
							saLink = modul.GetSubareaOutputLink(c, invertYAxis);
							if (saLink != null && !saLinksToDelete.ContainsKey(saLink)) {
								saLinksToDelete.Add(saLink, c);
							}
							saLink = modul.GetSubareaInputLink(c, invertYAxis);
							if (saLink != null && !saLinksToDelete.ContainsKey(saLink)) {
								saLinksToDelete.Add(saLink, c);
							}
						}
					} else if (this.cmbSelectedModuleOrientation.SelectedItem is KlimaFlaechenModul.ModulOrientationEnum) {
						foreach (KlimaFlaechenModul modul in modules) {
							bool orientationChanged = (modul.Orientation != (KlimaFlaechenModul.ModulOrientationEnum)this.cmbSelectedModuleOrientation.SelectedItem);
							modul.Orientation = (KlimaFlaechenModul.ModulOrientationEnum)this.cmbSelectedModuleOrientation.SelectedItem;
							if (orientationChanged) {
								c = this.modulKlimaDeckePlanner.Product.GetCircuitForModul(modul, out tmp);
								link = modul.GetOutputLink(c, invertYAxis);
								if (link != null && !linksToDelete.ContainsKey(link)) {
									sa = c.GetSubareaForModul(modul, out tmp);
									row = sa.GetRowForModul(modul, out tmp);
									linksToDelete.Add(link, row);
								}
								link = modul.GetInputLink(c, invertYAxis);
								if (link != null && !linksToDelete.ContainsKey(link)) {
									sa = c.GetSubareaForModul(modul, out tmp);
									row = sa.GetRowForModul(modul, out tmp);
									linksToDelete.Add(link, row);
								}
								saLink = modul.GetSubareaOutputLink(c, invertYAxis);
								if (saLink != null && !saLinksToDelete.ContainsKey(saLink)) {
									saLinksToDelete.Add(saLink, c);
								}
								saLink = modul.GetSubareaInputLink(c, invertYAxis);
								if (saLink != null && !saLinksToDelete.ContainsKey(saLink)) {
									saLinksToDelete.Add(saLink, c);
								}
							}
						}
					}
					foreach (KeyValuePair<KlimaFlaechenModulVerbindung, KlimaFlaechenList> linkToDelete in linksToDelete) {
						linkToDelete.Value.Links.Remove(linkToDelete.Key);
					}
					foreach (KlimaFlaechenModulVerbindung linkToUpdate in linksToUpdate) {
						linkToUpdate.Vertices[0] = linkToUpdate.Start.GetOutputConnection(measure, invertYAxis, this.modulKlimaDeckePlanner.Product);
						linkToUpdate.Vertices[1] = linkToUpdate.End.GetInputConnection(measure, invertYAxis, this.modulKlimaDeckePlanner.Product);
					}
					foreach (KeyValuePair<KlimaFlaechenSubAreaVerbindung, ModulDeckeCircuit> saLinkToDelete in saLinksToDelete) {
						saLinkToDelete.Value.Links.Remove(saLinkToDelete.Key);
					}
					this.UpdateSelectedModules();
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private void ModulKlimaDeckePlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ModulKlimaDeckePlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void ModulKlimaDeckePlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			this.connectionPlanner.ReGenerateConnectionPipes();

			SettingsKey settings = SettingsFile.Settings["ModulKlimaDeckePlannerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void rbSerie_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (sender is RadioButton && (sender as RadioButton).Checked) {

					this.changed = true;
					this.cmbModulType.Items.Clear();
					this.cmbSelectedModuleType.Items.Clear();

					if (sender == this.rbSerie30) {
						this.glatt.SchienenAbstand = 0.3;
						this.akustik.SchienenAbstand = 0.3;
					} else if (sender == this.rbSerie40) {
						this.glatt.SchienenAbstand = 0.4;
						this.akustik.SchienenAbstand = 0.4;
					}
					UpdateModuleTypesInCombobox(this.glatt.SchienenAbstand);
				}
				this.planPanel.InvalidateGraphics();
			}
		}

		private void rbRasterMass_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (sender is RadioButton && (sender as RadioButton).Checked) {
					this.changed = true;
					this.cmbModulType.Items.Clear();
					this.cmbSelectedModuleType.Items.Clear();

					if (sender == this.rb1050) {
						this.kassette.Raster = ModulKlimaDeckeConstructionKassette.RasterMass.Raster_1050_450;
					} else if (sender == this.rb625) {
						this.kassette.Raster = ModulKlimaDeckeConstructionKassette.RasterMass.Raster_625;
					} else if (sender == this.rb600) {
						this.kassette.Raster = ModulKlimaDeckeConstructionKassette.RasterMass.Raster_600;
					}
					UpdateModuleTypesInCombobox(this.kassette.Raster);
				}
				this.planPanel.InvalidateGraphics();
			}
		}

		private void UpdateModuleTypesInCombobox(double schienenAbstand) {
			this.cmbModulType.Items.Clear();
			this.cmbSelectedModuleType.Items.Clear();
			if (Math.Round(schienenAbstand, 2) == 0.3) {
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
				this.cmbModulType.SelectedIndex = 2;
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30);
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30);
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30);
				this.cmbSelectedModuleType.SelectedIndex = -1;
			} else {
				this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
				this.cmbModulType.SelectedIndex = 0;
				this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
				this.cmbSelectedModuleType.SelectedIndex = -1;
			}
		}

		private void UpdateModuleTypesInCombobox(ModulKlimaDeckeConstructionKassette.RasterMass rasterMass) {
			this.cmbModulType.Items.Clear();
			this.cmbSelectedModuleType.Items.Clear();
			switch (rasterMass) {
				case ModulKlimaDeckeConstructionKassette.RasterMass.Raster_1050_450:
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40);
					this.cmbSelectedModuleType.SelectedIndex = -1;
					break;

				case ModulKlimaDeckeConstructionKassette.RasterMass.Raster_625:
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbSelectedModuleType.SelectedIndex = -1;
					break;

				case ModulKlimaDeckeConstructionKassette.RasterMass.Raster_600:
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbModulType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbModulType.SelectedIndex = 0;
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C);
					this.cmbSelectedModuleType.Items.Add(KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D);
					this.cmbSelectedModuleType.SelectedIndex = -1;
					break;
			}
		}

		private void cbAutomaticOrientation_CheckedChanged(object sender, EventArgs e) {
			this.modulKlimaDeckePlanner.AutomaticOrientation = this.cbAutomaticOrientation.Checked;
		}

		private void cbAutomaticRows_CheckedChanged(object sender, EventArgs e) {
			this.modulKlimaDeckePlanner.AutomaticRows = this.cbAutomaticRows.Checked;
		}

		private void llHk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			object hkTag = llHk.Tag;

			if (hkTag != null && this.lstCircuits.Items.Count > (int)hkTag) {
				this.lstCircuits.SelectedIndex = -1;
				this.lstCircuits.SelectedIndex = (int)hkTag;
			}
		}

		private void llSubarea_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			object hkTag = llHk.Tag;
			object saTag = llSubarea.Tag;

			if (hkTag != null && this.lstCircuits.Items.Count > (int)hkTag) {
				this.lstCircuits.SelectedIndex = -1;
				this.lstCircuits.SelectedIndex = (int)hkTag;
				if (saTag != null && this.lstSubarea.Items.Count > (int)saTag) {
					this.lstSubarea.SelectedIndex = (int)saTag;
				}
			}
		}

		private void llReihe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			object hkTag = llHk.Tag;
			object saTag = llSubarea.Tag;
			object rTag = llReihe.Tag;

			if (hkTag != null && this.lstCircuits.Items.Count > (int)hkTag) {
				this.lstCircuits.SelectedIndex = -1;
				this.lstCircuits.SelectedIndex = (int)hkTag;
				if (saTag != null && this.lstSubarea.Items.Count > (int)saTag) {
					this.lstSubarea.SelectedIndex = (int)saTag;
					if (rTag != null && this.lstRows.Items.Count > (int)rTag) {
						this.lstRows.SelectedIndex = (int)rTag;
					}
				}
			}
		}

		private void CalculateAndUpdate() {
			this.plannedProduct.Product.ConfigureProduct(this.plannedProduct.RequestedHeatLoad, this.plannedProduct.RequestedCoolLoad, this.plannedProduct.CalculateHeat, this.plannedProduct.CalculateCool, false);
			string errorMsg = this.plannedProduct.Product.LastErrorMessage;
			this.lstError.Items.Clear();
			string[] messages;
			if (errorMsg != null) {
				messages = errorMsg.Split('\n');
				foreach (string message in messages) {
					if (!string.IsNullOrEmpty(message)) {
						ListViewItem item = new ListViewItem(message);
						item.ForeColor = Color.Red;
						//item.Font = new Font(item.Font, FontStyle.Bold);
						this.lstError.Items.Add(item);
					}
				}
			}
			string notifications = this.plannedProduct.Product.NotificationMessage;
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

			bool showHeat = this.plannedProduct.RequestedHeatLoad > 0;
			bool showCool = this.plannedProduct.RequestedCoolLoad > 0;

			lblQHeat.Visible = showHeat;
			lblQHeatUnit.Visible = showHeat;
			lblQAnbHeat.Visible = showHeat;
			lblQAnbHeatUnit.Visible = showHeat;
			lblQHeatDiff.Visible = showHeat;
			lblQHeatDiffUnit.Visible = showHeat;
			lblQHeatRest.Visible = showHeat;
			lblQHeatRestUnit.Visible = showHeat;

			lblQCool.Visible = showCool;
			lblQCoolUnit.Visible = showCool;
			lblQAnbCool.Visible = showCool;
			lblQAnbCoolUnit.Visible = showCool;
			lblQCoolDiff.Visible = showCool;
			lblQCoolDiffUnit.Visible = showCool;
			lblQCoolRest.Visible = showCool;
			lblQCoolRestUnit.Visible = showCool;

			// General
			double qDiffHeat = this.plannedProduct.PlannedHeatLoad - this.plannedProduct.RequestedHeatLoad;
			double qDiffCool = this.plannedProduct.PlannedCoolLoad - this.plannedProduct.RequestedCoolLoad;

			lblQHeat.Text = Math.Round(this.plannedProduct.Product.PlannedHeatLoad, 2).ToString();
			lblQAnbHeat.Text = Math.Round(this.plannedProduct.Product.PlannedHeatLoadAnbindung, 0).ToString();
			lblQHeatDiff.Text = Math.Round(qDiffHeat, 0).ToString("+0;-0");
			lblQHeatRest.Text = Math.Round(this.plannedProduct.Product.AssociatedRoom.OpenHeatLoad, 2).ToString("+0.00;-0.00");
			lblQCool.Text = Math.Round(this.plannedProduct.Product.PlannedCoolLoad, 2).ToString();
			lblQAnbCool.Text = Math.Round(this.plannedProduct.Product.PlannedCoolLoadAnbindung, 0).ToString();
			lblQCoolDiff.Text = Math.Round(qDiffCool, 0).ToString("+0;-0");
			lblQCoolRest.Text = Math.Round(this.plannedProduct.Product.AssociatedRoom.OpenCoolLoad, 2).ToString("+0.00;-0.00");
			
			double diff = 0;
			if (showHeat) {
				if (showCool) {
					diff = Math.Min(qDiffHeat, qDiffCool);
				} else {
					diff = qDiffHeat;
				}
			} else if (showCool) {
				diff = qDiffCool;

			}
			lblQDiff.Text = Math.Round(diff, 0).ToString("+0;-0");
		}

		private void button1_Click(object sender, EventArgs e) {
			CalculateAndUpdate();
		}

		private void numLength_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreListChange == 0 && GetSelectedRow() != null) {
					this.changed = true;
					GetSelectedRow().LengthVerbindeleitungen = (double)this.numLength.Value;
					CalculateAndUpdate();
				}
			}
		}

		private void modulKlimaBodenPlanner_ProjectChanged(object sender) {
			this.CalculateAndUpdate();
			this.changed = true;
		}

		public bool Changed {
			get { return this.changed; }
		}

		private int ignoreRandfries = 0;
		private void numRandfries_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (ignoreRandfries == 0) {
					this.changed = true;
					this.akustik.Randfries = ((double)numRandfries.Value) / 100.0;
					this.planPanel.InvalidateGraphics();
				}
			}
		}

		private int ignoreBeplankung = 0;

		private void cbBeplankung_CheckedChanged(object sender, EventArgs e) {
			if (ignoreBeplankung == 0) {
				// Akustik is subclass of Glatt!!!
				ModulKlimaDeckeConstructionGlatt constr = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
				if (constr != null) {
					ignoreBeplankungValue++;
					if (this.cbBeplankung.Checked) {
						this.numBeplankungBreite.Value = (decimal)(0.625 * Product.ConfigPlanMeasureMultiplier);
						this.numBeplankungLaenge.Value = (decimal)(2.500 * Product.ConfigPlanMeasureMultiplier);
						this.numBeplankungBreite.Enabled = true;
						this.numBeplankungLaenge.Enabled = true;
						this.numBeplankungBreite.ReadOnly = false;
						this.numBeplankungLaenge.ReadOnly = false;
						constr.Beplankung = new WW.Math.Size2D((double)this.numBeplankungLaenge.Value / Product.ConfigPlanMeasureMultiplier, (double)this.numBeplankungBreite.Value / Product.ConfigPlanMeasureMultiplier);
					} else {
						this.numBeplankungBreite.Enabled = false;
						this.numBeplankungLaenge.Enabled = false;
						this.numBeplankungLaenge.ReadOnly = true;
						this.numBeplankungBreite.ReadOnly = true;
						this.numBeplankungLaenge.Text = "";
						this.numBeplankungBreite.Text = "";
						constr.Beplankung = null;
					}
                    this.btnMoveBeplankung.Enabled = constr.Beplankung.HasValue;
                    if (!this.btnMoveBeplankung.Enabled && this.btnMoveBeplankung.Checked) {
                        constr.Mode = ModulKlimaDeckeConstructionGlatt.ConstructionModifyMode.MOVE_SCHIENEN;
                        this.btnMoveBeplankung.Checked = false;
                        this.btnConstruction.Enabled = true;
                    }
					this.planPanel.InvalidateGraphics();
					ignoreBeplankungValue--;
				}
			}
		}

		private int ignoreBeplankungValue = 0;
		private void numBeplankung_ValueChanged(object sender, EventArgs e) {
			if (this.ignoreBeplankungValue == 0) {
				// Akustik is subclass of Glatt!!!
				ModulKlimaDeckeConstructionGlatt constr = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
				if (constr != null && this.cbBeplankung.Checked) {
					constr.Beplankung = new WW.Math.Size2D((double)this.numBeplankungLaenge.Value / Product.ConfigPlanMeasureMultiplier, (double)this.numBeplankungBreite.Value / Product.ConfigPlanMeasureMultiplier);
					this.planPanel.InvalidateGraphics();
				}				
			}
		}

		private void btnColor_Click(object sender, EventArgs e) {
			int dec = this.newVisible ? 1 : 0;
			if (this.lstCircuits.SelectedIndex >= 0 && this.lstCircuits.SelectedIndex < this.lstCircuits.Items.Count - dec) {
				ModulDeckeCircuit circuit = (this.lstCircuits.Items.Count - dec > this.lstCircuits.SelectedIndex ? (this.modulKlimaDeckePlanner.Product.PlannedCircuits[this.lstCircuits.SelectedIndex] as ModulDeckeCircuit) : null);
				if (circuit != null) {
					colorDialog.Color = circuit.CircuitColor;
					if (colorDialog.ShowDialog() == DialogResult.OK) {
						circuit.CircuitColor = colorDialog.Color;
						this.btnColor.BackColor = circuit.CircuitColor;
						this.planPanel.InvalidateGraphics();
					}
				}
			}
		}

		private void btnShowBeplankung_Click(object sender, EventArgs e) {
			this.modulKlimaDeckePlanner.DrawBeplankung = btnShowBeplankung.Checked;
			this.planPanel.InvalidateGraphics();
		}

		private void btnInvertDirection_Click(object sender, EventArgs e) {
			/*this.changed = true;
			List<KlimaFlaechenModul> modules = this.modulKlimaDeckePlanner.GetAllSelectedModules();
			foreach (KlimaFlaechenModul modul in modules) {
				modul.GraphBottomUp = !modul.GraphBottomUp;
			}
			this.UpdateSelectedModules();
			this.planPanel.InvalidateGraphics();*/

			this.changed = true;
			List<KlimaFlaechenModul> selectedModules = this.modulKlimaDeckePlanner.GetAllSelectedModules();
			List<KlimaFlaechenModul> modulesToInvert = new List<KlimaFlaechenModul>();
			List<IKlimaFlaechenVerbindung> linksToInvert = new List<IKlimaFlaechenVerbindung>();

			bool invertUnselected = false;
			//bool connectedToAnbindung = false;
			//List<KlimaFlaechenModul> ruecklaufConnected = new List<KlimaFlaechenModul>();
			//List<KlimaFlaechenModul> vorlaufConnected = new List<KlimaFlaechenModul>();
			Dictionary<IKlimaFlaechenVerbindung, ModulDeckeCircuit> anbindungen = new Dictionary<IKlimaFlaechenVerbindung, ModulDeckeCircuit>();

			foreach (KlimaFlaechenModul m1 in selectedModules) {
				int tmp;
				ModulDeckeCircuit c = this.modulKlimaDeckePlanner.Product.GetCircuitForModul(m1, out tmp);
				IKlimaFlaechenVerbindung link = c.GetNextLink(m1);
				if (link != null) {
					if (!linksToInvert.Contains(link)) {
						linksToInvert.Add(link);
					}
					if (link.EndConnectedToAnbindung) {
						anbindungen[link] = c;
					}
				}
				link = c.GetPreviousLink(m1);
				if (link != null) {
					if (!linksToInvert.Contains(link)) {
						linksToInvert.Add(link);
					}
					if (link.StartConnectedToAnbindung) {
						anbindungen[link] = c;
					}
				}
				foreach (KlimaFlaechenModul m2 in c.GetAllLinkedModules(m1)) {
					link = c.GetNextLink(m2);
					if (link != null) {
						if (!linksToInvert.Contains(link)) {
							linksToInvert.Add(link);
						}
						if (link.EndConnectedToAnbindung) {
							anbindungen[link] = c;
						}
					}
					link = c.GetPreviousLink(m2);
					if (link != null) {
						if (!linksToInvert.Contains(link)) {
							linksToInvert.Add(link);
						}
						if (link.StartConnectedToAnbindung) {
							anbindungen[link] = c;
						}
					}
					if (!modulesToInvert.Contains(m2)) {
						if (!selectedModules.Contains(m2)) {
							invertUnselected = true;
						}
						modulesToInvert.Add(m2);
					}
				}
			}

			if (anbindungen.Count > 0) {
				if (MessageBox.Show(Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_DurchstroemungsrichtungAendernText, Europlan.Common.EuroplanRes.ModulKlimaDeckePlannerForm_DurchstroemungsrichtungAendernTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
					return;
				}
				foreach (KeyValuePair<IKlimaFlaechenVerbindung, ModulDeckeCircuit> kvp in anbindungen) {
					if (kvp.Key is KlimaFlaechenSubAreaVerbindung) {
						kvp.Value.Links.Remove(kvp.Key as KlimaFlaechenSubAreaVerbindung);
					}
				}
			}

			foreach (KlimaFlaechenModul modul in modulesToInvert) {
				modul.GraphBottomUp = !modul.GraphBottomUp;
				if (!modul.DiagonalDurchstroemt) {
					modul.Orientation = (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
				}
			}
			foreach (IKlimaFlaechenVerbindung link in linksToInvert) {
				link.InvertDirection();
				//KlimaFlaechenModul tmp = link.Start;
				//link.Start = link.End;
				//link.End = tmp;
			}
			this.UpdateSelectedModules();
			this.planPanel.InvalidateGraphics();

		}
		private void lst_KeyDown(object sender, KeyEventArgs e) {
			this.modulKlimaDeckePlanner.PlannerKeyPress(e.KeyCode);
		}

		private void modulKlimaDeckePlanner_UpdateNewCount(object sender, ModulKlimaDeckePlanner.UpdateNewCountArgs e) {
			if (e.count == 0) {
				this.lblNewModules.Visible = false;
			} else {
				this.lblNewModules.Text = e.count.ToString() + " " + Europlan.Common.EuroplanRes.ModulKlimaDeckeForm_NeueModule;
				this.lblNewModules.Visible = true;
			}
		}

		private void SetProductPlanner() {
			if (this.planPanel.ProductPlanner != this.modulKlimaDeckePlanner) {
				double scale = this.planPanel.PlanScale;
				Vector2D translation = this.planPanel.PlanTranslation;
				this.planPanel.ProductPlanner = this.modulKlimaDeckePlanner;
				this.planPanel.PlanScale = scale;
				this.planPanel.PlanTranslation = translation;
			}
		}

		private void SetConnectionPlanner() {
			if (this.planPanel.ProductPlanner != this.connectionPlanner) {
				double scale = this.planPanel.PlanScale;
				Vector2D translation = this.planPanel.PlanTranslation;
				this.planPanel.ProductPlanner = this.connectionPlanner;
				this.planPanel.PlanScale = scale;
				this.planPanel.PlanTranslation = translation;
			}
		}

		private void btnMoveRow_Click(object sender, EventArgs e) {
			if (this.modulKlimaDeckePlanner.MoveRow(this.rowToMove)) {
				this.UpdateLists(true, true, true, true);
			}
			/*SelectMoveTargetForm form = new SelectMoveTargetForm(this.modulKlimaDeckePlanner.Product, true);
			if (form.ShowDialog() == DialogResult.OK) {
				this.modulKlimaDeckePlanner.Product.MoveRow(this.rowToMove, form.SelectedSubarea);
				this.planPanel.InvalidateGraphics();
			}
			form.Dispose();*/
		}

		private void btnMoveSubarea_Click(object sender, EventArgs e) {
			if (this.modulKlimaDeckePlanner.MoveSubarea(this.subAreaToMove)) {
				this.UpdateLists(true, true, true, true);
			}
			/*SelectMoveTargetForm form = new SelectMoveTargetForm(this.modulKlimaDeckePlanner.Product, false);
			if (form.ShowDialog() == DialogResult.OK) {
				this.modulKlimaDeckePlanner.Product.MoveSubarea(this.subAreaToMove, form.SelectedCircuit);
				this.planPanel.InvalidateGraphics();
			}
			form.Dispose();*/
		}

		private void cbVlFlexible_CheckedChanged(object sender, EventArgs e) {
			if (this.ignoreModuleFlexibleChange == 0 && this.cbVlFlexible.CheckState != CheckState.Indeterminate) {
				KlimaFlaechenModulVerbindung link;
				KlimaFlaechenSubAreaVerbindung saLink;
				int tmp;
				bool invertYAxis = this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.InvertYAxis;
				ModulDeckeCircuit c;
				foreach (KlimaFlaechenModul modul in this.modulKlimaDeckePlanner.HighlightModules) {
					c = this.modulKlimaDeckePlanner.Product.GetCircuitForModul(modul, out tmp);
					link = modul.GetInputLink(c, invertYAxis);
					if (link != null) {
						link.IsFlexible = (this.cbVlFlexible.CheckState == CheckState.Checked);
					} else {
						saLink = modul.GetSubareaInputLink(c, invertYAxis);
						if (saLink != null) {
							saLink.SetEndFlexible(modul, (this.cbVlFlexible.CheckState == CheckState.Checked));
						}
					}
				}
				this.planPanel.InvalidateGraphics();
			}
		}

		private void cbRlFlexible_CheckedChanged(object sender, EventArgs e) {
			if (this.ignoreModuleFlexibleChange == 0 && this.cbRlFlexible.CheckState != CheckState.Indeterminate) {
				KlimaFlaechenModulVerbindung link;
				KlimaFlaechenSubAreaVerbindung saLink;
				int tmp;
				bool invertYAxis = this.modulKlimaDeckePlanner.Product.AssociatedRoom.AssociatedPlan.InvertYAxis;
				ModulDeckeCircuit c;
				foreach (KlimaFlaechenModul modul in this.modulKlimaDeckePlanner.HighlightModules) {
					c = this.modulKlimaDeckePlanner.Product.GetCircuitForModul(modul, out tmp);
					link = modul.GetOutputLink(c, invertYAxis);
					if (link != null) {
						link.IsFlexible = (this.cbRlFlexible.CheckState == CheckState.Checked);
					} else {
						saLink = modul.GetSubareaOutputLink(c, invertYAxis);
						if (saLink != null) {
							saLink.SetStartFlexible(modul, (this.cbRlFlexible.CheckState == CheckState.Checked));
						}
					}
				}
				this.planPanel.InvalidateGraphics();
			}
		}

        private void btnShowPlanBg_Click(object sender, EventArgs e) {
            Product.ShowPlanInBackground = !Product.ShowPlanInBackground;
            this.btnShowPlanBg.Checked = Product.ShowPlanInBackground;
            this.planPanel.InvalidateGraphics();
        }

        private void numSchienenBreite_ValueChanged(object sender, EventArgs e) {
            if (!updateOngoing) {
				if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionAkustik) {
					ModulKlimaDeckeConstructionAkustik akustik = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionAkustik;
                    akustik.SchienenBreite = (double)this.numSchienenBreite.Value / 100;
					this.planPanel.InvalidateGraphics();
				} else if (this.modulKlimaDeckePlanner.Product.GraphConstruction is ModulKlimaDeckeConstructionGlatt) {
					ModulKlimaDeckeConstructionGlatt glatt = this.modulKlimaDeckePlanner.Product.GraphConstruction as ModulKlimaDeckeConstructionGlatt;
                    glatt.SchienenBreite = (double)this.numSchienenBreite.Value / 100;
					this.planPanel.InvalidateGraphics();
				}
            }
        }
	}
}