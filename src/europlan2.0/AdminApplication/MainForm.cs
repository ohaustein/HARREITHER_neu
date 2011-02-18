using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;
using System.IO;
using Europlan.Common;
using Star.SettingsXpress;
using log4net;

namespace Europlan.AdminApplication {
	public partial class MainForm : Form {

		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

		public MainForm() {
			InitializeComponent();
		}

		private void btnNewLicense_Click(object sender, EventArgs e) {
			LicenseTemplate newLicense = new LicenseTemplate();
			newLicense.ValidUntil = DateTime.Now.AddYears(1);
			foreach (string module in AbstractLicensedModule.DefaultModules.Keys) {
			//foreach (string module in LicenseEditor.availableModules) {
				newLicense.SetModuleEnabled(module, AbstractLicensedModule.DefaultEnabledModules.Contains(module));
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
					FileUtils.SetAccessForEveryone(dialog.FileName);
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
				string licensesFile = Path.Combine(PathUtil.DataPath, "licenses.xml");
				using (Stream s = new FileStream(licensesFile, FileMode.Create)) {
					LicenseManager.Instance.SaveLicenseManager(s);
				}
				FileUtils.SetAccessForEveryone(licensesFile);
			} catch (Exception ex) {
				log.Error("Problem while saving licenses", ex);
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
				string licensesFile = Path.Combine(PathUtil.DataPath, "licenses.xml");
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
				log.Error("Problem while loading licenses", ex);
			}
		}

		private void btnNew_Click(object sender, EventArgs e) {
			//this.cmsNew.Show(this.btnNew, new Point(0, this.btnNew.Height));
		}

		private void btnView_Click(object sender, EventArgs e) {
			//this.cmsView.Show(this.btnView, new Point(0, this.btnView.Height));
		}

		private void artikelUndKostruktionenToolStripMenuItem_Click(object sender, EventArgs e) {
			Configuration.AdminTemplate.Save();
			FolderBrowserDialog dialog = new FolderBrowserDialog();

			if (dialog.ShowDialog() == DialogResult.OK) {
				string filename = Path.Combine(dialog.SelectedPath, "global.conf");
				File.Copy(Path.Combine(PathUtil.DataPath, "global.conf"), filename, true);

				FileUtils.SetAccessForEveryone(filename);
			}
		}

		private void datanormDateiToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.DefaultExt = "csv";
			dialog.Filter = "Artikelliste (*.csv)|*.csv";
			if (dialog.ShowDialog() == DialogResult.OK) {
				string path = Path.GetDirectoryName(dialog.FileName);
				if (!path.Equals(PathUtil.DataPath)) {
					//if (File.Exists(Path.Combine(path, "BruttoPreise.csv"))) {
						string filename = Path.Combine(PathUtil.DataPath, "BruttoPreise.csv");
						File.Copy(dialog.FileName, filename, true);

						FileUtils.SetAccessForEveryone(filename);
						
						MessageBox.Show("Die Anwendung muss nun neu gestartet werden, damit die neu importierte Artikelliste geladen werden kann.");
						Application.Restart();
					//}
				}
			}
		}

		private void lizenzenToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			//dialog.DefaultExt = "xml";
			dialog.Filter = "Lizenzvorlagen|licenses.xml|Endbenutzerlizenz (*.epl)|*.epl";
			if (dialog.ShowDialog() == DialogResult.OK) {
				string fileName = dialog.FileName;
				List<LicenseTemplate> newLicenses = null;
				if (Path.GetFileName(fileName).Equals("licenses.xml", StringComparison.InvariantCultureIgnoreCase)) {
					FileStream fs = new FileStream(fileName, FileMode.Open);
					newLicenses = LicenseManager.Instance.LoadAdditionalLicenses(fs, false);
					fs.Close();
				} else if (Path.GetFileName(fileName).EndsWith(".epl", StringComparison.InvariantCultureIgnoreCase)) {
					FileStream fs = new FileStream(fileName, FileMode.Open);
					newLicenses = LicenseManager.Instance.LoadAdditionalLicenses(fs, true);
					fs.Close();
					MessageBox.Show("In einer .epl-Datei ist leider keine Kontakt-Emailadresse enthalten. Bitte fügen Sie die Emailadresse, falls bekannt, zur eben importieren Lizenz manuell hinzu", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				foreach (LicenseTemplate license in newLicenses) {
					LicenseItem item = new LicenseItem(license);
					this.lstLicenses.Items.Add(item);
				}
			}
		}

		private void lstLicenses_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Delete) {
				if (this.lstLicenses.SelectedItems.Count == 1 && this.lstLicenses.SelectedItems[0] is LicenseItem) {
					if (MessageBox.Show("Wollen Sie diese Lizenz wirklich löschen?", "Bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
						LicenseManager.Instance.Licenses.Remove((this.lstLicenses.SelectedItems[0] as LicenseItem).License);
						this.lstLicenses.Items.Remove(this.lstLicenses.SelectedItems[0]);
					}
				}
			}
		}

		private void mainTabControl_Selecting(object sender, TabControlCancelEventArgs e) {
			if (e.TabPage == this.tabPageErrors) {
				this.lstTranslationMissing.Items.Clear();
				Configuration config = Configuration.AdminTemplate;
				foreach (Category category in config.Categories) {
					if (!category.ResourceOk) {
						this.lstTranslationMissing.Items.Add("Kategorie: " + category.Id + " (" + category.Name + ")");
					}
				}
				foreach (Material material in config.Materials) {
					if (material.Category != null && !material.ResourceOk) {
						this.lstTranslationMissing.Items.Add("Material: " + material.Id + " (" + material.Name + ")");
					}
				}
				foreach (Construction construction in config.Constructions) {
					if (!construction.ResourceOk) {
						this.lstTranslationMissing.Items.Add("Konstruktion: " + construction.Id + " (" + construction.Name + ")");
					}
				}
				if (this.lstTranslationMissing.Items.Count == 0) {
					this.lstTranslationMissing.Items.Add("keine Elemente ohne Vorbereitung zur Übersetzung gefunden");
				}
			}
		}

		/*private void cmsViewItem_Click(object sender, EventArgs e) {
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
		}*/
	}
}