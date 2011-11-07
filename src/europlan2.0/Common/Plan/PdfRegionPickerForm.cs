using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Drawing.Drawing2D;
using WW.Math;

namespace Europlan.Common {

	public partial class PdfRegionPickerForm : Form {
		
		public PdfRegionPickerForm(ImagePlan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.picturePanel.Plan = plan;
			this.picturePanel.ProductPlanner = this.pdfRegionPicker;
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.PdfRegionPickerForm_BereichAuswaehlen;
			this.btnZoomIn.Text = EuroplanRes.Plan_Heranzoomen;
			this.btnZoomIn.ToolTipText = EuroplanRes.Plan_Heranzoomen;
			this.btnZoomOut.Text = EuroplanRes.Plan_Herauszoomen;
			this.btnZoomOut.ToolTipText = EuroplanRes.Plan_Herauszoomen;
			this.btnMove.Text = EuroplanRes.Plan_Verschieben;
			this.btnMove.ToolTipText = EuroplanRes.Plan_Verschieben;
			// TODO this.btnSelectRegion
			this.btnOk.Text = EuroplanRes.General_Uebernehmen;
		}

		private void ImagePlanOptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			this.picturePanel.ApplyChangesToPlan();
			SettingsFile.Update();

			this.DialogResult = DialogResult.OK;
		}

		private void ImagePlanOptionsForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			picturePanel.AddScale(1.1, null);
			picturePanel.Invalidate();
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			picturePanel.AddScale(0.9, null);
			picturePanel.Invalidate();
		}

		private void btnMove_Click(object sender, EventArgs e) {
			picturePanel.Cursor = Cursors.Hand;
			btnMove.Checked = true;
			btnSelectRegion.Checked = false;
			picturePanel.Mode = PlanMode.PM_MOVE;
			pdfRegionPicker.Mode = PdfRegionPicker.PdfRegionPickerMode.DPM_NONE;
			picturePanel.StartPoint = null;
			picturePanel.EndPoint = null;
			picturePanel.Invalidate();
		}

		private void btnOk_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void btnSelectRegion_Click(object sender, EventArgs e) {
			btnMove.Checked = false;
			btnSelectRegion.Checked = true;
			picturePanel.Mode = PlanMode.PM_PLANNER_DRAG;
			pdfRegionPicker.Mode = PdfRegionPicker.PdfRegionPickerMode.DPM_PICK_REGION;
			picturePanel.Invalidate();
		}

		public Nullable<Point2D> TopLeft {
			get { return this.pdfRegionPicker.TopLeft; }
		}

		public Nullable<Point2D> BottomRight {
			get { return this.pdfRegionPicker.BottomRight; }
		}
	}
}