namespace Europlan.Common {
	partial class NewRoomTypeForm {
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
			this.gridRoomTypes = new Europlan.Common.RoomTypeGrid();
			this.SuspendLayout();
			// 
			// gridRoomTypes
			// 
			this.gridRoomTypes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridRoomTypes.Location = new System.Drawing.Point(0, 0);
			this.gridRoomTypes.Name = "gridRoomTypes";
			this.gridRoomTypes.Size = new System.Drawing.Size(567, 313);
			this.gridRoomTypes.TabIndex = 0;
			// 
			// NewRoomTypeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(567, 313);
			this.Controls.Add(this.gridRoomTypes);
			this.Name = "NewRoomTypeForm";
			this.Text = "Raumtypen";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewRoomTypeForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private RoomTypeGrid gridRoomTypes;
	}
}