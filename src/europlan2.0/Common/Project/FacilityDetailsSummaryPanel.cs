using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class FacilityDetailsSummaryPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		public FacilityDetailsSummaryPanel() {
			InitializeComponent();
			UpdateControl();
		}

		public void UpdateControl() {
			Project project = Project.Instance;
		}

		public bool AllowLeave() {
			return true;
		}


	}
}
