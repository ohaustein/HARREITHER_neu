namespace Europlan.Application {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.projectTree = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.demandedHeatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importGlobalConfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.datanormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.recentProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projektToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.updateController = new Kjs.AppLife.Update.Controller.UpdateController(this.components);
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.projectOverviewHeatToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.projectOverviewCoolToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.openGlobalConfDialog = new System.Windows.Forms.OpenFileDialog();
			this.auslegeAssistentButton = new System.Windows.Forms.ToolStripButton();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.mainMenu.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.splitContainer, "splitContainer");
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.projectTree);
			// 
			// splitContainer.Panel2
			// 
			resources.ApplyResources(this.splitContainer.Panel2, "splitContainer.Panel2");
			// 
			// projectTree
			// 
			this.projectTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.projectTree, "projectTree");
			this.projectTree.FullRowSelect = true;
			this.projectTree.HideSelection = false;
			this.projectTree.Name = "projectTree";
			this.projectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projectTree_AfterSelect);
			this.projectTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.projectTree_BeforeSelect);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "default.png");
			this.imageList.Images.SetKeyName(1, "kfm_home-alt.png");
			this.imageList.Images.SetKeyName(2, "kontact_journal.png");
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.projektToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
			resources.ApplyResources(this.mainMenu, "mainMenu");
			this.mainMenu.Name = "mainMenu";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator4,
            this.importToolStripMenuItem,
            this.toolStripSeparator5,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.recentProjectsToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			// 
			// importToolStripMenuItem
			// 
			this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.demandedHeatToolStripMenuItem,
            this.importGlobalConfToolStripMenuItem,
            this.datanormToolStripMenuItem});
			this.importToolStripMenuItem.Name = "importToolStripMenuItem";
			resources.ApplyResources(this.importToolStripMenuItem, "importToolStripMenuItem");
			// 
			// demandedHeatToolStripMenuItem
			// 
			this.demandedHeatToolStripMenuItem.Name = "demandedHeatToolStripMenuItem";
			resources.ApplyResources(this.demandedHeatToolStripMenuItem, "demandedHeatToolStripMenuItem");
			this.demandedHeatToolStripMenuItem.Click += new System.EventHandler(this.demandedHeatToolStripMenuItem_Click);
			// 
			// importGlobalConfToolStripMenuItem
			// 
			this.importGlobalConfToolStripMenuItem.Name = "importGlobalConfToolStripMenuItem";
			resources.ApplyResources(this.importGlobalConfToolStripMenuItem, "importGlobalConfToolStripMenuItem");
			this.importGlobalConfToolStripMenuItem.Click += new System.EventHandler(this.importGlobalConfToolStripMenuItem_Click);
			// 
			// datanormToolStripMenuItem
			// 
			this.datanormToolStripMenuItem.Name = "datanormToolStripMenuItem";
			resources.ApplyResources(this.datanormToolStripMenuItem, "datanormToolStripMenuItem");
			this.datanormToolStripMenuItem.Click += new System.EventHandler(this.datanormToolStripMenuItem_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// recentProjectsToolStripMenuItem
			// 
			resources.ApplyResources(this.recentProjectsToolStripMenuItem, "recentProjectsToolStripMenuItem");
			this.recentProjectsToolStripMenuItem.Name = "recentProjectsToolStripMenuItem";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			// 
			// projektToolStripMenuItem
			// 
			this.projektToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewReportToolStripMenuItem});
			this.projektToolStripMenuItem.Name = "projektToolStripMenuItem";
			resources.ApplyResources(this.projektToolStripMenuItem, "projektToolStripMenuItem");
			// 
			// viewReportToolStripMenuItem
			// 
			this.viewReportToolStripMenuItem.Name = "viewReportToolStripMenuItem";
			resources.ApplyResources(this.viewReportToolStripMenuItem, "viewReportToolStripMenuItem");
			this.viewReportToolStripMenuItem.Click += new System.EventHandler(this.viewReportToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.licenseToolStripMenuItem,
            this.settingsToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
			// 
			// licenseToolStripMenuItem
			// 
			this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
			resources.ApplyResources(this.licenseToolStripMenuItem, "licenseToolStripMenuItem");
			this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.updateToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
			// 
			// infoToolStripMenuItem
			// 
			this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
			resources.ApplyResources(this.infoToolStripMenuItem, "infoToolStripMenuItem");
			// 
			// updateToolStripMenuItem
			// 
			this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
			resources.ApplyResources(this.updateToolStripMenuItem, "updateToolStripMenuItem");
			this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
			// 
			// updateController
			// 
			this.updateController.ApplicationId = new System.Guid("1f24573e-033c-448d-8e15-9f396e67e44e");
			this.updateController.BypassProxyOnLocal = true;
			this.updateController.UpdateLocation = "http://helios.bluesource.at/EuroplanUpdates/";
			this.updateController.UseHostAssemblyVersion = true;
			this.updateController.Version = ((System.Version)(resources.GetObject("updateController.Version")));
			this.updateController.CheckForUpdateCompleted += new Kjs.AppLife.Update.Controller.CheckForUpdateCompletedEventHandler(this.updateController_CheckForUpdateCompleted);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator1,
            this.projectOverviewHeatToolStripButton,
            this.projectOverviewCoolToolStripButton,
            this.auslegeAssistentButton,
            this.toolStripSeparator6,
            this.helpToolStripButton});
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Name = "toolStrip1";
			// 
			// newToolStripButton
			// 
			this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.newToolStripButton, "newToolStripButton");
			this.newToolStripButton.Name = "newToolStripButton";
			this.newToolStripButton.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripButton
			// 
			this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.openToolStripButton, "openToolStripButton");
			this.openToolStripButton.Name = "openToolStripButton";
			this.openToolStripButton.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripButton
			// 
			this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.saveToolStripButton, "saveToolStripButton");
			this.saveToolStripButton.Name = "saveToolStripButton";
			this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// printToolStripButton
			// 
			this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.printToolStripButton, "printToolStripButton");
			this.printToolStripButton.Name = "printToolStripButton";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
			// 
			// cutToolStripButton
			// 
			this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.cutToolStripButton, "cutToolStripButton");
			this.cutToolStripButton.Name = "cutToolStripButton";
			// 
			// copyToolStripButton
			// 
			this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.copyToolStripButton, "copyToolStripButton");
			this.copyToolStripButton.Name = "copyToolStripButton";
			// 
			// pasteToolStripButton
			// 
			this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.pasteToolStripButton, "pasteToolStripButton");
			this.pasteToolStripButton.Name = "pasteToolStripButton";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// projectOverviewHeatToolStripButton
			// 
			this.projectOverviewHeatToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.projectOverviewHeatToolStripButton, "projectOverviewHeatToolStripButton");
			this.projectOverviewHeatToolStripButton.Name = "projectOverviewHeatToolStripButton";
			this.projectOverviewHeatToolStripButton.Click += new System.EventHandler(this.projectOverviewHeatToolStripButton_Click);
			// 
			// projectOverviewCoolToolStripButton
			// 
			this.projectOverviewCoolToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.projectOverviewCoolToolStripButton, "projectOverviewCoolToolStripButton");
			this.projectOverviewCoolToolStripButton.Name = "projectOverviewCoolToolStripButton";
			this.projectOverviewCoolToolStripButton.Click += new System.EventHandler(this.projectOverviewCoolToolStripButton_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
			// 
			// helpToolStripButton
			// 
			this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.helpToolStripButton, "helpToolStripButton");
			this.helpToolStripButton.Name = "helpToolStripButton";
			// 
			// statusStrip
			// 
			resources.ApplyResources(this.statusStrip, "statusStrip");
			this.statusStrip.Name = "statusStrip";
			// 
			// openGlobalConfDialog
			// 
			resources.ApplyResources(this.openGlobalConfDialog, "openGlobalConfDialog");
			// 
			// auslegeAssistentButton
			// 
			this.auslegeAssistentButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.auslegeAssistentButton, "auslegeAssistentButton");
			this.auslegeAssistentButton.Name = "auslegeAssistentButton";
			this.auslegeAssistentButton.Click += new System.EventHandler(this.auslegeAssistentButton_Click);
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.mainMenu);
			this.MainMenuStrip = this.mainMenu;
			this.Name = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.ResumeLayout(false);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private Kjs.AppLife.Update.Controller.UpdateController updateController;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton newToolStripButton;
		private System.Windows.Forms.ToolStripButton openToolStripButton;
		private System.Windows.Forms.ToolStripButton saveToolStripButton;
		private System.Windows.Forms.ToolStripButton printToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripButton cutToolStripButton;
		private System.Windows.Forms.ToolStripButton copyToolStripButton;
		private System.Windows.Forms.ToolStripButton pasteToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton helpToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.TreeView projectTree;
		private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem demandedHeatToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importGlobalConfToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openGlobalConfDialog;
		private System.Windows.Forms.ToolStripMenuItem datanormToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem recentProjectsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem projektToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewReportToolStripMenuItem;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ToolStripButton projectOverviewHeatToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton projectOverviewCoolToolStripButton;
		private System.Windows.Forms.ToolStripButton auslegeAssistentButton;

	}
}

