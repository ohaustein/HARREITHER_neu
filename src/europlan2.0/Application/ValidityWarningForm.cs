using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class ValidityWarningForm : Form {
		public ValidityWarningForm(int daysRemaining) {
			InitializeComponent();

			this.Text = Europlan.Common.EuroplanRes.License_WarnungGueltigkeitCaption;
			this.lblMessage.Text = Europlan.Common.EuroplanRes.License_WarnungGueltigkeit.Replace("%DAYS%", daysRemaining.ToString());
			this.chkDontShowAgain.Text = Europlan.Common.EuroplanRes.License_NichtMehrZeigen;
		}

		public bool DontShowAgain {
			get { return this.chkDontShowAgain.Checked; }
		}

		private void btnOk_Click(object sender, EventArgs e) {
			this.Close();
		}
	}
}