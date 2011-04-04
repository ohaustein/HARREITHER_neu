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
			KDM_DEL_CONNECTION
		}

		public ConnectionPlanner() {
			InitializeComponent();
			this.contextMenu.Items[0].Text = "Vorlauf";
			this.contextMenu.Items[1].Text = "Rücklauf";
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
		private Dictionary<Distributor, DistributorPositioner> distributorsInFloor = new Dictionary<Distributor, DistributorPositioner>();
		private Floor floor = null;

		public event EventHandler<EventArgs> ModeChanged;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Floor Floor {
			get { return this.floor; }
			set {
				this.floor = value;
				this.distributorsInFloor.Clear();
				if (this.floor == null) {
					this.Plan = null;
				} else {
					this.Plan = floor.AssociatedPlan;
					DistributorPositioner positioner;
					foreach (Distributor d in this.GetAllDistributors()) {
						positioner = new DistributorPositioner();
						positioner.Floor = floor;
						positioner.Distributor = d;
						positioner.ConnectedPlanPanel = this.connectedPlanPanel;
						this.distributorsInFloor.Add(d, positioner);
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
						foreach (Product p in this.GetAllProducts()) {
							if (p is ModulKlimaBodenProduct) {
								ModulKlimaBodenPlanner pp = new ModulKlimaBodenPlanner();
								pp.Product = p as ModulKlimaBodenProduct;
								pp.ConnectedPlanPanel = this.connectedPlanPanel;
								pp.DrawExpansionGaps = false;
								pp.HighlightRoomCoordinates = false;
								productsInFloor.Add(p, pp);
							}
						}
					}
				}
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
					foreach (Product p in this.GetAllProducts()) {
						if (p is ModulKlimaBodenProduct) {
							ModulKlimaBodenPlanner pp = new ModulKlimaBodenPlanner();
							pp.Product = p as ModulKlimaBodenProduct;
							pp.ConnectedPlanPanel = this.connectedPlanPanel;
							pp.DrawExpansionGaps = false;
							pp.HighlightRoomCoordinates = false;
							productsInFloor.Add(p, pp);
						}
					}
				}
			}
		}

		private void connectedPlanPanel_KeyDown(object sender, KeyEventArgs e) {
			if (this.mode == ConnectionMode.KDM_ADD_CONNECTION) {
				if (e.KeyCode == Keys.Escape) {
					this.newConnectionStart = null;
					this.newConnectionVertices = null;
				} else if (e.KeyCode == Keys.Back) {
					if (this.newConnectionVertices == null || this.newConnectionVertices.Count < 2) {
						this.newConnectionStart = null;
						this.newConnectionVertices = null;
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
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl);
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
			List<Distributor> distributors = new List<Distributor>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					distributors.AddRange(floor.GetAllAvailableDistributors());
				}
			}
			return distributors;
		}

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			if (this.Plan != null) {
				foreach (KeyValuePair<Distributor, DistributorPositioner> kvp in this.distributorsInFloor) {
					kvp.Value.PaintAfterPlanPannel(g, additionalTransformation, mousePositionInPlan, mousePositionInControl);
				}
				if (drawExpansionGaps) {
					foreach (Floor floor in this.GetAllFloors()) {
						foreach (Segment2D expansionGap in floor.ExpansionGaps) {
							Point2D start = additionalTransformation.TransformTo2D(expansionGap.Start);
							Point2D end = additionalTransformation.TransformTo2D(expansionGap.End);
							g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
						}
					}
				}
				foreach (KeyValuePair<Product, IProductPlanner> kvp in this.productsInFloor) {
					kvp.Value.PaintAfterPlanPannel(g, additionalTransformation, mousePositionInPlan, mousePositionInControl);
					foreach (GraphicalProductConnection connection in kvp.Key.Connections) {
						connection.Draw(g, additionalTransformation, connection.Vorlauf ? Color.Red : Color.Blue, this.Plan.Measure.Value);
					}
				}

				if (this.mode == ConnectionMode.KDM_ADD_CONNECTION) {
					if (this.possibleConnections != null) {
						foreach (PossibleConnection pc in this.possibleConnections) {
							List<PointF> connectionPoly = new List<PointF>();
							Point2D tmp;
							foreach (Point2D point in pc.ConnectionArea) {
								tmp = additionalTransformation.TransformTo2D(point);
								connectionPoly.Add(new PointF((float)tmp.X, (float)tmp.Y));
							}
							if (pc.PossibleInput && pc.Product != null || pc.PossibleOutput && pc.Distributor != null) {
								if (pc.PossibleOutput && pc.Product != null || pc.PossibleInput && pc.Distributor != null) {
									PointF[] poly = connectionPoly.ToArray();
									//g.DrawPolygon(new Pen(Color.LightGray), poly);
									g.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.LightGray)), poly);
								} else {
									PointF[] poly = connectionPoly.ToArray();
									//g.DrawPolygon(new Pen(Color.Red), poly);
									g.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.Red)), poly);
								}
							} else if (pc.PossibleOutput && pc.Product != null || pc.PossibleInput && pc.Distributor != null) {
								PointF[] poly = connectionPoly.ToArray();
								//g.DrawPolygon(new Pen(Color.Blue), poly);
								g.FillPolygon(new SolidBrush(Color.FromArgb(128, Color.Blue)), poly);
							}
						}
					}

					Pen p = new Pen(Color.Green, (float)(0.021 * this.Plan.Measure.Value * additionalTransformation.M00));
					if (this.newConnectionVertices != null && this.newConnectionVertices.Count > 0) {
						PointF oldVertex = PointF.Empty;
						Point2D newVertex2D;
						PointF newVertex;
						bool first = true;
						foreach (Point2D vertex in this.newConnectionVertices) {
							newVertex2D = additionalTransformation.TransformTo2D(vertex);
							newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
							if (first) {
								first = false;
							} else {
								g.DrawLine(p, oldVertex, newVertex);
							}
							oldVertex = newVertex;
						}
						foreach (Point2D vertex in this.nextConnectionPoints) {
							newVertex2D = additionalTransformation.TransformTo2D(vertex);
							newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
							g.DrawLine(p, oldVertex, newVertex);
							oldVertex = newVertex;
						}
					}
				}
			}
		}

		private List<Point2D> newConnectionVertices = null;
		private Nullable<PossibleConnection> newConnectionStart = null;
		private bool newConnectionStartAtOutput = true;
		//private bool newConnectionLastSegmentHorizontal = true;

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			bool redraw = false;
			if (this.mode == ConnectionMode.KDM_ADD_CONNECTION && button != MouseButtons.Middle) {
				if (this.newConnectionStart == null) {
					Nullable<PossibleConnection> connection = null;
					foreach (PossibleConnection pc in this.possibleConnections) {
						if (pc.ConnectionArea.IsInside(planPoint)) {
							connection = pc;
							break;
						}
					}
					if (connection != null) {
						if (connection.Value.PossibleInput && connection.Value.PossibleOutput) {
							this.contextMenu.Show(this.connectedPlanPanel as Control, pointInControl);
						} else if (connection.Value.PossibleInput) {
							this.AddConnection(connection.Value, true);
						} else if (connection.Value.PossibleOutput) {
							this.AddConnection(connection.Value, false);
						}
					}
				} else {
					Nullable<PossibleConnection> endConnection;
					this.newConnectionVertices.AddRange(this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endConnection));
					this.nextConnectionPoints.Clear();
					if (endConnection != null) {
						int index;
						// TODO check if connection is valid!
						PossibleConnection productConnection;
						PossibleConnection distributorConnection;
						bool ok = false;
						bool vorlauf = true;
						if (this.newConnectionStart.Value.Product != null && endConnection.Value.Distributor != null) {
							productConnection = this.newConnectionStart.Value;
							distributorConnection = endConnection.Value;
							vorlauf = !this.newConnectionStartAtOutput;
							ok = true;
						} else if (endConnection.Value.Product != null && this.newConnectionStart.Value.Distributor != null) {
							distributorConnection = this.newConnectionStart.Value;
							productConnection = endConnection.Value;
							vorlauf = this.newConnectionStartAtOutput;
							ok = true;
						} else {
							productConnection = new PossibleConnection();
							distributorConnection = new PossibleConnection();
						}
						if (ok) {
							productConnection.Product.Connections.Add(new GraphicalProductConnection(Project.Instance.GetPlannedProduct(productConnection.Product), distributorConnection.Distributor, this.newConnectionVertices, productConnection.Circuit, distributorConnection.DistributorIndex, vorlauf));
							this.newConnectionVertices = null;
							this.newConnectionStart = null;
						}
					}
					redraw = true;
				}
			} else if (this.Mode == ConnectionMode.KDM_DEL_CONNECTION) {
				double bestDist = double.MinValue;
				GraphicalProductConnection bestConnection = null;
				Product bestProduct = null;
				foreach (Room room in this.floor.Rooms) {
					foreach (PlannedProduct pp in room.PlannedProducts) {
						foreach (GraphicalProductConnection conn in pp.Product.Connections) {
							double dist = conn.HitTest(planPoint, this.Plan.Measure.Value * 0.025);
							if (dist <= 0 && dist > bestDist) {
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
			}
			return redraw;
		}

		private void AddConnection(PossibleConnection connection, bool input) {
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
		List<PossibleConnection> possibleConnections = null;

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			bool redraw = false;
			if (this.Mode == ConnectionMode.KDM_ADD_CONNECTION) {
				// TODO
				List<PossibleConnection> oldPossibleConnections = possibleConnections;
				possibleConnections = new List<PossibleConnection>();
				if (selectedDistributor == null) {
					foreach (Distributor d in this.GetAllDistributors()) {
						possibleConnections.AddRange(d.GetPossibleConnections(this.newConnectionStart == null || this.newConnectionStartAtOutput, this.newConnectionStart == null || !this.newConnectionStartAtOutput, this.Plan.Measure.Value, this.Plan.InvertYAxis, planPoint, selectedProduct, selectedCircuit, this.floor));
					}
				}
				if (selectedProduct == null) {
					foreach (Product p in this.GetAllProducts()) {
						possibleConnections.AddRange(p.GetPossibleConnections(this.newConnectionStart == null || this.newConnectionStartAtOutput, this.newConnectionStart == null || !this.newConnectionStartAtOutput, this.Plan.Measure.Value, this.Plan.InvertYAxis, planPoint, selectedDistributor, selectedDistributorNr));
					}
				}
				//redraw = true;

				if (oldPossibleConnections == null) {
					if (possibleConnections.Count == 0) {
						redraw = redraw || this.newConnectionStart != null;
					} else {
						redraw = true;
					}
				} else {
					foreach (PossibleConnection pc in oldPossibleConnections) {
						if (!possibleConnections.Contains(pc)) {
							redraw = true;
							break;
						}
						possibleConnections.Remove(pc);
					}
				}
				if (this.newConnectionStart != null) {
					Nullable<PossibleConnection> tmp;
					this.nextConnectionPoints = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out tmp);
				} else {
					this.nextConnectionPoints = new List<Point2D>();
				}
				redraw = redraw || possibleConnections.Count > 0 || this.newConnectionStart != null;
			}
			return redraw;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		internal bool ShiftPressed {
			get { return (Control.ModifierKeys & (Keys.Shift | Keys.ShiftKey | Keys.LShiftKey | Keys.RShiftKey)) != Keys.None; }
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			return false;
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

		}

		private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out Nullable<PossibleConnection> endConnection) {
			List<Point2D> nextConnectionPoints = new List<Point2D>();
			endConnection = null;
			int index;
			if (this.newConnectionStartAtOutput) {
				foreach (PossibleConnection pc in this.possibleConnections) {
					if (pc.ConnectionArea.IsInside(mousePoint)) {
						// TODO check if this connection is valid!
						endConnection = pc;
						break;
					}
				}
			} else {
				foreach (PossibleConnection pc in this.possibleConnections) {
					if (pc.ConnectionArea.IsInside(mousePoint)) {
						// TODO check if this connection is valid!
						endConnection = pc;
						break;
					}
				}
			}
			// TODO check if the connection is valid!
			/*if (this.product.GetCircuitForModul(this.newConnectionStart, out index).GetAllLinkedModules(this.newConnectionStart).Contains(endModule)) {
				endModule = null;
			}*/
			if (endConnection == null) {
				bool horizontal;
				nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, this.newConnectionStart.Value.Rotation, out horizontal));
			} else {
				Point2D connectionPoint = endConnection.Value.ConnectionPoint;
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
	}
}
