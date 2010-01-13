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

		
		public SystemParametersPanel() {
			InitializeComponent();

			InitializeEurovalPanel();
			InitializeEcothermPanel();
			InitializeHithermPanel();
			InitializeHithermCompactPanel();
			InitializeModulBodenPanel();
			InitializeModulDeckePanel();
			InitializeGeneralPanel();

			InitializeEurovalValues();
			InitializeEcothermValues();
			InitializeModulBodenValues();
			InitializeModulDeckeValues();
			InitializeHithermValues();
			InitializeHithermCompactValues();
			InitializeGeneralValues();
		}

		public void UpdateControl() {
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
			EurovalProduct.StaticInitialize();
			InitializeEurovalValues();
		}

		private void btnEcothermStandard_Click(object sender, EventArgs e) {
			InitializeEcothermValues();
		}

		private void btnModulBodenStandard_Click(object sender, EventArgs e) {
			ModulKlimaBodenProduct.StaticInitialize();
			InitializeModulBodenValues();
		}

		private void btnModulDeckeStandard_Click(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.StaticInitialize();
			InitializeModulDeckeValues();
		}

		private void btnHithermStandard_Click(object sender, EventArgs e) {
			HithermProduct.StaticInitialize();
			InitializeHithermValues();
		}

		private void btnHithermCompactStandard_Click(object sender, EventArgs e) {
			HithermCompactProduct.StaticInitialize();
			InitializeHithermCompactValues();
		}

		private void btnGeneralStandard_Click(object sender, EventArgs e) {
			Product.StaticInitialize();
			InitializeGeneralValues();
		}

		private void InitializeEurovalPanel() {
			Licensing.License license = Licensing.LicenseManager.Instance.License;
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEuroval)) {
				this.tabSystemParameters.TabPages.Remove(this.tabEuroval);
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

		private void InitializeEcothermPanel() {
			Licensing.License license = Licensing.LicenseManager.Instance.License;
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEcotherm)) {
				this.tabSystemParameters.TabPages.Remove(this.tabEcotherm);
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

		private void InitializeHithermPanel() {
			Licensing.License license = Licensing.LicenseManager.Instance.License;
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHitherm)) {
				this.tabSystemParameters.TabPages.Remove(this.tabHitherm);
			}
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
				// nothing to do
			}
		}

		private void InitializeHithermCompactPanel() {
			Licensing.License license = Licensing.LicenseManager.Instance.License;
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHithermCompact)) {
				this.tabSystemParameters.TabPages.Remove(this.tabHithermCompact);
			}
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
				// nothing to do
			}
		}

		private void InitializeModulBodenPanel() {
			Licensing.License license = Licensing.LicenseManager.Instance.License;
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaBoden)) {
				this.tabSystemParameters.TabPages.Remove(this.tabModulBoden);
			}
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
				// nothing to do
			}
		}

		private void InitializeModulDeckePanel() {
			Licensing.License license = Licensing.LicenseManager.Instance.License;
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaDecke)) {
				this.tabSystemParameters.TabPages.Remove(this.tabModulDecke);
			}
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
				if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
					this.layoutModulDecke.Controls.Remove(this.lblModulDeckeLeistungsfaktor);
					this.layoutModulDecke.Controls.Remove(this.numModulDeckeLeistungsfaktor);
					this.layoutModulDecke.SetRow(this.btnModulDeckeStandard, this.layoutModulDecke.GetRow(this.btnModulDeckeStandard) - 1);
				}
			}
		}

		private void InitializeGeneralPanel() {
			Licensing.License license = Licensing.LicenseManager.Instance.License;
			if (!license.IsModuleEnabled(Licensing.AbstractLicensedModule.FeatAdmin)) {
				this.tabSystemParameters.TabPages.Remove(this.tabGeneral);
			}			
		}

		private void InitializeEurovalValues() {
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

		private void InitializeEcothermValues() {
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

		private void InitializeModulBodenValues() {
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

		private void InitializeModulDeckeValues() {
			cmbModulDeckeConstruction.Items.Clear();
			foreach (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum item in Enum.GetValues(typeof(ModulKlimaDeckeProduct.ModulCeilingConstructionEnum))) {
				this.cmbModulDeckeConstruction.Items.Add(item);
			}
			numModulDeckePressurePa.Value = ModulKlimaDeckeProduct.ConfigMaxPressureLost;
			numModulDeckeDurchfluss.Value = ModulKlimaDeckeProduct.ConfigMaxDurchfluss;
			numModulDeckeMaxModulesInRow.Value = ModulKlimaDeckeProduct.ConfigMaxModulesInRow;
			numModulDeckeMaxRows.Value = ModulKlimaDeckeProduct.ConfigMaxModulesInParallel;
			numModulDeckeMaxModulesInCircuit.Value = ModulKlimaDeckeProduct.ConfigModulesInCircuit;
			numModulDeckeLeistungsfaktor.Value = (decimal)ModulKlimaDeckeProduct.ConfigLeistungsFaktor;
			numModulDeckeSpreizungHeizMin.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungHeizMin;
			numModulDeckeSpreizungHeizMax.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungHeizMax;
			numModulDeckeSpreizungKuehlMin.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungKuehlMin;
			numModulDeckeSpreizungKuehlMax.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungKuehlMax;
			cmbModulDeckeConstruction.SelectedItem = (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum)ModulKlimaDeckeProduct.ConfigModulCeilingConstruction;
		}

		private void InitializeHithermValues() {
			rbHitherm.Checked = !HithermProduct.ConfigUsePlus;
			rbHithermPlus.Checked = HithermProduct.ConfigUsePlus;
			numHithermPressurePa.Value = HithermProduct.ConfigMaxPressureLost;
			numHithermDurchfluss.Value = HithermProduct.ConfigMaxDurchfluss;
		}

		private void InitializeHithermCompactValues() {
			rbHithermCompact.Checked = !HithermCompactProduct.ConfigUsePlus;
			rbHithermCompactPlus.Checked = HithermCompactProduct.ConfigUsePlus;
			numHithermCompactPressurePa.Value = HithermCompactProduct.ConfigMaxPressureLost;
			numHithermCompactDurchfluss.Value = HithermCompactProduct.ConfigMaxDurchfluss;
		}

		private void InitializeGeneralValues() {
			numGeneralAlphaBoden.Value = (decimal)Product.ConfigAlphaBoden;
			numGeneralAlphaDecke.Value = (decimal)Product.ConfigAlphaDecke;
			numGeneralAlphaWand.Value = (decimal)Product.ConfigAlphaWand;
		}

		private void rbEurovalHarreitherNorm_CheckedChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigUseHarreitherNorm = rbEurovalHarreitherNorm.Checked;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigUseHarreitherNorm", rbEurovalHarreitherNorm.Checked.ToString());
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

		private void numLeistungsfaktor_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigLeistungsFaktor = (double)numModulDeckeLeistungsfaktor.Value;
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
				HithermProduct.ConfigUsePlus = this.rbHithermPlus.Checked;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
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

		private void cmbConstruction_SelectedIndexChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigModulCeilingConstruction = (int)cmbModulDeckeConstruction.SelectedItem;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbEcothermHarreitherNorm_CheckedChanged(object sender, EventArgs e) {
			EcothermProduct.ConfigUseHarreitherNorm = rbEcothermHarreitherNorm.Checked;
			//Project.Instance.Config.AddProductParameter<EcothermProduct>("ConfigUseHarreitherNorm", rbEcothermHarreitherNorm.Checked.ToString());
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
	}
}
