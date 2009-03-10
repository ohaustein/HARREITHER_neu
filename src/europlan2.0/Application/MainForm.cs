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

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			// TODO: check if anything has to be saved...
			System.Windows.Forms.Application.Exit();
		}



	}
}