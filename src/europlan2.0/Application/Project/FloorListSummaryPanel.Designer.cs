namespace Europlan.Application {
	partial class FloorListSummaryPanel {
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
			this.btnImport = new System.Windows.Forms.Button();
			this.gridFloors = new System.Windows.Forms.DataGridView();
			this.colView = new System.Windows.Forms.DataGridViewButtonColumn();
			this.associatedIconDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.associatedPanelTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.projectFloorsSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.gridFloors)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.projectFloorsSource)).BeginInit();
			this.SuspendLayout();
			// 
			// btnImport
			// 
			this.btnImport.Location = new System.Drawing.Point(4, 4);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(212, 23);
			this.btnImport.TabIndex = 0;
			this.btnImport.Text = "Geb‰ude- und Lastdaten importieren...";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// gridFloors
			// 
			this.gridFloors.AllowUserToResizeRows = false;
			this.gridFloors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridFloors.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridFloors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridFloors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridFloors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.associatedIconDataGridViewImageColumn,
            this.nameDataGridViewTextBoxColumn,
            this.colView,
            this.idDataGridViewTextBoxColumn,
            this.associatedPanelTypeDataGridViewTextBoxColumn});
			this.gridFloors.DataMember = "Floors";
			this.gridFloors.DataSource = this.projectFloorsSource;
			this.gridFloors.Location = new System.Drawing.Point(3, 33);
			this.gridFloors.MultiSelect = false;
			this.gridFloors.Name = "gridFloors";
			this.gridFloors.Size = new System.Drawing.Size(672, 297);
			this.gridFloors.TabIndex = 1;
			this.gridFloors.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFloors_CellValueChanged);
			this.gridFloors.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gridFloors_RowPrePaint);
			this.gridFloors.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridFloors_UserDeletedRow);
			this.gridFloors.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFloors_CellClick);
			// 
			// colView
			// 
			this.colView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colView.HeaderText = "Bearbeiten";
			this.colView.Name = "colView";
			this.colView.ReadOnly = true;
			this.colView.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colView.Text = "...";
			this.colView.UseColumnTextForButtonValue = true;
			this.colView.Width = 64;
			// 
			// associatedIconDataGridViewImageColumn
			// 
			this.associatedIconDataGridViewImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			this.associatedIconDataGridViewImageColumn.DataPropertyName = "AssociatedIcon";
			this.associatedIconDataGridViewImageColumn.HeaderText = "";
			this.associatedIconDataGridViewImageColumn.Name = "associatedIconDataGridViewImageColumn";
			this.associatedIconDataGridViewImageColumn.ReadOnly = true;
			this.associatedIconDataGridViewImageColumn.Visible = false;
			this.associatedIconDataGridViewImageColumn.Width = 5;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.nameDataGridViewTextBoxColumn.ToolTipText = "Bezeichnung des Geschoﬂes";
			this.nameDataGridViewTextBoxColumn.Width = 200;
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "Id";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.idDataGridViewTextBoxColumn.Visible = false;
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
			// projectFloorsSource
			// 
			this.projectFloorsSource.DataSource = typeof(Europlan.Application.Project);
			// 
			// FloorListSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridFloors);
			this.Controls.Add(this.btnImport);
			this.Name = "FloorListSummaryPanel";
			this.Size = new System.Drawing.Size(678, 333);
			((System.ComponentModel.ISupportInitialize)(this.gridFloors)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.projectFloorsSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.DataGridView gridFloors;
		private System.Windows.Forms.BindingSource projectFloorsSource;
		private System.Windows.Forms.DataGridViewImageColumn associatedIconDataGridViewImageColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewButtonColumn colView;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn associatedPanelTypeDataGridViewTextBoxColumn;
	}
}
