using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class WarningsAndErrorsForm : Form {

		public WarningsAndErrorsForm() {
			InitializeComponent();
		}

		private void WarningsAndErrorsForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["WarningsAndErrorsForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void WarningsAndErrorsForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["WarningsAndErrorsForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

	}
}