using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Drawing.Drawing2D;
using WW.Cad.IO;
using WW.Cad.Model.Tables;
using WW.Math;
using WW.Cad.Model;

namespace Europlan.Common {

	public partial class CadPlanOptionsForm : Form {

		private CadPlan plan;
		private bool unsavedChanges = false;
		private double length = 0;

		private class LayerListViewItem : ListViewItem {

			private DxfLayer layer = null;

			public LayerListViewItem(DxfLayer layer) : base(layer.Name) {
				this.layer = layer;
				this.Checked = layer.Enabled;
			}

			public DxfLayer Layer {
				get { return this.layer; }
				set { this.layer = value; }
			}
		}

		public CadPlanOptionsForm(CadPlan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.plan = plan;
			DxfModel model = this.plan.LoadModel(); ;
			foreach (DxfLayer layer in model.Layers) {
				this.lstLayers.Items.Add(new LayerListViewItem(layer));
			}
			this.cadPanel.Plan = plan;
			this.cadPanel.PlanScale = plan.Scale;
			this.cadPanel.PlanTranslation = new Vector2D(plan.TranslationX, plan.TranslationY);
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.CadPlanOptionsForm_Titel; //"Optionen";
			this.lblLength.Text = EuroplanRes.PlanOptionsForm_Leange; //"L‰nge:"
		}

		private void ImagePlanOptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["CadPlanOptionsForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();

			if (!plan.Measure.HasValue) {
				DialogResult result = MessageBox.Show(EuroplanRes.PlanOptionsForm_KeinMaﬂstabText, EuroplanRes.PlanOptionsForm_KeinMaﬂstabTitel, MessageBoxButtons.YesNo);
				if (result == DialogResult.Yes) {
					this.btnMove.Checked = false;
					this.btnDistance.Checked = true;
					this.cadPanel.Mode = PlanMode.PM_PICK_MEASURE;
					txtLength.Text = "";
					e.Cancel = true;
				} else {
					this.DialogResult = DialogResult.Cancel;
					return;
				}
			}

			this.plan.Scale = this.cadPanel.PlanScale;
			this.plan.TranslationX = this.cadPanel.PlanTranslation.X;
			this.plan.TranslationY = this.cadPanel.PlanTranslation.Y;

			this.plan.DisabledLayers.Clear();
			foreach (DxfLayer layer in this.cadPanel.Model.Layers) {
				if (!layer.Enabled) {
					this.plan.DisabledLayers.Add(layer.Name);
				}
			}
			this.DialogResult = DialogResult.OK;
		}

		private void ImagePlanOptionsForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["CadPlanOptionsForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			this.lstLayers.ItemChecked += new ItemCheckedEventHandler(lstLayers_ItemChecked);
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges || this.cadPanel.UnsavedChanges; }
		}

		private void txtLength_TextChanged(object sender, EventArgs e) {
			double len = 0;
			if (Double.TryParse(txtLength.Text, out len)) {
				unsavedChanges = true;
				plan.Measure = (float)(length / len);
			}
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			cadPanel.AddScale(1.1, null);
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			cadPanel.AddScale(0.9, null);
		}

		private void lstLayers_ItemChecked(object sender, ItemCheckedEventArgs e) {
			LayerListViewItem item = e.Item as LayerListViewItem;
			if (item != null) {
				this.unsavedChanges = true;
				item.Layer.Enabled = item.Checked;
				this.cadPanel.RecreateDrawables();
			}
		}

		private void btnDistance_Click(object sender, EventArgs e) {
			this.btnMove.Checked = false;
			this.btnDistance.Checked = true;
			this.cadPanel.Mode = PlanMode.PM_PICK_MEASURE;
			txtLength.Text = "";
			/*txtLength.Visible = true;
			lblLength.Visible = true;*/
		}

		private void btnMove_Click(object sender, EventArgs e) {
			this.btnDistance.Checked = false;
			this.btnMove.Checked = true;
			this.cadPanel.Mode = PlanMode.PM_MOVE;
			txtLength.Visible = false;
			lblLength.Visible = false;
			btnSetLength.Visible = false;
		}

		private void cadPanel_EndPointSelected(object sender, CadPanel.EndPointSelectedArgs e) {
			this.length = e.Length;
			if (this.plan.Measure.HasValue) {
				this.txtLength.Text = (this.length / this.plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
			} else {
				Nullable<double> length = plan.Measure.HasValue ? this.length / this.plan.Measure.Value : (Nullable<double>)null;
				PlanSetMeasureForm psmf = new PlanSetMeasureForm(length);
				if (psmf.ShowDialog() == DialogResult.OK) {
					if (length != psmf.Length) {
						this.unsavedChanges = true;
						plan.Measure = (float)(this.length / psmf.Length);
						this.txtLength.Text = (this.length / this.plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
					}
				} else {
					this.txtLength.Text = "???";
				}
			}
			this.lblLength.Visible = true;
			this.txtLength.Visible = true;
			this.btnSetLength.Visible = true;
		}

		private void cadPanel_StartPointSelected(object sender, CadPanel.StartPointSelectedArgs e) {
			/*this.txtLength.Enabled = false;
			this.txtLength.Text = "";*/
			this.txtLength.Visible = false;
			this.lblLength.Visible = false;
			this.btnSetLength.Visible = false;
		}

		private void btnSetLength_Click(object sender, EventArgs e) {
			Nullable<double> length = plan.Measure.HasValue ? this.length / this.plan.Measure.Value : (Nullable<double>)null;
			PlanSetMeasureForm psmf = new PlanSetMeasureForm(length);
			if (psmf.ShowDialog() == DialogResult.OK) {
				if (length != psmf.Length) {
					this.unsavedChanges = true;
					plan.Measure = (float)(this.length / psmf.Length);
					this.txtLength.Text = (this.length / this.plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
				}
			}
		}
	}
}