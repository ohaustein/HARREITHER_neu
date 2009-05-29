using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[XmlInclude(typeof(EurovalProduct))]
	public abstract class Product {

		protected int quickDimensioningHeatPower = 0;
		protected int quickDimensioningCoolPower = 0;

		public Product() {
			Initialize();
		}

		public abstract void Initialize();

		public int QuickDimensioningHeatPower {
			get {
				return quickDimensioningHeatPower;
			}
			set {
				quickDimensioningHeatPower = value;
			}
		}


		public int QuickDimensioningCoolPower {
			get {
				return quickDimensioningCoolPower;
			}
			set {
				quickDimensioningCoolPower = value;
			}
		}

	}
}
