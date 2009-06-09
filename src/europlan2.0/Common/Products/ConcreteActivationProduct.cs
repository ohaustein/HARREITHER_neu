using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ConcreteActivationProduct : EurovalProduct {

		public ConcreteActivationProduct(){

		}

		protected ConcreteActivationProduct(ConcreteActivationProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 80;
			quickDimensioningCoolPowerPerSquareMeter = 80;
			canHeat = true;
			canCool = true;
		}

	}
	
}
