using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class HithermProduct : Product {

		public HithermProduct(Room room) : base(room) {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 100;
		}

		public override Product Clone(Room room) {
			HithermProduct product = new HithermProduct(room);
			return product;
		}
	}
	
}
