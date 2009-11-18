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

		private bool projectOverview;
		private bool areaOverview;
		private bool auslegung;
		private bool auslegungBilanz;
		private bool verlegedaten;

		public ProjectReportOptions() {
			InitializeComponent();
		}

		private void ProjectReportOptions_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProjectReportOptions"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.projectOverview = settings.GetSetting("ProjectOverview", true);
			this.areaOverview = settings.GetSetting("AreaOverview", true);
			this.auslegung = settings.GetSetting("Auslegung", true);
			this.auslegungBilanz = settings.GetSetting("AuslegungBilanz", true);
			this.verlegedaten = settings.GetSetting("Verlegedaten", true);

			this.chkProjectOverview.Checked = this.projectOverview;
			this.chkAreaOverview.Enabled = this.projectOverview;
			this.chkAreaOverview.Checked = this.areaOverview;
			this.chkAuslegung.Checked = this.auslegung;
			this.chkAuslegungBilanz.Enabled = this.auslegung;
			this.chkAuslegungBilanz.Checked = this.auslegungBilanz;
			this.chkVerlegedaten.Checked = this.verlegedaten;
		}

		private void ProjectReportOptions_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProjectReportOptions"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSetting("ProjectOverview", this.projectOverview);
			settings.StoreSetting("AreaOverview", this.areaOverview);
			settings.StoreSetting("Auslegung", this.auslegung);
			settings.StoreSetting("AuslegungBilanz", this.auslegungBilanz);
			settings.StoreSetting("Verlegedaten", this.verlegedaten);
			SettingsFile.Update();
		}

		private void chkProjectOverview_CheckedChanged(object sender, EventArgs e) {
			this.projectOverview = this.chkProjectOverview.Checked;
			this.chkAreaOverview.Enabled = this.projectOverview;
		}

		private void chkAreaOverview_CheckedChanged(object sender, EventArgs e) {
			this.areaOverview = this.chkProjectOverview.Checked;
		}

		private void chkAuslegung_CheckedChanged(object sender, EventArgs e) {
			this.auslegung = chkAuslegung.Checked;
			this.chkAuslegungBilanz.Enabled = this.auslegung;
		}

		private void chkAuslegungBilanz_CheckedChanged(object sender, EventArgs e) {
			this.auslegungBilanz = chkAuslegungBilanz.Checked;
		}


		private void chkVerlegedaten_CheckedChanged(object sender, EventArgs e) {
			this.verlegedaten = this.chkVerlegedaten.Checked;
		}

		public bool ProjectOverview {
			get { return projectOverview; }
			set { projectOverview = value; }
		}

		public bool AreaOverview {
			get { return areaOverview; }
			set { areaOverview = value; }
		}

		public bool Auslegung {
			get { return auslegung; }
			set { auslegung = value; }
		}

		public bool AuslegungBilanz {
			get { return auslegungBilanz; }
			set { auslegungBilanz = value; }
		}

		public bool Verlegedaten {
			get { return verlegedaten; }
			set { verlegedaten = value; }
		}

	}
}