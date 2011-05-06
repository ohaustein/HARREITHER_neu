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

		private PossibleHithermRegisterConnection startConnection = null;
		private PossibleHithermRegisterConnection endConnection = null;
		private List<PossibleConnection> highlightedConnections = new List<PossibleConnection>();
		private List<Point2D> newConnectionVertices = new List<Point2D>();

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
					this.newRegister.PaintObject(g, newRegisterWallXOffset, newRegisterWallYOffset, Color.Green, scale, true, !this.newRegisterOk);
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
					foreach (PossibleConnection conn in this.highlightedConnections) {
						if (conn.PossibleInput) { // TODO handle connections that are input and output
							PointF[] polygon = new PointF[conn.ConnectionArea.Count];
							int i = 0;
							foreach (Point2D p in conn.ConnectionArea) {
								polygon[i] = new PointF((float)p.X, (float)p.Y);
								i++;
							}
							g.FillPolygon(bInput, polygon);
							g.DrawPolygon(pInput, polygon);
						} else if (conn.PossibleOutput) {
							PointF[] polygon = new PointF[conn.ConnectionArea.Count];
							int i = 0;
							foreach (Point2D p in conn.ConnectionArea) {
								polygon[i] = new PointF((float)p.X, (float)p.Y);
								i++;
							}
							g.FillPolygon(bOutput, polygon);
							g.DrawPolygon(pOutput, polygon);
						}
					}
				}
				if (this.startConnection != null) {
					HithermRegister endRegister;
					List<Point2D> nextVertices = this.GetNextConnectionVerticesInclConnectionPoints(mousePositionInPlan, out endRegister);
					bool first = true;
					PointF oldPoint = new PointF();
					PointF newPoint;
					Pen newConnectionPen = new Pen(Color.Green, 2);
					foreach (Point2D point in this.newConnectionVertices) {
						newPoint = new PointF((float)point.X, (float)point.Y);
						if (first) {
							first = false;
						} else {
							g.DrawLine(newConnectionPen, oldPoint, newPoint);
						}
						oldPoint = newPoint;
					}
					foreach (Point2D point in nextVertices) {
						newPoint = new PointF((float)point.X, (float)point.Y);
						if (first) {
							first = false;
						} else {
							g.DrawLine(newConnectionPen, oldPoint, newPoint);
						}
						oldPoint = newPoint;
					}
				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermPlannerMode.HPM_ADD_CONNECTION) {
				if (this.startConnection == null) {
					this.newConnectionVertices.Clear();
					double xOffset = 0;
					foreach (GraphicalWall baseWall in this.product.AssociatedRoom.Walls) {
						GraphicalWall wall = baseWall;
						double yOffset = 0;
						while (wall != null) {
							foreach (GraphicalRegisterWrapper wrapper in wall.Registers) {
								if (wrapper == this.connectedWallPanel.SelectedObject || wrapper is GraphicalHithermRegisterWrapper && wrapper.HitTest(planPoint, xOffset, yOffset)) {
									PossibleHithermRegisterConnection conn = (wrapper as GraphicalHithermRegisterWrapper).GetOutputConnection(xOffset, yOffset, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register));
									if (conn.ConnectionArea.IsInside(planPoint)) {
										this.startConnection = conn;
										this.newConnectionVertices.Add(this.startConnection.ConnectionPoint);
										return true;
									}
									conn = (wrapper as GraphicalHithermRegisterWrapper).GetInputConnection(xOffset, yOffset, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register));
									if (conn.ConnectionArea.IsInside(planPoint)) {
										this.startConnection = conn;
										this.newConnectionVertices.Add(this.startConnection.ConnectionPoint);
										return true;
									}
								}
							}
							yOffset += wall.GetWallHeight() * 100;
							wall = wall.DachSchraege;
						}
						xOffset += baseWall.GetWallWidth() * 100;
					}
				} else {
					HithermRegister endRegister;
					List<Point2D> nextVertices = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endRegister);
					this.newConnectionVertices.AddRange(nextVertices);
					if (endRegister != null) {
						HithermCircuit endCircuit = this.product.GetCircuitForRegister(endRegister);
						HithermCircuit startCircuit = this.product.GetCircuitForRegister(this.startConnection.Register);
						List<HithermRegister> registersToMove = new List<HithermRegister>(endCircuit.Registers);
						foreach (HithermRegister register in registersToMove) {
							//register.Heizkreis = this.startConnection.Register.Heizkreis;
							this.product.MoveRegisterToCircuit(register, this.startConnection.Register.Heizkreis);
						}
						HithermRegisterVerbindung link = new HithermRegisterVerbindung(this.startConnection.Register, endRegister, this.newConnectionVertices, this.product.GetCircuitForRegister(this.startConnection.Register), Project.Instance.GetPlannedProduct(this.product));
						startCircuit.Links.Add(link);
						this.startConnection = null;
						this.endConnection = null;
						this.newConnectionVertices.Clear();
						// TODO commit connection
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
				this.highlightedConnections.Clear();
				//if (this.startConnection == null) {
					double xOffset = 0;
					foreach (GraphicalWall baseWall in this.product.AssociatedRoom.Walls) {
						GraphicalWall wall = baseWall;
						double yOffset = 0;
						while (wall != null) {
							foreach (GraphicalRegisterWrapper wrapper in wall.Registers) {
								if (wrapper != this.connectedWallPanel.SelectedObject && wrapper is GraphicalHithermRegisterWrapper && wrapper.HitTest(planPoint, xOffset, yOffset)) {
									HithermRegister register = (wrapper as GraphicalHithermRegisterWrapper).Register;
									HithermCircuit circuit = this.product.GetCircuitForRegister(register);
									if (circuit.IsConnectionAvailable(register, true) && (this.startConnection == null || circuit != this.startConnection.Circuit)) {
										highlightedConnections.Add((wrapper as GraphicalHithermRegisterWrapper).GetOutputConnection(xOffset, yOffset, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register)));
									}
									if (circuit.IsConnectionAvailable(register, false) && (this.startConnection == null || circuit != this.startConnection.Circuit)) {
										highlightedConnections.Add((wrapper as GraphicalHithermRegisterWrapper).GetInputConnection(xOffset, yOffset, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register)));
									}
								}
							}
							yOffset += wall.GetWallHeight() * 100;
							wall = wall.DachSchraege;
						}
						xOffset += baseWall.GetWallWidth() * 100;
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

			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER) {
				this.newRegisterWall = this.GetWallForPoint(planPoint, out this.newRegisterWallXOffset, out this.newRegisterWallYOffset);
				if (this.newRegisterWall != null) {
					this.newRegister = new GraphicalHithermRegisterWrapper();
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
				double width = Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				double height = Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				Nullable<HithermRegister.HithermRegisterTypeEnum> registerType = HithermRegister.GetRegisterTypeForHoehe((int)Math.Floor(height), this.newRegisterRohrabstand == HithermRegister.RohrabstandEnum.RC_HOCHLEISTUNG);
				if (registerType.HasValue) {
					if (this.newRegister.Register == null) {
						this.newRegister.Register = new HithermRegister();
					}
					this.newRegister.Register.RegisterType = registerType.Value;
					this.newRegister.Register.RegisterBreiteForDrawing = width;
					this.newRegister.Register.GraphVorlaufRight = this.newRegisterVorlaufRight;
					if (this.newRegister.Register.RegisterBreiteForDrawing > width) {
						this.newRegister.Register = null;
					} else {
						double x = this.dragStart.Value.X < this.dragEnd.X ? this.dragStart.Value.X : this.dragStart.Value.X - this.newRegister.Register.RegisterBreiteForDrawing;
						double y = this.dragStart.Value.Y < this.dragEnd.Y ? this.dragStart.Value.Y : this.dragStart.Value.Y - this.newRegister.Register.RegisterHoehe;
						this.newRegister.Register.GraphPosX = x - this.newRegisterWallXOffset;
						this.newRegister.Register.GraphPosY = y - this.newRegisterWallYOffset;
						this.newRegister.Register.GraphWallId = this.newRegisterWall.Id;
					}
					if (this.newRegister.Register != null) {
						Polygon2D registerBorders = this.newRegister.GetObjectBorders(this.newRegisterWallXOffset, this.newRegisterWallYOffset);
						if (this.newRegisterWall.CollisionTest(registerBorders, this.newRegisterWallXOffset, this.newRegisterWallYOffset)) {
							this.newRegisterOk = false;
						} else {
							this.newRegisterOk = true;
							foreach (GraphicalHithermRegisterWrapper register in this.newRegisterWall.Registers) {
								if (register.CollisionTest(registerBorders, this.newRegisterWallXOffset, this.newRegisterWallYOffset)) {
									this.newRegisterOk = false;
									break;
								}
							}
						}
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
					int hkId = this.product.PlannedCircuits.Count + 1;
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
					this.startConnection = null;
					this.newConnectionVertices.Clear();
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
							wall.Registers.Add(new GraphicalHithermRegisterWrapper(r));
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
			}
		}

		private GraphicalWall GetWallForPoint(Point2D planPoint, out double xOffset, out double yOffset) {
			xOffset = 0;
			yOffset = 0;
			if (this.product == null || this.product.AssociatedRoom == null) {
				return null;
			}
			GraphicalWall pickedWall = null;
			foreach (GraphicalWall wall in this.product.AssociatedRoom.Walls) {
				pickedWall = wall.GetPickedWall(planPoint, xOffset, 0);
				if (pickedWall != null) {
					yOffset = wall.GetWallYOffset(pickedWall, 0).Value;
					break;
				}
				xOffset += wall.GetWallWidth() * 100;
			}
			if (pickedWall == null) {
				xOffset = 0;
				yOffset = 0;
			}
			return pickedWall;
		}

		/// <summary>
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
		}

		public HithermPlannerMode Mode {
			get { return this.mode; }
			set { this.mode = value; }
		}

		private Point2D GetNextConnectionVertex(Point2D mousePoint, double rotation, out bool horizontal) {
			if (this.newConnectionVertices == null || this.newConnectionVertices.Count == 0) {
				horizontal = true;
				return mousePoint;
			}

			Point2D lastVertex = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
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

		private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out HithermRegister endRegister) {
			List<Point2D> nextConnectionPoints = new List<Point2D>();
			endRegister = null;
			this.endConnection = this.GetHoveredRegisterConnection(mousePoint, this.startConnection.PossibleOutput, this.startConnection.PossibleInput);
			
			// TODO check if end connection is valid
			if (this.endConnection != null) {
				HithermCircuit circuit = this.product.GetCircuitForRegister(this.endConnection.Register);
				if (circuit == this.startConnection.Circuit || !circuit.IsConnectionAvailable(this.endConnection.Register, this.startConnection.PossibleOutput)) {
					this.endConnection = null;
				}
			}

			/*if (this.product.GetCircuitForModul(this.newConnectionStart, out index).GetAllLinkedModules(this.newConnectionStart).Contains(endModule)) {
				endModule = null;
			}*/

			if (this.endConnection == null) {
				bool horizontal;
				nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, this.startConnection.Rotation, out horizontal));
			} else {
				Point2D connectionPoint = this.endConnection.ConnectionPoint;
				if (this.newConnectionVertices.Count > 1) {
					Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
					Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
					Line2D line1 = new Line2D(p1, p1 - p2);
					Line2D line2 = new Line2D(connectionPoint, new Vector2D(line1.Direction.Y, -line1.Direction.X));
					Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
					if (intersection.HasValue) {
						nextConnectionPoints.Add(intersection.Value);
					}
				}
				nextConnectionPoints.Add(connectionPoint);
				endRegister = this.endConnection.Register;
			}
			return nextConnectionPoints;
		}

		private PossibleHithermRegisterConnection GetHoveredRegisterConnection(Point2D planPoint, bool checkInput, bool checkOutput) {
			double xOffset = 0;
			foreach (GraphicalWall baseWall in this.product.AssociatedRoom.Walls) {
				GraphicalWall wall = baseWall;
				double yOffset = 0;
				while (wall != null) {
					foreach (GraphicalRegisterWrapper wrapper in wall.Registers) {
						if (wrapper == this.connectedWallPanel.SelectedObject || wrapper is GraphicalHithermRegisterWrapper && wrapper.HitTest(planPoint, xOffset, yOffset)) {
							PossibleHithermRegisterConnection conn;
							if (checkOutput) {
								conn = (wrapper as GraphicalHithermRegisterWrapper).GetOutputConnection(xOffset, yOffset, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register));
								if (conn.ConnectionArea.IsInside(planPoint)) {
									return conn;
								}
							}
							if (checkInput) {
								conn = (wrapper as GraphicalHithermRegisterWrapper).GetInputConnection(xOffset, yOffset, this.product, this.product.GetCircuitForRegister((wrapper as GraphicalHithermRegisterWrapper).Register));
								if (conn.ConnectionArea.IsInside(planPoint)) {
									return conn;
								}
							}
						}
					}
					yOffset += wall.GetWallHeight() * 100;
					wall = wall.DachSchraege;
				}
				xOffset += baseWall.GetWallWidth() * 100;
			}
			return null;
		}

	}
}
