using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class ProjectReportOptions : Form {

		public ProjectReportOptions() {
			InitializeComponent();
		}

		private void ProjectReportOptions_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProjectReportOptions"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void ProjectReportOptions_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProjectReportOptions"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

	}
}