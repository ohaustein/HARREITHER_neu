using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class NewGraphicalProductToProductConnection : Form {
		public NewGraphicalProductToProductConnection() {
			InitializeComponent();
			this.SetLanguage();
			this.DialogResult = DialogResult.Cancel;
		}

		private void SetLanguage() {
			this.label1.Text = EuroplanRes.NewGraphicalProductToProductConnection_WieAnschliessen;
			this.rbVorlauf.Text = EuroplanRes.NewGraphicalProductToProductConnection_Vorlaufseitig;
			this.rbRuecklauf.Text = EuroplanRes.NewGraphicalProductToProductConnection_Ruecklaufseitig;
			this.btnOK.Text = EuroplanRes.General_Ok;
			this.btnCancel.Text = EuroplanRes.General_Abbrechen;
			this.Text = EuroplanRes.NewGraphicalProductToProductConnection_Titel;
		}

		private void btnOK_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		public bool Vorlaufseitig {
			get { return this.rbVorlauf.Checked; }
		}
	}
}