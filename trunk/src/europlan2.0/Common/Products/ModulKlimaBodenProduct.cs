using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ModulKlimaBodenProduct : Product {
		
		public ModulKlimaBodenProduct() {

		}

		public override void Initialize() {
			quickDimensioningHeatPower = 50;
		}

		public override Product Clone() {
			ModulKlimaBodenProduct product = new ModulKlimaBodenProduct();
			return product;
		}
	}
	
}
