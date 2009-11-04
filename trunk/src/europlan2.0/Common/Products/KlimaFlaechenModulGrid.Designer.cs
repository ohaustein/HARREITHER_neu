namespace Europlan.Common {
	partial class KlimaFlaechenModulGrid {
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
			this.dgvModules = new System.Windows.Forms.DataGridView();
			this.modulTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.orientationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.areaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.klimaFlaechenModulBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAlign = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvModules)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.klimaFlaechenModulBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvModules
			// 
			this.dgvModules.AllowUserToAddRows = false;
			this.dgvModules.AllowUserToDeleteRows = false;
			this.dgvModules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.dgvModules.AutoGenerateColumns = false;
			this.dgvModules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvModules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.modulTypeDataGridViewTextBoxColumn,
            this.orientationDataGridViewTextBoxColumn,
            this.areaDataGridViewTextBoxColumn});
			this.dgvModules.DataSource = this.klimaFlaechenModulBindingSource;
			this.dgvModules.Location = new System.Drawing.Point(0, 0);
			this.dgvModules.Name = "dgvModules";
			this.dgvModules.Size = new System.Drawing.Size(347, 343);
			this.dgvModules.TabIndex = 0;
			// 
			// modulTypeDataGridViewTextBoxColumn
			// 
			this.modulTypeDataGridViewTextBoxColumn.DataPropertyName = "ModulType";
			this.modulTypeDataGridViewTextBoxColumn.HeaderText = "Modultyp";
			this.modulTypeDataGridViewTextBoxColumn.Name = "modulTypeDataGridViewTextBoxColumn";
			this.modulTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.modulTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.modulTypeDataGridViewTextBoxColumn.Width = 150;
			// 
			// orientationDataGridViewTextBoxColumn
			// 
			this.orientationDataGridViewTextBoxColumn.DataPropertyName = "Orientation";
			this.orientationDataGridViewTextBoxColumn.HeaderText = "Ausrichtung";
			this.orientationDataGridViewTextBoxColumn.Name = "orientationDataGridViewTextBoxColumn";
			this.orientationDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.orientationDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.orientationDataGridViewTextBoxColumn.Width = 150;
			// 
			// areaDataGridViewTextBoxColumn
			// 
			this.areaDataGridViewTextBoxColumn.DataPropertyName = "Area";
			this.areaDataGridViewTextBoxColumn.HeaderText = "Area";
			this.areaDataGridViewTextBoxColumn.Name = "areaDataGridViewTextBoxColumn";
			this.areaDataGridViewTextBoxColumn.ReadOnly = true;
			this.areaDataGridViewTextBoxColumn.Visible = false;
			// 
			// klimaFlaechenModulBindingSource
			// 
			this.klimaFlaechenModulBindingSource.DataSource = typeof(Europlan.Common.KlimaFlaechenModul);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(353, 3);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(30, 23);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "+";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(353, 32);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(32, 23);
			this.btnRemove.TabIndex = 2;
			this.btnRemove.Text = "-";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnAlign
			// 
			this.btnAlign.Location = new System.Drawing.Point(353, 61);
			this.btnAlign.Name = "btnAlign";
			this.btnAlign.Size = new System.Drawing.Size(88, 43);
			this.btnAlign.TabIndex = 3;
			this.btnAlign.Text = "Automatische Ausrichtung";
			this.btnAlign.UseVisualStyleBackColor = true;
			this.btnAlign.Click += new System.EventHandler(this.btnAlign_Click);
			// 
			// KlimaFlaechenModulGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnAlign);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.dgvModules);
			this.Name = "KlimaFlaechenModulGrid";
			this.Size = new System.Drawing.Size(446, 343);
			((System.ComponentModel.ISupportInitialize)(this.dgvModules)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.klimaFlaechenModulBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvModules;
		private System.Windows.Forms.BindingSource klimaFlaechenModulBindingSource;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAlign;
		private System.Windows.Forms.DataGridViewComboBoxColumn modulTypeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn orientationDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn areaDataGridViewTextBoxColumn;
	}
}
