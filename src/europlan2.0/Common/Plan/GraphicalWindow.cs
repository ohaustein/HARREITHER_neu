using System;
using System.Collections.Generic;
using System.Text;
using WW.Math.Geometry;
using WW.Math;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Europlan.Common {

	public class GraphicalWindow : GraphicalWallObstacle {

		private string id = Guid.NewGuid().ToString();
		private double borderDistance = 0.1;
		private double graphPosX = 0;
		private double graphPosY = 0;
		private double width = 0;
		private double height = 0;
		private ObstacleTypeEnum windowType = ObstacleTypeEnum.Window;

		private double bakGraphPosX = 0;
		private double bakGraphPosY = 0;
		private double bakWidth = 0;
		private double bakHeight = 0;

		public GraphicalWindow() {

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

		public override double GraphPosY {
			get { return this.graphPosY; }
			set { this.graphPosY = value; }
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
			get { return windowType; }
			set {
				if (value == ObstacleTypeEnum.Window || value == ObstacleTypeEnum.WindowTriangleLeft || value == ObstacleTypeEnum.WindowTriangleRight) {
					windowType = value;
				}
			}
		}

		public override bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset).IsInside(planPoint);
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale) {
			PaintObject(g, xOffset, yOffset, selectedObject, scale, false);
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool error) {
			Pen windowBorderPen = this == selectedObject ? new Pen(Color.FromArgb(128, 0, 0), (float)(3.0 / scale)) : new Pen(Color.Black, (float)(1.0 / scale));
			Brush windowBrush = new SolidBrush(SystemColors.ControlLight);
			Pen unusableBorderPen = this == selectedObject ? new Pen(Color.FromArgb(128, 64, 64), (float)(1.0 / scale)) : new Pen(Color.Gray, (float)(1.0 / scale));
			Brush unusableBrush = new HatchBrush(HatchStyle.BackwardDiagonal, this == selectedObject ? Color.FromArgb(128, 64, 64) : Color.Gray, Color.Transparent);

			if (error) {
				windowBorderPen.DashStyle = DashStyle.DashDotDot;
				unusableBorderPen.DashStyle = DashStyle.DashDotDot;
			}

			Region oldClip = g.Clip;
			Region baseClip = new Region(oldClip.GetRegionData());
			Polygon2D windowArea = this.GetObjectBorders(xOffset, yOffset);
			Polygon2D outsideBorder = GetOutsideBorder(xOffset, yOffset);
			g.SmoothingMode = SmoothingMode.AntiAlias;

			List<PointF> windowPoints = new List<PointF>();
			foreach (Point2D vertex in windowArea) {
				windowPoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
			}

			PointF[] windowPointArr = windowPoints.ToArray();

			GraphicsPath windowPath = new GraphicsPath();
			windowPath.AddPolygon(windowPointArr);

			g.FillPolygon(windowBrush, windowPointArr);

			bool outsideOk = true;
			List<PointF> outsidePoints = new List<PointF>();
			foreach (Point2D vertex in outsideBorder) {
				if (Double.NaN.Equals(vertex.X) || Double.NaN.Equals(vertex.Y)) {
					outsideOk = false;
				}
				outsidePoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
			}

			if (outsideOk) {
				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(outsidePoints.ToArray());
				Region clip = new Region(path);
				GraphicsPath excludePath = new GraphicsPath();
				excludePath.AddPolygon(windowPoints.ToArray());
				clip.Exclude(excludePath);
				clip.Intersect(baseClip);
				g.Clip = clip;

				g.FillPolygon(unusableBrush, outsidePoints.ToArray());
			}

			g.Clip = baseClip;
			if (outsideOk) {
				g.DrawPolygon(unusableBorderPen, outsidePoints.ToArray());
			}
			g.DrawPolygon(windowBorderPen, windowPointArr);
			g.Clip = oldClip;
		}

		public override IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public override WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset) {
			Polygon2D windowBorder = new Polygon2D();
			if (ObstacleType == ObstacleTypeEnum.Window) {
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY)); // left bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY + height)); // left top
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY + height)); // right top
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY)); // right bottom
			} else if (ObstacleType == ObstacleTypeEnum.WindowTriangleLeft) {
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY)); // left bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY + height)); // right top
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY)); // right bottom
			} else if (ObstacleType == ObstacleTypeEnum.WindowTriangleRight) {
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY)); // left bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY + height)); // left top
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY)); // right bottom
			}
			return windowBorder;
		}

		private Nullable<Point2D> startDrag = null;
		private double startX, startY, startWidth, startHeight;

		public override bool StartDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall) {
			this.startDrag = planPoint;
			this.startX = this.GraphPosX;
			this.startY = this.GraphPosY;
			this.startHeight = this.Height;
			this.startWidth = this.Width;
			return false;
		}

		public override bool MoveDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall) {
			if (anchor == null) {
				// move
				double tmpX = this.GraphPosX;
				double tmpY = this.GraphPosY;
				
				//this.GraphPosX = startX + planPoint.X - startDrag.Value.X;
				this.GraphPosY = startY + planPoint.Y - startDrag.Value.Y;
				bool retryY = false;
				if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
					this.GraphPosY = tmpY;
					retryY = true;
				}
				
				this.GraphPosX = startX + planPoint.X - startDrag.Value.X;
				if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
					this.GraphPosX = tmpX;
					retryY = false;
				}

				if (retryY) {
					this.GraphPosY = startY + planPoint.Y - startDrag.Value.Y;
					if (!this.PositionAndSizeOk(owningWall, 0, 0)) {
						this.GraphPosY = tmpY;
					}
				}

			} else {
				if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT) {
					double tmpWidth = this.Width;
					double tmpX = this.GraphPosX;
					this.Width = this.startWidth - planPoint.X + startDrag.Value.X;
					this.GraphPosX = this.startX + this.startWidth - this.Width;
					if ((!this.PositionAndSizeOk(owningWall, 0, 0)) || this.Width < 0) {
						this.Width = tmpWidth;
						this.GraphPosX = tmpX;
					}
				} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT) {
					double tmpWidth = this.Width;
					this.Width = this.startWidth + planPoint.X - startDrag.Value.X;
					if ((!this.PositionAndSizeOk(owningWall, 0, 0)) || this.Width < 0) {
						this.Width = tmpWidth;
					}
				}

				if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_TOP) == AnchorTypeEnum.ANCHOR_SCALE_TOP) {
					double tmpHeight = this.Height;
					this.Height = this.startHeight + planPoint.Y - this.startDrag.Value.Y;
					if ((!this.PositionAndSizeOk(owningWall, 0, 0)) || this.Height < 0) {
						this.Height = tmpHeight;
					}
				} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) == AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) {
					double tmpHeight = this.Height;
					double tmpY = this.GraphPosY;
					this.Height = this.startHeight + this.startDrag.Value.Y - planPoint.Y;
					this.GraphPosY = this.startY + this.startHeight - this.Height;
					if ((!this.PositionAndSizeOk(owningWall, 0, 0)) || this.Height < 0) {
						this.Height = tmpHeight;
						this.GraphPosY = tmpY;
					}
				}
			}
			return true;
		}

		public override bool EndDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall) {
			this.startDrag = null;
			return false;
		}

		public override List<Anchor> GetAnchors(double scale) {
			List<Anchor> anchors = new List<Anchor>();
			double px = 4.0 / scale;
			anchors.Add(new Anchor(this.GraphPosX - px, this.GraphPosY - px, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_LEFT, this));
			anchors.Add(new Anchor(this.GraphPosX - px, this.GraphPosY + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_LEFT, this));
			anchors.Add(new Anchor(this.GraphPosX - px, this.GraphPosY + this.Height + px, AnchorTypeEnum.ANCHOR_SCALE_TOP_LEFT, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width / 2.0, this.GraphPosY + this.Height + px, AnchorTypeEnum.ANCHOR_SCALE_TOP, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width + px, this.GraphPosY + this.Height + px, AnchorTypeEnum.ANCHOR_SCALE_TOP_RIGHT, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width + px, this.GraphPosY + this.Height / 2.0, AnchorTypeEnum.ANCHOR_SCALE_RIGHT, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width + px, this.GraphPosY - px, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_RIGHT, this));
			anchors.Add(new Anchor(this.GraphPosX + this.Width / 2.0, this.GraphPosY - px, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM, this));
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

		public override Polygon2D GetOutsideBorder(double xOffset, double yOffset) {
			return this.GetOutsideBorder(this.GetObjectBorders(xOffset, yOffset));
		}
	}

}
