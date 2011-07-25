using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WW.Math;
using System.Drawing.Drawing2D;
using WW.Math.Geometry;
using Europlan.Common.Icons;

namespace Europlan.Common {
	public partial class GraphicalWallPanel : UserControl {

		private IWallProductPlanner productPlanner;
		private Room room;
		private double scale;
		private double xPos;
		private double yPos;

		private bool mouseDown = false;
		private bool inMove = false;
		private PlanMode mode = PlanMode.PM_MOVE;

		private double mouseDownXInPlan;
		private double mouseDownYInPlan;
		private double mouseDownXInCtrl;
		private double mouseDownYInCtrl;

		private double startXPos;
		private double startYPos;

		private bool shiftPressed = false;

		private Cursor tempCursor;

		private IGraphicalWallObject selectedObject;
		private GraphicalWall selectedWall;
		private double selectedWallXOffset;
		private double selectedWallYOffset;

		private GraphicalWallObstacle.ObstacleTypeEnum newObstacleType = GraphicalWallObstacle.ObstacleTypeEnum.Window;

		private GraphicalWallSchraege.OrientationEnum newSchraegeOrientation = GraphicalWallSchraege.OrientationEnum.LEFT;

		private bool snapEnabled = true;

		private event EventHandler<SelectedObjectArgs> objectSelected;
		public event EventHandler<SelectedObjectArgs> ObjectSelected {
			add { this.objectSelected += value; }
			remove { this.objectSelected -= value; }
		}

		private event EventHandler selectedObjectModified;
		public event EventHandler SelectedObjectModified {
			add { this.selectedObjectModified += value; }
			remove { this.selectedObjectModified -= value; }
		}

		protected virtual void OnObjectSelected(IGraphicalWallObject selectedObject, IGraphicalWallObject oldSelectedObject, GraphicalWall oldSelectedWall) {
			if (this.objectSelected != null) {
				this.objectSelected(this, new SelectedObjectArgs(selectedObject, oldSelectedObject, oldSelectedWall));
			}
		}

		protected virtual void OnSelectedObjectModified(IGraphicalWallObject selectedObject) {
			if (this.selectedObjectModified != null) {
				this.selectedObjectModified(this, EventArgs.Empty);
			}
		}

		public enum PlanMode {
			PM_MOVE,
			PM_SELECT_OBJECT,
			PM_PLANNER_CLICK,
			PM_PLANNER_DRAG,
			PM_ADD_OBSTACLE,
			PM_ADD_SCHRAEGE
		}

		public GraphicalWallPanel() {
			this.scale = 1;
			this.DoubleBuffered = true;
			InitializeComponent();
		}

		[DefaultValue(true)]
		public bool SnapEnabled {
			get { return this.snapEnabled; }
			set { this.snapEnabled = value; }
		}

