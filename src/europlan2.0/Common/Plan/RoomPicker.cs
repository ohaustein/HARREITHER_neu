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
	public partial class RoomPicker : Component, IPlanner {

		public enum RoomPickerMode {
			RPM_NONE,
			RPM_PICK_ROOM,
			RPM_PICK_UNUSED,
			RPM_ADD_UNUSED,
			RPM_SET_REFERENCE,
			RPM_DEL_UNUSED,
			RPM_ADD_EXPANSION_GAP,
			RPM_DEL_EXPANSION_GAP
		}

		public RoomPicker() {
			InitializeComponent();
		}

		public RoomPicker(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private Room room;
		private RoomPickerMode mode = RoomPickerMode.RPM_NONE;
		private List<Point2D> roomCoordinates = new List<Point2D>();
		private List<Point2D> oldRoomCoordinates = new List<Point2D>();
		private List<List<Point2D>> unusedCoordinates = new List<List<Point2D>>();
		private List<List<Point2D>> oldUnusedCoordinates = new List<List<Point2D>>();
		private List<Point2D> coordsPickedSoFar = new List<Point2D>();
		private bool inDesign = false;
		private bool unsavedChanges = false;
		private bool isCeiling = false;
		private Nullable<Point2D> referencePoint = null;
		private Point2D newUnheatedAreaPos = new Point2D();
		private Size2D newUnheatedAreaSize = new Size2D();
		private Point2D expansionGapStart = Point2D.Zero;
		private List<List<Point2D>> otherRoomCoordinates;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Room Room {
			get { return this.room; }
			set {
				this.room = value;
				if (this.ConnectedPlanPanel != null) {
					if (this.room == null || this.room.AssociatedPlan == null) {
						this.ConnectedPlanPanel.Plan = null;
					} else {
						this.ConnectedPlanPanel.Plan = this.room.AssociatedPlan;
					}
				}
				otherRoomCoordinates = new List<List<Point2D>>();
				foreach (Room r in this.room.AssociatedFloor.Rooms) {
					if (r.Id != this.room.Id) {
						if (IsCeiling) {
							if (r.CeilingCoordinatesToUse != null && r.CeilingCoordinatesToUse.Count > 2) {
								otherRoomCoordinates.Add(r.CeilingCoordinatesToUse);
							}
						} else {
							if (r.RoomCoordinates != null && r.RoomCoordinates.Count > 2) {
								otherRoomCoordinates.Add(r.RoomCoordinates);
							}
						}
					}
				}
			}
		}

		public RoomPickerMode Mode {
			get { return this.mode; }
			set {
				if (value == RoomPickerMode.RPM_PICK_ROOM) {
					bool resetFloorProducts = !this.isCeiling;
					bool resetCeilingProducts = this.isCeiling || this.room.CeilingCoordinates == null || this.room.CeilingCoordinates.Count == 0;
					bool resetWallProducts = false;
					if (this.room != null && this.room.RoomCoordinates != null && this.room.RoomCoordinates.Count > 0) {
						bool ask = false;
						foreach (PlannedProduct pp in this.room.PlannedProducts) {
							if (((resetFloorProducts && pp.Product.Type == Product.ProductType.FBH) ||
								(resetCeilingProducts && pp.Product.Type == Product.ProductType.DH) ||
								(resetWallProducts && pp.Product.Type == Product.ProductType.WH)) &&
								pp.Product.GraphicalMode.HasValue && pp.Product.GraphicalMode.Value) {
								ask = true;
								break;
							}
						}
						if (ask) {
							DialogResult result = MessageBox.Show(EuroplanRes.RoomPicker_NeueRaumgeometrieText, EuroplanRes.RoomPicker_NeueRaumgeometrieTitel, MessageBoxButtons.YesNo);
							if (result == DialogResult.No) {
								return;
							}
						}
					}
					this.oldRoomCoordinates.Clear();
					this.oldRoomCoordinates.AddRange(this.roomCoordinates);
					this.oldUnusedCoordinates.Clear();
					this.oldUnusedCoordinates.AddRange(this.unusedCoordinates);
					this.roomCoordinates.Clear();
					this.unusedCoordinates.Clear();
					if (this.ConnectedPlanPanel != null) {
						this.ConnectedPlanPanel.InvalidateGraphics();
					}
					foreach (PlannedProduct pp in this.room.PlannedProducts) {
						if ((resetFloorProducts && pp.Product.Type == Product.ProductType.FBH) ||
							(resetCeilingProducts && pp.Product.Type == Product.ProductType.DH) ||
							(resetWallProducts && pp.Product.Type == Product.ProductType.WH)) {
							pp.Product.GraphicalMode = false;
							pp.Product.ClearGraphicalRepresentation();
						}
					}
				}
				this.mode = value;
			}
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set { this.connectedPlanPanel = value; }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			Graphics g = e.Graphics;

			foreach (List<Point2D> otherRoomCoordinate in otherRoomCoordinates) {
				GraphicsPath fillPath = new GraphicsPath();
				fillPath.StartFigure();
				PointF[] array = new PointF[otherRoomCoordinate.Count];
				int i = 0;
				foreach (Point2D point in otherRoomCoordinate) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
				}
				fillPath.AddPolygon(array);
				fillPath.CloseFigure();
				Color c = Color.FromArgb(80, Color.DarkRed);
				Brush b = new SolidBrush(c);
				g.FillPath(b, fillPath);
				g.DrawPath(new Pen(b), fillPath);
				fillPath.Dispose();
			}

			if (roomCoordinates.Count > 2) {
				GraphicsPath fillPath = new GraphicsPath();
				fillPath.StartFigure();
				PointF[] array = new PointF[roomCoordinates.Count];
				int i = 0;
				foreach (Point2D point in roomCoordinates) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
				}
				fillPath.AddPolygon(array);
				fillPath.CloseFigure();
				Color c = Color.FromArgb(128, Color.Red);
				Brush b = new SolidBrush(c);
				g.FillPath(b, fillPath);
				g.DrawPath(new Pen(b), fillPath);
				fillPath.Dispose();
			}

			if (unusedCoordinates.Count > 0) {
				foreach (List<Point2D> unusedArea in unusedCoordinates) {
					GraphicsPath path = new GraphicsPath();
					path.StartFigure();
					PointF[] array = new PointF[unusedArea.Count];
					int i = 0;
					foreach (Point2D point in unusedArea) {
						Point2D tmp = additionalTransformation.TransformTo2D(point);
						array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					path.AddPolygon(array);
					path.CloseFigure();
					Color c = Color.FromArgb(0, Color.Red);
					Color c2 = Color.FromArgb(128, Color.White);
					Brush b = new HatchBrush(HatchStyle.BackwardDiagonal, c2, c);
					g.FillPath(b, path);
					b = new SolidBrush(c2);
					g.DrawPath(new Pen(b), path);
					path.Dispose();
				}
			}

			if (coordsPickedSoFar.Count > 0 && inDesign) {
				List<Point2D> points = new List<Point2D>(coordsPickedSoFar);
				Point2D pos = new Point2D((float)mousePositionInPlan.X, (float)mousePositionInPlan.Y);
				if (Product.ConfigActivateOrthoRasterung == ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT)) {
					if (points.Count == 1) {
						pos = GetNormalizedPoint(points[0], null, pos);
					} else if (points.Count == 2) {
						pos = GetNormalizedPoint(points[points.Count - 1], points[0], pos);
					} else {
						pos = GetNormalizedPoint(points[points.Count - 1], points[0], pos, points[0]);
					}
				}
				points.Add(pos);

				GraphicsPath path = new GraphicsPath();
				path.StartFigure();
				PointF[] array = new PointF[points.Count];
				int i = 0;
				foreach (Point2D point in points) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
				}
				if (array.Length > 2) {
					path.AddPolygon(array);
				} else {
					path.AddLine(array[0], array[1]);
				}
				path.CloseFigure();
				Brush b = null;
				if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
					Color c = Color.FromArgb(128, Color.Red);
					b = new SolidBrush(c);
					g.FillPath(b, path);
					g.DrawPath(new Pen(b), path);
				} else if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
					Color c = Color.FromArgb(0, Color.Red);
					Color c2 = Color.FromArgb(128, Color.White);
					b = new HatchBrush(HatchStyle.BackwardDiagonal, c2, c);
					g.FillPath(b, path);
					b = new SolidBrush(c2);
					g.DrawPath(new Pen(b), path);
				}
				path.Dispose();
			}

			if (expansionGapStart != Point2D.Zero) {
				Point2D pos = new Point2D((float)mousePositionInPlan.X, (float)mousePositionInPlan.Y);
                if (Product.ConfigActivateOrthoRasterung ==  ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT)) {
					pos = GetNormalizedPoint(expansionGapStart, null, pos);
				}
				Point2D start = additionalTransformation.TransformTo2D(expansionGapStart);
				Point2D end = additionalTransformation.TransformTo2D(pos);
				g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
			}

			foreach (Segment2D expansionGap in this.room.AssociatedFloor.ExpansionGaps) {
				Point2D start = additionalTransformation.TransformTo2D(expansionGap.Start);
				Point2D end = additionalTransformation.TransformTo2D(expansionGap.End);
				g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
			}
			
			if ((this.Mode == RoomPickerMode.RPM_SET_REFERENCE || this.Mode == RoomPickerMode.RPM_ADD_UNUSED) && this.referencePoint != null) {
				Pen p = new Pen(new SolidBrush(Color.Black));

				Matrix transform = g.Transform;
				g.Transform = new Matrix();

				Point2D tmp = additionalTransformation.TransformTo2D(this.referencePoint.Value);

				PointF[] refPoint = new PointF[] { new PointF((float)tmp.X, (float)tmp.Y) };
				transform.TransformPoints(refPoint);
				g.FillEllipse(new SolidBrush(Color.FromArgb(127, Color.White)), new RectangleF(refPoint[0].X - 5, refPoint[0].Y - 5, 10, 10));
				g.DrawLine(p, new PointF(refPoint[0].X - 5, refPoint[0].Y), new PointF(refPoint[0].X + 5, refPoint[0].Y));
				g.DrawLine(p, new PointF(refPoint[0].X, refPoint[0].Y - 5), new PointF(refPoint[0].X, refPoint[0].Y + 5));

				g.Transform = transform;

				if (this.Mode == RoomPickerMode.RPM_ADD_UNUSED && this.newUnheatedAreaSize.X > 0 && this.newUnheatedAreaSize.Y > 0) {
					if (this.newUnheatedArea.Count > 0) {
						foreach (List<Point2D> unusedArea in this.newUnheatedArea) {
							GraphicsPath path = new GraphicsPath();
							path.StartFigure();
							PointF[] array = new PointF[unusedArea.Count];
							int i = 0;
							foreach (Point2D point in unusedArea) {
								Point2D tmp2 = additionalTransformation.TransformTo2D(point);
								array[i++] = new PointF((float)tmp2.X, (float)tmp2.Y);
							}
							path.AddPolygon(array);
							path.CloseFigure();
							Color c = Color.FromArgb(0, Color.Red);
							Color c2 = Color.FromArgb(64, Color.White);
							Brush b = new HatchBrush(HatchStyle.BackwardDiagonal, c2, c);
							g.FillPath(b, path);
							b = new SolidBrush(c2);
							g.DrawPath(new Pen(b), path);
							path.Dispose();
						}
					}
				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == RoomPickerMode.RPM_PICK_ROOM || this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
				PointF pos = new PointF((float)planPoint.X, (float)planPoint.Y);

				Point2D normalizedPoint = planPoint;
				bool pick = (button == MouseButtons.Left) || (button == MouseButtons.Right);
				bool finishPick = button == MouseButtons.Right;
				bool addFinishinigPick = true;
                if (coordsPickedSoFar.Count > 0 && Product.ConfigActivateOrthoRasterung ==  ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT)) {
					if (coordsPickedSoFar.Count == 1) {
						normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[0], null, normalizedPoint);
					} else if (coordsPickedSoFar.Count == 2) {
						normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], normalizedPoint);
					} else {
						bool isStart;
						normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], normalizedPoint, coordsPickedSoFar[0], out isStart);
						addFinishinigPick = finishPick;
						finishPick = finishPick || (pick && isStart);
					}
				}

				if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
					// pick areas to exclude from the room area
					if (!this.UnusedAreaIsValid(normalizedPoint)) {
						return false;
					}
				}
				if (pick && !finishPick) {
					if (RoomAreaIsValid(normalizedPoint)) {
						unsavedChanges = true;
						coordsPickedSoFar.Add(normalizedPoint);
						this.SimplifyPolygon(coordsPickedSoFar, false);
						inDesign = true;
					}
				} else if (finishPick && RoomAreaIsValid(normalizedPoint)) {
					if (addFinishinigPick) {
						coordsPickedSoFar.Add(normalizedPoint);
					}
					this.SimplifyPolygon(coordsPickedSoFar, true);
					if (coordsPickedSoFar.Count > 2) {
						if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
							string text = EuroplanRes.RoomPicker_RaumgroesseText;
							text = text.Replace("%AREA%", Math.Round(Math.Abs(new Polygon2D(coordsPickedSoFar).GetArea()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2).ToString());
							DialogResult result = MessageBox.Show(text, EuroplanRes.RoomPicker_RaumgroesseTitel, MessageBoxButtons.YesNo);
							if (result.Equals(DialogResult.Yes)) {
								roomCoordinates.AddRange(coordsPickedSoFar);
								unsavedChanges = true;
								this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
								this.Mode = RoomPickerMode.RPM_NONE;
								if (ModeChanged != null) {
									this.ModeChanged(this, EventArgs.Empty);
								}
							}
						} else if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
							unusedCoordinates.Add(new List<Point2D>(coordsPickedSoFar));
							unsavedChanges = true;
						}
					}
					coordsPickedSoFar.Clear();
					inDesign = false;
				}
				return true;
			} else if (this.Mode == RoomPickerMode.RPM_SET_REFERENCE && (button == MouseButtons.Left || button == MouseButtons.Right)) {
				PointF pos = new PointF((float)planPoint.X, (float)planPoint.Y);

				Point2D normalizedPoint = planPoint;

				this.referencePoint = normalizedPoint;
				return true;
			} else if (this.Mode == RoomPickerMode.RPM_DEL_UNUSED) {
				if (button == MouseButtons.Left) {
					List<Point2D> areaToDelete = null;
					foreach (List<Point2D> unusedArea in unusedCoordinates) {
						Polygon2D unusedPoly = new Polygon2D(unusedArea);
						if (unusedPoly.IsInside(planPoint)) {
							areaToDelete = unusedArea;
							break;
						}
					}
					if (areaToDelete != null) {
						DialogResult result = MessageBox.Show(EuroplanRes.PicturePanel_DeleteUnusedText, EuroplanRes.PicturePanel_DeleteUnusedCaption, MessageBoxButtons.YesNo);
						if (result == DialogResult.Yes) {
							unusedCoordinates.Remove(areaToDelete);
							return true;
						}
					}
				}
			} else if (this.Mode == RoomPickerMode.RPM_ADD_EXPANSION_GAP && button == MouseButtons.Left) {
				if (expansionGapStart == Point2D.Zero) {
					expansionGapStart = planPoint;
				} else {
					Point2D normalizedPoint = planPoint;
                    if (Product.ConfigActivateOrthoRasterung == ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT)) {
						normalizedPoint = GetNormalizedPoint(expansionGapStart, null, normalizedPoint);
					}
					this.room.AssociatedFloor.ExpansionGaps.Add(new Segment2D(expansionGapStart.X, expansionGapStart.Y, normalizedPoint.X, normalizedPoint.Y));
					expansionGapStart = Point2D.Zero;
				}
				return true;
			} else if (this.Mode == RoomPickerMode.RPM_DEL_EXPANSION_GAP && button == MouseButtons.Left) {
				Segment2D toDelete = new Segment2D();
				bool found = false;
				foreach (Segment2D expansionGap in this.room.AssociatedFloor.ExpansionGaps) {
					double dist = expansionGap.GetDistance(planPoint);
					if (dist < (this.room.AssociatedPlan.Measure * 0.05)) {
						found = true;
						toDelete = expansionGap;
						break;
					}
				}
				if (found) {
					this.room.AssociatedFloor.ExpansionGaps.Remove(toDelete);
					return true;
				}
			}
			return false;
		}

		private double minSqDist = 0.00000001; // (0.1mm)

		private void SimplifyPolygon(List<Point2D> polygon, bool closed) {
			if (polygon.Count < 3) {
				return;
			}
			Point2D prev;
			Point2D cur;
			Point2D next;
			Line2D line;
			double measureSq = this.ConnectedPlanPanel.Plan.Measure.Value * this.ConnectedPlanPanel.Plan.Measure.Value;
			for (int i = 1; i < polygon.Count - 1; i++ ) {
				prev = polygon[i - 1];
				cur = polygon[i];
				next = polygon[i + 1];
				line = new Line2D(prev, next - prev);
				if (line.GetDistanceSquared(cur) / measureSq < minSqDist) {
					polygon.Remove(cur);
					SimplifyPolygon(polygon, closed);
					return;
				}
			}
			if (closed) {
				prev = polygon[polygon.Count - 2];
				cur = polygon[polygon.Count - 1];
				next = polygon[0];
				line = new Line2D(prev, next - prev);
				if (line.GetDistanceSquared(cur) / measureSq < minSqDist) {
					polygon.Remove(cur);
					SimplifyPolygon(polygon, closed);
					return;
				}
				prev = polygon[polygon.Count - 1];
				cur = polygon[0];
				next = polygon[1];
				line = new Line2D(prev, next - prev);
				if (line.GetDistanceSquared(cur) / measureSq < minSqDist) {
					polygon.Remove(cur);
					SimplifyPolygon(polygon, closed);
					return;
				}
			}
		}

		private bool RoomAreaIsValid(Point2D normalizedPoint) {
			Polygon2D polygon = null;
			foreach (List<Point2D> otherRoomCoordinate in otherRoomCoordinates) {
				polygon = new Polygon2D(otherRoomCoordinate);
				if (polygon.IsInside(normalizedPoint)) {
					return false;
				}
				if (inDesign) {
					if (coordsPickedSoFar.Count > 0) {
						if (Intersects(polygon, new Segment2D(coordsPickedSoFar[0], normalizedPoint))) {
							// the line from the current point to the next point intersects the room borders
							return false;
						}
						if (coordsPickedSoFar.Count > 1) {
							if (Intersects(polygon, new Segment2D(normalizedPoint, coordsPickedSoFar[coordsPickedSoFar.Count - 1]))) {
								// the line from the last point to the current point intersects the room borders
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		private bool UnusedAreaIsValid(Point2D normalizedPoint) {
			Polygon2D polygon = new Polygon2D(roomCoordinates);
			if (!polygon.IsInside(normalizedPoint)) {
				// the current point is not inside the room area
				return false;
			}
			if (inDesign) {
				if (Intersects(polygon, new Segment2D(coordsPickedSoFar[0], normalizedPoint))) {
					// the line from the current point to the next point intersects the room borders
					return false;
				}
				if (coordsPickedSoFar.Count > 1) {
					if (Intersects(polygon, new Segment2D(normalizedPoint, coordsPickedSoFar[coordsPickedSoFar.Count - 1]))) {
						// the line from the last point to the current point intersects the room borders
						return false;
					}
				}
			}
			foreach (List<Point2D> unusedArea in unusedCoordinates) {
				polygon = new Polygon2D(unusedArea);
				if (polygon.IsInside(normalizedPoint)) {
					// the current point is inside another unused area
					return false;
				}
				if (inDesign) {
					if (Intersects(polygon, new Segment2D(normalizedPoint, coordsPickedSoFar[0]))) {
						// the line from the current point to the next point intersects the another unused area
						return false;
					}
					if (coordsPickedSoFar.Count > 1) {
						if (Intersects(polygon, new Segment2D(normalizedPoint, coordsPickedSoFar[coordsPickedSoFar.Count - 1]))) {
							// the line from the last point to the current point intersects another unused area
							return false;
						}
					}
				}
			}
			return true;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
				Point2D normalizedPoint = planPoint;
				bool isStart = false;
                if (Product.ConfigActivateOrthoRasterung == ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) && coordsPickedSoFar.Count > 0) {
					if (coordsPickedSoFar.Count == 1) {
						normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[0], null, planPoint);
					} else if (coordsPickedSoFar.Count == 2) {
						normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], planPoint);
					} else {
						normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], planPoint, coordsPickedSoFar[0], out isStart);
					}
				}
				if (UnusedAreaIsValid(normalizedPoint)) {
					this.ConnectedPlanPanel.PlanCursor = isStart ? Cursors.Hand : Cursors.Cross;
				} else {
					this.ConnectedPlanPanel.PlanCursor = Cursors.No;
				}
				return inDesign;
			} else if (this.Mode == RoomPickerMode.RPM_DEL_UNUSED) {
				bool ok = false;
				foreach (List<Point2D> unusedArea in unusedCoordinates) {
					Polygon2D polygon = new Polygon2D(unusedArea);
					if (polygon.IsInside(planPoint)) {
						ok = true;
					}
				}
				if (ok) {
					this.ConnectedPlanPanel.PlanCursor = Cursors.Hand;
				} else {
					this.ConnectedPlanPanel.PlanCursor = Cursors.No;
				}

				return false;
			} else if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
				Point2D normalizedPoint = planPoint;
				bool isStart = false;
				if (Product.ConfigActivateOrthoRasterung == ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) && coordsPickedSoFar.Count > 0) {
					if (coordsPickedSoFar.Count > 2) {
						normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], planPoint, coordsPickedSoFar[0], out isStart);
					}
				}
				if (RoomAreaIsValid(normalizedPoint)) {
					this.ConnectedPlanPanel.PlanCursor = isStart ? Cursors.Hand : Cursors.Cross;
				} else {
					this.ConnectedPlanPanel.PlanCursor = Cursors.No;
				}
				return inDesign;
			} else if (this.Mode == RoomPickerMode.RPM_SET_REFERENCE) {
				this.ConnectedPlanPanel.PlanCursor = Cursors.Cross;
			} else if (this.Mode == RoomPickerMode.RPM_ADD_UNUSED) {
				this.ConnectedPlanPanel.PlanCursor = Cursors.No;
			} else if (this.Mode == RoomPickerMode.RPM_ADD_EXPANSION_GAP) {
				return expansionGapStart != Point2D.Zero;
			} else if (this.Mode == RoomPickerMode.RPM_DEL_EXPANSION_GAP) {
				bool ok = false;
				foreach (Segment2D expansionGap in this.room.AssociatedFloor.ExpansionGaps) {
					double dist = expansionGap.GetDistance(planPoint);
					if (dist < (this.room.AssociatedPlan.Measure * 0.05)) {
						ok = true;
						break;
					}
				}
				if (ok) {
					this.ConnectedPlanPanel.PlanCursor = Cursors.Hand;
				} else {
					this.ConnectedPlanPanel.PlanCursor = Cursors.No;
				}
				return false;
			}
			return false;
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
		#endregion

		private bool Intersects(Polygon2D polygon, Segment2D line) {
			List<Segment2D> segments = new List<Segment2D>();
			Polygon2D.GetSegments(polygon, segments);
			foreach (Segment2D segment in segments) {
				if (Segment2D.Intersects(segment, line)) {
					return true;
				}
			}
			return false;
		}

		private bool onlyHorizAndVert = false;

		private double angleSnapDist = Math.Tan(10.0 / 180.0 * Math.PI);
		private double startPointSnapSqDist = 100;

		private Point2D GetNormalizedPoint(Point2D basePoint1_, Nullable<Point2D> basePoint2_, Point2D currentPoint_, Nullable<Point2D> startPoint_, out bool isStartPoint) {
			Matrix3D rotationMatrix = Transformation3D.Rotate(this.ConnectedPlanPanel.Plan.Rotation / 180 * Math.PI);
			Point2D basePoint1 = rotationMatrix.Transform(basePoint1_);
			Nullable<Point2D> basePoint2 = basePoint2_.HasValue ? rotationMatrix.Transform(basePoint2_.Value) : basePoint2_;
			Point2D currentPoint = rotationMatrix.Transform(currentPoint_);
			Nullable<Point2D> startPoint = startPoint_.HasValue ? rotationMatrix.Transform(startPoint_.Value) : startPoint_;
			isStartPoint = false;
			if (startPoint.HasValue) {
				double scale = this.ConnectedPlanPanel.ScaleForCalculation;
				double xDistStart = (startPoint.Value.X - currentPoint.X) * scale;
				double yDistStart = (startPoint.Value.Y - currentPoint.Y) * scale;
				if (xDistStart * xDistStart + yDistStart * yDistStart < startPointSnapSqDist) {
					isStartPoint = true;
					return startPoint_.Value;
				}
			}
			if (ConnectedPlanPanel.SupportsSnap) {
				return currentPoint;
			}

			double xDistance1 = Math.Abs(basePoint1.X - currentPoint.X);
			double yDistance1 = Math.Abs(basePoint1.Y - currentPoint.Y);
			double xDistInMeter1 = (currentPoint.X - basePoint1.X) / this.ConnectedPlanPanel.Plan.Measure.Value;
			double yDistInMeter1 = (currentPoint.Y - basePoint1.Y) / this.ConnectedPlanPanel.Plan.Measure.Value;
			double xDistInMeterRounded1 = Math.Round(xDistInMeter1, 1);
			double yDistInMeterRounded1 = Math.Round(yDistInMeter1, 1);
			Point2D p;
			if (onlyHorizAndVert) {
				if (xDistance1 < yDistance1) {
					p = new Point2D(basePoint1.X, basePoint1.Y + ((float)yDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value));
				} else {
					p = new Point2D(basePoint1.X + ((float)xDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value), basePoint1.Y);
				}

			} else {
				if (basePoint2.HasValue) {
					double xDistance2 = Math.Abs(basePoint2.Value.X - currentPoint.X);
					double yDistance2 = Math.Abs(basePoint2.Value.Y - currentPoint.Y);
					double xDistInMeter2 = (currentPoint.X - basePoint2.Value.X) / this.ConnectedPlanPanel.Plan.Measure.Value;
					double yDistInMeter2 = (currentPoint.Y - basePoint2.Value.Y) / this.ConnectedPlanPanel.Plan.Measure.Value;
					double xDistInMeterRounded2 = Math.Round(xDistInMeter2, 1);
					double yDistInMeterRounded2 = Math.Round(yDistInMeter2, 1);

					double newX = currentPoint.X;
					double newY = currentPoint.Y;
					if (Math.Abs(xDistInMeter1 - xDistInMeterRounded1) <= Math.Abs(xDistInMeter2 - xDistInMeterRounded2)) {
						newX = basePoint1.X + xDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value;
					} else {
						newX = basePoint2.Value.X + xDistInMeterRounded2 * this.ConnectedPlanPanel.Plan.Measure.Value;
					}
					if (Math.Abs(yDistInMeter1 - yDistInMeterRounded1) <= Math.Abs(yDistInMeter2 - yDistInMeterRounded2)) {
						newY = basePoint1.Y + yDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value;
					} else {
						newY = basePoint2.Value.Y + yDistInMeterRounded2 * this.ConnectedPlanPanel.Plan.Measure.Value;
					}

					xDistance1 = Math.Abs(basePoint1.X - newX);
					yDistance1 = Math.Abs(basePoint1.Y - newY);
					xDistance2 = Math.Abs(basePoint2.Value.X - newX);
					yDistance2 = Math.Abs(basePoint2.Value.Y - newY);

					double xTan1 = xDistance1 / yDistance1;
					double xTan2 = xDistance2 / yDistance2;
					double yTan1 = yDistance1 / xDistance1;
					double yTan2 = yDistance2 / xDistance2;
					if (xTan1 < xTan2) {
						if (xTan1 < angleSnapDist) {
							newX = basePoint1.X;
						}
					} else {
						if (xTan2 < angleSnapDist) {
							newX = basePoint2.Value.X;
						}
					}
					if (yTan1 < yTan2) {
						if (yTan1 < angleSnapDist) {
							newY = basePoint1.Y;
						}
					} else {
						if (yTan2 < angleSnapDist) {
							newY = basePoint2.Value.Y;
						}
					}
					p = new Point2D(newX, newY);
				} else {

					double newX = basePoint1.X + xDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value;
					double newY = basePoint1.Y + yDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value;
					xDistance1 = Math.Abs(basePoint1.X - newX);
					yDistance1 = Math.Abs(basePoint1.Y - newY);

					if (xDistance1 / yDistance1 <= angleSnapDist) {
						newX = basePoint1.X;
					} else if (yDistance1 / xDistance1 <= angleSnapDist) {
						newY = basePoint1.Y;
					}
					p = new Point2D(newX, newY);
				}
			}
			rotationMatrix = Transformation3D.Rotate(-this.ConnectedPlanPanel.Plan.Rotation / 180 * Math.PI);
			p = rotationMatrix.Transform(p);
			return p;
		}

		private Point2D GetNormalizedPoint(Point2D basePoint1, Nullable<Point2D> basePoint2, Point2D currentPoint, Nullable<Point2D> startPoint) {
			bool tmp;
			return this.GetNormalizedPoint(basePoint1, basePoint2, currentPoint, startPoint, out tmp);
		}

		private Point2D GetNormalizedPoint(Point2D basePoint1, Nullable<Point2D> basePoint2, Point2D currentPoint) {
			bool tmp;
			return this.GetNormalizedPoint(basePoint1, basePoint2, currentPoint, null, out tmp);
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		public bool IsCeiling {
			get { return this.isCeiling; }
			set { this.isCeiling = value; }
		}

		public List<Point2D> RoomCoordinates {
			get { return this.roomCoordinates; }
			set {
				this.roomCoordinates = value;
				this.oldRoomCoordinates = new List<Point2D>(this.roomCoordinates);
			}
		}

		public List<List<Point2D>> UnusedCoordinates {
			get { return this.unusedCoordinates; }
			set {
				this.unusedCoordinates = value;
				this.oldUnusedCoordinates = new List<List<Point2D>>(this.unusedCoordinates);
			}
		}

		public Cursor CustomCursor {
			get { return null; }
		}

		public event EventHandler ModeChanged;

		public bool PlannerKeyPress(Keys key) {
			if (this.inDesign && coordsPickedSoFar.Count > 0 && key == Keys.Escape) {
				if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
					if (MessageBox.Show(EuroplanRes.RoomPicker_RaumgeometrieAbbrechenText, EuroplanRes.RoomPicker_RaumgeometrieAbbrechenTitel, MessageBoxButtons.YesNo) == DialogResult.Yes) {
						this.inDesign = false;
						this.coordsPickedSoFar.Clear();
						this.roomCoordinates.AddRange(this.oldRoomCoordinates);
						this.unusedCoordinates.AddRange(this.oldUnusedCoordinates);
						this.Mode = RoomPickerMode.RPM_NONE;
						this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
						if (this.ModeChanged != null) {
							this.ModeChanged(this, EventArgs.Empty);
						}
						return true;
					}
				} else {
					if (MessageBox.Show(EuroplanRes.RoomPicker_FlaecheAbbrechenText, EuroplanRes.RoomPicker_FlaecheAbbrechenTitel, MessageBoxButtons.YesNo) == DialogResult.Yes) {
						this.inDesign = false;
						this.coordsPickedSoFar.Clear();
						return true;
					}
				}
			}
			if (this.Mode == RoomPickerMode.RPM_ADD_EXPANSION_GAP || this.Mode == RoomPickerMode.RPM_DEL_EXPANSION_GAP) {
				expansionGapStart = Point2D.Zero;
				this.Mode = RoomPickerMode.RPM_NONE;
				this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
				this.ConnectedPlanPanel.InvalidateGraphics();
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
				return true;
			}
			return false;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Nullable<Point2D> ReferencePoint {
			get { return this.referencePoint; }
			set {
				this.referencePoint = value;
				this.CalulacteNewUnheatedArea();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point2D NewUnheatedAreaPos {
			get { return this.newUnheatedAreaPos; }
			set {
				this.newUnheatedAreaPos = value;
				this.CalulacteNewUnheatedArea();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Size2D NewUnheatedAreaSize {
			get { return this.newUnheatedAreaSize; }
			set {
				this.newUnheatedAreaSize = value;
				this.CalulacteNewUnheatedArea();
			}
		}

		public bool IsNewUnheatedAreaValid {
			get { return this.newUnheatedArea != null && this.newUnheatedArea.Count > 0; }
		}

		private List<Polygon2D> newUnheatedArea = null;

		private void CalulacteNewUnheatedArea() {
			if (this.referencePoint.HasValue && this.newUnheatedAreaSize.X > 0 && this.newUnheatedAreaSize.Y > 0) {
				double posX = this.referencePoint.Value.X + this.newUnheatedAreaPos.X * this.Room.AssociatedPlan.Measure.Value;
				double posY = this.referencePoint.Value.Y + this.newUnheatedAreaPos.Y * this.Room.AssociatedPlan.Measure.Value;
				double sizeX = this.newUnheatedAreaSize.X * this.Room.AssociatedPlan.Measure.Value;
				double sizeY = this.newUnheatedAreaSize.Y * this.Room.AssociatedPlan.Measure.Value;

				Matrix3D matrix = Matrix3D.Identity;
				matrix = matrix * Transformation3D.Translation(this.referencePoint.Value.X, this.referencePoint.Value.Y);
				matrix = matrix * Transformation3D.Rotate(-this.room.AssociatedPlan.Rotation / 180 * Math.PI);
				matrix = matrix * Transformation3D.Translation(-this.referencePoint.Value.X, -this.referencePoint.Value.Y);

				Point2D p1 = new Point2D(posX, posY);
				Point2D p2 = new Point2D(posX + sizeX, posY);
				Point2D p3 = new Point2D(posX + sizeX, posY + sizeY);
				Point2D p4 = new Point2D(posX, posY + sizeY);

				p1 = matrix.Transform(p1);
				p2 = matrix.Transform(p2);
				p3 = matrix.Transform(p3);
				p4 = matrix.Transform(p4);

				Polygon2D tmp = new Polygon2D(new Point2D[] { p1, p2, p3, p4 });
				if (tmp.IsClockwise()) {
					tmp.Reverse();
				}
				Polygon2D room = new Polygon2D(this.roomCoordinates);
				if (room.IsClockwise()) {
					room.Reverse();
				}
				List<Polygon2D> list1 = new List<Polygon2D>();
				list1.Add(tmp);
				List<Polygon2D> list2 = new List<Polygon2D>();
				list2.Add(room);
                try {
                    newUnheatedArea = Polygon2D.GetIntersection(list1, list2);
                } catch {
                    newUnheatedArea = new List<Polygon2D>();
                }

				foreach (List<Point2D> unused in this.unusedCoordinates) {
					list2.Clear();
					tmp = new Polygon2D(unused);
					if (tmp.IsClockwise()) {
						tmp.Reverse();
					}
					list2.Add(tmp);

					newUnheatedArea = Polygon2D.GetDifference(newUnheatedArea, list2);
				}
			}
		}

		public void AddNewUnheatedArea() {
			if (this.newUnheatedArea != null && this.newUnheatedArea.Count > 0) {
				if (this.unusedCoordinates == null) {
					this.unusedCoordinates = new List<List<Point2D>>();
				}
				foreach (Polygon2D poly in this.newUnheatedArea) {
					List<Point2D> newArea = new List<Point2D>(poly);
					this.unusedCoordinates.Add(newArea);
				}
			}
			this.newUnheatedAreaPos = new Point2D();
			this.newUnheatedAreaSize = new Size2D();
			this.newUnheatedArea = null;
		}

        public bool ShowPlanBackground {
            get { return true; }
        }
    }
}
