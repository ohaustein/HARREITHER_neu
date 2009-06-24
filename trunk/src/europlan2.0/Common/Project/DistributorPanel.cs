using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class DistributorPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Distributor distributor;

		public DistributorPanel() {
			InitializeComponent();
		}

		public void UpdateControl() {
			if (this.Tag != null) {
				this.distributor = this.Tag as Distributor;
			//    this.room = this.Tag as Room;
			//    this.txtName.Text = room.Name;
			//    this.txtArea.Value = (decimal)room.Area;
			//    this.txtTemperature.Text = room.RoomTemperature.ToString();
			//    this.txtHeat.Text = room.HeatLoad.ToString();
			//    this.txtNormHeat.Text = room.NormalizedHeatLoad.ToString();
			//    this.txtCool.Text = room.CoolLoad.ToString();
			//    this.txtNormCool.Text = room.NormalizedCoolLoad.ToString();
			}
		}

		public bool AllowLeave() {
			return true;
		}

	}
}
