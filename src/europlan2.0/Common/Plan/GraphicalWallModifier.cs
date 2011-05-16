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
	public partial class GraphicalWallModifier : Component, IPlanner {

		public enum GraphicalWallModifierMode {
			GWM_NONE,
			GWM_PICK,
			GWM_OBSTACLE,
		}

		public class SelectedObjectArgs : EventArgs {
			private IGraphicalWallObject selectedObject;

			public SelectedObjectArgs(IGraphicalWallObject selectedObject) {
				this.selectedObject = selectedObject;
			}

			public IGraphicalWallObject SelectedObject {
				get { return this.selectedObject; }
				set { this.selectedObject = value; }
			}
		}

		private event EventHandler<SelectedObjectArgs> objectSelected;
		public event EventHandler<SelectedObjectArgs> ObjectSelected {
			add { this.objectSelected += value; }
			remove { this.objectSelected -= value; }
		}

		public GraphicalWallModifier() {
			InitializeComponent();
		}

		public GraphicalWallModifier(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private GraphicalWallModifierMode mode = GraphicalWallModifierMode.GWM_NONE;
		private Room room = null;
		private bool unsavedChanges = false;
		private IGraphicalWallObject selectedObject = null;
		private IGraphicalWallObject tempSelectedObject = null;
		private Point2D obstacleStart = Point2D.Zero;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Room Room {
			get { return this.room; }
			set { this.room = value; }
		}

		public GraphicalWallModifierMode Mode {
			get { return this.mode; }
			set {
				selectedObject = null;
				tempSelectedObject = null;
				obstacleStart = Point2D.Zero;
				if (value == GraphicalWallModifierMode.GWM_PICK) {

				} else if (value == GraphicalWallModifierMode.GWM_OBSTACLE) {

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
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl);
		}

		private void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			g.SmoothingMode = SmoothingMode.AntiAlias;

			GraphicsPath path = new GraphicsPath();
			List<PointF> transformedPoints = new List<PointF>();
			foreach (Point2D point in room.RoomCoordinates) {
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
			g.FillRegion(b, clipDisabled);

			Region oldClip = g.Clip;
			g.Clip = new Region(path);
			foreach (GraphicalWall wall in room.Walls) {
				if (wall.Enabled) {
					c = Color.FromArgb(255, Color.DarkRed);
				} else {
					c = Color.FromArgb(128, Color.DarkRed);
				}
				float size = (float)(room.AssociatedPlan.Measure * 0.1 * Math.Abs(additionalTransformation.M00));
				Pen p = new Pen(c, size);
				p.StartCap = LineCap.Square;
				p.EndCap = LineCap.Square;
				Point2D start = additionalTransformation.TransformTo2D(wall.PlanStartPoint.Value);
				Point2D end = additionalTransformation.TransformTo2D(wall.PlanEndPoint.Value);
				g.DrawLine(p, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);

				foreach (GraphicalWallObstacle obstacle in wall.Obstacles) {
					double startPos = 0;
					double width = 0; 
					if (obstacle is GraphicalDoor) {
						startPos = (obstacle as GraphicalDoor).GraphPosX;
						width = (obstacle as GraphicalDoor).Width;
					} else if (obstacle is GraphicalWindow) {
						startPos = (obstacle as GraphicalWindow).GraphPosX;
						width = (obstacle as GraphicalWindow).Width;
					}
					startPos /= 100;
					width /= 100;
					startPos /= room.AssociatedPlan.Measure.Value;
					width /= room.AssociatedPlan.Measure.Value;
					Vector2D vector = wall.PlanEndPoint.Value - wall.PlanStartPoint.Value;
					vector.Normalize();
					Point2D obstacleStart = wall.PlanStartPoint.Value + (vector * startPos);
					Point2D obstacleEnd = obstacleStart + (vector * width);
					obstacleStart = additionalTransformation.TransformTo2D(obstacleStart);
					obstacleEnd = additionalTransformation.TransformTo2D(obstacleEnd);
					size = (float)(room.AssociatedPlan.Measure * 0.1 * Math.Abs(additionalTransformation.M00));
					Pen pen = new Pen(Color.FromArgb(128, Color.Blue), size);
					g.DrawLine(pen, (float)obstacleStart.X, (float)obstacleStart.Y, (float)obstacleEnd.X, (float)obstacleEnd.Y);
				}

			}
			g.Clip = oldClip;

			if (this.Mode == GraphicalWallModifierMode.GWM_PICK) {
				GraphicalWall wall = GetClosestWall(mousePositionInPlan);
				if (wall != null) {
					oldClip = g.Clip;
					g.Clip = new Region(path);
					c = Color.FromArgb(255, Color.Red);
					float size = (float)(room.AssociatedPlan.Measure * 0.1 * Math.Abs(additionalTransformation.M00));
					Pen p = new Pen(c, size);
					//p.StartCap = LineCap.Square;
					//p.EndCap = LineCap.Square;
					Point2D start = additionalTransformation.TransformTo2D(wall.PlanStartPoint.Value);
					Point2D end = additionalTransformation.TransformTo2D(wall.PlanEndPoint.Value);
					g.DrawLine(p, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					g.Clip = oldClip;
				}

				if (selectedObject != null && selectedObject is GraphicalWall) {
					wall = selectedObject as GraphicalWall;
					oldClip = g.Clip;
					g.Clip = new Region(path);
					c = Color.FromArgb(255, Color.Red);
					float size = (float)(room.AssociatedPlan.Measure * 0.1 * Math.Abs(additionalTransformation.M00));
					Pen p = new Pen(c, size);
					//p.StartCap = LineCap.Square;
					//p.EndCap = LineCap.Square;
					Point2D start = additionalTransformation.TransformTo2D(wall.PlanStartPoint.Value);
					Point2D end = additionalTransformation.TransformTo2D(wall.PlanEndPoint.Value);
					g.DrawLine(p, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					g.Clip = oldClip;
				}
			}

			if (this.Mode == GraphicalWallModifierMode.GWM_OBSTACLE) {
				Point2D pointOnWall = Point2D.Zero;
				GraphicalWall result = null;
				double closest = double.PositiveInfinity;
				foreach (GraphicalWall wall in room.Walls) {
					Segment2D line = new Segment2D(wall.PlanStartPoint.Value, wall.PlanEndPoint.Value);
					double distance = line.GetDistance(mousePositionInPlan);
					if (distance < (room.AssociatedPlan.Measure * 0.1) && distance < closest) {
						result = wall;
						closest = distance;
						pointOnWall = line.GetClosestPoint(mousePositionInPlan);
					}
				}
				float size = (float)(this.Room.AssociatedPlan.Measure * 0.05 * Math.Abs(additionalTransformation.M00));
				Pen pen = new Pen(Color.Blue, 2);

				if (pointOnWall != Point2D.Zero) {
					Point2D p = additionalTransformation.TransformTo2D(pointOnWall);
					g.DrawLine(pen, (float)p.X - size, (float)p.Y - size, (float)p.X + size, (float)p.Y + size);
					g.DrawLine(pen, (float)p.X - size, (float)p.Y + size, (float)p.X + size, (float)p.Y - size);
				}
				if (obstacleStart != Point2D.Zero) {
					List<Segment2D> segments = GetSegments(room.RoomCoordinates, obstacleStart);
					double dist = double.MaxValue;
					Segment2D closestSegment = new Segment2D();
					foreach (Segment2D segment in segments) {
						if (segment.GetDistance(mousePositionInPlan) < dist) {
							dist = segment.GetDistance(mousePositionInPlan);
							closestSegment = segment;
						}
					}

					size = (float)(room.AssociatedPlan.Measure * 0.1 * Math.Abs(additionalTransformation.M00));
					pen = new Pen(Color.FromArgb(128, Color.Blue), size);

					Point2D tempEnd = closestSegment.GetClosestPoint(mousePositionInPlan);
					Point2D start = additionalTransformation.TransformTo2D(obstacleStart);
					Point2D end = additionalTransformation.TransformTo2D(tempEnd);
					g.DrawLine(pen, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);

					//Point2D start = additionalTransformation.TransformTo2D(obstacleStart);
					//g.DrawLine(pen, (float)start.X, (float)start.Y, (float)p.X, (float)p.Y);
				}

			}

			//Pen pen = Pens.Red;
			//double width = distributor.Width * this.floor.AssociatedPlan.Measure.Value;
			//double height = distributor.Height * this.floor.AssociatedPlan.Measure.Value;

			//Point2D leftBottom = Point2D.Zero;
			//Point2D leftTop = Point2D.Zero;
			//Point2D rightTop = Point2D.Zero;
			//Point2D rightBottom = Point2D.Zero;

			//if (this.Mode == DistributorPositionerMode.DPM_POSITION) {
			//    if (this.ConnectedPlanPanel.Plan is CadPlan) {
			//        additionalTransformation = additionalTransformation * Transformation4D.Translation(mousePositionInPlan.X, mousePositionInPlan.Y, 0);
			//        additionalTransformation = additionalTransformation * Transformation4D.RotateZ(-this.RotationInclPlan * Math.PI / 180.0);
			//        additionalTransformation = additionalTransformation * Transformation4D.Translation(-mousePositionInPlan.X, -mousePositionInPlan.Y, 0);

			//        leftBottom = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X, mousePositionInPlan.Y));
			//        leftTop = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X, mousePositionInPlan.Y + height));
			//        rightTop = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X + width, mousePositionInPlan.Y + height));
			//        rightBottom = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X + width, mousePositionInPlan.Y));
			//    } else {
			//        additionalTransformation = additionalTransformation * Transformation4D.Translation(mousePositionInPlan.X, mousePositionInPlan.Y, 0);
			//        additionalTransformation = additionalTransformation * Transformation4D.RotateZ(this.RotationInclPlan * Math.PI / 180.0);
			//        additionalTransformation = additionalTransformation * Transformation4D.Translation(-mousePositionInPlan.X, -mousePositionInPlan.Y, 0);

			//        leftBottom = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X, mousePositionInPlan.Y));
			//        leftTop = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X, mousePositionInPlan.Y - height));
			//        rightTop = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X + width, mousePositionInPlan.Y - height));
			//        rightBottom = additionalTransformation.TransformTo2D(new Point2D(mousePositionInPlan.X + width, mousePositionInPlan.Y));
			//    }
			//    g.DrawLine(pen, (float)leftBottom.X, (float)leftBottom.Y, (float)rightBottom.X, (float)rightBottom.Y);
			//    g.DrawLine(pen, (float)rightBottom.X, (float)rightBottom.Y, (float)rightTop.X, (float)rightTop.Y);
			//    g.DrawLine(pen, (float)rightTop.X, (float)rightTop.Y, (float)leftTop.X, (float)leftTop.Y);
			//    g.DrawLine(pen, (float)leftTop.X, (float)leftTop.Y, (float)leftBottom.X, (float)leftBottom.Y);
			//    g.FillPolygon(Brushes.Red, new PointF[] { new PointF((float)leftBottom.X, (float)leftBottom.Y), new PointF((float)rightBottom.X, (float)rightBottom.Y), new PointF((float)rightTop.X, (float)rightTop.Y) });
			//} else {
			//    foreach (Distributor.GraphicalRepresentation gp in distributor.GraphicalRepresentations) {
			//        if (gp.floorId == this.floor.Id) {
			//            if (this.ConnectedPlanPanel.Plan is CadPlan) {
			//                additionalTransformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
			//                additionalTransformation = additionalTransformation * Transformation4D.RotateZ(-gp.rotation * Math.PI / 180.0);
			//                additionalTransformation = additionalTransformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

			//                leftBottom = additionalTransformation.TransformTo2D(gp.position);
			//                leftTop = additionalTransformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y + height));
			//                rightTop = additionalTransformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y + height));
			//                rightBottom = additionalTransformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
			//            } else {
			//                additionalTransformation = additionalTransformation * Transformation4D.Translation(gp.position.X, gp.position.Y, 0);
			//                additionalTransformation = additionalTransformation * Transformation4D.RotateZ(gp.rotation * Math.PI / 180.0);
			//                additionalTransformation = additionalTransformation * Transformation4D.Translation(-gp.position.X, -gp.position.Y, 0);

			//                leftBottom = additionalTransformation.TransformTo2D(gp.position);
			//                leftTop = additionalTransformation.TransformTo2D(new Point2D(gp.position.X, gp.position.Y - height));
			//                rightTop = additionalTransformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y - height));
			//                rightBottom = additionalTransformation.TransformTo2D(new Point2D(gp.position.X + width, gp.position.Y));
			//            }
			//            g.DrawLine(pen, (float)leftBottom.X, (float)leftBottom.Y, (float)rightBottom.X, (float)rightBottom.Y);
			//            g.DrawLine(pen, (float)rightBottom.X, (float)rightBottom.Y, (float)rightTop.X, (float)rightTop.Y);
			//            g.DrawLine(pen, (float)rightTop.X, (float)rightTop.Y, (float)leftTop.X, (float)leftTop.Y);
			//            g.DrawLine(pen, (float)leftTop.X, (float)leftTop.Y, (float)leftBottom.X, (float)leftBottom.Y);
			//            g.FillPolygon(Brushes.Red, new PointF[] { new PointF((float)leftBottom.X, (float)leftBottom.Y), new PointF((float)rightBottom.X, (float)rightBottom.Y), new PointF((float)rightTop.X, (float)rightTop.Y) });
			//            break;
			//        }
			//    }
			//}
			path.Dispose();
		}

		private GraphicalWall GetClosestWall(Point2D mousePositionInPlan) {
			GraphicalWall result = null;
			double closest = double.PositiveInfinity;
			foreach (GraphicalWall wall in room.Walls) {
				Segment2D line = new Segment2D(wall.PlanStartPoint.Value, wall.PlanEndPoint.Value);
				double distance = line.GetDistance(mousePositionInPlan);
				if (distance < (room.AssociatedPlan.Measure * 0.1) && distance < closest) {
					result = wall;
					closest = distance;
				}
			}
			return result;
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == GraphicalWallModifierMode.GWM_PICK && button == MouseButtons.Left) {
				GraphicalWall wall = GetClosestWall(planPoint);
				if (wall != null && selectedObject != wall) {
					selectedObject = wall;
					OnObjectSelected(selectedObject);
					return true;
				} else {
					selectedObject = null;
					OnObjectSelected(selectedObject);
					return true;
				}
			}
			if (this.Mode == GraphicalWallModifierMode.GWM_OBSTACLE && button == MouseButtons.Left) {
				Point2D pointOnWall = Point2D.Zero;
				GraphicalWall wall = null;
				double closest = double.PositiveInfinity;
				Segment2D line;
				foreach (GraphicalWall w in room.Walls) {
					line = new Segment2D(w.PlanStartPoint.Value, w.PlanEndPoint.Value);
					double distance = line.GetDistance(planPoint);
					if (distance < (room.AssociatedPlan.Measure * 0.1) && distance < closest) {
						closest = distance;
						wall = w;
						pointOnWall = line.GetClosestPoint(planPoint);
					}
				}
				if (pointOnWall != Point2D.Zero) {
					foreach (Point2D point in this.room.RoomCoordinates) {
						Segment2D temp = new Segment2D(point, pointOnWall);
						if (temp.GetLength() < (room.AssociatedPlan.Measure * 0.1)) {
							pointOnWall = point;
							break;
						}
					}

					if (obstacleStart == Point2D.Zero) {
						obstacleStart = pointOnWall;
					} else {
						List<Segment2D> segments = GetSegments(room.RoomCoordinates, pointOnWall);
						foreach (Segment2D segment in segments) {
							if (GetSegments(room.RoomCoordinates, obstacleStart).Contains(segment)) {
								NewObstacleForm form = new NewObstacleForm();

								form.Width = (new Segment2D(obstacleStart, pointOnWall).GetLength() * 100) / room.AssociatedPlan.Measure.Value;
								DialogResult result = form.ShowDialog();
								if (result == DialogResult.OK) {
									Segment2D first = new Segment2D(wall.PlanStartPoint.Value, obstacleStart);
									Segment2D second = new Segment2D(wall.PlanStartPoint.Value, planPoint);
									double x = first.GetLength() < second.GetLength() ? first.GetLength() : second.GetLength();
									x = (x * 100) / room.AssociatedPlan.Measure.Value;
									if (form.ObstacleType == Europlan.Common.GraphicalWallObstacle.ObstacleTypeEnum.Door) {
										GraphicalDoor door = new GraphicalDoor();
										door.Width = form.Width;
										door.Height = form.Height;
										door.GraphPosX = x;
										wall.Obstacles.Add(door);
									} else {
										GraphicalWindow window = new GraphicalWindow();
										window.Width = form.Width;
										window.Height = form.Height;
										window.GraphPosX = x;
										window.GraphPosY = form.HeightOffset;
										wall.Obstacles.Add(window);
									}
								}
								form.Dispose();
							}
						}
						obstacleStart = Point2D.Zero;
					}
				}
				return true;
			}
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == GraphicalWallModifierMode.GWM_PICK) {
				GraphicalWall wall = GetClosestWall(planPoint);
				if (tempSelectedObject != wall) {
					tempSelectedObject = wall;
					if (tempSelectedObject != null) {
						OnObjectSelected(tempSelectedObject);
					} else {
						OnObjectSelected(selectedObject);
					}
					return true;
				}
			} else if (this.Mode == GraphicalWallModifierMode.GWM_OBSTACLE) {
				return true;
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

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		public Cursor CustomCursor {
			get {
				if (this.mode == GraphicalWallModifierMode.GWM_PICK) {
					return Cursors.Cross;
				} else if (this.mode == GraphicalWallModifierMode.GWM_OBSTACLE) {
					return Cursors.Cross;
				} else {
					return null;
				}
			}
		}

		public event EventHandler ModeChanged;

		public bool PlannerKeyPress(Keys key) {
			if (key == Keys.Escape && this.Mode == GraphicalWallModifierMode.GWM_OBSTACLE) {
				this.Mode = GraphicalWallModifierMode.GWM_NONE;
				this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
				return true;
			}
			return false;
		}

		protected virtual void OnObjectSelected(IGraphicalWallObject selectedObject) {
			if (this.objectSelected != null) {
				this.objectSelected(this, new SelectedObjectArgs(selectedObject));
			}
		}

		private List<Segment2D> GetSegments(List<Point2D> border, Point2D referencePoint) {
			double distance = Double.MaxValue;

			Point2D rzPoint = Point2D.Zero;
			Point2D prevPoint = Point2D.Zero;
			List<Segment2D> list = new List<Segment2D>();

			Segment2D line = new Segment2D();
			foreach (Point2D point in border) {
				if (prevPoint != Point2D.Zero) {
					line = new Segment2D(prevPoint, point);
					if (line.GetDistance(referencePoint) < (room.AssociatedPlan.Measure * 0.1)) {
						if (line.GetDistance(referencePoint) < distance) {
							list.Add(line);
						}
					}
				}
				prevPoint = point;
			}

			line = new Segment2D(prevPoint, border[0]);
			if (line.GetDistance(referencePoint) < (room.AssociatedPlan.Measure * 0.1)) {
				if (line.GetDistance(referencePoint) < distance) {
					list.Add(line);
				}
			}

			return list;
		}
	}
}
