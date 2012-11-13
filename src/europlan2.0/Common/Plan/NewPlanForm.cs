using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class NewPlanForm : Form {

		private bool showPageSelection = false;
		private int numOfPages = 0;

		public NewPlanForm(bool showPageSelection) {
			InitializeComponent();

			this.SetLanguage();
			this.showPageSelection = showPageSelection;
		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"&Abbrechen"
			this.btnOk.Text = EuroplanRes.General_Ok; //"&OK"

			this.lblCaption.Text = EuroplanRes.NewPlanForm_Bezeichnung; //"Bezeichnung:"
			this.Text = EuroplanRes.NewPlanForm_PlanImportieren; //"Plan importieren"
			this.lblPage.Text = EuroplanRes.NewPlanForm_Seite;
		}

		private void NewPlanForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewPlanForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
			if (this.DialogResult == DialogResult.OK) {
				if (txtName.Text == "") {
					MessageBox.Show(EuroplanRes.NewPlanForm_KeinBezeichnerText, EuroplanRes.NewPlanForm_KeinBezeichnerTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					e.Cancel = true;
				}
			}
		}

		private void NewPlanForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewPlanForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			if (this.showPageSelection) {
				this.lblPage.Enabled = this.numOfPages > 1;
				this.numPage.Enabled = this.numOfPages > 1;
				this.numPage.MinValue = 1;
				this.numPage.MaxValue = this.numOfPages;
				this.lblNumOfPages.Visible = this.numOfPages > 1;
				this.lblNumOfPages.Text = EuroplanRes.NewPlanForm_Seiten + " 1 - " + this.numOfPages;
			} else {
				this.lblPage.Enabled = false;
				this.numPage.Enabled = false;
				this.numPage.MinValue = 1;
				this.lblNumOfPages.Visible = false;
			}
		}

		public string PlanName {
			get { return this.txtName.Text; }
		}

		public int NumOfPages {
			set { this.numOfPages = value; }
		}

		public int PageNumber {
			get { return (int)numPage.Value; }
		}
	}
}