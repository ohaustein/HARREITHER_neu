namespace Europlan.Application {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.gridRooms = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Area = new Europlan.Application.NumericColumn();
			this.RoomTemperature = new Europlan.Application.NumericColumn();
			this.HeatPower = new Europlan.Application.NumericColumn();
			this.FloorHeatingLoss = new Europlan.Application.NumericColumn();
			this.AdditionalHeatPower = new Europlan.Application.NumericColumn();
			this.colView = new System.Windows.Forms.DataGridViewButtonColumn();
			this.associatedPanelTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.associatedIconDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.floorRoomsSource = new System.Windows.Forms.BindingSource(this.components);
			this.numericColumn1 = new Europlan.Application.NumericColumn();
			this.numericColumn2 = new Europlan.Application.NumericColumn();
			this.numericColumn3 = new Europlan.Application.NumericColumn();
			this.numericColumn4 = new Europlan.Application.NumericColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.floorRoomsSource)).BeginInit();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(117, 2);
			this.txtName.Name = "txtName";
			this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtName.Size = new System.Drawing.Size(464, 20);
			this.txtName.TabIndex = 15;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(3, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(108, 23);
			this.lblName.TabIndex = 14;
			this.lblName.Text = "Name:";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.gridRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridRooms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.Area,
            this.RoomTemperature,
            this.HeatPower,
            this.FloorHeatingLoss,
            this.AdditionalHeatPower,
            this.colView,
            this.associatedPanelTypeDataGridViewTextBoxColumn,
            this.associatedIconDataGridViewImageColumn});
			this.gridRooms.DataMember = "Rooms";
			this.gridRooms.DataSource = this.floorRoomsSource;
			this.gridRooms.Location = new System.Drawing.Point(3, 28);
			this.gridRooms.MultiSelect = false;
			this.gridRooms.Name = "gridRooms";
			this.gridRooms.Size = new System.Drawing.Size(578, 233);
			this.gridRooms.TabIndex = 16;
			this.gridRooms.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRooms_CellValueChanged);
			this.gridRooms.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridRooms_UserAddedRow);
			this.gridRooms.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gridRooms_RowPrePaint);
			this.gridRooms.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridRooms_UserDeletedRow);
			this.gridRooms.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRooms_CellClick);
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "Nr.";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.idDataGridViewTextBoxColumn.ToolTipText = "eindeutige Raumnummer";
			this.idDataGridViewTextBoxColumn.Width = 50;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.nameDataGridViewTextBoxColumn.ToolTipText = "Bezeichnung des Raumes";
			this.nameDataGridViewTextBoxColumn.Width = 200;
			// 
			// Area
			// 
			this.Area.DataPropertyName = "Area";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F1";
			this.Area.DefaultCellStyle = dataGridViewCellStyle2;
			this.Area.HeaderText = "A (m≤)";
			this.Area.Name = "Area";
			this.Area.NumEditType = Europlan.Application.NumericBox.NumericEditType.ROOM_AREA;
			this.Area.ToolTipText = "Raumfl‰che";
			this.Area.Width = 50;
			// 
			// RoomTemperature
			// 
			this.RoomTemperature.DataPropertyName = "RoomTemperature";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F0";
			this.RoomTemperature.DefaultCellStyle = dataGridViewCellStyle3;
			this.RoomTemperature.HeaderText = "Ti (∞C)";
			this.RoomTemperature.Name = "RoomTemperature";
			this.RoomTemperature.NumEditType = Europlan.Application.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.RoomTemperature.ToolTipText = "Norminnentemperatur laut W‰rmebedarfsberechnung";
			this.RoomTemperature.Width = 40;
			// 
			// HeatPower
			// 
			this.HeatPower.DataPropertyName = "HeatPower";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.HeatPower.DefaultCellStyle = dataGridViewCellStyle4;
			this.HeatPower.HeaderText = "QN (W)";
			this.HeatPower.Name = "HeatPower";
			this.HeatPower.NumEditType = Europlan.Application.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.HeatPower.ToolTipText = "Normw‰rmebedarf laut W‰rmebedarfsrechnung";
			this.HeatPower.Width = 50;
			// 
			// FloorHeatingLoss
			// 
			this.FloorHeatingLoss.DataPropertyName = "FloorHeatingLoss";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "F0";
			this.FloorHeatingLoss.DefaultCellStyle = dataGridViewCellStyle5;
			this.FloorHeatingLoss.HeaderText = "QFB (W)";
			this.FloorHeatingLoss.Name = "FloorHeatingLoss";
			this.FloorHeatingLoss.NumEditType = Europlan.Application.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.FloorHeatingLoss.ToolTipText = "im W‰rmebedarf enthaltene Fuﬂbodentransmissionen";
			this.FloorHeatingLoss.Width = 50;
			// 
			// AdditionalHeatPower
			// 
			this.AdditionalHeatPower.DataPropertyName = "AdditionalHeatPower";
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "F0";
			this.AdditionalHeatPower.DefaultCellStyle = dataGridViewCellStyle6;
			this.AdditionalHeatPower.HeaderText = "QFr (W)";
			this.AdditionalHeatPower.Name = "AdditionalHeatPower";
			this.AdditionalHeatPower.NumEditType = Europlan.Application.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.AdditionalHeatPower.ToolTipText = "zus‰tzliche Fremdw‰rmeleistung";
			this.AdditionalHeatPower.Width = 50;
			// 
			// colView
			// 
			this.colView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colView.HeaderText = "Bearbeiten";
			this.colView.Name = "colView";
			this.colView.ReadOnly = true;
			this.colView.Text = "...";
			this.colView.UseColumnTextForButtonValue = true;
			this.colView.Width = 64;
			// 
			// associatedPanelTypeDataGridViewTextBoxColumn
			// 
			this.associatedPanelTypeDataGridViewTextBoxColumn.DataPropertyName = "AssociatedPanelType";
			this.associatedPanelTypeDataGridViewTextBoxColumn.HeaderText = "AssociatedPanelType";
			this.associatedPanelTypeDataGridViewTextBoxColumn.Name = "associatedPanelTypeDataGridViewTextBoxColumn";
			this.associatedPanelTypeDataGridViewTextBoxColumn.ReadOnly = true;
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
			// floorRoomsSource
			// 
			this.floorRoomsSource.DataSource = typeof(Europlan.Application.Floor);
			// 
			// numericColumn1
			// 
			this.numericColumn1.DataPropertyName = "Area";
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle7.Format = "F1";
			this.numericColumn1.DefaultCellStyle = dataGridViewCellStyle7;
			this.numericColumn1.HeaderText = "A (m≤)";
			this.numericColumn1.Name = "numericColumn1";
			this.numericColumn1.NumEditType = Europlan.Application.NumericBox.NumericEditType.ROOM_AREA;
			// 
			// numericColumn2
			// 
			this.numericColumn2.DataPropertyName = "RoomTemperature";
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle8.Format = "F0";
			this.numericColumn2.DefaultCellStyle = dataGridViewCellStyle8;
			this.numericColumn2.HeaderText = "Ti (∞C)";
			this.numericColumn2.Name = "numericColumn2";
			this.numericColumn2.ToolTipText = "Norminnentemperatur laut W‰rmebedarfsberechnung";
			// 
			// numericColumn3
			// 
			this.numericColumn3.DataPropertyName = "HeatPower";
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle9.Format = "F0";
			this.numericColumn3.DefaultCellStyle = dataGridViewCellStyle9;
			this.numericColumn3.HeaderText = "Heizlast";
			this.numericColumn3.Name = "numericColumn3";
			// 
			// numericColumn4
			// 
			this.numericColumn4.DataPropertyName = "NormalizedHeatPower";
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle10.Format = "F0";
			this.numericColumn4.DefaultCellStyle = dataGridViewCellStyle10;
			this.numericColumn4.HeaderText = "Heizlast (normiert)";
			this.numericColumn4.Name = "numericColumn4";
			// 
			// FloorSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridRooms);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.Name = "FloorSummaryPanel";
			this.Size = new System.Drawing.Size(584, 264);
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.floorRoomsSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.DataGridView gridRooms;
		private System.Windows.Forms.BindingSource floorRoomsSource;
		private NumericColumn numericColumn1;
		private NumericColumn numericColumn2;
		private NumericColumn numericColumn3;
		private NumericColumn numericColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn Area;
		private NumericColumn RoomTemperature;
		private NumericColumn HeatPower;
		private NumericColumn FloorHeatingLoss;
		private NumericColumn AdditionalHeatPower;
		private System.Windows.Forms.DataGridViewButtonColumn colView;
		private System.Windows.Forms.DataGridViewTextBoxColumn associatedPanelTypeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewImageColumn associatedIconDataGridViewImageColumn;
	}
}
