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

namespace Europlan.Application {
	public partial class MainForm : Form {
		
		private string projectFileName = null;
		private Project currentProject = null;

		private static readonly string defaultTitle = "Europlan";
		private string title;

		private Dictionary<Type, UserControl> userControls = new Dictionary<Type, UserControl>();
		private bool guiUpdateInProgress = false;
		private bool projectUnsaved = false;

		private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

		public MainForm() {
			InitializeComponent();

			this.updateController.ApplicationId = Program.updateGuid;
			this.updateController.UpdateLocation = Program.updateLocation;
			this.updateController.PublicKeyToken = Program.updatePublicKey;

			LicenseManager.Instance.LicenseChanged += new EventHandler(licenseManager_LicenseChanged);

			this.UpdateAvailableFeatures();
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
				this.title = MainForm.defaultTitle + " (" + resources.GetString("NotLicensed", Thread.CurrentThread.CurrentUICulture) + ")";
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
				string messageText = resources.GetString("UnsavedMessage", Thread.CurrentThread.CurrentUICulture);
				string caption = resources.GetString("UnsavedCaption", Thread.CurrentThread.CurrentUICulture);
				DialogResult result = MessageBox.Show(messageText, caption, MessageBoxButtons.YesNoCancel);
				if (result == DialogResult.Cancel) {
					return false;
				} else if (result == DialogResult.No) {
					return true;
				} else if (result == DialogResult.Yes) {
					string tempFileName = projectFileName;
					if (projectFileName == null) {
						SaveFileDialog dialog = new SaveFileDialog();
						dialog.CheckPathExists = true;
						dialog.DefaultExt = "e2p";
						dialog.Filter = "Europlan 2.0 (*.e2p)|*.e2p";
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

			Project.ProjectLoaded += new Project.ProjectLoadedHandler(myProject_ProjectLoaded);
			Project.ProjectSaved += new Project.ProjectSavedHandler(myProject_ProjectSaved);

			if (projectFileName != null) {
				LoadProject();
			} else {
				NewProject();
			}

			Project.Instance.InitializeTreeView(this.projectTree);

			if (!LicenseManager.Instance.LicenseFoundAndValid) {
				LicenseForm license = new LicenseForm();
				license.ShowDialog();
				license.Dispose();
			}
			this.updateController.CheckForUpdateAsync();
		}

		private void LoadProject() {
			try {
				if (projectFileName != null) {
					Project.Load(projectFileName);
					currentProject = Project.Instance;
				}
			} catch (Exception ex) {
				log.Error("Problem loading project:", ex);
				currentProject = null;
				projectFileName = null;
			}
		}

		private void SaveProject() {
			try {
				if (currentProject != null && projectFileName != null) {
					Project.Save(projectFileName);
				}
			} catch (Exception ex) {
				log.Error("Problem saving project:", ex);
			}
		}

		private void NewProject() {
			if (CheckForUnsavedChanges()) {
				if (currentProject == null) {
					currentProject = Project.New();
				} else {
					currentProject = Project.New();
				}
				projectFileName = null;
				projectUnsaved = false;
				UpdateTitle();
				Project.Instance.InitializeTreeView(this.projectTree);
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
				settings.StoreSetting("SplitterDistance", this.splitContainer.SplitterDistance);
				SettingsFile.Update();
			} else {
				e.Cancel = true;
			}
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			OptionsForm options = new OptionsForm();
			DialogResult result = options.ShowDialog();
			if (result == DialogResult.OK && options.RestartRequired) {
				string message = resources.GetString("RestartMessage", Thread.CurrentThread.CurrentUICulture);
				string caption = resources.GetString("RestartCaption", Thread.CurrentThread.CurrentUICulture);
				result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel);
				if (result == DialogResult.OK) {
					System.Windows.Forms.Application.Restart();
				}
			}
			options.Dispose();
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e) {
			this.updateController.UpdateInteractive();
		}

		private void updateController_CheckForUpdateCompleted(object sender, Kjs.AppLife.Update.Controller.CheckForUpdateCompletedEventArgs e) {
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
					title += resources.GetString("NewProjectTitle", Thread.CurrentThread.CurrentUICulture);
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
				dialog.Filter = "Europlan 2.0 (*.e2p)|*.e2p";
				dialog.Multiselect = false;
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					projectFileName = dialog.FileName;
					LoadProject();
				}
			}
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e) {
			NewProject();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			if (projectFileName == null) {
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.CheckPathExists = true;
				dialog.DefaultExt = "e2p";
				dialog.Filter = "Europlan 2.0 (*.e2p)|*.e2p";
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
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.CheckPathExists = true;
			dialog.DefaultExt = "e2p";
			dialog.Filter = "Europlan 2.0 (*.e2p)|*.e2p";
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				projectFileName = dialog.FileName;
				SaveProject();
			}
		}

		private void licenseToolStripMenuItem_Click(object sender, EventArgs e) {
			LicenseForm license = new LicenseForm();
			license.ShowDialog();
			license.Dispose();
		}

		private void projectTree_AfterSelect(object sender, TreeViewEventArgs e) {
			TreeNode selectedNode = e.Node;
			Control oldControl = null;
			if (splitContainer.Panel2.Controls.Count > 0) {
				oldControl = splitContainer.Panel2.Controls[0];
			}
			//splitContainer.Panel2.Controls.Clear();
			if (selectedNode != null && selectedNode.Tag != null) {
				UserControl control = null;
				if (selectedNode.Tag is IGuiRepresentation) {
					IGuiRepresentation guiRepresentation = selectedNode.Tag as IGuiRepresentation;
					if (userControls.ContainsKey(guiRepresentation.AssociatedPanelType)) {
						control = userControls[guiRepresentation.AssociatedPanelType];
					} else {
						control = (UserControl)Activator.CreateInstance(guiRepresentation.AssociatedPanelType);
						(control as IEditorUserControl).ProjectStructureChanged += new ProjectStructureChangedHandler(MainForm_ProjectStructureChanged);
						(control as IEditorUserControl).ProjectChanged += new ProjectChangedHandler(MainForm_ProjectChanged);
						(control as IEditorUserControl).TreeSelectionRequested += new TreeSelectionRequestedHandler(MainForm_TreeSelectionRequested);
						userControls[guiRepresentation.AssociatedPanelType] = control;
					}
					control.Tag = selectedNode.Tag;
				} else if (selectedNode.Tag is Type) {
					if (userControls.ContainsKey(selectedNode.Tag as Type)) {
						control = userControls[selectedNode.Tag as Type];
					} else {
						control = (UserControl)Activator.CreateInstance(selectedNode.Tag as Type);
						(control as IEditorUserControl).ProjectStructureChanged += new ProjectStructureChangedHandler(MainForm_ProjectStructureChanged);
						(control as IEditorUserControl).ProjectChanged += new ProjectChangedHandler(MainForm_ProjectChanged);
						(control as IEditorUserControl).TreeSelectionRequested += new TreeSelectionRequestedHandler(MainForm_TreeSelectionRequested);
						userControls[selectedNode.Tag as Type] = control;
					}
				}
				if (control != null) {
					guiUpdateInProgress = true;
					if (control != oldControl) {
						splitContainer.Panel2.Controls.Clear();
						splitContainer.Panel2.Controls.Add(control);
						control.Dock = DockStyle.Fill;
					}
					if (control.Tag != oldControl.Tag) {
						(control as IEditorUserControl).UpdateControl();
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
		}

	}
}