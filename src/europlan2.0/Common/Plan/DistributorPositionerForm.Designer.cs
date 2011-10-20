namespace Europlan.Common {
	partial class DistributorPositionerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DistributorPositionerForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.btnPosition = new System.Windows.Forms.ToolStripButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblRotationUnit = new System.Windows.Forms.Label();
			this.numRotation = new Europlan.Common.NumericBox();
			this.lblRotation = new System.Windows.Forms.Label();
			this.btnCwLarge = new System.Windows.Forms.Button();
			this.btnCwSmall = new System.Windows.Forms.Button();
			this.btnCcwSmall = new System.Windows.Forms.Button();
			this.btnCcwLarge = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.panel = new Europlan.Common.PlanPanel();
			this.distributorPositioner = new Europlan.Common.DistributorPositioner(this.components);
			this.toolStrip.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomIn,
            this.btnZoomOut,
            this.toolStripSeparator1,
            this.btnMove,
            this.btnPosition});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(679, 25);
			this.toolStrip.TabIndex = 7;
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
			this.btnZoomIn.Text = "zoomIn";
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
			this.btnZoomOut.Text = "zoomOut";
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
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
			// btnPosition
			// 
			this.btnPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPosition.Image = ((System.Drawing.Image)(resources.GetObject("btnPosition.Image")));
			this.btnPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPosition.Name = "btnPosition";
			this.btnPosition.Size = new System.Drawing.Size(23, 22);
			this.btnPosition.Text = "Verteiler positionieren";
			this.btnPosition.Click += new System.EventHandler(this.btnPosition_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblRotationUnit);
			this.panel1.Controls.Add(this.numRotation);
			this.panel1.Controls.Add(this.lblRotation);
			this.panel1.Controls.Add(this.btnCwLarge);
			this.panel1.Controls.Add(this.btnCwSmall);
			this.panel1.Controls.Add(this.btnCcwSmall);
			this.panel1.Controls.Add(this.btnCcwLarge);
			this.panel1.Controls.Add(this.btnOk);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 433);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(679, 29);
			this.panel1.TabIndex = 4;
			// 
			// lblRotationUnit
			// 
			this.lblRotationUnit.AutoSize = true;
			this.lblRotationUnit.Location = new System.Drawing.Point(293, 8);
			this.lblRotationUnit.Name = "lblRotationUnit";
			this.lblRotationUnit.Size = new System.Drawing.Size(11, 13);
			this.lblRotationUnit.TabIndex = 20;
			this.lblRotationUnit.Text = "°";
			// 
			// numRotation
			// 
			this.numRotation.EditType = Europlan.Common.NumericBox.NumericEditType.ROTATION_360;
			this.numRotation.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numRotation.Location = new System.Drawing.Point(213, 5);
			this.numRotation.MaxValue = new decimal(new int[] {
            3599,
            0,
            0,
            65536});
			this.numRotation.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numRotation.Name = "numRotation";
			this.numRotation.Size = new System.Drawing.Size(74, 20);
			this.numRotation.TabIndex = 5;
			this.numRotation.Text = "0";
			this.numRotation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numRotation.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numRotation.ValueChanged += new System.EventHandler(this.numRotation_ValueChanged);
			// 
			// lblRotation
			// 
			this.lblRotation.AutoSize = true;
			this.lblRotation.Location = new System.Drawing.Point(102, 8);
			this.lblRotation.Name = "lblRotation";
			this.lblRotation.Size = new System.Drawing.Size(66, 13);
			this.lblRotation.TabIndex = 18;
			this.lblRotation.Text = "Ausrichtung:";
			// 
			// btnCwLarge
			// 
			this.btnCwLarge.Image = ((System.Drawing.Image)(resources.GetObject("btnCwLarge.Image")));
			this.btnCwLarge.Location = new System.Drawing.Point(72, 3);
			this.btnCwLarge.Name = "btnCwLarge";
			this.btnCwLarge.Size = new System.Drawing.Size(24, 24);
			this.btnCwLarge.TabIndex = 4;
			this.btnCwLarge.UseVisualStyleBackColor = true;
			this.btnCwLarge.Click += new System.EventHandler(this.btnRotate_Click);
			// 
			// btnCwSmall
			// 
			this.btnCwSmall.Image = ((System.Drawing.Image)(resources.GetObject("btnCwSmall.Image")));
			this.btnCwSmall.Location = new System.Drawing.Point(49, 3);
			this.btnCwSmall.Name = "btnCwSmall";
			this.btnCwSmall.Size = new System.Drawing.Size(24, 24);
			this.btnCwSmall.TabIndex = 3;
			this.btnCwSmall.UseVisualStyleBackColor = true;
			this.btnCwSmall.Click += new System.EventHandler(this.btnRotate_Click);
			// 
			// btnCcwSmall
			// 
			this.btnCcwSmall.Image = ((System.Drawing.Image)(resources.GetObject("btnCcwSmall.Image")));
			this.btnCcwSmall.Location = new System.Drawing.Point(26, 3);
			this.btnCcwSmall.Name = "btnCcwSmall";
			this.btnCcwSmall.Size = new System.Drawing.Size(24, 24);
			this.btnCcwSmall.TabIndex = 2;
			this.btnCcwSmall.UseVisualStyleBackColor = true;
			this.btnCcwSmall.Click += new System.EventHandler(this.btnRotate_Click);
			// 
			// btnCcwLarge
			// 
			this.btnCcwLarge.Image = ((System.Drawing.Image)(resources.GetObject("btnCcwLarge.Image")));
			this.btnCcwLarge.Location = new System.Drawing.Point(3, 3);
			this.btnCcwLarge.Name = "btnCcwLarge";
			this.btnCcwLarge.Size = new System.Drawing.Size(24, 24);
			this.btnCcwLarge.TabIndex = 1;
			this.btnCcwLarge.UseVisualStyleBackColor = true;
			this.btnCcwLarge.Click += new System.EventHandler(this.btnRotate_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(581, 3);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(95, 23);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "Übernehmen";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(0, 25);
			this.panel.Name = "panel";
			this.panel.ProductPlanner = this.distributorPositioner;
			this.panel.Size = new System.Drawing.Size(679, 437);
			this.panel.TabIndex = 0;
			// 
			// distributorPositioner
			// 
			this.distributorPositioner.DrawOtherDistributorsInPlan = false;
			this.distributorPositioner.Mode = Europlan.Common.DistributorPositioner.DistributorPositionerMode.DPM_NONE;
			this.distributorPositioner.ModeChanged += new System.EventHandler(this.distributorPositioner_ModeChanged);
			// 
			// DistributorPositionerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(679, 462);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.toolStrip);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 500);
			this.Name = "DistributorPositionerForm";
			this.Text = "Raumtypen";
			this.Load += new System.EventHandler(this.DistributionPositionerForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DistributionPositionerForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnPosition;
		private PlanPanel panel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOk;
		private DistributorPositioner distributorPositioner;
		private System.Windows.Forms.Button btnCwLarge;
		private System.Windows.Forms.Button btnCwSmall;
		private System.Windows.Forms.Button btnCcwSmall;
		private System.Windows.Forms.Button btnCcwLarge;
		private System.Windows.Forms.Label lblRotationUnit;
		private NumericBox numRotation;
		private System.Windows.Forms.Label lblRotation;


	}
}