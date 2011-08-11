namespace Europlan.Common {
	partial class SelectRoomForm {
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
			this.lstRooms = new System.Windows.Forms.ListView();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.btnOk = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstRooms
			// 
			this.lstRooms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstRooms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
			this.lstRooms.FullRowSelect = true;
			this.lstRooms.HideSelection = false;
			this.lstRooms.Location = new System.Drawing.Point(12, 12);
			this.lstRooms.MultiSelect = false;
			this.lstRooms.Name = "lstRooms";
			this.lstRooms.Size = new System.Drawing.Size(300, 175);
			this.lstRooms.TabIndex = 0;
			this.lstRooms.UseCompatibleStateImageBehavior = false;
			this.lstRooms.View = System.Windows.Forms.View.List;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 300;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(156, 193);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(237, 193);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Abbrechen";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// SelectRoomForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(324, 228);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lstRooms);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SelectRoomForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Bitte wählen Sie den gewünschten Raum";
			this.Load += new System.EventHandler(this.SelectRoomForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectRoomForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lstRooms;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ColumnHeader colName;
	}
}