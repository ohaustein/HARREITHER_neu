namespace Europlan.Common {
	partial class QuickDimensioningPanel {
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
			this.components = new System.ComponentModel.Container();
			this.tabQuickDimensioning = new System.Windows.Forms.TabControl();
			this.pageSummary = new System.Windows.Forms.TabPage();
			this.button1 = new System.Windows.Forms.Button();
			this.listLabelPreviewControl1 = new combit.ListLabel14.ListLabelPreviewControl();
			this.pageSettings = new System.Windows.Forms.TabPage();
			this.cmbHeatFlowTemperature = new System.Windows.Forms.ComboBox();
			this.btnRevert = new System.Windows.Forms.Button();
			this.lblTemp4 = new System.Windows.Forms.Label();
			this.lblTemp3 = new System.Windows.Forms.Label();
			this.lblAllocation2 = new System.Windows.Forms.Label();
			this.lblAllocation = new System.Windows.Forms.Label();
			this.lblDistance = new System.Windows.Forms.Label();
			this.cmbDistance = new System.Windows.Forms.ComboBox();
			this.lblTemp2 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblTemp1 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblEuroval = new System.Windows.Forms.Label();
			this.cbEurovalHeat = new System.Windows.Forms.CheckBox();
			this.cbEurovalCool = new System.Windows.Forms.CheckBox();
			this.lblHeat = new System.Windows.Forms.Label();
			this.lblCool = new System.Windows.Forms.Label();
			this.cbModulKlimaDeckeCool = new System.Windows.Forms.CheckBox();
			this.cbModulKlimaDeckeHeat = new System.Windows.Forms.CheckBox();
			this.lblModulKlimaDecke = new System.Windows.Forms.Label();
			this.cbModulKlimaBodenCool = new System.Windows.Forms.CheckBox();
			this.cbModulKlimaBodenHeat = new System.Windows.Forms.CheckBox();
			this.lblModulKlimaBoden = new System.Windows.Forms.Label();
			this.cbHithermCompactCool = new System.Windows.Forms.CheckBox();
			this.cbHithermCompactHeat = new System.Windows.Forms.CheckBox();
			this.lblHithermCompact = new System.Windows.Forms.Label();
			this.cbHithermCool = new System.Windows.Forms.CheckBox();
			this.cbHithermHeat = new System.Windows.Forms.CheckBox();
			this.lblHitherm = new System.Windows.Forms.Label();
			this.lblBka = new System.Windows.Forms.Label();
			this.cbBkaHeat = new System.Windows.Forms.CheckBox();
			this.cbBkaCool = new System.Windows.Forms.CheckBox();
			this.lblAssumptions = new System.Windows.Forms.Label();
			this.txtCoolTemperature = new Europlan.Common.NumericBox();
			this.txtAllocation = new Europlan.Common.NumericBox();
			this.pageDistributors = new System.Windows.Forms.TabPage();
			this.quickDimensioningDistributorsSummary = new Europlan.Common.QuickDimensioningDistributorsSummary();
			this.quickDimensioningRoomDistributorsWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.tabQuickDimensioning.SuspendLayout();
			this.pageSummary.SuspendLayout();
			this.pageSettings.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.pageDistributors.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.quickDimensioningRoomDistributorsWrapperBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// tabQuickDimensioning
			// 
			this.tabQuickDimensioning.Controls.Add(this.pageSummary);
			this.tabQuickDimensioning.Controls.Add(this.pageSettings);
			this.tabQuickDimensioning.Controls.Add(this.pageDistributors);
			this.tabQuickDimensioning.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabQuickDimensioning.Location = new System.Drawing.Point(0, 0);
			this.tabQuickDimensioning.Name = "tabQuickDimensioning";
			this.tabQuickDimensioning.SelectedIndex = 0;
			this.tabQuickDimensioning.Size = new System.Drawing.Size(920, 538);
			this.tabQuickDimensioning.TabIndex = 0;
			this.tabQuickDimensioning.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabQuickDimensioning_Selecting);
			this.tabQuickDimensioning.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabQuickDimensioning_Selected);
			// 
			// pageSummary
			// 
			this.pageSummary.Controls.Add(this.listLabelPreviewControl1);
			this.pageSummary.Controls.Add(this.button1);
			this.pageSummary.Location = new System.Drawing.Point(4, 22);
			this.pageSummary.Name = "pageSummary";
			this.pageSummary.Padding = new System.Windows.Forms.Padding(3);
			this.pageSummary.Size = new System.Drawing.Size(912, 512);
			this.pageSummary.TabIndex = 1;
			this.pageSummary.Text = "Ergebnis Flächenaufstellung";
			this.pageSummary.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Enabled = false;
			this.button1.Location = new System.Drawing.Point(824, 483);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(82, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// listLabelPreviewControl1
			// 
			this.listLabelPreviewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listLabelPreviewControl1.BackColor = System.Drawing.SystemColors.Control;
			this.listLabelPreviewControl1.CurrentPage = 0;
			this.listLabelPreviewControl1.ForceReadOnly = false;
			this.listLabelPreviewControl1.Location = new System.Drawing.Point(0, 0);
			this.listLabelPreviewControl1.Name = "listLabelPreviewControl1";
			this.listLabelPreviewControl1.Size = new System.Drawing.Size(906, 506);
			this.listLabelPreviewControl1.SlideshowMode = false;
			this.listLabelPreviewControl1.TabIndex = 4;
			this.listLabelPreviewControl1.Text = "listLabelPreviewControl1";
			this.listLabelPreviewControl1.ToolbarButtons.Exit = combit.ListLabel14.LlButtonState.Invisible;
			this.listLabelPreviewControl1.ToolbarButtons.GotoFirst = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.GotoLast = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.GotoNext = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.GotoPrev = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.PageRange = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.PrintAllPages = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.PrintCurrentPage = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.PrintToFax = combit.ListLabel14.LlButtonState.Invisible;
			this.listLabelPreviewControl1.ToolbarButtons.SaveAs = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.SendTo = combit.ListLabel14.LlButtonState.Invisible;
			this.listLabelPreviewControl1.ToolbarButtons.SlideshowMode = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.ZoomCombo = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.ZoomReset = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.ZoomRevert = combit.ListLabel14.LlButtonState.Default;
			this.listLabelPreviewControl1.ToolbarButtons.ZoomTimes2 = combit.ListLabel14.LlButtonState.Default;
			// 
			// pageSettings
			// 
			this.pageSettings.Controls.Add(this.cmbHeatFlowTemperature);
			this.pageSettings.Controls.Add(this.btnRevert);
			this.pageSettings.Controls.Add(this.lblTemp4);
			this.pageSettings.Controls.Add(this.lblTemp3);
			this.pageSettings.Controls.Add(this.lblAllocation2);
			this.pageSettings.Controls.Add(this.lblAllocation);
			this.pageSettings.Controls.Add(this.lblDistance);
			this.pageSettings.Controls.Add(this.cmbDistance);
			this.pageSettings.Controls.Add(this.lblTemp2);
			this.pageSettings.Controls.Add(this.label2);
			this.pageSettings.Controls.Add(this.lblTemp1);
			this.pageSettings.Controls.Add(this.label1);
			this.pageSettings.Controls.Add(this.tableLayoutPanel1);
			this.pageSettings.Controls.Add(this.lblAssumptions);
			this.pageSettings.Controls.Add(this.txtCoolTemperature);
			this.pageSettings.Controls.Add(this.txtAllocation);
			this.pageSettings.Location = new System.Drawing.Point(4, 22);
			this.pageSettings.Name = "pageSettings";
			this.pageSettings.Padding = new System.Windows.Forms.Padding(3);
			this.pageSettings.Size = new System.Drawing.Size(912, 512);
			this.pageSettings.TabIndex = 0;
			this.pageSettings.Text = "Einstellungen";
			this.pageSettings.UseVisualStyleBackColor = true;
			// 
			// cmbHeatFlowTemperature
			// 
			this.cmbHeatFlowTemperature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbHeatFlowTemperature.FormattingEnabled = true;
			this.cmbHeatFlowTemperature.Items.AddRange(new object[] {
            "30",
            "35",
            "40",
            "45"});
			this.cmbHeatFlowTemperature.Location = new System.Drawing.Point(412, 95);
			this.cmbHeatFlowTemperature.Name = "cmbHeatFlowTemperature";
			this.cmbHeatFlowTemperature.Size = new System.Drawing.Size(56, 21);
			this.cmbHeatFlowTemperature.TabIndex = 30;
			this.cmbHeatFlowTemperature.SelectedIndexChanged += new System.EventHandler(this.cmbHeatFlowTemperature_SelectedIndexChanged);
			// 
			// btnRevert
			// 
			this.btnRevert.Location = new System.Drawing.Point(11, 239);
			this.btnRevert.Name = "btnRevert";
			this.btnRevert.Size = new System.Drawing.Size(251, 23);
			this.btnRevert.TabIndex = 29;
			this.btnRevert.Text = "Flächenaufstellung zurücksetzen";
			this.btnRevert.UseVisualStyleBackColor = true;
			this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
			// 
			// lblTemp4
			// 
			this.lblTemp4.AutoSize = true;
			this.lblTemp4.Location = new System.Drawing.Point(474, 130);
			this.lblTemp4.Name = "lblTemp4";
			this.lblTemp4.Size = new System.Drawing.Size(40, 13);
			this.lblTemp4.TabIndex = 26;
			this.lblTemp4.Text = "°C (Tv)";
			// 
			// lblTemp3
			// 
			this.lblTemp3.Location = new System.Drawing.Point(313, 131);
			this.lblTemp3.Name = "lblTemp3";
			this.lblTemp3.Size = new System.Drawing.Size(93, 29);
			this.lblTemp3.TabIndex = 27;
			this.lblTemp3.Text = "Vorlauftemperatur\r\n(Kühlen)";
			// 
			// lblAllocation2
			// 
			this.lblAllocation2.AutoSize = true;
			this.lblAllocation2.Location = new System.Drawing.Point(453, 215);
			this.lblAllocation2.Name = "lblAllocation2";
			this.lblAllocation2.Size = new System.Drawing.Size(15, 13);
			this.lblAllocation2.TabIndex = 21;
			this.lblAllocation2.Text = "%";
			// 
			// lblAllocation
			// 
			this.lblAllocation.Location = new System.Drawing.Point(315, 211);
			this.lblAllocation.Name = "lblAllocation";
			this.lblAllocation.Size = new System.Drawing.Size(70, 19);
			this.lblAllocation.TabIndex = 22;
			this.lblAllocation.Text = "Belegefaktor";
			this.lblAllocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblDistance
			// 
			this.lblDistance.AutoSize = true;
			this.lblDistance.Location = new System.Drawing.Point(530, 99);
			this.lblDistance.Name = "lblDistance";
			this.lblDistance.Size = new System.Drawing.Size(81, 13);
			this.lblDistance.TabIndex = 19;
			this.lblDistance.Text = "Verlegeabstand";
			// 
			// cmbDistance
			// 
			this.cmbDistance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDistance.FormattingEnabled = true;
			this.cmbDistance.Items.AddRange(new object[] {
            "EV 5",
            "EV10",
            "EV15",
            "EV20",
            "EV25",
            "EV30",
            "EV35"});
			this.cmbDistance.Location = new System.Drawing.Point(617, 96);
			this.cmbDistance.Name = "cmbDistance";
			this.cmbDistance.Size = new System.Drawing.Size(83, 21);
			this.cmbDistance.TabIndex = 5;
			this.cmbDistance.SelectedIndexChanged += new System.EventHandler(this.cmbDistance_SelectedIndexChanged);
			// 
			// lblTemp2
			// 
			this.lblTemp2.AutoSize = true;
			this.lblTemp2.Location = new System.Drawing.Point(474, 98);
			this.lblTemp2.Name = "lblTemp2";
			this.lblTemp2.Size = new System.Drawing.Size(40, 13);
			this.lblTemp2.TabIndex = 3;
			this.lblTemp2.Text = "°C (Tv)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(519, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Bitte wählen Sie jene Harreither-Produkte aus, welche in der Flächenaufstellung z" +
				"ur Verfügung stehen sollen:";
			// 
			// lblTemp1
			// 
			this.lblTemp1.Location = new System.Drawing.Point(313, 99);
			this.lblTemp1.Name = "lblTemp1";
			this.lblTemp1.Size = new System.Drawing.Size(93, 29);
			this.lblTemp1.TabIndex = 23;
			this.lblTemp1.Text = "Vorlauftemperatur\r\n(Heizen)";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(7, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(899, 29);
			this.label1.TabIndex = 1;
			this.label1.Text = "Flächenaufstellung";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.31055F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.68945F));
			this.tableLayoutPanel1.Controls.Add(this.lblEuroval, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cbEurovalHeat, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cbEurovalCool, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblHeat, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblCool, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaDeckeCool, 2, 6);
			this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaDeckeHeat, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblModulKlimaDecke, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaBodenCool, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaBodenHeat, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblModulKlimaBoden, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactCool, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactHeat, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblHithermCompact, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermCool, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermHeat, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblHitherm, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblBka, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.cbBkaHeat, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.cbBkaCool, 2, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 71);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(251, 162);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblEuroval
			// 
			this.lblEuroval.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblEuroval.BackColor = System.Drawing.Color.Transparent;
			this.lblEuroval.Location = new System.Drawing.Point(3, 26);
			this.lblEuroval.Margin = new System.Windows.Forms.Padding(3);
			this.lblEuroval.Name = "lblEuroval";
			this.lblEuroval.Size = new System.Drawing.Size(114, 17);
			this.lblEuroval.TabIndex = 1;
			this.lblEuroval.Text = "Euroval";
			this.lblEuroval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbEurovalHeat
			// 
			this.cbEurovalHeat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbEurovalHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbEurovalHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbEurovalHeat.Location = new System.Drawing.Point(123, 26);
			this.cbEurovalHeat.Name = "cbEurovalHeat";
			this.cbEurovalHeat.Size = new System.Drawing.Size(59, 17);
			this.cbEurovalHeat.TabIndex = 2;
			this.cbEurovalHeat.UseVisualStyleBackColor = false;
			this.cbEurovalHeat.CheckedChanged += new System.EventHandler(this.cbEurovalHeat_CheckedChanged);
			// 
			// cbEurovalCool
			// 
			this.cbEurovalCool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbEurovalCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbEurovalCool.Location = new System.Drawing.Point(188, 26);
			this.cbEurovalCool.Name = "cbEurovalCool";
			this.cbEurovalCool.Size = new System.Drawing.Size(60, 17);
			this.cbEurovalCool.TabIndex = 3;
			this.cbEurovalCool.UseVisualStyleBackColor = false;
			this.cbEurovalCool.CheckedChanged += new System.EventHandler(this.cbEurovalCool_CheckedChanged);
			// 
			// lblHeat
			// 
			this.lblHeat.BackColor = System.Drawing.Color.Transparent;
			this.lblHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeat.Location = new System.Drawing.Point(123, 3);
			this.lblHeat.Margin = new System.Windows.Forms.Padding(3);
			this.lblHeat.Name = "lblHeat";
			this.lblHeat.Size = new System.Drawing.Size(59, 17);
			this.lblHeat.TabIndex = 4;
			this.lblHeat.Text = "Heizen";
			this.lblHeat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCool
			// 
			this.lblCool.BackColor = System.Drawing.Color.Transparent;
			this.lblCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblCool.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCool.Location = new System.Drawing.Point(188, 3);
			this.lblCool.Margin = new System.Windows.Forms.Padding(3);
			this.lblCool.Name = "lblCool";
			this.lblCool.Size = new System.Drawing.Size(60, 17);
			this.lblCool.TabIndex = 5;
			this.lblCool.Text = "Kühlen";
			this.lblCool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cbModulKlimaDeckeCool
			// 
			this.cbModulKlimaDeckeCool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbModulKlimaDeckeCool.BackColor = System.Drawing.Color.Transparent;
			this.cbModulKlimaDeckeCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModulKlimaDeckeCool.Location = new System.Drawing.Point(188, 141);
			this.cbModulKlimaDeckeCool.Name = "cbModulKlimaDeckeCool";
			this.cbModulKlimaDeckeCool.Size = new System.Drawing.Size(60, 18);
			this.cbModulKlimaDeckeCool.TabIndex = 16;
			this.cbModulKlimaDeckeCool.UseVisualStyleBackColor = false;
			this.cbModulKlimaDeckeCool.CheckedChanged += new System.EventHandler(this.cbModulKlimaDeckeCool_CheckedChanged);
			// 
			// cbModulKlimaDeckeHeat
			// 
			this.cbModulKlimaDeckeHeat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbModulKlimaDeckeHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbModulKlimaDeckeHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModulKlimaDeckeHeat.Location = new System.Drawing.Point(123, 141);
			this.cbModulKlimaDeckeHeat.Name = "cbModulKlimaDeckeHeat";
			this.cbModulKlimaDeckeHeat.Size = new System.Drawing.Size(59, 18);
			this.cbModulKlimaDeckeHeat.TabIndex = 17;
			this.cbModulKlimaDeckeHeat.UseVisualStyleBackColor = false;
			this.cbModulKlimaDeckeHeat.CheckedChanged += new System.EventHandler(this.cbModulKlimaDeckeHeat_CheckedChanged);
			// 
			// lblModulKlimaDecke
			// 
			this.lblModulKlimaDecke.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblModulKlimaDecke.BackColor = System.Drawing.Color.Transparent;
			this.lblModulKlimaDecke.Location = new System.Drawing.Point(3, 141);
			this.lblModulKlimaDecke.Margin = new System.Windows.Forms.Padding(3);
			this.lblModulKlimaDecke.Name = "lblModulKlimaDecke";
			this.lblModulKlimaDecke.Size = new System.Drawing.Size(114, 18);
			this.lblModulKlimaDecke.TabIndex = 9;
			this.lblModulKlimaDecke.Text = "Modul Klimadecke";
			this.lblModulKlimaDecke.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbModulKlimaBodenCool
			// 
			this.cbModulKlimaBodenCool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbModulKlimaBodenCool.BackColor = System.Drawing.Color.Transparent;
			this.cbModulKlimaBodenCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModulKlimaBodenCool.Location = new System.Drawing.Point(188, 118);
			this.cbModulKlimaBodenCool.Name = "cbModulKlimaBodenCool";
			this.cbModulKlimaBodenCool.Size = new System.Drawing.Size(60, 17);
			this.cbModulKlimaBodenCool.TabIndex = 14;
			this.cbModulKlimaBodenCool.UseVisualStyleBackColor = false;
			this.cbModulKlimaBodenCool.CheckedChanged += new System.EventHandler(this.cbModulKlimaBodenCool_CheckedChanged);
			// 
			// cbModulKlimaBodenHeat
			// 
			this.cbModulKlimaBodenHeat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbModulKlimaBodenHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbModulKlimaBodenHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModulKlimaBodenHeat.Location = new System.Drawing.Point(123, 118);
			this.cbModulKlimaBodenHeat.Name = "cbModulKlimaBodenHeat";
			this.cbModulKlimaBodenHeat.Size = new System.Drawing.Size(59, 17);
			this.cbModulKlimaBodenHeat.TabIndex = 13;
			this.cbModulKlimaBodenHeat.UseVisualStyleBackColor = false;
			this.cbModulKlimaBodenHeat.CheckedChanged += new System.EventHandler(this.cbModulKlimaBodenHeat_CheckedChanged);
			// 
			// lblModulKlimaBoden
			// 
			this.lblModulKlimaBoden.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblModulKlimaBoden.BackColor = System.Drawing.Color.Transparent;
			this.lblModulKlimaBoden.Location = new System.Drawing.Point(3, 118);
			this.lblModulKlimaBoden.Margin = new System.Windows.Forms.Padding(3);
			this.lblModulKlimaBoden.Name = "lblModulKlimaBoden";
			this.lblModulKlimaBoden.Size = new System.Drawing.Size(114, 17);
			this.lblModulKlimaBoden.TabIndex = 8;
			this.lblModulKlimaBoden.Text = "Modul Klimaboden";
			this.lblModulKlimaBoden.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbHithermCompactCool
			// 
			this.cbHithermCompactCool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbHithermCompactCool.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermCompactCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermCompactCool.Location = new System.Drawing.Point(188, 95);
			this.cbHithermCompactCool.Name = "cbHithermCompactCool";
			this.cbHithermCompactCool.Size = new System.Drawing.Size(60, 17);
			this.cbHithermCompactCool.TabIndex = 15;
			this.cbHithermCompactCool.UseVisualStyleBackColor = false;
			this.cbHithermCompactCool.CheckedChanged += new System.EventHandler(this.cbHithermCompactCool_CheckedChanged);
			// 
			// cbHithermCompactHeat
			// 
			this.cbHithermCompactHeat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbHithermCompactHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermCompactHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermCompactHeat.Location = new System.Drawing.Point(123, 95);
			this.cbHithermCompactHeat.Name = "cbHithermCompactHeat";
			this.cbHithermCompactHeat.Size = new System.Drawing.Size(59, 17);
			this.cbHithermCompactHeat.TabIndex = 12;
			this.cbHithermCompactHeat.UseVisualStyleBackColor = false;
			this.cbHithermCompactHeat.CheckedChanged += new System.EventHandler(this.cbHithermCompactHeat_CheckedChanged);
			// 
			// lblHithermCompact
			// 
			this.lblHithermCompact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblHithermCompact.BackColor = System.Drawing.Color.Transparent;
			this.lblHithermCompact.Location = new System.Drawing.Point(3, 95);
			this.lblHithermCompact.Margin = new System.Windows.Forms.Padding(3);
			this.lblHithermCompact.Name = "lblHithermCompact";
			this.lblHithermCompact.Size = new System.Drawing.Size(114, 17);
			this.lblHithermCompact.TabIndex = 7;
			this.lblHithermCompact.Text = "Hitherm Compact";
			this.lblHithermCompact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbHithermCool
			// 
			this.cbHithermCool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbHithermCool.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermCool.Location = new System.Drawing.Point(188, 72);
			this.cbHithermCool.Name = "cbHithermCool";
			this.cbHithermCool.Size = new System.Drawing.Size(60, 17);
			this.cbHithermCool.TabIndex = 11;
			this.cbHithermCool.UseVisualStyleBackColor = false;
			this.cbHithermCool.CheckedChanged += new System.EventHandler(this.cbHithermCool_CheckedChanged);
			// 
			// cbHithermHeat
			// 
			this.cbHithermHeat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbHithermHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermHeat.Location = new System.Drawing.Point(123, 72);
			this.cbHithermHeat.Name = "cbHithermHeat";
			this.cbHithermHeat.Size = new System.Drawing.Size(59, 17);
			this.cbHithermHeat.TabIndex = 10;
			this.cbHithermHeat.UseVisualStyleBackColor = false;
			this.cbHithermHeat.CheckedChanged += new System.EventHandler(this.cbHithermHeat_CheckedChanged);
			// 
			// lblHitherm
			// 
			this.lblHitherm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblHitherm.BackColor = System.Drawing.Color.Transparent;
			this.lblHitherm.Location = new System.Drawing.Point(3, 72);
			this.lblHitherm.Margin = new System.Windows.Forms.Padding(3);
			this.lblHitherm.Name = "lblHitherm";
			this.lblHitherm.Size = new System.Drawing.Size(114, 17);
			this.lblHitherm.TabIndex = 6;
			this.lblHitherm.Text = "Hitherm";
			this.lblHitherm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblBka
			// 
			this.lblBka.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblBka.BackColor = System.Drawing.Color.Transparent;
			this.lblBka.Location = new System.Drawing.Point(3, 49);
			this.lblBka.Margin = new System.Windows.Forms.Padding(3);
			this.lblBka.Name = "lblBka";
			this.lblBka.Size = new System.Drawing.Size(114, 17);
			this.lblBka.TabIndex = 1;
			this.lblBka.Text = "Betonkernaktivierung";
			this.lblBka.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbBkaHeat
			// 
			this.cbBkaHeat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbBkaHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbBkaHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbBkaHeat.Location = new System.Drawing.Point(123, 49);
			this.cbBkaHeat.Name = "cbBkaHeat";
			this.cbBkaHeat.Size = new System.Drawing.Size(59, 17);
			this.cbBkaHeat.TabIndex = 2;
			this.cbBkaHeat.UseVisualStyleBackColor = false;
			this.cbBkaHeat.CheckedChanged += new System.EventHandler(this.cbBkaHeat_CheckedChanged);
			// 
			// cbBkaCool
			// 
			this.cbBkaCool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbBkaCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbBkaCool.Location = new System.Drawing.Point(188, 49);
			this.cbBkaCool.Name = "cbBkaCool";
			this.cbBkaCool.Size = new System.Drawing.Size(60, 17);
			this.cbBkaCool.TabIndex = 3;
			this.cbBkaCool.UseVisualStyleBackColor = false;
			this.cbBkaCool.CheckedChanged += new System.EventHandler(this.cbBkaCool_CheckedChanged);
			// 
			// lblAssumptions
			// 
			this.lblAssumptions.BackColor = System.Drawing.Color.Transparent;
			this.lblAssumptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAssumptions.Location = new System.Drawing.Point(315, 74);
			this.lblAssumptions.Margin = new System.Windows.Forms.Padding(3);
			this.lblAssumptions.Name = "lblAssumptions";
			this.lblAssumptions.Size = new System.Drawing.Size(93, 17);
			this.lblAssumptions.TabIndex = 18;
			this.lblAssumptions.Text = "Annahmen";
			this.lblAssumptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtCoolTemperature
			// 
			this.txtCoolTemperature.EditType = Europlan.Common.NumericBox.NumericEditType.FLOW_TEMPERATURE;
			this.txtCoolTemperature.InternalValue = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.txtCoolTemperature.Location = new System.Drawing.Point(412, 128);
			this.txtCoolTemperature.Name = "txtCoolTemperature";
			this.txtCoolTemperature.ReadOnly = true;
			this.txtCoolTemperature.Size = new System.Drawing.Size(56, 20);
			this.txtCoolTemperature.TabIndex = 28;
			this.txtCoolTemperature.Text = "16";
			this.txtCoolTemperature.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
			// 
			// txtAllocation
			// 
			this.txtAllocation.EditType = Europlan.Common.NumericBox.NumericEditType.PERCENTAGE;
			this.txtAllocation.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtAllocation.Location = new System.Drawing.Point(391, 211);
			this.txtAllocation.Name = "txtAllocation";
			this.txtAllocation.Size = new System.Drawing.Size(56, 20);
			this.txtAllocation.TabIndex = 25;
			this.txtAllocation.Text = "0";
			this.txtAllocation.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtAllocation.ValueChanged += new System.EventHandler(this.txtAllocation_ValueChanged);
			// 
			// pageDistributors
			// 
			this.pageDistributors.Controls.Add(this.quickDimensioningDistributorsSummary);
			this.pageDistributors.Location = new System.Drawing.Point(4, 22);
			this.pageDistributors.Name = "pageDistributors";
			this.pageDistributors.Padding = new System.Windows.Forms.Padding(3);
			this.pageDistributors.Size = new System.Drawing.Size(912, 512);
			this.pageDistributors.TabIndex = 2;
			this.pageDistributors.Text = "Verteiler";
			this.pageDistributors.UseVisualStyleBackColor = true;
			// 
			// quickDimensioningDistributorsSummary
			// 
			this.quickDimensioningDistributorsSummary.ConcreteActivation = true;
			this.quickDimensioningDistributorsSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			this.quickDimensioningDistributorsSummary.Euroval = true;
			this.quickDimensioningDistributorsSummary.Hitherm = true;
			this.quickDimensioningDistributorsSummary.HithermCompact = true;
			this.quickDimensioningDistributorsSummary.Location = new System.Drawing.Point(3, 3);
			this.quickDimensioningDistributorsSummary.ModulKlimaBoden = true;
			this.quickDimensioningDistributorsSummary.ModulKlimaDecke = true;
			this.quickDimensioningDistributorsSummary.Name = "quickDimensioningDistributorsSummary";
			this.quickDimensioningDistributorsSummary.Size = new System.Drawing.Size(906, 506);
			this.quickDimensioningDistributorsSummary.TabIndex = 0;
			this.quickDimensioningDistributorsSummary.ProjectChanged += new Europlan.Common.ProjectChangedHandler(this.quickDimensioningDistributorsSummary_ProjectChanged);
			// 
			// quickDimensioningRoomDistributorsWrapperBindingSource
			// 
			this.quickDimensioningRoomDistributorsWrapperBindingSource.DataSource = typeof(Europlan.Common.QuickDimensioningRoomDistributorsWrapper);
			// 
			// QuickDimensioningPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabQuickDimensioning);
			this.Name = "QuickDimensioningPanel";
			this.Size = new System.Drawing.Size(920, 538);
			this.tabQuickDimensioning.ResumeLayout(false);
			this.pageSummary.ResumeLayout(false);
			this.pageSettings.ResumeLayout(false);
			this.pageSettings.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.pageDistributors.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.quickDimensioningRoomDistributorsWrapperBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabQuickDimensioning;
		private System.Windows.Forms.TabPage pageSettings;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblEuroval;
		private System.Windows.Forms.CheckBox cbEurovalHeat;
		private System.Windows.Forms.CheckBox cbEurovalCool;
		private System.Windows.Forms.CheckBox cbModulKlimaDeckeHeat;
		private System.Windows.Forms.Label lblModulKlimaDecke;
		private System.Windows.Forms.Label lblHeat;
		private System.Windows.Forms.Label lblCool;
		private System.Windows.Forms.Label lblHitherm;
		private System.Windows.Forms.Label lblHithermCompact;
		private System.Windows.Forms.Label lblModulKlimaBoden;
		private System.Windows.Forms.CheckBox cbHithermHeat;
		private System.Windows.Forms.CheckBox cbHithermCool;
		private System.Windows.Forms.CheckBox cbHithermCompactHeat;
		private System.Windows.Forms.CheckBox cbModulKlimaBodenHeat;
		private System.Windows.Forms.CheckBox cbModulKlimaBodenCool;
		private System.Windows.Forms.CheckBox cbHithermCompactCool;
		private System.Windows.Forms.CheckBox cbModulKlimaDeckeCool;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblTemp2;
		private System.Windows.Forms.ComboBox cmbDistance;
		private System.Windows.Forms.Label lblDistance;
		private System.Windows.Forms.Label lblAssumptions;
		private System.Windows.Forms.Label lblTemp1;
		private System.Windows.Forms.Label lblAllocation;
		private System.Windows.Forms.Label lblAllocation2;
		private NumericBox txtAllocation;
		private System.Windows.Forms.Label lblBka;
		private System.Windows.Forms.CheckBox cbBkaHeat;
		private System.Windows.Forms.CheckBox cbBkaCool;
		private NumericBox txtCoolTemperature;
		private System.Windows.Forms.Label lblTemp4;
		private System.Windows.Forms.Label lblTemp3;
		private System.Windows.Forms.Button btnRevert;
		private System.Windows.Forms.ComboBox cmbHeatFlowTemperature;
		private System.Windows.Forms.TabPage pageDistributors;
		private QuickDimensioningDistributorsSummary quickDimensioningDistributorsSummary;
		private System.Windows.Forms.BindingSource quickDimensioningRoomDistributorsWrapperBindingSource;
		private System.Windows.Forms.TabPage pageSummary;
		private combit.ListLabel14.ListLabelPreviewControl listLabelPreviewControl1;
		private System.Windows.Forms.Button button1;
	}
}