		public Room Room {
			get { return this.room; }
			set {
				this.room = value;
				// quick hack to recalculate correct x and y pos
				this.XPos = this.XPos;
				this.YPos = this.YPos;
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			e.Graphics.ResetClip();
			e.Graphics.Clear(Color.LightGray);
		}

		public double Scale {
			get { return this.scale; }
			set { this.scale = value; }
		}

		public GraphicalWallObstacle.ObstacleTypeEnum NewObstacleType {
			get { return newObstacleType; }
			set { newObstacleType = value; }
		}

		public GraphicalWallSchraege.OrientationEnum NewSchraegeOrientation {
			get { return this.newSchraegeOrientation; }
			set { this.newSchraegeOrientation = value; }
		}

		public double TotalWidth {
			get {
				/*double totalWidth = 0;
				if (this.room != null && this.room.Walls != null) {
					foreach (GraphicalWall wall in this.room.Walls) {
						totalWidth += wall.GetWallWidth();
					}
				}
				return totalWidth;*/
				if (this.room == null || this.room.Walls == null || this.room.Walls.Count == 0) {
					return 0;
				} else {
					return this.room.GetWallOffset(this.room.Walls[this.room.Walls.Count - 1]).Value.X + this.room.Walls[this.room.Walls.Count - 1].GetWallWidth();
				}
			}
		}

		public double TotalHeight {
			get {
				double totalHeight = 0;
				if (this.room != null && this.room.Walls != null) {
					double wallHeight;
					foreach (GraphicalWall wall in this.room.Walls) {
						wallHeight = wall.GetTotalWallHeight();
						if (wallHeight > totalHeight) {
							totalHeight = wallHeight;
						}
					}
				}
				return totalHeight;
			}
		}

		public double XPos {
			get { return this.xPos; }
			set {
				double maxX = 10;
				double minX = (-TotalWidth * 100) + this.Width / this.Scale - 10;
				if (maxX < minX) {
					// center
					this.xPos = (this.Width / this.Scale - (TotalWidth + 0.2) * 100) / 2.0;
				} else {
					if (value > maxX) {
						this.xPos = maxX;
					} else if (value < minX) {
						this.xPos = minX;
					} else {
						this.xPos = value;
					}
				}
			}
		}

		public double YPos {
			get { return this.yPos; }
			set {
				double maxY = -this.Height / this.Scale + 10 + 10 / this.Scale;
				double minY = -TotalHeight * 100 - 10;
				if (maxY < minY) {
					// center
					this.yPos = -(this.Height / this.Scale) + (-TotalHeight * 100 + (this.Height) / this.Scale) / 2.0;
				} else {
					if (value > maxY) {
						this.yPos = maxY;
					} else if (value < minY) {
						this.yPos = minY;
					} else {
						this.yPos = value;
					}
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			if (this.room == null || this.room.Walls == null) {
				return;
			}
			Matrix oldTransform = e.Graphics.Transform;
			Region oldClip = e.Graphics.Clip;
			e.Graphics.ResetClip();
			e.Graphics.Clear(Color.LightGray);

			/*Matrix paintMatrix = new Matrix();
			paintMatrix.Scale((float)this.Scale, -(float)this.Scale);
			paintMatrix.Translate((float)this.XPos, (float)(this.YPos));
			e.Graphics.Transform = paintMatrix;*/
			Matrix paintMatrix = this.PlanToControlMatrix;
			e.Graphics.Transform = paintMatrix;

			//double xOffset = 0;
			Pen wallBorderPen = Pens.Black;
			Brush wallBrush = new SolidBrush(Color.White);
			Pen unusableBorderPen = Pens.Gray;
			Brush unusableBrush = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Gray, Color.White);
			foreach (GraphicalWall wall in this.Room.Walls) {
				double xOffset = this.room.GetWallOffset(wall).Value.X * 100.0;
				double yOffset = 0;
				wall.PaintObject(e.Graphics, xOffset, yOffset, this.SelectedObject, this.SelectedWall, this.Scale, false);
				//xOffset += wall.CeilingContour[wall.CeilingContour.Count - 1].X * 100.0;
			}

			e.Graphics.ResetClip();
			if (this.productPlanner != null) {
				e.Graphics.Transform = paintMatrix;
				Point pointInCtrl = this.PointToClient(MousePosition);
				Point2D pointInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(pointInCtrl.X, pointInCtrl.Y));
				this.productPlanner.PaintAfterPlanPannel(e, pointInPlan, pointInCtrl, this.Scale);
			}

			if (this.newObstacle != null && this.newObstacleWall != null) {
				Vector2D offset = this.room.GetWallOffset(this.newObstacleWall).Value * 100;
				List<PointF> borderPoints = new List<PointF>();
				foreach (Point2D vertex in this.newObstacleWall.GetObjectBorders(offset.X, offset.Y)) {
					borderPoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
				}

				PointF[] pointArr = borderPoints.ToArray();

				GraphicsPath wallPath = new GraphicsPath();
				wallPath.AddPolygon(pointArr);
				Region wallClip = new Region(wallPath);
				e.Graphics.Clip = wallClip;

				this.newObstacle.PaintObject(e.Graphics, offset.X, offset.Y, this.newObstacle, scale, !this.newObstacleOk);
			}
			if (this.newSchraege != null && this.newSchraegeWall != null) {
				Vector2D offset = this.room.GetWallOffset(this.newSchraegeWall).Value * 100;
				this.newSchraege.PaintObject(e.Graphics, offset.X, offset.Y, this.newSchraege, scale, !this.newObstacleOk);
			}

			if (this.selectedObject != null && (!(this.selectedObject is GraphicalWall))) {
				if (this.selectedWall != null) {
					Vector2D offset = this.room.GetWallOffset(this.selectedWall).Value * 100;
					List<PointF> borderPoints = new List<PointF>();
					foreach (Point2D vertex in this.selectedWall.GetObjectBorders(offset.X, offset.Y)) {
						borderPoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
					}

					PointF[] pointArr = borderPoints.ToArray();

					GraphicsPath wallPath = new GraphicsPath();
					wallPath.AddPolygon(pointArr);
					Region wallClip = new Region(wallPath);
					e.Graphics.Clip = wallClip;
					this.selectedObject.PaintObject(e.Graphics, offset.X, offset.Y, this.selectedObject, scale, false);
				} else {
					this.selectedObject.PaintObject(e.Graphics, 0, 0, this.selectedObject, scale, false);
				}
			}
			if (this.mode == PlanMode.PM_SELECT_OBJECT && this.selectedObject != null) {
				e.Graphics.ResetClip();
				foreach (Anchor a in this.selectedObject.GetAnchors(scale)) {
					a.PaintAnchor(e.Graphics, this.selectedWallXOffset * 100, this.selectedWallYOffset * 100, scale);
				}
			}

			e.Graphics.Transform = oldTransform;
			e.Graphics.DrawRectangle(Pens.Gray, 0, 0, this.Width - 1, this.Height - 1);

			e.Graphics.Clip = oldClip;
		}

		public void InvalidateGraphics() {
			// quick hack to recalculate correct x and y pos
			this.XPos = this.XPos;
			this.YPos = this.YPos;

			this.Invalidate();
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);

			// quick hack to recalculate correct x and y pos
			this.XPos = this.XPos;
			this.YPos = this.YPos;

			this.InvalidateGraphics();
		}

