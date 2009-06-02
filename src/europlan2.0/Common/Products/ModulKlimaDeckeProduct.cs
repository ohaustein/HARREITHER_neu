using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ModulKlimaDeckeProduct : Product {

		public ModulKlimaDeckeProduct() {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 80;
		}

		public override Product Clone() {
			ModulKlimaDeckeProduct product = new ModulKlimaDeckeProduct();
			return product;
		}	
	}
	
}
