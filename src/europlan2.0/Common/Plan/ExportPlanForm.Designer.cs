namespace Europlan.Common {
	partial class ExportPlanForm {
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
			this.components = new System.ComponentModel.Container();
			this.exportOptionTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnExport = new System.Windows.Forms.Button();
			this.btnSaveAs = new System.Windows.Forms.Button();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.lblFileName = new System.Windows.Forms.Label();
			this.cmbExportOption = new System.Windows.Forms.ComboBox();
			this.lblExportOption = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkExportWallNumbers = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.exportOptionTypeBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// exportOptionTypeBindingSource
			// 
			this.exportOptionTypeBindingSource.DataSource = typeof(Europlan.Common.ExportPlanForm.ExportOptionType);
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExport.Enabled = false;
			this.btnExport.Location = new System.Drawing.Point(199, 93);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(137, 23);
			this.btnExport.TabIndex = 3;
			this.btnExport.Text = "Exportieren";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveAs.Location = new System.Drawing.Point(443, 54);
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(36, 23);
			this.btnSaveAs.TabIndex = 2;
			this.btnSaveAs.Text = "...";
			this.btnSaveAs.UseVisualStyleBackColor = true;
			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
			// 
			// txtPath
			// 
			this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPath.Location = new System.Drawing.Point(152, 56);
			this.txtPath.Name = "txtPath";
			this.txtPath.ReadOnly = true;
			this.txtPath.Size = new System.Drawing.Size(285, 20);
			this.txtPath.TabIndex = 10;
			this.txtPath.TabStop = false;
			// 
			// lblFileName
			// 
			this.lblFileName.AutoSize = true;
			this.lblFileName.Location = new System.Drawing.Point(12, 59);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(56, 13);
			this.lblFileName.TabIndex = 9;
			this.lblFileName.Text = "Dateipfad:";
			// 
			// cmbExportOption
			// 
			this.cmbExportOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbExportOption.DataSource = this.exportOptionTypeBindingSource;
			this.cmbExportOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbExportOption.FormattingEnabled = true;
			this.cmbExportOption.Location = new System.Drawing.Point(152, 6);
			this.cmbExportOption.Name = "cmbExportOption";
			this.cmbExportOption.Size = new System.Drawing.Size(327, 21);
			this.cmbExportOption.TabIndex = 1;
			// 
			// lblExportOption
			// 
			this.lblExportOption.AutoSize = true;
			this.lblExportOption.Location = new System.Drawing.Point(12, 9);
			this.lblExportOption.Name = "lblExportOption";
			this.lblExportOption.Size = new System.Drawing.Size(75, 13);
			this.lblExportOption.TabIndex = 7;
			this.lblExportOption.Text = "Exportumfang:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(342, 93);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(137, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkExportWallNumbers
			// 
			this.chkExportWallNumbers.AutoSize = true;
			this.chkExportWallNumbers.Location = new System.Drawing.Point(152, 33);
			this.chkExportWallNumbers.Name = "chkExportWallNumbers";
			this.chkExportWallNumbers.Size = new System.Drawing.Size(197, 17);
			this.chkExportWallNumbers.TabIndex = 11;
			this.chkExportWallNumbers.Text = "Numerierung der Wände exportieren";
			this.chkExportWallNumbers.UseVisualStyleBackColor = true;
			// 
			// ExportPlanForm
			// 
			this.AcceptButton = this.btnExport;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(491, 128);
			this.Controls.Add(this.chkExportWallNumbers);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnSaveAs);
			this.Controls.Add(this.txtPath);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.cmbExportOption);
			this.Controls.Add(this.lblExportOption);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExportPlanForm";
			this.Text = "Plan exportieren";
			this.Load += new System.EventHandler(this.ExportPlanForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportPlanForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.exportOptionTypeBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.BindingSource exportOptionTypeBindingSource;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnSaveAs;
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.Label lblFileName;
		private System.Windows.Forms.ComboBox cmbExportOption;
		private System.Windows.Forms.Label lblExportOption;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkExportWallNumbers;
	}
}