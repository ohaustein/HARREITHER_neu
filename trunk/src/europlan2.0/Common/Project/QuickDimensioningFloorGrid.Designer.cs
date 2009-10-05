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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
			this.quickDimensioningGrid = new System.Windows.Forms.DataGridView();
			this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRoomTemperature = new Europlan.Common.NumericColumn();
			this.colArea = new Europlan.Common.NumericColumn();
			this.colRoomType = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colHeatLoad = new Europlan.Common.NumericColumn();
			this.colCoolLoad = new Europlan.Common.NumericColumn();
			this.colEuroval = new Europlan.Common.NumericColumn();
			this.colEurovalCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colConcreteActivation = new Europlan.Common.NumericColumn();
			this.colConcreteActivationCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colHitherm = new Europlan.Common.NumericColumn();
			this.colHithermCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colHithermCompact = new Europlan.Common.NumericColumn();
			this.colHithermCompactCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colHithermCompactRoof = new Europlan.Common.NumericColumn();
			this.colHithermCompactRoofCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colModulKlimaBoden = new Europlan.Common.NumericColumn();
			this.colModulKlimaBodenCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colModulKlimaDecke = new Europlan.Common.NumericColumn();
			this.colModulKlimaDeckeCircuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRoomController = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colNrOfServos = new Europlan.Common.NumericColumn();
			this.colComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRevert = new System.Windows.Forms.DataGridViewButtonColumn();
			this.roomBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.quickDimensioningGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.roomBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// quickDimensioningGrid
			// 
			this.quickDimensioningGrid.AllowUserToAddRows = false;
			this.quickDimensioningGrid.AllowUserToDeleteRows = false;
			this.quickDimensioningGrid.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.quickDimensioningGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.quickDimensioningGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.quickDimensioningGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colName,
            this.colRoomTemperature,
            this.colArea,
            this.colRoomType,
            this.colHeatLoad,
            this.colCoolLoad,
            this.colEuroval,
            this.colEurovalCircuits,
            this.colConcreteActivation,
            this.colConcreteActivationCircuits,
            this.colHitherm,
            this.colHithermCircuits,
            this.colHithermCompact,
            this.colHithermCompactCircuits,
            this.colHithermCompactRoof,
            this.colHithermCompactRoofCircuits,
            this.colModulKlimaBoden,
            this.colModulKlimaBodenCircuits,
            this.colModulKlimaDecke,
            this.colModulKlimaDeckeCircuits,
            this.colRoomController,
            this.colNrOfServos,
            this.colComments,
            this.colRevert});
			this.quickDimensioningGrid.DataSource = this.roomBindingSource;
			this.quickDimensioningGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.quickDimensioningGrid.Location = new System.Drawing.Point(0, 0);
			this.quickDimensioningGrid.MultiSelect = false;
			this.quickDimensioningGrid.Name = "quickDimensioningGrid";
			this.quickDimensioningGrid.Size = new System.Drawing.Size(753, 265);
			this.quickDimensioningGrid.TabIndex = 0;
			this.quickDimensioningGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.quickDimensioningGrid_CellValueChanged);
			this.quickDimensioningGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.quickDimensioningGrid_CellBeginEdit);
			this.quickDimensioningGrid.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.quickDimensioningGrid_CellValidated);
			this.quickDimensioningGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.quickDimensioningGrid_PreviewKeyDown);
			this.quickDimensioningGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.quickDimensioningGrid_CellValidating);
			this.quickDimensioningGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.quickDimensioningGrid_RowsAdded);
			this.quickDimensioningGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.quickDimensioningGrid_CellClick);
			this.quickDimensioningGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.quickDimensioningGrid_EditingControlShowing);
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
			this.colRoomTemperature.DataPropertyName = "QuickDimensioningRoomTemperature";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.colRoomTemperature.DefaultCellStyle = dataGridViewCellStyle2;
			this.colRoomTemperature.FillWeight = 50F;
			this.colRoomTemperature.HeaderText = "Temp.\n(°C)";
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
			this.colArea.HeaderText = "Raumfl.\n(m²)";
			this.colArea.Name = "colArea";
			this.colArea.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colArea.ReadOnly = true;
			this.colArea.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colArea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colArea.Width = 50;
			// 
			// colRoomType
			// 
			this.colRoomType.DataPropertyName = "QuickDimensioningRoomType";
			this.colRoomType.FillWeight = 80F;
			this.colRoomType.HeaderText = "Raumtyp";
			this.colRoomType.Name = "colRoomType";
			this.colRoomType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colRoomType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colRoomType.Width = 80;
			// 
			// colHeatLoad
			// 
			this.colHeatLoad.DataPropertyName = "QuickDimensioningHeatLoad";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "F0";
			this.colHeatLoad.DefaultCellStyle = dataGridViewCellStyle4;
			this.colHeatLoad.FillWeight = 50F;
			this.colHeatLoad.HeaderText = "Heizlast\n(W)";
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
			this.colCoolLoad.HeaderText = "Kühllast\n(W)";
			this.colCoolLoad.Name = "colCoolLoad";
			this.colCoolLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.colCoolLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colCoolLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colCoolLoad.Visible = false;
			this.colCoolLoad.Width = 50;
			// 
			// colEuroval
			// 
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "F1";
			this.colEuroval.DefaultCellStyle = dataGridViewCellStyle6;
			this.colEuroval.FillWeight = 70F;
			this.colEuroval.HeaderText = "Euroval®\n(m²)";
			this.colEuroval.Name = "colEuroval";
			this.colEuroval.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colEuroval.Visible = false;
			this.colEuroval.Width = 70;
			// 
			// colEurovalCircuits
			// 
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colEurovalCircuits.DefaultCellStyle = dataGridViewCellStyle7;
			this.colEurovalCircuits.FillWeight = 70F;
			this.colEurovalCircuits.HeaderText = "Euroval®\nHeizkreise";
			this.colEurovalCircuits.Name = "colEurovalCircuits";
			this.colEurovalCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colEurovalCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colEurovalCircuits.Visible = false;
			this.colEurovalCircuits.Width = 70;
			// 
			// colConcreteActivation
			// 
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle8.Format = "F1";
			this.colConcreteActivation.DefaultCellStyle = dataGridViewCellStyle8;
			this.colConcreteActivation.FillWeight = 70F;
			this.colConcreteActivation.HeaderText = "BKA\n(m²)";
			this.colConcreteActivation.Name = "colConcreteActivation";
			this.colConcreteActivation.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colConcreteActivation.Visible = false;
			this.colConcreteActivation.Width = 70;
			// 
			// colConcreteActivationCircuits
			// 
			this.colConcreteActivationCircuits.FillWeight = 70F;
			this.colConcreteActivationCircuits.HeaderText = "BKA\nHeizkreise";
			this.colConcreteActivationCircuits.Name = "colConcreteActivationCircuits";
			this.colConcreteActivationCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colConcreteActivationCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colConcreteActivationCircuits.Visible = false;
			this.colConcreteActivationCircuits.Width = 70;
			// 
			// colHitherm
			// 
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle9.Format = "F1";
			this.colHitherm.DefaultCellStyle = dataGridViewCellStyle9;
			this.colHitherm.FillWeight = 70F;
			this.colHitherm.HeaderText = "Hitherm®\n(m²)";
			this.colHitherm.Name = "colHitherm";
			this.colHitherm.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colHitherm.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colHitherm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colHitherm.Visible = false;
			this.colHitherm.Width = 70;
			// 
			// colHithermCircuits
			// 
			this.colHithermCircuits.FillWeight = 70F;
			this.colHithermCircuits.HeaderText = "Hitherm®\nHeizkreise";
			this.colHithermCircuits.Name = "colHithermCircuits";
			this.colHithermCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colHithermCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colHithermCircuits.Visible = false;
			this.colHithermCircuits.Width = 70;
			// 
			// colHithermCompact
			// 
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle10.Format = "F1";
			this.colHithermCompact.DefaultCellStyle = dataGridViewCellStyle10;
			this.colHithermCompact.FillWeight = 75F;
			this.colHithermCompact.HeaderText = "Hitherm® Co\n(m²)";
			this.colHithermCompact.Name = "colHithermCompact";
			this.colHithermCompact.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colHithermCompact.Visible = false;
			this.colHithermCompact.Width = 75;
			// 
			// colHithermCompactCircuits
			// 
			this.colHithermCompactCircuits.FillWeight = 75F;
			this.colHithermCompactCircuits.HeaderText = "Hitherm® Co\nHeizkreise";
			this.colHithermCompactCircuits.Name = "colHithermCompactCircuits";
			this.colHithermCompactCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colHithermCompactCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colHithermCompactCircuits.Visible = false;
			this.colHithermCompactCircuits.Width = 75;
			// 
			// colHithermCompactRoof
			// 
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle11.Format = "F1";
			this.colHithermCompactRoof.DefaultCellStyle = dataGridViewCellStyle11;
			this.colHithermCompactRoof.FillWeight = 75F;
			this.colHithermCompactRoof.HeaderText = "Hitherm® Co\nDach (m²)";
			this.colHithermCompactRoof.Name = "colHithermCompactRoof";
			this.colHithermCompactRoof.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colHithermCompactRoof.Visible = false;
			this.colHithermCompactRoof.Width = 75;
			// 
			// colHithermCompactRoofCircuits
			// 
			this.colHithermCompactRoofCircuits.FillWeight = 75F;
			this.colHithermCompactRoofCircuits.HeaderText = "Hitherm® Co\nDach Hkr.";
			this.colHithermCompactRoofCircuits.Name = "colHithermCompactRoofCircuits";
			this.colHithermCompactRoofCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colHithermCompactRoofCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colHithermCompactRoofCircuits.Visible = false;
			this.colHithermCompactRoofCircuits.Width = 75;
			// 
			// colModulKlimaBoden
			// 
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle12.Format = "F1";
			this.colModulKlimaBoden.DefaultCellStyle = dataGridViewCellStyle12;
			this.colModulKlimaBoden.FillWeight = 75F;
			this.colModulKlimaBoden.HeaderText = "Klima-Boden\n(m²)";
			this.colModulKlimaBoden.Name = "colModulKlimaBoden";
			this.colModulKlimaBoden.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colModulKlimaBoden.Visible = false;
			this.colModulKlimaBoden.Width = 75;
			// 
			// colModulKlimaBodenCircuits
			// 
			this.colModulKlimaBodenCircuits.FillWeight = 75F;
			this.colModulKlimaBodenCircuits.HeaderText = "Klima-Boden\nHeizkreise";
			this.colModulKlimaBodenCircuits.Name = "colModulKlimaBodenCircuits";
			this.colModulKlimaBodenCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colModulKlimaBodenCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colModulKlimaBodenCircuits.Visible = false;
			this.colModulKlimaBodenCircuits.Width = 75;
			// 
			// colModulKlimaDecke
			// 
			dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle13.Format = "F1";
			this.colModulKlimaDecke.DefaultCellStyle = dataGridViewCellStyle13;
			this.colModulKlimaDecke.FillWeight = 75F;
			this.colModulKlimaDecke.HeaderText = "Klima-Decke\n(m²)";
			this.colModulKlimaDecke.Name = "colModulKlimaDecke";
			this.colModulKlimaDecke.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colModulKlimaDecke.Visible = false;
			this.colModulKlimaDecke.Width = 75;
			// 
			// colModulKlimaDeckeCircuits
			// 
			this.colModulKlimaDeckeCircuits.FillWeight = 750F;
			this.colModulKlimaDeckeCircuits.HeaderText = "Klima-Decke\nHeizkreise";
			this.colModulKlimaDeckeCircuits.Name = "colModulKlimaDeckeCircuits";
			this.colModulKlimaDeckeCircuits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colModulKlimaDeckeCircuits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colModulKlimaDeckeCircuits.Visible = false;
			this.colModulKlimaDeckeCircuits.Width = 75;
			// 
			// colRoomController
			// 
			this.colRoomController.DataPropertyName = "QuickDimensioningRoomController";
			this.colRoomController.FillWeight = 60F;
			this.colRoomController.HeaderText = "Raum-\ncontroller";
			this.colRoomController.Name = "colRoomController";
			this.colRoomController.Width = 60;
			// 
			// colNrOfServos
			// 
			this.colNrOfServos.DataPropertyName = "QuickDimensioningNrOfServos";
			dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle14.Format = "F0";
			this.colNrOfServos.DefaultCellStyle = dataGridViewCellStyle14;
			this.colNrOfServos.FillWeight = 50F;
			this.colNrOfServos.HeaderText = "Stell-\nmotore";
			this.colNrOfServos.Name = "colNrOfServos";
			this.colNrOfServos.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colNrOfServos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.colNrOfServos.Width = 50;
			// 
			// colComments
			// 
			this.colComments.DataPropertyName = "QuickDimensioningComments";
			this.colComments.HeaderText = "Bemerkung";
			this.colComments.Name = "colComments";
			// 
			// colRevert
			// 
			this.colRevert.FillWeight = 70F;
			this.colRevert.HeaderText = "Rücksetzen";
			this.colRevert.Name = "colRevert";
			this.colRevert.ReadOnly = true;
			this.colRevert.Text = "Rücksetzen";
			this.colRevert.UseColumnTextForButtonValue = true;
			this.colRevert.Width = 70;
			// 
			// roomBindingSource
			// 
			this.roomBindingSource.DataSource = typeof(Europlan.Common.Room);
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 265);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(753, 26);
			this.panel1.TabIndex = 1;
			this.panel1.Visible = false;
			// 
			// QuickDimensioningFloorGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.quickDimensioningGrid);
			this.Controls.Add(this.panel1);
			this.Name = "QuickDimensioningFloorGrid";
			this.Size = new System.Drawing.Size(753, 291);
			((System.ComponentModel.ISupportInitialize)(this.quickDimensioningGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.roomBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView quickDimensioningGrid;
		private System.Windows.Forms.BindingSource roomBindingSource;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridViewTextBoxColumn colId;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private NumericColumn colRoomTemperature;
		private NumericColumn colArea;
		private System.Windows.Forms.DataGridViewComboBoxColumn colRoomType;
		private NumericColumn colHeatLoad;
		private NumericColumn colCoolLoad;
		private NumericColumn colEuroval;
		private System.Windows.Forms.DataGridViewTextBoxColumn colEurovalCircuits;
		private NumericColumn colConcreteActivation;
		private System.Windows.Forms.DataGridViewTextBoxColumn colConcreteActivationCircuits;
		private NumericColumn colHitherm;
		private System.Windows.Forms.DataGridViewTextBoxColumn colHithermCircuits;
		private NumericColumn colHithermCompact;
		private System.Windows.Forms.DataGridViewTextBoxColumn colHithermCompactCircuits;
		private NumericColumn colHithermCompactRoof;
		private System.Windows.Forms.DataGridViewTextBoxColumn colHithermCompactRoofCircuits;
		private NumericColumn colModulKlimaBoden;
		private System.Windows.Forms.DataGridViewTextBoxColumn colModulKlimaBodenCircuits;
		private NumericColumn colModulKlimaDecke;
		private System.Windows.Forms.DataGridViewTextBoxColumn colModulKlimaDeckeCircuits;
		private System.Windows.Forms.DataGridViewComboBoxColumn colRoomController;
		private NumericColumn colNrOfServos;
		private System.Windows.Forms.DataGridViewTextBoxColumn colComments;
		private System.Windows.Forms.DataGridViewButtonColumn colRevert;
	}
}
