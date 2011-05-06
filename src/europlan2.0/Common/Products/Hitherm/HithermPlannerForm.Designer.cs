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
			this.btnRegister = new System.Windows.Forms.ToolStripButton();
			this.panelTop = new System.Windows.Forms.Panel();
			this.panelDefineWalls = new System.Windows.Forms.Panel();
			this.btnWallHelpLine = new System.Windows.Forms.Button();
			this.btnWallEdgeDistance = new System.Windows.Forms.Button();
			this.btnWallNewWall = new System.Windows.Forms.Button();
			this.btnWallDelete = new System.Windows.Forms.Button();
			this.btnWallRevert = new System.Windows.Forms.Button();
			this.btnWallApply = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnWallLeft = new System.Windows.Forms.Button();
			this.btnWallRight = new System.Windows.Forms.Button();
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
			this.lblSelectedWall = new System.Windows.Forms.Label();
			this.btnCreateWalls = new System.Windows.Forms.Button();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.graphicalWallPanel = new Europlan.Common.GraphicalWallPanel();
			this.hithermPlanner = new Europlan.Common.HithermPlanner();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.panelTop.SuspendLayout();
			this.panelDefineWalls.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
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
            this.btnObstacle,
            this.btnRegister});
			this.toolStrip2.Location = new System.Drawing.Point(0, 25);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(24, 440);
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
			this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
			// 
			// btnMove
			// 
			this.btnMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnMove.Image = ((System.Drawing.Image)(resources.GetObject("btnMove.Image")));
			this.btnMove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnMove.Name = "btnMove";
			this.btnMove.Size = new System.Drawing.Size(21, 41);
			this.btnMove.Text = "move";
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
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
			// btnRegister
			// 
			this.btnRegister.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnRegister.Image = ((System.Drawing.Image)(resources.GetObject("btnRegister.Image")));
			this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRegister.Name = "btnRegister";
			this.btnRegister.Size = new System.Drawing.Size(21, 31);
			this.btnRegister.Text = "add";
			this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
			// 
			// panelTop
			// 
			this.panelTop.Controls.Add(this.panelDefineWalls);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTop.Location = new System.Drawing.Point(24, 25);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(862, 91);
			this.panelTop.TabIndex = 2;
			// 
			// panelDefineWalls
			// 
			this.panelDefineWalls.Controls.Add(this.btnWallHelpLine);
			this.panelDefineWalls.Controls.Add(this.btnWallEdgeDistance);
			this.panelDefineWalls.Controls.Add(this.btnWallNewWall);
			this.panelDefineWalls.Controls.Add(this.btnWallDelete);
			this.panelDefineWalls.Controls.Add(this.btnWallRevert);
			this.panelDefineWalls.Controls.Add(this.btnWallApply);
			this.panelDefineWalls.Controls.Add(this.groupBox3);
			this.panelDefineWalls.Controls.Add(this.groupBox2);
			this.panelDefineWalls.Controls.Add(this.groupBox1);
			this.panelDefineWalls.Controls.Add(this.lblSelectedWall);
			this.panelDefineWalls.Controls.Add(this.btnCreateWalls);
			this.panelDefineWalls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelDefineWalls.Location = new System.Drawing.Point(0, 0);
			this.panelDefineWalls.Name = "panelDefineWalls";
			this.panelDefineWalls.Size = new System.Drawing.Size(862, 91);
			this.panelDefineWalls.TabIndex = 1;
			// 
			// btnWallHelpLine
			// 
			this.btnWallHelpLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.btnWallHelpLine.Location = new System.Drawing.Point(527, 56);
			this.btnWallHelpLine.Name = "btnWallHelpLine";
			this.btnWallHelpLine.Size = new System.Drawing.Size(25, 25);
			this.btnWallHelpLine.TabIndex = 22;
			this.btnWallHelpLine.UseVisualStyleBackColor = true;
			// 
			// btnWallEdgeDistance
			// 
			this.btnWallEdgeDistance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.btnWallEdgeDistance.Location = new System.Drawing.Point(527, 25);
			this.btnWallEdgeDistance.Name = "btnWallEdgeDistance";
			this.btnWallEdgeDistance.Size = new System.Drawing.Size(25, 25);
			this.btnWallEdgeDistance.TabIndex = 21;
			this.btnWallEdgeDistance.UseVisualStyleBackColor = true;
			this.btnWallEdgeDistance.Click += new System.EventHandler(this.btnWallEdgeDistance_Click);
			// 
			// btnWallNewWall
			// 
			this.btnWallNewWall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.btnWallNewWall.Location = new System.Drawing.Point(496, 56);
			this.btnWallNewWall.Name = "btnWallNewWall";
			this.btnWallNewWall.Size = new System.Drawing.Size(25, 25);
			this.btnWallNewWall.TabIndex = 20;
			this.btnWallNewWall.UseVisualStyleBackColor = true;
			this.btnWallNewWall.Click += new System.EventHandler(this.btnWallNewWall_Click);
			// 
			// btnWallDelete
			// 
			this.btnWallDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.btnWallDelete.Location = new System.Drawing.Point(496, 25);
			this.btnWallDelete.Name = "btnWallDelete";
			this.btnWallDelete.Size = new System.Drawing.Size(25, 25);
			this.btnWallDelete.TabIndex = 19;
			this.btnWallDelete.UseVisualStyleBackColor = true;
			this.btnWallDelete.Click += new System.EventHandler(this.btnWallDelete_Click);
			// 
			// btnWallRevert
			// 
			this.btnWallRevert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.btnWallRevert.Location = new System.Drawing.Point(465, 56);
			this.btnWallRevert.Name = "btnWallRevert";
			this.btnWallRevert.Size = new System.Drawing.Size(25, 25);
			this.btnWallRevert.TabIndex = 18;
			this.btnWallRevert.UseVisualStyleBackColor = true;
			this.btnWallRevert.Click += new System.EventHandler(this.btnWallRevert_Click);
			// 
			// btnWallApply
			// 
			this.btnWallApply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.btnWallApply.Location = new System.Drawing.Point(465, 25);
			this.btnWallApply.Name = "btnWallApply";
			this.btnWallApply.Size = new System.Drawing.Size(25, 25);
			this.btnWallApply.TabIndex = 17;
			this.btnWallApply.UseVisualStyleBackColor = true;
			this.btnWallApply.Click += new System.EventHandler(this.btnWallApply_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox3.Controls.Add(this.btnWallLeft);
			this.groupBox3.Controls.Add(this.btnWallRight);
			this.groupBox3.Location = new System.Drawing.Point(382, 19);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(77, 66);
			this.groupBox3.TabIndex = 16;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Position";
			// 
			// btnWallLeft
			// 
			this.btnWallLeft.Location = new System.Drawing.Point(6, 22);
			this.btnWallLeft.Name = "btnWallLeft";
			this.btnWallLeft.Size = new System.Drawing.Size(30, 36);
			this.btnWallLeft.TabIndex = 16;
			this.btnWallLeft.Text = "<";
			this.btnWallLeft.UseVisualStyleBackColor = true;
			this.btnWallLeft.Click += new System.EventHandler(this.btnWallLeft_Click);
			// 
			// btnWallRight
			// 
			this.btnWallRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWallRight.Location = new System.Drawing.Point(41, 22);
			this.btnWallRight.Name = "btnWallRight";
			this.btnWallRight.Size = new System.Drawing.Size(30, 36);
			this.btnWallRight.TabIndex = 15;
			this.btnWallRight.Text = ">";
			this.btnWallRight.UseVisualStyleBackColor = true;
			this.btnWallRight.Click += new System.EventHandler(this.btnWallRight_Click);
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
			this.groupBox2.Location = new System.Drawing.Point(159, 19);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(217, 66);
			this.groupBox2.TabIndex = 16;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Größe";
			// 
			// numWallVertical
			// 
			this.numWallVertical.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numWallVertical.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numWallVertical.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWallVertical.Location = new System.Drawing.Point(97, 42);
			this.numWallVertical.MaxValue = null;
			this.numWallVertical.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWallVertical.Name = "numWallVertical";
			this.numWallVertical.Size = new System.Drawing.Size(87, 20);
			this.numWallVertical.TabIndex = 21;
			this.numWallVertical.Text = "0";
			this.numWallVertical.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numWallVertical.Value = new decimal(new int[] {
            1,
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
			this.numWallHorizontal.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWallHorizontal.Location = new System.Drawing.Point(97, 19);
			this.numWallHorizontal.MaxValue = null;
			this.numWallHorizontal.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWallHorizontal.Name = "numWallHorizontal";
			this.numWallHorizontal.Size = new System.Drawing.Size(87, 20);
			this.numWallHorizontal.TabIndex = 20;
			this.numWallHorizontal.Text = "0";
			this.numWallHorizontal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numWallHorizontal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWallHorizontal.ValueChanged += new System.EventHandler(this.numWallHorizontal_ValueChanged);
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
			this.groupBox1.Location = new System.Drawing.Point(7, 19);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(146, 66);
			this.groupBox1.TabIndex = 2;
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
			// lblSelectedWall
			// 
			this.lblSelectedWall.AutoSize = true;
			this.lblSelectedWall.Location = new System.Drawing.Point(4, 3);
			this.lblSelectedWall.Name = "lblSelectedWall";
			this.lblSelectedWall.Size = new System.Drawing.Size(123, 13);
			this.lblSelectedWall.TabIndex = 1;
			this.lblSelectedWall.Text = "Keine Wand ausgewählt";
			// 
			// btnCreateWalls
			// 
			this.btnCreateWalls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreateWalls.Location = new System.Drawing.Point(607, 3);
			this.btnCreateWalls.Name = "btnCreateWalls";
			this.btnCreateWalls.Size = new System.Drawing.Size(252, 23);
			this.btnCreateWalls.TabIndex = 0;
			this.btnCreateWalls.Text = "Wände aus Raumgeometrie erzeugen";
			this.btnCreateWalls.UseVisualStyleBackColor = true;
			this.btnCreateWalls.Click += new System.EventHandler(this.btnCreateWalls_Click);
			// 
			// panelBottom
			// 
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(24, 377);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(862, 88);
			this.panelBottom.TabIndex = 3;
			// 
			// graphicalWallPanel
			// 
			this.graphicalWallPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphicalWallPanel.Location = new System.Drawing.Point(24, 116);
			this.graphicalWallPanel.Mode = Europlan.Common.GraphicalWallPanel.PlanMode.PM_MOVE;
			this.graphicalWallPanel.Name = "graphicalWallPanel";
			this.graphicalWallPanel.ProductPlanner = this.hithermPlanner;
			this.graphicalWallPanel.Room = null;
			this.graphicalWallPanel.Scale = 1;
			this.graphicalWallPanel.SelectedObject = null;
			this.graphicalWallPanel.Size = new System.Drawing.Size(862, 261);
			this.graphicalWallPanel.TabIndex = 4;
			this.graphicalWallPanel.XPos = 421;
			this.graphicalWallPanel.YPos = -130.5;
			this.graphicalWallPanel.ObjectSelected += new System.EventHandler<Europlan.Common.GraphicalWallPanel.SelectedObjectArgs>(this.graphicalWallPanel_ObjectSelected);
			// 
			// hithermPlanner
			// 
			this.hithermPlanner.ConnectedWallPanel = this.graphicalWallPanel;
			this.hithermPlanner.Mode = Europlan.Common.HithermPlanner.HithermPlannerMode.HPM_NONE;
			// 
			// HithermPlannerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(886, 465);
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
			this.panelDefineWalls.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
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
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblSelectedWall;
		private System.Windows.Forms.Button btnWallSelectConstruction;
		private System.Windows.Forms.TextBox txtWallConstruction;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblWallHorizontal;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblWallVertical;
		private System.Windows.Forms.Label label2;
		private NumericBox numWallVertical;
		private NumericBox numWallHorizontal;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnWallLeft;
		private System.Windows.Forms.Button btnWallRight;
		private System.Windows.Forms.Button btnWallHelpLine;
		private System.Windows.Forms.Button btnWallEdgeDistance;
		private System.Windows.Forms.Button btnWallNewWall;
		private System.Windows.Forms.Button btnWallDelete;
		private System.Windows.Forms.Button btnWallRevert;
		private System.Windows.Forms.Button btnWallApply;
		private HithermPlanner hithermPlanner;
		private System.Windows.Forms.ToolStripButton btnRegister;


	}
}