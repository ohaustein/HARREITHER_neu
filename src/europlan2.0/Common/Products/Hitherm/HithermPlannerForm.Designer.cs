namespace Europlan.Common {
	partial class HithermPlannerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HithermPlannerForm));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.btnPick = new System.Windows.Forms.ToolStripButton();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.btnWall = new System.Windows.Forms.ToolStripButton();
			this.btnObstacle = new System.Windows.Forms.ToolStripButton();
			this.panelTop = new System.Windows.Forms.Panel();
			this.btnCreateWalls = new System.Windows.Forms.Button();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.panelDefineWalls = new System.Windows.Forms.Panel();
			this.graphicalWallPanel = new Europlan.Common.GraphicalWallPanel();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.panelTop.SuspendLayout();
			this.panelDefineWalls.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomOut,
            this.btnZoomIn});
			this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(886, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
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
			// 
			// toolStrip2
			// 
			this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPick,
            this.btnMove,
            this.btnWall,
            this.btnObstacle});
			this.toolStrip2.Location = new System.Drawing.Point(0, 25);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(24, 461);
			this.toolStrip2.TabIndex = 1;
			this.toolStrip2.Text = "toolStrip2";
			this.toolStrip2.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical270;
			// 
			// btnPick
			// 
			this.btnPick.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnPick.Image = ((System.Drawing.Image)(resources.GetObject("btnPick.Image")));
			this.btnPick.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPick.Name = "btnPick";
			this.btnPick.Size = new System.Drawing.Size(21, 33);
			this.btnPick.Text = "pick";
			// 
			// btnMove
			// 
			this.btnMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnMove.Image = ((System.Drawing.Image)(resources.GetObject("btnMove.Image")));
			this.btnMove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnMove.Name = "btnMove";
			this.btnMove.Size = new System.Drawing.Size(21, 41);
			this.btnMove.Text = "move";
			// 
			// btnWall
			// 
			this.btnWall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnWall.Image = ((System.Drawing.Image)(resources.GetObject("btnWall.Image")));
			this.btnWall.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnWall.Name = "btnWall";
			this.btnWall.Size = new System.Drawing.Size(21, 32);
			this.btnWall.Text = "wall";
			// 
			// btnObstacle
			// 
			this.btnObstacle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnObstacle.Image = ((System.Drawing.Image)(resources.GetObject("btnObstacle.Image")));
			this.btnObstacle.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnObstacle.Name = "btnObstacle";
			this.btnObstacle.Size = new System.Drawing.Size(21, 55);
			this.btnObstacle.Text = "obstacle";
			// 
			// panelTop
			// 
			this.panelTop.Controls.Add(this.panelDefineWalls);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTop.Location = new System.Drawing.Point(24, 25);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(862, 76);
			this.panelTop.TabIndex = 2;
			// 
			// btnCreateWalls
			// 
			this.btnCreateWalls.Location = new System.Drawing.Point(3, 3);
			this.btnCreateWalls.Name = "btnCreateWalls";
			this.btnCreateWalls.Size = new System.Drawing.Size(205, 41);
			this.btnCreateWalls.TabIndex = 0;
			this.btnCreateWalls.Text = "Wände aus Raumgeometrie erzeugen";
			this.btnCreateWalls.UseVisualStyleBackColor = true;
			this.btnCreateWalls.Click += new System.EventHandler(this.btnCreateWalls_Click);
			// 
			// panelBottom
			// 
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(24, 398);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(862, 88);
			this.panelBottom.TabIndex = 3;
			// 
			// panelDefineWalls
			// 
			this.panelDefineWalls.Controls.Add(this.btnCreateWalls);
			this.panelDefineWalls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelDefineWalls.Location = new System.Drawing.Point(0, 0);
			this.panelDefineWalls.Name = "panelDefineWalls";
			this.panelDefineWalls.Size = new System.Drawing.Size(862, 76);
			this.panelDefineWalls.TabIndex = 1;
			// 
			// graphicalWallPanel
			// 
			this.graphicalWallPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphicalWallPanel.Location = new System.Drawing.Point(24, 101);
			this.graphicalWallPanel.Name = "graphicalWallPanel";
			this.graphicalWallPanel.ProductPlanner = null;
			this.graphicalWallPanel.Room = null;
			this.graphicalWallPanel.Scale = 1;
			this.graphicalWallPanel.Size = new System.Drawing.Size(862, 297);
			this.graphicalWallPanel.TabIndex = 4;
			this.graphicalWallPanel.XPos = 0;
			this.graphicalWallPanel.YPos = 277;
			// 
			// HithermPlannerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(886, 486);
			this.Controls.Add(this.graphicalWallPanel);
			this.Controls.Add(this.panelBottom);
			this.Controls.Add(this.panelTop);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip1);
			this.Name = "HithermPlannerForm";
			this.Text = "HithermPlannerForm";
			this.Load += new System.EventHandler(this.HithermPlannerForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HithermPlannerForm_FormClosing);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.panelTop.ResumeLayout(false);
			this.panelDefineWalls.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton btnPick;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.ToolStripButton btnWall;
		private System.Windows.Forms.ToolStripButton btnObstacle;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Button btnCreateWalls;
		private System.Windows.Forms.Panel panelBottom;
		private GraphicalWallPanel graphicalWallPanel;
		private System.Windows.Forms.Panel panelDefineWalls;


	}
}