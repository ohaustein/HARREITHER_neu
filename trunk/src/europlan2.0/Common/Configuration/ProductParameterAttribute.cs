using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	[AttributeUsage(AttributeTargets.Property)]
	public class ProductParameterAttribute: Attribute {
		public bool overrideableInPlanning = false;
		public bool overrideableInQuickDimensioning = false;

		public ProductParameterAttribute() {
		}

		public ProductParameterAttribute(bool overrideableInPlanning, bool overrideableInQuickDimensioning) {
			this.overrideableInPlanning = overrideableInPlanning;
			this.overrideableInQuickDimensioning = overrideableInQuickDimensioning;
		}
	}
}
