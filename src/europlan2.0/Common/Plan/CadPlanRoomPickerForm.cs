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

	public partial class CadPlanRoomPickerForm : Form {

		private CadPlan plan;
		private bool unsavedChanges = false;

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

		public CadPlanRoomPickerForm(CadPlan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.plan = plan;
			DxfModel model;
			if (this.plan.AbsoluteFileName.EndsWith(".dwg", StringComparison.InvariantCultureIgnoreCase)) {
				model = DwgReader.Read(this.plan.AbsoluteFileName);
			} else {
				model = DxfReader.Read(this.plan.AbsoluteFileName);
			}
			foreach (DxfLayer layer in model.Layers) {
				layer.Enabled = !plan.DisabledLayers.Contains(layer.Name);
				this.lstLayers.Items.Add(new LayerListViewItem(layer));
			}
			this.cadPanel.Model = model;
			this.cadPanel.PlanScale = plan.Scale;
			this.cadPanel.PlanTranslation = new Vector2D(plan.TranslationX, plan.TranslationY);
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.CadPlanOptionsForm_Titel; //"Optionen";
		}

		private void ImagePlanOptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
			//this.plan.Scale = this.cadPanel.PlanScale;
			//this.plan.TranslationX = this.cadPanel.PlanTranslation.X;
			//this.plan.TranslationY = this.cadPanel.PlanTranslation.Y;

			//this.plan.DisabledLayers.Clear();
			//foreach (DxfLayer layer in this.cadPanel.Model.Layers) {
			//    if (!layer.Enabled) {
			//        this.plan.DisabledLayers.Add(layer.Name);
			//    }
			//}

			SettingsKey settings = SettingsFile.Settings["CadPlanRoomPickerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void ImagePlanOptionsForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["CadPlanRoomPickerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			this.lstLayers.ItemChecked += new ItemCheckedEventHandler(lstLayers_ItemChecked);
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges || this.cadPanel.UnsavedChanges; }
		}

		public List<PointF> RoomCoordinates {
			get { return cadPanel.RoomCoordinates; }
			set { cadPanel.RoomCoordinates = value; }
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
				//this.unsavedChanges = true;
				item.Layer.Enabled = item.Checked;
				this.cadPanel.RecreateDrawables();
			}
		}

		private void btnMove_Click(object sender, EventArgs e) {
			this.btnPick.Checked = false;
			this.btnMove.Checked = true;
			this.cadPanel.MoveMode = true;
			this.cadPanel.RoomPickerMode = false;
		}

		private void btnPick_Click(object sender, EventArgs e) {
			if (!btnPick.Checked) {
				this.btnMove.Checked = false;
				this.btnPick.Checked = true;
				this.cadPanel.MoveMode = false;
				this.cadPanel.RoomPickerMode = true;
			}
		}

	}
}