using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;
using System.IO;
using Europlan.Common;

namespace Europlan.AdminApplication {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		private void btnNewLicense_Click(object sender, EventArgs e) {
			LicenseTemplate newLicense = new LicenseTemplate();
			newLicense.ValidUntil = DateTime.Now.AddYears(1);
			foreach (string module in LicenseEditor.availableModules) {
				newLicense.SetModuleEnabled(module, false);
			}
			LicenseItem newItem = new LicenseItem(newLicense);
			this.lstLicenses.Items.Add(newItem);
			LicenseManager.Instance.Licenses.Add(newLicense);
			/*foreach (LicenseItem item in this.lstLicenses.Items) {
				item.Selected = item == newItem;
			}*/
			newItem.Selected = true;

		}

		private void lstLicenses_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			LicenseItem item = e.Item as LicenseItem;
			if (item == null) {
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
			}
		}

		private void btnSaveLicense_Click(object sender, EventArgs e) {
			License lic = this.licenseEditor1.License.CreateLicense();
			if (lic != null) {
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.CheckPathExists = true;
				dialog.DefaultExt = "epl";
				dialog.Filter = "Europlan 2.0 Lizenz (*.epl)|*.epl";

				if (dialog.ShowDialog() == DialogResult.OK) {
					using (Stream s = new FileStream(dialog.FileName, FileMode.Create)) {
						lic.SaveLicense(s);
					}
				}
			}
		}

		private void beendenToolStripMenuItem_Click(object sender, EventArgs e) {
			System.Windows.Forms.Application.Exit();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["MainForm"];
			if (this.WindowState == FormWindowState.Normal) {
				settings.StorePoint("Location", this.Location);
				settings.StoreSize("Size", this.Size);
				settings.StoreSetting("Maximized", false);
			} else if (this.WindowState == FormWindowState.Maximized) {
				settings.StoreSetting("Maximized", true);
			}
			SettingsFile.Update();

			try {
				string licensesFile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath), "licenses.xml");
				using (Stream s = new FileStream(licensesFile, FileMode.Create)) {
					LicenseManager.Instance.SaveLicenseManager(s);
				}
			} catch (Exception ex) {
				// TODO log
			}
			Configuration.AdminTemplate.Save();
		}

		private void MainForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["MainForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			if (settings.GetSetting("Maximized", false)) {
				this.WindowState = FormWindowState.Maximized;
			} else {
				this.WindowState = FormWindowState.Normal;
			}

			try {
				string licensesFile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath), "licenses.xml");
				if (File.Exists(licensesFile)) {
					using (Stream s = new FileStream(licensesFile, FileMode.Open)) {
						LicenseManager.LoadLicenseManager(s);
					}
					foreach (LicenseTemplate license in LicenseManager.Instance.Licenses) {
						LicenseItem item = new LicenseItem(license);
						this.lstLicenses.Items.Add(item);
					}
				}
			} catch (Exception ex) {
				// TODO log
			}
		}

		private void btnNew_Click(object sender, EventArgs e) {
			this.cmsNew.Show(this.btnNew, new Point(0, this.btnNew.Height));
		}

		private void btnView_Click(object sender, EventArgs e) {
			this.cmsView.Show(this.btnView, new Point(0, this.btnView.Height));
		}

		private void cmsViewItem_Click(object sender, EventArgs e) {
			int i = 0;
			string selected = "";
			if (this.tsmiFloorConstruction.Checked) {
				selected += ", FB";
				i++;
			}
			if (this.tsmiInsulationConstruction.Checked) {
				selected += ", WD";
				i++;
			}
			if (i == 0) {
				selected = "keine";
				this.constructionEditorPage.Filter = ConstructionScopeEnum.UnknownConstruction;
			} else if (i == 2) {
				selected = "alle";
				this.constructionEditorPage.Filter = ConstructionScopeEnum.All;
			} else {
				selected = selected.Substring(2);
				if (this.tsmiFloorConstruction.Checked) {
					this.constructionEditorPage.Filter = ConstructionScopeEnum.FloorConstruction;
				} else {
					this.constructionEditorPage.Filter = ConstructionScopeEnum.InsulationConstruction;
				}
			}
			this.btnView.Text = "Angezeigte Konstruktionen (" + selected + ")";
		}

		private void tsmiNewConstruction_Click(object sender, EventArgs e) {
			Construction c = null;
			if (sender == this.tsmiNewFloorConstruction) {
				c = new FloorConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH);
			} else if (sender == this.tsmiNewInsulationConstruction) {
				c = new InsulationConstruction();
				c.Type = ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_DAEMM);
			} else if (sender == this.tsmiNewWallConstruction) {
				// TODO
			}
			if (c != null) {
				ConstructionEditorForm form = new ConstructionEditorForm(c);
				form.ShowDialog();
				this.constructionEditorPage.AddConstruction(c);
			}
		}
	}
}