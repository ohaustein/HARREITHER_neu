namespace Europlan.Common {
	partial class ImagePlanOptionsForm {
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
			this.picturePanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// picturePanel
			// 
			this.picturePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.picturePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picturePanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picturePanel.Location = new System.Drawing.Point(13, 72);
			this.picturePanel.Name = "picturePanel";
			this.picturePanel.Size = new System.Drawing.Size(643, 347);
			this.picturePanel.TabIndex = 0;
			this.picturePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.picturePanel_Paint);
			// 
			// ImagePlanOptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(668, 431);
			this.Controls.Add(this.picturePanel);
			this.MinimizeBox = false;
			this.Name = "ImagePlanOptionsForm";
			this.Text = "Raumtypen";
			this.Load += new System.EventHandler(this.ImagePlanOptionsForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImagePlanOptionsForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel picturePanel;


	}
}