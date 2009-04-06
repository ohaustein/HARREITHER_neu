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

		public EventHandler ValueChanged;

		public HardwareIdBox() {
			InitializeComponent();
		}

		protected override void OnResize(EventArgs e) {
			this.Height = this.txtHardwareId.Height;
			base.OnResize(e);
		}

		private void txtHardwareId_KeyDown(object sender, KeyEventArgs e) {
			char keyChar = (char)e.KeyValue;
			keyChar = char.ToLower(keyChar);
			if (e.KeyCode == Keys.Delete) {
				keyChar = '0';
			}
			if (e.Alt ||
				e.Control ||
				(HardwareId.hardwareIdKeyChars.IndexOf(keyChar) < 0 &&
				//keyChar != '-' &&
					e.KeyCode != Keys.Back)
				) {
				e.Handled = !(
					e.KeyCode == Keys.Left					// allow LEFT
					|| e.KeyCode == Keys.Right				// allow RIGHT
					//|| e.Control && e.KeyCode == Keys.A		// allow 'select all'
					|| e.Control && e.KeyCode == Keys.C		// allow 'copy'
					|| e.KeyCode == Keys.End				// allow end
					|| e.KeyCode == Keys.Home				// allow home
				);
				e.SuppressKeyPress = e.Handled;
				if (e.Control && e.KeyCode == Keys.V) {
					if (Clipboard.ContainsText()) {
						string text = Clipboard.GetText();
						while (txtHardwareId.SelectionStart < 20 && text.Length > 0) {
							if (HardwareId.hardwareIdKeyChars.IndexOf(text[0]) >= 0) {
								this.VirtualKeyPress(text[0], txtHardwareId.SelectionStart);
							}
							text = text.Substring(1);
						}
					}
				}
				return;
			}
			int selStart = txtHardwareId.SelectionStart;
			if (e.KeyCode == Keys.Back) {
				if (selStart != 0) {
					if (selStart % 7 == 0) {
						selStart--;
					}
					txtHardwareId.Text = txtHardwareId.Text.Substring(0, selStart - 1) + '0' + txtHardwareId.Text.Substring(selStart);
					txtHardwareId.SelectionStart = selStart - 1;
					txtHardwareId.SelectionLength = 0;
				}
				e.Handled = true;
				e.SuppressKeyPress = e.Handled;
				return;
			}
			if (selStart > 19) {
				e.Handled = true;
				e.SuppressKeyPress = e.Handled;
				return;
			}
			e.Handled = this.VirtualKeyPress(keyChar, selStart);
			e.SuppressKeyPress = e.Handled;
			return;
		}

		private bool VirtualKeyPress(char keyChar, int pos) {
			/*if (keyChar == '-') {
				if (pos % 7 == 6) {
					txtHardwareId.SelectionStart++;
					txtHardwareId.SelectionLength = 0;
				}
				return true;
			}*/
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
	}
}
