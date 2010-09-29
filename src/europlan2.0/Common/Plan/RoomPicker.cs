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
	public partial class RoomPicker : Component, IProductPlanner {

		public enum RoomPickerMode {
			RPM_NONE,
			RPM_PICK_ROOM,
			RPM_PICK_UNUSED,
			RPM_DEL_UNUSED
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
			}
		}

		public RoomPickerMode Mode {
			get { return this.mode; }
			set {
				if (value == RoomPickerMode.RPM_PICK_ROOM) {
					DialogResult result = MessageBox.Show("Wollen Sie die bereits definierte Raumgeometrie verwerfen und neu definieren?", "Verwerfen und neu definieren?", MessageBoxButtons.YesNo);
					if (result == DialogResult.No) {
						return;
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
				//PointF pos = mousePosInPlan;
				Point2D pos = new Point2D((float)mousePositionInPlan.X, (float)mousePositionInPlan.Y);
				if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
					if (points.Count == 1) {
						pos = GetNormalizedPoint(points[0], null, pos);
					} else if (points.Count == 2) {
						pos = GetNormalizedPoint(points[points.Count - 1], points[0], pos);
					} else {
						pos = GetNormalizedPoint(points[points.Count - 1], points[0], pos, points[0]);
					}
				}
				points.Add(pos);
				//g.DrawPolygon(Pens.Black, points.ToArray());

				GraphicsPath path = new GraphicsPath();
				//path.StartFigure();
				PointF[] array = new PointF[points.Count];
				int i = 0;
				foreach (Point2D point in points) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
				}
				if (array.Length > 2) {
					path.AddLines(array);
				} else {
					path.AddLine(array[0], array[1]);
				}
				//path.CloseFigure();
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
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.Mode == RoomPickerMode.RPM_PICK_ROOM || this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
				PointF pos = new PointF((float)planPoint.X, (float)planPoint.Y);

				Point2D normalizedPoint = planPoint;
				bool pick = (button == MouseButtons.Left) || (button == MouseButtons.Right);
				bool finishPick = button == MouseButtons.Right;
				bool addFinishinigPick = true;
				if (coordsPickedSoFar.Count > 0 && (this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
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
					/*if (!inDesign && this.Mode == RoomPickerMode.RPM_PICK_ROOM && roomCoordinates.Count > 0) {
						// TODO
						DialogResult result = MessageBox.Show("Wollen Sie die bereits definierte Raumgeometrie verwerfen und neu definieren?", "Verwerfen und neu definieren?", MessageBoxButtons.YesNo);
						if (result == DialogResult.No) {
							return false;
						}
						roomCoordinates.Clear();
					}*/
					unsavedChanges = true;
					coordsPickedSoFar.Add(normalizedPoint);
					this.SimplifyPolygon(coordsPickedSoFar, false);
					inDesign = true;
				} else if (finishPick) {
					if (addFinishinigPick) {
						coordsPickedSoFar.Add(normalizedPoint);
					}
					this.SimplifyPolygon(coordsPickedSoFar, true);
					if (coordsPickedSoFar.Count > 2) {
						if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
							DialogResult result = MessageBox.Show("Die definierte Fläche beträgt " + Math.Round(Math.Abs(new Polygon2D(coordsPickedSoFar).GetArea()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2) + "m². Kleine Ungenauigkeiten in der Flächenberechnung können nachträglich manuell geändert werden. Wollen Sie diese Raumgeometrie übernehmen?", "Raumgeometrie übernehmen?", MessageBoxButtons.YesNo);
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
			} else {
				if (this.Mode == RoomPickerMode.RPM_DEL_UNUSED) {
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
				if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT && coordsPickedSoFar.Count > 0) {
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
				if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT && coordsPickedSoFar.Count > 0) {
					if (coordsPickedSoFar.Count > 2) {
						normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], planPoint, coordsPickedSoFar[0], out isStart);
					}
				}
				this.ConnectedPlanPanel.PlanCursor = isStart ? Cursors.Hand : Cursors.Cross;
				return inDesign;
			}
			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
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

		private Point2D GetNormalizedPoint(Point2D basePoint1, Nullable<Point2D> basePoint2, Point2D currentPoint, Nullable<Point2D> startPoint, out bool isStartPoint) {
			if (startPoint.HasValue) {
				double scale = this.ConnectedPlanPanel.ScaleForCalculation;
				double xDistStart = (startPoint.Value.X - currentPoint.X) * scale;
				double yDistStart = (startPoint.Value.Y - currentPoint.Y) * scale;
				if (xDistStart * xDistStart + yDistStart * yDistStart < startPointSnapSqDist) {
					isStartPoint = true;
					return startPoint.Value;
				}
			}
			isStartPoint = false;
			if (ConnectedPlanPanel.SupportsSnap) {
				return currentPoint;
			}
			double xDistance1 = Math.Abs(basePoint1.X - currentPoint.X);
			double yDistance1 = Math.Abs(basePoint1.Y - currentPoint.Y);
			double xDistInMeter1 = Math.Round((currentPoint.X - basePoint1.X) / this.ConnectedPlanPanel.Plan.Measure.Value, 1);
			double yDistInMeter1 = Math.Round((currentPoint.Y - basePoint1.Y) / this.ConnectedPlanPanel.Plan.Measure.Value, 1);
			Point2D p;
			if (onlyHorizAndVert) {
				if (xDistance1 < yDistance1) {
					p = new Point2D(basePoint1.X, basePoint1.Y + ((float)yDistInMeter1 * this.ConnectedPlanPanel.Plan.Measure.Value));
				} else {
					p = new Point2D(basePoint1.X + ((float)xDistInMeter1 * this.ConnectedPlanPanel.Plan.Measure.Value), basePoint1.Y);
				}

			} else {
				if (basePoint2.HasValue) {
					double xDistance2 = Math.Abs(basePoint2.Value.X - currentPoint.X);
					double yDistance2 = Math.Abs(basePoint2.Value.Y - currentPoint.Y);
					double xTan1 = xDistance1 / yDistance1;
					double xTan2 = xDistance2 / yDistance2;
					double yTan1 = yDistance1 / xDistance1;
					double yTan2 = yDistance2 / xDistance2;
					double newX = currentPoint.X;
					double newY = currentPoint.Y;
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
					if (xDistance1 / yDistance1 <= angleSnapDist) {
						//p = new Point2D(basePoint1.X, basePoint1.Y + yDistInMeter1 * this.ConnectedPlanPanel.Plan.Measure.Value);
						p = new Point2D(basePoint1.X, currentPoint.Y);
					} else if (yDistance1 / xDistance1 <= angleSnapDist) {
						//p = new Point2D(basePoint1.X + xDistInMeter1 * this.ConnectedPlanPanel.Plan.Measure.Value, basePoint1.Y);
						p = new Point2D(currentPoint.X, basePoint1.Y);
					} else {
						//p = new Point2D(basePoint1.X + xDistInMeter1 * this.ConnectedPlanPanel.Plan.Measure.Value, basePoint1.Y + yDistInMeter1 * this.ConnectedPlanPanel.Plan.Measure.Value);
						p = new Point2D(currentPoint.X, currentPoint.Y);
					}
				}
			}
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
					if (MessageBox.Show("Wollen Sie das definieren der Raumgeometrie abbrechen?", "Abbrechen?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
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
					if (MessageBox.Show("Wollen Sie das definieren der Fläche abbrechen?", "Abbrechen?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
						this.inDesign = false;
						this.coordsPickedSoFar.Clear();
						return true;
					}
				}
			}
			return false;
		}
	}
}
