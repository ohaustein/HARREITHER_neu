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
				Point2D start = additionalTransformation.TransformTo2D(wall.PlanStartPoint.Value);
				Point2D end = additionalTransformation.TransformTo2D(wall.PlanEndPoint.Value);
				g.DrawLine(p, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);

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
					Point2D start = additionalTransformation.TransformTo2D(wall.PlanStartPoint.Value);
					Point2D end = additionalTransformation.TransformTo2D(wall.PlanEndPoint.Value);
					g.DrawLine(p, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					g.Clip = oldClip;
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
	}
}
