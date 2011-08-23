namespace Europlan.Application {
	partial class ValidityWarningForm {
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
			this.btnOk = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.chkDontShowAgain = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(183, 80);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.Location = new System.Drawing.Point(12, 9);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(422, 38);
			this.lblMessage.TabIndex = 1;
			this.lblMessage.Text = "label1";
			// 
			// chkDontShowAgain
			// 
			this.chkDontShowAgain.Location = new System.Drawing.Point(12, 50);
			this.chkDontShowAgain.Name = "chkDontShowAgain";
			this.chkDontShowAgain.Size = new System.Drawing.Size(422, 24);
			this.chkDontShowAgain.TabIndex = 2;
			this.chkDontShowAgain.Text = "checkBox1";
			this.chkDontShowAgain.UseVisualStyleBackColor = true;
			// 
			// ValidityWarningForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(446, 115);
			this.Controls.Add(this.chkDontShowAgain);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ValidityWarningForm";
			this.Text = "ValidityWarningForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.CheckBox chkDontShowAgain;
	}
}