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
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Europlan.Common {
	public partial class HithermCompactPlannerForm : Form {

		private bool updateOngoing = false;
		private bool unsavedChanges = false;

		public HithermCompactPlannerForm(HithermCompactProduct product) {
			InitializeComponent();
			this.SetLanguage();

			this.hithermCompactPlanner.HithermCompactProduct = product;
			this.btnCreateWalls.Enabled = this.graphicalWallPanel.Room != null && this.graphicalWallPanel.Room.RoomCoordinates != null && this.graphicalWallPanel.Room.RoomCoordinates.Count > 2 && this.graphicalWallPanel.Room.AssociatedPlan != null && this.graphicalWallPanel.Room.AssociatedPlan.Measure.HasValue;
			this.panelDefineWalls.BringToFront();

			this.graphicalWallPanel.SelectedObject = null;
			this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_SELECT_OBJECT;
			this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_NONE;
						
			UpdateDefineWallsPanelButtons(null);
			ApplyButtonCheckedState(this.btnPick);

			this.UpdateAdditionalVlRlForCircuit();
			this.CalculateAndUpdate();

			this.connectionPlanner.Product = product;
		}

		private void SetLanguage() {
			this.btnZoomIn.Text = EuroplanRes.Plan_Heranzoomen;
			this.btnZoomIn.ToolTipText = EuroplanRes.Plan_Heranzoomen;
			this.btnZoomOut.Text = EuroplanRes.Plan_Herauszoomen;
			this.btnZoomOut.ToolTipText = EuroplanRes.Plan_Herauszoomen;
			this.btnExport.Text = EuroplanRes.HithermPlannerForm_Exportieren;
			this.btnMove.Text = EuroplanRes.HithermPlannerForm_Verschieben;
			this.btnPick.Text = EuroplanRes.HithermPlannerForm_ObjekteBearbeiten;
			this.btnWall.Text = EuroplanRes.HithermPlannerForm_Waende;
			this.btnObstacle.Text = EuroplanRes.HithermPlannerForm_FensterTueren;
			this.btnSchraege.Text = EuroplanRes.HithermPlannerForm_Schraegen;
			this.btnRegister.Text = EuroplanRes.HithermPlannerForm_Waermeregister;
			this.btnConnection.Text = EuroplanRes.HithermPlannerForm_AnbindeleitungenHinzufuegen;
			this.btnUseHelplines.Text = EuroplanRes.HithermPlannerForm_AnHilfslinineAusrichten;
			this.btnAddAnbindeleitungen.Text = EuroplanRes.HithermPlannerForm_Anbindeleitungen;
			this.btnSelectAnbindeleitungen.Text = EuroplanRes.HithermPlannerForm_AnbindeleitungenAendern;
			this.btnObstacleDoor.Text = EuroplanRes.HithermPlannerForm_Tuer;
			this.btnObstacleWindow.Text = EuroplanRes.HithermPlannerForm_Fenster;
			this.btnObstacleTriangleWindowLeft.Text = EuroplanRes.HithermPlannerForm_DreieckfensterLinks;
			this.btnObstacleTriangleWindowRight.Text = EuroplanRes.HithermPlannerForm_DreiecksfensterRechts;
			this.btnObstacleOther.Text = EuroplanRes.HithermPlannerForm_Sonstiges;
			this.btnRegisterVertical.Text = EuroplanRes.HithermPlannerForm_WaermeregisterSenkrecht;
			this.btnRegisterHorizontal.Text = EuroplanRes.HithermPlannerForm_WaermeregisterWaagrecht;
			this.btnConnectionManual.Text = EuroplanRes.HithermPlannerForm_ManuellVerbinden;
			this.btnConnectionAuto.Text = EuroplanRes.HithermPlannerForm_AutomatischVerbinden;
			this.btnConnectionAlign.Text = EuroplanRes.HithermPlannerForm_RechtwinkeligVerbinden;
			this.btnConnectionDirect.Text = EuroplanRes.HithermPlannerForm_DirektVerbinden;
			this.btnSchraegeLeft.Text = EuroplanRes.HithermPlannerForm_SchraegeLinks;
			this.btnSchraegeRight.Text = EuroplanRes.HithermPlannerForm_SchraegeRechts;
			this.lblSchraege.Text = EuroplanRes.HithermPlannerForm_KeineSchraege;
			this.toolTip.SetToolTip(this.btnSchraegeDelete, EuroplanRes.HithermPlannerForm_SchraegeLoeschen);
			this.toolTip.SetToolTip(this.btnSchraegeRevert, EuroplanRes.HithermPlannerForm_AenderungenVerwerfen);
			this.toolTip.SetToolTip(this.btnSchraegeApply, EuroplanRes.HithermPlannerForm_AenderungenSpeichern);
			this.groupBox17.Text = EuroplanRes.HithermPlannerForm_Position;
			this.rbSchraegeLeft.Text = EuroplanRes.HithermPlannerForm_Links;
			this.rbSchraegeRight.Text = EuroplanRes.HithermPlannerForm_Rechts;
			this.groupBox18.Text = EuroplanRes.HithermPlannerForm_Groesse;
			this.label19.Text = EuroplanRes.Unit_Zentimeter;
			this.label20.Text = EuroplanRes.HithermPlannerForm_Vertikal;
			this.label21.Text = EuroplanRes.Unit_Zentimeter;
			this.label22.Text = EuroplanRes.HithermPlannerForm_Horizontal;
			this.toolTip.SetToolTip(this.btnConnectionRevert, EuroplanRes.HithermPlannerForm_AenderungenVerwerfen);
			this.toolTip.SetToolTip(this.btnConnectionApply, EuroplanRes.HithermPlannerForm_AenderungenSpeichern);
			this.label23.Text = EuroplanRes.HithermPlannerForm_Anbindeleitungen;
			this.toolTip.SetToolTip(this.btnConnectionDelete, EuroplanRes.HithermPlannerForm_AnbindeleitungLoeschen);
			this.groupBox16.Text = EuroplanRes.HithermPlannerForm_MittelmassHor;
			this.label15.Text = EuroplanRes.Unit_Zentimeter;
			this.label18.Text = EuroplanRes.Unit_Zentimeter;
			this.groupBox15.Text = EuroplanRes.HithermPlannerForm_AbstandHor;
			this.label13.Text = EuroplanRes.Unit_Zentimeter;
			this.label14.Text = EuroplanRes.Unit_Zentimeter;
			this.groupBox14.Text = EuroplanRes.HithermPlannerForm_PositionHor;
			this.label9.Text = EuroplanRes.Unit_Zentimeter;
			this.label12.Text = EuroplanRes.Unit_Zentimeter;
			this.lblObstacle.Text = EuroplanRes.HithermPlannerForm_KeinObjekt;
			this.toolTip.SetToolTip(this.btnObstacleRevert, EuroplanRes.HithermPlannerForm_AenderungenVerwerfen);
			this.toolTip.SetToolTip(this.btnObstacleApply, EuroplanRes.HithermPlannerForm_AenderungenSpeichern);
			this.groupBox13.Text = EuroplanRes.HithermPlannerForm_PositionVert;
			this.label11.Text = EuroplanRes.Unit_Zentimeter;
			this.groupBox12.Text = EuroplanRes.HithermPlannerForm_Groesse;
			this.label8.Text = EuroplanRes.Unit_Zentimeter;
			this.label10.Text = EuroplanRes.Unit_Zentimeter;
			this.toolTip.SetToolTip(this.btnObstacleBorder, EuroplanRes.HithermPlannerForm_RandabstandAendern);
			this.toolTip.SetToolTip(this.btnObstacleRemove, EuroplanRes.HithermPlannerForm_ObjektLoeschen);
			this.toolTip.SetToolTip(this.btnWallHelpLine, EuroplanRes.HithermPlannerForm_HilfslinienEinrichten);
			this.toolTip.SetToolTip(this.btnWallEdgeDistance, EuroplanRes.HithermPlannerForm_RandabstandAendern);
			this.groupBox3.Text = EuroplanRes.HithermPlannerForm_Position;
			this.toolTip.SetToolTip(this.btnWallLeft, EuroplanRes.WandNachLinksVerschienben);
			this.toolTip.SetToolTip(this.btnWallRight, EuroplanRes.HithermPlannerForm_WandNachRechtsVerschieben);
			this.groupBox1.Text = EuroplanRes.HithermPlannerForm_Konstruktion;
			this.lblSelectedWall.Text = EuroplanRes.HithermPlannerForm_KeineWand;
			this.btnCreateWalls.Text = EuroplanRes.HithermPlannerForm_WaendeErzeugen;
			this.groupBox2.Text = EuroplanRes.HithermPlannerForm_Groesse;
			this.label3.Text = EuroplanRes.Unit_Zentimeter;
			this.lblWallVertical.Text = EuroplanRes.HithermPlannerForm_Senkrecht;
			this.label2.Text = EuroplanRes.Unit_Zentimeter;
			this.lblWallHorizontal.Text = EuroplanRes.HithermPlannerForm_Waagrecht;
			this.toolTip.SetToolTip(this.btnWallNewWall, EuroplanRes.HithermPlannerForm_NeueWand);
			this.toolTip.SetToolTip(this.btnWallDelete, EuroplanRes.HithermPlannerForm_WandLoeschenTitel);
			this.toolTip.SetToolTip(this.btnWallRevert, EuroplanRes.HithermPlannerForm_AenderungenVerwerfen);
			this.toolTip.SetToolTip(this.btnWallApply, EuroplanRes.HithermPlannerForm_AenderungenSpeichern);
			this.chkRegisterHelpLines.Text = EuroplanRes.HithermPlannerForm_AnHilfslinienAusrichten;
			this.toolTip.SetToolTip(this.btnRegisterConnect, EuroplanRes.HithermPlannerForm_RegisterVerbinden);
			this.toolTip.SetToolTip(this.btnRegisterDelete, EuroplanRes.HithermPlannerForm_RegisterLoeschen);
			this.toolTip.SetToolTip(this.btnRegisterRevert, EuroplanRes.HithermPlannerForm_AenderungenVerwerfen);
			this.toolTip.SetToolTip(this.btnRegisterAccept, EuroplanRes.HithermPlannerForm_AenderungenSpeichern);
			this.groupBox4.Text = EuroplanRes.HithermPlannerForm_Position;
			this.label1.Text = EuroplanRes.Unit_Zentimeter;
			this.label4.Text = EuroplanRes.HithermPlannerForm_Vertikal;
			this.label5.Text = EuroplanRes.Unit_Zentimeter;
			this.label6.Text = EuroplanRes.HithermPlannerForm_Links3;
			this.lblRegisterSelectedRegister.Text = EuroplanRes.HithermPlannerForm_KeineWand;
			this.label7.Text = EuroplanRes.PlannedModulProductPanel_Berechnungsergebnisse;
			this.lblHeat.Text = EuroplanRes.HithermPlannerForm_Heizbetrieb;
			this.lblCool.Text = EuroplanRes.HithermPlannerForm_Kuehlbetrieb;
			this.label17.Text = EuroplanRes.HithermPlannerForm_ErreichteLeistung;
			this.label16.Text = EuroplanRes.HithermPlannerForm_DifferenzZurErwartetenLeistung;
			this.lblQHeatUnit.Text = EuroplanRes.Unit_Watt;
			this.lblQHeatDiffUnit.Text = EuroplanRes.Unit_Watt;
			this.lblQHeatRestUnit.Text = EuroplanRes.Unit_Watt;
			this.lblQCoolUnit.Text = EuroplanRes.Unit_Watt;
			this.lblQCoolDiffUnit.Text = EuroplanRes.Unit_Watt;
			this.lblQCoolRestUnit.Text = EuroplanRes.Unit_Watt;
			this.label32.Text = EuroplanRes.HithermPlannerForm_ZusInformationen;
			this.lblCoveredAreaTitle.Text = EuroplanRes.HithermPlannerForm_BelegteFlaeche;
			this.lblCoveredAreaUnit.Text = EuroplanRes.Unit_Quadratmeter;
			this.lblNecessaryWaermestromdichteTitle.Text = EuroplanRes.HithermPlannerForm_BenoetigteWaermestromdichte;
			this.lblNecessaryWaermestromdichteUnit.Text = EuroplanRes.Unit_WattProQm;
			this.lblNecessaryAreaTitle.Text = EuroplanRes.HithermPlannerForm_BenoetigteFlaeche;
			this.lblNecessaryAreaUnit.Text = EuroplanRes.Unit_Quadratmeter;
			this.lblAvailableAreaTitle.Text = EuroplanRes.HithermPlannerForm_VerfuegbareFlaeche;
			this.lblAvailableAreaUnit.Text = EuroplanRes.Unit_Quadratmeter;
			this.lblRestAreaTitle.Text = EuroplanRes.HithermPlannerForm_UebrigeFlaeche;
			this.lblRestAreaUnit.Text = EuroplanRes.Unit_Quadratmeter;
			this.btnConnectionInFloor.Text = EuroplanRes.HithermPlannerForm_HeizkreiseVerbinden;
			this.Text = EuroplanRes.HithermCompactPlannerForm_Titel;
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
			this.connectionPlanner.ReGenerateConnectionPipes();

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

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.AddScale(0.9, null);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.AddScale(1.1, null);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void ApplyButtonCheckedState(ToolStripButton buttonToCheck) {
			ToolStripButton[] buttons = new ToolStripButton[] { this.btnPick, this.btnMove, this.btnWall, this.btnObstacle, this.btnSchraege, this.btnRegister, this.btnConnection, this.btnAddAnbindeleitungen, this.btnSelectAnbindeleitungen };
			foreach (ToolStripButton button in buttons) {
				button.Checked = (button == buttonToCheck);
			}
			/*this.btnPick.Checked = false;
			this.btnMove.Checked = false;
			this.btnWall.Checked = false;
			this.btnObstacle.Checked = false;
			this.btnSchraege.Checked = false;
			this.btnRegister.Checked = false;
			this.btnConnection.Checked = false;
			buttonToCheck.Checked = true;*/
		}

		private void ApplyRegisterButtonCheckedState(ToolStripButton buttonToCheck) {
			this.btnRegisterHorizontal.Checked = this.btnRegisterHorizontal == buttonToCheck;
			this.btnRegisterVertical.Checked = this.btnRegisterVertical == buttonToCheck;
		}

		private void ApplyObstacleButtonCheckedState(ToolStripButton buttonToCheck) {
			this.btnObstacleDoor.Checked = this.btnObstacleDoor == buttonToCheck;
			this.btnObstacleWindow.Checked = this.btnObstacleWindow == buttonToCheck;
			this.btnObstacleTriangleWindowLeft.Checked = this.btnObstacleTriangleWindowLeft == buttonToCheck;
			this.btnObstacleTriangleWindowRight.Checked = this.btnObstacleTriangleWindowRight == buttonToCheck;
			this.btnObstacleOther.Checked = this.btnObstacleOther == buttonToCheck;
		}

		private void ApplyConnectionButtonCheckedState(ToolStripButton buttonToCheck) {
			if (buttonToCheck != null) {
				this.btnConnectionManual.Checked = this.btnConnectionManual == buttonToCheck;
				this.btnConnectionAuto.Checked = this.btnConnectionAuto == buttonToCheck;
				this.btnConnectionInFloor.Checked = this.btnConnectionInFloor == buttonToCheck;
			} else {
				this.btnConnectionDirect.Checked = !this.hithermCompactPlanner.NewConnectionsAlign;
				this.btnConnectionAlign.Checked = this.hithermCompactPlanner.NewConnectionsAlign;
			}
		}

		private void ApplySchraegeCuttonCheckedState(ToolStripButton buttonToCheck) {
			this.btnSchraegeLeft.Checked = this.btnSchraegeLeft == buttonToCheck;
			this.btnSchraegeRight.Checked = this.btnSchraegeRight == buttonToCheck;
		}

		private void btnWallNewWall_Click(object sender, EventArgs e) {
			NewWallForm form = new NewWallForm(false, true, SelectedObject != null ? (SelectedObject as GraphicalWall).GetWallWidth() * 100 : 0, graphicalWallPanel.Room.Walls.Count, (graphicalWallPanel.SelectedWall != null && !graphicalWallPanel.SelectedWall.IsDachSchraege), graphicalWallPanel.Room.AssociatedFloor.DefaultRoomHeight);
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
				if (SelectedObject == null) {
					this.graphicalWallPanel.Room.Walls.Add(newWall);
				} else {
					GraphicalWall wall = SelectedObject as GraphicalWall;
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
				DialogResult result = MessageBox.Show(EuroplanRes.HithermPlannerForm_WaendeVorhandenText, EuroplanRes.HithermPlannerForm_WaendeVorhandenTitel, MessageBoxButtons.YesNo);
				if (result == DialogResult.No) {
					ok = false;
				}
			}
			if (ok) {
				this.graphicalWallPanel.Room.Walls.Clear();
				//this.hithermCompactPlanner.HithermCompactProduct.PlannedCircuits.Clear();
				this.hithermCompactPlanner.HithermCompactProduct.ResetProduct();
				NewWallForm form = new NewWallForm(true, true, 0, 0, false, graphicalWallPanel.Room.AssociatedFloor.DefaultRoomHeight);
				DialogResult result = form.ShowDialog();
				if (result == DialogResult.OK) {
					double height = form.Height / 100.0;
					double measure = this.graphicalWallPanel.Room.AssociatedPlan.Measure.Value;
					Polygon2D roomCoords = new Polygon2D(this.graphicalWallPanel.Room.RoomCoordinates);
					if (this.graphicalWallPanel.Room.AssociatedPlan.InvertYAxis) {
						if (!roomCoords.IsClockwise()) {
							roomCoords.Reverse();
						}
					} else {
						if (roomCoords.IsClockwise()) {
							roomCoords.Reverse();
						}
					}
					Point2D lastVertex = roomCoords[roomCoords.Count - 1];
					foreach (Point2D vertex in roomCoords) {
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

					GraphicalWallModifierForm gwmFrom = new GraphicalWallModifierForm(this.graphicalWallPanel.Room);
					gwmFrom.ShowDialog();
					gwmFrom.Dispose();
				}
				form.Dispose();
			}
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnWallEdgeDistance_Click(object sender, EventArgs e) {
			GraphicalWall wall = SelectedObject as GraphicalWall;
			EdgeDistanceForm form = new EdgeDistanceForm(wall.BorderDistance);
			if (form.ShowDialog() == DialogResult.OK) {
				double distance = form.EdgeDistance > 0 ? form.EdgeDistance / 100.0 : 0;
				wall.BorderDistance = distance;
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void btnPick_Click(object sender, EventArgs e) {
			//this.graphicalWallPanel.SelectedObject = null;
			if (!this.btnPick.Checked) {
				this.SetProductPlanner();
				this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_SELECT_OBJECT;
				this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_NONE;
				ApplyButtonCheckedState(this.btnPick);
				this.UpdateSubmenuToolstrip();
			}
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!this.btnMove.Checked) {
				this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_MOVE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_NONE;
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.CM_NONE;
				ApplyButtonCheckedState(this.btnMove);
				this.UpdateSubmenuToolstrip();
			}
		}

		private void btnWall_Click(object sender, EventArgs e) {
			if (!this.btnWall.Checked) {
				this.SetProductPlanner();
				this.graphicalWallPanel.SelectedObject = null;
				this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_MOVE;
				this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_NONE;
				UpdateDefineWallsPanel(null);
				this.panelDefineWalls.BringToFront();
				ApplyButtonCheckedState(this.btnWall);
				this.UpdateSubmenuToolstrip();
			}
		}

		private void btnObstacle_Click(object sender, EventArgs e) {
			if (!this.btnObstacle.Checked) {
				this.SetProductPlanner();
				this.graphicalWallPanel.SelectedObject = null;
				this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_ADD_OBSTACLE;
				this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_NONE;
				this.btnObstacleDoor.Checked = this.graphicalWallPanel.NewObstacleType == GraphicalWallObstacle.ObstacleTypeEnum.Door;
				this.btnObstacleWindow.Checked = this.graphicalWallPanel.NewObstacleType == GraphicalWallObstacle.ObstacleTypeEnum.Window;
				this.btnObstacleTriangleWindowLeft.Checked = this.graphicalWallPanel.NewObstacleType == GraphicalWallObstacle.ObstacleTypeEnum.WindowTriangleLeft;
				this.btnObstacleTriangleWindowRight.Checked = this.graphicalWallPanel.NewObstacleType == GraphicalWallObstacle.ObstacleTypeEnum.WindowTriangleRight;
				this.btnObstacleOther.Checked = this.graphicalWallPanel.NewObstacleType == GraphicalWallObstacle.ObstacleTypeEnum.Other;
				UpdateModifyObstaclesPanel(null);
				this.panelModifyObstacle.BringToFront();
				ApplyButtonCheckedState(this.btnObstacle);
				this.UpdateSubmenuToolstrip();
			}
		}


		private void btnSchraege_Click(object sender, EventArgs e) {
			if (!this.btnSchraege.Checked) {
				this.SetProductPlanner();
				this.graphicalWallPanel.SelectedObject = null;
				this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_ADD_SCHRAEGE;
				this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_NONE;
				UpdateModifySchraegePanel(null);
				this.panelModifySchraege.BringToFront();
				ApplyButtonCheckedState(this.btnSchraege);
				this.UpdateSubmenuToolstrip();
			}
		}

		private void btnRegister_Click(object sender, EventArgs e) {
			if (!this.btnRegister.Checked) {
				this.SetProductPlanner();
				this.graphicalWallPanel.SelectedObject = null;
				this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_PLANNER_DRAG;
				this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_ADD_REGISTER;
				this.btnRegisterHorizontal.Checked = this.hithermCompactPlanner.NewRegisterParapet;
				this.btnRegisterVertical.Checked = !this.hithermCompactPlanner.NewRegisterParapet;
				UpdateModifyRegisterPanel(null);
				this.panelModifyHitherm.BringToFront();
				ApplyButtonCheckedState(this.btnRegister);
				this.UpdateSubmenuToolstrip();
			}
		}

		private void btnConnection_Click(object sender, EventArgs e) {
			if (!this.btnConnection.Checked) {
				this.SetProductPlanner();
				this.graphicalWallPanel.SelectedObject = null;
				this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_PLANNER_CLICK;
				this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_ADD_CONNECTION;
				this.btnConnectionManual.Checked = this.hithermCompactPlanner.NewConnectionMode == HithermCompactPlanner.NewConnectionModeEnum.NCM_MANUAL;
				this.btnConnectionAuto.Checked = this.hithermCompactPlanner.NewConnectionMode == HithermCompactPlanner.NewConnectionModeEnum.NCM_AUTO;
				this.btnConnectionDirect.Checked = this.hithermCompactPlanner.NewConnectionMode == HithermCompactPlanner.NewConnectionModeEnum.NCM_DIRECT;
				this.btnConnectionInFloor.Checked = this.hithermCompactPlanner.NewConnectionMode == HithermCompactPlanner.NewConnectionModeEnum.NCM_CONNECT_HKS;
				UpdateModifyConnectionPanel(null);
				this.panelModifyConnection.BringToFront();
				ApplyButtonCheckedState(this.btnConnection);
				this.UpdateSubmenuToolstrip();
			}
		}

		private void btnAddAnbindeleitungen_Click(object sender, EventArgs e) {
			if (!this.btnAddAnbindeleitungen.Checked) {
				this.SetConnectionPlanner();
				this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION;
				this.ApplyButtonCheckedState(this.btnAddAnbindeleitungen);
			}
		}

		private void btnSelectAnbindeleitungen_Click(object sender, EventArgs e) {
			if (!this.btnSelectAnbindeleitungen.Checked) {
				this.SetConnectionPlanner();
				this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION;
				this.ApplyButtonCheckedState(this.btnSelectAnbindeleitungen);
			}
		}

		private void btnRegisterVertical_Click(object sender, EventArgs e) {
			this.hithermCompactPlanner.NewRegisterParapet = false;
			ApplyRegisterButtonCheckedState(this.btnRegisterVertical);
		}

		private void btnRegisterHorizontal_Click(object sender, EventArgs e) {
			this.hithermCompactPlanner.NewRegisterParapet = true;
			ApplyRegisterButtonCheckedState(this.btnRegisterHorizontal);
		}

		private void btnObstacleDoor_Click(object sender, EventArgs e) {
			graphicalWallPanel.NewObstacleType = GraphicalWallObstacle.ObstacleTypeEnum.Door;
			ApplyObstacleButtonCheckedState(this.btnObstacleDoor);
		}

		private void btnObstacleWindow_Click(object sender, EventArgs e) {
			graphicalWallPanel.NewObstacleType = GraphicalWallObstacle.ObstacleTypeEnum.Window;
			ApplyObstacleButtonCheckedState(this.btnObstacleWindow);
		}

		private void btnObstacleTriangleWindowLeft_Click(object sender, EventArgs e) {
			graphicalWallPanel.NewObstacleType = GraphicalWallObstacle.ObstacleTypeEnum.WindowTriangleLeft;
			ApplyObstacleButtonCheckedState(this.btnObstacleTriangleWindowLeft);
		}

		private void btnObstacleTriangleWindowRight_Click(object sender, EventArgs e) {
			graphicalWallPanel.NewObstacleType = GraphicalWallObstacle.ObstacleTypeEnum.WindowTriangleRight;
			ApplyObstacleButtonCheckedState(this.btnObstacleTriangleWindowRight);
		}

		private void btnObstacleOther_Click(object sender, EventArgs e) {
			graphicalWallPanel.NewObstacleType = GraphicalWallObstacle.ObstacleTypeEnum.Other;
			ApplyObstacleButtonCheckedState(this.btnObstacleOther);
		}

		private void btnConnectionManual_Click(object sender, EventArgs e) {
			this.hithermCompactPlanner.NewConnectionMode = HithermCompactPlanner.NewConnectionModeEnum.NCM_MANUAL;
			ApplyConnectionButtonCheckedState(this.btnConnectionManual);
		}

		private void btnConnectionAuto_Click(object sender, EventArgs e) {
			this.hithermCompactPlanner.NewConnectionMode = HithermCompactPlanner.NewConnectionModeEnum.NCM_AUTO;
			ApplyConnectionButtonCheckedState(this.btnConnectionAuto);
		}

		private void btnConnectionInFloor_Click(object sender, EventArgs e) {
			this.hithermCompactPlanner.NewConnectionMode = HithermCompactPlanner.NewConnectionModeEnum.NCM_CONNECT_HKS;
			ApplyConnectionButtonCheckedState(this.btnConnectionInFloor);
		}

		private void btnConnectionDirect_Click(object sender, EventArgs e) {
			this.hithermCompactPlanner.NewConnectionsAlign = false;
			ApplyConnectionButtonCheckedState(null);
		}

		private void btnConnectionAlign_Click(object sender, EventArgs e) {
			this.hithermCompactPlanner.NewConnectionsAlign = true;
			ApplyConnectionButtonCheckedState(null);
		}

		private void btnSchraegeLeft_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.NewSchraegeOrientation = GraphicalWallSchraege.OrientationEnum.LEFT;
			ApplySchraegeCuttonCheckedState(this.btnSchraegeLeft);
		}

		private void btnSchraegeRight_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.NewSchraegeOrientation = GraphicalWallSchraege.OrientationEnum.RIGHT;
			ApplySchraegeCuttonCheckedState(this.btnSchraegeRight);
		}

		private IGraphicalWallObject SelectedObject {
			get { return this.graphicalWallPanel.SelectedObject; }
		}

		private void graphicalWallPanel_ObjectSelected(object sender, GraphicalWallPanel.SelectedObjectArgs e) {
			unsavedChanges = false;
			if (SelectedObject != null) {
				if (SelectedObject is GraphicalWall) {
					UpdateDefineWallsPanel(SelectedObject as GraphicalWall);
				} else if (SelectedObject is GraphicalHithermCompactRegisterWrapper) {
					UpdateModifyRegisterPanel(SelectedObject as GraphicalHithermCompactRegisterWrapper);
				} else if (SelectedObject is GraphicalWallObstacle) {
					UpdateModifyObstaclesPanel(SelectedObject as GraphicalWallObstacle);
				} else if (SelectedObject is GraphicalHithermCompactVerbindung) {
					UpdateModifyConnectionPanel(SelectedObject as GraphicalHithermCompactVerbindung);
				} else if (SelectedObject is GraphicalWallSchraege) {
					UpdateModifySchraegePanel(SelectedObject as GraphicalWallSchraege);
				}
			}
			/*if (e.OldSelectedObject is GraphicalWallObstacle) {
				GraphicalWallObstacle obstacle = e.OldSelectedObject as GraphicalWallObstacle;
				if (!obstacle.CheckValidity(e.OldSelectedWall, 0, 0)) {
					obstacle.RevertState();
				} else {
					Vector2D offset = this.graphicalWallPanel.Room.GetWallOffset(e.OldSelectedWall).Value * 100;
					Polygon2D border = obstacle.GetObjectBorders(offset.X, offset.Y);
					Polygon2D outsideBorder = obstacle.GetOutsideBorder(offset.X, offset.Y);
					List<GraphicalRegisterWrapper> toDelete = new List<GraphicalRegisterWrapper>();
					foreach (GraphicalRegisterWrapper wrapper in e.OldSelectedWall.Registers) {
						if (wrapper.CollisionTest(outsideBorder, offset.X, offset.Y, false)) {
							toDelete.Add(wrapper);
						}
					}
					foreach (GraphicalRegisterWrapper wrapper in toDelete) {
						DeleteRegister(wrapper as GraphicalHithermCompactRegisterWrapper);
					}
					List<GraphicalHithermCompactVerbindung> linksToDelete = new List<GraphicalHithermCompactVerbindung>();
					foreach (HithermCompactCircuit c in this.hithermCompactPlanner.HithermCompactProduct.PlannedCircuits) {
						foreach (GraphicalHithermCompactVerbindung link in c.Links) {
							if (link.CollisionTest(outsideBorder, offset.X, offset.Y, false)) {
								linksToDelete.Add(link);
							}
						}
					}
					foreach (GraphicalHithermCompactVerbindung link in linksToDelete) {
						DeleteVerbindung(link);
					}
				}
			}*/

			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void graphicalWallPanel_SelectedObjectModified(object sender, EventArgs e) {
			if (SelectedObject != null) {
				unsavedChanges = true;
				if (SelectedObject is GraphicalWall) {
					UpdateDefineWallsPanel(SelectedObject as GraphicalWall);
				} else if (SelectedObject is GraphicalHithermCompactRegisterWrapper) {
					UpdateModifyRegisterPanel(SelectedObject as GraphicalHithermCompactRegisterWrapper);
				} else if (SelectedObject is GraphicalWallObstacle) {
					UpdateModifyObstaclesPanel(SelectedObject as GraphicalWallObstacle);
				} else if (SelectedObject is GraphicalHithermCompactVerbindung) {
					UpdateModifyConnectionPanel(SelectedObject as GraphicalHithermCompactVerbindung);
				} else if (SelectedObject is GraphicalWallSchraege) {
					UpdateModifySchraegePanel(SelectedObject as GraphicalWallSchraege);
				}
			}
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void UpdateSubmenuToolstrip() {
			this.btnObstacleDoor.Visible = this.btnObstacle.Checked;
			this.btnObstacleWindow.Visible = this.btnObstacle.Checked;
			this.btnObstacleTriangleWindowLeft.Visible = this.btnObstacle.Checked;
			this.btnObstacleTriangleWindowRight.Visible = this.btnObstacle.Checked;
			this.btnObstacleOther.Visible = this.btnObstacle.Checked;

			this.btnRegisterHorizontal.Visible = this.btnRegister.Checked;
			this.btnRegisterVertical.Visible = this.btnRegister.Checked;

			this.btnConnectionManual.Visible = this.btnConnection.Checked;
			this.btnConnectionAuto.Visible = this.btnConnection.Checked;
			this.btnConnectionInFloor.Visible = this.btnConnection.Checked;
			this.seperatorConnections.Visible = false; //this.btnConnection.Checked;
			this.btnConnectionDirect.Visible = false; //this.btnConnection.Checked;
			this.btnConnectionAlign.Visible = false; //this.btnConnection.Checked;

			this.btnSchraegeLeft.Visible = this.btnSchraege.Checked;
			this.btnSchraegeRight.Visible = this.btnSchraege.Checked;
		}

		private void UpdateModifyRegisterPanel(GraphicalHithermCompactRegisterWrapper hithermRegister) {
			updateOngoing = true;
			this.panelModifyHitherm.BringToFront();
			if (hithermRegister != null) {
				//string text = "Hitherm Klimawand   " + hithermRegister.Register.RegisterCount + " Stück Hitherm-Compact ";
				string text = EuroplanRes.HithermCompactPlannerForm_RegisterText;
				text = text.Replace("%ANZAHL%", hithermRegister.Register.RegisterCount.ToString());
				if (hithermRegister.Register.IsParapet) {
					//text += "Parapetelement " + hithermRegister.Register.RegisterBreite.ToString() + " Par";
					text = text.Replace("%TYP%", EuroplanRes.HithermCompactPlannerForm_Parapetelement);
					text = text.Replace("%HOEHE%", hithermRegister.Register.RegisterBreite.ToString());
					text = text.Replace("%TYPABK%", EuroplanRes.HithermCompactPlannerForm_Par);
				} else if (hithermRegister.Register.IsDachschraege) {
					//text += "Dachschrägenelement(e) " + hithermRegister.Register.RegisterHoehe.ToString() + " Ds";
					text = text.Replace("%TYP%", EuroplanRes.HithermCompactPlannerForm_Dachschraegenelemente);
					text = text.Replace("%HOEHE%", hithermRegister.Register.RegisterBreite.ToString());
					text = text.Replace("%TYPABK%", EuroplanRes.HithermCompactPlannerForm_Ds);
				} else {
					//text += "Wandelement(e) " + hithermRegister.Register.RegisterHoehe.ToString() + " Std";
					text = text.Replace("%TYP%", EuroplanRes.HithermCompactPlannerForm_Wandelemente);
					text = text.Replace("%HOEHE%", hithermRegister.Register.RegisterBreite.ToString());
					text = text.Replace("%TYPABK%", EuroplanRes.HithermCompactPlannerForm_Std);
				}
				lblRegisterSelectedRegister.Text = text;
				numRegisterLeft.Value = (decimal)hithermRegister.Register.GraphPosX;
				numRegisterVertical.Value = (decimal)hithermRegister.Register.GraphPosY;
				chkRegisterHelpLines.Checked = this.hithermCompactPlanner.NewRegisterUseHelpline;
			} else {
				lblRegisterSelectedRegister.Text = EuroplanRes.HithermPlannerForm_KeinRegister;
				numRegisterLeft.Value = 0;
				numRegisterVertical.Value = 0;
				chkRegisterHelpLines.Checked = this.hithermCompactPlanner.NewRegisterUseHelpline;
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
				string text = EuroplanRes.HithermPlannerForm_WandText;
				text = text.Replace("%WAND%", (index + 1).ToString());
				this.lblSelectedWall.Text = text;
				this.txtWallConstruction.Text = wall.WallId;
				this.numWallHorizontal.Value = (decimal)wall.GetWallWidth() * 100;
				this.numWallVertical.Value = (decimal)wall.GetWallHeight() * 100;
				this.btnWallNewWall.Enabled = !wall.IsDachSchraege;
			} else {
				this.lblSelectedWall.Text = EuroplanRes.HithermPlannerForm_KeineWand;
				this.txtWallConstruction.Text = "";
				this.numWallHorizontal.Text = "";
				this.numWallVertical.Text = "";
				this.btnWallNewWall.Enabled = true;
			}
			UpdateDefineWallsPanelButtons(wall);
			updateOngoing = false;
		}

		private void UpdateModifyObstaclesPanel(GraphicalWallObstacle obstacle) {
			updateOngoing = true;
			this.panelModifyObstacle.BringToFront();
			if (obstacle != null) {
				if (obstacle is GraphicalWallObstacle) {
					this.numObstacleWidth.Value = (decimal)(obstacle as GraphicalWallObstacle).Width;
					this.numObstacleHeight.Value = (decimal)(obstacle as GraphicalWallObstacle).Height;
					this.numObstacleVertical.Value = (decimal)(obstacle as GraphicalWallObstacle).GraphPosY;
					this.numObstacleHorizontalLeft.Value = (decimal)obstacle.GetGraphPosXLeft(graphicalWallPanel.SelectedWall);
					this.numObstacleHorizontalRight.Value = (decimal)obstacle.GetGraphPosXRight(graphicalWallPanel.SelectedWall);
					Nullable<double> distanceLeft = obstacle.GetGraphDistanceXLeft(graphicalWallPanel.SelectedWall);
					if (distanceLeft.HasValue) {
						this.numObstacleDistanceLeft.Value = (decimal)distanceLeft.Value;
					} else {
						this.numObstacleDistanceLeft.Text = "";
					}
					Nullable<double> distanceRight = obstacle.GetGraphDistanceXRight(graphicalWallPanel.SelectedWall);
					if (distanceRight.HasValue) {
						this.numObstacleDistanceRight.Value = (decimal)distanceRight.Value;
					} else {
						this.numObstacleDistanceRight.Text = "";
					}
					Nullable<double> middleDistanceLeft = obstacle.GetGraphMiddleDistanceXLeft(graphicalWallPanel.SelectedWall);
					if (middleDistanceLeft.HasValue) {
						this.numObstacleMiddleDistanceLeft.Value = (decimal)middleDistanceLeft.Value;
					} else {
						this.numObstacleMiddleDistanceLeft.Text = "";
					}
					Nullable<double> middleDistanceRight = obstacle.GetGraphMiddleDistanceXRight(graphicalWallPanel.SelectedWall);
					if (middleDistanceRight.HasValue) {
						this.numObstacleMiddleDistanceRight.Value = (decimal)middleDistanceRight.Value;
					} else {
						this.numObstacleMiddleDistanceRight.Text = "";
					}

					string text = EuroplanRes.HithermPlannerForm_ObjektText;
					text = text.Replace("%OBJEKT%", new GraphicalWallObstacle.ObstacleTypeConverter().ConvertToString(obstacle.ObstacleType));
					text = text.Replace("%BREITE%", this.numObstacleWidth.Value.ToString());
					text = text.Replace("%HOEHE%", this.numObstacleHeight.Value.ToString());
					this.lblObstacle.Text = text;
					//this.lblObstacle.Text = "Typ: " + new GraphicalWallObstacle.ObstacleTypeConverter().ConvertToString(obstacle.ObstacleType) + " " + this.numObstacleWidth.Value + "cm x " + this.numObstacleHeight.Value + EuroplanRes.Unit_Zentimeter;
				}
			} else {
				this.numObstacleHeight.Text = "";
				this.numObstacleWidth.Text = "";
				this.numObstacleVertical.Text = "";
				this.numObstacleHorizontalLeft.Text = "";
				this.numObstacleHorizontalRight.Text = "";
				this.numObstacleDistanceLeft.Text = "";
				this.numObstacleDistanceRight.Text = "";
				this.numObstacleMiddleDistanceLeft.Text = "";
				this.numObstacleMiddleDistanceRight.Text = "";
				this.lblObstacle.Text = EuroplanRes.HithermPlannerForm_KeinObjekt;
			}
			UpdateModifyObstaclesPanelButtons(obstacle);
			updateOngoing = false;
		}

		private void UpdateModifySchraegePanel(GraphicalWallSchraege schraege) {
			updateOngoing = true;
			this.panelModifySchraege.BringToFront();
			if (schraege != null) {
				this.numSchraegeHorizontal.Value = (decimal)schraege.Width;
				this.numSchraegeVertical.Value = (decimal)schraege.Height;
				string text = EuroplanRes.HithermPlannerForm_SchraegeText;
				text = text.Replace("%AUSRICHTUNG%", (schraege.Orientation == GraphicalWallSchraege.OrientationEnum.LEFT ? EuroplanRes.HithermPlannerForm_Links2 : EuroplanRes.HithermPlannerForm_Rechts2));
				text = text.Replace("%BREITE%", Math.Round(schraege.Width, 0).ToString());
				text = text.Replace("%HOEHE%", Math.Round(schraege.Height, 0).ToString());
				this.lblSchraege.Text = text;
				//this.lblSchraege.Text = "Schräge " + (schraege.Orientation == GraphicalWallSchraege.OrientationEnum.LEFT ? "links" : "rechts") + ": " + Math.Round(schraege.Width, 0).ToString() + "cm x " + Math.Round(schraege.Height, 0).ToString() + EuroplanRes.Unit_Zentimeter;
			} else {
				this.numSchraegeHorizontal.Text = "";
				this.numSchraegeVertical.Text = "";
				this.lblSchraege.Text = EuroplanRes.HithermPlannerForm_KeineSchraege;
			}
			UpdateModifySchraegePanelButtons(schraege);
			updateOngoing = false;
		}


		private void UpdateModifyConnectionPanel(GraphicalHithermCompactVerbindung connection) {
			updateOngoing = true;
			this.panelModifyConnection.BringToFront();
			if (connection != null) {

			} else {

			}
			UpdateModifyConnectionPanelButtons(connection);
			updateOngoing = false;
		}

		
		private void UpdateDefineWallsPanelButtons(GraphicalWall wall) {
			if (wall != null) {
				this.btnWallSelectConstruction.Enabled = true;
				this.numWallHorizontal.Enabled = !wall.IsDachSchraege;
				this.numWallVertical.Enabled = true;
				this.btnWallApply.Enabled = unsavedChanges;
				this.btnWallRevert.Enabled = unsavedChanges;
				this.btnWallDelete.Enabled = true;
				this.btnWallEdgeDistance.Enabled = true;
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
			}
		}

		private void UpdateModifyRegisterPanelButtons(GraphicalHithermCompactRegisterWrapper hithermRegister) {
			if (hithermRegister != null) {
				this.btnRegisterAccept.Enabled = unsavedChanges || hithermRegister.IsNew;
				this.btnRegisterRevert.Enabled = unsavedChanges;
				this.btnRegisterConnect.Enabled = true;
				this.btnRegisterDelete.Enabled = true;
			} else {
				this.btnRegisterAccept.Enabled = false;
				this.btnRegisterRevert.Enabled = false;
				this.btnRegisterConnect.Enabled = false;
				this.btnRegisterDelete.Enabled = false;
			}
		}

		private void UpdateModifyObstaclesPanelButtons(GraphicalWallObstacle obstacle) {
			if (obstacle != null) {
				this.btnObstacleApply.Enabled = unsavedChanges || obstacle.IsNew;
				this.btnObstacleRevert.Enabled = unsavedChanges;
				this.btnObstacleRemove.Enabled = true;
				this.btnObstacleBorder.Enabled = true;
				this.numObstacleWidth.Enabled = true;
				this.numObstacleHeight.Enabled = true;
				this.numObstacleHorizontalLeft.Enabled = true;
				this.numObstacleHorizontalRight.Enabled = true;
				this.numObstacleDistanceLeft.Enabled = true;
				this.numObstacleDistanceRight.Enabled = true;
				this.numObstacleMiddleDistanceLeft.Enabled = true;
				this.numObstacleMiddleDistanceRight.Enabled = true;
				if (obstacle is GraphicalDoor) {
					this.numObstacleVertical.Enabled = false;
				} else if (obstacle is GraphicalWindow) {
					this.numObstacleVertical.Enabled = true;
				} else if (obstacle is GraphicalOtherObstacle) {
					this.numObstacleVertical.Enabled = true;
				}
			} else {
				this.btnObstacleApply.Enabled = false;
				this.btnObstacleRevert.Enabled = false;
				this.btnObstacleRemove.Enabled = false;
				this.btnObstacleBorder.Enabled = false;
				this.numObstacleWidth.Enabled = false;
				this.numObstacleHeight.Enabled = false;
				this.numObstacleHorizontalLeft.Enabled = false;
				this.numObstacleHorizontalRight.Enabled = false;
				this.numObstacleDistanceLeft.Enabled = false;
				this.numObstacleDistanceRight.Enabled = false;
				this.numObstacleMiddleDistanceLeft.Enabled = false;
				this.numObstacleMiddleDistanceRight.Enabled = false;
			}
		}


		private void UpdateModifySchraegePanelButtons(IGraphicalWallObject schraege) {
			if (schraege != null) {
				this.btnSchraegeApply.Enabled = unsavedChanges || schraege.IsNew;
				this.btnSchraegeRevert.Enabled = unsavedChanges;
				this.btnSchraegeDelete.Enabled = true;
				this.numSchraegeHorizontal.Enabled = true;
				this.numSchraegeVertical.Enabled = true;
			} else {
				this.numSchraegeHorizontal.Enabled = false;
				this.numSchraegeVertical.Enabled = false;
				this.rbSchraegeLeft.Enabled = false;
				this.rbSchraegeRight.Enabled = false;
				this.btnSchraegeApply.Enabled = false;
				this.btnSchraegeDelete.Enabled = false;
				this.btnSchraegeRevert.Enabled = false;
			}
		}

		private void UpdateModifyConnectionPanelButtons(GraphicalHithermCompactVerbindung connection) {
			if (connection != null) {
				this.btnConnectionApply.Enabled = unsavedChanges || connection.IsNew;
				this.btnConnectionRevert.Enabled = unsavedChanges;
				this.btnConnectionDelete.Enabled = true;
			} else {
				this.btnConnectionApply.Enabled = false;
				this.btnConnectionRevert.Enabled = false;
				this.btnConnectionDelete.Enabled = false;
			}
		}

		private void btnWallSelectConstruction_Click(object sender, EventArgs e) {
			SelectHithermWallForm form = new SelectHithermWallForm(true);
			if (form.ShowDialog() == DialogResult.OK) {
				if (SelectedObject is GraphicalWall) {
					this.txtWallConstruction.Text = form.SelectedWall.Id;
					(SelectedObject as GraphicalWall).WallId = form.SelectedWall.Id;
					foreach (GraphicalHithermCompactRegisterWrapper register in (SelectedObject as GraphicalWall).Registers) {
						register.Register.WallId = form.SelectedWall.Id;
					}
					//this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateDefineWallsPanelButtons(SelectedObject as GraphicalWall);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
			form.Dispose();
		}

		private void numWallHorizontal_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWall) {
					(SelectedObject as GraphicalWall).SetWallWidth((double)numWallHorizontal.Value / 100.0);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateDefineWallsPanelButtons(SelectedObject as GraphicalWall);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numWallVertical_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWall) {
					(SelectedObject as GraphicalWall).SetWallHeight((double)numWallVertical.Value / 100.0);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateDefineWallsPanelButtons(SelectedObject as GraphicalWall);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void btnWallLeft_Click(object sender, EventArgs e) {
			if (IsChangeAllowed()) {
				GraphicalWall wall = SelectedObject as GraphicalWall;
				int index = graphicalWallPanel.Room.Walls.IndexOf(wall);
				graphicalWallPanel.Room.Walls.RemoveAt(index);
				graphicalWallPanel.Room.Walls.Insert(index - 1, wall);
				UpdateDefineWallsPanel(wall);
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void btnWallRight_Click(object sender, EventArgs e) {
			if (IsChangeAllowed()) {
				GraphicalWall wall = SelectedObject as GraphicalWall;
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
					if (wall.PlanStartPoint.HasValue || wall.PlanEndPoint.HasValue) {
						if (!removePlanPointsAllowed) {
							DialogResult result = MessageBox.Show(EuroplanRes.HithermPlannerForm_WandManuellText, EuroplanRes.HithermPlannerForm_WandManuellTitel, MessageBoxButtons.YesNoCancel);
							if (result != DialogResult.Yes) {
								return false;
							} else {
								removePlanPointsAllowed = true;
								wall.PlanStartPoint = null;
								wall.PlanEndPoint = null;
							}
						} else {
							wall.PlanStartPoint = null;
							wall.PlanEndPoint = null;
						}
					}
				}
				return true;
			}
			return false;
		}

		private void btnWallDelete_Click(object sender, EventArgs e) {
			DeleteWall();
		}

		private void DeleteWall() {
			if (MessageBox.Show(EuroplanRes.HithermPlannerForm_WandLoeschenText, EuroplanRes.HithermPlannerForm_WandLoeschenTitel, MessageBoxButtons.YesNo) == DialogResult.Yes) {
				GraphicalWall wall = SelectedObject as GraphicalWall;
				graphicalWallPanel.Room.Walls.Remove(wall);
				wall.RemoveAllRegisters(this.hithermCompactPlanner.HithermCompactProduct);
				this.graphicalWallPanel.SelectedObject = null;
				UpdateDefineWallsPanel(null);
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void btnWallApply_Click(object sender, EventArgs e) {
			GraphicalWall wall = graphicalWallPanel.SelectedObject as GraphicalWall;
			graphicalWallPanel.SelectedObject = null;
			graphicalWallPanel.SelectedObject = wall;
			this.graphicalWallPanel.InvalidateGraphics();

			/*GraphicalWall wall = SelectedObject as GraphicalWall;
			if (Math.Round((decimal)wall.GetWallWidth() * 100, 0) != numWallHorizontal.Value) {
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
			foreach (GraphicalHithermCompactRegisterWrapper register in wall.Registers) {
				register.Register.WallId = wall.WallId;
			}
			unsavedChanges = false;
			UpdateDefineWallsPanel(wall);
			this.graphicalWallPanel.InvalidateGraphics();*/
		}

		private void btnWallRevert_Click(object sender, EventArgs e) {
			GraphicalWall wall = SelectedObject as GraphicalWall;
			wall.RevertState();
			this.graphicalWallPanel.Room.MarkErrors(wall, this.graphicalWallPanel.SelectedWall);
			//graphicalWallPanel.SelectedObject = null;
			unsavedChanges = false;
			UpdateDefineWallsPanel(wall);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void numRegisterLeft_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalHithermCompactRegisterWrapper) {
					GraphicalHithermCompactRegisterWrapper wrapper = SelectedObject as GraphicalHithermCompactRegisterWrapper;
					HithermCompactCircuit circuit = this.hithermCompactPlanner.HithermCompactProduct.GetCircuitForRegister(wrapper.Register);
					foreach (GraphicalHithermCompactVerbindung link in circuit.Links) {
						if (link.Start == wrapper.Register) {
							link.RevertState();
						}
						if (link.End == wrapper.Register) {
							link.RevertState();
						}
					}
					wrapper.UpdatePosition(this.graphicalWallPanel.SelectedWall, (double)numRegisterLeft.Value, wrapper.Y, true, false);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyRegisterPanelButtons(wrapper);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numRegisterVertical_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				GraphicalHithermCompactRegisterWrapper wrapper = SelectedObject as GraphicalHithermCompactRegisterWrapper;
				this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
				unsavedChanges = true;
				UpdateModifyRegisterPanelButtons(wrapper);
			}
		}

		private void btnRegisterAccept_Click(object sender, EventArgs e) {
			graphicalWallPanel.SelectedObject = null;
			UpdateModifyRegisterPanel(null);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnRegisterDelete_Click(object sender, EventArgs e) {
			DeleteRegister(this.SelectedObject as GraphicalHithermCompactRegisterWrapper);
		}

		private void DeleteRegister(GraphicalHithermCompactRegisterWrapper register) {
			if (register != null) {
				register.Error = true;
				foreach (GraphicalWall wall in graphicalWallPanel.Room.Walls) {
					GraphicalWall wrapperWall = wall.GetWallForWrapper(register);
					if (wrapperWall != null) {
						wrapperWall.Registers.Remove(register);
					}
					hithermCompactPlanner.HithermCompactProduct.RemoveRegisterFromCircuit(register.Register);
					this.hithermCompactPlanner.HithermCompactProduct.CorrectCircuitIds();
				}
				this.graphicalWallPanel.SelectedObject = null;
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void btnRegisterRevert_Click(object sender, EventArgs e) {
			GraphicalHithermCompactRegisterWrapper wrapper = SelectedObject as GraphicalHithermCompactRegisterWrapper;
			wrapper.RevertState();
			this.graphicalWallPanel.Room.MarkErrors(wrapper, this.graphicalWallPanel.SelectedWall);
			unsavedChanges = false;
			UpdateModifyRegisterPanel(wrapper);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private GraphicalWallPanel.PlanMode oldPanelMode;
		private HithermCompactPlanner.HithermCompactPlannerMode oldPlannerMode;

		private void btnRegisterConnect_Click(object sender, EventArgs e) {
			oldPanelMode = this.graphicalWallPanel.Mode;
			oldPlannerMode = this.hithermCompactPlanner.Mode;
			this.hithermCompactPlanner.Mode = HithermCompactPlanner.HithermCompactPlannerMode.HPM_CONNECT_REGISTERS;
			this.graphicalWallPanel.Mode = GraphicalWallPanel.PlanMode.PM_PLANNER_CLICK;
		}

		private void chkRegisterHelpLines_CheckedChanged(object sender, EventArgs e) {
			hithermCompactPlanner.NewRegisterUseHelpline = chkRegisterHelpLines.Checked;
		}

		private void hithermPlanner_RecalculationNecessary(object sender, EventArgs e) {
			CalculateAndUpdate();
		}

		private class HithermCompactCircuitItem {
			private HithermCompactCircuit circuit = null;

			public HithermCompactCircuitItem(HithermCompactCircuit circuit) {
				this.circuit = circuit;
			}

			public HithermCompactCircuit Circuit {
				get { return this.circuit; }
			}

			public override string ToString() {
				return (this.circuit != null) ? this.circuit.HkLabelNr.ToString() : "";
			}

			public override bool Equals(object obj) {
				return (obj == null || !(obj is HithermCompactCircuitItem)) ? false : this.circuit == (obj as HithermCompactCircuitItem).circuit;
			}

			public override int GetHashCode() {
				return (circuit == null) ? 0 : circuit.GetHashCode();
			}
		}

		private void CalculateAndUpdate() {
			HithermCompactCircuitItem selectedItem = this.cmbHk.SelectedItem as HithermCompactCircuitItem;
			this.cmbHk.Items.Clear();
			foreach (HithermCompactCircuit c in this.hithermCompactPlanner.HithermCompactProduct.PlannedCircuits) {
				this.cmbHk.Items.Add(new HithermCompactCircuitItem(c));
			}
			if (selectedItem != null && cmbHk.Items.Contains(selectedItem)) {
				this.cmbHk.SelectedItem = selectedItem;
			} else {
				if (cmbHk.Items.Count > 0) {
					this.cmbHk.SelectedItem = this.cmbHk.Items[0];
				} else {
					this.cmbHk.SelectedItem = null;
				}
			}

			HithermCompactProduct product = this.hithermCompactPlanner.HithermCompactProduct;
			PlannedProduct pp = Project.Instance.GetPlannedProduct(product);
			product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
			string errorMsg = pp.Product.LastErrorMessage;
			this.lstError.Items.Clear();
			string[] messages;
			if (errorMsg != null) {
				messages = errorMsg.Split('\n');
				foreach (string message in messages) {
					if (!string.IsNullOrEmpty(message)) {
						ListViewItem item = new ListViewItem(message);
						item.ForeColor = Color.Red;
						//item.Font = new Font(item.Font, FontStyle.Bold);
						this.lstError.Items.Add(item);
					}
				}
			}
			string notifications = pp.Product.NotificationMessage;
			if (notifications != null) {
				messages = notifications.Split('\n');
				foreach (string message in messages) {
					if (!string.IsNullOrEmpty(message)) {
						ListViewItem item = new ListViewItem(message);
						item.ForeColor = Color.Orange;
						this.lstError.Items.Add(item);
					}
				}
			}
			notifications = ModulKlimaDeckeProduct.GlobalNotificationMessage;
			if (notifications != null) {
				messages = notifications.Split('\n');
				foreach (string message in messages) {
					if (!string.IsNullOrEmpty(message)) {
						ListViewItem item = new ListViewItem(message);
						item.ForeColor = Color.Orange;
						this.lstError.Items.Add(item);
					}
				}
			}
			if (lstError.Items.Count > 0) {
				this.lstError.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				int height = this.lstError.Items[this.lstError.Items.Count - 1].Position.Y + this.lstError.Items[this.lstError.Items.Count - 1].Bounds.Height + 5;
				this.lstError.Height = height;
				this.lstError.Visible = true;
			} else {
				this.lstError.Visible = false;
			}

			bool showHeat = pp.RequestedHeatLoad > 0;
			bool showCool = pp.RequestedCoolLoad > 0;
			//bool showHeatCircuit = selectedCircuit >= 0 && showHeat;
			//bool showCoolCircuit = selectedCircuit >= 0 && showCool;
			bool showRestArea = product.HithermCompactType == Product.ProductType.FBH || product.HithermCompactType == Product.ProductType.DH;

			lblHeat.Visible = showHeat;
			lblQHeat.Visible = showHeat;
			lblQHeatUnit.Visible = showHeat;
			lblQHeatDiff.Visible = showHeat;
			lblQHeatDiffUnit.Visible = showHeat;
			lblQHeatRest.Visible = showHeat;
			lblQHeatRestUnit.Visible = showHeat;
			//lblAvgqHeat.Visible = showHeatCircuit;
			//lblAvgqHeatUnit.Visible = showHeatCircuit;
			//lblDurchflussHeat.Visible = showHeatCircuit;
			//lblDurchflussHeatUnit.Visible = showHeatCircuit;
			//lblDruckverlustHeat.Visible = showHeatCircuit;
			//lblDruckverlustHeatUnit.Visible = showHeatCircuit;
			lblCool.Visible = showCool;
			lblQCool.Visible = showCool;
			lblQCoolUnit.Visible = showCool;
			lblQCoolDiff.Visible = showCool;
			lblQCoolDiffUnit.Visible = showCool;
			lblQCoolRest.Visible = showCool;
			lblQCoolRestUnit.Visible = showCool;
			//lblAvgqCool.Visible = showCoolCircuit;
			//lblAvgqCoolUnit.Visible = showCoolCircuit;
			//lblDurchflussCool.Visible = showCoolCircuit;
			//lblDurchflussCoolUnit.Visible = showCoolCircuit;
			//lblDruckverlustCool.Visible = showCoolCircuit;
			//lblDruckverlustCoolUnit.Visible = showCoolCircuit;

			int xDiff = this.lblNecessaryArea.Top - this.lblNecessaryWaermestromdichte.Top;
			if (showRestArea) {
				this.lblCoveredAreaTitle.Top = this.lblAvailableAreaUnit.Top + xDiff;
				this.lblCoveredArea.Top = this.lblAvailableArea.Top + xDiff;
				this.lblCoveredAreaUnit.Top = this.lblAvailableAreaUnit.Top + xDiff;
				this.lblNecessaryWaermestromdichteTitle.Top = this.lblRestAreaTitle.Top + xDiff;
				this.lblNecessaryWaermestromdichte.Top = this.lblRestArea.Top + xDiff;
				this.lblNecessaryWaermestromdichteUnit.Top = this.lblRestAreaUnit.Top + xDiff;
				this.lblNecessaryAreaTitle.Top = this.lblNecessaryWaermestromdichteTitle.Top + xDiff;
				this.lblNecessaryArea.Top = this.lblNecessaryWaermestromdichte.Top + xDiff;
				this.lblNecessaryAreaUnit.Top = this.lblNecessaryWaermestromdichteUnit.Top + xDiff;
				this.lblAvailableAreaTitle.Visible = true;
				this.lblAvailableArea.Visible = true;
				this.lblAvailableAreaUnit.Visible = true;
				this.lblRestAreaTitle.Visible = true;
				this.lblRestArea.Visible = true;
				this.lblRestAreaUnit.Visible = true;
				this.lineInfo.Height = 135;
			} else {
				this.lblAvailableAreaTitle.Visible = false;
				this.lblAvailableArea.Visible = false;
				this.lblAvailableAreaUnit.Visible = false;
				this.lblRestAreaTitle.Visible = false;
				this.lblRestArea.Visible = false;
				this.lblRestAreaUnit.Visible = false;
				this.lblCoveredAreaTitle.Top = this.lblAvailableAreaUnit.Top;
				this.lblCoveredArea.Top = this.lblAvailableArea.Top;
				this.lblCoveredAreaUnit.Top = this.lblAvailableAreaUnit.Top;
				this.lblNecessaryWaermestromdichteTitle.Top = this.lblCoveredAreaTitle.Top + xDiff;
				this.lblNecessaryWaermestromdichte.Top = this.lblCoveredArea.Top + xDiff;
				this.lblNecessaryWaermestromdichteUnit.Top = this.lblCoveredAreaUnit.Top + xDiff;
				this.lblNecessaryAreaTitle.Top = this.lblNecessaryWaermestromdichteTitle.Top + xDiff;
				this.lblNecessaryArea.Top = this.lblNecessaryWaermestromdichte.Top + xDiff;
				this.lblNecessaryAreaUnit.Top = this.lblNecessaryWaermestromdichteUnit.Top + xDiff;
				this.lineInfo.Height = 100;
			}


			//this.lblAreaTxt.Visible = showArea;
			//this.numArea.Visible = showArea;
			//this.lblAreaUnit.Visible = showArea;
			//this.numAreaPercentage.Visible = showArea;
			//this.lblAreaPercentage.Visible = showArea;



			double qDiffHeat = pp.PlannedHeatLoad - pp.RequestedHeatLoad;
			double qDiffCool = pp.PlannedCoolLoad - pp.RequestedCoolLoad;

			lblRest.Text = EuroplanRes.PlannedHithermProductPanel_Rest + " (" + product.AssociatedRoom.ToString() + ")";
			lblQHeat.Text = Math.Round(pp.PlannedHeatLoad, 0).ToString();
			lblQHeatDiff.Text = Math.Round(qDiffHeat, 0).ToString("+0;-0");
			lblQHeatRest.Text = Math.Round(product.AssociatedRoom.OpenHeatLoad, 0).ToString("+0;-0");
			lblQCool.Text = Math.Round(pp.PlannedCoolLoad, 0).ToString();
			lblQCoolDiff.Text = Math.Round(qDiffCool, 0).ToString("+0;-0");
			lblQCoolRest.Text = Math.Round(product.AssociatedRoom.OpenCoolLoad, 0).ToString("+0;-0");
			double area = product.PlannedRegisterArea;
			lblCoveredArea.Text = Math.Round(area, 2).ToString();
			lblAvailableArea.Text = Math.Round(pp.PlannedArea.HasValue ? pp.PlannedArea.Value : 0, 2).ToString();
			lblRestArea.Text = Math.Round((pp.PlannedArea.HasValue ? pp.PlannedArea.Value : 0) - area, 2).ToString();
			lblNecessaryWaermestromdichte.Text = (area > 0) ? Math.Round(pp.RequestedHeatLoad / area, 2).ToString() : "--";
			lblNecessaryArea.Text = (product.PlannedHeatLoad > 0 && area > 0) ? Math.Round(pp.RequestedHeatLoad / (product.PlannedHeatLoad / area), 2).ToString() : "--";
				

		}

		private void btnObstacleApply_Click(object sender, EventArgs e) {
			graphicalWallPanel.SelectedObject = null;
			UpdateModifyObstaclesPanel(null);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnObstacleRevert_Click(object sender, EventArgs e) {
			GraphicalWallObstacle obstacle = SelectedObject as GraphicalWallObstacle;
			obstacle.RevertState();
			this.graphicalWallPanel.Room.MarkErrors(obstacle, this.graphicalWallPanel.SelectedWall);
			//graphicalWallPanel.SelectedObject = null;
			unsavedChanges = false;
			UpdateModifyObstaclesPanel(obstacle);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnObstacleRemove_Click(object sender, EventArgs e) {
			DeleteObstacle(SelectedObject as GraphicalWallObstacle);
		}

		private void btnObstacleBorder_Click(object sender, EventArgs e) {
			GraphicalWallObstacle obstacle = SelectedObject as GraphicalWallObstacle;
			EdgeDistanceForm form = new EdgeDistanceForm(obstacle.BorderDistance);
			if (form.ShowDialog() == DialogResult.OK) {
				double distance = form.EdgeDistance > 0 ? form.EdgeDistance / 100.0 : 0;
				obstacle.BorderDistance = distance;
				this.graphicalWallPanel.InvalidateGraphics();
			}
		}

		private void DeleteObstacle(GraphicalWallObstacle obstacle) {
			if (obstacle != null) {
				foreach (GraphicalWall wall in graphicalWallPanel.Room.Walls) {
					GraphicalWall obstacleWall = wall.GetWallForObstacle(obstacle);
					if (obstacleWall != null) {
						obstacleWall.Obstacles.Remove(obstacle);
						obstacle.Error = true;
						if (obstacle == this.SelectedObject) {
							graphicalWallPanel.SelectedObject = null;
							UpdateModifyObstaclesPanel(null);
						}
						this.graphicalWallPanel.InvalidateGraphics();
					}
				}
			}
		}

		private void DeleteSchraege(GraphicalWallSchraege schraege) {
			if (schraege != null) {
				schraege.Width = 0;
				this.UpdateModifySchraegePanel(null);
				this.graphicalWallPanel.SelectedObject = null;
				/*foreach (GraphicalWall wall in graphicalWallPanel.Room.Walls) {
					GraphicalWall schraegeWall = wall.GetWallForObstacle(schraege);
					if (schraegeWall != null) {
						obstacleWall.Obstacles.Remove(obstacle);
						obstacle.Error = true;
						if (obstacle == this.SelectedObject) {
							graphicalWallPanel.SelectedObject = null;
							UpdateModifyObstaclesPanel(null);
						}
						this.graphicalWallPanel.InvalidateGraphics();
					}
				}*/
			}
		}

		private void graphicalWallPanel_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Delete) {
				if (SelectedObject != null) {
					if (SelectedObject is GraphicalWall) {
						DeleteWall();
					} else if (SelectedObject is GraphicalWallObstacle) {
						DeleteObstacle(SelectedObject as GraphicalWallObstacle);
					} else if (SelectedObject is GraphicalHithermCompactRegisterWrapper) {
						DeleteRegister(SelectedObject as GraphicalHithermCompactRegisterWrapper);
					} else if (SelectedObject is GraphicalHithermCompactVerbindung) {
						DeleteVerbindung(SelectedObject as GraphicalHithermCompactVerbindung);
					} else if (SelectedObject is GraphicalWallSchraege) {
						DeleteSchraege(SelectedObject as GraphicalWallSchraege);
					}
				}
			}
			if (e.Shift) {
				InvertSnap();
			}
		}

		private void graphicalWallPanel_KeyUp(object sender, KeyEventArgs e) {
			RestoreSnap();
		}

		private void DeleteVerbindung(GraphicalHithermCompactVerbindung verbindung) {
			if (verbindung != null) {
				foreach (HithermCompactCircuit c in this.hithermCompactPlanner.HithermCompactProduct.PlannedCircuits) {
					GraphicalHithermCompactVerbindung foundLink = null;
					foreach (GraphicalHithermCompactVerbindung curLink in c.Links) {
						if (curLink.EqualsOrIsPart(verbindung)) {
							foundLink = curLink;
							break;
						}
					}

					if (foundLink != null) {
					//if (c.Links.Contains(verbindung)) {
						bool moveCircuit = foundLink.HasStart && foundLink.HasEnd;
						c.Links.Remove(foundLink);
						if (foundLink is GraphicalHithermCompactUnderfloorVerbindung) {
							if (verbindung != (foundLink as GraphicalHithermCompactUnderfloorVerbindung).StartLink) {
								(foundLink as GraphicalHithermCompactUnderfloorVerbindung).StartLink.IsPartOfCompound = false;
								c.Links.Add((foundLink as GraphicalHithermCompactUnderfloorVerbindung).StartLink);
							}
							if (verbindung != (foundLink as GraphicalHithermCompactUnderfloorVerbindung).EndLink) {
								(foundLink as GraphicalHithermCompactUnderfloorVerbindung).EndLink.IsPartOfCompound = false;
								c.Links.Add((foundLink as GraphicalHithermCompactUnderfloorVerbindung).EndLink);
							}
						}
						if (verbindung == this.graphicalWallPanel.SelectedObject) {
							this.graphicalWallPanel.SelectedObject = null;
						}
						if (moveCircuit) {
							HithermCompactRegister registerToMove = foundLink.End;
							this.hithermCompactPlanner.HithermCompactProduct.MoveRegisterToCircuit(registerToMove, this.hithermCompactPlanner.HithermCompactProduct.GetNewHkId());
							this.hithermCompactPlanner.HithermCompactProduct.CorrectCircuitIds();
						}
						this.graphicalWallPanel.InvalidateGraphics();
						break;
					}
				}
			}
		}

		private void numObstacleWidth_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).Width = (double)numObstacleWidth.Value;
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanelButtons(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numObstacleHeight_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).Height = (double)numObstacleHeight.Value;
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanelButtons(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numObstacleVertical_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).GraphPosY = (double)numObstacleVertical.Value;
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanelButtons(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numObstacleHorizontalLeft_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).SetGraphPosXLeft((double)numObstacleHorizontalLeft.Value, graphicalWallPanel.SelectedWall);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanel(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numObstacleHorizontalRight_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).SetGraphPosXRight((double)numObstacleHorizontalRight.Value, graphicalWallPanel.SelectedWall);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanel(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numObstacleDistanceLeft_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).SetGraphDistanceXLeft((double)numObstacleDistanceLeft.Value, graphicalWallPanel.SelectedWall);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanel(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numObstacleDistanceRight_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).SetGraphDistanceXRight((double)numObstacleDistanceRight.Value, graphicalWallPanel.SelectedWall);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanel(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numObstacleMiddleDistanceLeft_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).SetGraphMiddleDistanceXLeft((double)numObstacleMiddleDistanceLeft.Value, graphicalWallPanel.SelectedWall);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanel(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numObstacleMiddleDistanceRight_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallObstacle) {
					(SelectedObject as GraphicalWallObstacle).SetGraphMiddleDistanceXRight((double)numObstacleMiddleDistanceRight.Value, graphicalWallPanel.SelectedWall);
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifyObstaclesPanel(SelectedObject as GraphicalWallObstacle);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void btnSchraegeApply_Click(object sender, EventArgs e) {
			graphicalWallPanel.SelectedObject = null;
			UpdateModifySchraegePanel(null);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnSchraegeRevert_Click(object sender, EventArgs e) {
			GraphicalWallSchraege schraege = SelectedObject as GraphicalWallSchraege;
			schraege.RevertState();
			this.graphicalWallPanel.Room.MarkErrors(schraege, this.graphicalWallPanel.SelectedWall);
			//graphicalWallPanel.SelectedObject = null;
			unsavedChanges = false;
			UpdateModifySchraegePanel(schraege);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnSchraegeDelete_Click(object sender, EventArgs e) {
			GraphicalWallSchraege schraege = SelectedObject as GraphicalWallSchraege;
			this.DeleteSchraege(schraege);
		}

		private void numSchraegeHorizontal_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallSchraege) {
					(SelectedObject as GraphicalWallSchraege).Width = (double)numSchraegeHorizontal.Value;
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifySchraegePanelButtons(SelectedObject as GraphicalWallSchraege);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void numSchraegeVertical_ValueChanged(object sender, EventArgs e) {
			if (!updateOngoing) {
				if (SelectedObject is GraphicalWallSchraege) {
					(SelectedObject as GraphicalWallSchraege).Height = (double)numSchraegeVertical.Value;
					this.graphicalWallPanel.Room.MarkErrors(SelectedObject, this.graphicalWallPanel.SelectedWall);
					unsavedChanges = true;
					UpdateModifySchraegePanelButtons(SelectedObject as GraphicalWallSchraege);
					this.graphicalWallPanel.InvalidateGraphics();
				}
			}
		}

		private void rbSchraegeLeft_CheckedChanged(object sender, EventArgs e) {
			DeleteSchraege(SelectedObject as GraphicalWallSchraege);
		}

		private void rbSchraegeRight_CheckedChanged(object sender, EventArgs e) {

		}

		private void btnConnectionDelete_Click(object sender, EventArgs e) {
			DeleteVerbindung(SelectedObject as GraphicalHithermCompactVerbindung);
		}

		private void btnConnectionRevert_Click(object sender, EventArgs e) {
			SelectedObject.RevertState();
			this.graphicalWallPanel.Room.MarkErrors(SelectedObject, null);
			unsavedChanges = false;
			UpdateModifyConnectionPanel(SelectedObject as GraphicalHithermCompactVerbindung);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnConnectionApply_Click(object sender, EventArgs e) {
			graphicalWallPanel.SelectedObject = null;
			UpdateModifyConnectionPanel(null);
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnWallHelpLine_Click(object sender, EventArgs e) {
			WallHelpLinesForm form = new WallHelpLinesForm(graphicalWallPanel.Room, graphicalWallPanel.SelectedObject as GraphicalWall);
			form.ShowDialog();
			form.Dispose();
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void btnUseHelplines_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.SnapEnabled = !this.btnUseHelplines.Checked;
			this.btnUseHelplines.Checked = this.graphicalWallPanel.SnapEnabled;
		}

		private void btnExport_Click(object sender, EventArgs e) {
			float border = 20.0f;
			Room room = graphicalWallPanel.Room;
			if (room != null && room.Walls.Count > 0) {
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.CheckPathExists = true;
				dialog.CreatePrompt = true;
				dialog.OverwritePrompt = true;
				dialog.InitialDirectory = Path.GetDirectoryName(Project.Instance.ProjectFileName);
				string productName = "";
				foreach (PlannedProduct pp in room.PlannedProducts) {
					if (pp.Product == this.hithermCompactPlanner.Product) {
						productName = pp.Node.Text.Replace(":", "_");
						productName = productName.Replace(" ", "");
						break;
					}
				}
				dialog.FileName = room.AssociatedFloor.Name + "_" + room.Name + "_" + productName;
				dialog.DefaultExt = ".jpg";
				dialog.Filter = EuroplanRes.HithermPlannerForm_FilterBild + "|*.jpg;*.png;*.bmp";
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					
					Bitmap b;
					Graphics g;
					double width = (room.GetWallOffset(room.Walls[room.Walls.Count - 1]).Value.X * 100.0) + (room.Walls[room.Walls.Count - 1].GetWallWidth() * 100.0);
					double height = 0;
					foreach (GraphicalWall wall in room.Walls) {
						if (wall.DachSchraege != null) {
							height = Math.Max(height, (wall.GetWallHeight() + wall.DachSchraege.GetWallHeight()) * 100.0);
						} else {
							height = Math.Max(height, wall.GetWallHeight() * 100.0);
						}
					}
					width += border * 2;
					height += border * 2;
					b = new Bitmap((int)width, (int)height);
					g = Graphics.FromImage(b);
					//g.InterpolationMode = InterpolationMode.HighQualityBicubic;

					g.ResetClip();
					g.FillRectangle(Brushes.White, 0, 0, (float)width, (float)height);
					Matrix matrix = new Matrix();
					matrix.Scale(1.0f, -1.0f);
					matrix.Translate(border, -(float)(height - border));
					g.Transform = matrix;

					foreach (GraphicalWall wall in room.Walls) {
						double xOffset = room.GetWallOffset(wall).Value.X * 100.0;
						double yOffset = 0;
						wall.PaintObject(g, xOffset, yOffset, null, null, 1.0, true);
					}

					foreach (HithermCompactCircuit c in hithermCompactPlanner.Product.PlannedCircuits) {
						foreach (GraphicalHithermCompactVerbindung link in c.Links) {
							link.PaintObject(g, 0, 0, null, 1.0, true);
						}
					}

					ImageFormat format = null;
					string extension = Path.GetExtension(dialog.FileName);
					if (extension.ToLower() == ".jpg") {
						format = ImageFormat.Jpeg;
					} else if (extension.ToLower() == ".png") {
						format = ImageFormat.Png;
					} if (extension.ToLower() == ".bmp") {
						format = ImageFormat.Bmp;
					}
					b.Save(dialog.FileName, format);
					g.Dispose();
				}
			} else {
				MessageBox.Show(EuroplanRes.HithermPlannerForm_KeineWaendeText);
			}
		}

		//private bool oldSnap = true;
		//private bool oldDirect = false;
		private bool snapInverted = false;

		private void InvertSnap() {
			if (!this.snapInverted) {
				this.graphicalWallPanel.SnapEnabled = !this.btnUseHelplines.Checked;
				this.btnUseHelplines.Checked = this.graphicalWallPanel.SnapEnabled;

				this.hithermCompactPlanner.NewConnectionsAlign = !this.btnConnectionAlign.Checked;
				this.btnConnectionAlign.Checked = this.hithermCompactPlanner.NewConnectionsAlign;
				this.btnConnectionDirect.Checked = !this.hithermCompactPlanner.NewConnectionsAlign;
				this.snapInverted = true;
			}
		}

		private void RestoreSnap() {
			if (this.snapInverted) {
				this.graphicalWallPanel.SnapEnabled = !this.btnUseHelplines.Checked;
				this.btnUseHelplines.Checked = this.graphicalWallPanel.SnapEnabled;

				this.hithermCompactPlanner.NewConnectionsAlign = !this.btnConnectionAlign.Checked;
				this.btnConnectionAlign.Checked = this.hithermCompactPlanner.NewConnectionsAlign;
				this.btnConnectionDirect.Checked = !this.hithermCompactPlanner.NewConnectionsAlign;
				this.snapInverted = false;
			}
		}

		private void hithermPlanner_RestoreMode(object sender, EventArgs e) {
			this.graphicalWallPanel.Mode = this.oldPanelMode;
			this.hithermCompactPlanner.Mode = this.oldPlannerMode;
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void SetProductPlanner() {
			this.planPanel.Visible = false;
			this.graphicalWallPanel.Visible = true;
			this.panelTop.Visible = true;
		}

		private void SetConnectionPlanner() {
			this.graphicalWallPanel.Visible = false;
			this.panelTop.Visible = false;
			this.planPanel.Visible = true;
		}

		private int ignoreVlRlChange = 0;

		private void numVl_ValueChanged(object sender, EventArgs e) {
			if (ignoreVlRlChange == 0) {
				if (this.cmbHk.SelectedItem != null && this.cmbHk.SelectedItem is HithermCompactCircuitItem) {
					(this.cmbHk.SelectedItem as HithermCompactCircuitItem).Circuit.GraphicalAdditionalVl = (double)this.numVl.Value;
				}
			}
		}

		private void numRl_ValueChanged(object sender, EventArgs e) {
			if (ignoreVlRlChange == 0) {
				if (this.cmbHk.SelectedItem != null && this.cmbHk.SelectedItem is HithermCompactCircuitItem) {
					(this.cmbHk.SelectedItem as HithermCompactCircuitItem).Circuit.GraphicalAdditionalRl = (double)this.numRl.Value;
				}
			}
		}

		private void cmbHk_SelectedValueChanged(object sender, EventArgs e) {
			this.UpdateAdditionalVlRlForCircuit();
		}

		private void UpdateAdditionalVlRlForCircuit() {
			ignoreVlRlChange++;
			if (this.cmbHk.SelectedItem != null && this.cmbHk.SelectedItem is HithermCompactCircuitItem) {
				this.numVl.Enabled = true;
				this.numRl.Enabled = true;
				this.numVl.Value = (decimal)(this.cmbHk.SelectedItem as HithermCompactCircuitItem).Circuit.GraphicalAdditionalVl;
				this.numRl.Value = (decimal)(this.cmbHk.SelectedItem as HithermCompactCircuitItem).Circuit.GraphicalAdditionalRl;
			} else {
				this.numVl.Enabled = false;
				this.numRl.Enabled = false;
				this.numVl.Text = "";
				this.numRl.Text = "";
			}
			ignoreVlRlChange--;
		}
	}
}