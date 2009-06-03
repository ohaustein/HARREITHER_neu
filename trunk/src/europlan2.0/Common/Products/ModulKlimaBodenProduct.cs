using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ModulKlimaBodenProduct : Product {

		public ModulKlimaBodenProduct() {

		}

		protected ModulKlimaBodenProduct(ModulKlimaBodenProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 50;
			canHeat = true;
			canCool = false;
		}

		public override Product Clone(Room room) {
			ModulKlimaBodenProduct product = new ModulKlimaBodenProduct(this);
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
