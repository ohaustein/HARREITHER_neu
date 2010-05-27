using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Resources;
using System.Reflection;
using System.Threading;
using log4net;
using Europlan.Licensing;
using System.IO;
using Europlan.Common;
#if CLIPBOARD_TEST
using System.Runtime.Serialization.Formatters.Binary;
#endif


namespace Europlan.Application {
	public partial class MainForm : Form {
		
		private string projectFileName = null;
		private Project currentProject = null;

		private static readonly string defaultTitle = "Europlan";
		private string title;

		private Dictionary<Type, UserControl> userControls = new Dictionary<Type, UserControl>();
		private bool guiUpdateInProgress = false;
		private bool projectUnsaved = false;
		private TreeNode selectedTreeNode = null;

		private bool loadAfterRestart = false;

		Queue<string> mruList = new Queue<string>();

		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

		private IEditorUserControl currentEditorUserControl = null;

		private bool updateCheckCompleted = false;

		public MainForm() {
			InitializeComponent();

			this.SetLanguage();

			this.updateController.ApplicationId = Program.updateGuid;
			this.updateController.UpdateLocation = Program.updateLocation;
			this.updateController.PublicKeyToken = Program.updatePublicKey;

			LicenseManager.Instance.LicenseChanged += new EventHandler(licenseManager_LicenseChanged);

			this.UpdateAvailableFeatures();
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.MainForm_Titel; //"Europlan";
			auslegeAssistentButton.Text = EuroplanRes.MainForm_Auslegungshilfe; //"FBH-Auslegungshilfe";
			copyToolStripButton.Text = EuroplanRes.General_Kopieren; //"&Kopieren";
			copyToolStripMenuItem.Text = EuroplanRes.General_Kopieren; //"Kopieren";
			cutToolStripButton.Text = EuroplanRes.General_Ausschneiden; //"&Ausschneiden";
			cutToolStripMenuItem.Text = EuroplanRes.General_Ausschneiden; //"Ausschneiden";
			datanormToolStripMenuItem.Text = EuroplanRes.MainForm_Artikelstamm; //"Artikelstamm";
			demandedHeatToolStripMenuItem.Text = EuroplanRes.MainForm_WaermeUndKuehlbedarf; //"&Wärme-/Kühlbedarf";
			editToolStripMenuItem.Text = EuroplanRes.MainForm_Bearbeiten; //"&Bearbeiten";
			exitToolStripMenuItem.Text = EuroplanRes.MainForm_Beenden; //"B&eenden";
			fileToolStripMenuItem.Text = EuroplanRes.MainForm_Datei; //"&Datei";
			helpToolStripButton.Text = EuroplanRes.MainForm_Hilfe; //"&Hilfe";
			helpToolStripMenuItem.Text = EuroplanRes.MainForm_Hilfe; //"&Hilfe";
			importGlobalConfToolStripMenuItem.Text = EuroplanRes.MainForm_ArtikelUndKonstruktionen; //"Artikel und Konstruktionen";
			importToolStripMenuItem.Text = EuroplanRes.MainForm_Importieren; //"&Importieren";
			inhaltToolStripMenuItem.Text = EuroplanRes.MainForm_HilfeInhalt;
			tutorialToolStripMenuItem.Text = EuroplanRes.MainForm_HilfeTutorial;
			infoToolStripMenuItem.Text = EuroplanRes.MainForm_Info; //"Info";
			licenseToolStripMenuItem.Text = EuroplanRes.MainForm_Lizenz; //"&Lizenz";
			newToolStripButton.Text = EuroplanRes.MainForm_Neu; //"&Neu";
			newToolStripMenuItem.Text = EuroplanRes.MainForm_Neu; //"&Neu";
			openGlobalConfDialog.Filter = EuroplanRes.MainForm_ArtikelUndKonstruktionenFilter + "|global.conf"; //"Artikel und Konstruktionen|global.conf";
			openToolStripButton.Text = EuroplanRes.MainForm_Oeffnen; //"Ö&ffnen";
			openToolStripMenuItem.Text = EuroplanRes.MainForm_Oeffnen; //"Ö&ffnen";
			optionsToolStripMenuItem.Text = EuroplanRes.MainForm_Optionen; //"&Optionen";
			pasteToolStripButton.Text = EuroplanRes.MainForm_Einfuegen; //"&Einfügen";
			pasteToolStripMenuItem.Text = EuroplanRes.MainForm_Einfuegen; //"Einfügen";
			printToolStripButton.Text = EuroplanRes.MainForm_Drucken; //"&Drucken";
			projectOverviewCoolToolStripButton.Text = EuroplanRes.MainForm_UebersichtKuehlen; //"Übersicht Kühlen";
			projectOverviewHeatToolStripButton.Text = EuroplanRes.MainForm_UebersichtHeizen; //"Übersicht Heizen";
			projektToolStripMenuItem.Text = EuroplanRes.MainForm_Projekt; //"Projekt";
			recentProjectsToolStripMenuItem.Text = EuroplanRes.MainForm_LetzteProjekte; //"Letzte Projekte";
			saveAsToolStripMenuItem.Text = EuroplanRes.MainForm_SpeichernUnter; //"Speichern &unter...";
			saveToolStripButton.Text = EuroplanRes.MainForm_Speichern; //"Speic&hern";
			saveToolStripMenuItem.Text = EuroplanRes.MainForm_Speichern; //"Speic&hern";
			settingsToolStripMenuItem.Text = EuroplanRes.MainForm_Einstellungen; //"&Einstellungen";
			updateToolStripMenuItem.Text = EuroplanRes.MainForm_Aktualisieren; //"Auf Aktualisierungen prüfen...";
			viewReportToolStripMenuItem.Text = EuroplanRes.MainForm_Ansehen; //"Ansehen";
			warningsAndErrorsToolStripMenuItem.Text = EuroplanRes.MainForm_Warnungen; //"Warnungen und Fehler";
		}

