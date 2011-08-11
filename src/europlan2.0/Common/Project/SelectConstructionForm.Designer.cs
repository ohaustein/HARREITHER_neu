namespace Europlan.Common {
	partial class SelectConstructionForm {
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
			this.dgvConstructions = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.rValueDataGridViewTextBoxColumn = new Europlan.Common.NumericColumn();
			this.constructionBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvConstructions)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.constructionBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvConstructions
			// 
			this.dgvConstructions.AutoGenerateColumns = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvConstructions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvConstructions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvConstructions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.rValueDataGridViewTextBoxColumn});
			this.dgvConstructions.DataSource = this.constructionBindingSource;
			this.dgvConstructions.Location = new System.Drawing.Point(12, 12);
			this.dgvConstructions.Name = "dgvConstructions";
			this.dgvConstructions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvConstructions.Size = new System.Drawing.Size(492, 240);
			this.dgvConstructions.TabIndex = 0;
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.FillWeight = 50F;
			this.idDataGridViewTextBoxColumn.HeaderText = "Nr.";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.ReadOnly = true;
			this.idDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.idDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.idDataGridViewTextBoxColumn.Width = 50;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "LocalizedName";
			this.nameDataGridViewTextBoxColumn.FillWeight = 170F;
			this.nameDataGridViewTextBoxColumn.HeaderText = "Bezeichnung";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.ReadOnly = true;
			this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.nameDataGridViewTextBoxColumn.Width = 170;
			// 
			// typeDataGridViewTextBoxColumn
			// 
			this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
			this.typeDataGridViewTextBoxColumn.FillWeight = 150F;
			this.typeDataGridViewTextBoxColumn.HeaderText = "Typ";
			this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
			this.typeDataGridViewTextBoxColumn.ReadOnly = true;
			this.typeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.typeDataGridViewTextBoxColumn.Width = 150;
			// 
			// rValueDataGridViewTextBoxColumn
			// 
			this.rValueDataGridViewTextBoxColumn.DataPropertyName = "RValue";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "F3";
			this.rValueDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.rValueDataGridViewTextBoxColumn.FillWeight = 50F;
			this.rValueDataGridViewTextBoxColumn.HeaderText = "R";
			this.rValueDataGridViewTextBoxColumn.Name = "rValueDataGridViewTextBoxColumn";
			this.rValueDataGridViewTextBoxColumn.NumEditType = Europlan.Common.NumericBox.NumericEditType.R_VALUE;
			this.rValueDataGridViewTextBoxColumn.ReadOnly = true;
			this.rValueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.rValueDataGridViewTextBoxColumn.Width = 50;
			// 
			// constructionBindingSource
			// 
			this.constructionBindingSource.DataSource = typeof(Europlan.Common.Construction);
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(348, 258);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(429, 258);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// SelectConstructionForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(516, 293);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.dgvConstructions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SelectConstructionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Bitte wählen Sie die gewünschte Konstruktion";
			this.Load += new System.EventHandler(this.SelectConstructionForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectConstructionForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.dgvConstructions)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.constructionBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvConstructions;
		private System.Windows.Forms.BindingSource constructionBindingSource;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
		private NumericColumn rValueDataGridViewTextBoxColumn;
	}
}