		private Matrix3D PlanToControlMatrix3D {
			get {
				Matrix3D ctrlToPlan = Matrix3D.Identity;
				ctrlToPlan = ctrlToPlan * Transformation3D.Scaling(this.Scale, -this.Scale);
				ctrlToPlan = ctrlToPlan * Transformation3D.Translation(this.XPos, this.YPos);
				return ctrlToPlan;
			}
		}
		private Matrix3D ControlToPlanMatrix3D {
			get { return this.PlanToControlMatrix3D.GetInverse(); }
		}

		private Matrix PlanToControlMatrix {
			get {
				Matrix ctrlToPlan = new Matrix();
				ctrlToPlan.Scale((float)this.Scale, -(float)this.Scale);
				ctrlToPlan.Translate((float)this.XPos, (float)(this.YPos));
				return ctrlToPlan;
			}
		}
		private Matrix ControlToPlanMatrix {
			get {
				Matrix result = this.PlanToControlMatrix;
				result.Invert();
				return result;
			}
		}

		private Nullable<Point2D> dragStart = null;
		private Point2D dragEnd = Point2D.Zero;

		private GraphicalWallObstacle newObstacle = null;
		private GraphicalWall newObstacleWall = null;
		private double newObstacleWallXOffset, newObstacleWallYOffset;
		private bool newObstacleOk = true;

		private GraphicalWallSchraege newSchraege = null;
		private GraphicalWall newSchraegeWall = null;
		private double newSchraegeWallXOffset, newSchraegeWallYOffset;
		private bool newSchraegeOk = true;

		private Cursor oldCursor = null;

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			if (this.room == null) {
				return;
			}

			mouseDown = true;
			//Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			Point mousePosInCtrl = new Point(e.X, e.Y);
			Point2D mousePosInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(mousePosInCtrl.X, mousePosInCtrl.Y)); ;
			bool invalidate = false;
			if (mode == PlanMode.PM_PLANNER_DRAG && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				if (this.productPlanner != null) {
					invalidate = this.productPlanner.PlannerDragStart(mousePosInPlan, mousePosInCtrl, e.Button);
				}
			}

