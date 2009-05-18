namespace Europlan.AdminApplication {
	partial class MaterialMapper {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Windows.Forms.ListViewGroup listViewGroup19 = new System.Windows.Forms.ListViewGroup("Boden", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup20 = new System.Windows.Forms.ListViewGroup("Wand", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup21 = new System.Windows.Forms.ListViewGroup("Decke", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup22 = new System.Windows.Forms.ListViewGroup("Verteiler", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup23 = new System.Windows.Forms.ListViewGroup("Dämmung", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup24 = new System.Windows.Forms.ListViewGroup("Allgemein", System.Windows.Forms.HorizontalAlignment.Left);
			this.listUncategorizedMaterials = new System.Windows.Forms.ListView();
			this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderPartNumber = new System.Windows.Forms.ColumnHeader();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.txtCategoryName = new System.Windows.Forms.TextBox();
			this.listCategories = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbRootCategories = new System.Windows.Forms.ComboBox();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// listUncategorizedMaterials
			// 
			this.listUncategorizedMaterials.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listUncategorizedMaterials.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderPartNumber});
			this.listUncategorizedMaterials.FullRowSelect = true;
			this.listUncategorizedMaterials.GridLines = true;
			listViewGroup19.Header = "Boden";
			listViewGroup19.Name = "listViewGroupFloor";
			listViewGroup20.Header = "Wand";
			listViewGroup20.Name = "listViewGroupWall";
			listViewGroup21.Header = "Decke";
			listViewGroup21.Name = "listViewGroupCeiling";
			listViewGroup22.Header = "Verteiler";
			listViewGroup22.Name = "listViewGroupDistributor";
			listViewGroup23.Header = "Dämmung";
			listViewGroup23.Name = "listViewGroupInsulation";
			listViewGroup24.Header = "Allgemein";
			listViewGroup24.Name = "listViewGroupGeneral";
			this.listUncategorizedMaterials.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup19,
            listViewGroup20,
            listViewGroup21,
            listViewGroup22,
            listViewGroup23,
            listViewGroup24});
			this.listUncategorizedMaterials.Location = new System.Drawing.Point(3, 23);
			this.listUncategorizedMaterials.MultiSelect = false;
			this.listUncategorizedMaterials.Name = "listUncategorizedMaterials";
			this.listUncategorizedMaterials.ShowGroups = false;
			this.listUncategorizedMaterials.Size = new System.Drawing.Size(268, 561);
			this.listUncategorizedMaterials.TabIndex = 0;
			this.listUncategorizedMaterials.UseCompatibleStateImageBehavior = false;
			this.listUncategorizedMaterials.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderName
			// 
			this.columnHeaderName.DisplayIndex = 1;
			this.columnHeaderName.Text = "Name";
			this.columnHeaderName.Width = 97;
			// 
			// columnHeaderPartNumber
			// 
			this.columnHeaderPartNumber.DisplayIndex = 0;
			this.columnHeaderPartNumber.Text = "Bestellnr.";
			this.columnHeaderPartNumber.Width = 73;
			// 
			// splitContainer
			// 
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.label1);
			this.splitContainer.Panel1.Controls.Add(this.listUncategorizedMaterials);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.btnDown);
			this.splitContainer.Panel2.Controls.Add(this.btnUp);
			this.splitContainer.Panel2.Controls.Add(this.btnRemove);
			this.splitContainer.Panel2.Controls.Add(this.btnAdd);
			this.splitContainer.Panel2.Controls.Add(this.txtCategoryName);
			this.splitContainer.Panel2.Controls.Add(this.listCategories);
			this.splitContainer.Panel2.Controls.Add(this.label3);
			this.splitContainer.Panel2.Controls.Add(this.label2);
			this.splitContainer.Panel2.Controls.Add(this.cmbRootCategories);
			this.splitContainer.Size = new System.Drawing.Size(824, 588);
			this.splitContainer.SplitterDistance = 278;
			this.splitContainer.TabIndex = 1;
			this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(209, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Artikel ohne Zuordnung zu einer Kategorie:";
			// 
			// btnDown
			// 
			this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDown.Enabled = false;
			this.btnDown.Location = new System.Drawing.Point(433, 127);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(102, 23);
			this.btnDown.TabIndex = 8;
			this.btnDown.Text = "Ab";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// btnUp
			// 
			this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUp.Enabled = false;
			this.btnUp.Location = new System.Drawing.Point(433, 98);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(102, 23);
			this.btnUp.TabIndex = 7;
			this.btnUp.Text = "Auf";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.Enabled = false;
			this.btnRemove.Location = new System.Drawing.Point(433, 69);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(102, 23);
			this.btnRemove.TabIndex = 6;
			this.btnRemove.Text = "Löschen";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(433, 43);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(102, 23);
			this.btnAdd.TabIndex = 5;
			this.btnAdd.Text = "Neu";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// txtCategoryName
			// 
			this.txtCategoryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCategoryName.Enabled = false;
			this.txtCategoryName.Location = new System.Drawing.Point(90, 43);
			this.txtCategoryName.Name = "txtCategoryName";
			this.txtCategoryName.Size = new System.Drawing.Size(336, 20);
			this.txtCategoryName.TabIndex = 4;
			this.txtCategoryName.TextChanged += new System.EventHandler(this.txtCategoryName_TextChanged);
			// 
			// listCategories
			// 
			this.listCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listCategories.FormattingEnabled = true;
			this.listCategories.Location = new System.Drawing.Point(90, 69);
			this.listCategories.Name = "listCategories";
			this.listCategories.ScrollAlwaysVisible = true;
			this.listCategories.Size = new System.Drawing.Size(337, 82);
			this.listCategories.TabIndex = 3;
			this.listCategories.SelectedIndexChanged += new System.EventHandler(this.listCategories_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(4, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Unterkategorie:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Hauptkategorie:";
			// 
			// cmbRootCategories
			// 
			this.cmbRootCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbRootCategories.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbRootCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRootCategories.FormattingEnabled = true;
			this.cmbRootCategories.Items.AddRange(new object[] {
            "Boden",
            "Wand",
            "Decke",
            "Verteiler",
            "Dämmung",
            "Allgemein"});
			this.cmbRootCategories.Location = new System.Drawing.Point(93, 5);
			this.cmbRootCategories.Name = "cmbRootCategories";
			this.cmbRootCategories.Size = new System.Drawing.Size(442, 21);
			this.cmbRootCategories.TabIndex = 0;
			this.cmbRootCategories.SelectedIndexChanged += new System.EventHandler(this.cmbRootCategories_SelectedIndexChanged);
			// 
			// MaterialMapper
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer);
			this.Name = "MaterialMapper";
			this.Size = new System.Drawing.Size(824, 588);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel1.PerformLayout();
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.Panel2.PerformLayout();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listUncategorizedMaterials;
		private System.Windows.Forms.ColumnHeader columnHeaderName;
		private System.Windows.Forms.ColumnHeader columnHeaderPartNumber;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbRootCategories;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.TextBox txtCategoryName;
		private System.Windows.Forms.ListBox listCategories;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Button btnUp;


	}
}
