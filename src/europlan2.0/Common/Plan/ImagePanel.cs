using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using WW.Math;
using Europlan.Common.Icons;

namespace Europlan.Common {
	public partial class ImagePanel : UserControl, IPlanPanel {

		public delegate void LengthChangedEventHandler(object sender);
		public event LengthChangedEventHandler LengthChanged;

		private ImagePlan plan;
		private Image image = null;
		private PlanMode mode = PlanMode.PM_MOVE;
		private bool showRaster = false;
		private bool unsavedChanges = false;
		private bool unsavedRoomPickerChanges = false;
		Nullable<PointF> startPoint = null;
		Nullable<PointF> endPoint = null;
		private double length = 0;
		private float mouseDownX, mouseUpX, mouseDownY, mouseUpY;
		private bool inDesign = false;
		private bool inMove = false;
		private bool shiftPressed = false;
		private Cursor tempCursor = Cursors.Default;
		private bool mouseDown = false;

		private float angle = 0;
		private float xPos = 0;
		private float yPos = 0;
		private Nullable<float> scale = null;

		public ImagePanel() {
			InitializeComponent();
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			this.Mode = PlanMode.PM_MOVE;
		}

		public bool ShowRaster {
			get { return this.showRaster; }
			set { this.showRaster = value; }
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		public bool UnsavedRoomPickerChanges {
			get { return this.unsavedRoomPickerChanges; }
		}

		public Nullable<PointF> StartPoint {
			get { return this.startPoint; }
			set { this.startPoint = value; }
		}

		public Nullable<PointF> EndPoint {
			get { return this.endPoint; }
			set { this.endPoint = value; }
		}

		public double Length {
			get { return this.length; }
			set { this.length = value; }
		}

		public float Angle {
			get { return angle; }
			set { 
				angle = value;
				this.CalculateMatrices();
				this.Invalidate();
			}
		}

		public float XPos {
			get { return xPos; }
			set { 
				xPos = value;
				this.CalculateMatrices();
				this.Invalidate();
			}
		}

		public float YPos {
			get { return yPos; }
			set { 
				yPos = value;
				this.CalculateMatrices();
				this.Invalidate();
			}
		}

		public Nullable<float> Scale {
			get { return scale; }
			set { 
				scale = value;
				this.CalculateMatrices();
				this.Invalidate();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			shiftPressed = e.Shift;
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			shiftPressed = false;
			base.OnKeyUp(e);
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			base.OnMouseClick(e);
			Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			PointF[] tmp = new PointF[] { mousePosInCtrl };

			Matrix ctrlToPlan = new Matrix();
			ctrlToPlan.Translate(((float)image.Width / 2 + this.XPos) * (float)this.PlanScale, ((float)image.Height / 2 + this.YPos) * (float)this.PlanScale);
			ctrlToPlan.Rotate(this.Angle);
			ctrlToPlan.Translate(-((float)image.Width / 2 + this.XPos) * (float)this.PlanScale, -((float)image.Height / 2 + this.YPos) * (float)this.PlanScale);
			ctrlToPlan.Scale((float)this.PlanScale, (float)this.PlanScale);
			ctrlToPlan.Translate(this.XPos, this.YPos);
			ctrlToPlan.Invert();
			ctrlToPlan.TransformPoints(tmp);
			PointF mousePosInPlan = tmp[0];

			bool invalidate = false;

			if ((this.mode == PlanMode.PM_PLANNER_CLICK || this.mode == PlanMode.PM_PLANNER_DRAG || this.mode == PlanMode.PM_SET_DISTRIBUTOR) && this.productPlanner != null && e.Button != MouseButtons.Middle) {
				invalidate = this.productPlanner.PlannerClick(new WW.Math.Point2D(mousePosInPlan.X, mousePosInPlan.Y), mousePosInCtrl, e.Button);
			} else if (mode == PlanMode.PM_PICK_MEASURE && e.Button == MouseButtons.Left) {
				if (startPoint.HasValue && !endPoint.HasValue) {
					endPoint = mousePosInPlan;
					length = this.GetDistance(startPoint.Value.X, startPoint.Value.Y, endPoint.Value.X, endPoint.Value.Y);
					//txtLength.Enabled = true;
					if (LengthChanged != null) {
						LengthChanged(this);
					}
					//startPoint = null;
				} else {
					startPoint = mousePosInPlan;
					endPoint = null;
				}
			}
			if (invalidate) {
				this.Invalidate();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e) {
			base.OnMouseWheel(e);
			if (image == null) {
				return;
			}

			unsavedChanges = true;
			Point center = this.PointToClient(this.PointToScreen(e.Location));
			AddScale(1.0f + ((float)e.Delta) / 1200.0f, new WW.Math.Point2D(center.X, center.Y));
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e) {
			Point mousePosInCtrl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));

			Graphics g = e.Graphics;
			g.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);
			if (image != null) {
				Matrix paintMatrix = new Matrix();
				if (!this.Scale.HasValue) {
					float scaleX = (float)this.Width / (float)image.Width;
					float scaleY = (float)this.Height / (float)image.Height;
					float scale = Math.Min(scaleX, scaleY);
					scale = 1;
					this.Scale = scale;
				}
				paintMatrix.Translate((float)((image.Width / 2 + this.XPos) * this.PlanScale), (float)((image.Height / 2 + this.YPos) * this.PlanScale));
				paintMatrix.Rotate(this.Angle);
				paintMatrix.Translate(-(float)((image.Width / 2 + this.XPos) * this.PlanScale), -(float)((image.Height / 2 + this.YPos) * this.PlanScale));
				paintMatrix.Scale((float)this.PlanScale, (float)this.PlanScale);
				paintMatrix.Translate(this.XPos, this.YPos);
				g.Transform = paintMatrix;

				g.DrawImage(image, 0, 0, image.Width, image.Height);

				Matrix ctrlToPlan = paintMatrix.Clone();
				/*transformPointsMatrix.Translate(((float)image.Width / 2 + this.XPos) * this.Scale.Value, ((float)image.Height / 2 + this.YPos) * this.Scale.Value);
				transformPointsMatrix.Rotate(this.Angle);
				transformPointsMatrix.Translate(-((float)image.Width / 2 + this.XPos) * this.Scale.Value, -((float)image.Height / 2 + this.YPos) * this.Scale.Value);
				transformPointsMatrix.Scale(this.Scale.Value, this.Scale.Value);
				transformPointsMatrix.Translate(this.XPos, this.YPos);*/
				ctrlToPlan.Invert();
				PointF[] tmp = new PointF[] { mousePosInCtrl };
				ctrlToPlan.TransformPoints(tmp);
				PointF mousePosInPlan = tmp[0];

				g.SmoothingMode = SmoothingMode.AntiAlias;

				if (mode == PlanMode.PM_PICK_MEASURE && startPoint.HasValue) {
					if (endPoint.HasValue) {
						g.DrawLine(Pens.Red, startPoint.Value, endPoint.Value);
					} else {
						g.DrawLine(Pens.Red, startPoint.Value, mousePosInPlan);
					}
				}

				g.Transform = new Matrix();

				if (showRaster) {
					Pen pen = Pens.DarkGray.Clone() as Pen;
					//pen.DashStyle = DashStyle.Dash;
					for (int i = 0; i < this.Height; i = i + 100) {
						g.DrawLine(pen, 0, i, this.Width, i);
					}
					for (int i = 0; i < this.Width; i = i + 100) {
						g.DrawLine(pen, i, 0, i, this.Height);
					}
					pen.Dispose();
				}
				if (this.productPlanner != null) {
					e.Graphics.Transform = paintMatrix;
					this.productPlanner.PaintAfterPlanPannel(e, WW.Math.Matrix4D.Identity, new WW.Math.Point2D(mousePosInPlan.X, mousePosInPlan.Y), this.PointToClient(MousePosition));
				}
			}
		}

