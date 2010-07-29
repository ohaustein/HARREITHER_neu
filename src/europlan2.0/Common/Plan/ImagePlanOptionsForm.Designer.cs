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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImagePlanOptionsForm));
			this.picturePanel = new System.Windows.Forms.Panel();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnRotateLeft = new System.Windows.Forms.ToolStripButton();
			this.btnRotateRight = new System.Windows.Forms.ToolStripButton();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// picturePanel
			// 
			this.picturePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.picturePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picturePanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picturePanel.Location = new System.Drawing.Point(9, 25);
			this.picturePanel.Margin = new System.Windows.Forms.Padding(0);
			this.picturePanel.Name = "picturePanel";
			this.picturePanel.Size = new System.Drawing.Size(650, 397);
			this.picturePanel.TabIndex = 0;
			this.picturePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.picturePanel_Paint);
			this.picturePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturePanel_MouseDown);
			this.picturePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picturePanel_MouseUp);
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRotateLeft,
            this.btnRotateRight});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(668, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip";
			// 
			// btnRotateLeft
			// 
			this.btnRotateLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeft.Image")));
			this.btnRotateLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRotateLeft.Name = "btnRotateLeft";
			this.btnRotateLeft.Size = new System.Drawing.Size(23, 22);
			this.btnRotateLeft.Text = "btnRotateLeft";
			this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
			// 
			// btnRotateRight
			// 
			this.btnRotateRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRotateRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateRight.Image")));
			this.btnRotateRight.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRotateRight.Name = "btnRotateRight";
			this.btnRotateRight.Size = new System.Drawing.Size(23, 22);
			this.btnRotateRight.Text = "toolStripButton2";
			this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click);
			// 
			// ImagePlanOptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(668, 431);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.picturePanel);
			this.MinimizeBox = false;
			this.Name = "ImagePlanOptionsForm";
			this.Text = "Raumtypen";
			this.Load += new System.EventHandler(this.ImagePlanOptionsForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImagePlanOptionsForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel picturePanel;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnRotateLeft;
		private System.Windows.Forms.ToolStripButton btnRotateRight;


	}
}