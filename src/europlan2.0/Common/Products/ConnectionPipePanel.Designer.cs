namespace Europlan.Common {
	partial class ConnectionPipePanel {
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
			this.dgvConnectionPipes = new System.Windows.Forms.DataGridView();
			this.connectionPipeBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Room = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.vorlaufDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.ruecklaufDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.roomDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.productDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PlannedCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.onlyFirstDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.printDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.PipeType = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.Verlegeart = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.Insulation = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.Area = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.HeatLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CoolLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgvConnectionPipes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.connectionPipeBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvConnectionPipes
			// 
			this.dgvConnectionPipes.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvConnectionPipes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvConnectionPipes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvConnectionPipes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Room,
            this.vorlaufDataGridViewTextBoxColumn,
            this.ruecklaufDataGridViewTextBoxColumn,
            this.roomDataGridViewComboBoxColumn,
            this.productDataGridViewComboBoxColumn,
            this.PlannedCircuits,
            this.onlyFirstDataGridViewCheckBoxColumn,
            this.printDataGridViewCheckBoxColumn,
            this.PipeType,
            this.Verlegeart,
            this.Insulation,
            this.Area,
            this.HeatLoad,
            this.CoolLoad});
			this.dgvConnectionPipes.DataSource = this.connectionPipeBindingSource;
			this.dgvConnectionPipes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvConnectionPipes.Location = new System.Drawing.Point(0, 0);
			this.dgvConnectionPipes.MultiSelect = false;
			this.dgvConnectionPipes.Name = "dgvConnectionPipes";
			this.dgvConnectionPipes.Size = new System.Drawing.Size(903, 375);
			this.dgvConnectionPipes.TabIndex = 2;
			this.dgvConnectionPipes.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConnectionPipes_CellValueChanged);
			this.dgvConnectionPipes.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConnectionPipes_CellLeave);
			this.dgvConnectionPipes.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvConnectionPipes_PreviewKeyDown);
			this.dgvConnectionPipes.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvConnectionPipes_UserDeletedRow);
			this.dgvConnectionPipes.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dgvConnectionPipes_CellParsing);
			this.dgvConnectionPipes.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvConnectionPipes_DataError);
			this.dgvConnectionPipes.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConnectionPipes_CellEnter);
			// 
			// connectionPipeBindingSource
			// 
			this.connectionPipeBindingSource.DataSource = typeof(Europlan.Common.ConnectionPipe);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "Room";
			this.dataGridViewTextBoxColumn1.FillWeight = 70F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Raum";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 70;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "PlannedProduct";
			this.dataGridViewTextBoxColumn2.FillWeight = 70F;
			this.dataGridViewTextBoxColumn2.HeaderText = "Teilsystem";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewTextBoxColumn2.Width = 120;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "PlannedCircuits";
			this.dataGridViewTextBoxColumn3.HeaderText = "Anz.";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Visible = false;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.DataPropertyName = "Area";
			this.dataGridViewTextBoxColumn4.HeaderText = "Fläche";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.Visible = false;
			this.dataGridViewTextBoxColumn4.Width = 50;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.DataPropertyName = "HeatLoad";
			this.dataGridViewTextBoxColumn5.HeaderText = "Heiz-\nleistung";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Visible = false;
			this.dataGridViewTextBoxColumn5.Width = 50;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.DataPropertyName = "CoolLoad";
			this.dataGridViewTextBoxColumn6.HeaderText = "Kühl-\nleistung";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.Visible = false;
			this.dataGridViewTextBoxColumn6.Width = 50;
			// 
			// Room
			// 
			this.Room.DataPropertyName = "DestinationRoom";
			this.Room.FillWeight = 70F;
			this.Room.HeaderText = "Raum";
			this.Room.Name = "Room";
			this.Room.ReadOnly = true;
			this.Room.Visible = false;
			this.Room.Width = 120;
			// 
			// vorlaufDataGridViewTextBoxColumn
			// 
			this.vorlaufDataGridViewTextBoxColumn.DataPropertyName = "Vorlauf";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.vorlaufDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.vorlaufDataGridViewTextBoxColumn.FillWeight = 70F;
			this.vorlaufDataGridViewTextBoxColumn.HeaderText = "Länge\nVorlauf\n(m)";
			this.vorlaufDataGridViewTextBoxColumn.Name = "vorlaufDataGridViewTextBoxColumn";
			this.vorlaufDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PIPE_LENGTH;
			this.vorlaufDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.vorlaufDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.vorlaufDataGridViewTextBoxColumn.Width = 70;
			// 
			// ruecklaufDataGridViewTextBoxColumn
			// 
			this.ruecklaufDataGridViewTextBoxColumn.DataPropertyName = "Ruecklauf";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F0";
			this.ruecklaufDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.ruecklaufDataGridViewTextBoxColumn.FillWeight = 70F;
			this.ruecklaufDataGridViewTextBoxColumn.HeaderText = "Länge\nRücklauf\n(m)";
			this.ruecklaufDataGridViewTextBoxColumn.Name = "ruecklaufDataGridViewTextBoxColumn";
			this.ruecklaufDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PIPE_LENGTH;
			this.ruecklaufDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ruecklaufDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.ruecklaufDataGridViewTextBoxColumn.Width = 70;
			// 
			// roomDataGridViewComboBoxColumn
			// 
			this.roomDataGridViewComboBoxColumn.DataPropertyName = "Room";
			this.roomDataGridViewComboBoxColumn.FillWeight = 70F;
			this.roomDataGridViewComboBoxColumn.HeaderText = "durch\nRaum\nNr.";
			this.roomDataGridViewComboBoxColumn.Name = "roomDataGridViewComboBoxColumn";
			this.roomDataGridViewComboBoxColumn.ReadOnly = true;
			this.roomDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.roomDataGridViewComboBoxColumn.Width = 120;
			// 
			// productDataGridViewComboBoxColumn
			// 
			this.productDataGridViewComboBoxColumn.DataPropertyName = "PlannedProduct";
			this.productDataGridViewComboBoxColumn.FillWeight = 70F;
			this.productDataGridViewComboBoxColumn.HeaderText = "Teilsystem";
			this.productDataGridViewComboBoxColumn.Name = "productDataGridViewComboBoxColumn";
			this.productDataGridViewComboBoxColumn.ReadOnly = true;
			this.productDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.productDataGridViewComboBoxColumn.Width = 120;
			// 
			// PlannedCircuits
			// 
			this.PlannedCircuits.DataPropertyName = "PlannedCircuits";
			this.PlannedCircuits.HeaderText = "Anz.";
			this.PlannedCircuits.Name = "PlannedCircuits";
			this.PlannedCircuits.ReadOnly = true;
			this.PlannedCircuits.Visible = false;
			this.PlannedCircuits.Width = 40;
			// 
			// onlyFirstDataGridViewCheckBoxColumn
			// 
			this.onlyFirstDataGridViewCheckBoxColumn.DataPropertyName = "OnlyFirst";
			this.onlyFirstDataGridViewCheckBoxColumn.FillWeight = 55F;
			this.onlyFirstDataGridViewCheckBoxColumn.HeaderText = "nur\nerster\nHK";
			this.onlyFirstDataGridViewCheckBoxColumn.Name = "onlyFirstDataGridViewCheckBoxColumn";
			this.onlyFirstDataGridViewCheckBoxColumn.Width = 55;
			// 
			// printDataGridViewCheckBoxColumn
			// 
			this.printDataGridViewCheckBoxColumn.DataPropertyName = "Print";
			this.printDataGridViewCheckBoxColumn.FillWeight = 55F;
			this.printDataGridViewCheckBoxColumn.HeaderText = "Verlege-\ndaten\ndrucken";
			this.printDataGridViewCheckBoxColumn.Name = "printDataGridViewCheckBoxColumn";
			this.printDataGridViewCheckBoxColumn.Width = 55;
			// 
			// PipeType
			// 
			this.PipeType.DataPropertyName = "PipeType";
			this.PipeType.FillWeight = 125F;
			this.PipeType.HeaderText = "Rohr-\nsystem";
			this.PipeType.Name = "PipeType";
			this.PipeType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.PipeType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.PipeType.Width = 125;
			// 
			// Verlegeart
			// 
			this.Verlegeart.DataPropertyName = "Verlegeart";
			this.Verlegeart.FillWeight = 85F;
			this.Verlegeart.HeaderText = "Verlege-\nart";
			this.Verlegeart.Name = "Verlegeart";
			this.Verlegeart.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Verlegeart.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.Verlegeart.Width = 85;
			// 
			// Insulation
			// 
			this.Insulation.DataPropertyName = "Insulation";
			this.Insulation.FillWeight = 60F;
			this.Insulation.HeaderText = "Dämmung";
			this.Insulation.Name = "Insulation";
			this.Insulation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Insulation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.Insulation.Width = 60;
			// 
			// Area
			// 
			this.Area.DataPropertyName = "Area";
			this.Area.HeaderText = "Fläche";
			this.Area.Name = "Area";
			this.Area.ReadOnly = true;
			this.Area.Visible = false;
			this.Area.Width = 50;
			// 
			// HeatLoad
			// 
			this.HeatLoad.DataPropertyName = "HeatLoad";
			this.HeatLoad.HeaderText = "Heiz-\nleistung";
			this.HeatLoad.Name = "HeatLoad";
			this.HeatLoad.ReadOnly = true;
			this.HeatLoad.Visible = false;
			this.HeatLoad.Width = 50;
			// 
			// CoolLoad
			// 
			this.CoolLoad.DataPropertyName = "CoolLoad";
			this.CoolLoad.HeaderText = "Kühl-\nleistung";
			this.CoolLoad.Name = "CoolLoad";
			this.CoolLoad.ReadOnly = true;
			this.CoolLoad.Visible = false;
			this.CoolLoad.Width = 50;
			// 
			// ConnectionPipePanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dgvConnectionPipes);
			this.Name = "ConnectionPipePanel";
			this.Size = new System.Drawing.Size(903, 375);
			((System.ComponentModel.ISupportInitialize)(this.dgvConnectionPipes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.connectionPipeBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.BindingSource connectionPipeBindingSource;
		private System.Windows.Forms.DataGridView dgvConnectionPipes;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn Room;
		private NumericColumn vorlaufDataGridViewTextBoxColumn;
		private NumericColumn ruecklaufDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn roomDataGridViewComboBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn productDataGridViewComboBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn PlannedCircuits;
		private System.Windows.Forms.DataGridViewCheckBoxColumn onlyFirstDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn printDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn PipeType;
		private System.Windows.Forms.DataGridViewComboBoxColumn Verlegeart;
		private System.Windows.Forms.DataGridViewComboBoxColumn Insulation;
		private System.Windows.Forms.DataGridViewTextBoxColumn Area;
		private System.Windows.Forms.DataGridViewTextBoxColumn HeatLoad;
		private System.Windows.Forms.DataGridViewTextBoxColumn CoolLoad;

	}
}
