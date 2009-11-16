namespace Europlan.Common {
	partial class SystemParametersPanel {
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
			this.label1 = new System.Windows.Forms.Label();
			this.tabSystemParameters = new System.Windows.Forms.TabControl();
			this.tabEuroval = new System.Windows.Forms.TabPage();
			this.btnEurovalStandard = new System.Windows.Forms.Button();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.numSpreizungKühlMax = new Europlan.Common.NumericBox();
			this.label16 = new System.Windows.Forms.Label();
			this.numSpreizungKühlMin = new Europlan.Common.NumericBox();
			this.label17 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.numSpreizungHeizMax = new Europlan.Common.NumericBox();
			this.label11 = new System.Windows.Forms.Label();
			this.numSpreizungHeizMin = new Europlan.Common.NumericBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.numEurovalDurchfluss = new Europlan.Common.NumericBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.numEurovalPressureMbar = new Europlan.Common.NumericBox();
			this.label5 = new System.Windows.Forms.Label();
			this.numEurovalPressurePa = new Europlan.Common.NumericBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.numCircuitLength = new Europlan.Common.NumericBox();
			this.label3 = new System.Windows.Forms.Label();
			this.rbEurovalEN1264 = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.rbEurovalHarreitherNorm = new System.Windows.Forms.RadioButton();
			this.tabHitherm = new System.Windows.Forms.TabPage();
			this.tabModulBoden = new System.Windows.Forms.TabPage();
			this.numModulBodenMaxModulesInCircuit = new Europlan.Common.NumericBox();
			this.label26 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.numModulBodenDurchfluss = new Europlan.Common.NumericBox();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.numModulBodenPressureMbar = new Europlan.Common.NumericBox();
			this.label22 = new System.Windows.Forms.Label();
			this.numModulBodenPressurePa = new Europlan.Common.NumericBox();
			this.label23 = new System.Windows.Forms.Label();
			this.btnModulBodenStandard = new System.Windows.Forms.Button();
			this.rbModulBodenEN1264 = new System.Windows.Forms.RadioButton();
			this.label18 = new System.Windows.Forms.Label();
			this.rbModulBodenHarreitherNorm = new System.Windows.Forms.RadioButton();
			this.tabModulDecke = new System.Windows.Forms.TabPage();
			this.numericBox1 = new Europlan.Common.NumericBox();
			this.label27 = new System.Windows.Forms.Label();
			this.numericBox2 = new Europlan.Common.NumericBox();
			this.label28 = new System.Windows.Forms.Label();
			this.numericBox3 = new Europlan.Common.NumericBox();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.numericBox4 = new Europlan.Common.NumericBox();
			this.label31 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.numericBox5 = new Europlan.Common.NumericBox();
			this.label33 = new System.Windows.Forms.Label();
			this.numericBox6 = new Europlan.Common.NumericBox();
			this.label34 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.label35 = new System.Windows.Forms.Label();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.tabSystemParameters.SuspendLayout();
			this.tabEuroval.SuspendLayout();
			this.tabModulBoden.SuspendLayout();
			this.tabModulDecke.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(171, 24);
			this.label1.TabIndex = 3;
			this.label1.Text = "Systemparameter";
			// 
			// tabSystemParameters
			// 
			this.tabSystemParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabSystemParameters.Controls.Add(this.tabEuroval);
			this.tabSystemParameters.Controls.Add(this.tabHitherm);
			this.tabSystemParameters.Controls.Add(this.tabModulBoden);
			this.tabSystemParameters.Controls.Add(this.tabModulDecke);
			this.tabSystemParameters.Location = new System.Drawing.Point(0, 27);
			this.tabSystemParameters.Name = "tabSystemParameters";
			this.tabSystemParameters.SelectedIndex = 0;
			this.tabSystemParameters.Size = new System.Drawing.Size(719, 392);
			this.tabSystemParameters.TabIndex = 4;
			this.tabSystemParameters.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabSystemParameters_Selected);
			// 
			// tabEuroval
			// 
			this.tabEuroval.Controls.Add(this.btnEurovalStandard);
			this.tabEuroval.Controls.Add(this.label14);
			this.tabEuroval.Controls.Add(this.label15);
			this.tabEuroval.Controls.Add(this.numSpreizungKühlMax);
			this.tabEuroval.Controls.Add(this.label16);
			this.tabEuroval.Controls.Add(this.numSpreizungKühlMin);
			this.tabEuroval.Controls.Add(this.label17);
			this.tabEuroval.Controls.Add(this.label13);
			this.tabEuroval.Controls.Add(this.label10);
			this.tabEuroval.Controls.Add(this.numSpreizungHeizMax);
			this.tabEuroval.Controls.Add(this.label11);
			this.tabEuroval.Controls.Add(this.numSpreizungHeizMin);
			this.tabEuroval.Controls.Add(this.label12);
			this.tabEuroval.Controls.Add(this.label8);
			this.tabEuroval.Controls.Add(this.numEurovalDurchfluss);
			this.tabEuroval.Controls.Add(this.label9);
			this.tabEuroval.Controls.Add(this.label7);
			this.tabEuroval.Controls.Add(this.numEurovalPressureMbar);
			this.tabEuroval.Controls.Add(this.label5);
			this.tabEuroval.Controls.Add(this.numEurovalPressurePa);
			this.tabEuroval.Controls.Add(this.label6);
			this.tabEuroval.Controls.Add(this.label4);
			this.tabEuroval.Controls.Add(this.numCircuitLength);
			this.tabEuroval.Controls.Add(this.label3);
			this.tabEuroval.Controls.Add(this.rbEurovalEN1264);
			this.tabEuroval.Controls.Add(this.label2);
			this.tabEuroval.Controls.Add(this.rbEurovalHarreitherNorm);
			this.tabEuroval.Location = new System.Drawing.Point(4, 22);
			this.tabEuroval.Name = "tabEuroval";
			this.tabEuroval.Padding = new System.Windows.Forms.Padding(3);
			this.tabEuroval.Size = new System.Drawing.Size(711, 366);
			this.tabEuroval.TabIndex = 0;
			this.tabEuroval.Text = "Euroval®";
			this.tabEuroval.UseVisualStyleBackColor = true;
			// 
			// btnEurovalStandard
			// 
			this.btnEurovalStandard.Location = new System.Drawing.Point(324, 181);
			this.btnEurovalStandard.Name = "btnEurovalStandard";
			this.btnEurovalStandard.Size = new System.Drawing.Size(110, 23);
			this.btnEurovalStandard.TabIndex = 26;
			this.btnEurovalStandard.Text = "Standardwerte";
			this.btnEurovalStandard.UseVisualStyleBackColor = true;
			this.btnEurovalStandard.Click += new System.EventHandler(this.btnEurovalStandard_Click);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(162, 158);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(26, 23);
			this.label14.TabIndex = 25;
			this.label14.Text = "min.";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(406, 158);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(76, 23);
			this.label15.TabIndex = 24;
			this.label15.Text = "K";
			// 
			// numSpreizungKühlMax
			// 
			this.numSpreizungKühlMax.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numSpreizungKühlMax.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungKühlMax.Location = new System.Drawing.Point(324, 155);
			this.numSpreizungKühlMax.MaxValue = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numSpreizungKühlMax.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungKühlMax.Name = "numSpreizungKühlMax";
			this.numSpreizungKühlMax.Size = new System.Drawing.Size(76, 20);
			this.numSpreizungKühlMax.TabIndex = 23;
			this.numSpreizungKühlMax.Text = "1";
			this.numSpreizungKühlMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numSpreizungKühlMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungKühlMax.ValueChanged += new System.EventHandler(this.numSpreizungKühlMax_ValueChanged);
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(276, 158);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(48, 23);
			this.label16.TabIndex = 22;
			this.label16.Text = "K    max.";
			// 
			// numSpreizungKühlMin
			// 
			this.numSpreizungKühlMin.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numSpreizungKühlMin.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungKühlMin.Location = new System.Drawing.Point(194, 155);
			this.numSpreizungKühlMin.MaxValue = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numSpreizungKühlMin.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungKühlMin.Name = "numSpreizungKühlMin";
			this.numSpreizungKühlMin.Size = new System.Drawing.Size(76, 20);
			this.numSpreizungKühlMin.TabIndex = 21;
			this.numSpreizungKühlMin.Text = "1";
			this.numSpreizungKühlMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numSpreizungKühlMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungKühlMin.ValueChanged += new System.EventHandler(this.numSpreizungKühlMin_ValueChanged);
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(3, 158);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(116, 23);
			this.label17.TabIndex = 20;
			this.label17.Text = "Spreizung Kühlbetrieb:";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(162, 135);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(26, 23);
			this.label13.TabIndex = 19;
			this.label13.Text = "min.";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(406, 135);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(76, 23);
			this.label10.TabIndex = 18;
			this.label10.Text = "K";
			// 
			// numSpreizungHeizMax
			// 
			this.numSpreizungHeizMax.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numSpreizungHeizMax.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungHeizMax.Location = new System.Drawing.Point(324, 132);
			this.numSpreizungHeizMax.MaxValue = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numSpreizungHeizMax.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungHeizMax.Name = "numSpreizungHeizMax";
			this.numSpreizungHeizMax.Size = new System.Drawing.Size(76, 20);
			this.numSpreizungHeizMax.TabIndex = 17;
			this.numSpreizungHeizMax.Text = "1";
			this.numSpreizungHeizMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numSpreizungHeizMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungHeizMax.ValueChanged += new System.EventHandler(this.numSpreizungHeizMax_ValueChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(276, 135);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(48, 23);
			this.label11.TabIndex = 16;
			this.label11.Text = "K    max.";
			// 
			// numSpreizungHeizMin
			// 
			this.numSpreizungHeizMin.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numSpreizungHeizMin.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungHeizMin.Location = new System.Drawing.Point(194, 132);
			this.numSpreizungHeizMin.MaxValue = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numSpreizungHeizMin.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungHeizMin.Name = "numSpreizungHeizMin";
			this.numSpreizungHeizMin.Size = new System.Drawing.Size(76, 20);
			this.numSpreizungHeizMin.TabIndex = 15;
			this.numSpreizungHeizMin.Text = "1";
			this.numSpreizungHeizMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numSpreizungHeizMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSpreizungHeizMin.ValueChanged += new System.EventHandler(this.numSpreizungHeizMin_ValueChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(3, 135);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(116, 23);
			this.label12.TabIndex = 14;
			this.label12.Text = "Spreizung Heizbetrieb:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(276, 112);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(76, 23);
			this.label8.TabIndex = 13;
			this.label8.Text = "l/h";
			// 
			// numEurovalDurchfluss
			// 
			this.numEurovalDurchfluss.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numEurovalDurchfluss.InternalValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numEurovalDurchfluss.Location = new System.Drawing.Point(194, 109);
			this.numEurovalDurchfluss.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numEurovalDurchfluss.MinValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numEurovalDurchfluss.Name = "numEurovalDurchfluss";
			this.numEurovalDurchfluss.Size = new System.Drawing.Size(76, 20);
			this.numEurovalDurchfluss.TabIndex = 12;
			this.numEurovalDurchfluss.Text = "100";
			this.numEurovalDurchfluss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numEurovalDurchfluss.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numEurovalDurchfluss.ValueChanged += new System.EventHandler(this.numEurovalDurchfluss_ValueChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(3, 112);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(181, 23);
			this.label9.TabIndex = 11;
			this.label9.Text = "Max. Durchflußmenge:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(406, 89);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(76, 23);
			this.label7.TabIndex = 10;
			this.label7.Text = "mbar";
			// 
			// numEurovalPressureMbar
			// 
			this.numEurovalPressureMbar.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numEurovalPressureMbar.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numEurovalPressureMbar.Location = new System.Drawing.Point(324, 86);
			this.numEurovalPressureMbar.MaxValue = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numEurovalPressureMbar.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numEurovalPressureMbar.Name = "numEurovalPressureMbar";
			this.numEurovalPressureMbar.Size = new System.Drawing.Size(76, 20);
			this.numEurovalPressureMbar.TabIndex = 9;
			this.numEurovalPressureMbar.Text = "0,1";
			this.numEurovalPressureMbar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numEurovalPressureMbar.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numEurovalPressureMbar.ValueChanged += new System.EventHandler(this.numEurovalPressureMbar_ValueChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(276, 89);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 23);
			this.label5.TabIndex = 8;
			this.label5.Text = "Pa    =";
			// 
			// numEurovalPressurePa
			// 
			this.numEurovalPressurePa.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numEurovalPressurePa.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numEurovalPressurePa.Location = new System.Drawing.Point(194, 86);
			this.numEurovalPressurePa.MaxValue = new decimal(new int[] {
            50000,
            0,
            0,
            0});
			this.numEurovalPressurePa.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numEurovalPressurePa.Name = "numEurovalPressurePa";
			this.numEurovalPressurePa.Size = new System.Drawing.Size(76, 20);
			this.numEurovalPressurePa.TabIndex = 7;
			this.numEurovalPressurePa.Text = "0";
			this.numEurovalPressurePa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numEurovalPressurePa.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numEurovalPressurePa.ValueChanged += new System.EventHandler(this.numEurovalPressurePa_ValueChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 89);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(181, 23);
			this.label6.TabIndex = 6;
			this.label6.Text = "Max. Druckverlust:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(276, 63);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(76, 23);
			this.label4.TabIndex = 5;
			this.label4.Text = "m";
			// 
			// numCircuitLength
			// 
			this.numCircuitLength.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numCircuitLength.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numCircuitLength.Location = new System.Drawing.Point(194, 60);
			this.numCircuitLength.MaxValue = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numCircuitLength.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numCircuitLength.Name = "numCircuitLength";
			this.numCircuitLength.Size = new System.Drawing.Size(76, 20);
			this.numCircuitLength.TabIndex = 4;
			this.numCircuitLength.Text = "0,1";
			this.numCircuitLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numCircuitLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numCircuitLength.ValueChanged += new System.EventHandler(this.numCircuitLength_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 63);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(181, 23);
			this.label3.TabIndex = 3;
			this.label3.Text = "Max. Rohrlänge je Heizkreis:";
			// 
			// rbEurovalEN1264
			// 
			this.rbEurovalEN1264.Location = new System.Drawing.Point(194, 33);
			this.rbEurovalEN1264.Name = "rbEurovalEN1264";
			this.rbEurovalEN1264.Size = new System.Drawing.Size(195, 17);
			this.rbEurovalEN1264.TabIndex = 2;
			this.rbEurovalEN1264.Text = "29 °C (EN 1264)";
			this.rbEurovalEN1264.UseVisualStyleBackColor = true;
			this.rbEurovalEN1264.CheckedChanged += new System.EventHandler(this.rbEurovalEN1264_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(181, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Maximale Oberflächentemperatur:";
			// 
			// rbEurovalHarreitherNorm
			// 
			this.rbEurovalHarreitherNorm.Checked = true;
			this.rbEurovalHarreitherNorm.Location = new System.Drawing.Point(194, 3);
			this.rbEurovalHarreitherNorm.Name = "rbEurovalHarreitherNorm";
			this.rbEurovalHarreitherNorm.Size = new System.Drawing.Size(206, 24);
			this.rbEurovalHarreitherNorm.TabIndex = 0;
			this.rbEurovalHarreitherNorm.TabStop = true;
			this.rbEurovalHarreitherNorm.Text = "27 °C (Harreither Werksempfehlung)";
			this.rbEurovalHarreitherNorm.UseVisualStyleBackColor = true;
			this.rbEurovalHarreitherNorm.CheckedChanged += new System.EventHandler(this.rbEurovalHarreitherNorm_CheckedChanged);
			// 
			// tabHitherm
			// 
			this.tabHitherm.Location = new System.Drawing.Point(4, 22);
			this.tabHitherm.Name = "tabHitherm";
			this.tabHitherm.Size = new System.Drawing.Size(711, 366);
			this.tabHitherm.TabIndex = 1;
			this.tabHitherm.Text = "Hitherm®";
			this.tabHitherm.UseVisualStyleBackColor = true;
			// 
			// tabModulBoden
			// 
			this.tabModulBoden.Controls.Add(this.numModulBodenMaxModulesInCircuit);
			this.tabModulBoden.Controls.Add(this.label26);
			this.tabModulBoden.Controls.Add(this.label19);
			this.tabModulBoden.Controls.Add(this.numModulBodenDurchfluss);
			this.tabModulBoden.Controls.Add(this.label20);
			this.tabModulBoden.Controls.Add(this.label21);
			this.tabModulBoden.Controls.Add(this.numModulBodenPressureMbar);
			this.tabModulBoden.Controls.Add(this.label22);
			this.tabModulBoden.Controls.Add(this.numModulBodenPressurePa);
			this.tabModulBoden.Controls.Add(this.label23);
			this.tabModulBoden.Controls.Add(this.btnModulBodenStandard);
			this.tabModulBoden.Controls.Add(this.rbModulBodenEN1264);
			this.tabModulBoden.Controls.Add(this.label18);
			this.tabModulBoden.Controls.Add(this.rbModulBodenHarreitherNorm);
			this.tabModulBoden.Location = new System.Drawing.Point(4, 22);
			this.tabModulBoden.Name = "tabModulBoden";
			this.tabModulBoden.Size = new System.Drawing.Size(711, 366);
			this.tabModulBoden.TabIndex = 2;
			this.tabModulBoden.Text = "Modul Klima-Boden";
			this.tabModulBoden.UseVisualStyleBackColor = true;
			// 
			// numModulBodenMaxModulesInCircuit
			// 
			this.numModulBodenMaxModulesInCircuit.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numModulBodenMaxModulesInCircuit.InternalValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numModulBodenMaxModulesInCircuit.Location = new System.Drawing.Point(194, 56);
			this.numModulBodenMaxModulesInCircuit.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numModulBodenMaxModulesInCircuit.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numModulBodenMaxModulesInCircuit.Name = "numModulBodenMaxModulesInCircuit";
			this.numModulBodenMaxModulesInCircuit.Size = new System.Drawing.Size(76, 20);
			this.numModulBodenMaxModulesInCircuit.TabIndex = 41;
			this.numModulBodenMaxModulesInCircuit.Text = "50";
			this.numModulBodenMaxModulesInCircuit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numModulBodenMaxModulesInCircuit.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numModulBodenMaxModulesInCircuit.ValueChanged += new System.EventHandler(this.numModulBodenMaxModulesInCircuit_ValueChanged);
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(3, 59);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(181, 23);
			this.label26.TabIndex = 40;
			this.label26.Text = "Max Modulanzahl pro Heizkreis:";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(276, 108);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(76, 23);
			this.label19.TabIndex = 35;
			this.label19.Text = "l/h";
			// 
			// numModulBodenDurchfluss
			// 
			this.numModulBodenDurchfluss.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numModulBodenDurchfluss.InternalValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numModulBodenDurchfluss.Location = new System.Drawing.Point(194, 105);
			this.numModulBodenDurchfluss.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numModulBodenDurchfluss.MinValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numModulBodenDurchfluss.Name = "numModulBodenDurchfluss";
			this.numModulBodenDurchfluss.Size = new System.Drawing.Size(76, 20);
			this.numModulBodenDurchfluss.TabIndex = 34;
			this.numModulBodenDurchfluss.Text = "100";
			this.numModulBodenDurchfluss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numModulBodenDurchfluss.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numModulBodenDurchfluss.ValueChanged += new System.EventHandler(this.numModulBodenDurchfluss_ValueChanged);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(3, 108);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(181, 23);
			this.label20.TabIndex = 33;
			this.label20.Text = "Max. Durchflußmenge:";
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(406, 85);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(76, 23);
			this.label21.TabIndex = 32;
			this.label21.Text = "mbar";
			// 
			// numModulBodenPressureMbar
			// 
			this.numModulBodenPressureMbar.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numModulBodenPressureMbar.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numModulBodenPressureMbar.Location = new System.Drawing.Point(324, 82);
			this.numModulBodenPressureMbar.MaxValue = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numModulBodenPressureMbar.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numModulBodenPressureMbar.Name = "numModulBodenPressureMbar";
			this.numModulBodenPressureMbar.Size = new System.Drawing.Size(76, 20);
			this.numModulBodenPressureMbar.TabIndex = 31;
			this.numModulBodenPressureMbar.Text = "0,1";
			this.numModulBodenPressureMbar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numModulBodenPressureMbar.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numModulBodenPressureMbar.ValueChanged += new System.EventHandler(this.numModulBodenPressureMbar_ValueChanged);
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(276, 85);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(42, 23);
			this.label22.TabIndex = 30;
			this.label22.Text = "Pa    =";
			// 
			// numModulBodenPressurePa
			// 
			this.numModulBodenPressurePa.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numModulBodenPressurePa.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numModulBodenPressurePa.Location = new System.Drawing.Point(194, 82);
			this.numModulBodenPressurePa.MaxValue = new decimal(new int[] {
            50000,
            0,
            0,
            0});
			this.numModulBodenPressurePa.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numModulBodenPressurePa.Name = "numModulBodenPressurePa";
			this.numModulBodenPressurePa.Size = new System.Drawing.Size(76, 20);
			this.numModulBodenPressurePa.TabIndex = 29;
			this.numModulBodenPressurePa.Text = "0";
			this.numModulBodenPressurePa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numModulBodenPressurePa.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numModulBodenPressurePa.ValueChanged += new System.EventHandler(this.numModulBodenPressurePa_ValueChanged);
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(3, 85);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(181, 23);
			this.label23.TabIndex = 28;
			this.label23.Text = "Max. Druckverlust:";
			// 
			// btnModulBodenStandard
			// 
			this.btnModulBodenStandard.Location = new System.Drawing.Point(324, 134);
			this.btnModulBodenStandard.Name = "btnModulBodenStandard";
			this.btnModulBodenStandard.Size = new System.Drawing.Size(110, 23);
			this.btnModulBodenStandard.TabIndex = 27;
			this.btnModulBodenStandard.Text = "Standardwerte";
			this.btnModulBodenStandard.UseVisualStyleBackColor = true;
			this.btnModulBodenStandard.Click += new System.EventHandler(this.btnModulBodenStandard_Click);
			// 
			// rbModulBodenEN1264
			// 
			this.rbModulBodenEN1264.Location = new System.Drawing.Point(194, 33);
			this.rbModulBodenEN1264.Name = "rbModulBodenEN1264";
			this.rbModulBodenEN1264.Size = new System.Drawing.Size(195, 17);
			this.rbModulBodenEN1264.TabIndex = 5;
			this.rbModulBodenEN1264.Text = "29 °C (EN 1264)";
			this.rbModulBodenEN1264.UseVisualStyleBackColor = true;
			this.rbModulBodenEN1264.CheckedChanged += new System.EventHandler(this.rbModulBodenEN1264_CheckedChanged);
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(3, 9);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(181, 23);
			this.label18.TabIndex = 4;
			this.label18.Text = "Maximale Oberflächentemperatur:";
			// 
			// rbModulBodenHarreitherNorm
			// 
			this.rbModulBodenHarreitherNorm.Checked = true;
			this.rbModulBodenHarreitherNorm.Location = new System.Drawing.Point(194, 3);
			this.rbModulBodenHarreitherNorm.Name = "rbModulBodenHarreitherNorm";
			this.rbModulBodenHarreitherNorm.Size = new System.Drawing.Size(206, 24);
			this.rbModulBodenHarreitherNorm.TabIndex = 3;
			this.rbModulBodenHarreitherNorm.TabStop = true;
			this.rbModulBodenHarreitherNorm.Text = "27 °C (Harreither Werksempfehlung)";
			this.rbModulBodenHarreitherNorm.UseVisualStyleBackColor = true;
			this.rbModulBodenHarreitherNorm.CheckedChanged += new System.EventHandler(this.rbModulBodenHarreitherNorm_CheckedChanged);
			// 
			// tabModulDecke
			// 
			this.tabModulDecke.Controls.Add(this.numericBox1);
			this.tabModulDecke.Controls.Add(this.label27);
			this.tabModulDecke.Controls.Add(this.numericBox2);
			this.tabModulDecke.Controls.Add(this.label28);
			this.tabModulDecke.Controls.Add(this.numericBox3);
			this.tabModulDecke.Controls.Add(this.label29);
			this.tabModulDecke.Controls.Add(this.label30);
			this.tabModulDecke.Controls.Add(this.numericBox4);
			this.tabModulDecke.Controls.Add(this.label31);
			this.tabModulDecke.Controls.Add(this.label32);
			this.tabModulDecke.Controls.Add(this.numericBox5);
			this.tabModulDecke.Controls.Add(this.label33);
			this.tabModulDecke.Controls.Add(this.numericBox6);
			this.tabModulDecke.Controls.Add(this.label34);
			this.tabModulDecke.Controls.Add(this.button1);
			this.tabModulDecke.Controls.Add(this.radioButton1);
			this.tabModulDecke.Controls.Add(this.label35);
			this.tabModulDecke.Controls.Add(this.radioButton2);
			this.tabModulDecke.Location = new System.Drawing.Point(4, 22);
			this.tabModulDecke.Name = "tabModulDecke";
			this.tabModulDecke.Size = new System.Drawing.Size(711, 366);
			this.tabModulDecke.TabIndex = 3;
			this.tabModulDecke.Text = "Modul Klima-Decke";
			this.tabModulDecke.UseVisualStyleBackColor = true;
			// 
			// numericBox1
			// 
			this.numericBox1.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numericBox1.InternalValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numericBox1.Location = new System.Drawing.Point(194, 108);
			this.numericBox1.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericBox1.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numericBox1.Name = "numericBox1";
			this.numericBox1.Size = new System.Drawing.Size(76, 20);
			this.numericBox1.TabIndex = 59;
			this.numericBox1.Text = "50";
			this.numericBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericBox1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(3, 111);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(181, 23);
			this.label27.TabIndex = 58;
			this.label27.Text = "Max Modulanzahl pro Heizkreis:";
			// 
			// numericBox2
			// 
			this.numericBox2.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numericBox2.InternalValue = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.numericBox2.Location = new System.Drawing.Point(194, 82);
			this.numericBox2.MaxValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numericBox2.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numericBox2.Name = "numericBox2";
			this.numericBox2.Size = new System.Drawing.Size(76, 20);
			this.numericBox2.TabIndex = 57;
			this.numericBox2.Text = "6";
			this.numericBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericBox2.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(3, 85);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(181, 23);
			this.label28.TabIndex = 56;
			this.label28.Text = "Max Modulreihen parallel:";
			// 
			// numericBox3
			// 
			this.numericBox3.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numericBox3.InternalValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.numericBox3.Location = new System.Drawing.Point(194, 56);
			this.numericBox3.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericBox3.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numericBox3.Name = "numericBox3";
			this.numericBox3.Size = new System.Drawing.Size(76, 20);
			this.numericBox3.TabIndex = 55;
			this.numericBox3.Text = "20";
			this.numericBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericBox3.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(3, 59);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(181, 23);
			this.label29.TabIndex = 54;
			this.label29.Text = "Max Modulanzahl in Serie:";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(276, 160);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(76, 23);
			this.label30.TabIndex = 53;
			this.label30.Text = "l/h";
			// 
			// numericBox4
			// 
			this.numericBox4.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numericBox4.InternalValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericBox4.Location = new System.Drawing.Point(194, 157);
			this.numericBox4.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericBox4.MinValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericBox4.Name = "numericBox4";
			this.numericBox4.Size = new System.Drawing.Size(76, 20);
			this.numericBox4.TabIndex = 52;
			this.numericBox4.Text = "100";
			this.numericBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericBox4.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(3, 160);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(181, 23);
			this.label31.TabIndex = 51;
			this.label31.Text = "Max. Durchflußmenge:";
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(406, 137);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(76, 23);
			this.label32.TabIndex = 50;
			this.label32.Text = "mbar";
			// 
			// numericBox5
			// 
			this.numericBox5.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numericBox5.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numericBox5.Location = new System.Drawing.Point(324, 134);
			this.numericBox5.MaxValue = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numericBox5.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numericBox5.Name = "numericBox5";
			this.numericBox5.Size = new System.Drawing.Size(76, 20);
			this.numericBox5.TabIndex = 49;
			this.numericBox5.Text = "0,1";
			this.numericBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericBox5.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(276, 137);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(42, 23);
			this.label33.TabIndex = 48;
			this.label33.Text = "Pa    =";
			// 
			// numericBox6
			// 
			this.numericBox6.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.numericBox6.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numericBox6.Location = new System.Drawing.Point(194, 134);
			this.numericBox6.MaxValue = new decimal(new int[] {
            50000,
            0,
            0,
            0});
			this.numericBox6.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numericBox6.Name = "numericBox6";
			this.numericBox6.Size = new System.Drawing.Size(76, 20);
			this.numericBox6.TabIndex = 47;
			this.numericBox6.Text = "0";
			this.numericBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericBox6.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(3, 137);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(181, 23);
			this.label34.TabIndex = 46;
			this.label34.Text = "Max. Druckverlust:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(324, 186);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(110, 23);
			this.button1.TabIndex = 45;
			this.button1.Text = "Standardwerte";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(194, 33);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(195, 17);
			this.radioButton1.TabIndex = 44;
			this.radioButton1.Text = "29 °C (EN 1264)";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(3, 9);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(181, 23);
			this.label35.TabIndex = 43;
			this.label35.Text = "Maximale Oberflächentemperatur:";
			// 
			// radioButton2
			// 
			this.radioButton2.Checked = true;
			this.radioButton2.Location = new System.Drawing.Point(194, 3);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(206, 24);
			this.radioButton2.TabIndex = 42;
			this.radioButton2.TabStop = true;
			this.radioButton2.Text = "27 °C (Harreither Werksempfehlung)";
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// SystemParametersPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabSystemParameters);
			this.Controls.Add(this.label1);
			this.Name = "SystemParametersPanel";
			this.Size = new System.Drawing.Size(719, 422);
			this.tabSystemParameters.ResumeLayout(false);
			this.tabEuroval.ResumeLayout(false);
			this.tabEuroval.PerformLayout();
			this.tabModulBoden.ResumeLayout(false);
			this.tabModulBoden.PerformLayout();
			this.tabModulDecke.ResumeLayout(false);
			this.tabModulDecke.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabSystemParameters;
		private System.Windows.Forms.TabPage tabEuroval;
		private System.Windows.Forms.TabPage tabHitherm;
		private System.Windows.Forms.TabPage tabModulBoden;
		private System.Windows.Forms.TabPage tabModulDecke;
		private System.Windows.Forms.Label label4;
		private NumericBox numCircuitLength;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton rbEurovalEN1264;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton rbEurovalHarreitherNorm;
		private System.Windows.Forms.Label label7;
		private NumericBox numEurovalPressureMbar;
		private System.Windows.Forms.Label label5;
		private NumericBox numEurovalPressurePa;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private NumericBox numEurovalDurchfluss;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private NumericBox numSpreizungKühlMax;
		private System.Windows.Forms.Label label16;
		private NumericBox numSpreizungKühlMin;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label10;
		private NumericBox numSpreizungHeizMax;
		private System.Windows.Forms.Label label11;
		private NumericBox numSpreizungHeizMin;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button btnEurovalStandard;
		private System.Windows.Forms.RadioButton rbModulBodenEN1264;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.RadioButton rbModulBodenHarreitherNorm;
		private System.Windows.Forms.Button btnModulBodenStandard;
		private System.Windows.Forms.Label label19;
		private NumericBox numModulBodenDurchfluss;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private NumericBox numModulBodenPressureMbar;
		private System.Windows.Forms.Label label22;
		private NumericBox numModulBodenPressurePa;
		private System.Windows.Forms.Label label23;
		private NumericBox numModulBodenMaxModulesInCircuit;
		private System.Windows.Forms.Label label26;
		private NumericBox numericBox1;
		private System.Windows.Forms.Label label27;
		private NumericBox numericBox2;
		private System.Windows.Forms.Label label28;
		private NumericBox numericBox3;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private NumericBox numericBox4;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private NumericBox numericBox5;
		private System.Windows.Forms.Label label33;
		private NumericBox numericBox6;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.RadioButton radioButton2;
	}
}
