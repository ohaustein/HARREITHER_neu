using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Xml.Serialization;
using WW.Math.Geometry;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Europlan.Common {

	public class GraphicalWall : IGraphicalWallObject {

		private string id = Guid.NewGuid().ToString();
		private string nextWallId = null;
		private string prevWallId = null;
		private Point2D planStartPoint = Point2D.Zero;
		private Point2D planEndPoint = Point2D.Zero;
		private List<Point2D> ceilingContour = new List<Point2D>();
		private string wallId = "";
		private List<GraphicalWallObstacle> obstacles = new List<GraphicalWallObstacle>();
		private List<GraphicalRegisterWrapper> registers = new List<GraphicalRegisterWrapper>();
		private GraphicalWall dachSchraege = null;
		private double borderDistance = 0.1;
		private bool isDachSchraege = false;

		public GraphicalWall() {

		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string PrevWallId {
			get { return prevWallId; }
			set { prevWallId = value; }
		}

		public string NextWallId {
			get { return nextWallId; }
			set { nextWallId = value; }
		}

		//public GraphicalWall NextWall {
		//    get { return nextWall; }
		//}

		//public GraphicalWall PrevWall {
		//    get { return prevWall; }
		//}

		public Point2D PlanStartPoint {
			get { return planStartPoint; }
			set { planStartPoint = value; }
		}

		public Point2D PlanEndPoint {
			get { return planEndPoint; }
			set { planEndPoint = value; }
		}

		public List<Point2D> CeilingContour {
			get { return ceilingContour; }
			set { ceilingContour = value; }
		}

		public List<GraphicalWallObstacle> Obstacles {
			get { return obstacles; }
			set { obstacles = value; }
		}

		public string WallId {
			get { return wallId; }
			set { wallId = value; }
		}

		[XmlIgnore]
		public HithermWall Wall {
			get {
				if (wallId != null && wallId != "") {
					foreach (HithermWall wall in Project.Instance.HithermWalls) {
						if (wall.Id == wallId) {
							return wall;
						}
					}
					foreach (HithermWall wall in Project.Instance.HithermCompactWalls) {
						if (wall.Id == wallId) {
							return wall;
						}
					}
				}
				return null;
			}
		}

		public GraphicalWall DachSchraege {
			get { return dachSchraege; }
			set { dachSchraege = value; }
		}

		public bool IsDachSchraege {
			get { return isDachSchraege; }
			set { isDachSchraege = value; }
		}

		public double BorderDistance {
			get { return borderDistance; }
			set { borderDistance = value; }
		}

		#region IGraphicalWallObject Members
		public bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset).IsInside(planPoint);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale) {
			Pen wallBorderPen = (this == selectedObject) ? new Pen(Color.FromArgb(128, 0, 0), (float)(3.0 / scale)) : new Pen(Color.Black, (float)(1.0 / scale));
			Brush wallBrush = new SolidBrush(Color.White);
			Pen unusableBorderPen = (this == selectedObject) ? new Pen(Color.FromArgb(128, 64, 64), (float)(1.0 / scale)) : new Pen(Color.Gray, (float)(1.0 / scale));
			Brush unusableBrush = new HatchBrush(HatchStyle.BackwardDiagonal, (this == selectedObject) ? Color.FromArgb(128, 64, 64) : Color.Gray, Color.White);

			Region oldClip = g.Clip;
			Polygon2D wallBorder = this.GetObjectBorders(xOffset, yOffset);
			g.SmoothingMode = SmoothingMode.AntiAlias;

			List<PointF> borderPoints = new List<PointF>();
			foreach (Point2D vertex in wallBorder) {
				borderPoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
			}

			PointF[] pointArr = borderPoints.ToArray();

			GraphicsPath wallPath = new GraphicsPath();
			wallPath.AddPolygon(pointArr);
			Region wallClip = new Region(wallPath);
			g.Clip = wallClip;


			g.FillPolygon(wallBrush, pointArr);

			Polygon2D usableArea = new Polygon2D(wallBorder);
			usableArea.Outset(-this.BorderDistance * 100.0);
			List<PointF> usablePoints = new List<PointF>();
			foreach (Point2D vertex in usableArea) {
				usablePoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
			}

			/*foreach (GraphicalWallObstacle obstacle in this.Obstacles) {
				obstacle
			}*/
			foreach (GraphicalRegisterWrapper register in this.Registers) {
				register.PaintObject(g, xOffset, yOffset, selectedObject, scale);
			}

			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(borderPoints.ToArray());
			Region clip = new Region(path);
			GraphicsPath excludePath = new GraphicsPath();
			excludePath.AddPolygon(usablePoints.ToArray());
			clip.Exclude(excludePath);
			g.Clip = clip;

			g.FillPolygon(unusableBrush, borderPoints.ToArray());

			g.Clip = wallClip;
			g.DrawPolygon(unusableBorderPen, usablePoints.ToArray());
			g.DrawPolygon(wallBorderPen, pointArr);
			g.Clip = oldClip;

			if (this.DachSchraege != null) {
				this.DachSchraege.PaintObject(g, xOffset, yOffset + this.GetWallHeight() * 100, selectedObject, scale);
			}
		}

		public IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset) {
			IGraphicalWallObject pickedObject = null;
			if (this.dachSchraege != null) {
				pickedObject = this.dachSchraege.GetPickedObject(planPoint, xOffset, yOffset + this.GetWallHeight() * 100);
				if (pickedObject != null) {
					return pickedObject;
				}
			}
			foreach (GraphicalWallObstacle obstacle in this.Obstacles) {
				//pickedObject = obstacle.GetPickedObject(planPoint, xOffset, yOffset);
				if (pickedObject != null) {
					return pickedObject;
				}
			}
			foreach (GraphicalRegisterWrapper register in this.Registers) {
				pickedObject = register.GetPickedObject(planPoint, xOffset, yOffset);
				if (pickedObject != null) {
					return pickedObject;
				}
			}
			// TODO check for register here
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public GraphicalWall GetPickedWall(Point2D planPoint, double xOffset, double yOffset) {
			GraphicalWall pickedWall = null;
			if (this.dachSchraege != null) {
				pickedWall = this.dachSchraege.GetPickedWall(planPoint, xOffset, yOffset + this.GetWallHeight() * 100);
				if (pickedWall != null) {
					return pickedWall;
				}
			}
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}
		#endregion

		public double GetWallHeight() {
			double height = 0;
			foreach (Point2D point in this.ceilingContour) {
				if (point.Y > height) {
					height = point.Y;
				}
			}
			return height;
		}

		public double GetWallWidth() {
			if (this.CeilingContour == null || this.CeilingContour.Count == 0) {
				return 0;
			}
			return this.CeilingContour[this.CeilingContour.Count - 1].X;
		}

		public double GetTotalWallHeight() {
			return this.DachSchraege != null ? this.GetWallHeight() + this.DachSchraege.GetWallHeight() : this.GetWallHeight();
		}

		/*public Polygon2D GetWallPolygon(double xOffset, double yOffset) {
		}*/

		public Polygon2D GetObjectBorders(double xOffset, double yOffset) {
			Polygon2D wallBorder = new Polygon2D();
			Point2D lastPoint = new Point2D(xOffset, yOffset);
			wallBorder.Add(lastPoint);
			if (this.CeilingContour[0].X != 0) {
				lastPoint = new Point2D(xOffset, yOffset + this.CeilingContour[0].Y * 100.0);
				wallBorder.Add(lastPoint);
			}
			Point2D curPoint;
			foreach (Point2D vertex in this.CeilingContour) {
				curPoint = new Point2D(xOffset + vertex.X * 100.0, yOffset + vertex.Y * 100.0);
				if (curPoint != lastPoint) {
					wallBorder.Add(curPoint);
					lastPoint = curPoint;
				}
			}
			curPoint = new Point2D(xOffset + this.CeilingContour[this.CeilingContour.Count - 1].X * 100.0, yOffset);
			if (curPoint != lastPoint) {
				wallBorder.Add(curPoint);
			}
			return wallBorder;
		}


		public Nullable<double> GetWallYOffset(GraphicalWall wall, double startOffset) {
			if (this == wall) {
				return startOffset;
			}
			if (this.DachSchraege != null) {
				return this.DachSchraege.GetWallYOffset(wall, startOffset + this.GetWallHeight());
			}
			return null;
		}

		public void SetWallWidth(double width) {
			double currentWidth = GetWallWidth();
			double delta = width - currentWidth;
			// TODO: breite der dachschräge prüfen
			if (Math.Round(delta, 2) != 0) {
				ceilingContour[2] = new Point2D(ceilingContour[2].X + delta, ceilingContour[2].Y);
				ceilingContour[3] = new Point2D(ceilingContour[3].X + delta, ceilingContour[3].Y);
			}
		}

		public void SetWallHeight(double height) {
			double currentHeight = GetWallHeight();
			double delta = height - currentHeight;
			if (Math.Round(delta, 2) > 0) {
				for (int i = 0; i < CeilingContour.Count; i++) {
					Point2D p = ceilingContour[i];
					double y = p.Y + delta > 0 ? p.Y + delta : 0;
					ceilingContour[i] = new Point2D(p.X, y);
				}
			}
		}

		[XmlIgnore]
		public List<GraphicalRegisterWrapper> Registers {
			get { return this.registers; }
			set { this.registers = value; }
		}

		public GraphicalWall GetWallForId(string wallId) {
			if (this.Id == wallId) {
				return this;
			}
			if (this.DachSchraege != null) {
				return this.DachSchraege.GetWallForId(wallId);
			}
			return null;
		}

		public GraphicalWall GetWallForWrapper(GraphicalHithermRegisterWrapper wrapper) {
			if (this.Registers.Contains(wrapper)) {
				return this;
			}
			if (this.DachSchraege != null) {
				return this.DachSchraege.GetWallForWrapper(wrapper);
			}
			return null;
		}
	}
}
