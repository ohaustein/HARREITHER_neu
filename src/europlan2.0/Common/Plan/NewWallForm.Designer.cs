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
			this.numWidth = new Europlan.Common.NumericBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblWidth = new System.Windows.Forms.Label();
			this.lblHeight = new System.Windows.Forms.Label();
			this.numHeight = new Europlan.Common.NumericBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtConstruction = new System.Windows.Forms.TextBox();
			this.btnSelectConstruction = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblConstruction
			// 
			this.lblConstruction.Location = new System.Drawing.Point(12, 9);
			this.lblConstruction.Name = "lblConstruction";
			this.lblConstruction.Size = new System.Drawing.Size(167, 17);
			this.lblConstruction.TabIndex = 2;
			this.lblConstruction.Text = "Konstruktion:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(375, 264);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "&Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(294, 264);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "&OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// numWidth
			// 
			this.numWidth.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numWidth.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numWidth.Location = new System.Drawing.Point(185, 33);
			this.numWidth.MaxValue = null;
			this.numWidth.MinValue = null;
			this.numWidth.Name = "numWidth";
			this.numWidth.Size = new System.Drawing.Size(121, 20);
			this.numWidth.TabIndex = 4;
			this.numWidth.Text = "0";
			this.numWidth.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(473, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(21, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "cm";
			// 
			// lblWidth
			// 
			this.lblWidth.Location = new System.Drawing.Point(12, 36);
			this.lblWidth.Name = "lblWidth";
			this.lblWidth.Size = new System.Drawing.Size(167, 17);
			this.lblWidth.TabIndex = 6;
			this.lblWidth.Text = "Breite:";
			// 
			// lblHeight
			// 
			this.lblHeight.Location = new System.Drawing.Point(12, 62);
			this.lblHeight.Name = "lblHeight";
			this.lblHeight.Size = new System.Drawing.Size(167, 17);
			this.lblHeight.TabIndex = 7;
			this.lblHeight.Text = "Höhe:";
			// 
			// numHeight
			// 
			this.numHeight.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numHeight.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numHeight.Location = new System.Drawing.Point(185, 59);
			this.numHeight.MaxValue = null;
			this.numHeight.MinValue = null;
			this.numHeight.Name = "numHeight";
			this.numHeight.Size = new System.Drawing.Size(121, 20);
			this.numHeight.TabIndex = 8;
			this.numHeight.Text = "0";
			this.numHeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(312, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(21, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "cm";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(312, 62);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(21, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "cm";
			// 
			// txtConstruction
			// 
			this.txtConstruction.Location = new System.Drawing.Point(185, 6);
			this.txtConstruction.Name = "txtConstruction";
			this.txtConstruction.ReadOnly = true;
			this.txtConstruction.Size = new System.Drawing.Size(121, 20);
			this.txtConstruction.TabIndex = 12;
			// 
			// btnSelectConstruction
			// 
			this.btnSelectConstruction.Location = new System.Drawing.Point(315, 4);
			this.btnSelectConstruction.Name = "btnSelectConstruction";
			this.btnSelectConstruction.Size = new System.Drawing.Size(30, 23);
			this.btnSelectConstruction.TabIndex = 13;
			this.btnSelectConstruction.Text = "...";
			this.btnSelectConstruction.UseVisualStyleBackColor = true;
			this.btnSelectConstruction.Click += new System.EventHandler(this.btnSelectConstruction_Click);
			// 
			// NewWallForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(462, 299);
			this.Controls.Add(this.btnSelectConstruction);
			this.Controls.Add(this.txtConstruction);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.numHeight);
			this.Controls.Add(this.lblHeight);
			this.Controls.Add(this.lblWidth);
			this.Controls.Add(this.label1);
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
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblConstruction;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.HelpProvider helpProvider;
		private NumericBox numWidth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblWidth;
		private System.Windows.Forms.Label lblHeight;
		private NumericBox numHeight;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtConstruction;
		private System.Windows.Forms.Button btnSelectConstruction;
	}
}