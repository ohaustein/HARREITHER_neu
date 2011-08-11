using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using WW.Math;
using System.Drawing;
using System.Drawing.Drawing2D;
using WW.Math.Geometry;
using System.Windows.Forms;

namespace Europlan.Common {
	public class HithermCompactPlanner : Component, IWallProductPlanner {

		public enum HithermCompactPlannerMode {
			HPM_NONE,
			HPM_ADD_REGISTER,
			HPM_ADD_CONNECTION,
			HPM_CONNECT_REGISTERS
		}

		public enum NewConnectionModeEnum {
			NCM_MANUAL,
			NCM_AUTO,
			NCM_DIRECT
		}

		private GraphicalWallPanel connectedWallPanel;
		private HithermCompactPlannerMode mode = HithermCompactPlannerMode.HPM_NONE;

		private Nullable<Point2D> dragStart = null;
		private Point2D dragEnd = Point2D.Zero;

		private GraphicalHithermCompactRegisterWrapper newRegister = null;
		private GraphicalWall newRegisterWall = null;
		private double newRegisterWallXOffset = 0;
		private double newRegisterWallYOffset = 0;
		private bool newRegisterOk = true;

		private bool newRegisterVorlaufRight = true;
		private bool newRegisterUseHelpline = true;
		private bool newRegisterParapet = false;

		private PossibleHithermCompactRegisterConnection newConnectionStart = null;
		private PossibleConnection endConnection = null;
		private PossibleConnection startConnection = null;
		private List<PossibleConnection> highlightedConnections = new List<PossibleConnection>();
		private GraphicalHithermCompactVerbindung newConnection = new GraphicalHithermCompactVerbindung();
		private GraphicalHithermCompactVerbindung newConnectionDraw = new GraphicalHithermCompactVerbindung(false);
		private bool newConnectionsAlign = true;

		private GraphicalHithermCompactRegisterWrapper newConnectionAutoStart = null;
		private PossibleHithermCompactRegisterConnection newConnectionEnd = null;
		private Vector2D newConnectionAutoStartWallOffset = new Vector2D();

		//private GraphicalHithermRegisterWrapper connectRegistersFirst = null;
		//
		private NewConnectionModeEnum newConnectionMode = NewConnectionModeEnum.NCM_AUTO;

		public NewConnectionModeEnum NewConnectionMode {
			get { return this.newConnectionMode; }
			set {
				if (value != this.newConnectionMode) {
					this.newConnectionAutoStart = null;
					this.newConnectionStart = null;
					this.newConnection.Vertices.Clear();
					this.newConnectionDraw.Vertices.Clear();
					this.newConnectionMode = value;
					this.customCursor = null;
					if (this.connectedWallPanel != null) {
						this.connectedWallPanel.SelectedObject = null;
						this.connectedWallPanel.InvalidateGraphics();
					}
				}
			}
		}

		public bool NewConnectionsAlign {
			get { return this.newConnectionsAlign; }
			set { this.newConnectionsAlign = value; }
		}

		public bool NewRegisterVorlaufRight {
			get { return newRegisterVorlaufRight; }
			set { newRegisterVorlaufRight = value; }
		}

		public bool NewRegisterUseHelpline {
			get { return newRegisterUseHelpline; }
			set { newRegisterUseHelpline = value; }
		}

		public bool NewRegisterParapet {
			get { return newRegisterParapet; }
			set { newRegisterParapet = value; }
		}

		private HithermCompactProduct product = null;

		private event EventHandler<EventArgs> recalculationNecessary;
		public event EventHandler<EventArgs> RecalculationNecessary {
			add { this.recalculationNecessary += value; }
			remove { this.recalculationNecessary -= value; }
		}

		private event EventHandler<EventArgs> restoreMode;
		public event EventHandler<EventArgs> RestoreMode {
			add { this.restoreMode += value; }
			remove { this.restoreMode -= value; }
		}

		protected virtual void OnRecalculationNecessary() {
			if (this.recalculationNecessary != null) {
				this.recalculationNecessary(this, EventArgs.Empty);
			}
		}

		#region IWallProductPlanner Members
		public GraphicalWallPanel ConnectedWallPanel {
			get { return this.connectedWallPanel; }
			set {
				/*if (this.connectedWallPanel != null) {
					this.connectedWallPanel.KeyDown -= new KeyEventHandler(connectedPlanPanel_KeyDown);
				}*/
				this.connectedWallPanel = value;
				/*if (this.connectedWallPanel != null) {
					this.connectedWallPanel.KeyDown += new KeyEventHandler(connectedPlanPanel_KeyDown);
				}*/
			}
		}

		private Cursor customCursor = null;

		public Cursor CustomCursor {
			//get { return customCursor; }
			get { return null; }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl, double scale) {
			this.PaintAfterPlanPannel(e.Graphics, mousePositionInPlan, mousePositionInControl, scale);
		}

