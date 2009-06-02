using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class HithermCompactProduct : Product {

		public HithermCompactProduct(Room room)	: base(room) {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 100;
		}

		public override Product Clone(Room room) {
			HithermCompactProduct product = new HithermCompactProduct(room);
			return product;
		}
	}
	
}
