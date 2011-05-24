using System;
using System.Collections.Generic;
using System.Text;
using WW.Math.Geometry;
using WW.Math;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Europlan.Common {

	public class GraphicalOtherObstacle : GraphicalWallObstacle {

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
		
		public GraphicalOtherObstacle() {

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
			get { return ObstacleTypeEnum.Other; }
			set {
				// obstacle type cannot be changed
			}
		}

		public override bool HitTest(Point2D planPoint, double xOffset, double yOffset) {
			return this.GetObjectBorders(xOffset, yOffset).IsInside(planPoint);
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale) {
			PaintObject(g, xOffset, yOffset, selectedObject, scale, false);
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool error) {
			Pen windowBorderPen = this.GetObstacleBorderPen(scale, this == selectedObject, error);
			Brush windowBrush = this.GetUnusableBrushForOther(scale, this == selectedObject, error);
			Pen unusableBorderPen = this.GetUnusableBorderPen(scale, this == selectedObject, error);
			Brush unusableBrush = this.GetUnusableBrush(scale, this == selectedObject, error);

			Region oldClip = g.Clip;
			Region baseClip = new Region(oldClip.GetRegionData());
			Polygon2D doorArea = this.GetObjectBorders(xOffset, yOffset);
			Polygon2D outsideBorder = GetOutsideBorder(xOffset, yOffset);
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


			g.FillPolygon(windowBrush, doorPointArr);


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
			g.Clip = clip;

			g.FillPolygon(unusableBrush, outsidePoints.ToArray());

			g.Clip = baseClip;
			g.DrawPolygon(unusableBorderPen, outsidePoints.ToArray());
			g.DrawPolygon(windowBorderPen, doorPointArr);
			g.Clip = oldClip;

			/*Region oldClip = g.Clip;
			Polygon2D usableArea = this.GetObjectBorders(xOffset, yOffset);
			Polygon2D windowBorder = GetOutsideBorder(xOffset, yOffset);
			g.SmoothingMode = SmoothingMode.AntiAlias;

			List<PointF> borderPoints = new List<PointF>();
			foreach (Point2D vertex in windowBorder) {
				borderPoints.Add(new PointF((float)vertex.X, (float)vertex.Y));
			}

			PointF[] pointArr = borderPoints.ToArray();

			GraphicsPath windowPath = new GraphicsPath();
			windowPath.AddPolygon(pointArr);
			Region windowClip = new Region(windowPath);
			g.Clip = windowClip;


			g.FillPolygon(windowBrush, pointArr);

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

			g.Clip = windowClip;
			g.DrawPolygon(unusableBorderPen, usablePoints.ToArray());
			g.DrawPolygon(windowBorderPen, pointArr);
			g.Clip = oldClip;*/
		}

		public override IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			if (HitTest(planPoint, xOffset, yOffset)) {
				return this;
			}
			return null;
		}

		public override WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset) {
			Polygon2D windowBorder = new Polygon2D();
			windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY)); // left bottom
			windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY + height)); // left top
			windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY + height)); // right top
			windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY)); // right bottom
			return windowBorder;
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
				
				//this.GraphPosX = startX + planPoint.X - startDrag.Value.X;
				this.GraphPosY = startY + planPoint.Y - startDrag.Value.Y;
				bool retryY = false;
				if (useSnap) {
					this.SnapToHelplines(owningWall.AllHelpLines, true, true);
				}
				if (!this.CheckValidity(owningWall, 0, 0)) {
					this.GraphPosY = tmpY;
					retryY = true;
				}
				
				this.GraphPosX = startX + planPoint.X - startDrag.Value.X;
				if (!this.CheckValidity(owningWall, 0, 0)) {
					this.GraphPosX = tmpX;
					retryY = false;
				}

				if (retryY) {
					this.GraphPosY = startY + planPoint.Y - startDrag.Value.Y;
					if (useSnap) {
						this.SnapToHelplines(owningWall.AllHelpLines, true, true);
					}
					if (!this.CheckValidity(owningWall, 0, 0)) {
						this.GraphPosY = tmpY;
					}
				}

			} else {
				if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT) {
					double tmpWidth = this.Width;
					double tmpX = this.GraphPosX;
					this.Width = this.startWidth - planPoint.X + startDrag.Value.X;
					this.GraphPosX = this.startX + this.startWidth - this.Width;
					if ((!this.CheckValidity(owningWall, 0, 0)) || this.Width < 0) {
						this.Width = tmpWidth;
						this.GraphPosX = tmpX;
					}
				} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT) {
					double tmpWidth = this.Width;
					this.Width = this.startWidth + planPoint.X - startDrag.Value.X;
					if ((!this.CheckValidity(owningWall, 0, 0)) || this.Width < 0) {
						this.Width = tmpWidth;
					}
				}

				if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_TOP) == AnchorTypeEnum.ANCHOR_SCALE_TOP) {
					double tmpHeight = this.Height;
					this.Height = this.startHeight + planPoint.Y - this.startDrag.Value.Y;
					if (useSnap) {
						this.SnapToHelplines(owningWall.AllHelpLines, true, false);
					}
					if ((!this.CheckValidity(owningWall, 0, 0)) || this.Height < 0) {
						this.Height = tmpHeight;
					}
				} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) == AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) {
					double tmpHeight = this.Height;
					double tmpY = this.GraphPosY;
					this.Height = this.startHeight + this.startDrag.Value.Y - planPoint.Y;
					this.GraphPosY = this.startY + this.startHeight - this.Height;
					if (useSnap) {
						this.SnapToHelplines(owningWall.AllHelpLines, false, true);
					}
					if ((!this.CheckValidity(owningWall, 0, 0)) || this.Height < 0) {
						this.Height = tmpHeight;
						this.GraphPosY = tmpY;
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

		public override bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom) {
			if (helplines == null) {
				return false;
			}
			double top = this.GraphPosY + this.Height;
			double bottom = this.GraphPosY;

			double deltaTop = double.MaxValue;
			double deltaBottom = double.MaxValue;

			double newTop = top;
			double newBottom = bottom;

			double newDeltaTop, newDeltaBottom;
			bool snappedTop = false;
			bool snappedBottom = false;
			foreach (double helpline in helplines) {
				newDeltaTop = Math.Abs(helpline - top);
				newDeltaBottom = Math.Abs(helpline - bottom);
				if (newDeltaTop < newDeltaBottom) {
					if (snapTop && newDeltaTop <= GraphicalWall.HELPLINE_SNAP_DISTANCE && newDeltaTop < deltaTop) {
						deltaTop = newDeltaTop;
						newTop = helpline;
						snappedTop = true;
					}
				} else {
					if (snapBottom && newDeltaBottom <= GraphicalWall.HELPLINE_SNAP_DISTANCE && newDeltaBottom < deltaBottom) {
						deltaBottom = newDeltaBottom;
						newBottom = helpline;
						snappedBottom = true;
					}
				}
			}
			if (snapTop && snapBottom) {
				if (deltaTop <= deltaBottom) {
					this.GraphPosY = newTop - this.Height;
				} else {
					this.GraphPosY = newBottom;
				}
			} else if (snapTop) {
				this.Height = newTop - this.GraphPosY;
			} else if (snapBottom) {
				this.GraphPosY = newBottom;
				this.Height = newTop - newBottom;
			}
			return (snappedTop && snapTop) || (snappedBottom && snapBottom);
		}
	}

}
