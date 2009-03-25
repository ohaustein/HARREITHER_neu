namespace Europlan.Application {
	partial class LicenseForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
			this.btnOk = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtLicensedTo = new System.Windows.Forms.TextBox();
			this.txtHeader = new System.Windows.Forms.TextBox();
			this.btnImport = new System.Windows.Forms.Button();
			this.lblHeader = new System.Windows.Forms.Label();
			this.txtValidUntil = new System.Windows.Forms.TextBox();
			this.lblValidUntil = new System.Windows.Forms.Label();
			this.lstModule = new System.Windows.Forms.ListView();
			this.lblModules = new System.Windows.Forms.Label();
			this.lblValid = new System.Windows.Forms.Label();
			this.txtValid = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// txtLicensedTo
			// 
			resources.ApplyResources(this.txtLicensedTo, "txtLicensedTo");
			this.txtLicensedTo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtLicensedTo.Name = "txtLicensedTo";
			this.txtLicensedTo.ReadOnly = true;
			// 
			// txtHeader
			// 
			resources.ApplyResources(this.txtHeader, "txtHeader");
			this.txtHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtHeader.Name = "txtHeader";
			this.txtHeader.ReadOnly = true;
			// 
			// btnImport
			// 
			resources.ApplyResources(this.btnImport, "btnImport");
			this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnImport.Name = "btnImport";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// lblHeader
			// 
			resources.ApplyResources(this.lblHeader, "lblHeader");
			this.lblHeader.Name = "lblHeader";
			// 
			// txtValidUntil
			// 
			resources.ApplyResources(this.txtValidUntil, "txtValidUntil");
			this.txtValidUntil.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtValidUntil.Name = "txtValidUntil";
			this.txtValidUntil.ReadOnly = true;
			// 
			// lblValidUntil
			// 
			resources.ApplyResources(this.lblValidUntil, "lblValidUntil");
			this.lblValidUntil.Name = "lblValidUntil";
			// 
			// lstModule
			// 
			resources.ApplyResources(this.lstModule, "lstModule");
			this.lstModule.BackColor = System.Drawing.SystemColors.Control;
			this.lstModule.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstModule.MultiSelect = false;
			this.lstModule.Name = "lstModule";
			this.lstModule.UseCompatibleStateImageBehavior = false;
			this.lstModule.View = System.Windows.Forms.View.List;
			// 
			// lblModules
			// 
			resources.ApplyResources(this.lblModules, "lblModules");
			this.lblModules.Name = "lblModules";
			// 
			// lblValid
			// 
			resources.ApplyResources(this.lblValid, "lblValid");
			this.lblValid.Name = "lblValid";
			// 
			// txtValid
			// 
			resources.ApplyResources(this.txtValid, "txtValid");
			this.txtValid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtValid.Name = "txtValid";
			this.txtValid.ReadOnly = true;
			// 
			// LicenseForm
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ControlBox = false;
			this.Controls.Add(this.lblValid);
			this.Controls.Add(this.txtValid);
			this.Controls.Add(this.lblModules);
			this.Controls.Add(this.lstModule);
			this.Controls.Add(this.lblValidUntil);
			this.Controls.Add(this.txtValidUntil);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.txtHeader);
			this.Controls.Add(this.txtLicensedTo);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LicenseForm";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.LicenseForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LicenseForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLicensedTo;
		private System.Windows.Forms.TextBox txtHeader;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TextBox txtValidUntil;
		private System.Windows.Forms.Label lblValidUntil;
		private System.Windows.Forms.ListView lstModule;
		private System.Windows.Forms.Label lblModules;
		private System.Windows.Forms.Label lblValid;
		private System.Windows.Forms.TextBox txtValid;
	}
}