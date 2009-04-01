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
			this.lstSystems = new System.Windows.Forms.ListView();
			this.colHardwareId = new System.Windows.Forms.ColumnHeader();
			this.colSystemAddedAt = new System.Windows.Forms.ColumnHeader();
			this.colAnnotation = new System.Windows.Forms.ColumnHeader();
			this.lblSystem = new System.Windows.Forms.Label();
			this.lstModules = new System.Windows.Forms.ListView();
			this.lblModules = new System.Windows.Forms.Label();
			this.lblHeader = new System.Windows.Forms.Label();
			this.txtHeader = new System.Windows.Forms.TextBox();
			this.lblLicensedTo = new System.Windows.Forms.Label();
			this.txtLicensedTo = new System.Windows.Forms.TextBox();
			this.lblValidUntil = new System.Windows.Forms.Label();
			this.dtpValidUntil = new System.Windows.Forms.DateTimePicker();
			this.syeCurrentSystem = new Europlan.AdminApplication.SystemEditor();
			this.lblEmail = new System.Windows.Forms.Label();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnAddSystem
			// 
			this.btnAddSystem.Location = new System.Drawing.Point(4, 455);
			this.btnAddSystem.Name = "btnAddSystem";
			this.btnAddSystem.Size = new System.Drawing.Size(134, 23);
			this.btnAddSystem.TabIndex = 26;
			this.btnAddSystem.Text = "Neuer Rechner";
			this.btnAddSystem.UseVisualStyleBackColor = true;
			this.btnAddSystem.Click += new System.EventHandler(this.btnAddSystem_Click);
			// 
			// lstSystems
			// 
			this.lstSystems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstSystems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHardwareId,
            this.colSystemAddedAt,
            this.colAnnotation});
			this.lstSystems.FullRowSelect = true;
			this.lstSystems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstSystems.HideSelection = false;
			this.lstSystems.Location = new System.Drawing.Point(144, 331);
			this.lstSystems.MultiSelect = false;
			this.lstSystems.Name = "lstSystems";
			this.lstSystems.ShowGroups = false;
			this.lstSystems.Size = new System.Drawing.Size(404, 119);
			this.lstSystems.TabIndex = 24;
			this.lstSystems.UseCompatibleStateImageBehavior = false;
			this.lstSystems.View = System.Windows.Forms.View.Details;
			this.lstSystems.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstSystems_ItemSelectionChanged);
			this.lstSystems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstSystems_KeyDown);
			// 
			// colHardwareId
			// 
			this.colHardwareId.DisplayIndex = 1;
			this.colHardwareId.Text = "Hardware ID";
			this.colHardwareId.Width = 130;
			// 
			// colSystemAddedAt
			// 
			this.colSystemAddedAt.DisplayIndex = 0;
			this.colSystemAddedAt.Text = "Hinzugefügt am";
			this.colSystemAddedAt.Width = 90;
			// 
			// colAnnotation
			// 
			this.colAnnotation.Text = "Anmerkung";
			this.colAnnotation.Width = 170;
			// 
			// lblSystem
			// 
			this.lblSystem.Location = new System.Drawing.Point(1, 331);
			this.lblSystem.Name = "lblSystem";
			this.lblSystem.Size = new System.Drawing.Size(137, 16);
			this.lblSystem.TabIndex = 23;
			this.lblSystem.Text = "Lizenzierte Rechner:";
			this.lblSystem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lstModules
			// 
			this.lstModules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstModules.CheckBoxes = true;
			this.lstModules.Location = new System.Drawing.Point(144, 206);
			this.lstModules.Name = "lstModules";
			this.lstModules.Size = new System.Drawing.Size(404, 119);
			this.lstModules.TabIndex = 22;
			this.lstModules.UseCompatibleStateImageBehavior = false;
			this.lstModules.View = System.Windows.Forms.View.List;
			this.lstModules.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstModules_ItemChecked);
			// 
			// lblModules
			// 
			this.lblModules.Location = new System.Drawing.Point(1, 206);
			this.lblModules.Name = "lblModules";
			this.lblModules.Size = new System.Drawing.Size(137, 16);
			this.lblModules.TabIndex = 21;
			this.lblModules.Text = "Lizenzierte Produkte:";
			this.lblModules.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblHeader
			// 
			this.lblHeader.Location = new System.Drawing.Point(1, 82);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(137, 16);
			this.lblHeader.TabIndex = 20;
			this.lblHeader.Text = "Seitenkopf:";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtHeader
			// 
			this.txtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHeader.BackColor = System.Drawing.SystemColors.Window;
			this.txtHeader.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtHeader.Location = new System.Drawing.Point(144, 81);
			this.txtHeader.Multiline = true;
			this.txtHeader.Name = "txtHeader";
			this.txtHeader.Size = new System.Drawing.Size(404, 118);
			this.txtHeader.TabIndex = 19;
			this.txtHeader.TextChanged += new System.EventHandler(this.txtHeader_TextChanged);
			// 
			// lblLicensedTo
			// 
			this.lblLicensedTo.Location = new System.Drawing.Point(1, 4);
			this.lblLicensedTo.Name = "lblLicensedTo";
			this.lblLicensedTo.Size = new System.Drawing.Size(137, 16);
			this.lblLicensedTo.TabIndex = 18;
			this.lblLicensedTo.Text = "Lizenziert für:";
			this.lblLicensedTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtLicensedTo
			// 
			this.txtLicensedTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLicensedTo.BackColor = System.Drawing.SystemColors.Window;
			this.txtLicensedTo.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtLicensedTo.Location = new System.Drawing.Point(144, 3);
			this.txtLicensedTo.Name = "txtLicensedTo";
			this.txtLicensedTo.Size = new System.Drawing.Size(404, 20);
			this.txtLicensedTo.TabIndex = 17;
			this.txtLicensedTo.TextChanged += new System.EventHandler(this.txtLicensedTo_TextChanged);
			// 
			// lblValidUntil
			// 
			this.lblValidUntil.Location = new System.Drawing.Point(1, 56);
			this.lblValidUntil.Name = "lblValidUntil";
			this.lblValidUntil.Size = new System.Drawing.Size(137, 16);
			this.lblValidUntil.TabIndex = 28;
			this.lblValidUntil.Text = "Gültig bis:";
			this.lblValidUntil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// dtpValidUntil
			// 
			this.dtpValidUntil.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dtpValidUntil.Location = new System.Drawing.Point(144, 55);
			this.dtpValidUntil.Name = "dtpValidUntil";
			this.dtpValidUntil.Size = new System.Drawing.Size(404, 20);
			this.dtpValidUntil.TabIndex = 29;
			this.dtpValidUntil.ValueChanged += new System.EventHandler(this.dtpValidUntil_ValueChanged);
			// 
			// syeCurrentSystem
			// 
			this.syeCurrentSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.syeCurrentSystem.Enabled = false;
			this.syeCurrentSystem.LicensedSystem = null;
			this.syeCurrentSystem.Location = new System.Drawing.Point(144, 456);
			this.syeCurrentSystem.Name = "syeCurrentSystem";
			this.syeCurrentSystem.Size = new System.Drawing.Size(404, 105);
			this.syeCurrentSystem.TabIndex = 31;
			// 
			// lblEmail
			// 
			this.lblEmail.Location = new System.Drawing.Point(1, 30);
			this.lblEmail.Name = "lblEmail";
			this.lblEmail.Size = new System.Drawing.Size(137, 16);
			this.lblEmail.TabIndex = 33;
			this.lblEmail.Text = "E-Mail:";
			this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtEmail
			// 
			this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEmail.BackColor = System.Drawing.SystemColors.Window;
			this.txtEmail.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtEmail.Location = new System.Drawing.Point(144, 29);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(404, 20);
			this.txtEmail.TabIndex = 32;
			this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
			// 
			// LicenseEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(400, 0);
			this.Controls.Add(this.lblEmail);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.syeCurrentSystem);
			this.Controls.Add(this.dtpValidUntil);
			this.Controls.Add(this.lblValidUntil);
			this.Controls.Add(this.btnAddSystem);
			this.Controls.Add(this.lstSystems);
			this.Controls.Add(this.lblSystem);
			this.Controls.Add(this.lstModules);
			this.Controls.Add(this.lblModules);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.txtHeader);
			this.Controls.Add(this.lblLicensedTo);
			this.Controls.Add(this.txtLicensedTo);
			this.Name = "LicenseEditor";
			this.Size = new System.Drawing.Size(551, 589);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnAddSystem;
		private System.Windows.Forms.ListView lstSystems;
		private System.Windows.Forms.Label lblSystem;
		private System.Windows.Forms.ListView lstModules;
		private System.Windows.Forms.Label lblModules;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TextBox txtHeader;
		private System.Windows.Forms.Label lblLicensedTo;
		private System.Windows.Forms.TextBox txtLicensedTo;
		private System.Windows.Forms.Label lblValidUntil;
		private System.Windows.Forms.DateTimePicker dtpValidUntil;
		private System.Windows.Forms.ColumnHeader colSystemAddedAt;
		private System.Windows.Forms.ColumnHeader colHardwareId;
		private System.Windows.Forms.ColumnHeader colAnnotation;
		private SystemEditor syeCurrentSystem;
		private System.Windows.Forms.Label lblEmail;
		private System.Windows.Forms.TextBox txtEmail;
	}
}
