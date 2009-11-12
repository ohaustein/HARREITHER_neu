namespace Europlan.Common {
	partial class QuickDimensioningDistributorGrid {
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
			this.gridDistributors = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.quickDimensioningDistributorBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.gridDistributors)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.quickDimensioningDistributorBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridDistributors
			// 
			this.gridDistributors.AutoGenerateColumns = false;
			this.gridDistributors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridDistributors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn});
			this.gridDistributors.DataSource = this.quickDimensioningDistributorBindingSource;
			this.gridDistributors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridDistributors.Location = new System.Drawing.Point(0, 0);
			this.gridDistributors.MultiSelect = false;
			this.gridDistributors.Name = "gridDistributors";
			this.gridDistributors.Size = new System.Drawing.Size(412, 219);
			this.gridDistributors.TabIndex = 0;
			this.gridDistributors.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridDistributors_UserDeletingRow);
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "Id";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.Visible = false;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			// 
			// quickDimensioningDistributorBindingSource
			// 
			this.quickDimensioningDistributorBindingSource.DataSource = typeof(Europlan.Common.Distributor);
			// 
			// QuickDimensioningDistributorGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridDistributors);
			this.Name = "QuickDimensioningDistributorGrid";
			this.Size = new System.Drawing.Size(412, 219);
			((System.ComponentModel.ISupportInitialize)(this.gridDistributors)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.quickDimensioningDistributorBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridDistributors;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.BindingSource quickDimensioningDistributorBindingSource;
	}
}
