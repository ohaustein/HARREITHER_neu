namespace Europlan.Application {
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
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.gridRooms = new System.Windows.Forms.DataGridView();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colView = new System.Windows.Forms.DataGridViewButtonColumn();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.associatedPanelTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.associatedIconDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.floorRoomsSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.floorRoomsSource)).BeginInit();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(117, 2);
			this.txtName.Name = "txtName";
			this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtName.Size = new System.Drawing.Size(464, 20);
			this.txtName.TabIndex = 15;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(3, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(108, 23);
			this.lblName.TabIndex = 14;
			this.lblName.Text = "Name:";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridRooms
			// 
			this.gridRooms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridRooms.AutoGenerateColumns = false;
			this.gridRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridRooms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.colView,
            this.idDataGridViewTextBoxColumn,
            this.associatedPanelTypeDataGridViewTextBoxColumn,
            this.associatedIconDataGridViewImageColumn});
			this.gridRooms.DataSource = this.floorRoomsSource;
			this.gridRooms.Location = new System.Drawing.Point(3, 28);
			this.gridRooms.Name = "gridRooms";
			this.gridRooms.Size = new System.Drawing.Size(578, 233);
			this.gridRooms.TabIndex = 16;
			this.gridRooms.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRooms_CellValueChanged);
			this.gridRooms.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridRooms_UserDeletedRow);
			this.gridRooms.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRooms_CellClick);
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			// 
			// colView
			// 
			this.colView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colView.DataPropertyName = "Name";
			this.colView.HeaderText = "Bearbeiten";
			this.colView.Name = "colView";
			this.colView.Text = "...";
			this.colView.UseColumnTextForButtonValue = true;
			this.colView.Width = 64;
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "Id";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			this.idDataGridViewTextBoxColumn.Visible = false;
			// 
			// associatedPanelTypeDataGridViewTextBoxColumn
			// 
			this.associatedPanelTypeDataGridViewTextBoxColumn.DataPropertyName = "AssociatedPanelType";
			this.associatedPanelTypeDataGridViewTextBoxColumn.HeaderText = "AssociatedPanelType";
			this.associatedPanelTypeDataGridViewTextBoxColumn.Name = "associatedPanelTypeDataGridViewTextBoxColumn";
			this.associatedPanelTypeDataGridViewTextBoxColumn.ReadOnly = true;
			this.associatedPanelTypeDataGridViewTextBoxColumn.Visible = false;
			// 
			// associatedIconDataGridViewImageColumn
			// 
			this.associatedIconDataGridViewImageColumn.DataPropertyName = "AssociatedIcon";
			this.associatedIconDataGridViewImageColumn.HeaderText = "AssociatedIcon";
			this.associatedIconDataGridViewImageColumn.Name = "associatedIconDataGridViewImageColumn";
			this.associatedIconDataGridViewImageColumn.ReadOnly = true;
			this.associatedIconDataGridViewImageColumn.Visible = false;
			// 
			// floorRoomsSource
			// 
			this.floorRoomsSource.DataSource = typeof(Europlan.Application.Floor);
			// 
			// FloorSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridRooms);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.Name = "FloorSummaryPanel";
			this.Size = new System.Drawing.Size(584, 264);
			((System.ComponentModel.ISupportInitialize)(this.gridRooms)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.floorRoomsSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.DataGridView gridRooms;
		private System.Windows.Forms.BindingSource floorRoomsSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewButtonColumn colView;
		private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn associatedPanelTypeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewImageColumn associatedIconDataGridViewImageColumn;
	}
}
