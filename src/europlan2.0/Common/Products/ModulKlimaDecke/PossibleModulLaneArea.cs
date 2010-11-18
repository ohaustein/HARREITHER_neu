using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using WW.Math.Geometry;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class PossibleModulLaneArea : FreeModulLaneArea {
		//private Polygon2D area;
		private Point2D topLeft, topRight, bottomRight, bottomLeft;
		//private double top, bottom;
		private double length, width;

		public PossibleModulLaneArea(/*Polygon2D area*/Point2D topLeft, Point2D bottomLeft, Point2D bottomRight, Point2D topRight, double top, double bottom)
			: base(top, bottom) {
			//this.area = area;
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomRight = bottomRight;
			this.bottomLeft = bottomLeft;
			this.length = new Segment2D(this.topLeft, this.bottomLeft).GetLength();
			this.width = new Segment2D(this.topLeft, this.topRight).GetLength();
			//this.top = top;
			//this.bottom = bottom;
		}

		public Polygon2D Area {
			get {
				// return this.area;
				return new Polygon2D(new Point2D[] { this.topLeft, this.bottomLeft, this.bottomRight, this.topRight });
			}
			/*set {
				this.area = value;
				if (this.area.Count != 4) {
					this.area = null;
				}
				if (this.area == null) {
					length = 0;
					width = 0;
					//borderLeft = null;
					//borderRight = null;
				} else {
					length = new Segment2D(this.area[0], this.area[1]).GetLength();
					width = new Segment2D(this.area[1], this.area[2]).GetLength();
					//borderLeft = new Line2D(area[0], area[0] - area[3]);
					//borderRight = new Line2D(area[1], area[1] - area[2]);
				}
			}*/
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

		//[XmlIgnore]
		//public double Top {
		//	get { return this.top; }
		//}
		//
		//[XmlIgnore]
		//public double Bottom {
		//	get { return this.bottom; }
		//}
		//
		//public bool Fits(double moduleTop, double moduleBottom) {
		//	return (this.top <= moduleTop && this.bottom >= moduleBottom);
		//}
		//
		//public Nullable<double> BestStart(double moduleTop, double moduleBottom, bool bottomUp) {
		//	if (bottomUp) {
		//		if (moduleBottom <= this.bottom) {
		//			if (moduleTop >= this.top) {
		//				return moduleTop;
		//			} else {
		//				return null;
		//			}
		//		} else {
		//			if (moduleTop <= this.bottom) {
		//				return this.BestStart(this.bottom + moduleTop - moduleBottom, this.bottom, bottomUp);
		//			} else {
		//				return null;
		//			}
		//		}
		//	} else {
		//		if (moduleTop >= this.top) {
		//			if (moduleBottom <= this.bottom) {
		//				return moduleTop;
		//			} else {
		//				return null;
		//			}
		//		} else {
		//			if (moduleBottom >= this.top) {
		//				return this.BestStart(this.top, this.top + moduleBottom - moduleTop, bottomUp);
		//			} else {
		//				return null;
		//			}
		//		}
		//	}
		//}
	}
}
