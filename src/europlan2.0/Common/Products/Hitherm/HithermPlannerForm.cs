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
			//this.graphicalWallPanel.Room = product.AssociatedRoom;
			this.hithermPlanner.Product = product;
			this.btnCreateWalls.Enabled = this.graphicalWallPanel.Room != null && this.graphicalWallPanel.Room.RoomCoordinates != null && this.graphicalWallPanel.Room.RoomCoordinates.Count > 2 && this.graphicalWallPanel.Room.AssociatedPlan != null && this.graphicalWallPanel.Room.AssociatedPlan.Measure.HasValue;
			this.panelDefineWalls.BringToFront();
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
			NewWallForm form = new NewWallForm(false, false, selectedObject != null ? (selectedObject as GraphicalWall).GetWallWidth() * 100 : 0, graphicalWallPanel.Room.Walls.Count);
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK) {
				double height = form.Height / 100.0;
				double width = form.Width / 100.0;
				GraphicalWall newWall = new GraphicalWall();
				newWall.WallId = form.WallId;
				newWall.BorderDistance = HithermProduct.ConfigGraphicalRandabstandDefault;
				newWall.CeilingContour.Add(new Point2D(0, height));
				newWall.CeilingContour.Add(new Point2D(0, height));
				newWall.CeilingContour.Add(new Point2D(width, height));
				newWall.CeilingContour.Add(new Point2D(width, height));
				if (selectedObject == null) {
					this.graphicalWallPanel.Room.Walls.Add(newWall);
				} else {
					GraphicalWall wall = selectedObject as GraphicalWall;
					if (form.CreationType == NewWallForm.CreationTypeEnum.Prev) {
						this.graphicalWallPanel.Room.Walls.Insert(this.graphicalWallPanel.Room.Walls.IndexOf(wall), newWall);
					} else if (form.CreationType == NewWallForm.CreationTypeEnum.Next) {
						this.graphicalWallPanel.Room.Walls.Insert(this.graphicalWallPanel.Room.Walls.IndexOf(wall) + 1, newWall);
					} else if (form.CreationType == NewWallForm.CreationTypeEnum.After) {
						this.graphicalWallPanel.Room.Walls.Insert(form.AfterWallNumber, newWall);
					} else {
						wall.DachSchraege = newWall;
						newWall.IsDachSchraege = true;
					}

				}
				
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
			NewWallForm form = new NewWallForm(true, false, 0, 0);
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
			UpdateModifyRegisterPanel(null);
			this.panelModifyHitherm.BringToFront();
		}

		private void graphicalWallPanel_ObjectSelected(object sender, GraphicalWallPanel.SelectedObjectArgs e) {
			if (this.selectedObject != e.SelectedObject) {
				unsavedChanges = false;
			}
			this.selectedObject = e.SelectedObject;
			if (selectedObject != null) {
				if (selectedObject is GraphicalWall) {
					UpdateDefineWallsPanel(selectedObject as GraphicalWall);
				} else if (selectedObject is GraphicalHithermRegisterWrapper) {
					UpdateModifyRegisterPanel(selectedObject as GraphicalHithermRegisterWrapper);
				}
			}
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void UpdateModifyRegisterPanel(GraphicalHithermRegisterWrapper hithermRegister) {
			updateOngoing = true;
			this.panelModifyHitherm.BringToFront();
			if (hithermRegister != null) {
				string text = "Hitherm Klimawand   " + hithermRegister.Register.RegisterCount + " Stück Hitherm ";
				if (hithermRegister.Register.IsHochleistungsRegister) {
					text += "Hochleistungsregister ";
				} else {
					text += "Standardregister ";
				}
				text += hithermRegister.Register.RegisterHoehe + "cm (" + hithermRegister.Register.Rohre + " Leistungsrohre)";
				lblRegisterSelectedRegister.Text = text;
				numRegisterLeft.Value = (decimal)hithermRegister.Register.GraphPosX;
				numRegisterVertical.Value = (decimal)hithermRegister.Register.GraphPosY;
				if (hithermRegister.Register.GraphVorlaufRight) {
					rbRegisterRight.Checked = true;
				} else {
					rbRegisterLeft.Checked = true;
				}
				if (hithermRegister.Register.IsHochleistungsRegister) {
					rbRegister5.Checked = true;
				} else {
					rbRegister10.Checked = true;
				}
				chkRegisterHelpLines.Checked = this.hithermPlanner.NewRegisterUseHelpline;
				chkRegisterWholeRegister.Checked = this.hithermPlanner.NewRegisterOnlyWhole;
			} else {
				lblRegisterSelectedRegister.Text = "Kein Register ausgewählt";
				numRegisterLeft.Value = 0;
				numRegisterVertical.Value = 0;
				if (this.hithermPlanner.NewRegisterVorlaufRight) {
					rbRegisterRight.Checked = true;
				} else {
					rbRegisterLeft.Checked = true;
				}
				if (this.hithermPlanner.NewRegisterRohrabstand == HithermRegister.RohrabstandEnum.RC_HOCHLEISTUNG) {
					rbRegister5.Checked = true;
				} else {
					rbRegister10.Checked = true;
				}
				chkRegisterHelpLines.Checked = this.hithermPlanner.NewRegisterUseHelpline;
				chkRegisterWholeRegister.Checked = this.hithermPlanner.NewRegisterOnlyWhole;
			}
			UpdateModifyRegisterPanelButtons(hithermRegister);
			updateOngoing = false;
		}

		private void UpdateDefineWallsPanel(GraphicalWall wall) {
			updateOngoing = true;
			if (wall != null) {
				this.panelDefineWalls.BringToFront();
				int index = 0;
				if (wall.IsDachSchraege) {
					foreach (GraphicalWall graphicalWall in this.graphicalWallPanel.Room.Walls) {
						if (graphicalWall.GetWallForId(wall.Id) != null) {
							index = this.graphicalWallPanel.Room.Walls.IndexOf(graphicalWall);
							break;
						}
					}
				} else {
					index = this.graphicalWallPanel.Room.Walls.IndexOf(wall);
				}
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

		private void UpdateModifyRegisterPanelButtons(GraphicalHithermRegisterWrapper hithermRegister) {
			if (hithermRegister != null) {
				this.btnRegisterAccept.Enabled = unsavedChanges;
				this.btnRegisterRevert.Enabled = unsavedChanges;
				// TODO
				//this.btnRegisterConnect.Enabled = true;
				this.btnRegisterConnect.Enabled = false;
				this.btnRegisterDelete.Enabled = true;
			} else {
				this.btnRegisterAccept.Enabled = false;
				this.btnRegisterRevert.Enabled = false;
				this.btnRegisterConnect.Enabled = false;
				this.btnRegisterDelete.Enabled = false;
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
				if (MessageBox.Show("Wollen Sie die aktuelle Wand wirklich löschen?", "Wand löschen", MessageBoxButtons.YesNo) == DialogResult.Yes) {
					GraphicalWall wall = selectedObject as GraphicalWall;
					graphicalWallPanel.Room.Walls.Remove(wall);
					selectedObject = null;
					UpdateDefineWallsPanel(null);
					this.graphicalWallPanel.InvalidateGraphics();
				}
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
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnWallRevert_Click(object sender, EventArgs e) {
			GraphicalWall wall = selectedObject as GraphicalWall;
			UpdateDefineWallsPanel(wall);		
		}

		private void numRegisterLeft_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				GraphicalHithermRegisterWrapper wrapper = selectedObject as GraphicalHithermRegisterWrapper;
				unsavedChanges = true;
				UpdateModifyRegisterPanelButtons(wrapper);
			}
		}

		private void numRegisterVertical_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				GraphicalHithermRegisterWrapper wrapper = selectedObject as GraphicalHithermRegisterWrapper;
				unsavedChanges = true;
				UpdateModifyRegisterPanelButtons(wrapper);
			}
		}

		private void rbRegisterLeftRight_CheckedChanged(object sender, EventArgs e) {
			if (selectedObject != null) {
				GraphicalHithermRegisterWrapper wrapper = selectedObject as GraphicalHithermRegisterWrapper;
				wrapper.Register.GraphVorlaufRight = rbRegisterRight.Checked;
			}
			hithermPlanner.NewRegisterVorlaufRight = rbRegisterRight.Checked;
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void rbPipeDistance_CheckedChanged(object sender, EventArgs e) {
			if (selectedObject != null) {
				GraphicalHithermRegisterWrapper wrapper = selectedObject as GraphicalHithermRegisterWrapper;
				wrapper.Register.IsHochleistungsRegister = rbRegister5.Checked;
			}
			hithermPlanner.NewRegisterRohrabstand = rbRegister5.Checked ? HithermRegister.RohrabstandEnum.RC_HOCHLEISTUNG : HithermRegister.RohrabstandEnum.RC_STANDARD;
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnRegisterAccept_Click(object sender, EventArgs e) {
			GraphicalHithermRegisterWrapper wrapper = selectedObject as GraphicalHithermRegisterWrapper;
			wrapper.Register.GraphPosX = (double)numRegisterLeft.Value;
			wrapper.Register.GraphPosY = (double)numRegisterVertical.Value;
			UpdateModifyRegisterPanel(wrapper);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnRegisterDelete_Click(object sender, EventArgs e) {
			if (selectedObject != null) {
				GraphicalHithermRegisterWrapper wrapper = selectedObject as GraphicalHithermRegisterWrapper;
				foreach (GraphicalWall wall in graphicalWallPanel.Room.Walls) {
					GraphicalWall wrapperWall = wall.GetWallForWrapper(wrapper);
					if (wrapperWall != null) {
						wrapperWall.Registers.Remove(wrapper);
					}
					HithermCircuit toDelete = null;
					foreach (HithermCircuit circuit in hithermPlanner.Product.PlannedCircuits) {
						if (circuit.Registers.Contains(wrapper.Register)) {
							circuit.Registers.Remove(wrapper.Register);
							if (circuit.Registers.Count == 0) {
								toDelete = circuit;
							}
						}
					}
					if (toDelete != null) {
						hithermPlanner.Product.PlannedCircuits.Remove(toDelete);
					}
				}
				UpdateModifyRegisterPanel(wrapper);
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void btnRegisterRevert_Click(object sender, EventArgs e) {
			GraphicalHithermRegisterWrapper wrapper = selectedObject as GraphicalHithermRegisterWrapper;
			UpdateModifyRegisterPanel(wrapper);
		}

		private void btnRegisterConnect_Click(object sender, EventArgs e) {
			// TODO
		}

		private void chkRegisterHelpLines_CheckedChanged(object sender, EventArgs e) {
			hithermPlanner.NewRegisterUseHelpline = chkRegisterHelpLines.Checked;
		}

		private void chkRegisterWholeRegister_CheckedChanged(object sender, EventArgs e) {
			hithermPlanner.NewRegisterOnlyWhole = chkRegisterWholeRegister.Checked;
		}

		private void hithermPlanner_RecalculationNecessary(object sender, EventArgs e) {
			CalculateAndUpdate();
		}

		private void CalculateAndUpdate() {
			HithermProduct product = this.hithermPlanner.Product;
			PlannedProduct pp = Project.Instance.GetPlannedProduct(product);
			product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
			string errorMsg = pp.Product.LastErrorMessage;
			//this.lstError.Items.Clear();
			//string[] messages;
		}
	}
}