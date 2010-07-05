namespace Europlan.AdminApplication {
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
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.tabPageLicenses = new System.Windows.Forms.TabPage();
			this.splitContainerLicenses = new System.Windows.Forms.SplitContainer();
			this.btnNewLicense = new System.Windows.Forms.Button();
			this.lstLicenses = new System.Windows.Forms.ListView();
			this.colLicensedTo = new System.Windows.Forms.ColumnHeader();
			this.colValidUntil = new System.Windows.Forms.ColumnHeader();
			this.btnSaveLicense = new System.Windows.Forms.Button();
			this.licenseEditor1 = new Europlan.AdminApplication.LicenseEditor();
			this.tabPageArticles = new System.Windows.Forms.TabPage();
			this.materialMapper1 = new Europlan.AdminApplication.MaterialMapper();
			this.tabPageAdditionalArticles = new System.Windows.Forms.TabPage();
			this.materialEditorGrid1 = new Europlan.Common.MaterialEditorGrid();
			this.tabPageConstructions = new System.Windows.Forms.TabPage();
			this.constructionEditorPage = new Europlan.Common.ConstructionEditorGrid();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.datanormDateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.artikelUndKostruktionenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainTabControl.SuspendLayout();
			this.tabPageLicenses.SuspendLayout();
			this.splitContainerLicenses.Panel1.SuspendLayout();
			this.splitContainerLicenses.Panel2.SuspendLayout();
			this.splitContainerLicenses.SuspendLayout();
			this.tabPageArticles.SuspendLayout();
			this.tabPageAdditionalArticles.SuspendLayout();
			this.tabPageConstructions.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTabControl
			// 
			this.mainTabControl.Controls.Add(this.tabPageLicenses);
			this.mainTabControl.Controls.Add(this.tabPageArticles);
			this.mainTabControl.Controls.Add(this.tabPageAdditionalArticles);
			this.mainTabControl.Controls.Add(this.tabPageConstructions);
			this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTabControl.Location = new System.Drawing.Point(0, 24);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(787, 489);
			this.mainTabControl.TabIndex = 0;
			// 
			// tabPageLicenses
			// 
			this.tabPageLicenses.Controls.Add(this.splitContainerLicenses);
			this.tabPageLicenses.Location = new System.Drawing.Point(4, 22);
			this.tabPageLicenses.Name = "tabPageLicenses";
			this.tabPageLicenses.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageLicenses.Size = new System.Drawing.Size(779, 463);
			this.tabPageLicenses.TabIndex = 1;
			this.tabPageLicenses.Text = "Lizenzen";
			this.tabPageLicenses.UseVisualStyleBackColor = true;
			// 
			// splitContainerLicenses
			// 
			this.splitContainerLicenses.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainerLicenses.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerLicenses.Location = new System.Drawing.Point(3, 3);
			this.splitContainerLicenses.Name = "splitContainerLicenses";
			// 
			// splitContainerLicenses.Panel1
			// 
			this.splitContainerLicenses.Panel1.Controls.Add(this.btnNewLicense);
			this.splitContainerLicenses.Panel1.Controls.Add(this.lstLicenses);
			// 
			// splitContainerLicenses.Panel2
			// 
			this.splitContainerLicenses.Panel2.AutoScroll = true;
			this.splitContainerLicenses.Panel2.AutoScrollMinSize = new System.Drawing.Size(400, 0);
			this.splitContainerLicenses.Panel2.Controls.Add(this.btnSaveLicense);
			this.splitContainerLicenses.Panel2.Controls.Add(this.licenseEditor1);
			this.splitContainerLicenses.Size = new System.Drawing.Size(773, 457);
			this.splitContainerLicenses.SplitterDistance = 207;
			this.splitContainerLicenses.TabIndex = 0;
			// 
			// btnNewLicense
			// 
			this.btnNewLicense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnNewLicense.Location = new System.Drawing.Point(3, 427);
			this.btnNewLicense.Name = "btnNewLicense";
			this.btnNewLicense.Size = new System.Drawing.Size(197, 23);
			this.btnNewLicense.TabIndex = 1;
			this.btnNewLicense.Text = "Neue Lizenz";
			this.btnNewLicense.UseVisualStyleBackColor = true;
			this.btnNewLicense.Click += new System.EventHandler(this.btnNewLicense_Click);
			// 
			// lstLicenses
			// 
			this.lstLicenses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstLicenses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colLicensedTo,
            this.colValidUntil});
			this.lstLicenses.FullRowSelect = true;
			this.lstLicenses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstLicenses.HideSelection = false;
			this.lstLicenses.LabelWrap = false;
			this.lstLicenses.Location = new System.Drawing.Point(3, 3);
			this.lstLicenses.MultiSelect = false;
			this.lstLicenses.Name = "lstLicenses";
			this.lstLicenses.ShowGroups = false;
			this.lstLicenses.Size = new System.Drawing.Size(197, 418);
			this.lstLicenses.TabIndex = 0;
			this.lstLicenses.UseCompatibleStateImageBehavior = false;
			this.lstLicenses.View = System.Windows.Forms.View.Details;
			this.lstLicenses.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstLicenses_ItemSelectionChanged);
			// 
			// colLicensedTo
			// 
			this.colLicensedTo.Text = "Lizenziert für";
			this.colLicensedTo.Width = 150;
			// 
			// colValidUntil
			// 
			this.colValidUntil.Text = "Gültig bis";
			this.colValidUntil.Width = 100;
			// 
			// btnSaveLicense
			// 
			this.btnSaveLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveLicense.Location = new System.Drawing.Point(424, 427);
			this.btnSaveLicense.Name = "btnSaveLicense";
			this.btnSaveLicense.Size = new System.Drawing.Size(131, 23);
			this.btnSaveLicense.TabIndex = 1;
			this.btnSaveLicense.Text = "Lizenz erzeugen";
			this.btnSaveLicense.UseVisualStyleBackColor = true;
			this.btnSaveLicense.Click += new System.EventHandler(this.btnSaveLicense_Click);
			// 
			// licenseEditor1
			// 
			this.licenseEditor1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.licenseEditor1.AutoScroll = true;
			this.licenseEditor1.AutoScrollMinSize = new System.Drawing.Size(400, 0);
			this.licenseEditor1.Enabled = false;
			this.licenseEditor1.Location = new System.Drawing.Point(3, 3);
			this.licenseEditor1.Name = "licenseEditor1";
			this.licenseEditor1.Size = new System.Drawing.Size(552, 418);
			this.licenseEditor1.TabIndex = 0;
			// 
			// tabPageArticles
			// 
			this.tabPageArticles.Controls.Add(this.materialMapper1);
			this.tabPageArticles.Location = new System.Drawing.Point(4, 22);
			this.tabPageArticles.Name = "tabPageArticles";
			this.tabPageArticles.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageArticles.Size = new System.Drawing.Size(779, 463);
			this.tabPageArticles.TabIndex = 2;
			this.tabPageArticles.Text = "Artikelstamm";
			this.tabPageArticles.UseVisualStyleBackColor = true;
			// 
			// materialMapper1
			// 
			this.materialMapper1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialMapper1.Location = new System.Drawing.Point(3, 3);
			this.materialMapper1.Name = "materialMapper1";
			this.materialMapper1.Size = new System.Drawing.Size(773, 457);
			this.materialMapper1.TabIndex = 0;
			// 
			// tabPageAdditionalArticles
			// 
			this.tabPageAdditionalArticles.Controls.Add(this.materialEditorGrid1);
			this.tabPageAdditionalArticles.Location = new System.Drawing.Point(4, 22);
			this.tabPageAdditionalArticles.Name = "tabPageAdditionalArticles";
			this.tabPageAdditionalArticles.Size = new System.Drawing.Size(779, 463);
			this.tabPageAdditionalArticles.TabIndex = 4;
			this.tabPageAdditionalArticles.Text = "Zusätzliche Artikel";
			this.tabPageAdditionalArticles.UseVisualStyleBackColor = true;
			// 
			// materialEditorGrid1
			// 
			this.materialEditorGrid1.Admin = true;
			this.materialEditorGrid1.AllowToAdd = true;
			this.materialEditorGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialEditorGrid1.Filter = null;
			this.materialEditorGrid1.Location = new System.Drawing.Point(0, 0);
			this.materialEditorGrid1.Name = "materialEditorGrid1";
			this.materialEditorGrid1.ShowOnlyAdditional = true;
			this.materialEditorGrid1.Size = new System.Drawing.Size(779, 463);
			this.materialEditorGrid1.TabIndex = 0;
			// 
			// tabPageConstructions
			// 
			this.tabPageConstructions.Controls.Add(this.constructionEditorPage);
			this.tabPageConstructions.Location = new System.Drawing.Point(4, 22);
			this.tabPageConstructions.Name = "tabPageConstructions";
			this.tabPageConstructions.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageConstructions.Size = new System.Drawing.Size(779, 463);
			this.tabPageConstructions.TabIndex = 3;
			this.tabPageConstructions.Text = "Konstruktionen";
			this.tabPageConstructions.UseVisualStyleBackColor = true;
			// 
			// constructionEditorPage
			// 
			this.constructionEditorPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.constructionEditorPage.Filter = Europlan.Common.ConstructionScopeEnum.All;
			this.constructionEditorPage.Location = new System.Drawing.Point(3, 3);
			this.constructionEditorPage.Name = "constructionEditorPage";
			this.constructionEditorPage.Size = new System.Drawing.Size(773, 460);
			this.constructionEditorPage.TabIndex = 0;
			this.constructionEditorPage.Type = Europlan.Common.Configuration.ConfigurationType.AdminConfiguration;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(787, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// dateiToolStripMenuItem
			// 
			this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.beendenToolStripMenuItem});
			this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
			this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			this.dateiToolStripMenuItem.Text = "&Datei";
			// 
			// importToolStripMenuItem
			// 
			this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.datanormDateiToolStripMenuItem});
			this.importToolStripMenuItem.Name = "importToolStripMenuItem";
			this.importToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.importToolStripMenuItem.Text = "Importieren";
			// 
			// datanormDateiToolStripMenuItem
			// 
			this.datanormDateiToolStripMenuItem.Name = "datanormDateiToolStripMenuItem";
			this.datanormDateiToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.datanormDateiToolStripMenuItem.Text = "Artikelliste...";
			this.datanormDateiToolStripMenuItem.Click += new System.EventHandler(this.datanormDateiToolStripMenuItem_Click);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.artikelUndKostruktionenToolStripMenuItem});
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.exportToolStripMenuItem.Text = "Exportieren";
			// 
			// artikelUndKostruktionenToolStripMenuItem
			// 
			this.artikelUndKostruktionenToolStripMenuItem.Name = "artikelUndKostruktionenToolStripMenuItem";
			this.artikelUndKostruktionenToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
			this.artikelUndKostruktionenToolStripMenuItem.Text = "Artikel und Kostruktionen...";
			this.artikelUndKostruktionenToolStripMenuItem.Click += new System.EventHandler(this.artikelUndKostruktionenToolStripMenuItem_Click);
			// 
			// beendenToolStripMenuItem
			// 
			this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
			this.beendenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.beendenToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.beendenToolStripMenuItem.Text = "&Beenden";
			this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(787, 513);
			this.Controls.Add(this.mainTabControl);
			this.Controls.Add(this.menuStrip1);
			this.Name = "MainForm";
			this.Text = "Europlan Admin";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.mainTabControl.ResumeLayout(false);
			this.tabPageLicenses.ResumeLayout(false);
			this.splitContainerLicenses.Panel1.ResumeLayout(false);
			this.splitContainerLicenses.Panel2.ResumeLayout(false);
			this.splitContainerLicenses.ResumeLayout(false);
			this.tabPageArticles.ResumeLayout(false);
			this.tabPageAdditionalArticles.ResumeLayout(false);
			this.tabPageConstructions.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage tabPageLicenses;
		private System.Windows.Forms.SplitContainer splitContainerLicenses;
		private System.Windows.Forms.ListView lstLicenses;
		private System.Windows.Forms.Button btnNewLicense;
		private LicenseEditor licenseEditor1;
		private System.Windows.Forms.Button btnSaveLicense;
		private System.Windows.Forms.ColumnHeader colLicensedTo;
		private System.Windows.Forms.ColumnHeader colValidUntil;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
		private System.Windows.Forms.TabPage tabPageArticles;
		private System.Windows.Forms.TabPage tabPageConstructions;
		private Europlan.Common.ConstructionEditorGrid constructionEditorPage;
		private MaterialMapper materialMapper1;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem artikelUndKostruktionenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem datanormDateiToolStripMenuItem;
		private System.Windows.Forms.TabPage tabPageAdditionalArticles;
		private Europlan.Common.MaterialEditorGrid materialEditorGrid1;
	}
}

