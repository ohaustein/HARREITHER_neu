namespace Europlan.Common {
	partial class GraphicalWallModifierForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphicalWallModifierForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.btnPickWall = new System.Windows.Forms.ToolStripButton();
			this.btnObstacle = new System.Windows.Forms.ToolStripButton();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panelWall = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.numWallVertical = new Europlan.Common.NumericBox();
			this.numWallHorizontal = new Europlan.Common.NumericBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lblWallVertical = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblWallHorizontal = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnWallSelectConstruction = new System.Windows.Forms.Button();
			this.txtWallConstruction = new System.Windows.Forms.TextBox();
			this.chkStartWall = new System.Windows.Forms.CheckBox();
			this.chkEnable = new System.Windows.Forms.CheckBox();
			this.panel = new Europlan.Common.PlanPanel();
			this.graphicalWallModifier = new Europlan.Common.GraphicalWallModifier(this.components);
			this.toolStrip.SuspendLayout();
			this.panelBottom.SuspendLayout();
			this.panelWall.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
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
            this.btnPickWall,
            this.btnObstacle});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(679, 25);
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
			// btnPickWall
			// 
			this.btnPickWall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPickWall.Image = ((System.Drawing.Image)(resources.GetObject("btnPickWall.Image")));
			this.btnPickWall.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPickWall.Name = "btnPickWall";
			this.btnPickWall.Size = new System.Drawing.Size(23, 22);
			this.btnPickWall.Text = "Wand wählen";
			this.btnPickWall.Click += new System.EventHandler(this.btnPickWall_Click);
			// 
			// btnObstacle
			// 
			this.btnObstacle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnObstacle.Image = ((System.Drawing.Image)(resources.GetObject("btnObstacle.Image")));
			this.btnObstacle.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnObstacle.Name = "btnObstacle";
			this.btnObstacle.Size = new System.Drawing.Size(23, 22);
			this.btnObstacle.Text = "Fenster und Türen";
			this.btnObstacle.Click += new System.EventHandler(this.btnObstacle_Click);
			// 
			// panelBottom
			// 
			this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelBottom.Controls.Add(this.btnOk);
			this.panelBottom.Controls.Add(this.btnCancel);
			this.panelBottom.Controls.Add(this.panelWall);
			this.panelBottom.Location = new System.Drawing.Point(0, 378);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(679, 84);
			this.panelBottom.TabIndex = 3;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(475, 49);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(93, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(574, 49);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(93, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// panelWall
			// 
			this.panelWall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelWall.Controls.Add(this.groupBox2);
			this.panelWall.Controls.Add(this.groupBox1);
			this.panelWall.Controls.Add(this.chkStartWall);
			this.panelWall.Controls.Add(this.chkEnable);
			this.panelWall.Location = new System.Drawing.Point(0, 0);
			this.panelWall.Name = "panelWall";
			this.panelWall.Size = new System.Drawing.Size(679, 78);
			this.panelWall.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.numWallVertical);
			this.groupBox2.Controls.Add(this.numWallHorizontal);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.lblWallVertical);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.lblWallHorizontal);
			this.groupBox2.Location = new System.Drawing.Point(164, 9);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(217, 66);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Größe";
			// 
			// numWallVertical
			// 
			this.numWallVertical.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numWallVertical.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numWallVertical.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numWallVertical.Location = new System.Drawing.Point(97, 42);
			this.numWallVertical.MaxValue = null;
			this.numWallVertical.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numWallVertical.Name = "numWallVertical";
			this.numWallVertical.Size = new System.Drawing.Size(87, 20);
			this.numWallVertical.TabIndex = 21;
			this.numWallVertical.Text = "0";
			this.numWallVertical.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numWallVertical.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numWallVertical.ValueChanged += new System.EventHandler(this.numWallVertical_ValueChanged);
			// 
			// numWallHorizontal
			// 
			this.numWallHorizontal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numWallHorizontal.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numWallHorizontal.Enabled = false;
			this.numWallHorizontal.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numWallHorizontal.Location = new System.Drawing.Point(97, 19);
			this.numWallHorizontal.MaxValue = null;
			this.numWallHorizontal.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numWallHorizontal.Name = "numWallHorizontal";
			this.numWallHorizontal.Size = new System.Drawing.Size(87, 20);
			this.numWallHorizontal.TabIndex = 20;
			this.numWallHorizontal.Text = "0";
			this.numWallHorizontal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numWallHorizontal.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(190, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(21, 13);
			this.label3.TabIndex = 19;
			this.label3.Text = "cm";
			// 
			// lblWallVertical
			// 
			this.lblWallVertical.Location = new System.Drawing.Point(6, 45);
			this.lblWallVertical.Name = "lblWallVertical";
			this.lblWallVertical.Size = new System.Drawing.Size(85, 23);
			this.lblWallVertical.TabIndex = 18;
			this.lblWallVertical.Text = "Senkrecht:";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(190, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(21, 13);
			this.label2.TabIndex = 16;
			this.label2.Text = "cm";
			// 
			// lblWallHorizontal
			// 
			this.lblWallHorizontal.Location = new System.Drawing.Point(6, 22);
			this.lblWallHorizontal.Name = "lblWallHorizontal";
			this.lblWallHorizontal.Size = new System.Drawing.Size(85, 23);
			this.lblWallHorizontal.TabIndex = 15;
			this.lblWallHorizontal.Text = "Waagrecht:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.btnWallSelectConstruction);
			this.groupBox1.Controls.Add(this.txtWallConstruction);
			this.groupBox1.Location = new System.Drawing.Point(12, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(146, 66);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Konstruktion";
			// 
			// btnWallSelectConstruction
			// 
			this.btnWallSelectConstruction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWallSelectConstruction.Location = new System.Drawing.Point(110, 17);
			this.btnWallSelectConstruction.Name = "btnWallSelectConstruction";
			this.btnWallSelectConstruction.Size = new System.Drawing.Size(30, 23);
			this.btnWallSelectConstruction.TabIndex = 15;
			this.btnWallSelectConstruction.Text = "...";
			this.btnWallSelectConstruction.UseVisualStyleBackColor = true;
			this.btnWallSelectConstruction.Click += new System.EventHandler(this.btnWallSelectConstruction_Click);
			// 
			// txtWallConstruction
			// 
			this.txtWallConstruction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtWallConstruction.Location = new System.Drawing.Point(6, 19);
			this.txtWallConstruction.Name = "txtWallConstruction";
			this.txtWallConstruction.ReadOnly = true;
			this.txtWallConstruction.Size = new System.Drawing.Size(98, 20);
			this.txtWallConstruction.TabIndex = 14;
			// 
			// chkStartWall
			// 
			this.chkStartWall.AutoSize = true;
			this.chkStartWall.Location = new System.Drawing.Point(513, 26);
			this.chkStartWall.Name = "chkStartWall";
			this.chkStartWall.Size = new System.Drawing.Size(154, 17);
			this.chkStartWall.TabIndex = 1;
			this.chkStartWall.Text = "Als erste Wand verwenden";
			this.chkStartWall.UseVisualStyleBackColor = true;
			this.chkStartWall.CheckedChanged += new System.EventHandler(this.chkStartWall_CheckedChanged);
			// 
			// chkEnable
			// 
			this.chkEnable.AutoSize = true;
			this.chkEnable.Location = new System.Drawing.Point(387, 26);
			this.chkEnable.Name = "chkEnable";
			this.chkEnable.Size = new System.Drawing.Size(108, 17);
			this.chkEnable.TabIndex = 0;
			this.chkEnable.Text = "Wand erzeugen?";
			this.chkEnable.UseVisualStyleBackColor = true;
			this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(0, 25);
			this.panel.Name = "panel";
			this.panel.ProductPlanner = this.graphicalWallModifier;
			this.panel.Size = new System.Drawing.Size(679, 437);
			this.panel.TabIndex = 2;
			// 
			// graphicalWallModifier
			// 
			this.graphicalWallModifier.Mode = Europlan.Common.GraphicalWallModifier.GraphicalWallModifierMode.GWM_NONE;
			this.graphicalWallModifier.ObjectSelected += new System.EventHandler<Europlan.Common.GraphicalWallModifier.SelectedObjectArgs>(this.graphicalWallModifier_ObjectSelected);
			this.graphicalWallModifier.ModeChanged += new System.EventHandler(this.graphicalWallModifier_ModeChanged);
			// 
			// GraphicalWallModifierForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(679, 462);
			this.Controls.Add(this.panelBottom);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.toolStrip);
			this.DoubleBuffered = true;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 500);
			this.Name = "GraphicalWallModifierForm";
			this.Text = "Wände definieren";
			this.Load += new System.EventHandler(this.GraphicalWallModifierForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphicalWallModifierForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.panelBottom.ResumeLayout(false);
			this.panelWall.ResumeLayout(false);
			this.panelWall.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private PlanPanel panel;
		private GraphicalWallModifier graphicalWallModifier;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.ToolStripButton btnPickWall;
		private System.Windows.Forms.ToolStripButton btnObstacle;
		private System.Windows.Forms.Panel panelWall;
		private System.Windows.Forms.CheckBox chkEnable;
		private System.Windows.Forms.CheckBox chkStartWall;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnWallSelectConstruction;
		private System.Windows.Forms.TextBox txtWallConstruction;
		private System.Windows.Forms.GroupBox groupBox2;
		private NumericBox numWallVertical;
		private NumericBox numWallHorizontal;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblWallVertical;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblWallHorizontal;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;


	}
}