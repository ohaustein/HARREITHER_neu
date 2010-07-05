using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class EmptyEditorUserControl : UserControl, IEditorUserControl {
		public EmptyEditorUserControl() {
			InitializeComponent();
		}

		#region IEditorUserControl Members

		public event ProjectStructureChangedHandler ProjectStructureChanged;

		public event ProjectChangedHandler ProjectChanged;

		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		public void UpdateControl(bool resetUserInterface) {
		}

		public bool AllowLeave() {
			return true;
		}

		#endregion
	}
}