		private void licenseManager_LicenseChanged(object sender, EventArgs e) {
			this.UpdateAvailableFeatures();
		}

		private void UpdateAvailableFeatures() {
			if (!LicenseManager.Instance.LicenseFoundAndValid) {
				this.newToolStripMenuItem.Enabled = false;
				this.newToolStripButton.Enabled = false;
				this.openToolStripMenuItem.Enabled = false;
				this.openToolStripButton.Enabled = false;
				this.saveToolStripMenuItem.Enabled = false;
				this.saveToolStripButton.Enabled = false;
				this.saveAsToolStripMenuItem.Enabled = false;
				this.printToolStripButton.Enabled = false;
				this.title = MainForm.defaultTitle + " (" + EuroplanRes.General_NichtLizenziert + ")";
				this.UpdateTitle();
			} else {
				this.newToolStripMenuItem.Enabled = true;
				this.newToolStripButton.Enabled = true;
				this.openToolStripMenuItem.Enabled = true;
				this.openToolStripButton.Enabled = true;
				this.saveToolStripMenuItem.Enabled = true;
				this.saveToolStripButton.Enabled = true;
				this.saveAsToolStripMenuItem.Enabled = true;
				this.printToolStripButton.Enabled = true;
				this.title = MainForm.defaultTitle;
				this.UpdateTitle();
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			System.Windows.Forms.Application.Exit();
		}

		private bool CheckForUnsavedChanges() {
			if (projectUnsaved) {
				string messageText = EuroplanRes.General_AenderungenSpeichernText;
				string caption = EuroplanRes.General_AenderungenSpeichernTitel;
				DialogResult result = MessageBox.Show(messageText, caption, MessageBoxButtons.YesNoCancel);
				if (result == DialogResult.Cancel) {
					return false;
				} else if (result == DialogResult.No) {
					return true;
				} else if (result == DialogResult.Yes) {
					IEditorUserControl oldControl = splitContainer.Panel2.Controls[0] as IEditorUserControl;
					if (oldControl != null && !oldControl.AllowLeave()) {
						return false;
					}
					string tempFileName = projectFileName;
					if (projectFileName == null) {
						SaveFileDialog dialog = new SaveFileDialog();
						dialog.CheckPathExists = true;
						dialog.DefaultExt = "e2p";
						dialog.Filter = EuroplanRes.MainForm_E2pFilter + "|*.e2p";
						result = dialog.ShowDialog();
						if (result == DialogResult.OK) {
							projectFileName = dialog.FileName;
							SaveProject();
						} else {
							return false;
						}
					} else {
						SaveProject();
					}
					projectFileName = tempFileName;
				}
			}
			return true;
		}

		private void MainForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["MainForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			this.splitContainer.SplitterDistance = settings.GetSetting("SplitterDistance", this.splitContainer.SplitterDistance);
			if (settings.GetSetting("Maximized", false)) {
				this.WindowState = FormWindowState.Maximized;
			} else {
				this.WindowState = FormWindowState.Normal;
			}

			string recentProjectsSetting = settings.GetSetting("RecentProjects", "");
			string[] recentProjects = recentProjectsSetting.Split(';');
			recentProjectsToolStripMenuItem.DropDownItems.Clear();
			foreach (string item in recentProjects) {
				if (File.Exists(item)) {
					mruList.Enqueue(item);
					ToolStripMenuItem fileRecent = new ToolStripMenuItem(item, null, RecentProject_click);
					recentProjectsToolStripMenuItem.DropDownItems.Insert(0, fileRecent);
				}
			}
			recentProjectsToolStripMenuItem.Enabled = mruList.Count > 0;

			Project.ProjectLoaded += new Project.ProjectLoadedHandler(myProject_ProjectLoaded);
			Project.ProjectSaved += new Project.ProjectSavedHandler(myProject_ProjectSaved);

			string loadProject = settings.GetSetting("LoadProject", "");
			if (loadProject != "") {
				projectFileName = loadProject;
			}

			if (projectFileName != null) {
				LoadProject();
			} else {
				NewProject(true);
			}

			Project.Instance.InitializeTreeView(this.projectTree);

			bool restart = false;
			if (!LicenseManager.Instance.LicenseFoundAndValid) {
				LicenseForm license = new LicenseForm();
				license.ShowDialog();
				restart = license.RestartRequired;
				if (restart) {
					string message = EuroplanRes.General_NeustartErforderlichText;
					string caption = EuroplanRes.General_NeustartErforderlichTitel;
					MessageBox.Show(message, caption, MessageBoxButtons.OK);
					System.Windows.Forms.Application.Restart();
				}
				license.Dispose();
			}
			if (!restart) {
				this.updateController.CheckForUpdateAsync();
			}
		}

		private void AddRecentProject(string fileName) {
			if (!mruList.Contains(projectFileName)) {
				mruList.Enqueue(projectFileName);
				if (mruList.Count > 6) {
					mruList.Dequeue();
				}
			}
			recentProjectsToolStripMenuItem.DropDownItems.Clear();
			foreach (string item in mruList) {
				ToolStripMenuItem fileRecent = new ToolStripMenuItem(item, null, RecentProject_click);
				recentProjectsToolStripMenuItem.DropDownItems.Insert(0, fileRecent);
			}
			recentProjectsToolStripMenuItem.Enabled = mruList.Count > 0;
		}

		private void RecentProject_click(object sender, EventArgs e) {
			if (CheckForUnsavedChanges()) {
				projectFileName = sender.ToString();
				LoadProject();
			}
		}

		private void LoadProject() {
			this.splitContainer.Panel2.Controls.Clear();
			try {
				if (projectFileName != null) {
					Project.Load(projectFileName);
					currentProject = Project.Instance;
					AddRecentProject(projectFileName);
				}
			} catch (Exception ex) {
				if (ex is ProductNotLicensedException || ex.InnerException is ProductNotLicensedException) {
					ProductNotLicensedException pnle;
					if (ex is ProductNotLicensedException) {
						pnle = ex as ProductNotLicensedException;
					} else {
						pnle = ex.InnerException as ProductNotLicensedException;
					}
					object[] names = pnle.ProductType.GetCustomAttributes(typeof(ProductNameAttribute), true);
					string name;
					if (names.Length > 0) {
						name = (names[0] as ProductNameAttribute).FullName;
					} else {
						name = pnle.ProductType.Name;
					}
					string message = EuroplanRes.MainForm_NichtLizensiertesProduktImProjektText;
					message = message.Replace("%PRODUKT%", name);
					MessageBox.Show(message, EuroplanRes.MainForm_NichtLizensiertesProduktImProjektTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else if (ex is ProjectVersionNotCompatibleException) {
					// nothing to do here
				} else {
					log.Error("Problem loading project:", ex);
				}
				currentProject = null;
				projectFileName = null;
				this.NewProject(false);
			}
		}

		private void SaveProject() {
			IEditorUserControl oldControl = splitContainer.Panel2.Controls[0] as IEditorUserControl;
			if (oldControl != null && !oldControl.AllowLeave()) {
				return;
			}
			try {
				if (currentProject != null && projectFileName != null) {
					Project.Save(projectFileName);
					AddRecentProject(projectFileName);
				}
			} catch (Exception ex) {
				log.Error("Problem saving project:", ex);
			}
		}

		private void NewProject(bool checkChanges) {
			if (!checkChanges || CheckForUnsavedChanges()) {
				this.splitContainer.Panel2.Controls.Clear();
				if (currentProject == null) {
					currentProject = Project.New();
				} else {
					currentProject = Project.New();
				}
				projectFileName = null;
				projectUnsaved = false;
				UpdateTitle();
				Project.Instance.InitializeTreeView(this.projectTree);
				projectTree.SelectedNode = projectTree.Nodes[0];
				if (currentEditorUserControl != null) {
					currentEditorUserControl.UpdateControl(true);
				}
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (CheckForUnsavedChanges()) {
				Project.ProjectLoaded -= myProject_ProjectLoaded;
				Project.ProjectSaved -= myProject_ProjectSaved;

				SettingsKey settings = SettingsFile.Settings["MainForm"];
				if (this.WindowState == FormWindowState.Normal) {
					settings.StorePoint("Location", this.Location);
					settings.StoreSize("Size", this.Size);
					settings.StoreSetting("Maximized", false);
				} else if (this.WindowState == FormWindowState.Maximized) {
					settings.StoreSetting("Maximized", true);
				}

				string recentProjects = "";
				foreach (string recentProject in mruList) {
					recentProjects += recentProject;
					recentProjects += ";";
				}
				recentProjects = recentProjects.TrimEnd(';');
				if (recentProjects != string.Empty) {
					settings.StoreSetting("RecentProjects", recentProjects);
				}

				settings.StoreSetting("SplitterDistance", this.splitContainer.SplitterDistance);

				if (loadAfterRestart) {
					settings.StoreSetting("LoadProject", projectFileName);
				} else {
					try {
						settings.DeleteSetting("LoadProject");
					} catch { }
				}

				SettingsFile.Update();
			} else {
				e.Cancel = true;
			}
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			OptionsForm options = new OptionsForm();
			DialogResult result = options.ShowDialog();
			if (result == DialogResult.OK && options.RestartRequired) {
				string message = EuroplanRes.General_NeustartErforderlichText;
				string caption = EuroplanRes.General_NeustartErforderlichTitel;
				result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel);
				if (result == DialogResult.OK) {
					System.Windows.Forms.Application.Restart();
				}
			}
			options.Dispose();
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
				if (!this.updateCheckCompleted) {
					MessageBox.Show(EuroplanRes.MainForm_AktualisierungWirdGeprueft);
				} else {
					this.updateController.UpdateInteractive();
				}
			} catch (Exception ex) {
				// TODO
				MessageBox.Show(EuroplanRes.MainForm_AktualisierungWirdGeprueft);
			}
		}

		private void updateController_CheckForUpdateCompleted(object sender, Kjs.AppLife.Update.Controller.CheckForUpdateCompletedEventArgs e) {
			this.updateCheckCompleted = true;
			if (e.Error == null  && e.Result) {
				this.updateController.UpdateInteractive(this, Kjs.AppLife.Update.Controller.ErrorDisplayLevel.ShowExceptionMessage);
			}
		}

		void myProject_ProjectSaved(object sender) {
			projectUnsaved = false;
			UpdateTitle();
			Project.Instance.InitializeTreeView(this.projectTree);
		}

		private void UpdateTitle() {
			if (!LicenseManager.Instance.LicenseFoundAndValid) {
				this.Text = this.title;
			} else {
				string title = this.title + " - [";
				if (projectFileName != null) {
					title += projectFileName;
				} else {
					title += EuroplanRes.General_NeuerProjektName;
				}
				if (projectUnsaved) {
					title += "*";
				}
				title += "]";

				this.Text = title;
			}
		}

		void myProject_ProjectLoaded(object sender) {
			projectUnsaved = false;
			UpdateTitle();
			Project.Instance.InitializeTreeView(this.projectTree);
			this.projectTree.SelectedNode = this.projectTree.Nodes[0];
		}

		public string ProjectToLoad {
			set {
				this.projectFileName = value;
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e) {
			if (CheckForUnsavedChanges()) {
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.CheckFileExists = true;
				dialog.CheckPathExists = true;
				dialog.DefaultExt = "e2p";
				dialog.Filter = EuroplanRes.MainForm_E2pFilter + "|*.e2p";
				dialog.Multiselect = false;
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					projectFileName = dialog.FileName;
					LoadProject();
				}
			}
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e) {
			NewProject(true);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			IEditorUserControl oldControl = splitContainer.Panel2.Controls[0] as IEditorUserControl;
			if (oldControl != null && !oldControl.AllowLeave()) {
				return;
			}
			if (projectFileName == null) {
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.CheckPathExists = true;
				dialog.DefaultExt = "e2p";
				dialog.Filter = EuroplanRes.MainForm_E2pFilter + "|*.e2p";
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					projectFileName = dialog.FileName;
					SaveProject();
				}
			} else {
				SaveProject();
			}
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
			IEditorUserControl oldControl = splitContainer.Panel2.Controls[0] as IEditorUserControl;
			if (oldControl != null && !oldControl.AllowLeave()) {
				return;
			}
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.CheckPathExists = true;
			dialog.DefaultExt = "e2p";
			dialog.Filter = EuroplanRes.MainForm_E2pFilter + "|*.e2p";
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				projectFileName = dialog.FileName;
				SaveProject();
			}
		}

