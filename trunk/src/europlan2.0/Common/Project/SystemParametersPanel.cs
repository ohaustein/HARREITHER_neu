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
		}

		public void UpdateControl() {
			InitializeEurovalValues();
		}

		public bool AllowLeave() {
			return true;
		}

		private void btnEurovalStandard_Click(object sender, EventArgs e) {
			EurovalProduct.ConfigUseHarreitherNorm = true;
			EurovalProduct.ConfigMaxCircuitLength = 100;
			EurovalProduct.ConfigMaxPressureLost = 15000;
			EurovalProduct.ConfigMaxDurchfluss = 240;
			EurovalProduct.ConfigSpreizungHeizMin = 4;
			EurovalProduct.ConfigSpreizungHeizMax = 12;
			EurovalProduct.ConfigSpreizungKühlMin = 2;
			EurovalProduct.ConfigSpreizungKühlMax = 6;
			InitializeEurovalValues();
		}

		private void InitializeEurovalValues() {
			rbHarreitherNorm.Checked = EurovalProduct.ConfigUseHarreitherNorm;
			rbEN1264.Checked = !EurovalProduct.ConfigUseHarreitherNorm;
			numCircuitLength.Value = (decimal)EurovalProduct.ConfigMaxCircuitLength;
			numPressurePa.Value = EurovalProduct.ConfigMaxPressureLost / 100;
			numDurchfluss.Value = EurovalProduct.ConfigMaxDurchfluss;
			numSpreizungHeizMin.Value = (decimal)EurovalProduct.ConfigSpreizungHeizMin;
			numSpreizungHeizMax.Value = (decimal)EurovalProduct.ConfigSpreizungHeizMax;
			numSpreizungKühlMin.Value = (decimal)EurovalProduct.ConfigSpreizungKühlMin;
			numSpreizungKühlMax.Value = (decimal)EurovalProduct.ConfigSpreizungKühlMax;
		}

		private void rbHarreitherNorm_CheckedChanged(object sender, EventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void rbEN1264_CheckedChanged(object sender, EventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numCircuitLength_ValueChanged(object sender, EventArgs e) {
			Project.Instance.Config.AddProductParameter<EurovalProduct>("ConfigMaxCircuitLength", numCircuitLength.Value.ToString());
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numDurchfluss_ValueChanged(object sender, EventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numPressurePa_ValueChanged(object sender, EventArgs e) {
			numPressureMbar.Value = numPressurePa.Value / 100;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numPressureMbar_ValueChanged(object sender, EventArgs e) {
			numPressurePa.Value = numPressureMbar.Value * 100;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungHeizMin_ValueChanged(object sender, EventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungHeizMax_ValueChanged(object sender, EventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungKühlMin_ValueChanged(object sender, EventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void numSpreizungKühlMax_ValueChanged(object sender, EventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

	}
}
