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
	public partial class ModulKlimaBodenPlanner : Component, IProductPlanner {

		public enum VerlegungsAbstand {
			VA_DICHT = 0,
			VA_MODULIEREND = 1,
			VA_MODULIEREND_X2 = 2
		}

		private Color[] circuitColors = new Color[] {
			Color.FromArgb(192, 0, 0),
			Color.FromArgb(0, 128, 0),
			Color.FromArgb(0, 0, 192),
			Color.FromArgb(255, 128, 0),
			Color.FromArgb(128, 128, 0),
			Color.FromArgb(0, 128, 255)
		};

		public enum KlimaBodenMode {
			KDM_NONE,
			KDM_CONSTRUCTION,
			KDM_LAYOUT_ADD_AREA,
			KDM_LAYOUT_ADD_AREA_FINISH,
			KDM_LAYOUT_ADD_AREA_PICK_REFERENCE,
			KDM_PICK_MODULE,
			KDM_CONNECTIONS,
			KDM_DEL_CONNECTION
		}

		public class UpdateNewCountArgs : EventArgs {
			public int count;

			public UpdateNewCountArgs(int count) {
				this.count = count;
			}
		}

		public event EventHandler<UpdateNewCountArgs> UpdateNewCount;

		public delegate void AddModuleDelegate(double x, double y, double rotation, out bool added, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, bool bottomUp, out KlimaFlaechenModul addedModul, out ModulBodenCircuit circuitOfModul, bool fits);

		public ModulKlimaBodenPlanner() {
			InitializeComponent();
		}

		public ModulKlimaBodenPlanner(IContainer container) {
			container.Add(this);


			InitializeComponent();
		}

		private ModulKlimaBodenProduct product;
		private KlimaBodenMode mode = KlimaBodenMode.KDM_NONE;
		private Cursor customCursor = null;
		private bool highlightRoomCoordinates = true;
		private bool drawExpansionGaps = true;
		private double newModulesRotation = 0.0;
		private VerlegungsAbstand newModulesXDicht = VerlegungsAbstand.VA_MODULIEREND;
		private VerlegungsAbstand newModulesYDicht = VerlegungsAbstand.VA_MODULIEREND;
		private double newModulesOffsetX = 0.0;
		private double newModulesOffsetY = 0.0;
		private bool newModulesConnectHorizontal = false;
		private KlimaFlaechenModul.ModulOrientationEnum newModulesStartingOrientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
		private static double modulierendDistance = 0.1;

		public event EventHandler<EventArgs> ModeChanged;
		public event EventHandler<EventArgs> ListsNeedUpdate;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulKlimaBodenProduct Product {
			get { return this.product; }
			set {
				this.product = value;
				if (this.ConnectedPlanPanel != null) {
					if (this.product == null || this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null) {
						this.ConnectedPlanPanel.Plan = null;
					} else {
						this.ConnectedPlanPanel.Plan = this.product.AssociatedRoom.AssociatedPlan;
						Room room = this.product.AssociatedRoom;
						if (room.PlanSettingX.HasValue &&
							room.PlanSettingY.HasValue &&
							room.PlanSettingScale.HasValue &&
							room.PlanSettingAngle.HasValue) {
							this.ConnectedPlanPanel.SetPlanTransformations(room.PlanSettingScale.Value, room.PlanSettingX.Value, room.PlanSettingY.Value, room.PlanSettingAngle.Value);
						}
					}
				}
				this.connectionDrawer.Product = this.product;
				//this.connectionDrawer.Floor = (this.product != null && this.product.AssociatedRoom != null) ? this.product.AssociatedRoom.AssociatedFloor : null;
			}
		}

		public KlimaBodenMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				if (this.mode != KlimaBodenMode.KDM_PICK_MODULE && this.HighlightModules != null) {
					this.HighlightModules = null;
				}
				if (this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA && this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH && this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
					this.layoutAddArea = null;
					if (this.connectedPlanPanel != null) {
						this.connectedPlanPanel.InvalidateGraphics();
					}
				}
				if (this.mode != KlimaBodenMode.KDM_CONNECTIONS) {
					this.possibleAnbindungspunkte = new List<GraphicalConnectionAnbindungsPunkt>();
				}
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
				}
				this.connectedPlanPanel = value;
				if (this.connectedPlanPanel != null) {
					this.connectedPlanPanel.KeyDown += new KeyEventHandler(connectedPlanPanel_KeyDown);
				}
			}
		}

		private void connectedPlanPanel_KeyDown(object sender, KeyEventArgs e) {
			if (this.mode == KlimaBodenMode.KDM_PICK_MODULE) {
				//KeyDown(e.KeyCode, this.highlightModules);
			} else if (this.mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH || this.mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
				if (e.KeyCode == Keys.Escape) {
					this.layoutAddArea = null;
					this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
					this.connectedPlanPanel.InvalidateGraphics();
				}
			} else if (this.mode == KlimaBodenMode.KDM_CONNECTIONS) {
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

		private bool KeyDown(Keys key, List<KlimaFlaechenModul> modules) {
			if (key == Keys.Delete && modules != null) {
				if (modules.Count > 0 && this.product.Connections != null && this.product.Connections.Count > 0) {
					bool ask = false;
					foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
						bool empty = true;
						foreach (KlimaFlaechenModul m in c.Row.List) {
							if (!modules.Contains(m)) {
								empty = false;
								break;
							}
						}
						if (empty) {
							ask = true;
							break;
						}
					}
					if (ask) {
						if (MessageBox.Show("Wenn Sie einen Heizkreis löschen werden die bestehenden Anbindeleitungen an den Verteiler gelöscht. Wollen Sie die Anbindeleitungen löschen?", "Heizkreis löschen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
							return true;
						}
						this.product.Connections.Clear();
					}
				}
				List<Circuit> emptyCircuits = new List<Circuit>();
				foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
					foreach (KlimaFlaechenModul kfm in modules) {
						if (c.Row.List.Contains(kfm)) {
							KlimaFlaechenModulVerbindung link = kfm.GetInputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
							if (link != null) {
								c.Links.Remove(link);
							}
							link = kfm.GetOutputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
							if (link != null) {
								c.Links.Remove(link);
							}
							c.Row.List.Remove(kfm);
						}
					}
					if (c.Row.List.Count == 0) {
						emptyCircuits.Add(c);
					}
				}
				foreach (Circuit emptyCircuit in emptyCircuits) {
					this.product.PlannedCircuits.Remove(emptyCircuit);
				}
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				if (this.ModuleSelected != null) {
					this.ModuleSelected(this, EventArgs.Empty);
				}
				this.ConnectedPlanPanel.InvalidateGraphics();
				if (this.ListsNeedUpdate != null) {
					this.ListsNeedUpdate(this, EventArgs.Empty);
				}
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				return true;
			}
			return false;
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl, false);
		}

		private List<GraphicalConnectionAnbindungsPunkt> possibleAnbindungspunkte = new List<GraphicalConnectionAnbindungsPunkt>();

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl, bool export) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {

				if (!export) {
					// paint anbindeleitungen and distributors
					this.connectionDrawer.Paint(g, additionalTransformation);
				}

				// generate clip for product
				GraphicsPath path = new GraphicsPath();
				List<PointF> transformedPoints = new List<PointF>();
				foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
				}
				path.AddPolygon(transformedPoints.ToArray());
				Region clipDisabled = new Region();
				clipDisabled.MakeInfinite();
				clipDisabled.Exclude(path);
				path.Dispose();

				// select color for painting depending depending on the background color of the plan
				Color c = Color.Black;
				if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
					c = Color.White;
				}
				Brush b = new SolidBrush(c);
				b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));

				if (highlightRoomCoordinates && !export) {
					// gray out all except the room
					g.FillRegion(b, clipDisabled);
				}

				// paint construction
				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.Paint(g, this.Mode);
					}
				}

				if (this.layoutAddArea != null) {
					// calculate and draw rectangle that is currently selected for adding modules
					PointF[] drawArea = new PointF[this.layoutAddArea.Count];
					for (int i = 0; i < this.layoutAddArea.Count; i++) {
						Point2D tmp = additionalTransformation.TransformTo2D(this.layoutAddArea[i]);
						drawArea[i] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					g.DrawPolygon(Pens.Red, drawArea);

					// draw modules that are currently being added
					int count = 0;
					this.AddModulesForLayoutArea(delegate(double x, double y, double rotation, out bool added, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, bool bottomUp, out KlimaFlaechenModul addedModul, out ModulBodenCircuit circuitOfModul, bool fits) {
						added = this.TryDrawModule(g, additionalTransformation, x, y, rotation, orientation, bottomUp, fits ? Color.Green : Color.FromArgb(63, Color.Red));
						if (fits) {
							count++;
						}
						addedModul = null;
						circuitOfModul = null;
					}, true, false);
					if (this.UpdateNewCount != null) {
						this.UpdateNewCount(this, new UpdateNewCountArgs(count));
					}
				}

				// paint unused areas of room
				if (this.product.AssociatedRoom.RoomUnusedAreaCoordinates != null) {
					List<PointF> unusedPoints = new List<PointF>();
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
						foreach (Point2D point in unusedArea) {
							Point2D tmp = additionalTransformation.TransformTo2D(point);
							unusedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
						}
						PointF[] pointArray = unusedPoints.ToArray();
						g.DrawPolygon(new Pen(c), pointArray);
						g.FillPolygon(b, pointArray);
						unusedPoints.Clear();
					}
				}

				// paint expansion gaps
				if (drawExpansionGaps) {
					foreach (Segment2D expansionGap in this.Product.AssociatedRoom.AssociatedFloor.ExpansionGaps) {
						Point2D start = additionalTransformation.TransformTo2D(expansionGap.Start);
						Point2D end = additionalTransformation.TransformTo2D(expansionGap.End);
						g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					}
				}

				// draw rectangle for module selection
				if (this.dragStartedInPlan.HasValue && this.dragEndedInPlan.HasValue) {
					Matrix transform = g.Transform;
					g.Transform = new Matrix();
					g.DrawPolygon(new Pen(Color.Red), new Point[] { this.dragStartedInControl.Value, new Point(this.dragStartedInControl.Value.X, this.dragEndedInControl.Value.Y), this.dragEndedInControl.Value, new Point(this.dragEndedInControl.Value.X, this.dragStartedInControl.Value.Y) });
					g.Transform = transform;
				}

				// paint modules
				List<KlimaFlaechenModul> selectedModules = this.GetAllSelectedModules();
				foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
					foreach (KlimaFlaechenModul modul in circuit.Row.List) {
						this.DrawModule(modul.ModulType, modul.Orientation, new Point2D(modul.GraphPosX, modul.GraphPosY), modul.GraphRotation, additionalTransformation, g, modul.GraphBottomUp, selectedModules.Contains(modul), circuit.CircuitColor, modul == this.hoveredModul && this.hoverInput, modul == this.hoveredModul && this.hoverOutput);
					}
					if (circuit.Links != null) {
						foreach (KlimaFlaechenModulVerbindung link in circuit.Links) {
							link.Draw(g, additionalTransformation, circuit.CircuitColor, this.product.AssociatedRoom.AssociatedPlan.Measure.Value);
						}
					}
				}

				if (this.mode == KlimaBodenMode.KDM_CONNECTIONS) {
					if (hoverAnbindungen) {
						// paint possible connections to anbindeleitung
						g.ResetClip();
						Brush bi = new SolidBrush(Color.FromArgb(127, Color.Red));
						Brush bo = new SolidBrush(Color.FromArgb(127, Color.Blue));
						if (!this.newConnectionStartAtOutput) {
							//foreach (GraphicalConnectionAnbindungsPunkt anbindung in this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, this.newConnectionCircuitDistributorIndex, mousePositionInPlan)) {
							foreach (GraphicalConnectionAnbindungsPunkt anbindung in this.possibleAnbindungspunkte) {
								PointF[] points = new PointF[anbindung.Area.Count];
								for (int i = 0; i < anbindung.Area.Count; i++) {
									Point2D tmp = additionalTransformation.TransformTo2D(anbindung.Area[i]);
									points[i] = new PointF((float)tmp.X, (float)tmp.Y);
								}
								g.FillPolygon(bi, points);
								g.DrawPolygon(Pens.Red, points);
							}
						} else {
							//foreach (GraphicalConnectionAnbindungsPunkt anbindung in this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, this.newConnectionCircuitDistributorIndex, mousePositionInPlan)) {
							foreach (GraphicalConnectionAnbindungsPunkt anbindung in this.possibleAnbindungspunkte) {
								PointF[] points = new PointF[anbindung.Area.Count];
								for (int i = 0; i < anbindung.Area.Count; i++) {
									Point2D tmp = additionalTransformation.TransformTo2D(anbindung.Area[i]);
									points[i] = new PointF((float)tmp.X, (float)tmp.Y);
								}
								g.FillPolygon(bo, points);
								g.DrawPolygon(Pens.Blue, points);
							}
						}
					}

					// paint connection that is currently beeing added
					Pen p = new Pen(Color.Green, (float)(0.021 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * additionalTransformation.M00));
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
		private KlimaFlaechenModul newConnectionStart = null;
		private bool newConnectionStartAtOutput = true;
		private ModulBodenCircuit newConnectionCircuit = null;
		private int newConnectionCircuitDistributorIndex = -1;
		private List<int> newConnectionIgnoreDistributorIndices = null;

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			bool redraw = false;
			if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
				Dictionary<KlimaFlaechenModul, Polygon2D> moduleAreas = this.GetModuleAreas();
				KlimaFlaechenModul pickedModul = null;
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> kvp in moduleAreas) {
					if (kvp.Value.IsInside(planPoint)) {
						pickedModul = kvp.Key;
						break;
					}
				}

				if (pickedModul != null) {
					Matrix3D rotation = Transformation3D.Rotate(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
					Matrix3D invRotation = rotation.GetInverse();

					double top = double.MaxValue;
					double bottom = double.MinValue;
					double left = double.MaxValue;
					double right = double.MinValue;
					foreach (Point2D point in this.layoutAddArea) {
						Point2D rotatedPoint = invRotation.Transform(point);
						if (rotatedPoint.X < left) {
							left = rotatedPoint.X;
						}
						if (rotatedPoint.X > right) {
							right = rotatedPoint.X;
						}
						if (rotatedPoint.Y < top) {
							top = rotatedPoint.Y;
						}
						if (rotatedPoint.Y > bottom) {
							bottom = rotatedPoint.Y;
						}
					}
					Point2D referencePoint = invRotation.Transform(new Point2D(pickedModul.GraphPosX, pickedModul.GraphPosY));

					double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

					double stepX = width;
					stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesXDicht;
					double stepY = height;
					stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesYDicht;

					double refLeft = referencePoint.X;
					while (refLeft - stepX > left) {
						refLeft -= stepX;
					}
					while (refLeft < left) {
						refLeft += stepX;
					}
					double refTop = referencePoint.Y;
					while (refTop - stepY > top) {
						refTop -= stepY;
					}
					while (refTop < top) {
						refTop += stepY;
					}
					this.NewModulesOffsetX = refLeft - left;
					this.NewModulesOffsetY = refTop - top;

					this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
					if (this.connectedPlanPanel != null) {
						this.connectedPlanPanel.InvalidateGraphics();
					}
					if (this.ModeChanged != null) {
						this.ModeChanged(this, EventArgs.Empty);
					}
				}
			} else if (this.mode == KlimaBodenMode.KDM_CONNECTIONS) {
				if (this.newConnectionStart == null) {
					foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> input in this.GetModuleInputs()) {
						if (input.Value.IsInside(planPoint)) {
							int tmp;
							ModulBodenCircuit circuit = this.product.GetCircuitForModul(input.Key, out tmp);
							if (input.Key.GetInputLink(circuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null) {
								this.newConnectionStart = input.Key;
								this.newConnectionStartAtOutput = false;
								this.newConnectionCircuit = circuit;
								this.newConnectionCircuitDistributorIndex = this.newConnectionCircuit.GetDistributorConnectionIndex(true, true);
								this.newConnectionIgnoreDistributorIndices = new List<int>();
								foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
									int index = c.GetDistributorConnectionIndex(true, false);
									if (index >= 0) {
										this.newConnectionIgnoreDistributorIndices.Add(index);
									}
								}
							}
							break;
						}
					}
					if (this.newConnectionStart == null) {
						foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> output in this.GetModuleOutputs()) {
							if (output.Value.IsInside(planPoint)) {
								int tmp;
								ModulBodenCircuit circuit = this.product.GetCircuitForModul(output.Key, out tmp);
								if (output.Key.GetOutputLink(circuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null) {
									this.newConnectionStart = output.Key;
									this.newConnectionStartAtOutput = true;
									this.newConnectionCircuit = circuit;
									this.newConnectionCircuitDistributorIndex = this.newConnectionCircuit.GetDistributorConnectionIndex(true, true);
									this.newConnectionIgnoreDistributorIndices = new List<int>();
									foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
										int index = c.GetDistributorConnectionIndex(false, true);
										if (index >= 0) {
											this.newConnectionIgnoreDistributorIndices.Add(index);
										}
									}
								}
								break;
							}
						}
					}
					if (this.newConnectionStart != null) {
						this.newConnectionVertices = new List<Point2D>();
						if (this.newConnectionStartAtOutput) {
							this.newConnectionVertices.Add(this.newConnectionStart.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product));
						} else {
							this.newConnectionVertices.Add(this.newConnectionStart.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product));
						}
					}
				} else {
					KlimaFlaechenModul endModul;
					GraphicalConnectionAnbindungsPunkt endAnbindung;
					this.newConnectionVertices.AddRange(this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endModul, out endAnbindung));
					this.nextConnectionPoints.Clear();
					if (endModul != null) {
						int index;
						ModulBodenCircuit c = this.product.GetCircuitForModul(this.newConnectionStart, out index);
						if (c.Links == null) {
							c.Links = new List<KlimaFlaechenModulVerbindung>();
						}
						if (this.newConnectionStartAtOutput) {
							c.Links.Add(new KlimaFlaechenModulVerbindung(this.newConnectionStart, endModul, this.newConnectionVertices, c, Project.Instance.GetPlannedProduct(this.product)));
						} else {
							c.Links.Add(new KlimaFlaechenModulVerbindung(endModul, this.newConnectionStart, this.newConnectionVertices, c, Project.Instance.GetPlannedProduct(this.product)));
						}
						this.newConnectionVertices = null;
						this.newConnectionStart = null;
					} else if (endAnbindung != null) {
						int index;
						if (endAnbindung.NewProductConnection != null) {
							if (this.product.Connections == null) {
								this.product.Connections = new List<GraphicalProductConnection>();
							}
							this.product.Connections.Add(endAnbindung.NewProductConnection);
						}
						ModulBodenCircuit c = this.product.GetCircuitForModul(this.newConnectionStart, out index);
						if (c.Links == null) {
							c.Links = new List<KlimaFlaechenModulVerbindung>();
						}
						if (this.newConnectionStartAtOutput) {
							c.Links.Add(new KlimaFlaechenModulVerbindung(this.newConnectionStart, true, this.newConnectionVertices, c, Project.Instance.GetPlannedProduct(this.product), endAnbindung.Index));
						} else {
							c.Links.Add(new KlimaFlaechenModulVerbindung(this.newConnectionStart, false, this.newConnectionVertices, c, Project.Instance.GetPlannedProduct(this.product), endAnbindung.Index));
						}
						this.newConnectionVertices = null;
						this.newConnectionStart = null;
					}
					redraw = true;
				}
			} else if (this.Mode == KlimaBodenMode.KDM_DEL_CONNECTION) {
				double bestDist = double.MaxValue;
				KlimaFlaechenModulVerbindung bestLink = null;
				ModulBodenCircuit bestCircuit = null;
				foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
					foreach (KlimaFlaechenModulVerbindung link in circuit.Links) {
						double dist = link.GetDistance(planPoint);
						if (dist < bestDist && dist <= this.product.AssociatedRoom.AssociatedPlan.Measure.Value * 0.025) {
							bestDist = dist;
							bestLink = link;
							bestCircuit = circuit;
						}
					}
				}
				if (bestLink != null) {
					bestCircuit.Links.Remove(bestLink);
					bool stillConnected = false;
					foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
						foreach (KlimaFlaechenModulVerbindung link in c.Links) {
							if (link.StartConnectedToAnbindung || link.EndConnectedToAnbindung) {
								stillConnected = true;
								break;
							}
						}
						if (stillConnected) {
							break;
						}
					}
					if (!stillConnected) {
						List<GraphicalProductConnection> connectionsToDelete = new List<GraphicalProductConnection>();
						foreach (GraphicalProductConnection conn in this.product.Connections) {
							if (conn.Automatic) {
								connectionsToDelete.Add(conn);
							}
						}
						foreach (GraphicalProductConnection conn in connectionsToDelete) {
							this.product.Connections.Remove(conn);
						}
					}
					redraw = true;
				}
			}
			return redraw;
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

		private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out KlimaFlaechenModul endModule, out GraphicalConnectionAnbindungsPunkt endAnbindungsPunkt) {
			List<Point2D> nextConnectionPoints = new List<Point2D>();
			endModule = null;
			endAnbindungsPunkt = null;
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
				if (endModule == null) {
					//foreach (GraphicalConnectionAnbindungsPunkt output in this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, this.newConnectionCircuitDistributorIndex, mousePoint)) {
					foreach (GraphicalConnectionAnbindungsPunkt output in this.possibleAnbindungspunkte) {
						if (output.Area.IsInside(mousePoint)) {
							endAnbindungsPunkt = output;
							break;
						}
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
				if (endModule == null) {
					//foreach (GraphicalConnectionAnbindungsPunkt input in this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, this.newConnectionCircuitDistributorIndex, mousePoint)) {
					foreach (GraphicalConnectionAnbindungsPunkt input in this.possibleAnbindungspunkte) {
						if (input.Area.IsInside(mousePoint)) {
							endAnbindungsPunkt = input;
							break;
						}
					}
				}
			}
			if (this.product.GetCircuitForModul(this.newConnectionStart, out index).GetAllLinkedModules(this.newConnectionStart).Contains(endModule)) {
				endModule = null;
			}
			if (endModule != null) {
				Point2D connectionPoint = this.newConnectionStartAtOutput ? endModule.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product) : endModule.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
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
			} else if (endAnbindungsPunkt != null) {
				Point2D connectionPoint = endAnbindungsPunkt.Point;
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
			} else {
				bool horizontal;
				nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, this.newConnectionStart.GraphRotation, out horizontal));
			}
			return nextConnectionPoints;
		}

		private KlimaFlaechenModul hoveredModul = null;
		private bool hoverInput = false;
		private bool hoverOutput = false;
		private List<Point2D> nextConnectionPoints = new List<Point2D>();
		private bool hoverAnbindungen = false;

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
				if (this.product.GraphConstruction != null) {
					if (this.product.GraphConstruction.HitTest(planPoint, pointInControl)) {
						this.customCursor = this.product.GraphConstruction.PickCursor;
						this.ConnectedPlanPanel.PlanCursor = this.product.GraphConstruction.PickCursor;
					} else {
						this.customCursor = Cursors.Default;
						this.ConnectedPlanPanel.PlanCursor = Cursors.Default;
					}
				}
			} else if (this.Mode == KlimaBodenMode.KDM_CONNECTIONS) {

				if (this.newConnectionStart != null) {
					if (!this.newConnectionStartAtOutput) {
						this.possibleAnbindungspunkte = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, this.newConnectionCircuitDistributorIndex, this.newConnectionIgnoreDistributorIndices, planPoint);
					} else {
						this.possibleAnbindungspunkte = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, this.newConnectionCircuitDistributorIndex, this.newConnectionIgnoreDistributorIndices, planPoint);
					}
				}

				Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
				KlimaFlaechenModul modul = null;
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas) {
					if (area.Value.IsInside(planPoint)) {
						modul = area.Key;
						break;
					}
				}
				int index;
				ModulBodenCircuit currentCircuit = this.product.GetCircuitForModul(modul, out index);
				if (modul != null && (this.newConnectionStart == null || currentCircuit == this.product.GetCircuitForModul(this.newConnectionStart, out index))) {
					List<KlimaFlaechenModul> connectedModules;
					if (this.newConnectionStart != null) {
						connectedModules = currentCircuit.GetAllLinkedModules(this.newConnectionStart);
					} else {
						connectedModules = new List<KlimaFlaechenModul>();
					}
					if (!connectedModules.Contains(modul)) {
						this.hoveredModul = modul;
						this.hoverInput = (this.newConnectionStart == null || this.newConnectionStartAtOutput) && modul.GetInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
						this.hoverOutput = (this.newConnectionStart == null || !this.newConnectionStartAtOutput) && modul.GetOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
					} else {
						this.hoveredModul = null;
						this.hoverInput = false;
						this.hoverOutput = false;
					}
				} else {
					this.hoveredModul = null;
					this.hoverInput = false;
					this.hoverOutput = false;
				}

				
				Point2D lastVertex = this.product.AssociatedRoom.RoomCoordinates[this.product.AssociatedRoom.RoomCoordinates.Count - 1];
				//Segment2D segment;
				this.hoverAnbindungen = this.newConnectionStart != null;
				/*foreach (Point2D vertex in this.product.AssociatedRoom.RoomCoordinates) {
					segment = new Segment2D(lastVertex, vertex);
					if (segment.GetDistance(planPoint) < 10) {
						this.hoverAnbindungen = true;
						break;
					}
					lastVertex = vertex;
				}*/

				if (this.newConnectionStart != null) {
					KlimaFlaechenModul tmp;
					GraphicalConnectionAnbindungsPunkt tmp2;
					this.nextConnectionPoints = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out tmp, out tmp2);
				} else {
					this.nextConnectionPoints = new List<Point2D>();
				}
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
			return false;
		}

		private Point2D layoutAddAreaStart;
		private PointF layoutAddAreaStartScreen;
		private Polygon2D layoutAddArea = null;
		private bool layoutAddAreaBottomUp = false;
		//private bool layoutAddAreaRightToLeft = false;

		private Nullable<Point> dragStartedInControl;
		private Nullable<Point2D> dragStartedInPlan;
		private Nullable<Point> dragEndedInControl;
		private Nullable<Point2D> dragEndedInPlan;

		private Point2D lastPlanPoint;
		private Point lastPointInControl;

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.StartDrag(planPoint, pointInControl);
				}
			} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				this.newModulesOffsetX = 0;
				this.newModulesOffsetY = 0;
				this.layoutAddAreaStart = planPoint;
				this.layoutAddAreaStartScreen = pointInControl;
			} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
				this.lastPlanPoint = planPoint;
				this.lastPointInControl = pointInControl;
			} else if (this.Mode == KlimaBodenMode.KDM_PICK_MODULE) {
				if (!this.ShiftPressed) {

					/*double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					KlimaFlaechenModul pickedModul = null;
					foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
						Point2D left = rotation.Transform(lane.BorderLeft.Origin);
						Point2D right = rotation.Transform(lane.BorderRight.Origin);
						if (rotatedPoint.X >= left.X && rotatedPoint.X <= right.X) {
							List<KlimaFlaechenModul> modules = this.product.GetModulesInLane(lane.Nr);
							foreach (KlimaFlaechenModul module in modules) {
								if (rotatedPoint.Y >= module.GraphPositionInLan && rotatedPoint.Y <= module.GraphPositionInLan + KlimaFlaechenModul.GetModuleHeight(module.ModulType) * measure) {
									pickedModul = module;
									break;
								}
							}
							if (pickedModul != null) {
								break;
							}
						}
					}
					this.moveModules = this.GetAllSelectedModules().Contains(pickedModul);*/
					this.dragStartedInControl = pointInControl;
					this.dragStartedInPlan = planPoint;
					/*if (this.moveModules) {
						oldModulPositions = new Dictionary<KlimaFlaechenModul, double>();
						foreach (KlimaFlaechenModul modul in this.GetAllSelectedModules()) {
							oldModulPositions.Add(modul, modul.GraphPositionInLan);
						}
					}*/
					//dragIsPick = true;
				} else {
					//this.moveModules = false;
					this.dragStartedInControl = pointInControl;
					this.dragStartedInPlan = planPoint;
					//dragIsPick = true;
				}
			}
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.MoveDrag(planPoint, pointInControl);
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
					return true;
				}
			} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					Matrix3D matrix = Transformation3D.Rotate(this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
					Point2D rotatedP1 = matrix.Transform(layoutAddAreaStart);
					Point2D rotatedP3 = matrix.Transform(planPoint);
					this.layoutAddAreaBottomUp = (rotatedP1.Y > rotatedP3.Y);
					Point2D rotatedP2 = new Point2D(rotatedP1.X, rotatedP3.Y);
					Point2D rotatedP4 = new Point2D(rotatedP3.X, rotatedP1.Y);
					matrix = matrix.GetInverse();
					//Point2D p2 = matrix.Transform(rotatedP2);
					//Point2D p4 = matrix.Transform(rotatedP4);
					layoutAddArea = new Polygon2D();
					layoutAddArea.Add(layoutAddAreaStart);
					layoutAddArea.Add(matrix.Transform(rotatedP2));
					layoutAddArea.Add(planPoint);
					layoutAddArea.Add(matrix.Transform(rotatedP4));
					return true;
				}
			} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH && button == MouseButtons.Left) {
				Vector2D vector = planPoint - this.lastPlanPoint;
				Matrix3D rotate = Transformation3D.Rotate(this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
				vector = rotate.Transform(vector);
				this.NewModulesOffsetX += vector.X;
				this.NewModulesOffsetY += vector.Y;
				this.lastPlanPoint = planPoint;
				this.lastPointInControl = pointInControl;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			} else if (this.Mode == KlimaBodenMode.KDM_PICK_MODULE && button == MouseButtons.Left) {
				//int deltaY = this.dragStartedInControl.Y - pointInControl.Y;
				//Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);

				/*if (deltaX * deltaX + deltaY * deltaY > 25) {
					dragIsPick = false;
				}*/
				//if (/*!dragIsPick && */moveModules) {
				/*	Point2D rotatedStartPoint = rotation.Transform(this.dragStartedInPlan.Value);
					Point2D rotatedCurPoint = rotation.Transform(planPoint);
					double delta = rotatedCurPoint.Y - rotatedStartPoint.Y;
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					Dictionary<int, List<KlimaFlaechenModul>> modulesPerLane = new Dictionary<int, List<KlimaFlaechenModul>>();
					foreach (KlimaFlaechenModul modul in this.GetAllSelectedModules()) {
						if (!modulesPerLane.ContainsKey(modul.GraphLane)) {
							modulesPerLane.Add(modul.GraphLane, new List<KlimaFlaechenModul>());
						}
						modulesPerLane[modul.GraphLane].Add(modul);
					}
					foreach (KeyValuePair<int, List<KlimaFlaechenModul>> kvp in modulesPerLane) {
						if (kvp.Value.Count > 0) {
							KlimaFlaechenModul firstModul = kvp.Value[0];
							bool bottomUp = firstModul.GraphPositionInLan < this.oldModulPositions[firstModul] + delta;
							kvp.Value.Sort(new KlimaFlaechenModuleComparer(!bottomUp));
							PossibleModulLane lane = this.product.GraphConstruction.PossibleLanes[kvp.Key];
							foreach (KlimaFlaechenModul modul in kvp.Value) {
								Nullable<double> bestMove = lane.BestMovePossible(modul, this.oldModulPositions[modul] + delta, measure, this.product, bottomUp);
								if (bestMove.HasValue) {
									modul.GraphPositionInLan = bestMove.Value;
								}
							}
						}
					}
					if (this.ProjectChanged != null) {
						this.ProjectChanged(this);
					}
					this.connectedPlanPanel.InvalidateGraphics();
				} else {*/
					this.dragEndedInControl = pointInControl;
					this.dragEndedInPlan = planPoint;
					this.connectedPlanPanel.InvalidateGraphics();
				//}
			}
			return false;
		}

		private bool AllPointsInside(Polygon2D polygon, IEnumerable<Point2D> points) {
			bool inside = true;
			foreach (Point2D point in points) {
				if (!polygon.IsInside(point)) {
					inside = false;
					break;
				}
			}
			return inside;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
			} else if (this.mode == KlimaBodenMode.KDM_PICK_MODULE) {
				if (this.dragStartedInPlan.HasValue) {
					dragEndedInPlan = planPoint;

					Matrix3D rotation = Transformation3D.Rotate(this.product.AssociatedRoom.AssociatedPlan.Rotation * Math.PI / 180.0);
					Matrix3D invRotation = rotation.GetInverse();

					Point2D p1 = rotation.Transform(dragStartedInPlan.Value);
					Point2D p3 = rotation.Transform(dragEndedInPlan.Value);
					Point2D p2 = new Point2D(p1.X, p3.Y);
					Point2D p4 = new Point2D(p3.X, p1.Y);

					Polygon2D selection = new Polygon2D(new Point2D[] { dragStartedInPlan.Value, invRotation.Transform(p2), dragEndedInPlan.Value, invRotation.Transform(p4) });

					List<KlimaFlaechenModul> selectedModules;
					if (this.ShiftPressed && this.HighlightModules != null && this.HighlightModules.Count > 0) {
						selectedModules = new List<KlimaFlaechenModul>(this.HighlightModules);
					} else {
						selectedModules = new List<KlimaFlaechenModul>();
					}

					foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
						foreach (KlimaFlaechenModul modul in c.Row.List) {
							Polygon2D modulArea = GetModuleArea(modul);
							if (AllPointsInside(selection, modulArea)) {
								selectedModules.Add(modul);
							}
							if (AllPointsInside(modulArea, selection)) {
								selectedModules.Add(modul);
							}
						}
					}

					if (selectedModules.Count > 0) {
						this.HighlightModules = selectedModules;
					} else {
						this.HighlightModules = null;
					}

					this.dragStartedInControl = null;
					this.dragStartedInPlan = null;
					this.dragEndedInControl = null;
					this.dragEndedInPlan = null;
					if (this.ConnectedPlanPanel != null) {
						this.ConnectedPlanPanel.InvalidateGraphics();
					}
					if (this.ModuleSelected != null) {
						this.ModuleSelected(this, EventArgs.Empty);
					}
					return true;
				}
			}
			return false;
		}

		internal bool ShiftPressed {
			get { return (Control.ModifierKeys & (Keys.Shift | Keys.ShiftKey | Keys.LShiftKey | Keys.RShiftKey)) != Keys.None; }
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			return KeyDown(key, this.GetAllSelectedModules());
		}

		#endregion

		public event ProjectChangedHandler ProjectChanged;
		public event EventHandler ModuleSelected;

		[XmlIgnore]
		public double NewModulesRotation {
			get { return newModulesRotation; }
			set { newModulesRotation = value; }
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double NewModulesRotationInclPlanRotation {
			get {
				return -this.newModulesRotation + this.product.AssociatedRoom.AssociatedPlan.Rotation;
			}
		}

		private double NewModulesStepX {
			get {
				double stepX;
				if (this.product.GraphConstruction is ModulKlimaBodenConstructionStaffeln) {
					stepX = (this.product.GraphConstruction as ModulKlimaBodenConstructionStaffeln).StaffelnAchsabstand * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				} else {
					stepX = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesXDicht;
				}
				return stepX;
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double NewModulesOffsetX {
			get { return newModulesOffsetX; }
			set {
				double stepX = this.NewModulesStepX;

				newModulesOffsetX = value;
				while (newModulesOffsetX < 0) {
					newModulesOffsetX += stepX;
				}
				while (newModulesOffsetX >= stepX) {
					newModulesOffsetX -= stepX;
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double NewModulesOffsetY {
			get { return newModulesOffsetY; }
			set {
				double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				double stepY = height;
				stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesYDicht;
				/*if (!this.newModulesYDicht) {
					stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				}*/

				newModulesOffsetY = value;
				while (newModulesOffsetY < 0) {
					newModulesOffsetY += stepY;
				}
				while (newModulesOffsetY >= stepY) {
					newModulesOffsetY -= stepY;
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public VerlegungsAbstand NewModulesXDicht {
			get { return this.newModulesXDicht; }
			set {
				this.newModulesXDicht = value;
				if (this.connectedPlanPanel != null && this.layoutAddArea != null) {
					this.connectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public VerlegungsAbstand NewModulesYDicht {
			get { return this.newModulesYDicht; }
			set {
				this.newModulesYDicht = value;
				if (this.connectedPlanPanel != null && this.layoutAddArea != null) {
					this.connectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NewModulesConnectHorizontal {
			get { return newModulesConnectHorizontal; }
			set {
				newModulesConnectHorizontal = value;
				if (this.connectedPlanPanel != null && this.layoutAddArea != null) {
					this.connectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public KlimaFlaechenModul.ModulOrientationEnum NewModulesStartingOrientation {
			get { return newModulesStartingOrientation; }
			set { newModulesStartingOrientation = value; }
		}

		private bool TryDrawModule(Graphics g, Matrix4D additionalTransformation, double x, double y, double rotation, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, bool bottomUp, Color color) {
			this.DrawModule(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40, orientation, new Point2D(x, y), rotation, additionalTransformation, g, bottomUp, true, color, false, false);
			return true;
		}

		private KlimaFlaechenModul TryAddModule(bool bottomUp, double x, double y, double rotation, ModulBodenCircuit circuit, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation) {
			KlimaFlaechenModul modul = new KlimaFlaechenModul();
			modul.ModulType = KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40;
			modul.GraphPosX = x;
			modul.GraphPosY = y;
			modul.GraphRotation = rotation;
			if (orientation.HasValue) {
				modul.Orientation = orientation.Value;
			}
			modul.GraphBottomUp = bottomUp;
			circuit.Row.List.Add(modul);
			/*foreach (FreeModulLaneArea area in lane.GetFreeAreas(this.product, null)) {
				if (area.Fits(y, y + step)) {
					KlimaFlaechenModul modul = new KlimaFlaechenModul(moduleTypeToAdd, left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
					modul.GraphLane = lane.Nr;
					modul.GraphPositionInLan = y;
					modul.GraphBottomUp = bottomUp;
					KlimaFlaechenList usedRow;
					if (row != null) {
						usedRow = row;
					} else if (laneToRowMapping.ContainsKey(lane)) {
						usedRow = laneToRowMapping[lane];
					} else {
						usedRow = new KlimaFlaechenList();
						subArea.Rows.Add(usedRow);
						laneToRowMapping.Add(lane, usedRow);
					}
					usedRow.List.Add(modul);

					left = !left;
					return true;
				}
			}*/
			return modul;
		}

		private int AddModulesForLayoutArea(AddModuleDelegate doIt, bool addIncomplete, bool addConnections) {
			if (this.layoutAddArea == null || this.layoutAddArea.Count != 4) {
				return 0;
			}
			Matrix3D rotation = Transformation3D.Rotate(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
			Matrix3D invRotation = rotation.GetInverse();

			//Polygon2D rotatedAddArea = new Polygon2D();
			double top = double.MaxValue;
			double bottom = double.MinValue;
			double left = double.MaxValue;
			double right = double.MinValue;
			foreach (Point2D point in this.layoutAddArea) {
				Point2D rotatedPoint = invRotation.Transform(point);
				if (rotatedPoint.X < left) {
					left = rotatedPoint.X;
				}
				if (rotatedPoint.X > right) {
					right = rotatedPoint.X;
				}
				if (rotatedPoint.Y < top) {
					top = rotatedPoint.Y;
				}
				if (rotatedPoint.Y > bottom) {
					bottom = rotatedPoint.Y;
				}
			}

			Point2D rotatedTopLeft = invRotation.Transform(this.layoutAddArea[0]);
			Point2D rotatedBottomLeft = invRotation.Transform(this.layoutAddArea[1]);
			Point2D rotatedBottomRight = invRotation.Transform(this.layoutAddArea[2]);
			Point2D rotatedTopRight = invRotation.Transform(this.layoutAddArea[3]);

			bool rightToLeft = rotatedTopLeft.X > rotatedTopRight.X;
			bool bottomUp = rotatedTopLeft.Y > rotatedBottomLeft.Y;
			left = Math.Min(rotatedTopLeft.X, rotatedTopRight.X);
			right = Math.Max(rotatedTopLeft.X, rotatedTopRight.X);
			top = Math.Min(rotatedTopLeft.Y, rotatedBottomLeft.Y);
			bottom = Math.Max(rotatedTopLeft.Y, rotatedBottomLeft.Y);

			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;

			/*double stepX = width;
			if (this.product.GraphConstruction is ModulKlimaBodenConstructionStaffeln) {
				stepX = ((this.product.GraphConstruction as ModulKlimaBodenConstructionStaffeln).StaffelnAbstand + (this.product.GraphConstruction as ModulKlimaBodenConstructionStaffeln).StaffelnBreite) * measure;
			} else {
				stepX += modulierendDistance * measure * (int)this.newModulesXDicht;
			}*/
			/*if (!this.newModulesXDicht) {
				stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			}*/
			double stepX = this.NewModulesStepX;
			double stepY = height;
			stepY += modulierendDistance * measure * (int)this.newModulesYDicht;
			/*if (!this.newModulesYDicht) {
				stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			}*/

			right -= width;
			bottom -= height;

			bool added = false;
			List<Polygon2D> roomList = new List<Polygon2D>();
			Polygon2D room = new Polygon2D(this.product.AssociatedRoom.RoomCoordinates);
			if (room.IsClockwise()) {
				room.Reverse();
			}
			roomList.Add(room);

			List<List<Polygon2D>> unusedList = new List<List<Polygon2D>>();
			foreach (List<Point2D> unused in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
				Polygon2D newUnused = new Polygon2D(unused);
				if (newUnused.IsClockwise()) {
					newUnused.Reverse();
				}
				List<Polygon2D> newUnusedList = new List<Polygon2D>();
				newUnusedList.Add(newUnused);
				unusedList.Add(newUnusedList);
			}

			foreach (List<Point2D> staffelPoints in this.product.GraphConstruction.Staffeln) {
				Polygon2D staffel = new Polygon2D(staffelPoints);
				if (staffel.IsClockwise()) {
					staffel.Reverse();
				}
				List<Polygon2D> newUnusedList = new List<Polygon2D>();
				newUnusedList.Add(staffel);
				unusedList.Add(newUnusedList);
			}

			foreach (Polygon2D module in this.GetModuleAreas().Values) {
				if (module.IsClockwise()) {
					module.Reverse();
				}
				List<Polygon2D> newUnusedList = new List<Polygon2D>();
				newUnusedList.Add(module);
				unusedList.Add(newUnusedList);
			}

			Matrix4D xyRotation = Transformation4D.RotateZ(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
			bool orientationLeft = this.newModulesStartingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;

			double startY = bottomUp ? bottom - (newModulesOffsetY > 0 ? stepY - newModulesOffsetY : 0) : top + newModulesOffsetY;
			double endY = bottomUp ? top : bottom;
			double incY = bottomUp ? -stepY : stepY;
			double startX = rightToLeft ? right - (newModulesOffsetX > 0 ? stepX - newModulesOffsetX : 0) : left + newModulesOffsetX;
			double endX = rightToLeft ? left : right;
			double incX = rightToLeft ? -stepX : stepX;

			if (this.newModulesConnectHorizontal) {
				bool invertRow = false;
				for (double y = startY; y >= top && y <= bottom; y += incY) {
					bool invertDirection = false;
					orientationLeft = this.newModulesStartingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT; ;
					KlimaFlaechenModul lastAddedModul = null;
					KlimaFlaechenModul addedModul = null;
					ModulBodenCircuit lastCircuitOfModul = null;
					ModulBodenCircuit circuitOfModul = null;
					for (double x = startX; x >= left && x <= right; x += incX) {
						Point3D xy = xyRotation.Transform(new Point3D(x, y, 0));
						Matrix4D moduleTransformation = Matrix4D.Identity;
						moduleTransformation = moduleTransformation * Transformation4D.Translation(xy.X, xy.Y, 0);
						moduleTransformation = moduleTransformation * Transformation4D.RotateZ(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);

						Point2D topLeft2D = moduleTransformation.TransformTo2D(new Point2D(0, 0));
						Point2D topRight2D = moduleTransformation.TransformTo2D(new Point2D(width, 0));
						Point2D bottomRight2D = moduleTransformation.TransformTo2D(new Point2D(width, height));
						Point2D bottomLeft2D = moduleTransformation.TransformTo2D(new Point2D(0, height));

						Polygon2D newModule = new Polygon2D(new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D });
						if (newModule.IsClockwise()) {
							newModule.Reverse();
						}
						List<Polygon2D> newModuleList = new List<Polygon2D>();
						newModuleList.Add(newModule);

						bool fits = true;
						// check if module is inside room coordinates
						try {
							List<Polygon2D> difference = Polygon2D.GetDifference(newModuleList, roomList);
							if (difference != null && difference.Count > 0) {
								//continue;
								fits = false;
							}
						} catch (Exception e) {
							Console.WriteLine(e);
						}

						// check if module intersects unused area or another module
						bool intersectionFound = false;
						foreach (List<Polygon2D> unused in unusedList) {
							List<Polygon2D> intersection = Polygon2D.GetIntersection(newModuleList, unused);
							if (intersection != null && intersection.Count > 0) {
								intersectionFound = true;
								break;
							}
						}
						if (intersectionFound) {
							fits = false;
							//continue;
						}

						if (addIncomplete || fits) {
							bool thisModuleBottomUp = ((rightToLeft && !orientationLeft) || (!rightToLeft && orientationLeft));
							KlimaFlaechenModul.ModulOrientationEnum thisModuleOrientation = (invertRow != orientationLeft) ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
							doIt(xy.X, xy.Y, -this.NewModulesRotationInclPlanRotation, out added, thisModuleOrientation, thisModuleBottomUp, out addedModul, out circuitOfModul, fits);
							if (fits) {
								orientationLeft = !orientationLeft;
								invertDirection = true;
							}

							if (addConnections) {
								// add Verbindeleitung between current and last modul
								if (lastAddedModul != null && circuitOfModul == lastCircuitOfModul) {
									//Point2D output = lastAddedModul.GetOutputConnection(measure);
									//Point2D input = addedModul.GetInputConnection(measure);
									if (circuitOfModul.Links == null) {
										circuitOfModul.Links = new List<KlimaFlaechenModulVerbindung>();
									}
									if (((thisModuleBottomUp == (thisModuleOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT)) != rightToLeft) != this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) {
										circuitOfModul.Links.Add(new KlimaFlaechenModulVerbindung(addedModul, lastAddedModul, new Point2D[] { addedModul.GetOutputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product), lastAddedModul.GetInputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product) }, circuitOfModul, Project.Instance.GetPlannedProduct(this.product)));
									} else {
										circuitOfModul.Links.Add(new KlimaFlaechenModulVerbindung(lastAddedModul, addedModul, new Point2D[] { lastAddedModul.GetOutputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product), addedModul.GetInputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product) }, circuitOfModul, Project.Instance.GetPlannedProduct(this.product)));
									}
								}
								lastAddedModul = addedModul;
								lastCircuitOfModul = circuitOfModul;
							}
						}
					}
					if (invertDirection) {
						invertRow = !invertRow;
					}
				}
			} else {
				bool invertRow = false;
				for (double x = startX; x >= left && x <= right; x += incX) {
					bool invertDirection = false;
					orientationLeft = this.newModulesStartingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT; ;
					KlimaFlaechenModul lastAddedModul = null;
					KlimaFlaechenModul addedModul = null;
					ModulBodenCircuit lastCircuitOfModul = null;
					ModulBodenCircuit circuitOfModul = null;
					for (double y = startY; y >= top && y <= bottom; y += incY) {
						Point3D xy = xyRotation.Transform(new Point3D(x, y, 0));
						Matrix4D moduleTransformation = Matrix4D.Identity;
						moduleTransformation = moduleTransformation * Transformation4D.Translation(xy.X, xy.Y, 0);
						moduleTransformation = moduleTransformation * Transformation4D.RotateZ(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);

						Point2D topLeft2D = moduleTransformation.TransformTo2D(new Point2D(0, 0));
						Point2D topRight2D = moduleTransformation.TransformTo2D(new Point2D(width, 0));
						Point2D bottomRight2D = moduleTransformation.TransformTo2D(new Point2D(width, height));
						Point2D bottomLeft2D = moduleTransformation.TransformTo2D(new Point2D(0, height));

						Polygon2D newModule = new Polygon2D(new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D });
						if (newModule.IsClockwise()) {
							newModule.Reverse();
						}
						List<Polygon2D> newModuleList = new List<Polygon2D>();
						newModuleList.Add(newModule);

						bool fits = true;
						// check if module is inside room coordinates
						try {
							List<Polygon2D> difference = Polygon2D.GetDifference(newModuleList, roomList);
							if (difference != null && difference.Count > 0) {
								//continue;
								fits = false;
							}
						} catch (Exception e) {
							Console.WriteLine(e);
						}

						// check if module intersects unused area or another module
						bool intersectionFound = false;
						foreach (List<Polygon2D> unused in unusedList) {
							List<Polygon2D> intersection = Polygon2D.GetIntersection(newModuleList, unused);
							if (intersection != null && intersection.Count > 0) {
								intersectionFound = true;
								break;
							}
						}
						if (intersectionFound) {
							fits = false;
							//continue;
						}

						if (addIncomplete || fits) {
							bool thisModuleBottomUp = invertRow != bottomUp;
							KlimaFlaechenModul.ModulOrientationEnum thisModuleOrientation = (invertRow != orientationLeft) ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
							doIt(xy.X, xy.Y, -this.NewModulesRotationInclPlanRotation, out added, thisModuleOrientation, thisModuleBottomUp, out addedModul, out circuitOfModul, fits);
							if (fits) {
								orientationLeft = !orientationLeft;
								invertDirection = true;
							}

							if (addConnections) {
								// add Verbindeleitung between current and last modul
								if (lastAddedModul != null && circuitOfModul == lastCircuitOfModul) {
									//Point2D output = lastAddedModul.GetOutputConnection(measure);
									//Point2D input = addedModul.GetInputConnection(measure);
									if (circuitOfModul.Links == null) {
										circuitOfModul.Links = new List<KlimaFlaechenModulVerbindung>();
									}
									if ((thisModuleBottomUp != bottomUp) != this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) {
										circuitOfModul.Links.Add(new KlimaFlaechenModulVerbindung(lastAddedModul, addedModul, new Point2D[] { lastAddedModul.GetOutputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product), addedModul.GetInputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product) }, circuitOfModul, Project.Instance.GetPlannedProduct(this.product)));
									} else {
										circuitOfModul.Links.Add(new KlimaFlaechenModulVerbindung(addedModul, lastAddedModul, new Point2D[] { addedModul.GetOutputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product), lastAddedModul.GetInputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product) }, circuitOfModul, Project.Instance.GetPlannedProduct(this.product)));
									}
								}
								lastAddedModul = addedModul;
								lastCircuitOfModul = circuitOfModul;
							}
						}
					}
					if (invertDirection) {
						invertRow = !invertRow;
					}
				}
			}

			// TODO
			return 1;
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleAreas() {
			Dictionary<KlimaFlaechenModul, Polygon2D> moduleAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * measure;
			foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
				foreach (KlimaFlaechenModul modul in c.Row.List) {
					Matrix3D transformation = Matrix3D.Identity;
					transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
					transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);
					Polygon2D modulArea = new Polygon2D();
					modulArea.Add(transformation.Transform(new Point2D(0, 0)));
					modulArea.Add(transformation.Transform(new Point2D(width, 0)));
					modulArea.Add(transformation.Transform(new Point2D(width, height)));
					modulArea.Add(transformation.Transform(new Point2D(0, height)));
					moduleAreas.Add(modul, modulArea);
				}
			}

			return moduleAreas;
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleInputs() {
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
							/*output12D = topLeft2D;
							output22D = transformation.Transform(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));*/
							input12D = transformation.Transform(new Point2D(width, height));
							input22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
						} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							/*output12D = topRight2D;
							output22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));*/
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
							/*output12D = bottomRight2D;
							output22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));*/
						} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							input12D = transformation.Transform(new Point2D(width, 0));
							input22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
							/*output12D = bottomLeft2D;
							output22D = transformation.Transform(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));*/
						}
					}

					/*Polygon2D modulArea = new Polygon2D();
					modulArea.Add(transformation.Transform(new Point2D(0, 0)));
					modulArea.Add(transformation.Transform(new Point2D(width, 0)));
					modulArea.Add(transformation.Transform(new Point2D(width, height)));
					modulArea.Add(transformation.Transform(new Point2D(0, height)));
					moduleAreas.Add(modul, modulArea);*/
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

					/*Polygon2D modulArea = new Polygon2D();
					modulArea.Add(transformation.Transform(new Point2D(0, 0)));
					modulArea.Add(transformation.Transform(new Point2D(width, 0)));
					modulArea.Add(transformation.Transform(new Point2D(width, height)));
					modulArea.Add(transformation.Transform(new Point2D(0, height)));
					moduleAreas.Add(modul, modulArea);*/
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
		}

		public void DrawModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, double rotation, Matrix4D additionalTransformation, Graphics g, bool bottomUp, bool highlight, Color circuitColor, bool highlightInput, bool highlightOutput) {
			if (this.product == null || this.product.GraphConstruction == null ||
				this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
				this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(rotation * Math.PI / 180.0);

			double height = KlimaFlaechenModul.GetModuleHeight(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double width = KlimaFlaechenModul.GetModuleWidth(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Point2D topLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, 0));
			Point2D topRight2D = additionalTransformation.TransformTo2D(new Point2D(width, 0));
			Point2D bottomRight2D = additionalTransformation.TransformTo2D(new Point2D(width, height));
			Point2D bottomLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, height));

			Point2D input12D = Point2D.Zero;
			Point2D input22D = Point2D.Zero;
			Point2D input32D = Point2D.Zero;
			Point2D input42D = Point2D.Zero;
			Point2D output12D = Point2D.Zero;
			Point2D output22D = Point2D.Zero;
			Point2D output32D = Point2D.Zero;
			Point2D output42D = Point2D.Zero;

			Point2D directionTop12D;
			Point2D directionTop22D;
			Point2D directionTop32D;
			Point2D directionBottom12D;
			Point2D directionBottom22D;
			Point2D directionBottom32D;

			/*if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
				if (bottomUp) {
					directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0));

					directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				} else {
					directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
					directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
					directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

					directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				}
			} else {
				if (bottomUp) {
					directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.20 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.20 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

					directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				} else {
					directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.20 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

					directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height));
				}
			}*/
			if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == bottomUp) {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				if (highlightInput || highlightOutput) {
					if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
						output12D = topLeft2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						input12D = bottomRight2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
						output12D = topRight2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						input12D = bottomLeft2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					}
				}
			} else {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				if (highlightInput || highlightOutput) {
					if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
						input12D = topLeft2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						output12D = bottomRight2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
						input12D = topRight2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						output12D = bottomLeft2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					}
				}
			}

			PointF topLeft = new PointF((float)topLeft2D.X, (float)topLeft2D.Y);
			PointF topRight = new PointF((float)topRight2D.X, (float)topRight2D.Y);
			PointF bottomRight = new PointF((float)bottomRight2D.X, (float)bottomRight2D.Y);
			PointF bottomLeft = new PointF((float)bottomLeft2D.X, (float)bottomLeft2D.Y);
			PointF middle = new PointF((topLeft.X + bottomRight.X) / 2, (topLeft.Y + bottomRight.Y) / 2);

			PointF directionTop1 = new PointF((float)directionTop12D.X, (float)directionTop12D.Y);
			PointF directionTop2 = new PointF((float)directionTop22D.X, (float)directionTop22D.Y);
			PointF directionTop3 = new PointF((float)directionTop32D.X, (float)directionTop32D.Y);

			PointF directionBottom1 = new PointF((float)directionBottom12D.X, (float)directionBottom12D.Y);
			PointF directionBottom2 = new PointF((float)directionBottom22D.X, (float)directionBottom22D.Y);
			PointF directionBottom3 = new PointF((float)directionBottom32D.X, (float)directionBottom32D.Y);

			Color c;
			if (highlight) {
				int cr = Math.Min((int)(circuitColor.R * 1.5) + 32, 255);
				int cg = Math.Min((int)(circuitColor.G * 1.5) + 32, 255);
				int cb = Math.Min((int)(circuitColor.B * 1.5) + 32, 255);
				c = Color.FromArgb(circuitColor.A / 2, cr, cg, cb);
			} else {
				c = Color.FromArgb(circuitColor.A / 2, circuitColor);
			}

			Pen p = new Pen(c);
			if (highlight) {
				p.Width = 1.5f;
			}
			Brush b = new SolidBrush(Color.FromArgb(c.A / 2, c));
			if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					g.DrawLines(p, new PointF[] { topRight, bottomRight, bottomLeft, topLeft, topRight, middle, bottomRight });
				} else {
					g.DrawLines(p, new PointF[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft, topRight });
				}
			} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					g.DrawLines(p, new PointF[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft, middle, topLeft });
				} else {
					g.DrawLines(p, new PointF[] { topLeft, topRight, bottomRight, bottomLeft, topLeft, bottomRight });
				}
			} else {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				g.DrawLines(p, new PointF[] { topLeft, topRight, bottomRight, bottomLeft, topLeft });
			}

			if (orientation != null) {
				if (highlight) {
					g.FillPolygon(b, new PointF[] { directionTop1, directionTop2, directionTop3 });
					g.FillPolygon(b, new PointF[] { directionBottom1, directionBottom2, directionBottom3 });
				} else {
					g.DrawPolygon(p, new PointF[] { directionTop1, directionTop2, directionTop3 });
					g.DrawPolygon(p, new PointF[] { directionBottom1, directionBottom2, directionBottom3 });
				}
				if (highlightInput) {
					PointF input1 = new PointF((float)input12D.X, (float)input12D.Y);
					PointF input2 = new PointF((float)input22D.X, (float)input22D.Y);
					PointF input3 = new PointF((float)input32D.X, (float)input32D.Y);
					PointF input4 = new PointF((float)input42D.X, (float)input42D.Y);

					Brush bInput = new SolidBrush(Color.FromArgb(127, Color.Red));
					Pen pInput = new Pen(Color.Red);
					//if (this.newConnectionStart == null || this.newConnectionStartAtOutput) {
						g.FillPolygon(bInput, new PointF[] { input1, input2, input3, input4 });
						g.DrawPolygon(pInput, new PointF[] { input1, input2, input3, input4 });
					//}
				}
				if (highlightOutput) {
					PointF output1 = new PointF((float)output12D.X, (float)output12D.Y);
					PointF output2 = new PointF((float)output22D.X, (float)output22D.Y);
					PointF output3 = new PointF((float)output32D.X, (float)output32D.Y);
					PointF output4 = new PointF((float)output42D.X, (float)output42D.Y);

					Brush bOutput = new SolidBrush(Color.FromArgb(127, Color.Blue));
					Pen pOutput = new Pen(Color.Blue);
					//if (this.newConnectionStart == null || !this.newConnectionStartAtOutput) {
						g.FillPolygon(bOutput, new PointF[] { output1, output2, output3, output4 });
						g.DrawPolygon(pOutput, new PointF[] { output1, output2, output3, output4 });
					//}
				}
			}

			GraphicsPath path = new GraphicsPath();

			Matrix oldTransform = g.Transform;
			Matrix newTransform = g.Transform.Clone();
			g.Transform = new Matrix();

			if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
				newTransform.RotateAt(-(float)(rotation), bottomLeft);
			} else {
				newTransform.RotateAt((float)(rotation), topLeft);
			}
			g.Transform = newTransform;

			string moduleString = "";
			switch (type) {
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_100_30_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40:
					moduleString = EuroplanRes.KlimaFlaechenModul_100_40_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_120_30_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60B_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60C_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60D_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_80_30_Short;
					break;
			}

			if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
				g.DrawString(moduleString, new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value), new SolidBrush(Color.FromArgb(circuitColor.A, c)), bottomLeft);
				g.Transform = oldTransform;
			} else {
				g.DrawString(moduleString, new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value), new SolidBrush(Color.FromArgb(circuitColor.A, c)), topLeft);
				g.Transform = oldTransform;
			}
		}

		private void DrawDxfModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, Matrix4D additionalTransformation, DxfModel model, DxfLayer layer, bool bottomUp, Color circuitColor, double rotation) {
			if (this.product == null || this.product.GraphConstruction == null ||
						this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
						this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}

			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(rotation * Math.PI / 180.0);

			double height = KlimaFlaechenModul.GetModuleHeight(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double width = KlimaFlaechenModul.GetModuleWidth(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Point2D topLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, 0));
			Point2D topRight2D = additionalTransformation.TransformTo2D(new Point2D(width, 0));
			Point2D bottomRight2D = additionalTransformation.TransformTo2D(new Point2D(width, height));
			Point2D bottomLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, height));
			Point2D middle2D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height / 2));
			Point2D textStart = additionalTransformation.TransformTo2D(new Point2D(0.01 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.06 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

			Point2D directionTop12D;
			Point2D directionTop22D;
			Point2D directionTop32D;
			Point2D directionBottom12D;
			Point2D directionBottom22D;
			Point2D directionBottom32D;

			if (bottomUp) {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
			} else {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
			}

			Color c = Color.FromArgb(128, circuitColor);

			Pen p = new Pen(c);

			Point2D[] polygon = null;
			if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
				polygon = new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D };
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					DxfLine line = new DxfLine(c, bottomLeft2D, middle2D);
					line.Layer = layer;
					model.Entities.Add(line);
					line = new DxfLine(c, middle2D, topLeft2D);
					line.Layer = layer;
					model.Entities.Add(line);
				} else {
					DxfLine line = new DxfLine(c, topRight2D, bottomLeft2D);
					line.Layer = layer;
					model.Entities.Add(line);
				}
			} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
				polygon = new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D };
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					DxfLine line = new DxfLine(c, bottomRight2D, middle2D);
					line.Layer = layer;
					model.Entities.Add(line);
					line = new DxfLine(c, middle2D, topRight2D);
					line.Layer = layer;
					model.Entities.Add(line);
				} else {
					DxfLine line = new DxfLine(c, topLeft2D, bottomRight2D);
					line.Layer = layer;
					model.Entities.Add(line);
				}
			} else {
				polygon = new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D };
			}

			DxfPolyline2D polyLine = new DxfPolyline2D(c, polygon);
			polyLine.Closed = true;
			polyLine.Layer = layer;
			model.Entities.Add(polyLine);

			if (orientation != null) {
				polygon = new Point2D[] { directionTop12D, directionTop22D, directionTop32D };
				polyLine = new DxfPolyline2D(c, polygon);
				polyLine.Closed = true;
				polyLine.Layer = layer;
				model.Entities.Add(polyLine);
				polygon = new Point2D[] { directionBottom12D, directionBottom22D, directionBottom32D };
				polyLine = new DxfPolyline2D(c, polygon);
				polyLine.Closed = true;
				polyLine.Layer = layer;
				model.Entities.Add(polyLine);
			}

			string moduleString = "";
			switch (type) {
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_100_30_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40:
					moduleString = EuroplanRes.KlimaFlaechenModul_100_40_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_120_30_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60B_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60C_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60D_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_80_30_Short;
					break;
			}

			if (!model.TextStyles.Contains("HarreitherStyle")) {
				DxfTextStyle textStyle = new DxfTextStyle("HarreitherStyle", "Arial.ttf");
				model.TextStyles.Add(textStyle);
			}
			DxfText text = new DxfText(moduleString, (Point3D)textStart, 0.05f * this.product.AssociatedRoom.AssociatedPlan.Measure.Value);
			//text3.Thickness = 0.4d;
			text.Style = model.TextStyles["HarreitherStyle"];
			text.Layer = layer;
			text.Color = c;
			text.Rotation = rotation / 180.0 * Math.PI;
			model.Entities.Add(text);
		}

		internal void DrawDxf(DxfModel model, DxfLayer modulLayer, DxfLayer floorConstructionLayer) {
			Matrix4D additionalTransformation = Matrix4D.Identity;

			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {
				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.PaintDxf(model, floorConstructionLayer);
					}
				}

				if (this.mode != KlimaBodenMode.KDM_CONSTRUCTION) {
					Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
					Matrix3D invRotation = rotation.GetInverse();

					List<KlimaFlaechenModul> selectedModules = this.GetAllSelectedModules();
					foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
						foreach (KlimaFlaechenModul modul in circuit.Row.List) {
							this.DrawDxfModule(modul.ModulType, modul.Orientation, invRotation.Transform(new Point2D(modul.GraphPosX, modul.GraphPosY)), additionalTransformation, model, modulLayer, modul.GraphBottomUp, circuit.CircuitColor, modul.GraphRotation);
						}
					}
				}

				if (this.product.AssociatedRoom.RoomUnusedAreaCoordinates != null) {
					Color gray = Color.Gray;
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
						Polygon2D polygon = new Polygon2D(unusedArea);
						DxfPolyline2D polyLine = new DxfPolyline2D(gray, polygon.ToArray());
						polyLine.Closed = true;
						polyLine.Layer = modulLayer;
						model.Entities.Add(polyLine);
					}
				}
			}
		}

		public ModulBodenCircuit ConfirmNewModules() {
			bool newCircuit = this.highlightCircuit == null;
			if (newCircuit) {
				bool productConnected = false;
				foreach (GraphicalProductConnection conn in this.product.Connections) {
					if (conn.OtherCircuits) {
						productConnected = true;
						break;
					}
				}
				if (productConnected) {
					if (MessageBox.Show("Wenn Sie einen neuen Heizkreis hinzufügen werden die bestehenden Anbindeleitungen an den Verteiler gelöscht. Wollen Sie die Anbindeleitungen löschen?", "Neuer Heizkreis", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
						this.layoutAddArea = null;
						this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
						if (this.connectedPlanPanel != null) {
							this.connectedPlanPanel.InvalidateGraphics();
						}
						return null;
					}
					this.product.Connections.Clear();
					foreach (ModulBodenCircuit mbc in this.product.PlannedCircuits) {
						List<KlimaFlaechenModulVerbindung> linksToDelete = new List<KlimaFlaechenModulVerbindung>();
						foreach (KlimaFlaechenModulVerbindung link in mbc.Links) {
							if (link.Start == null || link.End == null) {
								linksToDelete.Add(link);
							}
						}
						foreach (KlimaFlaechenModulVerbindung link in linksToDelete) {
							mbc.Links.Remove(link);
						}
					}
				}
			}
			ModulBodenCircuit c;
			if (newCircuit) {
				c = new ModulBodenCircuit();
				c.CircuitColor = this.GetNewCircuitColor();
			} else {
				c = this.highlightCircuit;
			}
			int newModules = this.AddModulesForLayoutArea(delegate(double x, double y, double rotation, out bool added, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, bool bottomUp, out KlimaFlaechenModul addedModul, out ModulBodenCircuit circuitOfModul, bool fits) {
				if (fits) {
					addedModul = this.TryAddModule(bottomUp, x, y, rotation, c, orientation);
					circuitOfModul = c;
					added = addedModul != null;
				} else {
					addedModul = null;
					circuitOfModul = null;
					added = false;
				}
			}, false, true);
			if (this.UpdateNewCount != null) {
				this.UpdateNewCount(this, new UpdateNewCountArgs(0));
			}
			if (newModules > 0 && newCircuit) {
				this.product.PlannedCircuits.Add(c);
			}
			this.layoutAddArea = null;
			if (this.connectedPlanPanel != null) {
				this.connectedPlanPanel.InvalidateGraphics();
			}

			int i = 0;
			while (i < this.product.PlannedCircuits.Count && this.product.PlannedCircuits.Count > 1) {
				if ((this.product.PlannedCircuits[i] as ModulBodenCircuit).Row.List.Count == 0) {
					this.product.PlannedCircuits.RemoveAt(i);
				} else {
					i++;
				}
			}

			this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
			return newModules > 0 && newCircuit ? c : null;
		}

		private ModulBodenCircuit highlightCircuit = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulBodenCircuit HighlightCircuit {
			get { return this.highlightCircuit; }
			set {
				this.highlightCircuit = value;
				this.highlightModules = null;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		private List<KlimaFlaechenModul> highlightModules = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<KlimaFlaechenModul> HighlightModules {
			get { return this.highlightModules; }
			set {
				this.highlightCircuit = null;
				this.highlightModules = value;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		public List<KlimaFlaechenModul> GetAllSelectedModules() {
			List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
			if (this.HighlightCircuit != null) {
				foreach (KlimaFlaechenModul modul in this.HighlightCircuit.Row.List) {
					modules.Add(modul);
				}
			} else if (this.HighlightModules != null) {
				foreach (KlimaFlaechenModul modul in this.HighlightModules) {
					modules.Add(modul);
				}
			}
			return modules;
		}

		public bool ContainsNotConfirmedModules {
			get {
				// TODO check if the area really contains modules
				return this.layoutAddArea != null;
			}
		}

		private Color GetNewCircuitColor() {
			Dictionary<Color, int> dict = new Dictionary<Color, int>();
			foreach (Color c in this.circuitColors) {
				dict.Add(c, 0);
			}
			foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
				if (dict.ContainsKey(circuit.CircuitColor)) {
					dict[circuit.CircuitColor]++;
				}
			}
			int minValue = Int32.MaxValue;
			Color newColor = this.circuitColors[0];
			foreach (KeyValuePair<Color, int> kvp in dict) {
				if (kvp.Value < minValue) {
					newColor = kvp.Key;
					minValue = kvp.Value;
				}
			}
			return newColor;
			//return Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
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
	}
}
