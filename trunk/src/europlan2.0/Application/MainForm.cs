using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Resources;
using System.Reflection;
using System.Threading;

namespace Europlan.Application {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			// TODO: check if anything has to be saved...
			System.Windows.Forms.Application.Exit();
		}

		private void MainForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["MainForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			if (settings.GetSetting("Maximized", false)) {
				this.WindowState = FormWindowState.Maximized;
			} else {
				this.WindowState = FormWindowState.Normal;
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["MainForm"];
			if (this.WindowState == FormWindowState.Normal) {
				settings.StorePoint("Location", this.Location);
				settings.StoreSize("Size", this.Size);
				settings.StoreSetting("Maximized", false);
			} else if (this.WindowState == FormWindowState.Maximized) {
				settings.StoreSetting("Maximized", true);
			}
			SettingsFile.Update();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			OptionsForm options = new OptionsForm();
			DialogResult result = options.ShowDialog();
			if (result == DialogResult.OK && options.RestartRequired) {
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
				string message = resources.GetString("RestartMessage", Thread.CurrentThread.CurrentUICulture);
				string caption = resources.GetString("RestartCaption");
				result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel);
				if (result == DialogResult.OK) {
					System.Windows.Forms.Application.Restart();
				}
			}
			options.Dispose();
		}



	}
}