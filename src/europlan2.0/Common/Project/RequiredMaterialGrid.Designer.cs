namespace Europlan.Common {
	partial class RequiredMaterialGrid {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvRequiredMaterial = new System.Windows.Forms.DataGridView();
			this.partNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.requiredAmountDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.calculatedAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.unitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.requiredMaterialWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dgvRequiredMaterial)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.requiredMaterialWrapperBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvRequiredMaterial
			// 
			this.dgvRequiredMaterial.AllowUserToAddRows = false;
			this.dgvRequiredMaterial.AllowUserToDeleteRows = false;
			this.dgvRequiredMaterial.AllowUserToResizeColumns = false;
			this.dgvRequiredMaterial.AllowUserToResizeRows = false;
			this.dgvRequiredMaterial.AutoGenerateColumns = false;
			this.dgvRequiredMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvRequiredMaterial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.partNumberDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.requiredAmountDataGridViewTextBoxColumn,
            this.calculatedAmountDataGridViewTextBoxColumn,
            this.unitDataGridViewTextBoxColumn});
			this.dgvRequiredMaterial.DataSource = this.requiredMaterialWrapperBindingSource;
			this.dgvRequiredMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvRequiredMaterial.Location = new System.Drawing.Point(0, 0);
			this.dgvRequiredMaterial.Name = "dgvRequiredMaterial";
			this.dgvRequiredMaterial.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvRequiredMaterial.Size = new System.Drawing.Size(695, 441);
			this.dgvRequiredMaterial.TabIndex = 0;
			this.dgvRequiredMaterial.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequiredMaterial_CellValueChanged);
			// 
			// partNumberDataGridViewTextBoxColumn
			// 
			this.partNumberDataGridViewTextBoxColumn.DataPropertyName = "PartNumber";
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.partNumberDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
			this.partNumberDataGridViewTextBoxColumn.HeaderText = "Bestellnummer";
			this.partNumberDataGridViewTextBoxColumn.Name = "partNumberDataGridViewTextBoxColumn";
			this.partNumberDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.nameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// requiredAmountDataGridViewTextBoxColumn
			// 
			this.requiredAmountDataGridViewTextBoxColumn.DataPropertyName = "RequiredAmount";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F0";
			this.requiredAmountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.requiredAmountDataGridViewTextBoxColumn.HeaderText = "Menge";
			this.requiredAmountDataGridViewTextBoxColumn.Name = "requiredAmountDataGridViewTextBoxColumn";
			this.requiredAmountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.requiredAmountDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// calculatedAmountDataGridViewTextBoxColumn
			// 
			this.calculatedAmountDataGridViewTextBoxColumn.DataPropertyName = "CalculatedAmount";
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.calculatedAmountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
			this.calculatedAmountDataGridViewTextBoxColumn.HeaderText = "(berechnet)";
			this.calculatedAmountDataGridViewTextBoxColumn.Name = "calculatedAmountDataGridViewTextBoxColumn";
			this.calculatedAmountDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// unitDataGridViewTextBoxColumn
			// 
			this.unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.unitDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
			this.unitDataGridViewTextBoxColumn.HeaderText = "Einheit";
			this.unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			this.unitDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// requiredMaterialWrapperBindingSource
			// 
			this.requiredMaterialWrapperBindingSource.DataSource = typeof(Europlan.Common.RequiredMaterialWrapper);
			// 
			// RequiredMaterialGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dgvRequiredMaterial);
			this.Name = "RequiredMaterialGrid";
			this.Size = new System.Drawing.Size(695, 441);
			((System.ComponentModel.ISupportInitialize)(this.dgvRequiredMaterial)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.requiredMaterialWrapperBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvRequiredMaterial;
		private System.Windows.Forms.BindingSource requiredMaterialWrapperBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn partNumberDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn requiredAmountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn calculatedAmountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
	}
}
