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
			this.lblHardwareId = new System.Windows.Forms.Label();
			this.txtHardwareId = new System.Windows.Forms.TextBox();
			this.panModules = new System.Windows.Forms.Panel();
			this.lblLicenseSystemInvalid = new System.Windows.Forms.Label();
			this.lblLicenseDateInvalid = new System.Windows.Forms.Label();
			this.lblLicenseSignatureInvalid = new System.Windows.Forms.Label();
			this.lblLicenseMissing = new System.Windows.Forms.Label();
			this.lblLicenseInvalidUnknown = new System.Windows.Forms.Label();
			this.panModules.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// txtLicensedTo
			// 
			resources.ApplyResources(this.txtLicensedTo, "txtLicensedTo");
			this.txtLicensedTo.BackColor = System.Drawing.SystemColors.Window;
			this.txtLicensedTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLicensedTo.Name = "txtLicensedTo";
			this.txtLicensedTo.ReadOnly = true;
			// 
			// txtHeader
			// 
			resources.ApplyResources(this.txtHeader, "txtHeader");
			this.txtHeader.BackColor = System.Drawing.SystemColors.Window;
			this.txtHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtHeader.Name = "txtHeader";
			this.txtHeader.ReadOnly = true;
			// 
			// btnImport
			// 
			resources.ApplyResources(this.btnImport, "btnImport");
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
			this.txtValidUntil.BackColor = System.Drawing.SystemColors.Window;
			this.txtValidUntil.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
			this.lstModule.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.lstModule, "lstModule");
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
			// lblHardwareId
			// 
			resources.ApplyResources(this.lblHardwareId, "lblHardwareId");
			this.lblHardwareId.Name = "lblHardwareId";
			// 
			// txtHardwareId
			// 
			resources.ApplyResources(this.txtHardwareId, "txtHardwareId");
			this.txtHardwareId.BackColor = System.Drawing.SystemColors.Window;
			this.txtHardwareId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtHardwareId.Name = "txtHardwareId";
			this.txtHardwareId.ReadOnly = true;
			// 
			// panModules
			// 
			resources.ApplyResources(this.panModules, "panModules");
			this.panModules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panModules.Controls.Add(this.lstModule);
			this.panModules.Name = "panModules";
			// 
			// lblLicenseSystemInvalid
			// 
			resources.ApplyResources(this.lblLicenseSystemInvalid, "lblLicenseSystemInvalid");
			this.lblLicenseSystemInvalid.ForeColor = System.Drawing.Color.Red;
			this.lblLicenseSystemInvalid.Name = "lblLicenseSystemInvalid";
			// 
			// lblLicenseDateInvalid
			// 
			resources.ApplyResources(this.lblLicenseDateInvalid, "lblLicenseDateInvalid");
			this.lblLicenseDateInvalid.ForeColor = System.Drawing.Color.Red;
			this.lblLicenseDateInvalid.Name = "lblLicenseDateInvalid";
			// 
			// lblLicenseSignatureInvalid
			// 
			resources.ApplyResources(this.lblLicenseSignatureInvalid, "lblLicenseSignatureInvalid");
			this.lblLicenseSignatureInvalid.ForeColor = System.Drawing.Color.Red;
			this.lblLicenseSignatureInvalid.Name = "lblLicenseSignatureInvalid";
			// 
			// lblLicenseMissing
			// 
			resources.ApplyResources(this.lblLicenseMissing, "lblLicenseMissing");
			this.lblLicenseMissing.ForeColor = System.Drawing.Color.Red;
			this.lblLicenseMissing.Name = "lblLicenseMissing";
			// 
			// lblLicenseInvalidUnknown
			// 
			resources.ApplyResources(this.lblLicenseInvalidUnknown, "lblLicenseInvalidUnknown");
			this.lblLicenseInvalidUnknown.ForeColor = System.Drawing.Color.Red;
			this.lblLicenseInvalidUnknown.Name = "lblLicenseInvalidUnknown";
			// 
			// LicenseForm
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add(this.lblLicenseInvalidUnknown);
			this.Controls.Add(this.lblLicenseMissing);
			this.Controls.Add(this.lblLicenseSignatureInvalid);
			this.Controls.Add(this.lblLicenseDateInvalid);
			this.Controls.Add(this.lblLicenseSystemInvalid);
			this.Controls.Add(this.panModules);
			this.Controls.Add(this.lblHardwareId);
			this.Controls.Add(this.txtHardwareId);
			this.Controls.Add(this.lblModules);
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
			this.panModules.ResumeLayout(false);
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
		private System.Windows.Forms.Label lblHardwareId;
		private System.Windows.Forms.TextBox txtHardwareId;
		private System.Windows.Forms.Panel panModules;
		private System.Windows.Forms.Label lblLicenseSystemInvalid;
		private System.Windows.Forms.Label lblLicenseDateInvalid;
		private System.Windows.Forms.Label lblLicenseSignatureInvalid;
		private System.Windows.Forms.Label lblLicenseMissing;
		private System.Windows.Forms.Label lblLicenseInvalidUnknown;
	}
}