		private void licenseToolStripMenuItem_Click(object sender, EventArgs e) {
			LicenseForm license = new LicenseForm();
			license.ShowDialog();
			if (license.RestartRequired) {
				string message = EuroplanRes.General_NeustartErforderlichText;
				string caption = EuroplanRes.General_NeustartErforderlichTitel;
				MessageBox.Show(message, caption, MessageBoxButtons.OK);
				System.Windows.Forms.Application.Restart();
			}
			license.Dispose();
		}

		private void projectTree_BeforeSelect(object sender, TreeViewCancelEventArgs e) {
			if (splitContainer.Panel2.Controls.Count > 0) {
				IEditorUserControl oldControl = splitContainer.Panel2.Controls[0] as IEditorUserControl;
				if (oldControl != null && !oldControl.AllowLeave()) {
					e.Cancel = true;
				}
			}
		}

		private void projectTree_AfterSelect(object sender, TreeViewEventArgs e) {
			if (selectedTreeNode != null) {
				selectedTreeNode.NodeFont = new Font(this.projectTree.Font, FontStyle.Regular);
			}
			selectedTreeNode = e.Node;
			selectedTreeNode.NodeFont = new Font(this.projectTree.Font, FontStyle.Bold);
			selectedTreeNode.Text = selectedTreeNode.Text;
			Control oldControl = null;
			bool tagChanged = true;
			if (splitContainer.Panel2.Controls.Count > 0) {
				oldControl = splitContainer.Panel2.Controls[0];
			}
			if (selectedTreeNode != null && selectedTreeNode.Tag != null) {
				UserControl control = null;
				if (selectedTreeNode.Tag is IGuiRepresentation) {
					IGuiRepresentation guiRepresentation = selectedTreeNode.Tag as IGuiRepresentation;
					Type associatedPanelType = guiRepresentation.AssociatedPanelType;
					if (associatedPanelType == null) {
						associatedPanelType = typeof(EmptyEditorUserControl);
					}
					if (userControls.ContainsKey(associatedPanelType)) {
						control = userControls[associatedPanelType];
					} else {
						control = (UserControl)Activator.CreateInstance(associatedPanelType);
						(control as IEditorUserControl).ProjectStructureChanged += new ProjectStructureChangedHandler(MainForm_ProjectStructureChanged);
						(control as IEditorUserControl).ProjectChanged += new ProjectChangedHandler(MainForm_ProjectChanged);
						(control as IEditorUserControl).TreeSelectionRequested += new TreeSelectionRequestedHandler(MainForm_TreeSelectionRequested);
						userControls[associatedPanelType] = control;
					}
					if (control.Tag != selectedTreeNode.Tag) {
						tagChanged = true;
						control.Tag = selectedTreeNode.Tag;
					}
				} else if (selectedTreeNode.Tag is Type) {
					if (userControls.ContainsKey(selectedTreeNode.Tag as Type)) {
						control = userControls[selectedTreeNode.Tag as Type];
					} else {
						control = (UserControl)Activator.CreateInstance(selectedTreeNode.Tag as Type);
						(control as IEditorUserControl).ProjectStructureChanged += new ProjectStructureChangedHandler(MainForm_ProjectStructureChanged);
						(control as IEditorUserControl).ProjectChanged += new ProjectChangedHandler(MainForm_ProjectChanged);
						(control as IEditorUserControl).TreeSelectionRequested += new TreeSelectionRequestedHandler(MainForm_TreeSelectionRequested);
						userControls[selectedTreeNode.Tag as Type] = control;
					}
				}
				if (control != null) {
					guiUpdateInProgress = true;
					if (control != oldControl) {
						splitContainer.Panel2.Controls.Clear();
						splitContainer.Panel2.Controls.Add(control);
						control.Dock = DockStyle.Fill;
						if (control is IEditorUserControl) {
							currentEditorUserControl = control as IEditorUserControl;
						}
					}
					//if (oldControl == null || control.Tag != oldControl.Tag) {
					if (tagChanged) {
						(control as IEditorUserControl).UpdateControl(true);
					}
					guiUpdateInProgress = false;
				}
			}
		}

