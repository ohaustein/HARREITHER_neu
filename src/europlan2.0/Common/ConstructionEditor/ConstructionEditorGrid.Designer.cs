namespace Europlan.Common {
	partial class ConstructionEditorGrid {
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
			this.panBottom = new System.Windows.Forms.Panel();
			this.btnView = new System.Windows.Forms.Button();
			this.btnNew = new System.Windows.Forms.Button();
			this.gridConstructions = new System.Windows.Forms.DataGridView();
			this.cmsView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiFloorConstruction = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiInsulationConstruction = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsNew = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiNewFloorConstruction = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiNewInsulationConstruction = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsNewAdmin = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiNewFloorConstructionScreedAdmin = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiNewFloorConstructionDryAdmin = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiNewInsulationConstructionAdmin = new System.Windows.Forms.ToolStripMenuItem();
			this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
			this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.thicknessDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colScope = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.constructionsWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panBottom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridConstructions)).BeginInit();
			this.cmsView.SuspendLayout();
			this.cmsNew.SuspendLayout();
			this.cmsNewAdmin.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.constructionsWrapperBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// panBottom
			// 
			this.panBottom.Controls.Add(this.btnView);
			this.panBottom.Controls.Add(this.btnNew);
			this.panBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panBottom.Location = new System.Drawing.Point(0, 398);
			this.panBottom.Name = "panBottom";
			this.panBottom.Size = new System.Drawing.Size(704, 26);
			this.panBottom.TabIndex = 2;
			// 
			// btnView
			// 
			this.btnView.Location = new System.Drawing.Point(120, 3);
			this.btnView.Name = "btnView";
			this.btnView.Size = new System.Drawing.Size(231, 23);
			this.btnView.TabIndex = 1;
			this.btnView.Text = "Angezeigte Konstruktionen (Alle)";
			this.btnView.UseVisualStyleBackColor = true;
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(0, 3);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(114, 23);
			this.btnNew.TabIndex = 0;
			this.btnNew.Text = "Neue Konstruktion";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// gridConstructions
			// 
			this.gridConstructions.AllowUserToAddRows = false;
			this.gridConstructions.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridConstructions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridConstructions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridConstructions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colName,
            this.colType,
            this.thicknessDataGridViewTextBoxColumn,
            this.colRValue,
            this.colEdit,
            this.colScope});
			this.gridConstructions.DataSource = this.constructionsWrapperBindingSource;
			this.gridConstructions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridConstructions.Location = new System.Drawing.Point(0, 0);
			this.gridConstructions.Name = "gridConstructions";
			this.gridConstructions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridConstructions.Size = new System.Drawing.Size(704, 398);
			this.gridConstructions.TabIndex = 3;
			this.gridConstructions.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridConstructions_UserDeletingRow);
			this.gridConstructions.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.gridConstructions_RowsAdded);
			this.gridConstructions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridConstructions_CellClick);
			// 
			// cmsView
			// 
			this.cmsView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFloorConstruction,
            this.tsmiInsulationConstruction});
			this.cmsView.Name = "cmsView";
			this.cmsView.Size = new System.Drawing.Size(228, 48);
			// 
			// tsmiFloorConstruction
			// 
			this.tsmiFloorConstruction.Checked = true;
			this.tsmiFloorConstruction.CheckOnClick = true;
			this.tsmiFloorConstruction.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmiFloorConstruction.Name = "tsmiFloorConstruction";
			this.tsmiFloorConstruction.Size = new System.Drawing.Size(227, 22);
			this.tsmiFloorConstruction.Text = "Fuﬂbodenkonstruktionen";
			this.tsmiFloorConstruction.Click += new System.EventHandler(this.cmsViewItem_Click);
			// 
			// tsmiInsulationConstruction
			// 
			this.tsmiInsulationConstruction.Checked = true;
			this.tsmiInsulationConstruction.CheckOnClick = true;
			this.tsmiInsulationConstruction.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmiInsulationConstruction.Name = "tsmiInsulationConstruction";
			this.tsmiInsulationConstruction.Size = new System.Drawing.Size(227, 22);
			this.tsmiInsulationConstruction.Text = "W‰rmed‰mmkonstruktionen";
			this.tsmiInsulationConstruction.Click += new System.EventHandler(this.cmsViewItem_Click);
			// 
			// cmsNew
			// 
			this.cmsNew.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewFloorConstruction,
            this.tsmiNewInsulationConstruction});
			this.cmsNew.Name = "cmsNew";
			this.cmsNew.Size = new System.Drawing.Size(215, 48);
			// 
			// tsmiNewFloorConstruction
			// 
			this.tsmiNewFloorConstruction.Name = "tsmiNewFloorConstruction";
			this.tsmiNewFloorConstruction.Size = new System.Drawing.Size(214, 22);
			this.tsmiNewFloorConstruction.Text = "Fuﬂbodenkonstruktion";
			this.tsmiNewFloorConstruction.Click += new System.EventHandler(this.tsmiNewConstruction_Click);
			// 
			// tsmiNewInsulationConstruction
			// 
			this.tsmiNewInsulationConstruction.Name = "tsmiNewInsulationConstruction";
			this.tsmiNewInsulationConstruction.Size = new System.Drawing.Size(214, 22);
			this.tsmiNewInsulationConstruction.Text = "W‰rmed‰mmkonstruktion";
			this.tsmiNewInsulationConstruction.Click += new System.EventHandler(this.tsmiNewConstruction_Click);
			// 
			// cmsNewAdmin
			// 
			this.cmsNewAdmin.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewFloorConstructionScreedAdmin,
            this.tsmiNewFloorConstructionDryAdmin,
            this.tsmiNewInsulationConstructionAdmin});
			this.cmsNewAdmin.Name = "cmsNew";
			this.cmsNewAdmin.Size = new System.Drawing.Size(250, 70);
			// 
			// tsmiNewFloorConstructionScreedAdmin
			// 
			this.tsmiNewFloorConstructionScreedAdmin.Name = "tsmiNewFloorConstructionScreedAdmin";
			this.tsmiNewFloorConstructionScreedAdmin.Size = new System.Drawing.Size(249, 22);
			this.tsmiNewFloorConstructionScreedAdmin.Text = "Fuﬂbodenkonstruktion (Estrich)";
			this.tsmiNewFloorConstructionScreedAdmin.Click += new System.EventHandler(this.tsmiNewConstructionAdmin_Click);
			// 
			// tsmiNewFloorConstructionDryAdmin
			// 
			this.tsmiNewFloorConstructionDryAdmin.Name = "tsmiNewFloorConstructionDryAdmin";
			this.tsmiNewFloorConstructionDryAdmin.Size = new System.Drawing.Size(249, 22);
			this.tsmiNewFloorConstructionDryAdmin.Text = "Fuﬂbodenkonstruktion (Trocken)";
			this.tsmiNewFloorConstructionDryAdmin.Click += new System.EventHandler(this.tsmiNewConstructionAdmin_Click);
			// 
			// tsmiNewInsulationConstructionAdmin
			// 
			this.tsmiNewInsulationConstructionAdmin.Name = "tsmiNewInsulationConstructionAdmin";
			this.tsmiNewInsulationConstructionAdmin.Size = new System.Drawing.Size(249, 22);
			this.tsmiNewInsulationConstructionAdmin.Text = "W‰rmed‰mmkonstruktion";
			this.tsmiNewInsulationConstructionAdmin.Click += new System.EventHandler(this.tsmiNewConstructionAdmin_Click);
			// 
			// colEdit
			// 
			this.colEdit.FillWeight = 65F;
			this.colEdit.HeaderText = "Bearbeiten";
			this.colEdit.Name = "colEdit";
			this.colEdit.ReadOnly = true;
			this.colEdit.Text = "...";
			this.colEdit.UseColumnTextForButtonValue = true;
			this.colEdit.Width = 65;
			// 
			// colId
			// 
			this.colId.DataPropertyName = "Id";
			this.colId.FillWeight = 50F;
			this.colId.HeaderText = "Nr.";
			this.colId.Name = "colId";
			this.colId.ReadOnly = true;
			this.colId.Width = 50;
			// 
			// colName
			// 
			this.colName.DataPropertyName = "Name";
			this.colName.FillWeight = 150F;
			this.colName.HeaderText = "Bezeichnung";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			this.colName.Width = 150;
			// 
			// colType
			// 
			this.colType.DataPropertyName = "Type";
			this.colType.FillWeight = 150F;
			this.colType.HeaderText = "Type";
			this.colType.Name = "colType";
			this.colType.ReadOnly = true;
			this.colType.Width = 150;
			// 
			// thicknessDataGridViewTextBoxColumn
			// 
			this.thicknessDataGridViewTextBoxColumn.DataPropertyName = "Thickness";
			this.thicknessDataGridViewTextBoxColumn.HeaderText = "Thickness";
			this.thicknessDataGridViewTextBoxColumn.Name = "thicknessDataGridViewTextBoxColumn";
			this.thicknessDataGridViewTextBoxColumn.ReadOnly = true;
			this.thicknessDataGridViewTextBoxColumn.Visible = false;
			// 
			// colRValue
			// 
			this.colRValue.DataPropertyName = "RValue";
			this.colRValue.FillWeight = 50F;
			this.colRValue.HeaderText = "R (m≤K/W)";
			this.colRValue.Name = "colRValue";
			this.colRValue.ReadOnly = true;
			this.colRValue.Width = 50;
			// 
			// colScope
			// 
			this.colScope.DataPropertyName = "Scope";
			this.colScope.HeaderText = "Scope";
			this.colScope.Name = "colScope";
			this.colScope.ReadOnly = true;
			this.colScope.Visible = false;
			// 
			// constructionsWrapperBindingSource
			// 
			this.constructionsWrapperBindingSource.DataSource = typeof(Europlan.Common.ConstructionListWrapper);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
			this.dataGridViewTextBoxColumn1.FillWeight = 50F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Nr.";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Visible = false;
			this.dataGridViewTextBoxColumn1.Width = 50;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn2.FillWeight = 150F;
			this.dataGridViewTextBoxColumn2.HeaderText = "Bezeichnung";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Width = 150;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn3.FillWeight = 150F;
			this.dataGridViewTextBoxColumn3.HeaderText = "Type";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 150;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.DataPropertyName = "Thickness";
			this.dataGridViewTextBoxColumn4.HeaderText = "Thickness";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.Visible = false;
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.DataPropertyName = "RValue";
			this.dataGridViewTextBoxColumn5.FillWeight = 50F;
			this.dataGridViewTextBoxColumn5.HeaderText = "R (m≤K/W)";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Width = 50;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.DataPropertyName = "Scope";
			this.dataGridViewTextBoxColumn6.HeaderText = "Scope";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.Visible = false;
			// 
			// ConstructionEditorGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridConstructions);
			this.Controls.Add(this.panBottom);
			this.Name = "ConstructionEditorGrid";
			this.Size = new System.Drawing.Size(704, 424);
			this.panBottom.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridConstructions)).EndInit();
			this.cmsView.ResumeLayout(false);
			this.cmsNew.ResumeLayout(false);
			this.cmsNewAdmin.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.constructionsWrapperBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.BindingSource constructionsWrapperBindingSource;
		private System.Windows.Forms.Panel panBottom;
		private System.Windows.Forms.Button btnView;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridView gridConstructions;
		private System.Windows.Forms.ContextMenuStrip cmsView;
		private System.Windows.Forms.ToolStripMenuItem tsmiFloorConstruction;
		private System.Windows.Forms.ToolStripMenuItem tsmiInsulationConstruction;
		private System.Windows.Forms.ContextMenuStrip cmsNew;
		private System.Windows.Forms.ToolStripMenuItem tsmiNewFloorConstruction;
		private System.Windows.Forms.ToolStripMenuItem tsmiNewInsulationConstruction;
		private System.Windows.Forms.ContextMenuStrip cmsNewAdmin;
		private System.Windows.Forms.ToolStripMenuItem tsmiNewFloorConstructionScreedAdmin;
		private System.Windows.Forms.ToolStripMenuItem tsmiNewInsulationConstructionAdmin;
		private System.Windows.Forms.ToolStripMenuItem tsmiNewFloorConstructionDryAdmin;
		private System.Windows.Forms.DataGridViewTextBoxColumn colId;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colType;
		private System.Windows.Forms.DataGridViewTextBoxColumn thicknessDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn colRValue;
		private System.Windows.Forms.DataGridViewButtonColumn colEdit;
		private System.Windows.Forms.DataGridViewTextBoxColumn colScope;

	}
}
