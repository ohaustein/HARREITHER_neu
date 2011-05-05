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
		private GraphicalWall dachSchraege = null;
		private double borderDistance = 0.1;

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

		public double BorderDistance {
			get { return borderDistance; }
			set { borderDistance = value; }
		}

		#region IGraphicalWallObject Members
		public bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.GetWallPolygon(xOffset, yOffset).IsInside(planPoint);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject) {
			Pen wallBorderPen = (this == selectedObject) ? new Pen(Color.FromArgb(128, 0, 0), 3) : Pens.Black;
			Brush wallBrush = new SolidBrush(Color.White);
			Pen unusableBorderPen = (this == selectedObject) ? new Pen(Color.FromArgb(128, 64, 64)) : Pens.Gray;
			Brush unusableBrush = new HatchBrush(HatchStyle.BackwardDiagonal, (this == selectedObject) ? Color.FromArgb(128, 64, 64) : Color.Gray, Color.White);

			Region oldClip = g.Clip;
			Polygon2D wallBorder = this.GetWallPolygon(xOffset, yOffset);
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
		}

		public IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset) {
			IGraphicalWallObject pickedObject = null;
			if (this.dachSchraege != null) {
				pickedObject = this.dachSchraege.GetPickedObject(planPoint, xOffset, yOffset + this.GetWallHeight());
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
			// TODO check for register here
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

		public Polygon2D GetWallPolygon(double xOffset, double yOffset) {
			Polygon2D wallBorder = new Polygon2D();
			wallBorder.Add(new Point2D(xOffset, yOffset));
			if (this.CeilingContour[0].X != 0) {
				wallBorder.Add(new Point2D(xOffset, yOffset + this.CeilingContour[0].Y * 100.0));
			}
			foreach (Point2D vertex in this.CeilingContour) {
				wallBorder.Add(new Point2D(xOffset + vertex.X * 100.0, yOffset + vertex.Y * 100.0));
			}
			wallBorder.Add(new Point2D(xOffset + this.CeilingContour[this.CeilingContour.Count - 1].X * 100.0, yOffset));
			return wallBorder;
		}
	}
}
