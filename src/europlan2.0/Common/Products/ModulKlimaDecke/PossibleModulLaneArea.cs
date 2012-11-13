using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using WW.Math.Geometry;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class PossibleModulLaneArea : FreeModulLaneArea {
		private Point2D topLeft, topRight, bottomRight, bottomLeft;
		private double length, width;

		public PossibleModulLaneArea(Point2D topLeft, Point2D bottomLeft, Point2D bottomRight, Point2D topRight, double top, double bottom)
			: base(top, bottom) {
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomRight = bottomRight;
			this.bottomLeft = bottomLeft;
			this.length = new Segment2D(this.topLeft, this.bottomLeft).GetLength();
			this.width = new Segment2D(this.topLeft, this.topRight).GetLength();
		}

		public Polygon2D Area {
			get {
				return new Polygon2D(new Point2D[] { this.topLeft, this.bottomLeft, this.bottomRight, this.topRight });
			}
		}

		[XmlIgnore]
		public double Length {
			get { return this.length; }
		}

		[XmlIgnore]
		public double Width {
			get { return this.width; }
		}

		[XmlIgnore]
		public Point2D TopLeft {
			get { return this.topLeft; }
		}

		[XmlIgnore]
		public Point2D TopRight {
			get { return this.topRight; }
		}

		[XmlIgnore]
		public Point2D BottomRight {
			get { return this.bottomRight; }
		}

		[XmlIgnore]
		public Point2D BottomLeft {
			get { return this.bottomLeft; }
		}
	}
}
