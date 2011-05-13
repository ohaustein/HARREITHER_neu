namespace Europlan.Common {
	partial class NewObstacleForm {
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
			this.label1 = new System.Windows.Forms.Label();
			this.numHeightOffset = new Europlan.Common.NumericBox();
			this.lblHeightOffset = new System.Windows.Forms.Label();
			this.cmbType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.numHeight = new Europlan.Common.NumericBox();
			this.lblHeight = new System.Windows.Forms.Label();
			this.lblWidth = new System.Windows.Forms.Label();
			this.numWidth = new Europlan.Common.NumericBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblConstruction = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(324, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(21, 13);
			this.label1.TabIndex = 28;
			this.label1.Text = "cm";
			// 
			// numHeightOffset
			// 
			this.numHeightOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numHeightOffset.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numHeightOffset.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numHeightOffset.Location = new System.Drawing.Point(138, 85);
			this.numHeightOffset.MaxValue = null;
			this.numHeightOffset.MinValue = null;
			this.numHeightOffset.Name = "numHeightOffset";
			this.numHeightOffset.Size = new System.Drawing.Size(168, 20);
			this.numHeightOffset.TabIndex = 26;
			this.numHeightOffset.Text = "0";
			this.numHeightOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numHeightOffset.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblHeightOffset
			// 
			this.lblHeightOffset.Location = new System.Drawing.Point(12, 88);
			this.lblHeightOffset.Name = "lblHeightOffset";
			this.lblHeightOffset.Size = new System.Drawing.Size(120, 17);
			this.lblHeightOffset.TabIndex = 27;
			this.lblHeightOffset.Text = "Parapethöhe:";
			// 
			// cmbType
			// 
			this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.Location = new System.Drawing.Point(138, 6);
			this.cmbType.Name = "cmbType";
			this.cmbType.Size = new System.Drawing.Size(168, 21);
			this.cmbType.TabIndex = 25;
			this.cmbType.SelectedValueChanged += new System.EventHandler(this.cmbType_SelectedValueChanged);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(324, 62);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(21, 13);
			this.label3.TabIndex = 24;
			this.label3.Text = "cm";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(324, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(21, 13);
			this.label2.TabIndex = 23;
			this.label2.Text = "cm";
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
			this.numHeight.Location = new System.Drawing.Point(138, 59);
			this.numHeight.MaxValue = null;
			this.numHeight.MinValue = null;
			this.numHeight.Name = "numHeight";
			this.numHeight.Size = new System.Drawing.Size(168, 20);
			this.numHeight.TabIndex = 18;
			this.numHeight.Text = "0";
			this.numHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numHeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// lblHeight
			// 
			this.lblHeight.Location = new System.Drawing.Point(12, 62);
			this.lblHeight.Name = "lblHeight";
			this.lblHeight.Size = new System.Drawing.Size(120, 17);
			this.lblHeight.TabIndex = 22;
			this.lblHeight.Text = "Höhe:";
			// 
			// lblWidth
			// 
			this.lblWidth.Location = new System.Drawing.Point(12, 36);
			this.lblWidth.Name = "lblWidth";
			this.lblWidth.Size = new System.Drawing.Size(120, 17);
			this.lblWidth.TabIndex = 21;
			this.lblWidth.Text = "Breite:";
			// 
			// numWidth
			// 
			this.numWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numWidth.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numWidth.Enabled = false;
			this.numWidth.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numWidth.Location = new System.Drawing.Point(138, 33);
			this.numWidth.MaxValue = null;
			this.numWidth.MinValue = null;
			this.numWidth.Name = "numWidth";
			this.numWidth.Size = new System.Drawing.Size(168, 20);
			this.numWidth.TabIndex = 16;
			this.numWidth.Text = "0";
			this.numWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numWidth.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(216, 121);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 19;
			this.btnOk.Text = "&OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(297, 121);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 20;
			this.btnCancel.Text = "&Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblConstruction
			// 
			this.lblConstruction.Location = new System.Drawing.Point(12, 9);
			this.lblConstruction.Name = "lblConstruction";
			this.lblConstruction.Size = new System.Drawing.Size(120, 17);
			this.lblConstruction.TabIndex = 17;
			this.lblConstruction.Text = "Typ:";
			// 
			// NewObstacleForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(384, 156);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numHeightOffset);
			this.Controls.Add(this.lblHeightOffset);
			this.Controls.Add(this.cmbType);
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
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewObstacleForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Fenster/Tür hinzufügen";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private NumericBox numHeightOffset;
		private System.Windows.Forms.Label lblHeightOffset;
		private System.Windows.Forms.ComboBox cmbType;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private NumericBox numHeight;
		private System.Windows.Forms.Label lblHeight;
		private System.Windows.Forms.Label lblWidth;
		private NumericBox numWidth;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblConstruction;

	}
}