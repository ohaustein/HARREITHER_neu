namespace Europlan.Common {
	partial class ExtendedCorrectionsGrid {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.gridExtendedCorrections = new System.Windows.Forms.DataGridView();
			this.rbExtendedCorrections = new System.Windows.Forms.RadioButton();
			this.rbStandardCorrections = new System.Windows.Forms.RadioButton();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.extendedCorrectionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.CircuitNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.correctAreaDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.areaValueDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.areaPercentageDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.areaReducedValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.areaUnheatedValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.correctRimDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.rimLengthValueDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.rimPercentageDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.rimCornersValueDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.correctConnectionsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.connectionsPercentageDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.connectionsValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridExtendedCorrections)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.extendedCorrectionsBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridExtendedCorrections
			// 
			this.gridExtendedCorrections.AllowUserToAddRows = false;
			this.gridExtendedCorrections.AllowUserToDeleteRows = false;
			this.gridExtendedCorrections.AllowUserToResizeColumns = false;
			this.gridExtendedCorrections.AllowUserToResizeRows = false;
			this.gridExtendedCorrections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridExtendedCorrections.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridExtendedCorrections.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridExtendedCorrections.ColumnHeadersHeight = 70;
			this.gridExtendedCorrections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.gridExtendedCorrections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CircuitNr,
            this.correctAreaDataGridViewCheckBoxColumn,
            this.areaValueDataGridViewTextBoxColumn,
            this.areaPercentageDataGridViewTextBoxColumn,
            this.areaReducedValueDataGridViewTextBoxColumn,
            this.areaUnheatedValueDataGridViewTextBoxColumn,
            this.correctRimDataGridViewCheckBoxColumn,
            this.rimLengthValueDataGridViewTextBoxColumn,
            this.rimPercentageDataGridViewTextBoxColumn,
            this.rimCornersValueDataGridViewTextBoxColumn,
            this.correctConnectionsDataGridViewCheckBoxColumn,
            this.connectionsPercentageDataGridViewTextBoxColumn,
            this.connectionsValueDataGridViewTextBoxColumn});
			this.gridExtendedCorrections.DataSource = this.extendedCorrectionsBindingSource;
			this.gridExtendedCorrections.Enabled = false;
			this.gridExtendedCorrections.Location = new System.Drawing.Point(0, 49);
			this.gridExtendedCorrections.MultiSelect = false;
			this.gridExtendedCorrections.Name = "gridExtendedCorrections";
			this.gridExtendedCorrections.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.gridExtendedCorrections.Size = new System.Drawing.Size(845, 407);
			this.gridExtendedCorrections.TabIndex = 5;
			this.gridExtendedCorrections.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridExtendedCorrections_CellValueChanged);
			this.gridExtendedCorrections.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.gridExtendedCorrections_RowsAdded);
			this.gridExtendedCorrections.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gridExtendedCorrections_CellPainting_1);
			this.gridExtendedCorrections.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridExtendedCorrections_CurrentCellDirtyStateChanged);
			// 
			// rbExtendedCorrections
			// 
			this.rbExtendedCorrections.AutoSize = true;
			this.rbExtendedCorrections.Location = new System.Drawing.Point(3, 26);
			this.rbExtendedCorrections.Name = "rbExtendedCorrections";
			this.rbExtendedCorrections.Size = new System.Drawing.Size(178, 17);
			this.rbExtendedCorrections.TabIndex = 4;
			this.rbExtendedCorrections.Text = "erweiterte Korrekturen aktivieren";
			this.rbExtendedCorrections.UseVisualStyleBackColor = true;
			this.rbExtendedCorrections.CheckedChanged += new System.EventHandler(this.rbExtendedCorrections_CheckedChanged);
			// 
			// rbStandardCorrections
			// 
			this.rbStandardCorrections.AutoSize = true;
			this.rbStandardCorrections.Checked = true;
			this.rbStandardCorrections.Location = new System.Drawing.Point(3, 3);
			this.rbStandardCorrections.Name = "rbStandardCorrections";
			this.rbStandardCorrections.Size = new System.Drawing.Size(344, 17);
			this.rbStandardCorrections.TabIndex = 3;
			this.rbStandardCorrections.TabStop = true;
			this.rbStandardCorrections.Text = "nur Standardkorrekturen verwenden (keine erweiterten Korrekturen)";
			this.rbStandardCorrections.UseVisualStyleBackColor = true;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "CircuitNr";
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
			this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle9;
			this.dataGridViewTextBoxColumn1.FillWeight = 70F;
			this.dataGridViewTextBoxColumn1.Frozen = true;
			this.dataGridViewTextBoxColumn1.HeaderText = "Heiz-\nkreis\nNr.";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn1.Width = 70;
			// 
			// extendedCorrectionsBindingSource
			// 
			this.extendedCorrectionsBindingSource.DataSource = typeof(Europlan.Common.ExtendedCorrections);
			// 
			// CircuitNr
			// 
			this.CircuitNr.DataPropertyName = "CircuitNr";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			this.CircuitNr.DefaultCellStyle = dataGridViewCellStyle2;
			this.CircuitNr.FillWeight = 80F;
			this.CircuitNr.Frozen = true;
			this.CircuitNr.HeaderText = "Heiz-\nkreis\nNr.";
			this.CircuitNr.Name = "CircuitNr";
			this.CircuitNr.ReadOnly = true;
			this.CircuitNr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.CircuitNr.Width = 80;
			// 
			// correctAreaDataGridViewCheckBoxColumn
			// 
			this.correctAreaDataGridViewCheckBoxColumn.DataPropertyName = "CorrectArea";
			this.correctAreaDataGridViewCheckBoxColumn.FillWeight = 70F;
			this.correctAreaDataGridViewCheckBoxColumn.Frozen = true;
			this.correctAreaDataGridViewCheckBoxColumn.HeaderText = "Vor-\ngabe";
			this.correctAreaDataGridViewCheckBoxColumn.Name = "correctAreaDataGridViewCheckBoxColumn";
			this.correctAreaDataGridViewCheckBoxColumn.Width = 70;
			// 
			// areaValueDataGridViewTextBoxColumn
			// 
			this.areaValueDataGridViewTextBoxColumn.DataPropertyName = "AreaValue";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F0";
			this.areaValueDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.areaValueDataGridViewTextBoxColumn.FillWeight = 55F;
			this.areaValueDataGridViewTextBoxColumn.Frozen = true;
			this.areaValueDataGridViewTextBoxColumn.HeaderText = "m²\n";
			this.areaValueDataGridViewTextBoxColumn.Name = "areaValueDataGridViewTextBoxColumn";
			this.areaValueDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.areaValueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.areaValueDataGridViewTextBoxColumn.Width = 55;
			// 
			// areaPercentageDataGridViewTextBoxColumn
			// 
			this.areaPercentageDataGridViewTextBoxColumn.DataPropertyName = "AreaPercentage";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.areaPercentageDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
			this.areaPercentageDataGridViewTextBoxColumn.FillWeight = 55F;
			this.areaPercentageDataGridViewTextBoxColumn.Frozen = true;
			this.areaPercentageDataGridViewTextBoxColumn.HeaderText = "%\n";
			this.areaPercentageDataGridViewTextBoxColumn.Name = "areaPercentageDataGridViewTextBoxColumn";
			this.areaPercentageDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PERCENTAGE;
			this.areaPercentageDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.areaPercentageDataGridViewTextBoxColumn.Width = 55;
			// 
			// areaReducedValueDataGridViewTextBoxColumn
			// 
			this.areaReducedValueDataGridViewTextBoxColumn.DataPropertyName = "AreaReducedValue";
			this.areaReducedValueDataGridViewTextBoxColumn.HeaderText = "AreaReducedValue";
			this.areaReducedValueDataGridViewTextBoxColumn.Name = "areaReducedValueDataGridViewTextBoxColumn";
			this.areaReducedValueDataGridViewTextBoxColumn.ReadOnly = true;
			this.areaReducedValueDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.areaReducedValueDataGridViewTextBoxColumn.Visible = false;
			// 
			// areaUnheatedValueDataGridViewTextBoxColumn
			// 
			this.areaUnheatedValueDataGridViewTextBoxColumn.DataPropertyName = "AreaUnheatedValue";
			this.areaUnheatedValueDataGridViewTextBoxColumn.HeaderText = "AreaUnheatedValue";
			this.areaUnheatedValueDataGridViewTextBoxColumn.Name = "areaUnheatedValueDataGridViewTextBoxColumn";
			this.areaUnheatedValueDataGridViewTextBoxColumn.ReadOnly = true;
			this.areaUnheatedValueDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.areaUnheatedValueDataGridViewTextBoxColumn.Visible = false;
			// 
			// correctRimDataGridViewCheckBoxColumn
			// 
			this.correctRimDataGridViewCheckBoxColumn.DataPropertyName = "CorrectRim";
			this.correctRimDataGridViewCheckBoxColumn.FillWeight = 70F;
			this.correctRimDataGridViewCheckBoxColumn.Frozen = true;
			this.correctRimDataGridViewCheckBoxColumn.HeaderText = "Vor-\ngabe";
			this.correctRimDataGridViewCheckBoxColumn.Name = "correctRimDataGridViewCheckBoxColumn";
			this.correctRimDataGridViewCheckBoxColumn.Width = 70;
			// 
			// rimLengthValueDataGridViewTextBoxColumn
			// 
			this.rimLengthValueDataGridViewTextBoxColumn.DataPropertyName = "RimLengthValue";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "F0";
			this.rimLengthValueDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
			this.rimLengthValueDataGridViewTextBoxColumn.FillWeight = 55F;
			this.rimLengthValueDataGridViewTextBoxColumn.Frozen = true;
			this.rimLengthValueDataGridViewTextBoxColumn.HeaderText = "m\n";
			this.rimLengthValueDataGridViewTextBoxColumn.Name = "rimLengthValueDataGridViewTextBoxColumn";
			this.rimLengthValueDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.rimLengthValueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.rimLengthValueDataGridViewTextBoxColumn.Width = 55;
			// 
			// rimPercentageDataGridViewTextBoxColumn
			// 
			this.rimPercentageDataGridViewTextBoxColumn.DataPropertyName = "RimPercentage";
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "F0";
			this.rimPercentageDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
			this.rimPercentageDataGridViewTextBoxColumn.FillWeight = 55F;
			this.rimPercentageDataGridViewTextBoxColumn.Frozen = true;
			this.rimPercentageDataGridViewTextBoxColumn.HeaderText = "%\n";
			this.rimPercentageDataGridViewTextBoxColumn.Name = "rimPercentageDataGridViewTextBoxColumn";
			this.rimPercentageDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PERCENTAGE;
			this.rimPercentageDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.rimPercentageDataGridViewTextBoxColumn.Width = 55;
			// 
			// rimCornersValueDataGridViewTextBoxColumn
			// 
			this.rimCornersValueDataGridViewTextBoxColumn.DataPropertyName = "RimCornersString";
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle7.Format = "F0";
			this.rimCornersValueDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
			this.rimCornersValueDataGridViewTextBoxColumn.FillWeight = 55F;
			this.rimCornersValueDataGridViewTextBoxColumn.Frozen = true;
			this.rimCornersValueDataGridViewTextBoxColumn.HeaderText = "Anzahl\nEcken";
			this.rimCornersValueDataGridViewTextBoxColumn.Name = "rimCornersValueDataGridViewTextBoxColumn";
			this.rimCornersValueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.rimCornersValueDataGridViewTextBoxColumn.Width = 55;
			// 
			// correctConnectionsDataGridViewCheckBoxColumn
			// 
			this.correctConnectionsDataGridViewCheckBoxColumn.DataPropertyName = "CorrectConnections";
			this.correctConnectionsDataGridViewCheckBoxColumn.FillWeight = 70F;
			this.correctConnectionsDataGridViewCheckBoxColumn.Frozen = true;
			this.correctConnectionsDataGridViewCheckBoxColumn.HeaderText = "Vor-\ngabe";
			this.correctConnectionsDataGridViewCheckBoxColumn.Name = "correctConnectionsDataGridViewCheckBoxColumn";
			this.correctConnectionsDataGridViewCheckBoxColumn.Width = 70;
			// 
			// connectionsPercentageDataGridViewTextBoxColumn
			// 
			this.connectionsPercentageDataGridViewTextBoxColumn.DataPropertyName = "ConnectionsPercentage";
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle8.Format = "F0";
			this.connectionsPercentageDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
			this.connectionsPercentageDataGridViewTextBoxColumn.FillWeight = 70F;
			this.connectionsPercentageDataGridViewTextBoxColumn.HeaderText = "Fläche\n%";
			this.connectionsPercentageDataGridViewTextBoxColumn.Name = "connectionsPercentageDataGridViewTextBoxColumn";
			this.connectionsPercentageDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PERCENTAGE;
			this.connectionsPercentageDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.connectionsPercentageDataGridViewTextBoxColumn.Width = 70;
			// 
			// connectionsValueDataGridViewTextBoxColumn
			// 
			this.connectionsValueDataGridViewTextBoxColumn.DataPropertyName = "ConnectionsValue";
			this.connectionsValueDataGridViewTextBoxColumn.HeaderText = "ConnectionsValue";
			this.connectionsValueDataGridViewTextBoxColumn.Name = "connectionsValueDataGridViewTextBoxColumn";
			this.connectionsValueDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.connectionsValueDataGridViewTextBoxColumn.Visible = false;
			// 
			// ExtendedCorrectionsGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridExtendedCorrections);
			this.Controls.Add(this.rbExtendedCorrections);
			this.Controls.Add(this.rbStandardCorrections);
			this.Name = "ExtendedCorrectionsGrid";
			this.Size = new System.Drawing.Size(845, 456);
			((System.ComponentModel.ISupportInitialize)(this.gridExtendedCorrections)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.extendedCorrectionsBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gridExtendedCorrections;
		private System.Windows.Forms.BindingSource extendedCorrectionsBindingSource;
		private System.Windows.Forms.RadioButton rbExtendedCorrections;
		private System.Windows.Forms.RadioButton rbStandardCorrections;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn CircuitNr;
		private System.Windows.Forms.DataGridViewCheckBoxColumn correctAreaDataGridViewCheckBoxColumn;
		private NumericColumn areaValueDataGridViewTextBoxColumn;
		private NumericColumn areaPercentageDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn areaReducedValueDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn areaUnheatedValueDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn correctRimDataGridViewCheckBoxColumn;
		private NumericColumn rimLengthValueDataGridViewTextBoxColumn;
		private NumericColumn rimPercentageDataGridViewTextBoxColumn;
		private NumericColumn rimCornersValueDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn correctConnectionsDataGridViewCheckBoxColumn;
		private NumericColumn connectionsPercentageDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn connectionsValueDataGridViewTextBoxColumn;
	}
}
