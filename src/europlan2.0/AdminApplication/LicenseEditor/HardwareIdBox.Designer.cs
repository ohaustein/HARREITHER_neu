namespace Europlan.AdminApplication {
	partial class HardwareIdBox {
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
			this.txtHardwareId = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtHardwareId
			// 
			this.txtHardwareId.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtHardwareId.Location = new System.Drawing.Point(0, 0);
			this.txtHardwareId.Name = "txtHardwareId";
			this.txtHardwareId.Size = new System.Drawing.Size(349, 20);
			this.txtHardwareId.TabIndex = 0;
			this.txtHardwareId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHardwareId_KeyDown);
			this.txtHardwareId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHardwareId_KeyPress);
			// 
			// HardwareIdBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtHardwareId);
			this.Name = "HardwareIdBox";
			this.Size = new System.Drawing.Size(349, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtHardwareId;
	}
}
