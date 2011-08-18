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
			return this.GetObjectBorders(xOffset, yOffset)[0].IsInside(planPoint);
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
			PaintObject(g, xOffset, yOffset, selectedObject, scale, false, export);
		}

		public override void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool error, bool export) {
			Pen windowBorderPen = this.GetObstacleBorderPen(scale, this == selectedObject, error);
			Brush windowBrush = this.GetObstacleBrush(scale, this == selectedObject, error);
			Pen unusableBorderPen = this.GetUnusableBorderPen(scale, this == selectedObject, error);
			Brush unusableBrush = this.GetUnusableBrush(scale, this == selectedObject, error);

			Region oldClip = g.Clip;
			Region baseClip = new Region(oldClip.GetRegionData());
			Polygon2D windowArea = this.GetObjectBorders(xOffset, yOffset)[0];
			Polygon2D outsideBorder = GetOutsideBorder(xOffset, yOffset)[0];
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

			if (!export) {
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

		public override List<WW.Math.Geometry.Polygon2D> GetObjectBorders(double xOffset, double yOffset) {
			Polygon2D windowBorder = new Polygon2D();
			if (ObstacleType == ObstacleTypeEnum.Window) {
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY)); // left bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY)); // right bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY + height)); // right top
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY + height)); // left top
			} else if (ObstacleType == ObstacleTypeEnum.WindowTriangleLeft) {
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY)); // left bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY)); // right bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY + height)); // right top
			} else if (ObstacleType == ObstacleTypeEnum.WindowTriangleRight) {
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY)); // left bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX + width, yOffset + graphPosY)); // right bottom
				windowBorder.Add(new Point2D(xOffset + graphPosX, yOffset + graphPosY + height)); // left top
			}
			return new List<Polygon2D>(new Polygon2D[] { windowBorder });
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

		public override List<Polygon2D> GetOutsideBorder(double xOffset, double yOffset) {
			return new List<Polygon2D>(new Polygon2D[] { this.GetOutsideBorder(this.GetObjectBorders(xOffset, yOffset)[0]) });
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
