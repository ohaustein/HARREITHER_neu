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
			switch (Product.ConfigPlanMeasureEnum) {
				case Product.PlanMeasureEnum.PM_CENTIMETER:
					this.lblLength.Text = "L‰nge in cm:";
					break;

				case Product.PlanMeasureEnum.PM_MILLIMETER:
					this.lblLength.Text = "L‰nge in mm:";
					break;

				case Product.PlanMeasureEnum.PM_METER:
				default:
					this.lblLength.Text = "L‰nge in m:";
					break;
			}
			this.SetLanguage();
			this.picturePanel.Plan = plan;
			this.picturePanel.LengthChanged += new ImagePanel.LengthChangedEventHandler(picturePanel_LengthChanged);
		}

		void picturePanel_LengthChanged(object sender) {
			if (this.picturePanel.Plan.Measure.HasValue) {
				switch (Product.ConfigPlanMeasureEnum) {
					case Product.PlanMeasureEnum.PM_CENTIMETER:
						txtLength.Text = "" + (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value * Product.ConfigPlanMeasureMultiplier).ToString("0") + "cm";
						break;

					case Product.PlanMeasureEnum.PM_MILLIMETER:
						txtLength.Text = "" + (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value * Product.ConfigPlanMeasureMultiplier).ToString("0") + "mm";
						break;

					case Product.PlanMeasureEnum.PM_METER:
					default:
						txtLength.Text = "" + (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value * Product.ConfigPlanMeasureMultiplier).ToString("0.00") + EuroplanRes.Unit_Meter;
						break;
				}
			} else {
				Nullable<double> length = this.picturePanel.Plan.Measure.HasValue ? this.picturePanel.Length / this.picturePanel.Plan.Measure.Value : (Nullable<double>)null;
				PlanSetMeasureForm psmf = new PlanSetMeasureForm(length);
				if (psmf.ShowDialog() == DialogResult.OK) {
					if (length != psmf.Length) {
						this.unsavedChanges = true;
						this.picturePanel.Plan.Measure = (float)(this.picturePanel.Length / psmf.Length);
						this.txtLength.Text = (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
					}
				} else {
					txtLength.Text = "???";
				}
			}
			lblLength.Visible = true;
			txtLength.Visible = true;
			btnSetLength.Visible = true;
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.ImagePlanOptionsForm_Titel; //"Optionen";
			this.btnRaster.ToolTipText = EuroplanRes.ImagePlanOptionsForm_RasterEin;
			this.lblLength.Text = EuroplanRes.PlanOptionsForm_Leange; //"L‰nge:"
		}

		private void ImagePlanOptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			this.picturePanel.ApplyChangesToPlan();
			SettingsFile.Update();

			if (!this.picturePanel.Plan.Measure.HasValue) {
				DialogResult result = MessageBox.Show(EuroplanRes.PlanOptionsForm_KeinMaﬂstabText, EuroplanRes.PlanOptionsForm_KeinMaﬂstabTitel, MessageBoxButtons.YesNo);
				if (result == DialogResult.Yes) {
					picturePanel.Cursor = Cursors.Cross;
					btnMove.Checked = false;
					btnDistance.Checked = true;
					picturePanel.Mode = PlanMode.PM_PICK_MEASURE;
					e.Cancel = true;
				} else {
					this.DialogResult = DialogResult.Cancel;
					return;
				}
			}
			this.DialogResult = DialogResult.OK;
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
				btnRaster.ToolTipText = EuroplanRes.ImagePlanOptionsForm_RasterAus;
			} else {
				btnRaster.ToolTipText = EuroplanRes.ImagePlanOptionsForm_RasterEin;
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
			picturePanel.Mode = PlanMode.PM_MOVE;
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
			picturePanel.Mode = PlanMode.PM_PICK_MEASURE;
		}

		private void txtLength_TextChanged(object sender, EventArgs e) {
			double len = 0;
			if (Double.TryParse(txtLength.Text, out len)) {
				unsavedChanges = true;
				this.picturePanel.Plan.Measure = (float)(this.picturePanel.Length / len / Product.ConfigPlanMeasureMultiplier);
			}
		}

		private void btnSetLength_Click(object sender, EventArgs e) {
			Nullable<double> length = this.picturePanel.Plan.Measure.HasValue ? this.picturePanel.Length / this.picturePanel.Plan.Measure.Value : (Nullable<double>)null;
			PlanSetMeasureForm psmf = new PlanSetMeasureForm(length);
			if (psmf.ShowDialog() == DialogResult.OK) {
				if (length != psmf.Length) {
					this.unsavedChanges = true;
					this.picturePanel.Plan.Measure = (float)(this.picturePanel.Length / psmf.Length);
					//this.txtLength.Text = (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
					switch (Product.ConfigPlanMeasureEnum) {
						case Product.PlanMeasureEnum.PM_CENTIMETER:
							txtLength.Text = "" + (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value * Product.ConfigPlanMeasureMultiplier).ToString("0") + "cm";
							break;

						case Product.PlanMeasureEnum.PM_MILLIMETER:
							txtLength.Text = "" + (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value * Product.ConfigPlanMeasureMultiplier).ToString("0") + "mm";
							break;

						case Product.PlanMeasureEnum.PM_METER:
						default:
							txtLength.Text = "" + (this.picturePanel.Length / this.picturePanel.Plan.Measure.Value * Product.ConfigPlanMeasureMultiplier).ToString("0.00") + EuroplanRes.Unit_Meter;
							break;
					}
				}
			}
		}

		private void btnOk_Click(object sender, EventArgs e) {
			this.Close();
		}

	}
}