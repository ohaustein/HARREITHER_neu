namespace Europlan.Common {
	partial class RoomTypeGrid {
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
			this.gridRoomTypes = new System.Windows.Forms.DataGridView();
			this.roomTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.userDefinedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridRoomTypes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.roomTypeBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridRoomTypes
			// 
			this.gridRoomTypes.AutoGenerateColumns = false;
			this.gridRoomTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridRoomTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.heatLoadPerSquareMeterDataGridViewTextBoxColumn,
            this.coolLoadPerSquareMeterDataGridViewTextBoxColumn,
            this.userDefinedDataGridViewCheckBoxColumn});
			this.gridRoomTypes.DataSource = this.roomTypeBindingSource;
			this.gridRoomTypes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridRoomTypes.Location = new System.Drawing.Point(0, 0);
			this.gridRoomTypes.Name = "gridRoomTypes";
			this.gridRoomTypes.Size = new System.Drawing.Size(562, 256);
			this.gridRoomTypes.TabIndex = 0;
			this.gridRoomTypes.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridRoomTypes_UserDeletingRow);
			this.gridRoomTypes.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.gridRoomTypes_RowsAdded);
			// 
			// roomTypeBindingSource
			// 
			this.roomTypeBindingSource.DataSource = typeof(Europlan.Common.RoomType);
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "Id";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.Visible = false;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.Width = 200;
			// 
			// heatLoadPerSquareMeterDataGridViewTextBoxColumn
			// 
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.DataPropertyName = "HeatLoadPerSquareMeter";
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle1.Format = "F0";
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.HeaderText = "Heizlast (W/m²)";
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.Name = "heatLoadPerSquareMeterDataGridViewTextBoxColumn";
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// coolLoadPerSquareMeterDataGridViewTextBoxColumn
			// 
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.DataPropertyName = "CoolLoadPerSquareMeter";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.HeaderText = "Kühllast (W/m²)";
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.Name = "coolLoadPerSquareMeterDataGridViewTextBoxColumn";
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// userDefinedDataGridViewCheckBoxColumn
			// 
			this.userDefinedDataGridViewCheckBoxColumn.DataPropertyName = "UserDefined";
			this.userDefinedDataGridViewCheckBoxColumn.HeaderText = "UserDefined";
			this.userDefinedDataGridViewCheckBoxColumn.Name = "userDefinedDataGridViewCheckBoxColumn";
			this.userDefinedDataGridViewCheckBoxColumn.Visible = false;
			// 
			// RoomTypeGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridRoomTypes);
			this.Name = "RoomTypeGrid";
			this.Size = new System.Drawing.Size(562, 256);
			((System.ComponentModel.ISupportInitialize)(this.gridRoomTypes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.roomTypeBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridRoomTypes;
		private System.Windows.Forms.BindingSource roomTypeBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn heatLoadPerSquareMeterDataGridViewTextBoxColumn;
		private NumericColumn coolLoadPerSquareMeterDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn userDefinedDataGridViewCheckBoxColumn;
	}
}
