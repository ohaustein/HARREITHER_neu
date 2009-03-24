using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public partial class LicenseEditor : UserControl {

		private License license = null;

		public static readonly string[] availableModules = { "Produkt 1", "Produkt 2", "Produkt 3", "Produkt 4", "Feature 1", "Feature 2", "Feature 3" };

		public LicenseEditor() {
			InitializeComponent();
			this.UpdateGui();
			this.EnabledChanged += new EventHandler(LicenseEditor_EnabledChanged);
			foreach (string availableModule in availableModules) {
				this.lstModules.Items.Add(new ModuleItem(new LicensedModule(availableModule, false)));
			}
		}

		public License License {
			get { return this.license; }
			set {
				this.license = value;
				this.UpdateGui();
			}
		}

		private void UpdateGui() {
			if (this.license == null) {
				if (this.Enabled) {
					txtKey.Text = "AAAA-AAAA-AAAA-AAAA";
				} else {
					txtKey.Text = "";
				}
				txtLicensedTo.Text = "";
				txtHeader.Text = "";
				this.CheckModules();
				lstSystems.Items.Clear();
				txtSystem.Text = "";
			} else {
				txtKey.Text = this.license.Key.KeyString;
				txtLicensedTo.Text = this.license.LicensedTo;
				txtHeader.Text = this.license.Header;
			}
			
		}

		private void CheckModules() {
			foreach (ModuleItem item in this.lstModules.Items) {
				if (this.license == null) {
					item.Checked = false;
				} else {
					item.Checked = this.license.IsModuleEnabled(item.Module.Name);
				}
			}
		}

		private void txtKey_KeyDown(object sender, KeyEventArgs e) {
			char keyChar = (char)e.KeyValue;
			keyChar = char.ToUpper(keyChar);
			if (e.KeyCode == Keys.Delete) {
				keyChar = 'A';
			}
			if (e.Alt ||
				e.Control ||
				(LicenseKey.licenseKeyChars.IndexOf(keyChar) < 0 &&
					keyChar != '-' &&
					e.KeyCode != Keys.Back)
				) {
				e.Handled = !(
					e.KeyCode == Keys.Left					// allow LEFT
					|| e.KeyCode == Keys.Right				// allow RIGHT
					//|| e.Control && e.KeyCode == Keys.A		// allow 'select all'
					|| e.Control && e.KeyCode == Keys.C		// allow 'copy'
				);
				if (e.Control && e.KeyCode == Keys.V) {
					if (Clipboard.ContainsText()) {
						string text = Clipboard.GetText();
						while (txtKey.SelectionStart < 19 && text.Length > 0) {
							this.VirtualKeyPress(text[0], txtKey.SelectionStart);
							text = text.Substring(1);
						}
					}
				}
				return;
			}
			int selStart = txtKey.SelectionStart;
			if (e.KeyCode == Keys.Back) {
				if (selStart != 0) {
					if (selStart % 5 == 0) {
						selStart--;
					}
					txtKey.Text = txtKey.Text.Substring(0, selStart - 1) + 'A' + txtKey.Text.Substring(selStart);
					txtKey.SelectionStart = selStart - 1;
					txtKey.SelectionLength = 0;
				}
				e.Handled = true;
				return;
			}
			if (selStart > 18) {
				e.Handled = true;
				return;
			}
			e.Handled = this.VirtualKeyPress(keyChar, selStart);
			return;
		}

		private bool VirtualKeyPress(char keyChar, int pos) {
			if (keyChar == '-') {
				if (pos % 5 == 4) {
					txtKey.SelectionStart++;
					txtKey.SelectionLength = 0;
				}
				return true;
			}
			if (pos % 5 == 4) {
				pos++;
			}
			txtKey.Text = txtKey.Text.Substring(0, pos) + keyChar + txtKey.Text.Substring(pos + 1);
			txtKey.SelectionStart = pos + 1;
			txtKey.SelectionLength = 0;

			return true;
		}

		private void btnGenerateKey_Click(object sender, EventArgs e) {
			LicenseKey key = new LicenseKey();
			txtKey.Text = key.KeyString;
		}

		private void LicenseEditor_EnabledChanged(object sender, EventArgs e) {
			//this.UpdateGui();
		}
	}
}
