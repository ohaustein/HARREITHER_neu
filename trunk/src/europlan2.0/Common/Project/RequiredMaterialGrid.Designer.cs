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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvRequiredMaterial = new System.Windows.Forms.DataGridView();
			this.CanBeCalculated = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.partNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.requiredAmountDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.calculatedAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.unitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.requiredMaterialWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label1 = new System.Windows.Forms.Label();
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
			this.dgvRequiredMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvRequiredMaterial.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvRequiredMaterial.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvRequiredMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvRequiredMaterial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.partNumberDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.requiredAmountDataGridViewTextBoxColumn,
            this.calculatedAmountDataGridViewTextBoxColumn,
            this.unitDataGridViewTextBoxColumn,
            this.CanBeCalculated});
			this.dgvRequiredMaterial.DataSource = this.requiredMaterialWrapperBindingSource;
			this.dgvRequiredMaterial.Location = new System.Drawing.Point(0, 0);
			this.dgvRequiredMaterial.Name = "dgvRequiredMaterial";
			this.dgvRequiredMaterial.RowHeadersVisible = false;
			this.dgvRequiredMaterial.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvRequiredMaterial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dgvRequiredMaterial.Size = new System.Drawing.Size(695, 424);
			this.dgvRequiredMaterial.TabIndex = 0;
			this.dgvRequiredMaterial.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequiredMaterial_CellValueChanged);
			this.dgvRequiredMaterial.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvRequiredMaterial_PreviewKeyDown);
			this.dgvRequiredMaterial.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRequiredMaterial_CellFormatting);
			// 
			// CanBeCalculated
			// 
			this.CanBeCalculated.DataPropertyName = "CanBeCalculated";
			this.CanBeCalculated.HeaderText = "CanBeCalculated";
			this.CanBeCalculated.Name = "CanBeCalculated";
			this.CanBeCalculated.ReadOnly = true;
			this.CanBeCalculated.Visible = false;
			// 
			// partNumberDataGridViewTextBoxColumn
			// 
			this.partNumberDataGridViewTextBoxColumn.DataPropertyName = "PartNumber";
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			this.partNumberDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.partNumberDataGridViewTextBoxColumn.HeaderText = "Bestellnummer";
			this.partNumberDataGridViewTextBoxColumn.Name = "partNumberDataGridViewTextBoxColumn";
			this.partNumberDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			this.nameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// requiredAmountDataGridViewTextBoxColumn
			// 
			this.requiredAmountDataGridViewTextBoxColumn.DataPropertyName = "RequiredAmount";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.requiredAmountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
			this.requiredAmountDataGridViewTextBoxColumn.HeaderText = "Menge";
			this.requiredAmountDataGridViewTextBoxColumn.Name = "requiredAmountDataGridViewTextBoxColumn";
			this.requiredAmountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.requiredAmountDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.requiredAmountDataGridViewTextBoxColumn.Width = 70;
			// 
			// calculatedAmountDataGridViewTextBoxColumn
			// 
			this.calculatedAmountDataGridViewTextBoxColumn.DataPropertyName = "CalculatedAmount";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
			this.calculatedAmountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
			this.calculatedAmountDataGridViewTextBoxColumn.HeaderText = "(berechnet)";
			this.calculatedAmountDataGridViewTextBoxColumn.Name = "calculatedAmountDataGridViewTextBoxColumn";
			this.calculatedAmountDataGridViewTextBoxColumn.ReadOnly = true;
			this.calculatedAmountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.calculatedAmountDataGridViewTextBoxColumn.Width = 70;
			// 
			// unitDataGridViewTextBoxColumn
			// 
			this.unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
			this.unitDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
			this.unitDataGridViewTextBoxColumn.HeaderText = "Einheit";
			this.unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			this.unitDataGridViewTextBoxColumn.ReadOnly = true;
			this.unitDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.unitDataGridViewTextBoxColumn.Width = 70;
			// 
			// requiredMaterialWrapperBindingSource
			// 
			this.requiredMaterialWrapperBindingSource.DataSource = typeof(Europlan.Common.RequiredMaterialWrapper);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(0, 427);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(648, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Bitte unbedingt beachten: Rot markierte Materialpositionen müssen vom Planenden s" +
				"elbst anhand der Planungsvorlage ermittelt werden!";
			// 
			// RequiredMaterialGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dgvRequiredMaterial);
			this.Name = "RequiredMaterialGrid";
			this.Size = new System.Drawing.Size(695, 441);
			((System.ComponentModel.ISupportInitialize)(this.dgvRequiredMaterial)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.requiredMaterialWrapperBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvRequiredMaterial;
		private System.Windows.Forms.BindingSource requiredMaterialWrapperBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn partNumberDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn requiredAmountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn calculatedAmountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn CanBeCalculated;
		private System.Windows.Forms.Label label1;
	}
}
