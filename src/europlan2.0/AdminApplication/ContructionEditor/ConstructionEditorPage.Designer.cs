namespace Europlan.AdminApplication {
	partial class ConstructionEditorPage {
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
			this.gridConstructions = new System.Windows.Forms.DataGridView();
			this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.thicknessDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
			this.colScope = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.constructionsWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridConstructions)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.constructionsWrapperBindingSource)).BeginInit();
			this.SuspendLayout();
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
			this.gridConstructions.Size = new System.Drawing.Size(704, 424);
			this.gridConstructions.TabIndex = 0;
			this.gridConstructions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.constructionsGrid_CellClick);
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
			this.colRValue.HeaderText = "R (m²K/W)";
			this.colRValue.Name = "colRValue";
			this.colRValue.ReadOnly = true;
			this.colRValue.Width = 50;
			// 
			// colEdit
			// 
			this.colEdit.FillWeight = 65F;
			this.colEdit.HeaderText = "Bearbeiten";
			this.colEdit.Name = "colEdit";
			this.colEdit.ReadOnly = true;
			this.colEdit.Width = 65;
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
			this.dataGridViewTextBoxColumn1.HeaderText = "Nr.";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn2.HeaderText = "Bezeichnung";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "Type";
			this.dataGridViewTextBoxColumn3.HeaderText = "Type";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
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
			this.dataGridViewTextBoxColumn5.HeaderText = "R (m²K/W)";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			// 
			// ConstructionEditorPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridConstructions);
			this.Name = "ConstructionEditorPage";
			this.Size = new System.Drawing.Size(704, 424);
			((System.ComponentModel.ISupportInitialize)(this.gridConstructions)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.constructionsWrapperBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridConstructions;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.BindingSource constructionsWrapperBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn colId;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colType;
		private System.Windows.Forms.DataGridViewTextBoxColumn thicknessDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn colRValue;
		private System.Windows.Forms.DataGridViewButtonColumn colEdit;
		private System.Windows.Forms.DataGridViewTextBoxColumn colScope;

	}
}
