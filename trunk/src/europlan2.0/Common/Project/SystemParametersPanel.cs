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
		
		}

		public bool AllowLeave() {
			return true;
		}

		private void btnEurovalStandard_Click(object sender, EventArgs e) {
			InitializeEurovalValues();
		}

		private void InitializeEurovalValues() {
			rbHarreitherNorm.Checked = true;
			numCircuitLength.Value = 100;
			numPressurePa.Value = 15000;
			numDurchfluss.Value = 240;
			numSpreizungHeizMin.Value = 4;
			numSpreizungHeizMax.Value = 12;
			numSpreizungKühlMin.Value = 2;
			numSpreizungKühlMax.Value = 6;
		}

		private void numCircuitLength_ValueChanged(object sender, EventArgs e) {
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
