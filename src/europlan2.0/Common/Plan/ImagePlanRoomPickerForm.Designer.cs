namespace Europlan.Common {
	partial class ImagePlanRoomPickerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImagePlanRoomPickerForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.btnPick = new System.Windows.Forms.ToolStripButton();
			this.btnUnused = new System.Windows.Forms.ToolStripButton();
			this.picturePanel = new Europlan.Common.ImagePanel();
			this.btnDeleteUnused = new System.Windows.Forms.ToolStripButton();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomOut,
            this.btnZoomIn,
            this.toolStripSeparator2,
            this.btnMove,
            this.btnPick,
            this.btnUnused,
            this.btnDeleteUnused});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(668, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip";
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.AutoToolTip = false;
			this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
			this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
			this.btnZoomOut.Text = "toolStripButton1";
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.AutoToolTip = false;
			this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
			this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
			this.btnZoomIn.Text = "toolStripButton1";
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnMove
			// 
			this.btnMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnMove.Image = ((System.Drawing.Image)(resources.GetObject("btnMove.Image")));
			this.btnMove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnMove.Name = "btnMove";
			this.btnMove.Size = new System.Drawing.Size(23, 22);
			this.btnMove.Text = "toolStripButton1";
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			// 
			// btnPick
			// 
			this.btnPick.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPick.Image = ((System.Drawing.Image)(resources.GetObject("btnPick.Image")));
			this.btnPick.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPick.Name = "btnPick";
			this.btnPick.Size = new System.Drawing.Size(23, 22);
			this.btnPick.Text = "toolStripButton1";
			this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
			// 
			// btnUnused
			// 
			this.btnUnused.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnUnused.Image = ((System.Drawing.Image)(resources.GetObject("btnUnused.Image")));
			this.btnUnused.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnUnused.Name = "btnUnused";
			this.btnUnused.Size = new System.Drawing.Size(23, 22);
			this.btnUnused.Text = "toolStripButton1";
			this.btnUnused.Click += new System.EventHandler(this.btnUnused_Click);
			// 
			// picturePanel
			// 
			this.picturePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.picturePanel.Angle = 0F;
			this.picturePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.picturePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picturePanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picturePanel.EndPoint = null;
			this.picturePanel.Length = 0;
			this.picturePanel.Location = new System.Drawing.Point(9, 25);
			this.picturePanel.Margin = new System.Windows.Forms.Padding(0);
			this.picturePanel.Mode = PlanMode.PM_MOVE;
			this.picturePanel.Name = "picturePanel";
			this.picturePanel.Plan = null;
			this.picturePanel.RoomCoordinates = ((System.Collections.Generic.List<System.Drawing.PointF>)(resources.GetObject("picturePanel.RoomCoordinates")));
			this.picturePanel.Scale = null;
			this.picturePanel.ShowRaster = false;
			this.picturePanel.Size = new System.Drawing.Size(650, 397);
			this.picturePanel.StartPoint = null;
			this.picturePanel.TabIndex = 0;
			this.picturePanel.XPos = 0F;
			this.picturePanel.YPos = 0F;
			// 
			// btnDeleteUnused
			// 
			this.btnDeleteUnused.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDeleteUnused.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteUnused.Image")));
			this.btnDeleteUnused.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDeleteUnused.Name = "btnDeleteUnused";
			this.btnDeleteUnused.Size = new System.Drawing.Size(23, 22);
			this.btnDeleteUnused.Text = "toolStripButton1";
			this.btnDeleteUnused.Click += new System.EventHandler(this.btnDeleteUnused_Click);
			// 
			// ImagePlanRoomPickerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(668, 431);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.picturePanel);
			this.DoubleBuffered = true;
			this.MinimizeBox = false;
			this.Name = "ImagePlanRoomPickerForm";
			this.Text = "Raumtypen";
			this.Load += new System.EventHandler(this.ImagePlanRoomPickerForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImagePlanRoomPickerForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ImagePanel picturePanel;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.ToolStripButton btnPick;
		private System.Windows.Forms.ToolStripButton btnUnused;
		private System.Windows.Forms.ToolStripButton btnDeleteUnused;


	}
}