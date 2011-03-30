namespace Europlan.Common.Products {
	partial class EurovalPlannerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EurovalPlannerForm));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnMove = new System.Windows.Forms.ToolStripButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabs = new System.Windows.Forms.TabControl();
			this.pageConstruction = new System.Windows.Forms.TabPage();
			this.pageCalculations = new System.Windows.Forms.TabPage();
			this.button1 = new System.Windows.Forms.Button();
			this.lstError = new System.Windows.Forms.ListView();
			this.defaultColumn = new System.Windows.Forms.ColumnHeader();
			this.lblQAnbCoolUnit = new System.Windows.Forms.Label();
			this.lblQAnbHeatUnit = new System.Windows.Forms.Label();
			this.lblQAnbCool = new System.Windows.Forms.Label();
			this.lblQAnbHeat = new System.Windows.Forms.Label();
			this.label45 = new System.Windows.Forms.Label();
			this.lblQCoolRestUnit = new System.Windows.Forms.Label();
			this.lblQCoolDiffUnit = new System.Windows.Forms.Label();
			this.lblQCoolUnit = new System.Windows.Forms.Label();
			this.lblQHeatRestUnit = new System.Windows.Forms.Label();
			this.lblQHeatDiffUnit = new System.Windows.Forms.Label();
			this.lblQHeatUnit = new System.Windows.Forms.Label();
			this.lblQCoolRest = new System.Windows.Forms.Label();
			this.lblQCoolDiff = new System.Windows.Forms.Label();
			this.lblQCool = new System.Windows.Forms.Label();
			this.lblQHeatRest = new System.Windows.Forms.Label();
			this.lblQHeatDiff = new System.Windows.Forms.Label();
			this.lblQHeat = new System.Windows.Forms.Label();
			this.lblRest = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.planPanel = new Europlan.Common.PlanPanel();
			this.eurovalPlanner = new Europlan.Common.EurovalPlanner(this.components);
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.toolStrip.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabs.SuspendLayout();
			this.pageCalculations.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomOut,
            this.btnZoomIn,
            this.toolStripSeparator1,
            this.btnMove});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(846, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
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
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.tabs);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 261);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(846, 204);
			this.panel1.TabIndex = 2;
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.pageConstruction);
			this.tabs.Controls.Add(this.pageCalculations);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(846, 204);
			this.tabs.TabIndex = 2;
			this.tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabs_Selecting);
			this.tabs.Deselected += new System.Windows.Forms.TabControlEventHandler(this.tabs_Deselected);
			// 
			// pageConstruction
			// 
			this.pageConstruction.Location = new System.Drawing.Point(4, 22);
			this.pageConstruction.Name = "pageConstruction";
			this.pageConstruction.Padding = new System.Windows.Forms.Padding(3);
			this.pageConstruction.Size = new System.Drawing.Size(838, 178);
			this.pageConstruction.TabIndex = 3;
			this.pageConstruction.Text = "Konstruktion einrichten";
			this.pageConstruction.UseVisualStyleBackColor = true;
			// 
			// pageCalculations
			// 
			this.pageCalculations.Controls.Add(this.button1);
			this.pageCalculations.Controls.Add(this.lstError);
			this.pageCalculations.Controls.Add(this.lblQAnbCoolUnit);
			this.pageCalculations.Controls.Add(this.lblQAnbHeatUnit);
			this.pageCalculations.Controls.Add(this.lblQAnbCool);
			this.pageCalculations.Controls.Add(this.lblQAnbHeat);
			this.pageCalculations.Controls.Add(this.label45);
			this.pageCalculations.Controls.Add(this.lblQCoolRestUnit);
			this.pageCalculations.Controls.Add(this.lblQCoolDiffUnit);
			this.pageCalculations.Controls.Add(this.lblQCoolUnit);
			this.pageCalculations.Controls.Add(this.lblQHeatRestUnit);
			this.pageCalculations.Controls.Add(this.lblQHeatDiffUnit);
			this.pageCalculations.Controls.Add(this.lblQHeatUnit);
			this.pageCalculations.Controls.Add(this.lblQCoolRest);
			this.pageCalculations.Controls.Add(this.lblQCoolDiff);
			this.pageCalculations.Controls.Add(this.lblQCool);
			this.pageCalculations.Controls.Add(this.lblQHeatRest);
			this.pageCalculations.Controls.Add(this.lblQHeatDiff);
			this.pageCalculations.Controls.Add(this.lblQHeat);
			this.pageCalculations.Controls.Add(this.lblRest);
			this.pageCalculations.Controls.Add(this.label16);
			this.pageCalculations.Controls.Add(this.label17);
			this.pageCalculations.Controls.Add(this.groupBox3);
			this.pageCalculations.Controls.Add(this.groupBox2);
			this.pageCalculations.Controls.Add(this.groupBox7);
			this.pageCalculations.Controls.Add(this.label11);
			this.pageCalculations.Controls.Add(this.label10);
			this.pageCalculations.Controls.Add(this.label6);
			this.pageCalculations.Location = new System.Drawing.Point(4, 22);
			this.pageCalculations.Name = "pageCalculations";
			this.pageCalculations.Padding = new System.Windows.Forms.Padding(3);
			this.pageCalculations.Size = new System.Drawing.Size(884, 178);
			this.pageCalculations.TabIndex = 2;
			this.pageCalculations.Text = "Berechnungsergebnisse";
			this.pageCalculations.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(276, 147);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 170;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// lstError
			// 
			this.lstError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstError.BackColor = System.Drawing.SystemColors.Window;
			this.lstError.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstError.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.defaultColumn});
			this.lstError.FullRowSelect = true;
			this.lstError.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lstError.LabelWrap = false;
			this.lstError.Location = new System.Drawing.Point(417, 6);
			this.lstError.MinimumSize = new System.Drawing.Size(200, 164);
			this.lstError.Name = "lstError";
			this.lstError.ShowGroups = false;
			this.lstError.Size = new System.Drawing.Size(451, 164);
			this.lstError.TabIndex = 169;
			this.lstError.UseCompatibleStateImageBehavior = false;
			this.lstError.View = System.Windows.Forms.View.Details;
			this.lstError.Visible = false;
			// 
			// lblQAnbCoolUnit
			// 
			this.lblQAnbCoolUnit.Location = new System.Drawing.Point(376, 83);
			this.lblQAnbCoolUnit.Name = "lblQAnbCoolUnit";
			this.lblQAnbCoolUnit.Size = new System.Drawing.Size(35, 13);
			this.lblQAnbCoolUnit.TabIndex = 165;
			this.lblQAnbCoolUnit.Text = "W";
			// 
			// lblQAnbHeatUnit
			// 
			this.lblQAnbHeatUnit.Location = new System.Drawing.Point(273, 83);
			this.lblQAnbHeatUnit.Name = "lblQAnbHeatUnit";
			this.lblQAnbHeatUnit.Size = new System.Drawing.Size(35, 13);
			this.lblQAnbHeatUnit.TabIndex = 164;
			this.lblQAnbHeatUnit.Text = "W";
			// 
			// lblQAnbCool
			// 
			this.lblQAnbCool.Location = new System.Drawing.Point(320, 83);
			this.lblQAnbCool.Name = "lblQAnbCool";
			this.lblQAnbCool.Size = new System.Drawing.Size(50, 13);
			this.lblQAnbCool.TabIndex = 163;
			this.lblQAnbCool.Text = "0";
			this.lblQAnbCool.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblQAnbHeat
			// 
			this.lblQAnbHeat.Location = new System.Drawing.Point(217, 83);
			this.lblQAnbHeat.Name = "lblQAnbHeat";
			this.lblQAnbHeat.Size = new System.Drawing.Size(50, 13);
			this.lblQAnbHeat.TabIndex = 162;
			this.lblQAnbHeat.Text = "0";
			this.lblQAnbHeat.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label45
			// 
			this.label45.Location = new System.Drawing.Point(4, 83);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(200, 13);
			this.label45.TabIndex = 161;
			this.label45.Text = "Leistung Anbindeleitungen:";
			// 
			// lblQCoolRestUnit
			// 
			this.lblQCoolRestUnit.Location = new System.Drawing.Point(376, 123);
			this.lblQCoolRestUnit.Name = "lblQCoolRestUnit";
			this.lblQCoolRestUnit.Size = new System.Drawing.Size(35, 13);
			this.lblQCoolRestUnit.TabIndex = 160;
			this.lblQCoolRestUnit.Text = "W";
			// 
			// lblQCoolDiffUnit
			// 
			this.lblQCoolDiffUnit.Location = new System.Drawing.Point(376, 103);
			this.lblQCoolDiffUnit.Name = "lblQCoolDiffUnit";
			this.lblQCoolDiffUnit.Size = new System.Drawing.Size(35, 13);
			this.lblQCoolDiffUnit.TabIndex = 159;
			this.lblQCoolDiffUnit.Text = "W";
			// 
			// lblQCoolUnit
			// 
			this.lblQCoolUnit.Location = new System.Drawing.Point(376, 63);
			this.lblQCoolUnit.Name = "lblQCoolUnit";
			this.lblQCoolUnit.Size = new System.Drawing.Size(35, 13);
			this.lblQCoolUnit.TabIndex = 158;
			this.lblQCoolUnit.Text = "W";
			// 
			// lblQHeatRestUnit
			// 
			this.lblQHeatRestUnit.Location = new System.Drawing.Point(273, 123);
			this.lblQHeatRestUnit.Name = "lblQHeatRestUnit";
			this.lblQHeatRestUnit.Size = new System.Drawing.Size(35, 13);
			this.lblQHeatRestUnit.TabIndex = 157;
			this.lblQHeatRestUnit.Text = "W";
			// 
			// lblQHeatDiffUnit
			// 
			this.lblQHeatDiffUnit.Location = new System.Drawing.Point(273, 103);
			this.lblQHeatDiffUnit.Name = "lblQHeatDiffUnit";
			this.lblQHeatDiffUnit.Size = new System.Drawing.Size(35, 13);
			this.lblQHeatDiffUnit.TabIndex = 156;
			this.lblQHeatDiffUnit.Text = "W";
			// 
			// lblQHeatUnit
			// 
			this.lblQHeatUnit.Location = new System.Drawing.Point(273, 63);
			this.lblQHeatUnit.Name = "lblQHeatUnit";
			this.lblQHeatUnit.Size = new System.Drawing.Size(35, 13);
			this.lblQHeatUnit.TabIndex = 155;
			this.lblQHeatUnit.Text = "W";
			// 
			// lblQCoolRest
			// 
			this.lblQCoolRest.Location = new System.Drawing.Point(320, 123);
			this.lblQCoolRest.Name = "lblQCoolRest";
			this.lblQCoolRest.Size = new System.Drawing.Size(50, 13);
			this.lblQCoolRest.TabIndex = 154;
			this.lblQCoolRest.Text = "0";
			this.lblQCoolRest.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblQCoolDiff
			// 
			this.lblQCoolDiff.Location = new System.Drawing.Point(320, 103);
			this.lblQCoolDiff.Name = "lblQCoolDiff";
			this.lblQCoolDiff.Size = new System.Drawing.Size(50, 13);
			this.lblQCoolDiff.TabIndex = 153;
			this.lblQCoolDiff.Text = "0";
			this.lblQCoolDiff.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblQCool
			// 
			this.lblQCool.Location = new System.Drawing.Point(320, 63);
			this.lblQCool.Name = "lblQCool";
			this.lblQCool.Size = new System.Drawing.Size(50, 13);
			this.lblQCool.TabIndex = 152;
			this.lblQCool.Text = "0";
			this.lblQCool.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblQHeatRest
			// 
			this.lblQHeatRest.Location = new System.Drawing.Point(217, 123);
			this.lblQHeatRest.Name = "lblQHeatRest";
			this.lblQHeatRest.Size = new System.Drawing.Size(50, 13);
			this.lblQHeatRest.TabIndex = 151;
			this.lblQHeatRest.Text = "0";
			this.lblQHeatRest.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblQHeatDiff
			// 
			this.lblQHeatDiff.Location = new System.Drawing.Point(217, 103);
			this.lblQHeatDiff.Name = "lblQHeatDiff";
			this.lblQHeatDiff.Size = new System.Drawing.Size(50, 13);
			this.lblQHeatDiff.TabIndex = 150;
			this.lblQHeatDiff.Text = "0";
			this.lblQHeatDiff.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblQHeat
			// 
			this.lblQHeat.Location = new System.Drawing.Point(217, 63);
			this.lblQHeat.Name = "lblQHeat";
			this.lblQHeat.Size = new System.Drawing.Size(50, 13);
			this.lblQHeat.TabIndex = 149;
			this.lblQHeat.Text = "0";
			this.lblQHeat.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblRest
			// 
			this.lblRest.Location = new System.Drawing.Point(4, 123);
			this.lblRest.Name = "lblRest";
			this.lblRest.Size = new System.Drawing.Size(200, 26);
			this.lblRest.TabIndex = 148;
			this.lblRest.Text = "Rest (Raum 1)";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(4, 103);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(200, 13);
			this.label16.TabIndex = 147;
			this.label16.Text = "Differenz zur erwarteten Leistung:";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(4, 63);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(200, 13);
			this.label17.TabIndex = 146;
			this.label17.Text = "Erreichte Leistung:";
			// 
			// groupBox3
			// 
			this.groupBox3.Location = new System.Drawing.Point(313, 31);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(2, 110);
			this.groupBox3.TabIndex = 145;
			this.groupBox3.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(210, 31);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(2, 110);
			this.groupBox2.TabIndex = 143;
			this.groupBox2.TabStop = false;
			// 
			// groupBox7
			// 
			this.groupBox7.Location = new System.Drawing.Point(3, 47);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(390, 2);
			this.groupBox7.TabIndex = 142;
			this.groupBox7.TabStop = false;
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(321, 5);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(92, 39);
			this.label11.TabIndex = 141;
			this.label11.Text = "Kühlbetrieb";
			this.label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(216, 5);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(92, 39);
			this.label10.TabIndex = 140;
			this.label10.Text = "Heizbetrieb";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(4, 5);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(200, 39);
			this.label6.TabIndex = 139;
			this.label6.Text = "Berechnungsergebnisse:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// planPanel
			// 
			this.planPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planPanel.Location = new System.Drawing.Point(0, 25);
			this.planPanel.Name = "planPanel";
			this.planPanel.PlanCursor = System.Windows.Forms.Cursors.SizeAll;
			this.planPanel.ProductPlanner = this.eurovalPlanner;
			this.planPanel.Size = new System.Drawing.Size(846, 236);
			this.planPanel.TabIndex = 0;
			// 
			// eurovalPlanner
			// 
			this.eurovalPlanner.Mode = Europlan.Common.EurovalPlanner.EurovalMode.EVM_NONE;

			this.eurovalPlanner.ModeChanged += new System.EventHandler<System.EventArgs>(this.eurovalPlanner_ModeChanged);
			this.eurovalPlanner.ProjectChanged += new Europlan.Common.ProjectChangedHandler(this.europlanPlanner_ProjectChanged);
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
			// EurovalPlannerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(846, 465);
			this.Controls.Add(this.planPanel);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip);
			this.Name = "EurovalPlannerForm";
			this.Text = "Modul Klima-Boden - grafische Auslegung";
			this.Load += new System.EventHandler(this.EurovalPlannerForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EurovalPlannerForm_FormClosing);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.tabs.ResumeLayout(false);
			this.pageCalculations.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private EurovalPlanner eurovalPlanner;
		private PlanPanel planPanel;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnMove;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage pageCalculations;
		private System.Windows.Forms.Label lblQAnbCoolUnit;
		private System.Windows.Forms.Label lblQAnbHeatUnit;
		private System.Windows.Forms.Label lblQAnbCool;
		private System.Windows.Forms.Label lblQAnbHeat;
		private System.Windows.Forms.Label label45;
		private System.Windows.Forms.Label lblQCoolRestUnit;
		private System.Windows.Forms.Label lblQCoolDiffUnit;
		private System.Windows.Forms.Label lblQCoolUnit;
		private System.Windows.Forms.Label lblQHeatRestUnit;
		private System.Windows.Forms.Label lblQHeatDiffUnit;
		private System.Windows.Forms.Label lblQHeatUnit;
		private System.Windows.Forms.Label lblQCoolRest;
		private System.Windows.Forms.Label lblQCoolDiff;
		private System.Windows.Forms.Label lblQCool;
		private System.Windows.Forms.Label lblQHeatRest;
		private System.Windows.Forms.Label lblQHeatDiff;
		private System.Windows.Forms.Label lblQHeat;
		private System.Windows.Forms.Label lblRest;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListView lstError;
		private System.Windows.Forms.ColumnHeader defaultColumn;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TabPage pageConstruction;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
	}
}