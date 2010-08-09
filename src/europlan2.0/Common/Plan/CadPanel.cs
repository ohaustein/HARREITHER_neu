using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WW.Cad.Model;
using WW.Cad.Drawing.GDI;
using WW.Math;
using WW.Cad.Drawing;
using WW.Cad.Base;

namespace Europlan.Common {
	public partial class CadPanel : UserControl {

		private DxfModel model;
		private GDIGraphics3D gdiGraphics3D;
		private Bounds3D bounds;
		private Matrix4D modelTransform = Matrix4D.Identity;
		private Matrix4D from2DTransform;
		private Vector3D translation = Vector3D.Zero;
		private Point lastMouseLocation;
		private Point mouseClickLocation;
		private double scale = 1.0;
		private bool mouseDown = false;
		private double defaultHeight = 1000.0;
		private double defaultWidth = 1000.0;
		private double defaultMargin = 5.0;
		private Matrix4D toDefaultSize = Matrix4D.Identity;
		private Matrix4D fromDefaultSize = Matrix4D.Identity;

		public CadPanel() {
			this.InitializeComponent();
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			GraphicsConfig graphicsConfig = new GraphicsConfig();
			graphicsConfig.BackColor = BackColor;
			graphicsConfig.CorrectColorForBackgroundColor = true;
			gdiGraphics3D = new GDIGraphics3D(graphicsConfig);
			//gdiGraphics3D = new GDIGraphics3D();
			bounds = new Bounds3D();
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			
		}

		protected override void OnPaint(PaintEventArgs e) {
			gdiGraphics3D.Draw(e.Graphics, this.ClientRectangle);
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			CalculateTo2DTransform();
			Invalidate();
		}

		public DxfModel Model {
			get { return model; }
			set {
				if (value != model) {
					model = value;
					if (model != null) {
						gdiGraphics3D.CreateDrawables(model);
						gdiGraphics3D.BoundingBox(bounds, modelTransform);
						defaultWidth = (bounds.Delta.X >= bounds.Delta.Y) ? 1000.0 : 1000.0 * bounds.Delta.X / bounds.Delta.Y;
						defaultHeight = (bounds.Delta.X <= bounds.Delta.Y) ? 1000.0 : 1000.0 * bounds.Delta.Y / bounds.Delta.X;
						this.CalculateToDefaultSizeTransform();
						this.CalculateTo2DTransform();
						this.Invalidate();
					}
				}
			}
		}

		public Point2D GetModelSpaceCoordinates(Point2D screenScapeCoordinates) {
			return from2DTransform.TransformTo2D(screenScapeCoordinates);
		}

		private Matrix4D CalculateToDefaultSizeTransform() {
			toDefaultSize = DxfUtil.GetScaleTransform(bounds.Corner1, bounds.Corner2, bounds.Center,
				new Point3D(defaultMargin, defaultHeight - 2.0 * defaultMargin, 0.0),
				new Point3D(defaultWidth - 2.0 * defaultMargin, defaultMargin, 0.0),
				new Point3D(defaultWidth / 2.0, defaultHeight / 2.0, 0.0));
			fromDefaultSize = toDefaultSize.GetInverse();
			return toDefaultSize;
		}

		private Matrix4D CalculateTo2DTransform() {
			Matrix4D to2DTransform = Matrix4D.Identity;
			if (model != null && bounds != null) {
				to2DTransform = to2DTransform * Transformation4D.Translation(translation);
				to2DTransform = to2DTransform * Transformation4D.Scaling(this.scale);

				to2DTransform = to2DTransform * toDefaultSize;

				gdiGraphics3D.To2DTransform = to2DTransform * modelTransform;
				from2DTransform = gdiGraphics3D.To2DTransform.GetInverse();
			}
			return to2DTransform;
		}

		public double PlanScale {
			get { return this.scale; }
			set {
				if (this.scale != value) {
					this.scale = value;
					CalculateTo2DTransform();
					this.Invalidate();
				}
			}
		}

		public Vector2D PlanTranslation {
			get { return new Vector2D(this.translation.X, this.translation.Y); }
			set {
				if (this.translation.X != value.X || this.translation.Y != value.Y) {
					this.translation.X = value.X;
					this.translation.Y = value.Y;
					CalculateTo2DTransform();
					this.Invalidate();
				}
			}
		}

		public void SetPlanScaleAndTranslation(double scale, double translationX, double translationY) {
			if (this.scale != scale || this.translation.X != translationX || this.translation.Y != translationY) {
				this.scale = scale;
				this.translation.X = translationX;
				this.translation.Y = translationY;
				CalculateTo2DTransform();
				this.Invalidate();
			}
		}

		public double PlanDefaultMargin {
			get { return this.defaultMargin; }
			set {
				if (this.defaultMargin != value) {
					this.defaultMargin = value;
					CalculateToDefaultSizeTransform();
					CalculateTo2DTransform();
					this.Invalidate();
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			lastMouseLocation = e.Location;
			mouseDown = true;
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (mouseDown) {
				translation += new Vector3D(e.X - lastMouseLocation.X, e.Y - lastMouseLocation.Y, 0);
				this.CalculateTo2DTransform();
				this.Invalidate();
			}
			lastMouseLocation = e.Location;
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			mouseDown = false;
		}

		public void RecreateDrawables() {
			gdiGraphics3D.CreateDrawables(model);
			this.Invalidate();
		}

		public void AddScale(double addedScale, Nullable<Point2D> center) {
			if (!center.HasValue) {
				center = new Point2D(this.ClientSize.Width / 2.0, this.ClientSize.Height / 2.0);
			}
			Matrix4D matrix = Matrix4D.Identity;

			matrix = matrix * Transformation4D.Translation(center.Value.X, center.Value.Y, 0);
			matrix = matrix * Transformation4D.Scaling(addedScale);
			matrix = matrix * Transformation4D.Translation(-center.Value.X, -center.Value.Y, 0);
			matrix = matrix * Transformation4D.Translation(this.translation);
			matrix = matrix * Transformation4D.Scaling(this.scale);

			Point3D referencePoint1 = new Point3D(1, 0, 0);
			Point3D transformedPoint1 = matrix.Transform(referencePoint1);
			Point3D referencePoint2 = new Point3D(0, 1, 0);
			Point3D transformedPoint2 = matrix.Transform(referencePoint2);
			Point3D referencePoint3 = new Point3D(0, 0, 1);
			Point3D transformedPoint3 = matrix.Transform(referencePoint3);

			Matrix4D scale = Transformation4D.GetScaleTransform(referencePoint1, referencePoint2, referencePoint3, transformedPoint1, transformedPoint2, transformedPoint3);
			this.scale = scale.M00;
			this.translation = new Vector3D(scale.M03, scale.M13, 0);
			
			CalculateTo2DTransform();
			Invalidate();
		}

		protected override void OnMouseWheel(MouseEventArgs e) {
			base.OnMouseWheel(e);
			this.AddScale(1.0 + e.Delta / 1200.0, new Point2D(e.X, e.Y));
		}
	}
}
