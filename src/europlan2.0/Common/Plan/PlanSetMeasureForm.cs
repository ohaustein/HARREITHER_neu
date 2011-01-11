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
			switch (Product.ConfigPlanMeasureEnum) {
				case Product.PlanMeasureEnum.PM_CENTIMETER:
					this.lblLength.Text = "Länge in cm:";
					this.txtLength.EditType = NumericBox.NumericEditType.LENGTH_CM;
					break;

				case Product.PlanMeasureEnum.PM_MILLIMETER:
					this.lblLength.Text = "Länge in mm:";
					this.txtLength.EditType = NumericBox.NumericEditType.LENGTH_MM;
					break;

				case Product.PlanMeasureEnum.PM_METER:
				default:
					this.lblLength.Text = "Länge in m:";
					this.txtLength.EditType = NumericBox.NumericEditType.LENGTH;
					break;
			}
			if (this.length.HasValue) {
				switch (Product.ConfigPlanMeasureEnum) {
					case Product.PlanMeasureEnum.PM_CENTIMETER:
						this.lblText.Text = EuroplanRes.PlanSetMeasureForm_Massstab.Replace("%LAENGE%", length.Value.ToString("0.00")).Replace("%EINHEIT%", "cm");
						break;

					case Product.PlanMeasureEnum.PM_MILLIMETER:
						this.lblText.Text = EuroplanRes.PlanSetMeasureForm_Massstab.Replace("%LAENGE%", length.Value.ToString("0.00")).Replace("%EINHEIT%", "mm");
						break;

					case Product.PlanMeasureEnum.PM_METER:
					default:
						this.lblText.Text = EuroplanRes.PlanSetMeasureForm_Massstab.Replace("%LAENGE%", length.Value.ToString("0.00")).Replace("%EINHEIT%", "m");
						break;
				}
				this.txtLength.Value = (decimal)(length.Value * Product.ConfigPlanMeasureMultiplier);
			} else {
				this.lblText.Text = EuroplanRes.PlanSetMeasureForm_KeinMassstab;
				this.txtLength.Value = (decimal)(1 * Product.ConfigPlanMeasureMultiplier);
			}
		}

		public double Length {
			get { return (double)this.txtLength.Value / Product.ConfigPlanMeasureMultiplier; }
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