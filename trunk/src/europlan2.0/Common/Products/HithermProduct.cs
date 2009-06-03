using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class HithermProduct : Product {

		public HithermProduct() {

		}

		protected HithermProduct(HithermProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 100;
			canHeat = true;
			canCool = false;
		}

		public override Product Clone(Room room) {
			HithermProduct product = new HithermProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override int CalculateQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 10);
		}

	}
	
}
