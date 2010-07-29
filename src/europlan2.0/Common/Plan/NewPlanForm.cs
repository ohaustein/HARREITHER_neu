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

		public NewPlanForm() {
			InitializeComponent();

			this.SetLanguage();

		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"&Abbrechen";
			this.btnOk.Text = EuroplanRes.General_Ok; //"&OK";

			this.label3.Text = EuroplanRes.NewPlanForm_Bezeichnung; //"Bezeichnung:";
			this.Text = EuroplanRes.NewPlanForm_PlanImportieren; //"Plan importieren";
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
		}

		public string PlanName {
			get { return this.txtName.Text; }
		}
	}
}