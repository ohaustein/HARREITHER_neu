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
				double bestSqDistance = double.PositiveInfinity;
				Nullable<Point2D> bestPoint = null;
				Point2D referencePoint = new Point2D(e.X, e.Y);
				double grabDist = 50.0;
				if (!shiftPressed) {
					IList<IList<DxfEntity>> closeEntityChains = EntitySelector.GetEntitiesCloseToPoint(
						model, GraphicsConfig.BlackBackgroundCorrectForBackColor,
						gdiGraphics3D.To2DTransform, referencePoint, 2 * grabDist);

					List<Polygon2D> closePolygons = new List<Polygon2D>();
					foreach (List<DxfEntity> entityChain in closeEntityChains) {
						closePolygons.AddRange(GetEntityAsPolygons(entityChain));
					}

					double curSqDistance = double.PositiveInfinity;
					Nullable<Point2D> curPoint = null;

					foreach (Polygon2D polygon1 in closePolygons) {
						Nullable<Point2D> oldVertex1 = polygon1.isClosed ? polygon1.vertices[polygon1.vertices.Count - 1] : (Nullable<Point2D>)null;
						foreach (Point2D vertex1 in polygon1.vertices) {
							curSqDistance = CalcSqDist(referencePoint, vertex1);
							if (curSqDistance < bestSqDistance) {
								bestSqDistance = curSqDistance;
								bestPoint = vertex1;
							}

							if (oldVertex1.HasValue) {
								foreach (Polygon2D polygon2 in closePolygons) {
									Nullable<Point2D> oldVertex2 = polygon2.isClosed ? polygon2.vertices[polygon2.vertices.Count - 1] : (Nullable<Point2D>)null;
									foreach (Point2D vertex2 in polygon2.vertices) {
										if (oldVertex2.HasValue) {
											curPoint = GetIntersection(referencePoint, oldVertex1.Value, vertex1, oldVertex2.Value, vertex2, out curSqDistance);
											if (curSqDistance < bestSqDistance) {
												bestSqDistance = curSqDistance;
												bestPoint = curPoint;
											}
										}
										oldVertex2 = vertex2;
									}
								}
							}
							oldVertex1 = vertex1;
						}
					}

					if (bestSqDistance > grabDist * grabDist) {
						// do not use points which distance is greater than 5 pixels (=> sqDistance > 25)
						bestSqDistance = double.PositiveInfinity;
						bestPoint = null;
					}

				}
				if (selectedStartPointCad.HasValue && !selectedEndPointCad.HasValue) {
					Point3D tmp = new Point3D((bestPoint.HasValue ? bestPoint.Value : referencePoint), 0);
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
					Point3D tmp = new Point3D((bestPoint.HasValue ? bestPoint.Value : referencePoint), 0);
					Matrix4D inverse = gdiGraphics3D.To2DTransform.GetInverse();
					selectedStartPointCad = inverse.Transform(tmp);
					if (this.StartPointSelected != null) {
						this.StartPointSelected(this, new StartPointSelectedArgs(selectedStartPointCad.Value)); ;
					}
				}
				Invalidate();
			}
		}

		private Point3D CorrectPoint(Point3D point, List<DxfEntity> entityChain, int pos) {
			if (pos >= entityChain.Count) {
				return point;
			} else {
				// todo correct point
				DxfEntity correctionEntity = entityChain[pos];
				if (correctionEntity is DxfInsert) {
					
					DxfInsert insert = correctionEntity as DxfInsert;

					Matrix4D matrix = Matrix4D.Identity;
					//insert.
					//point = point * ((Vector4D)insert.ScaleFactor);
					
					double cosPhi = Math.Cos(insert.Rotation);
					double sinPhi = Math.Sin(insert.Rotation);
					Vector3D u = insert.ZAxis;
					Matrix4D rotation =
						new Matrix4D(
							new Vector4D(
								1.0 + (1.0 - cosPhi) * (u.X * u.X - 1),
								(1.0 - cosPhi) * u.X * u.Y + u.Z * sinPhi,
								(1.0 - cosPhi) * u.X * u.Z - u.Y + sinPhi,
								0),
							new Vector4D(
								(1.0 - cosPhi) * u.X * u.Y - u.Z * sinPhi,
								1.0 + (1.0 - cosPhi) * (u.Y * u.Y - 1),
								(1.0 - cosPhi) * u.Y * u.Z + u.X * sinPhi,
								0),
							new Vector4D(
								(1.0 - cosPhi) * u.X * u.Z + u.Y * sinPhi,
								(1.0 - cosPhi) * u.Y * u.Z - u.X * sinPhi,
								1.0 + (1.0 - cosPhi) * (u.Z * u.Z - 1),
								0),
							new Vector4D(
								0,
								0,
								0,
								1));

					Matrix4D scale = new Matrix4D(
						insert.ScaleFactor.X, 0, 0, 0,
						0, insert.ScaleFactor.Y, 0, 0,
						0, 0, insert.ScaleFactor.Z, 0,
						0, 0, 0, 1);

					Matrix4D translation = new Matrix4D(
						1, 0, 0, insert.InsertionPoint.X,
						0, 1, 0, insert.InsertionPoint.Y,
						0, 0, 1, insert.InsertionPoint.Z,
						0, 0, 0, 1);

					Matrix4D totalMatrix = translation * rotation * scale;

					point = totalMatrix.Transform(point);
								
													

					/*point.X = point.X * insert.ScaleFactor.X + insert.InsertionPoint.X;
					point.Y = point.Y * insert.ScaleFactor.Y + insert.InsertionPoint.Y;
					point.Z = point.Z * insert.ScaleFactor.Z + insert.InsertionPoint.Z;*/
					// todo
				} else if (correctionEntity is DxfDimension) {
					DxfDimension dimension = correctionEntity as DxfDimension;

					point = point + (dimension.InsertionPoint - new Point3D(0, 0, 0));
					/*point.X = point.X + dimension.InsertionPoint.X;
					point.Y = point.Y + dimension.InsertionPoint.X;
					point.Z = point.Z + dimension.InsertionPoint.X;*/
					// todo
				}

				if (pos == entityChain.Count - 1) {
					return point;
				} else {
					return CorrectPoint(point, entityChain, pos + 1);
				}
			}
		}

		private struct Polygon3D {

			public Polygon3D(bool isClosed) {
				this.vertices = new List<Point3D>();
				this.isClosed = isClosed;
			}

			public Polygon3D(ICollection<Point3D> vertices, bool isClosed) {
				this.vertices = new List<Point3D>(vertices);
				this.isClosed = isClosed;
			}

			public readonly List<Point3D> vertices;
			public bool isClosed;
		}

		private struct Polygon2D {

			public Polygon2D(bool isClosed) {
				this.vertices = new List<Point2D>();
				this.isClosed = isClosed;
			}

			public Polygon2D(ICollection<Point2D> vertices, bool isClosed) {
				this.vertices = new List<Point2D>(vertices);
				this.isClosed = isClosed;
			}

			public readonly List<Point2D> vertices;
			public bool isClosed;
		}

		private List<Polygon2D> GetEntityAsPolygons(List<DxfEntity> entityChain) {
			DxfEntity entity = entityChain[0];
			List<Polygon3D> polygons3d = new List<Polygon3D>();

			if (entity is DxfLine) {
				Polygon3D poly = new Polygon3D(false);
				poly.vertices.Add((entity as DxfLine).Start);
				poly.vertices.Add((entity as DxfLine).End);
				polygons3d.Add(poly);
			} else if (entity is DxfPolyline2D) {
				Polygon3D poly = new Polygon3D((entity as DxfPolyline2D).Closed);
				foreach (DxfVertex2D vertex in (entity as DxfPolyline2D).Vertices) {
					poly.vertices.Add((Point3D)vertex.Position);
				}
				polygons3d.Add(poly);
			} else if (entity is DxfPolyline3D) {
				Polygon3D poly = new Polygon3D((entity as DxfPolyline3D).Closed);
				foreach (DxfVertex3D vertex in (entity as DxfPolyline3D).Vertices) {
					poly.vertices.Add(vertex.Position);
				}
				polygons3d.Add(poly);
			} else if (entity is DxfLwPolyline) {
				Polygon3D poly = new Polygon3D((entity as DxfLwPolyline).Closed);
				foreach (DxfLwPolyline.Vertex vertex in (entity as DxfLwPolyline).Vertices) {
					poly.vertices.Add((Point3D)vertex.Position);
				}
				polygons3d.Add(poly);
			}

			List<Polygon2D> polygons2d = new List<Polygon2D>();
			foreach (Polygon3D polygon3d in polygons3d) {
				Polygon2D polygon2d = new Polygon2D(polygon3d.isClosed);
				foreach (Point3D vertex3d in polygon3d.vertices) {
					polygon2d.vertices.Add(gdiGraphics3D.To2DTransform.TransformTo2D(CorrectPoint(vertex3d, entityChain, 1)));
				}
				polygons2d.Add(polygon2d);
			}
			return polygons2d;
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
			Vector2D w = p0 - q0;

			if (u.X == 0 && u.Y == 0 && v.X == 0 && v.Y == 0) {
				// they are both points;
				if (p0.X == q0.X) {
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

			double tmp = v.X * u.Y - u.X * v.Y;
			if (tmp == 0) {
				sqDistance = double.PositiveInfinity;
				return null;
			}

			double si = (w.X * v.Y - v.X * w.Y) / tmp;
			double ti = (w.X * u.Y - u.X * w.Y) / tmp;

			if (si < 0.0 || si > 1.0 || ti < 0.0 || ti > 1.0) {
				sqDistance = double.PositiveInfinity;
				return null;
			}

			Point2D i = p0 + si * u;
			sqDistance = CalcSqDist(referencePoint, i);
			return i;







			/*
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
			return null;*/
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
