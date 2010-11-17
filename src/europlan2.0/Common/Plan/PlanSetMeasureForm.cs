using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class PlanSetMeasureForm : Form {

		private Nullable<double> length;

		public PlanSetMeasureForm(Nullable<double> length) {
			InitializeComponent();
			this.length = length;
			this.SetLanguage();
		}

		private void SetLanguage() {
			if (this.length.HasValue) {
				this.lblText.Text = EuroplanRes.PlanSetMeasureForm_Massstab.Replace("%LAENGE%", length.Value.ToString("0.00"));
				this.txtLength.Value = (decimal)length.Value;
			} else {
				this.lblText.Text = EuroplanRes.PlanSetMeasureForm_KeinMassstab;
				this.txtLength.Value = 1;
			}
		}

		public double Length {
			get { return (double)this.txtLength.Value; }
		}

		private void PlanSetMeasureForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["PlanSetMeasureForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void PlanSetMeasureForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["PlanSetMeasureForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}
	}
}