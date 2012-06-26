using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using WW.Math;
using WW.Math.Geometry;
using System.Xml.Serialization;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	public partial class ConnectionPlanner : Component, IPlanner {

		public enum ConnectionMode {
			CM_NONE,
			KDM_ADD_CONNECTION,
			KDM_DEL_CONNECTION,
			KDM_SELECT_CONNECTION
		}

		public ConnectionPlanner() {
			InitializeComponent();
			this.SetLanguage();
		}

		private void SetLanguage() {
			this.cmVorlauf.Text = EuroplanRes.ConnectionPlanner_Vorlauf;
			this.cmRuecklauf.Text = EuroplanRes.ConnectionPlanner_Ruecklauf;
		}

		public ConnectionPlanner(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private ConnectionMode mode = ConnectionMode.CM_NONE;
		private Cursor customCursor = null;
		private bool highlightRoomCoordinates = true;
		private bool drawExpansionGaps = true;
		private Dictionary<Product, IProductPlanner> productsInFloor = new Dictionary<Product, IProductPlanner>();
		//private Dictionary<Distributor, DistributorPositioner> distributorsInFloor = new Dictionary<Distributor, DistributorPositioner>();
		private List<Distributor> distributorsInFloor = new List<Distributor>();
		private Floor floor = null;
		private bool planFloor = true;
		private Product product = null;

		public event EventHandler<EventArgs> ModeChanged;
		public event EventHandler<EventArgs> AnbindeleitungAdded;

		private GraphicalProductConnection selectedConnection = null;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Floor Floor {
			get { return this.floor; }
			set {
				if (value != this.floor) {
					this.floor = value;
					this.distributorsInFloor.Clear();
					if (this.floor == null) {
						this.Plan = null;
						this.product = null;
					} else {
						if (this.product != null && (this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedFloor != this.floor)) {
							this.product = null;
						}
						this.Plan = floor.AssociatedPlan;
						/*DistributorPositioner positioner;
						foreach (Distributor d in this.GetAllDistributors()) {
							positioner = new DistributorPositioner();
							positioner.Floor = floor;
							positioner.Distributor = d;
							positioner.ConnectedPlanPanel = this.connectedPlanPanel;
							this.distributorsInFloor.Add(d, positioner);
						}*/
						this.distributorsInFloor.AddRange(this.GetAllDistributors());
					}
				}
			}
		}

		// if this is null all products in the floor can be planned
		public Product Product {
			get { return this.product; }
			set {
				if (this.product != value) {
					this.product = value;
					this.Floor = (this.product != null && this.product.AssociatedRoom != null) ? this.product.AssociatedRoom.AssociatedFloor : null;
				}
			}
		}

		public bool PlanFloor {
			get { return this.planFloor; }
			set {
				if (this.planFloor != value) {
					this.planFloor = value;
					if (this.ConnectedPlanPanel != null) {
						this.ConnectedPlanPanel.InvalidateGraphics();
					}
				}
			}
		}

		public bool PlanCeiling {
			get { return !this.planFloor; }
			set {
				if (this.planFloor == value) {
					this.planFloor = !value;
					if (this.ConnectedPlanPanel != null) {
						this.ConnectedPlanPanel.InvalidateGraphics();
					}
				}
			}
		}

		private Plan tmpPlan = null;
		private Plan Plan {
			get { return this.connectedPlanPanel == null ? this.tmpPlan : this.connectedPlanPanel.Plan; }
			set {
				if (this.connectedPlanPanel == null) {
					this.tmpPlan = value;
				} else {
					this.connectedPlanPanel.Plan = value;
					this.productsInFloor.Clear();
					if (this.Plan != null) {
						ResetProducts();
					}
				}
			}
		}

		public bool AddFirstCircuit {
			get { return this.addFirstCircuit; }
			set { this.addFirstCircuit = value; }
		}

		public bool AddOtherCircuits {
			get { return this.addOtherCircuits; }
			set { this.addOtherCircuits = value; }
		}

		public bool AddVorlauf {
			get { return this.addInput; }
			set { this.addInput = value; }
		}

		public bool AddRuecklauf {
			get { return this.addOutput; }
			set { this.addOutput = value; }
		}

		private void ResetProducts() {
			this.productsInFloor.Clear();
			foreach (Product p in this.GetAllProducts()) {
				// TODO add other products
				if (p.GraphicalMode == true) {
					if (p is ModulKlimaBodenProduct) {
						ModulKlimaBodenPlanner pp = new ModulKlimaBodenPlanner();
						pp.Product = p as ModulKlimaBodenProduct;
						pp.ConnectedPlanPanel = this.connectedPlanPanel;
						pp.DrawExpansionGaps = false;
						pp.HighlightRoomCoordinates = false;
						productsInFloor.Add(p, pp);
					} else if (p is EurovalProduct) {
						EurovalPlanner pp = new EurovalPlanner();
						pp.Product = p as EurovalProduct;
						pp.ConnectedPlanPanel = this.connectedPlanPanel;
						pp.DrawExpansionGaps = false;
						pp.HighlightRoomCoordinates = false;
						productsInFloor.Add(p, pp);
					} else if (p is EcothermProduct) {
						EcothermPlanner pp = new EcothermPlanner();
						pp.Product = p as EcothermProduct;
						pp.ConnectedPlanPanel = this.connectedPlanPanel;
						pp.DrawExpansionGaps = false;
						pp.HighlightRoomCoordinates = false;
						productsInFloor.Add(p, pp);
					} else if (p is HithermProduct) {
						productsInFloor.Add(p, null);
                    } else if (p is HithermCompactProduct) {
                        productsInFloor.Add(p, null);
					} else if (p is ModulKlimaDeckeProduct) {
						ModulKlimaDeckePlanner pp = new ModulKlimaDeckePlanner();
						pp.Product = p as ModulKlimaDeckeProduct;
						pp.ConnectedPlanPanel = this.connectedPlanPanel;
						pp.HighlightRoomCoordinates = false;
						productsInFloor.Add(p, pp);
					}
				}
			}
			if (this.connectedPlanPanel as Control != null) {
				this.connectedPlanPanel.InvalidateGraphics();
			}
		}

		public ConnectionMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
			}
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set {
				if (this.connectedPlanPanel != null) {
					this.connectedPlanPanel.KeyDown -= new KeyEventHandler(connectedPlanPanel_KeyDown);
					this.tmpPlan = this.connectedPlanPanel.Plan;
				}
				this.connectedPlanPanel = value;
				if (this.connectedPlanPanel != null) {
					this.connectedPlanPanel.KeyDown += new KeyEventHandler(connectedPlanPanel_KeyDown);
					this.connectedPlanPanel.Plan = this.tmpPlan;
					this.tmpPlan = null;
				}
				this.productsInFloor.Clear();
				if (this.Plan != null) {
					this.ResetProducts();
				}
				//foreach (DistributorPositioner distPositioner in this.distributorsInFloor.Values) {
					//distPositioner.ConnectedPlanPanel = this.connectedPlanPanel;
				//}
			}
		}

		private void connectedPlanPanel_KeyDown(object sender, KeyEventArgs e) {
			if (this.mode == ConnectionMode.KDM_ADD_CONNECTION) {
				if (e.KeyCode == Keys.Escape) {
					if (this.newConnectionStart != null && this.newConnectionStart.ProductConnection != null) {
						this.newConnectionStart.ProductConnection.ConnectedProduct = null;
						this.newConnectionStart.ProductConnection.ResetCachedVerticesForDrawing();
						this.product.Connections = new List<GraphicalProductConnection>();
					}
					this.newConnectionStart = null;
					this.newConnectionVertices = null;
					this.selectedCircuit = null;
					this.selectedProduct = null;
					this.selectedDistributor = null;
					this.selectedDistributorNr = null;
					if (this.ConnectedPlanPanel != null) {
						this.ConnectedPlanPanel.InvalidateGraphics();
					}
				} else if (e.KeyCode == Keys.Back) {
					if (this.newConnectionVertices == null || this.newConnectionVertices.Count < 2) {
						if (this.newConnectionStart != null && this.newConnectionStart.ProductConnection != null) {
							this.newConnectionStart.ProductConnection.ConnectedProduct = null;
							this.newConnectionStart.ProductConnection.ResetCachedVerticesForDrawing();
							this.product.Connections = new List<GraphicalProductConnection>();
						}
						this.newConnectionStart = null;
						this.newConnectionVertices = null;
						this.selectedCircuit = null;
						this.selectedProduct = null;
						this.selectedDistributor = null;
						this.selectedDistributorNr = null;
					} else {
						this.newConnectionVertices.RemoveAt(this.newConnectionVertices.Count - 1);
					}
					if (this.connectedPlanPanel != null) {
						this.connectedPlanPanel.InvalidateGraphics();
					}
				}
			}
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl, false);
		}

		private List<Floor> GetAllFloors() {
			List<Floor> floors = new List<Floor>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.AssociatedPlanId == this.Plan.Id) {
						floors.Add(floor);
					}
				}
			}
			return floors;
		}

		private List<Room> GetAllRooms() {
			List<Room> rooms = new List<Room>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.AssociatedPlanId == this.Plan.Id) {
						foreach (Room room in floor.Rooms) {
							rooms.Add(room);
						}
					}
				}
			}
			return rooms;
		}

		private List<Product> GetAllProducts() {
			List<Product> products = new List<Product>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.AssociatedPlanId == this.Plan.Id) {
						foreach (Room room in floor.Rooms) {
							foreach (PlannedProduct product in room.PlannedProducts) {
								products.Add(product.Product);
							}
						}
					}
				}
			}
			return products;
		}

		private List<Distributor> GetAllDistributors() {
			return this.Floor.GetAllAvailableDistributors(true);
			/*List<Distributor> distributors = new List<Distributor>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					distributors.AddRange(floor.GetAllAvailableDistributors(false));
				}
			}
			return distributors;*/
		}

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl, bool export) {
			if (this.Plan != null) {
				Region clip = g.Clip;
				if (!export) {
					//foreach (KeyValuePair<Distributor, DistributorPositioner> kvp in this.distributorsInFloor) {
					foreach (Distributor dist in this.distributorsInFloor) {
						//kvp.Value.PaintAfterPlanPannel(g, additionalTransformation, mousePositionInPlan, mousePositionInControl);
						dist.Draw(g, additionalTransformation, (float)this.Plan.Measure.Value, this.Plan.InvertYAxis, this.Floor);
					}
					g.Clip = clip;
				}
				if (drawExpansionGaps && this.planFloor && !export) {
					foreach (Floor floor in this.GetAllFloors()) {
						foreach (Segment2D expansionGap in floor.ExpansionGaps) {
							Point2D start = additionalTransformation.TransformTo2D(expansionGap.Start);
							Point2D end = additionalTransformation.TransformTo2D(expansionGap.End);
							g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
						}
					}
				}
				if (!export) {
					foreach (KeyValuePair<Product, IProductPlanner> kvp in this.productsInFloor) {
						if (kvp.Value != null && ((kvp.Key.Type == Product.ProductType.FBH && this.PlanFloor) || (kvp.Key.Type == Product.ProductType.DH && this.PlanCeiling))) {
							kvp.Value.PaintAfterPlanPannel(g, additionalTransformation, mousePositionInPlan, mousePositionInControl, export);
						}
					}
				}
				g.Clip = clip;
				foreach (KeyValuePair<Product, IProductPlanner> kvp in this.productsInFloor) {
					foreach (GraphicalProductConnection connection in kvp.Key.Connections) {
						if ((connection.ConnectionType == Product.ProductType.FBH && this.PlanFloor) || (connection.ConnectionType == Product.ProductType.DH && this.PlanCeiling)) {
							connection.Draw(g, additionalTransformation, this.Plan.Measure.Value, connection == this.selectedConnection && this.Mode == ConnectionMode.KDM_SELECT_CONNECTION, this.product != null && kvp.Key != this.product);
						}
					}
				}
				if (this.mode == ConnectionMode.KDM_ADD_CONNECTION) {
					if (this.possibleProductConnection != null) {
						//foreach (PossibleConnection pc in this.possibleConnections) {
							List<PointF> connectionPoly = new List<PointF>();
							Point2D tmp;
							foreach (Point2D point in this.possibleProductConnection.ConnectionArea) {
								tmp = additionalTransformation.TransformTo2D(point);
								connectionPoly.Add(new PointF((float)tmp.X, (float)tmp.Y));
							}
							if (this.possibleProductConnection.PossibleInput && this.possibleProductConnection.Product != null || this.possibleProductConnection.PossibleOutput && this.possibleProductConnection.Distributor != null) {
								if (this.possibleProductConnection.PossibleOutput && this.possibleProductConnection.Product != null || this.possibleProductConnection.PossibleInput && this.possibleProductConnection.Distributor != null) {
									PointF[] poly = connectionPoly.ToArray();
									g.FillPolygon(new SolidBrush(System.Drawing.Color.FromArgb(128, this.connectedPlanPanel.ColorMode == ColorMode.CM_WHITE_BG ? System.Drawing.Color.Black : System.Drawing.Color.White)), poly);
									g.DrawPolygon(new Pen(this.connectedPlanPanel.ColorMode == ColorMode.CM_WHITE_BG ? System.Drawing.Color.Black : System.Drawing.Color.White), poly);
								} else {
									PointF[] poly = connectionPoly.ToArray();
									g.FillPolygon(new SolidBrush(System.Drawing.Color.FromArgb(128, System.Drawing.Color.Red)), poly);
									g.DrawPolygon(new Pen(System.Drawing.Color.Red), poly);
								}
							} else if (this.possibleProductConnection.PossibleOutput && this.possibleProductConnection.Product != null || this.possibleProductConnection.PossibleInput && this.possibleProductConnection.Distributor != null) {
								PointF[] poly = connectionPoly.ToArray();
								g.FillPolygon(new SolidBrush(System.Drawing.Color.FromArgb(128, System.Drawing.Color.Blue)), poly);
								g.DrawPolygon(new Pen(System.Drawing.Color.Blue), poly);
							} else if (this.possibleProductConnection.ProductConnection != null && this.possibleProductConnection.PossibleInput && this.possibleProductConnection.PossibleOutput) {
								PointF[] poly = connectionPoly.ToArray();
								g.FillPolygon(new SolidBrush(System.Drawing.Color.FromArgb(128, this.connectedPlanPanel.ColorMode == ColorMode.CM_WHITE_BG ? System.Drawing.Color.Black : System.Drawing.Color.White)), poly);
								g.DrawPolygon(new Pen(this.connectedPlanPanel.ColorMode == ColorMode.CM_WHITE_BG ? System.Drawing.Color.Black : System.Drawing.Color.White), poly);
							}
						//}
					}

					//Pen p = new Pen(Color.Green, (float)(0.021 * this.Plan.Measure.Value * additionalTransformation.M00));
					if (this.newConnectionVertices != null && this.newConnectionVertices.Count > 0) {
						List<Point2D> vertices = new List<Point2D>();
						vertices.AddRange(this.newConnectionVertices);
						vertices.AddRange(this.nextConnectionPoints);
						if (this.newConnectionStart.Distributor != null) {
							vertices.Reverse();
						}
						Europlan.Common.Product productToUse = this.selectedProduct != null ? this.selectedProduct : this.product;
						GraphicalProductConnection tmpConnection = new GraphicalProductConnection(Project.Instance.GetPlannedProduct(productToUse), this.newConnectionStart.Distributor, vertices, this.addFirstCircuit, this.addOtherCircuits, 0, this.AddVorlauf, this.AddRuecklauf, Product.ProductType.REST);
						tmpConnection.FinishedConnection = this.newConnectionEndsAtDistributor || this.newConnectionStart.Distributor != null;
						//tmpConnection.Vertices.AddRange(this.newConnectionVertices);
						//tmpConnection.Vertices.AddRange(this.nextConnectionPoints);
						tmpConnection.Draw(g, additionalTransformation, this.Plan.Measure.Value, true, false);
						/*double width = this.newConnectionStart.ProductCircuitCount * 0.05;
						Pen p = new Pen(new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.LargeCheckerBoard, Color.Red, Color.Blue), (float)(width * this.Plan.Measure.Value * additionalTransformation.M00));
						Point2D oldVertex2D = additionalTransformation.TransformTo2D(this.newConnectionVertices.Vertices[0]);
						PointF oldVertex = new PointF((float)oldVertex2D.X, (float)oldVertex2D.Y);
						Point2D newVertex2D;
						PointF newVertex;
						p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
						for (int i = 1; i < this.newConnectionVertices.Count; i++) {
							newVertex2D = additionalTransformation.TransformTo2D(this.newConnectionVertices.Vertices[i]);
							newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
							if (i == 2) {
								p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
							}
							g.DrawLine(p, oldVertex, newVertex);
							oldVertex = newVertex;
						}
						for (int i = 0; i < this.nextConnectionPoints.Count; i++) {
							newVertex2D = additionalTransformation.TransformTo2D(this.nextConnectionPoints[i]);
							newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
							if (i == 2) {
								p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
							}
							if (i == this.nextConnectionPoints.Count - 1) {
								p.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
							}
							g.DrawLine(p, oldVertex, newVertex);
							oldVertex = newVertex;
						}*/
					}
				}
				if (!export && this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null && this.product.AssociatedRoom.RoomCoordinates.Count > 2) {
					GraphicsPath fillPath = new GraphicsPath();
					fillPath.StartFigure();
					PointF[] array = new PointF[this.product.AssociatedRoom.RoomCoordinates.Count];
					int i = 0;
					foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
						Point2D tmp = additionalTransformation.TransformTo2D(point);
						array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					fillPath.AddPolygon(array);
					fillPath.CloseFigure();
					System.Drawing.Color c = System.Drawing.Color.FromArgb(128, System.Drawing.Color.Green);
					Brush b = new SolidBrush(c);
					//g.FillPath(b, fillPath);
					g.DrawPath(new Pen(b, (float)(0.05 * this.Plan.Measure.Value * additionalTransformation.M00)), fillPath);
					fillPath.Dispose();
				}
			}
		}

		private List<Point2D> newConnectionVertices = null;
		//private GraphicalProductConnection newConnectionVertices = null;
		private PossibleProductConnection newConnectionStart = null;
		private bool newConnectionStartAtOutput = true;
		//private bool newConnectionLastSegmentHorizontal = true;

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			bool redraw = false;
			if (this.mode == ConnectionMode.KDM_ADD_CONNECTION && button != MouseButtons.Middle) {
				if (this.newConnectionStart == null && this.possibleProductConnection != null) {
					if (this.possibleProductConnection.ProductConnection != null && this.product != null) {
						NewGraphicalProductToProductConnection form = new NewGraphicalProductToProductConnection();
						if (form.ShowDialog() == DialogResult.OK) {
							this.possibleProductConnection.ProductConnection.ConnectedProduct = Project.Instance.GetPlannedProduct(this.product);
							this.possibleProductConnection.ProductConnection.ProductConnectedAtSegment = this.possibleProductConnection.SegmentId;
							this.possibleProductConnection.ProductConnection.ProductConnectedPoint = this.possibleProductConnection.DistFromSegmentStart;
							this.possibleProductConnection.ProductConnection.ProductConnectedVorlaufseitig = form.Vorlaufseitig;
							this.product.PlannedConnection = new ProductConnection(this.possibleProductConnection.ProductConnection.Product, Circuit.CircuitConnectionTypeEnum.VORLAUF);
							this.product.Connections = new List<GraphicalProductConnection>();
							this.product.Connections.Add(new GraphicalProductToProductConnection(Project.Instance.GetPlannedProduct(this.product), this.possibleProductConnection.ProductConnection.Product));
							this.possibleProductConnection.ProductConnection.ResetCachedVerticesForDrawing();
							this.newConnectionStart = this.possibleProductConnection;
							this.possibleProductConnection = null;
						}
						form.Dispose();
					} else {
						PossibleProductConnection connection = null;
						/*foreach (PossibleConnection pc in this.possibleConnections) {
							if (pc.ConnectionArea.IsInside(planPoint)) {
								connection = pc;
								break;
							}
						}*/
						if (this.possibleProductConnection.ConnectionArea.IsInside(planPoint)) {
							connection = this.possibleProductConnection;
						}
						if (connection != null) {
							if (connection.PossibleInput && connection.PossibleOutput) {
								//this.newConnectionStart = connection;
								//this.contextMenu.Show(this.connectedPlanPanel as Control, pointInControl);
								this.AddConnection(connection, true);
							} else if (connection.PossibleInput) {
								this.AddConnection(connection, true);
							} else if (connection.PossibleOutput) {
								this.AddConnection(connection, false);
							}
							this.newConnectionEndsAtDistributor = false;
							this.selectedProduct = connection.Product;
						}
					}
				} else if (this.newConnectionStart != null) {
					if (this.newConnectionStart.ProductConnection != null) {
						this.newConnectionStart = null;
					} else {
						PossibleProductConnection endConnection;
						this.newConnectionVertices.AddRange(this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endConnection));
						this.nextConnectionPoints.Clear();
						if (endConnection != null) {
							int index;
							// TODO check if connection is valid!
							PossibleProductConnection productConnection;
							PossibleProductConnection distributorConnection;
							bool ok = false;
							bool vorlauf = true;
							if (this.newConnectionStart.Product != null && endConnection.Distributor != null) {
								productConnection = this.newConnectionStart;
								distributorConnection = endConnection;
								//vorlauf = !this.newConnectionStartAtOutput;
								ok = true;
							} else if (endConnection.Product != null && this.newConnectionStart.Distributor != null) {
								distributorConnection = this.newConnectionStart;
								productConnection = endConnection;
								this.newConnectionVertices.Reverse();
								//vorlauf = this.newConnectionStartAtOutput;
								ok = true;
							} else {
								productConnection = new PossibleProductConnection();
								distributorConnection = new PossibleProductConnection();
							}
							int count = productConnection.Product.PlannedCircuits.Count;
							List<int> openInputs = distributorConnection.Distributor.GetOpenInputs();
							List<int> openOutputs = distributorConnection.Distributor.GetOpenOutputs();
							List<int> distributorIndices = new List<int>();
							if (ok) {
								if (productConnection.Product.PlannedConnection == null || distributorConnection.Distributor != productConnection.Product.PlannedConnection.Distributor) {
									productConnection.Product.PlannedConnection = new ProductConnection(distributorConnection.Distributor);
								}
								//productConnection.Product.Connections.Add(new GraphicalProductConnection(Project.Instance.GetPlannedProduct(productConnection.Product), distributorConnection.Distributor, this.newConnectionVertices, productConnection.Circuits, distributorConnection.DistributorStartPosition, distributorConnection.DistributorCircuitCount, vorlauf, this.planFloor ? Product.ProductType.FBH : Product.ProductType.DH));
								productConnection.Product.Connections.Add(new GraphicalProductConnection(Project.Instance.GetPlannedProduct(productConnection.Product), distributorConnection.Distributor, this.newConnectionVertices, productConnection.ProductFirstCircuit, productConnection.ProductOtherCircuits, distributorConnection.DistributorStartPosition, productConnection.PossibleInput, productConnection.PossibleOutput, this.planFloor ? Product.ProductType.FBH : Product.ProductType.DH));
								this.newConnectionVertices = null;
								this.newConnectionStart = null;
								this.selectedCircuit = null;
								this.selectedProduct = null;
								this.selectedDistributor = null;
								this.selectedDistributorNr = null;
								if (this.AnbindeleitungAdded != null) {
									this.AnbindeleitungAdded(this, EventArgs.Empty);
								}
							}
						}
					}
					redraw = true;
				}
			} else if (this.Mode == ConnectionMode.KDM_DEL_CONNECTION) {
				double bestDist = double.MaxValue;
				GraphicalProductConnection bestConnection = null;
				Product bestProduct = null;
				foreach (Room room in this.floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						foreach (GraphicalProductConnection conn in pp.Product.Connections) {
							double dist = conn.GetDistance(planPoint);
							if (dist < bestDist && dist <= this.Plan.Measure.Value * 0.025) {
								bestDist = dist;
								bestConnection = conn;
								bestProduct = pp.Product;
							}
						}
					}
				}
				if (bestConnection != null) {
					bestProduct.Connections.Remove(bestConnection);
					redraw = true;
				}
			} else if (this.Mode == ConnectionMode.KDM_SELECT_CONNECTION) {
				this.selectedConnection = null;
				foreach (Room room in this.floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						foreach (GraphicalProductConnection conn in pp.Product.Connections) {
							if (this.product == null || pp.Product == this.product) {
								if (conn.HitTest(planPoint, this.Plan.Measure.Value)) {
									this.selectedConnection = conn;
									break;
								}
							}
						}
						if (this.selectedConnection != null) {
							break;
						}
					}
					if (this.selectedConnection != null) {
						break;
					}
				}
				if (this.connectedPlanPanel != null) {
					this.connectedPlanPanel.InvalidateGraphics();
				}
			}
			return redraw;
		}

		private void AddConnection(PossibleProductConnection connection, bool input) {
			this.newConnectionVertices = new List<Point2D>();
			this.newConnectionVertices.Add(connection.ConnectionPoint);
			this.newConnectionStart = connection;
			this.newConnectionStartAtOutput = !input;
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

		/*private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out KlimaFlaechenModul endModule) {
			List<Point2D> nextConnectionPoints = new List<Point2D>();
			endModule = null;
			int index;
			if (this.newConnectionStartAtOutput) {
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> input in this.GetModuleInputs()) {
					if (input.Value.IsInside(mousePoint)) {
						if (this.product.GetCircuitForModul(input.Key, out index) == this.product.GetCircuitForModul(this.newConnectionStart, out index)) {
							endModule = input.Key;
						}
						break;
					}
				}
			} else {
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> output in this.GetModuleOutputs()) {
					if (output.Value.IsInside(mousePoint)) {
						if (this.product.GetCircuitForModul(output.Key, out index) == this.product.GetCircuitForModul(this.newConnectionStart, out index)) {
							endModule = output.Key;
						}
						break;
					}
				}
			}
			if (this.product.GetCircuitForModul(this.newConnectionStart, out index).GetAllLinkedModules(this.newConnectionStart).Contains(endModule)) {
				endModule = null;
			}
			if (endModule == null) {
				bool horizontal;
				nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, this.newConnectionStart.GraphRotation, out horizontal));
			} else {
				Point2D connectionPoint = this.newConnectionStartAtOutput ? endModule.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) : endModule.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
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
			}
			return nextConnectionPoints;
		}*/

		private List<Point2D> nextConnectionPoints = new List<Point2D>();
		//private Nullable<Point2D> startPoint = null;
		//private Nullable<Point2D> endPoint = null;
		private Distributor selectedDistributor = null;
		private Nullable<int> selectedDistributorNr = null;
		private Product selectedProduct = null;
		private Circuit selectedCircuit = null;
		//List<PossibleConnection> possibleConnections = null;
		private PossibleProductConnection possibleProductConnection = null;
		private bool newConnectionEndsAtDistributor = false;

		private bool addInput = true;
		private bool addOutput = true;
		private bool addFirstCircuit = true;
		private bool addOtherCircuits = true;

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			bool redraw = false;
			if (this.Mode == ConnectionMode.KDM_ADD_CONNECTION) {
				if (this.newConnectionStart != null && this.newConnectionStart.ProductConnection != null) {
					Point2D v1 = this.newConnectionStart.ProductConnection.Vertices[this.newConnectionStart.ProductConnection.ProductConnectedAtSegment];
					Point2D v2 = this.newConnectionStart.ProductConnection.Vertices[this.newConnectionStart.ProductConnection.ProductConnectedAtSegment + 1];
					Line2D l = new Line2D(v1, v1 - v2);
					Point2D closestPoint = l.GetClosestPoint(planPoint);
					Vector2D v = closestPoint - planPoint;
					if (v.X > 0) {
						this.newConnectionStart.ProductConnection.ProductConnectionRight = true;
					} else if (v.X == 0) {
						if (v.Y > 0) {
							this.newConnectionStart.ProductConnection.ProductConnectionRight = true;
						} else {
							this.newConnectionStart.ProductConnection.ProductConnectionRight = false;
						}
					} else {
						this.newConnectionStart.ProductConnection.ProductConnectionRight = false;
					}
					this.newConnectionStart.ProductConnection.ResetCachedVerticesForDrawing();
					redraw = true;
				} else {
					PossibleProductConnection oldPossibleProductConnection = possibleProductConnection;
					possibleProductConnection = null;
					Europlan.Common.Product productToUse = this.selectedProduct != null ? this.selectedProduct : this.product;
					if (productToUse != null && selectedDistributor == null) {
						foreach (Distributor d in this.GetAllDistributors()) {
							if (productToUse.Connections != null) {
								bool ok = true;
								foreach (GraphicalProductConnection conn in productToUse.Connections) {
									if (conn.Distributor != null && conn.Distributor != d) {
										ok = false;
										break;
									}
								}
								if (!ok) {
									continue;
								}
							}
							bool addFirst = this.addFirstCircuit;
							bool addOthers = this.addOtherCircuits;
							foreach (GraphicalProductConnection conn in productToUse.Connections) {
								if (conn.FirstCircuit) {
									addFirst = false;
								}
								if (conn.OtherCircuits) {
									addOthers = false;
								}
							}
							int circuitCount = 0;
							if (addFirst) {
								circuitCount++;
							}
							if (addOthers) {
								circuitCount += productToUse.PlannedCircuitCount - 1;
							}
							if (circuitCount > 0) {
								possibleProductConnection = d.GetPossibleProductConnections(addInput, addOutput, this.Plan.Measure.Value, this.Plan.InvertYAxis, planPoint, productToUse, this.floor, circuitCount, false, 0.0);
								if (possibleProductConnection != null) {
									break;
								}
								if (possibleProductConnection == null && productToUse != null && productToUse.GraphicalArea != null && productToUse.GraphicalArea.Count > 2 && productToUse.GraphicalArea.IsInside(planPoint)) {
									foreach (PlannedProduct p in d.PlannedConnectedProducts) {
										if (productToUse != p.Product) {
											foreach (GraphicalProductConnection gpc in p.Product.Connections) {
												if ((gpc.ConnectionType == Product.ProductType.FBH && this.PlanFloor) || (gpc.ConnectionType == Product.ProductType.DH && this.PlanCeiling)) {
													possibleProductConnection = gpc.GetPossibleProductConnection(this.Plan.Measure.Value, this.Plan.InvertYAxis, planPoint, productToUse, this.floor, circuitCount);
													if (possibleProductConnection != null) {
														break;
													}
												}
											}
											if (possibleProductConnection != null) {
												break;
											}
										}
									}
								}
							}
						}
					}
					if (possibleProductConnection == null && selectedProduct == null) {
						if (this.product == null) {
							foreach (Product p in this.GetAllProducts()) {
								possibleProductConnection = p.GetPossibleProductConnection(addInput, addOutput, addFirstCircuit, addOtherCircuits, this.Plan.Measure.Value, this.Plan.InvertYAxis, planPoint);
								if (possibleProductConnection != null) {
									break;
								}
							}
						} else {
							possibleProductConnection = this.product.GetPossibleProductConnection(addInput, addOutput, addFirstCircuit, addOtherCircuits, this.Plan.Measure.Value, this.Plan.InvertYAxis, planPoint);
						}
					}

					if (oldPossibleProductConnection == null) {
						redraw = redraw || possibleProductConnection != null || this.newConnectionStart != null;
					} else {
						redraw = redraw || possibleProductConnection == null || this.newConnectionStart != null;
					}
					if (this.newConnectionStart != null) {
						PossibleProductConnection endConnection;
						this.nextConnectionPoints = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endConnection);
						this.newConnectionEndsAtDistributor = endConnection != null && endConnection.Distributor != null;
					} else {
						this.nextConnectionPoints = new List<Point2D>();
					}
				}
				redraw = redraw || possibleProductConnection != null || this.newConnectionStart != null;
			} else if (this.mode == ConnectionMode.KDM_SELECT_CONNECTION) {
				double bestDist = double.MaxValue;
				GraphicalProductConnection bestConnection = null;
				Product bestProduct = null;
				double scale = 1;
				if (this.Plan is ImagePlan) {
					scale = (this.Plan as ImagePlan).Scale.Value;
				} else if (this.Plan is CadPlan) {
					scale = (this.Plan as CadPlan).Scale;
				}
				bool found = false;
				if (this.selectedConnection != null) {
					foreach (GraphicalConnectionAnchor a in this.selectedConnection.GetAnchors(this.Plan.Measure.Value)) {
						if (a.HitTest(planPoint, this.Plan.Measure.Value)) {
							(this.ConnectedPlanPanel as Control).Cursor = a.Cursor;
							found = true;
							break;
						}
					}
				}
				if (!found) {
					(this.ConnectedPlanPanel as Control).Cursor = Cursors.Cross;
				}
			}
			return redraw;
		}

		private GraphicalProductConnection draggingConnection = null;
		private GraphicalConnectionAnchor draggingAnchor = null;

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == ConnectionMode.KDM_SELECT_CONNECTION) {
				if (this.selectedConnection != null) {
					foreach (GraphicalConnectionAnchor a in this.selectedConnection.GetAnchors(this.Plan.Measure.Value)) {
						if (a.HitTest(planPoint, this.Plan.Measure.Value)) {
							draggingConnection = this.selectedConnection;
							draggingAnchor = a;
							draggingConnection.StartDrag(draggingAnchor, planPoint);
							break;
						}
					}
				}
				/*foreach (Product p in this.productsInFloor.Keys) {
					foreach (GraphicalProductConnection c in p.Connections) {
						foreach (GraphicalConnectionAnchor a in c.GetAnchors(this.Plan.Measure.Value)) {
							if (a.HitTest(planPoint, this.Plan.Measure.Value)) {
								draggingConnection = c;
								draggingAnchor = a;
								draggingConnection.StartDrag(draggingAnchor, planPoint);
								break;
							}
						}
					}
				}*/
			}
			return true;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == ConnectionMode.KDM_SELECT_CONNECTION && this.draggingConnection != null && this.draggingAnchor != null) {
				this.draggingConnection.MoveDrag(draggingAnchor, planPoint);
			}
			return true;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == ConnectionMode.KDM_SELECT_CONNECTION && this.draggingConnection != null && this.draggingAnchor != null) {
				this.draggingConnection.EndDrag(draggingAnchor, planPoint);
				this.draggingAnchor = null;
			}
			return true;
		}

		internal bool ShiftPressed {
			get { return (Control.ModifierKeys & (Keys.Shift | Keys.ShiftKey | Keys.LShiftKey | Keys.RShiftKey)) != Keys.None; }
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			bool redraw = false;
			if (this.Mode == ConnectionMode.KDM_SELECT_CONNECTION && this.selectedConnection != null) {
				if (key == Keys.Delete) {
					if (this.selectedConnection.ConnectedProduct != null) {
						this.selectedConnection.ConnectedProduct.Product.Connections = new List<GraphicalProductConnection>();
					}
					foreach (Product product in this.productsInFloor.Keys) {
						if (product.Connections.Contains(this.selectedConnection)) {
							product.DeleteConnection(this.selectedConnection);
							this.selectedConnection = null;
							redraw = true;
							break;
						}
					}
				}
			}
			if (redraw && this.ConnectedPlanPanel != null) {
				this.ConnectedPlanPanel.InvalidateGraphics();
			}
			return redraw;
		}

		#endregion

		public event ProjectChangedHandler ProjectChanged;

		/*private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleInputs() {
			Dictionary<KlimaFlaechenModul, Polygon2D> inputAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			Point2D input12D, input22D, input32D, input42D;

			foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
				foreach (KlimaFlaechenModul modul in c.Row.List) {
					Matrix3D transformation = Matrix3D.Identity;
					transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
					transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);

					if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == modul.GraphBottomUp) {
						if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							input12D = transformation.Transform(new Point2D(width, height));
							input22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
						} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							input12D = transformation.Transform(new Point2D(0, height));
							input22D = transformation.Transform(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
						}
					} else {
						if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							input12D = transformation.Transform(new Point2D(0, 0));
							input22D = transformation.Transform(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
						} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							input12D = transformation.Transform(new Point2D(width, 0));
							input22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
						}
					}
				}
			}

			return inputAreas;
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleOutputs() {
			Dictionary<KlimaFlaechenModul, Polygon2D> outputAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			Point2D output12D, output22D, output32D, output42D;

			foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
				foreach (KlimaFlaechenModul modul in c.Row.List) {
					Matrix3D transformation = Matrix3D.Identity;
					transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
					transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);

					if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == modul.GraphBottomUp) {
						if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							output12D = transformation.Transform(new Point2D(0, 0));
							output22D = transformation.Transform(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
						} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							output12D = transformation.Transform(new Point2D(width, 0));
							output22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
						}
					} else {
						if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							output12D = transformation.Transform(new Point2D(width, height));
							output22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
						} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							output12D = transformation.Transform(new Point2D(0, height));
							output22D = transformation.Transform(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
						}
					}
				}
			}

			return outputAreas;
		}

		private Polygon2D GetModuleArea(KlimaFlaechenModul modul) {
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			Matrix3D transformation = Matrix3D.Identity;
			transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
			transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);
			Polygon2D modulArea = new Polygon2D();
			modulArea.Add(transformation.Transform(new Point2D(0, 0)));
			modulArea.Add(transformation.Transform(new Point2D(width, 0)));
			modulArea.Add(transformation.Transform(new Point2D(width, height)));
			modulArea.Add(transformation.Transform(new Point2D(0, height)));
			return modulArea;
		}*/

		internal void DrawDxf(DxfModel model, DxfLayer modulLayer, DxfLayer floorConstructionLayer) {
			// TODO
		}

		[DefaultValue(true)]
		public bool HighlightRoomCoordinates {
			get { return this.highlightRoomCoordinates; }
			set { this.highlightRoomCoordinates = value; }
		}

		[DefaultValue(true)]
		public bool DrawExpansionGaps {
			get { return this.drawExpansionGaps; }
			set { this.drawExpansionGaps = value; }
		}

		private void cmVorRuecklauf(object sender, EventArgs e) {
			if (sender == this.cmVorlauf) {
				this.newConnectionStart.PossibleInput = false;
				this.AddConnection(this.newConnectionStart, true);
			} else if (sender == this.cmRuecklauf) {
				this.newConnectionStart.PossibleOutput = false;
				this.AddConnection(this.newConnectionStart, false);
			}
		}

		private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out PossibleProductConnection endConnection) {
			List<Point2D> nextConnectionPoints = new List<Point2D>();
			endConnection = null;
			int index;
			if (this.newConnectionStartAtOutput) {
				/*foreach (PossibleConnection pc in this.possibleConnections) {
					if (pc.ConnectionArea.IsInside(mousePoint)) {
						// TODO check if this connection is valid!
						endConnection = pc;
						break;
					}
				}*/
				if (this.possibleProductConnection != null && this.possibleProductConnection.ConnectionArea.IsInside(mousePoint)) {
					// TODO check if this connection is valid
					endConnection = this.possibleProductConnection;
				}
			} else {
				/*foreach (PossibleConnection pc in this.possibleConnections) {
					if (pc.ConnectionArea.IsInside(mousePoint)) {
						// TODO check if this connection is valid!
						endConnection = pc;
						break;
					}
				}*/
				if (this.possibleProductConnection != null && this.possibleProductConnection.ConnectionArea.IsInside(mousePoint)) {
					// TODO check if this connection is valid
					endConnection = this.possibleProductConnection;
				}
			}
			// TODO check if the connection is valid!
			/*if (this.product.GetCircuitForModul(this.newConnectionStart, out index).GetAllLinkedModules(this.newConnectionStart).Contains(endModule)) {
				endModule = null;
			}*/
			if (endConnection == null) {
				bool horizontal;
				nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, this.newConnectionStart.Rotation, out horizontal));
			} else {
				Point2D connectionPoint = endConnection.ConnectionPoint;
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
			}
			return nextConnectionPoints;
		}

		public void ReGenerateConnectionPipes() {
			ConnectionPlanner.ReGenerateConnectionPipes(this.Floor);
		}

		public static void ReGenerateConnectionPipes(Floor floor) {
			if (floor == null) {
				return;
			}
			Plan plan = floor.AssociatedPlan;
			if (plan == null) {
				return;
			}
			double measure = plan.Measure.Value;
			// Update connectionpipes in products to match the graphical connections
			foreach (Room room in floor.Rooms) {
				foreach (PlannedProduct pp in room.PlannedProducts) {
					Dictionary<Product, ConnectionPipe> previousProductPipesVl = new Dictionary<Product, ConnectionPipe>();
					Dictionary<Product, ConnectionPipe> previousProductPipesRl = new Dictionary<Product, ConnectionPipe>();
					Dictionary<Room, ConnectionPipe> previousRoomPipesVl = new Dictionary<Room, ConnectionPipe>();
					Dictionary<Room, ConnectionPipe> previousRoomPipesRl = new Dictionary<Room, ConnectionPipe>();
					ConnectionPipe previousRestPipeVl = null;
					ConnectionPipe previousRestPipeRl = null;
					List<ConnectionPipe> deletePipes = new List<ConnectionPipe>();
					foreach (ConnectionPipe pipe in pp.Product.PlannedConnectionPipes) {
						if (pipe.IsGenerated) {
							deletePipes.Add(pipe);
							if (pipe.ConnectionThrough != null) {
								if (pipe.Ruecklauf > 0) {
									previousProductPipesRl.Add(pipe.ConnectionThrough.Product, pipe);
								} else {
									previousProductPipesVl.Add(pipe.ConnectionThrough.Product, pipe);
								}
							} else if (pipe.Room != null) {
								if (pipe.Ruecklauf > 0) {
									previousRoomPipesRl.Add(pipe.Room, pipe);
								} else {
									previousRoomPipesVl.Add(pipe.Room, pipe);
								}
							} else {
								if (pipe.Ruecklauf > 0) {
									previousRestPipeRl = pipe;
								} else {
									previousRestPipeVl = pipe;
								}
							}
						}
					}

					Dictionary<Product, double> vlThroughProduct = new Dictionary<Product, double>();
					Dictionary<Product, double> rlThroughProduct = new Dictionary<Product, double>();
					Dictionary<Room, double> vlThroughRoom = new Dictionary<Room, double>();
					Dictionary<Room, double> rlThroughRoom = new Dictionary<Room, double>();
					double vlRest = 0;
					double rlRest = 0;

					foreach (GraphicalProductConnection connection in pp.Product.Connections) {
						if (connection.Vorlauf) {
							vlRest += connection.GetLength(measure);
						}
						if (connection.Ruecklauf) {
							rlRest += connection.GetLength(measure);
						}
						foreach (Room roomThrough in floor.Rooms) {
							double roomLength = connection.GetPartInsidePolygon(new Polygon2D(roomThrough.RoomCoordinates)) / measure;
							if (roomLength > 0) {
								if (connection.Vorlauf) {
									vlRest -= roomLength;
								}
								if (connection.Ruecklauf) {
									rlRest -= roomLength;
								}
								foreach (PlannedProduct ppThrough in roomThrough.PlannedProducts) {
									if (pp != ppThrough && connection.ConnectionType == ppThrough.Product.Type) {
										double throughLength = connection.GetPartInsidePolygon(ppThrough.Product.GraphicalArea) / measure;
										if (throughLength > 0) {
											roomLength -= throughLength;
											if (connection.Vorlauf) {
												if (vlThroughProduct.ContainsKey(ppThrough.Product)) {
													vlThroughProduct[ppThrough.Product] += throughLength;
												} else {
													vlThroughProduct[ppThrough.Product] = throughLength;
												}
											}
											if (connection.Ruecklauf) {
												if (rlThroughProduct.ContainsKey(ppThrough.Product)) {
													rlThroughProduct[ppThrough.Product] += throughLength;
												} else {
													rlThroughProduct[ppThrough.Product] = throughLength;
												}
											}
										}
									}
								}
								if (roomLength > 0) {
									if (connection.Vorlauf) {
										if (vlThroughRoom.ContainsKey(roomThrough)) {
											vlThroughRoom[roomThrough] += roomLength;
										} else {
											vlThroughRoom[roomThrough] = roomLength;
										}
									}
									if (connection.Ruecklauf) {
										if (rlThroughRoom.ContainsKey(roomThrough)) {
											rlThroughRoom[roomThrough] += roomLength;
										} else {
											rlThroughRoom[roomThrough] = roomLength;
										}
									}
								}
							}
						}
					}

					foreach (ConnectionPipe pipe in deletePipes) {
						pp.Product.PlannedConnectionPipes.Remove(pipe);
					}

					foreach (Room roomThrough in floor.Rooms) {
						foreach (PlannedProduct ppThrough in roomThrough.PlannedProducts) {
							if (vlThroughProduct.ContainsKey(ppThrough.Product)) {
								ConnectionPipe pipe = new ConnectionPipe();
								if (previousProductPipesVl.ContainsKey(ppThrough.Product)) {
									pipe.PipeType = previousProductPipesVl[ppThrough.Product].PipeType;
									pipe.Insulation = previousProductPipesVl[ppThrough.Product].Insulation;
									pipe.Verlegeart = previousProductPipesVl[ppThrough.Product].Verlegeart;
								} else if (previousRoomPipesVl.ContainsKey(roomThrough)) {
									pipe.PipeType = previousRoomPipesVl[roomThrough].PipeType;
									pipe.Insulation = previousRoomPipesVl[roomThrough].Insulation;
									pipe.Verlegeart = previousRoomPipesVl[roomThrough].Verlegeart;
								} else if (previousRestPipeVl != null) {
									pipe.PipeType = previousRestPipeVl.PipeType;
									pipe.Insulation = previousRestPipeVl.Insulation;
									pipe.Verlegeart = previousRestPipeVl.Verlegeart;
								} else if (previousProductPipesRl.ContainsKey(ppThrough.Product)) {
									pipe.PipeType = previousProductPipesRl[ppThrough.Product].PipeType;
									pipe.Insulation = previousProductPipesRl[ppThrough.Product].Insulation;
									pipe.Verlegeart = previousProductPipesRl[ppThrough.Product].Verlegeart;
								} else if (previousRoomPipesRl.ContainsKey(roomThrough)) {
									pipe.PipeType = previousRoomPipesRl[roomThrough].PipeType;
									pipe.Insulation = previousRoomPipesRl[roomThrough].Insulation;
									pipe.Verlegeart = previousRoomPipesRl[roomThrough].Verlegeart;
								} else if (previousRestPipeRl != null) {
									pipe.PipeType = previousRestPipeRl.PipeType;
									pipe.Insulation = previousRestPipeRl.Insulation;
									pipe.Verlegeart = previousRestPipeRl.Verlegeart;
								} else {
									pipe.PipeType = pp.Product.DefaultPipeType;
									if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
										pipe.Insulation = ConnectionPipe.InsulationEnum.IN_VL_RL;
										pipe.Verlegeart = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
									}
								}
								pipe.IsGenerated = true;
								pipe.Room = roomThrough;
								pipe.ConnectionThrough = ppThrough;
								pipe.Vorlauf = vlThroughProduct[ppThrough.Product];
								pp.Product.PlannedConnectionPipes.Add(pipe);
							}
							if (rlThroughProduct.ContainsKey(ppThrough.Product)) {
								ConnectionPipe pipe = new ConnectionPipe();
								if (previousProductPipesRl.ContainsKey(ppThrough.Product)) {
									pipe.PipeType = previousProductPipesRl[ppThrough.Product].PipeType;
									pipe.Insulation = previousProductPipesRl[ppThrough.Product].Insulation;
									pipe.Verlegeart = previousProductPipesRl[ppThrough.Product].Verlegeart;
								} else if (previousRoomPipesRl.ContainsKey(roomThrough)) {
									pipe.PipeType = previousRoomPipesRl[roomThrough].PipeType;
									pipe.Insulation = previousRoomPipesRl[roomThrough].Insulation;
									pipe.Verlegeart = previousRoomPipesRl[roomThrough].Verlegeart;
								} else if (previousRestPipeRl != null) {
									pipe.PipeType = previousRestPipeRl.PipeType;
									pipe.Insulation = previousRestPipeRl.Insulation;
									pipe.Verlegeart = previousRestPipeRl.Verlegeart;
								} else if (previousProductPipesVl.ContainsKey(ppThrough.Product)) {
									pipe.PipeType = previousProductPipesVl[ppThrough.Product].PipeType;
									pipe.Insulation = previousProductPipesVl[ppThrough.Product].Insulation;
									pipe.Verlegeart = previousProductPipesVl[ppThrough.Product].Verlegeart;
								} else if (previousRoomPipesVl.ContainsKey(roomThrough)) {
									pipe.PipeType = previousRoomPipesVl[roomThrough].PipeType;
									pipe.Insulation = previousRoomPipesVl[roomThrough].Insulation;
									pipe.Verlegeart = previousRoomPipesVl[roomThrough].Verlegeart;
								} else if (previousRestPipeVl != null) {
									pipe.PipeType = previousRestPipeVl.PipeType;
									pipe.Insulation = previousRestPipeVl.Insulation;
									pipe.Verlegeart = previousRestPipeVl.Verlegeart;
								} else {
									pipe.PipeType = pp.Product.DefaultPipeType;
									if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
										pipe.Insulation = ConnectionPipe.InsulationEnum.IN_VL_RL;
										pipe.Verlegeart = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
									}
								}
								pipe.IsGenerated = true;
								pipe.Room = roomThrough;
								pipe.ConnectionThrough = ppThrough;
								pipe.Ruecklauf = rlThroughProduct[ppThrough.Product];
								pp.Product.PlannedConnectionPipes.Add(pipe);
							}
						}
						if (vlThroughRoom.ContainsKey(roomThrough)) {
							ConnectionPipe pipe = new ConnectionPipe();
							if (previousRoomPipesVl.ContainsKey(roomThrough)) {
								pipe.PipeType = previousRoomPipesVl[roomThrough].PipeType;
								pipe.Insulation = previousRoomPipesVl[roomThrough].Insulation;
								pipe.Verlegeart = previousRoomPipesVl[roomThrough].Verlegeart;
							} else if (previousRestPipeVl != null) {
								pipe.PipeType = previousRestPipeVl.PipeType;
								pipe.Insulation = previousRestPipeVl.Insulation;
								pipe.Verlegeart = previousRestPipeVl.Verlegeart;
							} else if (previousRoomPipesRl.ContainsKey(roomThrough)) {
								pipe.PipeType = previousRoomPipesRl[roomThrough].PipeType;
								pipe.Insulation = previousRoomPipesRl[roomThrough].Insulation;
								pipe.Verlegeart = previousRoomPipesRl[roomThrough].Verlegeart;
							} else if (previousRestPipeRl != null) {
								pipe.PipeType = previousRestPipeRl.PipeType;
								pipe.Insulation = previousRestPipeRl.Insulation;
								pipe.Verlegeart = previousRestPipeRl.Verlegeart;
							} else {
								pipe.PipeType = pp.Product.DefaultPipeType;
								if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
									pipe.Insulation = ConnectionPipe.InsulationEnum.IN_VL_RL;
									pipe.Verlegeart = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
								}
							}
							pipe.IsGenerated = true;
							pipe.Room = roomThrough;
							pipe.ConnectionThrough = null;
							pipe.Vorlauf = vlThroughRoom[roomThrough];
							pp.Product.PlannedConnectionPipes.Add(pipe);
						}
						if (rlThroughRoom.ContainsKey(roomThrough)) {
							ConnectionPipe pipe = new ConnectionPipe();
							if (previousRoomPipesRl.ContainsKey(roomThrough)) {
								pipe.PipeType = previousRoomPipesRl[roomThrough].PipeType;
								pipe.Insulation = previousRoomPipesRl[roomThrough].Insulation;
								pipe.Verlegeart = previousRoomPipesRl[roomThrough].Verlegeart;
							} else if (previousRestPipeRl != null) {
								pipe.PipeType = previousRestPipeRl.PipeType;
								pipe.Insulation = previousRestPipeRl.Insulation;
								pipe.Verlegeart = previousRestPipeRl.Verlegeart;
							} else if (previousRoomPipesVl.ContainsKey(roomThrough)) {
								pipe.PipeType = previousRoomPipesVl[roomThrough].PipeType;
								pipe.Insulation = previousRoomPipesVl[roomThrough].Insulation;
								pipe.Verlegeart = previousRoomPipesVl[roomThrough].Verlegeart;
							} else if (previousRestPipeVl != null) {
								pipe.PipeType = previousRestPipeVl.PipeType;
								pipe.Insulation = previousRestPipeVl.Insulation;
								pipe.Verlegeart = previousRestPipeVl.Verlegeart;
							} else {
								pipe.PipeType = pp.Product.DefaultPipeType;
								if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
									pipe.Insulation = ConnectionPipe.InsulationEnum.IN_VL_RL;
									pipe.Verlegeart = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
								}
							}
							pipe.IsGenerated = true;
							pipe.Room = roomThrough;
							pipe.ConnectionThrough = null;
							pipe.Ruecklauf = rlThroughRoom[roomThrough];
							pp.Product.PlannedConnectionPipes.Add(pipe);
						}
					}
					if (vlRest > 0) {
						ConnectionPipe pipe = new ConnectionPipe();
						if (previousRestPipeVl != null) {
							pipe.PipeType = previousRestPipeVl.PipeType;
							pipe.Insulation = previousRestPipeVl.Insulation;
							pipe.Verlegeart = previousRestPipeVl.Verlegeart;
						} else if (previousRestPipeRl != null) {
							pipe.PipeType = previousRestPipeRl.PipeType;
							pipe.Insulation = previousRestPipeRl.Insulation;
							pipe.Verlegeart = previousRestPipeRl.Verlegeart;
						} else {
							pipe.PipeType = pp.Product.DefaultPipeType;
							if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
								pipe.Insulation = ConnectionPipe.InsulationEnum.IN_VL_RL;
								pipe.Verlegeart = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
							}
						}
						pipe.IsGenerated = true;
						pipe.Vorlauf = vlRest;
						pp.Product.PlannedConnectionPipes.Add(pipe);
					}
					if (rlRest > 0) {
						ConnectionPipe pipe = new ConnectionPipe();
						if (previousRestPipeRl != null) {
							pipe.PipeType = previousRestPipeRl.PipeType;
							pipe.Insulation = previousRestPipeRl.Insulation;
							pipe.Verlegeart = previousRestPipeRl.Verlegeart;
						} else if (previousRestPipeVl != null) {
							pipe.PipeType = previousRestPipeVl.PipeType;
							pipe.Insulation = previousRestPipeVl.Insulation;
							pipe.Verlegeart = previousRestPipeVl.Verlegeart;
						} else {
							pipe.PipeType = pp.Product.DefaultPipeType;
							if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
								pipe.Insulation = ConnectionPipe.InsulationEnum.IN_VL_RL;
								pipe.Verlegeart = ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH;
							}
						}
						pipe.IsGenerated = true;
						pipe.Ruecklauf = rlRest;
						pp.Product.PlannedConnectionPipes.Add(pipe);
					}
				}
			}
		}

        public bool ShowPlanBackground {
            get { return Europlan.Common.Product.ShowPlanInBackground; }
        }
    }
}
