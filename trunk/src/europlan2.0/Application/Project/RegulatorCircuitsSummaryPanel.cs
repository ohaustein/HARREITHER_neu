using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class RegulatorCircuitsSummaryPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;
		
		public RegulatorCircuitsSummaryPanel() {
			InitializeComponent();
		}

		public void UpdateControl() {

		}

		public bool AllowLeave() {
			return true;
		}

	}
}
