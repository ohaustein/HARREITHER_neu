using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class SelectHithermWallForm : Form {

		private class WallItem : ListViewItem {
			private HithermWall wall;

			public WallItem(HithermWall wall) {
				this.wall = wall;
				this.Text = wall.Id + ": " + wall.Name;
			}

			public HithermWall Wall {
				get { return this.wall; }
			}
		}

		public SelectHithermWallForm(bool showCompact) {
			InitializeComponent();

			this.SetLanguage();

			List<HithermWall> allWalls = showCompact ? Project.Instance.HithermCompactWalls : Project.Instance.HithermWalls;
			foreach (HithermWall hw in allWalls) {
				this.lstWalls.Items.Add(new WallItem(hw));
			}
			if (this.lstWalls.Items.Count > 0) {
				this.lstWalls.Items[0].Selected = true;
			}
		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen;
			this.btnOk.Text = EuroplanRes.General_Ok;
			this.Text = EuroplanRes.SelectHithermWallForm_KonstruktionWaehlen;//"Bitte wählen Sie die Konstruktion!"
		}

		public HithermWall SelectedWall {
			get {
				if (this.lstWalls.SelectedItems.Count > 0) {
					return (this.lstWalls.SelectedItems[0] as WallItem).Wall;
				} else {
					return null;
				}
			}
			set {
				this.lstWalls.SelectedItems.Clear();
				foreach (WallItem wi in this.lstWalls.Items) {
					if (wi.Wall.Equals(value)) {
						wi.Selected = true;
						return;
					}
				}
			}
		}

		private void SelectHithermWallConstruction_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.lstWalls.SelectedItems.Count == 0 && this.DialogResult == DialogResult.OK) {
				MessageBox.Show(EuroplanRes.SelectHithermWallForm_KeineKonstruktionGewaehltText, EuroplanRes.SelectHithermWallForm_KeineKonstruktionGewaehltTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				e.Cancel = true;
			}
			SettingsKey settings = SettingsFile.Settings["SelectHithermWallForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void SelectHithermWallConstructionForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["SelectHithermWallForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void lstConstructions_DoubleClick(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}