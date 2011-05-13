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

		private event EventHandler<SelectedObjectArgs> objectSelected;
		public event EventHandler<SelectedObjectArgs> ObjectSelected {
			add { this.objectSelected += value; }
			remove { this.objectSelected -= value; }
		}

		protected virtual void OnObjectSelected(IGraphicalWallObject selectedObject) {
			if (this.objectSelected != null) {
				this.objectSelected(this, new SelectedObjectArgs(selectedObject));
			}
		}

		public enum PlanMode {
			PM_MOVE,
			PM_SELECT_OBJECT,
			PM_PLANNER_CLICK,
			PM_PLANNER_DRAG
		}

		public GraphicalWallPanel() {
			this.scale = 1;
			this.DoubleBuffered = true;
			this.Cursor = Cursors.SizeAll;
			InitializeComponent();
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

		public double TotalWidth {
			get {
				double totalWidth = 0;
				if (this.room != null && this.room.Walls != null) {
					foreach (GraphicalWall wall in this.room.Walls) {
						totalWidth += wall.GetWallWidth();
					}
				}
				return totalWidth;
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
				double maxY = - this.Height / this.Scale + 10;
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
				Console.WriteLine(this.yPos);
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
				wall.PaintObject(e.Graphics, this.room.GetWallOffset(wall).Value.X * 100.0, 0, this.SelectedObject, this.SelectedWall, this.Scale);
				//xOffset += wall.CeilingContour[wall.CeilingContour.Count - 1].X * 100.0;
			}

			e.Graphics.ResetClip();
			if (this.productPlanner != null) {
				e.Graphics.Transform = paintMatrix;
				Point pointInCtrl = this.PointToClient(MousePosition);
				Point2D pointInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(pointInCtrl.X, pointInCtrl.Y));
				this.productPlanner.PaintAfterPlanPannel(e, pointInPlan, pointInCtrl, this.Scale);
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

			if (mode == PlanMode.PM_SELECT_OBJECT) {
				if (this.selectedObject != null && this.selectedWall != null) {
					foreach (Anchor a in this.selectedObject.GetAnchors(this.scale)) {
						if (a.HitTest(mousePosInPlan, selectedWallXOffset * 100, selectedWallYOffset * 100, this.scale)) {
							this.draggingObject = a.Owner;
							this.draggingAnchor = a;
						}
					}
					if (this.draggingObject == null && this.selectedObject.IsMoveable && this.selectedObject.HitTest(mousePosInPlan, selectedWallXOffset * 100, selectedWallYOffset * 100)) {
						this.draggingObject = this.selectedObject;
						this.draggingAnchor = null;
					}
					if (this.draggingObject != null) {
						invalidate = invalidate || this.draggingObject.StartDrag(this.draggingAnchor, mousePosInPlan, this.selectedWall);
					}
				}
			}
			if ((mode == PlanMode.PM_MOVE && e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Middle)) {
				this.mouseDownXInCtrl = mousePosInCtrl.X;
				this.mouseDownYInCtrl = mousePosInCtrl.Y;
				this.mouseDownXInPlan = mousePosInPlan.X;
				this.mouseDownYInPlan = mousePosInPlan.Y;
				this.startXPos = this.XPos;
				this.startYPos = this.YPos;
			}
			if (e.Button == MouseButtons.Middle) {
				inMove = true;
				this.tempCursor = this.Cursor;
				this.Cursor = Cursors.SizeAll;
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

			if (mode == PlanMode.PM_SELECT_OBJECT) {
				if (this.draggingObject != null) {
					invalidate = invalidate || this.draggingObject.EndDrag(this.draggingAnchor, mousePosInPlan, this.selectedWall);
					this.draggingObject = null;
					this.draggingAnchor = null;
					// TODO set cursor correctly;
				}
			}
			if (e.Button == MouseButtons.Middle) {
				this.Cursor = this.tempCursor;
				inMove = false;
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

			if (mode == PlanMode.PM_SELECT_OBJECT) {
				if (this.draggingObject != null) {
					invalidate = invalidate || this.draggingObject.MoveDrag(this.draggingAnchor, mousePosInPlan, this.selectedWall);
				} else {
					if (this.selectedObject != null && this.selectedWall != null) {
						bool found = false;
						foreach (Anchor a in this.selectedObject.GetAnchors(this.scale)) {
							if (a.HitTest(mousePosInPlan, selectedWallXOffset * 100, selectedWallYOffset * 100, this.scale)) {
								found = true;
								this.Cursor = a.Cursor;
							}
						}
						if (!found && this.selectedObject.IsMoveable && this.selectedObject.HitTest(mousePosInPlan, selectedWallXOffset * 100, selectedWallYOffset * 100)) {
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
			if (invalidate) {
				this.Invalidate();
			}
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			base.OnMouseClick(e);
			if (this.room == null) {
				return;
			}

			//Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			Point mousePosInCtrl = new Point(e.X, e.Y);
			Point2D mousePosInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(mousePosInCtrl.X, mousePosInCtrl.Y)); ;
			bool invalidate = false;

			if (this.mode == PlanMode.PM_PLANNER_CLICK && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				invalidate = this.productPlanner.PlannerClick(mousePosInPlan, mousePosInCtrl, e.Button);
			}
			if (this.mode == PlanMode.PM_SELECT_OBJECT && e.Button == MouseButtons.Left) {
				/*if (this.selectedObject != null && this.selectedWall != null) {
					bool found = false;
					foreach (Anchor a in this.selectedObject.GetAnchors(this.scale)) {
						if (a.HitTest(mousePosInPlan, selectedWallXOffset * 100, selectedWallYOffset * 100, this.scale)) {
							found = true;
							this.Cursor = a.Cursor;
						}
					}
					if (!found && this.selectedObject.IsMoveable && this.selectedObject.HitTest(mousePosInPlan, selectedWallXOffset * 100, selectedWallYOffset * 100)) {
						found = true;
						this.Cursor = Cursors.SizeAll;
					}
					if (!found) {
						this.Cursor = Cursors.Default;
					}
				}*/
				// TODO
				if (this.draggingObject == null) {
					double xOffset = 0;
					IGraphicalWallObject pickedObject = null;
					foreach (GraphicalWall wall in this.room.Walls) {
						pickedObject = wall.GetPickedObject(mousePosInPlan, xOffset, 0);
						if (pickedObject != null) {
							break;
						}
						xOffset += wall.CeilingContour[wall.CeilingContour.Count - 1].X * 100;
					}
					if (pickedObject == null && this.productPlanner != null) {
						pickedObject = this.productPlanner.PickObject(mousePosInPlan);
					}
					if (pickedObject != null) {
						invalidate = true;
						this.SelectedObject = pickedObject;
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
					this.selectedObject = value;
					if (this.selectedObject != null) {
						GraphicalWall owningWall = null;
						bool found = false;
						//double xOffset = 0.0;
						foreach (GraphicalWall wall in this.room.Walls) {
							if (wall == this.selectedObject) {
								this.selectedWall = wall;
								//this.selectedWallXOffset = xOffset;
								//this.selectedWallYOffset = 0;
								found = true;
								break;
							}
							owningWall = wall.GetOwningWall(this.selectedObject);
							if (owningWall != null) {
								this.selectedWall = owningWall;
								//this.selectedWallXOffset = xOffset;
								//this.selectedWallYOffset = wall.GetWallYOffset(owningWall, 0).Value;
								found = true;
								break;
							}
							//xOffset += wall.GetWallWidth();
						}
						if (found) {
							Vector2D offset = this.room.GetWallOffset(this.selectedWall).Value;
							this.selectedWallXOffset = offset.X;
							this.selectedWallYOffset = offset.Y;
						} else {
							this.selectedWall = null;
						}
					}
					this.Invalidate();
					this.OnObjectSelected(this.selectedObject);
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
		}

		public PlanMode Mode {
			get { return this.mode; }
			set {
				if (this.mode != value) {
					this.mode = value;
					if (this.mode == PlanMode.PM_MOVE) {
						this.Cursor = Cursors.SizeAll;
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

			public SelectedObjectArgs(IGraphicalWallObject selectedObject) {
				this.selectedObject = selectedObject;
			}

			public IGraphicalWallObject SelectedObject {
				get { return this.selectedObject; }
				set { this.selectedObject = value; }
			}
		}
	}
}
