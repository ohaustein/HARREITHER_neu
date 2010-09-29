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

	public partial class RoomPickerForm : Form {

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

		public RoomPickerForm(Plan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.panel.Plan = plan;
			this.panel.Mode = PlanMode.PM_MOVE;
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
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges || this.panel.UnsavedChanges || this.roomPicker.UnsavedChanges; }
		}

		public List<Point2D> RoomCoordinates {
			get { return this.roomPicker.RoomCoordinates; }
			set { this.roomPicker.RoomCoordinates = value; }
		}

		public List<List<Point2D>> UnusedCoordinates {
			get { return this.roomPicker.UnusedCoordinates; }
			set { this.roomPicker.UnusedCoordinates = value; }
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.panel.AddScale(1.1, null);
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.panel.AddScale(0.9, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_NONE;
				this.panel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnPickRoom_Click(object sender, EventArgs e) {
			if (!btnPickRoom.Checked) {
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_PICK_ROOM;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnPickUnused_Click(object sender, EventArgs e) {
			if (!btnPickUnused.Checked) {
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_PICK_UNUSED;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnDelUnused_Click(object sender, EventArgs e) {
			if (!btnDelUnused.Checked) {
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_DEL_UNUSED;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.panel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnPickRoom.Checked = false;
				this.btnPickUnused.Checked = false;
				this.btnDelUnused.Checked = false;
			} else if (this.panel.Mode == PlanMode.PM_PLANNER_CLICK) {
				if (this.roomPicker.Mode == RoomPicker.RoomPickerMode.RPM_PICK_ROOM) {
					this.btnMove.Checked = false;
					this.btnPickRoom.Checked = true;
					this.btnPickUnused.Checked = false;
					this.btnDelUnused.Checked = false;
				} else if (this.roomPicker.Mode == RoomPicker.RoomPickerMode.RPM_PICK_UNUSED) {
					this.btnMove.Checked = false;
					this.btnPickRoom.Checked = false;
					this.btnPickUnused.Checked = true;
					this.btnDelUnused.Checked = false;
				} else if (this.roomPicker.Mode == RoomPicker.RoomPickerMode.RPM_DEL_UNUSED) {
					this.btnMove.Checked = false;
					this.btnPickRoom.Checked = false;
					this.btnPickUnused.Checked = false;
					this.btnDelUnused.Checked = true;
				}
			} else {
				this.btnMove.Checked = false;
				this.btnPickRoom.Checked = false;
				this.btnPickUnused.Checked = false;
				this.btnDelUnused.Checked = false;
			}
		}

		private void roomPicker_ModeChanged(object sender, EventArgs e) {
			this.UpdateButtons();
		}
	}
}