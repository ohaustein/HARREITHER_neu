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

		public override void CalculateQuickDimensioningCircuits() {
			quickDimensioningCircuits = (int)Math.Ceiling(quickDimensioningPlannedArea / 10);
		}

		public override void CalculateQuickDimensioningPlannedArea() {
			this.quickDimensioningPlannedArea = 0;
		}

	}
	
}
