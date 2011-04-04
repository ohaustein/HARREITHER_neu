namespace Europlan.Common {
	partial class ConnectionPlanner {
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
			this.components = new System.ComponentModel.Container();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmVorlauf = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRuecklauf = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmVorlauf,
            this.cmRuecklauf});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(121, 48);
			// 
			// cmVorlauf
			// 
			this.cmVorlauf.Name = "cmVorlauf";
			this.cmVorlauf.Size = new System.Drawing.Size(120, 22);
			this.cmVorlauf.Text = "Vorlauf";
			this.cmVorlauf.Click += new System.EventHandler(this.cmVorRuecklauf);
			// 
			// cmRuecklauf
			// 
			this.cmRuecklauf.Name = "cmRuecklauf";
			this.cmRuecklauf.Size = new System.Drawing.Size(120, 22);
			this.cmRuecklauf.Text = "Rücklauf";
			this.cmRuecklauf.Click += new System.EventHandler(this.cmVorRuecklauf);
			this.contextMenu.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem cmVorlauf;
		private System.Windows.Forms.ToolStripMenuItem cmRuecklauf;
	}
}
