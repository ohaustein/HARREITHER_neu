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
				this.selectedRoomType = value;
				// TODO select room type
			}
		}

		private void NewRoomTypeForm_FormClosing(object sender, FormClosingEventArgs e) {
			gridRoomTypes.Cleanup();
		}
	}
}