using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class EdgeDistanceForm : Form {

		public EdgeDistanceForm(double edgeDistance) {
			InitializeComponent();

			this.SetLanguage();
			this.numEdgeDistance.Value = (decimal)(edgeDistance * 100.0);
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.EdgeDistanceForm_Titel;
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"&Abbrechen";
			this.btnOk.Text = EuroplanRes.General_Ok; //"&OK";
			this.label3.Text = EuroplanRes.EdgeDistanceForm_Randabstand;
			this.label1.Text = EuroplanRes.Unit_Zentimeter;
		}

		private void EdgeDistanceForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["EdgeDistanceForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
			if (this.DialogResult == DialogResult.OK) {
				if (numEdgeDistance.Text == "") {
					//MessageBox.Show(EuroplanRes.NewPlanForm_KeinBezeichnerText, EuroplanRes.NewPlanForm_KeinBezeichnerTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					e.Cancel = true;
				}
			}
		}

		private void EdgeDistanceForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["EdgeDistanceForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		public double EdgeDistance {
			get { return (double)this.numEdgeDistance.Value; }
		}

	}
}