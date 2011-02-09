namespace Europlan.Common {
	partial class PlanPanel {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.cadOptions = new System.Windows.Forms.Panel();
			this.imagePanel = new Europlan.Common.ImagePanel();
			this.cadPanel = new Europlan.Common.CadPanel();
			this.cadPanelOptions = new Europlan.Common.CadPanelOptions();
			this.cadOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// cadOptions
			// 
			this.cadOptions.Controls.Add(this.cadPanelOptions);
			this.cadOptions.Dock = System.Windows.Forms.DockStyle.Left;
			this.cadOptions.Location = new System.Drawing.Point(0, 0);
			this.cadOptions.Name = "cadOptions";
			this.cadOptions.Size = new System.Drawing.Size(200, 459);
			this.cadOptions.TabIndex = 0;
			this.cadOptions.Visible = false;
			// 
			// imagePanel
			// 
			this.imagePanel.Angle = 0F;
			this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imagePanel.EndPoint = null;
			this.imagePanel.Length = 0;
			this.imagePanel.Location = new System.Drawing.Point(200, 0);
			this.imagePanel.Mode = Europlan.Common.PlanMode.PM_MOVE;
			this.imagePanel.Name = "imagePanel";
			this.imagePanel.PlanCursor = System.Windows.Forms.Cursors.SizeAll;
			this.imagePanel.Scale = null;
			this.imagePanel.ShowRaster = false;
			this.imagePanel.Size = new System.Drawing.Size(561, 459);
			this.imagePanel.StartPoint = null;
			this.imagePanel.TabIndex = 2;
			this.imagePanel.XPos = 0F;
			this.imagePanel.YPos = 0F;
			this.imagePanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.imagePanel_KeyUp);
			this.imagePanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imagePanel_KeyDown);
			// 
			// cadPanel
			// 
			this.cadPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cadPanel.Location = new System.Drawing.Point(200, 0);
			this.cadPanel.Name = "cadPanel";
			this.cadPanel.PlanCursor = System.Windows.Forms.Cursors.SizeAll;
			this.cadPanel.Size = new System.Drawing.Size(561, 459);
			this.cadPanel.TabIndex = 1;
			this.cadPanel.Visible = false;
			this.cadPanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.imagePanel_KeyUp);
			this.cadPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imagePanel_KeyDown);
			// 
			// cadPanelOptions
			// 
			this.cadPanelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cadPanelOptions.Location = new System.Drawing.Point(0, 0);
			this.cadPanelOptions.Name = "cadPanelOptions";
			this.cadPanelOptions.Plan = null;
			this.cadPanelOptions.Size = new System.Drawing.Size(200, 459);
			this.cadPanelOptions.TabIndex = 1;
			this.cadPanelOptions.InvalidateNeeded += new System.EventHandler(this.cadPanelOptions_InvalidateNeeded);
			// 
			// PlanPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.imagePanel);
			this.Controls.Add(this.cadPanel);
			this.Controls.Add(this.cadOptions);
			this.Name = "PlanPanel";
			this.Size = new System.Drawing.Size(761, 459);
			this.cadOptions.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel cadOptions;
		private CadPanelOptions cadPanelOptions;
		private CadPanel cadPanel;
		private ImagePanel imagePanel;
	}
}
