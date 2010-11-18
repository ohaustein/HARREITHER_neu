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

		public Nullable<double> BestStart(double moduleTop, double moduleBottom, bool bottomUp) {
			if (bottomUp) {
				if (moduleBottom <= this.bottom) {
					if (moduleTop >= this.top) {
						return moduleTop;
					} else {
						return null;
					}
				} else {
					if (moduleTop <= this.bottom) {
						return this.BestStart(this.bottom + moduleTop - moduleBottom, this.bottom, bottomUp);
					} else {
						return null;
					}
				}
			} else {
				if (moduleTop >= this.top) {
					if (moduleBottom <= this.bottom) {
						return moduleTop;
					} else {
						return null;
					}
				} else {
					if (moduleBottom >= this.top) {
						return this.BestStart(this.top, this.top + moduleBottom - moduleTop, bottomUp);
					} else {
						return null;
					}
				}
			}
		}
	}

}
