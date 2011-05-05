using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class FreeModulLaneArea {
		protected double top, bottom;

		public FreeModulLaneArea(double top, double bottom) {
			this.top = top;
			this.bottom = bottom;
		}

		[XmlIgnore]
		public double Top {
			get { return this.top; }
		}

		[XmlIgnore]
		public double Bottom {
			get { return this.bottom; }
		}

		public bool Fits(double moduleTop, double moduleBottom) {
			return (this.top <= moduleTop && this.bottom >= moduleBottom);
		}

		// to avoid rounding errors two values are treated as equal if their difference is smaller than epsion
		public Nullable<double> BestStart(double moduleTop, double moduleBottom, bool bottomUp, double epsilon) {
			if (bottomUp) {
				//if (moduleBottom <= this.bottom) {
				if (moduleBottom - this.bottom < epsilon) {
					//if (moduleTop >= this.top) {
					if (this.top - moduleTop < epsilon) {
						return moduleTop;
					} else {
						return null;
					}
				} else {
					//if (moduleTop <= this.bottom) {
					if (moduleTop - this.bottom < epsilon) {
						return this.BestStart(this.bottom + moduleTop - moduleBottom, this.bottom, bottomUp, epsilon);
					} else {
						return null;
					}
				}
			} else {
				//if (moduleTop >= this.top) {
				if (this.top - moduleTop < epsilon) {	
					//if (moduleBottom <= this.bottom) {
					if (moduleBottom - this.bottom < epsilon) {
						return moduleTop;
					} else {
						return null;
					}
				} else {
					//if (moduleBottom >= this.top) {
					if (this.top - moduleBottom < epsilon) {
						return this.BestStart(this.top, this.top + moduleBottom - moduleTop, bottomUp, epsilon);
					} else {
						return null;
					}
				}
			}
		}
	}

}
