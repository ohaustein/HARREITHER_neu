using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public partial class HardwareIdBox : UserControl {

		private HardwareId value = new HardwareId("000000-000000-000000");

		public event EventHandler ValueChanged;

		public HardwareIdBox() {
			InitializeComponent();
		}

		protected override void OnResize(EventArgs e) {
			this.Height = this.txtHardwareId.Height;
			base.OnResize(e);
		}

		private void txtHardwareId_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Delete) {
				//if (this.txtHardwareId.SelectionLength == 0) {
					int sel = this.txtHardwareId.SelectionStart;
					if (sel < 20) {
						this.VirtualKeyPress('0', this.txtHardwareId.SelectionStart);
					}
				//} else {
				//	int end = this.txtHardwareId.SelectionStart + this.txtHardwareId.SelectionLength;
				//	while (this.txtHardwareId.SelectionStart < end) {
				//		this.VirtualKeyPress('0', this.txtHardwareId.SelectionStart);
				//	}
				//}
				e.Handled = true;
				e.SuppressKeyPress = true;
				return;
			} else if (e.KeyCode == Keys.Back) {
				int sel = this.txtHardwareId.SelectionStart + this.txtHardwareId.SelectionLength;
				if (sel > 0) {
					this.VirtualKeyPress('0', sel - 1);
					this.txtHardwareId.SelectionStart = sel - 1;
					this.txtHardwareId.SelectionLength = 0;
				}
				e.Handled = true;
				e.SuppressKeyPress = true;
				return;
			} else if (e.Control && e.KeyCode == Keys.C) {
				Clipboard.SetText(this.txtHardwareId.SelectedText);
				e.Handled = true;
				e.SuppressKeyPress = true;
				return;
			} else if (e.Control && e.KeyCode == Keys.V) {
				if (Clipboard.ContainsText()) {
					string text = Clipboard.GetText();
					while (txtHardwareId.SelectionStart < 20 && text.Length > 0) {
						if (HardwareId.hardwareIdKeyChars.IndexOf(text[0]) >= 0) {
							this.VirtualKeyPress(text[0], txtHardwareId.SelectionStart);
						}
						text = text.Substring(1);
					}
				}
				e.Handled = true;
				e.SuppressKeyPress = true;
				return;
			} else if (e.Control && e.KeyCode == Keys.A) {
				this.txtHardwareId.SelectionStart = 0;
				this.txtHardwareId.SelectionLength = this.txtHardwareId.Text.Length;
				e.Handled = true;
				e.SuppressKeyPress = true;
				return;
			} else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Home || e.KeyCode == Keys.End) {
				e.Handled = false;
				e.SuppressKeyPress = false;
				return;
			}
		}

		private bool VirtualKeyPress(char keyChar, int pos) {
			if (pos % 7 == 6) {
				pos++;
			}
			txtHardwareId.Text = txtHardwareId.Text.Substring(0, pos) + keyChar + txtHardwareId.Text.Substring(pos + 1);
			txtHardwareId.SelectionStart = pos + 1;
			txtHardwareId.SelectionLength = 0;
			this.OnValueChanged(EventArgs.Empty);
			return true;
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HardwareId Value {
			get { return new HardwareId(this.txtHardwareId.Text); }
			set {
				if (value == null) {
					this.value = new HardwareId("000000-000000-000000");
				} else {
					this.value = value;
				}
				this.txtHardwareId.Text = this.value.IdString;
			}
		}

		protected virtual void OnValueChanged(EventArgs args) {
			if (this.ValueChanged != null) {
				this.ValueChanged(this, args);
			}
		}

		private void txtHardwareId_KeyPress(object sender, KeyPressEventArgs e) {
			char keyChar = e.KeyChar;
			keyChar = char.ToLower(keyChar);
			int selStart = txtHardwareId.SelectionStart;

			if (selStart > 19) {
				e.Handled = true;
				return;
			}
			e.Handled = this.VirtualKeyPress(keyChar, selStart);
			return;
		}
	}
}
