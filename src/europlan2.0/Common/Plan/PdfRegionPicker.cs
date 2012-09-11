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
	public partial class PdfRegionPicker : Component, IPlanner {

		public enum PdfRegionPickerMode {
			DPM_NONE,
			DPM_PICK_REGION,
		}

		public PdfRegionPicker() {
			InitializeComponent();
		}

		public PdfRegionPicker(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

        private event EventHandler regionPicked;

        public event EventHandler RegionPicked {
            add { regionPicked += value; }
            remove { regionPicked -= value; }
        }

		private PdfRegionPickerMode mode = PdfRegionPickerMode.DPM_NONE;
		private bool unsavedChanges = false;

		private Nullable<Point2D> oldStartPoint = null;
		private Nullable<Point2D> oldEndPoint = null;
		private Nullable<Point2D> startPoint = null;
		private Nullable<Point2D> curPoint = null;
		private Nullable<Point2D> endPoint = null;

		public PdfRegionPickerMode Mode {
			get { return this.mode; }
			set { this.mode = value; }
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set {
				this.connectedPlanPanel = value;
				if (this.connectedPlanPanel != null && this.connectedPlanPanel.Plan != null && this.connectedPlanPanel.Plan is TempImagePlan) {
					this.startPoint = new Point2D(0, 0);
					this.endPoint = (this.connectedPlanPanel.Plan as TempImagePlan).GetImageSize();
				} else {
					this.startPoint = null;
					this.endPoint = null;
				}
			}
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl);
		}

		internal void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Pen pen = Pens.Green;
			//Pen otherPen = new Pen(Color.FromArgb(128, Color.Red));
			Brush brush = new SolidBrush(Color.FromArgb(32, Color.Green));

			if (this.startPoint.HasValue) {
				if (this.endPoint.HasValue) {
					Point2D start = additionalTransformation.TransformTo2D(this.startPoint.Value);
					Point2D end = additionalTransformation.TransformTo2D(this.endPoint.Value);
					float x = (float)((start.X < end.X) ? start.X : end.X);
					float y = (float)((start.Y < end.Y) ? start.Y : end.Y);
					float width = (float)Math.Abs(end.X - start.X);
					float height = (float)Math.Abs(end.Y - start.Y);
					g.FillRectangle(brush, x, y, width, height);
					g.DrawRectangle(pen, x, y, width, height);
				} else if (this.curPoint.HasValue) {
					Point2D start = additionalTransformation.TransformTo2D(this.startPoint.Value);
					Point2D end = additionalTransformation.TransformTo2D(this.curPoint.Value);
					float x = (float)((start.X < end.X) ? start.X : end.X);
					float y = (float)((start.Y < end.Y) ? start.Y : end.Y);
					float width = (float)Math.Abs(end.X - start.X);
					float height = (float)Math.Abs(end.Y - start.Y);
					g.FillRectangle(brush, x, y, width, height);
					g.DrawRectangle(pen, x, y, width, height);
				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			this.oldStartPoint = this.startPoint;
			this.oldEndPoint = this.endPoint;
			this.startPoint = planPoint;
			this.curPoint = this.startPoint;
			this.endPoint = null;
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (curPoint != planPoint) {
				this.curPoint = planPoint;
				return !this.endPoint.HasValue;
			}
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			bool redraw = false;
			if (planPoint.X != this.startPoint.Value.X && planPoint.Y != this.startPoint.Value.Y) {
				this.endPoint = planPoint;
			} else {
				this.startPoint = this.oldStartPoint;
				this.endPoint = this.oldEndPoint;
				redraw = true;
			}
            this.OnRegionPicked();
			this.curPoint = null;
			return redraw;
		}
		#endregion

        private void OnRegionPicked() {
            if (this.regionPicked != null) {
                this.regionPicked(this, EventArgs.Empty);
            }
        }

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		public Cursor CustomCursor {
			get {
				if (this.mode == PdfRegionPickerMode.DPM_PICK_REGION) {
					return Cursors.Cross;
				} else {
					return null;
				}
			}
		}

        private event EventHandler modeChanged;
        public event EventHandler ModeChanged {
            add { this.modeChanged += value; }
            remove { this.modeChanged -= value; }
        }

		public bool PlannerKeyPress(Keys key) {
			return false;
		}

		public Nullable<Point2D> TopLeft {
			get {
				if (this.startPoint.HasValue && this.endPoint.HasValue) {
					return new Point2D((float)(Math.Min(this.startPoint.Value.X, this.endPoint.Value.X)), (float)(Math.Min(this.startPoint.Value.Y, this.endPoint.Value.Y)));
				} else {
					return null;
				}
			}
		}

		public Nullable<Point2D> BottomRight {
			get {
				if (this.startPoint.HasValue && this.endPoint.HasValue) {
					return new Point2D((float)(Math.Max(this.startPoint.Value.X, this.endPoint.Value.X)), (float)(Math.Max(this.startPoint.Value.Y, this.endPoint.Value.Y)));
				} else {
					return null;
				}
			}
		}

        public bool ShowPlanBackground {
            get { return true; }
        }
    }
}
