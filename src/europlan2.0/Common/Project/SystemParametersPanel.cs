using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class SystemParametersPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Configuration.ConfigurationType configurationType = Configuration.ConfigurationType.ProjectConfiguration;
		
		public SystemParametersPanel() {
			InitializeComponent();

			this.SetLanguage();

			this.tabSystemParameters.TabPages.Remove(this.tabHithermDefault);

			InitializeAllPanels();
			UpdateControl(true);
		}

		private void SetLanguage() {
			this.lblInfoProjectconfig.Text = EuroplanRes.SystemParametersPanel_InfoProjectConf;
			this.lblInfoUserconfig.Text = EuroplanRes.SystemParametersPanel_InfoCustomConf;

			this.lblEurovalSpreizungHeizMaxUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblEurovalSpreizungKuehlMaxUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblEurovalSpreizungKuehlMinUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblEurovalSpreizungHeizMinUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblEcothermSpreizungHeizMaxUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblEcothermSpreizungKuehlMaxUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblEcothermSpreizungKuehlMinUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblEcothermSpreizungHeizMinUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblModulBodenSpreizungHeizMaxUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblModulBodenSpreizungKuehlMaxUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblModulBodenSpreizungKuehlMinUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblModulBodenSpreizungHeizMinUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblModulDeckeSpreizungHeizMaxUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblModulDeckeSpreizungKuehlMaxUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblModulDeckeSpreizungKuehlMinUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblModulDeckeSpreizungHeizMinUnit.Text = EuroplanRes.Unit_Kelvin; //"K";
			this.lblEurovalEstrichueberdeckungUnit.Text = EuroplanRes.Unit_Meter; //"m";
			this.lblEurovalMindestueberdeckungUnit.Text = EuroplanRes.Unit_Meter; //"m";
			this.lblEurovalCircuitLengthUnit.Text = EuroplanRes.Unit_Meter; //"m";
			this.lblEcothermEstrichueberdeckungUnit.Text = EuroplanRes.Unit_Meter; //"m";
			this.lblEcothermMindestueberdeckungUnit.Text = EuroplanRes.Unit_Meter; //"m";
			this.lblEcothermCircuitLengthUnit.Text = EuroplanRes.Unit_Meter; //"m";
			this.lblHithermRegisterAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblHithermCompactRegisterAreaUnit.Text = EuroplanRes.Unit_Quadratmeter; //"m²";
			this.lblEurovalViskositaetUnit.Text = EuroplanRes.Unit_QmProSek; //"m²/s";
			this.lblEurovalWaermekapazitaetUnit.Text = EuroplanRes.Unit_KjProKgKelvin; //"kJ/(kg*K)";
			this.lblEurovalDichteUnit.Text = EuroplanRes.Unit_KgProKubikmeter; //"kg/m³";
			this.lblEurovalPressureMbarUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblEcothermPressureMbarUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblHithermPressureMbarUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblHithermCompactPressureMbarUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblModulBodenPressureMbarUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblModulDeckePressureMbarUnit.Text = EuroplanRes.Unit_Mbar; //"mbar";
			this.lblEurovalDurchflussUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblEcothermDurchflussUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblHithermDurchflussUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblHithermCompactDurchflussUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblModulBodenDurchflussUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblModulDeckeDurchflussUnit.Text = EuroplanRes.Unit_LiterProStunde; //"l/h";
			this.lblEurovalPressurePaUnit.Text = EuroplanRes.General_Pascal; //"Pa";
			this.lblEcothermPressurePaUnit.Text = EuroplanRes.General_Pascal; //"Pa";
			this.lblHithermPressurePaUnit.Text = EuroplanRes.General_Pascal; //"Pa";
			this.lblHithermCompactPressurePaUnit.Text = EuroplanRes.General_Pascal; //"Pa";
			this.lblModulBodenPressurePaUnit.Text = EuroplanRes.General_Pascal; //"Pa";
			this.lblModulDeckePressurePaUnit.Text = EuroplanRes.General_Pascal; //"Pa";
			this.lblGeneralAlphaWandUnitHeat.Text = EuroplanRes.Unit_WattProQmKelvin; //"W/(m²K)";
			this.lblGeneralAlphaDeckeUnitHeat.Text = EuroplanRes.Unit_WattProQmKelvin; //"W/(m²K)";
			this.lblGeneralAlphaBodenUnitHeat.Text = EuroplanRes.Unit_WattProQmKelvin; //"W/(m²K)";
			this.lblGeneralAlphaBodenUnitCool.Text = EuroplanRes.Unit_WattProQmKelvin; //"W/(m²K)";
			this.lblGeneralAlphaDeckeUnitCool.Text = EuroplanRes.Unit_WattProQmKelvin; //"W/(m²K)";
			this.lblGeneralAlphaWandUnitCool.Text = EuroplanRes.Unit_WattProQmKelvin; //"W/(m²K)";

			this.lblTitle.Text = EuroplanRes.SystemParametersPanel_Titel; //"Systemparameter";

			this.tabEuroval.Text = EuroplanRes.SystemParametersPanel_Euroval; //"Euroval®";
			this.lblEurovalViskositaet.Text = EuroplanRes.SystemParametersPanel_Viskositaet; //"kinematische Viskosität:";
			this.lblEurovalWaermekapazitaet.Text = EuroplanRes.SystemParametersPanel_Waermekapazitaet; //"spezifische Wärmekapazität:";
			this.lblEurovalDichte.Text = EuroplanRes.SystemParametersPanel_Dichte; //"Dichte des Mediums:";
			this.lblEurovalEstrichueberdeckung.Text = EuroplanRes.SystemParametersPanel_Estrichueberdeckung; //"Estrichüberdeckung:";
			this.lblEurovalMindestueberdeckung.Text = EuroplanRes.SystemParametersPanel_Mindestueberdeckung; //"Mindestüberdeckung:";
			this.lblEurovalGeometrie.Text = EuroplanRes.SystemParametersPanel_Geometriefaktor; //"Geometriefaktor:";
			this.lblEurovalSpreizungKuehlMax.Text = EuroplanRes.SystemParametersPanel_Max; //"max.";
			this.lblEurovalSpreizungHeizMax.Text = EuroplanRes.SystemParametersPanel_Max; //"max.";
			this.lblEurovalSpreizungKuehlMin.Text = EuroplanRes.SystemParametersPanel_Min; //"min.";
			this.lblEurovalSpreizungHeizMin.Text = EuroplanRes.SystemParametersPanel_Min; //"min.";
			this.lblEurovalNorm.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur; //"Maximale Oberflächentemperatur:";
			this.lblEurovalCircuitLength.Text = EuroplanRes.SystemParametersPanel_Rohrlaenge; //"Max. Rohrlänge je Heizkreis:";
			this.lblEurovalPressure.Text = EuroplanRes.SystemParametersPanel_Druckverlust; //"Max. Druckverlust:";
			this.lblEurovalDurchfluss.Text = EuroplanRes.SystemParametersPanel_Durchfluss; //"Max. Durchflußmenge:";
			this.lblEurovalSpreizungHeiz.Text = EuroplanRes.SystemParametersPanel_SpreizungHeiz; //"Spreizung Heizbetrieb:";
			this.lblEurovalSpreizungKuehl.Text = EuroplanRes.SystemParametersPanel_SpreizungKuehl; //"Spreizung Kühlbetrieb:";
			this.rbEurovalHarreitherNorm.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur27; //"27 °C (Harreither Werksempfehlung)";
			this.rbEurovalEN1264.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur29; //"29 °C (EN 1264)";
			this.btnEurovalStandard.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";
			this.cbEurovalGeometrieAktiviert.Text = EuroplanRes.SystemParametersPanel_GeometriefaktorAktiviert; //"aktiviert";

			this.tabEcotherm.Text = EuroplanRes.SystemParametersPanel_Ecotherm; //"Ecotherm®";
			this.lblEcothermEstrichueberdeckung.Text = EuroplanRes.SystemParametersPanel_Estrichueberdeckung; //"Estrichüberdeckung:";
			this.btnEcothermStandard.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";
			this.lblEcothermMindestueberdeckung.Text = EuroplanRes.SystemParametersPanel_Mindestueberdeckung; //"Mindestüberdeckung:";
			this.lblEcothermSpreizungKuehlMax.Text = EuroplanRes.SystemParametersPanel_Max; //"max.";
			this.lblEcothermSpreizungHeizMax.Text = EuroplanRes.SystemParametersPanel_Max; //"max.";
			this.lblEcothermNorm.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur; //"Maximale Oberflächentemperatur:";
			this.lblEcothermCircuitLength.Text = EuroplanRes.SystemParametersPanel_Rohrlaenge; //"Max. Rohrlänge je Heizkreis:";
			this.lblEcothermSpreizungKuehlMin.Text = EuroplanRes.SystemParametersPanel_Min; //"min.";
			this.lblEcothermPressure.Text = EuroplanRes.SystemParametersPanel_Druckverlust; //"Max. Druckverlust:";
			this.rbEcothermEN1264.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur29; //"29 °C (EN 1264)";
			this.lblEcothermDurchfluss.Text = EuroplanRes.SystemParametersPanel_Durchfluss; //"Max. Durchflußmenge:";
			this.rbEcothermHarreitherNorm.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur27; //"27 °C (Harreither Werksempfehlung)";
			this.lblEcothermSpreizungHeiz.Text = EuroplanRes.SystemParametersPanel_SpreizungHeiz; //"Spreizung Heizbetrieb:";
			this.lblEcothermSpreizungKuehl.Text = EuroplanRes.SystemParametersPanel_SpreizungKuehl; //"Spreizung Kühlbetrieb:";
			this.lblEcothermSpreizungHeizMin.Text = EuroplanRes.SystemParametersPanel_Min; //"min.";

			this.tabHitherm.Text = EuroplanRes.SystemParametersPanel_Hitherm; //"Hitherm®";
			this.lblHithermLeistungsfaktorCool.Text = EuroplanRes.SystemParametersPanel_LeistungsfaktorKuehl; //"Leistungsfaktor Kühlen:";
			this.lblHithermLeistungsfaktorHeat.Text = EuroplanRes.SystemParametersPanel_LeistungsfaktorHeiz; //"Leistungsfaktor Heizen:";
			this.lblHithermRegisterArea.Text = EuroplanRes.SystemParametersPanel_Heizflaeche; //"Max. Heizfläche je Heizkreis:";
			this.rbHithermPlus.Text = EuroplanRes.SystemParametersPanel_SystemHithermPlus; //"Hitherm®+";
			this.rbHitherm.Text = EuroplanRes.SystemParametersPanel_SystemHitherm; //"Hitherm®";
			this.btnHithermStandard.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";
			this.lblHithermSystem.Text = EuroplanRes.SystemParametersPanel_System; //"System:";
			this.lblHithermPressure.Text = EuroplanRes.SystemParametersPanel_Druckverlust; //"Max. Druckverlust:";
			this.lblHithermDurchfluss.Text = EuroplanRes.SystemParametersPanel_Durchfluss; //"Max. Durchflußmenge:";

			this.tabHithermDefault.Text = EuroplanRes.SystemParametersPanel_Hitherm; //"Hitherm®";
			this.rbHithermPlusDefault.Text = EuroplanRes.SystemParametersPanel_SystemHithermPlus; //"Hitherm®+";
			this.rbHithermDefault.Text = EuroplanRes.SystemParametersPanel_SystemHitherm; //"Hitherm®";
			this.lblHithermSystemDefault.Text = EuroplanRes.SystemParametersPanel_System; //"System:";
			this.btnHithermStandardDefault.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";

			this.tabHithermCompactDefault.Text = EuroplanRes.SystemParametersPanel_HithermCompact;
			this.rbHithermCompactPlusDefault.Text = EuroplanRes.SystemParametersPanel_SystemHithermCompactPlus; //"Hitherm®+";
			this.rbHithermCompactDefault.Text = EuroplanRes.SystemParametersPanel_SystemHithermCompact; //"Hitherm®";
			this.lblHithermCompactSystemDefault.Text = EuroplanRes.SystemParametersPanel_System; //"System:";
			this.btnHithermCompactStandardDefault.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";

			this.tabHithermCompact.Text = EuroplanRes.SystemParametersPanel_HithermCompact; //"Hitherm® Compact";
			this.lblHithermCompactLeistungsfaktorCool.Text = EuroplanRes.SystemParametersPanel_LeistungsfaktorKuehl; //"Leistungsfaktor Kühlen:";
			this.lblHithermCompactLeistungsfaktorHeat.Text = EuroplanRes.SystemParametersPanel_LeistungsfaktorHeiz; //"Leistungsfaktor Heizen:";
			this.lblHithermCompactRegisterArea.Text = EuroplanRes.SystemParametersPanel_Heizflaeche; //"Max. Heizfläche je Heizkreis:";
			this.rbHithermCompactPlus.Text = EuroplanRes.SystemParametersPanel_SystemHithermCompactPlus; //"Hitherm®+ Compact";
			this.btnHithermCompactStandard.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";
			this.rbHithermCompact.Text = EuroplanRes.SystemParametersPanel_SystemHithermCompact; //"Hitherm® Compact";
			this.lblHithermCompactSystem.Text = EuroplanRes.SystemParametersPanel_System; //"System:";
			this.lblHithermCompactPressure.Text = EuroplanRes.SystemParametersPanel_Druckverlust; //"Max. Druckverlust:";
			this.lblHithermCompactDurchfluss.Text = EuroplanRes.SystemParametersPanel_Durchfluss; //"Max. Durchflußmenge:";

			this.tabModulBoden.Text = EuroplanRes.SystemParametersPanel_KlimaBoden; //"Modul Klima-Boden";
			this.lblModulBodenSpreizungKuehlMax.Text = EuroplanRes.SystemParametersPanel_Max; //"max.";
			this.lblModulBodenSpreizungHeizMax.Text = EuroplanRes.SystemParametersPanel_Max; //"max.";
			this.lblModulBodenNorm.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur; //"Maximale Oberflächentemperatur:";
			this.lblModulBodenMaxModulesInCircuit.Text = EuroplanRes.SystemParametersPanel_Modulanzahl; //"Max Modulanzahl pro Heizkreis:";
			this.lblModulBodenSpreizungKuehlMin.Text = EuroplanRes.SystemParametersPanel_Min; //"min.";
			this.btnModulBodenStandard.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";
			this.lblModulBodenPressure.Text = EuroplanRes.SystemParametersPanel_Druckverlust; //"Max. Druckverlust:";
			this.lblModulBodenDurchfluss.Text = EuroplanRes.SystemParametersPanel_Durchfluss; //"Max. Durchflußmenge:";
			this.rbModulBodenEN1264.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur29; //"29 °C (EN 1264)";
			this.rbModulBodenHarreitherNorm.Text = EuroplanRes.SystemParametersPanel_Oberflaechentemperatur27; //"27 °C (Harreither Werksempfehlung)";
			this.lblModulBodenSpreizungHeiz.Text = EuroplanRes.SystemParametersPanel_SpreizungHeiz; //"Spreizung Heizbetrieb:";
			this.lblModulBodenSpreizungKuehl.Text = EuroplanRes.SystemParametersPanel_SpreizungKuehl; //"Spreizung Kühlbetrieb:";
			this.lblModulBodenSpreizungHeizMin.Text = EuroplanRes.SystemParametersPanel_Min; //"min.";

			this.tabModulDecke.Text = EuroplanRes.SystemParametersPanel_KlimaDecke; //"Modul Klima-Decke";
			this.lblModulDeckeLeistungsfaktorCool.Text = EuroplanRes.SystemParametersPanel_LeistungsfaktorKuehl; //"Leistungsfaktor Kühlen:";
			this.lblModulDeckeConstruction.Text = EuroplanRes.SystemParametersPanel_Unterkonstruktion; //"Unterkonstruktion:";
			this.lblModulDeckeLeistungsfaktorHeat.Text = EuroplanRes.SystemParametersPanel_LeistungsfaktorHeiz; //"Leistungsfaktor Heizen:";
			this.lblModulDeckeMaxRows.Text = EuroplanRes.SystemParametersPanel_Modulreihen; //"Max Modulreihen parallel:";
			this.lblModulDeckeSpreizungKuehlMax.Text = EuroplanRes.SystemParametersPanel_Max; //"max.";
			this.lblModulDeckeSpreizungHeizMax.Text = EuroplanRes.SystemParametersPanel_Max; //"max.";
			this.lblModulDeckeMaxModulesInRow.Text = EuroplanRes.SystemParametersPanel_ModuleInSerie; //"Max Modulanzahl in Serie:";
			this.lblModulDeckeMaxModulesInCircuit.Text = EuroplanRes.SystemParametersPanel_Modulanzahl; //"Max Modulanzahl pro Heizkreis:";
			this.lblModulDeckeSpreizungKuehlMin.Text = EuroplanRes.SystemParametersPanel_Min; //"min.";
			this.lblModulDeckePressurePa.Text = EuroplanRes.SystemParametersPanel_Druckverlust; //"Max. Druckverlust:";
			this.lblModulDeckeDurchfluss.Text = EuroplanRes.SystemParametersPanel_Durchfluss; //"Max. Durchflußmenge:";
			this.btnModulDeckeStandard.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";
			this.lblModulDeckeSpreizungHeiz.Text = EuroplanRes.SystemParametersPanel_SpreizungHeiz; //"Spreizung Heizbetrieb:";
			this.lblModulDeckeSpreizungKuehl.Text = EuroplanRes.SystemParametersPanel_SpreizungKuehl; //"Spreizung Kühlbetrieb:";
			this.lblModulDeckeSpreizungHeizMin.Text = EuroplanRes.SystemParametersPanel_Min; //"min.";

			this.tabGeneral.Text = EuroplanRes.SystemParametersPanel_Allgemein; //"Allgemein";
			this.lblGeneralAlphaWandHeat.Text = EuroplanRes.SystemParametersPanel_AlphaWand; //"Alpha Wand:";
			this.lblGeneralAlphaDeckeHeat.Text = EuroplanRes.SystemParametersPanel_AlphaDecke; //"Alpha Decke:";
			this.lblGeneralAlphaBodenHeat.Text = EuroplanRes.SystemParametersPanel_AlphaBoden; //"Alpha Boden:";
			this.lblHeat.Text = EuroplanRes.SystemParametersPanel_Heizen; //"Heizen";
			this.lblCool.Text = EuroplanRes.SystemParametersPanel_Kuehlen; //"Kühlen";
			this.lblGeneralAlphaBodenCool.Text = EuroplanRes.SystemParametersPanel_AlphaBoden; //"Alpha Boden:";
			this.lblGeneralAlphaDeckeCool.Text = EuroplanRes.SystemParametersPanel_AlphaDecke; //"Alpha Decke:";
			this.btnGeneralStandard.Text = EuroplanRes.SystemParametersPanel_Ruecksetzen; //"Standardwerte";
			this.lblGeneralAlphaWandCool.Text = EuroplanRes.SystemParametersPanel_AlphaWand; //"Alpha Wand:";
		}

		public Configuration.ConfigurationType ConfigurationType {
			get { return this.configurationType; }
			set {
				if (this.configurationType != value) {
					this.configurationType = value;
					this.panelHeader.Visible = this.configurationType != Configuration.ConfigurationType.UserConfiguration;
					this.panInfoProjectConfig.Visible = this.configurationType != Configuration.ConfigurationType.UserConfiguration;
					this.panInfoUserConfig.Visible = this.configurationType == Configuration.ConfigurationType.UserConfiguration;

					InitializeAllPanels();
					UpdateControl(true);
				}
			}
		}

		public void InitializeAllPanels() {
			InitializeEurovalPanel();
			InitializeEcothermPanel();
			InitializeHithermPanel();
			InitializeHithermCompactPanel();
			InitializeModulBodenPanel();
			InitializeModulDeckePanel();
			InitializeGeneralPanel();
		}

		public void UpdateControl(bool resetUserInterface) {
			InitializeEurovalValues();
			InitializeEcothermValues();
			InitializeModulBodenValues();
			InitializeModulDeckeValues();
			InitializeHithermValues();
			InitializeHithermCompactValues();
		}

		public bool AllowLeave() {
			return true;
		}

		private void btnEurovalStandard_Click(object sender, EventArgs e) {
			EurovalProduct.StaticInitialize(Configuration.UserTemplate);
			InitializeEurovalValues();
		}

		private void btnEcothermStandard_Click(object sender, EventArgs e) {
			EcothermProduct.StaticInitialize(Configuration.UserTemplate);
			InitializeEcothermValues();
		}

		private void btnModulBodenStandard_Click(object sender, EventArgs e) {
			ModulKlimaBodenProduct.StaticInitialize(Configuration.UserTemplate);
			InitializeModulBodenValues();
		}

		private void btnModulDeckeStandard_Click(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.StaticInitialize(Configuration.UserTemplate);
			InitializeModulDeckeValues();
		}

		private void btnHithermStandard_Click(object sender, EventArgs e) {
			HithermProduct.StaticInitialize(Configuration.UserTemplate);
			InitializeHithermValues();
		}

		private void btnHithermCompactStandard_Click(object sender, EventArgs e) {
			HithermCompactProduct.StaticInitialize(Configuration.UserTemplate);
			InitializeHithermCompactValues();
		}

		private void btnGeneralStandard_Click(object sender, EventArgs e) {
			Product.StaticInitialize(Configuration.UserTemplate);
			InitializeGeneralValues();
		}

		private void InitializeEurovalPanel() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				if (this.tabSystemParameters.TabPages.Contains(this.tabEuroval)) {
					this.tabSystemParameters.TabPages.Remove(this.tabEuroval);
				}
			} else {
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEuroval)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabEuroval)) {
						this.tabSystemParameters.TabPages.Remove(this.tabEuroval);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabEuroval)) {
						this.tabSystemParameters.TabPages.Add(this.tabEuroval);
					}
				}
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
					this.layoutEuroval.Controls.Remove(this.lblEurovalGeometrie);
					this.layoutEuroval.Controls.Remove(this.numEurovalGeometrie);
					this.layoutEuroval.Controls.Remove(this.cbEurovalGeometrieAktiviert);
					this.layoutEuroval.Controls.Remove(this.lblEurovalMindestueberdeckung);
					this.layoutEuroval.Controls.Remove(this.numEurovalMindestueberdeckung);
					this.layoutEuroval.Controls.Remove(this.lblEurovalMindestueberdeckungUnit);
					this.layoutEuroval.Controls.Remove(this.lblEurovalEstrichueberdeckung);
					this.layoutEuroval.Controls.Remove(this.numEurovalEstrichueberdeckung);
					this.layoutEuroval.Controls.Remove(this.lblEurovalEstrichueberdeckungUnit);
					this.layoutEuroval.Controls.Remove(this.lblEurovalDichte);
					this.layoutEuroval.Controls.Remove(this.numEurovalDichte);
					this.layoutEuroval.Controls.Remove(this.lblEurovalDichteUnit);
					this.layoutEuroval.Controls.Remove(this.lblEurovalWaermekapazitaet);
					this.layoutEuroval.Controls.Remove(this.numEurovalWaermekapazitaet);
					this.layoutEuroval.Controls.Remove(this.lblEurovalWaermekapazitaetUnit);
					this.layoutEuroval.Controls.Remove(this.lblEurovalViskositaet);
					this.layoutEuroval.Controls.Remove(this.numEurovalViskositaet);
					this.layoutEuroval.Controls.Remove(this.lblEurovalViskositaetUnit);
					this.layoutEuroval.SetRow(this.btnEurovalStandard, this.layoutEuroval.GetRow(this.btnEurovalStandard) - 6);
				}
			}
		}

		private void InitializeEcothermPanel() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				if (this.tabSystemParameters.TabPages.Contains(this.tabEcotherm)) {
					this.tabSystemParameters.TabPages.Remove(this.tabEcotherm);
				}
			} else {
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEcotherm)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabEcotherm)) {
						this.tabSystemParameters.TabPages.Remove(this.tabEcotherm);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabEcotherm)) {
						this.tabSystemParameters.TabPages.Add(this.tabEcotherm);
					}
				}
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
					this.layoutEcotherm.Controls.Remove(this.lblEcothermMindestueberdeckung);
					this.layoutEcotherm.Controls.Remove(this.numEcothermMindestueberdeckung);
					this.layoutEcotherm.Controls.Remove(this.lblEcothermMindestueberdeckungUnit);
					this.layoutEcotherm.Controls.Remove(this.lblEcothermEstrichueberdeckung);
					this.layoutEcotherm.Controls.Remove(this.numEcothermEstrichueberdeckung);
					this.layoutEcotherm.Controls.Remove(this.lblEcothermEstrichueberdeckungUnit);
					this.layoutEcotherm.SetRow(this.btnEcothermStandard, this.layoutEcotherm.GetRow(this.btnEcothermStandard) - 2);
				}
			}
		}

		private void InitializeHithermPanel() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				if (this.tabSystemParameters.TabPages.Contains(this.tabHitherm)) {
					this.tabSystemParameters.TabPages.Remove(this.tabHitherm);
				}
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHitherm)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabHithermDefault)) {
						this.tabSystemParameters.TabPages.Remove(this.tabHithermDefault);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabHithermDefault)) {
						this.tabSystemParameters.TabPages.Add(this.tabHithermDefault);
					}
				}
			} else {
				if (this.tabSystemParameters.TabPages.Contains(this.tabHithermDefault)) {
					this.tabSystemParameters.TabPages.Remove(this.tabHithermDefault);
				}
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHitherm)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabHitherm)) {
						this.tabSystemParameters.TabPages.Remove(this.tabHitherm);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabHitherm)) {
						this.tabSystemParameters.TabPages.Add(this.tabHitherm);
					}
				}
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
					this.layoutHitherm.Controls.Remove(this.lblHithermLeistungsfaktorHeat);
					this.layoutHitherm.Controls.Remove(this.numHithermLeistungsfaktorHeat);
					this.layoutHitherm.Controls.Remove(this.lblHithermLeistungsfaktorCool);
					this.layoutHitherm.Controls.Remove(this.numHithermLeistungsfaktorCool);
					this.layoutHitherm.SetRow(this.btnHithermStandard, this.layoutHitherm.GetRow(this.btnHithermStandard) - 2);
				}
			}
		}

		private void InitializeHithermCompactPanel() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				if (this.tabSystemParameters.TabPages.Contains(this.tabHithermCompact)) {
					this.tabSystemParameters.TabPages.Remove(this.tabHithermCompact);
				}
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHitherm)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabHithermCompactDefault)) {
						this.tabSystemParameters.TabPages.Remove(this.tabHithermCompactDefault);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabHithermCompactDefault)) {
						this.tabSystemParameters.TabPages.Add(this.tabHithermCompactDefault);
					}
				}
			} else {
				if (this.tabSystemParameters.TabPages.Contains(this.tabHithermCompactDefault)) {
					this.tabSystemParameters.TabPages.Remove(this.tabHithermCompactDefault);
				}
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHithermCompact)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabHithermCompact)) {
						this.tabSystemParameters.TabPages.Remove(this.tabHithermCompact);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabHithermCompact)) {
						this.tabSystemParameters.TabPages.Add(this.tabHithermCompact);
					}
				}
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
					this.layoutHithermCompact.Controls.Remove(this.lblHithermCompactLeistungsfaktorHeat);
					this.layoutHithermCompact.Controls.Remove(this.numHithermCompactLeistungsfaktorHeat);
					this.layoutHithermCompact.Controls.Remove(this.lblHithermCompactLeistungsfaktorCool);
					this.layoutHithermCompact.Controls.Remove(this.numHithermCompactLeistungsfaktorCool);
					this.layoutHithermCompact.SetRow(this.btnHithermCompactStandard, this.layoutHithermCompact.GetRow(this.btnHithermCompactStandard) - 2);
				}
			}
		}

		private void InitializeModulBodenPanel() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				if (this.tabSystemParameters.TabPages.Contains(this.tabModulBoden)) {
					this.tabSystemParameters.TabPages.Remove(this.tabModulBoden);
				}
			} else {
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaBoden)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabModulBoden)) {
						this.tabSystemParameters.TabPages.Remove(this.tabModulBoden);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabModulBoden)) {
						this.tabSystemParameters.TabPages.Add(this.tabModulBoden);
					}
				}
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
					// nothing to do
				}
			}
		}

		private void InitializeModulDeckePanel() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				if (this.tabSystemParameters.TabPages.Contains(this.tabModulDecke)) {
					this.tabSystemParameters.TabPages.Remove(this.tabModulDecke);
				}
			} else {
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaDecke)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabModulDecke)) {
						this.tabSystemParameters.TabPages.Remove(this.tabModulDecke);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabModulDecke)) {
						this.tabSystemParameters.TabPages.Add(this.tabModulDecke);
					}
				}
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
					if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
						this.layoutModulDecke.Controls.Remove(this.lblModulDeckeLeistungsfaktorHeat);
						this.layoutModulDecke.Controls.Remove(this.numModulDeckeLeistungsfaktorHeat);
						this.layoutModulDecke.Controls.Remove(this.lblModulDeckeLeistungsfaktorCool);
						this.layoutModulDecke.Controls.Remove(this.numModulDeckeLeistungsfaktorCool);
						this.layoutModulDecke.SetRow(this.btnModulDeckeStandard, this.layoutModulDecke.GetRow(this.btnModulDeckeStandard) - 2);
					}
				}
			}
		}

		private void InitializeGeneralPanel() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				if (this.tabSystemParameters.TabPages.Contains(this.tabGeneral)) {
					this.tabSystemParameters.TabPages.Remove(this.tabGeneral);
				}
			} else {
				Licensing.License license = Licensing.LicenseManager.Instance.License;
				if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
					if (this.tabSystemParameters.TabPages.Contains(this.tabGeneral)) {
						this.tabSystemParameters.TabPages.Remove(this.tabGeneral);
					}
				} else {
					if (!this.tabSystemParameters.TabPages.Contains(this.tabGeneral)) {
						this.tabSystemParameters.TabPages.Add(this.tabGeneral);
					}
				}
			}
		}

		private void InitializeEurovalValues() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				// nothing to do yet
			} else {
				rbEurovalHarreitherNorm.Checked = EurovalProduct.ConfigUseHarreitherNorm;
				rbEurovalEN1264.Checked = !EurovalProduct.ConfigUseHarreitherNorm;
				numEurovalCircuitLength.Value = (decimal)EurovalProduct.ConfigMaxCircuitLength;
				numEurovalPressurePa.Value = EurovalProduct.ConfigMaxPressureLost;
				numEurovalDurchfluss.Value = EurovalProduct.ConfigMaxDurchfluss;
				numEurovalSpreizungHeizMin.Value = (decimal)EurovalProduct.ConfigSpreizungHeizMin;
				numEurovalSpreizungHeizMax.Value = (decimal)EurovalProduct.ConfigSpreizungHeizMax;
				numEurovalSpreizungKuehlMin.Value = (decimal)EurovalProduct.ConfigSpreizungKuehlMin;
				numEurovalSpreizungKuehlMax.Value = (decimal)EurovalProduct.ConfigSpreizungKuehlMax;
				numEurovalGeometrie.Value = (decimal)EurovalProduct.ConfigAg;
				cbEurovalGeometrieAktiviert.Checked = EurovalProduct.ConfigAgActivated;
				numEurovalGeometrie.Enabled = EurovalProduct.ConfigAgActivated;
				numEurovalMindestueberdeckung.Value = (decimal)EurovalProduct.ConfigSu0;
				numEurovalEstrichueberdeckung.Value = (decimal)EurovalProduct.ConfigSu;
				numEurovalDichte.Value = (decimal)EurovalProduct.ConfigRho;
				numEurovalWaermekapazitaet.Value = (decimal)EurovalProduct.ConfigC;
				numEurovalViskositaet.Value = (decimal)EurovalProduct.ConfigV;
			}
		}

		private void InitializeEcothermValues() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				// nothing to do yet
			} else {
				rbEcothermHarreitherNorm.Checked = EcothermProduct.ConfigUseHarreitherNorm;
				rbEcothermEN1264.Checked = !EcothermProduct.ConfigUseHarreitherNorm;
				numEcothermCircuitLength.Value = (decimal)EcothermProduct.ConfigMaxCircuitLength;
				numEcothermPressurePa.Value = EcothermProduct.ConfigMaxPressureLost;
				numEcothermDurchfluss.Value = EcothermProduct.ConfigMaxDurchfluss;
				numEcothermSpreizungHeizMin.Value = (decimal)EcothermProduct.ConfigSpreizungHeizMin;
				numEcothermSpreizungHeizMax.Value = (decimal)EcothermProduct.ConfigSpreizungHeizMax;
				numEcothermSpreizungKuehlMin.Value = (decimal)EcothermProduct.ConfigSpreizungKuehlMin;
				numEcothermSpreizungKuehlMax.Value = (decimal)EcothermProduct.ConfigSpreizungKuehlMax;
				numEcothermMindestueberdeckung.Value = (decimal)EcothermProduct.ConfigSu0;
				numEcothermEstrichueberdeckung.Value = (decimal)EcothermProduct.ConfigSu;
			}
		}

		private void InitializeModulBodenValues() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				// nothing to do yet
			} else {
				rbModulBodenHarreitherNorm.Checked = ModulKlimaBodenProduct.ConfigUseHarreitherNorm;
				rbModulBodenEN1264.Checked = !ModulKlimaBodenProduct.ConfigUseHarreitherNorm;
				numModulBodenPressurePa.Value = ModulKlimaBodenProduct.ConfigMaxPressureLost;
				numModulBodenDurchfluss.Value = ModulKlimaBodenProduct.ConfigMaxDurchfluss;
				numModulBodenMaxModulesInCircuit.Value = ModulKlimaBodenProduct.ConfigModulesInCircuit;
				numModulBodenSpreizungHeizMin.Value = (decimal)ModulKlimaBodenProduct.ConfigSpreizungHeizMin;
				numModulBodenSpreizungHeizMax.Value = (decimal)ModulKlimaBodenProduct.ConfigSpreizungHeizMax;
				numModulBodenSpreizungKuehlMin.Value = (decimal)ModulKlimaBodenProduct.ConfigSpreizungKuehlMin;
				numModulBodenSpreizungKuehlMax.Value = (decimal)ModulKlimaBodenProduct.ConfigSpreizungKuehlMax;
			}
		}

		private void InitializeModulDeckeValues() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				// nothing to do yet
			} else {
				cmbModulDeckeConstruction.Items.Clear();
				foreach (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum item in Enum.GetValues(typeof(ModulKlimaDeckeProduct.ModulCeilingConstructionEnum))) {
					this.cmbModulDeckeConstruction.Items.Add(item);
				}
				numModulDeckePressurePa.Value = ModulKlimaDeckeProduct.ConfigMaxPressureLost;
				numModulDeckeDurchfluss.Value = ModulKlimaDeckeProduct.ConfigMaxDurchfluss;
				numModulDeckeMaxModulesInRow.Value = ModulKlimaDeckeProduct.ConfigMaxModulesInRow;
				numModulDeckeMaxRows.Value = ModulKlimaDeckeProduct.ConfigMaxModulesInParallel;
				numModulDeckeMaxModulesInCircuit.Value = ModulKlimaDeckeProduct.ConfigModulesInCircuit;
				numModulDeckeLeistungsfaktorHeat.Value = (decimal)ModulKlimaDeckeProduct.ConfigLeistungsFaktorHeizen;
				numModulDeckeLeistungsfaktorCool.Value = (decimal)ModulKlimaDeckeProduct.ConfigLeistungsFaktorKuehlen;
				numModulDeckeSpreizungHeizMin.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungHeizMin;
				numModulDeckeSpreizungHeizMax.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungHeizMax;
				numModulDeckeSpreizungKuehlMin.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungKuehlMin;
				numModulDeckeSpreizungKuehlMax.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungKuehlMax;
				cmbModulDeckeConstruction.SelectedItem = (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum)ModulKlimaDeckeProduct.ConfigModulCeilingConstruction;
			}
		}

		private void InitializeHithermValues() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				rbHithermDefault.Checked = !Configuration.UserTemplate.GetProductParameterAsBool<HithermProduct>("ConfigUsePlus");
				rbHithermPlusDefault.Checked = !rbHithermDefault.Checked;
			} else {
				rbHitherm.Checked = !HithermProduct.ConfigUsePlus;
				numHithermRegisterArea.Value = (decimal)HithermProduct.ConfigMaxRegisterArea;
				rbHithermPlus.Checked = HithermProduct.ConfigUsePlus;
				numHithermPressurePa.Value = HithermProduct.ConfigMaxPressureLost;
				numHithermDurchfluss.Value = HithermProduct.ConfigMaxDurchfluss;
				numHithermLeistungsfaktorHeat.Value = (decimal)HithermProduct.ConfigLeistungsFaktorHeizen;
				numHithermLeistungsfaktorCool.Value = (decimal)HithermProduct.ConfigLeistungsFaktorKuehlen;
			}
		}

		private void InitializeHithermCompactValues() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				rbHithermCompactDefault.Checked = !Configuration.UserTemplate.GetProductParameterAsBool<HithermCompactProduct>("ConfigUsePlus");
				rbHithermCompactPlusDefault.Checked = !rbHithermCompactDefault.Checked;
			} else {
				rbHithermCompact.Checked = !HithermCompactProduct.ConfigUsePlus;
				numHithermCompactRegisterArea.Value = (decimal)HithermCompactProduct.ConfigMaxRegisterArea;
				rbHithermCompactPlus.Checked = HithermCompactProduct.ConfigUsePlus;
				numHithermCompactPressurePa.Value = HithermCompactProduct.ConfigMaxPressureLost;
				numHithermCompactDurchfluss.Value = HithermCompactProduct.ConfigMaxDurchfluss;
				numHithermCompactLeistungsfaktorHeat.Value = (decimal)HithermCompactProduct.ConfigLeistungsFaktorHeizen;
				numHithermCompactLeistungsfaktorCool.Value = (decimal)HithermCompactProduct.ConfigLeistungsFaktorKuehlen;
			}
		}

		private void InitializeGeneralValues() {
			if (this.configurationType == Configuration.ConfigurationType.UserConfiguration) {
				// nothing to do yet
			} else {
				numGeneralAlphaBodenHeat.Value = (decimal)Product.ConfigAlphaBodenHeat;
				numGeneralAlphaDeckeHeat.Value = (decimal)Product.ConfigAlphaDeckeHeat;
				numGeneralAlphaWandHeat.Value = (decimal)Product.ConfigAlphaWandHeat;
				numGeneralAlphaBodenCool.Value = (decimal)Product.ConfigAlphaBodenCool;
				numGeneralAlphaDeckeCool.Value = (decimal)Product.ConfigAlphaDeckeCool;
				numGeneralAlphaWandCool.Value = (decimal)Product.ConfigAlphaWandCool;
			}
		}

		private void rbEurovalHarreitherNorm_CheckedChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigUseHarreitherNorm = rbEurovalHarreitherNorm.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbEurovalEN1264_CheckedChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigUseHarreitherNorm = rbEurovalHarreitherNorm.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalCircuitLength_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigMaxCircuitLength = (double)numEurovalCircuitLength.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalDurchfluss_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigMaxDurchfluss = (int)numEurovalDurchfluss.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalPressurePa_ValueChanged(object sender, EventArgs e) {
			numEurovalPressureMbar.Value = numEurovalPressurePa.Value / 100;
			EurovalProduct.ConfigMaxPressureLost = (int)numEurovalPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalPressureMbar_ValueChanged(object sender, EventArgs e) {
			numEurovalPressurePa.Value = numEurovalPressureMbar.Value * 100;
			EurovalProduct.ConfigMaxPressureLost = (int)numEurovalPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalSpreizungHeizMin_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungHeizMin = (double)numEurovalSpreizungHeizMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalSpreizungHeizMax_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungHeizMax = (double)numEurovalSpreizungHeizMax.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalSpreizungKühlMin_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungKuehlMin = (double)numEurovalSpreizungKuehlMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalSpreizungKühlMax_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungKuehlMax = (double)numEurovalSpreizungKuehlMax.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalGeometrie_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigAg = (double)numEurovalGeometrie.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void cbEurovalGeometrieAktiviert_CheckedChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigAgActivated = cbEurovalGeometrieAktiviert.Checked;
			numEurovalGeometrie.Enabled = EurovalProduct.ConfigAgActivated;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalMindestueberdeckung_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSu0 = (double)numEurovalMindestueberdeckung.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalEstrichueberdeckung_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSu = (double)numEurovalEstrichueberdeckung.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalDichte_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigRho = (double)numEurovalDichte.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalWaermekapazitaet_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigC = (double)numEurovalWaermekapazitaet.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalViskositaet_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigV = (double)numEurovalViskositaet.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbModulBodenHarreitherNorm_CheckedChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigUseHarreitherNorm = rbModulBodenHarreitherNorm.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbModulBodenEN1264_CheckedChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigUseHarreitherNorm = rbModulBodenHarreitherNorm.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenDurchfluss_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigMaxDurchfluss = (int)numModulBodenDurchfluss.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}
		
		private void numModulBodenPressurePa_ValueChanged(object sender, EventArgs e) {
			numModulBodenPressureMbar.Value = numModulBodenPressurePa.Value / 100;
			ModulKlimaBodenProduct.ConfigMaxPressureLost = (int)numModulBodenPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}
		
		private void numModulBodenPressureMbar_ValueChanged(object sender, EventArgs e) {
			numModulBodenPressurePa.Value = numModulBodenPressureMbar.Value * 100;
			ModulKlimaBodenProduct.ConfigMaxPressureLost = (int)numModulBodenPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenMaxModulesInCircuit_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigModulesInCircuit = (int)numModulBodenMaxModulesInCircuit.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeMaxModulesInRow_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigMaxModulesInRow = (int)numModulDeckeMaxModulesInRow.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeMaxRows_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigMaxModulesInParallel = (int)numModulDeckeMaxRows.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeMaxModulesInCircuit_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigModulesInCircuit = (int)numModulDeckeMaxModulesInCircuit.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckePressurePa_ValueChanged(object sender, EventArgs e) {
			numModulDeckePressureMbar.Value = numModulDeckePressurePa.Value / 100;
			ModulKlimaDeckeProduct.ConfigMaxPressureLost = (int)numModulDeckePressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckePressureMbar_ValueChanged(object sender, EventArgs e) {
			numModulDeckePressurePa.Value = numModulDeckePressureMbar.Value * 100;
			ModulKlimaDeckeProduct.ConfigMaxPressureLost = (int)numModulDeckePressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeDurchfluss_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigMaxDurchfluss = (int)numModulDeckeDurchfluss.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeLeistungsfaktorHeat_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigLeistungsFaktorHeizen = (double)numModulDeckeLeistungsfaktorHeat.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeLeistungsfaktorCool_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigLeistungsFaktorKuehlen = (double)numModulDeckeLeistungsfaktorCool.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenSpreizungHeizMin_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigSpreizungHeizMin = (double)numModulBodenSpreizungHeizMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenSpreizungHeizMax_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigSpreizungHeizMax = (double)numModulBodenSpreizungHeizMax.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenSpreizungKuehlMin_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigSpreizungKuehlMin = (double)numModulBodenSpreizungKuehlMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenSpreizungKuehlMax_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigSpreizungKuehlMax = (double)numModulBodenSpreizungKuehlMax.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeSpreizungHeizMin_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigSpreizungHeizMin = (double)numModulDeckeSpreizungHeizMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeSpreizungHeizMax_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigSpreizungHeizMax = (double)numModulDeckeSpreizungHeizMax.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeSpreizungKuehlMin_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigSpreizungKuehlMin = (double)numModulDeckeSpreizungKuehlMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeSpreizungKuehlMax_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigSpreizungKuehlMax = (double)numModulDeckeSpreizungKuehlMax.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbHitherm_CheckedChanged(object sender, EventArgs e) {
			if (rbHitherm.Checked != rbHithermPlus.Checked) {
				bool canceled = false;
				if (rbHithermPlus.Checked) {
					if (MessageBox.Show(EuroplanRes.SystemParametersPanel_LeistungsregisterLoeschenText, EuroplanRes.SystemParametersPanel_LeistungsregisterLoeschenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
						foreach (Floor f in Project.Instance.Floors) {
							foreach (Room r in f.Rooms) {
								foreach (PlannedProduct pp in r.PlannedProducts) {
									if (pp.Product is HithermProduct) {
										HithermProduct hp = pp.Product as HithermProduct;
										List<HithermRegister> removeRegisters = new List<HithermRegister>();
										foreach (HithermCircuit hc in hp.PlannedCircuits) {
											foreach (HithermRegister hr in hc.Registers) {
												if (!hr.IsHochleistungsRegister) {
													removeRegisters.Add(hr);
												}
											}
										}
										foreach (HithermRegister hr in removeRegisters) {
											hp.RemoveRegisterFromCircuit(hr);
										}
									}
								}
							}
						}
					} else {
						canceled = true;
					}
				}
				if (canceled) {
					this.InitializeHithermValues();
				} else {
					HithermProduct.ConfigUsePlus = this.rbHithermPlus.Checked;
					if (ProjectChanged != null) {
						ProjectChanged(null);
					}
				}
			}
		}

		private void numHithermRegisterArea_ValueChanged(object sender, EventArgs e) {
			HithermProduct.ConfigMaxRegisterArea = (double)numHithermRegisterArea.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermPressurePa_ValueChanged(object sender, EventArgs e) {
			numHithermPressureMbar.Value = numHithermPressurePa.Value / 100;
			HithermProduct.ConfigMaxPressureLost = (int)numHithermPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermPressureMbar_ValueChanged(object sender, EventArgs e) {
			numHithermPressurePa.Value = numHithermPressureMbar.Value * 100;
			HithermProduct.ConfigMaxPressureLost = (int)numHithermPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermDurchfluss_ValueChanged(object sender, EventArgs e) {
			HithermProduct.ConfigMaxDurchfluss = (int)numHithermDurchfluss.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}


		private void rbHithermCompact_CheckedChanged(object sender, EventArgs e) {
			if (rbHithermCompact.Checked != rbHithermCompactPlus.Checked) {
				HithermCompactProduct.ConfigUsePlus = this.rbHithermCompactPlus.Checked;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			}
		}

		private void numHithermLeistungsfaktorHeat_ValueChanged(object sender, EventArgs e) {
			HithermProduct.ConfigLeistungsFaktorHeizen = (double)numHithermLeistungsfaktorHeat.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermLeistungsfaktorCool_ValueChanged(object sender, EventArgs e) {
			HithermProduct.ConfigLeistungsFaktorKuehlen = (double)numHithermLeistungsfaktorCool.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermCompactRegisterArea_ValueChanged(object sender, EventArgs e) {
			HithermCompactProduct.ConfigMaxRegisterArea = (double)numHithermCompactRegisterArea.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermCompactPressurePa_ValueChanged(object sender, EventArgs e) {
			numHithermCompactPressureMbar.Value = numHithermCompactPressurePa.Value / 100;
			HithermCompactProduct.ConfigMaxPressureLost = (int)numHithermCompactPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermCompactPressureMbar_ValueChanged(object sender, EventArgs e) {
			numHithermCompactPressurePa.Value = numHithermCompactPressureMbar.Value * 100;
			HithermCompactProduct.ConfigMaxPressureLost = (int)numHithermCompactPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermCompactDurchfluss_ValueChanged(object sender, EventArgs e) {
			HithermCompactProduct.ConfigMaxDurchfluss = (int)numHithermCompactDurchfluss.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermCompactLeistungsfaktorHeat_ValueChanged(object sender, EventArgs e) {
			HithermCompactProduct.ConfigLeistungsFaktorHeizen = (double)numHithermCompactLeistungsfaktorHeat.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numHithermCompactLeistungsfaktorCool_ValueChanged(object sender, EventArgs e) {
			HithermCompactProduct.ConfigLeistungsFaktorKuehlen = (double)numHithermCompactLeistungsfaktorCool.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void cmbConstruction_SelectedIndexChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigModulCeilingConstruction = (int)cmbModulDeckeConstruction.SelectedItem;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbEcothermHarreitherNorm_CheckedChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigUseHarreitherNorm = rbEcothermHarreitherNorm.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbEcothermEN1264_CheckedChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigUseHarreitherNorm = rbEcothermHarreitherNorm.Checked;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermCircuitLength_ValueChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigMaxCircuitLength = (double)numEcothermCircuitLength.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermPressurePa_ValueChanged(object sender, EventArgs e) {
			numEcothermPressureMbar.Value = numEcothermPressurePa.Value / 100;
			EcothermProduct.ConfigMaxPressureLost = (int)numEcothermPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermPressureMbar_ValueChanged(object sender, EventArgs e) {
			numEcothermPressurePa.Value = numEcothermPressureMbar.Value * 100;
			EcothermProduct.ConfigMaxPressureLost = (int)numEcothermPressurePa.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermDurchfluss_ValueChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigMaxDurchfluss = (int)numEcothermDurchfluss.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermSpreizungHeizMin_ValueChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigSpreizungHeizMin = (double)numEcothermSpreizungHeizMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermSpreizungHeizMax_ValueChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigSpreizungHeizMax = (double)numEcothermSpreizungHeizMax.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermSpreizungKühlMin_ValueChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigSpreizungKuehlMin = (double)numEcothermSpreizungKuehlMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermSpreizungKühlMax_ValueChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigSpreizungKuehlMax = (double)numEcothermSpreizungKuehlMax.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermMindestueberdeckung_ValueChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigSu0 = (double)numEcothermMindestueberdeckung.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEcothermEstrichueberdeckung_ValueChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigSu = (double)numEcothermEstrichueberdeckung.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numGeneralAlphaBodenHeat_ValueChanged(object sender, EventArgs e) {
			Product.ConfigAlphaBodenHeat = (double)numGeneralAlphaBodenHeat.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numGeneralAlphaDeckeHeat_ValueChanged(object sender, EventArgs e) {
			Product.ConfigAlphaDeckeHeat = (double)numGeneralAlphaDeckeHeat.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numGeneralAlphaWandHeat_ValueChanged(object sender, EventArgs e) {
			Product.ConfigAlphaWandHeat = (double)numGeneralAlphaWandHeat.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numGeneralAlphaBodenCool_ValueChanged(object sender, EventArgs e) {
			Product.ConfigAlphaBodenCool = (double)numGeneralAlphaBodenCool.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numGeneralAlphaDeckeCool_ValueChanged(object sender, EventArgs e) {
			Product.ConfigAlphaDeckeCool = (double)numGeneralAlphaDeckeCool.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numGeneralAlphaWandCool_ValueChanged(object sender, EventArgs e) {
			Product.ConfigAlphaWandCool = (double)numGeneralAlphaWandCool.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbHithermDefault_CheckedChanged(object sender, EventArgs e) {
			if (rbHithermDefault.Checked != rbHithermPlusDefault.Checked) {
				Configuration.UserTemplate.AddProductParameter<HithermProduct>("ConfigUsePlus", this.rbHithermPlusDefault.Checked);
				//HithermProduct.ConfigUsePlus = this.rbHithermPlusDefault.Checked;
			}
		}

		private void rbHithermCompactDefault_CheckedChanged(object sender, EventArgs e) {
			if (rbHithermCompactDefault.Checked != rbHithermCompactPlusDefault.Checked) {
				Configuration.UserTemplate.AddProductParameter<HithermCompactProduct>("ConfigUsePlus", this.rbHithermCompactPlusDefault.Checked);
				//HithermProduct.ConfigUsePlus = this.rbHithermPlusDefault.Checked;
			}
		}

		private void btnHithermStandardDefault_Click(object sender, EventArgs e) {
			bool usePlus = Configuration.AdminTemplate.GetProductParameterAsBool<HithermProduct>("ConfigUsePlus");
			Configuration.UserTemplate.AddProductParameter<HithermProduct>("ConfigUsePlus", usePlus);
			this.InitializeHithermValues();
		}

		private void btnHithermCompactStandardDefault_Click(object sender, EventArgs e) {
			bool usePlus = Configuration.AdminTemplate.GetProductParameterAsBool<HithermCompactProduct>("ConfigUsePlus");
			Configuration.UserTemplate.AddProductParameter<HithermCompactProduct>("ConfigUsePlus", usePlus);
			this.InitializeHithermCompactValues();
		}
	}
}
