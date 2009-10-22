using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class LengthAssistantForm : Form {

		public LengthAssistantForm(ConnectionPipe pipe) {
			InitializeComponent();
		}

		private void LengthAssistantForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["LengthAssistantForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void LengthAssistantForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["LengthAssistantForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}
	}
}