			if (mode == PlanMode.PM_ADD_OBSTACLE && this.room != null && e.Button == MouseButtons.Left) {
				this.dragStart = mousePosInPlan;
				this.newObstacleWall = this.room.GetWallForPoint(mousePosInPlan, out this.newObstacleWallXOffset, out this.newObstacleWallYOffset);
				this.SelectedObject = null;
				this.selectedWall = this.newObstacleWall;
				if (this.newObstacleWall != null) {
					switch (this.newObstacleType) {
						case GraphicalWallObstacle.ObstacleTypeEnum.Door:
							if (!this.newObstacleWall.IsDachSchraege) {
								this.newObstacle = new GraphicalDoor();
							} else {
								this.newObstacle = null;
							}
							break;
						case GraphicalWallObstacle.ObstacleTypeEnum.Window:
						case GraphicalWallObstacle.ObstacleTypeEnum.WindowTriangleLeft:
						case GraphicalWallObstacle.ObstacleTypeEnum.WindowTriangleRight:
							this.newObstacle = new GraphicalWindow();
							this.newObstacle.ObstacleType = this.newObstacleType;
							break;
						case GraphicalWallObstacle.ObstacleTypeEnum.Other:
							this.newObstacle = new GraphicalOtherObstacle();
							break;
						default:
							this.newObstacle = null;
							break;
					}
					if (this.newObstacle == null) {
						this.newObstacleWall = null;
					} else {
						this.newObstacleOk = false;
						this.newObstacle.IsNew = true;
						//this.SelectedObject = this.newObstacle;
					}
				}
			}
			if (mode == PlanMode.PM_ADD_SCHRAEGE && this.room != null && e.Button == MouseButtons.Left) {
				//this.dragStart = mousePosInPlan;
				this.newSchraegeWall = this.room.GetWallForPoint(mousePosInPlan, out this.newSchraegeWallXOffset, out this.newSchraegeWallYOffset);
				this.SelectedObject = null;
				this.selectedWall = this.newSchraegeWall;
				if (this.newSchraegeWall != null) {
					this.newSchraege = new GraphicalWallSchraege(this.newSchraegeWall, this.newSchraegeOrientation);
					//this.newSchraegeOk = false;
					this.newSchraege.IsNew = true;
				}
			}
			if (mode == PlanMode.PM_SELECT_OBJECT && e.Button == MouseButtons.Left) {
				if (this.selectedObject != null) {
					double offsetX = 0;
					double offsetY = 0;
					if (this.selectedWall != null) {
						offsetX = selectedWallXOffset * 100;
						offsetY = selectedWallYOffset * 100;
					}
					foreach (Anchor a in this.selectedObject.GetAnchors(this.scale)) {
						if (a.HitTest(mousePosInPlan, offsetX, offsetY, this.scale)) {
							this.draggingObject = a.Owner;
							this.draggingAnchor = a;
						}
					}
					if (this.draggingObject == null && this.selectedObject.IsMoveable && this.selectedObject.HitTest(mousePosInPlan, offsetX, offsetY)) {
						this.draggingObject = this.selectedObject;
						this.draggingAnchor = null;
					}
					if (this.draggingObject != null) {
						invalidate = invalidate || this.draggingObject.StartDrag(this.draggingAnchor, mousePosInPlan, this.selectedWall, this.room, this.productPlanner == null ? null : this.productPlanner.Product, snapEnabled);
					}
				}
			}
			if (e.Button == MouseButtons.Middle) {
				inMove = true;
				this.tempCursor = this.Cursor;
			}
			if ((mode == PlanMode.PM_MOVE && e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Middle)) {
				this.Cursor = EuroplanCursors.MOVE_PLAN_ACTIVE;
				this.mouseDownXInCtrl = mousePosInCtrl.X;
				this.mouseDownYInCtrl = mousePosInCtrl.Y;
				this.mouseDownXInPlan = mousePosInPlan.X;
				this.mouseDownYInPlan = mousePosInPlan.Y;
				this.startXPos = this.XPos;
				this.startYPos = this.YPos;
			}
			if (this.productPlanner != null) {
				if (this.productPlanner.CustomCursor != null) {
					if (this.oldCursor == null) {
						this.oldCursor = this.Cursor;
					}
					this.Cursor = this.productPlanner.CustomCursor;
				} else {
					if (this.oldCursor != null) {
						this.Cursor = this.oldCursor;
						this.oldCursor = null;
					}
				}
			}
			if (invalidate) {
				this.Invalidate();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			if (this.room == null) {
				return;
			}

			mouseDown = false;
			//Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			Point mousePosInCtrl = new Point(e.X, e.Y);
			Point2D mousePosInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(mousePosInCtrl.X, mousePosInCtrl.Y)); ;
			bool invalidate = false;
			if (mode == PlanMode.PM_PLANNER_DRAG && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				if (this.productPlanner != null) {
					invalidate = this.productPlanner.PlannerDragEnd(mousePosInPlan, mousePosInCtrl, e.Button);
				}
			}

