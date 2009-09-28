using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Hitherm Compact Dachschräge")]
	public class HithermCompactRoofProduct : HithermCompactProduct {

		public HithermCompactRoofProduct() {

		}

		protected HithermCompactRoofProduct(HithermCompactRoofProduct product)
			: base(product) {

		}


		public override Product Clone(Room room) {
			HithermCompactRoofProduct product = new HithermCompactRoofProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override string Name {
			get { return "Hitherm Compact Dachschräge"; }
		}

		public override string QuickDimensioningName {
			get { return "Hitherm\nCompact\nDachschr.\n(m²)"; }
		}

		public override ProductType Type {
			get { return ProductType.DH; }
		}
	}

}
