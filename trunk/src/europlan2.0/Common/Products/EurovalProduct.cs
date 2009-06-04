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

		public override void CalculateQuickDimensioningCircuits() {
			// TODO
			// vorlauftemperatur und verlegeabstand berücksichtigen
			quickDimensioningCircuits = (int)Math.Ceiling(quickDimensioningPlannedArea / 15);
		}

		public override void CalculateQuickDimensioningPlannedArea() {
			if (this.AssociatedRoom != null) {
				this.quickDimensioningPlannedArea = this.AssociatedRoom.Area;
			}
		}

	}
	
}
