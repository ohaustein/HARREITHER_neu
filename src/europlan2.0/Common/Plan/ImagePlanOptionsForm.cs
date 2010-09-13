using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Drawing.Drawing2D;

namespace Europlan.Common {

	public partial class ImagePlanOptionsForm : Form {
		
		private bool unsavedChanges = false;
		
		public ImagePlanOptionsForm(ImagePlan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.picturePanel.Plan = plan;
			this.picturePanel.LengthChanged += new PicturePanel.LengthChangedEventHandler(picturePanel_LengthChanged);
		}

		void picturePanel_LengthChanged(object sender) {
			if (this.picturePanel.Plan.Measure.HasValue) {
				txtLength.Text = "" + (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
			} else {
				txtLength.Text = "???";
			}
			lblLength.Visible = true;
			txtLength.Visible = true;
			btnSetLength.Visible = true;
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.ImagePlanOptionsForm_Titel; //"Optionen";
			this.lblLength.Text = EuroplanRes.PlanOptionsForm_Leange; //"Länge:"
		}

		private void ImagePlanOptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			this.picturePanel.ApplyChangesToPlan();
			SettingsFile.Update();

		}

		private void ImagePlanOptionsForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void btnRotateLeft_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			this.picturePanel.Angle -= 5;
			picturePanel.Invalidate();
		}
		
		private void btnRotateLeftSmall_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			this.picturePanel.Angle -= 0.1f;
			picturePanel.Invalidate();
		}

		private void btnRotateRight_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			this.picturePanel.Angle += 5;
			picturePanel.Invalidate();
		}

		private void btnRotateRightSmall_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			this.picturePanel.Angle += 0.1f;
			picturePanel.Invalidate();
		}

		public bool UnsavedChanges {
			get { return unsavedChanges || picturePanel.UnsavedChanges; }
		}

		private void btnRaster_Click(object sender, EventArgs e) {
			picturePanel.ShowRaster = !picturePanel.ShowRaster;
			btnRaster.Checked = picturePanel.ShowRaster;
			if (picturePanel.ShowRaster) {
				btnRaster.Text = EuroplanRes.ImagePlanOptionsForm_RasterAus;
			} else {
				btnRaster.Text = EuroplanRes.ImagePlanOptionsForm_RasterEin;
			}
			picturePanel.Invalidate();
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			//plan.Scale *= 1.1f;
			picturePanel.AddScale(1.1, null);
			picturePanel.Invalidate();
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			//plan.Scale *= 0.9f;
			picturePanel.AddScale(0.9, null);
			picturePanel.Invalidate();
		}

		private void btnMove_Click(object sender, EventArgs e) {
			picturePanel.Cursor = Cursors.Hand;
			btnMove.Checked = true;
			btnDistance.Checked = false;
			txtLength.Visible = false;
			lblLength.Visible = false;
			btnSetLength.Visible = false;
			picturePanel.MoveMode = true;
			picturePanel.StartPoint = null;
			picturePanel.EndPoint = null;
			picturePanel.Invalidate();
		}

		private void btnDistance_Click(object sender, EventArgs e) {
			picturePanel.Cursor = Cursors.Cross;
			btnMove.Checked = false;
			btnDistance.Checked = true;
			//txtLength.Visible = true;
			//lblLength.Visible = true;
		    //txtLength.Enabled = plan.Measure.HasValue;
			//txtLength.Text = "";
			picturePanel.MoveMode = false;
		}

		private void txtLength_TextChanged(object sender, EventArgs e) {
			double len = 0;
			if (Double.TryParse(txtLength.Text, out len)) {
				unsavedChanges = true;
				this.picturePanel.Plan.Measure = (float)(this.picturePanel.Length / len);
			}
		}

		private void btnSetLength_Click(object sender, EventArgs e) {
			Nullable<double> length = this.picturePanel.Plan.Measure.HasValue ? this.picturePanel.Length / this.picturePanel.Plan.Measure.Value : (Nullable<double>)null;
			PlanSetMeasureForm psmf = new PlanSetMeasureForm(length);
			if (psmf.ShowDialog() == DialogResult.OK) {
				if (length != psmf.Length) {
					this.unsavedChanges = true;
					this.picturePanel.Plan.Measure = (float)(this.picturePanel.Length / psmf.Length);
					this.txtLength.Text = (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
				}
			}
		}

	}
}