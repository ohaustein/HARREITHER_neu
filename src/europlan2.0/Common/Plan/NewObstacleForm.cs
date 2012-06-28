using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class NewObstacleForm : Form {

		public NewObstacleForm() {
			InitializeComponent();
			cmbType.DataSource = Enum.GetValues(typeof(Europlan.Common.GraphicalWallObstacle.ObstacleTypeEnum));
			UpdateControls();
			this.SetLanguage();
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.NewObstacleForm_Titel;
			this.btnCancel.Text = EuroplanRes.General_Abbrechen; //"&Abbrechen";
			this.btnOk.Text = EuroplanRes.General_Ok; //"&OK";

			this.label1.Text = EuroplanRes.Unit_Zentimeter;
			this.lblHeightOffset.Text = EuroplanRes.NewObstacleForm_Parapethoehe;
			this.label3.Text = EuroplanRes.Unit_Zentimeter;
			this.label2.Text = EuroplanRes.Unit_Zentimeter;
			this.lblHeight.Text = EuroplanRes.NewObstacleForm_Hoehe;
			this.lblWidth.Text = EuroplanRes.NewObstacleForm_Breite;
			this.lblConstruction.Text = EuroplanRes.NewObstacleForm_Typ;

		}

		private void NewObstacleForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewObstacleForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
			if (this.DialogResult == DialogResult.OK) {

			}
		}

		private void NewObstacleForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["NewObstacleForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		public new double Width {
			get { return (double)this.numWidth.Value; }
			set { this.numWidth.Value = (decimal)value; }
		}

		public new double Height {
			get { return (double)this.numHeight.Value; }
			set { this.numHeight.Value = (decimal)value; }
		}

		public double HeightOffset {
			get { return (double)this.numHeightOffset.Value; }
			set { this.numHeightOffset.Value = (decimal)value; }
		}

		public Europlan.Common.GraphicalWallObstacle.ObstacleTypeEnum ObstacleType {
			get { return (Europlan.Common.GraphicalWallObstacle.ObstacleTypeEnum)cmbType.SelectedValue; }
		}

		private void cmbType_SelectedValueChanged(object sender, EventArgs e) {
			UpdateControls();
		}

		private void UpdateControls() {
			if (ObstacleType == Europlan.Common.GraphicalWallObstacle.ObstacleTypeEnum.Door) {
				numHeightOffset.Visible = false;
				lblHeightOffset.Visible = false;
			} else {
				numHeightOffset.Visible = true;
				lblHeightOffset.Visible = true;
			}
		}

	}
}