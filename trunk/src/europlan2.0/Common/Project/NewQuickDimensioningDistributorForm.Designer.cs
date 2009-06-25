namespace Europlan.Common {
	partial class NewQuickDimensioningDistributorForm {
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
			this.gridQuickDimensioningDistributor = new Europlan.Common.QuickDimensioningDistributorGrid();
			this.SuspendLayout();
			// 
			// gridQuickDimensioningDistributor
			// 
			this.gridQuickDimensioningDistributor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridQuickDimensioningDistributor.Location = new System.Drawing.Point(0, 0);
			this.gridQuickDimensioningDistributor.Name = "gridQuickDimensioningDistributor";
			this.gridQuickDimensioningDistributor.Size = new System.Drawing.Size(356, 218);
			this.gridQuickDimensioningDistributor.TabIndex = 0;
			// 
			// NewQuickDimensioningDistributorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(356, 218);
			this.Controls.Add(this.gridQuickDimensioningDistributor);
			this.MaximizeBox = false;
			this.Name = "NewQuickDimensioningDistributorForm";
			this.Text = "Verteiler";
			this.Load += new System.EventHandler(this.NewQuickDimensioningDistributorForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewQuickDimensioningDistributorForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private QuickDimensioningDistributorGrid gridQuickDimensioningDistributor;

	}
}