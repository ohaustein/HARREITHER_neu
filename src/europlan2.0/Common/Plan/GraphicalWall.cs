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


		public static double HELPLINE_SNAP_DISTANCE = 5.0;

		private string id = Guid.NewGuid().ToString();
		private string nextWallId = null;
		private string prevWallId = null;
		private Nullable<Point2D> planStartPoint = null;
		private Nullable<Point2D> planEndPoint = null;
		private List<Point2D> ceilingContour = new List<Point2D>();
		private string wallId = "";
		private List<GraphicalWallObstacle> obstacles = new List<GraphicalWallObstacle>();
		private List<GraphicalRegisterWrapper> registers = new List<GraphicalRegisterWrapper>();
		private GraphicalWall dachSchraege = null;
		private double borderDistance = 0.1;
		private bool isDachSchraege = false;
		private bool enabled = true;
		private List<double> helpLines = new List<double>();
		private bool showGlobalHelpLines = true;
		private Room assiociatedRoom = null;
		private List<GraphicalWallSchraege> schraegen = null;

		public GraphicalWall() {
			//GraphicalDoor door = new GraphicalDoor();
			//door.GraphPosX = 20;
			//door.Width = 100;
			//door.Height = 210;
			//this.obstacles.Add(door);
			//GraphicalWindow window = new GraphicalWindow();
			//window.GraphPosX = 20;
			//window.GraphPosY = 120;
			//window.Width = 100;
			//window.Height = 120;
			//this.obstacles.Add(window);
			//helpLines.Add(20);
			//helpLines.Add(200);

		}

		public bool IsMoveable {
			get { return false; }
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

		public Nullable<Point2D> PlanStartPoint {
			get { return planStartPoint; }
			set { planStartPoint = value; }
		}

		public Nullable<Point2D> PlanEndPoint {
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

		[XmlIgnore]
		public List<GraphicalWallSchraege> Schraegen {
			get {
				//if (this.schraegen == null & this.ceilingContour != null && this.ceilingContour.Count > 3) {
					this.schraegen = new List<GraphicalWallSchraege>();
					GraphicalWallSchraege newSchraege;
					if (this.ceilingContour[0].X != this.ceilingContour[1].X) {
						newSchraege = new GraphicalWallSchraege(this, GraphicalWallSchraege.OrientationEnum.LEFT);
						newSchraege.Width = (this.ceilingContour[1].X - this.ceilingContour[0].X) * 100.0;
						newSchraege.Height = (this.ceilingContour[1].Y - this.ceilingContour[0].Y) * 100.0;
						this.schraegen.Add(newSchraege);
					}
					if (this.ceilingContour[this.ceilingContour.Count  - 1].X != this.ceilingContour[this.ceilingContour.Count - 2].X) {
						newSchraege = new GraphicalWallSchraege(this, GraphicalWallSchraege.OrientationEnum.RIGHT);
						newSchraege.Width = (this.ceilingContour[this.ceilingContour.Count - 1].X - this.ceilingContour[this.ceilingContour.Count - 2].X) * 100.0;
						newSchraege.Height = (this.ceilingContour[this.ceilingContour.Count - 2].Y - this.ceilingContour[this.ceilingContour.Count - 1].Y) * 100.0;
						this.schraegen.Add(newSchraege);
					}
				//}
				//if (this.schraegen == null) {
					//return new List<GraphicalDachschraege>();
				//}
				return this.schraegen;
			}
		}

		public string WallId {
			get { return wallId; }
			set { wallId = value; }
		}

		public List<double> HelpLines {
			get { return this.helpLines; }
			set { this.helpLines = value; }
		}

		[XmlIgnore]
		public List<double> AllHelpLines {
			get {
				List<double> allHelplines = new List<double>();
				if (this.HelpLines != null) {
					allHelplines.AddRange(this.HelpLines);
				}
				if (this.ShowGlobalHelpLines && this.AssiociatedRoom != null && this.AssiociatedRoom.HelpLines != null) {
					allHelplines.AddRange(this.AssiociatedRoom.HelpLines);
				}
				return allHelplines;
			}
		}

		public bool ShowGlobalHelpLines {
			get { return !IsDachSchraege && showGlobalHelpLines; }
			set { showGlobalHelpLines = value; }
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
		
		[XmlIgnore]
		public bool Enabled {
			get { return enabled; }
			set { enabled = value; }
		}

		public double BorderDistance {
			get { return borderDistance; }
			set { borderDistance = value; }
		}

		#region IGraphicalWallObject Members
		public bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset).IsInside(planPoint);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
			this.PaintObject(g, xOffset, yOffset, selectedObject, this.GetOwningWall(selectedObject), scale, export);
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, GraphicalWall selectedWall, double scale, bool export) {
			Pen wallBorderPen = (this == selectedObject || this == selectedWall) ? new Pen(Color.FromArgb(128, 0, 0), (float)(3.0 / scale)) : new Pen(Color.Black, (float)(1.0 / scale));
			Brush wallBrush = new SolidBrush(Color.White);
			Pen unusableBorderPen = (this == selectedObject || this == selectedWall) ? new Pen(Color.FromArgb(128, 64, 64), (float)(1.0 / scale)) : new Pen(Color.Gray, (float)(1.0 / scale));
			Brush unusableBrush = new HatchBrush(HatchStyle.BackwardDiagonal, (this == selectedObject || this == selectedWall) ? Color.FromArgb(128, 64, 64) : Color.Gray, Color.White);

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
			//g.Clip = wallClip;

			g.FillPolygon(wallBrush, pointArr);

			Polygon2D usableArea = GetUsableBorder(wallBorder);
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

			if (!export) {
				g.Clip = clip;
				g.FillPolygon(unusableBrush, borderPoints.ToArray());
				g.Clip = wallClip;
				g.DrawPolygon(unusableBorderPen, usablePoints.ToArray());
			}
			g.DrawPolygon(wallBorderPen, pointArr);

			if (!export) {
				Pen helpLinesPen = new Pen(Color.Blue, (float)(1.0 / scale));
				helpLinesPen.DashStyle = DashStyle.Dash;
				if (ShowGlobalHelpLines && AssiociatedRoom != null) {
					foreach (double offset in AssiociatedRoom.HelpLines) {
						g.DrawLine(helpLinesPen, (float)xOffset, (float)(yOffset + offset), (float)(xOffset + GetWallWidth() * 100), (float)(yOffset + offset));
					}
				}
				foreach (double offset in HelpLines) {
					g.DrawLine(helpLinesPen, (float)xOffset, (float)(yOffset + offset), (float)(xOffset + GetWallWidth() * 100), (float)(yOffset + offset));
				}
			}

			//bool drawSelected = false;

			foreach (GraphicalWallObstacle obstacle in this.Obstacles) {
				if (obstacle != selectedObject) {
					g.Clip = wallClip;
					obstacle.PaintObject(g, xOffset, yOffset, selectedObject, scale, export);
				/*} else {
					drawSelected = true;*/
				}
			}
			foreach (GraphicalRegisterWrapper register in this.Registers) {
				if (register != selectedObject) {
					g.Clip = wallClip;
					register.PaintObject(g, xOffset, yOffset, selectedObject, scale, export);
				/*} else {
					drawSelected = true;*/
				}
			}

			//g.Clip = wallClip;
			/*if (drawSelected) {
				selectedObject.PaintObject(g, xOffset, yOffset, selectedObject, scale);
			}*/

			g.Clip = oldClip;

			if (this.DachSchraege != null) {
				this.DachSchraege.PaintObject(g, xOffset + this.GetDachschraegeXOffset(), yOffset + this.GetWallHeight() * 100, selectedObject, selectedWall, scale, export);
			}
		}

		private double GetDachschraegeXOffset() {
			return (this.ceilingContour[1].X - this.ceilingContour[0].X) * 100.0;
		}

		private Polygon2D GetUsableBorder(Polygon2D wallBorder) {
			Polygon2D usableArea = new Polygon2D(wallBorder);
			usableArea.Outset(-this.BorderDistance * 100.0);
			return usableArea;
		}

		public IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset) {
			IGraphicalWallObject pickedObject = null;
			if (this.dachSchraege != null) {
				pickedObject = this.dachSchraege.GetPickedObject(planPoint, xOffset + this.GetDachschraegeXOffset(), yOffset + this.GetWallHeight() * 100);
				if (pickedObject != null) {
					return pickedObject;
				}
			}

			pickedObject = this.Obstacles.FindLast(delegate(GraphicalWallObstacle obstacle) {
				return obstacle.HitTest(planPoint, xOffset, yOffset);
			});
			if (pickedObject != null) {
				return pickedObject.GetPickedObject(planPoint, xOffset, yOffset);
			}
			/*foreach (GraphicalWallObstacle obstacle in this.Obstacles) {
				pickedObject = obstacle.GetPickedObject(planPoint, xOffset, yOffset);
				if (pickedObject != null) {
					return pickedObject;
				}
			}*/

			foreach (GraphicalRegisterWrapper register in this.Registers) {
				pickedObject = register.GetPickedObject(planPoint, xOffset, yOffset);
				if (pickedObject != null) {
					return pickedObject;
				}
			}
			foreach (GraphicalWallSchraege schraege in this.Schraegen) {
				pickedObject = schraege.GetPickedObject(planPoint, xOffset, yOffset);
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
				pickedWall = this.dachSchraege.GetPickedWall(planPoint, xOffset + this.GetDachschraegeXOffset(), yOffset + this.GetWallHeight() * 100);
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

		/// <summary>
		/// Returns the height of the wall in meter
		/// </summary>
		/// <returns></returns>
		public double GetWallHeight() {
			double height = 0;
			foreach (Point2D point in this.ceilingContour) {
				if (point.Y > height) {
					height = point.Y;
				}
			}
			return height;
		}

		/// <summary>
		/// Returns the width of the wall in meter
		/// </summary>
		/// <returns></returns>
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

		public List<Anchor> GetAnchors(double scale) {
			return new List<Anchor>();
		}

		public bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			// nothing to do here as the wall doesn't have any anchors
			return false;
		}

		public bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			// nothing to do here as the wall doesn't have any anchors
			return false;
		}

		public bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			// nothing to do here as the wall doesn't have any anchors
			return false;
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

		public Nullable<double> GetDachschraegeXOffset(GraphicalWall dachschraege, double startOffset) {
			if (dachschraege == this) {
				return startOffset;
			}
			if (this.DachSchraege != null) {
				return this.DachSchraege.GetDachschraegeXOffset(dachschraege, startOffset + this.ceilingContour[1].X - this.ceilingContour[0].X);
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
			if (this.DachSchraege != null) {
				this.DachSchraege.SetWallWidth(ceilingContour[2].X - ceilingContour[1].X);
			}
		}

		public void SetWallHeight(double height) {
			double currentHeight = GetWallHeight();
			double delta = height - currentHeight;
			if (Math.Round(delta, 2) != 0) {
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

		public GraphicalWall GetWallForWrapper(GraphicalRegisterWrapper wrapper) {
			if (this.Registers.Contains(wrapper)) {
				return this;
			}
			if (this.DachSchraege != null) {
				return this.DachSchraege.GetWallForWrapper(wrapper);
			}
			return null;
		}

		public GraphicalWall GetWallForObstacle(GraphicalWallObstacle obstacle) {
			if (this.Obstacles.Contains(obstacle)) {
				return this;
			}
			if (this.DachSchraege != null) {
				return this.DachSchraege.GetWallForObstacle(obstacle);
			}
			return null;
		}

		public GraphicalWall GetWallForObject(IGraphicalWallObject obj) {
			if (obj is GraphicalRegisterWrapper) {
				return this.GetWallForWrapper(obj as GraphicalRegisterWrapper);
			} else if (obj is GraphicalWallObstacle) {
				return this.GetWallForObstacle(obj as GraphicalWallObstacle);
			}
			return null;
		}

		public bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset, bool ignoreBorders) {
			Polygon2D wall;
			if (ignoreBorders) {
				wall = GetObjectBorders(xOffset, yOffset);
			} else {
				wall = GetUsableBorder(this.GetObjectBorders(xOffset, yOffset));
			}
			bool outside = false;
			foreach (Point2D point in polygon) {
				if (!Polygon2D.IsInside(point, wall)) {
					IList<Segment2D> segments = new List<Segment2D>();
					Polygon2D.GetSegments(wall, segments);
					outside = true;
					foreach (Segment2D segment in segments) {
						if (segment.GetDistance(point) < 0.01) {
							outside = false;
						}
					}
					if (outside) {
						break;
					}
				}
			}
			return outside;
		}

		public GraphicalWall GetOwningWall(IGraphicalWallObject obj) {
			foreach (GraphicalRegisterWrapper register in this.registers) {
				if (register == obj) {
					return this;
				}
			}
			foreach (GraphicalWallObstacle obstacle in this.obstacles) {
				if (obstacle == obj) {
					return this;
				}
			}
			foreach (GraphicalWallSchraege schraege in this.Schraegen) {
				if (schraege.Equals(obj)) {
					return this;
				}
			}
			if (this.DachSchraege != null) {
				return this.DachSchraege.GetOwningWall(obj);
			}
			return null;
		}

		public void RemoveAllRegisters(HithermProduct hithermProduct) {
			if (this.DachSchraege != null) {
				this.DachSchraege.RemoveAllRegisters(hithermProduct);
			}
			foreach (GraphicalHithermRegisterWrapper wrapper in this.Registers) {
				hithermProduct.RemoveRegisterFromCircuit(wrapper.Register);
			}
		}

		public PossibleConnection GetPossibleConnection(Point2D mousePointInPlan, double offsetX, double offsetY) {
			if (offsetY > 0) {
				// no connections for dachschrägen
				return null;
			}
			double width = this.GetWallWidth() * 100;
			if (mousePointInPlan.Y <= 10 && mousePointInPlan.Y >= 0 && mousePointInPlan.X >= offsetX && mousePointInPlan.X <= offsetX + width) {
				Polygon2D area = new Polygon2D();
				double left;
				double right;
				if (width < 10) {
					left = offsetX;
					right = offsetX + width;
				} else {
					left = mousePointInPlan.X - 5;
					if (left < offsetX) {
						left = offsetX;
					}
					right = left + 10;
					if (right > offsetX + width) {
						right = offsetX + width;
						left = right - 10;
					}
				}
				area.Add(new Point2D(left, 0));
				area.Add(new Point2D(left, 10));
				area.Add(new Point2D(right, 10));
				area.Add(new Point2D(right, 0));
				return new PossibleConnection(new Point2D(mousePointInPlan.X, 0), area, true, true, 0, true, false, true, new Vector2D(0, 10));
			}
			return null;
		}

		[XmlIgnore]
		public Room AssiociatedRoom {
			get {
				if (assiociatedRoom == null) {
					foreach (Floor floor in Project.Instance.Floors) {
						foreach (Room room in floor.Rooms) {
							foreach (GraphicalWall baseWall in room.Walls) {
								GraphicalWall wall = baseWall;
								while (wall != null) {
									if (wall == this) {
										this.assiociatedRoom = room;
										return room;
									}
									wall = wall.DachSchraege;
								}
							}
						}
					}
				}
				return this.assiociatedRoom;
			}
		}

		[XmlIgnore]
		public bool Error {
			get { return false; }
			set { }
		}

		public void BackupState() {
			// TODO
			//throw new Exception("TODO");
		}

		public void RevertState() {
			// TODO
			//throw new Exception("TODO");
		}

		public bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY) {
			// TODO
			//throw new Exception("TODO");
			return true;
		}

		private bool isNew = false;
		[XmlIgnore]
		public bool IsNew {
			get { return this.isNew; }
			set { this.isNew = value; }
		}

		public bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom) {
			// nothing to do here
			return false;
		}

		public void AdjustWidth(double newWidth, bool adjustLeftSide) {
			double currentWidth = GetWallWidth();
			double delta = newWidth - currentWidth;
			// TODO: breite der dachschräge prüfen
			if (Math.Round(delta, 2) != 0) {
				ceilingContour[2] = new Point2D(ceilingContour[2].X + delta, ceilingContour[2].Y);
				ceilingContour[3] = new Point2D(ceilingContour[3].X + delta, ceilingContour[3].Y);
			}
			if (this.DachSchraege != null) {
				this.DachSchraege.SetWallWidth(ceilingContour[2].X - ceilingContour[1].X);
			}
			if (adjustLeftSide) {
				foreach (GraphicalWallObstacle obstacle in this.Obstacles) {
					obstacle.GraphPosX += (delta * 100.0);
				}
				foreach (GraphicalHithermRegisterWrapper wrapper in this.Registers) {
					wrapper.Register.GraphPosX += (delta * 100.0);
				}
			}
		}
	}
}
