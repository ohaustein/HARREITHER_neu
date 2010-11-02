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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FloorSummaryPanel));
			this.gridRooms = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Area = new Europlan.Common.NumericColumn();
			this.IsNassraum = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.RoomTemperature = new Europlan.Common.NumericColumn();
			this.HeatLoad = new Europlan.Common.NumericColumn();
			this.FloorHeatingLoss = new Europlan.Common.NumericColumn();
			this.AdditionalHeatLoad = new Europlan.Common.NumericColumn();
			this.RoomCoolTemperature = new Europlan.Common.NumericColumn();
			this.RoomRelativeHumidity = new Europlan.Common.NumericColumn();
			this.CoolLoad = new Europlan.Common.NumericColumn();
			this.associatedPanelTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colView = new System.Windows.Forms.DataGridViewButtonColumn();
			this.floorRoomsSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnAddDistributor = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lblFloorName = new System.Windows.Forms.Label();
			this.btnRemoveDistributor = new System.Windows.Forms.Button();
			this.btnWhatIsNext = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.chkAssignPlan = new System.Windows.Forms.CheckBox();
			this.cmbPlans = new System.Windows.Forms.ComboBox();
			this.planBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.floorRoomsSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.planBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridRooms
			// 
			this.gridRooms.AllowUserToResizeRows = false;
			this.gridRooms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridRooms.AutoGenerateColumns = false;
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridRooms.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
			this.gridRooms.ColumnHeadersHeight = 55;
			this.gridRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.gridRooms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.Area,
            this.IsNassraum,
            this.RoomTemperature,
            this.HeatLoad,
            this.FloorHeatingLoss,
            this.AdditionalHeatLoad,
            this.RoomCoolTemperature,
            this.RoomRelativeHumidity,
            this.CoolLoad,
            this.associatedPanelTypeDataGridViewTextBoxColumn,
            this.colView});
			this.gridRooms.DataSource = this.floorRoomsSource;
			this.gridRooms.Location = new System.Drawing.Point(3, 58);
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
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle11.Format = "F0";
			this.Area.DefaultCellStyle = dataGridViewCellStyle11;
			this.Area.HeaderText = "A\n(m²)";
			this.Area.Name = "Area";
			this.Area.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.Area.ToolTipText = "Raumfläche";
			this.Area.Width = 50;
			// 
			// IsNassraum
			// 
			this.IsNassraum.DataPropertyName = "IsNassraum";
			this.IsNassraum.HeaderText = "Nassraum";
			this.IsNassraum.Name = "IsNassraum";
			this.IsNassraum.Width = 60;
			// 
			// RoomTemperature
			// 
			this.RoomTemperature.DataPropertyName = "RoomHeatTemperature";
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle12.Format = "F0";
			this.RoomTemperature.DefaultCellStyle = dataGridViewCellStyle12;
			this.RoomTemperature.HeaderText = "Ti\n(°C)";
			this.RoomTemperature.Name = "RoomTemperature";
			this.RoomTemperature.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_TEMPERATURE;
			this.RoomTemperature.ToolTipText = "Norminnentemperatur laut Wärmebedarfsberechnung";
			this.RoomTemperature.Width = 40;
			// 
			// HeatLoad
			// 
			this.HeatLoad.DataPropertyName = "HeatLoad";
			dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle13.Format = "F0";
			this.HeatLoad.DefaultCellStyle = dataGridViewCellStyle13;
			this.HeatLoad.HeaderText = "QN\n(W)";
			this.HeatLoad.Name = "HeatLoad";
			this.HeatLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.HeatLoad.ToolTipText = "Normwärmebedarf laut Wärmebedarfsrechnung";
			this.HeatLoad.Width = 50;
			// 
			// FloorHeatingLoss
			// 
			this.FloorHeatingLoss.DataPropertyName = "FloorHeatingLoss";
			dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle14.Format = "F0";
			this.FloorHeatingLoss.DefaultCellStyle = dataGridViewCellStyle14;
			this.FloorHeatingLoss.HeaderText = "QFB\n(W)";
			this.FloorHeatingLoss.Name = "FloorHeatingLoss";
			this.FloorHeatingLoss.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.FloorHeatingLoss.ToolTipText = "Im Wärmebedarf enthaltene Fußbodentransmissionen";
			this.FloorHeatingLoss.Width = 50;
			// 
			// AdditionalHeatLoad
			// 
			this.AdditionalHeatLoad.DataPropertyName = "AdditionalHeatLoad";
			dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle15.Format = "F0";
			this.AdditionalHeatLoad.DefaultCellStyle = dataGridViewCellStyle15;
			this.AdditionalHeatLoad.HeaderText = "QFr\n(W)";
			this.AdditionalHeatLoad.Name = "AdditionalHeatLoad";
			this.AdditionalHeatLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.AdditionalHeatLoad.ToolTipText = "Zusätzliche Fremdwärmeleistung";
			this.AdditionalHeatLoad.Width = 50;
			// 
			// RoomCoolTemperature
			// 
			this.RoomCoolTemperature.DataPropertyName = "RoomCoolTemperature";
			dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle16.Format = "F0";
			this.RoomCoolTemperature.DefaultCellStyle = dataGridViewCellStyle16;
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
			dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle17.Format = "F0";
			this.RoomRelativeHumidity.DefaultCellStyle = dataGridViewCellStyle17;
			this.RoomRelativeHumidity.HeaderText = "RF\n(%)";
			this.RoomRelativeHumidity.Name = "RoomRelativeHumidity";
			this.RoomRelativeHumidity.NumEditType = Europlan.Common.NumericBox.NumericEditType.PERCENTAGE;
			this.RoomRelativeHumidity.ToolTipText = "Relative Luftfeuchtigkeit für Kühlung";
			this.RoomRelativeHumidity.Visible = false;
			this.RoomRelativeHumidity.Width = 60;
			// 
			// CoolLoad
			// 
			this.CoolLoad.DataPropertyName = "CoolLoad";
			dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle18.Format = "F0";
			this.CoolLoad.DefaultCellStyle = dataGridViewCellStyle18;
			this.CoolLoad.HeaderText = "QKühl\n(W)";
			this.CoolLoad.Name = "CoolLoad";
			this.CoolLoad.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.CoolLoad.ToolTipText = "Erforderliche Külleistung laut Kühllastberechnung";
			this.CoolLoad.Visible = false;
			this.CoolLoad.Width = 65;
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
			// floorRoomsSource
			// 
			this.floorRoomsSource.DataSource = typeof(Europlan.Common.Room);
			// 
			// btnAddDistributor
			// 
			this.btnAddDistributor.Location = new System.Drawing.Point(3, 33);
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
			this.label1.Location = new System.Drawing.Point(38, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 24);
			this.label1.TabIndex = 18;
			this.label1.Text = "Geschoßdaten:";
			// 
			// lblFloorName
			// 
			this.lblFloorName.AutoSize = true;
			this.lblFloorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFloorName.Location = new System.Drawing.Point(191, 5);
			this.lblFloorName.Name = "lblFloorName";
			this.lblFloorName.Size = new System.Drawing.Size(0, 24);
			this.lblFloorName.TabIndex = 19;
			// 
			// btnRemoveDistributor
			// 
			this.btnRemoveDistributor.Location = new System.Drawing.Point(127, 33);
			this.btnRemoveDistributor.Name = "btnRemoveDistributor";
			this.btnRemoveDistributor.Size = new System.Drawing.Size(118, 23);
			this.btnRemoveDistributor.TabIndex = 20;
			this.btnRemoveDistributor.Text = "Verteiler löschen";
			this.btnRemoveDistributor.UseVisualStyleBackColor = true;
			this.btnRemoveDistributor.Click += new System.EventHandler(this.btnRemoveDistributor_Click);
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
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(4, 0);
			this.pictureBox1.MaximumSize = new System.Drawing.Size(32, 32);
			this.pictureBox1.MinimumSize = new System.Drawing.Size(32, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 77;
			this.pictureBox1.TabStop = false;
			// 
			// chkAssignPlan
			// 
			this.chkAssignPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkAssignPlan.AutoSize = true;
			this.chkAssignPlan.Enabled = false;
			this.chkAssignPlan.Location = new System.Drawing.Point(531, 12);
			this.chkAssignPlan.Name = "chkAssignPlan";
			this.chkAssignPlan.Size = new System.Drawing.Size(162, 17);
			this.chkAssignPlan.TabIndex = 78;
			this.chkAssignPlan.Text = "Plan für Geschoß vorhanden";
			this.chkAssignPlan.UseVisualStyleBackColor = true;
			this.chkAssignPlan.CheckedChanged += new System.EventHandler(this.chkAssignPlan_CheckedChanged);
			// 
			// cmbPlans
			// 
			this.cmbPlans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbPlans.DataSource = this.planBindingSource;
			this.cmbPlans.DisplayMember = "Name";
			this.cmbPlans.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPlans.Enabled = false;
			this.cmbPlans.FormattingEnabled = true;
			this.cmbPlans.Location = new System.Drawing.Point(457, 33);
			this.cmbPlans.Name = "cmbPlans";
			this.cmbPlans.Size = new System.Drawing.Size(236, 21);
			this.cmbPlans.TabIndex = 79;
			this.cmbPlans.SelectedValueChanged += new System.EventHandler(this.cmbPlans_SelectedValueChanged);
			// 
			// planBindingSource
			// 
			this.planBindingSource.DataSource = typeof(Europlan.Common.Plan);
			// 
			// FloorSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cmbPlans);
			this.Controls.Add(this.chkAssignPlan);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnWhatIsNext);
			this.Controls.Add(this.btnRemoveDistributor);
			this.Controls.Add(this.lblFloorName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnAddDistributor);
			this.Controls.Add(this.gridRooms);
			this.helpProvider.SetHelpKeyword(this, "html\\euro5mn8.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.MinimumSize = new System.Drawing.Size(696, 200);
			this.Name = "FloorSummaryPanel";
			this.helpProvider.SetShowHelp(this, true);
			this.Size = new System.Drawing.Size(696, 379);
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.floorRoomsSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.planBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gridRooms;
		private System.Windows.Forms.BindingSource floorRoomsSource;
		private System.Windows.Forms.Button btnAddDistributor;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblFloorName;
		private System.Windows.Forms.Button btnRemoveDistributor;
		private System.Windows.Forms.Button btnWhatIsNext;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private NumericColumn Area;
		private System.Windows.Forms.DataGridViewCheckBoxColumn IsNassraum;
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
        private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.CheckBox chkAssignPlan;
		private System.Windows.Forms.ComboBox cmbPlans;
		private System.Windows.Forms.BindingSource planBindingSource;
	}
}
