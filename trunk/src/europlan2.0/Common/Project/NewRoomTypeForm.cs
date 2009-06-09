using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
		}
	}
}