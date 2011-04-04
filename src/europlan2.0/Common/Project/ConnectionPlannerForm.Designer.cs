namespace Europlan.Common {
	partial class ConnectionPlannerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionPlannerForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.btnConnections = new System.Windows.Forms.ToolStripButton();
			this.btnDeleteConnection = new System.Windows.Forms.ToolStripButton();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.planPanel = new Europlan.Common.PlanPanel();
			this.connectionPlanner1 = new Europlan.Common.ConnectionPlanner(this.components);
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomOut,
            this.btnZoomIn,
            this.toolStripSeparator1,
            this.btnMove,
            this.btnConnections,
            this.btnDeleteConnection});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(892, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
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
			this.btnMove.Text = "Plan verschieben";
			this.btnMove.ToolTipText = "Plan verschieben";
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			// 
			// btnConnections
			// 
			this.btnConnections.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnConnections.Image = ((System.Drawing.Image)(resources.GetObject("btnConnections.Image")));
			this.btnConnections.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnConnections.Name = "btnConnections";
			this.btnConnections.Size = new System.Drawing.Size(23, 22);
			this.btnConnections.Text = "Verbindeleitung hinzufügen";
			this.btnConnections.Click += new System.EventHandler(this.btnConnections_Click);
			// 
			// btnDeleteConnection
			// 
			this.btnDeleteConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDeleteConnection.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteConnection.Image")));
			this.btnDeleteConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDeleteConnection.Name = "btnDeleteConnection";
			this.btnDeleteConnection.Size = new System.Drawing.Size(23, 22);
			this.btnDeleteConnection.Text = "Verbindeleitung löschen";
			this.btnDeleteConnection.Click += new System.EventHandler(this.btnDeleteConnection_Click);
			// 
			// planPanel
			// 
			this.planPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planPanel.Location = new System.Drawing.Point(0, 25);
			this.planPanel.Name = "planPanel";
			this.planPanel.PlanCursor = System.Windows.Forms.Cursors.SizeAll;
			this.planPanel.ProductPlanner = this.connectionPlanner1;
			this.planPanel.Size = new System.Drawing.Size(892, 440);
			this.planPanel.TabIndex = 0;
			// 
			// connectionPlanner1
			// 
			this.connectionPlanner1.Mode = Europlan.Common.ConnectionPlanner.ConnectionMode.CM_NONE;
			// 
			// ConnectionPlannerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 465);
			this.Controls.Add(this.planPanel);
			this.Controls.Add(this.toolStrip);
			this.Name = "ConnectionPlannerForm";
			this.Text = "Modul Klima-Boden - grafische Auslegung";
			this.Load += new System.EventHandler(this.ModulKlimaBodenPlannerForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModulKlimaBodenPlannerForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private PlanPanel planPanel;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.ToolStripButton btnConnections;
		private System.Windows.Forms.ToolStripButton btnDeleteConnection;
		private ConnectionPlanner connectionPlanner1;
	}
}