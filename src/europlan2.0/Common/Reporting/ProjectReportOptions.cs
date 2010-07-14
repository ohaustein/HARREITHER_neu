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
		private bool konstruktionen;
		private bool requiredMaterial;
		private bool recommendedMaterial;
		private bool prices;


		public ProjectReportOptions() {
			InitializeComponent();

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.btnOK.Text = EuroplanRes.General_Ok;
			this.btnCancel.Text = EuroplanRes.General_Abbrechen;

			this.groupBox1.Text = EuroplanRes.ProjectReportOptions_DruckbereicheWaehlen; //"Bitte wählen Sie die gewünschten Druckbereiche";
			this.chkProjectOverview.Text = EuroplanRes.ProjectReportOptions_Projektuebersicht; //"Projektübersicht";
			this.chkAreaOverview.Text = EuroplanRes.ProjectReportOptions_Flaechenuebersicht; //"Flächenübersicht";
			this.chkAuslegung.Text = EuroplanRes.ProjectReportOptions_Auslegung; //"Auslegung";
			this.chkAuslegungBilanz.Text = EuroplanRes.ProjectReportOptions_Bilanz; //"Bilanz";
			this.chkVerlegedaten.Text = EuroplanRes.ProjectReportOptions_Verlegedaten; //"Verlegedaten";
			this.chkRequiredMaterial.Text = EuroplanRes.ProjectReportOptions_Materialbedarf; //"Materialbedarf";
			this.chkRecommendedMaterial.Text = EuroplanRes.ProjectReportOptions_Bestellvorschlag; //"Bestellvorschlag";
			this.chkKonstruktionen.Text = EuroplanRes.ProjectReportOptions_Konstruktionen; //"Konstruktionen";
			this.chkPrice.Text = EuroplanRes.ProjectReportOptions_Prices; //"Preise"
			this.Text = EuroplanRes.ProjectReportOptions_Titel; //"Druckbereich";
		}

		private void ProjectReportOptions_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProjectReportOptions"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.projectOverview = settings.GetSetting("ProjectOverview", true);
			this.areaOverview = settings.GetSetting("AreaOverview", true);
			this.auslegung = settings.GetSetting("Auslegung", true);
			this.auslegungBilanz = settings.GetSetting("AuslegungBilanz", true);
			this.verlegedaten = settings.GetSetting("Verlegedaten", true);
			this.konstruktionen = settings.GetSetting("Konstruktionen", true);
			this.requiredMaterial = settings.GetSetting("RequiredMaterial", true);
			this.recommendedMaterial = settings.GetSetting("RecommendedMaterial", true);
			this.prices = settings.GetSetting("Prices", true);

			this.chkProjectOverview.Checked = this.projectOverview;
			this.chkAreaOverview.Enabled = this.projectOverview;
			this.chkAreaOverview.Checked = this.areaOverview;
			this.chkAuslegung.Checked = this.auslegung;
			this.chkAuslegungBilanz.Enabled = this.auslegung;
			this.chkAuslegungBilanz.Checked = this.auslegungBilanz;
			this.chkVerlegedaten.Checked = this.verlegedaten;
			this.chkKonstruktionen.Checked = this.konstruktionen;
			this.chkRequiredMaterial.Checked = this.requiredMaterial;
			this.chkRecommendedMaterial.Checked = this.recommendedMaterial;
			this.chkPrice.Checked = this.prices;
		}

		private void ProjectReportOptions_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ProjectReportOptions"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSetting("ProjectOverview", this.projectOverview);
			settings.StoreSetting("AreaOverview", this.areaOverview);
			settings.StoreSetting("Auslegung", this.auslegung);
			settings.StoreSetting("AuslegungBilanz", this.auslegungBilanz);
			settings.StoreSetting("Verlegedaten", this.verlegedaten);
			settings.StoreSetting("Konstruktionen", this.konstruktionen);
			settings.StoreSetting("RequiredMaterial", this.requiredMaterial);
			settings.StoreSetting("RecommendedMaterial", this.recommendedMaterial);
			settings.StoreSetting("Prices", this.prices);
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

		private void chkKonstruktionen_CheckedChanged(object sender, EventArgs e) {
			this.konstruktionen = this.chkKonstruktionen.Checked;
		}

		private void chkRequiredMaterial_CheckedChanged(object sender, EventArgs e) {
			this.requiredMaterial = this.chkRequiredMaterial.Checked;
		}

		private void chkRecommendedMaterial_CheckedChanged(object sender, EventArgs e) {
			this.recommendedMaterial = this.chkRecommendedMaterial.Checked;
		}

		private void chkPrice_CheckedChanged(object sender, EventArgs e) {
			this.prices = this.chkPrice.Checked;
		}

		public bool ProjectOverview {
			get { return projectOverview; }
		}

		public bool AreaOverview {
			get { return areaOverview; }
		}

		public bool Auslegung {
			get { return auslegung; }
		}

		public bool AuslegungBilanz {
			get { return auslegungBilanz; }
		}

		public bool Verlegedaten {
			get { return verlegedaten; }
		}

		public bool Konstruktionen {
			get { return konstruktionen; }
		}
		
		public bool RequiredMaterial {
			get { return requiredMaterial; }
		}
		
		public bool RecommendedMaterial {
			get { return recommendedMaterial; }
		}

		public bool Prices {
			// TODO
			get { return false; }
		}

	}
}