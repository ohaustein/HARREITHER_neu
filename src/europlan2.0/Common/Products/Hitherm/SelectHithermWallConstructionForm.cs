using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class SelectHithermWallConstructionForm : Form {

		private class WallConstructionItem : ListViewItem {
			private WallConstruction construction;

			public WallConstructionItem(WallConstruction construction) {
				this.construction = construction;
				this.Text = construction.Id + ": " + construction.LocalizedName;
			}

			public WallConstruction Construction {
				get { return this.construction; }
			}
		}
		
		public SelectHithermWallConstructionForm(bool showCompact) {
			InitializeComponent();

			this.SetLanguage();

			ConstructionListWrapper clw = new ConstructionListWrapper(Configuration.ConfigurationType.UserConfiguration);
			clw.ConstructionScopeFilter = ConstructionScopeEnum.WallConstruction;
			foreach (WallConstruction wc in clw) {
				if ((!showCompact && wc.IsHithermWall) || (showCompact && wc.IsHithermCompactWall)) {
					this.lstConstructions.Items.Add(new WallConstructionItem(wc));
				}
			}
			if (this.lstConstructions.Items.Count > 0) {
				this.lstConstructions.Items[0].Selected = true;
			}
		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen;
			this.btnOk.Text = EuroplanRes.General_Ok;
			this.Text = EuroplanRes.SelectHithermWallConstructionForm_KonstruktionWaehlen;//"Bitte wählen Sie die Basiskonstruktion!"
		}

		public WallConstruction SelectedConstruction {
			get {
				if (this.lstConstructions.SelectedItems.Count > 0) {
					return (this.lstConstructions.SelectedItems[0] as WallConstructionItem).Construction;
				} else {
					return null;
				}
			}
			set {
				this.lstConstructions.SelectedItems.Clear();
				foreach (WallConstructionItem wci in this.lstConstructions.Items) {
					if (wci.Construction.Equals(value)) {
						wci.Selected = true;
						return;
					}
				}
			}
		}

		private void SelectHithermWallConstruction_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.lstConstructions.SelectedItems.Count == 0 && this.DialogResult == DialogResult.OK) {
				MessageBox.Show(EuroplanRes.SelectHithermWallConstructionForm_KeineKonstruktionGewaehltText, EuroplanRes.SelectHithermWallConstructionForm_KeineKonstruktionGewaehltTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				e.Cancel = true;
			}
			SettingsKey settings = SettingsFile.Settings["SelectHithermWallConstructionForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void SelectHithermWallConstructionForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["SelectHithermWallConstructionForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void lstConstructions_DoubleClick(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}