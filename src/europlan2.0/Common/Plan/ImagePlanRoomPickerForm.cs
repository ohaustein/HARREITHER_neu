using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Drawing.Drawing2D;

namespace Europlan.Common {

	public partial class ImagePlanRoomPickerForm : Form {
		
		private bool unsavedChanges = false;

		public ImagePlanRoomPickerForm(ImagePlan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.picturePanel.Plan = plan;
			this.picturePanel.MoveMode = true;
			btnMove.Checked = true;
			btnPick.Checked = false;
			this.picturePanel.Cursor = Cursors.Hand;
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.ImagePlanRoomPickerForm_Titel; //"Optionen";
		}

		public List<PointF> RoomCoordinates {
			get { return picturePanel.RoomCoordinates; }
			set { picturePanel.RoomCoordinates = value; }
		}

		private void ImagePlanRoomPickerForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanRoomPickerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void ImagePlanRoomPickerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanRoomPickerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		public bool UnsavedChanges {
			get { return unsavedChanges || picturePanel.UnsavedRoomPickerChanges; }
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			//unsavedChanges = true;
			picturePanel.AddScale(1.1, null);
			picturePanel.Invalidate();
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			//unsavedChanges = true;
			picturePanel.AddScale(0.9, null);
			picturePanel.Invalidate();
		}

		private void btnMove_Click(object sender, EventArgs e) {
			this.picturePanel.Cursor = Cursors.Hand;
			this.picturePanel.MoveMode = true;
			this.picturePanel.RoomPickerMode = false;
			btnMove.Checked = true;
			btnPick.Checked = false;
		}

		private void btnPick_Click(object sender, EventArgs e) {
			if (!btnPick.Checked) {
				this.picturePanel.Cursor = Cursors.Cross;
				this.picturePanel.MoveMode = false;
				this.picturePanel.RoomPickerMode = true;
				btnMove.Checked = false;
				btnPick.Checked = true;
			}
		}

	}
}