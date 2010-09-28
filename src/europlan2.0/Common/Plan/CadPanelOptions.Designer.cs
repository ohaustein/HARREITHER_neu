namespace Europlan.Common {
	partial class CadPanelOptions {
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
			this.lstLayers = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// lstLayers
			// 
			this.lstLayers.CheckBoxes = true;
			this.lstLayers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstLayers.Location = new System.Drawing.Point(0, 0);
			this.lstLayers.Name = "lstLayers";
			this.lstLayers.Size = new System.Drawing.Size(317, 370);
			this.lstLayers.TabIndex = 1;
			this.lstLayers.UseCompatibleStateImageBehavior = false;
			this.lstLayers.View = System.Windows.Forms.View.List;
			// 
			// CadPanelOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lstLayers);
			this.Name = "CadPanelOptions";
			this.Size = new System.Drawing.Size(317, 370);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lstLayers;
	}
}
