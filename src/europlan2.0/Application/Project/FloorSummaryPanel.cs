using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class FloorSummaryPanel : UserControl, IEditorUserControl {
		
		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;

		private Floor floor;
		
		public FloorSummaryPanel() {
			InitializeComponent();
		}
		
		public void UpdateControl() {
			if (this.Tag != null) {
				this.floor = this.Tag as Floor;
				this.txtName.Text = floor.Name;
			}		
		}

		public bool AllowLeave() {
			return true;
		}

		private void txtName_TextChanged(object sender, EventArgs e) {
			this.floor.Name = this.txtName.Text;
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(this);
			}
		}

	}
}
