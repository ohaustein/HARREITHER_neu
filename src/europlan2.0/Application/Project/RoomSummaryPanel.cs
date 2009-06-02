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
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Room room;
		
		public RoomSummaryPanel() {
			InitializeComponent();
			room = null;
		}


		public void UpdateControl() {
			if (this.Tag != null) {
				this.room = this.Tag as Room;
				this.txtName.Text = room.Name;
				this.txtArea.Value = (decimal)room.Area;
				this.txtTemperature.Text = room.RoomTemperature.ToString();
				this.txtHeat.Text = room.HeatLoad.ToString();
				this.txtNormHeat.Text = room.NormalizedHeatLoad.ToString();
				this.txtCool.Text = room.CoolLoad.ToString();
				this.txtNormCool.Text = room.NormalizedCoolLoad.ToString();
			}
		}

		public bool AllowLeave() {
			return true;
		}

		private void txtName_TextChanged(object sender, EventArgs e) {
			this.room.Name = this.txtName.Text;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void txtArea_TextChanged(object sender, EventArgs e) {
			try {
				this.room.Area = (float)this.txtArea.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtTemperature.Text = room.RoomTemperature.ToString();
			}
		}

		private void txtTemperature_TextChanged(object sender, EventArgs e) {
			try {
				this.room.RoomTemperature = (int)this.txtTemperature.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtTemperature.Text = room.RoomTemperature.ToString();
			}
		}

		private void txtHeat_TextChanged(object sender, EventArgs e) {
			try {
				this.room.HeatLoad = (int)this.txtHeat.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtHeat.Text = room.HeatLoad.ToString();
			}
		}

		private void txtNormHeat_TextChanged(object sender, EventArgs e) {
			try {
				this.room.NormalizedHeatLoad = (int)this.txtNormHeat.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtNormHeat.Text = room.NormalizedHeatLoad.ToString();
			}
		}

		private void txtCool_TextChanged(object sender, EventArgs e) {
			try {
				this.room.CoolLoad = (int)this.txtCool.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtCool.Text = room.CoolLoad.ToString();
			}
		}

		private void txtNormCool_TextChanged(object sender, EventArgs e) {
			try {
				this.room.NormalizedCoolLoad = (int)this.txtNormCool.Value;
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtNormCool.Text = room.NormalizedCoolLoad.ToString();
			}
		}


	}
}
