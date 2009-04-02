using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Application {
	public partial class StartingForm : Form {
		public StartingForm() {
			InitializeComponent();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e) {
			e.Graphics.DrawString("Die Anwendung wird gestartet", DefaultFont, Brushes.Black, 0, 0);
		}

		private void StartingForm_FormClosing(object sender, FormClosingEventArgs e) {
			Thread.Sleep(1000);
		}
	}
}