using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		private void Beenden_Click(object sender, Janus.Windows.UI.CommandBars.CommandEventArgs e) {
			// TODO: Ask for saving
			System.Windows.Forms.Application.Exit();
		}

		private void Update_Click(object sender, Janus.Windows.UI.CommandBars.CommandEventArgs e) {

		}
	}
}