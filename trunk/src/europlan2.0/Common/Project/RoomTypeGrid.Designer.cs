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
			this.gridRoomTypes = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.userDefinedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.roomTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.HeaderText = "Heizlast (W/m²)";
			this.heatLoadPerSquareMeterDataGridViewTextBoxColumn.Name = "heatLoadPerSquareMeterDataGridViewTextBoxColumn";
			// 
			// coolLoadPerSquareMeterDataGridViewTextBoxColumn
			// 
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.DataPropertyName = "CoolLoadPerSquareMeter";
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.HeaderText = "Kühllast (W/m²)";
			this.coolLoadPerSquareMeterDataGridViewTextBoxColumn.Name = "coolLoadPerSquareMeterDataGridViewTextBoxColumn";
			// 
			// userDefinedDataGridViewCheckBoxColumn
			// 
			this.userDefinedDataGridViewCheckBoxColumn.DataPropertyName = "UserDefined";
			this.userDefinedDataGridViewCheckBoxColumn.HeaderText = "UserDefined";
			this.userDefinedDataGridViewCheckBoxColumn.Name = "userDefinedDataGridViewCheckBoxColumn";
			this.userDefinedDataGridViewCheckBoxColumn.Visible = false;
			// 
			// roomTypeBindingSource
			// 
			this.roomTypeBindingSource.DataSource = typeof(Europlan.Common.RoomType);
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
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn heatLoadPerSquareMeterDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn coolLoadPerSquareMeterDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn userDefinedDataGridViewCheckBoxColumn;
		private System.Windows.Forms.BindingSource roomTypeBindingSource;
	}
}
