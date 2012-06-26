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
            this.btnBoden = new System.Windows.Forms.ToolStripButton();
            this.btnDecke = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFirstCircuit = new System.Windows.Forms.ToolStripButton();
            this.btnOtherCircuits = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMove = new System.Windows.Forms.ToolStripButton();
            this.btnConnections = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteConnection = new System.Windows.Forms.ToolStripButton();
            this.btnPickConnection = new System.Windows.Forms.ToolStripButton();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.planPanel = new Europlan.Common.PlanPanel();
            this.connectionPlanner = new Europlan.Common.ConnectionPlanner(this.components);
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnShowPlanBg = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBoden,
            this.btnDecke,
            this.toolStripSeparator2,
            this.btnZoomOut,
            this.btnZoomIn,
            this.toolStripSeparator1,
            this.btnFirstCircuit,
            this.btnOtherCircuits,
            this.toolStripSeparator3,
            this.btnMove,
            this.btnConnections,
            this.btnDeleteConnection,
            this.btnPickConnection,
            this.toolStripSeparator4,
            this.btnShowPlanBg});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(892, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnBoden
            // 
            this.btnBoden.Checked = true;
            this.btnBoden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnBoden.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBoden.Image = ((System.Drawing.Image)(resources.GetObject("btnBoden.Image")));
            this.btnBoden.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBoden.Name = "btnBoden";
            this.btnBoden.Size = new System.Drawing.Size(45, 22);
            this.btnBoden.Text = "Boden";
            this.btnBoden.Click += new System.EventHandler(this.btnBoden_Click);
            // 
            // btnDecke
            // 
            this.btnDecke.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDecke.Image = ((System.Drawing.Image)(resources.GetObject("btnDecke.Image")));
            this.btnDecke.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDecke.Name = "btnDecke";
            this.btnDecke.Size = new System.Drawing.Size(43, 22);
            this.btnDecke.Text = "Decke";
            this.btnDecke.Click += new System.EventHandler(this.btnDecke_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            this.btnZoomIn.Text = "Heranzoomen";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnFirstCircuit
            // 
            this.btnFirstCircuit.Checked = true;
            this.btnFirstCircuit.CheckOnClick = true;
            this.btnFirstCircuit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnFirstCircuit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFirstCircuit.Image = ((System.Drawing.Image)(resources.GetObject("btnFirstCircuit.Image")));
            this.btnFirstCircuit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirstCircuit.Name = "btnFirstCircuit";
            this.btnFirstCircuit.Size = new System.Drawing.Size(23, 22);
            this.btnFirstCircuit.Text = "erster Heizkreis";
            this.btnFirstCircuit.Visible = false;
            this.btnFirstCircuit.Click += new System.EventHandler(this.btnCircuits_Click);
            // 
            // btnOtherCircuits
            // 
            this.btnOtherCircuits.Checked = true;
            this.btnOtherCircuits.CheckOnClick = true;
            this.btnOtherCircuits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnOtherCircuits.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOtherCircuits.Image = ((System.Drawing.Image)(resources.GetObject("btnOtherCircuits.Image")));
            this.btnOtherCircuits.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOtherCircuits.Name = "btnOtherCircuits";
            this.btnOtherCircuits.Size = new System.Drawing.Size(23, 22);
            this.btnOtherCircuits.Text = "restliche Heizkreise";
            this.btnOtherCircuits.Visible = false;
            this.btnOtherCircuits.Click += new System.EventHandler(this.btnCircuits_Click);
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
            // btnConnections
            // 
            this.btnConnections.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnections.Image = ((System.Drawing.Image)(resources.GetObject("btnConnections.Image")));
            this.btnConnections.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnections.Name = "btnConnections";
            this.btnConnections.Size = new System.Drawing.Size(23, 22);
            this.btnConnections.Text = "Anbindeleitung hinzufügen";
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
            this.btnDeleteConnection.Visible = false;
            this.btnDeleteConnection.Click += new System.EventHandler(this.btnDeleteConnection_Click);
            // 
            // btnPickConnection
            // 
            this.btnPickConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPickConnection.Image = ((System.Drawing.Image)(resources.GetObject("btnPickConnection.Image")));
            this.btnPickConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPickConnection.Name = "btnPickConnection";
            this.btnPickConnection.Size = new System.Drawing.Size(23, 22);
            this.btnPickConnection.Text = "Anbindeleitungen ändern";
            this.btnPickConnection.Click += new System.EventHandler(this.btnPickConnection_Click);
            // 
            // planPanel
            // 
            this.planPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.planPanel.Location = new System.Drawing.Point(0, 25);
            this.planPanel.Name = "planPanel";
            this.planPanel.ProductPlanner = this.connectionPlanner;
            this.planPanel.Size = new System.Drawing.Size(892, 440);
            this.planPanel.TabIndex = 0;
            // 
            // connectionPlanner
            // 
            this.connectionPlanner.AddFirstCircuit = true;
            this.connectionPlanner.AddOtherCircuits = true;
            this.connectionPlanner.AddRuecklauf = true;
            this.connectionPlanner.AddVorlauf = true;
            this.connectionPlanner.Mode = Europlan.Common.ConnectionPlanner.ConnectionMode.CM_NONE;
            this.connectionPlanner.PlanCeiling = false;
            this.connectionPlanner.PlanFloor = true;
            this.connectionPlanner.Product = null;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnShowPlanBg
            // 
            this.btnShowPlanBg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowPlanBg.Image = ((System.Drawing.Image)(resources.GetObject("btnShowPlanBg.Image")));
            this.btnShowPlanBg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowPlanBg.Name = "btnShowPlanBg";
            this.btnShowPlanBg.Size = new System.Drawing.Size(23, 22);
            this.btnShowPlanBg.Click += new System.EventHandler(this.btnShowPlanBg_Click);
            // 
            // ConnectionPlannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 465);
            this.Controls.Add(this.planPanel);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
		private ConnectionPlanner connectionPlanner;
		private System.Windows.Forms.ToolStripButton btnBoden;
		private System.Windows.Forms.ToolStripButton btnDecke;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnPickConnection;
		private System.Windows.Forms.ToolStripButton btnFirstCircuit;
		private System.Windows.Forms.ToolStripButton btnOtherCircuits;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnShowPlanBg;
	}
}