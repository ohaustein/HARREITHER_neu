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
		private RoomPickerMode mode = RoomPickerMode.RPM_PICK_ROOM;
		private List<Point2D> roomCoordinates = new List<Point2D>();
		private List<List<Point2D>> unusedCoordinates = new List<List<Point2D>>();
		private List<Point2D> coordsPickedSoFar = new List<Point2D>();
		private bool inDesign = false;
		private bool unsavedChanges = false;

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
			set { this.mode = value; }
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set { this.connectedPlanPanel = value; }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			Graphics g = e.Graphics;
			if (roomCoordinates.Count > 2) {
				GraphicsPath path = new GraphicsPath();
				path.StartFigure();
				PointF[] array = new PointF[roomCoordinates.Count];
				int i = 0;
				foreach (Point2D point in roomCoordinates) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
				}
				path.AddPolygon(array);
				path.CloseFigure();
				Color c = Color.FromArgb(128, Color.Red);
				Brush b = new SolidBrush(c);
				g.FillPath(b, path);
				g.DrawPath(new Pen(b), path);
				path.Dispose();
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
					pos = GetNormalizedPoint(points[points.Count - 1], pos);
				}
				points.Add(pos);
				//g.DrawPolygon(Pens.Black, points.ToArray());

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
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.Mode == RoomPickerMode.RPM_PICK_ROOM || this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
				PointF pos = new PointF((float)planPoint.X, (float)planPoint.Y);

				Point2D normalizedPoint = planPoint;
				if (coordsPickedSoFar.Count > 0 && (this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
					normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], normalizedPoint);
				}

				if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
					// pick areas to exclude from the room area
					if (!this.UnusedAreaIsValid(normalizedPoint)) {
						return false;
					}
				}
				if (button == MouseButtons.Left) {
					if (!inDesign && this.Mode == RoomPickerMode.RPM_PICK_ROOM && roomCoordinates.Count > 0) {
						// TODO
						DialogResult result = MessageBox.Show("Wollen Sie die bereits definierte Raumgeometrie verwerfen und neu definieren?", "Verwerfen und neu definieren?", MessageBoxButtons.YesNo);
						if (result == DialogResult.No) {
							return false;
						}
						roomCoordinates.Clear();
					}
					unsavedChanges = true;
					coordsPickedSoFar.Add(normalizedPoint);
					inDesign = true;
				} else if (button == MouseButtons.Right) {
					coordsPickedSoFar.Add(normalizedPoint);
					if (coordsPickedSoFar.Count > 2) {
						if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
							DialogResult result = MessageBox.Show("Die definierte Fläche beträgt " + Math.Round(Math.Abs(new Polygon2D(coordsPickedSoFar).GetArea()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2) + "m². Kleine Ungenauigkeiten in der Flächenberechnung können nachträglich manuell geändert werden. Wollen Sie diese Raumgeometrie übernehmen?", "Raumgeometrie übernehmen?", MessageBoxButtons.YesNo);
							if (result.Equals(DialogResult.Yes)) {
								roomCoordinates.AddRange(coordsPickedSoFar);
								unsavedChanges = true;
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
			}
			return false;
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
				if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT && coordsPickedSoFar.Count > 0) {
					normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], planPoint);
				}
				if (UnusedAreaIsValid(normalizedPoint)) {
					this.ConnectedPlanPanel.Cursor = Cursors.Cross;
				} else {
					this.ConnectedPlanPanel.Cursor = Cursors.No;
				}
				return inDesign;
			} else if (this.Mode == RoomPickerMode.RPM_DEL_UNUSED) {
				foreach (List<Point2D> unusedArea in unusedCoordinates) {
					Polygon2D polygon = new Polygon2D(unusedArea);
					if (polygon.IsInside(planPoint)) {
						this.ConnectedPlanPanel.Cursor = Cursors.Hand;
					} else {
						this.ConnectedPlanPanel.Cursor = Cursors.No;
					}
				}
				return false;
			} else if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
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

		private Point2D GetNormalizedPoint(Point2D basePoint, Point2D currentPoint) {
			if (ConnectedPlanPanel.SupportsSnap) {
				return currentPoint;
			}
			double xDistance = Math.Abs(basePoint.X - currentPoint.X);
			double yDistance = Math.Abs(basePoint.Y - currentPoint.Y);
			Point2D p;
			if (xDistance < yDistance) {
				double distanceInMeter = (currentPoint.Y - basePoint.Y) / this.ConnectedPlanPanel.Plan.Measure.Value;
				distanceInMeter = Math.Round(distanceInMeter, 1);
				p = new Point2D(basePoint.X, basePoint.Y + ((float)distanceInMeter * this.ConnectedPlanPanel.Plan.Measure.Value));
			} else {
				double distanceInMeter = (currentPoint.X - basePoint.X) / this.ConnectedPlanPanel.Plan.Measure.Value;
				distanceInMeter = Math.Round(distanceInMeter, 1);
				p = new Point2D(basePoint.X + ((float)distanceInMeter * this.ConnectedPlanPanel.Plan.Measure.Value), basePoint.Y);
			}

			return p;
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		public List<Point2D> RoomCoordinates {
			get { return this.roomCoordinates; }
			set { this.roomCoordinates = value; }
		}

		public List<List<Point2D>> UnusedCoordinates {
			get { return this.unusedCoordinates; }
			set { this.unusedCoordinates = value; }
		}

		public Cursor CustomCursor {
			get { return null; }
		}
	}
}
