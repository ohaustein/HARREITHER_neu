namespace Europlan.Common {
	partial class HithermWallGrid {
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
			this.dgvWalls = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Construction = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.constructionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Deckschicht = new Europlan.Common.NumericColumn();
			this.kValueDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.bereinigenDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.additionalInsulationDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.tempBehindHeatDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.tempBehindCoolDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.hithermWallBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnSelectConstruction = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvWalls)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.hithermWallBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvWalls
			// 
			this.dgvWalls.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvWalls.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvWalls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvWalls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.Construction,
            this.constructionDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.Deckschicht,
            this.kValueDataGridViewTextBoxColumn,
            this.bereinigenDataGridViewCheckBoxColumn,
            this.additionalInsulationDataGridViewTextBoxColumn,
            this.tempBehindHeatDataGridViewTextBoxColumn,
            this.tempBehindCoolDataGridViewTextBoxColumn});
			this.dgvWalls.DataSource = this.hithermWallBindingSource;
			this.dgvWalls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvWalls.Location = new System.Drawing.Point(0, 0);
			this.dgvWalls.MultiSelect = false;
			this.dgvWalls.Name = "dgvWalls";
			this.dgvWalls.Size = new System.Drawing.Size(684, 408);
			this.dgvWalls.TabIndex = 0;
			this.dgvWalls.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWalls_CellLeave);
			this.dgvWalls.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvWalls_RowsAdded);
			this.dgvWalls.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvWalls_DefaultValuesNeeded);
			this.dgvWalls.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWalls_CellEnter);
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.FillWeight = 60F;
			this.idDataGridViewTextBoxColumn.HeaderText = "Nummer";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.Width = 60;
			// 
			// Construction
			// 
			this.Construction.DataPropertyName = "Construction";
			this.Construction.HeaderText = "Construction";
			this.Construction.Name = "Construction";
			this.Construction.Visible = false;
			// 
			// constructionDataGridViewTextBoxColumn
			// 
			this.constructionDataGridViewTextBoxColumn.DataPropertyName = "ConstructionName";
			this.constructionDataGridViewTextBoxColumn.FillWeight = 70F;
			this.constructionDataGridViewTextBoxColumn.HeaderText = "Basis-\nKonstr.";
			this.constructionDataGridViewTextBoxColumn.Name = "constructionDataGridViewTextBoxColumn";
			this.constructionDataGridViewTextBoxColumn.ReadOnly = true;
			this.constructionDataGridViewTextBoxColumn.Width = 70;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.FillWeight = 120F;
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.Width = 120;
			// 
			// Deckschicht
			// 
			this.Deckschicht.DataPropertyName = "Deckschicht";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F3";
			this.Deckschicht.DefaultCellStyle = dataGridViewCellStyle2;
			this.Deckschicht.FillWeight = 60F;
			this.Deckschicht.HeaderText = "Decksch.\nR\n(m²K/W)";
			this.Deckschicht.Name = "Deckschicht";
			this.Deckschicht.NumEditType = Europlan.Common.NumericBox.NumericEditType.R_VALUE;
			this.Deckschicht.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Deckschicht.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.Deckschicht.Width = 60;
			// 
			// kValueDataGridViewTextBoxColumn
			// 
			this.kValueDataGridViewTextBoxColumn.DataPropertyName = "UValue";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F3";
			this.kValueDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.kValueDataGridViewTextBoxColumn.FillWeight = 60F;
			this.kValueDataGridViewTextBoxColumn.HeaderText = "U-Wert\n(W/m²K)";
			this.kValueDataGridViewTextBoxColumn.Name = "kValueDataGridViewTextBoxColumn";
			this.kValueDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.R_VALUE;
			this.kValueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.kValueDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.kValueDataGridViewTextBoxColumn.Width = 60;
			// 
			// bereinigenDataGridViewCheckBoxColumn
			// 
			this.bereinigenDataGridViewCheckBoxColumn.DataPropertyName = "Bereinigen";
			this.bereinigenDataGridViewCheckBoxColumn.FillWeight = 60F;
			this.bereinigenDataGridViewCheckBoxColumn.HeaderText = "Wärme\nBedarf\nberein.";
			this.bereinigenDataGridViewCheckBoxColumn.Name = "bereinigenDataGridViewCheckBoxColumn";
			this.bereinigenDataGridViewCheckBoxColumn.Width = 60;
			// 
			// additionalInsulationDataGridViewTextBoxColumn
			// 
			this.additionalInsulationDataGridViewTextBoxColumn.DataPropertyName = "AdditionalInsulation";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F3";
			this.additionalInsulationDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
			this.additionalInsulationDataGridViewTextBoxColumn.FillWeight = 80F;
			this.additionalInsulationDataGridViewTextBoxColumn.HeaderText = "zus. Dämmg\nR\n(m²K/W)";
			this.additionalInsulationDataGridViewTextBoxColumn.Name = "additionalInsulationDataGridViewTextBoxColumn";
			this.additionalInsulationDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.R_VALUE;
			this.additionalInsulationDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.additionalInsulationDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.additionalInsulationDataGridViewTextBoxColumn.Width = 80;
			// 
			// tempBehindHeatDataGridViewTextBoxColumn
			// 
			this.tempBehindHeatDataGridViewTextBoxColumn.DataPropertyName = "TempBehindHeat";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "F0";
			this.tempBehindHeatDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
			this.tempBehindHeatDataGridViewTextBoxColumn.FillWeight = 60F;
			this.tempBehindHeatDataGridViewTextBoxColumn.HeaderText = "Temp.\nHeiz\n(°C)";
			this.tempBehindHeatDataGridViewTextBoxColumn.Name = "tempBehindHeatDataGridViewTextBoxColumn";
			this.tempBehindHeatDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.tempBehindHeatDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.tempBehindHeatDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.tempBehindHeatDataGridViewTextBoxColumn.Width = 60;
			// 
			// tempBehindCoolDataGridViewTextBoxColumn
			// 
			this.tempBehindCoolDataGridViewTextBoxColumn.DataPropertyName = "TempBehindCool";
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "F0";
			this.tempBehindCoolDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
			this.tempBehindCoolDataGridViewTextBoxColumn.FillWeight = 60F;
			this.tempBehindCoolDataGridViewTextBoxColumn.HeaderText = "Temp.\nKühl\n(°C)";
			this.tempBehindCoolDataGridViewTextBoxColumn.Name = "tempBehindCoolDataGridViewTextBoxColumn";
			this.tempBehindCoolDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.tempBehindCoolDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.tempBehindCoolDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.tempBehindCoolDataGridViewTextBoxColumn.Width = 60;
			// 
			// hithermWallBindingSource
			// 
			this.hithermWallBindingSource.DataSource = typeof(Europlan.Common.HithermWall);
			// 
			// btnSelectConstruction
			// 
			this.btnSelectConstruction.Location = new System.Drawing.Point(372, 202);
			this.btnSelectConstruction.Name = "btnSelectConstruction";
			this.btnSelectConstruction.Size = new System.Drawing.Size(24, 23);
			this.btnSelectConstruction.TabIndex = 1;
			this.btnSelectConstruction.Text = "...";
			this.btnSelectConstruction.UseVisualStyleBackColor = true;
			this.btnSelectConstruction.Visible = false;
			this.btnSelectConstruction.Click += new System.EventHandler(this.btnSelectConstruction_Click);
			// 
			// HithermWallGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnSelectConstruction);
			this.Controls.Add(this.dgvWalls);
			this.Name = "HithermWallGrid";
			this.Size = new System.Drawing.Size(684, 408);
			((System.ComponentModel.ISupportInitialize)(this.dgvWalls)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.hithermWallBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvWalls;
		private System.Windows.Forms.BindingSource hithermWallBindingSource;
		private System.Windows.Forms.Button btnSelectConstruction;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn Construction;
		private System.Windows.Forms.DataGridViewTextBoxColumn constructionDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn Deckschicht;
		private NumericColumn kValueDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn bereinigenDataGridViewCheckBoxColumn;
		private NumericColumn additionalInsulationDataGridViewTextBoxColumn;
		private NumericColumn tempBehindHeatDataGridViewTextBoxColumn;
		private NumericColumn tempBehindCoolDataGridViewTextBoxColumn;
	}
}
