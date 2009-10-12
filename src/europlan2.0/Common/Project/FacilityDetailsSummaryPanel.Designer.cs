namespace Europlan.Common {
	partial class FacilityDetailsSummaryPanel {
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
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.chkSpreizung = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.numNormOutsideTemperature = new Europlan.Common.NumericBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.numDewPoint = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.numInsideTemperature = new Europlan.Common.NumericBox();
			this.label8 = new System.Windows.Forms.Label();
			this.numHumidity = new Europlan.Common.NumericBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.numOutsideTemperature = new Europlan.Common.NumericBox();
			this.chkCool = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 24);
			this.label1.TabIndex = 20;
			this.label1.Text = "Anlagedaten";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(133, 23);
			this.label3.TabIndex = 23;
			this.label3.Text = "Normaußentemperatur:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(133, 23);
			this.label2.TabIndex = 24;
			this.label2.Text = "Spreizung:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkSpreizung
			// 
			this.chkSpreizung.AutoSize = true;
			this.chkSpreizung.Location = new System.Drawing.Point(145, 43);
			this.chkSpreizung.Name = "chkSpreizung";
			this.chkSpreizung.Size = new System.Drawing.Size(297, 17);
			this.chkSpreizung.TabIndex = 26;
			this.chkSpreizung.Text = "variable Spreizung für endgültige Berechnung verwenden";
			this.chkSpreizung.UseVisualStyleBackColor = true;
			this.chkSpreizung.CheckedChanged += new System.EventHandler(this.chkSpreizung_CheckedChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(204, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(46, 23);
			this.label4.TabIndex = 27;
			this.label4.Text = "°C";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.chkSpreizung);
			this.groupBox1.Controls.Add(this.numNormOutsideTemperature);
			this.groupBox1.Location = new System.Drawing.Point(3, 39);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(713, 71);
			this.groupBox1.TabIndex = 28;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Generell";
			// 
			// numNormOutsideTemperature
			// 
			this.numNormOutsideTemperature.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numNormOutsideTemperature.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numNormOutsideTemperature.Location = new System.Drawing.Point(145, 18);
			this.numNormOutsideTemperature.MaxValue = null;
			this.numNormOutsideTemperature.MinValue = null;
			this.numNormOutsideTemperature.Name = "numNormOutsideTemperature";
			this.numNormOutsideTemperature.Size = new System.Drawing.Size(53, 20);
			this.numNormOutsideTemperature.TabIndex = 25;
			this.numNormOutsideTemperature.Text = "0";
			this.numNormOutsideTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numNormOutsideTemperature.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numNormOutsideTemperature.ValueChanged += new System.EventHandler(this.numNormOutsideTemperature_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.numDewPoint);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.numInsideTemperature);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.numHumidity);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.numOutsideTemperature);
			this.groupBox2.Controls.Add(this.chkCool);
			this.groupBox2.Location = new System.Drawing.Point(3, 116);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(713, 141);
			this.groupBox2.TabIndex = 29;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Kühlung";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(236, 62);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(46, 23);
			this.label5.TabIndex = 40;
			this.label5.Text = " %";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 108);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(133, 23);
			this.label12.TabIndex = 37;
			this.label12.Text = "Taupunkttemperatur:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(236, 108);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(46, 23);
			this.label13.TabIndex = 39;
			this.label13.Text = "°C";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numDewPoint
			// 
			this.numDewPoint.Enabled = false;
			this.numDewPoint.Location = new System.Drawing.Point(178, 110);
			this.numDewPoint.Name = "numDewPoint";
			this.numDewPoint.Size = new System.Drawing.Size(52, 20);
			this.numDewPoint.TabIndex = 38;
			this.numDewPoint.Text = "0";
			this.numDewPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(6, 85);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(158, 23);
			this.label10.TabIndex = 34;
			this.label10.Text = "Innentemperatur für Kühlung:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(236, 85);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(46, 23);
			this.label11.TabIndex = 36;
			this.label11.Text = "°C";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numInsideTemperature
			// 
			this.numInsideTemperature.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numInsideTemperature.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numInsideTemperature.Location = new System.Drawing.Point(178, 87);
			this.numInsideTemperature.MaxValue = null;
			this.numInsideTemperature.MinValue = null;
			this.numInsideTemperature.Name = "numInsideTemperature";
			this.numInsideTemperature.Size = new System.Drawing.Size(52, 20);
			this.numInsideTemperature.TabIndex = 35;
			this.numInsideTemperature.Text = "0";
			this.numInsideTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numInsideTemperature.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numInsideTemperature.ValueChanged += new System.EventHandler(this.numInsideTemperature_ValueChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 62);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(133, 23);
			this.label8.TabIndex = 31;
			this.label8.Text = "Relative Luftfeuchtigkeit:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numHumidity
			// 
			this.numHumidity.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numHumidity.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numHumidity.Location = new System.Drawing.Point(178, 64);
			this.numHumidity.MaxValue = null;
			this.numHumidity.MinValue = null;
			this.numHumidity.Name = "numHumidity";
			this.numHumidity.Size = new System.Drawing.Size(52, 20);
			this.numHumidity.TabIndex = 32;
			this.numHumidity.Text = "0";
			this.numHumidity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numHumidity.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numHumidity.ValueChanged += new System.EventHandler(this.numHumidity_ValueChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 39);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(166, 23);
			this.label6.TabIndex = 28;
			this.label6.Text = "Außentemperatur für Kühlung:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(236, 39);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(46, 23);
			this.label7.TabIndex = 30;
			this.label7.Text = "°C";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numOutsideTemperature
			// 
			this.numOutsideTemperature.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numOutsideTemperature.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numOutsideTemperature.Location = new System.Drawing.Point(178, 41);
			this.numOutsideTemperature.MaxValue = null;
			this.numOutsideTemperature.MinValue = null;
			this.numOutsideTemperature.Name = "numOutsideTemperature";
			this.numOutsideTemperature.Size = new System.Drawing.Size(52, 20);
			this.numOutsideTemperature.TabIndex = 29;
			this.numOutsideTemperature.Text = "0";
			this.numOutsideTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numOutsideTemperature.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numOutsideTemperature.ValueChanged += new System.EventHandler(this.numOutsideTemperature_ValueChanged);
			// 
			// chkCool
			// 
			this.chkCool.AutoSize = true;
			this.chkCool.Location = new System.Drawing.Point(178, 20);
			this.chkCool.Name = "chkCool";
			this.chkCool.Size = new System.Drawing.Size(137, 17);
			this.chkCool.TabIndex = 26;
			this.chkCool.Text = "Kühlleistung berechnen";
			this.chkCool.UseVisualStyleBackColor = true;
			this.chkCool.CheckedChanged += new System.EventHandler(this.chkCool_CheckedChanged);
			// 
			// FacilityDetailsSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Name = "FacilityDetailsSummaryPanel";
			this.Size = new System.Drawing.Size(716, 473);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private NumericBox numNormOutsideTemperature;
		private System.Windows.Forms.CheckBox chkSpreizung;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkCool;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox numDewPoint;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private NumericBox numInsideTemperature;
		private System.Windows.Forms.Label label8;
		private NumericBox numHumidity;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private NumericBox numOutsideTemperature;


	}
}
