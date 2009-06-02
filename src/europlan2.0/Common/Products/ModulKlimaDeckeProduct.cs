using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ModulKlimaDeckeProduct : Product {

		public ModulKlimaDeckeProduct(Room room) : base(room) {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 80;
		}

		public override Product Clone(Room room) {
			ModulKlimaDeckeProduct product = new ModulKlimaDeckeProduct(room);
			return product;
		}	
	}
	
}
