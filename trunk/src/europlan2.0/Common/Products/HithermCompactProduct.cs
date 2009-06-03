using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class HithermCompactProduct : Product {

		public HithermCompactProduct() {

		}

		protected HithermCompactProduct(HithermCompactProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 100;
			canHeat = true;
			canCool = false;
		}

		public override Product Clone(Room room) {
			HithermCompactProduct product = new HithermCompactProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override int CalculateQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 10);
		}

	}
	
}
