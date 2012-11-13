using System;
using System.Collections.Generic;
using System.Text;
using Europlan.Licensing;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Product_ConcreteActivationName", "Product_ConcreteActivationFullName")]
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
			Product.StaticInitialize<ConcreteActivationProduct>(config);
		}

		public override Product Clone(Room room) {
			ConcreteActivationProduct product = new ConcreteActivationProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		#region Product Parameters
		[BoolProductParameter(true)]
		public new static bool ConfigQuickDimensioningCanHeat {
			get { return canHeat; }
			set { canHeat = value; }
		}
		public override bool QuickDimensioningCanHeat {
			get { return canHeat; }
		}

		[BoolProductParameter(true)]
		public new static bool ConfigQuickDimensioningCanCool {
			get { return canCool; }
			set { canCool = value; }
		}
		public override bool QuickDimensioningCanCool {
			get { return canCool; }
		}

		[IntProductParameter(80)]
		public new static int ConfigQuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
			set { quickDimensioningHeatPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
		}

		[IntProductParameter(80)]
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

		public new static string QuickDimensioningNameStatic {
			get { return EuroplanRes.ConcreteActivationProduct_Schnellauslegung; }
		}

		public override ProductType Type {
			get { return ProductType.FBH; }
		}
	}
	
}
