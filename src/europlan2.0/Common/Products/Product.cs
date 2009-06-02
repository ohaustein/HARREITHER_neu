using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[XmlInclude(typeof(EurovalProduct))]
	[XmlInclude(typeof(HithermProduct))]
	[XmlInclude(typeof(HithermCompactProduct))]
	[XmlInclude(typeof(ModulKlimaBodenProduct))]
	[XmlInclude(typeof(ModulKlimaDeckeProduct))]
	public abstract class Product {

		protected int quickDimensioningHeatPower = 0;
		protected int quickDimensioningCoolPower = 0;
		protected int quickDimensioningCircuits = 0;
		protected bool canHeat = false;
		protected bool canCool = false;

		protected Room associatedRoom = null;

		public Product(Room room) {
			associatedRoom = room;
			room.UsedProducts.Add(this);
			Initialize();
		}

		public abstract void Initialize();
		public abstract Product Clone(Room room);

		public int QuickDimensioningHeatPower {
			get { return quickDimensioningHeatPower; }
			set { quickDimensioningHeatPower = value; }
		}
		
		public int QuickDimensioningCoolPower {
			get { return quickDimensioningCoolPower; }
			set { quickDimensioningCoolPower = value; }
		}

		public int QuickDimensioningCircuits {
			get { return quickDimensioningCircuits; }
			set { quickDimensioningCircuits = value; }
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