			if (mode == PlanMode.PM_ADD_OBSTACLE) {
				if (this.newObstacleWall != null && this.newObstacle != null) {
					if (this.newObstacleOk) {
						this.newObstacleWall.Obstacles.Add(this.newObstacle);
						this.SelectedObject = this.newObstacle;
					}
					this.dragStart = null;
					this.newObstacle = null;
					this.newObstacleWall = null;
				}
				invalidate = true;
			}
			if (mode == PlanMode.PM_ADD_SCHRAEGE) {
				if (this.newSchraegeWall != null && this.newSchraege != null) {
					if (this.newSchraegeOk) {
						this.newSchraegeWall.Schraegen.Add(this.newSchraege);
						this.SelectedObject = this.newSchraege;
					}
					this.newSchraege = null;
					this.newSchraegeWall = null;
				}
				invalidate = true;
			}

			if (mode == PlanMode.PM_SELECT_OBJECT) {
				if (this.draggingObject != null) {
					invalidate = invalidate || this.draggingObject.EndDrag(this.draggingAnchor, mousePosInPlan, this.selectedWall, this.room, this.productPlanner == null ? null : this.productPlanner.Product, snapEnabled);
					this.draggingObject = null;
					this.draggingAnchor = null;
					// TODO set cursor correctly;
				}
			}
			if (mode == PlanMode.PM_MOVE) {
				this.Cursor = EuroplanCursors.MOVE_PLAN;
			}
			if (e.Button == MouseButtons.Middle) {
				this.Cursor = this.tempCursor;
				inMove = false;
			} else if (this.productPlanner != null) {
				if (this.productPlanner.CustomCursor != null) {
					if (this.oldCursor == null) {
						this.oldCursor = this.Cursor;
					}
					this.Cursor = this.productPlanner.CustomCursor;
				} else {
					if (this.oldCursor != null) {
						this.Cursor = this.oldCursor;
						this.oldCursor = null;
					}
				}
			}
			if (invalidate) {
				this.Invalidate();
			}
		}

		private IGraphicalWallObject draggingObject = null;
		private Anchor draggingAnchor = null;

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (this.room == null) {
				return;
			}

			//Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			Point mousePosInCtrl = new Point(e.X, e.Y);
			Point2D mousePosInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(mousePosInCtrl.X, mousePosInCtrl.Y)); ;

			bool invalidate = false;

			if (mode == PlanMode.PM_PLANNER_DRAG && mouseDown && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				if (this.productPlanner != null) {
					invalidate = this.productPlanner.PlannerDragMove(mousePosInPlan, mousePosInCtrl, e.Button);
				}
			}
			if (this.mode == PlanMode.PM_PLANNER_CLICK && this.productPlanner != null) {
				invalidate = this.productPlanner.PlannerMouseMove(mousePosInPlan, mousePosInCtrl, e.Button);
			}

			if (this.mode == PlanMode.PM_ADD_OBSTACLE && this.dragStart.HasValue && this.newObstacle != null && this.newObstacleWall != null && e.Button != MouseButtons.Middle) {
				double x = (mousePosInPlan.X < this.dragStart.Value.X ? mousePosInPlan.X : this.dragStart.Value.X) - this.newObstacleWallXOffset;
				double y = (mousePosInPlan.Y < this.dragStart.Value.Y ? mousePosInPlan.Y : this.dragStart.Value.Y) - this.newObstacleWallYOffset;
				double width = Math.Abs(mousePosInPlan.X - this.dragStart.Value.X);
				double height = this.newObstacle is GraphicalDoor ? mousePosInPlan.Y : Math.Abs(mousePosInPlan.Y - this.dragStart.Value.Y);
				this.newObstacle.GraphPosX = x;
				this.newObstacle.GraphPosY = y;
				this.newObstacle.Height = height;
				this.newObstacle.Width = width;
				if (this.snapEnabled) {
					this.newObstacle.SnapToHelplines(this.newObstacleWall.AllHelpLines, false, true);
					this.newObstacle.SnapToHelplines(this.newObstacleWall.AllHelpLines, true, false);
				}
				this.newObstacleOk = this.newObstacle.CheckValidity(this.newObstacleWall, this.newObstacleWallXOffset, this.newObstacleWallYOffset);
				if (this.newObstacleOk) {
					this.room.MarkErrors(this.newObstacle, this.newObstacleWall);
				} else {
					this.room.ClearErrors();
				}
				invalidate = true;
			}
			if (this.mode == PlanMode.PM_ADD_SCHRAEGE && this.newSchraege != null && this.newSchraegeWall != null && e.Button != MouseButtons.Middle) {
				double width, height;
				if (this.newSchraege.Orientation == GraphicalWallSchraege.OrientationEnum.LEFT) {
					width = mousePosInPlan.X - this.newSchraegeWallXOffset;
					height = -(mousePosInPlan.Y - this.newSchraegeWallYOffset - this.newSchraegeWall.GetWallHeight() * 100.0);
					this.newSchraege.Width = width;
					this.newSchraege.Height = height;
				} else {
					width = this.newSchraegeWallXOffset + this.newSchraegeWall.GetWallWidth() * 100.0 - mousePosInPlan.X;
					height = -(mousePosInPlan.Y - this.newSchraegeWallYOffset - this.newSchraegeWall.GetWallHeight() * 100.0);
					this.newSchraege.Width = width;
					this.newSchraege.Height = height;
				}
				this.room.MarkErrors(this.newSchraege, this.newSchraegeWall);
				// TODO
				invalidate = true;
			}
			if (mode == PlanMode.PM_SELECT_OBJECT && e.Button != MouseButtons.Middle) {
				if (this.draggingObject != null) {
					bool changed = this.draggingObject.MoveDrag(this.draggingAnchor, mousePosInPlan, this.selectedWall, this.room, this.productPlanner == null ? null : this.productPlanner.Product, snapEnabled);
					if (changed) {
						this.OnSelectedObjectModified(this.selectedObject);
					}
					invalidate = invalidate || changed;
				} else {
					if (this.selectedObject != null) {
						double offsetX = 0;
						double offsetY = 0;
						if (this.selectedWall != null) {
							offsetX = selectedWallXOffset * 100;
							offsetY = selectedWallYOffset * 100;
						}
						bool found = false;
						foreach (Anchor a in this.selectedObject.GetAnchors(this.scale)) {
							if (a.HitTest(mousePosInPlan, offsetX, offsetY, this.scale)) {
								found = true;
								this.Cursor = a.Cursor;
							}
						}
						if (!found && this.selectedObject.IsMoveable && this.selectedObject.HitTest(mousePosInPlan, offsetX, offsetY)) {
							found = true;
							this.Cursor = Cursors.SizeAll;
						}
						if (!found) {
							this.Cursor = Cursors.Default;
						}
					}
				}
			}
			if (mouseDown && ((mode == PlanMode.PM_MOVE && e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Middle))) {
				this.XPos = this.startXPos + (mousePosInCtrl.X - mouseDownXInCtrl) / this.Scale;
				this.YPos = this.startYPos + (mousePosInCtrl.Y - mouseDownYInCtrl) / -this.Scale;
				this.RecalculateScrollBar();
				invalidate = true;
			}
			if (this.productPlanner != null) {
				if (this.productPlanner.CustomCursor != null) {
					if (this.oldCursor == null) {
						this.oldCursor = this.Cursor;
					}
					this.Cursor = this.productPlanner.CustomCursor;
				} else {
					if (this.oldCursor != null) {
						this.Cursor = this.oldCursor;
						this.oldCursor = null;
					}
				}
			}
			if (invalidate) {
				this.Invalidate();
			}
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			base.OnMouseClick(e);
			if (this.room == null) {
				return;
			}

			Point mousePosInCtrl = new Point(e.X, e.Y);
			Point2D mousePosInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(mousePosInCtrl.X, mousePosInCtrl.Y)); ;
			bool invalidate = false;

			if (this.mode == PlanMode.PM_PLANNER_CLICK && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				invalidate = this.productPlanner.PlannerClick(mousePosInPlan, mousePosInCtrl, e.Button);
			}
			if (this.mode == PlanMode.PM_SELECT_OBJECT && e.Button == MouseButtons.Left) {
				if (this.draggingObject == null) {
					IGraphicalWallObject pickedObject = null;
					Vector2D offset;
					foreach (GraphicalWall wall in this.room.Walls) {
						offset = this.room.GetWallOffset(wall).Value * 100;
						pickedObject = wall.GetPickedObject(mousePosInPlan, offset.X, 0);
						if (pickedObject != null) {
							break;
						}
					}
					if (pickedObject == null && this.productPlanner != null || pickedObject is GraphicalWall) {
						IGraphicalWallObject pickedWall = pickedObject;
						pickedObject = this.productPlanner.PickObject(mousePosInPlan);
						if (pickedObject == null) {
							pickedObject = pickedWall;
						}
					}
					if (pickedObject != null) {
						invalidate = true;
						this.SelectedObject = pickedObject;
					}
				}
			}
			if (this.productPlanner != null) {
				if (this.productPlanner.CustomCursor != null) {
					if (this.oldCursor == null) {
						this.oldCursor = this.Cursor;
					}
					this.Cursor = this.productPlanner.CustomCursor;
				} else {
					if (this.oldCursor != null) {
						this.Cursor = this.oldCursor;
						this.oldCursor = null;
					}
				}
			}
			if (invalidate) {
				this.Invalidate();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			shiftPressed = e.Shift;
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			shiftPressed = false;
			base.OnKeyUp(e);
			if ((this.Mode == PlanMode.PM_PLANNER_CLICK || this.mode == PlanMode.PM_PLANNER_DRAG) && this.productPlanner != null) {
				this.productPlanner.PlannerKeyPress(e.KeyCode);
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e) {
			base.OnMouseWheel(e);
			if (this.room == null) {
				return;
			}

			Point center = this.PointToClient(this.PointToScreen(e.Location));
			AddScale(1.0f + ((float)e.Delta) / 1200.0f, new WW.Math.Point2D(center.X, center.Y));
			this.Invalidate();
		}

		public void AddScale(double addedScale, Nullable<WW.Math.Point2D> center) {
			if (this.Scale * addedScale < 0.01) {
				addedScale = 0.01 / this.Scale;
			}
			if (this.Scale * addedScale > 10000.0) {
				addedScale = 10000.0 / this.Scale;
			}
			double oldScale = this.Scale;
			double newScale = oldScale * addedScale;
			this.Scale = (float)newScale;
			double centerX = center.HasValue ? center.Value.X : this.ClientSize.Width / 2.0;
			double centerY = center.HasValue ? -center.Value.Y : -this.ClientSize.Height / 2.0;
			this.XPos = (float)((centerX - (centerX - this.XPos * oldScale) * addedScale) / newScale);
			this.YPos = (float)((centerY - (centerY - this.YPos * oldScale) * addedScale) / newScale);
			this.RecalculateScrollBar();
		}

		private void RecalculateScrollBar() {
			/*int value = -(int)(this.XPos + (this.Width / this.Scale));
			int min = -(10 + (int)(this.Width / this.Scale));
			int max = -(int)((-TotalWidth * 100) + this.Width / this.Scale - 10);
			if (value <= max && value >= min) {
				this.hScrollBar1.Minimum = min;
				this.hScrollBar1.Maximum = max;
				this.hScrollBar1.LargeChange = (int)(this.Width / this.Scale);
				this.hScrollBar1.Value = value;
				this.hScrollBar1.Visible = true;
			} else {
				this.hScrollBar1.Minimum = min;
				this.hScrollBar1.Maximum = max;
				this.hScrollBar1.LargeChange = max - min + 1;
				this.hScrollBar1.Value = min;
				this.hScrollBar1.Visible = false;
			}*/
		}

		public IWallProductPlanner ProductPlanner {
			get { return this.productPlanner; }
			set {
				if (this.productPlanner != null) {
					this.productPlanner.ConnectedWallPanel = null;
				}
				if (value != null && value.ConnectedWallPanel != null) {
					value.ConnectedWallPanel = null;
				}
				this.productPlanner = value;
				if (this.productPlanner != null) {
					this.productPlanner.ConnectedWallPanel = this;
				}
				this.Invalidate();
			}
		}

		private void hScrollBar1_Scroll(object sender, ScrollEventArgs e) {

		}

		public IGraphicalWallObject SelectedObject {
			get { return this.selectedObject; }
			set {
				if (this.selectedObject != value) {
					IGraphicalWallObject oldSelectedObject = this.selectedObject;
					GraphicalWall oldSelectedWall = this.SelectedWall;
					this.selectedObject = value;
					if (this.selectedObject != null) {
						GraphicalWall owningWall = null;
						bool found = false;
						foreach (GraphicalWall wall in this.room.Walls) {
							if (wall == this.selectedObject) {
								this.selectedWall = wall;
								found = true;
								break;
							}
							owningWall = wall.GetOwningWall(this.selectedObject);
							if (owningWall != null) {
								this.selectedWall = owningWall;
								found = true;
								break;
							}
						}
						if (found) {
							Vector2D offset = this.room.GetWallOffset(this.selectedWall).Value;
							this.selectedWallXOffset = offset.X;
							this.selectedWallYOffset = offset.Y;
						} else {
							this.selectedWall = null;
						}
					}
					if (selectedObject != oldSelectedObject && oldSelectedObject != null && oldSelectedWall != null) {
						/*Vector2D offset = this.room.GetWallOffset(oldSelectedWall).Value * 100;
						if (!oldSelectedObject.CheckValidity(oldSelectedWall, offset.X, offset.Y)) {
							if (oldSelectedObject is GraphicalWallObstacle) {
								oldSelectedWall.Obstacles.Remove(oldSelectedObject as GraphicalWallObstacle);
							} else if (oldSelectedObject is GraphicalRegisterWrapper) {
								oldSelectedWall.Registers.Remove(oldSelectedObject as GraphicalRegisterWrapper);
								foreach (PlannedProduct pp in this.room.PlannedProducts) {
									if (pp.Product is HithermProduct && oldSelectedObject is GraphicalHithermRegisterWrapper) {
										(pp.Product as HithermProduct).RemoveRegisterFromCircuit((oldSelectedObject as GraphicalHithermRegisterWrapper).Register);
									} else if (pp.Product is HithermCompactProduct) {
										// TODO
									}
								}
							} else if (oldSelectedObject is GraphicalHithermVerbindung) {
								foreach (PlannedProduct pp in this.room.PlannedProducts) {
									if (pp.Product is HithermProduct) {
										HithermProduct hp = pp.Product as HithermProduct;
										foreach (HithermCircuit hc in hp.PlannedCircuits) {
											if (hc.Links.Contains(oldSelectedObject as GraphicalHithermVerbindung)) {
												hc.Links.Remove(oldSelectedObject as GraphicalHithermVerbindung);
											}
										}
									}
								}
							}
						} else {
							this.room.DeleteErroneousObjects();
						}*/
						oldSelectedObject.IsNew = false;
						if (oldSelectedObject.Error) {
							oldSelectedObject.RevertState();
							this.room.ClearErrors();
						} else {
							this.room.DeleteErroneousObjects();
						}
					}
					if (selectedObject != null) {
						this.selectedObject.BackupState();
					}
					this.Invalidate();
					this.OnObjectSelected(this.selectedObject, oldSelectedObject, oldSelectedWall);
				}
			}
		}

		public double SelectedWallXOffset {
			get { return this.selectedWallXOffset; }
		}

		public double SelectedWallYOffset {
			get { return this.selectedWallYOffset; }
		}

		public GraphicalWall SelectedWall {
			get { return this.selectedWall; }
			set {
				if (this.selectedObject == null) {
					this.selectedWall = value;
					Nullable<Vector2D> offset = null;
					if (this.selectedWall != null) {
						offset = this.room.GetWallOffset(this.selectedWall);
					}
					if (offset.HasValue) {
						this.selectedWallXOffset = offset.Value.X;
						this.selectedWallXOffset = offset.Value.Y;
					} else {
						this.selectedWallXOffset = 0;
						this.selectedWallXOffset = 0;
					}
				}
			}
		}

		public PlanMode Mode {
			get { return this.mode; }
			set {
				if (this.mode != value) {
					this.mode = value;
					if (this.mode == PlanMode.PM_MOVE) {
						this.Cursor = EuroplanCursors.MOVE_PLAN; ;
					} else if (this.mode == PlanMode.PM_SELECT_OBJECT) {
						this.Cursor = Cursors.Default;
					} else {
						this.Cursor = Cursors.Default;
					}
					this.Invalidate();
				}
			}
		}

		public class SelectedObjectArgs : EventArgs {
			private IGraphicalWallObject selectedObject;
			private IGraphicalWallObject oldSelectedObject;
			private GraphicalWall oldSelectedWall;
			
			public SelectedObjectArgs(IGraphicalWallObject selectedObject, IGraphicalWallObject oldSelectedObject, GraphicalWall oldSelectedWall) {
				this.selectedObject = selectedObject;
				this.oldSelectedObject = oldSelectedObject;
				this.oldSelectedWall = oldSelectedWall;
			}

			public IGraphicalWallObject SelectedObject {
				get { return this.selectedObject; }
				set { this.selectedObject = value; }
			}

			public IGraphicalWallObject OldSelectedObject {
				get { return this.oldSelectedObject; }
				set { this.oldSelectedObject = value; }
			}

			public GraphicalWall OldSelectedWall {
				get { return this.oldSelectedWall; }
				set { this.oldSelectedWall = value; }
			}
		}
	}
}
