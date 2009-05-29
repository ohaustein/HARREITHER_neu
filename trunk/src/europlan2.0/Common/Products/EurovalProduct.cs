using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class EurovalProduct : Product {

		public EurovalProduct() {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 50;
		}

	}
	
}
