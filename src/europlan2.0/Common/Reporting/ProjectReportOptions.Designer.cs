namespace Europlan.Common {
	partial class ProjectReportOptions {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectReportOptions));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkProjectOverview = new System.Windows.Forms.CheckBox();
			this.chkAreaOverview = new System.Windows.Forms.CheckBox();
			this.chkAuslegungBilanz = new System.Windows.Forms.CheckBox();
			this.chkAuslegung = new System.Windows.Forms.CheckBox();
			this.chkVerlegedaten = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.chkVerlegedaten);
			this.groupBox1.Controls.Add(this.chkAuslegungBilanz);
			this.groupBox1.Controls.Add(this.chkAuslegung);
			this.groupBox1.Controls.Add(this.chkAreaOverview);
			this.groupBox1.Controls.Add(this.chkProjectOverview);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// chkProjectOverview
			// 
			resources.ApplyResources(this.chkProjectOverview, "chkProjectOverview");
			this.chkProjectOverview.Name = "chkProjectOverview";
			this.chkProjectOverview.UseVisualStyleBackColor = true;
			this.chkProjectOverview.CheckedChanged += new System.EventHandler(this.chkProjectOverview_CheckedChanged);
			// 
			// chkAreaOverview
			// 
			resources.ApplyResources(this.chkAreaOverview, "chkAreaOverview");
			this.chkAreaOverview.Name = "chkAreaOverview";
			this.chkAreaOverview.UseVisualStyleBackColor = true;
			this.chkAreaOverview.CheckedChanged += new System.EventHandler(this.chkAreaOverview_CheckedChanged);
			// 
			// chkAuslegungBilanz
			// 
			resources.ApplyResources(this.chkAuslegungBilanz, "chkAuslegungBilanz");
			this.chkAuslegungBilanz.Name = "chkAuslegungBilanz";
			this.chkAuslegungBilanz.UseVisualStyleBackColor = true;
			this.chkAuslegungBilanz.CheckedChanged += new System.EventHandler(this.chkAuslegungBilanz_CheckedChanged);
			// 
			// chkAuslegung
			// 
			resources.ApplyResources(this.chkAuslegung, "chkAuslegung");
			this.chkAuslegung.Name = "chkAuslegung";
			this.chkAuslegung.UseVisualStyleBackColor = true;
			this.chkAuslegung.CheckedChanged += new System.EventHandler(this.chkAuslegung_CheckedChanged);
			// 
			// chkVerlegedaten
			// 
			resources.ApplyResources(this.chkVerlegedaten, "chkVerlegedaten");
			this.chkVerlegedaten.Name = "chkVerlegedaten";
			this.chkVerlegedaten.UseVisualStyleBackColor = true;
			this.chkVerlegedaten.CheckedChanged += new System.EventHandler(this.chkVerlegedaten_CheckedChanged);
			// 
			// ProjectReportOptions
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Name = "ProjectReportOptions";
			this.Load += new System.EventHandler(this.ProjectReportOptions_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectReportOptions_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkProjectOverview;
		private System.Windows.Forms.CheckBox chkAreaOverview;
		private System.Windows.Forms.CheckBox chkAuslegungBilanz;
		private System.Windows.Forms.CheckBox chkAuslegung;
		private System.Windows.Forms.CheckBox chkVerlegedaten;
	}
}