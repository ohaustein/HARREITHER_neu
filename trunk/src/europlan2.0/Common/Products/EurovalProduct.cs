using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class EurovalProduct : Product {

		public EurovalProduct(){

		}

		protected EurovalProduct(EurovalProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 50;
			canHeat = true;
			canCool = false;
		}

		public override Product Clone(Room room) {
			EurovalProduct product = new EurovalProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override int GetDefaultQuickDimensioningCircuits() {
			// TODO
			// vorlauftemperatur und verlegeabstand berücksichtigen
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 15);
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			if (this.AssociatedRoom != null) {
				return this.AssociatedRoom.Area;
			}
			return 0;
		}

	}
	
}