		public void AddScale(double addedScale, Nullable<WW.Math.Point2D> center) {
			if (this.PlanScale * addedScale < 0.01) {
				addedScale = 0.01 / this.PlanScale;
			}
			if (this.PlanScale * addedScale > 10000.0) {
				addedScale = 10000.0 / this.PlanScale;
			}
			double oldScale = this.PlanScale;
			double newScale = oldScale * addedScale;
			this.Scale = (float)newScale;
			double centerX = center.HasValue ? center.Value.X : this.ClientSize.Width / 2.0;
			double centerY = center.HasValue ? center.Value.Y : this.ClientSize.Height / 2.0;
			this.XPos = (float)((centerX - (centerX - this.XPos * oldScale) * addedScale) / newScale);
			this.YPos = (float)((centerY - (centerY - this.YPos * oldScale) * addedScale) / newScale);
		}

		private double GetDistance(double x1, double y1, double x2, double y2) {
			double result = 0;
			double part1 = Math.Pow((x2 - x1), 2);
			double part2 = Math.Pow((y2 - y1), 2);
			double underRadical = part1 + part2;
			result = Math.Sqrt(underRadical);
			return result;
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			if (image == null) {
				return;
			}

			mouseDown = true;
			if (mode == PlanMode.PM_PLANNER_DRAG && this.productPlanner != null && e.Button != MouseButtons.Middle) {
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
			}
			if ((mode == PlanMode.PM_MOVE && e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Middle)) {
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

				mouseDownX = arr[0].X;
				mouseDownY = arr[0].Y;
			}
			if (e.Button == MouseButtons.Middle) {
				inMove = true;
				this.tempCursor = this.Cursor;
				this.Cursor = EuroplanCursors.MOVE_PLAN_ACTIVE;
			}
			if (this.mode == PlanMode.PM_MOVE) {
				this.Cursor = EuroplanCursors.MOVE_PLAN_ACTIVE;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			if (image == null) {
				return;
			}

			mouseDown = false;
			bool invalidate = false;
			if (mode == PlanMode.PM_PLANNER_DRAG && this.productPlanner != null && e.Button != MouseButtons.Middle) {
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
			}
			if (this.mode == PlanMode.PM_MOVE) {
				this.Cursor = EuroplanCursors.MOVE_PLAN;
			}
			if (e.Button == MouseButtons.Middle) {
				this.Cursor = this.tempCursor;
				inMove = false;
			}
			if (invalidate) {
				this.Invalidate();
			}
		}

		private bool IsIntersecting (PointF p1, PointF p2, PointF p3, PointF p4) {
			float x1, x2, x3, x4, y1, y2, y3, y4;
			float ua, ub, ud;
			//float x, y;
			x1 = p1.X; x2 = p2.X; x3 = p3.X; x4 = p4.X;
			y1 = p1.Y; y2 = p2.Y; y3 = p3.Y; y4 = p4.Y;
			ud = ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
			if (ud != 0) {
				ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ud;
				ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ud;
				if (IsBetween(ua, 0, 1) && IsBetween(ub, 0, 1)) {
					return true;
				//    x = x1 + ua * (x2 - x1);
				//    y = y1 + ua * (y2 - y1);
				}
			}
			return false;
		}

		private bool IsBetween(float value, float min, float max) {
			if (value >= min && value <= max) return true;
			return false;
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (image == null) {
				return;
			}

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

			bool invalidate = false;

			if (mode == PlanMode.PM_PLANNER_DRAG && this.productPlanner != null && e.Button != MouseButtons.Middle && e.Button != MouseButtons.None) {
				if (this.productPlanner != null) {
					invalidate = this.productPlanner.PlannerDragMove(new WW.Math.Point2D(mousePosInPlan.X, mousePosInPlan.Y), mousePosInCtrl, e.Button);
				}
			} 
			if ((this.mode == PlanMode.PM_PLANNER_CLICK || this.mode == PlanMode.PM_SET_DISTRIBUTOR || (this.mode == PlanMode.PM_PLANNER_DRAG && e.Button == MouseButtons.None)) && this.productPlanner != null) {
				invalidate = this.productPlanner.PlannerMouseMove(new WW.Math.Point2D(arr[0].X, arr[0].Y), mousePosInCtrl, e.Button);
			}

			if (mouseDown && ((mode == PlanMode.PM_MOVE && e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Middle))) {
				unsavedChanges = true;

				mouseUpX = arr[0].X;
				mouseUpY = arr[0].Y;

				this.XPos += mouseUpX - mouseDownX;
				this.YPos += mouseUpY - mouseDownY;
				invalidate = true;
			} else if (mode == PlanMode.PM_PICK_MEASURE && startPoint.HasValue && !endPoint.HasValue) {
				this.Invalidate();
			}
			if (invalidate) {
				this.Invalidate();
			}
		}

		private void PicturePanel_Resize(object sender, EventArgs e) {
			this.Invalidate();
		}


		internal void ApplyChangesToPlan() {
			this.plan.Scale = this.Scale;
			this.plan.XPos = this.XPos;
			this.plan.YPos = this.YPos;
			this.plan.Angle = this.Angle;
		}

		private PointF GetNormalizedPoint(PointF basePoint, PointF currentPoint) {
			float xDistance = Math.Abs(basePoint.X - currentPoint.X);
			float yDistance = Math.Abs(basePoint.Y - currentPoint.Y);
			PointF p;
			if (xDistance < yDistance) {
				double distanceInMeter = (currentPoint.Y - basePoint.Y) / this.Plan.Measure.Value;
				distanceInMeter = Math.Round(distanceInMeter, 1);
				p = new PointF(basePoint.X, basePoint.Y + ((float)distanceInMeter * this.Plan.Measure.Value));
			} else {
				double distanceInMeter = (currentPoint.X - basePoint.X) / this.Plan.Measure.Value;
				distanceInMeter = Math.Round(distanceInMeter, 1);
				p = new PointF(basePoint.X + ((float)distanceInMeter * this.Plan.Measure.Value), basePoint.Y);
			}

			return p;
		}

		#region IPlanPanel Members
		private IPlanner productPlanner = null;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanner ProductPlanner {
			get { return this.productPlanner; }
			set {
				if (this.productPlanner != null) {
					this.productPlanner.ConnectedPlanPanel = null;
				}
				if (value != null && value.ConnectedPlanPanel != null) {
					value = null;
				}
				this.productPlanner = value;
				if (this.productPlanner != null) {
					this.productPlanner.ConnectedPlanPanel = this;
				}
				this.Invalidate();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double PlanScale {
			get { return this.Scale.HasValue ? this.Scale.Value : 1.0; }
			set { this.Scale = (float)value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WW.Math.Vector2D PlanTranslation {
			get { return new WW.Math.Vector2D(this.XPos, this.YPos); }
			set {
				this.XPos = (float)value.X;
				this.YPos = (float)value.Y;
			}
		}

		public PlanMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				switch (this.mode) {
					case PlanMode.PM_MOVE:
						this.Cursor = EuroplanCursors.MOVE_PLAN;
						break;
					case PlanMode.PM_PICK_MEASURE:
						this.Cursor = Cursors.Cross;
						break;
					case PlanMode.PM_PLANNER_CLICK:
						this.Cursor = this.ProductPlanner != null && this.ProductPlanner.CustomCursor != null ? this.ProductPlanner.CustomCursor : Cursors.Cross;
						break;
					case PlanMode.PM_PLANNER_DRAG:
						this.Cursor = this.ProductPlanner != null && this.ProductPlanner.CustomCursor != null ? this.ProductPlanner.CustomCursor : Cursors.Cross;
						break;
					case PlanMode.PM_SET_DISTRIBUTOR:
						this.Cursor = this.ProductPlanner != null && this.ProductPlanner.CustomCursor != null ? this.ProductPlanner.CustomCursor : Cursors.Cross;
						break;
					default:
						this.Cursor = Cursors.Default;
						break;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Plan Plan {
			get { return plan; }
			set {
				if (value is ImagePlan) {
					this.plan = value as ImagePlan;
					if (this.plan.AbsoluteFileName != "") {
						image = Image.FromFile(this.plan.AbsoluteFileName);
					}
					this.angle = plan.Angle;
					this.xPos = plan.XPos;
					this.yPos = plan.YPos;
					this.scale = plan.Scale;
					this.CalculateMatrices();
				} else if (value == null) {
					this.plan = null;
				}
			}
		}


		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WW.Math.Matrix4D PlanTransformation {
			get { return WW.Math.Matrix4D.Identity; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ColorMode ColorMode {
			get { return ColorMode.CM_WHITE_BG; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModifierKey ModifierKey {
			get {
				ModifierKey key = ModifierKey.MK_NONE;
				if (this.shiftPressed) {
					key = key | ModifierKey.MK_SHIFT;
				}
				return key;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SupportsSnap {
			get { return false; }
		}

		public void InvalidateGraphics() {
			this.Invalidate();
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Cursor PlanCursor {
			get { return this.Cursor; }
			set {
				if (this.Mode == PlanMode.PM_PLANNER_CLICK || this.Mode == PlanMode.PM_PLANNER_DRAG) {
					if (inMove) {
						this.tempCursor = value;
					} else {
						this.Cursor = value;
					}
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double ScaleForCalculation {
			get { return this.PlanScale; }
		}
		#endregion

		private void ImagePanel_KeyDown(object sender, KeyEventArgs e) {
			if (this.ProductPlanner != null) {
				if (this.ProductPlanner.PlannerKeyPress(e.KeyCode)) {
					this.Invalidate();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Cursor Cursor {
			get { return base.Cursor; }
			set { base.Cursor = value; }
		}

		private Matrix4D planToControl = Matrix4D.Identity;
		private Matrix4D controlToPlan = Matrix4D.Identity;

		private void CalculateMatrices() {
			Matrix4D tmp = Matrix4D.Identity;
			if (image == null) {
				planToControl = tmp;
				controlToPlan = tmp;
				return;
			}
			double scale = this.Scale.HasValue ? this.Scale.Value : 1.0;
			tmp = tmp * Transformation4D.Translation(((float)image.Width / 2.0 + this.XPos) * scale, ((float)image.Height / 2.0 + this.YPos) * scale, 0);
			tmp = tmp * Transformation4D.RotateZ(this.Angle);
			tmp = tmp * Transformation4D.Translation(-((float)image.Width / 2.0 + this.XPos) * scale, -((float)image.Height / 2.0 + this.YPos) * scale, 0);
			tmp = tmp * Transformation4D.Scaling(scale);
			tmp = tmp * Transformation4D.Translation(this.XPos, this.YPos, 0);

			planToControl = tmp;
			controlToPlan = planToControl.GetInverse();
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Matrix4D PlanToControl {
			get { return this.planToControl; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Matrix4D ControlToPlan {
			get { return this.controlToPlan; }
		}

		public void SetPlanTransformations(double scale, double translationX, double translationY, double rotation) {
			if (this.scale != scale || this.xPos != translationX || this.yPos != translationY || this.angle != rotation) {
				this.unsavedChanges = true;
				this.scale = (float)scale;
				this.xPos = (float)translationX;
				this.yPos = (float)translationY;
				this.angle = (float)rotation;
				this.CalculateMatrices();
				this.Invalidate();
			}
		}

		public void GetPlanTransformations(out double scale, out double translationX, out double translationY, out double rotation) {
			scale = this.scale.HasValue ? this.scale.Value : 1.0;
			translationX = this.xPos;
			translationY = this.yPos;
			rotation = this.angle;
		}
	}

}
