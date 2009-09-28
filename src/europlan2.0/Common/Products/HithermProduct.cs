using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Hitherm")]
	public class HithermProduct : Product {

		public HithermProduct() {

		}

		protected HithermProduct(HithermProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 100;
			quickDimensioningCoolPowerPerSquareMeter = 100;
			canHeat = true;
			canCool = false;
		}

		public override Product Clone(Room room) {
			HithermProduct product = new HithermProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override int GetDefaultQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 10);
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			return 0;
		}

		public override float QuickDimensioningMaximumArea {
			get { return Int32.MaxValue; }
		}

		public override string Name {
			get { return "Hitherm"; }
		}

		public override string QuickDimensioningName {
			get { return "Hitherm\n(m²)"; }
		}

		public override ProductType Type {
			get { return ProductType.WH; }
		}

		public override void ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad) {
			// TODO
		}

		public override double PlannedCoolLoad {
			get { return 0; }
		}

		public override double PlannedHeatLoad {
			get { return 0; }
		}
	}
	
}
