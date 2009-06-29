using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	[Serializable()]
	public class EurovalProduct : Product {

		public enum LayDistance {
			EV5,
			EV10,
			EV15,
			EV20,
			EV25,
			EV30,
			EV35
		}

		public EurovalProduct(){

		}

		protected EurovalProduct(EurovalProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 50;
			quickDimensioningCoolPowerPerSquareMeter = 50;
			canHeat = true;
			canCool = false;
		}

		public override Product Clone(Room room) {
			EurovalProduct product = new EurovalProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override int GetDefaultQuickDimensioningCircuits() {
			LayDistance distance = Project.Instance.QuickDimensioning.LayDistance;
			double factor = 0;
			switch (distance) {
				case LayDistance.EV5:
					factor = 10;
					break;
				case LayDistance.EV10:
					factor = 7.5;
					break;
				case LayDistance.EV15:
					factor = 6.7;
					break;
				case LayDistance.EV20:
					factor = 5;
					break;
				case LayDistance.EV25:
					factor = 4;
					break;
				case LayDistance.EV30:
					factor = 3.5;
					break;
				case LayDistance.EV35:
					factor = 3;
					break;
			}
			return (int)Math.Ceiling(quickDimensioningPlannedArea / (100/factor));
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			if (this.AssociatedRoom != null) {
				return this.AssociatedRoom.Area;
			}
			return 0;
		}

		public override float QuickDimensioningMaximumArea {
			get {
				if (this.AssociatedRoom != null) {
					return this.AssociatedRoom.Area;
				}
				return 0;
			}
		}

		public override string Name {
			get { return "Euroval"; }
		}
	}
	
}
