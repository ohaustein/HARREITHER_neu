namespace Europlan.Application {
	partial class RegulatorCircuitsSummaryPanel {
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
			this.gridRegulatoryCircuits = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.flowTemperatureDataGridViewTextBoxColumn = new Europlan.Application.NumericColumn();
			this.regulatoryCircuitsSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridRegulatoryCircuits)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.regulatoryCircuitsSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridRegulatoryCircuits
			// 
			this.gridRegulatoryCircuits.AllowUserToResizeRows = false;
			this.gridRegulatoryCircuits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridRegulatoryCircuits.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridRegulatoryCircuits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridRegulatoryCircuits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridRegulatoryCircuits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.flowTemperatureDataGridViewTextBoxColumn});
			this.gridRegulatoryCircuits.DataMember = "RegulatorCircuits";
			this.gridRegulatoryCircuits.DataSource = this.regulatoryCircuitsSource;
			this.gridRegulatoryCircuits.Location = new System.Drawing.Point(3, 3);
			this.gridRegulatoryCircuits.MultiSelect = false;
			this.gridRegulatoryCircuits.Name = "gridRegulatoryCircuits";
			this.gridRegulatoryCircuits.Size = new System.Drawing.Size(507, 255);
			this.gridRegulatoryCircuits.TabIndex = 0;
			this.gridRegulatoryCircuits.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.regulatoryCircuitsGrid_CellValueChanged);
			this.gridRegulatoryCircuits.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.regulatoryCircuitsGrid_UserDeletedRow);
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "Nr.";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.idDataGridViewTextBoxColumn.ToolTipText = "Eindeutige Regelkreisnummer";
			this.idDataGridViewTextBoxColumn.Width = 50;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.nameDataGridViewTextBoxColumn.ToolTipText = "Bezeichnung des Regelkreises";
			this.nameDataGridViewTextBoxColumn.Width = 200;
			// 
			// flowTemperatureDataGridViewTextBoxColumn
			// 
			this.flowTemperatureDataGridViewTextBoxColumn.DataPropertyName = "FlowTemperature";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.flowTemperatureDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.flowTemperatureDataGridViewTextBoxColumn.HeaderText = "TvHeiz (°C)";
			this.flowTemperatureDataGridViewTextBoxColumn.Name = "flowTemperatureDataGridViewTextBoxColumn";
			this.flowTemperatureDataGridViewTextBoxColumn.NumEditType = Europlan.Application.NumericBox.NumericEditType.REGULATORY_CIRCUIT_TEMP;
			this.flowTemperatureDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.flowTemperatureDataGridViewTextBoxColumn.ToolTipText = "Vorlauftemperatur im Heizbetrieb";
			this.flowTemperatureDataGridViewTextBoxColumn.Width = 50;
			// 
			// regulatoryCircuitsSource
			// 
			this.regulatoryCircuitsSource.DataSource = typeof(Europlan.Application.Project);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
			this.dataGridViewTextBoxColumn1.HeaderText = "Nr.";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn2.HeaderText = "Beschreibung";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "FlowTemperature";
			this.dataGridViewTextBoxColumn3.HeaderText = "TvHeiz °C";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.Width = 80;
			// 
			// RegulatorCircuitsSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridRegulatoryCircuits);
			this.Name = "RegulatorCircuitsSummaryPanel";
			this.Size = new System.Drawing.Size(513, 261);
			((System.ComponentModel.ISupportInitialize)(this.gridRegulatoryCircuits)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.regulatoryCircuitsSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridRegulatoryCircuits;
		private System.Windows.Forms.BindingSource regulatoryCircuitsSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn flowTemperatureDataGridViewTextBoxColumn;
	}
}
