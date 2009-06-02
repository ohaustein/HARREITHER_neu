using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class HithermCompactProduct : Product {

		public HithermCompactProduct() {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 100;
		}

		public override Product Clone() {
			HithermCompactProduct product = new HithermCompactProduct();
			return product;
		}
	}
	
}
