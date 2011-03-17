namespace Europlan.Common {
	partial class NewDistributorForm {
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.txtId = new System.Windows.Forms.TextBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.cmbCircuit = new System.Windows.Forms.ComboBox();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(153, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Neuer Verteiler";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Nummer:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(14, 81);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Bezeichnung:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(14, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 23);
			this.label4.TabIndex = 3;
			this.label4.Text = "Regelkreis:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(313, 139);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "&Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(232, 139);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "&OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// txtId
			// 
			this.txtId.Location = new System.Drawing.Point(120, 55);
			this.txtId.Name = "txtId";
			this.txtId.Size = new System.Drawing.Size(76, 20);
			this.txtId.TabIndex = 6;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(120, 78);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(268, 20);
			this.txtName.TabIndex = 1;
			// 
			// cmbCircuit
			// 
			this.cmbCircuit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCircuit.FormattingEnabled = true;
			this.cmbCircuit.Location = new System.Drawing.Point(120, 101);
			this.cmbCircuit.Name = "cmbCircuit";
			this.cmbCircuit.Size = new System.Drawing.Size(268, 21);
			this.cmbCircuit.TabIndex = 8;
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// NewDistributorForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(400, 174);
			this.Controls.Add(this.cmbCircuit);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.txtId);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.helpProvider.SetHelpKeyword(this, "html\\euro8ud1.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewDistributorForm";
			this.helpProvider.SetShowHelp(this, true);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Verteiler anlegen";
			this.Load += new System.EventHandler(this.NewDistributorForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewDistributor_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.ComboBox cmbCircuit;
		private System.Windows.Forms.HelpProvider helpProvider;
	}
}