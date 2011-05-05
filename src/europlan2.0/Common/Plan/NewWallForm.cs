using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class NewWallForm : Form {

		private bool autoGeneration;
		private bool isCompact;
		private string wallId;

		public NewWallForm(bool autoGeneration, bool isCompact) {
			InitializeComponent();

			this.autoGeneration = autoGeneration;
			this.isCompact = isCompact;

			this.numWidth.Enabled = !autoGeneration;

			if (isCompact) {
				wallId = Project.Instance.HithermCompactWalls[0].Id;
			} else {
				wallId = Project.Instance.HithermWalls[0].Id;
			}
			this.txtConstruction.Text = wallId;

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"&Abbrechen";
			this.btnOk.Text = EuroplanRes.General_Ok; //"&OK";

			//this.label3.Text = EuroplanRes.NewPlanForm_Bezeichnung; //"Bezeichnung:";
			//this.Text = EuroplanRes.NewPlanForm_PlanImportieren; //"Plan importieren";
		}

		private void NewWallForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewWallForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
			if (this.DialogResult == DialogResult.OK) {
				//if (txtName.Text == "") {
				//    MessageBox.Show(EuroplanRes.NewPlanForm_KeinBezeichnerText, EuroplanRes.NewPlanForm_KeinBezeichnerTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				//    e.Cancel = true;
				//}
			}
		}

		private void NewWallForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewWallForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		public double Width {
			get { return (double)this.numWidth.Value; }
		}

		public double Height {
			get { return (double)this.numHeight.Value; }
		}

		public string WallId {
			get { return this.wallId; }
		}

		private void btnSelectConstruction_Click(object sender, EventArgs e) {
			SelectHithermWallForm form = new SelectHithermWallForm(this.isCompact);
			if (form.ShowDialog() == DialogResult.OK) {
				this.wallId = form.SelectedWall.Id;
				this.txtConstruction.Text = this.wallId;
			}
			form.Dispose();
		}
	}
}