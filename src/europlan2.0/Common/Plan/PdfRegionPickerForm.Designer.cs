namespace Europlan.Common {
	partial class PdfRegionPickerForm {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PdfRegionPickerForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnSelectRegion = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.btnOk = new System.Windows.Forms.Button();
			this.sliderSize = new System.Windows.Forms.TrackBar();
			this.lblSize = new System.Windows.Forms.Label();
			this.lblSmall = new System.Windows.Forms.Label();
			this.lblMedium = new System.Windows.Forms.Label();
			this.lblLarge = new System.Windows.Forms.Label();
			this.picturePanel = new Europlan.Common.ImagePanel();
			this.pdfRegionPicker = new Europlan.Common.PdfRegionPicker(this.components);
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sliderSize)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomIn,
            this.btnZoomOut,
            this.toolStripSeparator2,
            this.btnSelectRegion,
            this.toolStripSeparator3,
            this.btnMove});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(668, 25);
			this.toolStrip.TabIndex = 2;
			this.toolStrip.Text = "toolStrip";
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.AutoToolTip = false;
			this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
			this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
			this.btnZoomIn.Text = "Heranzoomen";
			this.btnZoomIn.ToolTipText = "Heranzoomen";
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.AutoToolTip = false;
			this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
			this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
			this.btnZoomOut.Text = "Herauszoomen";
			this.btnZoomOut.ToolTipText = "Herauszoomen";
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnSelectRegion
			// 
			this.btnSelectRegion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSelectRegion.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectRegion.Image")));
			this.btnSelectRegion.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSelectRegion.Name = "btnSelectRegion";
			this.btnSelectRegion.Size = new System.Drawing.Size(23, 22);
			this.btnSelectRegion.Click += new System.EventHandler(this.btnSelectRegion_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// btnMove
			// 
			this.btnMove.AutoToolTip = false;
			this.btnMove.Checked = true;
			this.btnMove.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnMove.Image = ((System.Drawing.Image)(resources.GetObject("btnMove.Image")));
			this.btnMove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnMove.Name = "btnMove";
			this.btnMove.Size = new System.Drawing.Size(23, 22);
			this.btnMove.Text = "Plan verschieben";
			this.btnMove.ToolTipText = "Plan verschieben";
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(568, 428);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(95, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Übernehmen";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// sliderSize
			// 
			this.sliderSize.LargeChange = 2;
			this.sliderSize.Location = new System.Drawing.Point(73, 415);
			this.sliderSize.Maximum = 4;
			this.sliderSize.Name = "sliderSize";
			this.sliderSize.Size = new System.Drawing.Size(197, 45);
			this.sliderSize.TabIndex = 3;
			this.sliderSize.Value = 2;
			this.sliderSize.Visible = false;
			// 
			// lblSize
			// 
			this.lblSize.AutoSize = true;
			this.lblSize.Location = new System.Drawing.Point(12, 418);
			this.lblSize.Name = "lblSize";
			this.lblSize.Size = new System.Drawing.Size(54, 13);
			this.lblSize.TabIndex = 4;
			this.lblSize.Text = "Bildgröße:";
			this.lblSize.Visible = false;
			// 
			// lblSmall
			// 
			this.lblSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSmall.Location = new System.Drawing.Point(52, 440);
			this.lblSmall.Name = "lblSmall";
			this.lblSmall.Size = new System.Drawing.Size(70, 13);
			this.lblSmall.TabIndex = 5;
			this.lblSmall.Text = "klein";
			this.lblSmall.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblSmall.Visible = false;
			// 
			// lblMedium
			// 
			this.lblMedium.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMedium.Location = new System.Drawing.Point(137, 440);
			this.lblMedium.Name = "lblMedium";
			this.lblMedium.Size = new System.Drawing.Size(70, 13);
			this.lblMedium.TabIndex = 6;
			this.lblMedium.Text = "normal";
			this.lblMedium.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblMedium.Visible = false;
			// 
			// lblLarge
			// 
			this.lblLarge.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLarge.Location = new System.Drawing.Point(222, 440);
			this.lblLarge.Name = "lblLarge";
			this.lblLarge.Size = new System.Drawing.Size(70, 13);
			this.lblLarge.TabIndex = 7;
			this.lblLarge.Text = "groß";
			this.lblLarge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblLarge.Visible = false;
			// 
			// picturePanel
			// 
			this.picturePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.picturePanel.Angle = 0F;
			this.picturePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.picturePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picturePanel.EndPoint = null;
			this.picturePanel.Length = 0;
			this.picturePanel.Location = new System.Drawing.Point(-5, 25);
			this.picturePanel.Margin = new System.Windows.Forms.Padding(0);
			this.picturePanel.Mode = Europlan.Common.PlanMode.PM_MOVE;
			this.picturePanel.Name = "picturePanel";
			this.picturePanel.Scale = null;
			this.picturePanel.ShowRaster = false;
			this.picturePanel.Size = new System.Drawing.Size(668, 390);
			this.picturePanel.StartPoint = null;
			this.picturePanel.TabIndex = 0;
			this.picturePanel.XPos = 0F;
			this.picturePanel.YPos = 0F;
			// 
			// pdfRegionPicker
			// 
			this.pdfRegionPicker.Mode = Europlan.Common.PdfRegionPicker.PdfRegionPickerMode.DPM_NONE;
			// 
			// PdfRegionPickerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(668, 455);
			this.Controls.Add(this.lblLarge);
			this.Controls.Add(this.lblMedium);
			this.Controls.Add(this.lblSmall);
			this.Controls.Add(this.lblSize);
			this.Controls.Add(this.sliderSize);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.picturePanel);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "PdfRegionPickerForm";
			this.Text = "Raumtypen";
			this.Load += new System.EventHandler(this.ImagePlanOptionsForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImagePlanOptionsForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sliderSize)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ImagePanel picturePanel;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.ToolStripButton btnSelectRegion;
		private PdfRegionPicker pdfRegionPicker;
		private System.Windows.Forms.TrackBar sliderSize;
		private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.Label lblSmall;
		private System.Windows.Forms.Label lblMedium;
		private System.Windows.Forms.Label lblLarge;


	}
}