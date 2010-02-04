using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class RequiredMaterialPanel : UserControl, IEditorUserControl {


		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		
		public RequiredMaterialPanel() {
			InitializeComponent();
			UpdateControl(true);
		}

		public void UpdateControl(bool resetUserInterface) {
			Project.Instance.CalculateRequiredMaterial();
			requiredMaterialGridFloor.UpdateControl(true);
			requiredMaterialGridWall.UpdateControl(true);
			requiredMaterialGridCeiling.UpdateControl(true);
			requiredMaterialGridDistributor.UpdateControl(true);
			requiredMaterialGridInsulation.UpdateControl(true);
			requiredMaterialGridGeneral.UpdateControl(true);
		}

		public bool AllowLeave() {
			return true;
		}

		private void requiredMaterialGrid_GridContentChanged(object sender) {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

	}
}
