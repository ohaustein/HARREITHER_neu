namespace Europlan.Common {
	partial class PlanSetMeasureForm {
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
			this.lblText = new System.Windows.Forms.Label();
			this.lblLength = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtLength = new Europlan.Common.NumericBox();
			this.SuspendLayout();
			// 
			// lblText
			// 
			this.lblText.Location = new System.Drawing.Point(12, 9);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(248, 58);
			this.lblText.TabIndex = 0;
			this.lblText.Text = "Für diesen Plan ist noch kein Maßstab gesetzt. Bitte geben Sie nun die Länge für " +
				"die gewählte Linie ein!";
			this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblLength
			// 
			this.lblLength.AutoSize = true;
			this.lblLength.Location = new System.Drawing.Point(12, 73);
			this.lblLength.Name = "lblLength";
			this.lblLength.Size = new System.Drawing.Size(81, 13);
			this.lblLength.TabIndex = 1;
			this.lblLength.Text = "Länge in Meter:";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(185, 96);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(104, 96);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// txtLength
			// 
			this.txtLength.EditType = Europlan.Common.NumericBox.NumericEditType.LENGTH;
			this.txtLength.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.txtLength.Location = new System.Drawing.Point(104, 70);
			this.txtLength.MaxValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.txtLength.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.txtLength.Name = "txtLength";
			this.txtLength.Size = new System.Drawing.Size(156, 20);
			this.txtLength.TabIndex = 1;
			this.txtLength.Text = "0,01";
			this.txtLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			// 
			// PlanSetMeasureForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(272, 131);
			this.Controls.Add(this.txtLength);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lblLength);
			this.Controls.Add(this.lblText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "PlanSetMeasureForm";
			this.Text = "Maßstab setzen";
			this.Load += new System.EventHandler(this.PlanSetMeasureForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlanSetMeasureForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblText;
		private System.Windows.Forms.Label lblLength;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private NumericBox txtLength;
	}
}