		private void MainForm_TreeSelectionRequested(object sender, object requestedItem) {
			TreeNode node = Project.Instance.FindNode(requestedItem);
			this.projectTree.SelectedNode = node;
		}

		void MainForm_ProjectStructureChanged(object sender) {
			if (!guiUpdateInProgress) {
				Project.Instance.InitializeTreeView(this.projectTree);
				projectUnsaved = true;
				UpdateTitle();
			}
		}


		void MainForm_ProjectChanged(object sender) {
			if (!guiUpdateInProgress) {
				projectUnsaved = true;
				UpdateTitle();
			}
		}

		private Control GetActiveControl() {
			Control activeControl = this.ActiveControl;
			while (activeControl != null && activeControl is ContainerControl) {
				activeControl = (activeControl as ContainerControl).ActiveControl;
			}
			return activeControl;
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
			Control activeControl = GetActiveControl();
			if (activeControl != null) {
				if (activeControl is TextBoxBase) {
					(activeControl as TextBoxBase).Cut();
				} else {
					Clipboard.SetText(activeControl.Text);
					activeControl.Text = "";
				}
			}
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
			Control activeControl = GetActiveControl();
			if (activeControl != null) {
				if (activeControl is TextBoxBase) {
					(activeControl as TextBoxBase).Copy();
				} else if (activeControl == projectTree) {
					if (projectTree.SelectedNode != null) {
						if (projectTree.SelectedNode.Tag is IClipboard) {
							IClipboard clipboardObject = projectTree.SelectedNode.Tag as IClipboard;
							if (clipboardObject.SupportsCopy) {
								DataFormats.Format format =  DataFormats.GetFormat(clipboardObject.DataFormat);
#if CLIPBOARD_TEST
								IsSerializable(clipboardObject.Copy());
#endif
								Clipboard.SetData(format.Name, clipboardObject.Copy());
							}
						}
					}
				} else if (activeControl is DataGridView) {
					DataGridView grid = activeControl as DataGridView;
					if (grid.SelectedRows.Count > 0 && grid.SelectedRows[0].DataBoundItem != null && grid.SelectedRows[0].DataBoundItem is IClipboard) {
						IClipboard selectedItem = grid.SelectedRows[0].DataBoundItem as IClipboard;
						if (selectedItem.SupportsCopy) {
							DataFormats.Format format = DataFormats.GetFormat(selectedItem.DataFormat);
#if CLIPBOARD_TEST
							IsSerializable(selectedItem.Copy());
#endif
							Clipboard.SetData(format.Name, selectedItem.Copy());
						}
						/*if (grid.Name == "gridFloors") {

						} else if (grid.Name = "gridRooms") {

						}*/
					}
				} else {
					Clipboard.SetText(activeControl.Text);
				}
			}
		}

#if CLIPBOARD_TEST
		private static bool IsSerializable(object obj)
		{
		  System.IO.MemoryStream mem = new System.IO.MemoryStream();
		  BinaryFormatter bin = new BinaryFormatter();
		  try
		  {
			bin.Serialize(mem, obj);
			return true;
		  }
		  catch(Exception ex)
		  {
			MessageBox.Show("Your object cannot be serialized." + 
							 " The reason is: " + ex.ToString());
			return false;
		  }
		}
#endif

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
			Control activeControl = GetActiveControl();
			if (activeControl != null) {
				if (activeControl is TextBoxBase) {
					(activeControl as TextBoxBase).Paste();
				} else if (activeControl == projectTree) {
					if (projectTree.SelectedNode != null) {
						if (projectTree.SelectedNode.Tag is IClipboard) {
							IClipboard clipboardObject = projectTree.SelectedNode.Tag as IClipboard;
							if (clipboardObject.SupportedPasteFormat != null) {
								DataFormats.Format format = DataFormats.GetFormat(clipboardObject.SupportedPasteFormat);
								if (Clipboard.ContainsData(format.Name)) {
									object o = Clipboard.GetData(format.Name);
									if (o != null) {
										clipboardObject.Paste(o);
										Project.Instance.InitializeTreeView(this.projectTree);
										projectUnsaved = true;
										UpdateTitle();
									}
								}
							}
						}
					}
				} else if (activeControl is DataGridView) {
					DataGridView grid = activeControl as DataGridView;
					if (grid.DataSource is BindingSource) {
						BindingSource source = grid.DataSource as BindingSource;
						if (source.DataSource is IClipboard) {
							IClipboard clipboardObject = source.DataSource as IClipboard;
							if (clipboardObject.SupportedPasteFormat != null) {
								DataFormats.Format format = DataFormats.GetFormat(clipboardObject.SupportedPasteFormat);
								if (Clipboard.ContainsData(format.Name)) {
									object o = Clipboard.GetData(format.Name);
									if (o != null) {
										clipboardObject.Paste(o);
										Project.Instance.InitializeTreeView(this.projectTree);
										projectUnsaved = true;
										UpdateTitle();
									}
								}
							}
						}
					}
				} else {
					activeControl.Text = Clipboard.GetText();
				}
			}
			if (splitContainer.Panel2.Controls.Count > 0) {
				if (splitContainer.Panel2.Controls[0] is IEditorUserControl) {
					(splitContainer.Panel2.Controls[0] as IEditorUserControl).UpdateControl(true);
				}
			}
		}

