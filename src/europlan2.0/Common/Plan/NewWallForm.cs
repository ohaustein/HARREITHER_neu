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

		public enum CreationTypeEnum {
			Prev,
			Next,
			After,
			Schraege
		}

		private static double defaultHeight = 250.0;
		private bool autoGeneration;
		private bool isCompact;
		private string wallId;
		private double selectedWidth = 0;
		private int wallCount = 0;

		public NewWallForm(bool autoGeneration, bool isCompact, double selectedWidth, int wallCount) {
			InitializeComponent();

			this.autoGeneration = autoGeneration;
			this.isCompact = isCompact;
			this.selectedWidth = selectedWidth;
			this.wallCount = wallCount;

			this.numWidth.Enabled = !autoGeneration;
			this.numHeight.Value = (decimal)defaultHeight;

			if (isCompact) {
				wallId = Project.Instance.HithermCompactWalls[0].Id;
				this.lblConstructionName.Text = Project.Instance.HithermCompactWalls[0].Name;
			} else {
				wallId = Project.Instance.HithermWalls[0].Id;
				this.lblConstructionName.Text = Project.Instance.HithermWalls[0].Name;
			}
			this.txtConstruction.Text = wallId;

			groupBox1.Enabled = !autoGeneration;

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
				defaultHeight = (double)this.numHeight.Value;
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

		public int AfterWallNumber {
			get { return (int)this.numWallId.Value; }
		}

		public CreationTypeEnum CreationType {
			get {
				if (rbPrev.Checked) {
					return CreationTypeEnum.Prev;
				} else if (rbNext.Checked) {
					return CreationTypeEnum.Next;
				} else if (rbAfter.Checked) {
					return CreationTypeEnum.After;
				} else {
					return CreationTypeEnum.Schraege;
				}
			}
		}

		private void btnSelectConstruction_Click(object sender, EventArgs e) {
			SelectHithermWallForm form = new SelectHithermWallForm(this.isCompact);
			if (form.ShowDialog() == DialogResult.OK) {
				this.wallId = form.SelectedWall.Id;
				this.txtConstruction.Text = this.wallId;
				this.lblConstructionName.Text = form.SelectedWall.Name;
			}
			form.Dispose();
		}

		private void rb_CheckedChanged(object sender, EventArgs e) {
			numWallId.Enabled = rbAfter.Checked;
			numWidth.Enabled = !rbSchraege.Checked;
			if (rbSchraege.Checked) {
				numWidth.Value = (decimal)selectedWidth;
			}
			if (rbAfter.Checked) {
				numWallId.MaxValue = wallCount;
			}
		}
	}
}