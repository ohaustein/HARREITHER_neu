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
		}

		public void UpdateControl() {
			InitializeEurovalValues();
		}

		public bool AllowLeave() {
			return true;
		}

		private void btnEurovalStandard_Click(object sender, EventArgs e) {
			Project.Instance.Config.EurovalProduct.StaticInitialize();
			Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigUseHarreitherNorm");
			Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigMaxCircuitLength");
			Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigMaxPressureLost");
			Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigMaxDurchfluss");
			Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigSpreizungHeizMin");
			Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigSpreizungHeizMax");
			Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigSpreizungKühlMin");
			Project.Instance.Config.RemoveProductParameter<EurovalProduct>("ConfigSpreizungKühlMax");
			InitializeEurovalValues();
		}


		private void btnModulBodenStandard_Click(object sender, EventArgs e) {
			Project.Instance.Config.ModulKlimaBodenProduct.StaticInitialize();
			Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigUseHarreitherNorm");
			Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigMaxPressureLost");
			Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigMaxDurchfluss");
			Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigMaxModulesInRow");
			Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigMaxModulesInParallel");
			Project.Instance.Config.RemoveProductParameter<ModulKlimaBodenProduct>("ConfigModulesInCircuit");
			InitializeModulBodenValues();
		}

		private void InitializeEurovalValues() {
			rbEurovalHarreitherNorm.Checked = EurovalProduct.ConfigUseHarreitherNorm;
			rbEurovalEN1264.Checked = !EurovalProduct.ConfigUseHarreitherNorm;
			numCircuitLength.Value = (decimal)EurovalProduct.ConfigMaxCircuitLength;
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
			numModulBodenMaxModuleInRow.Value = ModulKlimaBodenProduct.ConfigMaxModulesInRow;
			numModulBodenMaxModulesInParallel.Value = ModulKlimaBodenProduct.ConfigMaxModulesInParallel;
			numModulBodenMaxModulesInCircuit.Value = ModulKlimaBodenProduct.ConfigModulesInCircuit;
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
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigUseHarreitherNorm", rbEurovalHarreitherNorm.Checked.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numCircuitLength_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigMaxCircuitLength = (double)numCircuitLength.Value;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxCircuitLength", numCircuitLength.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalDurchfluss_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigMaxDurchfluss = (int)numEurovalDurchfluss.Value;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxDurchfluss", numEurovalDurchfluss.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalPressurePa_ValueChanged(object sender, EventArgs e) {
			numEurovalPressureMbar.Value = numEurovalPressurePa.Value / 100;
			EurovalProduct.ConfigMaxPressureLost = (int)numEurovalPressurePa.Value;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxPressureLost", numEurovalPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numEurovalPressureMbar_ValueChanged(object sender, EventArgs e) {
			numEurovalPressurePa.Value = numEurovalPressureMbar.Value * 100;
			EurovalProduct.ConfigMaxPressureLost = (int)numEurovalPressurePa.Value;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxPressureLost", numEurovalPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungHeizMin_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungHeizMin = (double)numSpreizungHeizMin.Value;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigSpreizungHeizMin", numSpreizungHeizMin.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungHeizMax_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungHeizMax = (double)numSpreizungHeizMax.Value;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigSpreizungHeizMax", numSpreizungHeizMax.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungKühlMin_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungKühlMin = (double)numSpreizungKühlMin.Value;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigSpreizungKühlMin", numSpreizungKühlMin.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungKühlMax_ValueChanged(object sender, EventArgs e) {
			EurovalProduct.ConfigSpreizungKühlMax = (double)numSpreizungKühlMax.Value;
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigSpreizungKühlMax", numSpreizungKühlMax.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void tabSystemParameters_Selected(object sender, TabControlEventArgs e) {
			if (tabSystemParameters.SelectedTab != tabEuroval && tabSystemParameters.SelectedTab != tabModulBoden) {
				MessageBox.Show("Die Konfiguration von Systemparameter ist für dieses System derzeit nicht möglich.");
				tabSystemParameters.SelectedTab = tabEuroval;
			}
		}

		private void rbModulBodenHarreitherNorm_CheckedChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigUseHarreitherNorm = rbModulBodenHarreitherNorm.Checked;
			Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigUseHarreitherNorm", rbModulBodenHarreitherNorm.Checked.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbModulBodenEN1264_CheckedChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigUseHarreitherNorm = rbModulBodenHarreitherNorm.Checked;
			Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigUseHarreitherNorm", rbModulBodenHarreitherNorm.Checked.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenDurchfluss_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigMaxDurchfluss = (int)numModulBodenDurchfluss.Value;
			Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxDurchfluss", numModulBodenDurchfluss.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}
		
		private void numModulBodenPressurePa_ValueChanged(object sender, EventArgs e) {
			numModulBodenPressureMbar.Value = numModulBodenPressurePa.Value / 100;
			ModulKlimaBodenProduct.ConfigMaxPressureLost = (int)numModulBodenPressurePa.Value;
			Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxPressureLost", numModulBodenPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}
		
		private void numModulBodenPressureMbar_ValueChanged(object sender, EventArgs e) {
			numModulBodenPressurePa.Value = numModulBodenPressureMbar.Value * 100;
			ModulKlimaBodenProduct.ConfigMaxPressureLost = (int)numModulBodenPressurePa.Value;
			Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxPressureLost", numModulBodenPressurePa.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenMaxModuleInRow_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigMaxModulesInRow = (int)numModulBodenMaxModuleInRow.Value;
			Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxModulesInRow", numModulBodenMaxModuleInRow.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenMaxModulesInParallel_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigMaxModulesInParallel = (int)numModulBodenMaxModulesInParallel.Value;
			Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigMaxModulesInParallel", numModulBodenMaxModulesInParallel.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numModulBodenMaxModulesInCircuit_ValueChanged(object sender, EventArgs e) {
			ModulKlimaBodenProduct.ConfigModulesInCircuit = (int)numModulBodenMaxModulesInCircuit.Value;
			Project.Instance.Config.AddProductParameter<ModulKlimaBodenProduct>("ConfigModulesInCircuit", numModulBodenMaxModulesInCircuit.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}



	}
}
