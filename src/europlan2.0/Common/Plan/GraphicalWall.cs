using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Xml.Serialization;

namespace Europlan.Common {

	public class GraphicalWall {

		private GraphicalWall nextWall = null;
		private GraphicalWall prevWall = null;
		private Point2D planStartPoint = Point2D.Zero;
		private Point2D planEndPoint = Point2D.Zero;
		private List<Point2D> ceilingContour = new List<Point2D>();
		private string wallId = "";
		private List<GraphicalWallObstacle> obstacles = new List<GraphicalWallObstacle>();
		private GraphicalWall dachSchraege = null;

		public GraphicalWall() {

		}

		public GraphicalWall NextWall {
			get { return nextWall; }
			set { nextWall = value; }
		}

		public GraphicalWall PrevWall {
			get { return prevWall; }
			set { prevWall = value; }
		}

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

	}
}
