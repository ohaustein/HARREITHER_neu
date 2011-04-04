namespace Europlan.Common {
	partial class RoomPickerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomPickerForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.btnPickRoom = new System.Windows.Forms.ToolStripButton();
			this.btnPickUnused = new System.Windows.Forms.ToolStripButton();
			this.btnDelUnused = new System.Windows.Forms.ToolStripButton();
			this.btnAddExpansionGap = new System.Windows.Forms.ToolStripButton();
			this.btnRemoveExpansionGap = new System.Windows.Forms.ToolStripButton();
			this.panUnheatedArea = new System.Windows.Forms.Panel();
			this.cbEnterArea = new System.Windows.Forms.CheckBox();
			this.btnAddUnheatedArea = new System.Windows.Forms.Button();
			this.grpSize = new System.Windows.Forms.GroupBox();
			this.lblSizeYUnit = new System.Windows.Forms.Label();
			this.numSizeY = new Europlan.Common.NumericBox();
			this.lblSizeY = new System.Windows.Forms.Label();
			this.lblSizeXUnit = new System.Windows.Forms.Label();
			this.numSizeX = new Europlan.Common.NumericBox();
			this.lblSizeX = new System.Windows.Forms.Label();
			this.grpDistance = new System.Windows.Forms.GroupBox();
			this.lblDistanceYUnit = new System.Windows.Forms.Label();
			this.numDistanceY = new Europlan.Common.NumericBox();
			this.lblDistanceY = new System.Windows.Forms.Label();
			this.lblDistanceXUnit = new System.Windows.Forms.Label();
			this.numDistanceX = new Europlan.Common.NumericBox();
			this.lblDistanceX = new System.Windows.Forms.Label();
			this.cbReferencePoint = new System.Windows.Forms.CheckBox();
			this.cbUnheatedTextual = new System.Windows.Forms.RadioButton();
			this.cbUnheatedGraphical = new System.Windows.Forms.RadioButton();
			this.lblAddUnheatedArea = new System.Windows.Forms.Label();
			this.panel = new Europlan.Common.PlanPanel();
			this.roomPicker = new Europlan.Common.RoomPicker(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnOk = new System.Windows.Forms.Button();
			this.toolStrip.SuspendLayout();
			this.panUnheatedArea.SuspendLayout();
			this.grpSize.SuspendLayout();
			this.grpDistance.SuspendLayout();
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
            this.btnPickRoom,
            this.btnPickUnused,
            this.btnDelUnused,
            this.btnAddExpansionGap,
            this.btnRemoveExpansionGap});
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
			// btnPickRoom
			// 
			this.btnPickRoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPickRoom.Image = ((System.Drawing.Image)(resources.GetObject("btnPickRoom.Image")));
			this.btnPickRoom.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPickRoom.Name = "btnPickRoom";
			this.btnPickRoom.Size = new System.Drawing.Size(23, 22);
			this.btnPickRoom.Text = "Raumgeometrie definieren";
			this.btnPickRoom.Click += new System.EventHandler(this.btnPickRoom_Click);
			// 
			// btnPickUnused
			// 
			this.btnPickUnused.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPickUnused.Image = ((System.Drawing.Image)(resources.GetObject("btnPickUnused.Image")));
			this.btnPickUnused.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPickUnused.Name = "btnPickUnused";
			this.btnPickUnused.Size = new System.Drawing.Size(23, 22);
			this.btnPickUnused.Text = "Unbeheizte Flächen definieren";
			this.btnPickUnused.Click += new System.EventHandler(this.btnPickUnused_Click);
			// 
			// btnDelUnused
			// 
			this.btnDelUnused.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDelUnused.Image = ((System.Drawing.Image)(resources.GetObject("btnDelUnused.Image")));
			this.btnDelUnused.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDelUnused.Name = "btnDelUnused";
			this.btnDelUnused.Size = new System.Drawing.Size(23, 22);
			this.btnDelUnused.Text = "Unbeheizte Flächen löschen";
			this.btnDelUnused.Click += new System.EventHandler(this.btnDelUnused_Click);
			// 
			// btnAddExpansionGap
			// 
			this.btnAddExpansionGap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAddExpansionGap.Image = ((System.Drawing.Image)(resources.GetObject("btnAddExpansionGap.Image")));
			this.btnAddExpansionGap.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAddExpansionGap.Name = "btnAddExpansionGap";
			this.btnAddExpansionGap.Size = new System.Drawing.Size(23, 22);
			this.btnAddExpansionGap.Text = "Dehnfuge hinzufügen";
			this.btnAddExpansionGap.Click += new System.EventHandler(this.btnAddExpansionGap_Click);
			// 
			// btnRemoveExpansionGap
			// 
			this.btnRemoveExpansionGap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRemoveExpansionGap.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveExpansionGap.Image")));
			this.btnRemoveExpansionGap.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRemoveExpansionGap.Name = "btnRemoveExpansionGap";
			this.btnRemoveExpansionGap.Size = new System.Drawing.Size(23, 22);
			this.btnRemoveExpansionGap.Text = "Dehnfuge löschen";
			this.btnRemoveExpansionGap.Click += new System.EventHandler(this.btnRemoveExpansionGap_Click);
			// 
			// panUnheatedArea
			// 
			this.panUnheatedArea.Controls.Add(this.cbEnterArea);
			this.panUnheatedArea.Controls.Add(this.btnAddUnheatedArea);
			this.panUnheatedArea.Controls.Add(this.grpSize);
			this.panUnheatedArea.Controls.Add(this.grpDistance);
			this.panUnheatedArea.Controls.Add(this.cbReferencePoint);
			this.panUnheatedArea.Controls.Add(this.cbUnheatedTextual);
			this.panUnheatedArea.Controls.Add(this.cbUnheatedGraphical);
			this.panUnheatedArea.Controls.Add(this.lblAddUnheatedArea);
			this.panUnheatedArea.Dock = System.Windows.Forms.DockStyle.Right;
			this.panUnheatedArea.Location = new System.Drawing.Point(522, 25);
			this.panUnheatedArea.Name = "panUnheatedArea";
			this.panUnheatedArea.Size = new System.Drawing.Size(157, 408);
			this.panUnheatedArea.TabIndex = 3;
			this.panUnheatedArea.Visible = false;
			// 
			// cbEnterArea
			// 
			this.cbEnterArea.Appearance = System.Windows.Forms.Appearance.Button;
			this.cbEnterArea.Enabled = false;
			this.cbEnterArea.Location = new System.Drawing.Point(6, 107);
			this.cbEnterArea.Name = "cbEnterArea";
			this.cbEnterArea.Size = new System.Drawing.Size(145, 23);
			this.cbEnterArea.TabIndex = 7;
			this.cbEnterArea.Text = "Fläche eingeben";
			this.cbEnterArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbEnterArea.UseVisualStyleBackColor = true;
			this.cbEnterArea.CheckedChanged += new System.EventHandler(this.cbEnterArea_CheckedChanged);
			// 
			// btnAddUnheatedArea
			// 
			this.btnAddUnheatedArea.Enabled = false;
			this.btnAddUnheatedArea.Location = new System.Drawing.Point(6, 363);
			this.btnAddUnheatedArea.Name = "btnAddUnheatedArea";
			this.btnAddUnheatedArea.Size = new System.Drawing.Size(145, 23);
			this.btnAddUnheatedArea.TabIndex = 4;
			this.btnAddUnheatedArea.Text = "Fläche hinzufügen";
			this.btnAddUnheatedArea.UseVisualStyleBackColor = true;
			this.btnAddUnheatedArea.Click += new System.EventHandler(this.btnAddUnheatedArea_Click);
			// 
			// grpSize
			// 
			this.grpSize.Controls.Add(this.lblSizeYUnit);
			this.grpSize.Controls.Add(this.numSizeY);
			this.grpSize.Controls.Add(this.lblSizeY);
			this.grpSize.Controls.Add(this.lblSizeXUnit);
			this.grpSize.Controls.Add(this.numSizeX);
			this.grpSize.Controls.Add(this.lblSizeX);
			this.grpSize.Enabled = false;
			this.grpSize.Location = new System.Drawing.Point(6, 257);
			this.grpSize.Name = "grpSize";
			this.grpSize.Size = new System.Drawing.Size(145, 100);
			this.grpSize.TabIndex = 6;
			this.grpSize.TabStop = false;
			this.grpSize.Text = "Größe";
			// 
			// lblSizeYUnit
			// 
			this.lblSizeYUnit.Location = new System.Drawing.Point(116, 74);
			this.lblSizeYUnit.Name = "lblSizeYUnit";
			this.lblSizeYUnit.Size = new System.Drawing.Size(23, 13);
			this.lblSizeYUnit.TabIndex = 13;
			this.lblSizeYUnit.Text = "m";
			// 
			// numSizeY
			// 
			this.numSizeY.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numSizeY.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numSizeY.Location = new System.Drawing.Point(14, 71);
			this.numSizeY.MaxValue = null;
			this.numSizeY.MinValue = null;
			this.numSizeY.Name = "numSizeY";
			this.numSizeY.Size = new System.Drawing.Size(100, 20);
			this.numSizeY.TabIndex = 12;
			this.numSizeY.Text = "0";
			this.numSizeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numSizeY.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numSizeY.ValueChanged += new System.EventHandler(this.numSize_ValueChanged);
			// 
			// lblSizeY
			// 
			this.lblSizeY.Location = new System.Drawing.Point(6, 55);
			this.lblSizeY.Name = "lblSizeY";
			this.lblSizeY.Size = new System.Drawing.Size(133, 13);
			this.lblSizeY.TabIndex = 11;
			this.lblSizeY.Text = "Höhe:";
			// 
			// lblSizeXUnit
			// 
			this.lblSizeXUnit.Location = new System.Drawing.Point(116, 35);
			this.lblSizeXUnit.Name = "lblSizeXUnit";
			this.lblSizeXUnit.Size = new System.Drawing.Size(23, 13);
			this.lblSizeXUnit.TabIndex = 8;
			this.lblSizeXUnit.Text = "m";
			// 
			// numSizeX
			// 
			this.numSizeX.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numSizeX.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numSizeX.Location = new System.Drawing.Point(14, 32);
			this.numSizeX.MaxValue = null;
			this.numSizeX.MinValue = null;
			this.numSizeX.Name = "numSizeX";
			this.numSizeX.Size = new System.Drawing.Size(100, 20);
			this.numSizeX.TabIndex = 9;
			this.numSizeX.Text = "0";
			this.numSizeX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numSizeX.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numSizeX.ValueChanged += new System.EventHandler(this.numSize_ValueChanged);
			// 
			// lblSizeX
			// 
			this.lblSizeX.Location = new System.Drawing.Point(6, 16);
			this.lblSizeX.Name = "lblSizeX";
			this.lblSizeX.Size = new System.Drawing.Size(133, 13);
			this.lblSizeX.TabIndex = 10;
			this.lblSizeX.Text = "Breite:";
			// 
			// grpDistance
			// 
			this.grpDistance.Controls.Add(this.lblDistanceYUnit);
			this.grpDistance.Controls.Add(this.numDistanceY);
			this.grpDistance.Controls.Add(this.lblDistanceY);
			this.grpDistance.Controls.Add(this.lblDistanceXUnit);
			this.grpDistance.Controls.Add(this.numDistanceX);
			this.grpDistance.Controls.Add(this.lblDistanceX);
			this.grpDistance.Enabled = false;
			this.grpDistance.Location = new System.Drawing.Point(6, 136);
			this.grpDistance.Name = "grpDistance";
			this.grpDistance.Size = new System.Drawing.Size(145, 115);
			this.grpDistance.TabIndex = 4;
			this.grpDistance.TabStop = false;
			this.grpDistance.Text = "Abstand vom Referenzpunkt";
			// 
			// lblDistanceYUnit
			// 
			this.lblDistanceYUnit.Location = new System.Drawing.Point(116, 87);
			this.lblDistanceYUnit.Name = "lblDistanceYUnit";
			this.lblDistanceYUnit.Size = new System.Drawing.Size(23, 13);
			this.lblDistanceYUnit.TabIndex = 7;
			this.lblDistanceYUnit.Text = "m";
			// 
			// numDistanceY
			// 
			this.numDistanceY.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numDistanceY.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numDistanceY.Location = new System.Drawing.Point(14, 84);
			this.numDistanceY.MaxValue = null;
			this.numDistanceY.MinValue = null;
			this.numDistanceY.Name = "numDistanceY";
			this.numDistanceY.Size = new System.Drawing.Size(100, 20);
			this.numDistanceY.TabIndex = 6;
			this.numDistanceY.Text = "0";
			this.numDistanceY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numDistanceY.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numDistanceY.ValueChanged += new System.EventHandler(this.numDistance_ValueChanged);
			// 
			// lblDistanceY
			// 
			this.lblDistanceY.Location = new System.Drawing.Point(6, 68);
			this.lblDistanceY.Name = "lblDistanceY";
			this.lblDistanceY.Size = new System.Drawing.Size(133, 13);
			this.lblDistanceY.TabIndex = 5;
			this.lblDistanceY.Text = "Vertikaler Abstand:";
			// 
			// lblDistanceXUnit
			// 
			this.lblDistanceXUnit.Location = new System.Drawing.Point(116, 48);
			this.lblDistanceXUnit.Name = "lblDistanceXUnit";
			this.lblDistanceXUnit.Size = new System.Drawing.Size(23, 13);
			this.lblDistanceXUnit.TabIndex = 4;
			this.lblDistanceXUnit.Text = "m";
			// 
			// numDistanceX
			// 
			this.numDistanceX.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numDistanceX.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numDistanceX.Location = new System.Drawing.Point(14, 45);
			this.numDistanceX.MaxValue = null;
			this.numDistanceX.MinValue = null;
			this.numDistanceX.Name = "numDistanceX";
			this.numDistanceX.Size = new System.Drawing.Size(100, 20);
			this.numDistanceX.TabIndex = 4;
			this.numDistanceX.Text = "0";
			this.numDistanceX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numDistanceX.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numDistanceX.ValueChanged += new System.EventHandler(this.numDistance_ValueChanged);
			// 
			// lblDistanceX
			// 
			this.lblDistanceX.Location = new System.Drawing.Point(6, 29);
			this.lblDistanceX.Name = "lblDistanceX";
			this.lblDistanceX.Size = new System.Drawing.Size(133, 13);
			this.lblDistanceX.TabIndex = 4;
			this.lblDistanceX.Text = "Horizontaler Abstand:";
			// 
			// cbReferencePoint
			// 
			this.cbReferencePoint.Appearance = System.Windows.Forms.Appearance.Button;
			this.cbReferencePoint.Enabled = false;
			this.cbReferencePoint.Location = new System.Drawing.Point(6, 85);
			this.cbReferencePoint.Name = "cbReferencePoint";
			this.cbReferencePoint.Size = new System.Drawing.Size(145, 23);
			this.cbReferencePoint.TabIndex = 4;
			this.cbReferencePoint.Text = "Referenzpunkt festlegen";
			this.cbReferencePoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cbReferencePoint.UseVisualStyleBackColor = true;
			this.cbReferencePoint.CheckedChanged += new System.EventHandler(this.cbReferencePoint_CheckedChanged);
			// 
			// cbUnheatedTextual
			// 
			this.cbUnheatedTextual.AutoSize = true;
			this.cbUnheatedTextual.Location = new System.Drawing.Point(12, 41);
			this.cbUnheatedTextual.Name = "cbUnheatedTextual";
			this.cbUnheatedTextual.Size = new System.Drawing.Size(101, 17);
			this.cbUnheatedTextual.TabIndex = 5;
			this.cbUnheatedTextual.Text = "Größe eingeben";
			this.cbUnheatedTextual.UseVisualStyleBackColor = true;
			this.cbUnheatedTextual.CheckedChanged += new System.EventHandler(this.cbUnheatedTextual_CheckedChanged);
			// 
			// cbUnheatedGraphical
			// 
			this.cbUnheatedGraphical.AutoSize = true;
			this.cbUnheatedGraphical.Checked = true;
			this.cbUnheatedGraphical.Location = new System.Drawing.Point(12, 22);
			this.cbUnheatedGraphical.Name = "cbUnheatedGraphical";
			this.cbUnheatedGraphical.Size = new System.Drawing.Size(108, 17);
			this.cbUnheatedGraphical.TabIndex = 4;
			this.cbUnheatedGraphical.TabStop = true;
			this.cbUnheatedGraphical.Text = "grafisch festlegen";
			this.cbUnheatedGraphical.UseVisualStyleBackColor = true;
			this.cbUnheatedGraphical.CheckedChanged += new System.EventHandler(this.cbUnheatedGraphical_CheckedChanged);
			// 
			// lblAddUnheatedArea
			// 
			this.lblAddUnheatedArea.Location = new System.Drawing.Point(3, 6);
			this.lblAddUnheatedArea.Name = "lblAddUnheatedArea";
			this.lblAddUnheatedArea.Size = new System.Drawing.Size(136, 13);
			this.lblAddUnheatedArea.TabIndex = 4;
			this.lblAddUnheatedArea.Text = "Neue Unbeheizte Fläche";
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(0, 25);
			this.panel.Name = "panel";
			this.panel.PlanCursor = System.Windows.Forms.Cursors.SizeAll;
			this.panel.ProductPlanner = this.roomPicker;
			this.panel.Size = new System.Drawing.Size(679, 437);
			this.panel.TabIndex = 2;
			// 
			// roomPicker
			// 
			this.roomPicker.Mode = Europlan.Common.RoomPicker.RoomPickerMode.RPM_NONE;
			this.roomPicker.RoomCoordinates = ((System.Collections.Generic.List<WW.Math.Point2D>)(resources.GetObject("roomPicker.RoomCoordinates")));
			this.roomPicker.UnusedCoordinates = ((System.Collections.Generic.List<System.Collections.Generic.List<WW.Math.Point2D>>)(resources.GetObject("roomPicker.UnusedCoordinates")));
			this.roomPicker.ModeChanged += new System.EventHandler(this.roomPicker_ModeChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnOk);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 433);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(679, 29);
			this.panel1.TabIndex = 4;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(581, 3);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(95, 23);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "Übernehmen";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// RoomPickerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(679, 462);
			this.Controls.Add(this.panUnheatedArea);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.toolStrip);
			this.DoubleBuffered = true;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 500);
			this.Name = "RoomPickerForm";
			this.Text = "Raumtypen";
			this.Load += new System.EventHandler(this.ImagePlanOptionsForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImagePlanOptionsForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.panUnheatedArea.ResumeLayout(false);
			this.panUnheatedArea.PerformLayout();
			this.grpSize.ResumeLayout(false);
			this.grpSize.PerformLayout();
			this.grpDistance.ResumeLayout(false);
			this.grpDistance.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnPickRoom;
		private RoomPicker roomPicker;
		private PlanPanel panel;
		private System.Windows.Forms.ToolStripButton btnPickUnused;
		private System.Windows.Forms.ToolStripButton btnDelUnused;
		private System.Windows.Forms.Panel panUnheatedArea;
		private System.Windows.Forms.RadioButton cbUnheatedTextual;
		private System.Windows.Forms.RadioButton cbUnheatedGraphical;
		private System.Windows.Forms.Label lblAddUnheatedArea;
		private System.Windows.Forms.CheckBox cbReferencePoint;
		private System.Windows.Forms.GroupBox grpDistance;
		private System.Windows.Forms.Button btnAddUnheatedArea;
		private System.Windows.Forms.GroupBox grpSize;
		private System.Windows.Forms.Label lblSizeYUnit;
		private NumericBox numSizeY;
		private System.Windows.Forms.Label lblSizeY;
		private System.Windows.Forms.Label lblSizeXUnit;
		private NumericBox numSizeX;
		private System.Windows.Forms.Label lblSizeX;
		private System.Windows.Forms.Label lblDistanceYUnit;
		private NumericBox numDistanceY;
		private System.Windows.Forms.Label lblDistanceY;
		private System.Windows.Forms.Label lblDistanceXUnit;
		private NumericBox numDistanceX;
		private System.Windows.Forms.Label lblDistanceX;
		private System.Windows.Forms.CheckBox cbEnterArea;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.ToolStripButton btnAddExpansionGap;
		private System.Windows.Forms.ToolStripButton btnRemoveExpansionGap;


	}
}