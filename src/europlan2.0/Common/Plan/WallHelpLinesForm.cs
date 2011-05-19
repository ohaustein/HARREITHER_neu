using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class WallHelpLinesForm : Form {

		private Room room;
		private GraphicalWall wall;
		private bool updateOngoing = false;

		public WallHelpLinesForm(Room room, GraphicalWall wall) {
			InitializeComponent();
			updateOngoing = true;

			if (wall == null) {
				rbWall.Enabled = false;
				chkUseGlobal.Enabled = false;
			} else {
				chkUseGlobal.Checked = wall.ShowGlobalHelpLines;
			}
			lstOffsets.DataSource = room.HelpLines;
			
			updateOngoing = false;
			this.SetLanguage();
		}

		private void SetLanguage() {

		}

		private void WallHelpLinesForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["WallHelpLinesForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void WallHelpLinesForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["WallHelpLinesForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void rbGlobal_CheckedChanged(object sender, EventArgs e) {

		}

		private void rbWall_CheckedChanged(object sender, EventArgs e) {

		}

		private void chkUseGlobal_CheckedChanged(object sender, EventArgs e) {

		}

		private void numOffset_TextChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (numOffset.Text != "") {
					btnAdd.Enabled = true;
				} else {
					btnAdd.Enabled = false;
				}
			}
		}

		private void btnAdd_Click(object sender, EventArgs e) {

		}

		private void btnDelete_Click(object sender, EventArgs e) {

		}

		private void lstOffsets_SelectedValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (lstOffsets.SelectedIndex >= 0) {
					btnDelete.Enabled = true;
				} else {
					btnDelete.Enabled = false;
				}
			}

		}

	}
}