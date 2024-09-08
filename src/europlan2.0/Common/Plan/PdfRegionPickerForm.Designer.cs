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
            this.lblActualSize = new System.Windows.Forms.Label();
            this.picturePanel = new Europlan.Common.ImagePanel();
            this.pdfRegionPicker = new Europlan.Common.PdfRegionPicker(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ProgressLoad = new System.Windows.Forms.ProgressBar();
            this.FlowPanelPreview = new System.Windows.Forms.FlowLayoutPanel();
            this.BackgroundWorkerPdfLoading = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblLarge = new System.Windows.Forms.Label();
            this.lblMedium = new System.Windows.Forms.Label();
            this.lblSmall = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.sliderSize = new System.Windows.Forms.TrackBar();
            this.toolStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderSize)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
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
            this.toolStrip.Size = new System.Drawing.Size(622, 25);
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
            this.btnOk.Location = new System.Drawing.Point(524, 31);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(95, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Übernehmen";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblActualSize
            // 
            this.lblActualSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblActualSize.AutoSize = true;
            this.lblActualSize.Location = new System.Drawing.Point(276, 536);
            this.lblActualSize.Name = "lblActualSize";
            this.lblActualSize.Size = new System.Drawing.Size(0, 13);
            this.lblActualSize.TabIndex = 8;
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
            this.picturePanel.Length = 0D;
            this.picturePanel.Location = new System.Drawing.Point(0, 25);
            this.picturePanel.Margin = new System.Windows.Forms.Padding(0);
            this.picturePanel.Mode = Europlan.Common.PlanMode.PM_MOVE;
            this.picturePanel.Name = "picturePanel";
            this.picturePanel.Scale = null;
            this.picturePanel.ShowRaster = false;
            this.picturePanel.Size = new System.Drawing.Size(622, 491);
            this.picturePanel.StartPoint = null;
            this.picturePanel.TabIndex = 0;
            this.picturePanel.XPos = 0F;
            this.picturePanel.YPos = 0F;
            // 
            // pdfRegionPicker
            // 
            this.pdfRegionPicker.Mode = Europlan.Common.PdfRegionPicker.PdfRegionPickerMode.DPM_NONE;
            this.pdfRegionPicker.RegionPicked += new System.EventHandler(this.pdfRegionPicker_RegionPicked);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(892, 573);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // ProgressLoad
            // 
            this.ProgressLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressLoad.Location = new System.Drawing.Point(8, 549);
            this.ProgressLoad.Margin = new System.Windows.Forms.Padding(8);
            this.ProgressLoad.Name = "ProgressLoad";
            this.ProgressLoad.Size = new System.Drawing.Size(254, 16);
            this.ProgressLoad.TabIndex = 6;
            // 
            // FlowPanelPreview
            // 
            this.FlowPanelPreview.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.FlowPanelPreview.AutoScroll = true;
            this.FlowPanelPreview.AutoSize = true;
            this.FlowPanelPreview.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FlowPanelPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FlowPanelPreview.Location = new System.Drawing.Point(118, 8);
            this.FlowPanelPreview.Margin = new System.Windows.Forms.Padding(8);
            this.FlowPanelPreview.Name = "FlowPanelPreview";
            this.FlowPanelPreview.Padding = new System.Windows.Forms.Padding(8, 8, 24, 8);
            this.FlowPanelPreview.Size = new System.Drawing.Size(34, 18);
            this.FlowPanelPreview.TabIndex = 5;
            // 
            // BackgroundWorkerPdfLoading
            // 
            this.BackgroundWorkerPdfLoading.WorkerReportsProgress = true;
            this.BackgroundWorkerPdfLoading.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerPdfLoading_DoWork);
            this.BackgroundWorkerPdfLoading.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkerPdfLoading_ProgressChanged);
            this.BackgroundWorkerPdfLoading.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerPdfLoading_RunWorkerCompleted);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.FlowPanelPreview, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ProgressLoad, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(270, 573);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.picturePanel, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.toolStrip, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(270, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(622, 573);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnOk, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 516);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(622, 57);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblLarge);
            this.panel1.Controls.Add(this.lblMedium);
            this.panel1.Controls.Add(this.lblSmall);
            this.panel1.Controls.Add(this.lblSize);
            this.panel1.Controls.Add(this.sliderSize);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 57);
            this.panel1.TabIndex = 10;
            // 
            // lblLarge
            // 
            this.lblLarge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLarge.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLarge.Location = new System.Drawing.Point(214, 35);
            this.lblLarge.Name = "lblLarge";
            this.lblLarge.Size = new System.Drawing.Size(70, 13);
            this.lblLarge.TabIndex = 12;
            this.lblLarge.Text = "groß";
            this.lblLarge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMedium
            // 
            this.lblMedium.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMedium.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedium.Location = new System.Drawing.Point(129, 35);
            this.lblMedium.Name = "lblMedium";
            this.lblMedium.Size = new System.Drawing.Size(70, 13);
            this.lblMedium.TabIndex = 11;
            this.lblMedium.Text = "normal";
            this.lblMedium.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSmall
            // 
            this.lblSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmall.Location = new System.Drawing.Point(44, 35);
            this.lblSmall.Name = "lblSmall";
            this.lblSmall.Size = new System.Drawing.Size(70, 13);
            this.lblSmall.TabIndex = 10;
            this.lblSmall.Text = "klein";
            this.lblSmall.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSize
            // 
            this.lblSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(4, 13);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(54, 13);
            this.lblSize.TabIndex = 9;
            this.lblSize.Text = "Bildgröße:";
            // 
            // sliderSize
            // 
            this.sliderSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sliderSize.LargeChange = 2;
            this.sliderSize.Location = new System.Drawing.Point(65, 10);
            this.sliderSize.Maximum = 4;
            this.sliderSize.Name = "sliderSize";
            this.sliderSize.Size = new System.Drawing.Size(197, 42);
            this.sliderSize.TabIndex = 8;
            this.sliderSize.Value = 2;
            // 
            // PdfRegionPickerForm__new
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 573);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblActualSize);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "PdfRegionPickerForm__new";
            this.Text = "Raumtypen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.Label lblActualSize;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel FlowPanelPreview;
        private System.Windows.Forms.ProgressBar ProgressLoad;
        private System.ComponentModel.BackgroundWorker BackgroundWorkerPdfLoading;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblLarge;
        private System.Windows.Forms.Label lblMedium;
        private System.Windows.Forms.Label lblSmall;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.TrackBar sliderSize;
    }
}