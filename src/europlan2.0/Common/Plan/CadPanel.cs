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
using System.Collections;
using WW.Cad.Model.Entities;
using WW.Math.Geometry;

namespace Europlan.Common {
	public partial class CadPanel : UserControl {

		public class StartPointSelectedArgs : EventArgs {
			private Point3D startPoint;

			internal StartPointSelectedArgs(Point3D startPoint) {
				this.startPoint = startPoint;
			}

			public Point3D StartPoint {
				get { return this.startPoint; }
			}
		}

		public class EndPointSelectedArgs : EventArgs {
			private Point3D startPoint;
			private Point3D endPoint;
			private double length;

			internal EndPointSelectedArgs(Point3D startPoint, Point3D endPoint, double length) {
				this.startPoint = startPoint;
				this.endPoint = endPoint;
				this.length = length;
			}

			public Point3D StartPoint {
				get { return this.startPoint; }
			}

			public Point3D EndPoint {
				get { return this.endPoint; }
			}

			public double Length {
				get { return this.length; }
			}
		}

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

		private Nullable<Point3D> selectedStartPointCad = null;
		private Nullable<Point3D> selectedEndPointCad = null;

		public event EventHandler<StartPointSelectedArgs> StartPointSelected;
		public event EventHandler<EndPointSelectedArgs> EndPointSelected;

		private bool unsavedChanges = false;

		private bool moveMode = true;
		private bool shiftPressed = false;

