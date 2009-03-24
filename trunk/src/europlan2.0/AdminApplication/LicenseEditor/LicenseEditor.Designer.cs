namespace Europlan.AdminApplication {
	partial class LicenseEditor {
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
			this.btnAddSystem = new System.Windows.Forms.Button();
			this.txtSystem = new System.Windows.Forms.TextBox();
			this.lstSystems = new System.Windows.Forms.ListView();
			this.lblSystem = new System.Windows.Forms.Label();
			this.lstModules = new System.Windows.Forms.ListView();
			this.lblModules = new System.Windows.Forms.Label();
			this.lblHeader = new System.Windows.Forms.Label();
			this.txtHeader = new System.Windows.Forms.TextBox();
			this.lblLicensedTo = new System.Windows.Forms.Label();
			this.txtLicensedTo = new System.Windows.Forms.TextBox();
			this.btnGenerateKey = new System.Windows.Forms.Button();
			this.lblKey = new System.Windows.Forms.Label();
			this.txtKey = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnAddSystem
			// 
			this.btnAddSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddSystem.Location = new System.Drawing.Point(439, 432);
			this.btnAddSystem.Name = "btnAddSystem";
			this.btnAddSystem.Size = new System.Drawing.Size(109, 23);
			this.btnAddSystem.TabIndex = 26;
			this.btnAddSystem.Text = "Neuer Rechner";
			this.btnAddSystem.UseVisualStyleBackColor = true;
			// 
			// txtSystem
			// 
			this.txtSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSystem.BackColor = System.Drawing.SystemColors.Window;
			this.txtSystem.Enabled = false;
			this.txtSystem.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtSystem.Location = new System.Drawing.Point(147, 434);
			this.txtSystem.Name = "txtSystem";
			this.txtSystem.ReadOnly = true;
			this.txtSystem.Size = new System.Drawing.Size(286, 20);
			this.txtSystem.TabIndex = 25;
			// 
			// lstSystems
			// 
			this.lstSystems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstSystems.Location = new System.Drawing.Point(147, 307);
			this.lstSystems.Name = "lstSystems";
			this.lstSystems.Size = new System.Drawing.Size(401, 119);
			this.lstSystems.TabIndex = 24;
			this.lstSystems.UseCompatibleStateImageBehavior = false;
			this.lstSystems.View = System.Windows.Forms.View.List;
			// 
			// lblSystem
			// 
			this.lblSystem.Location = new System.Drawing.Point(4, 307);
			this.lblSystem.Name = "lblSystem";
			this.lblSystem.Size = new System.Drawing.Size(137, 16);
			this.lblSystem.TabIndex = 23;
			this.lblSystem.Text = "Rechner:";
			this.lblSystem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lstModules
			// 
			this.lstModules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstModules.CheckBoxes = true;
			this.lstModules.Location = new System.Drawing.Point(147, 182);
			this.lstModules.Name = "lstModules";
			this.lstModules.Size = new System.Drawing.Size(401, 119);
			this.lstModules.TabIndex = 22;
			this.lstModules.UseCompatibleStateImageBehavior = false;
			this.lstModules.View = System.Windows.Forms.View.List;
			// 
			// lblModules
			// 
			this.lblModules.Location = new System.Drawing.Point(4, 182);
			this.lblModules.Name = "lblModules";
			this.lblModules.Size = new System.Drawing.Size(137, 16);
			this.lblModules.TabIndex = 21;
			this.lblModules.Text = "Module:";
			this.lblModules.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblHeader
			// 
			this.lblHeader.Location = new System.Drawing.Point(4, 58);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(137, 16);
			this.lblHeader.TabIndex = 20;
			this.lblHeader.Text = "Kopf:";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtHeader
			// 
			this.txtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHeader.BackColor = System.Drawing.SystemColors.Window;
			this.txtHeader.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtHeader.Location = new System.Drawing.Point(147, 57);
			this.txtHeader.Multiline = true;
			this.txtHeader.Name = "txtHeader";
			this.txtHeader.Size = new System.Drawing.Size(401, 118);
			this.txtHeader.TabIndex = 19;
			// 
			// lblLicensedTo
			// 
			this.lblLicensedTo.Location = new System.Drawing.Point(4, 32);
			this.lblLicensedTo.Name = "lblLicensedTo";
			this.lblLicensedTo.Size = new System.Drawing.Size(137, 16);
			this.lblLicensedTo.TabIndex = 18;
			this.lblLicensedTo.Text = "Lizenznehmer:";
			this.lblLicensedTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtLicensedTo
			// 
			this.txtLicensedTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLicensedTo.BackColor = System.Drawing.SystemColors.Window;
			this.txtLicensedTo.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtLicensedTo.Location = new System.Drawing.Point(147, 31);
			this.txtLicensedTo.Name = "txtLicensedTo";
			this.txtLicensedTo.Size = new System.Drawing.Size(401, 20);
			this.txtLicensedTo.TabIndex = 17;
			// 
			// btnGenerateKey
			// 
			this.btnGenerateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGenerateKey.Location = new System.Drawing.Point(439, 3);
			this.btnGenerateKey.Name = "btnGenerateKey";
			this.btnGenerateKey.Size = new System.Drawing.Size(109, 23);
			this.btnGenerateKey.TabIndex = 16;
			this.btnGenerateKey.Text = "Neuer Schlüssel";
			this.btnGenerateKey.UseVisualStyleBackColor = true;
			this.btnGenerateKey.Click += new System.EventHandler(this.btnGenerateKey_Click);
			// 
			// lblKey
			// 
			this.lblKey.Location = new System.Drawing.Point(4, 6);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(137, 16);
			this.lblKey.TabIndex = 15;
			this.lblKey.Text = "Lizenzschlüssel:";
			this.lblKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtKey
			// 
			this.txtKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtKey.BackColor = System.Drawing.SystemColors.Window;
			this.txtKey.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtKey.Location = new System.Drawing.Point(147, 5);
			this.txtKey.Name = "txtKey";
			this.txtKey.ReadOnly = true;
			this.txtKey.Size = new System.Drawing.Size(286, 20);
			this.txtKey.TabIndex = 14;
			this.txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKey_KeyDown);
			// 
			// LicenseEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(400, 0);
			this.Controls.Add(this.btnAddSystem);
			this.Controls.Add(this.txtSystem);
			this.Controls.Add(this.lstSystems);
			this.Controls.Add(this.lblSystem);
			this.Controls.Add(this.lstModules);
			this.Controls.Add(this.lblModules);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.txtHeader);
			this.Controls.Add(this.lblLicensedTo);
			this.Controls.Add(this.txtLicensedTo);
			this.Controls.Add(this.btnGenerateKey);
			this.Controls.Add(this.lblKey);
			this.Controls.Add(this.txtKey);
			this.Name = "LicenseEditor";
			this.Size = new System.Drawing.Size(551, 464);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnAddSystem;
		private System.Windows.Forms.TextBox txtSystem;
		private System.Windows.Forms.ListView lstSystems;
		private System.Windows.Forms.Label lblSystem;
		private System.Windows.Forms.ListView lstModules;
		private System.Windows.Forms.Label lblModules;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TextBox txtHeader;
		private System.Windows.Forms.Label lblLicensedTo;
		private System.Windows.Forms.TextBox txtLicensedTo;
		private System.Windows.Forms.Button btnGenerateKey;
		private System.Windows.Forms.Label lblKey;
		private System.Windows.Forms.TextBox txtKey;
	}
}
