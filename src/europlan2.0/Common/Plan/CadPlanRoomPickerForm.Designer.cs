namespace Europlan.Common {
	partial class CadPlanRoomPickerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CadPlanRoomPickerForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lstLayers = new System.Windows.Forms.ListView();
			this.cadPanel = new Europlan.Common.CadPanel();
			this.btnPick = new System.Windows.Forms.ToolStripButton();
			this.toolStrip.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomOut,
            this.btnZoomIn,
            this.toolStripSeparator1,
            this.btnMove,
            this.btnPick});
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
			this.btnZoomOut.Text = "zoomOut";
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
			this.btnZoomIn.Text = "zoomIn";
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
			this.btnMove.Text = "toolStripButton1";
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lstLayers);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(200, 406);
			this.panel1.TabIndex = 3;
			// 
			// lstLayers
			// 
			this.lstLayers.CheckBoxes = true;
			this.lstLayers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstLayers.Location = new System.Drawing.Point(0, 0);
			this.lstLayers.Name = "lstLayers";
			this.lstLayers.Size = new System.Drawing.Size(200, 406);
			this.lstLayers.TabIndex = 0;
			this.lstLayers.UseCompatibleStateImageBehavior = false;
			this.lstLayers.View = System.Windows.Forms.View.List;
			// 
			// cadPanel
			// 
			this.cadPanel.BackColor = System.Drawing.Color.White;
			this.cadPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.cadPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cadPanel.Location = new System.Drawing.Point(200, 25);
			this.cadPanel.Model = null;
			this.cadPanel.MoveMode = true;
			this.cadPanel.Name = "cadPanel";
			this.cadPanel.PlanDefaultMargin = 5;
			this.cadPanel.PlanScale = 1;
			this.cadPanel.PlanTranslation = ((WW.Math.Vector2D)(resources.GetObject("cadPanel.PlanTranslation")));
			this.cadPanel.Size = new System.Drawing.Size(468, 406);
			this.cadPanel.TabIndex = 2;
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
			// CadPlanRoomPickerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(668, 431);
			this.Controls.Add(this.cadPanel);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip);
			this.DoubleBuffered = true;
			this.MinimizeBox = false;
			this.Name = "CadPlanRoomPickerForm";
			this.Text = "Raumtypen";
			this.Load += new System.EventHandler(this.ImagePlanOptionsForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImagePlanOptionsForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripButton btnMove;
		private CadPanel cadPanel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListView lstLayers;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnPick;


	}
}