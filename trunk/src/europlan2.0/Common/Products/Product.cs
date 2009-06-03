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

		private Room associatedRoom = null;

		public Product() {
			Initialize();
		}

		protected Product(Room room) {
			associatedRoom = room;
			if (room != null) {
				room.UsedProductsForQuickDimensioning.Add(this);
			}
			Initialize();
		}

		public Product(Product product) {
			this.quickDimensioningHeatPower = product.quickDimensioningHeatPower;
			this.quickDimensioningCoolPower = product.quickDimensioningCoolPower;
			this.quickDimensioningCircuits = product.quickDimensioningCircuits;
			this.canHeat = product.canHeat;
			this.canCool = product.canCool;
			this.associatedRoom = null;
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

		protected Room AssociatedRoom {
			get { return associatedRoom; }
			set { 
				associatedRoom = value;
				if (associatedRoom != null && !associatedRoom.UsedProductsForQuickDimensioning.Contains(this)) {
					associatedRoom.UsedProductsForQuickDimensioning.Add(this);
				}			
			}
		}

	}
}
