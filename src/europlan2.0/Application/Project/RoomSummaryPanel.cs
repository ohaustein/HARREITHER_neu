using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class RoomSummaryPanel : UserControl, IEditorUserControl {
		
		public event ProjectStructureChangedHandler ProjectStructureChanged;
		
		public RoomSummaryPanel() {
			InitializeComponent();
		}


		public void UpdateControl() {
			
		}

		public bool AllowLeave() {
			return true;
		}


	}
}
