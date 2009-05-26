namespace Europlan.Application {
	partial class OptionsForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
			this.lblLanguage = new System.Windows.Forms.Label();
			this.cmbLanguage = new System.Windows.Forms.ComboBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.tabMaterials = new System.Windows.Forms.TabPage();
			this.tabConstructions = new System.Windows.Forms.TabPage();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabPageFloor = new System.Windows.Forms.TabPage();
			this.tabPageWall = new System.Windows.Forms.TabPage();
			this.tabPageCeiling = new System.Windows.Forms.TabPage();
			this.tabPageDistributor = new System.Windows.Forms.TabPage();
			this.tabPageInsulation = new System.Windows.Forms.TabPage();
			this.tabPageGeneral = new System.Windows.Forms.TabPage();
			this.megFloor = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.megWall = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.megCeiling = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.megDistributor = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.megInsulation = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.megGeneral = new Europlan.Application.ContructionEditor.MaterialEditorGrid();
			this.constructionEditorGrid1 = new Europlan.Common.ConstructionEditorGrid();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			this.tabMaterials.SuspendLayout();
			this.tabConstructions.SuspendLayout();
			this.tabs.SuspendLayout();
			this.tabPageFloor.SuspendLayout();
			this.tabPageWall.SuspendLayout();
			this.tabPageCeiling.SuspendLayout();
			this.tabPageDistributor.SuspendLayout();
			this.tabPageInsulation.SuspendLayout();
			this.tabPageGeneral.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblLanguage
			// 
			resources.ApplyResources(this.lblLanguage, "lblLanguage");
			this.lblLanguage.Name = "lblLanguage";
			// 
			// cmbLanguage
			// 
			this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLanguage.FormattingEnabled = true;
			resources.ApplyResources(this.cmbLanguage, "cmbLanguage");
			this.cmbLanguage.Name = "cmbLanguage";
			this.cmbLanguage.Sorted = true;
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnOk);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabGeneral);
			this.tabControl1.Controls.Add(this.tabMaterials);
			this.tabControl1.Controls.Add(this.tabConstructions);
			resources.ApplyResources(this.tabControl1, "tabControl1");
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.lblLanguage);
			this.tabGeneral.Controls.Add(this.cmbLanguage);
			resources.ApplyResources(this.tabGeneral, "tabGeneral");
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// tabMaterials
			// 
			this.tabMaterials.Controls.Add(this.tabs);
			resources.ApplyResources(this.tabMaterials, "tabMaterials");
			this.tabMaterials.Name = "tabMaterials";
			this.tabMaterials.UseVisualStyleBackColor = true;
			// 
			// tabConstructions
			// 
			this.tabConstructions.Controls.Add(this.constructionEditorGrid1);
			resources.ApplyResources(this.tabConstructions, "tabConstructions");
			this.tabConstructions.Name = "tabConstructions";
			this.tabConstructions.UseVisualStyleBackColor = true;
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.tabPageFloor);
			this.tabs.Controls.Add(this.tabPageWall);
			this.tabs.Controls.Add(this.tabPageCeiling);
			this.tabs.Controls.Add(this.tabPageDistributor);
			this.tabs.Controls.Add(this.tabPageInsulation);
			this.tabs.Controls.Add(this.tabPageGeneral);
			resources.ApplyResources(this.tabs, "tabs");
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			// 
			// tabPageFloor
			// 
			this.tabPageFloor.Controls.Add(this.megFloor);
			resources.ApplyResources(this.tabPageFloor, "tabPageFloor");
			this.tabPageFloor.Name = "tabPageFloor";
			this.tabPageFloor.UseVisualStyleBackColor = true;
			// 
			// tabPageWall
			// 
			this.tabPageWall.Controls.Add(this.megWall);
			resources.ApplyResources(this.tabPageWall, "tabPageWall");
			this.tabPageWall.Name = "tabPageWall";
			this.tabPageWall.UseVisualStyleBackColor = true;
			// 
			// tabPageCeiling
			// 
			this.tabPageCeiling.Controls.Add(this.megCeiling);
			resources.ApplyResources(this.tabPageCeiling, "tabPageCeiling");
			this.tabPageCeiling.Name = "tabPageCeiling";
			this.tabPageCeiling.UseVisualStyleBackColor = true;
			// 
			// tabPageDistributor
			// 
			this.tabPageDistributor.Controls.Add(this.megDistributor);
			resources.ApplyResources(this.tabPageDistributor, "tabPageDistributor");
			this.tabPageDistributor.Name = "tabPageDistributor";
			this.tabPageDistributor.UseVisualStyleBackColor = true;
			// 
			// tabPageInsulation
			// 
			this.tabPageInsulation.Controls.Add(this.megInsulation);
			resources.ApplyResources(this.tabPageInsulation, "tabPageInsulation");
			this.tabPageInsulation.Name = "tabPageInsulation";
			this.tabPageInsulation.UseVisualStyleBackColor = true;
			// 
			// tabPageGeneral
			// 
			this.tabPageGeneral.Controls.Add(this.megGeneral);
			resources.ApplyResources(this.tabPageGeneral, "tabPageGeneral");
			this.tabPageGeneral.Name = "tabPageGeneral";
			this.tabPageGeneral.UseVisualStyleBackColor = true;
			// 
			// megFloor
			// 
			this.megFloor.AllowToAdd = false;
			resources.ApplyResources(this.megFloor, "megFloor");
			this.megFloor.Filter = Europlan.Common.CategoryType.Floor;
			this.megFloor.Name = "megFloor";
			// 
			// megWall
			// 
			this.megWall.AllowToAdd = false;
			resources.ApplyResources(this.megWall, "megWall");
			this.megWall.Filter = Europlan.Common.CategoryType.Wall;
			this.megWall.Name = "megWall";
			// 
			// megCeiling
			// 
			this.megCeiling.AllowToAdd = false;
			resources.ApplyResources(this.megCeiling, "megCeiling");
			this.megCeiling.Filter = Europlan.Common.CategoryType.Ceiling;
			this.megCeiling.Name = "megCeiling";
			// 
			// megDistributor
			// 
			this.megDistributor.AllowToAdd = false;
			resources.ApplyResources(this.megDistributor, "megDistributor");
			this.megDistributor.Filter = Europlan.Common.CategoryType.Distributor;
			this.megDistributor.Name = "megDistributor";
			// 
			// megInsulation
			// 
			this.megInsulation.AllowToAdd = true;
			resources.ApplyResources(this.megInsulation, "megInsulation");
			this.megInsulation.Filter = Europlan.Common.CategoryType.Insulation;
			this.megInsulation.Name = "megInsulation";
			// 
			// megGeneral
			// 
			this.megGeneral.AllowToAdd = true;
			resources.ApplyResources(this.megGeneral, "megGeneral");
			this.megGeneral.Filter = Europlan.Common.CategoryType.General;
			this.megGeneral.Name = "megGeneral";
			// 
			// constructionEditorGrid1
			// 
			resources.ApplyResources(this.constructionEditorGrid1, "constructionEditorGrid1");
			this.constructionEditorGrid1.Filter = Europlan.Common.ConstructionScopeEnum.All;
			this.constructionEditorGrid1.Name = "constructionEditorGrid1";
			this.constructionEditorGrid1.Type = Europlan.Common.Configuration.ConfigurationType.UserConfiguration;
			// 
			// OptionsForm
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ControlBox = false;
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.OptionsForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
			this.panel1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.tabGeneral.PerformLayout();
			this.tabMaterials.ResumeLayout(false);
			this.tabConstructions.ResumeLayout(false);
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

		private System.Windows.Forms.Label lblLanguage;
		private System.Windows.Forms.ComboBox cmbLanguage;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TabPage tabMaterials;
		private System.Windows.Forms.TabPage tabConstructions;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabPageFloor;
		private Europlan.Application.ContructionEditor.MaterialEditorGrid megFloor;
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
		private Europlan.Common.ConstructionEditorGrid constructionEditorGrid1;
	}
}