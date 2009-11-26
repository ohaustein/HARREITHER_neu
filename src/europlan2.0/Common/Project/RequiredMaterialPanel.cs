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
			UpdateControl();
		}
		
		public void UpdateControl() {
			requiredMaterialGridFloor.UpdateControl();
			requiredMaterialGridWall.UpdateControl();
			requiredMaterialGridCeiling.UpdateControl();
			requiredMaterialGridDistributor.UpdateControl();
			requiredMaterialGridInsulation.UpdateControl();
			requiredMaterialGridGeneral.UpdateControl();
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
