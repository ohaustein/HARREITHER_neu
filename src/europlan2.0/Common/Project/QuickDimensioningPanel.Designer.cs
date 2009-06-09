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
			this.tabQuickDimensioning = new System.Windows.Forms.TabControl();
			this.pageSettings = new System.Windows.Forms.TabPage();
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
			this.lblHitherm = new System.Windows.Forms.Label();
			this.lblHithermCompact = new System.Windows.Forms.Label();
			this.lblModulKlimaBoden = new System.Windows.Forms.Label();
			this.cbHithermHeat = new System.Windows.Forms.CheckBox();
			this.cbHithermCool = new System.Windows.Forms.CheckBox();
			this.cbHithermCompactHeat = new System.Windows.Forms.CheckBox();
			this.cbModulKlimaBodenCool = new System.Windows.Forms.CheckBox();
			this.cbHithermCompactCool = new System.Windows.Forms.CheckBox();
			this.cbModulKlimaBodenHeat = new System.Windows.Forms.CheckBox();
			this.cbModulKlimaDeckeHeat = new System.Windows.Forms.CheckBox();
			this.cbModulKlimaDeckeCool = new System.Windows.Forms.CheckBox();
			this.lblModulKlimaDecke = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblBka = new System.Windows.Forms.Label();
			this.cbBkaHeat = new System.Windows.Forms.CheckBox();
			this.cbBkaCool = new System.Windows.Forms.CheckBox();
			this.txtAllocation = new Europlan.Common.NumericBox();
			this.txtTemperature = new Europlan.Common.NumericBox();
			this.tabQuickDimensioning.SuspendLayout();
			this.pageSettings.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabQuickDimensioning
			// 
			this.tabQuickDimensioning.Controls.Add(this.pageSettings);
			this.tabQuickDimensioning.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabQuickDimensioning.Location = new System.Drawing.Point(0, 0);
			this.tabQuickDimensioning.Name = "tabQuickDimensioning";
			this.tabQuickDimensioning.SelectedIndex = 0;
			this.tabQuickDimensioning.Size = new System.Drawing.Size(894, 431);
			this.tabQuickDimensioning.TabIndex = 0;
			// 
			// pageSettings
			// 
			this.pageSettings.Controls.Add(this.txtAllocation);
			this.pageSettings.Controls.Add(this.txtTemperature);
			this.pageSettings.Controls.Add(this.lblAllocation2);
			this.pageSettings.Controls.Add(this.lblAllocation);
			this.pageSettings.Controls.Add(this.lblDistance);
			this.pageSettings.Controls.Add(this.cmbDistance);
			this.pageSettings.Controls.Add(this.lblTemp2);
			this.pageSettings.Controls.Add(this.label2);
			this.pageSettings.Controls.Add(this.lblTemp1);
			this.pageSettings.Controls.Add(this.label1);
			this.pageSettings.Controls.Add(this.tableLayoutPanel1);
			this.pageSettings.Controls.Add(this.label4);
			this.pageSettings.Location = new System.Drawing.Point(4, 22);
			this.pageSettings.Name = "pageSettings";
			this.pageSettings.Padding = new System.Windows.Forms.Padding(3);
			this.pageSettings.Size = new System.Drawing.Size(886, 405);
			this.pageSettings.TabIndex = 0;
			this.pageSettings.Text = "Einstellungen";
			this.pageSettings.UseVisualStyleBackColor = true;
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
			this.lblTemp1.Size = new System.Drawing.Size(93, 43);
			this.lblTemp1.TabIndex = 23;
			this.lblTemp1.Text = "Vorlauftemperatur\r\n(Heizen)";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(7, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(187, 24);
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
			this.cbEurovalCool.Visible = false;
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
			this.cbHithermCool.Visible = false;
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
			this.cbModulKlimaBodenCool.Visible = false;
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
			this.cbHithermCompactCool.Visible = false;
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
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(315, 74);
			this.label4.Margin = new System.Windows.Forms.Padding(3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(93, 17);
			this.label4.TabIndex = 18;
			this.label4.Text = "Annahmen";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			// txtTemperature
			// 
			this.txtTemperature.EditType = Europlan.Common.NumericBox.NumericEditType.FLOW_TEMPERATURE;
			this.txtTemperature.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtTemperature.Location = new System.Drawing.Point(412, 96);
			this.txtTemperature.Name = "txtTemperature";
			this.txtTemperature.Size = new System.Drawing.Size(56, 20);
			this.txtTemperature.TabIndex = 24;
			this.txtTemperature.Text = "0";
			this.txtTemperature.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtTemperature.ValueChanged += new System.EventHandler(this.txtTemperature_ValueChanged);
			// 
			// QuickDimensioningPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabQuickDimensioning);
			this.Name = "QuickDimensioningPanel";
			this.Size = new System.Drawing.Size(894, 431);
			this.tabQuickDimensioning.ResumeLayout(false);
			this.pageSettings.ResumeLayout(false);
			this.pageSettings.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
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
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblTemp1;
		private System.Windows.Forms.Label lblAllocation;
		private System.Windows.Forms.Label lblAllocation2;
		private NumericBox txtAllocation;
		private NumericBox txtTemperature;
		private System.Windows.Forms.Label lblBka;
		private System.Windows.Forms.CheckBox cbBkaHeat;
		private System.Windows.Forms.CheckBox cbBkaCool;
	}
}
