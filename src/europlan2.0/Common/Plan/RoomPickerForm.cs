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

		public RoomPickerForm(Room room, bool isCeiling) {
			InitializeComponent();
			this.SetLanguage();
			this.roomPicker.Room = room;
			this.roomPicker.IsCeiling = isCeiling;
			this.panel.Plan = room.AssociatedPlan;
			this.panel.Mode = PlanMode.PM_MOVE;
			switch (Product.ConfigPlanMeasureEnum) {
				case Product.PlanMeasureEnum.PM_CENTIMETER:
					this.lblDistanceXUnit.Text = "cm";
					this.lblDistanceYUnit.Text = "cm";
					this.lblSizeXUnit.Text = "cm";
					this.lblSizeYUnit.Text = "cm";
					this.numDistanceX.EditType = NumericBox.NumericEditType.DIST_CM;
					this.numDistanceY.EditType = NumericBox.NumericEditType.DIST_CM;
					this.numSizeX.EditType = NumericBox.NumericEditType.LENGTH_CM;
					this.numSizeY.EditType = NumericBox.NumericEditType.LENGTH_CM;
					break;

				case Product.PlanMeasureEnum.PM_MILLIMETER:
					this.lblDistanceXUnit.Text = "mm";
					this.lblDistanceYUnit.Text = "mm";
					this.lblSizeXUnit.Text = "mm";
					this.lblSizeYUnit.Text = "mm";
					this.numDistanceX.EditType = NumericBox.NumericEditType.DIST_MM;
					this.numDistanceY.EditType = NumericBox.NumericEditType.DIST_MM;
					this.numSizeX.EditType = NumericBox.NumericEditType.LENGTH_MM;
					this.numSizeY.EditType = NumericBox.NumericEditType.LENGTH_MM;
					break;

				case Product.PlanMeasureEnum.PM_METER:
				default:
					this.lblDistanceXUnit.Text = "m";
					this.lblDistanceYUnit.Text = "m";
					this.lblSizeXUnit.Text = "m";
					this.lblSizeYUnit.Text = "m";
					this.numDistanceX.EditType = NumericBox.NumericEditType.DIST_M;
					this.numDistanceY.EditType = NumericBox.NumericEditType.DIST_M;
					this.numSizeX.EditType = NumericBox.NumericEditType.LENGTH;
					this.numSizeY.EditType = NumericBox.NumericEditType.LENGTH;
					break;
			}
		}

		public PlanPanel Panel {
			get {
				return this.panel;
			}
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.CadPlanOptionsForm_Titel; //"Optionen";
		}

		private void RoomPickerForm_FormClosing(object sender, FormClosingEventArgs e) {
			//this.plan.Scale = this.cadPanel.PlanScale;
			//this.plan.TranslationX = this.cadPanel.PlanTranslation.X;
			//this.plan.TranslationY = this.cadPanel.PlanTranslation.Y;

			//this.plan.DisabledLayers.Clear();
			//foreach (DxfLayer layer in this.cadPanel.Model.Layers) {
			//    if (!layer.Enabled) {
			//        this.plan.DisabledLayers.Add(layer.Name);
			//    }
			//}
			ConnectionPlanner.ReGenerateConnectionPipes(this.roomPicker.Room.AssociatedFloor);

			SettingsKey settings = SettingsFile.Settings["CadPlanRoomPickerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void RoomPickerForm_Load(object sender, EventArgs e) {
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
				this.panUnheatedArea.Visible = false;
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_NONE;
				this.panel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void btnPickRoom_Click(object sender, EventArgs e) {
			if (!btnPickRoom.Checked) {
				this.panUnheatedArea.Visible = false;
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_PICK_ROOM;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnPickUnused_Click(object sender, EventArgs e) {
			if (!btnPickUnused.Checked) {
				this.cbUnheatedGraphical.Checked = true;
				this.panUnheatedArea.Visible = true;
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_PICK_UNUSED;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnDelUnused_Click(object sender, EventArgs e) {
			if (!btnDelUnused.Checked) {
				this.panUnheatedArea.Visible = false;
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_DEL_UNUSED;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnAddExpansionGap_Click(object sender, EventArgs e) {
			if (!btnAddExpansionGap.Checked) {
				this.panUnheatedArea.Visible = false;
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_ADD_EXPANSION_GAP;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.UpdateButtons();
			}
		}

		private void btnRemoveExpansionGap_Click(object sender, EventArgs e) {
			if (!btnRemoveExpansionGap.Checked) {
				this.panUnheatedArea.Visible = false;
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_DEL_EXPANSION_GAP;
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
				this.btnAddExpansionGap.Checked = false;
				this.btnRemoveExpansionGap.Checked = false;
			} else if (this.panel.Mode == PlanMode.PM_PLANNER_CLICK) {
				if (this.roomPicker.Mode == RoomPicker.RoomPickerMode.RPM_PICK_ROOM) {
					this.btnMove.Checked = false;
					this.btnPickRoom.Checked = true;
					this.btnPickUnused.Checked = false;
					this.btnDelUnused.Checked = false;
					this.btnAddExpansionGap.Checked = false;
					this.btnRemoveExpansionGap.Checked = false;
				} else if (this.roomPicker.Mode == RoomPicker.RoomPickerMode.RPM_PICK_UNUSED) {
					this.btnMove.Checked = false;
					this.btnPickRoom.Checked = false;
					this.btnPickUnused.Checked = true;
					this.btnDelUnused.Checked = false;
					this.btnAddExpansionGap.Checked = false;
					this.btnRemoveExpansionGap.Checked = false;
				} else if (this.roomPicker.Mode == RoomPicker.RoomPickerMode.RPM_DEL_UNUSED) {
					this.btnMove.Checked = false;
					this.btnPickRoom.Checked = false;
					this.btnPickUnused.Checked = false;
					this.btnDelUnused.Checked = true;
					this.btnAddExpansionGap.Checked = false;
					this.btnRemoveExpansionGap.Checked = false;
				} else if (this.roomPicker.Mode == RoomPicker.RoomPickerMode.RPM_ADD_EXPANSION_GAP) {
					this.btnMove.Checked = false;
					this.btnPickRoom.Checked = false;
					this.btnPickUnused.Checked = false;
					this.btnDelUnused.Checked = false;
					this.btnAddExpansionGap.Checked = true;
					this.btnRemoveExpansionGap.Checked = false;
				} else if (this.roomPicker.Mode == RoomPicker.RoomPickerMode.RPM_DEL_EXPANSION_GAP) {
					this.btnMove.Checked = false;
					this.btnPickRoom.Checked = false;
					this.btnPickUnused.Checked = false;
					this.btnDelUnused.Checked = false;
					this.btnAddExpansionGap.Checked = false;
					this.btnRemoveExpansionGap.Checked = true;
				}
			} else {
				this.btnMove.Checked = false;
				this.btnPickRoom.Checked = false;
				this.btnPickUnused.Checked = false;
				this.btnDelUnused.Checked = false;
				this.btnAddExpansionGap.Checked = false;
				this.btnRemoveExpansionGap.Checked = false;
			}
		}

		private void roomPicker_ModeChanged(object sender, EventArgs e) {
			this.UpdateButtons();
		}

		private int ignoreReferencePoint = 0;
		private void cbReferencePoint_CheckedChanged(object sender, EventArgs e) {
			if (ignoreReferencePoint == 0) {
				if (!cbReferencePoint.Checked && this.roomPicker.ReferencePoint == null) {
					ignoreReferencePoint++;
					cbReferencePoint.Checked = true;
					ignoreReferencePoint--;
					MessageBox.Show("Es muss ein Referenzpunk gewählt werden um unbeheizte Flächen definieren zu können", "Kein Referenzpunkt gewählt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				} else {
					this.grpDistance.Enabled = !this.cbReferencePoint.Checked;
					this.grpSize.Enabled = !this.cbReferencePoint.Checked;
					this.btnAddUnheatedArea.Enabled = !this.cbReferencePoint.Checked;
				}
				this.cbEnterArea.Checked = !this.cbReferencePoint.Checked;
			}
			if (this.cbUnheatedTextual.Checked) {
				if (this.cbReferencePoint.Checked) {
					this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_SET_REFERENCE;
					this.panel.InvalidateGraphics();
				} else if (this.cbEnterArea.Checked) {
					this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_ADD_UNUSED;
					this.panel.InvalidateGraphics();
				}
			}
		}


		private void cbEnterArea_CheckedChanged(object sender, EventArgs e) {
			if (ignoreReferencePoint == 0) {
				this.cbReferencePoint.Checked = !this.cbEnterArea.Checked;
			}
			if (this.cbUnheatedTextual.Checked) {
				if (this.cbReferencePoint.Checked) {
					this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_SET_REFERENCE;
					this.panel.InvalidateGraphics();
				} else if (this.cbEnterArea.Checked) {
					this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_ADD_UNUSED;
					this.panel.InvalidateGraphics();
				}
			}
		}

		private void cbUnheatedGraphical_CheckedChanged(object sender, EventArgs e) {
			if (this.cbUnheatedGraphical.Checked) {
				this.numDistanceX.Value = 0;
				this.numDistanceY.Value = 0;
				this.numSizeX.Value = 0;
				this.numSizeY.Value = 0;
				this.roomPicker.NewUnheatedAreaPos = new Point2D(0, 0);
				this.roomPicker.NewUnheatedAreaSize = new Size2D(0, 0);
				this.cbReferencePoint.Enabled = false;
				this.cbEnterArea.Enabled = false;
				this.grpDistance.Enabled = false;
				this.grpSize.Enabled = false;
				this.btnAddUnheatedArea.Enabled = false;
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_PICK_UNUSED;
				ignoreReferencePoint++;
				this.cbReferencePoint.Checked = false;
				this.cbEnterArea.Checked = false;
				ignoreReferencePoint--;
			}
		}

		private void cbUnheatedTextual_CheckedChanged(object sender, EventArgs e) {
			if (this.cbUnheatedTextual.Checked) {
				this.cbReferencePoint.Checked = this.roomPicker.ReferencePoint == null;
				this.cbReferencePoint.Enabled = true;
				this.cbEnterArea.Enabled = true;
				//this.grpDistance.Enabled = false;
				//this.grpSize.Enabled = false;
				//this.btnAddUnheatedArea.Enabled = false;
				this.roomPicker.Mode = RoomPicker.RoomPickerMode.RPM_SET_REFERENCE;
				ignoreReferencePoint++;
				this.cbReferencePoint.Checked = this.roomPicker.ReferencePoint == null;
				this.cbEnterArea.Checked = !this.cbReferencePoint.Checked;
				this.grpDistance.Enabled = this.cbEnterArea.Checked;
				this.grpSize.Enabled = this.cbEnterArea.Checked;
				this.btnAddUnheatedArea.Enabled = false;
				ignoreReferencePoint--;
			}
			this.Panel.InvalidateGraphics();
		}

		private void numDistance_ValueChanged(object sender, EventArgs e) {
			this.roomPicker.NewUnheatedAreaPos = new Point2D((double)this.numDistanceX.Value / Product.ConfigPlanMeasureMultiplier, (double)this.numDistanceY.Value / Product.ConfigPlanMeasureMultiplier);
			this.btnAddUnheatedArea.Enabled = this.roomPicker.IsNewUnheatedAreaValid;
			this.Panel.InvalidateGraphics();
		}

		private void numSize_ValueChanged(object sender, EventArgs e) {
			this.roomPicker.NewUnheatedAreaSize = new Size2D((double)this.numSizeX.Value / Product.ConfigPlanMeasureMultiplier, (double)this.numSizeY.Value / Product.ConfigPlanMeasureMultiplier);
			this.btnAddUnheatedArea.Enabled = this.roomPicker.IsNewUnheatedAreaValid;
			this.Panel.InvalidateGraphics();
		}

		private void btnAddUnheatedArea_Click(object sender, EventArgs e) {
			this.roomPicker.AddNewUnheatedArea();
			this.numDistanceX.Value = 0;
			this.numDistanceY.Value = 0;
			this.numSizeX.Value = 0;
			this.numSizeY.Value = 0;
		}

		private void btnOk_Click(object sender, EventArgs e) {
			this.Close();
		}
	}
}