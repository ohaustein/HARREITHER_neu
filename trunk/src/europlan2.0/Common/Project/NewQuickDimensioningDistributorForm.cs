using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class NewQuickDimensioningDistributorForm : Form {

		public NewQuickDimensioningDistributorForm(Project project) {
			InitializeComponent();
			if (project != null) {
				gridQuickDimensioningDistributor.Distributors = project.QuickDimensioning.Distributors;
			}
		}

		private void NewQuickDimensioningDistributorForm_FormClosing(object sender, FormClosingEventArgs e) {
			gridQuickDimensioningDistributor.Cleanup();
			SettingsKey settings = SettingsFile.Settings["NewQuickDimensioningDistributorForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void NewQuickDimensioningDistributorForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewQuickDimensioningDistributorForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}
	}
}