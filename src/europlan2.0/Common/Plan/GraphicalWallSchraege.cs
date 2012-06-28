using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class GraphicalWallSchraege : IGraphicalWallObject {

		public enum OrientationEnum {
			LEFT,
			RIGHT
		}

		private GraphicalWall wall;
		private OrientationEnum orientation;
		//private double width;
		//private double height;

		public GraphicalWallSchraege(GraphicalWall wall, OrientationEnum orientation) {
			this.wall = wall;
			this.orientation = orientation;
		}

		public GraphicalWall Wall {
			get { return this.wall; }
			//set { this.wall = value; }
		}

		public OrientationEnum Orientation {
			get { return this.orientation; }
			//set { this.orientation = value; }
		}

		public double Width {
			get { 
				if (this.orientation == OrientationEnum.LEFT) {
					if (this.wall.CeilingContour[0].Y < this.wall.CeilingContour[1].Y) {
						return (this.wall.CeilingContour[1].X - this.wall.CeilingContour[0].X) * 100.0;
					}
				} else {
					int count = this.wall.CeilingContour.Count;
					if (this.wall.CeilingContour[count - 1].Y < this.wall.CeilingContour[count - 2].Y) {
						return (this.wall.CeilingContour[count - 1].X - this.wall.CeilingContour[count - 2].X) * 100.0;
					}
				}
				return 0;
			}
			set {
				double newWidth = value / 100.0;
				if (this.orientation == OrientationEnum.LEFT) {
					this.wall.CeilingContour[1] = new Point2D(this.wall.CeilingContour[0].X + newWidth, this.wall.CeilingContour[1].Y);
					if (this.wall.CeilingContour[1].X > this.wall.CeilingContour[2].X) {
						this.wall.CeilingContour[1] = new Point2D(this.wall.CeilingContour[2].X, this.wall.CeilingContour[1].Y);
					}
					if (this.wall.CeilingContour[1].X < this.wall.CeilingContour[0].X) {
						this.wall.CeilingContour[1] = new Point2D(this.wall.CeilingContour[0].X, this.wall.CeilingContour[1].Y);
					}
				} else {
					int count = this.wall.CeilingContour.Count;
					this.wall.CeilingContour[count - 2] = new Point2D(this.wall.CeilingContour[count - 1].X - newWidth, this.wall.CeilingContour[count - 2].Y);
					if (this.wall.CeilingContour[count - 3].X > this.wall.CeilingContour[count - 2].X) {
						this.wall.CeilingContour[count - 2] = new Point2D(this.wall.CeilingContour[count - 3].X, this.wall.CeilingContour[count - 2].Y);
					}
					if (this.wall.CeilingContour[count - 1].X < this.wall.CeilingContour[count - 2].X) {
						this.wall.CeilingContour[count - 2] = new Point2D(this.wall.CeilingContour[count - 1].X, this.wall.CeilingContour[count - 2].Y);
					}
				}
				if (this.wall.DachSchraege != null) {
					this.wall.DachSchraege.AdjustWidth(this.wall.CeilingContour[2].X - this.wall.CeilingContour[1].X, this.orientation == OrientationEnum.LEFT);
				}
			}
		}

		public double Height {
			get {
				if (this.orientation == OrientationEnum.LEFT) {
					if (this.wall.CeilingContour[0].Y < this.wall.CeilingContour[1].Y) {
						return (this.wall.CeilingContour[1].Y - this.wall.CeilingContour[0].Y) * 100.0;
					}
				} else {
					int count = this.wall.CeilingContour.Count;
					if (this.wall.CeilingContour[count - 1].Y < this.wall.CeilingContour[count - 2].Y) {
						return (this.wall.CeilingContour[count - 2].Y - this.wall.CeilingContour[count - 1].Y) * 100.0;
					}
				}
				return 0;
			}
			set {
				double newHeight = value / 100.0;
				if (this.orientation == OrientationEnum.LEFT) {
					this.wall.CeilingContour[0] = new Point2D(this.wall.CeilingContour[0].X, this.wall.CeilingContour[1].Y - newHeight);
					if (this.wall.CeilingContour[0].Y < 0) {
						this.wall.CeilingContour[0] = new Point2D(this.wall.CeilingContour[0].X, 0);
					}
					if (this.wall.CeilingContour[0].Y > this.wall.CeilingContour[1].Y) {
						this.wall.CeilingContour[0] = new Point2D(this.wall.CeilingContour[0].X, this.wall.CeilingContour[1].Y);
					}
				} else {
					int count = this.wall.CeilingContour.Count;
					this.wall.CeilingContour[count - 1] = new Point2D(this.wall.CeilingContour[count - 1].X, this.wall.CeilingContour[count - 2].Y - newHeight);
					if (this.wall.CeilingContour[count - 1].Y < 0) {
						this.wall.CeilingContour[count - 1] = new Point2D(this.wall.CeilingContour[count - 1].X, 0);
					}
					if (this.wall.CeilingContour[count - 1].Y > this.wall.CeilingContour[count - 2].Y) {
						this.wall.CeilingContour[count - 1] = new Point2D(this.wall.CeilingContour[count - 1].X, this.wall.CeilingContour[count - 2].Y);
					}
				}
			}
		}

		#region IGraphicalWallObject Members
		public bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			throw new Exception("The method or operation is not implemented.");
		}

		public void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export) {
			if (selectedObject == this) {
				Region oldClip = g.Clip;
				Region clip = new Region();
				clip.MakeInfinite();
				g.Clip = clip;
				Pen pen = new Pen(Color.Red, (float)(2.0 / scale));
				PointF start;
				PointF end;
				if (this.orientation == OrientationEnum.LEFT) {
					start = new PointF((float)xOffset, (float)(yOffset + this.wall.GetWallHeight() * 100.0 - this.Height));
					end = new PointF((float)(xOffset + this.Width), (float)(yOffset + this.wall.GetWallHeight() * 100));
				} else {
					start = new PointF((float)(xOffset + this.wall.GetWallWidth() * 100), (float)(yOffset + this.wall.GetWallHeight() * 100 - this.Height));
					end = new PointF((float)(xOffset + this.wall.GetWallWidth() * 100 - this.Width), (float)(yOffset + this.wall.GetWallHeight() * 100.0));
				}
				PointF p = new PointF(end.X, start.Y);
				g.DrawLine(pen, start, end);
				pen.Width = (float)(1.0 / scale);
				pen.DashPattern = new float[] { 1, 2 };
				g.DrawLine(pen, start, p);
				g.DrawLine(pen, p, end);
				g.Clip = oldClip;
			}
			// nothing to do as the dachschraege is painted by the wall
		}

		public IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset) {
			Segment2D schraege;
			if (this.orientation == OrientationEnum.LEFT) {
				schraege = new Segment2D(new Point2D(xOffset, yOffset + this.wall.GetWallHeight() * 100.0 - this.Height), new Point2D(xOffset + this.Width, yOffset + this.wall.GetWallHeight() * 100.0));
			} else {
				schraege = new Segment2D(new Point2D(xOffset + this.wall.GetWallWidth() * 100.0, yOffset + this.wall.GetWallHeight() * 100.0 - this.Height), new Point2D(xOffset + this.wall.GetWallWidth() * 100.0 - this.Width, yOffset + this.wall.GetWallHeight() * 100.0));
			}
			if (schraege.GetDistance(planPoint) > 10) {
				return null;
			}
			if (this.orientation == OrientationEnum.LEFT) {
				if (planPoint.X - xOffset < 0) {
					return null;
				}
				if (planPoint.X - xOffset > this.Width) {
					return null;
				}
				if (planPoint.Y - yOffset > this.wall.GetWallHeight() * 100) {
					return null;
				}
				if (planPoint.Y - yOffset < this.wall.GetWallHeight() * 100 - this.Height) {
					return null;
				}
				return this;
			} else {
				if (planPoint.X - xOffset < this.wall.GetWallWidth() * 100 - this.Width) {
					return null;
				}
				if (planPoint.X - xOffset > this.wall.GetWallWidth() * 100) {
					return null;
				}
				if (planPoint.Y - yOffset > this.wall.GetWallHeight() * 100) {
					return null;
				}
				if (planPoint.Y - yOffset < this.wall.GetWallHeight() * 100 - this.Height) {
					return null;
				}
				return this;
			}
		}

		public List<WW.Math.Geometry.Polygon2D> GetObjectBorders(double xOffset, double yOffset) {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool CollisionTest(IList<WW.Math.Geometry.Polygon2D> polygon, double xOffset, double yOffset, bool ignoreBorders) {
			// nothing to do as the collision testing is handled by the wall
			return false;
		}

		private Nullable<Point2D> startDrag = null;
		private double startWidth, startHeight;

		public bool StartDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			this.startDrag = planPoint;
			this.startHeight = this.Height;
			this.startWidth = this.Width;
			return false;
		}

		public bool MoveDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			if (anchor != null) {
				if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_LEFT) == AnchorTypeEnum.ANCHOR_SCALE_LEFT && this.orientation == OrientationEnum.RIGHT) {
					this.Width = this.startWidth - planPoint.X + startDrag.Value.X;
				} else if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_RIGHT) == AnchorTypeEnum.ANCHOR_SCALE_RIGHT && this.orientation == OrientationEnum.LEFT) {
					this.Width = this.startWidth + planPoint.X - startDrag.Value.X;
				}

				if ((anchor.AnchorType & AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) == AnchorTypeEnum.ANCHOR_SCALE_BOTTOM) {
					this.Height = this.startHeight + this.startDrag.Value.Y - planPoint.Y;
				}
			}
			owningRoom.MarkErrors(this, owningWall);
			return true;
		}

		public bool EndDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap) {
			owningRoom.MarkErrors(this, owningWall);
			this.startDrag = null;
			return false;
		}

		public List<Anchor> GetAnchors(double scale) {
			if (this.orientation == OrientationEnum.LEFT) {
				double left = 0;
				double right = this.Width;
				double top = this.wall.GetWallHeight() * 100.0;
				double bottom = this.wall.GetWallHeight() * 100.0 - this.Height;

				List<Anchor> anchors = new List<Anchor>();
				//anchors.Add(new Anchor(left, top, AnchorTypeEnum.ANCHOR_NONE, this));
				anchors.Add(new Anchor(right, top, AnchorTypeEnum.ANCHOR_SCALE_RIGHT, this));
				anchors.Add(new Anchor(right, bottom, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_RIGHT, this));
				anchors.Add(new Anchor(left, bottom, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM, this));

				//anchors.Add(new Anchor((left + right) / 2.0, top, AnchorTypeEnum.ANCHOR_NONE, this));
				anchors.Add(new Anchor((left + right) / 2.0, bottom, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM, this));
				//anchors.Add(new Anchor(left, (top + bottom) / 2.0, AnchorTypeEnum.ANCHOR_NONE, this));
				anchors.Add(new Anchor(right, (top + bottom) / 2.0, AnchorTypeEnum.ANCHOR_SCALE_RIGHT, this));
				return anchors;
			} else {
				double left = this.wall.GetWallWidth() * 100.0 - this.Width;
				double right = this.wall.GetWallWidth() * 100.0;
				double top = this.wall.GetWallHeight() * 100.0;
				double bottom  = this.wall.GetWallHeight() * 100.0 - this.Height;
				List<Anchor> anchors = new List<Anchor>();
				anchors.Add(new Anchor(left, top, AnchorTypeEnum.ANCHOR_SCALE_LEFT, this));
				//anchors.Add(new Anchor(right, top, AnchorTypeEnum.ANCHOR_NONE, this));
				anchors.Add(new Anchor(right, bottom, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM, this));
				anchors.Add(new Anchor(left, bottom, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_LEFT, this));

				//anchors.Add(new Anchor((left + right) / 2.0, top, AnchorTypeEnum.ANCHOR_NONE, this));
				anchors.Add(new Anchor((left + right) / 2.0, bottom, AnchorTypeEnum.ANCHOR_SCALE_BOTTOM, this));
				anchors.Add(new Anchor(left, (top + bottom) / 2.0, AnchorTypeEnum.ANCHOR_SCALE_LEFT, this));
				//anchors.Add(new Anchor(right, (top + bottom) / 2.0, AnchorTypeEnum.ANCHOR_NONE, this));
				return anchors;
			}
		}

		public bool IsMoveable {
			get { return false; }
		}

		public bool Error {
			get {
				// TODO
				return false;
			}
			set {
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public void BackupState() {
			this.wall.BackupState();
		}

		public void RevertState() {
			this.wall.RevertState();
		}

		public bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY) {
			throw new Exception("The method or operation is not implemented.");
		}

		private bool isNew = false;
		[XmlIgnore]
		public bool IsNew {
			get { return this.isNew; }
			set { this.isNew = value; }
		}

		public bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom) {
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool Equals(object obj) {
			if (obj is GraphicalWallSchraege) {
				GraphicalWallSchraege other = obj as GraphicalWallSchraege;
				return this.wall == other.wall && this.orientation == other.orientation;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return (this.wall != null ? this.wall.GetHashCode() : 0) ^ this.orientation.GetHashCode();
		}
		#endregion
	}
}
