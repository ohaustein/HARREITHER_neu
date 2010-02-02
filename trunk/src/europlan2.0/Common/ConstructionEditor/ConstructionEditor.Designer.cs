namespace Europlan.Common {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblId = new System.Windows.Forms.Label();
			this.txtId = new System.Windows.Forms.TextBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.lblThickness = new System.Windows.Forms.Label();
			this.gridLayers = new System.Windows.Forms.DataGridView();
			this.constructionLayerBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cbPeFoil = new System.Windows.Forms.CheckBox();
			this.lblFactor = new System.Windows.Forms.Label();
			this.numFactor = new Europlan.Common.NumericBox();
			this.numThickness = new Europlan.Common.NumericBox();
			this.numericColumn1 = new Europlan.Common.NumericColumn();
			this.chkHitherm = new System.Windows.Forms.CheckBox();
			this.chkHithermCompact = new System.Windows.Forms.CheckBox();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colMaterial = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colLambdaValue = new Europlan.Common.NumericColumn();
			this.colThickness = new Europlan.Common.NumericColumn();
			this.colRValue = new Europlan.Common.NumericColumn();
			this.colMaterialId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridLayers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.constructionLayerBindingSource)).BeginInit();
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
			this.txtId.TabIndex = 0;
			this.txtId.TextChanged += new System.EventHandler(this.txtId_TextChanged);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(98, 29);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(298, 20);
			this.txtName.TabIndex = 1;
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
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridLayers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridLayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colMaterial,
            this.colLambdaValue,
            this.colThickness,
            this.colRValue,
            this.colMaterialId});
			this.gridLayers.DataSource = this.constructionLayerBindingSource;
			this.gridLayers.Location = new System.Drawing.Point(3, 81);
			this.gridLayers.MultiSelect = false;
			this.gridLayers.Name = "gridLayers";
			this.gridLayers.Size = new System.Drawing.Size(624, 268);
			this.gridLayers.TabIndex = 5;
			// 
			// constructionLayerBindingSource
			// 
			this.constructionLayerBindingSource.DataSource = typeof(Europlan.Common.ConstructionLayer);
			// 
			// cbPeFoil
			// 
			this.cbPeFoil.AutoSize = true;
			this.cbPeFoil.Location = new System.Drawing.Point(98, 57);
			this.cbPeFoil.Name = "cbPeFoil";
			this.cbPeFoil.Size = new System.Drawing.Size(65, 17);
			this.cbPeFoil.TabIndex = 4;
			this.cbPeFoil.Text = "PE Folie";
			this.cbPeFoil.UseVisualStyleBackColor = true;
			this.cbPeFoil.Visible = false;
			// 
			// lblFactor
			// 
			this.lblFactor.AutoSize = true;
			this.lblFactor.Location = new System.Drawing.Point(3, 58);
			this.lblFactor.Name = "lblFactor";
			this.lblFactor.Size = new System.Drawing.Size(40, 13);
			this.lblFactor.TabIndex = 9;
			this.lblFactor.Text = "Faktor:";
			this.lblFactor.Visible = false;
			// 
			// numFactor
			// 
			this.numFactor.EditType = Europlan.Common.NumericBox.NumericEditType.FACTOR;
			this.numFactor.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numFactor.Location = new System.Drawing.Point(98, 55);
			this.numFactor.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numFactor.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.numFactor.Name = "numFactor";
			this.numFactor.Size = new System.Drawing.Size(100, 20);
			this.numFactor.TabIndex = 2;
			this.numFactor.Text = "1";
			this.numFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numFactor.Visible = false;
			this.numFactor.ValueChanged += new System.EventHandler(this.numFactor_ValueChanged);
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
			this.numThickness.MaxValue = null;
			this.numThickness.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numThickness.Name = "numThickness";
			this.numThickness.Size = new System.Drawing.Size(100, 20);
			this.numThickness.TabIndex = 3;
			this.numThickness.Text = "0";
			this.numThickness.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numThickness.ValueChanged += new System.EventHandler(this.numThickness_ValueChanged);
			// 
			// numericColumn1
			// 
			this.numericColumn1.DataPropertyName = "LambdaValue";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "F0";
			this.numericColumn1.DefaultCellStyle = dataGridViewCellStyle5;
			this.numericColumn1.FillWeight = 70F;
			this.numericColumn1.HeaderText = "lambda (W/mK)";
			this.numericColumn1.Name = "numericColumn1";
			this.numericColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.numericColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.numericColumn1.Width = 70;
			// 
			// chkHitherm
			// 
			this.chkHitherm.AutoSize = true;
			this.chkHitherm.Location = new System.Drawing.Point(220, 57);
			this.chkHitherm.Name = "chkHitherm";
			this.chkHitherm.Size = new System.Drawing.Size(70, 17);
			this.chkHitherm.TabIndex = 10;
			this.chkHitherm.Text = "Hitherm®";
			this.chkHitherm.UseVisualStyleBackColor = true;
			this.chkHitherm.Visible = false;
			this.chkHitherm.CheckedChanged += new System.EventHandler(this.chkHitherm_CheckedChanged);
			// 
			// chkHithermCompact
			// 
			this.chkHithermCompact.AutoSize = true;
			this.chkHithermCompact.Location = new System.Drawing.Point(296, 57);
			this.chkHithermCompact.Name = "chkHithermCompact";
			this.chkHithermCompact.Size = new System.Drawing.Size(115, 17);
			this.chkHithermCompact.TabIndex = 11;
			this.chkHithermCompact.Text = "Hitherm® Compact";
			this.chkHithermCompact.UseVisualStyleBackColor = true;
			this.chkHithermCompact.Visible = false;
			this.chkHithermCompact.CheckedChanged += new System.EventHandler(this.chkHithermCompact_CheckedChanged);
			// 
			// colName
			// 
			this.colName.DataPropertyName = "Name";
			this.colName.FillWeight = 150F;
			this.colName.HeaderText = "Bezeichnung";
			this.colName.Name = "colName";
			this.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colName.Width = 150;
			// 
			// colMaterial
			// 
			this.colMaterial.DataPropertyName = "LayerMaterial";
			this.colMaterial.FillWeight = 150F;
			this.colMaterial.HeaderText = "Material";
			this.colMaterial.Name = "colMaterial";
			this.colMaterial.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colMaterial.Width = 150;
			// 
			// colLambdaValue
			// 
			this.colLambdaValue.DataPropertyName = "LambdaValue";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.colLambdaValue.DefaultCellStyle = dataGridViewCellStyle2;
			this.colLambdaValue.FillWeight = 70F;
			this.colLambdaValue.HeaderText = "lambda (W/m K)";
			this.colLambdaValue.Name = "colLambdaValue";
			this.colLambdaValue.NumEditType = Europlan.Common.NumericBox.NumericEditType.LAMBDA_VALUE;
			this.colLambdaValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colLambdaValue.Width = 70;
			// 
			// colThickness
			// 
			this.colThickness.DataPropertyName = "Thickness";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F0";
			this.colThickness.DefaultCellStyle = dataGridViewCellStyle3;
			this.colThickness.FillWeight = 70F;
			this.colThickness.HeaderText = "d (mm)";
			this.colThickness.Name = "colThickness";
			this.colThickness.NumEditType = Europlan.Common.NumericBox.NumericEditType.FLOOR_CONSTRUCTION_THICKNESS;
			this.colThickness.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colThickness.Width = 70;
			// 
			// colRValue
			// 
			this.colRValue.DataPropertyName = "RValue";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.colRValue.DefaultCellStyle = dataGridViewCellStyle4;
			this.colRValue.FillWeight = 70F;
			this.colRValue.HeaderText = "R (m²K/W)";
			this.colRValue.Name = "colRValue";
			this.colRValue.NumEditType = Europlan.Common.NumericBox.NumericEditType.R_VALUE;
			this.colRValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colRValue.Width = 70;
			// 
			// colMaterialId
			// 
			this.colMaterialId.DataPropertyName = "MaterialId";
			this.colMaterialId.HeaderText = "MaterialId";
			this.colMaterialId.Name = "colMaterialId";
			this.colMaterialId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colMaterialId.Visible = false;
			// 
			// ConstructionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkHithermCompact);
			this.Controls.Add(this.chkHitherm);
			this.Controls.Add(this.numFactor);
			this.Controls.Add(this.lblFactor);
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
			((System.ComponentModel.ISupportInitialize)(this.constructionLayerBindingSource)).EndInit();
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
		private System.Windows.Forms.CheckBox cbPeFoil;
		private Europlan.Common.NumericBox numThickness;
		private Europlan.Common.NumericColumn numericColumn1;
		private System.Windows.Forms.BindingSource constructionLayerBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.Label lblFactor;
		private NumericBox numFactor;
		private System.Windows.Forms.CheckBox chkHitherm;
		private System.Windows.Forms.CheckBox chkHithermCompact;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewComboBoxColumn colMaterial;
		private NumericColumn colLambdaValue;
		private NumericColumn colThickness;
		private NumericColumn colRValue;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialId;
	}
}
