namespace Europlan.Common {
	partial class DistributorPanel {
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
			this.label2 = new System.Windows.Forms.Label();
			this.lblId = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.cmbCircuit = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.numMaxCircuits = new System.Windows.Forms.NumericUpDown();
			this.listFloors = new System.Windows.Forms.CheckedListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.numFlanschkugelhaehne = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.chkEinbauschrank = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numMaxCircuits)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFlanschkugelhaehne)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(141, 24);
			this.label1.TabIndex = 19;
			this.label1.Text = "Verteilerdaten";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(133, 23);
			this.label2.TabIndex = 20;
			this.label2.Text = "Nummer:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblId
			// 
			this.lblId.Location = new System.Drawing.Point(142, 54);
			this.lblId.Name = "lblId";
			this.lblId.Size = new System.Drawing.Size(100, 23);
			this.lblId.TabIndex = 21;
			this.lblId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 77);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(133, 23);
			this.label3.TabIndex = 22;
			this.label3.Text = "Bezeichnung:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(142, 79);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(268, 20);
			this.txtName.TabIndex = 23;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// cmbCircuit
			// 
			this.cmbCircuit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCircuit.FormattingEnabled = true;
			this.cmbCircuit.Location = new System.Drawing.Point(142, 102);
			this.cmbCircuit.Name = "cmbCircuit";
			this.cmbCircuit.Size = new System.Drawing.Size(268, 21);
			this.cmbCircuit.TabIndex = 25;
			this.cmbCircuit.SelectedIndexChanged += new System.EventHandler(this.cmbCircuit_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(3, 100);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 23);
			this.label4.TabIndex = 24;
			this.label4.Text = "Regelkreis:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(4, 123);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 23);
			this.label5.TabIndex = 26;
			this.label5.Text = "max. Heizkreise:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numMaxCircuits
			// 
			this.numMaxCircuits.Location = new System.Drawing.Point(142, 126);
			this.numMaxCircuits.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.numMaxCircuits.Name = "numMaxCircuits";
			this.numMaxCircuits.Size = new System.Drawing.Size(51, 20);
			this.numMaxCircuits.TabIndex = 27;
			this.numMaxCircuits.ValueChanged += new System.EventHandler(this.numMaxCircuits_ValueChanged);
			// 
			// listFloors
			// 
			this.listFloors.CheckOnClick = true;
			this.listFloors.FormattingEnabled = true;
			this.listFloors.Location = new System.Drawing.Point(142, 152);
			this.listFloors.Name = "listFloors";
			this.listFloors.Size = new System.Drawing.Size(268, 79);
			this.listFloors.TabIndex = 28;
			this.listFloors.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listFloors_ItemCheck);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(4, 146);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(132, 62);
			this.label6.TabIndex = 29;
			this.label6.Text = "Geschoße, die diesen Verteiler auch nutzen können:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numFlanschkugelhaehne
			// 
			this.numFlanschkugelhaehne.Location = new System.Drawing.Point(565, 129);
			this.numFlanschkugelhaehne.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.numFlanschkugelhaehne.Name = "numFlanschkugelhaehne";
			this.numFlanschkugelhaehne.Size = new System.Drawing.Size(51, 20);
			this.numFlanschkugelhaehne.TabIndex = 31;
			this.numFlanschkugelhaehne.ValueChanged += new System.EventHandler(this.numFlanschkugelhaehne_ValueChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(427, 126);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(114, 23);
			this.label7.TabIndex = 30;
			this.label7.Text = "Flanschkugelhähne:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(427, 152);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(114, 23);
			this.label8.TabIndex = 32;
			this.label8.Text = "Einbauschrank:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkEinbauschrank
			// 
			this.chkEinbauschrank.AutoSize = true;
			this.chkEinbauschrank.Location = new System.Drawing.Point(565, 156);
			this.chkEinbauschrank.Name = "chkEinbauschrank";
			this.chkEinbauschrank.Size = new System.Drawing.Size(15, 14);
			this.chkEinbauschrank.TabIndex = 33;
			this.chkEinbauschrank.UseVisualStyleBackColor = true;
			this.chkEinbauschrank.CheckedChanged += new System.EventHandler(this.chkEinbauschrank_CheckedChanged);
			// 
			// DistributorPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkEinbauschrank);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.numFlanschkugelhaehne);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.listFloors);
			this.Controls.Add(this.numMaxCircuits);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cmbCircuit);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblId);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "DistributorPanel";
			this.Size = new System.Drawing.Size(780, 492);
			((System.ComponentModel.ISupportInitialize)(this.numMaxCircuits)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFlanschkugelhaehne)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblId;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.ComboBox cmbCircuit;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown numMaxCircuits;
		private System.Windows.Forms.CheckedListBox listFloors;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown numFlanschkugelhaehne;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.CheckBox chkEinbauschrank;
	}
}
