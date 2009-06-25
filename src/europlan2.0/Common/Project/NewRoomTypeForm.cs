using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class NewRoomTypeForm : Form {

		private RoomType selectedRoomType;

		public NewRoomTypeForm(Configuration config) {
			InitializeComponent();
			gridRoomTypes.Config = config;
		}

		public RoomType SelectedRoomType {
			get { return this.selectedRoomType; }
			set {
				// TODO select room type
				//this.selectedRoomType = value;
			}
		}

		private void NewRoomTypeForm_FormClosing(object sender, FormClosingEventArgs e) {
			this.selectedRoomType = gridRoomTypes.SelectedRoomType;
			gridRoomTypes.Cleanup();
			this.selectedRoomType = gridRoomTypes.SelectedRoomType;

			SettingsKey settings = SettingsFile.Settings["NewDistributorForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();

		}

		private void NewRoomTypeForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewDistributorForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}
	}
}