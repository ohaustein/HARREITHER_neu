namespace Europlan.Common {
	partial class AuslegeAssistentForm {
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
			this.label1 = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeProducts = new System.Windows.Forms.TreeView();
			this.graphicsPanel = new System.Windows.Forms.Panel();
			this.chkEuroval = new System.Windows.Forms.CheckBox();
			this.chkEcotherm = new System.Windows.Forms.CheckBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(498, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Die beiden Diagramme stellen die notwendigen Verlegearten für eine Variation der " +
				"Vorlauftemperatur dar.";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 84);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeProducts);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.graphicsPanel);
			this.splitContainer1.Size = new System.Drawing.Size(722, 419);
			this.splitContainer1.SplitterDistance = 240;
			this.splitContainer1.TabIndex = 1;
			// 
			// treeProducts
			// 
			this.treeProducts.CheckBoxes = true;
			this.treeProducts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeProducts.HideSelection = false;
			this.treeProducts.Location = new System.Drawing.Point(0, 0);
			this.treeProducts.Name = "treeProducts";
			this.treeProducts.Size = new System.Drawing.Size(240, 419);
			this.treeProducts.TabIndex = 0;
			this.treeProducts.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeProducts_AfterCheck);
			this.treeProducts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeProducts_AfterSelect);
			// 
			// graphicsPanel
			// 
			this.graphicsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.graphicsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphicsPanel.Location = new System.Drawing.Point(0, 0);
			this.graphicsPanel.Name = "graphicsPanel";
			this.graphicsPanel.Size = new System.Drawing.Size(478, 419);
			this.graphicsPanel.TabIndex = 0;
			this.graphicsPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.graphicsPanel_Paint);
			// 
			// chkEuroval
			// 
			this.chkEuroval.AutoSize = true;
			this.chkEuroval.Checked = true;
			this.chkEuroval.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEuroval.Location = new System.Drawing.Point(29, 38);
			this.chkEuroval.Name = "chkEuroval";
			this.chkEuroval.Size = new System.Drawing.Size(62, 17);
			this.chkEuroval.TabIndex = 2;
			this.chkEuroval.Text = "Euroval";
			this.chkEuroval.UseVisualStyleBackColor = true;
			this.chkEuroval.CheckStateChanged += new System.EventHandler(this.chk_CheckStateChanged);
			// 
			// chkEcotherm
			// 
			this.chkEcotherm.AutoSize = true;
			this.chkEcotherm.Checked = true;
			this.chkEcotherm.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEcotherm.Location = new System.Drawing.Point(29, 61);
			this.chkEcotherm.Name = "chkEcotherm";
			this.chkEcotherm.Size = new System.Drawing.Size(71, 17);
			this.chkEcotherm.TabIndex = 3;
			this.chkEcotherm.Text = "Ecotherm";
			this.chkEcotherm.UseVisualStyleBackColor = true;
			this.chkEcotherm.CheckStateChanged += new System.EventHandler(this.chk_CheckStateChanged);
			// 
			// AuslegeAssistentForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(746, 515);
			this.Controls.Add(this.chkEcotherm);
			this.Controls.Add(this.chkEuroval);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.label1);
			this.Name = "AuslegeAssistentForm";
			this.Text = "Auslegehilfe";
			this.Load += new System.EventHandler(this.AuslegeAssistentForm_Load);
			this.ResizeBegin += new System.EventHandler(this.AuslegeAssistentForm_ResizeBegin);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AuslegeAssistentForm_FormClosing);
			this.ResizeEnd += new System.EventHandler(this.AuslegeAssistentForm_ResizeEnd);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView treeProducts;
		private System.Windows.Forms.Panel graphicsPanel;
		private System.Windows.Forms.CheckBox chkEuroval;
		private System.Windows.Forms.CheckBox chkEcotherm;
	}
}