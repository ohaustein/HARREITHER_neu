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
			this.roomPicker = new Europlan.Common.RoomPicker(this.components);
			this.panel = new Europlan.Common.PlanPanel();
			this.toolStrip.SuspendLayout();
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
            this.btnDelUnused});
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
			// roomPicker
			// 
			this.roomPicker.Mode = Europlan.Common.RoomPicker.RoomPickerMode.RPM_NONE;
			this.roomPicker.RoomCoordinates = ((System.Collections.Generic.List<WW.Math.Point2D>)(resources.GetObject("roomPicker.RoomCoordinates")));
			this.roomPicker.UnusedCoordinates = ((System.Collections.Generic.List<System.Collections.Generic.List<WW.Math.Point2D>>)(resources.GetObject("roomPicker.UnusedCoordinates")));
			this.roomPicker.ModeChanged += new System.EventHandler(this.roomPicker_ModeChanged);
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(0, 25);
			this.panel.Name = "panel";
			this.panel.ProductPlanner = this.roomPicker;
			this.panel.Size = new System.Drawing.Size(668, 406);
			this.panel.TabIndex = 2;
			// 
			// RoomPickerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(668, 431);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.toolStrip);
			this.DoubleBuffered = true;
			this.MinimizeBox = false;
			this.Name = "RoomPickerForm";
			this.Text = "Raumtypen";
			this.Load += new System.EventHandler(this.ImagePlanOptionsForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImagePlanOptionsForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
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


	}
}