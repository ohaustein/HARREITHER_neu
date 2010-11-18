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

namespace Europlan.Common {
	public partial class ModulKlimaDeckePlanner : Component, IProductPlanner {

		public enum KlimaDeckeMode {
			KDM_NONE,
			KDM_CONSTRUCTION,
			KDM_LAYOUT_ADD_AREA,
			KDM_PICK_MODULE
		}

		public ModulKlimaDeckePlanner() {
			InitializeComponent();
		}

		public ModulKlimaDeckePlanner(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private ModulKlimaDeckeProduct product;
		private KlimaDeckeMode mode = KlimaDeckeMode.KDM_NONE;
		private Cursor customCursor = null;
		private bool automaticOrientation = true;
		private bool automaticRows = true;

		public bool AutomaticOrientation {
			get { return this.automaticOrientation; }
			set { this.automaticOrientation = value; }
		}

		public bool AutomaticRows {
			get { return this.automaticRows; }
			set { this.automaticRows = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulKlimaDeckeProduct Product {
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

		public KlimaDeckeMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				if (this.mode != KlimaDeckeMode.KDM_PICK_MODULE) {
					this.HighlightModules = null;
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
			if (this.mode == KlimaDeckeMode.KDM_PICK_MODULE) {
				if (e.KeyCode == Keys.Delete && this.highlightModules != null) {
					List<KlimaFlaechenList> emptyRows = new List<KlimaFlaechenList>();
					List<ModulDeckeSubArea> emptySubAreas = new List<ModulDeckeSubArea>();
					List<Circuit> emptyCircuits = new List<Circuit>();
					foreach (Circuit c in this.product.PlannedCircuits) {
						ModulDeckeCircuit dc = c as ModulDeckeCircuit;
						foreach (ModulDeckeSubArea sa in dc.SubAreas) {
							foreach (KlimaFlaechenList kfl in sa.Rows) {
								foreach (KlimaFlaechenModul kfm in this.highlightModules) {
									if (kfl.List.Contains(kfm)) {
										kfl.List.Remove(kfm);
									}
								}
								if (kfl.List.Count == 0) {
									emptyRows.Add(kfl);
								}
							}
							foreach (KlimaFlaechenList emptyRow in emptyRows) {
								sa.Rows.Remove(emptyRow);
							}
							emptyRows.Clear();
							if (sa.Rows.Count == 0) {
								emptySubAreas.Add(sa);
							}
						}
						foreach (ModulDeckeSubArea emptySubArea in emptySubAreas) {
							dc.SubAreas.Remove(emptySubArea);
						}
						emptySubAreas.Clear();
						if (dc.SubAreas.Count == 0) {
							emptyCircuits.Add(dc);
						}
					}
					foreach (Circuit emptyCircuit in emptyCircuits) {
						this.product.PlannedCircuits.Remove(emptyCircuit);
					}
					this.ModuleSelected(this, new ModuleSelectedEventArgs());
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		public event EventHandler ListsNeedUpdate;

		private double breite = 0.1; // meter
		private double abstand = 0.5; // meter

		public delegate void AddModuleDelegate(ref double y, double start, double end, double step, ref bool left, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin, out bool added);
			//Graphics g, Matrix4D additionalTransformation, Matrix3D invRotation, double step, ref bool left, PossibleModulLane lane, Point2D borderLeftOrigin, ref double y, bool bottomUp, double start, double end);

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.CeilingCoordinatesToUse != null) {
				GraphicsPath path = new GraphicsPath();
				List<PointF> transformedPoints = new List<PointF>();
				foreach (Point2D point in this.product.AssociatedRoom.CeilingCoordinatesToUse) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
				}
				path.AddPolygon(transformedPoints.ToArray());
				//e.Graphics.DrawPath(System.Drawing.Pens.Red, path);
				//Region clipEnabled = new Region(path);
				Region clipDisabled = new Region();
				clipDisabled.MakeInfinite();
				clipDisabled.Exclude(path);
				path.Dispose();
				Color c = Color.Black;
				if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
					c = Color.White;
				}
				Brush b = new SolidBrush(c);
				//e.Graphics.FillRegion(b, clipRegion);
				//c = Color.FromArgb(128, Color.Black);
				//b = new SolidBrush(c);
				b = new HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));
				e.Graphics.FillRegion(b, clipDisabled);

				//if (this.product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
				//    List<PointF> unusedPoints = new List<PointF>();
				//    foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.CeilingUnusedAreaCoordinates) {
				//        foreach (Point2D point in unusedArea) {
				//            Point2D tmp = additionalTransformation.TransformTo2D(point);
				//            unusedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
				//        }
				//        e.Graphics.FillPolygon(b, unusedPoints.ToArray());
				//        unusedPoints.Clear();
				//    }
				//}

				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.Paint(e.Graphics, this.Mode);
					}
				}

				if (this.mode != KlimaDeckeMode.KDM_CONSTRUCTION) {
					foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
						Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
						Matrix3D invRotation = rotation.GetInverse();

						double left = rotation.Transform(lane.BorderLeft.Origin).X;

						List<KlimaFlaechenModul> modules = lane.GetModulesInThisLane(this.product);
						foreach (KlimaFlaechenModul module in modules) {
							bool highlight = false;
							if (this.highlightCircuit != null && this.highlightCircuit.ContainsModul(module)) {
								highlight = true;
							}
							if (this.highlightSubArea != null && this.highlightSubArea.ContainsModul(module)) {
								highlight = true;
							}
							if (this.highlightRow != null && this.highlightRow.ContainsModul(module)) {
								highlight = true;
							}
							if (this.highlightModules != null && this.highlightModules.Contains(module)) {
								highlight = true;
							}
							this.DrawModule(module.ModulType, module.Orientation, invRotation.Transform(new Point2D(left, module.GraphPositionInLan)), additionalTransformation, e.Graphics, module.GraphBottomUp, highlight);
						}
					}
				}

