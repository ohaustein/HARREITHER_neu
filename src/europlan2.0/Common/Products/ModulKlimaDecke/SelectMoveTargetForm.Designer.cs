namespace Europlan.Common {
	partial class SelectMoveTargetForm {
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
			this.lstSubarea = new System.Windows.Forms.ListBox();
			this.lstCircuits = new System.Windows.Forms.ListBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lstSubarea
			// 
			this.lstSubarea.FormattingEnabled = true;
			this.lstSubarea.Location = new System.Drawing.Point(103, 55);
			this.lstSubarea.Name = "lstSubarea";
			this.lstSubarea.Size = new System.Drawing.Size(129, 134);
			this.lstSubarea.TabIndex = 1;
			// 
			// lstCircuits
			// 
			this.lstCircuits.FormattingEnabled = true;
			this.lstCircuits.Location = new System.Drawing.Point(12, 55);
			this.lstCircuits.Name = "lstCircuits";
			this.lstCircuits.Size = new System.Drawing.Size(85, 134);
			this.lstCircuits.TabIndex = 0;
			this.lstCircuits.SelectedIndexChanged += new System.EventHandler(this.lstCircuits_SelectedIndexChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(157, 197);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(76, 197);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(219, 39);
			this.label1.TabIndex = 153;
			// 
			// SelectMoveTargetForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(246, 230);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lstSubarea);
			this.Controls.Add(this.lstCircuits);
			this.Name = "SelectMoveTargetForm";
			this.Text = "Ziel wählen";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstSubarea;
		private System.Windows.Forms.ListBox lstCircuits;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label label1;
	}
}