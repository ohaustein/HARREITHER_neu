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
		void PaintObject(Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale);
		IGraphicalWallObject GetPickedObject(Point2D planPoint, double xOffset, double yOffset);
		Polygon2D GetObjectBorders(double xOffset, double yOffset);
		bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset);
		//List<Pickpoint> GetPickpoints(double xOffset, double yOffset);
	}

	public enum PickpointType {
		PPT_NONE = 0x0,
		PPT_SCALE_TOP = 0x1,
		PPT_SCALE_RIGHT = 0x2,
		PPT_SCALE_BOTTOM = 0x4,
		PPT_SCALE_LEFT = 0x8,
		PPT_SCALE_TOP_RIGHT = PPT_SCALE_TOP | PPT_SCALE_RIGHT,
		PPT_SCALE_BOTTOM_RIGHT = PPT_SCALE_BOTTOM | PPT_SCALE_RIGHT,
		PPT_SCALE_BOTTOM_LEFT = PPT_SCALE_BOTTOM | PPT_SCALE_LEFT,
		PPT_SCALE_TOP_LEFT = PPT_SCALE_TOP | PPT_SCALE_LEFT,
	}

	public class Pickpoint {
		private PickpointType ppType = PickpointType.PPT_NONE;
		private Point2D position = Point2D.Zero;
		private IGraphicalWallObject owner = null;

		public Pickpoint(Point2D position, PickpointType ppType, IGraphicalWallObject owner) {
			this.position = position;
			this.ppType = ppType;
			this.owner = owner;
		}

		public PickpointType PpType {
			get { return this.ppType; }
		}

		public Point2D Position {
			get { return this.position; }
		}

		public IGraphicalWallObject Owner {
			get { return this.owner; }
		}

		public Cursor Cursor {
			get {
				switch (this.ppType) {
					case PickpointType.PPT_NONE:
						return Cursors.No;
					case PickpointType.PPT_SCALE_LEFT:
					case PickpointType.PPT_SCALE_RIGHT:
						return Cursors.SizeWE;
					case PickpointType.PPT_SCALE_TOP:
					case PickpointType.PPT_SCALE_BOTTOM:
						return Cursors.SizeNS;
					case PickpointType.PPT_SCALE_BOTTOM_LEFT:
					case PickpointType.PPT_SCALE_TOP_RIGHT:
						return Cursors.SizeNESW;
					case PickpointType.PPT_SCALE_TOP_LEFT:
					case PickpointType.PPT_SCALE_BOTTOM_RIGHT:
						return Cursors.SizeNWSE;
					default:
						return Cursors.Default;
				}
			}
		}

		public void PaintPickpoint(Graphics g, double xOffset, double yOffset, double scale) {
			Brush b = new SolidBrush(Color.DarkBlue);
			g.FillRectangle(b, (float)(this.position.X + xOffset - 2.5 / scale), (float)(this.position.Y + yOffset - 2.5 / scale), (float)(5.0 / scale), (float)(5.0 / scale));
		}
	}
}