				if (this.layoutAddArea != null) {
					PointF[] drawArea = new PointF[this.layoutAddArea.Count];
					for (int i = 0; i < this.layoutAddArea.Count; i++) {
						Point2D tmp = additionalTransformation.TransformTo2D(this.layoutAddArea[i]);
						drawArea[i] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					e.Graphics.DrawPolygon(Pens.Red, drawArea);

					this.AddModulesForLayoutArea(delegate(ref double y, double start, double end, double step, ref bool left, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin, out bool added) {
						added = this.TryDrawModule(e.Graphics, additionalTransformation, ref y, start, end, step, ref left, this.layoutAddAreaBottomUp, invRotation, lane, borderLeftOrigin);
					});
				}

				if (this.product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
					List<PointF> unusedPoints = new List<PointF>();
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.CeilingUnusedAreaCoordinates) {
						foreach (Point2D point in unusedArea) {
							Point2D tmp = additionalTransformation.TransformTo2D(point);
							unusedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
						}
						PointF[] pointArray = unusedPoints.ToArray();
						e.Graphics.DrawPolygon(new Pen(c), pointArray);
						e.Graphics.FillPolygon(b, pointArray);
						unusedPoints.Clear();
					}
				}
			}
		}

		private int AddModulesForLayoutArea(AddModuleDelegate doIt) {
			Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			Matrix3D invRotation = rotation.GetInverse();

			bool added = false;
			int count = 0;
			if (alignRectangle) {
				// rectangle aligned to schienen
				Segment2D topSeg = new Segment2D(rotation.Transform(this.layoutAddArea[0]), rotation.Transform(this.layoutAddArea[3]));
				double start = topSeg.Start.Y;
				double end = rotation.Transform(this.layoutAddArea[1]).Y;
				double step = KlimaFlaechenModul.GetModuleHeight(this.moduleTypeToAdd) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				bool left;
				foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
					left = this.startingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
					Point2D borderLeftOrigin = rotation.Transform(lane.BorderLeft.Origin);
					borderLeftOrigin.Y = topSeg.Start.Y;
					Point2D borderRighOrigin = rotation.Transform(lane.BorderRight.Origin);
					borderLeftOrigin.Y = topSeg.Start.Y;
					if (Line2D.Intersects(new Line2D(borderLeftOrigin, new Vector2D(0, 1)), topSeg) && Line2D.Intersects(new Line2D(borderRighOrigin, new Vector2D(0, 1)), topSeg)) {
						if (this.layoutAddAreaBottomUp) {
							for (double y = start - step; y > end; y -= step) {
								doIt(ref y, start, end, step, ref left, invRotation, lane, borderLeftOrigin, out added);
								if (added) {
									count++;
								}
							}
						} else {
							for (double y = start; y < end - step; y += step) {
								doIt(ref y, start, end, step, ref left, invRotation, lane, borderLeftOrigin, out added);
								if (added) {
									count++;
								}
							}
						}
					}
				}
			} else {
				// rectangle not aligned to schienen
				// the optimalLayout flag is ignored in this mode as this will always be layouted optimal
				Polygon2D rotatedLayoutAddArea = new Polygon2D();
				foreach (Point2D p in this.layoutAddArea) {
					rotatedLayoutAddArea.Add(rotation.Transform(p));
				}

				double step = KlimaFlaechenModul.GetModuleHeight(this.moduleTypeToAdd) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				bool left;
				foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
					left = this.startingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
					Nullable<Point2D> topLeftIntersection = null;
					Nullable<Point2D> bottomLeftIntersection = null;
					
					Point2D borderLeftOrigin = rotation.Transform(lane.BorderLeft.Origin);

					if (this.GetIntersections(out topLeftIntersection, out bottomLeftIntersection, new Line2D(borderLeftOrigin, new Vector2D(0, 1)), rotatedLayoutAddArea /*, out bottomUpLeft*/)) {
						Point2D borderRightOrigin = rotation.Transform(lane.BorderRight.Origin);

						Nullable<Point2D> topRightIntersection = null;
						Nullable<Point2D> bottomRightIntersection = null;
						if (this.GetIntersections(out topRightIntersection, out bottomRightIntersection, new Line2D(borderRightOrigin, new Vector2D(0, 1)), rotatedLayoutAddArea /*, out bottomUpRight*/)) {
							double top = Math.Max(topLeftIntersection.Value.Y, topRightIntersection.Value.Y);
							double bottom = Math.Min(bottomLeftIntersection.Value.Y, bottomRightIntersection.Value.Y);
							if (top > bottom) {
								continue;
							}
							if (this.layoutAddAreaBottomUp /*bottomUpLeft && bottomUpRight*/) {
								for (double y = bottom - step; y > top; y -= step) {
									doIt(ref y, top, bottom, step, ref left, invRotation, lane, borderLeftOrigin, out added);
									if (added) {
										count++;
									}
								}
							} else {
								for (double y = top; y < bottom - step; y += step) {
									doIt(ref y, top, bottom, step, ref left, invRotation, lane, borderLeftOrigin, out added);
									if (added) {
										count++;
									}
								}
							}
						}

					}
				}
			}
			return count;
		}

		/// <summary>
		/// This method only is guaranteed to work if the passed poylgon really is a rectangle. this will not be checked inside this method
		/// </summary>
		/// <param name="topIntersection"></param>
		/// <param name="bottomIntersection"></param>
		/// <param name="line"></param>
		/// <param name="polygon"></param>
		private bool GetIntersections(out Nullable<Point2D> topIntersection, out Nullable<Point2D> bottomIntersection, Line2D line, Polygon2D  rectangle/*Point2D p1, Point2D p2, Point2D p3, Point2D p4, out bool bottomUp*/) {
			List<Segment2D> segments = new List<Segment2D>();
			Polygon2D.GetSegments(rectangle, segments);
			topIntersection = null;
			bottomIntersection = null;
			foreach (Segment2D segment in segments) {
				Nullable<Point2D> intersection = Line2D.GetIntersection(line, segment);
				if (intersection.HasValue && (!topIntersection.HasValue || intersection.Value.Y < topIntersection.Value.Y)) {
					topIntersection = intersection;
				}
				if (intersection.HasValue && (!bottomIntersection.HasValue ||intersection.Value.Y > bottomIntersection.Value.Y)) {
					bottomIntersection = intersection;
				}
			}
			if (!topIntersection.HasValue || !bottomIntersection.HasValue || topIntersection.Equals(bottomIntersection)) {
				topIntersection = null;
				bottomIntersection = null;
			}

			return topIntersection != null;
		}

		private ModulDeckeCircuit highlightCircuit = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulDeckeCircuit HighlightCircuit {
			get { return this.highlightCircuit; }
			set {
				this.highlightCircuit = value;
				this.highlightSubArea = null;
				this.highlightRow = null;
				this.highlightModules = null;
				this.ConnectedPlanPanel.InvalidateGraphics();
			}
		}

		private ModulDeckeSubArea highlightSubArea = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulDeckeSubArea HighlightSubArea {
			get { return this.highlightSubArea; }
			set {
				this.highlightSubArea = value;
				this.highlightCircuit = null;
				this.highlightRow = null;
				this.highlightModules = null;
				this.ConnectedPlanPanel.InvalidateGraphics();
			}
		}

		private KlimaFlaechenList highlightRow = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public KlimaFlaechenList HighlightRow {
			get { return this.highlightRow; }
			set {
				this.highlightRow = value;
				this.highlightCircuit = null;
				this.highlightSubArea = null;
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
				this.highlightRow = null;
				this.highlightCircuit = null;
				this.highlightSubArea = null;
				this.highlightModules = value;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		public List<KlimaFlaechenModul> GetAllSelectedModules() {
			List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
			if (this.HighlightCircuit != null) {
				foreach (ModulDeckeSubArea subArea in this.HighlightCircuit.SubAreas) {
					foreach (KlimaFlaechenList row in subArea.Rows) {
						foreach (KlimaFlaechenModul modul in row.List) {
							modules.Add(modul);
						}
					}
				}
			} else if (this.HighlightSubArea != null) {
				foreach (KlimaFlaechenList row in this.HighlightSubArea.Rows) {
					foreach (KlimaFlaechenModul modul in row.List) {
						modules.Add(modul);
					}
				}
			} else if (this.HighlightRow != null) {
				foreach (KlimaFlaechenModul modul in this.HighlightRow.List) {
					modules.Add(modul);
				}
			} else if (this.HighlightModules != null) {
				foreach (KlimaFlaechenModul modul in this.HighlightModules) {
					modules.Add(modul);
				}
			}
			return modules;
		}

		private bool TryDrawModule(Graphics g, Matrix4D additionalTransformation, ref double y, double start, double end, double step, ref bool left, bool bottomUp, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin) {
			foreach (FreeModulLaneArea area in lane.GetFreeAreas(this.product, null)) {
				if (!this.alignRectangle || this.optimalLayout) {
					Nullable<double> bestStart = area.BestStart(y, y + step, bottomUp);
					if (bestStart.HasValue) {
						y = bestStart.Value;
						if (bestStart.Value >= start && bestStart.Value + step <= end) {
							Point2D tmp = invRotation.Transform(new Point2D(borderLeftOrigin.X, y));
							this.DrawModule(this.moduleTypeToAdd, null /*left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT*/, tmp, additionalTransformation, g, false, true);
							left = !left;
							return true;
						}
						break;
					}
				} else {
					if (area.Fits(y, y + step)) {
						Point2D tmp = invRotation.Transform(new Point2D(borderLeftOrigin.X, y));
						this.DrawModule(this.moduleTypeToAdd, null /*left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT*/, tmp, additionalTransformation, g, false, true);
						left = !left;
						return true;
					}
				}
			}
			return false;
		}

		private bool TryAddModule(Matrix4D additionalTransformation, ref double y, double start, double end, double step, ref bool left, bool bottomUp, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin, Dictionary<PossibleModulLane ,KlimaFlaechenList> laneToRowMapping, ModulDeckeSubArea subArea, KlimaFlaechenList row, List<KlimaFlaechenModul> modulesAdded, bool tryToFindRow) {
			foreach (FreeModulLaneArea area in lane.GetFreeAreas(this.product, null)) {
				if (!this.alignRectangle || this.optimalLayout) {
					Nullable<double> bestStart = area.BestStart(y, y + step, bottomUp);
					if (bestStart.HasValue) {
						y = bestStart.Value;
						if (bestStart.Value >= start && bestStart.Value + step <= end) {
							KlimaFlaechenModul modul = new KlimaFlaechenModul(moduleTypeToAdd, left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
							modul.GraphLane = lane.Nr;
							modul.GraphPositionInLan = bestStart.Value;
							modul.GraphBottomUp = bottomUp;
							KlimaFlaechenList usedRow = null;
							if (tryToFindRow) {
								List<KlimaFlaechenModulWithRow> modulesInLane = this.product.GetModulesInLaneWithRow(lane.Nr);
								//modulesInLane.Sort(new KlimaFlaechenModuleComparer(true));
								double bestPosBefore = double.MinValue;
								double bestPosAfter = double.MaxValue;
								foreach (KlimaFlaechenModulWithRow modulInLane in modulesInLane) {
									if (modulInLane.modul.GraphPositionInLan < modul.GraphPositionInLan && modulInLane.modul.GraphPositionInLan > bestPosBefore) {
										bestPosBefore = modulInLane.modul.GraphPositionInLan;
										usedRow = modulInLane.row;
									}
									if (usedRow == null && modulInLane.modul.GraphPositionInLan > modul.GraphPositionInLan && modulInLane.modul.GraphPositionInLan < bestPosAfter) {
										bestPosAfter = modulInLane.modul.GraphPositionInLan;
										usedRow = modulInLane.row;
									}
								}
							} else if (row != null) {
								usedRow = row;
							}
							if (usedRow == null) {
								if (laneToRowMapping.ContainsKey(lane)) {
									usedRow = laneToRowMapping[lane];
								} else {
									usedRow = new KlimaFlaechenList();
									subArea.Rows.Add(usedRow);
									laneToRowMapping.Add(lane, usedRow);
								}
							}
							usedRow.List.Add(modul);
							modulesAdded.Add(modul);
							left = !left;
							return true;
						}
						break;
					}
				} else {
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
				}
			}
			return false;
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.mode == KlimaDeckeMode.KDM_PICK_MODULE) {
				if (this.product == null || this.product.AssociatedRoom == null ||
					this.product.AssociatedRoom.AssociatedPlan == null || this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
					return false;
				}
				Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
				Point2D rotatedPoint = rotation.Transform(planPoint);
				double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				bool moduleFound = false;
				foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
					Point2D left = rotation.Transform(lane.BorderLeft.Origin);
					Point2D right = rotation.Transform(lane.BorderRight.Origin);
					if (rotatedPoint.X >= left.X && rotatedPoint.X <= right.X) {
						List<KlimaFlaechenModul> modules = this.product.GetModulesInLane(lane.Nr);
						foreach (KlimaFlaechenModul module in modules) {
							if (rotatedPoint.Y >= module.GraphPositionInLan && rotatedPoint.Y <= module.GraphPositionInLan + KlimaFlaechenModul.GetModuleHeight(module.ModulType) * measure) {
								moduleFound = true;
								if (this.ShiftPressed) {
									if (this.HighlightModules == null) {
										this.HighlightModules = new List<KlimaFlaechenModul>();
									}
									if (this.HighlightModules.Contains(module)) {
										this.HighlightModules.Remove(module);
									} else {
										this.HighlightModules.Add(module);
									}
									//this.ConnectedPlanPanel.InvalidateGraphics();
									if (this.ModuleSelected != null) {
										this.ModuleSelected(this, new ModuleSelectedEventArgs(this.HighlightModules));
									}
								} else {
									//List<KlimaFlaechenModul> highlightedModules = new List<KlimaFlaechenModul>();
									//highlightedModules.Add(module);
									//this.HighlightModules = highlightedModules;
									if (this.HighlightModules == null) {
										this.HighlightModules = new List<KlimaFlaechenModul>();
									} else {
										this.HighlightModules.Clear();
									}
									this.HighlightModules.Add(module);
									if (this.ModuleSelected != null) {
										this.ModuleSelected(this, new ModuleSelectedEventArgs(module));
									}
								}
							}
						}
					}
				}
				if (!moduleFound && !this.ShiftPressed) {
					this.HighlightModules = null;
					if (this.ModuleSelected != null) {
						this.ModuleSelected(this, new ModuleSelectedEventArgs());
					}
					return true;
				} else if (moduleFound) {
					return true;
				}
			}
			return false;
		}

		public class ModuleSelectedEventArgs : EventArgs {
			public KlimaFlaechenModul modul;
			public List<KlimaFlaechenModul> modules;

			public ModuleSelectedEventArgs() {
				this.modul = null;
				this.modules = null;
			}

			public ModuleSelectedEventArgs(KlimaFlaechenModul modul) {
				this.modul = modul;
				this.modules = null;
			}

			public ModuleSelectedEventArgs(List<KlimaFlaechenModul> modules) {
				this.modules = modules;
				this.modul = null;
			}
		}

		public event EventHandler<ModuleSelectedEventArgs> ModuleSelected;

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
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
		
		private Point dragStartedInControl;
		private Point2D dragStartedInPlan;
		private bool dragIsPick;
		private Dictionary<KlimaFlaechenModul, double> oldModulPositions;
		private bool moveModules = false;

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.StartDrag(planPoint, pointInControl);
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				this.layoutAddAreaStart = planPoint;
				this.layoutAddAreaStartScreen = pointInControl;
			} else if (this.Mode == KlimaDeckeMode.KDM_PICK_MODULE) {
				Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
				Point2D rotatedPoint = rotation.Transform(planPoint);
				double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
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
				this.moveModules = this.GetAllSelectedModules().Contains(pickedModul);
					if (this.moveModules) {
					this.dragStartedInControl = pointInControl;
					this.dragStartedInPlan = planPoint;
					oldModulPositions = new Dictionary<KlimaFlaechenModul, double>();
					foreach (KlimaFlaechenModul modul in this.GetAllSelectedModules()) {
						oldModulPositions.Add(modul, modul.GraphPositionInLan);
					}
				}
				dragIsPick = true;
			}
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.MoveDrag(planPoint, pointInControl);
					return true;
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					if (alignRectangle) {
						Matrix3D matrix = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
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
					} else {
						Matrix3D matrix = Transformation3D.Rotate(this.product.AssociatedRoom.AssociatedPlan.Rotation * Math.PI / 180.0);
						Point2D rotatedP1 = matrix.Transform(layoutAddAreaStart);
						Point2D rotatedP3 = matrix.Transform(planPoint);
						//this.layoutAddAreaBottomUp = this.layoutAddAreaStartScreen.X < pointInControl.X;
						if (this.product.GraphConstruction.RotationRelativeToPlan <= 45 || this.product.GraphConstruction.RotationRelativeToPlan > 135) {
							if (this.product.GraphConstruction.RotationRelativeToPlan > 135) {
								this.layoutAddAreaBottomUp = this.layoutAddAreaStartScreen.Y < pointInControl.Y;
							} else {
								this.layoutAddAreaBottomUp = this.layoutAddAreaStartScreen.Y > pointInControl.Y;
							}
						} else {
							this.layoutAddAreaBottomUp = this.layoutAddAreaStartScreen.X < pointInControl.X;
						}
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

						/*layoutAddArea = new Polygon2D();
						layoutAddArea.Add(layoutAddAreaStart);
						layoutAddArea.Add(new Point2D(layoutAddAreaStart.X, planPoint.Y));
						layoutAddArea.Add(planPoint);
						layoutAddArea.Add(new Point2D(planPoint.X, layoutAddAreaStart.Y));
						return true;*/
					}
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_PICK_MODULE && button == MouseButtons.Left) {
				int deltaX = this.dragStartedInControl.X - pointInControl.X;
				int deltaY = this.dragStartedInControl.Y - pointInControl.Y;
				Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
				Point2D rotatedStartPoint = rotation.Transform(this.dragStartedInPlan);
				Point2D rotatedCurPoint = rotation.Transform(planPoint);
				double delta = rotatedCurPoint.Y - rotatedStartPoint.Y;

				if (deltaX * deltaX + deltaY * deltaY > 25) {
					dragIsPick = false;
				}
				if (!dragIsPick && moveModules) {
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					Console.WriteLine(delta / measure);
					// TODO move modules
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
					this.connectedPlanPanel.InvalidateGraphics();
				}
			}
			return false;
		}

		public class KlimaFlaechenModuleComparer : IComparer<KlimaFlaechenModul> {
			private bool ascending;

			public KlimaFlaechenModuleComparer(bool ascending) {
				this.ascending = ascending;
			}

			#region IComparer<KlimaFlaechenModul> Members
			public int Compare(KlimaFlaechenModul x, KlimaFlaechenModul y) {
				int result;
				if (ascending) {
					result = x.GraphLane.CompareTo(y.GraphLane);
					if (result == 0) {
						result = x.GraphPositionInLan.CompareTo(y.GraphPositionInLan);
					}
				} else {
					result = y.GraphLane.CompareTo(x.GraphLane);
					if (result == 0) {
						result = y.GraphPositionInLan.CompareTo(x.GraphPositionInLan);
					}
				}
				return result;
			}
			#endregion
		}

		public class KlimaFlaechenModuleWithRowComparer : IComparer<KlimaFlaechenModulWithRow> {
			private bool ascending;

			public KlimaFlaechenModuleWithRowComparer(bool ascending) {
				this.ascending = ascending;
			}

			#region IComparer<KlimaFlaechenModulWithRow> Members
			public int Compare(KlimaFlaechenModulWithRow x, KlimaFlaechenModulWithRow y) {
				int result;
				if (ascending) {
					result = x.modul.GraphLane.CompareTo(y.modul.GraphLane);
					if (result == 0) {
						result = x.modul.GraphPositionInLan.CompareTo(y.modul.GraphPositionInLan);
					}
				} else {
					result = y.modul.GraphLane.CompareTo(x.modul.GraphLane);
					if (result == 0) {
						result = y.modul.GraphPositionInLan.CompareTo(x.modul.GraphPositionInLan);
					}
				}
				return result;
			}
			#endregion
		}

		internal bool ShiftPressed {
			get { return (Control.ModifierKeys & (Keys.Shift | Keys.ShiftKey | Keys.LShiftKey | Keys.RShiftKey)) != Keys.None; }
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.mode == KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				if (this.layoutAddArea != null) {
					ModulDeckeSubArea newSubArea = null;
					ModulDeckeCircuit newCircuit = null;
					ModulDeckeSubArea oldSubArea = null;
					KlimaFlaechenList oldRow = null;
					if (this.highlightCircuit == null && highlightSubArea == null && highlightRow == null) {
						newCircuit = new ModulDeckeCircuit();
						newSubArea = newCircuit.SubAreas[0];
						newSubArea.Rows.Clear();
					} else if (this.highlightSubArea == null && highlightRow == null) {
						newSubArea = new ModulDeckeSubArea();
						newSubArea.Rows.Clear();
					} else if (this.highlightRow == null) {
						oldSubArea = this.highlightSubArea;
					} else {
						oldRow = this.highlightRow;
					}
					Dictionary<PossibleModulLane, KlimaFlaechenList> laneToRowMapping = new Dictionary<PossibleModulLane, KlimaFlaechenList>();
					List<KlimaFlaechenModul> modulesAdded = new List<KlimaFlaechenModul>();
					int count = this.AddModulesForLayoutArea(delegate(ref double y, double start, double end, double step, ref bool left, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin, out bool added) {
						added = this.TryAddModule(this.ConnectedPlanPanel.PlanTransformation, ref y, start, end, step, ref left, this.layoutAddAreaBottomUp, invRotation, lane, borderLeftOrigin, laneToRowMapping, (newSubArea != null ? newSubArea : oldSubArea), oldRow, modulesAdded, this.automaticRows);
					});

					if (count > 0) {
						if (!this.product.ContainsModules && newCircuit != null) {
							this.product.PlannedCircuits.Clear();
						}
						if (newCircuit != null && newCircuit.CountModules() > 0) {
							this.product.PlannedCircuits.Add(newCircuit);
						} else if (newSubArea != null && newSubArea.CountModules() > 0) {
							this.highlightCircuit.SubAreas.Add(newSubArea);
						}
						if (this.automaticOrientation) {
							foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
								List<KlimaFlaechenModulWithRow> modules = this.product.GetModulesInLaneWithRow(lane.Nr);
								modules.Sort(new KlimaFlaechenModuleWithRowComparer(false));
								Nullable<bool> left = null;
								KlimaFlaechenList lastRow = null;
								foreach (KlimaFlaechenModulWithRow moduleWithRow in modules) {
									if (left.HasValue && lastRow == moduleWithRow.row && modulesAdded.Contains(moduleWithRow.modul)) {
										moduleWithRow.modul.Orientation = left.Value ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
										left = !left;
									} else {
										lastRow = moduleWithRow.row;
										left = moduleWithRow.modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
									}
								}
								modules.Sort(new KlimaFlaechenModuleWithRowComparer(true));
								left = null;
								lastRow = null;
								foreach (KlimaFlaechenModulWithRow moduleWithRow in modules) {
									if (left.HasValue && lastRow == moduleWithRow.row && modulesAdded.Contains(moduleWithRow.modul)) {
										moduleWithRow.modul.Orientation = left.Value ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
										left = !left;
									} else {
										lastRow = moduleWithRow.row;
										left = moduleWithRow.modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
									}
								}
							}
						}
						/*foreach (ModulDeckeCircuit circuit in this.product.PlannedCircuits) {
							foreach (ModulDeckeSubArea subArea in circuit.SubAreas) {
								foreach (KlimaFlaechenList row in subArea.Rows) {
									if (row.List.Count > 1) {
										row.List.Sort(new KlimaFlaechenModuleComparer(true));
										bool left = (row.List[0].Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
										foreach (KlimaFlaechenModul modul in row.List) {
											modul.Orientation = (left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
											left = !left;
										}
									}
								}
							}
						}*/
						if (this.ListsNeedUpdate != null) {
							this.ListsNeedUpdate(this, EventArgs.Empty);
						}
					}

					this.layoutAddArea = null;
					return true;
				}
			} else if (this.mode == KlimaDeckeMode.KDM_PICK_MODULE) {
				if (dragIsPick) {
					if (this.product == null || this.product.AssociatedRoom == null ||
						this.product.AssociatedRoom.AssociatedPlan == null || this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
						return false;
					}
					Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
					Point2D rotatedPoint = rotation.Transform(planPoint);
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					bool moduleFound = false;
					foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
						Point2D left = rotation.Transform(lane.BorderLeft.Origin);
						Point2D right = rotation.Transform(lane.BorderRight.Origin);
						if (rotatedPoint.X >= left.X && rotatedPoint.X <= right.X) {
							List<KlimaFlaechenModul> modules = this.product.GetModulesInLane(lane.Nr);
							foreach (KlimaFlaechenModul module in modules) {
								if (rotatedPoint.Y >= module.GraphPositionInLan && rotatedPoint.Y <= module.GraphPositionInLan + KlimaFlaechenModul.GetModuleHeight(module.ModulType) * measure) {
									moduleFound = true;
									if (this.ShiftPressed) {
										if (this.HighlightModules == null) {
											this.HighlightModules = this.GetAllSelectedModules();
										}
										if (this.HighlightModules.Contains(module)) {
											this.HighlightModules.Remove(module);
										} else {
											this.HighlightModules.Add(module);
										}
										if (this.ModuleSelected != null) {
											this.ModuleSelected(this, new ModuleSelectedEventArgs(this.HighlightModules));
										}
									} else {
										if (this.HighlightModules == null) {
											this.HighlightModules = new List<KlimaFlaechenModul>();
										} else {
											this.HighlightModules.Clear();
										}
										this.HighlightModules.Add(module);
										if (this.ModuleSelected != null) {
											this.ModuleSelected(this, new ModuleSelectedEventArgs(module));
										}
									}
								}
							}
						}
					}
					if (!moduleFound && !this.ShiftPressed) {
						this.HighlightModules = null;
						if (this.ModuleSelected != null) {
							this.ModuleSelected(this, new ModuleSelectedEventArgs());
						}
						return true;
					} else if (moduleFound) {
						return true;
					}
				}
			}
			return false;
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			return false;
		}

		public void DrawModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, Matrix4D additionalTransformation, Graphics g, bool bottomUp, bool highlight) {
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

			PointF topLeft = new PointF((float)topLeft2D.X, (float)topLeft2D.Y);
			PointF topRight = new PointF((float)topRight2D.X, (float)topRight2D.Y);
			PointF bottomRight = new PointF((float)bottomRight2D.X, (float)bottomRight2D.Y);
			PointF bottomLeft = new PointF((float)bottomLeft2D.X, (float)bottomLeft2D.Y);

			Color c;
			if (highlight) {
				c = Color.FromArgb(128, 0, 240, 0);
			} else {
				c = Color.FromArgb(128, 0, 128, 0);
			}

			Pen p = new Pen(c);
			Brush b = new SolidBrush(Color.FromArgb(64, c));
			if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				g.DrawLines(p, new PointF[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft, topRight });
			} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				g.DrawLines(p, new PointF[] { topLeft, topRight, bottomRight, bottomLeft, topLeft, bottomRight });
			} else {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				g.DrawLines(p, new PointF[] { topLeft, topRight, bottomRight, bottomLeft, topLeft });
			}
		}
		#endregion

		#region layoutAddArea
		private KlimaFlaechenModul.ModulTypeEnum moduleTypeToAdd = KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30;
		private KlimaFlaechenModul.ModulOrientationEnum startingOrientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
		private bool optimalLayout = true;
		private bool alignRectangle = true;

		public KlimaFlaechenModul.ModulTypeEnum ModuleTypeToAdd {
			get { return this.moduleTypeToAdd; }
			set { this.moduleTypeToAdd = value; }
		}

		public KlimaFlaechenModul.ModulOrientationEnum StartingOrientation {
			get { return this.startingOrientation; }
			set { this.startingOrientation = value; }
		}

		public bool OptimalLayout {
			get { return this.optimalLayout; }
			set { this.optimalLayout = value; }
		}

		public bool AlignRectangle {
			get { return this.alignRectangle; }
			set { this.alignRectangle = value; }
		}
		#endregion layoutAddArea
	}
}
