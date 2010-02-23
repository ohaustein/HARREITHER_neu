using System;
using System.Collections.Generic;
using System.Text;
using Europlan.Licensing;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Betonkernaktivierung")]
	public class ConcreteActivationProduct : EurovalProduct {

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 80;
		private static int quickDimensioningCoolPowerPerSquareMeter = 80;
		private static bool canHeat = true;
		private static bool canCool = true;

		public ConcreteActivationProduct(){
			if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdConcreteActivation)) {
				throw new ProductNotLicensedException(this.GetType());
			}
		}

		protected ConcreteActivationProduct(ConcreteActivationProduct product) : base(product) {
		}

		public override void Initialize() {
		}

		public new static void StaticInitialize(Configuration config) {
			quickDimensioningHeatPowerPerSquareMeter = config.GetProductParameterAsInt<ConcreteActivationProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 80);
			quickDimensioningCoolPowerPerSquareMeter = config.GetProductParameterAsInt<ConcreteActivationProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 80);
			canHeat = config.GetProductParameterAsBool<ConcreteActivationProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = config.GetProductParameterAsBool<ConcreteActivationProduct>("ConfigQuickDimensioningCanCool", true);
		}

		public override Product Clone(Room room) {
			ConcreteActivationProduct product = new ConcreteActivationProduct(this);
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
			get { return "BKA\n(m²)"; }
		}

		public override ProductType Type {
			get { return ProductType.FBH; }
		}
	}
	
}
