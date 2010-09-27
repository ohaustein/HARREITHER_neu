namespace Europlan.Common.Products {
	partial class ModulKlimaDeckePlannerForm {
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
			this.modulKlimaBodenPlanner1 = new Europlan.Common.ModulKlimaDeckePlanner(this.components);
			this.roomPicker1 = new Europlan.Common.RoomPicker(this.components);
			this.SuspendLayout();
			// 
			// modulKlimaBodenPlanner1
			// 
			this.modulKlimaBodenPlanner1.Product = null;
			// 
			// roomPicker1
			// 
			this.roomPicker1.Mode = Europlan.Common.RoomPicker.RoomPickerMode.RPM_PICK_ROOM;
			this.roomPicker1.Room = null;
			// 
			// ModulKlimaBodenPlannerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(730, 450);
			this.Name = "ModulKlimaBodenPlannerForm";
			this.Text = "ModulKlimaBodenPlannerForm";
			this.ResumeLayout(false);

		}

		#endregion

		private ModulKlimaDeckePlanner modulKlimaBodenPlanner1;
		private RoomPicker roomPicker1;
	}
}