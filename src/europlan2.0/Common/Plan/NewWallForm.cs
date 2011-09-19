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

		//private static double defaultHeight = 250.0;
		private bool autoGeneration;
		private bool isCompact;
		private string wallId;
		private double selectedWidth = 0;
		private int wallCount = 0;

		public NewWallForm(bool autoGeneration, bool isCompact, double selectedWidth, int wallCount, bool allowSchraege, float defaultHeight) {
			InitializeComponent();

			this.autoGeneration = autoGeneration;
			this.isCompact = isCompact;
			this.selectedWidth = selectedWidth;
			this.wallCount = wallCount;
			rbSchraege.Enabled = allowSchraege;

			this.numWidth.Enabled = !autoGeneration;
			this.numHeight.Value = (decimal)(defaultHeight * 100.0f);

			if (isCompact) {
				wallId = Project.Instance.HithermCompactWalls[0].Id;
				this.lblConstructionName.Text = Project.Instance.HithermCompactWalls[0].Name;
			} else {
				bool found = false;
				foreach (HithermWall hw in Project.Instance.HithermWalls) {
					if (hw.Id == "STW02") {
						found = true;
						wallId = hw.Id;
						this.lblConstructionName.Text = hw.Name;
						break;
					}
				}
				if (!found && Project.Instance.HithermWalls.Count > 0) {
					wallId = Project.Instance.HithermWalls[0].Id;
					this.lblConstructionName.Text = Project.Instance.HithermWalls[0].Name;
				}
			}
			this.txtConstruction.Text = wallId;

			groupBox1.Enabled = !autoGeneration;

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.NewWallForm_Titel;
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"&Abbrechen";
			this.btnOk.Text = EuroplanRes.General_Ok; //"&OK";
			this.lblConstruction.Text = EuroplanRes.NewWallForm_Konstruktion;
			this.lblWidth.Text = EuroplanRes.NewWallForm_Breite;
			this.lblHeight.Text = EuroplanRes.NewWallForm_Hoehe;
			this.label2.Text = EuroplanRes.Unit_Zentimeter;
			this.label3.Text = EuroplanRes.Unit_Zentimeter;
			this.groupBox1.Text = EuroplanRes.NewWallForm_Position;
			this.rbSchraege.Text = EuroplanRes.NewWallForm_Schraege;
			this.rbAfter.Text = EuroplanRes.NewWallForm_NachWandNr;
			this.rbNext.Text = EuroplanRes.NewWallForm_NachWand;
			this.rbPrev.Text = EuroplanRes.NewWallForm_VorWand;
		}

		private void NewWallForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult == DialogResult.OK) {
				//if (txtName.Text == "") {
				//    MessageBox.Show(EuroplanRes.NewPlanForm_KeinBezeichnerText, EuroplanRes.NewPlanForm_KeinBezeichnerTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				//    e.Cancel = true;
				//}
				if (this.numWidth.Enabled && this.Width < 10) {
					MessageBox.Show("Bitte geben Sie für die Breite einen gültigen Wert ein (min. 10cm).", "Wand zu schmal", MessageBoxButtons.OK, MessageBoxIcon.Information);
					e.Cancel = true;
					return;
				}
				//defaultHeight = (double)this.numHeight.Value;
			}

			SettingsKey settings = SettingsFile.Settings["NewWallForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
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