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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvRequiredMaterial = new System.Windows.Forms.DataGridView();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.partNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.requiredAmountDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.calculatedAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.unitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CanBeCalculated = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.requiredMaterialWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
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
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvRequiredMaterial.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dgvRequiredMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvRequiredMaterial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.partNumberDataGridViewTextBoxColumn,
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
			this.dgvRequiredMaterial.Size = new System.Drawing.Size(695, 404);
			this.dgvRequiredMaterial.TabIndex = 0;
			this.dgvRequiredMaterial.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequiredMaterial_CellValueChanged);
			this.dgvRequiredMaterial.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvRequiredMaterial_PreviewKeyDown);
			this.dgvRequiredMaterial.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRequiredMaterial_CellFormatting);
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
			this.nameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.ReadOnly = true;
			this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// partNumberDataGridViewTextBoxColumn
			// 
			this.partNumberDataGridViewTextBoxColumn.DataPropertyName = "PartNumber";
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
			this.partNumberDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
			this.partNumberDataGridViewTextBoxColumn.HeaderText = "Bestellnummer";
			this.partNumberDataGridViewTextBoxColumn.Name = "partNumberDataGridViewTextBoxColumn";
			this.partNumberDataGridViewTextBoxColumn.ReadOnly = true;
			this.partNumberDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.partNumberDataGridViewTextBoxColumn.Width = 120;
			// 
			// requiredAmountDataGridViewTextBoxColumn
			// 
			this.requiredAmountDataGridViewTextBoxColumn.DataPropertyName = "RequiredAmount";
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle10.Format = "F0";
			this.requiredAmountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle10;
			this.requiredAmountDataGridViewTextBoxColumn.HeaderText = "Menge";
			this.requiredAmountDataGridViewTextBoxColumn.Name = "requiredAmountDataGridViewTextBoxColumn";
			this.requiredAmountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// calculatedAmountDataGridViewTextBoxColumn
			// 
			this.calculatedAmountDataGridViewTextBoxColumn.DataPropertyName = "CalculatedAmount";
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
			this.calculatedAmountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle11;
			this.calculatedAmountDataGridViewTextBoxColumn.HeaderText = "(berechnet)";
			this.calculatedAmountDataGridViewTextBoxColumn.Name = "calculatedAmountDataGridViewTextBoxColumn";
			this.calculatedAmountDataGridViewTextBoxColumn.ReadOnly = true;
			this.calculatedAmountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.calculatedAmountDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// unitDataGridViewTextBoxColumn
			// 
			this.unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
			this.unitDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle12;
			this.unitDataGridViewTextBoxColumn.HeaderText = "Einheit";
			this.unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			this.unitDataGridViewTextBoxColumn.ReadOnly = true;
			this.unitDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.unitDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.unitDataGridViewTextBoxColumn.Width = 70;
			// 
			// CanBeCalculated
			// 
			this.CanBeCalculated.DataPropertyName = "CanBeCalculated";
			this.CanBeCalculated.HeaderText = "CanBeCalculated";
			this.CanBeCalculated.Name = "CanBeCalculated";
			this.CanBeCalculated.ReadOnly = true;
			this.CanBeCalculated.Visible = false;
			// 
			// requiredMaterialWrapperBindingSource
			// 
			this.requiredMaterialWrapperBindingSource.DataSource = typeof(Europlan.Common.RequiredMaterialWrapper);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(0, 427);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(648, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Bitte unbedingt beachten: Rot markierte Materialpositionen müssen vom Planenden s" +
				"elbst anhand der Planungsvorlage ermittelt werden!";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(0, 410);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(679, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Der Materialbedarf wurde rechnerisch ermittelt und muss vom Planenden überprüft b" +
				"zw. an die tatsächlichen Anforderungen angepasst werden";
			// 
			// RequiredMaterialGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label2);
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
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn partNumberDataGridViewTextBoxColumn;
		private NumericColumn requiredAmountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn calculatedAmountDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn CanBeCalculated;
		private System.Windows.Forms.Label label2;
	}
}
