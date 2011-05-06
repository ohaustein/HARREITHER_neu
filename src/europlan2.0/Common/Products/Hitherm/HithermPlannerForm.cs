using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WW.Math;
using WW.Math.Geometry;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class HithermPlannerForm : Form {

		private IGraphicalWallObject selectedObject = null;
		private bool updateOngoing = false;
		private bool unsavedChanges = false;

		public HithermPlannerForm(HithermProduct product) {
			InitializeComponent();
			this.graphicalWallPanel.Room = product.AssociatedRoom;
			this.btnCreateWalls.Enabled = this.graphicalWallPanel.Room != null && this.graphicalWallPanel.Room.RoomCoordinates != null && this.graphicalWallPanel.Room.RoomCoordinates.Count > 2 && this.graphicalWallPanel.Room.AssociatedPlan != null && this.graphicalWallPanel.Room.AssociatedPlan.Measure.HasValue;
			UpdateDefineWallsPanelButtons(null);
		}

		private void HithermPlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["HithermPlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			if (settings.GetSetting("Maximized", false)) {
				this.WindowState = FormWindowState.Maximized;
			} else {
				this.WindowState = FormWindowState.Normal;
			}
		}

		private void HithermPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["HithermPlannerForm"];
			if (this.WindowState == FormWindowState.Normal) {
				settings.StorePoint("Location", this.Location);
				settings.StoreSize("Size", this.Size);
				settings.StoreSetting("Maximized", false);
			} else if (this.WindowState == FormWindowState.Maximized) {
				settings.StoreSetting("Maximized", true);
			}
			SettingsFile.Update();
		}

		private void btnWallNewWall_Click(object sender, EventArgs e) {
			NewWallForm form = new NewWallForm(true, false);
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK) {
				double height = form.Height / 100.0;
				double measure = this.graphicalWallPanel.Room.AssociatedPlan.Measure.Value;

			}
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnCreateWalls_Click(object sender, EventArgs e) {
			bool ok = true;
			if (this.graphicalWallPanel.Room.Walls.Count > 0) {
				DialogResult result = MessageBox.Show("Es sind bereits Wände vorhanden. Wollen Sie wirklich alle bestehenden Wände löschen und automatisch erzeugen?", "Wände vorhanden", MessageBoxButtons.YesNo);
				if (result == DialogResult.No) {
					ok = false;
				}
			}
			if (ok) {
			this.graphicalWallPanel.Room.Walls.Clear();
			NewWallForm form = new NewWallForm(true, false);
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK) {
				double height = form.Height / 100.0;
				double measure = this.graphicalWallPanel.Room.AssociatedPlan.Measure.Value;
				Point2D lastVertex = this.graphicalWallPanel.Room.RoomCoordinates[this.graphicalWallPanel.Room.RoomCoordinates.Count - 1];
				Polygon2D roomCoords = new Polygon2D(this.graphicalWallPanel.Room.RoomCoordinates);
				if (!roomCoords.IsClockwise()) {
					roomCoords.Reverse();
				}
				foreach (Point2D vertex in this.graphicalWallPanel.Room.RoomCoordinates) {
					double length = (lastVertex - vertex).GetLength() / measure;
					GraphicalWall newWall = new GraphicalWall();
					newWall.WallId = form.WallId;
					newWall.PlanStartPoint = lastVertex;
					newWall.PlanEndPoint = vertex;
					newWall.BorderDistance = HithermProduct.ConfigGraphicalRandabstandDefault;
					newWall.CeilingContour.Add(new Point2D(0, height));
					newWall.CeilingContour.Add(new Point2D(0, height));
					newWall.CeilingContour.Add(new Point2D(length, height));
					newWall.CeilingContour.Add(new Point2D(length, height));
					this.graphicalWallPanel.Room.Walls.Add(newWall);
					lastVertex = vertex;
				}
			}
			form.Dispose();
			}
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnWallEdgeDistance_Click(object sender, EventArgs e) {
			GraphicalWall wall = selectedObject as GraphicalWall;
			EdgeDistanceForm form = new EdgeDistanceForm(wall.BorderDistance);
			if (form.ShowDialog() == DialogResult.OK) {
				double distance = form.EdgeDistance > 0 ? form.EdgeDistance / 100.0 : 0;
				wall.BorderDistance = distance;
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void btnPick_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_SELECT_OBJECT;
			this.hithermPlanner.Mode = HithermPlanner.HithermPlannerMode.HPM_NONE;
		}

		private void btnMove_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_MOVE;
			this.hithermPlanner.Mode = HithermPlanner.HithermPlannerMode.HPM_NONE;
		}

		private void btnRegister_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_PLANNER_DRAG;
			this.hithermPlanner.Mode = HithermPlanner.HithermPlannerMode.HPM_ADD_REGISTER;
		}

		private void graphicalWallPanel_ObjectSelected(object sender, GraphicalWallPanel.SelectedObjectArgs e) {
			if (this.selectedObject != e.SelectedObject) {
				unsavedChanges = false;
			}
			this.selectedObject = e.SelectedObject;
			if (selectedObject != null) {
				if (selectedObject is GraphicalWall) {
					UpdateDefineWallsPanel(selectedObject as GraphicalWall);					
				}
			}
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void UpdateDefineWallsPanel(GraphicalWall wall) {
			updateOngoing = true;
			if (wall != null) {
				this.panelDefineWalls.BringToFront();
				int index = this.graphicalWallPanel.Room.Walls.IndexOf(wall);
				this.lblSelectedWall.Text = "Wand Nr: " + (index + 1);
				this.txtWallConstruction.Text = wall.WallId;
				this.numWallHorizontal.Value = (decimal)wall.GetWallWidth() * 100;
				this.numWallVertical.Value = (decimal)wall.GetWallHeight() * 100;
			} else {
				this.lblSelectedWall.Text = "Keine Wand ausgewählt";
				this.txtWallConstruction.Text = "";
				this.numWallHorizontal.Text = "";
				this.numWallVertical.Text = "";
			}
			UpdateDefineWallsPanelButtons(wall);
			updateOngoing = false;
		}

		private void UpdateDefineWallsPanelButtons(GraphicalWall wall) {
			if (wall != null) {
				this.btnWallSelectConstruction.Enabled = true;
				this.numWallHorizontal.Enabled = true;
				this.numWallVertical.Enabled = true;
				this.btnWallApply.Enabled = unsavedChanges;
				this.btnWallRevert.Enabled = unsavedChanges;
				this.btnWallDelete.Enabled = true;
				this.btnWallEdgeDistance.Enabled = true;
				this.btnWallHelpLine.Enabled = true;
								int index = this.graphicalWallPanel.Room.Walls.IndexOf(wall);
				if (index > 0) {
					this.btnWallLeft.Enabled = true;
				} else {
					this.btnWallLeft.Enabled = false;
				}
				if (this.graphicalWallPanel.Room.Walls.Count > 1 && index < (this.graphicalWallPanel.Room.Walls.Count - 1)) {
					this.btnWallRight.Enabled = true;
				} else {
					this.btnWallRight.Enabled = false;
				}
			} else {
				this.btnWallSelectConstruction.Enabled = false;
				this.numWallHorizontal.Enabled = false;
				this.numWallVertical.Enabled = false;
				this.btnWallLeft.Enabled = false;
				this.btnWallRight.Enabled = false;
				this.btnWallApply.Enabled = false;
				this.btnWallRevert.Enabled = false;
				this.btnWallDelete.Enabled = false;
				this.btnWallEdgeDistance.Enabled = false;
				this.btnWallHelpLine.Enabled = false;
			}
		}

		private void btnWallSelectConstruction_Click(object sender, EventArgs e) {
			SelectHithermWallForm form = new SelectHithermWallForm(false);
			if (form.ShowDialog() == DialogResult.OK) {
				GraphicalWall wall = selectedObject as GraphicalWall;
				this.txtWallConstruction.Text = form.SelectedWall.Id;
				unsavedChanges = true;
				UpdateDefineWallsPanelButtons(wall);
			}
			form.Dispose();
		}

		private void numWallHorizontal_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				GraphicalWall wall = selectedObject as GraphicalWall;
				unsavedChanges = true;
				UpdateDefineWallsPanelButtons(wall);
			}
			
		}

		private void numWallVertical_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				GraphicalWall wall = selectedObject as GraphicalWall;
				unsavedChanges = true;
				UpdateDefineWallsPanelButtons(wall);
			}
		}

		private void btnWallLeft_Click(object sender, EventArgs e) {
			if (IsChangeAllowed()) {
				GraphicalWall wall = selectedObject as GraphicalWall;
				int index = graphicalWallPanel.Room.Walls.IndexOf(wall);
				graphicalWallPanel.Room.Walls.RemoveAt(index);
				graphicalWallPanel.Room.Walls.Insert(index - 1, wall);
				UpdateDefineWallsPanel(wall);
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void btnWallRight_Click(object sender, EventArgs e) {
			if (IsChangeAllowed()) {
				GraphicalWall wall = selectedObject as GraphicalWall;
				int index = graphicalWallPanel.Room.Walls.IndexOf(wall);
				graphicalWallPanel.Room.Walls.RemoveAt(index);
				graphicalWallPanel.Room.Walls.Insert(index + 1, wall);
				UpdateDefineWallsPanel(wall);
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private bool IsChangeAllowed() {
			if (this.graphicalWallPanel.Room != null) {
				bool removePlanPointsAllowed = false;
				foreach (GraphicalWall wall in this.graphicalWallPanel.Room.Walls) {
					if (wall.PlanStartPoint != Point2D.Zero || wall.PlanEndPoint != Point2D.Zero) {
						if (!removePlanPointsAllowed) {
							DialogResult result = MessageBox.Show("Die Wanddefinitionen wurden automatisch erzeugt. Falls Sie Änderungen vornehmen wollen, können Anbindeleitungen nicht mehr grafisch verplant werden. Wollen Sie wirklich fortfahren?", "Wanddefinition manuell anpassen?", MessageBoxButtons.YesNoCancel);
							if (result != DialogResult.Yes) {
								return false;
							} else {
								removePlanPointsAllowed = true;
								wall.PlanStartPoint = Point2D.Zero;
								wall.PlanEndPoint = Point2D.Zero;
							}
						} else {
							wall.PlanStartPoint = Point2D.Zero;
							wall.PlanEndPoint = Point2D.Zero;
						}
					}
				}
				return true;
			}
			return false;
		}

		private void btnWallDelete_Click(object sender, EventArgs e) {
			if (IsChangeAllowed()) {
				GraphicalWall wall = selectedObject as GraphicalWall;
				graphicalWallPanel.Room.Walls.Remove(wall);
				selectedObject = null;
				UpdateDefineWallsPanel(null);
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void btnWallApply_Click(object sender, EventArgs e) {
			GraphicalWall wall = selectedObject as GraphicalWall;
			if (wall.WallId != txtWallConstruction.Text || Math.Round((decimal)wall.GetWallWidth() * 100, 0) != numWallHorizontal.Value) {
				if (IsChangeAllowed()) {
					wall.WallId = txtWallConstruction.Text;
					wall.SetWallWidth((double)numWallHorizontal.Value / 100.0);
					wall.SetWallHeight((double)numWallVertical.Value / 100.0);
				}
			} else {
				wall.WallId = txtWallConstruction.Text;
				wall.SetWallWidth((double)numWallHorizontal.Value / 100.0);
				wall.SetWallHeight((double)numWallVertical.Value / 100.0);
			}
			UpdateDefineWallsPanel(wall);
		}

		private void btnWallRevert_Click(object sender, EventArgs e) {
			GraphicalWall wall = selectedObject as GraphicalWall;
			UpdateDefineWallsPanel(wall);		
		}
	}
}