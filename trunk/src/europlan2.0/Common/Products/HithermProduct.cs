using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class HithermProduct : Product {

		public HithermProduct() {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 100;
		}

		public override Product Clone() {
			HithermProduct product = new HithermProduct();
			return product;
		}
	}
	
}
