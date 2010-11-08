namespace Europlan.Common.Products {
	partial class ModulKlimaDeckePlannerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModulKlimaDeckePlannerForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.btnConstruction = new System.Windows.Forms.ToolStripButton();
			this.btnAddModules = new System.Windows.Forms.ToolStripButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabs = new System.Windows.Forms.TabControl();
			this.pageConstruction = new System.Windows.Forms.TabPage();
			this.grpCeilingContruction = new System.Windows.Forms.GroupBox();
			this.rbKassetten = new System.Windows.Forms.RadioButton();
			this.rbAkustik = new System.Windows.Forms.RadioButton();
			this.rbGlatt = new System.Windows.Forms.RadioButton();
			this.grpConstructionParameter = new System.Windows.Forms.GroupBox();
			this.btnVertical = new System.Windows.Forms.Button();
			this.btnHorizontal = new System.Windows.Forms.Button();
			this.btnCwLarge = new System.Windows.Forms.Button();
			this.btnCwSmall = new System.Windows.Forms.Button();
			this.btnCcwSmall = new System.Windows.Forms.Button();
			this.btnCcwLarge = new System.Windows.Forms.Button();
			this.lblRotationUnit = new System.Windows.Forms.Label();
			this.lblRotation = new System.Windows.Forms.Label();
			this.lblRandfriesUnit = new System.Windows.Forms.Label();
			this.lblRandfries = new System.Windows.Forms.Label();
			this.pageLayout = new System.Windows.Forms.TabPage();
			this.label13 = new System.Windows.Forms.Label();
			this.lstSubarea = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstRows = new System.Windows.Forms.ListBox();
			this.label12 = new System.Windows.Forms.Label();
			this.lstCircuits = new System.Windows.Forms.ListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbModulType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbOrientation = new System.Windows.Forms.ComboBox();
			this.planPanel = new Europlan.Common.PlanPanel();
			this.modulKlimaBodenPlanner = new Europlan.Common.ModulKlimaDeckePlanner(this.components);
			this.numRotation = new Europlan.Common.NumericBox();
			this.numRandfries = new Europlan.Common.NumericBox();
			this.toolStrip.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabs.SuspendLayout();
			this.pageConstruction.SuspendLayout();
			this.grpCeilingContruction.SuspendLayout();
			this.grpConstructionParameter.SuspendLayout();
			this.pageLayout.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomOut,
            this.btnZoomIn,
            this.toolStripSeparator1,
            this.btnMove,
            this.btnConstruction,
            this.btnAddModules});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(730, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.AutoToolTip = false;
			this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
			this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
			this.btnZoomOut.Text = "zoomOut";
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.AutoToolTip = false;
			this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
			this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
			this.btnZoomIn.Text = "zoomIn";
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnMove
			// 
			this.btnMove.AutoToolTip = false;
			this.btnMove.Checked = true;
			this.btnMove.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnMove.Image = ((System.Drawing.Image)(resources.GetObject("btnMove.Image")));
			this.btnMove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnMove.Name = "btnMove";
			this.btnMove.Size = new System.Drawing.Size(23, 22);
			this.btnMove.Text = "toolStripButton1";
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			// 
			// btnConstruction
			// 
			this.btnConstruction.AutoToolTip = false;
			this.btnConstruction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnConstruction.Image = ((System.Drawing.Image)(resources.GetObject("btnConstruction.Image")));
			this.btnConstruction.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnConstruction.Name = "btnConstruction";
			this.btnConstruction.Size = new System.Drawing.Size(23, 22);
			this.btnConstruction.Text = "toolStripButton1";
			this.btnConstruction.Click += new System.EventHandler(this.btnConstruction_Click);
			// 
			// btnAddModules
			// 
			this.btnAddModules.AutoToolTip = false;
			this.btnAddModules.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAddModules.Image = ((System.Drawing.Image)(resources.GetObject("btnAddModules.Image")));
			this.btnAddModules.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAddModules.Name = "btnAddModules";
			this.btnAddModules.Size = new System.Drawing.Size(23, 22);
			this.btnAddModules.Text = "toolStripButton1";
			this.btnAddModules.Visible = false;
			this.btnAddModules.Click += new System.EventHandler(this.btnAddModules_Click);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.tabs);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 334);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(730, 131);
			this.panel1.TabIndex = 2;
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.pageConstruction);
			this.tabs.Controls.Add(this.pageLayout);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(730, 131);
			this.tabs.TabIndex = 2;
			this.tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabs_Selecting);
			this.tabs.Deselected += new System.Windows.Forms.TabControlEventHandler(this.tabs_Deselected);
			// 
			// pageConstruction
			// 
			this.pageConstruction.Controls.Add(this.grpCeilingContruction);
			this.pageConstruction.Controls.Add(this.grpConstructionParameter);
			this.pageConstruction.Location = new System.Drawing.Point(4, 22);
			this.pageConstruction.Name = "pageConstruction";
			this.pageConstruction.Padding = new System.Windows.Forms.Padding(3);
			this.pageConstruction.Size = new System.Drawing.Size(722, 105);
			this.pageConstruction.TabIndex = 0;
			this.pageConstruction.Text = "Konstruktion einrichten";
			this.pageConstruction.UseVisualStyleBackColor = true;
			// 
			// grpCeilingContruction
			// 
			this.grpCeilingContruction.Controls.Add(this.rbKassetten);
			this.grpCeilingContruction.Controls.Add(this.rbAkustik);
			this.grpCeilingContruction.Controls.Add(this.rbGlatt);
			this.grpCeilingContruction.Location = new System.Drawing.Point(6, 6);
			this.grpCeilingContruction.Name = "grpCeilingContruction";
			this.grpCeilingContruction.Size = new System.Drawing.Size(181, 90);
			this.grpCeilingContruction.TabIndex = 0;
			this.grpCeilingContruction.TabStop = false;
			this.grpCeilingContruction.Text = "Deckenkonstruktion";
			// 
			// rbKassetten
			// 
			this.rbKassetten.AutoSize = true;
			this.rbKassetten.Location = new System.Drawing.Point(9, 65);
			this.rbKassetten.Name = "rbKassetten";
			this.rbKassetten.Size = new System.Drawing.Size(102, 17);
			this.rbKassetten.TabIndex = 3;
			this.rbKassetten.TabStop = true;
			this.rbKassetten.Text = "Kassettendecke";
			this.rbKassetten.UseVisualStyleBackColor = true;
			this.rbKassetten.Visible = false;
			// 
			// rbAkustik
			// 
			this.rbAkustik.AutoSize = true;
			this.rbAkustik.Location = new System.Drawing.Point(9, 42);
			this.rbAkustik.Name = "rbAkustik";
			this.rbAkustik.Size = new System.Drawing.Size(154, 17);
			this.rbAkustik.TabIndex = 2;
			this.rbAkustik.TabStop = true;
			this.rbAkustik.Text = "Akustikdecke mit Randfries";
			this.rbAkustik.UseVisualStyleBackColor = true;
			this.rbAkustik.CheckedChanged += new System.EventHandler(this.rbAkustik_CheckedChanged);
			// 
			// rbGlatt
			// 
			this.rbGlatt.AutoSize = true;
			this.rbGlatt.Checked = true;
			this.rbGlatt.Location = new System.Drawing.Point(9, 19);
			this.rbGlatt.Name = "rbGlatt";
			this.rbGlatt.Size = new System.Drawing.Size(47, 17);
			this.rbGlatt.TabIndex = 1;
			this.rbGlatt.TabStop = true;
			this.rbGlatt.Text = "Glatt";
			this.rbGlatt.UseVisualStyleBackColor = true;
			this.rbGlatt.CheckedChanged += new System.EventHandler(this.rbGlatt_CheckedChanged);
			// 
			// grpConstructionParameter
			// 
			this.grpConstructionParameter.Controls.Add(this.btnVertical);
			this.grpConstructionParameter.Controls.Add(this.btnHorizontal);
			this.grpConstructionParameter.Controls.Add(this.btnCwLarge);
			this.grpConstructionParameter.Controls.Add(this.btnCwSmall);
			this.grpConstructionParameter.Controls.Add(this.btnCcwSmall);
			this.grpConstructionParameter.Controls.Add(this.btnCcwLarge);
			this.grpConstructionParameter.Controls.Add(this.lblRotationUnit);
			this.grpConstructionParameter.Controls.Add(this.numRotation);
			this.grpConstructionParameter.Controls.Add(this.lblRotation);
			this.grpConstructionParameter.Controls.Add(this.lblRandfriesUnit);
			this.grpConstructionParameter.Controls.Add(this.numRandfries);
			this.grpConstructionParameter.Controls.Add(this.lblRandfries);
			this.grpConstructionParameter.Location = new System.Drawing.Point(193, 6);
			this.grpConstructionParameter.Name = "grpConstructionParameter";
			this.grpConstructionParameter.Size = new System.Drawing.Size(224, 90);
			this.grpConstructionParameter.TabIndex = 1;
			this.grpConstructionParameter.TabStop = false;
			this.grpConstructionParameter.Text = "Konstruktionsparameter";
			// 
			// btnVertical
			// 
			this.btnVertical.Image = ((System.Drawing.Image)(resources.GetObject("btnVertical.Image")));
			this.btnVertical.Location = new System.Drawing.Point(131, 38);
			this.btnVertical.Name = "btnVertical";
			this.btnVertical.Size = new System.Drawing.Size(24, 24);
			this.btnVertical.TabIndex = 15;
			this.btnVertical.UseVisualStyleBackColor = true;
			this.btnVertical.Click += new System.EventHandler(this.btnVertical_Click);
			// 
			// btnHorizontal
			// 
			this.btnHorizontal.Image = ((System.Drawing.Image)(resources.GetObject("btnHorizontal.Image")));
			this.btnHorizontal.Location = new System.Drawing.Point(108, 38);
			this.btnHorizontal.Name = "btnHorizontal";
			this.btnHorizontal.Size = new System.Drawing.Size(24, 24);
			this.btnHorizontal.TabIndex = 14;
			this.btnHorizontal.UseVisualStyleBackColor = true;
			this.btnHorizontal.Click += new System.EventHandler(this.btnHorizontal_Click);
			// 
			// btnCwLarge
			// 
			this.btnCwLarge.Image = ((System.Drawing.Image)(resources.GetObject("btnCwLarge.Image")));
			this.btnCwLarge.Location = new System.Drawing.Point(78, 38);
			this.btnCwLarge.Name = "btnCwLarge";
			this.btnCwLarge.Size = new System.Drawing.Size(24, 24);
			this.btnCwLarge.TabIndex = 13;
			this.btnCwLarge.UseVisualStyleBackColor = true;
			this.btnCwLarge.Click += new System.EventHandler(this.btnRotate_Click);
			// 
			// btnCwSmall
			// 
			this.btnCwSmall.Image = ((System.Drawing.Image)(resources.GetObject("btnCwSmall.Image")));
			this.btnCwSmall.Location = new System.Drawing.Point(55, 38);
			this.btnCwSmall.Name = "btnCwSmall";
			this.btnCwSmall.Size = new System.Drawing.Size(24, 24);
			this.btnCwSmall.TabIndex = 12;
			this.btnCwSmall.UseVisualStyleBackColor = true;
			this.btnCwSmall.Click += new System.EventHandler(this.btnRotate_Click);
			// 
			// btnCcwSmall
			// 
			this.btnCcwSmall.Image = ((System.Drawing.Image)(resources.GetObject("btnCcwSmall.Image")));
			this.btnCcwSmall.Location = new System.Drawing.Point(32, 38);
			this.btnCcwSmall.Name = "btnCcwSmall";
			this.btnCcwSmall.Size = new System.Drawing.Size(24, 24);
			this.btnCcwSmall.TabIndex = 11;
			this.btnCcwSmall.UseVisualStyleBackColor = true;
			this.btnCcwSmall.Click += new System.EventHandler(this.btnRotate_Click);
			// 
			// btnCcwLarge
			// 
			this.btnCcwLarge.Image = ((System.Drawing.Image)(resources.GetObject("btnCcwLarge.Image")));
			this.btnCcwLarge.Location = new System.Drawing.Point(9, 38);
			this.btnCcwLarge.Name = "btnCcwLarge";
			this.btnCcwLarge.Size = new System.Drawing.Size(24, 24);
			this.btnCcwLarge.TabIndex = 10;
			this.btnCcwLarge.UseVisualStyleBackColor = true;
			this.btnCcwLarge.Click += new System.EventHandler(this.btnRotate_Click);
			// 
			// lblRotationUnit
			// 
			this.lblRotationUnit.AutoSize = true;
			this.lblRotationUnit.Location = new System.Drawing.Point(197, 21);
			this.lblRotationUnit.Name = "lblRotationUnit";
			this.lblRotationUnit.Size = new System.Drawing.Size(11, 13);
			this.lblRotationUnit.TabIndex = 9;
			this.lblRotationUnit.Text = "°";
			// 
			// lblRotation
			// 
			this.lblRotation.AutoSize = true;
			this.lblRotation.Location = new System.Drawing.Point(6, 21);
			this.lblRotation.Name = "lblRotation";
			this.lblRotation.Size = new System.Drawing.Size(66, 13);
			this.lblRotation.TabIndex = 7;
			this.lblRotation.Text = "Ausrichtung:";
			// 
			// lblRandfriesUnit
			// 
			this.lblRandfriesUnit.AutoSize = true;
			this.lblRandfriesUnit.Location = new System.Drawing.Point(197, 67);
			this.lblRandfriesUnit.Name = "lblRandfriesUnit";
			this.lblRandfriesUnit.Size = new System.Drawing.Size(21, 13);
			this.lblRandfriesUnit.TabIndex = 6;
			this.lblRandfriesUnit.Text = "cm";
			// 
			// lblRandfries
			// 
			this.lblRandfries.AutoSize = true;
			this.lblRandfries.Location = new System.Drawing.Point(6, 67);
			this.lblRandfries.Name = "lblRandfries";
			this.lblRandfries.Size = new System.Drawing.Size(55, 13);
			this.lblRandfries.TabIndex = 4;
			this.lblRandfries.Text = "Randfries:";
			// 
			// pageLayout
			// 
			this.pageLayout.Controls.Add(this.groupBox1);
			this.pageLayout.Controls.Add(this.label13);
			this.pageLayout.Controls.Add(this.lstSubarea);
			this.pageLayout.Controls.Add(this.label1);
			this.pageLayout.Controls.Add(this.lstRows);
			this.pageLayout.Controls.Add(this.label12);
			this.pageLayout.Controls.Add(this.lstCircuits);
			this.pageLayout.Location = new System.Drawing.Point(4, 22);
			this.pageLayout.Name = "pageLayout";
			this.pageLayout.Padding = new System.Windows.Forms.Padding(3);
			this.pageLayout.Size = new System.Drawing.Size(722, 105);
			this.pageLayout.TabIndex = 1;
			this.pageLayout.Text = "Module auslegen";
			this.pageLayout.UseVisualStyleBackColor = true;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(336, 3);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(103, 26);
			this.label13.TabIndex = 149;
			this.label13.Text = "Teilflächen";
			this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lstSubarea
			// 
			this.lstSubarea.FormattingEnabled = true;
			this.lstSubarea.Location = new System.Drawing.Point(337, 32);
			this.lstSubarea.Name = "lstSubarea";
			this.lstSubarea.Size = new System.Drawing.Size(129, 69);
			this.lstSubarea.TabIndex = 148;
			this.lstSubarea.SelectedIndexChanged += new System.EventHandler(this.lstCircuits_SelectedIndexChanged);
			this.lstSubarea.Click += new System.EventHandler(this.lstCircuits_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(471, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 26);
			this.label1.TabIndex = 147;
			this.label1.Text = "Parallele Reihen im Heizkreis";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lstRows
			// 
			this.lstRows.FormattingEnabled = true;
			this.lstRows.Location = new System.Drawing.Point(472, 32);
			this.lstRows.Name = "lstRows";
			this.lstRows.Size = new System.Drawing.Size(102, 69);
			this.lstRows.TabIndex = 146;
			this.lstRows.SelectedIndexChanged += new System.EventHandler(this.lstCircuits_SelectedIndexChanged);
			this.lstRows.Click += new System.EventHandler(this.lstCircuits_SelectedIndexChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(243, 3);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(88, 26);
			this.label12.TabIndex = 145;
			this.label12.Text = "Heizkreise";
			this.label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// lstCircuits
			// 
			this.lstCircuits.FormattingEnabled = true;
			this.lstCircuits.Location = new System.Drawing.Point(246, 32);
			this.lstCircuits.Name = "lstCircuits";
			this.lstCircuits.Size = new System.Drawing.Size(85, 69);
			this.lstCircuits.TabIndex = 144;
			this.lstCircuits.SelectedIndexChanged += new System.EventHandler(this.lstCircuits_SelectedIndexChanged);
			this.lstCircuits.Click += new System.EventHandler(this.lstCircuits_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cmbOrientation);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.cmbModulType);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(8, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(214, 90);
			this.groupBox1.TabIndex = 150;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Modul";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(50, 13);
			this.label2.TabIndex = 151;
			this.label2.Text = "Typ:";
			// 
			// cmbModulType
			// 
			this.cmbModulType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbModulType.FormattingEnabled = true;
			this.cmbModulType.Location = new System.Drawing.Point(62, 19);
			this.cmbModulType.Name = "cmbModulType";
			this.cmbModulType.Size = new System.Drawing.Size(146, 21);
			this.cmbModulType.TabIndex = 151;
			this.cmbModulType.SelectedIndexChanged += new System.EventHandler(this.cmbModulType_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 49);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 13);
			this.label3.TabIndex = 152;
			this.label3.Text = "Ausrichtung:";
			// 
			// cmbOrientation
			// 
			this.cmbOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbOrientation.FormattingEnabled = true;
			this.cmbOrientation.Location = new System.Drawing.Point(98, 46);
			this.cmbOrientation.Name = "cmbOrientation";
			this.cmbOrientation.Size = new System.Drawing.Size(110, 21);
			this.cmbOrientation.TabIndex = 153;
			// 
			// planPanel
			// 
			this.planPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planPanel.Location = new System.Drawing.Point(0, 25);
			this.planPanel.Name = "planPanel";
			this.planPanel.PlanCursor = System.Windows.Forms.Cursors.SizeAll;
			this.planPanel.ProductPlanner = this.modulKlimaBodenPlanner;
			this.planPanel.Size = new System.Drawing.Size(730, 309);
			this.planPanel.TabIndex = 0;
			// 
			// modulKlimaBodenPlanner
			// 
			this.modulKlimaBodenPlanner.AlignRectangle = false;
			this.modulKlimaBodenPlanner.Mode = Europlan.Common.ModulKlimaDeckePlanner.KlimaDeckeMode.KDM_NONE;
			this.modulKlimaBodenPlanner.ModuleTypeToAdd = ((Europlan.Common.KlimaFlaechenModul.ModulTypeEnum)(Europlan.Common.KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30));
			this.modulKlimaBodenPlanner.OptimalLayout = true;
			this.modulKlimaBodenPlanner.StartingOrientation = ((Europlan.Common.KlimaFlaechenModul.ModulOrientationEnum)(Europlan.Common.KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT));
			this.modulKlimaBodenPlanner.ListsNeedUpdate += new System.EventHandler(this.modulKlimaBodenPlanner_ListsNeedUpdate);
			// 
			// numRotation
			// 
			this.numRotation.EditType = Europlan.Common.NumericBox.NumericEditType.ROTATION;
			this.numRotation.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numRotation.Location = new System.Drawing.Point(117, 18);
			this.numRotation.MaxValue = new decimal(new int[] {
            1799,
            0,
            0,
            65536});
			this.numRotation.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numRotation.Name = "numRotation";
			this.numRotation.Size = new System.Drawing.Size(74, 20);
			this.numRotation.TabIndex = 8;
			this.numRotation.Text = "0";
			this.numRotation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numRotation.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numRotation.ValueChanged += new System.EventHandler(this.numAusrichtung_ValueChanged);
			// 
			// numRandfries
			// 
			this.numRandfries.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numRandfries.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numRandfries.Location = new System.Drawing.Point(117, 64);
			this.numRandfries.MaxValue = null;
			this.numRandfries.MinValue = null;
			this.numRandfries.Name = "numRandfries";
			this.numRandfries.Size = new System.Drawing.Size(74, 20);
			this.numRandfries.TabIndex = 5;
			this.numRandfries.Text = "0";
			this.numRandfries.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numRandfries.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// ModulKlimaDeckePlannerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(730, 465);
			this.Controls.Add(this.planPanel);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip);
			this.Name = "ModulKlimaDeckePlannerForm";
			this.Text = "ModulKlimaBodenPlannerForm";
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.tabs.ResumeLayout(false);
			this.pageConstruction.ResumeLayout(false);
			this.grpCeilingContruction.ResumeLayout(false);
			this.grpCeilingContruction.PerformLayout();
			this.grpConstructionParameter.ResumeLayout(false);
			this.grpConstructionParameter.PerformLayout();
			this.pageLayout.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ModulKlimaDeckePlanner modulKlimaBodenPlanner;
		private PlanPanel planPanel;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox grpCeilingContruction;
		private System.Windows.Forms.RadioButton rbKassetten;
		private System.Windows.Forms.RadioButton rbAkustik;
		private System.Windows.Forms.RadioButton rbGlatt;
		private System.Windows.Forms.GroupBox grpConstructionParameter;
		private System.Windows.Forms.ToolStripButton btnConstruction;
		private System.Windows.Forms.Label lblRandfries;
		private System.Windows.Forms.Label lblRandfriesUnit;
		private NumericBox numRandfries;
		private System.Windows.Forms.Label lblRotationUnit;
		private NumericBox numRotation;
		private System.Windows.Forms.Label lblRotation;
		private System.Windows.Forms.Button btnCwLarge;
		private System.Windows.Forms.Button btnCwSmall;
		private System.Windows.Forms.Button btnCcwSmall;
		private System.Windows.Forms.Button btnCcwLarge;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage pageConstruction;
		private System.Windows.Forms.TabPage pageLayout;
		private System.Windows.Forms.Button btnVertical;
		private System.Windows.Forms.Button btnHorizontal;
		private System.Windows.Forms.ToolStripButton btnAddModules;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.ListBox lstSubarea;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstRows;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ListBox lstCircuits;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cmbOrientation;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbModulType;
		private System.Windows.Forms.Label label2;
	}
}