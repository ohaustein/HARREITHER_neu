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

			this.room = room;
			this.wall = wall;

			if (wall == null) {
				rbWall.Enabled = false;
				chkUseGlobal.Enabled = false;
			} else {
				if (wall.IsDachSchraege) {
					chkUseGlobal.Enabled = false;
					rbWall.Checked = true;
				} else {
					chkUseGlobal.Checked = wall.ShowGlobalHelpLines;
				}
			}
			ApplyDataSource(GetOffsets());
			numOffset.Text = "";
			
			updateOngoing = false;
			this.SetLanguage();
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.WallHelpLinesForm_Titel;
			this.btnClose.Text = EuroplanRes.General_Schliessen;
			this.groupBox1.Text = EuroplanRes.WallHelpLinesForm_Modus;
			this.rbWall.Text = EuroplanRes.WallHelpLinesForm_Individuelle;
			this.rbGlobal.Text = EuroplanRes.WallHelpLinesForm_Globale;
			this.groupBox2.Text = EuroplanRes.WallHelpLinesForm_Eigenschaften;
			this.chkUseGlobal.Text = EuroplanRes.WallHelpLinesForm_GlobaleAktivieren;
			this.groupBox3.Text = EuroplanRes.WallHelpLinesForm_Positionen;
			this.btnDelete.Text = EuroplanRes.WallHelpLinesForm_Loeschen;
			this.btnAdd.Text = EuroplanRes.WallHelpLinesForm_Hinzufuegen;
			this.label1.Text = EuroplanRes.Unit_Zentimeter;
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

		private void rbType_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				ApplyDataSource(GetOffsets());
			}
		}

		private void chkUseGlobal_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				wall.ShowGlobalHelpLines = chkUseGlobal.Checked;
			}
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
			List<double> offsets = GetOffsets();
			if (!offsets.Contains((double)numOffset.Value)) {
				offsets.Add((double)numOffset.Value);
				offsets.Sort();
				ApplyDataSource(offsets);
			}
			numOffset.Text = "";
		}

		private void btnDelete_Click(object sender, EventArgs e) {
			if (lstOffsets.SelectedIndex >= 0) {
				GetOffsets().RemoveAt(lstOffsets.SelectedIndex);
				ApplyDataSource(GetOffsets());
			}
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

		private void ApplyDataSource(List<double> dataSource) {
			// dirty hack to make the fucking listbox work - do not remove!!! ;)
			lstOffsets.DataSource = null;
			lstOffsets.DataSource = dataSource;
		}

		private List<double> GetOffsets() {
			if (rbGlobal.Checked) {
				return room.HelpLines;
			} else {
				return wall.HelpLines;
			}
		}

		private void lstOffsets_DrawItem(object sender, DrawItemEventArgs e) {
			e.DrawBackground();
			if (e.Index >= 0) {
				SizeF stringSize = new SizeF();
				string text = lstOffsets.Items[e.Index].ToString() + " " + EuroplanRes.Unit_Zentimeter;
				stringSize = e.Graphics.MeasureString(text, e.Font);
				// Draw the current item text based on the current Font and the custom brush settings.
				e.Graphics.DrawString(text, e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.Right - stringSize.Width, e.Bounds.Y));
				// If the ListBox has focus, draw a focus rectangle around the selected item.
			}
			e.DrawFocusRectangle();
		}

	}
}