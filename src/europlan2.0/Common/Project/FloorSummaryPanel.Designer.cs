namespace Europlan.Common {
	partial class FloorSummaryPanel {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			this.gridRooms = new System.Windows.Forms.DataGridView();
			this.floorRoomsSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnAddDistributor = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lblFloorName = new System.Windows.Forms.Label();
			this.btnRemoveDistributor = new System.Windows.Forms.Button();
			this.numericColumn1 = new Europlan.Common.NumericColumn();
			this.numericColumn2 = new Europlan.Common.NumericColumn();
			this.numericColumn3 = new Europlan.Common.NumericColumn();
			this.numericColumn4 = new Europlan.Common.NumericColumn();
			this.btnWhatIsNext = new System.Windows.Forms.Button();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Area = new Europlan.Common.NumericColumn();
			this.RoomTemperature = new Europlan.Common.NumericColumn();
			this.HeatLoad = new Europlan.Common.NumericColumn();
			this.FloorHeatingLoss = new Europlan.Common.NumericColumn();
			this.AdditionalHeatLoad = new Europlan.Common.NumericColumn();
			this.RoomCoolTemperature = new Europlan.Common.NumericColumn();
			this.RoomRelativeHumidity = new Europlan.Common.NumericColumn();
			this.CoolLoad = new Europlan.Common.NumericColumn();
			this.associatedPanelTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.associatedIconDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.colView = new System.Windows.Forms.DataGridViewButtonColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.floorRoomsSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridRooms
			// 
			this.gridRooms.AllowUserToResizeRows = false;
			this.gridRooms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridRooms.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridRooms.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridRooms.ColumnHeadersHeight = 55;
			this.gridRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.gridRooms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.Area,
            this.RoomTemperature,
            this.HeatLoad,
            this.FloorHeatingLoss,
            this.AdditionalHeatLoad,
            this.RoomCoolTemperature,
            this.RoomRelativeHumidity,
            this.CoolLoad,
            this.associatedPanelTypeDataGridViewTextBoxColumn,
            this.associatedIconDataGridViewImageColumn,
            this.colView});
			this.gridRooms.DataSource = this.floorRoomsSource;
			this.gridRooms.Location = new System.Drawing.Point(3, 56);
			this.gridRooms.MultiSelect = false;
			this.gridRooms.Name = "gridRooms";
			this.gridRooms.Size = new System.Drawing.Size(690, 291);
			this.gridRooms.TabIndex = 16;
			this.gridRooms.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRooms_CellValueChanged);
			this.gridRooms.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridRooms_UserDeletingRow);
			this.gridRooms.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gridRooms_CellBeginEdit);
			this.gridRooms.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gridRooms_RowPrePaint);
			this.gridRooms.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridRooms_UserDeletedRow);
			this.gridRooms.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRooms_CellClick);
			// 
			// floorRoomsSource
			// 
			this.floorRoomsSource.DataSource = typeof(Europlan.Common.Room);
			// 
			// btnAddDistributor
			// 
			this.btnAddDistributor.Location = new System.Drawing.Point(3, 27);
			this.btnAddDistributor.Name = "btnAddDistributor";
			this.btnAddDistributor.Size = new System.Drawing.Size(118, 23);
			this.btnAddDistributor.TabIndex = 17;
			this.btnAddDistributor.Text = "Verteiler anlegen";
			this.btnAddDistributor.UseVisualStyleBackColor = true;
			this.btnAddDistributor.Click += new System.EventHandler(this.btnAddDistributor_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 24);
			this.label1.TabIndex = 18;
			this.label1.Text = "Geschoßdaten:";
			// 
			// lblFloorName
			// 
			this.lblFloorName.AutoSize = true;
			this.lblFloorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFloorName.Location = new System.Drawing.Point(161, 0);
			this.lblFloorName.Name = "lblFloorName";
			this.lblFloorName.Size = new System.Drawing.Size(0, 24);
			this.lblFloorName.TabIndex = 19;
			// 
			// btnRemoveDistributor
			// 
			this.btnRemoveDistributor.Location = new System.Drawing.Point(127, 27);
			this.btnRemoveDistributor.Name = "btnRemoveDistributor";
			this.btnRemoveDistributor.Size = new System.Drawing.Size(118, 23);
			this.btnRemoveDistributor.TabIndex = 20;
			this.btnRemoveDistributor.Text = "Verteiler löschen";
			this.btnRemoveDistributor.UseVisualStyleBackColor = true;
			this.btnRemoveDistributor.Click += new System.EventHandler(this.btnRemoveDistributor_Click);
			// 
			// numericColumn1
			// 
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle10.Format = "F0";
			this.numericColumn1.DefaultCellStyle = dataGridViewCellStyle10;
			this.numericColumn1.Name = "numericColumn1";
			// 
			// numericColumn2
			// 
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle11.Format = "F0";
			this.numericColumn2.DefaultCellStyle = dataGridViewCellStyle11;
			this.numericColumn2.Name = "numericColumn2";
			// 
			// numericColumn3
			// 
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle12.Format = "F0";
			this.numericColumn3.DefaultCellStyle = dataGridViewCellStyle12;
			this.numericColumn3.Name = "numericColumn3";
			// 
			// numericColumn4
			// 
			dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle13.Format = "F0";
			this.numericColumn4.DefaultCellStyle = dataGridViewCellStyle13;
			this.numericColumn4.Name = "numericColumn4";
			// 
			// btnWhatIsNext
			// 
			this.btnWhatIsNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWhatIsNext.Location = new System.Drawing.Point(584, 353);
			this.btnWhatIsNext.Name = "btnWhatIsNext";
			this.btnWhatIsNext.Size = new System.Drawing.Size(109, 23);
			this.btnWhatIsNext.TabIndex = 21;
			this.btnWhatIsNext.Text = "Wie geht\'s weiter?";
			this.btnWhatIsNext.UseVisualStyleBackColor = true;
			this.btnWhatIsNext.Click += new System.EventHandler(this.btnWhatIsNext_Click);
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "Nr.\n";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.idDataGridViewTextBoxColumn.ToolTipText = "eindeutige Raumnummer";
			this.idDataGridViewTextBoxColumn.Width = 50;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung\n";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.nameDataGridViewTextBoxColumn.ToolTipText = "Bezeichnung des Raumes";
			this.nameDataGridViewTextBoxColumn.Width = 200;
			// 
			// Area
			// 
			this.Area.DataPropertyName = "Area";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.Area.DefaultCellStyle = dataGridViewCellStyle2;
			this.Area.HeaderText = "A\n(m²)";
			this.Area.Name = "Area";
			this.Area.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.Area.ToolTipText = "Raumfläche";
			this.Area.Width = 50;
			// 
			// RoomTemperature
			// 
			this.RoomTemperature.DataPropertyName = "RoomHeatTemperature";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F0";
			this.RoomTemperature.DefaultCellStyle = dataGridViewCellStyle3;
			this.RoomTemperature.HeaderText = "Ti\n(°C)";
			this.RoomTemperature.Name = "RoomTemperature";
			this.RoomTemperature.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.RoomTemperature.ToolTipText = "Norminnentemperatur laut Wärmebedarfsberechnung";
			this.RoomTemperature.Width = 40;
			// 
			// HeatLoad
			// 
			this.HeatLoad.DataPropertyName = "HeatLoad";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.HeatLoad.DefaultCellStyle = dataGridViewCellStyle4;
			this.HeatLoad.HeaderText = "QN\n(W)";
			this.HeatLoad.Name = "HeatLoad";
			this.HeatLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.HeatLoad.ToolTipText = "Normwärmebedarf laut Wärmebedarfsrechnung";
			this.HeatLoad.Width = 50;
			// 
			// FloorHeatingLoss
			// 
			this.FloorHeatingLoss.DataPropertyName = "FloorHeatingLoss";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "F0";
			this.FloorHeatingLoss.DefaultCellStyle = dataGridViewCellStyle5;
			this.FloorHeatingLoss.HeaderText = "QFB\n(W)";
			this.FloorHeatingLoss.Name = "FloorHeatingLoss";
			this.FloorHeatingLoss.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.FloorHeatingLoss.ToolTipText = "Im Wärmebedarf enthaltene Fußbodentransmissionen";
			this.FloorHeatingLoss.Width = 50;
			// 
			// AdditionalHeatLoad
			// 
			this.AdditionalHeatLoad.DataPropertyName = "AdditionalHeatLoad";
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "F0";
			this.AdditionalHeatLoad.DefaultCellStyle = dataGridViewCellStyle6;
			this.AdditionalHeatLoad.HeaderText = "QFr\n(W)";
			this.AdditionalHeatLoad.Name = "AdditionalHeatLoad";
			this.AdditionalHeatLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.AdditionalHeatLoad.ToolTipText = "Zusätzliche Fremdwärmeleistung";
			this.AdditionalHeatLoad.Width = 50;
			// 
			// RoomCoolTemperature
			// 
			this.RoomCoolTemperature.DataPropertyName = "RoomCoolTemperature";
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle7.Format = "F0";
			this.RoomCoolTemperature.DefaultCellStyle = dataGridViewCellStyle7;
			this.RoomCoolTemperature.HeaderText = "Ti\n(°C)";
			this.RoomCoolTemperature.Name = "RoomCoolTemperature";
			this.RoomCoolTemperature.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.RoomCoolTemperature.ToolTipText = "Gewünschte Rauminnentemperatur bei Kühlung";
			this.RoomCoolTemperature.Visible = false;
			this.RoomCoolTemperature.Width = 40;
			// 
			// RoomRelativeHumidity
			// 
			this.RoomRelativeHumidity.DataPropertyName = "RoomRelativeHumidity";
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle8.Format = "F0";
			this.RoomRelativeHumidity.DefaultCellStyle = dataGridViewCellStyle8;
			this.RoomRelativeHumidity.HeaderText = "RF\n(%)";
			this.RoomRelativeHumidity.Name = "RoomRelativeHumidity";
			this.RoomRelativeHumidity.NumEditType = Europlan.Common.NumericBox.NumericEditType.PERCENTAGE;
			this.RoomRelativeHumidity.ToolTipText = "Relative Luftfeuchtigkeit für Kühlung";
			this.RoomRelativeHumidity.Visible = false;
			this.RoomRelativeHumidity.Width = 40;
			// 
			// CoolLoad
			// 
			this.CoolLoad.DataPropertyName = "CoolLoad";
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle9.Format = "F0";
			this.CoolLoad.DefaultCellStyle = dataGridViewCellStyle9;
			this.CoolLoad.HeaderText = "QKühl\n(W)";
			this.CoolLoad.Name = "CoolLoad";
			this.CoolLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.CoolLoad.ToolTipText = "Erforderliche Külleistung laut Kühllastberechnung";
			this.CoolLoad.Visible = false;
			this.CoolLoad.Width = 50;
			// 
			// associatedPanelTypeDataGridViewTextBoxColumn
			// 
			this.associatedPanelTypeDataGridViewTextBoxColumn.DataPropertyName = "AssociatedPanelType";
			this.associatedPanelTypeDataGridViewTextBoxColumn.HeaderText = "AssociatedPanelType";
			this.associatedPanelTypeDataGridViewTextBoxColumn.Name = "associatedPanelTypeDataGridViewTextBoxColumn";
			this.associatedPanelTypeDataGridViewTextBoxColumn.ReadOnly = true;
			this.associatedPanelTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.associatedPanelTypeDataGridViewTextBoxColumn.Visible = false;
			// 
			// associatedIconDataGridViewImageColumn
			// 
			this.associatedIconDataGridViewImageColumn.DataPropertyName = "AssociatedIcon";
			this.associatedIconDataGridViewImageColumn.HeaderText = "AssociatedIcon";
			this.associatedIconDataGridViewImageColumn.Name = "associatedIconDataGridViewImageColumn";
			this.associatedIconDataGridViewImageColumn.ReadOnly = true;
			this.associatedIconDataGridViewImageColumn.Visible = false;
			// 
			// colView
			// 
			this.colView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colView.HeaderText = "Bearbeiten\n";
			this.colView.Name = "colView";
			this.colView.ReadOnly = true;
			this.colView.Text = "...";
			this.colView.UseColumnTextForButtonValue = true;
			this.colView.Width = 64;
			// 
			// FloorSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnWhatIsNext);
			this.Controls.Add(this.btnRemoveDistributor);
			this.Controls.Add(this.lblFloorName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnAddDistributor);
			this.Controls.Add(this.gridRooms);
			this.Name = "FloorSummaryPanel";
			this.Size = new System.Drawing.Size(696, 379);
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.floorRoomsSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gridRooms;
		private System.Windows.Forms.BindingSource floorRoomsSource;
		private Europlan.Common.NumericColumn numericColumn1;
		private Europlan.Common.NumericColumn numericColumn2;
		private Europlan.Common.NumericColumn numericColumn3;
		private Europlan.Common.NumericColumn numericColumn4;
		private System.Windows.Forms.Button btnAddDistributor;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblFloorName;
		private System.Windows.Forms.Button btnRemoveDistributor;
		private System.Windows.Forms.Button btnWhatIsNext;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn Area;
		private NumericColumn RoomTemperature;
		private NumericColumn HeatLoad;
		private NumericColumn FloorHeatingLoss;
		private NumericColumn AdditionalHeatLoad;
		private NumericColumn RoomCoolTemperature;
		private NumericColumn RoomRelativeHumidity;
		private NumericColumn CoolLoad;
		private System.Windows.Forms.DataGridViewTextBoxColumn associatedPanelTypeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewImageColumn associatedIconDataGridViewImageColumn;
		private System.Windows.Forms.DataGridViewButtonColumn colView;
	}
}
