using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using WW.Math.Geometry;
using System.Windows.Forms;

namespace Europlan.Common {
	public interface IGraphicalWallObject {
		bool HitTest(Point2D planPoint, double xOffset, double yOffset);
		void PaintObject(Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export);
		IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset);
		Polygon2D GetObjectBorders(double xOffset, double yOffset);
		bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset, bool ignoreBorders);
		bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		List<Anchor> GetAnchors(double scale);
		bool IsMoveable {
			get;
		}
		bool Error {
			get;
			set;
		}

		void BackupState();
		void RevertState();
		bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY);

		bool IsNew {
			get;
			set;
		}

		bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom);
	}

	public enum AnchorTypeEnum {
		ANCHOR_NONE = 0x0,
		ANCHOR_SCALE_TOP = 0x1,
		ANCHOR_SCALE_RIGHT = 0x2,
		ANCHOR_SCALE_BOTTOM = 0x4,
		ANCHOR_SCALE_LEFT = 0x8,
		ANCHOR_SCALE_TOP_RIGHT = ANCHOR_SCALE_TOP | ANCHOR_SCALE_RIGHT,
		ANCHOR_SCALE_BOTTOM_RIGHT = ANCHOR_SCALE_BOTTOM | ANCHOR_SCALE_RIGHT,
		ANCHOR_SCALE_BOTTOM_LEFT = ANCHOR_SCALE_BOTTOM | ANCHOR_SCALE_LEFT,
		ANCHOR_SCALE_TOP_LEFT = ANCHOR_SCALE_TOP | ANCHOR_SCALE_LEFT,
		ANCHOR_MOVE_LEFT_RIGHT = ANCHOR_SCALE_LEFT | ANCHOR_SCALE_RIGHT,
		ANCHOR_MOVE_UP_DOWN = ANCHOR_SCALE_TOP | ANCHOR_SCALE_BOTTOM
	}

	public class Anchor {
		protected AnchorTypeEnum anchorType = AnchorTypeEnum.ANCHOR_NONE;
		protected Point2D position = Point2D.Zero;
		protected IGraphicalWallObject owner = null;

		public Anchor(double x, double y, AnchorTypeEnum anchorType, IGraphicalWallObject owner) {
			this.position = new Point2D(x, y); ;
			this.anchorType = anchorType;
			this.owner = owner;
		}

		public Anchor(Point2D position, AnchorTypeEnum anchorType, IGraphicalWallObject owner) {
			this.position = position;
			this.anchorType = anchorType;
			this.owner = owner;
		}

		public AnchorTypeEnum AnchorType {
			get { return this.anchorType; }
		}

		public Point2D Position {
			get { return this.position; }
		}

		public IGraphicalWallObject Owner {
			get { return this.owner; }
		}

		public Cursor Cursor {
			get {
				switch (this.anchorType) {
					case AnchorTypeEnum.ANCHOR_NONE:
						return Cursors.No;
					case AnchorTypeEnum.ANCHOR_MOVE_LEFT_RIGHT:
					case AnchorTypeEnum.ANCHOR_SCALE_LEFT:
					case AnchorTypeEnum.ANCHOR_SCALE_RIGHT:
						return Cursors.SizeWE;
					case AnchorTypeEnum.ANCHOR_MOVE_UP_DOWN:
					case AnchorTypeEnum.ANCHOR_SCALE_TOP:
					case AnchorTypeEnum.ANCHOR_SCALE_BOTTOM:
						return Cursors.SizeNS;
					case AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_LEFT:
					case AnchorTypeEnum.ANCHOR_SCALE_TOP_RIGHT:
						return Cursors.SizeNESW;
					case AnchorTypeEnum.ANCHOR_SCALE_TOP_LEFT:
					case AnchorTypeEnum.ANCHOR_SCALE_BOTTOM_RIGHT:
						return Cursors.SizeNWSE;
					default:
						return Cursors.Default;
				}
			}
		}

		public virtual void PaintAnchor(Graphics g, double xOffset, double yOffset, double scale) {
			Brush b = new SolidBrush(Color.DarkBlue);
			g.FillRectangle(b, (float)(this.position.X + xOffset - 3.0 / scale), (float)(this.position.Y + yOffset - 3.0 / scale), (float)(6.0 / scale), (float)(6.0 / scale));
		}

		protected bool IsInside(double value, double lowerBorder, double upperBorder) {
			return value >= lowerBorder && value <= upperBorder;
		}

		public virtual bool HitTest(Point2D planPoint, double xOffset, double yOffset, double scale) {
			return IsInside(planPoint.X, this.position.X + xOffset - 3.5 / scale, this.position.X + xOffset + 3.5 / scale) && IsInside(planPoint.Y, this.position.Y + yOffset - 3.5 / scale, this.position.Y + yOffset + 3.5 / scale);
		}
	}
}
