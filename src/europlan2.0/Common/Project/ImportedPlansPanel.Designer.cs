namespace Europlan.Common {
	partial class ImportedPlansPanel {
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
			this.lblImportedPlans = new System.Windows.Forms.Label();
			this.btnImport = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.dgvPlans = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.planSource = new System.Windows.Forms.BindingSource(this.components);
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RelativeFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colOptions = new System.Windows.Forms.DataGridViewButtonColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgvPlans)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.planSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lblImportedPlans
			// 
			this.lblImportedPlans.AutoSize = true;
			this.lblImportedPlans.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblImportedPlans.Location = new System.Drawing.Point(3, 0);
			this.lblImportedPlans.Name = "lblImportedPlans";
			this.lblImportedPlans.Size = new System.Drawing.Size(168, 24);
			this.lblImportedPlans.TabIndex = 20;
			this.lblImportedPlans.Text = "Importierte Pläne";
			// 
			// btnImport
			// 
			this.btnImport.Location = new System.Drawing.Point(3, 37);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(130, 23);
			this.btnImport.TabIndex = 21;
			this.btnImport.Text = "Plan importieren";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(139, 37);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(130, 23);
			this.btnDelete.TabIndex = 22;
			this.btnDelete.Text = "Plan entfernen";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// dgvPlans
			// 
			this.dgvPlans.AllowUserToAddRows = false;
			this.dgvPlans.AllowUserToDeleteRows = false;
			this.dgvPlans.AllowUserToResizeRows = false;
			this.dgvPlans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvPlans.AutoGenerateColumns = false;
			this.dgvPlans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvPlans.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.RelativeFileName,
            this.colOptions});
			this.dgvPlans.DataSource = this.planSource;
			this.dgvPlans.Location = new System.Drawing.Point(3, 66);
			this.dgvPlans.MultiSelect = false;
			this.dgvPlans.Name = "dgvPlans";
			this.dgvPlans.RowHeadersVisible = false;
			this.dgvPlans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvPlans.Size = new System.Drawing.Size(866, 487);
			this.dgvPlans.TabIndex = 23;
			this.dgvPlans.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlans_CellValueChanged);
			this.dgvPlans.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlans_CellClick);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn1.DataPropertyName = "RelativeFileName";
			this.dataGridViewTextBoxColumn1.HeaderText = "RelativeFileName";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// planSource
			// 
			this.planSource.DataSource = typeof(Europlan.Common.Plan);
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			// 
			// RelativeFileName
			// 
			this.RelativeFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.RelativeFileName.DataPropertyName = "RelativeFileName";
			this.RelativeFileName.HeaderText = "RelativeFileName";
			this.RelativeFileName.Name = "RelativeFileName";
			this.RelativeFileName.ReadOnly = true;
			// 
			// colOptions
			// 
			this.colOptions.HeaderText = "Optionen";
			this.colOptions.Name = "colOptions";
			this.colOptions.Text = "...";
			this.colOptions.UseColumnTextForButtonValue = true;
			this.colOptions.Width = 80;
			// 
			// ImportedPlansPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dgvPlans);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.lblImportedPlans);
			this.Name = "ImportedPlansPanel";
			this.Size = new System.Drawing.Size(872, 556);
			((System.ComponentModel.ISupportInitialize)(this.dgvPlans)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.planSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblImportedPlans;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.DataGridView dgvPlans;
		private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
		private System.Windows.Forms.BindingSource planSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn RelativeFileName;
		private System.Windows.Forms.DataGridViewButtonColumn colOptions;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
	}
}
