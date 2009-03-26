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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btnNewLicense = new System.Windows.Forms.Button();
			this.lstLicenses = new System.Windows.Forms.ListView();
			this.colLicensedTo = new System.Windows.Forms.ColumnHeader();
			this.colValidUntil = new System.Windows.Forms.ColumnHeader();
			this.btnSaveLicense = new System.Windows.Forms.Button();
			this.licenseEditor1 = new Europlan.AdminApplication.LicenseEditor();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 24);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(862, 580);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.splitContainer1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(854, 554);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Lizenzen";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.btnNewLicense);
			this.splitContainer1.Panel1.Controls.Add(this.lstLicenses);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.AutoScroll = true;
			this.splitContainer1.Panel2.AutoScrollMinSize = new System.Drawing.Size(400, 0);
			this.splitContainer1.Panel2.Controls.Add(this.btnSaveLicense);
			this.splitContainer1.Panel2.Controls.Add(this.licenseEditor1);
			this.splitContainer1.Size = new System.Drawing.Size(848, 548);
			this.splitContainer1.SplitterDistance = 281;
			this.splitContainer1.TabIndex = 0;
			// 
			// btnNewLicense
			// 
			this.btnNewLicense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnNewLicense.Location = new System.Drawing.Point(3, 522);
			this.btnNewLicense.Name = "btnNewLicense";
			this.btnNewLicense.Size = new System.Drawing.Size(275, 23);
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
			this.lstLicenses.Size = new System.Drawing.Size(275, 513);
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
			this.btnSaveLicense.Location = new System.Drawing.Point(429, 522);
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
			this.licenseEditor1.License = null;
			this.licenseEditor1.Location = new System.Drawing.Point(3, 3);
			this.licenseEditor1.Name = "licenseEditor1";
			this.licenseEditor1.Size = new System.Drawing.Size(557, 513);
			this.licenseEditor1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(854, 578);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(862, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// dateiToolStripMenuItem
			// 
			this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beendenToolStripMenuItem});
			this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
			this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			this.dateiToolStripMenuItem.Text = "&Datei";
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
			this.ClientSize = new System.Drawing.Size(862, 604);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.menuStrip1);
			this.Name = "MainForm";
			this.Text = "Europlan Admin";
			this.tabControl1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListView lstLicenses;
		private System.Windows.Forms.Button btnNewLicense;
		private LicenseEditor licenseEditor1;
		private System.Windows.Forms.Button btnSaveLicense;
		private System.Windows.Forms.ColumnHeader colLicensedTo;
		private System.Windows.Forms.ColumnHeader colValidUntil;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
	}
}

