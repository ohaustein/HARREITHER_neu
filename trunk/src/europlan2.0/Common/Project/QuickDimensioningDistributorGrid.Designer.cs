namespace Europlan.Common {
	partial class QuickDimensioningDistributorGrid {
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			this.gridRooms = new System.Windows.Forms.DataGridView();
			this.dataSourceRooms = new System.Windows.Forms.BindingSource(this.components);
			this.colRoomId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRoomName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colFloorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEurovalOpenCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEurovalPlannedCircuits = new Europlan.Common.DataGridViewNumericUpDownColumn();
			this.colConcreteActivationOpenCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colConcreteActivationPlannedCircuits = new Europlan.Common.NumericColumn();
			this.colHithermOpenCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colHithermPlannedCircuits = new Europlan.Common.NumericColumn();
			this.colHithermCompactOpenCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colHithermCompactPlannedCircuits = new Europlan.Common.NumericColumn();
			this.colModulKlimaBodenOpenCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colModulKlimaBodenPlannedCircuits = new Europlan.Common.NumericColumn();
			this.colModulKlimaDeckeOpenCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colModulKlimaDeckePlannedCircuits = new Europlan.Common.NumericColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSourceRooms)).BeginInit();
			this.SuspendLayout();
			// 
			// gridRooms
			// 
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
            this.colRoomId,
            this.colRoomName,
            this.colFloorName,
            this.colEurovalOpenCircuits,
            this.colEurovalPlannedCircuits,
            this.colConcreteActivationOpenCircuits,
            this.colConcreteActivationPlannedCircuits,
            this.colHithermOpenCircuits,
            this.colHithermPlannedCircuits,
            this.colHithermCompactOpenCircuits,
            this.colHithermCompactPlannedCircuits,
            this.colModulKlimaBodenOpenCircuits,
            this.colModulKlimaBodenPlannedCircuits,
            this.colModulKlimaDeckeOpenCircuits,
            this.colModulKlimaDeckePlannedCircuits});
			this.gridRooms.DataSource = this.dataSourceRooms;
			this.gridRooms.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridRooms.Location = new System.Drawing.Point(0, 0);
			this.gridRooms.MultiSelect = false;
			this.gridRooms.Name = "gridRooms";
			this.gridRooms.Size = new System.Drawing.Size(645, 390);
			this.gridRooms.TabIndex = 0;
			this.gridRooms.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.gridRooms_RowsAdded);
			// 
			// dataSourceRooms
			// 
			this.dataSourceRooms.DataSource = typeof(Europlan.Common.QuickDimensioningRoomDistributorsWrapper);
			// 
			// colRoomId
			// 
			this.colRoomId.DataPropertyName = "RoomId";
			this.colRoomId.FillWeight = 45F;
			this.colRoomId.HeaderText = "Raum-\nnr.";
			this.colRoomId.Name = "colRoomId";
			this.colRoomId.ReadOnly = true;
			this.colRoomId.Width = 45;
			// 
			// colRoomName
			// 
			this.colRoomName.DataPropertyName = "RoomName";
			this.colRoomName.HeaderText = "Raumname";
			this.colRoomName.Name = "colRoomName";
			this.colRoomName.ReadOnly = true;
			// 
			// colFloorName
			// 
			this.colFloorName.DataPropertyName = "FloorName";
			this.colFloorName.HeaderText = "Geschoﬂ";
			this.colFloorName.Name = "colFloorName";
			this.colFloorName.ReadOnly = true;
			// 
			// colEurovalOpenCircuits
			// 
			this.colEurovalOpenCircuits.DataPropertyName = "EurovalOpenCircuits";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colEurovalOpenCircuits.DefaultCellStyle = dataGridViewCellStyle2;
			this.colEurovalOpenCircuits.FillWeight = 70F;
			this.colEurovalOpenCircuits.HeaderText = "Euroval\noffene\nHeizkreise";
			this.colEurovalOpenCircuits.Name = "colEurovalOpenCircuits";
			this.colEurovalOpenCircuits.ReadOnly = true;
			this.colEurovalOpenCircuits.Width = 70;
			// 
			// colEurovalPlannedCircuits
			// 
			this.colEurovalPlannedCircuits.DataPropertyName = "EurovalPlannedCircuits";
			this.colEurovalPlannedCircuits.FillWeight = 70F;
			this.colEurovalPlannedCircuits.HeaderText = "Euroval\nangeschl.\nHeizkreise";
			this.colEurovalPlannedCircuits.Name = "colEurovalPlannedCircuits";
			this.colEurovalPlannedCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colEurovalPlannedCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colEurovalPlannedCircuits.Width = 70;
			// 
			// colConcreteActivationOpenCircuits
			// 
			this.colConcreteActivationOpenCircuits.DataPropertyName = "ConcreteActivationOpenCircuits";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colConcreteActivationOpenCircuits.DefaultCellStyle = dataGridViewCellStyle3;
			this.colConcreteActivationOpenCircuits.FillWeight = 70F;
			this.colConcreteActivationOpenCircuits.HeaderText = "BKA\noffene\nHeizkreise";
			this.colConcreteActivationOpenCircuits.Name = "colConcreteActivationOpenCircuits";
			this.colConcreteActivationOpenCircuits.ReadOnly = true;
			this.colConcreteActivationOpenCircuits.Width = 70;
			// 
			// colConcreteActivationPlannedCircuits
			// 
			this.colConcreteActivationPlannedCircuits.DataPropertyName = "ConcreteActivationPlannedCircuits";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.colConcreteActivationPlannedCircuits.DefaultCellStyle = dataGridViewCellStyle4;
			this.colConcreteActivationPlannedCircuits.FillWeight = 70F;
			this.colConcreteActivationPlannedCircuits.HeaderText = "BKA\nangeschl.\nHeizkreise";
			this.colConcreteActivationPlannedCircuits.Name = "colConcreteActivationPlannedCircuits";
			this.colConcreteActivationPlannedCircuits.ReadOnly = true;
			this.colConcreteActivationPlannedCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colConcreteActivationPlannedCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colConcreteActivationPlannedCircuits.Width = 70;
			// 
			// colHithermOpenCircuits
			// 
			this.colHithermOpenCircuits.DataPropertyName = "HithermOpenCircuits";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colHithermOpenCircuits.DefaultCellStyle = dataGridViewCellStyle5;
			this.colHithermOpenCircuits.FillWeight = 70F;
			this.colHithermOpenCircuits.HeaderText = "Hitherm\noffene\nHeizkreise";
			this.colHithermOpenCircuits.Name = "colHithermOpenCircuits";
			this.colHithermOpenCircuits.ReadOnly = true;
			this.colHithermOpenCircuits.Width = 70;
			// 
			// colHithermPlannedCircuits
			// 
			this.colHithermPlannedCircuits.DataPropertyName = "HithermPlannedCircuits";
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "F0";
			this.colHithermPlannedCircuits.DefaultCellStyle = dataGridViewCellStyle6;
			this.colHithermPlannedCircuits.FillWeight = 70F;
			this.colHithermPlannedCircuits.HeaderText = "Hitherm\nangeschl.\nHeizkreise";
			this.colHithermPlannedCircuits.Name = "colHithermPlannedCircuits";
			this.colHithermPlannedCircuits.ReadOnly = true;
			this.colHithermPlannedCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colHithermPlannedCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colHithermPlannedCircuits.Width = 70;
			// 
			// colHithermCompactOpenCircuits
			// 
			this.colHithermCompactOpenCircuits.DataPropertyName = "HithermCompactOpenCircuits";
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colHithermCompactOpenCircuits.DefaultCellStyle = dataGridViewCellStyle7;
			this.colHithermCompactOpenCircuits.FillWeight = 70F;
			this.colHithermCompactOpenCircuits.HeaderText = "Hitherm Co\noffene\nHeizkreise";
			this.colHithermCompactOpenCircuits.Name = "colHithermCompactOpenCircuits";
			this.colHithermCompactOpenCircuits.ReadOnly = true;
			this.colHithermCompactOpenCircuits.Width = 70;
			// 
			// colHithermCompactPlannedCircuits
			// 
			this.colHithermCompactPlannedCircuits.DataPropertyName = "HithermCompactPlannedCircuits";
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle8.Format = "F0";
			this.colHithermCompactPlannedCircuits.DefaultCellStyle = dataGridViewCellStyle8;
			this.colHithermCompactPlannedCircuits.FillWeight = 70F;
			this.colHithermCompactPlannedCircuits.HeaderText = "Hitherm Co\nangeschl.\nHeizkreise";
			this.colHithermCompactPlannedCircuits.Name = "colHithermCompactPlannedCircuits";
			this.colHithermCompactPlannedCircuits.ReadOnly = true;
			this.colHithermCompactPlannedCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colHithermCompactPlannedCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colHithermCompactPlannedCircuits.Width = 70;
			// 
			// colModulKlimaBodenOpenCircuits
			// 
			this.colModulKlimaBodenOpenCircuits.DataPropertyName = "ModulKlimaBodenOpenCircuits";
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colModulKlimaBodenOpenCircuits.DefaultCellStyle = dataGridViewCellStyle9;
			this.colModulKlimaBodenOpenCircuits.FillWeight = 70F;
			this.colModulKlimaBodenOpenCircuits.HeaderText = "Klimaboden\noffene\nHeizkreise";
			this.colModulKlimaBodenOpenCircuits.Name = "colModulKlimaBodenOpenCircuits";
			this.colModulKlimaBodenOpenCircuits.ReadOnly = true;
			this.colModulKlimaBodenOpenCircuits.Width = 70;
			// 
			// colModulKlimaBodenPlannedCircuits
			// 
			this.colModulKlimaBodenPlannedCircuits.DataPropertyName = "ModulKlimaBodenPlannedCircuits";
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle10.Format = "F0";
			this.colModulKlimaBodenPlannedCircuits.DefaultCellStyle = dataGridViewCellStyle10;
			this.colModulKlimaBodenPlannedCircuits.FillWeight = 70F;
			this.colModulKlimaBodenPlannedCircuits.HeaderText = "Klimaboden\nangeschl.\nHeizkreise";
			this.colModulKlimaBodenPlannedCircuits.Name = "colModulKlimaBodenPlannedCircuits";
			this.colModulKlimaBodenPlannedCircuits.ReadOnly = true;
			this.colModulKlimaBodenPlannedCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colModulKlimaBodenPlannedCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colModulKlimaBodenPlannedCircuits.Width = 70;
			// 
			// colModulKlimaDeckeOpenCircuits
			// 
			this.colModulKlimaDeckeOpenCircuits.DataPropertyName = "ModulKlimaDeckeOpenCircuits";
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colModulKlimaDeckeOpenCircuits.DefaultCellStyle = dataGridViewCellStyle11;
			this.colModulKlimaDeckeOpenCircuits.FillWeight = 70F;
			this.colModulKlimaDeckeOpenCircuits.HeaderText = "Klimadecke\noffene\nHeizkreise";
			this.colModulKlimaDeckeOpenCircuits.Name = "colModulKlimaDeckeOpenCircuits";
			this.colModulKlimaDeckeOpenCircuits.ReadOnly = true;
			this.colModulKlimaDeckeOpenCircuits.Width = 70;
			// 
			// colModulKlimaDeckePlannedCircuits
			// 
			this.colModulKlimaDeckePlannedCircuits.DataPropertyName = "ModulKlimaDeckePlannedCircuits";
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle12.Format = "F0";
			this.colModulKlimaDeckePlannedCircuits.DefaultCellStyle = dataGridViewCellStyle12;
			this.colModulKlimaDeckePlannedCircuits.FillWeight = 70F;
			this.colModulKlimaDeckePlannedCircuits.HeaderText = "Klimadecke\nangeschl.\nHeizkreise";
			this.colModulKlimaDeckePlannedCircuits.Name = "colModulKlimaDeckePlannedCircuits";
			this.colModulKlimaDeckePlannedCircuits.ReadOnly = true;
			this.colModulKlimaDeckePlannedCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colModulKlimaDeckePlannedCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colModulKlimaDeckePlannedCircuits.Width = 70;
			// 
			// QuickDimensioningDistributorGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridRooms);
			this.Name = "QuickDimensioningDistributorGrid";
			this.Size = new System.Drawing.Size(645, 390);
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSourceRooms)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridRooms;
		private System.Windows.Forms.BindingSource dataSourceRooms;
		private System.Windows.Forms.DataGridViewTextBoxColumn colRoomId;
		private System.Windows.Forms.DataGridViewTextBoxColumn colRoomName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colFloorName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colEurovalOpenCircuits;
		private DataGridViewNumericUpDownColumn colEurovalPlannedCircuits;
		private System.Windows.Forms.DataGridViewTextBoxColumn colConcreteActivationOpenCircuits;
		private NumericColumn colConcreteActivationPlannedCircuits;
		private System.Windows.Forms.DataGridViewTextBoxColumn colHithermOpenCircuits;
		private NumericColumn colHithermPlannedCircuits;
		private System.Windows.Forms.DataGridViewTextBoxColumn colHithermCompactOpenCircuits;
		private NumericColumn colHithermCompactPlannedCircuits;
		private System.Windows.Forms.DataGridViewTextBoxColumn colModulKlimaBodenOpenCircuits;
		private NumericColumn colModulKlimaBodenPlannedCircuits;
		private System.Windows.Forms.DataGridViewTextBoxColumn colModulKlimaDeckeOpenCircuits;
		private NumericColumn colModulKlimaDeckePlannedCircuits;
	}
}
