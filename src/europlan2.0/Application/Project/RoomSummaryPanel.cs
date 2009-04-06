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
				if (this.txtArea.Text.Length > 0) {
					string replacedText = this.txtArea.Text.Replace('.', ',');
					this.room.Area = float.Parse(replacedText);
					if (ProjectChanged != null) {
						ProjectChanged(null);
					}
					if (replacedText != this.txtArea.Text) {
						this.txtArea.Text = replacedText;
						this.txtArea.SelectionStart = this.txtArea.Text.Length;
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtArea.Text = room.Area.ToString();
			}
		}

		private void txtTemperature_TextChanged(object sender, EventArgs e) {
			try {
				if (this.txtTemperature.Text.Length > 0) {
					this.room.Temperature = Int32.Parse(this.txtTemperature.Text);
					if (ProjectChanged != null) {
						ProjectChanged(null);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtTemperature.Text = room.Temperature.ToString();
			}
		}

		private void txtHeat_TextChanged(object sender, EventArgs e) {
			try {
				if (this.txtHeat.Text.Length > 0) {
					this.room.HeatPower = Int32.Parse(this.txtHeat.Text);
					if (ProjectChanged != null) {
						ProjectChanged(null);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtHeat.Text = room.HeatPower.ToString();
			}
		}

		private void txtNormHeat_TextChanged(object sender, EventArgs e) {
			try {
				if (this.txtNormHeat.Text.Length > 0) {
					this.room.NormalizedHeatPower = Int32.Parse(this.txtNormHeat.Text);
					if (ProjectChanged != null) {
						ProjectChanged(null);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtNormHeat.Text = room.NormalizedHeatPower.ToString();
			}
		}

		private void txtCool_TextChanged(object sender, EventArgs e) {
			try {
				if (this.txtCool.Text.Length > 0) {
					this.room.CoolPower = Int32.Parse(this.txtCool.Text);
					if (ProjectChanged != null) {
						ProjectChanged(null);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtCool.Text = room.CoolPower.ToString();
			}
		}

		private void txtNormCool_TextChanged(object sender, EventArgs e) {
			try {
				if (this.txtNormCool.Text.Length > 0) {
					this.room.NormalizedCoolPower = Int32.Parse(this.txtNormCool.Text);
					if (ProjectChanged != null) {
						ProjectChanged(null);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Fehler im Format der Eingabe");
				this.txtNormCool.Text = room.NormalizedCoolPower.ToString();
			}
		}


	}
}
