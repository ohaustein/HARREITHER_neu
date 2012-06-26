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
using System.Drawing.Drawing2D;
using Europlan.Common.Icons;

namespace Europlan.Common {
	public partial class CadPanel : UserControl, IPlanPanel {

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
		private Matrix4D from2DTransform;
		private Vector3D translation = Vector3D.Zero;
		private PointF lastMouseLocation;
		private Point2D lastPlanPoint;
		private double scale = 1.0;
		private bool mouseDown = false;
		private static double grabDist = 10.0; // radius des "fang"
		
		private Nullable<Point3D> selectedStartPointCad = null;
		private Nullable<Point3D> selectedEndPointCad = null;

		public event EventHandler<StartPointSelectedArgs> StartPointSelected;
		public event EventHandler<EndPointSelectedArgs> EndPointSelected;

		private bool unsavedChanges = false;

		private PlanMode mode = PlanMode.PM_MOVE;
		private bool shiftPressed = false;
		private bool inDesign = false;

		public CadPanel() {
			this.InitializeComponent();
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.Mode = PlanMode.PM_MOVE;
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		protected override void OnPaint(PaintEventArgs e) {
			if (gdiGraphics3D != null) {
                if (this.productPlanner == null || this.productPlanner.ShowPlanBackground) {
                    gdiGraphics3D.Draw(e.Graphics, this.ClientRectangle);
                }
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
			}
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			CalculateTo2DTransform();
			Invalidate();
		}

		public DxfModel Model {
			get { return this.model; }
		}

		private DxfModel InternalModel {
			get { return model; }
			set {
				if (value != model) {
					model = value;
					if (model != null && plan != null) {
						plan.InitializeModel(model);
						this.gdiGraphics3D = plan.GdiGraphics3D;
						this.CalculateTo2DTransform();
						this.Invalidate();
					}
				}
			}
		}

		private Matrix4D CalculateTo2DTransform() {
			Matrix4D to2DTransform = Matrix4D.Identity;
			if (model != null && plan.Bounds != null) {
				to2DTransform = to2DTransform * Transformation4D.Translation(translation);
				to2DTransform = to2DTransform * Transformation4D.Scaling(this.scale);

				to2DTransform = to2DTransform * plan.ToDefaultSize;

				gdiGraphics3D.To2DTransform = to2DTransform;
				from2DTransform = gdiGraphics3D.To2DTransform.GetInverse();
			}
			return to2DTransform;
		}

		public Point2D GetModelSpaceCoordinates(Point2D screenScapeCoordinates) {
			return from2DTransform.TransformTo2D(screenScapeCoordinates);
		}

		/// <summary>
		/// rotation is ignored for cad plans
		/// </summary>
		/// <param name="scale"></param>
		/// <param name="translationX"></param>
		/// <param name="translationY"></param>
		/// <param name="rotation"></param>
		public void SetPlanTransformations(double scale, double translationX, double translationY, double rotation) {
			if (this.scale != scale || this.translation.X != translationX || this.translation.Y != translationY) {
				this.unsavedChanges = true;
				this.scale = scale;
				this.translation.X = translationX;
				this.translation.Y = translationY;
				CalculateTo2DTransform();
				this.Invalidate();
			}
		}

		/// <summary>
		/// rotation is ignored for cad plans
		/// </summary>
		/// <param name="scale"></param>
		/// <param name="translationX"></param>
		/// <param name="translationY"></param>
		/// <param name="rotation"></param>
		public void GetPlanTransformations(out double scale, out double translationX, out double translationY, out double rotation) {
			scale = this.scale;
			translationX = this.translation.X;
			translationY = this.translation.Y;
			rotation = 0;
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			shiftPressed = e.Shift;
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			shiftPressed = false;
			base.OnKeyUp(e);
		}

		/// <summary>
		/// Tries to get a snap point that is close to the reference point.
		/// </summary>
		/// <param name="referencePoint">The location where a snap point shall be searched</param>
		/// <param name="snapPoint">
		/// The closest snap point that was found, if no snap point that is close enough was
		/// found, this will just refurn the reference point
		/// </param>
		/// <returns>
		/// the square distance of the snap point from the reference point, if no snap point
		/// that is close enough was found, this will return double.PositiveInfinity.
		/// </returns>
		private double SnapPoint(Point2D referencePoint, out Point2D snapPoint) {
			if (shiftPressed) {
				snapPoint = referencePoint;
				return double.PositiveInfinity;
			}

			double bestSqDistance = double.PositiveInfinity;
			Nullable<Point2D> bestPoint = null;

			IList<RenderedEntityInfo> closeRenderedEntityInfos = EntitySelector.GetEntitiesCloseToPoint(
				model, GraphicsConfig.BlackBackgroundCorrectForBackColor,
				gdiGraphics3D.To2DTransform, referencePoint, 2 * grabDist);
			//IList<IList<DxfEntity>> closeEntityChains = EntitySelector.GetEntitiesCloseToPoint(
			//	model, GraphicsConfig.BlackBackgroundCorrectForBackColor,
			//	gdiGraphics3D.To2DTransform, referencePoint, 2 * grabDist);

			List<Polygon2D> closePolygons = new List<Polygon2D>();
			foreach (RenderedEntityInfo entityInfo in closeRenderedEntityInfos) {
				closePolygons.AddRange(GetEntityAsPolygons(entityInfo));
			}
			//foreach (List<DxfEntity> entityChain in closeEntityChains) {
			//	closePolygons.AddRange(GetEntityAsPolygons(entityChain));
			//}

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

			snapPoint = bestPoint.HasValue ? bestPoint.Value : referencePoint;
			return bestSqDistance;
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			base.OnMouseClick(e);
			bool invalidate = false;
			if ((this.mode == PlanMode.PM_PLANNER_CLICK || this.mode == PlanMode.PM_PLANNER_DRAG || this.mode == PlanMode.PM_SET_DISTRIBUTOR) && this.productPlanner != null) {
				Point2D pickedPoint;
				this.SnapPoint(new Point2D(e.X, e.Y), out pickedPoint);
				Point3D planPoint = gdiGraphics3D.To2DTransform.GetInverse().Transform(new Point3D(pickedPoint, 0));
				invalidate = this.productPlanner.PlannerClick(new Point2D(planPoint.X, planPoint.Y), new Point((int)pickedPoint.X, (int)pickedPoint.Y), e.Button);
			}
			if (invalidate) {
				Invalidate();
			}
		}

		private Cursor tempCursor = Cursors.Default;
		private bool inMove = false;

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			bool invalidate = false;
			if (this.mode == PlanMode.PM_PLANNER_DRAG && e.Button != MouseButtons.Middle && this.productPlanner != null) {
				Point2D pickedPoint;
				this.SnapPoint(new Point2D(e.X, e.Y), out pickedPoint);
				Point3D planPoint = gdiGraphics3D.To2DTransform.GetInverse().Transform(new Point3D(pickedPoint, 0));
				invalidate = this.productPlanner.PlannerDragStart(new Point2D(planPoint.X, planPoint.Y), e.Location, e.Button);
				lastPlanPoint = new Point2D(planPoint.X, planPoint.Y);
			}
			lastMouseLocation = e.Location;
			if (e.Button == MouseButtons.Middle) {
				this.inMove = true;
				this.tempCursor = this.Cursor;
				this.Cursor = EuroplanCursors.MOVE_PLAN_ACTIVE;
			}
			if (this.mode == PlanMode.PM_MOVE) {
				this.Cursor = EuroplanCursors.MOVE_PLAN_ACTIVE;
			}
			mouseDown = true;
			if (invalidate) {
				Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			bool invalidate = false;
			if (mouseDown && this.mode == PlanMode.PM_PLANNER_DRAG && e.Button != MouseButtons.Middle && e.Button != MouseButtons.None && this.productPlanner != null) {
				/*Point2D pickedPoint;
				this.SnapPoint(new Point2D(e.X, e.Y), out pickedPoint);
				Point3D planPoint = gdiGraphics3D.To2DTransform.GetInverse().Transform(new Point3D(pickedPoint, 0));*/
				Point3D planPoint = gdiGraphics3D.To2DTransform.GetInverse().Transform(new Point3D(e.X, e.Y, 0));
				invalidate = this.productPlanner.PlannerDragMove(new Point2D(planPoint.X, planPoint.Y), e.Location, e.Button);
			}
			if ((this.mode == PlanMode.PM_PLANNER_CLICK || this.mode == PlanMode.PM_SET_DISTRIBUTOR || (this.mode == PlanMode.PM_PLANNER_DRAG && e.Button == MouseButtons.None)) && this.productPlanner != null) {
				/*Point2D pickedPoint;
				this.SnapPoint(new Point2D(e.X, e.Y), out pickedPoint);
				Point3D planPoint = gdiGraphics3D.To2DTransform.GetInverse().Transform(new Point3D(pickedPoint, 0));*/
				Point3D planPoint = gdiGraphics3D.To2DTransform.GetInverse().Transform(new Point3D(e.X, e.Y, 0));
				invalidate = this.productPlanner.PlannerMouseMove(new Point2D(planPoint.X, planPoint.Y), e.Location, e.Button);
			} 
			if (mouseDown && ((mode == PlanMode.PM_MOVE && e.Button == MouseButtons.Left) || e.Button == MouseButtons.Middle)) {
				this.unsavedChanges = true;
				translation += new Vector3D(e.X - lastMouseLocation.X, e.Y - lastMouseLocation.Y, 0);
				this.CalculateTo2DTransform();
				invalidate = true;
			}
			lastMouseLocation = e.Location;
			if (mode == PlanMode.PM_PICK_MEASURE && selectedStartPointCad.HasValue && !selectedEndPointCad.HasValue) {
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
				invalidate = true;
			}
			if (invalidate) {
				Invalidate();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			bool invalidate = false;
			if (this.mode == PlanMode.PM_PLANNER_DRAG && e.Button != MouseButtons.Middle && this.productPlanner != null) {
				Point2D pickedPoint;
				this.SnapPoint(new Point2D(e.X, e.Y), out pickedPoint);
				Point3D planPoint = gdiGraphics3D.To2DTransform.GetInverse().Transform(new Point3D(pickedPoint, 0));
				invalidate = this.productPlanner.PlannerDragEnd(new Point2D(planPoint.X, planPoint.Y), e.Location, e.Button);
			}
			mouseDown = false;
			if (mode == PlanMode.PM_PICK_MEASURE && e.Button == MouseButtons.Left) {
				this.unsavedChanges = true;

				Point2D pickedPoint;
				this.SnapPoint(new Point2D(e.X, e.Y), out pickedPoint);

				if (selectedStartPointCad.HasValue && !selectedEndPointCad.HasValue) {
					Matrix4D inverse = gdiGraphics3D.To2DTransform.GetInverse();
					selectedEndPointCad = inverse.Transform(new Point3D(pickedPoint, 0));
					if (this.EndPointSelected != null) {
						double distX = selectedEndPointCad.Value.X - selectedStartPointCad.Value.X;
						double distY = selectedEndPointCad.Value.Y - selectedStartPointCad.Value.Y;
						double length = Math.Sqrt(distX * distX + distY * distY);
						this.EndPointSelected(this, new EndPointSelectedArgs(selectedStartPointCad.Value, selectedEndPointCad.Value, length));
					}
				} else {
					selectedEndPointCad = null;
					Matrix4D inverse = gdiGraphics3D.To2DTransform.GetInverse();
					selectedStartPointCad = inverse.Transform(new Point3D(pickedPoint, 0));
					if (this.StartPointSelected != null) {
						this.StartPointSelected(this, new StartPointSelectedArgs(selectedStartPointCad.Value)); ;
					}
				}
				invalidate = true;
			}
			if (this.mode == PlanMode.PM_MOVE) {
				this.Cursor = EuroplanCursors.MOVE_PLAN;
			}
			if (e.Button == MouseButtons.Middle) {
				this.inMove = false;
				this.Cursor = this.tempCursor;
			}
			if (invalidate) {
				Invalidate();
			}
		}

		//private Point3D CorrectPoint(Point3D point, List<DxfEntity> entityChain, int pos) {
		private Point3D CorrectPoint(Point3D point, RenderedEntityInfo entityInfo) {
			return entityInfo.Transform.Transform(point);
			//if (pos >= entityChain.Count) {
			//    return point;
			//} else {
			//    // todo correct point
			//    DxfEntity correctionEntity = entityChain[pos];
			//    if (correctionEntity is DxfInsert) {
					
			//        DxfInsert insert = correctionEntity as DxfInsert;

			//        Matrix4D matrix = Matrix4D.Identity;
			//        //insert.
			//        //point = point * ((Vector4D)insert.ScaleFactor);
					
			//        double cosPhi = Math.Cos(insert.Rotation);
			//        double sinPhi = Math.Sin(insert.Rotation);
			//        Vector3D u = insert.ZAxis;
			//        Matrix4D rotation =
			//            new Matrix4D(
			//                new Vector4D(
			//                    1.0 + (1.0 - cosPhi) * (u.X * u.X - 1),
			//                    (1.0 - cosPhi) * u.X * u.Y + u.Z * sinPhi,
			//                    (1.0 - cosPhi) * u.X * u.Z - u.Y + sinPhi,
			//                    0),
			//                new Vector4D(
			//                    (1.0 - cosPhi) * u.X * u.Y - u.Z * sinPhi,
			//                    1.0 + (1.0 - cosPhi) * (u.Y * u.Y - 1),
			//                    (1.0 - cosPhi) * u.Y * u.Z + u.X * sinPhi,
			//                    0),
			//                new Vector4D(
			//                    (1.0 - cosPhi) * u.X * u.Z + u.Y * sinPhi,
			//                    (1.0 - cosPhi) * u.Y * u.Z - u.X * sinPhi,
			//                    1.0 + (1.0 - cosPhi) * (u.Z * u.Z - 1),
			//                    0),
			//                new Vector4D(
			//                    0,
			//                    0,
			//                    0,
			//                    1));

			//        Matrix4D scale = new Matrix4D(
			//            insert.ScaleFactor.X, 0, 0, 0,
			//            0, insert.ScaleFactor.Y, 0, 0,
			//            0, 0, insert.ScaleFactor.Z, 0,
			//            0, 0, 0, 1);

			//        Matrix4D translation = new Matrix4D(
			//            1, 0, 0, insert.InsertionPoint.X,
			//            0, 1, 0, insert.InsertionPoint.Y,
			//            0, 0, 1, insert.InsertionPoint.Z,
			//            0, 0, 0, 1);

			//        Matrix4D totalMatrix = translation * rotation * scale;

			//        point = totalMatrix.Transform(point);
								
													

			//        /*point.X = point.X * insert.ScaleFactor.X + insert.InsertionPoint.X;
			//        point.Y = point.Y * insert.ScaleFactor.Y + insert.InsertionPoint.Y;
			//        point.Z = point.Z * insert.ScaleFactor.Z + insert.InsertionPoint.Z;*/
			//        // todo
			//    } else if (correctionEntity is DxfDimension) {
			//        DxfDimension dimension = correctionEntity as DxfDimension;

			//        point = point + (dimension.InsertionPoint - new Point3D(0, 0, 0));
			//        /*point.X = point.X + dimension.InsertionPoint.X;
			//        point.Y = point.Y + dimension.InsertionPoint.X;
			//        point.Z = point.Z + dimension.InsertionPoint.X;*/
			//        // todo
			//    }

			//    if (pos == entityChain.Count - 1) {
			//        return point;
			//    } else {
			//        return CorrectPoint(point, entityChain, pos + 1);
			//    }
			//}
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

		//private List<Polygon2D> GetEntityAsPolygons(List<DxfEntity> entityChain) {
		private List<Polygon2D> GetEntityAsPolygons(RenderedEntityInfo entityInfo) {
			DxfEntity entity = entityInfo.Entity;
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
					//polygon2d.vertices.Add(gdiGraphics3D.To2DTransform.TransformTo2D(CorrectPoint(vertex3d, entityChain, 1)));
					polygon2d.vertices.Add(gdiGraphics3D.To2DTransform.TransformTo2D(CorrectPoint(vertex3d, entityInfo)));
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

		public Matrix TransformationMatrix {
			get {
				Matrix m = new Matrix();
				m.Translate((float)translation.X, (float)translation.Y);
				m.Scale((float)this.scale, (float)scale);

				return m;
			}
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
			get { return this.scale; }
			set {
				if (this.scale != value) {
					this.scale = value;
					CalculateTo2DTransform();
					this.Invalidate();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		[DefaultValue(PlanMode.PM_MOVE)]
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
		public override Cursor Cursor {
			get { return base.Cursor; }
			set { base.Cursor = value; }
		}

		private CadPlan plan = null;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Plan Plan {
			get { return this.plan; }
			set { 
				if (value is CadPlan) {
					this.plan = value as CadPlan;
					this.InternalModel = this.plan.LoadModel(false);
				} else if (value == null) {
					this.plan = null;
					this.InternalModel = null;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Matrix4D PlanTransformation {
			get { return this.gdiGraphics3D.To2DTransform; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ColorMode ColorMode {
			get { return ColorMode.CM_BLACK_BG; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModifierKey ModifierKey {
			get { return ModifierKey.MK_NONE; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SupportsSnap {
			get { return true; }
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
					if (this.inMove) {
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
			get { return Math.Abs(this.PlanTransformation.M00); }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Matrix4D PlanToControl {
			get { return this.gdiGraphics3D.To2DTransform; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Matrix4D ControlToPlan {
			get { return this.from2DTransform;  }
		}
		#endregion

		private void CadPanel_KeyDown(object sender, KeyEventArgs e) {
			if (this.ProductPlanner != null) {
				this.ProductPlanner.PlannerKeyPress(e.KeyCode);
			}
		}

	}
}
