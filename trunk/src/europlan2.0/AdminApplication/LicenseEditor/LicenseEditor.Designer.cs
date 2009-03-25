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
			this.lblValidUntil = new System.Windows.Forms.Label();
			this.dtpValidUntil = new System.Windows.Forms.DateTimePicker();
			this.colSystemAddedAt = new System.Windows.Forms.ColumnHeader();
			this.colSystemId = new System.Windows.Forms.ColumnHeader();
			this.colAnnotation = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// btnAddSystem
			// 
			this.btnAddSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddSystem.Location = new System.Drawing.Point(439, 430);
			this.btnAddSystem.Name = "btnAddSystem";
			this.btnAddSystem.Size = new System.Drawing.Size(109, 23);
			this.btnAddSystem.TabIndex = 26;
			this.btnAddSystem.Text = "Neuer Rechner";
			this.btnAddSystem.UseVisualStyleBackColor = true;
			this.btnAddSystem.Click += new System.EventHandler(this.btnAddSystem_Click);
			// 
			// txtSystem
			// 
			this.txtSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSystem.BackColor = System.Drawing.SystemColors.Window;
			this.txtSystem.Enabled = false;
			this.txtSystem.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtSystem.Location = new System.Drawing.Point(144, 432);
			this.txtSystem.Name = "txtSystem";
			this.txtSystem.ReadOnly = true;
			this.txtSystem.Size = new System.Drawing.Size(289, 20);
			this.txtSystem.TabIndex = 25;
			// 
			// lstSystems
			// 
			this.lstSystems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstSystems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSystemAddedAt,
            this.colSystemId,
            this.colAnnotation});
			this.lstSystems.HideSelection = false;
			this.lstSystems.Location = new System.Drawing.Point(144, 305);
			this.lstSystems.Name = "lstSystems";
			this.lstSystems.Size = new System.Drawing.Size(404, 119);
			this.lstSystems.TabIndex = 24;
			this.lstSystems.UseCompatibleStateImageBehavior = false;
			this.lstSystems.View = System.Windows.Forms.View.Details;
			// 
			// lblSystem
			// 
			this.lblSystem.Location = new System.Drawing.Point(1, 305);
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
			this.lstModules.Location = new System.Drawing.Point(144, 180);
			this.lstModules.Name = "lstModules";
			this.lstModules.Size = new System.Drawing.Size(404, 119);
			this.lstModules.TabIndex = 22;
			this.lstModules.UseCompatibleStateImageBehavior = false;
			this.lstModules.View = System.Windows.Forms.View.List;
			this.lstModules.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstModules_ItemChecked);
			// 
			// lblModules
			// 
			this.lblModules.Location = new System.Drawing.Point(1, 180);
			this.lblModules.Name = "lblModules";
			this.lblModules.Size = new System.Drawing.Size(137, 16);
			this.lblModules.TabIndex = 21;
			this.lblModules.Text = "Module:";
			this.lblModules.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblHeader
			// 
			this.lblHeader.Location = new System.Drawing.Point(1, 56);
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
			this.txtHeader.Location = new System.Drawing.Point(144, 55);
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
			this.lblLicensedTo.Text = "Lizenznehmer:";
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
			this.lblValidUntil.Location = new System.Drawing.Point(1, 30);
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
			this.dtpValidUntil.Location = new System.Drawing.Point(144, 29);
			this.dtpValidUntil.Name = "dtpValidUntil";
			this.dtpValidUntil.Size = new System.Drawing.Size(404, 20);
			this.dtpValidUntil.TabIndex = 29;
			this.dtpValidUntil.ValueChanged += new System.EventHandler(this.dtpValidUntil_ValueChanged);
			// 
			// colSystemAddedAt
			// 
			this.colSystemAddedAt.Text = "Hinzugefügt am";
			this.colSystemAddedAt.Width = 90;
			// 
			// colSystemId
			// 
			this.colSystemId.Text = "Rechner ID";
			this.colSystemId.Width = 130;
			// 
			// colAnnotation
			// 
			this.colAnnotation.Text = "Anmerkung";
			this.colAnnotation.Width = 170;
			// 
			// LicenseEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(400, 0);
			this.Controls.Add(this.dtpValidUntil);
			this.Controls.Add(this.lblValidUntil);
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
			this.Name = "LicenseEditor";
			this.Size = new System.Drawing.Size(551, 509);
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
		private System.Windows.Forms.Label lblValidUntil;
		private System.Windows.Forms.DateTimePicker dtpValidUntil;
		private System.Windows.Forms.ColumnHeader colSystemAddedAt;
		private System.Windows.Forms.ColumnHeader colSystemId;
		private System.Windows.Forms.ColumnHeader colAnnotation;
	}
}
