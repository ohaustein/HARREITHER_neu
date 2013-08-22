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
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.cbAutoSave = new System.Windows.Forms.CheckBox();
            this.cbOrthoRasterung = new System.Windows.Forms.CheckBox();
            this.lblPlanUnit = new System.Windows.Forms.Label();
            this.cmbPlanUnit = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabMaterials = new System.Windows.Forms.TabPage();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPageFloor = new System.Windows.Forms.TabPage();
            this.tabPageWall = new System.Windows.Forms.TabPage();
            this.tabPageCeiling = new System.Windows.Forms.TabPage();
            this.tabPageDistributor = new System.Windows.Forms.TabPage();
            this.tabPageInsulation = new System.Windows.Forms.TabPage();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabConstructions = new System.Windows.Forms.TabPage();
            this.tabDefaultSystemParameters = new System.Windows.Forms.TabPage();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.lblAutoSaveInterval = new System.Windows.Forms.Label();
            this.lblAutoSaveMin = new System.Windows.Forms.Label();
            this.numAutoSaveInterval = new Europlan.Common.NumericBox();
            this.megFloor = new Europlan.Common.MaterialEditorGrid();
            this.megWall = new Europlan.Common.MaterialEditorGrid();
            this.megCeiling = new Europlan.Common.MaterialEditorGrid();
            this.megDistributor = new Europlan.Common.MaterialEditorGrid();
            this.megInsulation = new Europlan.Common.MaterialEditorGrid();
            this.megGeneral = new Europlan.Common.MaterialEditorGrid();
            this.constructionEditorGrid1 = new Europlan.Common.ConstructionEditorGrid();
            this.systemParametersPanel = new Europlan.Common.SystemParametersPanel();
            this.panel1.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabMaterials.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabPageFloor.SuspendLayout();
            this.tabPageWall.SuspendLayout();
            this.tabPageCeiling.SuspendLayout();
            this.tabPageDistributor.SuspendLayout();
            this.tabPageInsulation.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabConstructions.SuspendLayout();
            this.tabDefaultSystemParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.Name = "lblLanguage";
            this.helpProvider.SetShowHelp(this.lblLanguage, ((bool)(resources.GetObject("lblLanguage.ShowHelp"))));
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLanguage, "cmbLanguage");
            this.cmbLanguage.Name = "cmbLanguage";
            this.helpProvider.SetShowHelp(this.cmbLanguage, ((bool)(resources.GetObject("cmbLanguage.ShowHelp"))));
            this.cmbLanguage.Sorted = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.helpProvider.SetShowHelp(this.btnCancel, ((bool)(resources.GetObject("btnCancel.ShowHelp"))));
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.helpProvider.SetShowHelp(this.btnOk, ((bool)(resources.GetObject("btnOk.ShowHelp"))));
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.helpProvider.SetShowHelp(this.panel1, ((bool)(resources.GetObject("panel1.ShowHelp"))));
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.tabGeneral);
            this.tabOptions.Controls.Add(this.tabMaterials);
            this.tabOptions.Controls.Add(this.tabConstructions);
            this.tabOptions.Controls.Add(this.tabDefaultSystemParameters);
            resources.ApplyResources(this.tabOptions, "tabOptions");
            this.helpProvider.SetHelpKeyword(this.tabOptions, resources.GetString("tabOptions.HelpKeyword"));
            this.helpProvider.SetHelpNavigator(this.tabOptions, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("tabOptions.HelpNavigator"))));
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.helpProvider.SetShowHelp(this.tabOptions, ((bool)(resources.GetObject("tabOptions.ShowHelp"))));
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.lblAutoSaveMin);
            this.tabGeneral.Controls.Add(this.numAutoSaveInterval);
            this.tabGeneral.Controls.Add(this.lblAutoSaveInterval);
            this.tabGeneral.Controls.Add(this.cbAutoSave);
            this.tabGeneral.Controls.Add(this.cbOrthoRasterung);
            this.tabGeneral.Controls.Add(this.lblPlanUnit);
            this.tabGeneral.Controls.Add(this.cmbPlanUnit);
            this.tabGeneral.Controls.Add(this.button2);
            this.tabGeneral.Controls.Add(this.button1);
            this.tabGeneral.Controls.Add(this.pictureBox1);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Controls.Add(this.lblLanguage);
            this.tabGeneral.Controls.Add(this.cmbLanguage);
            resources.ApplyResources(this.tabGeneral, "tabGeneral");
            this.tabGeneral.Name = "tabGeneral";
            this.helpProvider.SetShowHelp(this.tabGeneral, ((bool)(resources.GetObject("tabGeneral.ShowHelp"))));
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // cbAutoSave
            // 
            resources.ApplyResources(this.cbAutoSave, "cbAutoSave");
            this.cbAutoSave.Name = "cbAutoSave";
            this.helpProvider.SetShowHelp(this.cbAutoSave, ((bool)(resources.GetObject("cbAutoSave.ShowHelp"))));
            this.cbAutoSave.UseVisualStyleBackColor = true;
            this.cbAutoSave.CheckedChanged += new System.EventHandler(this.cbAutoSave_CheckedChanged);
            // 
            // cbOrthoRasterung
            // 
            resources.ApplyResources(this.cbOrthoRasterung, "cbOrthoRasterung");
            this.cbOrthoRasterung.Name = "cbOrthoRasterung";
            this.helpProvider.SetShowHelp(this.cbOrthoRasterung, ((bool)(resources.GetObject("cbOrthoRasterung.ShowHelp"))));
            this.cbOrthoRasterung.UseVisualStyleBackColor = true;
            // 
            // lblPlanUnit
            // 
            resources.ApplyResources(this.lblPlanUnit, "lblPlanUnit");
            this.lblPlanUnit.Name = "lblPlanUnit";
            this.helpProvider.SetShowHelp(this.lblPlanUnit, ((bool)(resources.GetObject("lblPlanUnit.ShowHelp"))));
            // 
            // cmbPlanUnit
            // 
            this.cmbPlanUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlanUnit.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPlanUnit, "cmbPlanUnit");
            this.cmbPlanUnit.Name = "cmbPlanUnit";
            this.helpProvider.SetShowHelp(this.cmbPlanUnit, ((bool)(resources.GetObject("cmbPlanUnit.ShowHelp"))));
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.helpProvider.SetShowHelp(this.button2, ((bool)(resources.GetObject("button2.ShowHelp"))));
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.helpProvider.SetShowHelp(this.button1, ((bool)(resources.GetObject("button1.ShowHelp"))));
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Name = "pictureBox1";
            this.helpProvider.SetShowHelp(this.pictureBox1, ((bool)(resources.GetObject("pictureBox1.ShowHelp"))));
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.helpProvider.SetShowHelp(this.label1, ((bool)(resources.GetObject("label1.ShowHelp"))));
            // 
            // tabMaterials
            // 
            this.tabMaterials.Controls.Add(this.tabs);
            resources.ApplyResources(this.tabMaterials, "tabMaterials");
            this.tabMaterials.Name = "tabMaterials";
            this.helpProvider.SetShowHelp(this.tabMaterials, ((bool)(resources.GetObject("tabMaterials.ShowHelp"))));
            this.tabMaterials.UseVisualStyleBackColor = true;
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
            this.helpProvider.SetShowHelp(this.tabs, ((bool)(resources.GetObject("tabs.ShowHelp"))));
            // 
            // tabPageFloor
            // 
            this.tabPageFloor.Controls.Add(this.megFloor);
            resources.ApplyResources(this.tabPageFloor, "tabPageFloor");
            this.tabPageFloor.Name = "tabPageFloor";
            this.helpProvider.SetShowHelp(this.tabPageFloor, ((bool)(resources.GetObject("tabPageFloor.ShowHelp"))));
            this.tabPageFloor.UseVisualStyleBackColor = true;
            // 
            // tabPageWall
            // 
            this.tabPageWall.Controls.Add(this.megWall);
            resources.ApplyResources(this.tabPageWall, "tabPageWall");
            this.tabPageWall.Name = "tabPageWall";
            this.helpProvider.SetShowHelp(this.tabPageWall, ((bool)(resources.GetObject("tabPageWall.ShowHelp"))));
            this.tabPageWall.UseVisualStyleBackColor = true;
            // 
            // tabPageCeiling
            // 
            this.tabPageCeiling.Controls.Add(this.megCeiling);
            resources.ApplyResources(this.tabPageCeiling, "tabPageCeiling");
            this.tabPageCeiling.Name = "tabPageCeiling";
            this.helpProvider.SetShowHelp(this.tabPageCeiling, ((bool)(resources.GetObject("tabPageCeiling.ShowHelp"))));
            this.tabPageCeiling.UseVisualStyleBackColor = true;
            // 
            // tabPageDistributor
            // 
            this.tabPageDistributor.Controls.Add(this.megDistributor);
            resources.ApplyResources(this.tabPageDistributor, "tabPageDistributor");
            this.tabPageDistributor.Name = "tabPageDistributor";
            this.helpProvider.SetShowHelp(this.tabPageDistributor, ((bool)(resources.GetObject("tabPageDistributor.ShowHelp"))));
            this.tabPageDistributor.UseVisualStyleBackColor = true;
            // 
            // tabPageInsulation
            // 
            this.tabPageInsulation.Controls.Add(this.megInsulation);
            resources.ApplyResources(this.tabPageInsulation, "tabPageInsulation");
            this.tabPageInsulation.Name = "tabPageInsulation";
            this.helpProvider.SetShowHelp(this.tabPageInsulation, ((bool)(resources.GetObject("tabPageInsulation.ShowHelp"))));
            this.tabPageInsulation.UseVisualStyleBackColor = true;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.megGeneral);
            resources.ApplyResources(this.tabPageGeneral, "tabPageGeneral");
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.helpProvider.SetShowHelp(this.tabPageGeneral, ((bool)(resources.GetObject("tabPageGeneral.ShowHelp"))));
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabConstructions
            // 
            this.tabConstructions.Controls.Add(this.constructionEditorGrid1);
            resources.ApplyResources(this.tabConstructions, "tabConstructions");
            this.tabConstructions.Name = "tabConstructions";
            this.helpProvider.SetShowHelp(this.tabConstructions, ((bool)(resources.GetObject("tabConstructions.ShowHelp"))));
            this.tabConstructions.UseVisualStyleBackColor = true;
            // 
            // tabDefaultSystemParameters
            // 
            this.tabDefaultSystemParameters.Controls.Add(this.systemParametersPanel);
            resources.ApplyResources(this.tabDefaultSystemParameters, "tabDefaultSystemParameters");
            this.tabDefaultSystemParameters.Name = "tabDefaultSystemParameters";
            this.helpProvider.SetShowHelp(this.tabDefaultSystemParameters, ((bool)(resources.GetObject("tabDefaultSystemParameters.ShowHelp"))));
            this.tabDefaultSystemParameters.UseVisualStyleBackColor = true;
            // 
            // helpProvider
            // 
            resources.ApplyResources(this.helpProvider, "helpProvider");
            // 
            // lblAutoSaveInterval
            // 
            resources.ApplyResources(this.lblAutoSaveInterval, "lblAutoSaveInterval");
            this.lblAutoSaveInterval.Name = "lblAutoSaveInterval";
            this.helpProvider.SetShowHelp(this.lblAutoSaveInterval, ((bool)(resources.GetObject("lblAutoSaveInterval.ShowHelp"))));
            // 
            // lblAutoSaveMin
            // 
            resources.ApplyResources(this.lblAutoSaveMin, "lblAutoSaveMin");
            this.lblAutoSaveMin.Name = "lblAutoSaveMin";
            this.helpProvider.SetShowHelp(this.lblAutoSaveMin, ((bool)(resources.GetObject("lblAutoSaveMin.ShowHelp"))));
            // 
            // numAutoSaveInterval
            // 
            this.numAutoSaveInterval.EditType = Europlan.Common.NumericBox.NumericEditType.AUTO_SAVE_INTERVAL;
            this.numAutoSaveInterval.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            resources.ApplyResources(this.numAutoSaveInterval, "numAutoSaveInterval");
            this.numAutoSaveInterval.MaxValue = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numAutoSaveInterval.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAutoSaveInterval.Name = "numAutoSaveInterval";
            this.helpProvider.SetShowHelp(this.numAutoSaveInterval, ((bool)(resources.GetObject("numAutoSaveInterval.ShowHelp"))));
            this.numAutoSaveInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // megFloor
            // 
            this.megFloor.Admin = false;
            this.megFloor.AllowToAdd = false;
            resources.ApplyResources(this.megFloor, "megFloor");
            this.megFloor.Filter = null;
            this.megFloor.Name = "megFloor";
            this.helpProvider.SetShowHelp(this.megFloor, ((bool)(resources.GetObject("megFloor.ShowHelp"))));
            this.megFloor.ShowOnlyAdditional = false;
            // 
            // megWall
            // 
            this.megWall.Admin = false;
            this.megWall.AllowToAdd = false;
            resources.ApplyResources(this.megWall, "megWall");
            this.megWall.Filter = null;
            this.megWall.Name = "megWall";
            this.helpProvider.SetShowHelp(this.megWall, ((bool)(resources.GetObject("megWall.ShowHelp"))));
            this.megWall.ShowOnlyAdditional = false;
            // 
            // megCeiling
            // 
            this.megCeiling.Admin = false;
            this.megCeiling.AllowToAdd = false;
            resources.ApplyResources(this.megCeiling, "megCeiling");
            this.megCeiling.Filter = null;
            this.megCeiling.Name = "megCeiling";
            this.helpProvider.SetShowHelp(this.megCeiling, ((bool)(resources.GetObject("megCeiling.ShowHelp"))));
            this.megCeiling.ShowOnlyAdditional = false;
            // 
            // megDistributor
            // 
            this.megDistributor.Admin = false;
            this.megDistributor.AllowToAdd = false;
            resources.ApplyResources(this.megDistributor, "megDistributor");
            this.megDistributor.Filter = null;
            this.megDistributor.Name = "megDistributor";
            this.helpProvider.SetShowHelp(this.megDistributor, ((bool)(resources.GetObject("megDistributor.ShowHelp"))));
            this.megDistributor.ShowOnlyAdditional = false;
            // 
            // megInsulation
            // 
            this.megInsulation.Admin = false;
            this.megInsulation.AllowToAdd = true;
            resources.ApplyResources(this.megInsulation, "megInsulation");
            this.megInsulation.Filter = null;
            this.megInsulation.Name = "megInsulation";
            this.helpProvider.SetShowHelp(this.megInsulation, ((bool)(resources.GetObject("megInsulation.ShowHelp"))));
            this.megInsulation.ShowOnlyAdditional = false;
            // 
            // megGeneral
            // 
            this.megGeneral.Admin = false;
            this.megGeneral.AllowToAdd = true;
            resources.ApplyResources(this.megGeneral, "megGeneral");
            this.megGeneral.Filter = null;
            this.megGeneral.Name = "megGeneral";
            this.helpProvider.SetShowHelp(this.megGeneral, ((bool)(resources.GetObject("megGeneral.ShowHelp"))));
            this.megGeneral.ShowOnlyAdditional = false;
            // 
            // constructionEditorGrid1
            // 
            resources.ApplyResources(this.constructionEditorGrid1, "constructionEditorGrid1");
            this.constructionEditorGrid1.Filter = Europlan.Common.ConstructionScopeEnum.All;
            this.constructionEditorGrid1.Name = "constructionEditorGrid1";
            this.helpProvider.SetShowHelp(this.constructionEditorGrid1, ((bool)(resources.GetObject("constructionEditorGrid1.ShowHelp"))));
            this.constructionEditorGrid1.Type = Europlan.Common.Configuration.ConfigurationType.UserConfiguration;
            // 
            // systemParametersPanel
            // 
            this.systemParametersPanel.ConfigurationType = Europlan.Common.Configuration.ConfigurationType.UserConfiguration;
            resources.ApplyResources(this.systemParametersPanel, "systemParametersPanel");
            this.systemParametersPanel.Name = "systemParametersPanel";
            this.helpProvider.SetShowHelp(this.systemParametersPanel, ((bool)(resources.GetObject("systemParametersPanel.ShowHelp"))));
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ControlBox = false;
            this.Controls.Add(this.tabOptions);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabMaterials.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabPageFloor.ResumeLayout(false);
            this.tabPageWall.ResumeLayout(false);
            this.tabPageCeiling.ResumeLayout(false);
            this.tabPageDistributor.ResumeLayout(false);
            this.tabPageInsulation.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabConstructions.ResumeLayout(false);
            this.tabDefaultSystemParameters.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblLanguage;
		private System.Windows.Forms.ComboBox cmbLanguage;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabOptions;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TabPage tabMaterials;
		private System.Windows.Forms.TabPage tabConstructions;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabPageFloor;
		private Europlan.Common.MaterialEditorGrid megFloor;
		private System.Windows.Forms.TabPage tabPageWall;
		private Europlan.Common.MaterialEditorGrid megWall;
		private System.Windows.Forms.TabPage tabPageCeiling;
		private Europlan.Common.MaterialEditorGrid megCeiling;
		private System.Windows.Forms.TabPage tabPageDistributor;
		private Europlan.Common.MaterialEditorGrid megDistributor;
		private System.Windows.Forms.TabPage tabPageInsulation;
		private Europlan.Common.MaterialEditorGrid megInsulation;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private Europlan.Common.MaterialEditorGrid megGeneral;
		private Europlan.Common.ConstructionEditorGrid constructionEditorGrid1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.TabPage tabDefaultSystemParameters;
		private Europlan.Common.SystemParametersPanel systemParametersPanel;
		private System.Windows.Forms.Label lblPlanUnit;
        private System.Windows.Forms.ComboBox cmbPlanUnit;
        private System.Windows.Forms.CheckBox cbOrthoRasterung;
        private System.Windows.Forms.CheckBox cbAutoSave;
        private Europlan.Common.NumericBox numAutoSaveInterval;
        private System.Windows.Forms.Label lblAutoSaveInterval;
        private System.Windows.Forms.Label lblAutoSaveMin;
	}
}