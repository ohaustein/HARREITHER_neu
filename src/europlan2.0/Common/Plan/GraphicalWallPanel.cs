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

		private IProductPlanner productPlanner;
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

		private double totalWidth = 0;
		private double totalHeight = 0;

		private Cursor tempCursor;

		public enum PlanMode {
			PM_MOVE
		}

		public GraphicalWallPanel() {
			this.scale = 1;
			this.DoubleBuffered = true;
			InitializeComponent();
		}

		public IProductPlanner ProductPlanner {
			get { return this.productPlanner; }
			set { this.productPlanner = value; }
		}

		public Room Room {
			get { return this.room; }
			set {
				this.room = value;
				double totalWidth = 0;
				double totalHeight = 0;
				if (this.room != null && this.room.Walls != null) {
					foreach (GraphicalWall wall in this.room.Walls) {
						totalWidth += wall.CeilingContour[wall.CeilingContour.Count - 1].X;
						foreach (Point2D point in wall.CeilingContour) {
							if (point.Y > totalHeight) {
								totalHeight = point.Y;
							}
						}
					}
				}
				this.totalWidth = totalWidth;
				this.totalHeight = totalHeight;
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

		public double XPos {
			get { return this.xPos; }
			set {
				if (value > 0 || totalWidth * 100 * this.Scale < this.Width) {
					this.xPos = 0;
				} else if (value < (-totalWidth * 100) + this.Width / this.Scale - 20) {
					this.xPos = -totalWidth * 100 + this.Width / this.Scale - 20;
				} else {
					this.xPos = value;
				}
			}
		}

		public double YPos {
			get { return this.yPos; }
			set {
				if (value > 0/* || totalHeight * 100 * this.Scale < this.Height*/) {
					this.yPos = 0;
				} else if (value < (-totalHeight * 100) + this.Height / this.Scale - 20) {
					this.yPos = (-totalHeight * 100) + this.Height / this.Scale - 20;
				} else {
					this.yPos = value;
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

			Matrix paintMatrix = new Matrix();
			paintMatrix.Scale((float)this.Scale, -(float)this.Scale);
			paintMatrix.Translate((float)this.XPos + 10, (float)(-(this.Height - 10.0) + this.YPos));
			e.Graphics.Transform = paintMatrix;

			double startX = 0;
			Pen wallBorderPen = Pens.Black;
			Brush wallBrush = new SolidBrush(Color.White);
			Pen unusableBorderPen = Pens.Gray;
			Brush unusableBrush = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Gray, Color.White);
			foreach (GraphicalWall wall in this.Room.Walls) {
				Polygon2D wallBorder = new Polygon2D();
				wallBorder.Add(new Point2D(startX, 0));
				if (wall.CeilingContour[0].X != 0) {
					wallBorder.Add(new Point2D(startX, wall.CeilingContour[0].Y * 100.0));
				}
				foreach (Point2D vertex in wall.CeilingContour) {
					wallBorder.Add(new Point2D(startX + vertex.X * 100.0, vertex.Y * 100.0));
				}
				wallBorder.Add(new Point2D(startX + wall.CeilingContour[wall.CeilingContour.Count - 1].X * 100.0, 0));

				List<PointF> borderPoints = new List<PointF>();
				foreach (Point2D vertex in wallBorder) {
					borderPoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
				}
				borderPoints.Add(new PointF((float)(startX + wall.CeilingContour[wall.CeilingContour.Count - 1].X * 100.0), 0));
				PointF[] pointArr = borderPoints.ToArray();
				e.Graphics.FillPolygon(wallBrush, pointArr);

				Polygon2D usableArea = new Polygon2D(wallBorder);
				usableArea.Outset(-wall.BorderDistance * 100.0);
				List<PointF> usablePoints = new List<PointF>();
				foreach (Point2D vertex in usableArea) {
					usablePoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
				}

				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(borderPoints.ToArray());
				Region clip = new Region(path);
				GraphicsPath excludePath = new GraphicsPath();
				excludePath.AddPolygon(usablePoints.ToArray());
				clip.Exclude(excludePath);
				e.Graphics.Clip = clip;

				e.Graphics.FillPolygon(unusableBrush, borderPoints.ToArray());

				e.Graphics.ResetClip();
				e.Graphics.DrawPolygon(unusableBorderPen, usablePoints.ToArray());
				e.Graphics.DrawPolygon(wallBorderPen, pointArr);

				startX += wall.CeilingContour[wall.CeilingContour.Count - 1].X * 100.0;
			}

			e.Graphics.Transform = oldTransform;
			e.Graphics.DrawRectangle(Pens.Gray, 0, 0, this.Width - 1, this.Height - 1);
			e.Graphics.Clip = oldClip;


			/*if (gdiGraphics3D != null) {
				gdiGraphics3D.Draw(e.Graphics, this.ClientRectangle);
				if (selectedStartPointCad.HasValue) {
					Point3D start = gdiGraphics3D.To2DTransform.Transform(selectedStartPointCad.Value);
					if (selectedEndPointCad.HasValue) {
						Point3D end = gdiGraphics3D.To2DTransform.Transform(selectedEndPointCad.Value);
						e.Graphics.DrawLine(Pens.Red, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					} else {
						e.Graphics.DrawLine(Pens.Red, (float)start.X, (float)start.Y, (float)lastMouseLocation.X, (float)lastMouseLocation.Y);
					}
				}

				if (this.productPlanner != null) {
					Point mousePosInPlan = this.PointToClient(MousePosition);

					//Point3D planPoint = gdiGraphics3D.To2DTransform.GetInverse().Transform(new Point3D(mousePosInPlan.X, mousePosInPlan.Y, 0));
					Point3D planPoint = from2DTransform.Transform(new Point3D(mousePosInPlan.X, mousePosInPlan.Y, 0));
					this.productPlanner.PaintAfterPlanPannel(e, this.gdiGraphics3D.To2DTransform, new Point2D(planPoint.X, planPoint.Y), mousePosInPlan);
				}
			}*/
		}

		public void InvalidateGraphics() {
			this.Invalidate();
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			this.InvalidateGraphics();
		}

		private Matrix3D ControlToPlanMatrix3D {
			get {
				Matrix3D ctrlToPlan = Matrix3D.Identity;
				ctrlToPlan = ctrlToPlan * Transformation3D.Scaling(this.Scale, -this.Scale);
				ctrlToPlan = ctrlToPlan * Transformation3D.Translation(this.XPos + 10, -(this.Height - 10) + this.YPos);
				return ctrlToPlan;
			}
		}

		private Matrix ControlToPlanMatrix {
			get {
				Matrix ctrlToPlan = new Matrix();
				ctrlToPlan.Scale((float)this.Scale, -(float)this.Scale);
				ctrlToPlan.Translate((float)this.XPos + 10, (float)(-(this.Height - 10.0) + this.YPos));
				return ctrlToPlan;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			if (this.room == null) {
				return;
			}

			mouseDown = true;
			Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			mouseDownXInCtrl = mousePosInCtrl.X;
			mouseDownYInCtrl = mousePosInCtrl.Y;
			/*if (mode == PlanMode.PM_PLANNER_DRAG && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
				PointF[] arr = new PointF[] { mousePosInCtrl };

				Matrix ctrlToPlan = new Matrix();
				ctrlToPlan.Translate(((float)image.Width / 2 + this.XPos) * (float)this.PlanScale, ((float)image.Height / 2 + this.YPos) * (float)this.PlanScale);
				ctrlToPlan.Rotate(this.Angle);
				ctrlToPlan.Translate(-((float)image.Width / 2 + this.XPos) * (float)this.PlanScale, -((float)image.Height / 2 + this.YPos) * (float)this.PlanScale);
				ctrlToPlan.Scale((float)this.PlanScale, (float)this.PlanScale);
				ctrlToPlan.Translate(this.XPos, this.YPos);
				ctrlToPlan.Invert();
				ctrlToPlan.TransformPoints(arr);

				PointF mousePosInPlan = arr[0];

				if (this.productPlanner != null) {
					this.productPlanner.PlannerDragStart(new WW.Math.Point2D(mousePosInPlan.X, mousePosInPlan.Y), mousePosInCtrl, e.Button);
				}
			}*/
			if ((mode == PlanMode.PM_MOVE && e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Middle)) {
				

				Point2D mousePosInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(mousePosInCtrl.X, mousePosInCtrl.Y));
				this.mouseDownXInPlan = mousePosInPlan.X;
				this.mouseDownYInPlan = mousePosInPlan.Y;
				this.startXPos = this.XPos;
				this.startYPos = this.YPos;

				//				ctrlToPlan.Scale(

				/*PointF[] arr = new PointF[] { mousePosInCtrl };

				Matrix ctrlToPlan = new Matrix();
				ctrlToPlan.Translate(((float)image.Width / 2 + this.XPos) * (float)this.PlanScale, ((float)image.Height / 2 + this.YPos) * (float)this.PlanScale);
				ctrlToPlan.Rotate(this.Angle);
				ctrlToPlan.Translate(-((float)image.Width / 2 + this.XPos) * (float)this.PlanScale, -((float)image.Height / 2 + this.YPos) * (float)this.PlanScale);
				ctrlToPlan.Scale((float)this.PlanScale, (float)this.PlanScale);
				ctrlToPlan.Translate(this.XPos, this.YPos);
				ctrlToPlan.Invert();
				ctrlToPlan.TransformPoints(arr);

				mouseDownX = arr[0].X;
				mouseDownY = arr[0].Y;*/
			}
			if (e.Button == MouseButtons.Middle) {
				inMove = true;
				this.tempCursor = this.Cursor;
				this.Cursor = Cursors.SizeAll;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			if (this.room == null) {
				return;
			}

			mouseDown = false;
			bool invalidate = false;
			/*if (mode == PlanMode.PM_PLANNER_DRAG && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
				PointF[] arr = new PointF[] { mousePosInCtrl };

				Matrix ctrlToPlan = new Matrix();
				ctrlToPlan.Translate(((float)image.Width / 2 + this.XPos) * (float)this.PlanScale, ((float)image.Height / 2 + this.YPos) * (float)this.PlanScale);
				ctrlToPlan.Rotate(this.Angle);
				ctrlToPlan.Translate(-((float)image.Width / 2 + this.XPos) * (float)this.PlanScale, -((float)image.Height / 2 + this.YPos) * (float)this.PlanScale);
				ctrlToPlan.Scale((float)this.PlanScale, (float)this.PlanScale);
				ctrlToPlan.Translate(this.XPos, this.YPos);
				ctrlToPlan.Invert();
				ctrlToPlan.TransformPoints(arr);

				PointF mousePosInPlan = arr[0];

				if (this.productPlanner != null) {
					invalidate = this.productPlanner.PlannerDragEnd(new WW.Math.Point2D(mousePosInPlan.X, mousePosInPlan.Y), mousePosInCtrl, e.Button);
				}
			}*/
			if (e.Button == MouseButtons.Middle) {
				this.Cursor = this.tempCursor;
				inMove = false;
			}
			if (invalidate) {
				this.Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (this.room == null) {
				return;
			}

			Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			Point2D mousePosInPlan = this.ControlToPlanMatrix3D.Transform(new Point2D(mousePosInCtrl.X, mousePosInCtrl.Y)); ;

			bool invalidate = false;

			/*if (mode == PlanMode.PM_PLANNER_DRAG && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				if (this.productPlanner != null) {
					invalidate = this.productPlanner.PlannerDragMove(new WW.Math.Point2D(mousePosInPlan.X, mousePosInPlan.Y), mousePosInCtrl, e.Button);
				}
			}
			if ((this.mode == PlanMode.PM_PLANNER_CLICK || this.mode == PlanMode.PM_SET_DISTRIBUTOR) && this.productPlanner != null) {
				invalidate = this.productPlanner.PlannerMouseMove(new WW.Math.Point2D(arr[0].X, arr[0].Y), mousePosInCtrl, e.Button);
			}*/

			if (mouseDown && ((mode == PlanMode.PM_MOVE && e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Middle))) {
				this.XPos = this.startXPos + (mousePosInCtrl.X - mouseDownXInCtrl) / this.Scale;
				this.YPos = this.startYPos + (mousePosInCtrl.Y - mouseDownYInCtrl) / -this.Scale;
				Console.WriteLine("x: " + this.XPos + " y: " + this.YPos);
				invalidate = true;
			/*} else if (mode == PlanMode.PM_PICK_MEASURE && startPoint.HasValue && !endPoint.HasValue) {
				this.Invalidate();*/
			}
			if (invalidate) {
				this.Invalidate();
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
		}
	}
}
