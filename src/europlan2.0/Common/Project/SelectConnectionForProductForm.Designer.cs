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
			this.tvDistributors = new System.Windows.Forms.TreeView();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.grpInfo = new System.Windows.Forms.GroupBox();
			this.lblInfo = new System.Windows.Forms.Label();
			this.grpConnection = new System.Windows.Forms.GroupBox();
			this.rbRuecklauf = new System.Windows.Forms.RadioButton();
			this.rbVorlauf = new System.Windows.Forms.RadioButton();
			this.grpInfo.SuspendLayout();
			this.grpConnection.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvDistributors
			// 
			this.tvDistributors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
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
			this.lblInfo.Text = "label1";
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
			// SelectConnectionForProductForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(702, 475);
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
	}
}