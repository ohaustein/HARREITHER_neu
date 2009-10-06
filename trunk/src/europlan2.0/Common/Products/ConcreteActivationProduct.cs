using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Betonkernaktivierung")]
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

		public override Product Clone(Room room) {
			ConcreteActivationProduct product = new ConcreteActivationProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override string Name {
			get { return "Betonkernaktivierung"; }
		}

		public override string QuickDimensioningName {
			get { return "BKA\n(m²)"; }
		}

		/// <summary>
		/// The full name of this product
		/// </summary>
		public override string FullName {
			get { return Name; }
		}

		public override ProductType Type {
			get { return ProductType.FBH; }
		}
	}
	
}
