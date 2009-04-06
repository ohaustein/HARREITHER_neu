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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblSystem = new System.Windows.Forms.Label();
			this.lstModules = new System.Windows.Forms.ListView();
			this.lblModules = new System.Windows.Forms.Label();
			this.lblHeader = new System.Windows.Forms.Label();
			this.txtHeader = new System.Windows.Forms.TextBox();
			this.lblLicensedTo = new System.Windows.Forms.Label();
			this.txtLicensedTo = new System.Windows.Forms.TextBox();
			this.lblValidUntil = new System.Windows.Forms.Label();
			this.dtpValidUntil = new System.Windows.Forms.DateTimePicker();
			this.lblEmail = new System.Windows.Forms.Label();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.licenseTemplateBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewHardwareIdColumn1 = new Europlan.AdminApplication.HardwareIdColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchesCurrentSystemDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.addedDateDataGridViewTextBoxColumn = new Europlan.AdminApplication.DateColumn();
			this.idDataGridViewTextBoxColumn = new Europlan.AdminApplication.HardwareIdColumn();
			this.annotationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.licenseTemplateBindingSource)).BeginInit();
			this.SuspendLayout();
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
			this.lstModules.TabIndex = 4;
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
			this.txtHeader.TabIndex = 3;
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
			this.txtLicensedTo.TabIndex = 0;
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
			this.dtpValidUntil.TabIndex = 2;
			this.dtpValidUntil.ValueChanged += new System.EventHandler(this.dtpValidUntil_ValueChanged);
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
			this.txtEmail.TabIndex = 1;
			this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.dataGridView1);
			this.panel1.Location = new System.Drawing.Point(144, 331);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(404, 205);
			this.panel1.TabIndex = 35;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.matchesCurrentSystemDataGridViewCheckBoxColumn,
            this.addedDateDataGridViewTextBoxColumn,
            this.idDataGridViewTextBoxColumn,
            this.annotationDataGridViewTextBoxColumn});
			this.dataGridView1.DataMember = "Systems";
			this.dataGridView1.DataSource = this.licenseTemplateBindingSource;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(402, 203);
			this.dataGridView1.TabIndex = 5;
			this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
			// 
			// licenseTemplateBindingSource
			// 
			this.licenseTemplateBindingSource.DataSource = typeof(Europlan.Licensing.LicenseTemplate);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "AddedDate";
			this.dataGridViewTextBoxColumn1.HeaderText = "Hinzugefügt am";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.Width = 120;
			// 
			// dataGridViewHardwareIdColumn1
			// 
			this.dataGridViewHardwareIdColumn1.DataPropertyName = "Id";
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dataGridViewHardwareIdColumn1.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridViewHardwareIdColumn1.HeaderText = "Hardware ID";
			this.dataGridViewHardwareIdColumn1.Name = "dataGridViewHardwareIdColumn1";
			this.dataGridViewHardwareIdColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewHardwareIdColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Id";
			this.dataGridViewTextBoxColumn2.HeaderText = "Hardware ID";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "Annotation";
			this.dataGridViewTextBoxColumn3.HeaderText = "Anmerkung";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			// 
			// matchesCurrentSystemDataGridViewCheckBoxColumn
			// 
			this.matchesCurrentSystemDataGridViewCheckBoxColumn.DataPropertyName = "MatchesCurrentSystem";
			this.matchesCurrentSystemDataGridViewCheckBoxColumn.HeaderText = "MatchesCurrentSystem";
			this.matchesCurrentSystemDataGridViewCheckBoxColumn.Name = "matchesCurrentSystemDataGridViewCheckBoxColumn";
			this.matchesCurrentSystemDataGridViewCheckBoxColumn.ReadOnly = true;
			this.matchesCurrentSystemDataGridViewCheckBoxColumn.Visible = false;
			// 
			// addedDateDataGridViewTextBoxColumn
			// 
			this.addedDateDataGridViewTextBoxColumn.DataPropertyName = "AddedDate";
			this.addedDateDataGridViewTextBoxColumn.HeaderText = "Hinzugefügt am";
			this.addedDateDataGridViewTextBoxColumn.Name = "addedDateDataGridViewTextBoxColumn";
			this.addedDateDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.addedDateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.addedDateDataGridViewTextBoxColumn.Width = 120;
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.idDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
			this.idDataGridViewTextBoxColumn.HeaderText = "Hardware ID";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.idDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.idDataGridViewTextBoxColumn.Width = 170;
			// 
			// annotationDataGridViewTextBoxColumn
			// 
			this.annotationDataGridViewTextBoxColumn.DataPropertyName = "Annotation";
			this.annotationDataGridViewTextBoxColumn.HeaderText = "Anmerkung";
			this.annotationDataGridViewTextBoxColumn.Name = "annotationDataGridViewTextBoxColumn";
			this.annotationDataGridViewTextBoxColumn.Width = 200;
			// 
			// LicenseEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(400, 0);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblEmail);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.dtpValidUntil);
			this.Controls.Add(this.lblValidUntil);
			this.Controls.Add(this.lblSystem);
			this.Controls.Add(this.lstModules);
			this.Controls.Add(this.lblModules);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.txtHeader);
			this.Controls.Add(this.lblLicensedTo);
			this.Controls.Add(this.txtLicensedTo);
			this.Name = "LicenseEditor";
			this.Size = new System.Drawing.Size(551, 589);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.licenseTemplateBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSystem;
		private System.Windows.Forms.ListView lstModules;
		private System.Windows.Forms.Label lblModules;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TextBox txtHeader;
		private System.Windows.Forms.Label lblLicensedTo;
		private System.Windows.Forms.TextBox txtLicensedTo;
		private System.Windows.Forms.Label lblValidUntil;
		private System.Windows.Forms.DateTimePicker dtpValidUntil;
		private System.Windows.Forms.Label lblEmail;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.BindingSource licenseTemplateBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private HardwareIdColumn dataGridViewHardwareIdColumn1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridViewCheckBoxColumn matchesCurrentSystemDataGridViewCheckBoxColumn;
		private DateColumn addedDateDataGridViewTextBoxColumn;
		private HardwareIdColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn annotationDataGridViewTextBoxColumn;
	}
}
