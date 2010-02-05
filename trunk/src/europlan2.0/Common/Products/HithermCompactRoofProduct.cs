using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Hitherm® Compact Dachschräge")]
	public class HithermCompactRoofProduct : HithermCompactProduct {

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 100;
		private static int quickDimensioningCoolPowerPerSquareMeter = 100;
		private static bool canHeat = true;
		private static bool canCool = false;

		public HithermCompactRoofProduct() {

		}

		protected HithermCompactRoofProduct(HithermCompactRoofProduct product)
			: base(product) {

		}

		public new static void StaticInitialize(Configuration config) {
			quickDimensioningHeatPowerPerSquareMeter = config.GetProductParameterAsInt<HithermCompactRoofProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 100);
			quickDimensioningCoolPowerPerSquareMeter = config.GetProductParameterAsInt<HithermCompactRoofProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 100);
			canHeat = config.GetProductParameterAsBool<HithermCompactRoofProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = config.GetProductParameterAsBool<HithermCompactRoofProduct>("ConfigQuickDimensioningCanCool", false);
		}

		public override Product Clone(Room room) {
			HithermCompactRoofProduct product = new HithermCompactRoofProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		#region Product Parameters
		[ProductParameter]
		public new static bool ConfigQuickDimensioningCanHeat {
			get { return canHeat; }
			set { canHeat = value; }
		}
		public override bool QuickDimensioningCanHeat {
			get { return canHeat; }
		}

		[ProductParameter]
		public new static bool ConfigQuickDimensioningCanCool {
			get { return canCool; }
			set { canCool = value; }
		}
		public override bool QuickDimensioningCanCool {
			get { return canCool; }
		}

		[ProductParameter]
		public new static int ConfigQuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
			set { quickDimensioningHeatPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
		}

		[ProductParameter]
		public new static int ConfigQuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
			set { quickDimensioningCoolPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
		}
		#endregion Product Parameters

		public override string QuickDimensioningName {
			get { return QuickDimensioningNameStatic; }
		}

		public static string QuickDimensioningNameStatic {
			get { return "Hitherm®\nCompact\nDachschr.\n(m²)"; }
		}

		public override ProductType Type {
			get { return ProductType.DSH; }
		}
	}

}
