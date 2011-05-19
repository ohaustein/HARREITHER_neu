using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using WW.Math;
using System.Drawing;
using System.Drawing.Drawing2D;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class HithermPlanner : Component, IWallProductPlanner {

		public enum HithermPlannerMode {
			HPM_NONE,
			HPM_ADD_REGISTER,
			HPM_ADD_CONNECTION
		}

		private GraphicalWallPanel connectedWallPanel;
		private HithermPlannerMode mode = HithermPlannerMode.HPM_NONE;

		private Nullable<Point2D> dragStart = null;
		private Point2D dragEnd = Point2D.Zero;

		private GraphicalHithermRegisterWrapper newRegister = null;
		private GraphicalWall newRegisterWall = null;
		private double newRegisterWallXOffset = 0;
		private double newRegisterWallYOffset = 0;
		private bool newRegisterOk = true;

		private HithermRegister.RohrabstandEnum newRegisterRohrabstand = HithermRegister.RohrabstandEnum.RC_HOCHLEISTUNG;
		private bool newRegisterVorlaufRight = true;
		private bool newRegisterUseHelpline = true;
		private bool newRegisterOnlyWhole = false;
		private HithermRegister.RegisterOrientationEnum newRegisterOrientation = HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL;

		private PossibleHithermRegisterConnection newConnectionStart = null;
		private PossibleConnection endConnection = null;
		private PossibleConnection startConnection = null;
		private List<PossibleConnection> highlightedConnections = new List<PossibleConnection>();
		private HithermRegisterVerbindung newConnection = new HithermRegisterVerbindung();
		private HithermRegisterVerbindung newConnectionDraw = new HithermRegisterVerbindung(false);

		public HithermRegister.RohrabstandEnum NewRegisterRohrabstand {
			get { return newRegisterRohrabstand; }
			set { newRegisterRohrabstand = value; }
		}

		public bool NewRegisterVorlaufRight {
			get { return newRegisterVorlaufRight; }
			set { newRegisterVorlaufRight = value; }
		}

		public bool NewRegisterUseHelpline {
			get { return newRegisterUseHelpline; }
			set { newRegisterUseHelpline = value; }
		}

		public bool NewRegisterOnlyWhole {
			get { return newRegisterOnlyWhole; }
			set { newRegisterOnlyWhole = value; }
		}

		public HithermRegister.RegisterOrientationEnum NewRegisterOrientation {
			get { return newRegisterOrientation; }
			set { newRegisterOrientation = value; }
		}

		private HithermProduct product = null;

		private event EventHandler<EventArgs> recalculationNecessary;
		public event EventHandler<EventArgs> RecalculationNecessary {
			add { this.recalculationNecessary += value; }
			remove { this.recalculationNecessary -= value; }
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

		public System.Windows.Forms.Cursor CustomCursor {
			get { return null; }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl, double scale) {
			this.PaintAfterPlanPannel(e.Graphics, mousePositionInPlan, mousePositionInControl, scale);
		}

		public void PaintAfterPlanPannel(System.Drawing.Graphics g, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl, double scale) {
			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER && dragStart != null) {
				float x = (float)(this.dragStart.Value.X < mousePositionInPlan.X ? this.dragStart.Value.X : this.dragEnd.X);
				float width = (float)Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				float y = (float)(this.dragStart.Value.Y < mousePositionInPlan.Y ? this.dragStart.Value.Y : this.dragEnd.Y);
				float height = (float)Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				Brush brush = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Green, Color.Transparent);
				Pen pen = new Pen(brush, (float)(1.0 / scale));
				g.DrawRectangle(pen, x, y, width, height);
				if (this.newRegister != null) {
					this.newRegister.PaintObject(g, newRegisterWallXOffset, newRegisterWallYOffset, Color.Green, scale, true, !this.newRegisterOk/*, false*/);
				}
			}
			foreach (HithermCircuit c in this.product.PlannedCircuits) {
				foreach (HithermRegisterVerbindung link in c.Links) {
					link.PaintObject(g, 0, 0, this.connectedWallPanel.SelectedObject, scale);
				}
			}
			if (this.mode == HithermPlannerMode.HPM_ADD_CONNECTION) {
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
					PossibleConnection endConn;
					this.newConnectionDraw.Vertices = new List<Point2D>(this.newConnection.Vertices);
					//List<Point2D> nextVertices = this.GetNextConnectionVerticesInclConnectionPoints(mousePositionInPlan, out endConn);
					this.newConnectionDraw.Vertices.AddRange(this.GetNextConnectionVerticesInclConnectionPoints(mousePositionInPlan, out endConn));
					this.newConnectionDraw.PaintObject(g, Color.Green, !this.newConnectionDraw.CheckValidity(null, 0, 0), scale);
				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermPlannerMode.HPM_ADD_CONNECTION) {
				if (this.newConnectionStart == null) {
					this.newConnection.Vertices.Clear();
					foreach (GraphicalWall baseWall in this.product.AssociatedRoom.Walls) {
						GraphicalWall wall = baseWall;
						while (wall != null) {
							Nullable<Vector2D> offset = this.product.AssociatedRoom.GetWallOffset(wall) * 100;
							if (offset.HasValue) {
								foreach (PossibleConnection conn in this.highlightedConnections) {
									if (conn is PossibleHithermRegisterConnection && conn.ConnectionArea.IsInside(planPoint)) {
										this.newConnectionStart = conn as PossibleHithermRegisterConnection;
										this.newConnection.Vertices.Add(this.newConnectionStart.ConnectionPoint);
										return true;
									}
								}
								/*foreach (GraphicalRegisterWrapper wrapper in wall.Registers) {
									if (wrapper == this.connectedWallPanel.SelectedObject || wrapper is GraphicalHithermRegisterWrapper && wrapper.HitTest(planPoint, offset.Value.X, offset.Value.Y)) {
										PossibleHithermRegisterConnection conn = (wrapper as GraphicalHithermRegisterWrapper).GetOutputConnection(offset.Value.X, offset.Value.Y, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register));
										if (conn.ConnectionArea.IsInside(planPoint)) {
											this.startConnection = conn;
											this.newConnection.Vertices.Add(this.startConnection.ConnectionPoint);
											return true;
										}
										conn = (wrapper as GraphicalHithermRegisterWrapper).GetInputConnection(offset.Value.X, offset.Value.Y, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register));
										if (conn.ConnectionArea.IsInside(planPoint)) {
											this.startConnection = conn;
											this.newConnection.Vertices.Add(this.startConnection.ConnectionPoint);
											return true;
										}
									}
								}*/
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
							HithermRegister endRegister = null;
							HithermCircuit endCircuit = null;
							HithermRegister startRegister = null;
							HithermCircuit startCircuit = null;
							if (endConnection is PossibleHithermRegisterConnection) {
								endRegister = (endConnection as PossibleHithermRegisterConnection).Register;
								endCircuit = this.product.GetCircuitForRegister(endRegister);
							}
							if (startConnection is PossibleHithermRegisterConnection) {
								startRegister = (startConnection as PossibleHithermRegisterConnection).Register;
								startCircuit = this.product.GetCircuitForRegister(startRegister);
							}
							
							if (startRegister != null && endRegister != null) {
								HithermCircuit combinedCircuit = (startCircuit.HkLabelNr < endCircuit.HkLabelNr ? startCircuit : endCircuit);
								HithermCircuit deleteCircuit = combinedCircuit == startCircuit ? endCircuit : startCircuit;
								int combinedCircuitNr = combinedCircuit.HkLabelNr;
								while (deleteCircuit.Registers.Count > 0) {
									this.product.MoveRegisterToCircuit(deleteCircuit.Registers[0], combinedCircuitNr);
								}
								HithermRegisterVerbindung newLink;
								newLink = new HithermRegisterVerbindung(startRegister, endRegister, this.newConnection.Vertices, combinedCircuit, Project.Instance.GetPlannedProduct(this.product));
								combinedCircuit.Links.Add(newLink);
								this.product.CorrectCircuitIds();
							} else {
								HithermCircuit circuitToAdd = (startCircuit == null ? endCircuit : startCircuit);
								HithermRegisterVerbindung newLink;
								newLink = new HithermRegisterVerbindung(startRegister, endRegister, this.newConnection.Vertices, this.product.GetCircuitForRegister(this.newConnectionStart.Register), Project.Instance.GetPlannedProduct(this.product));
								circuitToAdd.Links.Add(newLink);
							}
							this.newConnectionStart = null;
							this.endConnection = null;
							this.newConnection.Vertices.Clear();
						}
					}
					return true;
				}
			}

			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermPlannerMode.HPM_ADD_CONNECTION) {
				foreach (PossibleConnection conn in this.highlightedConnections) {
					if (conn is PossibleHithermRegisterConnection &&  conn.ConnectionArea.IsInside(planPoint)) {
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
									if (wrapper != this.connectedWallPanel.SelectedObject && wrapper is GraphicalHithermRegisterWrapper && wrapper.HitTest(planPoint, offset.Value.X, offset.Value.Y)) {
										HithermRegister register = (wrapper as GraphicalHithermRegisterWrapper).Register;
										HithermCircuit circuit = this.product.GetCircuitForRegister(register);
										if (circuit.IsConnectionAvailable(register, true) && (this.newConnectionStart == null || circuit != this.newConnectionStart.Circuit)) {
											highlightedConnections.Add((wrapper as GraphicalHithermRegisterWrapper).GetInputConnection(offset.Value.X, offset.Value.Y, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register)));
										}
										if (circuit.IsConnectionAvailable(register, false) && (this.newConnectionStart == null || circuit != this.newConnectionStart.Circuit)) {
											highlightedConnections.Add((wrapper as GraphicalHithermRegisterWrapper).GetOutputConnection(offset.Value.X, offset.Value.Y, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register)));
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
				return true;
			}

			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			this.dragStart = planPoint;

			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER && this.product != null && this.product.AssociatedRoom != null) {
				this.newRegisterWall = this.product.AssociatedRoom.GetWallForPoint(planPoint, out this.newRegisterWallXOffset, out this.newRegisterWallYOffset);
				if (this.newRegisterWall != null) {
					if (this.connectedWallPanel != null) {
						this.connectedWallPanel.SelectedObject = null;
						this.connectedWallPanel.SelectedWall = this.newRegisterWall;
					}
					this.newRegister = new GraphicalHithermRegisterWrapper(this.product);
				}
			}

			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			this.dragEnd = planPoint;

			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER && this.dragStart.HasValue && this.newRegister != null) {
				bool vertical = this.newRegisterOrientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_VERTIKAL;
				double width =  Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				double height = Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				Nullable<HithermRegister.HithermRegisterTypeEnum> registerType = HithermRegister.GetRegisterTypeForHoehe(vertical ? (int)Math.Floor(height) : (int)Math.Floor(width), this.newRegisterRohrabstand == HithermRegister.RohrabstandEnum.RC_HOCHLEISTUNG, true);
				if (registerType.HasValue) {
					if (this.newRegister.Register == null) {
						this.newRegister.Register = new HithermRegister();
					}
					this.newRegister.Register.Orientation = this.newRegisterOrientation;
					this.newRegister.Register.RegisterType = registerType.Value;
					this.newRegister.Register.RegisterBreiteForDrawing = vertical ? width : height;
					this.newRegister.Register.GraphVorlaufRight = this.newRegisterVorlaufRight;
					if (this.newRegister.Register.RegisterBreiteForDrawing > (vertical ? width : height)) {
						this.newRegister.Register = null;
					} else {
						double x = this.dragStart.Value.X < this.dragEnd.X ? this.dragStart.Value.X : this.dragStart.Value.X - (vertical ? this.newRegister.Register.RegisterBreiteForDrawing : this.newRegister.Register.RegisterHoehe);
						double y = this.dragStart.Value.Y < this.dragEnd.Y ? this.dragStart.Value.Y : this.dragStart.Value.Y - (vertical ? this.newRegister.Register.RegisterHoehe : this.newRegister.Register.RegisterBreiteForDrawing);
						this.newRegister.Register.GraphPosX = x - this.newRegisterWallXOffset;
						this.newRegister.Register.GraphPosY = y - this.newRegisterWallYOffset;
						this.newRegister.Register.GraphWallId = this.newRegisterWall.Id;
					}
					if (this.newRegister.Register != null) {
						this.newRegisterOk = this.newRegister.CheckPositionAndSize(this.newRegisterWall, this.newRegisterWallXOffset, this.newRegisterWallYOffset);
					}
				} else {
					this.newRegister.Register = null;
				}
				return true;
			}
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}
			 
			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER && this.dragStart.HasValue && this.newRegister != null) {
				bool recalc = false;
				if (this.newRegister.Register != null && this.newRegisterOk) {
					//HithermCircuit c = new HithermCircuit();
					//this.product.PlannedCircuits.Add(c);

					this.newRegister.Register.Wall = this.newRegisterWall.Wall;
					this.newRegister.Register.PlannedProduct = Project.Instance.GetPlannedProduct(this.product);
					int hkId = this.product.GetNewHkId();
					this.product.AddRegisterToCircuit(this.newRegister.Register, hkId);
					//this.newRegister.Register.Heizkreis = hkId;

					this.newRegisterWall.Registers.Add(this.newRegister);
					//this.newRegister.Register.Heizkreis = this.product.PlannedCircuits.Count - 1;
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
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermPlannerMode.HPM_ADD_CONNECTION) {
				if (key == System.Windows.Forms.Keys.Escape) {
					this.newConnectionStart = null;
					this.newConnection.Vertices.Clear();
					this.newRegisterOk = true;
					this.newRegister = null;
					this.connectedWallPanel.InvalidateGraphics();
				}
			}
			return false;
		}
		#endregion

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HithermProduct Product {
			get { return this.product; }
			set {
				this.product = value;
				foreach (GraphicalWall wall in this.product.AssociatedRoom.Walls) {
					wall.Registers.Clear();
				}
				foreach (HithermCircuit c in product.PlannedCircuits) {
					foreach (HithermRegister r in c.Registers) {
						GraphicalWall wall = this.product.AssociatedRoom.GetWallForId(r.GraphWallId);
						if (wall != null) {
							wall.Registers.Add(new GraphicalHithermRegisterWrapper(r, this.product));
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
					this.newConnectionDraw = new HithermRegisterVerbindung(null, null, new List<Point2D>(), null, null);
					this.newConnectionDraw.Finished = false;
				} else {
					this.newConnectionDraw = new HithermRegisterVerbindung(null, null, new List<Point2D>(), null, Project.Instance.GetPlannedProduct(this.product));
					this.newConnectionDraw.Finished = false;
				}

			}
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

		public HithermPlannerMode Mode {
			get { return this.mode; }
			set { this.mode = value; }
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

		private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out PossibleConnection endConnection) {
			List<Point2D> nextConnectionPoints = new List<Point2D>();
			//endRegister = null;
			endConnection = this.GetHoveredRegisterConnection(mousePoint, this.newConnectionStart.PossibleOutput, this.newConnectionStart.PossibleInput);
			
			// TODO check if end connection is valid
			if (endConnection != null) {
				if (endConnection is PossibleHithermRegisterConnection) {
					HithermCircuit circuit = this.product.GetCircuitForRegister((endConnection as PossibleHithermRegisterConnection).Register);
					if (circuit == this.newConnectionStart.Circuit || !circuit.IsConnectionAvailable((endConnection as PossibleHithermRegisterConnection).Register, this.newConnectionStart.PossibleOutput)) {
						endConnection = null;
					}
				}
			}

			/*if (this.product.GetCircuitForModul(this.newConnectionStart, out index).GetAllLinkedModules(this.newConnectionStart).Contains(endModule)) {
				endModule = null;
			}*/

			if (endConnection == null) {
				bool horizontal;
				nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, this.newConnectionStart.Rotation, out horizontal));
			} else {
				Point2D connectionPoint = endConnection.ConnectionPoint;
				if (this.newConnection.Vertices.Count > 1) {
					Point2D p1 = this.newConnection.Vertices[this.newConnection.Vertices.Count - 2];
					Point2D p2 = this.newConnection.Vertices[this.newConnection.Vertices.Count - 1];
					Vector2D v = p1 - p2;
					if (!endConnection.ConnectHorizontal && endConnection.ConnectVertical) {
						v = new Vector2D(1, 0);
					} else if (endConnection.ConnectHorizontal && !endConnection.ConnectVertical) {
						v = new Vector2D(0, 1);
					}
					Line2D line1 = new Line2D(p2, v);
					Line2D line2 = new Line2D(connectionPoint, new Vector2D(line1.Direction.Y, -line1.Direction.X));
					Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
					if (intersection.HasValue) {
						nextConnectionPoints.Add(intersection.Value);
					}
				} else if (this.newConnection.Vertices.Count == 1) {
					nextConnectionPoints.Add(new Point2D(connectionPoint.X, this.newConnection.Vertices[0].Y));
				}
				nextConnectionPoints.Add(connectionPoint);
				//endRegister = (this.endConnection is PossibleHithermRegisterConnection) ? (this.endConnection as PossibleHithermRegisterConnection).Register : null;
			}
			this.endConnection = endConnection;
			return nextConnectionPoints;
		}

		private PossibleConnection GetHoveredRegisterConnection(Point2D planPoint, bool checkInput, bool checkOutput) {
		//private PossibleHithermRegisterConnection GetHoveredRegisterConnection(Point2D planPoint, bool checkInput, bool checkOutput) {
			/*foreach (GraphicalWall baseWall in this.product.AssociatedRoom.Walls) {
				GraphicalWall wall = baseWall;
				while (wall != null) {
					Nullable<Vector2D> offset = this.product.AssociatedRoom.GetWallOffset(wall);
					if (offset.HasValue) {
						offset = offset * 100;
						foreach (GraphicalRegisterWrapper wrapper in wall.Registers) {
							if (wrapper == this.connectedWallPanel.SelectedObject || wrapper is GraphicalHithermRegisterWrapper && wrapper.HitTest(planPoint, offset.Value.X, offset.Value.Y)) {
								PossibleHithermRegisterConnection conn;
								if (checkOutput) {
									conn = (wrapper as GraphicalHithermRegisterWrapper).GetOutputConnection(offset.Value.X, offset.Value.Y, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register));
									if (conn.ConnectionArea.IsInside(planPoint)) {
										return conn;
									}
								}
								if (checkInput) {
									conn = (wrapper as GraphicalHithermRegisterWrapper).GetInputConnection(offset.Value.X, offset.Value.Y, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register));
									if (conn.ConnectionArea.IsInside(planPoint)) {
										return conn;
									}
								}
							}
						}
						wall = wall.DachSchraege;
					}
				}
			}
			return null;*/
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
			foreach (HithermCircuit hc in this.product.PlannedCircuits) {
				foreach (HithermRegisterVerbindung link in hc.Links) {
					if (link.HitTest(mousePosInPlan, 2)) {
						return link;
					}
				}
			}
			return null;
		}

	}
}
