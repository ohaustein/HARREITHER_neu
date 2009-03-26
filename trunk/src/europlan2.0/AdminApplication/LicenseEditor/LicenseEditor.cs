using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public partial class LicenseEditor : UserControl {

		private LicenseTemplate license = null;

		public static readonly string[] availableModules = { "Produkt 1", "Produkt 2", "Produkt 3", "Produkt 4", "Feature 1", "Feature 2", "Feature 3" };

		public LicenseEditor() {
			InitializeComponent();
			/*foreach (string availableModule in availableModules) {
				this.lstModules.Items.Add(new ModuleItem(new LicensedModule(availableModule, false)));
			}*/
			this.UpdateGui();
		}

		public LicenseTemplate License {
			get { return this.license; }
			set {
				this.license = value;
				this.UpdateGui();
			}
		}

		private void UpdateGui() {
			this.Enabled = this.license != null;
			if (this.license == null) {
				txtLicensedTo.Text = "";
				dtpValidUntil.Value = DateTime.Today;
				txtHeader.Text = "";
				lstModules.Items.Clear();
				lstSystems.Items.Clear();
			} else {
				txtLicensedTo.Text = this.license.LicensedTo;
				dtpValidUntil.Value = this.license.ValidUntil;
				txtHeader.Text = this.license.Header;
				this.UpdateModulesEnablement();
				this.RefreshSystemList();
			}
			
		}

		private void UpdateModulesEnablement() {
			lstModules.Items.Clear();
			if (this.license != null) {
				foreach (LicensedModuleTemplate module in license.Modules) {
					lstModules.Items.Add(new ModuleItem(module));
				}
			}
			/*foreach (ModuleItem item in this.lstModules.Items) {
				if (this.license == null) {
					item.Checked = false;
				} else {
					item.Checked = this.license.IsModuleEnabled(item.Module.Name);
				}
				// TODO handle modules that are in license but not in list!!! 
			}*/
		}

		/*private void txtKey_KeyDown(object sender, KeyEventArgs e) {
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
		}*/

		private void txtLicensedTo_TextChanged(object sender, EventArgs e) {
			if (this.license != null) {
				this.license.LicensedTo = txtLicensedTo.Text;
			}
		}

		private void dtpValidUntil_ValueChanged(object sender, EventArgs e) {
			if (this.license != null) {
				this.license.ValidUntil = dtpValidUntil.Value;
			}
		}

		private void txtHeader_TextChanged(object sender, EventArgs e) {
			if (this.license != null) {
				this.license.Header = txtHeader.Text;
			}
		}

		private void lstModules_ItemChecked(object sender, ItemCheckedEventArgs e) {
			if (this.license != null) {
				ModuleItem item = e.Item as ModuleItem;
				if (item != null) {
					item.Module.Enabled = item.Checked;
				}
			}
		}

		private void btnAddSystem_Click(object sender, EventArgs e) {
			if (this.license != null) {
				LicensedSystemTemplate system = new LicensedSystemTemplate("000000-000000-000000");
				SystemItem newItem = new SystemItem(system);
				this.license.Systems.Add(system);
				this.lstSystems.Items.Add(newItem);
				newItem.Selected = true;
			}
		}

		private void RefreshSystemList() {
			lstSystems.Items.Clear();
			if (this.license != null) {
				foreach (LicensedSystemTemplate system in this.license.Systems) {
					lstSystems.Items.Add(new SystemItem(system));
				}
			}
		}

		private void lstSystems_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			SystemItem item = e.Item as SystemItem;
			if (item == null) {
				return;
			}
			if (this.syeCurrentSystem.LicensedSystem == item.LicensedSystem) {
				if (!e.IsSelected) {
					this.syeCurrentSystem.LicensedSystem = null;
				}
			} else if (e.IsSelected) {
				this.syeCurrentSystem.LicensedSystem = item.LicensedSystem;
			}
			/*LicensedSystem sytem = e.Item.Tag as LicensedSystem;
			if (system == null) {
				return;
			}
			if (this.licenseEditor1.License == item.License) {
				if (!e.IsSelected) {
					this.licenseEditor1.License = null;
					this.btnSaveLicense.Enabled = false;
				}
			} else if (e.IsSelected) {
				this.licenseEditor1.License = item.License;
				this.btnSaveLicense.Enabled = true;
			}*/

		}

		private void lstSystems_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Delete) {
				ListView.SelectedListViewItemCollection items = this.lstSystems.SelectedItems;
				foreach (ListViewItem item in items) {
					this.lstSystems.Items.Remove(item);
				}
			}
		}
	}
}
