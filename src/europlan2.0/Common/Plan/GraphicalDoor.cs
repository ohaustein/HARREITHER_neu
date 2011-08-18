using System;
using System.Collections.Generic;
using System.Text;
using WW.Math.Geometry;
using WW.Math;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Europlan.Common {

	public class GraphicalDoor : GraphicalWallObstacle {

		private string id = Guid.NewGuid().ToString();
		private double borderDistance = 0.1;
		private double graphPosX = 0;
		private double graphPosY = 0;
		private double width = 0;
		private double height = 0;

		private double bakGraphPosX = 0;
		private double bakGraphPosY = 0;
		private double bakWidth = 0;
		private double bakHeight = 0;

		public GraphicalDoor() {

		}

		public override double BorderDistance {
			get { return borderDistance; }
			set { borderDistance = value; }
		}

		public override bool IsMoveable {
			get { return true; }
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public override double GraphPosX {
			get { return this.graphPosX; }
			set { this.graphPosX = value; }
		}

		[XmlIgnore]
		public override double GraphPosY {
			get { return this.graphPosY; }
			set { }
		}

		public override double Width {
			get { return this.width; }
			set { this.width = value; }
		}

		public override double Height {
			get { return this.height; }
			set { this.height = value; }
		}

		public override ObstacleTypeEnum ObstacleType {
			get { return ObstacleTypeEnum.Door; }
			set {
				// obstacle type of door cannot be changed
			}
		}

		public override bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset)[0].IsInside(planPoint);
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
			PaintObject(g, xOffset, yOffset, selectedObject, scale, false, export);
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool error, bool export) {
			Pen doorBorderPen = this.GetObstacleBorderPen(scale, this == selectedObject, error);
			Brush doorBrush = this.GetObstacleBrush(scale, this == selectedObject, error);
			Pen unusableBorderPen = this.GetUnusableBorderPen(scale, this == selectedObject, error);
			Brush unusableBrush = this.GetUnusableBrush(scale, this == selectedObject, error);

			Region oldClip = g.Clip;
			Region baseClip = new Region(oldClip.GetRegionData());
			Polygon2D doorArea = this.GetObjectBorders(xOffset, yOffset)[0];
			Polygon2D outsideBorder = GetOutsideBorder(xOffset, yOffset)[0];
			g.SmoothingMode = SmoothingMode.AntiAlias;

			List<PointF> doorPoints = new List<PointF>();
			foreach (Point2D vertex in doorArea) {
				doorPoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
			}

			PointF[] doorPointArr = doorPoints.ToArray();

			GraphicsPath doorPath = new GraphicsPath();
			doorPath.AddPolygon(doorPointArr);
			//Region doorClip = new Region(doorPath);
			//doorClip.Intersect(baseClip);
			//g.Clip = doorClip;


			g.FillPolygon(doorBrush, doorPointArr);

			
			List<PointF> outsidePoints = new List<PointF>();
			foreach (Point2D vertex in outsideBorder) {
				outsidePoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
			}

			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(outsidePoints.ToArray());
			Region clip = new Region(path);
			GraphicsPath excludePath = new GraphicsPath();
			excludePath.AddPolygon(doorPoints.ToArray());
			clip.Exclude(excludePath);
			clip.Intersect(baseClip);
			if (!export) {
				g.Clip = clip;
				g.FillPolygon(unusableBrush, outsidePoints.ToArray());
				g.Clip = baseClip;
				g.DrawPolygon(unusableBorderPen, outsidePoints.ToArray());
			}
			g.DrawPolygon(doorBorderPen, doorPointArr);
			g.Clip = oldClip;
		}

		public override IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public override List<WW.Math.Geometry.Polygon2D> GetObjectBorders(double xOffset, double yOffset) {
			Polygon2D doorBorder = new Polygon2D();
			doorBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY)); // left bottom
			doorBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY)); // right bottom
			doorBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY + height)); // right top
			doorBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY + height)); // left top
			return new List<Polygon2D>(new Polygon2D[] { doorBorder });
		}

		private Nullable<Point2D> startDrag = null;
		private double startX, startY, startWidth, startHeight;

		public override bool StartDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			this.startDrag = planPoint;
			this.startX = this.GraphPosX;
			this.startY = this.GraphPosY;
			this.startHeight = this.Height;
			this.startWidth = this.Width;
			return false;
		}

		public override bool MoveDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			if (anchor == null) {
				// move
				double tmpX = this.GraphPosX;
				double tmpY = this.GraphPosY;
				this.GraphPosX = startX + planPoint.X - startDrag.Value.X;
				if (!this.CheckValidity(owningWall, 0, 0)) {
					this.GraphPosX = tmpX;
				}
			} else {
				if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT) {
					double tmpWidth = this.Width;
					double tmpX = this.GraphPosX;
					this.Width = this.startWidth - planPoint.X + startDrag.Value.X;
					this.GraphPosX = this.startX + this.startWidth - this.Width;
					if (!this.CheckValidity(owningWall, 0, 0)) {
						this.Width = tmpWidth;
						this.GraphPosX = tmpX;
					}
				} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT) {
					double tmpWidth = this.Width;
					this.Width = this.startWidth + planPoint.X - startDrag.Value.X;
					if (!this.CheckValidity(owningWall, 0, 0)) {
						this.Width = tmpWidth;
					}
				}

				if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_TOP) == AnchorTypeEnum.ANCHOR_SCALE_TOP) {
					double tmpHeight = this.Height;
					this.Height = this.startHeight + planPoint.Y - this.startDrag.Value.Y;
					if (useSnap) {
						this.SnapToHelplines(owningWall.AllHelpLines, true, false);
					}
					if (!this.CheckValidity(owningWall, 0, 0)) {
						this.Height = tmpHeight;
					}
				}
			}
			owningRoom.MarkErrors(this, owningWall);
			return true;
		}

		public override bool EndDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			this.startDrag = null;
			return false;
		}

		public override List<Anchor> GetAnchors(double scale) {
			List<Anchor> anchors = new List<Anchor>();
			double px = 4.0 / scale;
			anchors.Add(new Anchor(this.GraphPosX - px, this.GraphPosY - px, AnchorTypeEnum.ANCHOR_SCALE_LEFT, this));
			anchors.Add(new Anchor(this.GraphPosX - px, this.GraphPosY + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_LEFT, this));
			anchors.Add(new Anchor(this.GraphPosX - px, this.GraphPosY + this.Height + px, AnchorTypeEnum.ANCHOR_SCALE_TOP_LEFT, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width / 2.0, this.GraphPosY + this.Height + px, AnchorTypeEnum.ANCHOR_SCALE_TOP, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width + px, this.GraphPosY + this.Height + px, AnchorTypeEnum.ANCHOR_SCALE_TOP_RIGHT, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width + px, this.GraphPosY + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_RIGHT, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width + px, this.GraphPosY - px, AnchorTypeEnum.ANCHOR_SCALE_RIGHT, this));
			//anchors.Add(new Anchor(this.X + this.Width / 2.0, this.Y - px5, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM, this));
			return anchors;
		}

		public override void BackupState() {
			bakGraphPosX = GraphPosX;
			bakGraphPosY = GraphPosY;
			bakWidth = Width;
			bakHeight = Height;
		}

		public override void RevertState() {
			GraphPosX = bakGraphPosX;
			GraphPosY = bakGraphPosY;
			Width = bakWidth;
			Height = bakHeight;
		}

		public override List<Polygon2D> GetOutsideBorder(double xOffset, double yOffset) {
			Polygon2D doorBorder = new Polygon2D();
			double dist = this.BorderDistance * 100;
			doorBorder.Add(new Point2D(xOffset + graphPosX - dist, yOffset + graphPosY)); // left bottom
			doorBorder.Add(new Point2D(xOffset + graphPosX + width + dist, yOffset + graphPosY)); // right bottom
			doorBorder.Add(new Point2D(xOffset + graphPosX + width + dist, yOffset + graphPosY + height + dist)); // right top
			doorBorder.Add(new Point2D(xOffset + graphPosX - dist, yOffset + graphPosY + height + dist)); // left top
			return new List<Polygon2D>(new Polygon2D[] { doorBorder });
		}

		public override bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom) {
			if (helplines == null) {
				return false;
			}
			double top = this.Height;

			double deltaTop = double.MaxValue;

			double newTop = top;

			double newDeltaTop;
			bool snappedTop = false;
			foreach (double helpline in helplines) {
				newDeltaTop = Math.Abs(helpline - top);
				if (snapTop && newDeltaTop <= GraphicalWall.HELPLINE_SNAP_DISTANCE && newDeltaTop < deltaTop) {
					deltaTop = newDeltaTop;
					newTop = helpline;
					snappedTop = true;
				}
			}
			if (snapTop) {
				this.Height = newTop - this.GraphPosY;
			}
			return snappedTop && snapTop;
		}
	}

}