		public CadPanel() {
			this.InitializeComponent();
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			//GraphicsConfig graphicsConfig = GraphicsConfig.WhiteBackgroundCorrectForBackColor;
			GraphicsConfig graphicsConfig = new GraphicsConfig();
			graphicsConfig.BackColor = BackColor;
			graphicsConfig.CorrectColorForBackgroundColor = true;
			gdiGraphics3D = new GDIGraphics3D(graphicsConfig);
			//gdiGraphics3D = new GDIGraphics3D();
			bounds = new Bounds3D();
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		protected override void OnPaint(PaintEventArgs e) {
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
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			CalculateTo2DTransform();
			Invalidate();
		}

		public bool MoveMode {
			get { return this.moveMode; }
			set {
				this.moveMode = value;
				this.Cursor = this.moveMode ? Cursors.Hand : Cursors.Cross;
				if (this.moveMode) {
					if (this.selectedStartPointCad.HasValue || this.selectedEndPointCad.HasValue) {
						this.selectedEndPointCad = null;
						this.selectedStartPointCad = null;
						this.Invalidate();
					}
				}
			}
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
				this.unsavedChanges = true;
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

		protected override void OnKeyDown(KeyEventArgs e) {
			shiftPressed = e.Shift;
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			shiftPressed = false;
			base.OnKeyUp(e);
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			lastMouseLocation = e.Location;
			mouseDown = true;
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (mouseDown && ((moveMode && e.Button == MouseButtons.Left) || e.Button == MouseButtons.Middle)) {
				this.unsavedChanges = true;
				translation += new Vector3D(e.X - lastMouseLocation.X, e.Y - lastMouseLocation.Y, 0);
				this.CalculateTo2DTransform();
				this.Invalidate();
			}
			lastMouseLocation = e.Location;
			if (!moveMode && selectedStartPointCad.HasValue && !selectedEndPointCad.HasValue) {
				/*int x = (int)selectedStartPoint.Value.X;
				int y = (int)selectedStartPoint.Value.Y;
				int width = x - e.Location.X;
				int height = y - e.Location.Y;
				if (width < 0) {
					width = -width;
					x = e.Location.X;
				}
				if (height < 0) {
					height = -height;
					y = e.Location.Y;
				}
				Invalidate(new Rectangle(x, y, width, height));*/
				Invalidate();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			mouseDown = false;
			if (!moveMode && e.Button == MouseButtons.Left) {
				this.unsavedChanges = true;
				Nullable<Point2D> closestPoint = null;
				Point2D referencePoint = new Point2D(e.X, e.Y);
				if (!shiftPressed) {
					IList<IList<DxfEntity>> closeEntities = EntitySelector.GetEntitiesCloseToPoint(
						model, GraphicsConfig.BlackBackgroundCorrectForBackColor,
						gdiGraphics3D.To2DTransform, referencePoint, 10.0);
					double closestDistance = double.PositiveInfinity;
					IList<DxfEntity> allCloseEntities = new List<DxfEntity>();
					foreach (IList<DxfEntity> entities in closeEntities) {
						foreach (DxfEntity entity in entities) {
							allCloseEntities.Add(entity);
						}
					}
					foreach (DxfEntity entity in allCloseEntities) {
						double distance;
						Nullable<Point2D> point = GetClosestPointOfEntity(referencePoint, entity, out distance);
						if (distance < closestDistance) {
							closestDistance = distance;
							closestPoint = point;
						}
						foreach (DxfEntity entity2 in allCloseEntities) {
							if (entity != entity2) {
								point = GetClosestIntersection(referencePoint, entity, entity2, out distance);
								if (distance < closestDistance) {
									closestDistance = distance;
									closestPoint = point;
								}
							}
						}
					}
				}
				if (selectedStartPointCad.HasValue && !selectedEndPointCad.HasValue) {
					Point3D tmp = new Point3D((closestPoint.HasValue ? closestPoint.Value : referencePoint), 0);
					Matrix4D inverse = gdiGraphics3D.To2DTransform.GetInverse();
					selectedEndPointCad = inverse.Transform(tmp);
					if (this.EndPointSelected != null) {
						double distX = selectedEndPointCad.Value.X - selectedStartPointCad.Value.X;
						double distY = selectedEndPointCad.Value.Y - selectedStartPointCad.Value.Y;
						double length = Math.Sqrt(distX * distX + distY * distY);
						this.EndPointSelected(this, new EndPointSelectedArgs(selectedStartPointCad.Value, selectedEndPointCad.Value, length));
					}
				} else {
					selectedEndPointCad = null;
					Point3D tmp = new Point3D((closestPoint.HasValue ? closestPoint.Value : referencePoint), 0);
					Matrix4D inverse = gdiGraphics3D.To2DTransform.GetInverse();
					selectedStartPointCad = inverse.Transform(tmp);
					if (this.StartPointSelected != null) {
						this.StartPointSelected(this, new StartPointSelectedArgs(selectedStartPointCad.Value)); ;
					}
				}
				Invalidate();
			}
		}

		private Nullable<Point2D> GetClosestPointOfEntity(Point2D referencePoint, DxfEntity entity, out double sqDistance) {
			if (entity is DxfLine) {
				DxfLine line = entity as DxfLine;
				Point2D start = gdiGraphics3D.To2DTransform.TransformTo2D(line.Start);
				Point2D end = gdiGraphics3D.To2DTransform.TransformTo2D(line.End);
				double sqDistStart = CalcSqDist(referencePoint, start);
				double sqDistEnd = CalcSqDist(referencePoint, end);
				if (sqDistStart < sqDistEnd) {
					sqDistance = sqDistStart;
					return start;
				} else {
					sqDistance = sqDistEnd;
					return end;
				}
			} else if (entity is DxfPolyline2D) {
				DxfPolyline2D poly = entity as DxfPolyline2D;
				double curSqDist;
				sqDistance = double.PositiveInfinity;
				Point2D vertexPoint;
				Nullable<Point2D> bestPoint = null;
				foreach (DxfVertex2D vertex in poly.Vertices) {
					vertexPoint = gdiGraphics3D.To2DTransform.TransformTo2D(vertex.Position);
					curSqDist = CalcSqDist(referencePoint, vertexPoint);
					if (curSqDist < sqDistance) {
						sqDistance = curSqDist;
						bestPoint = vertexPoint;
					}
				}
				return bestPoint;
			} else if (entity is DxfPolyline3D) {
				DxfPolyline3D poly = entity as DxfPolyline3D;
				double curSqDist;
				sqDistance = double.PositiveInfinity;
				Point2D vertexPoint;
				Nullable<Point2D> bestPoint = null;
				foreach (DxfVertex3D vertex in poly.Vertices) {
					vertexPoint = gdiGraphics3D.To2DTransform.TransformTo2D(vertex.Position);
					curSqDist = CalcSqDist(referencePoint, vertexPoint);
					if (curSqDist < sqDistance) {
						sqDistance = curSqDist;
						bestPoint = vertexPoint;
					}
				}
				return bestPoint;
			} else if (entity is DxfLwPolyline) {
				DxfLwPolyline poly = entity as DxfLwPolyline;
				double curSqDist;
				sqDistance = double.PositiveInfinity;
				Point2D vertexPoint;
				Nullable<Point2D> bestPoint = null;
				foreach (DxfLwPolyline.Vertex vertex in poly.Vertices) {
					vertexPoint = gdiGraphics3D.To2DTransform.TransformTo2D(vertex.Position);
					curSqDist = CalcSqDist(referencePoint, vertexPoint);
					if (curSqDist < sqDistance) {
						sqDistance = curSqDist;
						bestPoint = vertexPoint;
					}
				}
				return bestPoint;
			}
			// TODO add missing entity types
			sqDistance = double.PositiveInfinity;
			return null;
		}

		private Nullable<Point2D> GetClosestIntersection(Point2D referencePoint, DxfEntity entity1, DxfEntity entity2, out double sqDistance) {
			if (entity1 is DxfLine) {
				DxfLine line1 = entity1 as DxfLine;
				Point2D start1 = gdiGraphics3D.To2DTransform.TransformTo2D(line1.Start);
				Point2D end1 = gdiGraphics3D.To2DTransform.TransformTo2D(line1.End);
				return GetClosestIntersection(referencePoint, start1, end1, entity2, out sqDistance);
			}

			sqDistance = double.PositiveInfinity;
			return null;
		}

		private Nullable<Point2D> GetClosestIntersection(Point2D referencePoint, Point2D p0, Point2D p1, DxfEntity entity2, out double sqDistance) {
			if (entity2 is DxfLine) {
				DxfLine line2 = entity2 as DxfLine;
				Point2D start2 = gdiGraphics3D.To2DTransform.TransformTo2D(line2.Start);
				Point2D end2 = gdiGraphics3D.To2DTransform.TransformTo2D(line2.End);
				return GetIntersection(referencePoint, p0, p1, start2, end2, out sqDistance);
			}

			sqDistance = double.PositiveInfinity;
			return null;
		}

		/// <summary>
		/// Checks if the two lines P0P1 and Q0Q1 intersect and returns the intersection (or null if they don't)
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="q1"></param>
		/// <param name="q2"></param>
		/// <returns></returns>
		private Nullable<Point2D> GetIntersection(Point2D referencePoint, Point2D p0, Point2D p1, Point2D q0, Point2D q1, out double sqDistance) {
			Vector2D u = p1 - p0;
			Vector2D v = q1 - q0;
			Vector2D w = p0 - q1;

			if (u.X == 0 && u.Y == 0 && v.X == 0 && v.Y == 0) {
				// they are both points;
				if (u.X == v.X) {
					// they are the same point
					sqDistance = CalcSqDist(referencePoint, p0);
					return p0;
				} else {
					// they are different points
					sqDistance = double.PositiveInfinity;
					return null;
				}
			}
			if (u.X == 0 && u.Y == 0) {
				// P0P1 is a point but Q0Q1 not
				if (PointIsOnLine(p0, q0, q1)) {
					sqDistance = CalcSqDist(referencePoint, p0);
					return p0;
				} else {
					sqDistance = double.PositiveInfinity;
					return null;
				}
			}
			if (v.X == 0 && v.Y == 0) {
				// Q0Q1 is a point but P0P1 not
				if (PointIsOnLine(q0, p0, p1)) {
					sqDistance = CalcSqDist(referencePoint, q0);
					return q0;
				} else {
					sqDistance = double.PositiveInfinity;
					return null;
				}
			}

			double t0, t1;
			Vector2D w2 = p1 - q0;
			if (v.X != 0) {
				t0 = w.X / v.X;
				t1 = w2.X / v.X;
			} else {
				t0 = w.Y / v.Y;
				t1 = w2.Y / v.Y;
			}

			// t0 must be smaller than t1 so swap them if this is not the case
			if (t0 > t1) {
				double t = t0;
				t0 = t1;
				t1 = t;
			}

			if (t0 > 1 || t1 < 0) {
				// intersection lies outside of the line
				sqDistance = double.PositiveInfinity;
				return null;
			}
			t0 = t0 < 0 ? 0 : t0;	// clip to min 0
			t1 = t1 > 1 ? 1 : t1;	// clip to max 1
			if (t0 == t1) {
				// intersection is a point
				Point2D intersection = q0 + t0 * v;
				sqDistance = CalcSqDist(referencePoint, intersection);
				return intersection;
			}

			sqDistance = double.PositiveInfinity;
			return null;
		}

		/// <summary>
		/// Calculates the square distance for two points
		/// </summary>
		private double CalcSqDist(Point2D p1, Point2D p2) {
			double x = p1.X - p2.X;
			double y = p1.Y - p2.Y;
			return x * x + y * y;
		}

		/// <summary>
		/// Checks if the point P is on the line Q0Q1
		/// </summary>
		/// <param name="p"></param>
		/// <param name="q0"></param>
		/// <param name="q1"></param>
		/// <returns></returns>
		private bool PointIsOnLine(Point2D p, Point2D q0, Point2D q1) {
			return false;
		}

		public void RecreateDrawables() {
			gdiGraphics3D.CreateDrawables(model);
			this.Invalidate();
		}

		public void AddScale(double addedScale, Nullable<Point2D> center) {
			if (this.scale * addedScale < 0.01) {
				addedScale = 0.01 / this.scale;
			}
			if (this.scale * addedScale > 10000.0) {
				addedScale = 10000.0 / this.scale;
			}
			this.unsavedChanges = true;
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

			//Matrix4D scale = Transformation4D.GetScaleTransform(referencePoint1, referencePoint2, referencePoint3, transformedPoint1, transformedPoint2, transformedPoint3);
			this.scale = matrix.M00;
			this.translation = new Vector3D(matrix.M03, matrix.M13, 0);
			
			CalculateTo2DTransform();
			Invalidate();
		}

		protected override void OnMouseWheel(MouseEventArgs e) {
			this.unsavedChanges = true;
			base.OnMouseWheel(e);
			this.AddScale(1.0 + e.Delta / 1200.0, new Point2D(e.X, e.Y));
		}
	}
}
