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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvConnectionPipes = new System.Windows.Forms.DataGridView();
			this.Room = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.vorlaufDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.ruecklaufDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.roomDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.productDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PlannedCircuits = new Europlan.Common.NumericColumn();
			this.onlyFirstDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.printDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.PipeType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PipeTypeText = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Verlegeart = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Insulation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Area = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.HeatLoad = new Europlan.Common.NumericColumn();
			this.CoolLoad = new Europlan.Common.NumericColumn();
			this.connectionPipeBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.PipeTypeText,
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
			this.dgvConnectionPipes.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvConnectionPipes_UserDeletingRow);
			this.dgvConnectionPipes.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConnectionPipes_CellLeave);
			this.dgvConnectionPipes.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvConnectionPipes_PreviewKeyDown);
			this.dgvConnectionPipes.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvConnectionPipes_UserDeletedRow);
			this.dgvConnectionPipes.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dgvConnectionPipes_CellParsing);
			this.dgvConnectionPipes.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvConnectionPipes_DefaultValuesNeeded);
			this.dgvConnectionPipes.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvConnectionPipes_DataError);
			this.dgvConnectionPipes.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConnectionPipes_CellEnter);
			// 
			// Room
			// 
			this.Room.DataPropertyName = "DestinationRoom";
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			this.Room.DefaultCellStyle = dataGridViewCellStyle2;
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
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F1";
			this.vorlaufDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
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
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F1";
			this.ruecklaufDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
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
			this.productDataGridViewComboBoxColumn.DataPropertyName = "ConnectionThrough";
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
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle5.Format = "F0";
			this.PlannedCircuits.DefaultCellStyle = dataGridViewCellStyle5;
			this.PlannedCircuits.HeaderText = "Anz.";
			this.PlannedCircuits.Name = "PlannedCircuits";
			this.PlannedCircuits.NumEditType = Europlan.Common.NumericBox.NumericEditType.DENOMINATION;
			this.PlannedCircuits.ReadOnly = true;
			this.PlannedCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.PlannedCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.PlannedCircuits.Visible = false;
			this.PlannedCircuits.Width = 40;
			// 
			// onlyFirstDataGridViewCheckBoxColumn
			// 
			this.onlyFirstDataGridViewCheckBoxColumn.DataPropertyName = "OnlyFirst";
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLightLight;
			dataGridViewCellStyle6.NullValue = false;
			this.onlyFirstDataGridViewCheckBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
			this.onlyFirstDataGridViewCheckBoxColumn.FillWeight = 55F;
			this.onlyFirstDataGridViewCheckBoxColumn.HeaderText = "nur\nerster\nHK";
			this.onlyFirstDataGridViewCheckBoxColumn.Name = "onlyFirstDataGridViewCheckBoxColumn";
			this.onlyFirstDataGridViewCheckBoxColumn.Width = 55;
			// 
			// printDataGridViewCheckBoxColumn
			// 
			this.printDataGridViewCheckBoxColumn.DataPropertyName = "Print";
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.ControlLightLight;
			dataGridViewCellStyle7.NullValue = false;
			this.printDataGridViewCheckBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
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
			this.PipeType.ReadOnly = true;
			this.PipeType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.PipeType.Width = 125;
			// 
			// PipeTypeText
			// 
			this.PipeTypeText.DataPropertyName = "PipeType";
			dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
			this.PipeTypeText.DefaultCellStyle = dataGridViewCellStyle8;
			this.PipeTypeText.HeaderText = "Rohrsystem";
			this.PipeTypeText.Name = "PipeTypeText";
			this.PipeTypeText.ReadOnly = true;
			this.PipeTypeText.Visible = false;
			// 
			// Verlegeart
			// 
			this.Verlegeart.DataPropertyName = "Verlegeart";
			this.Verlegeart.FillWeight = 85F;
			this.Verlegeart.HeaderText = "Verlege-\nart";
			this.Verlegeart.Name = "Verlegeart";
			this.Verlegeart.ReadOnly = true;
			this.Verlegeart.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Verlegeart.Width = 85;
			// 
			// Insulation
			// 
			this.Insulation.DataPropertyName = "Insulation";
			this.Insulation.FillWeight = 60F;
			this.Insulation.HeaderText = "Dämmung";
			this.Insulation.Name = "Insulation";
			this.Insulation.ReadOnly = true;
			this.Insulation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Insulation.Width = 60;
			// 
			// Area
			// 
			this.Area.DataPropertyName = "AreaTotal";
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
			this.Area.DefaultCellStyle = dataGridViewCellStyle9;
			this.Area.HeaderText = "Fläche";
			this.Area.Name = "Area";
			this.Area.ReadOnly = true;
			this.Area.Visible = false;
			this.Area.Width = 50;
			// 
			// HeatLoad
			// 
			this.HeatLoad.DataPropertyName = "HeatLoadTotal";
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle10.Format = "F0";
			this.HeatLoad.DefaultCellStyle = dataGridViewCellStyle10;
			this.HeatLoad.HeaderText = "Heiz-\nleistung";
			this.HeatLoad.Name = "HeatLoad";
			this.HeatLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.HeatLoad.ReadOnly = true;
			this.HeatLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.HeatLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.HeatLoad.Visible = false;
			this.HeatLoad.Width = 50;
			// 
			// CoolLoad
			// 
			this.CoolLoad.DataPropertyName = "CoolLoadTotal";
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle11.Format = "F0";
			this.CoolLoad.DefaultCellStyle = dataGridViewCellStyle11;
			this.CoolLoad.HeaderText = "Kühl-\nleistung";
			this.CoolLoad.Name = "CoolLoad";
			this.CoolLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.CoolLoad.ReadOnly = true;
			this.CoolLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.CoolLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.CoolLoad.Visible = false;
			this.CoolLoad.Width = 50;
			// 
			// connectionPipeBindingSource
			// 
			this.connectionPipeBindingSource.DataSource = typeof(Europlan.Common.ConnectionPipe);
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
		private System.Windows.Forms.DataGridViewTextBoxColumn Room;
		private NumericColumn vorlaufDataGridViewTextBoxColumn;
		private NumericColumn ruecklaufDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn roomDataGridViewComboBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn productDataGridViewComboBoxColumn;
		private NumericColumn PlannedCircuits;
		private System.Windows.Forms.DataGridViewCheckBoxColumn onlyFirstDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn printDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn PipeType;
		private System.Windows.Forms.DataGridViewTextBoxColumn PipeTypeText;
		private System.Windows.Forms.DataGridViewTextBoxColumn Verlegeart;
		private System.Windows.Forms.DataGridViewTextBoxColumn Insulation;
		private System.Windows.Forms.DataGridViewTextBoxColumn Area;
		private NumericColumn HeatLoad;
		private NumericColumn CoolLoad;

	}
}
