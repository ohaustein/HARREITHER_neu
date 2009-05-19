namespace Europlan.Application {
	partial class MaterialForm {
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
			this.materialEditorGrid1 = new Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabPageFloor = new System.Windows.Forms.TabPage();
			this.tabPageWall = new System.Windows.Forms.TabPage();
			this.materialEditorGrid2 = new Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid();
			this.tabPageCeiling = new System.Windows.Forms.TabPage();
			this.materialEditorGrid3 = new Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid();
			this.tabPageDistributor = new System.Windows.Forms.TabPage();
			this.materialEditorGrid4 = new Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid();
			this.tabPageInsulation = new System.Windows.Forms.TabPage();
			this.materialEditorGrid5 = new Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid();
			this.tabPageGeneral = new System.Windows.Forms.TabPage();
			this.materialEditorGrid6 = new Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid();
			this.tabs.SuspendLayout();
			this.tabPageFloor.SuspendLayout();
			this.tabPageWall.SuspendLayout();
			this.tabPageCeiling.SuspendLayout();
			this.tabPageDistributor.SuspendLayout();
			this.tabPageInsulation.SuspendLayout();
			this.tabPageGeneral.SuspendLayout();
			this.SuspendLayout();
			// 
			// materialEditorGrid1
			// 
			this.materialEditorGrid1.AllowToAdd = false;
			this.materialEditorGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialEditorGrid1.Filter = Europlan.Common.CategoryType.Floor;
			this.materialEditorGrid1.Location = new System.Drawing.Point(3, 3);
			this.materialEditorGrid1.Name = "materialEditorGrid1";
			this.materialEditorGrid1.Size = new System.Drawing.Size(678, 494);
			this.materialEditorGrid1.TabIndex = 0;
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.tabPageFloor);
			this.tabs.Controls.Add(this.tabPageWall);
			this.tabs.Controls.Add(this.tabPageCeiling);
			this.tabs.Controls.Add(this.tabPageDistributor);
			this.tabs.Controls.Add(this.tabPageInsulation);
			this.tabs.Controls.Add(this.tabPageGeneral);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(692, 526);
			this.tabs.TabIndex = 1;
			// 
			// tabPageFloor
			// 
			this.tabPageFloor.Controls.Add(this.materialEditorGrid1);
			this.tabPageFloor.Location = new System.Drawing.Point(4, 22);
			this.tabPageFloor.Name = "tabPageFloor";
			this.tabPageFloor.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageFloor.Size = new System.Drawing.Size(684, 500);
			this.tabPageFloor.TabIndex = 0;
			this.tabPageFloor.Text = "Fuﬂboden";
			this.tabPageFloor.UseVisualStyleBackColor = true;
			// 
			// tabPageWall
			// 
			this.tabPageWall.Controls.Add(this.materialEditorGrid2);
			this.tabPageWall.Location = new System.Drawing.Point(4, 22);
			this.tabPageWall.Name = "tabPageWall";
			this.tabPageWall.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageWall.Size = new System.Drawing.Size(684, 500);
			this.tabPageWall.TabIndex = 1;
			this.tabPageWall.Text = "Wand";
			this.tabPageWall.UseVisualStyleBackColor = true;
			// 
			// materialEditorGrid2
			// 
			this.materialEditorGrid2.AllowToAdd = false;
			this.materialEditorGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialEditorGrid2.Filter = Europlan.Common.CategoryType.Wall;
			this.materialEditorGrid2.Location = new System.Drawing.Point(3, 3);
			this.materialEditorGrid2.Name = "materialEditorGrid2";
			this.materialEditorGrid2.Size = new System.Drawing.Size(678, 494);
			this.materialEditorGrid2.TabIndex = 1;
			// 
			// tabPageCeiling
			// 
			this.tabPageCeiling.Controls.Add(this.materialEditorGrid3);
			this.tabPageCeiling.Location = new System.Drawing.Point(4, 22);
			this.tabPageCeiling.Name = "tabPageCeiling";
			this.tabPageCeiling.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCeiling.Size = new System.Drawing.Size(684, 500);
			this.tabPageCeiling.TabIndex = 2;
			this.tabPageCeiling.Text = "Decke";
			this.tabPageCeiling.UseVisualStyleBackColor = true;
			// 
			// materialEditorGrid3
			// 
			this.materialEditorGrid3.AllowToAdd = false;
			this.materialEditorGrid3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialEditorGrid3.Filter = Europlan.Common.CategoryType.Ceiling;
			this.materialEditorGrid3.Location = new System.Drawing.Point(3, 3);
			this.materialEditorGrid3.Name = "materialEditorGrid3";
			this.materialEditorGrid3.Size = new System.Drawing.Size(678, 494);
			this.materialEditorGrid3.TabIndex = 1;
			// 
			// tabPageDistributor
			// 
			this.tabPageDistributor.Controls.Add(this.materialEditorGrid4);
			this.tabPageDistributor.Location = new System.Drawing.Point(4, 22);
			this.tabPageDistributor.Name = "tabPageDistributor";
			this.tabPageDistributor.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageDistributor.Size = new System.Drawing.Size(684, 500);
			this.tabPageDistributor.TabIndex = 3;
			this.tabPageDistributor.Text = "Verteiler";
			this.tabPageDistributor.UseVisualStyleBackColor = true;
			// 
			// materialEditorGrid4
			// 
			this.materialEditorGrid4.AllowToAdd = false;
			this.materialEditorGrid4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialEditorGrid4.Filter = Europlan.Common.CategoryType.Distributor;
			this.materialEditorGrid4.Location = new System.Drawing.Point(3, 3);
			this.materialEditorGrid4.Name = "materialEditorGrid4";
			this.materialEditorGrid4.Size = new System.Drawing.Size(678, 494);
			this.materialEditorGrid4.TabIndex = 1;
			// 
			// tabPageInsulation
			// 
			this.tabPageInsulation.Controls.Add(this.materialEditorGrid5);
			this.tabPageInsulation.Location = new System.Drawing.Point(4, 22);
			this.tabPageInsulation.Name = "tabPageInsulation";
			this.tabPageInsulation.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageInsulation.Size = new System.Drawing.Size(684, 500);
			this.tabPageInsulation.TabIndex = 4;
			this.tabPageInsulation.Text = "D‰mmung";
			this.tabPageInsulation.UseVisualStyleBackColor = true;
			// 
			// materialEditorGrid5
			// 
			this.materialEditorGrid5.AllowToAdd = true;
			this.materialEditorGrid5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialEditorGrid5.Filter = Europlan.Common.CategoryType.Insulation;
			this.materialEditorGrid5.Location = new System.Drawing.Point(3, 3);
			this.materialEditorGrid5.Name = "materialEditorGrid5";
			this.materialEditorGrid5.Size = new System.Drawing.Size(678, 494);
			this.materialEditorGrid5.TabIndex = 1;
			// 
			// tabPageGeneral
			// 
			this.tabPageGeneral.Controls.Add(this.materialEditorGrid6);
			this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabPageGeneral.Name = "tabPageGeneral";
			this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageGeneral.Size = new System.Drawing.Size(684, 500);
			this.tabPageGeneral.TabIndex = 5;
			this.tabPageGeneral.Text = "Allgemein";
			this.tabPageGeneral.UseVisualStyleBackColor = true;
			// 
			// materialEditorGrid6
			// 
			this.materialEditorGrid6.AllowToAdd = true;
			this.materialEditorGrid6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialEditorGrid6.Filter = Europlan.Common.CategoryType.General;
			this.materialEditorGrid6.Location = new System.Drawing.Point(3, 3);
			this.materialEditorGrid6.Name = "materialEditorGrid6";
			this.materialEditorGrid6.Size = new System.Drawing.Size(678, 494);
			this.materialEditorGrid6.TabIndex = 1;
			// 
			// MaterialForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(692, 526);
			this.Controls.Add(this.tabs);
			this.Name = "MaterialForm";
			this.Text = "MaterialForm";
			this.tabs.ResumeLayout(false);
			this.tabPageFloor.ResumeLayout(false);
			this.tabPageWall.ResumeLayout(false);
			this.tabPageCeiling.ResumeLayout(false);
			this.tabPageDistributor.ResumeLayout(false);
			this.tabPageInsulation.ResumeLayout(false);
			this.tabPageGeneral.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid materialEditorGrid1;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabPageFloor;
		private System.Windows.Forms.TabPage tabPageWall;
		private Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid materialEditorGrid2;
		private System.Windows.Forms.TabPage tabPageCeiling;
		private Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid materialEditorGrid3;
		private System.Windows.Forms.TabPage tabPageDistributor;
		private Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid materialEditorGrid4;
		private System.Windows.Forms.TabPage tabPageInsulation;
		private Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid materialEditorGrid5;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private Europlan.AdminApplication.ContructionEditor.MaterialEditorGrid materialEditorGrid6;
	}
}