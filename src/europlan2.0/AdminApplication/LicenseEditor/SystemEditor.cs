using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public partial class SystemEditor : UserControl {

		private LicensedSystemTemplate system = null;

		public SystemEditor() {
			InitializeComponent();
			this.UpdateGui();
		}

		public LicensedSystemTemplate LicensedSystem {
			get { return this.system; }
			set {
				this.system = value;
				this.UpdateGui();
			}
		}

		private void UpdateGui() {
			this.Enabled = this.system != null;
			if (this.system == null) {
				txtHardwareId.Text = null;
				txtAnnotation.Text = null;
				dtpAdded.Value = DateTime.Today;
			} else {
				txtHardwareId.Text = this.system.Id;
				txtAnnotation.Text = this.system.Annotation;
				dtpAdded.Value = this.system.AddedDate;
			}
		}

		private void dtpAdded_ValueChanged(object sender, EventArgs e) {
			if (this.system != null) {
				this.system.AddedDate = dtpAdded.Value;
			}
		}

		private void txtHardwareId_TextChanged(object sender, EventArgs e) {
			if (this.system != null) {
				this.system.Id = txtHardwareId.Text;
			}
		}

		private void txtAnnotation_TextChanged(object sender, EventArgs e) {
			if (this.system != null) {
				this.system.Annotation = this.txtAnnotation.Text;
			}
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
					e.KeyCode != Keys.Back)
				) {
				e.Handled = !(
					e.KeyCode == Keys.Left					// allow LEFT
					|| e.KeyCode == Keys.Right				// allow RIGHT
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
			if (pos % 7 == 6) {
				pos++;
			}
			txtHardwareId.Text = txtHardwareId.Text.Substring(0, pos) + keyChar + txtHardwareId.Text.Substring(pos + 1);
			txtHardwareId.SelectionStart = pos + 1;
			txtHardwareId.SelectionLength = 0;

			return true;
		}

	}
}
