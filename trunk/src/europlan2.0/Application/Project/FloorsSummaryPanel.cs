using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class FloorsSummaryPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		
		public FloorsSummaryPanel() {
			InitializeComponent();
		}

		private void btnImport_Click(object sender, EventArgs e) {
			BuildingDataImportManager.Instance.ImportBuildingData();
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(null);
			}
		}

		public void UpdateControl() {

		}

		public bool AllowLeave() {
			return true;
		}

	}
}
