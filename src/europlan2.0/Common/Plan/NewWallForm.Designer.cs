namespace Europlan.Common {
	partial class NewWallForm {
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
			this.lblConstruction = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.lblWidth = new System.Windows.Forms.Label();
			this.lblHeight = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtConstruction = new System.Windows.Forms.TextBox();
			this.btnSelectConstruction = new System.Windows.Forms.Button();
			this.lblConstructionName = new System.Windows.Forms.Label();
			this.numHeight = new Europlan.Common.NumericBox();
			this.numWidth = new Europlan.Common.NumericBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.numWallId = new Europlan.Common.NumericBox();
			this.rbSchraege = new System.Windows.Forms.RadioButton();
			this.rbAfter = new System.Windows.Forms.RadioButton();
			this.rbNext = new System.Windows.Forms.RadioButton();
			this.rbPrev = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblConstruction
			// 
			this.lblConstruction.Location = new System.Drawing.Point(12, 9);
			this.lblConstruction.Name = "lblConstruction";
			this.lblConstruction.Size = new System.Drawing.Size(120, 17);
			this.lblConstruction.TabIndex = 2;
			this.lblConstruction.Text = "Konstruktion:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(297, 231);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(216, 231);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "&OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// lblWidth
			// 
			this.lblWidth.Location = new System.Drawing.Point(12, 54);
			this.lblWidth.Name = "lblWidth";
			this.lblWidth.Size = new System.Drawing.Size(120, 17);
			this.lblWidth.TabIndex = 6;
			this.lblWidth.Text = "Breite:";
			// 
			// lblHeight
			// 
			this.lblHeight.Location = new System.Drawing.Point(12, 80);
			this.lblHeight.Name = "lblHeight";
			this.lblHeight.Size = new System.Drawing.Size(120, 17);
			this.lblHeight.TabIndex = 7;
			this.lblHeight.Text = "Höhe:";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(324, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(21, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "cm";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(324, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(21, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "cm";
			// 
			// txtConstruction
			// 
			this.txtConstruction.Location = new System.Drawing.Point(138, 6);
			this.txtConstruction.Name = "txtConstruction";
			this.txtConstruction.ReadOnly = true;
			this.txtConstruction.Size = new System.Drawing.Size(168, 20);
			this.txtConstruction.TabIndex = 12;
			// 
			// btnSelectConstruction
			// 
			this.btnSelectConstruction.Location = new System.Drawing.Point(315, 4);
			this.btnSelectConstruction.Name = "btnSelectConstruction";
			this.btnSelectConstruction.Size = new System.Drawing.Size(30, 23);
			this.btnSelectConstruction.TabIndex = 6;
			this.btnSelectConstruction.Text = "...";
			this.btnSelectConstruction.UseVisualStyleBackColor = true;
			this.btnSelectConstruction.Click += new System.EventHandler(this.btnSelectConstruction_Click);
			// 
			// lblConstructionName
			// 
			this.lblConstructionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblConstructionName.Location = new System.Drawing.Point(15, 30);
			this.lblConstructionName.Name = "lblConstructionName";
			this.lblConstructionName.Size = new System.Drawing.Size(330, 18);
			this.lblConstructionName.TabIndex = 14;
			this.lblConstructionName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// numHeight
			// 
			this.numHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numHeight.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numHeight.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numHeight.Location = new System.Drawing.Point(138, 77);
			this.numHeight.MaxValue = null;
			this.numHeight.MinValue = null;
			this.numHeight.Name = "numHeight";
			this.numHeight.Size = new System.Drawing.Size(168, 20);
			this.numHeight.TabIndex = 2;
			this.numHeight.Text = "0";
			this.numHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numHeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// numWidth
			// 
			this.numWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numWidth.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numWidth.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numWidth.Location = new System.Drawing.Point(138, 51);
			this.numWidth.MaxValue = null;
			this.numWidth.MinValue = null;
			this.numWidth.Name = "numWidth";
			this.numWidth.Size = new System.Drawing.Size(168, 20);
			this.numWidth.TabIndex = 1;
			this.numWidth.Text = "0";
			this.numWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numWidth.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.numWallId);
			this.groupBox1.Controls.Add(this.rbSchraege);
			this.groupBox1.Controls.Add(this.rbAfter);
			this.groupBox1.Controls.Add(this.rbNext);
			this.groupBox1.Controls.Add(this.rbPrev);
			this.groupBox1.Location = new System.Drawing.Point(12, 103);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(360, 122);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Position";
			// 
			// numWallId
			// 
			this.numWallId.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numWallId.Enabled = false;
			this.numWallId.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWallId.Location = new System.Drawing.Point(204, 65);
			this.numWallId.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numWallId.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWallId.Name = "numWallId";
			this.numWallId.Size = new System.Drawing.Size(150, 20);
			this.numWallId.TabIndex = 4;
			this.numWallId.Text = "1";
			this.numWallId.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// rbSchraege
			// 
			this.rbSchraege.AutoSize = true;
			this.rbSchraege.Location = new System.Drawing.Point(7, 89);
			this.rbSchraege.Name = "rbSchraege";
			this.rbSchraege.Size = new System.Drawing.Size(169, 17);
			this.rbSchraege.TabIndex = 3;
			this.rbSchraege.Text = "Als Schräge für aktuelle Wand";
			this.rbSchraege.UseVisualStyleBackColor = true;
			this.rbSchraege.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
			// 
			// rbAfter
			// 
			this.rbAfter.AutoSize = true;
			this.rbAfter.Location = new System.Drawing.Point(7, 66);
			this.rbAfter.Name = "rbAfter";
			this.rbAfter.Size = new System.Drawing.Size(100, 17);
			this.rbAfter.TabIndex = 2;
			this.rbAfter.Text = "Nach Wand Nr.";
			this.rbAfter.UseVisualStyleBackColor = true;
			this.rbAfter.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
			// 
			// rbNext
			// 
			this.rbNext.AutoSize = true;
			this.rbNext.Checked = true;
			this.rbNext.Location = new System.Drawing.Point(7, 43);
			this.rbNext.Name = "rbNext";
			this.rbNext.Size = new System.Drawing.Size(126, 17);
			this.rbNext.TabIndex = 1;
			this.rbNext.TabStop = true;
			this.rbNext.Text = "Nach aktueller Wand";
			this.rbNext.UseVisualStyleBackColor = true;
			this.rbNext.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
			// 
			// rbPrev
			// 
			this.rbPrev.AutoSize = true;
			this.rbPrev.Location = new System.Drawing.Point(7, 20);
			this.rbPrev.Name = "rbPrev";
			this.rbPrev.Size = new System.Drawing.Size(116, 17);
			this.rbPrev.TabIndex = 0;
			this.rbPrev.Text = "Vor aktueller Wand";
			this.rbPrev.UseVisualStyleBackColor = true;
			this.rbPrev.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
			// 
			// NewWallForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(384, 266);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblConstructionName);
			this.Controls.Add(this.btnSelectConstruction);
			this.Controls.Add(this.txtConstruction);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.numHeight);
			this.Controls.Add(this.lblHeight);
			this.Controls.Add(this.lblWidth);
			this.Controls.Add(this.numWidth);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblConstruction);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.helpProvider.SetHelpKeyword(this, "html\\euro8ud1.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewWallForm";
			this.helpProvider.SetShowHelp(this, true);
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Wand hinzufügen";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblConstruction;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.HelpProvider helpProvider;
		private NumericBox numWidth;
		private System.Windows.Forms.Label lblWidth;
		private System.Windows.Forms.Label lblHeight;
		private NumericBox numHeight;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtConstruction;
		private System.Windows.Forms.Button btnSelectConstruction;
		private System.Windows.Forms.Label lblConstructionName;
		private System.Windows.Forms.GroupBox groupBox1;
		private NumericBox numWallId;
		private System.Windows.Forms.RadioButton rbSchraege;
		private System.Windows.Forms.RadioButton rbAfter;
		private System.Windows.Forms.RadioButton rbNext;
		private System.Windows.Forms.RadioButton rbPrev;
	}
}