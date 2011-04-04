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
	public partial class EurovalPlanner : Component, IProductPlanner {

		public enum EurovalMode {
			EVM_NONE,
			EVM_ADD_RZ,
			EVM_DEL_RZ
		}

		public EurovalPlanner() {
			InitializeComponent();
		}

		public EurovalPlanner(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private EurovalProduct product;
		private EurovalMode mode = EurovalMode.EVM_NONE;
		private Cursor customCursor = null;
		private bool highlightRoomCoordinates = true;
		private bool drawExpansionGaps = true;
		private Point2D rzStart = Point2D.Zero;

		public event EventHandler<EventArgs> ModeChanged;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public EurovalProduct Product {
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

		public EurovalMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				if (this.mode == EurovalMode.EVM_ADD_RZ) {
					rzStart = Point2D.Zero;
				}
				//if (this.mode != KlimaBodenMode.KDM_PICK_MODULE && this.HighlightModules != null) {
				//    this.HighlightModules = null;
				//}
				//if (this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA && this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH && this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
				//    this.layoutAddArea = null;
				//    if (this.connectedPlanPanel != null) {
				//        this.connectedPlanPanel.InvalidateGraphics();
				//    }
				//}
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
			//if (this.mode == KlimaBodenMode.KDM_PICK_MODULE) {
			//    KeyDown(e.KeyCode, this.highlightModules);
			//} 
			if (this.mode == EurovalMode.EVM_ADD_RZ || this.mode == EurovalMode.EVM_DEL_RZ) {
			    if (e.KeyCode == Keys.Escape) {
			//        this.layoutAddArea = null;
			//        this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
			//        this.connectedPlanPanel.InvalidateGraphics();
					this.Mode = EurovalMode.EVM_NONE;
					this.connectedPlanPanel.InvalidateGraphics();
			    }
			}
		}

		private bool KeyDown(Keys key) {
			//if (key == Keys.Delete && modules != null) {
			//    List<Circuit> emptyCircuits = new List<Circuit>();
			//    foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
			//        foreach (KlimaFlaechenModul kfm in modules) {
			//            if (c.Row.List.Contains(kfm)) {
			//                c.Row.List.Remove(kfm);
			//            }
			//        }
			//        if (c.Row.List.Count == 0) {
			//            emptyCircuits.Add(c);
			//        }
			//    }
			//    foreach (Circuit emptyCircuit in emptyCircuits) {
			//        this.product.PlannedCircuits.Remove(emptyCircuit);
			//    }
			//    if (this.ProjectChanged != null) {
			//        this.ProjectChanged(this);
			//    }
			//    this.ModuleSelected(this, EventArgs.Empty);
			//    this.ConnectedPlanPanel.InvalidateGraphics();
			//    if (this.ListsNeedUpdate != null) {
			//        this.ListsNeedUpdate(this, EventArgs.Empty);
			//    }
			//    if (this.ProjectChanged != null) {
			//        this.ProjectChanged(this);
			//    }
			//    return true;
			//}
			return false;
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl);
		}

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {
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

				// paint product
				g.FillPath(new SolidBrush(Color.FromArgb(64, Color.Red)), path);

				if (this.mode == EurovalMode.EVM_ADD_RZ) {
					Point2D rzPoint = GetRzPoint(mousePositionInPlan);

					if (rzPoint != Point2D.Zero) {
						foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
							Segment2D line = new Segment2D(point, rzPoint);
							if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
								rzPoint = point;
								break;
							}
						}
						Point2D p = additionalTransformation.TransformTo2D(rzPoint);
						float size = (float)(this.product.AssociatedRoom.AssociatedPlan.Measure * 0.05 * Math.Abs(additionalTransformation.M00));
						Pen pen = new Pen(Color.Blue, 2);
						g.DrawLine(pen, (float)p.X - size, (float)p.Y - size, (float)p.X + size, (float)p.Y + size);
						g.DrawLine(pen, (float)p.X - size, (float)p.Y + size, (float)p.X + size, (float)p.Y - size);
					}

					if (rzStart != Point2D.Zero) {
						List<Segment2D> rzSegments = GetSegments(rzStart);
						double dist = double.MaxValue;
						Segment2D closest = new Segment2D();
						foreach (Segment2D rzSegment in rzSegments) {
							if (rzSegment.GetDistance(mousePositionInPlan) < dist) {
								dist = rzSegment.GetDistance(mousePositionInPlan);
								closest = rzSegment;
							}
						}
						Point2D tempEnd = closest.GetClosestPoint(mousePositionInPlan);
						Pen pen = new Pen(Color.Blue, 2);
						Point2D start = additionalTransformation.TransformTo2D(rzStart);
						Point2D end = additionalTransformation.TransformTo2D(tempEnd);
						g.DrawLine(pen, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					}
				}

				// paint rim
				if (this.product.PlannedRimLength > 0) {
					Brush rzBrush = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.Percent30, Color.FromArgb(255, Color.Red), Color.FromArgb(0, Color.Red));
					foreach (Segment2D rimSegment in this.product.PlannedRimSegments) {
						float width = this.product.PlannedRimWidth > 0 ? this.product.PlannedRimWidth : 5.0f;
						Pen pen = new Pen(rzBrush, (float)((width * 2.0 / 100.0) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * Math.Abs(additionalTransformation.M00)));
						Region oldClip = g.Clip;
						g.Clip = new Region(path);
						Point2D start = additionalTransformation.TransformTo2D(rimSegment.Start);
						Point2D end = additionalTransformation.TransformTo2D(rimSegment.End);
						g.DrawLine(pen, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
						g.Clip = oldClip;
					}
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

				if (drawExpansionGaps) {
					foreach (Segment2D expansionGap in this.Product.AssociatedRoom.AssociatedFloor.ExpansionGaps) {
						Point2D start = additionalTransformation.TransformTo2D(expansionGap.Start);
						Point2D end = additionalTransformation.TransformTo2D(expansionGap.End);
						g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					}
				}
				path.Dispose();
			}
		}

		private Point2D GetRzPoint(Point2D mousePosition) {
			double distance = Double.MaxValue;
			Point2D rzPoint = Point2D.Zero;
			Point2D prevPoint = Point2D.Zero;
			Segment2D line = new Segment2D();
			foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
				if (prevPoint != Point2D.Zero) {
					line = new Segment2D(prevPoint, point);
					if (line.GetDistance(mousePosition) < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
						if (line.GetDistance(mousePosition) < distance) {
							distance = line.GetDistance(mousePosition);
							rzPoint = line.GetClosestPoint(mousePosition);
						}
					}
				}
				prevPoint = point;
			}
			if (rzPoint == Point2D.Zero) {
				line = new Segment2D(prevPoint, this.product.AssociatedRoom.RoomCoordinates[0]);
				if (line.GetDistance(mousePosition) < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
					if (line.GetDistance(mousePosition) < distance) {
						distance = line.GetDistance(mousePosition);
						rzPoint = line.GetClosestPoint(mousePosition);
					}
				}
			}
			return rzPoint;
		}

		private List<Segment2D> GetSegments(Point2D referencePoint) {
			double distance = Double.MaxValue;
			
			Point2D rzPoint = Point2D.Zero;
			Point2D prevPoint = Point2D.Zero;
			List<Segment2D> list = new List<Segment2D>();

			Segment2D line = new Segment2D();
			foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
				if (prevPoint != Point2D.Zero) {
					line = new Segment2D(prevPoint, point);
					if (line.GetDistance(referencePoint) < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
						if (line.GetDistance(referencePoint) < distance) {
							list.Add(line);
						}
					}
				}
				prevPoint = point;
			}

			line = new Segment2D(prevPoint, this.product.AssociatedRoom.RoomCoordinates[0]);
			if (line.GetDistance(referencePoint) < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
				if (line.GetDistance(referencePoint) < distance) {
					list.Add(line);
				}
			}

			return list;
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (button == MouseButtons.Right) {
				rzStart = Point2D.Zero;
				this.Mode = EurovalMode.EVM_NONE;
				this.connectedPlanPanel.InvalidateGraphics();
				return true;
			}
			if (this.Mode == EurovalMode.EVM_ADD_RZ && button == MouseButtons.Left) {
				if (rzStart == Point2D.Zero) {
					rzStart = GetRzPoint(planPoint);
					foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
						Segment2D line = new Segment2D(point, rzStart);
						if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
							rzStart = point;
							break;
						}
					}
				} else {
					Point2D rzEnd = GetRzPoint(planPoint);
					if (rzEnd != Point2D.Zero) {
						foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
							Segment2D line = new Segment2D(point, rzEnd);
							if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
								rzEnd = point;
								break;
							}
						}

						List<Segment2D> rzSegments = GetSegments(rzEnd);
						foreach (Segment2D rzSegment in rzSegments) {
							if (GetSegments(rzStart).Contains(rzSegment)) {
								this.product.PlannedRimSegments.Add(new Segment2D(rzStart, rzEnd));

								float sum = 0;
								foreach (Segment2D segment in this.product.PlannedRimSegments) {
									sum += (float)segment.GetLength() / this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
								}
								this.product.PlannedRimLength = sum;

								rzStart = Point2D.Zero;

								if (this.ProjectChanged != null) {
									this.ProjectChanged(this);
								}
								return true;
							}
						}
					}
				}
			} else if (this.Mode == EurovalMode.EVM_DEL_RZ && button == MouseButtons.Left) {
				double distance = double.MaxValue;
				Segment2D toDelete = new Segment2D();
				foreach (Segment2D rzSegment in this.product.PlannedRimSegments) {
					if (rzSegment.GetDistance(planPoint) < distance) {
						distance = rzSegment.GetDistance(planPoint);
						toDelete = rzSegment;
					}
				}
				if (this.product.PlannedRimSegments.Contains(toDelete)) {
					this.product.PlannedRimSegments.Remove(toDelete);
				}

				float sum = 0;
				foreach (Segment2D segment in this.product.PlannedRimSegments) {
					sum += (float)segment.GetLength() / this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				}
				this.product.PlannedRimLength = sum;
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				return true;
			}
			//if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
			//    Dictionary<KlimaFlaechenModul, Polygon2D> moduleAreas = this.GetModuleAreas();
			//    KlimaFlaechenModul pickedModul = null;
			//    foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> kvp in moduleAreas) {
			//        if (kvp.Value.IsInside(planPoint)) {
			//            pickedModul = kvp.Key;
			//            break;
			//        }
			//    }


			//    if (pickedModul != null) {
			//        Matrix3D rotation = Transformation3D.Rotate(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
			//        Matrix3D invRotation = rotation.GetInverse();

			//        //Polygon2D rotatedAddArea = new Polygon2D();
			//        double top = double.MaxValue;
			//        double bottom = double.MinValue;
			//        double left = double.MaxValue;
			//        double right = double.MinValue;
			//        foreach (Point2D point in this.layoutAddArea) {
			//            Point2D rotatedPoint = invRotation.Transform(point);
			//            if (rotatedPoint.X < left) {
			//                left = rotatedPoint.X;
			//            }
			//            if (rotatedPoint.X > right) {
			//                right = rotatedPoint.X;
			//            }
			//            if (rotatedPoint.Y < top) {
			//                top = rotatedPoint.Y;
			//            }
			//            if (rotatedPoint.Y > bottom) {
			//                bottom = rotatedPoint.Y;
			//            }
			//        }
			//        Point2D referencePoint = invRotation.Transform(new Point2D(pickedModul.GraphPosX, pickedModul.GraphPosY));

			//        double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			//        double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			//        double stepX = width;
			//        if (!this.newModulesXDicht) {
			//            stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			//        }
			//        double stepY = height;
			//        if (!this.newModulesYDicht) {
			//            stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			//        }

			//        double refLeft = referencePoint.X;
			//        while (refLeft - stepX > left) {
			//            refLeft -= stepX;
			//        }
			//        while (refLeft < left) {
			//            refLeft += stepX;
			//        }
			//        double refTop = referencePoint.Y;
			//        while (refTop - stepY > top) {
			//            refTop -= stepY;
			//        }
			//        while (refTop < top) {
			//            refTop += stepY;
			//        }
			//        this.NewModulesOffsetX = refLeft - left;
			//        this.NewModulesOffsetY = refTop - top;

			//        this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
			//        if (this.connectedPlanPanel != null) {
			//            this.connectedPlanPanel.InvalidateGraphics();
			//        }
			//        if (this.ModeChanged != null) {
			//            this.ModeChanged(this, EventArgs.Empty);
			//        }
			//    }
			//}
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.Mode == EurovalMode.EVM_ADD_RZ) {
				return true;
			}
			//if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
			//    if (this.product.GraphConstruction != null) {
			//        if (this.product.GraphConstruction.HitTest(planPoint, pointInControl)) {
			//            this.customCursor = this.product.GraphConstruction.PickCursor;
			//            this.ConnectedPlanPanel.PlanCursor = this.product.GraphConstruction.PickCursor;
			//        } else {
			//            this.customCursor = Cursors.Default;
			//            this.ConnectedPlanPanel.PlanCursor = Cursors.Default;
			//        }
			//    }
			//}
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
			//if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
			//    if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
			//        this.product.GraphConstruction.StartDrag(planPoint, pointInControl);
			//    }
			//} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
			//    this.newModulesOffsetX = 0;
			//    this.newModulesOffsetY = 0;
			//    this.layoutAddAreaStart = planPoint;
			//    this.layoutAddAreaStartScreen = pointInControl;
			//} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
			//    this.lastPlanPoint = planPoint;
			//    this.lastPointInControl = pointInControl;
			//} else if (this.Mode == KlimaBodenMode.KDM_PICK_MODULE) {
			//    if (!this.ShiftPressed) {

			//        /*double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			//        KlimaFlaechenModul pickedModul = null;
			//        foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
			//            Point2D left = rotation.Transform(lane.BorderLeft.Origin);
			//            Point2D right = rotation.Transform(lane.BorderRight.Origin);
			//            if (rotatedPoint.X >= left.X && rotatedPoint.X <= right.X) {
			//                List<KlimaFlaechenModul> modules = this.product.GetModulesInLane(lane.Nr);
			//                foreach (KlimaFlaechenModul module in modules) {
			//                    if (rotatedPoint.Y >= module.GraphPositionInLan && rotatedPoint.Y <= module.GraphPositionInLan + KlimaFlaechenModul.GetModuleHeight(module.ModulType) * measure) {
			//                        pickedModul = module;
			//                        break;
			//                    }
			//                }
			//                if (pickedModul != null) {
			//                    break;
			//                }
			//            }
			//        }
			//        this.moveModules = this.GetAllSelectedModules().Contains(pickedModul);*/
			//        this.dragStartedInControl = pointInControl;
			//        this.dragStartedInPlan = planPoint;
			//        /*if (this.moveModules) {
			//            oldModulPositions = new Dictionary<KlimaFlaechenModul, double>();
			//            foreach (KlimaFlaechenModul modul in this.GetAllSelectedModules()) {
			//                oldModulPositions.Add(modul, modul.GraphPositionInLan);
			//            }
			//        }*/
			//        //dragIsPick = true;
			//    } else {
			//        //this.moveModules = false;
			//        this.dragStartedInControl = pointInControl;
			//        this.dragStartedInPlan = planPoint;
			//        //dragIsPick = true;
			//    }
			//}
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			//if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
			//    if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
			//        this.product.GraphConstruction.MoveDrag(planPoint, pointInControl);
			//        if (this.ProjectChanged != null) {
			//            this.ProjectChanged(this);
			//        }
			//        return true;
			//    }
			//} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
			//    if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
			//        Matrix3D matrix = Transformation3D.Rotate(this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
			//        Point2D rotatedP1 = matrix.Transform(layoutAddAreaStart);
			//        Point2D rotatedP3 = matrix.Transform(planPoint);
			//        this.layoutAddAreaBottomUp = (rotatedP1.Y > rotatedP3.Y);
			//        Point2D rotatedP2 = new Point2D(rotatedP1.X, rotatedP3.Y);
			//        Point2D rotatedP4 = new Point2D(rotatedP3.X, rotatedP1.Y);
			//        matrix = matrix.GetInverse();
			//        //Point2D p2 = matrix.Transform(rotatedP2);
			//        //Point2D p4 = matrix.Transform(rotatedP4);
			//        layoutAddArea = new Polygon2D();
			//        layoutAddArea.Add(layoutAddAreaStart);
			//        layoutAddArea.Add(matrix.Transform(rotatedP2));
			//        layoutAddArea.Add(planPoint);
			//        layoutAddArea.Add(matrix.Transform(rotatedP4));
			//        return true;
			//    }
			//} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH && button == MouseButtons.Left) {
			//    Vector2D vector = planPoint - this.lastPlanPoint;
			//    Matrix3D rotate = Transformation3D.Rotate(this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
			//    vector = rotate.Transform(vector);
			//    this.NewModulesOffsetX += vector.X;
			//    this.NewModulesOffsetY += vector.Y;
			//    this.lastPlanPoint = planPoint;
			//    this.lastPointInControl = pointInControl;
			//    if (this.ConnectedPlanPanel != null) {
			//        this.ConnectedPlanPanel.InvalidateGraphics();
			//    }
			//} else if (this.Mode == KlimaBodenMode.KDM_PICK_MODULE && button == MouseButtons.Left) {
			//    //int deltaY = this.dragStartedInControl.Y - pointInControl.Y;
			//    //Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);

			//    /*if (deltaX * deltaX + deltaY * deltaY > 25) {
			//        dragIsPick = false;
			//    }*/
			//    //if (/*!dragIsPick && */moveModules) {
			//    /*	Point2D rotatedStartPoint = rotation.Transform(this.dragStartedInPlan.Value);
			//        Point2D rotatedCurPoint = rotation.Transform(planPoint);
			//        double delta = rotatedCurPoint.Y - rotatedStartPoint.Y;
			//        double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			//        Console.WriteLine(delta / measure);
			//        Dictionary<int, List<KlimaFlaechenModul>> modulesPerLane = new Dictionary<int, List<KlimaFlaechenModul>>();
			//        foreach (KlimaFlaechenModul modul in this.GetAllSelectedModules()) {
			//            if (!modulesPerLane.ContainsKey(modul.GraphLane)) {
			//                modulesPerLane.Add(modul.GraphLane, new List<KlimaFlaechenModul>());
			//            }
			//            modulesPerLane[modul.GraphLane].Add(modul);
			//        }
			//        foreach (KeyValuePair<int, List<KlimaFlaechenModul>> kvp in modulesPerLane) {
			//            if (kvp.Value.Count > 0) {
			//                KlimaFlaechenModul firstModul = kvp.Value[0];
			//                bool bottomUp = firstModul.GraphPositionInLan < this.oldModulPositions[firstModul] + delta;
			//                kvp.Value.Sort(new KlimaFlaechenModuleComparer(!bottomUp));
			//                PossibleModulLane lane = this.product.GraphConstruction.PossibleLanes[kvp.Key];
			//                foreach (KlimaFlaechenModul modul in kvp.Value) {
			//                    Nullable<double> bestMove = lane.BestMovePossible(modul, this.oldModulPositions[modul] + delta, measure, this.product, bottomUp);
			//                    if (bestMove.HasValue) {
			//                        modul.GraphPositionInLan = bestMove.Value;
			//                    }
			//                }
			//            }
			//        }
			//        if (this.ProjectChanged != null) {
			//            this.ProjectChanged(this);
			//        }
			//        this.connectedPlanPanel.InvalidateGraphics();
			//    } else {*/
			//        this.dragEndedInControl = pointInControl;
			//        this.dragEndedInPlan = planPoint;
			//        this.connectedPlanPanel.InvalidateGraphics();
			//    //}
			//}
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
			//if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
			//    this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
			//    if (this.ModeChanged != null) {
			//        this.ModeChanged(this, EventArgs.Empty);
			//    }
			//} else if (this.mode == KlimaBodenMode.KDM_PICK_MODULE) {
			//    if (this.dragStartedInPlan.HasValue) {
			//        dragEndedInPlan = planPoint;

			//        Matrix3D rotation = Transformation3D.Rotate(this.product.AssociatedRoom.AssociatedPlan.Rotation * Math.PI / 180.0);
			//        Matrix3D invRotation = rotation.GetInverse();

			//        Point2D p1 = rotation.Transform(dragStartedInPlan.Value);
			//        Point2D p3 = rotation.Transform(dragEndedInPlan.Value);
			//        Point2D p2 = new Point2D(p1.X, p3.Y);
			//        Point2D p4 = new Point2D(p3.X, p1.Y);

			//        Polygon2D selection = new Polygon2D(new Point2D[] { dragStartedInPlan.Value, invRotation.Transform(p2), dragEndedInPlan.Value, invRotation.Transform(p4) });

			//        List<KlimaFlaechenModul> selectedModules;
			//        if (this.ShiftPressed) {
			//            selectedModules = new List<KlimaFlaechenModul>(this.HighlightModules);
			//        } else {
			//            selectedModules = new List<KlimaFlaechenModul>();
			//        }

			//        foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
			//            foreach (KlimaFlaechenModul modul in c.Row.List) {
			//                Polygon2D modulArea = GetModuleArea(modul);
			//                if (AllPointsInside(selection, modulArea)) {
			//                    selectedModules.Add(modul);
			//                }
			//                if (AllPointsInside(modulArea, selection)) {
			//                    selectedModules.Add(modul);
			//                }
			//            }
			//        }

			//        if (selectedModules.Count > 0) {
			//            this.HighlightModules = selectedModules;
			//        } else {
			//            this.HighlightModules = null;
			//        }

			//        this.dragStartedInControl = null;
			//        this.dragStartedInPlan = null;
			//        this.dragEndedInControl = null;
			//        this.dragEndedInPlan = null;
			//        if (this.ConnectedPlanPanel != null) {
			//            this.ConnectedPlanPanel.InvalidateGraphics();
			//        }
			//        if (this.ModuleSelected != null) {
			//            this.ModuleSelected(this, EventArgs.Empty);
			//        }
			//        return true;
			//    }
			//}
			return false;
		}

		internal bool ShiftPressed {
			get { return (Control.ModifierKeys & (Keys.Shift | Keys.ShiftKey | Keys.LShiftKey | Keys.RShiftKey)) != Keys.None; }
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			return KeyDown(key);
		}

		#endregion

		public event ProjectChangedHandler ProjectChanged;

		internal void DrawDxf(DxfModel model, DxfLayer modulLayer, DxfLayer floorConstructionLayer) {
			Matrix4D additionalTransformation = Matrix4D.Identity;

			//if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.CeilingCoordinatesToUse != null) {
			//    if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
			//        if (this.product.GraphConstruction != null) {
			//            this.product.GraphConstruction.PaintDxf(model, floorConstructionLayer);
			//        }
			//    }

			//    if (this.mode != KlimaBodenMode.KDM_CONSTRUCTION) {
			//        Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			//        Matrix3D invRotation = rotation.GetInverse();

			//        List<KlimaFlaechenModul> selectedModules = this.GetAllSelectedModules();
			//        foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
			//            foreach (KlimaFlaechenModul modul in circuit.Row.List) {
			//                this.DrawDxfModule(modul.ModulType, modul.Orientation, invRotation.Transform(new Point2D(modul.GraphPosX, modul.GraphPosY)), additionalTransformation, model, modulLayer, modul.GraphBottomUp, circuit.CircuitColor);
			//            }
			//        }
			//    }
			//}
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
