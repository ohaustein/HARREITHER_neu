namespace Europlan.Common {
	partial class WallHelpLinesForm {
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
			this.btnClose = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbWall = new System.Windows.Forms.RadioButton();
			this.rbGlobal = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkUseGlobal = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnDelete = new System.Windows.Forms.Button();
			this.lstOffsets = new System.Windows.Forms.ListBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.numOffset = new Europlan.Common.NumericBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(302, 326);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 10;
			this.btnClose.Text = "Schließen";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.rbWall);
			this.groupBox1.Controls.Add(this.rbGlobal);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(365, 74);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Bearbeitungsmodus";
			// 
			// rbWall
			// 
			this.rbWall.AutoSize = true;
			this.rbWall.Location = new System.Drawing.Point(6, 43);
			this.rbWall.Name = "rbWall";
			this.rbWall.Size = new System.Drawing.Size(282, 17);
			this.rbWall.TabIndex = 2;
			this.rbWall.Text = "Individuelle Hilfslinien für die aktuelle Wand bearbeiten";
			this.rbWall.UseVisualStyleBackColor = true;
			this.rbWall.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
			// 
			// rbGlobal
			// 
			this.rbGlobal.AutoSize = true;
			this.rbGlobal.Checked = true;
			this.rbGlobal.Location = new System.Drawing.Point(7, 20);
			this.rbGlobal.Name = "rbGlobal";
			this.rbGlobal.Size = new System.Drawing.Size(277, 17);
			this.rbGlobal.TabIndex = 1;
			this.rbGlobal.TabStop = true;
			this.rbGlobal.Text = "Globale Hilfslinien für den gesamten Raum bearbeiten";
			this.rbGlobal.UseVisualStyleBackColor = true;
			this.rbGlobal.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.chkUseGlobal);
			this.groupBox2.Location = new System.Drawing.Point(12, 92);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(365, 47);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Eigenschaften der Wand";
			// 
			// chkUseGlobal
			// 
			this.chkUseGlobal.AutoSize = true;
			this.chkUseGlobal.Location = new System.Drawing.Point(7, 20);
			this.chkUseGlobal.Name = "chkUseGlobal";
			this.chkUseGlobal.Size = new System.Drawing.Size(158, 17);
			this.chkUseGlobal.TabIndex = 4;
			this.chkUseGlobal.Text = "Globale Hilfslinien aktivieren";
			this.chkUseGlobal.UseVisualStyleBackColor = true;
			this.chkUseGlobal.CheckedChanged += new System.EventHandler(this.chkUseGlobal_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.btnDelete);
			this.groupBox3.Controls.Add(this.lstOffsets);
			this.groupBox3.Controls.Add(this.btnAdd);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.numOffset);
			this.groupBox3.Location = new System.Drawing.Point(12, 145);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(365, 175);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Position der Hilfslinien";
			// 
			// btnDelete
			// 
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(156, 47);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(107, 23);
			this.btnDelete.TabIndex = 9;
			this.btnDelete.Text = "Löschen";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// lstOffsets
			// 
			this.lstOffsets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lstOffsets.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstOffsets.FormattingEnabled = true;
			this.lstOffsets.Location = new System.Drawing.Point(7, 46);
			this.lstOffsets.Name = "lstOffsets";
			this.lstOffsets.Size = new System.Drawing.Size(116, 121);
			this.lstOffsets.TabIndex = 7;
			this.lstOffsets.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstOffsets_DrawItem);
			this.lstOffsets.SelectedValueChanged += new System.EventHandler(this.lstOffsets_SelectedValueChanged);
			// 
			// btnAdd
			// 
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(156, 18);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(107, 23);
			this.btnAdd.TabIndex = 8;
			this.btnAdd.Text = "Hinzufügen";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(129, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(21, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "cm";
			// 
			// numOffset
			// 
			this.numOffset.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numOffset.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numOffset.Location = new System.Drawing.Point(7, 20);
			this.numOffset.MaxValue = null;
			this.numOffset.MinValue = null;
			this.numOffset.Name = "numOffset";
			this.numOffset.Size = new System.Drawing.Size(116, 20);
			this.numOffset.TabIndex = 6;
			this.numOffset.Text = "0";
			this.numOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numOffset.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numOffset.TextChanged += new System.EventHandler(this.numOffset_TextChanged);
			// 
			// WallHelpLinesForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(389, 361);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.helpProvider.SetHelpKeyword(this, "html\\euro8ud1.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WallHelpLinesForm";
			this.helpProvider.SetShowHelp(this, true);
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Hilfslinien";
			this.Load += new System.EventHandler(this.WallHelpLinesForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WallHelpLinesForm_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbWall;
		private System.Windows.Forms.RadioButton rbGlobal;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkUseGlobal;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Label label1;
		private NumericBox numOffset;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.ListBox lstOffsets;
	}
}