namespace Europlan.Common {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FloorListSummaryPanel));
			this.gridFloors = new System.Windows.Forms.DataGridView();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colView = new System.Windows.Forms.DataGridViewButtonColumn();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.associatedPanelTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.projectFloorsSource = new System.Windows.Forms.BindingSource(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.btnWhatIsNext = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.gridFloors)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.projectFloorsSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
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
            this.nameDataGridViewTextBoxColumn,
            this.colView,
            this.idDataGridViewTextBoxColumn,
            this.associatedPanelTypeDataGridViewTextBoxColumn});
			this.gridFloors.DataMember = "Floors";
			this.gridFloors.DataSource = this.projectFloorsSource;
			this.gridFloors.Location = new System.Drawing.Point(3, 35);
			this.gridFloors.MultiSelect = false;
			this.gridFloors.Name = "gridFloors";
			this.gridFloors.Size = new System.Drawing.Size(672, 277);
			this.gridFloors.TabIndex = 0;
			this.gridFloors.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFloors_CellValueChanged);
			this.gridFloors.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridFloors_UserDeletingRow);
			this.gridFloors.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gridFloors_RowPrePaint);
			this.gridFloors.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridFloors_UserDeletedRow);
			this.gridFloors.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFloors_CellClick);
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.FillWeight = 200F;
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.nameDataGridViewTextBoxColumn.ToolTipText = "Bezeichnung des Geschoﬂes";
			this.nameDataGridViewTextBoxColumn.Width = 200;
			// 
			// colView
			// 
			this.colView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colView.FillWeight = 64F;
			this.colView.HeaderText = "Bearbeiten";
			this.colView.Name = "colView";
			this.colView.ReadOnly = true;
			this.colView.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colView.Text = "...";
			this.colView.UseColumnTextForButtonValue = true;
			this.colView.Width = 64;
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
			this.projectFloorsSource.DataSource = typeof(Europlan.Common.Project);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(34, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "Geschoﬂe";
			// 
			// btnWhatIsNext
			// 
			this.btnWhatIsNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWhatIsNext.Location = new System.Drawing.Point(566, 314);
			this.btnWhatIsNext.Name = "btnWhatIsNext";
			this.btnWhatIsNext.Size = new System.Drawing.Size(109, 23);
			this.btnWhatIsNext.TabIndex = 1;
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
			this.pictureBox1.Location = new System.Drawing.Point(3, 0);
			this.pictureBox1.MaximumSize = new System.Drawing.Size(32, 32);
			this.pictureBox1.MinimumSize = new System.Drawing.Size(32, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 76;
			this.pictureBox1.TabStop = false;
			// 
			// FloorListSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnWhatIsNext);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridFloors);
			this.helpProvider.SetHelpKeyword(this, "html\\euro6ed3.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.Name = "FloorListSummaryPanel";
			this.helpProvider.SetShowHelp(this, true);
			this.Size = new System.Drawing.Size(678, 340);
			((System.ComponentModel.ISupportInitialize)(this.gridFloors)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.projectFloorsSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gridFloors;
		private System.Windows.Forms.BindingSource projectFloorsSource;
		private System.Windows.Forms.DataGridViewImageColumn associatedIconDataGridViewImageColumn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnWhatIsNext;
		private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewButtonColumn colView;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn associatedPanelTypeDataGridViewTextBoxColumn;
	}
}
