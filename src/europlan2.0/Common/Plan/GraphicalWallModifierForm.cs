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

	public partial class GraphicalWallModifierForm : Form {

		private bool unsavedChanges = false;
		private bool updateOngoing = false;
		private GraphicalWall selectedWall = null;

		public GraphicalWallModifierForm(Room room) {
			InitializeComponent();
			this.SetLanguage();
			this.graphicalWallModifier.Room = room;
			this.panel.Plan = room.AssociatedPlan;
			this.graphicalWallModifier.Mode = GraphicalWallModifier.GraphicalWallModifierMode.GWM_PICK;
			this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
			this.UpdateControls();
			this.DialogResult = DialogResult.Ignore;
		}

		public PlanPanel Panel {
			get {
				return this.panel;
			}
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.GraphicalWallModifierForm_Titel;
			this.btnZoomIn.Text = EuroplanRes.Plan_Heranzoomen;
			this.btnZoomOut.Text = EuroplanRes.Plan_Herauszoomen;
			this.btnMove.Text = EuroplanRes.Plan_Verschieben;
			this.btnPickWall.Text = EuroplanRes.GraphicalWallModifierForm_WandWaehlen;
			this.btnObstacle.Text = EuroplanRes.GraphicalWallModifierForm_FensterTueren;
			this.btnOk.Text = EuroplanRes.General_Ok;
			this.btnCancel.Text = EuroplanRes.General_Abbrechen;
			this.groupBox2.Text = EuroplanRes.GraphicalWallModifierForm_Groesse;
			this.label3.Text = EuroplanRes.Unit_Zentimeter;
			this.lblWallVertical.Text = EuroplanRes.GraphicalWallModifierForm_Senkrecht;
			this.label2.Text = EuroplanRes.Unit_Zentimeter;
			this.lblWallHorizontal.Text = EuroplanRes.GraphicalWallModifierForm_Waagrecht;
			this.groupBox1.Text = EuroplanRes.GraphicalWallModifierForm_Konstruktion;
			this.chkStartWall.Text = EuroplanRes.GraphicalWallModifierForm_ErsteWand;
			this.chkEnable.Text = EuroplanRes.GraphicalWallModifierForm_Erzeugen;
		}

		private void GraphicalWallModifierForm_FormClosing(object sender, FormClosingEventArgs e) {
			bool create = false;
			if (this.DialogResult == DialogResult.Cancel) {
				if (e.CloseReason == CloseReason.UserClosing) {
					DialogResult result = MessageBox.Show(EuroplanRes.GraphicalWallModifierForm_WaendeUebernehmenText, EuroplanRes.GraphicalWallModifierForm_WaendeUebernehmenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (result == DialogResult.Yes) {
						create = true;
					} 
				} 
			} else if (this.DialogResult == DialogResult.OK) {
				create = true;
			} 
			if (create) {
				List<GraphicalWall> toDelete = new List<GraphicalWall>();
				foreach (GraphicalWall wall in graphicalWallModifier.Room.Walls) {
					if (!wall.Enabled) {
						toDelete.Add(wall);
					}
				}
				foreach (GraphicalWall wall in toDelete) {
					graphicalWallModifier.Room.Walls.Remove(wall);
				}
			} else {
				graphicalWallModifier.Room.Walls.Clear();
			}

			SettingsKey settings = SettingsFile.Settings["GraphicalWallModifierForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		private void GraphicalWallModifierForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["GraphicalWallModifierForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges || this.panel.UnsavedChanges || this.graphicalWallModifier.UnsavedChanges ; }
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.panel.AddScale(1.1, null);
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.panel.AddScale(0.9, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				this.graphicalWallModifier.Mode = GraphicalWallModifier.GraphicalWallModifierMode.GWM_NONE;
				this.panel.Mode = PlanMode.PM_MOVE;
				this.panel.InvalidateGraphics();
				this.UpdateControls();
			}
		}

		private void btnPickWall_Click(object sender, EventArgs e) {
			if (!btnPickWall.Checked) {
				this.graphicalWallModifier.Mode = GraphicalWallModifier.GraphicalWallModifierMode.GWM_PICK;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.panel.InvalidateGraphics();
				this.UpdateControls();
			}
		}

		private void btnObstacle_Click(object sender, EventArgs e) {
			if (!btnObstacle.Checked) {
				this.graphicalWallModifier.Mode = GraphicalWallModifier.GraphicalWallModifierMode.GWM_OBSTACLE;
				this.panel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.panel.InvalidateGraphics();
				this.UpdateControls();
			}
		}

		private void UpdateControls() {
			this.btnMove.Checked = false;
			this.btnPickWall.Checked = false;
			this.btnObstacle.Checked = false;
			this.panelWall.Visible = true;

			if (this.panel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.panelWall.Visible = false;
			} else {
				if (this.graphicalWallModifier.Mode == GraphicalWallModifier.GraphicalWallModifierMode.GWM_PICK) {
					this.btnPickWall.Checked = true;
					if (selectedWall != null) {
						panelWall.Enabled = true;
						chkEnable.Checked = selectedWall.Enabled;
						chkStartWall.Enabled = selectedWall.Enabled;
						chkStartWall.Checked = graphicalWallModifier.Room.Walls.IndexOf(selectedWall) == 0;
						txtWallConstruction.Text = selectedWall.WallId;
						numWallHorizontal.Value = (decimal)selectedWall.GetWallWidth() * 100;
						numWallVertical.Value = (decimal)selectedWall.GetWallHeight() * 100;
					} else {
						panelWall.Enabled = false;
						txtWallConstruction.Text = "";
						numWallHorizontal.Value = 0;
						numWallVertical.Value = 0;
					}
				} else if (this.graphicalWallModifier.Mode == GraphicalWallModifier.GraphicalWallModifierMode.GWM_OBSTACLE) {
					this.btnObstacle.Checked = true;
				}
			}
		}

		private void graphicalWallModifier_ModeChanged(object sender, EventArgs e) {
			this.UpdateControls();
		}

		private void graphicalWallModifier_ObjectSelected(object sender, GraphicalWallModifier.SelectedObjectArgs e) {
			updateOngoing = true;
			if (e.SelectedObject != null) {
				if (e.SelectedObject is GraphicalWall) {
					selectedWall = e.SelectedObject as GraphicalWall;
				}
			} else {
				selectedWall = null;
				
			}
			UpdateControls();
			updateOngoing = false;
		}

		private void chkEnable_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (selectedWall != null) {
					if (chkEnable.Checked) {
						selectedWall.Enabled = true;
					} else {
						selectedWall.Enabled = false;
						if (graphicalWallModifier.Room.Walls.IndexOf(selectedWall) == 0) {
							for (int i = 1; i < graphicalWallModifier.Room.Walls.Count; i++ ) {
								if (graphicalWallModifier.Room.Walls[i].Enabled) {
									List<GraphicalWall> walls = graphicalWallModifier.Room.Walls.GetRange(0, i);
									graphicalWallModifier.Room.Walls.RemoveRange(0, i);
									graphicalWallModifier.Room.Walls.AddRange(walls);
									break;
								}
							}
						}
					}
					panel.InvalidateGraphics();
				}
			}
		}

		private void chkStartWall_CheckedChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (chkStartWall.Checked && selectedWall != null) {
					int index = graphicalWallModifier.Room.Walls.IndexOf(selectedWall);
					List<GraphicalWall> walls = graphicalWallModifier.Room.Walls.GetRange(0, index);
					graphicalWallModifier.Room.Walls.RemoveRange(0, index);
					graphicalWallModifier.Room.Walls.AddRange(walls);
					chkStartWall.Checked = true;
					panel.InvalidateGraphics();
				} else {
					MessageBox.Show(EuroplanRes.GraphicalWallModifierForm_ErsteWandText);
				}
			}
		}

		private void btnWallSelectConstruction_Click(object sender, EventArgs e) {
			SelectHithermWallForm form = new SelectHithermWallForm(false);
			if (form.ShowDialog() == DialogResult.OK) {
				selectedWall.WallId = form.SelectedWall.Id;
				this.txtWallConstruction.Text = form.SelectedWall.Id;
			}
			form.Dispose();
		}

		private void numWallVertical_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (selectedWall != null) {
					selectedWall.SetWallHeight((double)numWallVertical.Value / 100.0);
				}
			}
		}

	}
}