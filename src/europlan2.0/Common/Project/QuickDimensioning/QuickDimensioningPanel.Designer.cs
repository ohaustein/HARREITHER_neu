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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickDimensioningPanel));
			this.tabQuickDimensioning = new System.Windows.Forms.TabControl();
			this.pageSummary = new System.Windows.Forms.TabPage();
			this.pageSettings = new System.Windows.Forms.TabPage();
			this.cmbHeatFlowTemperature = new System.Windows.Forms.ComboBox();
			this.lblTemp4 = new System.Windows.Forms.Label();
			this.lblTemp3 = new System.Windows.Forms.Label();
			this.lblTemp2 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblTemp1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblEuroval = new System.Windows.Forms.Label();
			this.btnRevert = new System.Windows.Forms.Button();
			this.cbEurovalHeat = new System.Windows.Forms.CheckBox();
			this.lblAllocation2 = new System.Windows.Forms.Label();
			this.cbEurovalCool = new System.Windows.Forms.CheckBox();
			this.lblAllocation = new System.Windows.Forms.Label();
			this.lblHeat = new System.Windows.Forms.Label();
			this.lblAssumptions = new System.Windows.Forms.Label();
			this.cmbDistance = new System.Windows.Forms.ComboBox();
			this.lblCool = new System.Windows.Forms.Label();
			this.cbHithermCompactCool = new System.Windows.Forms.CheckBox();
			this.cbHithermCompactHeat = new System.Windows.Forms.CheckBox();
			this.lblHithermCompact = new System.Windows.Forms.Label();
			this.cbHithermCool = new System.Windows.Forms.CheckBox();
			this.cbHithermHeat = new System.Windows.Forms.CheckBox();
			this.lblHitherm = new System.Windows.Forms.Label();
			this.lblBka = new System.Windows.Forms.Label();
			this.cbBkaHeat = new System.Windows.Forms.CheckBox();
			this.cbBkaCool = new System.Windows.Forms.CheckBox();
			this.lblModulKlimaDecke = new System.Windows.Forms.Label();
			this.cbModulKlimaDeckeHeat = new System.Windows.Forms.CheckBox();
			this.cbModulKlimaDeckeCool = new System.Windows.Forms.CheckBox();
			this.lblModulKlimaBoden = new System.Windows.Forms.Label();
			this.cbModulKlimaBodenHeat = new System.Windows.Forms.CheckBox();
			this.cbModulKlimaBodenCool = new System.Windows.Forms.CheckBox();
			this.lblHithermCompactRoof = new System.Windows.Forms.Label();
			this.cbHithermCompactRoofHeat = new System.Windows.Forms.CheckBox();
			this.cbHithermCompactRoofCool = new System.Windows.Forms.CheckBox();
			this.lblDistance = new System.Windows.Forms.Label();
			this.txtAllocation = new Europlan.Common.NumericBox();
			this.txtCoolTemperature = new Europlan.Common.NumericBox();
			this.pageDistributors = new System.Windows.Forms.TabPage();
			this.quickDimensioningDistributorsSummary = new Europlan.Common.QuickDimensioningDistributorsSummary();
			this.label3 = new System.Windows.Forms.Label();
			this.quickDimensioningRoomDistributorsWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tabQuickDimensioning.SuspendLayout();
			this.pageSettings.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.pageDistributors.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.quickDimensioningRoomDistributorsWrapperBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tabQuickDimensioning
			// 
			this.tabQuickDimensioning.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabQuickDimensioning.Controls.Add(this.pageSummary);
			this.tabQuickDimensioning.Controls.Add(this.pageSettings);
			this.tabQuickDimensioning.Controls.Add(this.pageDistributors);
			this.tabQuickDimensioning.Location = new System.Drawing.Point(0, 38);
			this.tabQuickDimensioning.Name = "tabQuickDimensioning";
			this.tabQuickDimensioning.SelectedIndex = 0;
			this.tabQuickDimensioning.Size = new System.Drawing.Size(865, 521);
			this.tabQuickDimensioning.TabIndex = 0;
			this.tabQuickDimensioning.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabQuickDimensioning_Selecting);
			this.tabQuickDimensioning.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabQuickDimensioning_Selected);
			// 
			// pageSummary
			// 
			this.pageSummary.Location = new System.Drawing.Point(4, 22);
			this.pageSummary.Name = "pageSummary";
			this.pageSummary.Padding = new System.Windows.Forms.Padding(3);
			this.pageSummary.Size = new System.Drawing.Size(857, 495);
			this.pageSummary.TabIndex = 1;
			this.pageSummary.Text = "Ergebnis Flächenaufstellung";
			this.pageSummary.UseVisualStyleBackColor = true;
			// 
			// pageSettings
			// 
			this.pageSettings.Controls.Add(this.cmbHeatFlowTemperature);
			this.pageSettings.Controls.Add(this.lblTemp4);
			this.pageSettings.Controls.Add(this.lblTemp3);
			this.pageSettings.Controls.Add(this.lblTemp2);
			this.pageSettings.Controls.Add(this.label2);
			this.pageSettings.Controls.Add(this.lblTemp1);
			this.pageSettings.Controls.Add(this.tableLayoutPanel1);
			this.pageSettings.Controls.Add(this.txtCoolTemperature);
			this.pageSettings.Location = new System.Drawing.Point(4, 22);
			this.pageSettings.Name = "pageSettings";
			this.pageSettings.Padding = new System.Windows.Forms.Padding(3);
			this.pageSettings.Size = new System.Drawing.Size(857, 495);
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
			this.cmbHeatFlowTemperature.Location = new System.Drawing.Point(629, 53);
			this.cmbHeatFlowTemperature.Name = "cmbHeatFlowTemperature";
			this.cmbHeatFlowTemperature.Size = new System.Drawing.Size(56, 21);
			this.cmbHeatFlowTemperature.TabIndex = 30;
			this.cmbHeatFlowTemperature.SelectedIndexChanged += new System.EventHandler(this.cmbHeatFlowTemperature_SelectedIndexChanged);
			// 
			// lblTemp4
			// 
			this.lblTemp4.AutoSize = true;
			this.lblTemp4.Location = new System.Drawing.Point(691, 88);
			this.lblTemp4.Name = "lblTemp4";
			this.lblTemp4.Size = new System.Drawing.Size(40, 13);
			this.lblTemp4.TabIndex = 26;
			this.lblTemp4.Text = "°C (Tv)";
			// 
			// lblTemp3
			// 
			this.lblTemp3.Location = new System.Drawing.Point(530, 89);
			this.lblTemp3.Name = "lblTemp3";
			this.lblTemp3.Size = new System.Drawing.Size(93, 29);
			this.lblTemp3.TabIndex = 27;
			this.lblTemp3.Text = "Vorlauftemperatur\r\n(Kühlen)";
			// 
			// lblTemp2
			// 
			this.lblTemp2.AutoSize = true;
			this.lblTemp2.Location = new System.Drawing.Point(691, 56);
			this.lblTemp2.Name = "lblTemp2";
			this.lblTemp2.Size = new System.Drawing.Size(40, 13);
			this.lblTemp2.TabIndex = 3;
			this.lblTemp2.Text = "°C (Tv)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(519, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Bitte wählen Sie jene Harreither-Produkte aus, welche in der Flächenaufstellung z" +
				"ur Verfügung stehen sollen:";
			// 
			// lblTemp1
			// 
			this.lblTemp1.Location = new System.Drawing.Point(530, 57);
			this.lblTemp1.Name = "lblTemp1";
			this.lblTemp1.Size = new System.Drawing.Size(93, 29);
			this.lblTemp1.TabIndex = 23;
			this.lblTemp1.Text = "Vorlauftemperatur\r\n(Heizen)";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.tableLayoutPanel1.Controls.Add(this.lblEuroval, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnRevert, 0, 9);
			this.tableLayoutPanel1.Controls.Add(this.cbEurovalHeat, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblAllocation2, 5, 7);
			this.tableLayoutPanel1.Controls.Add(this.cbEurovalCool, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblAllocation, 3, 7);
			this.tableLayoutPanel1.Controls.Add(this.lblHeat, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblAssumptions, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmbDistance, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblCool, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactCool, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactHeat, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblHithermCompact, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermCool, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermHeat, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblHitherm, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblBka, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.cbBkaHeat, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.cbBkaCool, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblModulKlimaDecke, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaDeckeHeat, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaDeckeCool, 2, 7);
			this.tableLayoutPanel1.Controls.Add(this.lblModulKlimaBoden, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaBodenHeat, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.cbModulKlimaBodenCool, 2, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblHithermCompactRoof, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactRoofHeat, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.cbHithermCompactRoofCool, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblDistance, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtAllocation, 4, 7);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 29);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 10;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(504, 253);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblEuroval
			// 
			this.lblEuroval.BackColor = System.Drawing.Color.Transparent;
			this.lblEuroval.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblEuroval.Location = new System.Drawing.Point(3, 49);
			this.lblEuroval.Margin = new System.Windows.Forms.Padding(3);
			this.lblEuroval.Name = "lblEuroval";
			this.lblEuroval.Size = new System.Drawing.Size(165, 17);
			this.lblEuroval.TabIndex = 1;
			this.lblEuroval.Text = "Euroval® Fußbodenheizung";
			this.lblEuroval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnRevert
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnRevert, 3);
			this.btnRevert.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnRevert.Location = new System.Drawing.Point(0, 230);
			this.btnRevert.Margin = new System.Windows.Forms.Padding(0);
			this.btnRevert.Name = "btnRevert";
			this.btnRevert.Size = new System.Drawing.Size(323, 23);
			this.btnRevert.TabIndex = 29;
			this.btnRevert.Text = "Flächenaufstellung zurücksetzen";
			this.btnRevert.UseVisualStyleBackColor = true;
			this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
			// 
			// cbEurovalHeat
			// 
			this.cbEurovalHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbEurovalHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbEurovalHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbEurovalHeat.Location = new System.Drawing.Point(174, 49);
			this.cbEurovalHeat.Name = "cbEurovalHeat";
			this.cbEurovalHeat.Size = new System.Drawing.Size(70, 17);
			this.cbEurovalHeat.TabIndex = 2;
			this.cbEurovalHeat.UseVisualStyleBackColor = false;
			this.cbEurovalHeat.CheckedChanged += new System.EventHandler(this.cbEurovalHeat_CheckedChanged);
			// 
			// lblAllocation2
			// 
			this.lblAllocation2.AutoSize = true;
			this.lblAllocation2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblAllocation2.Location = new System.Drawing.Point(486, 184);
			this.lblAllocation2.Name = "lblAllocation2";
			this.lblAllocation2.Size = new System.Drawing.Size(15, 23);
			this.lblAllocation2.TabIndex = 21;
			this.lblAllocation2.Text = "%";
			this.lblAllocation2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbEurovalCool
			// 
			this.cbEurovalCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbEurovalCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbEurovalCool.Location = new System.Drawing.Point(250, 49);
			this.cbEurovalCool.Name = "cbEurovalCool";
			this.cbEurovalCool.Size = new System.Drawing.Size(70, 17);
			this.cbEurovalCool.TabIndex = 3;
			this.cbEurovalCool.UseVisualStyleBackColor = false;
			this.cbEurovalCool.CheckedChanged += new System.EventHandler(this.cbEurovalCool_CheckedChanged);
			// 
			// lblAllocation
			// 
			this.lblAllocation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblAllocation.Location = new System.Drawing.Point(326, 184);
			this.lblAllocation.Name = "lblAllocation";
			this.lblAllocation.Size = new System.Drawing.Size(84, 23);
			this.lblAllocation.TabIndex = 22;
			this.lblAllocation.Text = "Belegefaktor";
			this.lblAllocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblHeat
			// 
			this.lblHeat.BackColor = System.Drawing.Color.Transparent;
			this.lblHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeat.Location = new System.Drawing.Point(174, 3);
			this.lblHeat.Margin = new System.Windows.Forms.Padding(3);
			this.lblHeat.Name = "lblHeat";
			this.lblHeat.Size = new System.Drawing.Size(70, 40);
			this.lblHeat.TabIndex = 4;
			this.lblHeat.Text = "Heizen";
			this.lblHeat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblAssumptions
			// 
			this.lblAssumptions.BackColor = System.Drawing.Color.Transparent;
			this.lblAssumptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblAssumptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAssumptions.Location = new System.Drawing.Point(326, 3);
			this.lblAssumptions.Margin = new System.Windows.Forms.Padding(3);
			this.lblAssumptions.Name = "lblAssumptions";
			this.lblAssumptions.Size = new System.Drawing.Size(84, 40);
			this.lblAssumptions.TabIndex = 18;
			this.lblAssumptions.Text = "Annahmen";
			this.lblAssumptions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbDistance
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.cmbDistance, 2);
			this.cmbDistance.Dock = System.Windows.Forms.DockStyle.Fill;
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
			this.cmbDistance.Location = new System.Drawing.Point(416, 47);
			this.cmbDistance.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this.cmbDistance.Name = "cmbDistance";
			this.cmbDistance.Size = new System.Drawing.Size(85, 21);
			this.cmbDistance.TabIndex = 5;
			this.cmbDistance.SelectedIndexChanged += new System.EventHandler(this.cmbDistance_SelectedIndexChanged);
			// 
			// lblCool
			// 
			this.lblCool.BackColor = System.Drawing.Color.Transparent;
			this.lblCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblCool.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCool.Location = new System.Drawing.Point(250, 3);
			this.lblCool.Margin = new System.Windows.Forms.Padding(3);
			this.lblCool.Name = "lblCool";
			this.lblCool.Size = new System.Drawing.Size(70, 40);
			this.lblCool.TabIndex = 5;
			this.lblCool.Text = "Kühlen";
			this.lblCool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cbHithermCompactCool
			// 
			this.cbHithermCompactCool.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermCompactCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermCompactCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbHithermCompactCool.Location = new System.Drawing.Point(250, 118);
			this.cbHithermCompactCool.Name = "cbHithermCompactCool";
			this.cbHithermCompactCool.Size = new System.Drawing.Size(70, 17);
			this.cbHithermCompactCool.TabIndex = 15;
			this.cbHithermCompactCool.UseVisualStyleBackColor = false;
			this.cbHithermCompactCool.CheckedChanged += new System.EventHandler(this.cbHithermCompactCool_CheckedChanged);
			// 
			// cbHithermCompactHeat
			// 
			this.cbHithermCompactHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermCompactHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermCompactHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbHithermCompactHeat.Location = new System.Drawing.Point(174, 118);
			this.cbHithermCompactHeat.Name = "cbHithermCompactHeat";
			this.cbHithermCompactHeat.Size = new System.Drawing.Size(70, 17);
			this.cbHithermCompactHeat.TabIndex = 12;
			this.cbHithermCompactHeat.UseVisualStyleBackColor = false;
			this.cbHithermCompactHeat.CheckedChanged += new System.EventHandler(this.cbHithermCompactHeat_CheckedChanged);
			// 
			// lblHithermCompact
			// 
			this.lblHithermCompact.BackColor = System.Drawing.Color.Transparent;
			this.lblHithermCompact.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHithermCompact.Location = new System.Drawing.Point(3, 118);
			this.lblHithermCompact.Margin = new System.Windows.Forms.Padding(3);
			this.lblHithermCompact.Name = "lblHithermCompact";
			this.lblHithermCompact.Size = new System.Drawing.Size(165, 17);
			this.lblHithermCompact.TabIndex = 7;
			this.lblHithermCompact.Text = "Hitherm® Compact";
			this.lblHithermCompact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbHithermCool
			// 
			this.cbHithermCool.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbHithermCool.Location = new System.Drawing.Point(250, 95);
			this.cbHithermCool.Name = "cbHithermCool";
			this.cbHithermCool.Size = new System.Drawing.Size(70, 17);
			this.cbHithermCool.TabIndex = 11;
			this.cbHithermCool.UseVisualStyleBackColor = false;
			this.cbHithermCool.CheckedChanged += new System.EventHandler(this.cbHithermCool_CheckedChanged);
			// 
			// cbHithermHeat
			// 
			this.cbHithermHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbHithermHeat.Location = new System.Drawing.Point(174, 95);
			this.cbHithermHeat.Name = "cbHithermHeat";
			this.cbHithermHeat.Size = new System.Drawing.Size(70, 17);
			this.cbHithermHeat.TabIndex = 10;
			this.cbHithermHeat.UseVisualStyleBackColor = false;
			this.cbHithermHeat.CheckedChanged += new System.EventHandler(this.cbHithermHeat_CheckedChanged);
			// 
			// lblHitherm
			// 
			this.lblHitherm.BackColor = System.Drawing.Color.Transparent;
			this.lblHitherm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHitherm.Location = new System.Drawing.Point(3, 95);
			this.lblHitherm.Margin = new System.Windows.Forms.Padding(3);
			this.lblHitherm.Name = "lblHitherm";
			this.lblHitherm.Size = new System.Drawing.Size(165, 17);
			this.lblHitherm.TabIndex = 6;
			this.lblHitherm.Text = "Hitherm® Klimawand";
			this.lblHitherm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblBka
			// 
			this.lblBka.BackColor = System.Drawing.Color.Transparent;
			this.lblBka.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblBka.Location = new System.Drawing.Point(3, 72);
			this.lblBka.Margin = new System.Windows.Forms.Padding(3);
			this.lblBka.Name = "lblBka";
			this.lblBka.Size = new System.Drawing.Size(165, 17);
			this.lblBka.TabIndex = 1;
			this.lblBka.Text = "Betonkernaktivierung";
			this.lblBka.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbBkaHeat
			// 
			this.cbBkaHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbBkaHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbBkaHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbBkaHeat.Location = new System.Drawing.Point(174, 72);
			this.cbBkaHeat.Name = "cbBkaHeat";
			this.cbBkaHeat.Size = new System.Drawing.Size(70, 17);
			this.cbBkaHeat.TabIndex = 2;
			this.cbBkaHeat.UseVisualStyleBackColor = false;
			this.cbBkaHeat.CheckedChanged += new System.EventHandler(this.cbBkaHeat_CheckedChanged);
			// 
			// cbBkaCool
			// 
			this.cbBkaCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbBkaCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbBkaCool.Location = new System.Drawing.Point(250, 72);
			this.cbBkaCool.Name = "cbBkaCool";
			this.cbBkaCool.Size = new System.Drawing.Size(70, 17);
			this.cbBkaCool.TabIndex = 3;
			this.cbBkaCool.UseVisualStyleBackColor = false;
			this.cbBkaCool.CheckedChanged += new System.EventHandler(this.cbBkaCool_CheckedChanged);
			// 
			// lblModulKlimaDecke
			// 
			this.lblModulKlimaDecke.BackColor = System.Drawing.Color.Transparent;
			this.lblModulKlimaDecke.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblModulKlimaDecke.Location = new System.Drawing.Point(3, 187);
			this.lblModulKlimaDecke.Margin = new System.Windows.Forms.Padding(3);
			this.lblModulKlimaDecke.Name = "lblModulKlimaDecke";
			this.lblModulKlimaDecke.Size = new System.Drawing.Size(165, 17);
			this.lblModulKlimaDecke.TabIndex = 9;
			this.lblModulKlimaDecke.Text = "Modul Klima-Decke";
			this.lblModulKlimaDecke.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbModulKlimaDeckeHeat
			// 
			this.cbModulKlimaDeckeHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbModulKlimaDeckeHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModulKlimaDeckeHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbModulKlimaDeckeHeat.Location = new System.Drawing.Point(174, 187);
			this.cbModulKlimaDeckeHeat.Name = "cbModulKlimaDeckeHeat";
			this.cbModulKlimaDeckeHeat.Size = new System.Drawing.Size(70, 17);
			this.cbModulKlimaDeckeHeat.TabIndex = 17;
			this.cbModulKlimaDeckeHeat.UseVisualStyleBackColor = false;
			this.cbModulKlimaDeckeHeat.CheckedChanged += new System.EventHandler(this.cbModulKlimaDeckeHeat_CheckedChanged);
			// 
			// cbModulKlimaDeckeCool
			// 
			this.cbModulKlimaDeckeCool.BackColor = System.Drawing.Color.Transparent;
			this.cbModulKlimaDeckeCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModulKlimaDeckeCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbModulKlimaDeckeCool.Location = new System.Drawing.Point(250, 187);
			this.cbModulKlimaDeckeCool.Name = "cbModulKlimaDeckeCool";
			this.cbModulKlimaDeckeCool.Size = new System.Drawing.Size(70, 17);
			this.cbModulKlimaDeckeCool.TabIndex = 16;
			this.cbModulKlimaDeckeCool.UseVisualStyleBackColor = false;
			this.cbModulKlimaDeckeCool.CheckedChanged += new System.EventHandler(this.cbModulKlimaDeckeCool_CheckedChanged);
			// 
			// lblModulKlimaBoden
			// 
			this.lblModulKlimaBoden.BackColor = System.Drawing.Color.Transparent;
			this.lblModulKlimaBoden.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblModulKlimaBoden.Location = new System.Drawing.Point(3, 164);
			this.lblModulKlimaBoden.Margin = new System.Windows.Forms.Padding(3);
			this.lblModulKlimaBoden.Name = "lblModulKlimaBoden";
			this.lblModulKlimaBoden.Size = new System.Drawing.Size(165, 17);
			this.lblModulKlimaBoden.TabIndex = 8;
			this.lblModulKlimaBoden.Text = "Modul Klima-Boden";
			this.lblModulKlimaBoden.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbModulKlimaBodenHeat
			// 
			this.cbModulKlimaBodenHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbModulKlimaBodenHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModulKlimaBodenHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbModulKlimaBodenHeat.Location = new System.Drawing.Point(174, 164);
			this.cbModulKlimaBodenHeat.Name = "cbModulKlimaBodenHeat";
			this.cbModulKlimaBodenHeat.Size = new System.Drawing.Size(70, 17);
			this.cbModulKlimaBodenHeat.TabIndex = 13;
			this.cbModulKlimaBodenHeat.UseVisualStyleBackColor = false;
			this.cbModulKlimaBodenHeat.CheckedChanged += new System.EventHandler(this.cbModulKlimaBodenHeat_CheckedChanged);
			// 
			// cbModulKlimaBodenCool
			// 
			this.cbModulKlimaBodenCool.BackColor = System.Drawing.Color.Transparent;
			this.cbModulKlimaBodenCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbModulKlimaBodenCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbModulKlimaBodenCool.Location = new System.Drawing.Point(250, 164);
			this.cbModulKlimaBodenCool.Name = "cbModulKlimaBodenCool";
			this.cbModulKlimaBodenCool.Size = new System.Drawing.Size(70, 17);
			this.cbModulKlimaBodenCool.TabIndex = 14;
			this.cbModulKlimaBodenCool.UseVisualStyleBackColor = false;
			this.cbModulKlimaBodenCool.CheckedChanged += new System.EventHandler(this.cbModulKlimaBodenCool_CheckedChanged);
			// 
			// lblHithermCompactRoof
			// 
			this.lblHithermCompactRoof.BackColor = System.Drawing.Color.Transparent;
			this.lblHithermCompactRoof.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHithermCompactRoof.Location = new System.Drawing.Point(3, 141);
			this.lblHithermCompactRoof.Margin = new System.Windows.Forms.Padding(3);
			this.lblHithermCompactRoof.Name = "lblHithermCompactRoof";
			this.lblHithermCompactRoof.Size = new System.Drawing.Size(165, 17);
			this.lblHithermCompactRoof.TabIndex = 7;
			this.lblHithermCompactRoof.Text = "Hitherm® Compact Dachschräge";
			this.lblHithermCompactRoof.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbHithermCompactRoofHeat
			// 
			this.cbHithermCompactRoofHeat.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermCompactRoofHeat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermCompactRoofHeat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbHithermCompactRoofHeat.Location = new System.Drawing.Point(174, 141);
			this.cbHithermCompactRoofHeat.Name = "cbHithermCompactRoofHeat";
			this.cbHithermCompactRoofHeat.Size = new System.Drawing.Size(70, 17);
			this.cbHithermCompactRoofHeat.TabIndex = 12;
			this.cbHithermCompactRoofHeat.UseVisualStyleBackColor = false;
			this.cbHithermCompactRoofHeat.CheckedChanged += new System.EventHandler(this.cbHithermCompactRoofHeat_CheckedChanged);
			// 
			// cbHithermCompactRoofCool
			// 
			this.cbHithermCompactRoofCool.BackColor = System.Drawing.Color.Transparent;
			this.cbHithermCompactRoofCool.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbHithermCompactRoofCool.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbHithermCompactRoofCool.Location = new System.Drawing.Point(250, 141);
			this.cbHithermCompactRoofCool.Name = "cbHithermCompactRoofCool";
			this.cbHithermCompactRoofCool.Size = new System.Drawing.Size(70, 17);
			this.cbHithermCompactRoofCool.TabIndex = 15;
			this.cbHithermCompactRoofCool.UseVisualStyleBackColor = false;
			this.cbHithermCompactRoofCool.CheckedChanged += new System.EventHandler(this.cbHithermCompactRoofCool_CheckedChanged);
			// 
			// lblDistance
			// 
			this.lblDistance.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblDistance.Location = new System.Drawing.Point(326, 46);
			this.lblDistance.Name = "lblDistance";
			this.lblDistance.Size = new System.Drawing.Size(84, 23);
			this.lblDistance.TabIndex = 19;
			this.lblDistance.Text = "Verlegeabstand";
			this.lblDistance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtAllocation
			// 
			this.txtAllocation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtAllocation.EditType = Europlan.Common.NumericBox.NumericEditType.PERCENTAGE;
			this.txtAllocation.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtAllocation.Location = new System.Drawing.Point(416, 186);
			this.txtAllocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 1);
			this.txtAllocation.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.txtAllocation.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtAllocation.Name = "txtAllocation";
			this.txtAllocation.Size = new System.Drawing.Size(64, 20);
			this.txtAllocation.TabIndex = 25;
			this.txtAllocation.Text = "0";
			this.txtAllocation.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtAllocation.ValueChanged += new System.EventHandler(this.txtAllocation_ValueChanged);
			// 
			// txtCoolTemperature
			// 
			this.txtCoolTemperature.EditType = Europlan.Common.NumericBox.NumericEditType.FLOW_TEMPERATURE;
			this.txtCoolTemperature.InternalValue = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.txtCoolTemperature.Location = new System.Drawing.Point(629, 86);
			this.txtCoolTemperature.MaxValue = null;
			this.txtCoolTemperature.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
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
			// pageDistributors
			// 
			this.pageDistributors.Controls.Add(this.quickDimensioningDistributorsSummary);
			this.pageDistributors.Location = new System.Drawing.Point(4, 22);
			this.pageDistributors.Name = "pageDistributors";
			this.pageDistributors.Padding = new System.Windows.Forms.Padding(3);
			this.pageDistributors.Size = new System.Drawing.Size(857, 495);
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
			this.quickDimensioningDistributorsSummary.HithermCompactRoof = true;
			this.quickDimensioningDistributorsSummary.Location = new System.Drawing.Point(3, 3);
			this.quickDimensioningDistributorsSummary.ModulKlimaBoden = true;
			this.quickDimensioningDistributorsSummary.ModulKlimaDecke = true;
			this.quickDimensioningDistributorsSummary.Name = "quickDimensioningDistributorsSummary";
			this.quickDimensioningDistributorsSummary.Size = new System.Drawing.Size(851, 489);
			this.quickDimensioningDistributorsSummary.TabIndex = 0;
			this.quickDimensioningDistributorsSummary.ProjectChanged += new Europlan.Common.ProjectChangedHandler(this.quickDimensioningDistributorsSummary_ProjectChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(34, 5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(187, 24);
			this.label3.TabIndex = 3;
			this.label3.Text = "Flächenaufstellung";
			// 
			// quickDimensioningRoomDistributorsWrapperBindingSource
			// 
			this.quickDimensioningRoomDistributorsWrapperBindingSource.DataSource = typeof(Europlan.Common.QuickDimensioningRoomDistributorsWrapper);
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.MaximumSize = new System.Drawing.Size(32, 32);
			this.pictureBox1.MinimumSize = new System.Drawing.Size(32, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 78;
			this.pictureBox1.TabStop = false;
			// 
			// QuickDimensioningPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tabQuickDimensioning);
			this.helpProvider.SetHelpKeyword(this, "html\\Flächenaufstellung.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.Name = "QuickDimensioningPanel";
			this.helpProvider.SetShowHelp(this, true);
			this.Size = new System.Drawing.Size(865, 559);
			this.tabQuickDimensioning.ResumeLayout(false);
			this.pageSettings.ResumeLayout(false);
			this.pageSettings.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.pageDistributors.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.quickDimensioningRoomDistributorsWrapperBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private System.Windows.Forms.Label lblHithermCompactRoof;
		private System.Windows.Forms.CheckBox cbHithermCompactRoofHeat;
		private System.Windows.Forms.CheckBox cbHithermCompactRoofCool;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.PictureBox pictureBox1;
	}
}
