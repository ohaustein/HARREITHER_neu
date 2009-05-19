namespace Europlan.AdminApplication {
	partial class ConstructionEditor {
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
			this.lblId = new System.Windows.Forms.Label();
			this.txtId = new System.Windows.Forms.TextBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.lblThickness = new System.Windows.Forms.Label();
			this.gridLayers = new System.Windows.Forms.DataGridView();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colMaterial = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colLambdaValue = new Europlan.Common.NumericColumn();
			this.colThickness = new Europlan.Common.NumericColumn();
			this.colRValue = new Europlan.Common.NumericColumn();
			this.constructionBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dataGridViewComboBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.cbPeFoil = new System.Windows.Forms.CheckBox();
			this.dataGridViewComboBoxColumn4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.numericColumn1 = new Europlan.Common.NumericColumn();
			this.numThickness = new Europlan.Common.NumericBox();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridLayers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.constructionBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lblId
			// 
			this.lblId.AutoSize = true;
			this.lblId.Location = new System.Drawing.Point(3, 6);
			this.lblId.Name = "lblId";
			this.lblId.Size = new System.Drawing.Size(49, 13);
			this.lblId.TabIndex = 0;
			this.lblId.Text = "Nummer:";
			// 
			// txtId
			// 
			this.txtId.Location = new System.Drawing.Point(98, 3);
			this.txtId.Name = "txtId";
			this.txtId.Size = new System.Drawing.Size(100, 20);
			this.txtId.TabIndex = 1;
			this.txtId.TextChanged += new System.EventHandler(this.txtId_TextChanged);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(98, 29);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(298, 20);
			this.txtName.TabIndex = 3;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(3, 32);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(72, 13);
			this.lblName.TabIndex = 2;
			this.lblName.Text = "Bezeichnung:";
			// 
			// lblThickness
			// 
			this.lblThickness.AutoSize = true;
			this.lblThickness.Location = new System.Drawing.Point(3, 58);
			this.lblThickness.Name = "lblThickness";
			this.lblThickness.Size = new System.Drawing.Size(68, 13);
			this.lblThickness.TabIndex = 4;
			this.lblThickness.Text = "Estrichdicke:";
			// 
			// gridLayers
			// 
			this.gridLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridLayers.AutoGenerateColumns = false;
			this.gridLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridLayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colMaterial,
            this.colLambdaValue,
            this.colThickness,
            this.colRValue});
			this.gridLayers.DataMember = "Layers";
			this.gridLayers.DataSource = this.constructionBindingSource;
			this.gridLayers.Location = new System.Drawing.Point(3, 81);
			this.gridLayers.Name = "gridLayers";
			this.gridLayers.Size = new System.Drawing.Size(624, 268);
			this.gridLayers.TabIndex = 6;
			// 
			// colName
			// 
			this.colName.DataPropertyName = "Name";
			this.colName.FillWeight = 160F;
			this.colName.HeaderText = "Bezeichnung";
			this.colName.Name = "colName";
			this.colName.Width = 160;
			// 
			// colMaterial
			// 
			this.colMaterial.DataPropertyName = "LayerMaterial";
			this.colMaterial.FillWeight = 160F;
			this.colMaterial.HeaderText = "Material";
			this.colMaterial.Name = "colMaterial";
			this.colMaterial.Width = 160;
			// 
			// colLambdaValue
			// 
			this.colLambdaValue.DataPropertyName = "LambdaValue";
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle1.Format = "F3";
			this.colLambdaValue.DefaultCellStyle = dataGridViewCellStyle1;
			this.colLambdaValue.FillWeight = 70F;
			this.colLambdaValue.HeaderText = "lambda (W/mK)";
			this.colLambdaValue.Name = "colLambdaValue";
			this.colLambdaValue.NumEditType = Europlan.Common.NumericBox.NumericEditType.LAMBDA_VALUE;
			this.colLambdaValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colLambdaValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colLambdaValue.Width = 70;
			// 
			// colThickness
			// 
			this.colThickness.DataPropertyName = "Thickness";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F2";
			this.colThickness.DefaultCellStyle = dataGridViewCellStyle2;
			this.colThickness.FillWeight = 70F;
			this.colThickness.HeaderText = "d (mm)";
			this.colThickness.Name = "colThickness";
			this.colThickness.NumEditType = Europlan.Common.NumericBox.NumericEditType.FLOOR_CONSTRUCTION_THICKNESS;
			this.colThickness.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colThickness.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colThickness.Width = 70;
			// 
			// colRValue
			// 
			this.colRValue.DataPropertyName = "RValue";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F3";
			this.colRValue.DefaultCellStyle = dataGridViewCellStyle3;
			this.colRValue.FillWeight = 70F;
			this.colRValue.HeaderText = "R (m²K/W)";
			this.colRValue.Name = "colRValue";
			this.colRValue.NumEditType = Europlan.Common.NumericBox.NumericEditType.R_VALUE;
			this.colRValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colRValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colRValue.Width = 70;
			// 
			// constructionBindingSource
			// 
			this.constructionBindingSource.DataSource = typeof(Europlan.Common.Construction);
			// 
			// dataGridViewComboBoxColumn1
			// 
			this.dataGridViewComboBoxColumn1.DataPropertyName = "LayerMaterial";
			this.dataGridViewComboBoxColumn1.FillWeight = 160F;
			this.dataGridViewComboBoxColumn1.HeaderText = "Material";
			this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
			this.dataGridViewComboBoxColumn1.Width = 160;
			// 
			// dataGridViewComboBoxColumn2
			// 
			this.dataGridViewComboBoxColumn2.DataPropertyName = "LayerMaterial";
			this.dataGridViewComboBoxColumn2.FillWeight = 160F;
			this.dataGridViewComboBoxColumn2.HeaderText = "Material";
			this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
			this.dataGridViewComboBoxColumn2.Width = 160;
			// 
			// dataGridViewComboBoxColumn3
			// 
			this.dataGridViewComboBoxColumn3.DataPropertyName = "LayerMaterial";
			this.dataGridViewComboBoxColumn3.FillWeight = 160F;
			this.dataGridViewComboBoxColumn3.HeaderText = "Material";
			this.dataGridViewComboBoxColumn3.Name = "dataGridViewComboBoxColumn3";
			this.dataGridViewComboBoxColumn3.Width = 160;
			// 
			// cbPeFoil
			// 
			this.cbPeFoil.AutoSize = true;
			this.cbPeFoil.Location = new System.Drawing.Point(98, 57);
			this.cbPeFoil.Name = "cbPeFoil";
			this.cbPeFoil.Size = new System.Drawing.Size(65, 17);
			this.cbPeFoil.TabIndex = 7;
			this.cbPeFoil.Text = "PE Folie";
			this.cbPeFoil.UseVisualStyleBackColor = true;
			this.cbPeFoil.Visible = false;
			// 
			// dataGridViewComboBoxColumn4
			// 
			this.dataGridViewComboBoxColumn4.DataPropertyName = "LayerMaterial";
			this.dataGridViewComboBoxColumn4.FillWeight = 160F;
			this.dataGridViewComboBoxColumn4.HeaderText = "Material";
			this.dataGridViewComboBoxColumn4.Name = "dataGridViewComboBoxColumn4";
			this.dataGridViewComboBoxColumn4.Width = 160;
			// 
			// numericColumn1
			// 
			this.numericColumn1.DataPropertyName = "LambdaValue";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.numericColumn1.DefaultCellStyle = dataGridViewCellStyle4;
			this.numericColumn1.FillWeight = 70F;
			this.numericColumn1.HeaderText = "lambda (W/mK)";
			this.numericColumn1.Name = "numericColumn1";
			this.numericColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.numericColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.numericColumn1.Width = 70;
			// 
			// numThickness
			// 
			this.numThickness.EditType = Europlan.Common.NumericBox.NumericEditType.FLOOR_CONSTRUCTION_THICKNESS;
			this.numThickness.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numThickness.Location = new System.Drawing.Point(98, 55);
			this.numThickness.Name = "numThickness";
			this.numThickness.Size = new System.Drawing.Size(100, 20);
			this.numThickness.TabIndex = 8;
			this.numThickness.Text = "0";
			this.numThickness.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numThickness.ValueChanged += new System.EventHandler(this.numThickness_ValueChanged);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn1.FillWeight = 160F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Bezeichnung";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.Width = 160;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "LambdaValue";
			this.dataGridViewTextBoxColumn2.FillWeight = 70F;
			this.dataGridViewTextBoxColumn2.HeaderText = "lambda (W/mK)";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.Width = 70;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "Thickness";
			this.dataGridViewTextBoxColumn3.FillWeight = 70F;
			this.dataGridViewTextBoxColumn3.HeaderText = "d (mm)";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 70;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.DataPropertyName = "RValue";
			this.dataGridViewTextBoxColumn4.FillWeight = 70F;
			this.dataGridViewTextBoxColumn4.HeaderText = "R (m²K/W)";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.Width = 70;
			// 
			// ConstructionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cbPeFoil);
			this.Controls.Add(this.gridLayers);
			this.Controls.Add(this.lblThickness);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.txtId);
			this.Controls.Add(this.lblId);
			this.Controls.Add(this.numThickness);
			this.Name = "ConstructionEditor";
			this.Size = new System.Drawing.Size(630, 352);
			((System.ComponentModel.ISupportInitialize)(this.gridLayers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.constructionBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblId;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblThickness;
		private System.Windows.Forms.DataGridView gridLayers;
		private System.Windows.Forms.BindingSource constructionBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
		private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
		private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn3;
		private System.Windows.Forms.CheckBox cbPeFoil;
		private Europlan.Common.NumericBox numThickness;
		private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn4;
		private Europlan.Common.NumericColumn numericColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewComboBoxColumn colMaterial;
		private Europlan.Common.NumericColumn colLambdaValue;
		private Europlan.Common.NumericColumn colThickness;
		private Europlan.Common.NumericColumn colRValue;
	}
}
