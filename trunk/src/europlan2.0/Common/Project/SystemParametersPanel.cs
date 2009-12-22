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
			InitializeEurovalValues();
			InitializeModulBodenValues();
			InitializeModulDeckeValues();
			InitializeHithermValues();
		}

		public void UpdateControl() {
			InitializeEurovalValues();
		}

		public bool AllowLeave() {
			return true;
		}

		private void btnEurovalStandard_Click(object sender, EventArgs e) {
			Project.Instance.Config.EurovalProduct.StaticInitialize();
			//Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigUseHarreitherNorm");
			//Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigMaxCircuitLength");
			//Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigMaxPressureLost");
			//Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigMaxDurchfluss");
			//Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigSpreizungHeizMin");
			//Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigSpreizungHeizMax");
			//Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigSpreizungKühlMin");
			//Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigSpreizungKühlMax");
			InitializeEurovalValues();
		}


		private void btnModulBodenStandard_Click(object sender, EventArgs e) {
			Project.Instance.Config.ModulKlimaBodenProduct.StaticInitialize();
			//Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigUseHarreitherNorm");
			//Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigMaxPressureLost");
			//Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigMaxDurchfluss");
			//Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigMaxModulesInRow");
			//Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigMaxModulesInParallel");
			//Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigModulesInCircuit");
			InitializeModulBodenValues();
		}

		private void btnModulDeckeStandard_Click(object sender, EventArgs e) {
			Project.Instance.Config.ModulKlimaDeckeProduct.StaticInitialize();
			InitializeModulDeckeValues();
		}

		private void btnHithermStandard_Click(object sender, EventArgs e) {
			Project.Instance.Config.HithermProduct.StaticInitialize();
			InitializeHithermValues();
		}

		private void InitializeEurovalValues() {
			rbEurovalHarreitherNorm.Checked = EurovalProduct.ConfigUseHarreitherNorm;
			rbEurovalEN1264.Checked = !EurovalProduct.ConfigUseHarreitherNorm;
			numEurovalCircuitLength.Value = (decimal)EurovalProduct.ConfigMaxCircuitLength;
			numEurovalPressurePa.Value = EurovalProduct.ConfigMaxPressureLost;
			numEurovalDurchfluss.Value = EurovalProduct.ConfigMaxDurchfluss;
			numSpreizungHeizMin.Value = (decimal)EurovalProduct.ConfigSpreizungHeizMin;
			numSpreizungHeizMax.Value = (decimal)EurovalProduct.ConfigSpreizungHeizMax;
			numSpreizungKühlMin.Value = (decimal)EurovalProduct.ConfigSpreizungKühlMin;
			numSpreizungKühlMax.Value = (decimal)EurovalProduct.ConfigSpreizungKühlMax;
		}

		private void InitializeModulBodenValues() {
			rbModulBodenHarreitherNorm.Checked = ModulKlimaBodenProduct.ConfigUseHarreitherNorm;
			rbModulBodenEN1264.Checked = !ModulKlimaBodenProduct.ConfigUseHarreitherNorm;
			numModulBodenPressurePa.Value = ModulKlimaBodenProduct.ConfigMaxPressureLost;
			numModulBodenDurchfluss.Value = ModulKlimaBodenProduct.ConfigMaxDurchfluss;
			numModulBodenMaxModulesInCircuit.Value = ModulKlimaBodenProduct.ConfigModulesInCircuit;
			numModulBodenSpreizungHeizMin.Value = (decimal)ModulKlimaBodenProduct.ConfigSpreizungHeizMin;
			numModulBodenSpreizungHeizMax.Value = (decimal)ModulKlimaBodenProduct.ConfigSpreizungHeizMax;
			numModulBodenSpreizungKuehlMin.Value = (decimal)ModulKlimaBodenProduct.ConfigSpreizungKühlMin;
			numModulBodenSpreizungKuehlMax.Value = (decimal)ModulKlimaBodenProduct.ConfigSpreizungKühlMax;
		}

		private void InitializeModulDeckeValues() {
			cmbConstruction.Items.Clear();
			foreach (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum item in Enum.GetValues(typeof(ModulKlimaDeckeProduct.ModulCeilingConstructionEnum))) {
				this.cmbConstruction.Items.Add(item);
			}
			numModulDeckePressurePa.Value = ModulKlimaDeckeProduct.ConfigMaxPressureLost;
			numModulDeckeDurchfluss.Value = ModulKlimaDeckeProduct.ConfigMaxDurchfluss;
			numModulDeckeMaxModulesInRow.Value = ModulKlimaDeckeProduct.ConfigMaxModulesInRow;
			numModulDeckeMaxRows.Value = ModulKlimaDeckeProduct.ConfigMaxModulesInParallel;
			numModulDeckeMaxModulesInCircuit.Value = ModulKlimaDeckeProduct.ConfigModulesInCircuit;
			numModulDeckeLeistungsfaktor.Value = (decimal)ModulKlimaDeckeProduct.ConfigLeistungsFaktor;
			numModulDeckeSpreizungHeizMin.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungHeizMin;
			numModulDeckeSpreizungHeizMax.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungHeizMax;
			numModulDeckeSpreizungKuehlMin.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungKühlMin;
			numModulDeckeSpreizungKuehlMax.Value = (decimal)ModulKlimaDeckeProduct.ConfigSpreizungKühlMax;
			cmbConstruction.SelectedItem = (ModulKlimaDeckeProduct.ModulCeilingConstructionEnum)ModulKlimaDeckeProduct.ConfigModulCeilingConstruction;
		}

		private void InitializeHithermValues() {
			numHithermPressurePa.Value = HithermProduct.ConfigMaxPressureLost;
			numHithermDurchfluss.Value = HithermProduct.ConfigMaxDurchfluss;
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
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigUseHarreitherNorm", rbEurovalHarreitherNorm.Checked.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalCircuitLength_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigMaxCircuitLength = (double)numEurovalCircuitLength.Value;
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxCircuitLength", numCircuitLength.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalDurchfluss_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigMaxDurchfluss = (int)numEurovalDurchfluss.Value;
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxDurchfluss", numEurovalDurchfluss.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalPressurePa_ValueChanged(object sender, EventArgs e) {
			numEurovalPressureMbar.Value = numEurovalPressurePa.Value / 100;
			EurovalProduct.ConfigMaxPressureLost = (int)numEurovalPressurePa.Value;
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxPressureLost", numEurovalPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalPressureMbar_ValueChanged(object sender, EventArgs e) {
			numEurovalPressurePa.Value = numEurovalPressureMbar.Value * 100;
			EurovalProduct.ConfigMaxPressureLost = (int)numEurovalPressurePa.Value;
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxPressureLost", numEurovalPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungHeizMin_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungHeizMin = (double)numSpreizungHeizMin.Value;
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigSpreizungHeizMin", numSpreizungHeizMin.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungHeizMax_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungHeizMax = (double)numSpreizungHeizMax.Value;
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigSpreizungHeizMax", numSpreizungHeizMax.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungKühlMin_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungKühlMin = (double)numSpreizungKühlMin.Value;
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigSpreizungKühlMin", numSpreizungKühlMin.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungKühlMax_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungKühlMax = (double)numSpreizungKühlMax.Value;
			//Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigSpreizungKühlMax", numSpreizungKühlMax.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void tabSystemParameters_Selected(object sender, TabControlEventArgs e) {
			/*if (tabSystemParameters.SelectedTab != tabEuroval && tabSystemParameters.SelectedTab != tabModulBoden && tabSystemParameters.SelectedTab != tabModulDecke) {
				MessageBox.Show("Die Konfiguration von Systemparameter ist für dieses System derzeit nicht möglich.");
				tabSystemParameters.SelectedTab = tabEuroval;
			}*/
		}

		private void rbModulBodenHarreitherNorm_CheckedChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigUseHarreitherNorm = rbModulBodenHarreitherNorm.Checked;
			//Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigUseHarreitherNorm", rbModulBodenHarreitherNorm.Checked.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbModulBodenEN1264_CheckedChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigUseHarreitherNorm = rbModulBodenHarreitherNorm.Checked;
			//Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigUseHarreitherNorm", rbModulBodenHarreitherNorm.Checked.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenDurchfluss_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigMaxDurchfluss = (int)numModulBodenDurchfluss.Value;
			//Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxDurchfluss", numModulBodenDurchfluss.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}
		
		private void numModulBodenPressurePa_ValueChanged(object sender, EventArgs e) {
			numModulBodenPressureMbar.Value = numModulBodenPressurePa.Value / 100;
			ModulKlimaBodenProduct.ConfigMaxPressureLost = (int)numModulBodenPressurePa.Value;
			//Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxPressureLost", numModulBodenPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}
		
		private void numModulBodenPressureMbar_ValueChanged(object sender, EventArgs e) {
			numModulBodenPressurePa.Value = numModulBodenPressureMbar.Value * 100;
			ModulKlimaBodenProduct.ConfigMaxPressureLost = (int)numModulBodenPressurePa.Value;
			//Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxPressureLost", numModulBodenPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenMaxModulesInCircuit_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigModulesInCircuit = (int)numModulBodenMaxModulesInCircuit.Value;
			//Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigModulesInCircuit", numModulBodenMaxModulesInCircuit.Value.ToString());
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
			//Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxPressureLost", numModulBodenPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckePressureMbar_ValueChanged(object sender, EventArgs e) {
			numModulDeckePressurePa.Value = numModulDeckePressureMbar.Value * 100;
			ModulKlimaDeckeProduct.ConfigMaxPressureLost = (int)numModulDeckePressurePa.Value;
			//Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxPressureLost", numModulBodenPressurePa.Value.ToString());
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
			ModulKlimaBodenProduct.ConfigSpreizungKühlMin = (double)numModulBodenSpreizungKuehlMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenSpreizungKuehlMax_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigSpreizungKühlMax = (double)numModulBodenSpreizungKuehlMax.Value;
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
			ModulKlimaDeckeProduct.ConfigSpreizungKühlMin = (double)numModulDeckeSpreizungKuehlMin.Value;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulDeckeSpreizungKuehlMax_ValueChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigSpreizungKühlMax = (double)numModulDeckeSpreizungKuehlMax.Value;
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

		private void cmbConstruction_SelectedIndexChanged(object sender, EventArgs e) {
			ModulKlimaDeckeProduct.ConfigModulCeilingConstruction = (int)cmbConstruction.SelectedItem;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numCircuitLength_ValueChanged(object sender, EventArgs e) {

		}

	}
}
