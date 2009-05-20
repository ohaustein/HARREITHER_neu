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
			this.megFloor = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabPageFloor = new System.Windows.Forms.TabPage();
			this.tabPageWall = new System.Windows.Forms.TabPage();
			this.megWall = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.tabPageCeiling = new System.Windows.Forms.TabPage();
			this.megCeiling = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.tabPageDistributor = new System.Windows.Forms.TabPage();
			this.megDistributor = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.tabPageInsulation = new System.Windows.Forms.TabPage();
			this.megInsulation = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.tabPageGeneral = new System.Windows.Forms.TabPage();
			this.megGeneral = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.tabs.SuspendLayout();
			this.tabPageFloor.SuspendLayout();
			this.tabPageWall.SuspendLayout();
			this.tabPageCeiling.SuspendLayout();
			this.tabPageDistributor.SuspendLayout();
			this.tabPageInsulation.SuspendLayout();
			this.tabPageGeneral.SuspendLayout();
			this.SuspendLayout();
			// 
			// megFloor
			// 
			this.megFloor.AllowToAdd = false;
			this.megFloor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.megFloor.Filter = Europlan.Common.CategoryType.Floor;
			this.megFloor.Location = new System.Drawing.Point(3, 3);
			this.megFloor.Name = "megFloor";
			this.megFloor.Size = new System.Drawing.Size(678, 494);
			this.megFloor.TabIndex = 0;
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
			this.tabPageFloor.Controls.Add(this.megFloor);
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
			this.tabPageWall.Controls.Add(this.megWall);
			this.tabPageWall.Location = new System.Drawing.Point(4, 22);
			this.tabPageWall.Name = "tabPageWall";
			this.tabPageWall.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageWall.Size = new System.Drawing.Size(684, 500);
			this.tabPageWall.TabIndex = 1;
			this.tabPageWall.Text = "Wand";
			this.tabPageWall.UseVisualStyleBackColor = true;
			// 
			// megWall
			// 
			this.megWall.AllowToAdd = false;
			this.megWall.Dock = System.Windows.Forms.DockStyle.Fill;
			this.megWall.Filter = Europlan.Common.CategoryType.Wall;
			this.megWall.Location = new System.Drawing.Point(3, 3);
			this.megWall.Name = "megWall";
			this.megWall.Size = new System.Drawing.Size(678, 494);
			this.megWall.TabIndex = 1;
			// 
			// tabPageCeiling
			// 
			this.tabPageCeiling.Controls.Add(this.megCeiling);
			this.tabPageCeiling.Location = new System.Drawing.Point(4, 22);
			this.tabPageCeiling.Name = "tabPageCeiling";
			this.tabPageCeiling.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCeiling.Size = new System.Drawing.Size(684, 500);
			this.tabPageCeiling.TabIndex = 2;
			this.tabPageCeiling.Text = "Decke";
			this.tabPageCeiling.UseVisualStyleBackColor = true;
			// 
			// megCeiling
			// 
			this.megCeiling.AllowToAdd = false;
			this.megCeiling.Dock = System.Windows.Forms.DockStyle.Fill;
			this.megCeiling.Filter = Europlan.Common.CategoryType.Ceiling;
			this.megCeiling.Location = new System.Drawing.Point(3, 3);
			this.megCeiling.Name = "megCeiling";
			this.megCeiling.Size = new System.Drawing.Size(678, 494);
			this.megCeiling.TabIndex = 1;
			// 
			// tabPageDistributor
			// 
			this.tabPageDistributor.Controls.Add(this.megDistributor);
			this.tabPageDistributor.Location = new System.Drawing.Point(4, 22);
			this.tabPageDistributor.Name = "tabPageDistributor";
			this.tabPageDistributor.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageDistributor.Size = new System.Drawing.Size(684, 500);
			this.tabPageDistributor.TabIndex = 3;
			this.tabPageDistributor.Text = "Verteiler";
			this.tabPageDistributor.UseVisualStyleBackColor = true;
			// 
			// megDistributor
			// 
			this.megDistributor.AllowToAdd = false;
			this.megDistributor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.megDistributor.Filter = Europlan.Common.CategoryType.Distributor;
			this.megDistributor.Location = new System.Drawing.Point(3, 3);
			this.megDistributor.Name = "megDistributor";
			this.megDistributor.Size = new System.Drawing.Size(678, 494);
			this.megDistributor.TabIndex = 1;
			// 
			// tabPageInsulation
			// 
			this.tabPageInsulation.Controls.Add(this.megInsulation);
			this.tabPageInsulation.Location = new System.Drawing.Point(4, 22);
			this.tabPageInsulation.Name = "tabPageInsulation";
			this.tabPageInsulation.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageInsulation.Size = new System.Drawing.Size(684, 500);
			this.tabPageInsulation.TabIndex = 4;
			this.tabPageInsulation.Text = "D‰mmung";
			this.tabPageInsulation.UseVisualStyleBackColor = true;
			// 
			// megInsulation
			// 
			this.megInsulation.AllowToAdd = true;
			this.megInsulation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.megInsulation.Filter = Europlan.Common.CategoryType.Insulation;
			this.megInsulation.Location = new System.Drawing.Point(3, 3);
			this.megInsulation.Name = "megInsulation";
			this.megInsulation.Size = new System.Drawing.Size(678, 494);
			this.megInsulation.TabIndex = 1;
			// 
			// tabPageGeneral
			// 
			this.tabPageGeneral.Controls.Add(this.megGeneral);
			this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabPageGeneral.Name = "tabPageGeneral";
			this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageGeneral.Size = new System.Drawing.Size(684, 500);
			this.tabPageGeneral.TabIndex = 5;
			this.tabPageGeneral.Text = "Allgemein";
			this.tabPageGeneral.UseVisualStyleBackColor = true;
			// 
			// megGeneral
			// 
			this.megGeneral.AllowToAdd = true;
			this.megGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
			this.megGeneral.Filter = Europlan.Common.CategoryType.General;
			this.megGeneral.Location = new System.Drawing.Point(3, 3);
			this.megGeneral.Name = "megGeneral";
			this.megGeneral.Size = new System.Drawing.Size(678, 494);
			this.megGeneral.TabIndex = 1;
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

		private Europlan.Application.ContructionEditor.MaterialEditorGrid megFloor;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabPageFloor;
		private System.Windows.Forms.TabPage tabPageWall;
		private Europlan.Application.ContructionEditor.MaterialEditorGrid megWall;
		private System.Windows.Forms.TabPage tabPageCeiling;
		private Europlan.Application.ContructionEditor.MaterialEditorGrid megCeiling;
		private System.Windows.Forms.TabPage tabPageDistributor;
		private Europlan.Application.ContructionEditor.MaterialEditorGrid megDistributor;
		private System.Windows.Forms.TabPage tabPageInsulation;
		private Europlan.Application.ContructionEditor.MaterialEditorGrid megInsulation;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private Europlan.Application.ContructionEditor.MaterialEditorGrid megGeneral;
	}
}