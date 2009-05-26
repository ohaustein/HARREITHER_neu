namespace Europlan.Application.ContructionEditor {
	partial class MaterialEditorGrid {
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
			this.gridMaterials = new System.Windows.Forms.DataGridView();
			this.userDefinedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.partNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.denominationDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.unitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.priceDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.materialsWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridMaterials)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.materialsWrapperBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridMaterials
			// 
			this.gridMaterials.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridMaterials.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridMaterials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridMaterials.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userDefinedDataGridViewCheckBoxColumn,
            this.idDataGridViewTextBoxColumn,
            this.partNumberDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.denominationDataGridViewTextBoxColumn,
            this.unitDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn});
			this.gridMaterials.DataSource = this.materialsWrapperBindingSource;
			this.gridMaterials.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridMaterials.Location = new System.Drawing.Point(0, 0);
			this.gridMaterials.Name = "gridMaterials";
			this.gridMaterials.Size = new System.Drawing.Size(612, 440);
			this.gridMaterials.TabIndex = 2;
			this.gridMaterials.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridMaterials_UserDeletingRow);
			this.gridMaterials.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.gridMaterials_RowsAdded);
			// 
			// userDefinedDataGridViewCheckBoxColumn
			// 
			this.userDefinedDataGridViewCheckBoxColumn.DataPropertyName = "UserDefined";
			this.userDefinedDataGridViewCheckBoxColumn.HeaderText = "UserDefined";
			this.userDefinedDataGridViewCheckBoxColumn.Name = "userDefinedDataGridViewCheckBoxColumn";
			this.userDefinedDataGridViewCheckBoxColumn.ReadOnly = true;
			this.userDefinedDataGridViewCheckBoxColumn.Visible = false;
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.FillWeight = 50F;
			this.idDataGridViewTextBoxColumn.HeaderText = "Nr.";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.Visible = false;
			this.idDataGridViewTextBoxColumn.Width = 50;
			// 
			// partNumberDataGridViewTextBoxColumn
			// 
			this.partNumberDataGridViewTextBoxColumn.DataPropertyName = "PartNumber";
			this.partNumberDataGridViewTextBoxColumn.FillWeight = 70F;
			this.partNumberDataGridViewTextBoxColumn.HeaderText = "Bestellnr.";
			this.partNumberDataGridViewTextBoxColumn.Name = "partNumberDataGridViewTextBoxColumn";
			this.partNumberDataGridViewTextBoxColumn.Width = 70;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.FillWeight = 150F;
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.Width = 150;
			// 
			// denominationDataGridViewTextBoxColumn
			// 
			this.denominationDataGridViewTextBoxColumn.DataPropertyName = "Denomination";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.denominationDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.denominationDataGridViewTextBoxColumn.FillWeight = 75F;
			this.denominationDataGridViewTextBoxColumn.HeaderText = "Verpackungs- einheit";
			this.denominationDataGridViewTextBoxColumn.Name = "denominationDataGridViewTextBoxColumn";
			this.denominationDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.DENOMINATION;
			this.denominationDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.denominationDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.denominationDataGridViewTextBoxColumn.Width = 75;
			// 
			// unitDataGridViewTextBoxColumn
			// 
			this.unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			this.unitDataGridViewTextBoxColumn.FillWeight = 50F;
			this.unitDataGridViewTextBoxColumn.HeaderText = "Einheit";
			this.unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			this.unitDataGridViewTextBoxColumn.Width = 50;
			// 
			// priceDataGridViewTextBoxColumn
			// 
			this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F2";
			this.priceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.priceDataGridViewTextBoxColumn.FillWeight = 60F;
			this.priceDataGridViewTextBoxColumn.HeaderText = "Preis pro Einheit";
			this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
			this.priceDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PRICE;
			this.priceDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.priceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.priceDataGridViewTextBoxColumn.Width = 60;
			// 
			// materialsWrapperBindingSource
			// 
			this.materialsWrapperBindingSource.DataSource = typeof(Europlan.Common.MaterialListWrapper);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
			this.dataGridViewTextBoxColumn1.FillWeight = 50F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Nr.";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.Width = 50;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn2.FillWeight = 150F;
			this.dataGridViewTextBoxColumn2.HeaderText = "Bezeichnung";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.Width = 150;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "PartNumber";
			this.dataGridViewTextBoxColumn3.FillWeight = 70F;
			this.dataGridViewTextBoxColumn3.HeaderText = "Bestellnr.";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.Width = 70;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.DataPropertyName = "Denomination";
			this.dataGridViewTextBoxColumn4.FillWeight = 75F;
			this.dataGridViewTextBoxColumn4.HeaderText = "Verpackungs- einheit";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.Width = 75;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.DataPropertyName = "Unit";
			this.dataGridViewTextBoxColumn5.FillWeight = 50F;
			this.dataGridViewTextBoxColumn5.HeaderText = "Einheit";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.Width = 50;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.DataPropertyName = "Price";
			this.dataGridViewTextBoxColumn6.FillWeight = 60F;
			this.dataGridViewTextBoxColumn6.HeaderText = "Preis pro Einheit";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.Width = 60;
			// 
			// MaterialEditorGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridMaterials);
			this.Name = "MaterialEditorGrid";
			this.Size = new System.Drawing.Size(612, 440);
			((System.ComponentModel.ISupportInitialize)(this.gridMaterials)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.materialsWrapperBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.BindingSource materialsWrapperBindingSource;
		private System.Windows.Forms.DataGridView gridMaterials;
		private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewCheckBoxColumn userDefinedDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn partNumberDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private Europlan.Common.NumericColumn denominationDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private Europlan.Common.NumericColumn priceDataGridViewTextBoxColumn;

	}
}
