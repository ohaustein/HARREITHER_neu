using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[XmlInclude(typeof(EurovalProduct))]
	public abstract class Product {

		protected int quickDimensioningHeatPower = 0;
		protected int quickDimensioningCoolPower = 0;
		protected bool canHeat = false;
		protected bool canCool = false;

		public Product() {
			Initialize();
		}

		public abstract void Initialize();

		public int QuickDimensioningHeatPower {
			get { return quickDimensioningHeatPower; }
			set { quickDimensioningHeatPower = value; }
		}
		
		public int QuickDimensioningCoolPower {
			get { return quickDimensioningCoolPower; }
			set { quickDimensioningCoolPower = value; }
		}

		public bool CanHeat {
			get { return canHeat; }
			set { canHeat = value; }
		}

		public bool CanCool {
			get { return canCool; }
			set { canCool = value; }
		}

	}
}
