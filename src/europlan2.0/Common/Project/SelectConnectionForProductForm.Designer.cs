namespace Europlan.Common {
	partial class SelectConnectionForProductForm {
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
			this.tvDistributors = new System.Windows.Forms.TreeView();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.grpInfo = new System.Windows.Forms.GroupBox();
			this.lblInfo = new System.Windows.Forms.Label();
			this.grpConnection = new System.Windows.Forms.GroupBox();
			this.rbRuecklauf = new System.Windows.Forms.RadioButton();
			this.rbVorlauf = new System.Windows.Forms.RadioButton();
			this.grpUserDefinedConnection = new System.Windows.Forms.GroupBox();
			this.gridUserDefinedConnection = new System.Windows.Forms.DataGridView();
			this.hk1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.hk2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.userDefinedConnectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cbActivateUserDefinedConnection = new System.Windows.Forms.CheckBox();
			this.grpInfo.SuspendLayout();
			this.grpConnection.SuspendLayout();
			this.grpUserDefinedConnection.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridUserDefinedConnection)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.userDefinedConnectionBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// tvDistributors
			// 
			this.tvDistributors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.tvDistributors.HideSelection = false;
			this.tvDistributors.Location = new System.Drawing.Point(12, 12);
			this.tvDistributors.Name = "tvDistributors";
			this.tvDistributors.Size = new System.Drawing.Size(300, 422);
			this.tvDistributors.TabIndex = 0;
			this.tvDistributors.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDistributors_AfterSelect);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(615, 440);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Enabled = false;
			this.btnOk.Location = new System.Drawing.Point(534, 440);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// grpInfo
			// 
			this.grpInfo.Controls.Add(this.lblInfo);
			this.grpInfo.Location = new System.Drawing.Point(318, 12);
			this.grpInfo.Name = "grpInfo";
			this.grpInfo.Size = new System.Drawing.Size(372, 55);
			this.grpInfo.TabIndex = 3;
			this.grpInfo.TabStop = false;
			this.grpInfo.Text = "Information";
			// 
			// lblInfo
			// 
			this.lblInfo.Location = new System.Drawing.Point(6, 19);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(360, 29);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "";
			// 
			// grpConnection
			// 
			this.grpConnection.Controls.Add(this.rbRuecklauf);
			this.grpConnection.Controls.Add(this.rbVorlauf);
			this.grpConnection.Enabled = false;
			this.grpConnection.Location = new System.Drawing.Point(318, 73);
			this.grpConnection.Name = "grpConnection";
			this.grpConnection.Size = new System.Drawing.Size(372, 72);
			this.grpConnection.TabIndex = 4;
			this.grpConnection.TabStop = false;
			this.grpConnection.Text = "Heizkreisanschluﬂ (nur bei Anschluﬂ an anderen Heizkreis)";
			// 
			// rbRuecklauf
			// 
			this.rbRuecklauf.AutoSize = true;
			this.rbRuecklauf.Location = new System.Drawing.Point(9, 42);
			this.rbRuecklauf.Name = "rbRuecklauf";
			this.rbRuecklauf.Size = new System.Drawing.Size(87, 17);
			this.rbRuecklauf.TabIndex = 1;
			this.rbRuecklauf.Text = "r¸cklaufseitig";
			this.rbRuecklauf.UseVisualStyleBackColor = true;
			// 
			// rbVorlauf
			// 
			this.rbVorlauf.AutoSize = true;
			this.rbVorlauf.Checked = true;
			this.rbVorlauf.Location = new System.Drawing.Point(9, 19);
			this.rbVorlauf.Name = "rbVorlauf";
			this.rbVorlauf.Size = new System.Drawing.Size(81, 17);
			this.rbVorlauf.TabIndex = 0;
			this.rbVorlauf.TabStop = true;
			this.rbVorlauf.Text = "vorlaufseitig";
			this.rbVorlauf.UseVisualStyleBackColor = true;
			// 
			// grpUserDefinedConnection
			// 
			this.grpUserDefinedConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.grpUserDefinedConnection.Controls.Add(this.gridUserDefinedConnection);
			this.grpUserDefinedConnection.Controls.Add(this.cbActivateUserDefinedConnection);
			this.grpUserDefinedConnection.Enabled = false;
			this.grpUserDefinedConnection.Location = new System.Drawing.Point(318, 151);
			this.grpUserDefinedConnection.Name = "grpUserDefinedConnection";
			this.grpUserDefinedConnection.Size = new System.Drawing.Size(372, 283);
			this.grpUserDefinedConnection.TabIndex = 5;
			this.grpUserDefinedConnection.TabStop = false;
			this.grpUserDefinedConnection.Text = "Benuzerdefinierte Zuordnung (nur bei Anschluﬂ an anderen Heizkreis)";
			// 
			// gridUserDefinedConnection
			// 
			this.gridUserDefinedConnection.AllowUserToAddRows = false;
			this.gridUserDefinedConnection.AllowUserToDeleteRows = false;
			this.gridUserDefinedConnection.AllowUserToResizeRows = false;
			this.gridUserDefinedConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridUserDefinedConnection.AutoGenerateColumns = false;
			this.gridUserDefinedConnection.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridUserDefinedConnection.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridUserDefinedConnection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridUserDefinedConnection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hk1DataGridViewTextBoxColumn,
            this.hk2DataGridViewTextBoxColumn});
			this.gridUserDefinedConnection.DataSource = this.userDefinedConnectionBindingSource;
			this.gridUserDefinedConnection.Location = new System.Drawing.Point(6, 49);
			this.gridUserDefinedConnection.Name = "gridUserDefinedConnection";
			this.gridUserDefinedConnection.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.gridUserDefinedConnection.Size = new System.Drawing.Size(360, 228);
			this.gridUserDefinedConnection.TabIndex = 1;
			this.gridUserDefinedConnection.Visible = false;
			this.gridUserDefinedConnection.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridUserDefinedConnection_CellValueChanged);
			this.gridUserDefinedConnection.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridUserDefinedConnection_CurrentCellDirtyStateChanged);
			// 
			// hk1DataGridViewTextBoxColumn
			// 
			this.hk1DataGridViewTextBoxColumn.DataPropertyName = "Hk1";
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			this.hk1DataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.hk1DataGridViewTextBoxColumn.HeaderText = "";
			this.hk1DataGridViewTextBoxColumn.Name = "hk1DataGridViewTextBoxColumn";
			this.hk1DataGridViewTextBoxColumn.ReadOnly = true;
			this.hk1DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// hk2DataGridViewTextBoxColumn
			// 
			this.hk2DataGridViewTextBoxColumn.DataPropertyName = "Hk2";
			this.hk2DataGridViewTextBoxColumn.HeaderText = "";
			this.hk2DataGridViewTextBoxColumn.Name = "hk2DataGridViewTextBoxColumn";
			this.hk2DataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// userDefinedConnectionBindingSource
			// 
			this.userDefinedConnectionBindingSource.DataSource = typeof(Europlan.Common.SelectConnectionForProductForm.UserDefinedConnection);
			// 
			// cbActivateUserDefinedConnection
			// 
			this.cbActivateUserDefinedConnection.AutoSize = true;
			this.cbActivateUserDefinedConnection.Location = new System.Drawing.Point(9, 26);
			this.cbActivateUserDefinedConnection.Name = "cbActivateUserDefinedConnection";
			this.cbActivateUserDefinedConnection.Size = new System.Drawing.Size(255, 17);
			this.cbActivateUserDefinedConnection.TabIndex = 0;
			this.cbActivateUserDefinedConnection.Text = "benutzerdefinierte Heizkreiszuordnung aktivieren";
			this.cbActivateUserDefinedConnection.UseVisualStyleBackColor = true;
			this.cbActivateUserDefinedConnection.CheckedChanged += new System.EventHandler(this.cbActivateUserDefinedConnection_CheckedChanged);
			// 
			// SelectConnectionForProductForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(702, 475);
			this.Controls.Add(this.grpUserDefinedConnection);
			this.Controls.Add(this.grpConnection);
			this.Controls.Add(this.grpInfo);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.tvDistributors);
			this.Name = "SelectConnectionForProductForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Verteileranschluﬂ";
			this.Load += new System.EventHandler(this.SelectConnectionForProductForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectConnectionForProductForm_FormClosing);
			this.grpInfo.ResumeLayout(false);
			this.grpConnection.ResumeLayout(false);
			this.grpConnection.PerformLayout();
			this.grpUserDefinedConnection.ResumeLayout(false);
			this.grpUserDefinedConnection.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridUserDefinedConnection)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.userDefinedConnectionBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView tvDistributors;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.GroupBox grpInfo;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.GroupBox grpConnection;
		private System.Windows.Forms.RadioButton rbRuecklauf;
		private System.Windows.Forms.RadioButton rbVorlauf;
		private System.Windows.Forms.GroupBox grpUserDefinedConnection;
		private System.Windows.Forms.DataGridView gridUserDefinedConnection;
		private System.Windows.Forms.CheckBox cbActivateUserDefinedConnection;
		private System.Windows.Forms.BindingSource userDefinedConnectionBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn hk1DataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn hk2DataGridViewTextBoxColumn;
	}
}