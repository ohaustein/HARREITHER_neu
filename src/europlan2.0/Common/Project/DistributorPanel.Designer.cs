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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DistributorPanel));
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
			this.chkEinbauschrank = new System.Windows.Forms.CheckBox();
			this.chkFlansch = new System.Windows.Forms.CheckBox();
			this.cmbDistributorType = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbAnschlussHollaender = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.chkAnschluss = new System.Windows.Forms.CheckBox();
			this.numAdditionalCircuits = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.numZusStellantriebe = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.lstSystems = new System.Windows.Forms.CheckedListBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.lblCircuits = new System.Windows.Forms.Label();
			this.lstError = new System.Windows.Forms.ListView();
			this.defaultColumn = new System.Windows.Forms.ColumnHeader();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.numMaxCircuits)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numAdditionalCircuits)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numZusStellantriebe)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(35, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(141, 24);
			this.label1.TabIndex = 19;
			this.label1.Text = "Verteilerdaten";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(162, 23);
			this.label2.TabIndex = 20;
			this.label2.Text = "Nummer:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblId
			// 
			this.lblId.Location = new System.Drawing.Point(172, 54);
			this.lblId.Name = "lblId";
			this.lblId.Size = new System.Drawing.Size(100, 23);
			this.lblId.TabIndex = 21;
			this.lblId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 77);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(162, 23);
			this.label3.TabIndex = 22;
			this.label3.Text = "Bezeichnung:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(171, 77);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(385, 20);
			this.txtName.TabIndex = 23;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// cmbCircuit
			// 
			this.cmbCircuit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCircuit.FormattingEnabled = true;
			this.cmbCircuit.Location = new System.Drawing.Point(172, 105);
			this.cmbCircuit.Name = "cmbCircuit";
			this.cmbCircuit.Size = new System.Drawing.Size(385, 21);
			this.cmbCircuit.TabIndex = 25;
			this.cmbCircuit.SelectedIndexChanged += new System.EventHandler(this.cmbCircuit_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(3, 103);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(163, 23);
			this.label4.TabIndex = 24;
			this.label4.Text = "Regelkreis:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(4, 213);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(162, 23);
			this.label5.TabIndex = 26;
			this.label5.Text = "max. Heizkreise:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numMaxCircuits
			// 
			this.numMaxCircuits.Location = new System.Drawing.Point(172, 216);
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
			this.listFloors.Location = new System.Drawing.Point(172, 268);
			this.listFloors.Name = "listFloors";
			this.listFloors.Size = new System.Drawing.Size(385, 79);
			this.listFloors.TabIndex = 28;
			this.listFloors.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listFloors_ItemCheck);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 268);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(163, 45);
			this.label6.TabIndex = 29;
			this.label6.Text = "Geschoße, die diesen Verteiler auch nutzen können:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkEinbauschrank
			// 
			this.chkEinbauschrank.AutoSize = true;
			this.chkEinbauschrank.Location = new System.Drawing.Point(375, 170);
			this.chkEinbauschrank.Name = "chkEinbauschrank";
			this.chkEinbauschrank.Size = new System.Drawing.Size(97, 17);
			this.chkEinbauschrank.TabIndex = 33;
			this.chkEinbauschrank.Text = "Einbauschrank";
			this.chkEinbauschrank.UseVisualStyleBackColor = true;
			this.chkEinbauschrank.CheckedChanged += new System.EventHandler(this.chkEinbauschrank_CheckedChanged);
			// 
			// chkFlansch
			// 
			this.chkFlansch.AutoSize = true;
			this.chkFlansch.Location = new System.Drawing.Point(172, 170);
			this.chkFlansch.Name = "chkFlansch";
			this.chkFlansch.Size = new System.Drawing.Size(119, 17);
			this.chkFlansch.TabIndex = 34;
			this.chkFlansch.Text = "Flanschkugelhähne";
			this.chkFlansch.UseVisualStyleBackColor = true;
			this.chkFlansch.CheckedChanged += new System.EventHandler(this.chkFlansch_CheckedChanged);
			// 
			// cmbDistributorType
			// 
			this.cmbDistributorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDistributorType.FormattingEnabled = true;
			this.cmbDistributorType.Location = new System.Drawing.Point(389, 49);
			this.cmbDistributorType.Name = "cmbDistributorType";
			this.cmbDistributorType.Size = new System.Drawing.Size(51, 21);
			this.cmbDistributorType.TabIndex = 36;
			this.cmbDistributorType.Visible = false;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(386, 23);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 23);
			this.label9.TabIndex = 35;
			this.label9.Text = "Verteilertyp:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label9.Visible = false;
			// 
			// cmbAnschlussHollaender
			// 
			this.cmbAnschlussHollaender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAnschlussHollaender.FormattingEnabled = true;
			this.cmbAnschlussHollaender.Location = new System.Drawing.Point(172, 132);
			this.cmbAnschlussHollaender.Name = "cmbAnschlussHollaender";
			this.cmbAnschlussHollaender.Size = new System.Drawing.Size(385, 21);
			this.cmbAnschlussHollaender.TabIndex = 38;
			this.cmbAnschlussHollaender.SelectedIndexChanged += new System.EventHandler(this.cmbAnschlussHollaender_SelectedIndexChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(3, 130);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(163, 23);
			this.label10.TabIndex = 37;
			this.label10.Text = "Anschlußholländer:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(3, 166);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(163, 23);
			this.label11.TabIndex = 39;
			this.label11.Text = "Zubehör:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkAnschluss
			// 
			this.chkAnschluss.AutoSize = true;
			this.chkAnschluss.Location = new System.Drawing.Point(172, 193);
			this.chkAnschluss.Name = "chkAnschluss";
			this.chkAnschluss.Size = new System.Drawing.Size(137, 17);
			this.chkAnschluss.TabIndex = 41;
			this.chkAnschluss.Text = "Lange Anschlussbögen";
			this.chkAnschluss.UseVisualStyleBackColor = true;
			this.chkAnschluss.CheckedChanged += new System.EventHandler(this.chkAnschluss_CheckedChanged);
			// 
			// numAdditionalCircuits
			// 
			this.numAdditionalCircuits.Location = new System.Drawing.Point(172, 242);
			this.numAdditionalCircuits.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.numAdditionalCircuits.Name = "numAdditionalCircuits";
			this.numAdditionalCircuits.Size = new System.Drawing.Size(51, 20);
			this.numAdditionalCircuits.TabIndex = 43;
			this.numAdditionalCircuits.ValueChanged += new System.EventHandler(this.numAdditionalCircuits_ValueChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(4, 239);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(162, 23);
			this.label7.TabIndex = 42;
			this.label7.Text = "zus. Heizkreise:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numZusStellantriebe
			// 
			this.numZusStellantriebe.Location = new System.Drawing.Point(505, 242);
			this.numZusStellantriebe.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numZusStellantriebe.Name = "numZusStellantriebe";
			this.numZusStellantriebe.Size = new System.Drawing.Size(51, 20);
			this.numZusStellantriebe.TabIndex = 45;
			this.numZusStellantriebe.ValueChanged += new System.EventHandler(this.numZusStellantriebe_ValueChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(251, 239);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(179, 23);
			this.label8.TabIndex = 44;
			this.label8.Text = "zus. Stellantriebe:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lstSystems
			// 
			this.lstSystems.CheckOnClick = true;
			this.lstSystems.FormattingEnabled = true;
			this.lstSystems.Location = new System.Drawing.Point(172, 354);
			this.lstSystems.Name = "lstSystems";
			this.lstSystems.Size = new System.Drawing.Size(385, 79);
			this.lstSystems.TabIndex = 46;
			this.lstSystems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstSystems_ItemCheck);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(4, 354);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(162, 65);
			this.label12.TabIndex = 47;
			this.label12.Text = "Heizsysteme, die standard- mäßig an diesen Verteiler angeschlossen werden sollen:" +
				"";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(251, 213);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(179, 23);
			this.label13.TabIndex = 48;
			this.label13.Text = "zugewiesene Heizkreise: ";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblCircuits
			// 
			this.lblCircuits.Location = new System.Drawing.Point(456, 213);
			this.lblCircuits.Name = "lblCircuits";
			this.lblCircuits.Size = new System.Drawing.Size(101, 23);
			this.lblCircuits.TabIndex = 49;
			this.lblCircuits.Text = "7 (aktiv)";
			this.lblCircuits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lstError
			// 
			this.lstError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstError.BackColor = System.Drawing.SystemColors.Control;
			this.lstError.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstError.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.defaultColumn});
			this.lstError.FullRowSelect = true;
			this.lstError.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lstError.LabelWrap = false;
			this.lstError.Location = new System.Drawing.Point(7, 439);
			this.lstError.Name = "lstError";
			this.lstError.ShowGroups = false;
			this.lstError.Size = new System.Drawing.Size(770, 50);
			this.lstError.TabIndex = 74;
			this.lstError.UseCompatibleStateImageBehavior = false;
			this.lstError.View = System.Windows.Forms.View.Details;
			this.lstError.Visible = false;
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(1, 0);
			this.pictureBox1.MaximumSize = new System.Drawing.Size(32, 32);
			this.pictureBox1.MinimumSize = new System.Drawing.Size(32, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 75;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// DistributorPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.lstError);
			this.Controls.Add(this.lblCircuits);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.lstSystems);
			this.Controls.Add(this.numZusStellantriebe);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.numAdditionalCircuits);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.chkAnschluss);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.cmbAnschlussHollaender);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.cmbDistributorType);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.chkFlansch);
			this.Controls.Add(this.chkEinbauschrank);
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
			this.helpProvider.SetHelpKeyword(this, "html\\euro38of.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.Name = "DistributorPanel";
			this.helpProvider.SetShowHelp(this, true);
			this.Size = new System.Drawing.Size(780, 492);
			((System.ComponentModel.ISupportInitialize)(this.numMaxCircuits)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numAdditionalCircuits)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numZusStellantriebe)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
		private System.Windows.Forms.CheckBox chkEinbauschrank;
		private System.Windows.Forms.CheckBox chkFlansch;
		private System.Windows.Forms.ComboBox cmbDistributorType;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cmbAnschlussHollaender;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox chkAnschluss;
		private System.Windows.Forms.NumericUpDown numAdditionalCircuits;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown numZusStellantriebe;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.CheckedListBox lstSystems;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label lblCircuits;
		private System.Windows.Forms.ListView lstError;
		private System.Windows.Forms.ColumnHeader defaultColumn;
		private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.PictureBox pictureBox1;
	}
}
