using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ModulKlimaBodenProduct : Product {

		public ModulKlimaBodenProduct(Room room) : base(room) {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 50;
		}

		public override Product Clone(Room room) {
			ModulKlimaBodenProduct product = new ModulKlimaBodenProduct(room);
			return product;
		}
	}
	
}
