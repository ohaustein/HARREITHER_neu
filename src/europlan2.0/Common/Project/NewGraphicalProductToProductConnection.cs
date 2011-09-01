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
			this.DialogResult = DialogResult.Cancel;
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