namespace Europlan.Common {
	partial class NewGraphicalProductToProductConnection {
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
			this.rbVorlauf = new System.Windows.Forms.RadioButton();
			this.rbRuecklauf = new System.Windows.Forms.RadioButton();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(339, 23);
			this.label1.TabIndex = 0;
			this.label1.Tag = "";
			this.label1.Text = "Wollen Sie das System vorlauf oder rücklaufseitig anschließen?";
			// 
			// rbVorlauf
			// 
			this.rbVorlauf.AutoSize = true;
			this.rbVorlauf.Checked = true;
			this.rbVorlauf.Location = new System.Drawing.Point(27, 35);
			this.rbVorlauf.Name = "rbVorlauf";
			this.rbVorlauf.Size = new System.Drawing.Size(81, 17);
			this.rbVorlauf.TabIndex = 1;
			this.rbVorlauf.TabStop = true;
			this.rbVorlauf.Text = "vorlaufseitig";
			this.rbVorlauf.UseVisualStyleBackColor = true;
			// 
			// rbRuecklauf
			// 
			this.rbRuecklauf.AutoSize = true;
			this.rbRuecklauf.Location = new System.Drawing.Point(27, 58);
			this.rbRuecklauf.Name = "rbRuecklauf";
			this.rbRuecklauf.Size = new System.Drawing.Size(87, 17);
			this.rbRuecklauf.TabIndex = 2;
			this.rbRuecklauf.Text = "rücklaufseitig";
			this.rbRuecklauf.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(180, 81);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(261, 81);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "&Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// NewGraphicalProductToProductConnection
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(350, 119);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.rbRuecklauf);
			this.Controls.Add(this.rbVorlauf);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewGraphicalProductToProductConnection";
			this.Text = "System anschließen";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton rbVorlauf;
		private System.Windows.Forms.RadioButton rbRuecklauf;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}