		public void PaintAfterPlanPannel(System.Drawing.Graphics g, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl, double scale) {
			if (this.mode == HithermCompactPlannerMode.HPM_ADD_REGISTER && dragStart != null) {
				float x = (float)(this.dragStart.Value.X < mousePositionInPlan.X ? this.dragStart.Value.X : this.dragEnd.X);
				float width = (float)Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				float y = (float)(this.dragStart.Value.Y < mousePositionInPlan.Y ? this.dragStart.Value.Y : this.dragEnd.Y);
				float height = (float)Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				Brush brush = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Green, Color.Transparent);
				Pen pen = new Pen(brush, (float)(1.0 / scale));
				g.DrawRectangle(pen, x, y, width, height);
				if (this.newRegister != null) {
					this.newRegister.PaintObject(g, newRegisterWallXOffset, newRegisterWallYOffset, Color.Green, scale, !this.newRegisterOk/*, false*/);
				}
			}
			foreach (HithermCompactCircuit c in this.product.PlannedCircuits) {
				foreach (GraphicalHithermCompactVerbindung link in c.Links) {
					if (link != this.connectedWallPanel.SelectedObject) {
						link.PaintObject(g, 0, 0, this.connectedWallPanel.SelectedObject, scale, false);
					}
				}
			}
			if (this.mode == HithermCompactPlannerMode.HPM_ADD_CONNECTION) {
				if (this.highlightedConnections != null) {
					Brush bInput = new SolidBrush(Color.FromArgb(127, Color.Red));
					Pen pInput = new Pen(Color.Red, (float)(1.0 / scale));
					Brush bOutput = new SolidBrush(Color.FromArgb(127, Color.Blue));
					Pen pOutput = new Pen(Color.Blue, (float)(1.0 / scale));
					Brush bInOutput = new SolidBrush(Color.FromArgb(127, Color.Gray));
					Pen pInOutput = new Pen(Color.Gray, (float)(1.0 / scale));
					foreach (PossibleConnection conn in this.highlightedConnections) {
						Brush b;
						Pen p;
						if (conn.PossibleInput && conn.PossibleOutput) {
							b = bInOutput;
							p = pInOutput;
						} else if (conn.PossibleInput == !conn.InvertColors) {
							b = bInput;
							p = pInput;
						} else {
							b = bOutput;
							p = pOutput;
						}
						PointF[] polygon = new PointF[conn.ConnectionArea.Count];
						int i = 0;
						foreach (Point2D point in conn.ConnectionArea) {
							polygon[i] = new PointF((float)point.X, (float)point.Y);
							i++;
						}
						g.FillPolygon(b, polygon);
						g.DrawPolygon(p, polygon);
					}
				}
				if (this.newConnectionStart != null) {
					//PossibleConnection endConn;
					//this.newConnectionDraw.Vertices = new List<Point2D>(this.newConnection.Vertices);
					//this.newConnectionDraw.Vertices.AddRange(this.GetNextConnectionVerticesInclConnectionPoints(mousePositionInPlan, out endConn));
					this.newConnectionDraw.PaintObject(g, Color.Green, !this.newConnectionDraw.CheckValidity(null, 0, 0), scale, false);
				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermCompactPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermCompactPlannerMode.HPM_ADD_CONNECTION) {
				if (this.newConnectionMode == NewConnectionModeEnum.NCM_MANUAL) {
					if (this.newConnectionStart == null) {
						this.newConnection.Vertices.Clear();
						foreach (GraphicalWall baseWall in this.product.AssociatedRoom.Walls) {
							GraphicalWall wall = baseWall;
							while (wall != null) {
								Nullable<Vector2D> offset = this.product.AssociatedRoom.GetWallOffset(wall) * 100;
								if (offset.HasValue) {
									foreach (PossibleConnection conn in this.highlightedConnections) {
										if (conn is PossibleHithermCompactRegisterConnection && conn.ConnectionArea.IsInside(planPoint)) {
											if (this.connectedWallPanel != null) {
												this.connectedWallPanel.SelectedObject = null;
											}
											this.newConnectionStart = conn as PossibleHithermCompactRegisterConnection;
											this.newConnection.Vertices.Add(this.newConnectionStart.ConnectionPoint);
											return true;
										}
									}
								}
								wall = wall.DachSchraege;
							}
						}
					} else {
						PossibleConnection endConnection;

						this.newConnectionDraw.Vertices = new List<Point2D>(this.newConnection.Vertices);
						List<Point2D> nextVertices = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endConnection);
						this.newConnectionDraw.Vertices.AddRange(nextVertices);
						if (this.newConnectionDraw.CheckValidity(null, 0, 0)) {

							this.newConnection.Vertices.AddRange(nextVertices);
							this.newConnection.Simplify();
							if (endConnection != null) {
								if ((endConnection == null || endConnection.PossibleInput) && (startConnection == null || startConnection.PossibleOutput)) {
									startConnection = newConnectionStart;
								} else {
									startConnection = endConnection;
									endConnection = newConnectionStart;
									this.newConnection.Vertices.Reverse();
								}
								HithermCompactRegister endRegister = null;
								HithermCompactCircuit endCircuit = null;
								HithermCompactRegister startRegister = null;
								HithermCompactCircuit startCircuit = null;
								if (endConnection is PossibleHithermCompactRegisterConnection) {
									endRegister = (endConnection as PossibleHithermCompactRegisterConnection).Register;
									endCircuit = this.product.GetCircuitForRegister(endRegister);
								}
								if (startConnection is PossibleHithermCompactRegisterConnection) {
									startRegister = (startConnection as PossibleHithermCompactRegisterConnection).Register;
									startCircuit = this.product.GetCircuitForRegister(startRegister);
								}

								if (startRegister != null && endRegister != null) {
									HithermCompactCircuit combinedCircuit = (startCircuit.HkLabelNr < endCircuit.HkLabelNr ? startCircuit : endCircuit);
									HithermCompactCircuit deleteCircuit = combinedCircuit == startCircuit ? endCircuit : startCircuit;
									int combinedCircuitNr = combinedCircuit.HkLabelNr;
									while (deleteCircuit.Registers.Count > 0) {
										this.product.MoveRegisterToCircuit(deleteCircuit.Registers[0], combinedCircuitNr);
									}
									GraphicalHithermCompactVerbindung newLink = new GraphicalHithermCompactVerbindung(startRegister, endRegister, this.newConnection.Vertices, combinedCircuit, Project.Instance.GetPlannedProduct(this.product));
									combinedCircuit.Links.Add(newLink);
									if (this.connectedWallPanel != null) {
										this.connectedWallPanel.SelectedObject = newLink;
									}
								} else {
									HithermCompactCircuit circuitToAdd = (startCircuit == null ? endCircuit : startCircuit);
									GraphicalHithermCompactVerbindung newLink = new GraphicalHithermCompactVerbindung(startRegister, endRegister, this.newConnection.Vertices, circuitToAdd, Project.Instance.GetPlannedProduct(this.product));
									circuitToAdd.Links.Add(newLink);
									if (this.connectedWallPanel != null) {
										this.connectedWallPanel.SelectedObject = newLink;
									}
								}
								this.product.CorrectCircuitIds();
								this.newConnectionStart = null;
								this.endConnection = null;
								this.newConnection.Vertices.Clear();
							}
						}
						return true;
					}
				} else if (this.newConnectionMode == NewConnectionModeEnum.NCM_AUTO) {
					if (this.newConnectionAutoStart == null) {
						this.newConnectionAutoStart = this.GetRegisterForPoint(planPoint);
						double offsetX, offsetY;
						this.product.AssociatedRoom.GetWallForPoint(planPoint, out offsetX, out offsetY);
						this.newConnectionAutoStartWallOffset = new Vector2D(offsetX, offsetY);
						if (this.newConnectionAutoStart != null && this.connectedWallPanel != null) {
							this.connectedWallPanel.SelectedObject = this.newConnectionAutoStart;
							this.connectedWallPanel.InvalidateGraphics();
						}
					} else {
						if (this.newConnectionDraw.CheckValidity(null, 0, 0)) {
							HithermCompactRegister startRegister = this.newConnectionStart.PossibleOutput ? this.newConnectionStart.Register : this.newConnectionEnd.Register;
							HithermCompactRegister endRegister = this.newConnectionStart.PossibleOutput ? this.newConnectionEnd.Register : this.newConnectionStart.Register;
							if (!this.newConnectionStart.PossibleOutput) {
								this.newConnectionDraw.Vertices.Reverse();
							}
							HithermCompactCircuit startCircuit = this.product.GetCircuitForRegister(startRegister);
							HithermCompactCircuit endCircuit = this.product.GetCircuitForRegister(endRegister);
							HithermCompactCircuit combinedCircuit = (startCircuit.HkLabelNr < endCircuit.HkLabelNr ? startCircuit : endCircuit);
							HithermCompactCircuit deleteCircuit = combinedCircuit == startCircuit ? endCircuit : startCircuit;
							int combinedCircuitNr = combinedCircuit.HkLabelNr;
							while (deleteCircuit.Registers.Count > 0) {
								this.product.MoveRegisterToCircuit(deleteCircuit.Registers[0], combinedCircuitNr);
							}
							GraphicalHithermCompactVerbindung newLink = new GraphicalHithermCompactVerbindung(startRegister, endRegister, this.newConnectionDraw.Vertices, combinedCircuit, Project.Instance.GetPlannedProduct(this.product));
							combinedCircuit.Links.Add(newLink);
							if (this.connectedWallPanel != null) {
								this.connectedWallPanel.SelectedObject = newLink;
							}
							this.product.CorrectCircuitIds();
							this.newConnectionStart = null;
							this.newConnectionEnd = null;
							this.newConnectionDraw.Vertices.Clear();
							this.newConnectionAutoStart = null;
						}
					}
				}
				if (this.restoreMode != null) {
					this.restoreMode(this, EventArgs.Empty);
				}
			}

