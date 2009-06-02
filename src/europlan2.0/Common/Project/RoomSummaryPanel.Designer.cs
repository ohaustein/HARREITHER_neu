namespace Europlan.Common {
	partial class RoomSummaryPanel {
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
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.lblArea = new System.Windows.Forms.Label();
			this.lblTemperature = new System.Windows.Forms.Label();
			this.lblHeat = new System.Windows.Forms.Label();
			this.lblNormHeat = new System.Windows.Forms.Label();
			this.lblCool = new System.Windows.Forms.Label();
			this.lblNormCool = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtNormCool = new Europlan.Common.NumericBox();
			this.txtCool = new Europlan.Common.NumericBox();
			this.txtNormHeat = new Europlan.Common.NumericBox();
			this.txtHeat = new Europlan.Common.NumericBox();
			this.txtTemperature = new Europlan.Common.NumericBox();
			this.txtArea = new Europlan.Common.NumericBox();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(129, 2);
			this.txtName.Name = "txtName";
			this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtName.Size = new System.Drawing.Size(406, 20);
			this.txtName.TabIndex = 13;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(3, 2);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(108, 23);
			this.lblName.TabIndex = 12;
			this.lblName.Text = "Name:";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// lblArea
			// 
			this.lblArea.Location = new System.Drawing.Point(3, 25);
			this.lblArea.Name = "lblArea";
			this.lblArea.Size = new System.Drawing.Size(108, 23);
			this.lblArea.TabIndex = 14;
			this.lblArea.Text = "Fläche:";
			this.lblArea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTemperature
			// 
			this.lblTemperature.Location = new System.Drawing.Point(3, 48);
			this.lblTemperature.Name = "lblTemperature";
			this.lblTemperature.Size = new System.Drawing.Size(120, 23);
			this.lblTemperature.TabIndex = 16;
			this.lblTemperature.Text = "Norminnentemperatur:";
			this.lblTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblHeat
			// 
			this.lblHeat.Location = new System.Drawing.Point(3, 71);
			this.lblHeat.Name = "lblHeat";
			this.lblHeat.Size = new System.Drawing.Size(120, 23);
			this.lblHeat.TabIndex = 18;
			this.lblHeat.Text = "Heizlast:";
			this.lblHeat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblNormHeat
			// 
			this.lblNormHeat.Location = new System.Drawing.Point(3, 94);
			this.lblNormHeat.Name = "lblNormHeat";
			this.lblNormHeat.Size = new System.Drawing.Size(120, 23);
			this.lblNormHeat.TabIndex = 20;
			this.lblNormHeat.Text = "Heizlast (bereinigt):";
			this.lblNormHeat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblCool
			// 
			this.lblCool.Location = new System.Drawing.Point(3, 117);
			this.lblCool.Name = "lblCool";
			this.lblCool.Size = new System.Drawing.Size(120, 23);
			this.lblCool.TabIndex = 22;
			this.lblCool.Text = "Kühllast:";
			this.lblCool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblNormCool
			// 
			this.lblNormCool.Location = new System.Drawing.Point(3, 140);
			this.lblNormCool.Name = "lblNormCool";
			this.lblNormCool.Size = new System.Drawing.Size(120, 23);
			this.lblNormCool.TabIndex = 24;
			this.lblNormCool.Text = "Kühllast (bereinigt):";
			this.lblNormCool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(541, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 23);
			this.label1.TabIndex = 26;
			this.label1.Text = "m²";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(541, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 23);
			this.label2.TabIndex = 27;
			this.label2.Text = "°C";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(541, 71);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 23);
			this.label3.TabIndex = 28;
			this.label3.Text = "W";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(541, 94);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 23);
			this.label4.TabIndex = 29;
			this.label4.Text = "W";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(541, 117);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 23);
			this.label5.TabIndex = 30;
			this.label5.Text = "W";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(541, 140);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 23);
			this.label6.TabIndex = 31;
			this.label6.Text = "W";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNormCool
			// 
			this.txtNormCool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNormCool.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.txtNormCool.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormCool.Location = new System.Drawing.Point(129, 140);
			this.txtNormCool.Name = "txtNormCool";
			this.txtNormCool.Size = new System.Drawing.Size(406, 20);
			this.txtNormCool.TabIndex = 38;
			this.txtNormCool.Text = "0";
			this.txtNormCool.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormCool.ValueChanged += new System.EventHandler(this.txtNormCool_TextChanged);
			// 
			// txtCool
			// 
			this.txtCool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCool.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.txtCool.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtCool.Location = new System.Drawing.Point(129, 117);
			this.txtCool.Name = "txtCool";
			this.txtCool.Size = new System.Drawing.Size(406, 20);
			this.txtCool.TabIndex = 37;
			this.txtCool.Text = "0";
			this.txtCool.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtCool.ValueChanged += new System.EventHandler(this.txtCool_TextChanged);
			// 
			// txtNormHeat
			// 
			this.txtNormHeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNormHeat.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.txtNormHeat.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormHeat.Location = new System.Drawing.Point(129, 94);
			this.txtNormHeat.Name = "txtNormHeat";
			this.txtNormHeat.Size = new System.Drawing.Size(406, 20);
			this.txtNormHeat.TabIndex = 36;
			this.txtNormHeat.Text = "0";
			this.txtNormHeat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormHeat.ValueChanged += new System.EventHandler(this.txtNormHeat_TextChanged);
			// 
			// txtHeat
			// 
			this.txtHeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHeat.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.txtHeat.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtHeat.Location = new System.Drawing.Point(129, 71);
			this.txtHeat.Name = "txtHeat";
			this.txtHeat.Size = new System.Drawing.Size(406, 20);
			this.txtHeat.TabIndex = 35;
			this.txtHeat.Text = "0";
			this.txtHeat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtHeat.ValueChanged += new System.EventHandler(this.txtHeat_TextChanged);
			// 
			// txtTemperature
			// 
			this.txtTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTemperature.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.txtTemperature.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtTemperature.Location = new System.Drawing.Point(129, 48);
			this.txtTemperature.Name = "txtTemperature";
			this.txtTemperature.Size = new System.Drawing.Size(406, 20);
			this.txtTemperature.TabIndex = 34;
			this.txtTemperature.Text = "0";
			this.txtTemperature.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtTemperature.ValueChanged += new System.EventHandler(this.txtTemperature_TextChanged);
			// 
			// txtArea
			// 
			this.txtArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtArea.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.txtArea.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtArea.Location = new System.Drawing.Point(129, 25);
			this.txtArea.Name = "txtArea";
			this.txtArea.Size = new System.Drawing.Size(406, 20);
			this.txtArea.TabIndex = 33;
			this.txtArea.Text = "0";
			this.txtArea.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtArea.ValueChanged += new System.EventHandler(this.txtArea_TextChanged);
			// 
			// RoomSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtNormCool);
			this.Controls.Add(this.txtCool);
			this.Controls.Add(this.txtNormHeat);
			this.Controls.Add(this.txtHeat);
			this.Controls.Add(this.txtTemperature);
			this.Controls.Add(this.txtArea);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblNormCool);
			this.Controls.Add(this.lblCool);
			this.Controls.Add(this.lblNormHeat);
			this.Controls.Add(this.lblHeat);
			this.Controls.Add(this.lblTemperature);
			this.Controls.Add(this.lblArea);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.Name = "RoomSummaryPanel";
			this.Size = new System.Drawing.Size(583, 301);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblArea;
		private System.Windows.Forms.Label lblTemperature;
		private System.Windows.Forms.Label lblHeat;
		private System.Windows.Forms.Label lblNormHeat;
		private System.Windows.Forms.Label lblCool;
		private System.Windows.Forms.Label lblNormCool;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private Europlan.Common.NumericBox txtArea;
		private Europlan.Common.NumericBox txtTemperature;
		private Europlan.Common.NumericBox txtHeat;
		private Europlan.Common.NumericBox txtNormHeat;
		private Europlan.Common.NumericBox txtCool;
		private Europlan.Common.NumericBox txtNormCool;
	}
}
