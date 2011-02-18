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

		public enum KlimaBodenMode {
			KDM_NONE,
			KDM_CONSTRUCTION,
			KDM_LAYOUT_ADD_AREA,
			KDM_LAYOUT_ADD_AREA_FINISH,
			KDM_LAYOUT_ADD_AREA_PICK_REFERENCE,
			KDM_PICK_MODULE
		}

		public delegate void AddModuleDelegate(double x, double y, double rotation, out bool added, KlimaFlaechenModul.ModulOrientationEnum orientation, bool bottomUp);

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
		private double newModulesRotation = 0.0;
		private bool newModulesXDicht = false;
		private bool newModulesYDicht = false;
		private double newModulesOffsetX = 0.0;
		private double newModulesOffsetY = 0.0;
		private bool newModulesConnectHorizontal = false;
		private KlimaFlaechenModul.ModulOrientationEnum newModulesStartingOrientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
		private static double modulierendDistance = 0.1;

		public event EventHandler<EventArgs> ModeChanged;

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
			}
		}

		public KlimaBodenMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				if (this.mode != KlimaBodenMode.KDM_PICK_MODULE) {
					this.HighlightModules = null;
				}
				if (this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA && this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH && this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
					this.layoutAddArea = null;
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
				if (e.KeyCode == Keys.Delete && this.highlightModules != null) {
					List<Circuit> emptyCircuits = new List<Circuit>();
					foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
						foreach (KlimaFlaechenModul kfm in this.highlightModules) {
							if (c.Row.List.Contains(kfm)) {
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
					this.ModuleSelected(this, EventArgs.Empty);
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl);
		}

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.CeilingCoordinatesToUse != null) {
				GraphicsPath path = new GraphicsPath();
				List<PointF> transformedPoints = new List<PointF>();
				foreach (Point2D point in this.product.AssociatedRoom.CeilingCoordinatesToUse) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
				}
				path.AddPolygon(transformedPoints.ToArray());
				Region clipDisabled = new Region();
				clipDisabled.MakeInfinite();
				clipDisabled.Exclude(path);
				path.Dispose();
				Color c = Color.Black;
				if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
					c = Color.White;
				}
				Brush b = new SolidBrush(c);
				b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));

				if (highlightRoomCoordinates) {
					// gray out all except the room
					g.FillRegion(b, clipDisabled);
				}

				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.Paint(g, this.Mode);
					}
				}

				if (this.mode != KlimaBodenMode.KDM_CONSTRUCTION) {
					foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
						Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
						Matrix3D invRotation = rotation.GetInverse();

						double left = rotation.Transform(lane.BorderLeft.Origin).X;

					}
				}

				if (this.layoutAddArea != null) {
					PointF[] drawArea = new PointF[this.layoutAddArea.Count];
					for (int i = 0; i < this.layoutAddArea.Count; i++) {
						Point2D tmp = additionalTransformation.TransformTo2D(this.layoutAddArea[i]);
						drawArea[i] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					g.DrawPolygon(Pens.Red, drawArea);

					this.AddModulesForLayoutArea(delegate(double x, double y, double rotation, out bool added, KlimaFlaechenModul.ModulOrientationEnum orientation, bool bottomUp) {
						added = this.TryDrawModule(g, additionalTransformation, x, y, rotation, orientation, bottomUp);
					});
				}

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


				if (this.dragStartedInPlan.HasValue && this.dragEndedInPlan.HasValue) {
					//Matrix3D rotation = Transformation3D.Rotate(this.Product.AssociatedRoom.AssociatedPlan.Rotation * Math.PI / 180.0);
					//Point2D rotatedStart = rotation.Transform(this.dragStartedInPlan.Value);
					//Point2D rotatedEnd = rotation.Transform(this.dragEndedInPlan.Value);
					//Matrix3D invRotation = rotation.GetInverse();
					/*Polygon2D selectedPoly = new Polygon2D();
					selectedPoly.Add(this.dragStartedInPlan.Value);
					selectedPoly.Add(new);
					selectedPoly.Add(this.dragEndedInPlan.Value);
					selectedPoly.Add(invRotation.Transform(new Point2D(rotatedEnd.X, rotatedStart.Y)));

					PointF[] arr = new PointF[selectedPoly.Count];
					int i = 0;
					foreach (Point2D point in selectedPoly) {
						Point2D tmp = additionalTransformation.TransformTo2D(new Point3D(point, 0));
						arr[i] = new PointF((float)tmp.X, (float)tmp.Y);
						i++;
					}*/
					Matrix transform = g.Transform;
					g.Transform = new Matrix();
					g.DrawPolygon(new Pen(Color.Red), new Point[] { this.dragStartedInControl.Value, new Point(this.dragStartedInControl.Value.X, this.dragEndedInControl.Value.Y), this.dragEndedInControl.Value, new Point(this.dragEndedInControl.Value.X, this.dragStartedInControl.Value.Y) });
					g.Transform = transform;
				}

				List<KlimaFlaechenModul> selectedModules = this.GetAllSelectedModules();
				foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
					foreach (KlimaFlaechenModul modul in circuit.Row.List) {
						this.DrawModule(modul.ModulType, modul.Orientation, new Point2D(modul.GraphPosX, modul.GraphPosY), modul.GraphRotation, additionalTransformation, g, modul.GraphBottomUp, selectedModules.Contains(modul), circuit.CircuitColor);
					}
				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
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
					Point2D referencePoint = invRotation.Transform(new Point2D(pickedModul.GraphPosX, pickedModul.GraphPosY));

					double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

					double stepX = width;
					if (!this.newModulesXDicht) {
						stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					}
					double stepY = height;
					if (!this.newModulesYDicht) {
						stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					}

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
				}




				this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
				// TODO ausrichten 
			}
			return false;
		}

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
			}
			return false;
		}

		private Point2D layoutAddAreaStart;
		private PointF layoutAddAreaStartScreen;
		private Polygon2D layoutAddArea = null;
		private bool layoutAddAreaBottomUp = false;
		private bool layoutAddAreaRightToLeft = false;

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
				this.NewModulesOffsetX += planPoint.X - this.lastPlanPoint.X;
				this.NewModulesOffsetY += planPoint.Y - this.lastPlanPoint.Y;
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
					Console.WriteLine(delta / measure);
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
					if (this.ShiftPressed) {
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
			return false;
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
				return this.newModulesRotation + this.product.AssociatedRoom.AssociatedPlan.Rotation;
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double NewModulesOffsetX {
			get { return newModulesOffsetX; }
			set {
				double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				double stepX = width;
				if (!this.newModulesXDicht) {
					stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				}

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
				if (!this.newModulesYDicht) {
					stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				}

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
		public bool NewModulesXDicht {
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
		public bool NewModulesYDicht {
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

		private bool TryDrawModule(Graphics g, Matrix4D additionalTransformation, double x, double y, double rotation, KlimaFlaechenModul.ModulOrientationEnum orientation, bool bottomUp) {
			this.DrawModule(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40, orientation, new Point2D(x, y), rotation, additionalTransformation, g, bottomUp, false, Color.Green);
			return true;
		}

		private bool TryAddModule(bool bottomUp, double x, double y, double rotation, ModulBodenCircuit circuit, KlimaFlaechenModul.ModulOrientationEnum orientation) {
			KlimaFlaechenModul modul = new KlimaFlaechenModul();
			modul.ModulType = KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40;
			modul.GraphPosX = x;
			modul.GraphPosY = y;
			modul.GraphRotation = rotation;
			modul.Orientation = orientation;
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
			return false;
		}

		private int AddModulesForLayoutArea(AddModuleDelegate doIt) {
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

			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			double stepX = width;
			if (!this.newModulesXDicht) {
				stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			}
			double stepY = height;
			if (!this.newModulesYDicht) {
				stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			}

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

			if (this.newModulesConnectHorizontal) {
				for (double y = top + newModulesOffsetY; y <= bottom; y += stepY) {
					orientationLeft = true;
					for (double x = left + newModulesOffsetX; x <= right; x += stepX) {
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

						// check if module is inside room coordinates
						try {
							List<Polygon2D> difference = Polygon2D.GetDifference(newModuleList, roomList);
							if (difference != null && difference.Count > 0) {
								continue;
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
							continue;
						}

						doIt(xy.X, xy.Y, -this.NewModulesRotationInclPlanRotation, out added, orientationLeft ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT, orientationLeft);
						orientationLeft = !orientationLeft;
					}
				}
			} else {
				for (double x = left + newModulesOffsetX; x <= right; x += stepX) {
					orientationLeft = true;
					for (double y = top + newModulesOffsetY; y <= bottom; y += stepY) {
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

						// check if module is inside room coordinates
						try {
							List<Polygon2D> difference = Polygon2D.GetDifference(newModuleList, roomList);
							if (difference != null && difference.Count > 0) {
								continue;
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
							continue;
						}

						doIt(xy.X, xy.Y, -this.NewModulesRotationInclPlanRotation, out added, orientationLeft ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT, false);
						orientationLeft = !orientationLeft;
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

		public void DrawModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, double rotation, Matrix4D additionalTransformation, Graphics g, bool bottomUp, bool highlight, Color circuitColor) {
			if (this.product == null || this.product.GraphConstruction == null ||
				this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
				this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(this.product.GraphConstruction.Rotation * Math.PI / 180.0);

			double height = KlimaFlaechenModul.GetModuleHeight(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double width = KlimaFlaechenModul.GetModuleWidth(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Point2D topLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, 0));
			Point2D topRight2D = additionalTransformation.TransformTo2D(new Point2D(width, 0));
			Point2D bottomRight2D = additionalTransformation.TransformTo2D(new Point2D(width, height));
			Point2D bottomLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, height));

			Point2D directionTop12D;
			Point2D directionTop22D;
			Point2D directionTop32D;
			Point2D directionBottom12D;
			Point2D directionBottom22D;
			Point2D directionBottom32D;

			if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
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
				c = Color.FromArgb(128, cr, cg, cb);
			} else {
				c = Color.FromArgb(128, circuitColor);
			}

			Pen p = new Pen(c);
			if (highlight) {
				p.Width = 1.5f;
			}
			Brush b = new SolidBrush(Color.FromArgb(64, c));
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
			}

			GraphicsPath path = new GraphicsPath();

			Matrix oldTransform = g.Transform;
			Matrix newTransform = g.Transform.Clone();
			g.Transform = new Matrix();

			if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
				newTransform.RotateAt(-(float)(this.product.GraphConstruction.Rotation), bottomLeft);
			} else {
				newTransform.RotateAt((float)(this.product.GraphConstruction.Rotation), topLeft);
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
				g.DrawString(moduleString, new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value), new SolidBrush(Color.FromArgb(255, c)), bottomLeft);
				g.Transform = oldTransform;
			} else {
				g.DrawString(moduleString, new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value), new SolidBrush(Color.FromArgb(255, c)), topLeft);
				g.Transform = oldTransform;
			}
		}

		private void DrawDxfModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, Matrix4D additionalTransformation, DxfModel model, DxfLayer layer, bool bottomUp, Color circuitColor) {
			if (this.product == null || this.product.GraphConstruction == null ||
						this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
						this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}

			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(this.product.GraphConstruction.Rotation * Math.PI / 180.0);

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
			Console.WriteLine(this.product.GraphConstruction.Rotation);
			text.Rotation = this.product.GraphConstruction.Rotation / 180.0 * Math.PI;
			model.Entities.Add(text);
		}

		internal void DrawDxf(DxfModel model, DxfLayer layer) {
			Matrix4D additionalTransformation = Matrix4D.Identity;

			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.CeilingCoordinatesToUse != null) {
				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.PaintDxf(model, layer);
					}
				}

				if (this.mode != KlimaBodenMode.KDM_CONSTRUCTION) {
					Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
					Matrix3D invRotation = rotation.GetInverse();

					List<KlimaFlaechenModul> selectedModules = this.GetAllSelectedModules();
					foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
						foreach (KlimaFlaechenModul modul in circuit.Row.List) {
							this.DrawDxfModule(modul.ModulType, modul.Orientation, invRotation.Transform(new Point2D(modul.GraphPosX, modul.GraphPosY)), additionalTransformation, model, layer, modul.GraphBottomUp, circuit.CircuitColor);
						}
					}
				}
			}
		}

		public ModulBodenCircuit ConfirmNewModules() {
			bool newCircuit = this.highlightCircuit == null;
			ModulBodenCircuit c = newCircuit ? new ModulBodenCircuit() : this.highlightCircuit;
			int newModules = this.AddModulesForLayoutArea(delegate(double x, double y, double rotation, out bool added, KlimaFlaechenModul.ModulOrientationEnum orientation, bool bottomUp) {
				added = this.TryAddModule(bottomUp, x, y, rotation, c, orientation);
			});
			if (newModules > 0 && newCircuit) {
				this.product.PlannedCircuits.Add(c);
			}
			this.layoutAddArea = null;
			if (this.connectedPlanPanel != null) {
				this.connectedPlanPanel.InvalidateGraphics();
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
				this.ConnectedPlanPanel.InvalidateGraphics();
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

		[DefaultValue(true)]
		public bool HighlightRoomCoordinates {
			get { return this.highlightRoomCoordinates; }
			set { this.highlightRoomCoordinates = value; }
		}
	}
}
