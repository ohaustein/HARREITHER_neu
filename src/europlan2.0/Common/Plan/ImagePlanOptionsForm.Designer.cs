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
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnRotateLeft = new System.Windows.Forms.ToolStripButton();
			this.btnRotateRight = new System.Windows.Forms.ToolStripButton();
			this.btnRaster = new System.Windows.Forms.ToolStripButton();
			this.btnRotateLeftSmall = new System.Windows.Forms.ToolStripButton();
			this.btnRotateRightSmall = new System.Windows.Forms.ToolStripButton();
			this.picturePanel = new Europlan.Common.PicturePanel();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRotateLeft,
            this.btnRotateLeftSmall,
            this.btnRotateRightSmall,
            this.btnRotateRight,
            this.btnZoomIn,
            this.btnZoomOut,
            this.btnRaster});
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
			// btnRaster
			// 
			this.btnRaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnRaster.Image = ((System.Drawing.Image)(resources.GetObject("btnRaster.Image")));
			this.btnRaster.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRaster.Name = "btnRaster";
			this.btnRaster.Size = new System.Drawing.Size(64, 22);
			this.btnRaster.Text = "Raster aus";
			this.btnRaster.Click += new System.EventHandler(this.btnRaster_Click);
			// 
			// btnRotateLeftSmall
			// 
			this.btnRotateLeftSmall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRotateLeftSmall.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeftSmall.Image")));
			this.btnRotateLeftSmall.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRotateLeftSmall.Name = "btnRotateLeftSmall";
			this.btnRotateLeftSmall.Size = new System.Drawing.Size(23, 22);
			this.btnRotateLeftSmall.Text = "btnRotateLeft";
			this.btnRotateLeftSmall.Click += new System.EventHandler(this.btnRotateLeftSmall_Click);
			// 
			// btnRotateRightSmall
			// 
			this.btnRotateRightSmall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRotateRightSmall.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateRightSmall.Image")));
			this.btnRotateRightSmall.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRotateRightSmall.Name = "btnRotateRightSmall";
			this.btnRotateRightSmall.Size = new System.Drawing.Size(23, 22);
			this.btnRotateRightSmall.Text = "toolStripButton2";
			this.btnRotateRightSmall.Click += new System.EventHandler(this.btnRotateRightSmall_Click);
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
			this.picturePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturePanel_MouseMove);
			this.picturePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturePanel_MouseDown);
			this.picturePanel.Resize += new System.EventHandler(this.picturePanel_Resize);
			this.picturePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picturePanel_MouseUp);
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
			this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
			this.btnZoomIn.Text = "toolStripButton1";
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
			this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
			this.btnZoomOut.Text = "toolStripButton1";
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// ImagePlanOptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(668, 431);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.picturePanel);
			this.DoubleBuffered = true;
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

		private PicturePanel picturePanel;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnRotateLeft;
		private System.Windows.Forms.ToolStripButton btnRotateRight;
		private System.Windows.Forms.ToolStripButton btnRaster;
		private System.Windows.Forms.ToolStripButton btnRotateLeftSmall;
		private System.Windows.Forms.ToolStripButton btnRotateRightSmall;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;


	}
}