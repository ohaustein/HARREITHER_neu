using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ModulKlimaDeckeProduct : Product {

		public ModulKlimaDeckeProduct() {

		}

		protected ModulKlimaDeckeProduct(ModulKlimaDeckeProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 80;
			quickDimensioningCoolPowerPerSquareMeter = 80;
			canHeat = true;
			canCool = true;
		}

		public override Product Clone(Room room) {
			ModulKlimaDeckeProduct product = new ModulKlimaDeckeProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override void CalculateQuickDimensioningCircuits() {
			quickDimensioningCircuits = (int)Math.Ceiling(quickDimensioningPlannedArea / 18);
		}

		public override void CalculateQuickDimensioningPlannedArea() {
			if (this.AssociatedRoom != null) {
				this.quickDimensioningPlannedArea = this.AssociatedRoom.Area;
			}
		}

	}
	
}
