namespace Europlan.Common {
	partial class RoomSummaryPanel {
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
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.lblArea = new System.Windows.Forms.Label();
			this.lblTemperature = new System.Windows.Forms.Label();
			this.lblHeat = new System.Windows.Forms.Label();
			this.lblNormHeat = new System.Windows.Forms.Label();
			this.lblCool = new System.Windows.Forms.Label();
			this.lblNormCool = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.grpBoxSummary = new System.Windows.Forms.GroupBox();
			this.lblAreaValue = new System.Windows.Forms.Label();
			this.lblCoolLoadValue = new System.Windows.Forms.Label();
			this.lblHeatLoadValue = new System.Windows.Forms.Label();
			this.lblRoomName = new System.Windows.Forms.Label();
			this.lblRoomData = new System.Windows.Forms.Label();
			this.grpBoxSystems = new System.Windows.Forms.GroupBox();
			this.btnDelete = new System.Windows.Forms.Button();
			this.dgvProducts = new System.Windows.Forms.DataGridView();
			this.plannedProductWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnAdd = new System.Windows.Forms.Button();
			this.txtNormCool = new Europlan.Common.NumericBox();
			this.txtCool = new Europlan.Common.NumericBox();
			this.txtNormHeat = new Europlan.Common.NumericBox();
			this.txtHeat = new Europlan.Common.NumericBox();
			this.txtTemperature = new Europlan.Common.NumericBox();
			this.txtArea = new Europlan.Common.NumericBox();
			this.btnWhatIsNext = new System.Windows.Forms.Button();
			this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colFloorArea = new Europlan.Common.NumericColumn();
			this.colPlannedArea = new Europlan.Common.NumericColumn();
			this.colPlannedHeatLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colPlannedCoolLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
			this.grpBoxSummary.SuspendLayout();
			this.grpBoxSystems.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.plannedProductWrapperBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(129, 131);
			this.txtName.Name = "txtName";
			this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtName.Size = new System.Drawing.Size(562, 20);
			this.txtName.TabIndex = 13;
			this.txtName.Visible = false;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(3, 131);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(108, 23);
			this.lblName.TabIndex = 12;
			this.lblName.Text = "Name:";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblName.Visible = false;
			this.lblName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// lblArea
			// 
			this.lblArea.Location = new System.Drawing.Point(6, 37);
			this.lblArea.Name = "lblArea";
			this.lblArea.Size = new System.Drawing.Size(60, 13);
			this.lblArea.TabIndex = 14;
			this.lblArea.Text = "Fläche:";
			this.lblArea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTemperature
			// 
			this.lblTemperature.Location = new System.Drawing.Point(3, 177);
			this.lblTemperature.Name = "lblTemperature";
			this.lblTemperature.Size = new System.Drawing.Size(120, 23);
			this.lblTemperature.TabIndex = 16;
			this.lblTemperature.Text = "Norminnentemperatur:";
			this.lblTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblTemperature.Visible = false;
			// 
			// lblHeat
			// 
			this.lblHeat.Location = new System.Drawing.Point(6, 16);
			this.lblHeat.Name = "lblHeat";
			this.lblHeat.Size = new System.Drawing.Size(60, 13);
			this.lblHeat.TabIndex = 18;
			this.lblHeat.Text = "Heizlast:";
			this.lblHeat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblNormHeat
			// 
			this.lblNormHeat.Location = new System.Drawing.Point(3, 223);
			this.lblNormHeat.Name = "lblNormHeat";
			this.lblNormHeat.Size = new System.Drawing.Size(120, 23);
			this.lblNormHeat.TabIndex = 20;
			this.lblNormHeat.Text = "Heizlast (bereinigt):";
			this.lblNormHeat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblNormHeat.Visible = false;
			// 
			// lblCool
			// 
			this.lblCool.Location = new System.Drawing.Point(231, 16);
			this.lblCool.Name = "lblCool";
			this.lblCool.Size = new System.Drawing.Size(60, 13);
			this.lblCool.TabIndex = 22;
			this.lblCool.Text = "Kühllast:";
			this.lblCool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblNormCool
			// 
			this.lblNormCool.Location = new System.Drawing.Point(3, 269);
			this.lblNormCool.Name = "lblNormCool";
			this.lblNormCool.Size = new System.Drawing.Size(120, 23);
			this.lblNormCool.TabIndex = 24;
			this.lblNormCool.Text = "Kühllast (bereinigt):";
			this.lblNormCool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblNormCool.Visible = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(697, 154);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 23);
			this.label1.TabIndex = 26;
			this.label1.Text = "m²";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.Visible = false;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(697, 177);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 23);
			this.label2.TabIndex = 27;
			this.label2.Text = "°C";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label2.Visible = false;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(697, 200);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 23);
			this.label3.TabIndex = 28;
			this.label3.Text = "W";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label3.Visible = false;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(697, 223);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 23);
			this.label4.TabIndex = 29;
			this.label4.Text = "W";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label4.Visible = false;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(697, 246);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 23);
			this.label5.TabIndex = 30;
			this.label5.Text = "W";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label5.Visible = false;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(697, 269);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 23);
			this.label6.TabIndex = 31;
			this.label6.Text = "W";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label6.Visible = false;
			// 
			// grpBoxSummary
			// 
			this.grpBoxSummary.Controls.Add(this.lblAreaValue);
			this.grpBoxSummary.Controls.Add(this.lblCoolLoadValue);
			this.grpBoxSummary.Controls.Add(this.lblHeatLoadValue);
			this.grpBoxSummary.Controls.Add(this.lblArea);
			this.grpBoxSummary.Controls.Add(this.lblHeat);
			this.grpBoxSummary.Controls.Add(this.lblCool);
			this.grpBoxSummary.Location = new System.Drawing.Point(3, 25);
			this.grpBoxSummary.Name = "grpBoxSummary";
			this.grpBoxSummary.Size = new System.Drawing.Size(577, 63);
			this.grpBoxSummary.TabIndex = 39;
			this.grpBoxSummary.TabStop = false;
			// 
			// lblAreaValue
			// 
			this.lblAreaValue.Location = new System.Drawing.Point(72, 37);
			this.lblAreaValue.Name = "lblAreaValue";
			this.lblAreaValue.Size = new System.Drawing.Size(153, 13);
			this.lblAreaValue.TabIndex = 25;
			this.lblAreaValue.Text = "label9";
			// 
			// lblCoolLoadValue
			// 
			this.lblCoolLoadValue.Location = new System.Drawing.Point(297, 16);
			this.lblCoolLoadValue.Name = "lblCoolLoadValue";
			this.lblCoolLoadValue.Size = new System.Drawing.Size(153, 13);
			this.lblCoolLoadValue.TabIndex = 24;
			this.lblCoolLoadValue.Text = "label8";
			// 
			// lblHeatLoadValue
			// 
			this.lblHeatLoadValue.Location = new System.Drawing.Point(72, 16);
			this.lblHeatLoadValue.Name = "lblHeatLoadValue";
			this.lblHeatLoadValue.Size = new System.Drawing.Size(153, 13);
			this.lblHeatLoadValue.TabIndex = 23;
			this.lblHeatLoadValue.Text = "label7";
			// 
			// lblRoomName
			// 
			this.lblRoomName.AutoSize = true;
			this.lblRoomName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRoomName.Location = new System.Drawing.Point(131, 0);
			this.lblRoomName.Name = "lblRoomName";
			this.lblRoomName.Size = new System.Drawing.Size(0, 24);
			this.lblRoomName.TabIndex = 41;
			// 
			// lblRoomData
			// 
			this.lblRoomData.AutoSize = true;
			this.lblRoomData.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRoomData.Location = new System.Drawing.Point(3, 0);
			this.lblRoomData.Name = "lblRoomData";
			this.lblRoomData.Size = new System.Drawing.Size(122, 24);
			this.lblRoomData.TabIndex = 40;
			this.lblRoomData.Text = "Raumdaten:";
			// 
			// grpBoxSystems
			// 
			this.grpBoxSystems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpBoxSystems.Controls.Add(this.btnDelete);
			this.grpBoxSystems.Controls.Add(this.dgvProducts);
			this.grpBoxSystems.Controls.Add(this.btnAdd);
			this.grpBoxSystems.Location = new System.Drawing.Point(3, 94);
			this.grpBoxSystems.Name = "grpBoxSystems";
			this.grpBoxSystems.Size = new System.Drawing.Size(733, 289);
			this.grpBoxSystems.TabIndex = 42;
			this.grpBoxSystems.TabStop = false;
			this.grpBoxSystems.Text = "Heizsysteme";
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(85, 19);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "Löschen";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// dgvProducts
			// 
			this.dgvProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvProducts.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colType,
            this.colSystem,
            this.colComment,
            this.colFloorArea,
            this.colPlannedArea,
            this.colPlannedHeatLoad,
            this.colPlannedCoolLoad,
            this.colEdit});
			this.dgvProducts.DataSource = this.plannedProductWrapperBindingSource;
			this.dgvProducts.Location = new System.Drawing.Point(6, 48);
			this.dgvProducts.MultiSelect = false;
			this.dgvProducts.Name = "dgvProducts";
			this.dgvProducts.Size = new System.Drawing.Size(721, 235);
			this.dgvProducts.TabIndex = 2;
			this.dgvProducts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellValueChanged);
			this.dgvProducts.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
			this.dgvProducts.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView1_UserDeletedRow);
			this.dgvProducts.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvProducts_CellPainting);
			this.dgvProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
			this.dgvProducts.SelectionChanged += new System.EventHandler(this.dgvProducts_SelectionChanged);
			// 
			// plannedProductWrapperBindingSource
			// 
			this.plannedProductWrapperBindingSource.DataSource = typeof(Europlan.Common.PlannedProduct);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(4, 19);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Hinzufügen";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// txtNormCool
			// 
			this.txtNormCool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNormCool.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.txtNormCool.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormCool.Location = new System.Drawing.Point(129, 269);
			this.txtNormCool.MaxValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.txtNormCool.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormCool.Name = "txtNormCool";
			this.txtNormCool.Size = new System.Drawing.Size(562, 20);
			this.txtNormCool.TabIndex = 38;
			this.txtNormCool.Text = "0";
			this.txtNormCool.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormCool.Visible = false;
			this.txtNormCool.ValueChanged += new System.EventHandler(this.txtNormCool_TextChanged);
			// 
			// txtCool
			// 
			this.txtCool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCool.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.txtCool.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtCool.Location = new System.Drawing.Point(129, 246);
			this.txtCool.MaxValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.txtCool.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtCool.Name = "txtCool";
			this.txtCool.Size = new System.Drawing.Size(562, 20);
			this.txtCool.TabIndex = 37;
			this.txtCool.Text = "0";
			this.txtCool.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtCool.Visible = false;
			this.txtCool.ValueChanged += new System.EventHandler(this.txtCool_TextChanged);
			// 
			// txtNormHeat
			// 
			this.txtNormHeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNormHeat.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.txtNormHeat.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormHeat.Location = new System.Drawing.Point(129, 223);
			this.txtNormHeat.MaxValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.txtNormHeat.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormHeat.Name = "txtNormHeat";
			this.txtNormHeat.Size = new System.Drawing.Size(562, 20);
			this.txtNormHeat.TabIndex = 36;
			this.txtNormHeat.Text = "0";
			this.txtNormHeat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtNormHeat.Visible = false;
			this.txtNormHeat.ValueChanged += new System.EventHandler(this.txtNormHeat_TextChanged);
			// 
			// txtHeat
			// 
			this.txtHeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHeat.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.txtHeat.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtHeat.Location = new System.Drawing.Point(129, 200);
			this.txtHeat.MaxValue = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.txtHeat.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtHeat.Name = "txtHeat";
			this.txtHeat.Size = new System.Drawing.Size(562, 20);
			this.txtHeat.TabIndex = 35;
			this.txtHeat.Text = "0";
			this.txtHeat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtHeat.Visible = false;
			this.txtHeat.ValueChanged += new System.EventHandler(this.txtHeat_TextChanged);
			// 
			// txtTemperature
			// 
			this.txtTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTemperature.EditType = Europlan.Common.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.txtTemperature.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtTemperature.Location = new System.Drawing.Point(129, 177);
			this.txtTemperature.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.txtTemperature.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtTemperature.Name = "txtTemperature";
			this.txtTemperature.Size = new System.Drawing.Size(562, 20);
			this.txtTemperature.TabIndex = 34;
			this.txtTemperature.Text = "0";
			this.txtTemperature.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtTemperature.Visible = false;
			this.txtTemperature.ValueChanged += new System.EventHandler(this.txtTemperature_TextChanged);
			// 
			// txtArea
			// 
			this.txtArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtArea.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.txtArea.InternalValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtArea.Location = new System.Drawing.Point(129, 154);
			this.txtArea.MaxValue = null;
			this.txtArea.MinValue = null;
			this.txtArea.Name = "txtArea";
			this.txtArea.Size = new System.Drawing.Size(562, 20);
			this.txtArea.TabIndex = 33;
			this.txtArea.Text = "0";
			this.txtArea.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.txtArea.Visible = false;
			this.txtArea.ValueChanged += new System.EventHandler(this.txtArea_TextChanged);
			// 
			// btnWhatIsNext
			// 
			this.btnWhatIsNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWhatIsNext.Location = new System.Drawing.Point(627, 389);
			this.btnWhatIsNext.Name = "btnWhatIsNext";
			this.btnWhatIsNext.Size = new System.Drawing.Size(109, 23);
			this.btnWhatIsNext.TabIndex = 23;
			this.btnWhatIsNext.Text = "Wie geht\'s weiter?";
			this.btnWhatIsNext.UseVisualStyleBackColor = true;
			this.btnWhatIsNext.Click += new System.EventHandler(this.btnWhatIsNext_Click);
			// 
			// colType
			// 
			this.colType.DataPropertyName = "PlannedProductType";
			this.colType.FillWeight = 50F;
			this.colType.HeaderText = "Type";
			this.colType.Name = "colType";
			this.colType.ReadOnly = true;
			this.colType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colType.Width = 50;
			// 
			// colSystem
			// 
			this.colSystem.DataPropertyName = "System";
			this.colSystem.HeaderText = "System";
			this.colSystem.Name = "colSystem";
			this.colSystem.ReadOnly = true;
			this.colSystem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// colComment
			// 
			this.colComment.DataPropertyName = "Comment";
			this.colComment.HeaderText = "Bemerkung";
			this.colComment.Name = "colComment";
			this.colComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colComment.Width = 200;
			// 
			// colFloorArea
			// 
			this.colFloorArea.DataPropertyName = "FloorArea";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F0";
			this.colFloorArea.DefaultCellStyle = dataGridViewCellStyle2;
			this.colFloorArea.FillWeight = 50F;
			this.colFloorArea.HeaderText = "FBH-\nFläche\n(m²)";
			this.colFloorArea.Name = "colFloorArea";
			this.colFloorArea.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colFloorArea.ReadOnly = true;
			this.colFloorArea.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colFloorArea.Width = 50;
			// 
			// colPlannedArea
			// 
			this.colPlannedArea.DataPropertyName = "PlannedArea";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "F0";
			this.colPlannedArea.DefaultCellStyle = dataGridViewCellStyle3;
			this.colPlannedArea.FillWeight = 50F;
			this.colPlannedArea.HeaderText = "Heiz-\nfläche\n(m²)";
			this.colPlannedArea.Name = "colPlannedArea";
			this.colPlannedArea.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.colPlannedArea.ReadOnly = true;
			this.colPlannedArea.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colPlannedArea.Width = 50;
			// 
			// colPlannedHeatLoad
			// 
			this.colPlannedHeatLoad.DataPropertyName = "PlannedHeatLoadString";
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colPlannedHeatLoad.DefaultCellStyle = dataGridViewCellStyle4;
			this.colPlannedHeatLoad.FillWeight = 50F;
			this.colPlannedHeatLoad.HeaderText = "PHeiz\n(W)";
			this.colPlannedHeatLoad.Name = "colPlannedHeatLoad";
			this.colPlannedHeatLoad.ReadOnly = true;
			this.colPlannedHeatLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colPlannedHeatLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colPlannedHeatLoad.Width = 50;
			// 
			// colPlannedCoolLoad
			// 
			this.colPlannedCoolLoad.DataPropertyName = "PlannedCoolLoadString";
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colPlannedCoolLoad.DefaultCellStyle = dataGridViewCellStyle5;
			this.colPlannedCoolLoad.FillWeight = 50F;
			this.colPlannedCoolLoad.HeaderText = "PKühl\n(W)";
			this.colPlannedCoolLoad.Name = "colPlannedCoolLoad";
			this.colPlannedCoolLoad.ReadOnly = true;
			this.colPlannedCoolLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colPlannedCoolLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colPlannedCoolLoad.Width = 50;
			// 
			// colEdit
			// 
			this.colEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colEdit.FillWeight = 64F;
			this.colEdit.HeaderText = "Bearbeiten";
			this.colEdit.Name = "colEdit";
			this.colEdit.ReadOnly = true;
			this.colEdit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colEdit.Text = "...";
			this.colEdit.UseColumnTextForButtonValue = true;
			this.colEdit.Width = 64;
			// 
			// RoomSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnWhatIsNext);
			this.Controls.Add(this.grpBoxSystems);
			this.Controls.Add(this.lblRoomName);
			this.Controls.Add(this.lblRoomData);
			this.Controls.Add(this.grpBoxSummary);
			this.Controls.Add(this.txtNormCool);
			this.Controls.Add(this.txtCool);
			this.Controls.Add(this.txtNormHeat);
			this.Controls.Add(this.txtHeat);
			this.Controls.Add(this.txtTemperature);
			this.Controls.Add(this.txtArea);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblNormCool);
			this.Controls.Add(this.lblNormHeat);
			this.Controls.Add(this.lblTemperature);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.Name = "RoomSummaryPanel";
			this.Size = new System.Drawing.Size(739, 415);
			this.grpBoxSummary.ResumeLayout(false);
			this.grpBoxSystems.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.plannedProductWrapperBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblArea;
		private System.Windows.Forms.Label lblTemperature;
		private System.Windows.Forms.Label lblHeat;
		private System.Windows.Forms.Label lblNormHeat;
		private System.Windows.Forms.Label lblCool;
		private System.Windows.Forms.Label lblNormCool;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private Europlan.Common.NumericBox txtArea;
		private Europlan.Common.NumericBox txtTemperature;
		private Europlan.Common.NumericBox txtHeat;
		private Europlan.Common.NumericBox txtNormHeat;
		private Europlan.Common.NumericBox txtCool;
		private Europlan.Common.NumericBox txtNormCool;
		private System.Windows.Forms.GroupBox grpBoxSummary;
		private System.Windows.Forms.Label lblHeatLoadValue;
		private System.Windows.Forms.Label lblAreaValue;
		private System.Windows.Forms.Label lblCoolLoadValue;
		private System.Windows.Forms.Label lblRoomName;
		private System.Windows.Forms.Label lblRoomData;
		private System.Windows.Forms.GroupBox grpBoxSystems;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.DataGridView dgvProducts;
		private System.Windows.Forms.BindingSource plannedProductWrapperBindingSource;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnWhatIsNext;
		private System.Windows.Forms.DataGridViewTextBoxColumn colType;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSystem;
		private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
		private NumericColumn colFloorArea;
		private NumericColumn colPlannedArea;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPlannedHeatLoad;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPlannedCoolLoad;
		private System.Windows.Forms.DataGridViewButtonColumn colEdit;
	}
}
