namespace Europlan.Common {
	partial class ProductOverviewForm {
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

		#region Windows Form Designer generated code

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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvProductOverview = new System.Windows.Forms.DataGridView();
			this.roomIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.roomNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.teilSystemDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.systemNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nrOfCircuitsDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.RimType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LayDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pipeLengthDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.totalAreaDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.druckverlustHeatDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.heatNetLoadDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.heatRestDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.druckverlustCoolDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.coolNetLoadDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.coolRestDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.okDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.editColumn = new System.Windows.Forms.DataGridViewButtonColumn();
			this.productOverviewWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			((System.ComponentModel.ISupportInitialize)(this.dgvProductOverview)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.productOverviewWrapperBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvProductOverview
			// 
			this.dgvProductOverview.AllowUserToAddRows = false;
			this.dgvProductOverview.AllowUserToDeleteRows = false;
			this.dgvProductOverview.AllowUserToResizeRows = false;
			this.dgvProductOverview.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvProductOverview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvProductOverview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvProductOverview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.roomIdDataGridViewTextBoxColumn,
            this.roomNameDataGridViewTextBoxColumn,
            this.teilSystemDataGridViewTextBoxColumn,
            this.systemNameDataGridViewTextBoxColumn,
            this.nrOfCircuitsDataGridViewTextBoxColumn,
            this.RimType,
            this.LayDistance,
            this.pipeLengthDataGridViewTextBoxColumn,
            this.totalAreaDataGridViewTextBoxColumn,
            this.druckverlustHeatDataGridViewTextBoxColumn,
            this.heatNetLoadDataGridViewTextBoxColumn,
            this.heatRestDataGridViewTextBoxColumn,
            this.druckverlustCoolDataGridViewTextBoxColumn,
            this.coolNetLoadDataGridViewTextBoxColumn,
            this.coolRestDataGridViewTextBoxColumn,
            this.okDataGridViewCheckBoxColumn,
            this.editColumn});
			this.dgvProductOverview.DataSource = this.productOverviewWrapperBindingSource;
			this.dgvProductOverview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvProductOverview.Location = new System.Drawing.Point(0, 0);
			this.dgvProductOverview.MultiSelect = false;
			this.dgvProductOverview.Name = "dgvProductOverview";
			this.dgvProductOverview.RowHeadersVisible = false;
			this.dgvProductOverview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dgvProductOverview.Size = new System.Drawing.Size(710, 341);
			this.dgvProductOverview.TabIndex = 0;
			this.dgvProductOverview.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductOverview_CellValueChanged);
			this.dgvProductOverview.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductOverview_CellLeave);
			this.dgvProductOverview.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvProductOverview_PreviewKeyDown);
			this.dgvProductOverview.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvProductOverview_RowsAdded);
			this.dgvProductOverview.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvProductOverview_CellPainting);
			this.dgvProductOverview.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductOverview_CellClick);
			this.dgvProductOverview.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductOverview_CellEnter);
			// 
			// roomIdDataGridViewTextBoxColumn
			// 
			this.roomIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.roomIdDataGridViewTextBoxColumn.DataPropertyName = "RoomId";
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			this.roomIdDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.roomIdDataGridViewTextBoxColumn.HeaderText = "Raumnr.";
			this.roomIdDataGridViewTextBoxColumn.Name = "roomIdDataGridViewTextBoxColumn";
			this.roomIdDataGridViewTextBoxColumn.ReadOnly = true;
			this.roomIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.roomIdDataGridViewTextBoxColumn.Width = 53;
			// 
			// roomNameDataGridViewTextBoxColumn
			// 
			this.roomNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.roomNameDataGridViewTextBoxColumn.DataPropertyName = "RoomName";
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			this.roomNameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.roomNameDataGridViewTextBoxColumn.HeaderText = "Raumname";
			this.roomNameDataGridViewTextBoxColumn.Name = "roomNameDataGridViewTextBoxColumn";
			this.roomNameDataGridViewTextBoxColumn.ReadOnly = true;
			this.roomNameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.roomNameDataGridViewTextBoxColumn.Width = 67;
			// 
			// teilSystemDataGridViewTextBoxColumn
			// 
			this.teilSystemDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.teilSystemDataGridViewTextBoxColumn.DataPropertyName = "TeilSystem";
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
			this.teilSystemDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
			this.teilSystemDataGridViewTextBoxColumn.HeaderText = "Teil-\nsystem";
			this.teilSystemDataGridViewTextBoxColumn.Name = "teilSystemDataGridViewTextBoxColumn";
			this.teilSystemDataGridViewTextBoxColumn.ReadOnly = true;
			this.teilSystemDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.teilSystemDataGridViewTextBoxColumn.Width = 45;
			// 
			// systemNameDataGridViewTextBoxColumn
			// 
			this.systemNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.systemNameDataGridViewTextBoxColumn.DataPropertyName = "SystemName";
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
			this.systemNameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
			this.systemNameDataGridViewTextBoxColumn.HeaderText = "System";
			this.systemNameDataGridViewTextBoxColumn.Name = "systemNameDataGridViewTextBoxColumn";
			this.systemNameDataGridViewTextBoxColumn.ReadOnly = true;
			this.systemNameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.systemNameDataGridViewTextBoxColumn.Width = 47;
			// 
			// nrOfCircuitsDataGridViewTextBoxColumn
			// 
			this.nrOfCircuitsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.nrOfCircuitsDataGridViewTextBoxColumn.DataPropertyName = "NrOfCircuits";
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "F0";
			this.nrOfCircuitsDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
			this.nrOfCircuitsDataGridViewTextBoxColumn.HeaderText = "Heiz-\nkreis(e)";
			this.nrOfCircuitsDataGridViewTextBoxColumn.Name = "nrOfCircuitsDataGridViewTextBoxColumn";
			this.nrOfCircuitsDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.HK_COUNT;
			this.nrOfCircuitsDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.nrOfCircuitsDataGridViewTextBoxColumn.Width = 47;
			// 
			// RimType
			// 
			this.RimType.DataPropertyName = "RimType";
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RimType.DefaultCellStyle = dataGridViewCellStyle7;
			this.RimType.HeaderText = "Verlegeabstand\nRZ";
			this.RimType.Name = "RimType";
			this.RimType.ReadOnly = true;
			// 
			// LayDistance
			// 
			this.LayDistance.DataPropertyName = "LayDistance";
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.LayDistance.DefaultCellStyle = dataGridViewCellStyle8;
			this.LayDistance.HeaderText = "Verlegeabstand\nAZ";
			this.LayDistance.Name = "LayDistance";
			this.LayDistance.ReadOnly = true;
			// 
			// pipeLengthDataGridViewTextBoxColumn
			// 
			this.pipeLengthDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.pipeLengthDataGridViewTextBoxColumn.DataPropertyName = "PipeLength";
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle9.Format = "F1";
			this.pipeLengthDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
			this.pipeLengthDataGridViewTextBoxColumn.HeaderText = "Rohrlänge\nm";
			this.pipeLengthDataGridViewTextBoxColumn.Name = "pipeLengthDataGridViewTextBoxColumn";
			this.pipeLengthDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PIPE_LENGTH;
			this.pipeLengthDataGridViewTextBoxColumn.ReadOnly = true;
			this.pipeLengthDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.pipeLengthDataGridViewTextBoxColumn.Width = 62;
			// 
			// totalAreaDataGridViewTextBoxColumn
			// 
			this.totalAreaDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.totalAreaDataGridViewTextBoxColumn.DataPropertyName = "TotalArea";
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle10.Format = "F1";
			this.totalAreaDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle10;
			this.totalAreaDataGridViewTextBoxColumn.HeaderText = "Fläche\nm²";
			this.totalAreaDataGridViewTextBoxColumn.Name = "totalAreaDataGridViewTextBoxColumn";
			this.totalAreaDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_AREA;
			this.totalAreaDataGridViewTextBoxColumn.ReadOnly = true;
			this.totalAreaDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.totalAreaDataGridViewTextBoxColumn.Width = 45;
			// 
			// druckverlustHeatDataGridViewTextBoxColumn
			// 
			this.druckverlustHeatDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.druckverlustHeatDataGridViewTextBoxColumn.DataPropertyName = "DruckverlustHeat";
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle11.Format = "F1";
			this.druckverlustHeatDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle11;
			this.druckverlustHeatDataGridViewTextBoxColumn.HeaderText = "Druckverlust\nmbar";
			this.druckverlustHeatDataGridViewTextBoxColumn.Name = "druckverlustHeatDataGridViewTextBoxColumn";
			this.druckverlustHeatDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PIPE_LENGTH;
			this.druckverlustHeatDataGridViewTextBoxColumn.ReadOnly = true;
			this.druckverlustHeatDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.druckverlustHeatDataGridViewTextBoxColumn.Width = 73;
			// 
			// heatNetLoadDataGridViewTextBoxColumn
			// 
			this.heatNetLoadDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.heatNetLoadDataGridViewTextBoxColumn.DataPropertyName = "HeatNetLoad";
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle12.Format = "F0";
			this.heatNetLoadDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle12;
			this.heatNetLoadDataGridViewTextBoxColumn.HeaderText = "Normwärme\nW";
			this.heatNetLoadDataGridViewTextBoxColumn.Name = "heatNetLoadDataGridViewTextBoxColumn";
			this.heatNetLoadDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.heatNetLoadDataGridViewTextBoxColumn.ReadOnly = true;
			this.heatNetLoadDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.heatNetLoadDataGridViewTextBoxColumn.Width = 69;
			// 
			// heatRestDataGridViewTextBoxColumn
			// 
			this.heatRestDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.heatRestDataGridViewTextBoxColumn.DataPropertyName = "HeatRest";
			dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle13.Format = "F0";
			this.heatRestDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle13;
			this.heatRestDataGridViewTextBoxColumn.HeaderText = "Restwärme\nW";
			this.heatRestDataGridViewTextBoxColumn.Name = "heatRestDataGridViewTextBoxColumn";
			this.heatRestDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_HEAT_POWER;
			this.heatRestDataGridViewTextBoxColumn.ReadOnly = true;
			this.heatRestDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.heatRestDataGridViewTextBoxColumn.Width = 66;
			// 
			// druckverlustCoolDataGridViewTextBoxColumn
			// 
			this.druckverlustCoolDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.druckverlustCoolDataGridViewTextBoxColumn.DataPropertyName = "DruckverlustCool";
			dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle14.Format = "F1";
			this.druckverlustCoolDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle14;
			this.druckverlustCoolDataGridViewTextBoxColumn.HeaderText = "Druckverlust\nmbar";
			this.druckverlustCoolDataGridViewTextBoxColumn.Name = "druckverlustCoolDataGridViewTextBoxColumn";
			this.druckverlustCoolDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.PIPE_LENGTH;
			this.druckverlustCoolDataGridViewTextBoxColumn.ReadOnly = true;
			this.druckverlustCoolDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.druckverlustCoolDataGridViewTextBoxColumn.Width = 73;
			// 
			// coolNetLoadDataGridViewTextBoxColumn
			// 
			this.coolNetLoadDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.coolNetLoadDataGridViewTextBoxColumn.DataPropertyName = "CoolNetLoad";
			dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle15.Format = "F0";
			this.coolNetLoadDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle15;
			this.coolNetLoadDataGridViewTextBoxColumn.HeaderText = "Kühllast\nW";
			this.coolNetLoadDataGridViewTextBoxColumn.Name = "coolNetLoadDataGridViewTextBoxColumn";
			this.coolNetLoadDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.coolNetLoadDataGridViewTextBoxColumn.ReadOnly = true;
			this.coolNetLoadDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.coolNetLoadDataGridViewTextBoxColumn.Width = 50;
			// 
			// coolRestDataGridViewTextBoxColumn
			// 
			this.coolRestDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.coolRestDataGridViewTextBoxColumn.DataPropertyName = "CoolRest";
			dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle16.Format = "F0";
			this.coolRestDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle16;
			this.coolRestDataGridViewTextBoxColumn.HeaderText = "Rest\nW";
			this.coolRestDataGridViewTextBoxColumn.Name = "coolRestDataGridViewTextBoxColumn";
			this.coolRestDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.ROOM_COOL_POWER;
			this.coolRestDataGridViewTextBoxColumn.ReadOnly = true;
			this.coolRestDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.coolRestDataGridViewTextBoxColumn.Width = 35;
			// 
			// okDataGridViewCheckBoxColumn
			// 
			this.okDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.okDataGridViewCheckBoxColumn.DataPropertyName = "Ok";
			dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle17.NullValue = false;
			this.okDataGridViewCheckBoxColumn.DefaultCellStyle = dataGridViewCellStyle17;
			this.okDataGridViewCheckBoxColumn.HeaderText = "Ok";
			this.okDataGridViewCheckBoxColumn.Name = "okDataGridViewCheckBoxColumn";
			this.okDataGridViewCheckBoxColumn.ReadOnly = true;
			this.okDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.okDataGridViewCheckBoxColumn.Width = 27;
			// 
			// editColumn
			// 
			this.editColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.editColumn.HeaderText = "Bearbeiten";
			this.editColumn.Name = "editColumn";
			this.editColumn.Text = "...";
			this.editColumn.UseColumnTextForButtonValue = true;
			this.editColumn.Width = 64;
			// 
			// productOverviewWrapperBindingSource
			// 
			this.productOverviewWrapperBindingSource.DataSource = typeof(Europlan.Common.ProductOverviewWrapper);
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// ProductOverviewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(710, 341);
			this.Controls.Add(this.dgvProductOverview);
			this.helpProvider.SetHelpKeyword(this, "html\\euro3q9d.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProductOverviewForm";
			this.helpProvider.SetShowHelp(this, true);
			this.Text = "Übersicht";
			this.Load += new System.EventHandler(this.ProductOverviewForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProductOverviewForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.dgvProductOverview)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.productOverviewWrapperBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.BindingSource productOverviewWrapperBindingSource;
		private System.Windows.Forms.DataGridView dgvProductOverview;
		private System.Windows.Forms.DataGridViewTextBoxColumn roomIdDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn roomNameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn teilSystemDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn systemNameDataGridViewTextBoxColumn;
		private NumericColumn nrOfCircuitsDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn RimType;
		private System.Windows.Forms.DataGridViewTextBoxColumn LayDistance;
		private NumericColumn pipeLengthDataGridViewTextBoxColumn;
		private NumericColumn totalAreaDataGridViewTextBoxColumn;
		private NumericColumn druckverlustHeatDataGridViewTextBoxColumn;
		private NumericColumn heatNetLoadDataGridViewTextBoxColumn;
		private NumericColumn heatRestDataGridViewTextBoxColumn;
		private NumericColumn druckverlustCoolDataGridViewTextBoxColumn;
		private NumericColumn coolNetLoadDataGridViewTextBoxColumn;
		private NumericColumn coolRestDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn okDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewButtonColumn editColumn;
		private System.Windows.Forms.HelpProvider helpProvider;


	}
}