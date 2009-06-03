namespace Europlan.Common {
	partial class QuickDimensioningFloorGrid {
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
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.colRoomController = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRoomTemperature = new Europlan.Common.NumericColumn();
			this.colArea = new Europlan.Common.NumericColumn();
			this.colRoomType = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colHeatLoad = new Europlan.Common.NumericColumn();
			this.colCoolLoad = new Europlan.Common.NumericColumn();
			this.colNrOfServos = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDistributor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.roomBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.roomBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colName,
            this.colRoomTemperature,
            this.colArea,
            this.colRoomType,
            this.colHeatLoad,
            this.colCoolLoad,
            this.colNrOfServos,
            this.colRoomController,
            this.colDistributor,
            this.colComments});
			this.dataGridView1.DataSource = this.roomBindingSource;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(753, 265);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.textBox3);
			this.panel1.Controls.Add(this.textBox2);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 265);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(753, 26);
			this.panel1.TabIndex = 1;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(424, 3);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(45, 20);
			this.textBox3.TabIndex = 2;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(373, 3);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(45, 20);
			this.textBox2.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(225, 3);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(45, 20);
			this.textBox1.TabIndex = 0;
			// 
			// colRoomController
			// 
			this.colRoomController.DataPropertyName = "QuickDimensioningRoomController";
			this.colRoomController.FillWeight = 60F;
			this.colRoomController.HeaderText = "Raum- controller";
			this.colRoomController.Name = "colRoomController";
			this.colRoomController.Width = 60;
			// 
			// colId
			// 
			this.colId.DataPropertyName = "Id";
			this.colId.FillWeight = 40F;
			this.colId.HeaderText = "Nr.";
			this.colId.Name = "colId";
			this.colId.ReadOnly = true;
			this.colId.Width = 40;
			// 
			// colName
			// 
			this.colName.DataPropertyName = "Name";
			this.colName.HeaderText = "Bezeichnung";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colRoomTemperature
			// 
			this.colRoomTemperature.DataPropertyName = "RoomTemperature";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.colRoomTemperature.DefaultCellStyle = dataGridViewCellStyle2;
			this.colRoomTemperature.FillWeight = 50F;
			this.colRoomTemperature.HeaderText = "Temp. (°C)";
			this.colRoomTemperature.Name = "colRoomTemperature";
			this.colRoomTemperature.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.colRoomTemperature.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colRoomTemperature.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colRoomTemperature.Width = 50;
			// 
			// colArea
			// 
			this.colArea.DataPropertyName = "Area";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F1";
			this.colArea.DefaultCellStyle = dataGridViewCellStyle3;
			this.colArea.FillWeight = 50F;
			this.colArea.HeaderText = "Raumfl. (m²)";
			this.colArea.Name = "colArea";
			this.colArea.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colArea.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colArea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colArea.Width = 50;
			// 
			// colRoomType
			// 
			this.colRoomType.DataPropertyName = "RoomType";
			this.colRoomType.HeaderText = "Raumtyp";
			this.colRoomType.Name = "colRoomType";
			this.colRoomType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colRoomType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// colHeatLoad
			// 
			this.colHeatLoad.DataPropertyName = "QuickDimensioningHeatLoad";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.colHeatLoad.DefaultCellStyle = dataGridViewCellStyle4;
			this.colHeatLoad.FillWeight = 50F;
			this.colHeatLoad.HeaderText = "Heizlast (W)";
			this.colHeatLoad.Name = "colHeatLoad";
			this.colHeatLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.colHeatLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colHeatLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colHeatLoad.Width = 50;
			// 
			// colCoolLoad
			// 
			this.colCoolLoad.DataPropertyName = "QuickDimensioningCoolLoad";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "F0";
			this.colCoolLoad.DefaultCellStyle = dataGridViewCellStyle5;
			this.colCoolLoad.FillWeight = 50F;
			this.colCoolLoad.HeaderText = "Kühllast (W)";
			this.colCoolLoad.Name = "colCoolLoad";
			this.colCoolLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.colCoolLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colCoolLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colCoolLoad.Visible = false;
			this.colCoolLoad.Width = 50;
			// 
			// colNrOfServos
			// 
			this.colNrOfServos.DataPropertyName = "QuickDimensioningNrOfServos";
			this.colNrOfServos.FillWeight = 50F;
			this.colNrOfServos.HeaderText = "Stell- motore";
			this.colNrOfServos.Name = "colNrOfServos";
			this.colNrOfServos.ReadOnly = true;
			this.colNrOfServos.Width = 50;
			// 
			// colDistributor
			// 
			this.colDistributor.DataPropertyName = "QuickDimensioningDistributor";
			this.colDistributor.FillWeight = 50F;
			this.colDistributor.HeaderText = "Verteiler";
			this.colDistributor.Name = "colDistributor";
			this.colDistributor.Width = 50;
			// 
			// colComments
			// 
			this.colComments.DataPropertyName = "QuickDimensioningComments";
			this.colComments.HeaderText = "Bemerkung";
			this.colComments.Name = "colComments";
			// 
			// roomBindingSource
			// 
			this.roomBindingSource.DataSource = typeof(Europlan.Common.Room);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "QuickDimensioningNrOfServos";
			this.dataGridViewTextBoxColumn1.FillWeight = 50F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Stell- motore";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 50;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "QuickDimensioningDistributor";
			this.dataGridViewTextBoxColumn2.FillWeight = 50F;
			this.dataGridViewTextBoxColumn2.HeaderText = "Verteiler";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.Width = 50;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "QuickDimensioningComments";
			this.dataGridViewTextBoxColumn3.HeaderText = "Kommentar";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			// 
			// QuickDimensioningFloorGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.panel1);
			this.Name = "QuickDimensioningFloorGrid";
			this.Size = new System.Drawing.Size(753, 291);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.roomBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.BindingSource roomBindingSource;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn colId;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private NumericColumn colRoomTemperature;
		private NumericColumn colArea;
		private System.Windows.Forms.DataGridViewComboBoxColumn colRoomType;
		private NumericColumn colHeatLoad;
		private NumericColumn colCoolLoad;
		private System.Windows.Forms.DataGridViewTextBoxColumn colNrOfServos;
		private System.Windows.Forms.DataGridViewComboBoxColumn colRoomController;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDistributor;
		private System.Windows.Forms.DataGridViewTextBoxColumn colComments;
	}
}
