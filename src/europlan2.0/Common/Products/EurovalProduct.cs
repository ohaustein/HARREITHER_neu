using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class EurovalProduct : Product {

		public EurovalProduct(Room room) : base(room) {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 50;
		}

		public override Product Clone(Room room) {
			EurovalProduct product = new EurovalProduct(room);
			return product;
		}

	}
	
}