		private void demandedHeatToolStripMenuItem_Click(object sender, EventArgs e) {
			BuildingDataImportManager.Instance.ImportBuildingData();
			if (!guiUpdateInProgress) {
				Project.Instance.InitializeTreeView(this.projectTree);
				projectUnsaved = true;
				UpdateTitle();
				if (currentEditorUserControl != null) {
					currentEditorUserControl.UpdateControl(true);
				}
			}
		}

		private void importGlobalConfToolStripMenuItem_Click(object sender, EventArgs e) {
			if (this.CheckForUnsavedChanges()) {
				if (this.openGlobalConfDialog.ShowDialog() == DialogResult.OK) {
					string appDataPath = Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath);
					if (!Path.GetDirectoryName(this.openGlobalConfDialog.FileName).Equals(appDataPath)) {
						File.Copy(this.openGlobalConfDialog.FileName, Path.Combine(appDataPath, "global.conf"), true);
						Configuration.ResetConfigurations();
					}
					if (projectFileName == null) {
						// reset project
						if (currentProject == null) {
							currentProject = Project.New();
						} else {
							currentProject = Project.New();
						}
						projectFileName = null;
						projectUnsaved = false;
						UpdateTitle();
						Project.Instance.InitializeTreeView(this.projectTree);
						if (currentEditorUserControl != null) {
							currentEditorUserControl.UpdateControl(true);
						}
					} else {
						LoadProject();
					}
				}
			}
		}

		private void datanormToolStripMenuItem_Click(object sender, EventArgs e) {
			if (this.CheckForUnsavedChanges()) {
				OpenFileDialog dialog = new OpenFileDialog();
				//FolderBrowserDialog dialog = new FolderBrowserDialog();
				dialog.Filter = EuroplanRes.MainForm_BruttoPreiseFilter + "|BruttoPreise*.csv";
				string appDataPath = Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath);
				if (dialog.ShowDialog() == DialogResult.OK) {
					string path = Path.GetDirectoryName(dialog.FileName);
					if (!path.Equals(appDataPath)) {
						// TODO 
						// check if csv contains materials
						if (File.Exists(dialog.FileName)) {
							File.Copy(dialog.FileName, Path.Combine(appDataPath, "BruttoPreise.csv"), true);
						}
					}
					if (projectFileName == null) {
						// reset project
						if (currentProject == null) {
							currentProject = Project.New();
						} else {
							currentProject = Project.New();
						}
						projectFileName = null;
						projectUnsaved = false;
						UpdateTitle();
						Project.Instance.InitializeTreeView(this.projectTree);
						if (currentEditorUserControl != null) {
							currentEditorUserControl.UpdateControl(true);
						}
					} else {
						loadAfterRestart = true;
						System.Windows.Forms.Application.Restart();
					}
				}
			}
		}

		private void viewReportToolStripMenuItem_Click(object sender, EventArgs e) {
			ProjectReportOptions optionsForm = new ProjectReportOptions();
			if (optionsForm.ShowDialog() == DialogResult.OK) {
//#if !DEBUG
//				MessageBox.Show("Hinweis: Die Druckvorschau enthält großteils noch keine Echtdaten. Sie ist derzeit nur als Diskussionsgrundlage für das Layout zu verstehen.", "Hinweis");
//#endif
				ProjectReport reportForm = new ProjectReport(this.currentProject, optionsForm);
				reportForm.ShowDialog();
				reportForm.Dispose();
			}
			optionsForm.Dispose();
		}

		private void projectOverviewHeatToolStripButton_Click(object sender, EventArgs e) {
			ProductOverviewForm form = new ProductOverviewForm(true);
			form.ShowDialog();
			if (form.UnsavedChanges) {
				projectUnsaved = true;
				UpdateTitle();
			}
			if (form.ProductToEdit != null) {
				TreeNode node = Project.Instance.FindNode(form.ProductToEdit);
				this.projectTree.SelectedNode = node;
			} else {
				if (currentEditorUserControl != null) {
					currentEditorUserControl.UpdateControl(false);
				}
			}
			form.Dispose();
		}

		private void projectOverviewCoolToolStripButton_Click(object sender, EventArgs e) {
			ProductOverviewForm form = new ProductOverviewForm(false);
			form.ShowDialog();
			if (form.UnsavedChanges) {
				projectUnsaved = true;
				UpdateTitle();
			}
			if (form.ProductToEdit != null) {
				TreeNode node = Project.Instance.FindNode(form.ProductToEdit);
				this.projectTree.SelectedNode = node;
			} else {
				if (currentEditorUserControl != null) {
					currentEditorUserControl.UpdateControl(false);
				}
			}
			form.Dispose();
		}

		private void auslegeAssistentButton_Click(object sender, EventArgs e) {
			AuslegeAssistentForm form = new AuslegeAssistentForm();

			form.ShowDialog();

			form.Dispose();
		}

		private void warningsAndErrorsToolStripMenuItem_Click(object sender, EventArgs e) {
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct p in room.PlannedProducts) {
						p.ConfigureProduct(false);
					}
				}
			}
			WarningsAndErrorsForm form = new WarningsAndErrorsForm();
			form.ShowDialog();
			form.Dispose();
		}

		private void inhaltToolStripMenuItem_Click(object sender, EventArgs e) {
			Help.ShowHelp(this, "europlan.chm");
		}

		private void tutorialToolStripMenuItem_Click(object sender, EventArgs e) {
			Help.ShowHelp(this, "tutorial.chm");
		}

	}
}