			return false;
		}

		private GraphicalHithermCompactRegisterWrapper GetRegisterForPoint(Point2D planPoint) {
			double offsetX, offsetY;
			GraphicalWall wall = this.product.AssociatedRoom.GetWallForPoint(planPoint, out offsetX, out offsetY);
			if (wall != null) {
				foreach (GraphicalHithermCompactRegisterWrapper register in wall.Registers) {
					if (register.HitTest(planPoint, offsetX, offsetY)) {
						return register;
					}
				}
			}
			return null;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermCompactPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermCompactPlannerMode.HPM_ADD_CONNECTION) {
				if (this.newConnectionMode == NewConnectionModeEnum.NCM_MANUAL) {
					PossibleConnection endConn;
					foreach (PossibleConnection conn in this.highlightedConnections) {
						if (conn is PossibleHithermCompactRegisterConnection && conn.ConnectionArea.IsInside(planPoint)) {
							if (this.newConnectionStart != null) {
								this.newConnectionDraw.Vertices = new List<Point2D>(this.newConnection.Vertices);
								this.newConnectionDraw.Vertices.AddRange(this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endConn));
							}
							return true;
						}
					}
					this.highlightedConnections.Clear();
					//if (this.startConnection == null) {
					foreach (GraphicalWall baseWall in this.product.AssociatedRoom.Walls) {
						GraphicalWall wall = baseWall;
						while (wall != null) {
							Nullable<Vector2D> offset = this.product.AssociatedRoom.GetWallOffset(wall) * 100;
							if (offset.HasValue) {
								foreach (GraphicalRegisterWrapper wrapper in wall.Registers) {
									if (wrapper != this.connectedWallPanel.SelectedObject && wrapper is GraphicalHithermCompactRegisterWrapper && wrapper.HitTest(planPoint, offset.Value.X, offset.Value.Y)) {
										HithermCompactRegister register = (wrapper as GraphicalHithermCompactRegisterWrapper).Register;
										HithermCompactCircuit circuit = this.product.GetCircuitForRegister(register);
										if (circuit.IsConnectionAvailable(register, true) && (this.newConnectionStart == null || circuit != this.newConnectionStart.Circuit)) {
											highlightedConnections.Add((wrapper as GraphicalHithermCompactRegisterWrapper).GetInputConnection(offset.Value.X, offset.Value.Y, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermCompactRegisterWrapper).Register)));
										}
										if (circuit.IsConnectionAvailable(register, false) && (this.newConnectionStart == null || circuit != this.newConnectionStart.Circuit)) {
											highlightedConnections.Add((wrapper as GraphicalHithermCompactRegisterWrapper).GetOutputConnection(offset.Value.X, offset.Value.Y, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermCompactRegisterWrapper).Register)));
										}
									}
								}
							}
							if (this.newConnectionStart != null) {
								PossibleConnection wallConnection = wall.GetPossibleConnection(planPoint, offset.Value.X, offset.Value.Y);
								if (wallConnection != null) {
									if (this.newConnectionStart.PossibleInput) {
										wallConnection.PossibleInput = false;
									} else {
										wallConnection.PossibleOutput = false;
									}
									highlightedConnections.Add(wallConnection);
								}
							}
							wall = wall.DachSchraege;
						}
					}
					//}
					if (this.newConnectionStart != null) {
						this.newConnectionDraw.Vertices = new List<Point2D>(this.newConnection.Vertices);
						this.newConnectionDraw.Vertices.AddRange(this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endConn));
					}
					return true;
				} else if (this.newConnectionMode == NewConnectionModeEnum.NCM_AUTO) {
					if (this.newConnectionAutoStart == null) {
						GraphicalHithermCompactRegisterWrapper register = this.GetRegisterForPoint(planPoint);
						if (register != null && (register.GetOutputLink() == null || register.GetInputLink() == null)) {
							this.customCursor = null;
						} else {
							this.customCursor = Cursors.No;
						}
					} else {
						double endOffsetX, endOffsetY;
						GraphicalWall endWall = this.product.AssociatedRoom.GetWallForPoint(planPoint, out endOffsetX, out endOffsetY);
						GraphicalHithermCompactRegisterWrapper endRegister = this.GetRegisterForPoint(planPoint);

						if (endRegister != null) {
							GraphicalHithermCompactRegisterWrapper startRegister = this.newConnectionAutoStart;
							double startOffsetX = this.newConnectionAutoStartWallOffset.X;
							double startOffsetY = this.newConnectionAutoStartWallOffset.Y;
#if BLUB
							if (endRegister.Register.GraphVorlaufRight == this.newConnectionAutoStart.Register.GraphVorlaufRight) {
								if (endRegister.X + endOffsetX > startRegister.X + startOffsetX + startRegister.Width) {
									if (endRegister.Register.GraphVorlaufRight && startRegister.GetInputLink() == null && endRegister.GetOutputLink() == null) {
										this.newConnectionStart = startRegister.GetInputConnection(startOffsetX, startOffsetY, this.product, this.product.GetCircuitForRegister(startRegister.Register));
										this.newConnectionEnd = endRegister.GetOutputConnection(endOffsetX, endOffsetY, product, product.GetCircuitForRegister(endRegister.Register));
										this.newConnectionDraw.Vertices = this.GetNextAutomaticConnectionVertices(this.newConnectionEnd);
										this.customCursor = this.newConnectionDraw.CheckValidity(null, 0, 0) ? null : Cursors.No;
										return true;
									} else if (!endRegister.Register.GraphVorlaufRight && startRegister.GetOutputLink() == null && endRegister.GetInputLink() == null) {
										this.newConnectionStart = startRegister.GetOutputConnection(startOffsetX, startOffsetY, this.product, this.product.GetCircuitForRegister(startRegister.Register));
										this.newConnectionEnd = endRegister.GetInputConnection(endOffsetX, endOffsetY, product, product.GetCircuitForRegister(endRegister.Register));
										this.newConnectionDraw.Vertices = this.GetNextAutomaticConnectionVertices(this.newConnectionEnd);
										this.customCursor = this.newConnectionDraw.CheckValidity(null, 0, 0) ? null : Cursors.No;
										return true;
									}
								} else if (startRegister.X + startOffsetX > endRegister.X + endOffsetX + endRegister.Width) {
									if (endRegister.Register.GraphVorlaufRight && startRegister.GetOutputLink() == null && endRegister.GetInputLink() == null) {
										this.newConnectionStart = startRegister.GetOutputConnection(startOffsetX, startOffsetY, this.product, this.product.GetCircuitForRegister(startRegister.Register));
										this.newConnectionEnd = endRegister.GetInputConnection(endOffsetX, endOffsetY, product, product.GetCircuitForRegister(endRegister.Register));
										this.newConnectionDraw.Vertices = this.GetNextAutomaticConnectionVertices(this.newConnectionEnd);
										this.customCursor = this.newConnectionDraw.CheckValidity(null, 0, 0) ? null : Cursors.No;
										return true;
									} else if (!endRegister.Register.GraphVorlaufRight && startRegister.GetInputLink() == null && endRegister.GetOutputLink() == null) {
										this.newConnectionStart = startRegister.GetInputConnection(startOffsetX, startOffsetY, this.product, this.product.GetCircuitForRegister(startRegister.Register));
										this.newConnectionEnd = endRegister.GetOutputConnection(endOffsetX, endOffsetY, product, product.GetCircuitForRegister(endRegister.Register));
										this.newConnectionDraw.Vertices = this.GetNextAutomaticConnectionVertices(this.newConnectionEnd);
										this.customCursor = this.newConnectionDraw.CheckValidity(null, 0, 0) ? null : Cursors.No;
										return true;
									}
								}
							}
#endif
						}
						this.customCursor = Cursors.No;
						this.newConnectionStart = null;
						this.newConnectionEnd = null;
						if (this.newConnectionDraw.Vertices != null && this.newConnectionDraw.Vertices.Count > 0) {
							this.newConnectionDraw.Vertices.Clear();
							return true;
						}
					}
				}
			}

			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermCompactPlannerMode.HPM_NONE) {
				return false;
			}

			this.dragStart = planPoint;

			if (this.mode == HithermCompactPlannerMode.HPM_ADD_REGISTER && this.product != null && this.product.AssociatedRoom != null) {
				this.newRegisterWall = this.product.AssociatedRoom.GetWallForPoint(planPoint, out this.newRegisterWallXOffset, out this.newRegisterWallYOffset);
				if (this.newRegisterWall != null) {
					if (this.connectedWallPanel != null) {
						this.connectedWallPanel.SelectedObject = null;
						this.connectedWallPanel.SelectedWall = this.newRegisterWall;
					}
					this.newRegister = new GraphicalHithermCompactRegisterWrapper(this.product);
					this.newRegister.IsNew = true;
				}
			}

			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermCompactPlannerMode.HPM_NONE) {
				return false;
			}

			this.dragEnd = planPoint;
			if (this.mode == HithermCompactPlannerMode.HPM_ADD_REGISTER && this.dragStart.HasValue && this.newRegister != null) {
				bool parapet = this.newRegisterParapet;
				double width =  Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				double height = Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				Nullable<HithermCompactRegister.HithermCompactRegisterTypeEnum> registerType = HithermCompactRegister.GetRegisterTypeForSize(width, height, this.newRegisterParapet, this.newRegisterWall.IsDachSchraege);
				if (registerType.HasValue) {
					if (this.newRegister.Register == null) {
						this.newRegister.Register = new HithermCompactRegister();
					}
					this.newRegister.Register.RegisterType = registerType.Value;
					this.newRegister.Register.RegisterCount = this.newRegisterParapet ? 1 : (int)Math.Floor(width / HithermCompactRegister.GetRegisterBreite(registerType.Value));
					double x = this.dragStart.Value.X < this.dragEnd.X ? this.dragStart.Value.X : this.dragStart.Value.X - this.newRegister.Width;
					double y = this.dragStart.Value.Y < this.dragEnd.Y ? this.dragStart.Value.Y : this.dragStart.Value.Y - this.newRegister.Height;
					this.newRegister.Register.GraphPosX = x - this.newRegisterWallXOffset;
					this.newRegister.Register.GraphPosY = y - this.newRegisterWallYOffset;
					this.newRegister.Register.GraphWallId = this.newRegisterWall.Id;
					if (this.newRegister.Register != null) {
						if (this.SnapEnabled) {
							this.newRegister.SnapToHelplines(this.newRegisterWall.AllHelpLines, this.dragStart.Value.Y > this.dragEnd.Y, this.dragStart.Value.Y <= this.dragEnd.Y);
						}
						this.newRegisterOk = this.newRegister.CheckValidity(this.newRegisterWall, this.newRegisterWallXOffset, this.newRegisterWallYOffset);
					} else {
						this.newRegisterOk = false;
					}
					if (this.newRegisterOk) {
						this.newRegisterWall.AssiociatedRoom.MarkErrors(this.newRegister, this.newRegisterWall);
					} else {
						this.newRegisterWall.AssiociatedRoom.ClearErrors();
					}
				} else {
					this.newRegister.Register = null;
					this.newRegisterWall.AssiociatedRoom.ClearErrors();
				}
				return true;
			}
			return false;
		}

		public bool SnapEnabled {
			get { return this.connectedWallPanel != null ? this.connectedWallPanel.SnapEnabled : false; }
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermCompactPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermCompactPlannerMode.HPM_ADD_REGISTER && this.dragStart.HasValue && this.newRegister != null) {
				bool recalc = false;
				if (this.newRegister.Register != null && this.newRegisterOk) {
					this.newRegister.Register.Wall = this.newRegisterWall.Wall;
					this.newRegister.Register.PlannedProduct = Project.Instance.GetPlannedProduct(this.product);
					int hkId = this.product.GetNewHkId();
					this.product.AddRegisterToCircuit(this.newRegister.Register, hkId);

					this.newRegisterWall.Registers.Add(this.newRegister);
					if (this.ConnectedWallPanel != null) {
						this.ConnectedWallPanel.SelectedObject = this.newRegister;
					}
					recalc = true;
				}
				this.newRegister = null;
				this.newRegisterWall = null;
				if (recalc) {
					this.OnRecalculationNecessary();
				}
			}

			this.dragStart = null;

			return true;
		}

		public bool PlannerKeyPress(System.Windows.Forms.Keys key) {
			if (this.mode == HithermCompactPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermCompactPlannerMode.HPM_ADD_CONNECTION) {
				if (key == System.Windows.Forms.Keys.Escape) {
					this.newConnectionStart = null;
					this.newConnection.Vertices.Clear();
					this.newConnectionEnd = null;
					this.newConnectionDraw.Vertices.Clear();
					this.newConnectionAutoStart = null;
					if (this.connectedWallPanel != null) {
						this.connectedWallPanel.SelectedObject = null;
						this.connectedWallPanel.InvalidateGraphics();
					}
				} else if (key == Keys.Back) {
					if (this.newConnection.Vertices != null && this.newConnection.Vertices.Count > 1) {
						this.newConnection.Vertices.RemoveAt(this.newConnection.Vertices.Count - 1);
						if (this.connectedWallPanel != null) {
							this.connectedWallPanel.SelectedObject = null;
							this.connectedWallPanel.InvalidateGraphics();
						}
					} else {
						this.newConnectionStart = null;
						this.newConnection.Vertices.Clear();
						this.newConnectionEnd = null;
						this.newConnectionDraw.Vertices.Clear();
						this.newConnectionAutoStart = null;
						if (this.connectedWallPanel != null) {
							this.connectedWallPanel.SelectedObject = null;
							this.connectedWallPanel.InvalidateGraphics();
						}
					}
				}
			}
			return false;
		}
		#endregion

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HithermCompactProduct HithermCompactProduct {
			get { return this.product; }
			set {
				this.product = value;
				foreach (GraphicalWall baseWall in this.product.AssociatedRoom.Walls) {
					GraphicalWall wall = baseWall;
					while (wall != null) {
						wall.Registers.Clear();
						wall = wall.DachSchraege;
					}
				}
				foreach (HithermCompactCircuit c in product.PlannedCircuits) {
					foreach (HithermCompactRegister r in c.Registers) {
						GraphicalWall wall = this.product.AssociatedRoom.GetWallForId(r.GraphWallId);
						if (wall != null) {
							wall.Registers.Add(new GraphicalHithermCompactRegisterWrapper(r, this.product));
						}
					}
				}
				if (this.ConnectedWallPanel != null) {
					if (this.product == null || this.product.AssociatedRoom == null) {
						this.ConnectedWallPanel.Room = null;
					} else {
						this.ConnectedWallPanel.Room = this.product.AssociatedRoom;
						Room room = this.product.AssociatedRoom;
					}
				}
				if (this.product == null) {
					this.newConnectionDraw = new GraphicalHithermCompactVerbindung(null, null, new List<Point2D>(), null, null);
					this.newConnectionDraw.Finished = false;
				} else {
					this.newConnectionDraw = new GraphicalHithermCompactVerbindung(null, null, new List<Point2D>(), null, Project.Instance.GetPlannedProduct(this.product));
					this.newConnectionDraw.Finished = false;
				}

			}
		}

		public Product Product {
			get { return this.HithermCompactProduct; }
		}

		/*/// <summary>
		/// Returns x-offset in m
		/// </summary>
		/// <param name="wall"></param>
		/// <returns></returns>
		private Nullable<double> GetWallXOffset(GraphicalWall wall) {
			if (this.product == null || this.product.AssociatedRoom == null) {
				return null;
			}
			double xOffset = 0;
			foreach (GraphicalWall w in this.product.AssociatedRoom.Walls) {
				if (w.GetWallYOffset(wall, 0).HasValue) { // quick hack to determine if the searched wall is a dachschräge of w
					return xOffset;
				}
				xOffset += w.GetWallWidth();
			}
			return null;
		}

		private Nullable<double> GetWallYOffset(GraphicalWall wall) {
			if (this.product == null || this.product.AssociatedRoom == null) {
				return null;
			}
			Nullable<double> yOffset = null;
			foreach (GraphicalWall w in this.product.AssociatedRoom.Walls) {
				yOffset = w.GetWallYOffset(wall, 0);
				if (yOffset.HasValue) {
					return yOffset;
				}
			}
			return null;
		}*/

		public HithermCompactPlannerMode Mode {
			get { return this.mode; }
			set {
				if (value != this.mode) {
					this.mode = value;
					this.customCursor = null;
					this.newConnectionAutoStart = null;
					this.newConnectionStart = null;
					this.newConnection.Vertices.Clear();
					this.newConnectionDraw.Vertices.Clear();
					if (this.connectedWallPanel != null) {
						this.connectedWallPanel.InvalidateGraphics();
					}
				}
			}
		}

		private Point2D GetNextConnectionVertex(Point2D mousePoint, double rotation, out bool horizontal) {
			if (this.newConnection == null || this.newConnection.Vertices == null || this.newConnection.Vertices.Count == 0) {
				horizontal = true;
				return mousePoint;
			}

			Point2D lastVertex = this.newConnection.Vertices[this.newConnection.Vertices.Count - 1];
			Matrix3D transformation = Matrix3D.Identity;
			transformation = transformation * Transformation3D.Rotate(-rotation * Math.PI / 180.0);
			transformation = transformation * Transformation3D.Translation(-lastVertex.X, -lastVertex.Y);

			Point2D transformedMousePoint = transformation.Transform(mousePoint);
			if (Math.Abs(transformedMousePoint.X) < Math.Abs(transformedMousePoint.Y)) {
				transformedMousePoint.X = 0;
				horizontal = false;
			} else {
				transformedMousePoint.Y = 0;
				horizontal = true;
			}

			return transformation.GetInverse().Transform(transformedMousePoint);
		}

		private List<Point2D> GetNextAutomaticConnectionVertices(PossibleConnection endConnection) {
			List<Point2D> nextVertices = new List<Point2D>();
			if (this.newConnection != null && this.newConnectionStart != null) {
				if (this.newConnectionsAlign) {
					if (this.newConnection.Vertices.Count == 0) {
						nextVertices.Add(this.newConnectionStart.ConnectionPoint);
					}
					List<Point2D> lastVertices = new List<Point2D>();
					Point2D startPoint = this.newConnection.Vertices.Count == 0 ? nextVertices[0] : this.newConnection.Vertices[this.newConnection.Vertices.Count - 1];
					Vector2D startVector = new Vector2D(0, 0);
					if (this.newConnection.Vertices.Count < 2 && newConnectionStart.PreferredStartVector.HasValue) {
						startVector = newConnectionStart.PreferredStartVector.Value;
						startPoint = startPoint + startVector;
						nextVertices.Add(startPoint);
					} else {
						startVector = this.newConnection.Vertices[this.newConnection.Vertices.Count - 1] - this.newConnection.Vertices[this.newConnection.Vertices.Count - 2];
					}
					Point2D endPoint = endConnection.ConnectionPoint;
					lastVertices.Add(endPoint);
					Vector2D endVector = new Vector2D(0, 0);
					if (endConnection.PreferredStartVector.HasValue) {
						endVector = endConnection.PreferredStartVector.Value;
						endPoint = endPoint + endVector;
						lastVertices.Add(endPoint);
					}
					Vector2D todo = endPoint - startPoint;
					if ((startVector.Y < 0 && todo.Y > 0) || (startVector.Y > 0 && todo.Y < 0)) {
						startVector = new Vector2D(todo.X >= 0 ? 10 : -10, 0);
						startPoint = startPoint + startVector;
						nextVertices.Add(startPoint);
						todo = todo - startVector;
					}
					if ((startVector.X < 0 && todo.X > 0) || (startVector.X > 0 && todo.X < 0)) {
						startVector = new Vector2D(0, todo.Y >= 0 ? 10 : -10);
						startPoint = startPoint + startVector;
						nextVertices.Add(startPoint);
						todo = todo - startVector;
					}
					if ((endVector.X < 0 && todo.X < 0) || (endVector.X > 0 && todo.X > 0)) {
						endVector = new Vector2D(0, todo.Y >= 0 ? 10 : -10);
						endPoint = endPoint - endVector;
						lastVertices.Add(endPoint);
						todo = todo - endVector;
					}
					//if (Math.Abs(startVector.Y) <= 0.0001 && Math.Abs(endVector.X) <= 0.0001 &&
					//(Math.Abs(startVector.X) > 0.0001 || Math.Abs(endVector.Y) > 0.0001)) {
					if (Math.Abs(startVector.Y) <= 0.0001) {
						nextVertices.Add(new Point2D(endPoint.X, startPoint.Y));
						//nextVertices.Add(new Point2D(lastVertices[0].X, nextVertices.Count > 0 ? nextVertices[nextVertices.Count - 1].Y : this.newConnection.Vertices[this.newConnection.Vertices.Count - 1].Y));
					} else {
						nextVertices.Add(new Point2D(startPoint.X, endPoint.Y));
						//nextVertices.Add(new Point2D(nextVertices.Count > 0 ? nextVertices[nextVertices.Count - 1].X : this.newConnection.Vertices[this.newConnection.Vertices.Count - 1].X, lastVertices[0].Y));
					}
					lastVertices.Reverse();
					nextVertices.AddRange(lastVertices);
				} else {
					if (this.newConnection.Vertices.Count == 0) {
						nextVertices.Add(this.newConnectionStart.ConnectionPoint);
					}
					if (this.newConnection.Vertices.Count + nextVertices.Count == 1) {
						nextVertices.Add((this.newConnection.Vertices.Count == 1 ? this.newConnection.Vertices[0] : nextVertices[0]) + this.newConnectionStart.PreferredStartVector.Value);
					}
					nextVertices.Add(endConnection.ConnectionPoint + endConnection.PreferredStartVector.Value);
					nextVertices.Add(endConnection.ConnectionPoint);
				}
			}
			return nextVertices;
		}

		private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out PossibleConnection endConnection) {
			List<Point2D> nextConnectionPoints = new List<Point2D>();
			//endRegister = null;
			endConnection = this.GetHoveredRegisterConnection(mousePoint, this.newConnectionStart.PossibleOutput, this.newConnectionStart.PossibleInput);
			
			// TODO check if end connection is valid
			if (endConnection != null) {
				if (endConnection is PossibleHithermCompactRegisterConnection) {
					HithermCompactCircuit circuit = this.product.GetCircuitForRegister((endConnection as PossibleHithermCompactRegisterConnection).Register);
					if (circuit == this.newConnectionStart.Circuit || !circuit.IsConnectionAvailable((endConnection as PossibleHithermCompactRegisterConnection).Register, this.newConnectionStart.PossibleOutput)) {
						endConnection = null;
					}
				}
			}

			/*if (this.product.GetCircuitForModul(this.newConnectionStart, out index).GetAllLinkedModules(this.newConnectionStart).Contains(endModule)) {
				endModule = null;
			}*/

			if (endConnection == null) {
				bool horizontal;
				if (this.newConnectionsAlign) {
					nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, this.newConnectionStart.Rotation, out horizontal));
				} else {
					if (this.newConnection.Vertices.Count == 1) {
						nextConnectionPoints.Add(this.newConnection.Vertices[0] + this.newConnectionStart.PreferredStartVector.Value);
					}
					nextConnectionPoints.Add(mousePoint);
				}
			} else {
				if (this.newConnectionsAlign) {
					nextConnectionPoints = this.GetNextAutomaticConnectionVertices(endConnection);
				} else {
					if (this.newConnection.Vertices.Count == 1) {
						nextConnectionPoints.Add(this.newConnection.Vertices[0] + this.newConnectionStart.PreferredStartVector.Value);
					}
					if (endConnection.PreferredStartVector.HasValue) {
						nextConnectionPoints.Add(endConnection.ConnectionPoint + endConnection.PreferredStartVector.Value);
					}
					nextConnectionPoints.Add(endConnection.ConnectionPoint);
				}
			}
			this.endConnection = endConnection;
			return nextConnectionPoints;
		}

		private PossibleConnection GetHoveredRegisterConnection(Point2D planPoint, bool checkInput, bool checkOutput) {
			foreach (PossibleConnection conn in this.highlightedConnections) {
				if (conn.ConnectionArea.IsInside(planPoint) && ((conn.PossibleInput && checkInput) || (conn.PossibleOutput && checkOutput))) {
					return conn;
				}
			}
			return null;
		}

		public IGraphicalWallObject PickObject(Point2D mousePosInPlan) {
			if (this.product == null) {
				return null;
			}
			foreach (HithermCompactCircuit hc in this.product.PlannedCircuits) {
				foreach (GraphicalHithermCompactVerbindung link in hc.Links) {
					if (link.HitTest(mousePosInPlan, 2)) {
						return link;
					}
				}
			}
			return null;
		}
	}
}
