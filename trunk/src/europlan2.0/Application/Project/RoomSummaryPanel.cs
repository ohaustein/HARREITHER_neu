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

		private Room room;
		
		public RoomSummaryPanel() {
			InitializeComponent();
			room = null;
		}


		public void UpdateControl() {
			if (this.Tag != null) {
				this.room = this.Tag as Room;
				this.txtName.Text = room.Name;
				this.txtArea.Text = room.Area.ToString();
				this.txtTemperature.Text = room.Temperature.ToString();
				this.txtHeat.Text = room.HeatPower.ToString();
				this.txtNormHeat.Text = room.NormalizedHeatPower.ToString();
				this.txtCool.Text = room.CoolPower.ToString();
				this.txtNormCool.Text = room.NormalizedCoolPower.ToString();
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
				this.room.Area = float.Parse(this.txtArea.Text);
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception ex) {
				MessageBox.Show("");
				this.txtArea.Text = room.Area.ToString();
			}
		}

		private void txtTemperature_TextChanged(object sender, EventArgs e) {
			try {
				this.room.Temperature = Int32.Parse(this.txtTemperature.Text);
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception ex) {
				MessageBox.Show("");
				this.txtArea.Text = room.Area.ToString();
			}
		}

		private void txtHeat_TextChanged(object sender, EventArgs e) {
			try {
				this.room.HeatPower = Int32.Parse(this.txtHeat.Text);
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception ex) {
				MessageBox.Show("");
				this.txtArea.Text = room.Area.ToString();
			}
		}

		private void txtNormHeat_TextChanged(object sender, EventArgs e) {
			try {
				this.room.NormalizedHeatPower = Int32.Parse(this.txtNormHeat.Text);
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception ex) {
				MessageBox.Show("");
				this.txtArea.Text = room.Area.ToString();
			}
		}

		private void txtCool_TextChanged(object sender, EventArgs e) {
			try {
				this.room.CoolPower = Int32.Parse(this.txtCool.Text);
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception ex) {
				MessageBox.Show("");
				this.txtArea.Text = room.Area.ToString();
			}
		}

		private void txtNormCool_TextChanged(object sender, EventArgs e) {
			try {
				this.room.NormalizedCoolPower = Int32.Parse(this.txtNormCool.Text);
				if (ProjectChanged != null) {
					ProjectChanged(null);
				}
			} catch (Exception ex) {
				MessageBox.Show("");
				this.txtArea.Text = room.Area.ToString();
			}
		}


	}
}
