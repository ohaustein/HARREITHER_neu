using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class SelectRoomForm : Form {
		public SelectRoomForm(List<Room> rooms) {
			InitializeComponent();

			this.SetLanguage();

			foreach (Room room in rooms) {
				this.lstRooms.Items.Add(new RoomItem(room));
			}
			if (this.lstRooms.Items.Count > 0) {
				this.lstRooms.Items[0].Selected = true;
			}
		}

		private void SetLanguage() {
			this.btnOk.Text = EuroplanRes.General_Ok; //"OK";
			this.button2.Text = EuroplanRes.General_Abbrechen; //"Abbrechen";

			this.Text = EuroplanRes.SelectRoomForm_Titel; //"Bitte wählen Sie den gewünschten Raum";
		}

		private class RoomItem : ListViewItem {
			private Room room;

			public RoomItem(Room room) {
				this.room = room;
				this.Text = room.ToString();
			}

			public Room Room {
				get { return this.room; }
			}
		}

		public Room SelectedRoom {
			get {
				if (this.lstRooms.SelectedItems.Count > 0) {
					return (this.lstRooms.SelectedItems[0] as RoomItem).Room;
				} else {
					return null;
				}
			}
		}

		private void SelectRoomForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.lstRooms.SelectedItems.Count == 0 && this.DialogResult == DialogResult.OK) {
				MessageBox.Show(EuroplanRes.SelectRoomForm_KeinRaumText, EuroplanRes.SelectRoomForm_KeinRaumTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				e.Cancel = true;
			}
			SettingsKey settings = SettingsFile.Settings["SelectRoomForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void SelectRoomForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["SelectRoomForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}
	}
}