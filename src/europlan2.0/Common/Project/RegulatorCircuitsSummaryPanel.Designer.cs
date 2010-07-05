namespace Europlan.Common {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.gridRegulatoryCircuits = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.heatFlowTemperatureDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.coolFlowTemperatureDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.regulatoryCircuitsSource = new System.Windows.Forms.BindingSource(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.btnNext = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
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
            this.heatFlowTemperatureDataGridViewTextBoxColumn,
            this.coolFlowTemperatureDataGridViewTextBoxColumn});
			this.gridRegulatoryCircuits.DataMember = "RegulatorCircuits";
			this.gridRegulatoryCircuits.DataSource = this.regulatoryCircuitsSource;
			this.gridRegulatoryCircuits.Location = new System.Drawing.Point(3, 27);
			this.gridRegulatoryCircuits.MultiSelect = false;
			this.gridRegulatoryCircuits.Name = "gridRegulatoryCircuits";
			this.gridRegulatoryCircuits.Size = new System.Drawing.Size(756, 453);
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
			// heatFlowTemperatureDataGridViewTextBoxColumn
			// 
			this.heatFlowTemperatureDataGridViewTextBoxColumn.DataPropertyName = "HeatFlowTemperature";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.heatFlowTemperatureDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.heatFlowTemperatureDataGridViewTextBoxColumn.HeaderText = "TvHeiz (°C)";
			this.heatFlowTemperatureDataGridViewTextBoxColumn.Name = "heatFlowTemperatureDataGridViewTextBoxColumn";
			this.heatFlowTemperatureDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.REGULATORY_CIRCUIT_TEMP;
			this.heatFlowTemperatureDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.heatFlowTemperatureDataGridViewTextBoxColumn.ToolTipText = "Vorlauftemperatur im Heizbetrieb";
			this.heatFlowTemperatureDataGridViewTextBoxColumn.Width = 50;
			// 
			// coolFlowTemperatureDataGridViewTextBoxColumn
			// 
			this.coolFlowTemperatureDataGridViewTextBoxColumn.DataPropertyName = "CoolFlowTemperature";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F0";
			this.coolFlowTemperatureDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.coolFlowTemperatureDataGridViewTextBoxColumn.HeaderText = "TvKühl (°C)";
			this.coolFlowTemperatureDataGridViewTextBoxColumn.Name = "coolFlowTemperatureDataGridViewTextBoxColumn";
			this.coolFlowTemperatureDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.REGULATORY_CIRCUIT_TEMP;
			this.coolFlowTemperatureDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.coolFlowTemperatureDataGridViewTextBoxColumn.ToolTipText = "Vorlauftemperatur im Kühlbetrieb";
			this.coolFlowTemperatureDataGridViewTextBoxColumn.Visible = false;
			this.coolFlowTemperatureDataGridViewTextBoxColumn.Width = 50;
			// 
			// regulatoryCircuitsSource
			// 
			this.regulatoryCircuitsSource.DataSource = typeof(Europlan.Common.Project);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 24);
			this.label1.TabIndex = 3;
			this.label1.Text = "Regelkreise";
			// 
			// btnNext
			// 
			this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNext.Location = new System.Drawing.Point(684, 486);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 31;
			this.btnNext.Text = "Weiter";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// RegulatorCircuitsSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridRegulatoryCircuits);
			this.helpProvider.SetHelpKeyword(this, "html\\euro0sku.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.Name = "RegulatorCircuitsSummaryPanel";
			this.helpProvider.SetShowHelp(this, true);
			this.Size = new System.Drawing.Size(762, 509);
			((System.ComponentModel.ISupportInitialize)(this.gridRegulatoryCircuits)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.regulatoryCircuitsSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gridRegulatoryCircuits;
		private System.Windows.Forms.BindingSource regulatoryCircuitsSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn heatFlowTemperatureDataGridViewTextBoxColumn;
		private NumericColumn coolFlowTemperatureDataGridViewTextBoxColumn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.HelpProvider helpProvider;
	}
}
