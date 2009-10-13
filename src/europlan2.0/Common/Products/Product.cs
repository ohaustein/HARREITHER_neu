using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[XmlInclude(typeof(EurovalProduct))]
	[XmlInclude(typeof(ConcreteActivationProduct))]
	[XmlInclude(typeof(HithermProduct))]
	[XmlInclude(typeof(HithermCompactProduct))]
	[XmlInclude(typeof(ModulKlimaBodenProduct))]
	[XmlInclude(typeof(ModulKlimaDeckeProduct))]
	[Serializable()]
	public abstract class Product {

		public enum ProductType {
			FBH,
			WH,
			DH,
			REST
		}

		protected int quickDimensioningHeatPowerPerSquareMeter = 0;
		protected int quickDimensioningCoolPowerPerSquareMeter = 0;
		protected int quickDimensioningCircuits = 0;
		protected string quickDimensioningCircuitsAsString = null;
		protected float quickDimensioningPlannedArea = 0;
		protected bool canHeat = false;
		protected bool canCool = false;
		private Room associatedRoom = null;
		private SerializableDictionary<string, int> quickDimensioningConnectedDistributors = new SerializableDictionary<string,int>();
		private ProductConnection plannedConnectionVorlauf = null;
		private ProductConnection plannedConnectionRuecklauf = null;

		protected string comment = null;

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
			this.quickDimensioningHeatPowerPerSquareMeter = product.quickDimensioningHeatPowerPerSquareMeter;
			this.quickDimensioningCoolPowerPerSquareMeter = product.quickDimensioningCoolPowerPerSquareMeter;
			this.quickDimensioningCircuits = product.quickDimensioningCircuits;
			this.quickDimensioningCircuitsAsString = product.quickDimensioningCircuitsAsString;
			this.canHeat = product.canHeat;
			this.canCool = product.canCool;
			this.quickDimensioningPlannedArea = 0;
			this.associatedRoom = null;
			this.quickDimensioningConnectedDistributors = new SerializableDictionary<string, int>();
		}

		public abstract void Initialize();
		public abstract void StaticInitialize();
		public abstract Product Clone(Room room);
		public abstract int GetDefaultQuickDimensioningCircuits();
		public abstract float GetDefaultQuickDimensioningPlannedArea();

		public int QuickDimensioningHeatPower {
			get {
				if (canHeat) {
					return (int)(QuickDimensioningPlannedArea * quickDimensioningHeatPowerPerSquareMeter);
				}
				return 0; 
			}
		}
		
		public int QuickDimensioningCoolPower {
			get {
				if (canCool) {
					return (int)(QuickDimensioningPlannedArea * quickDimensioningCoolPowerPerSquareMeter);
				}
				return 0; 
			}
		}

		public int QuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
			set { quickDimensioningHeatPowerPerSquareMeter = value; }
		}

		public int QuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
			set { quickDimensioningCoolPowerPerSquareMeter = value; }
		}

		public float QuickDimensioningPlannedArea {
			get {
				return quickDimensioningPlannedArea; 
			}
			set { quickDimensioningPlannedArea = value; }
		}

		public abstract float QuickDimensioningMaximumArea {
			get;
		}

		public int QuickDimensioningCircuits {
			get {
				return quickDimensioningCircuits; 
			}
			set {
				quickDimensioningCircuits = value;
				this.CheckPlannedCircuits();
			}
		}

		public string QuickDimensioningCircuitsAsString {
			get {
				if (quickDimensioningCircuitsAsString == null) {
					return quickDimensioningCircuits.ToString();
				} else {
					return quickDimensioningCircuitsAsString;
				}
			}
			set {
				if (Int32.TryParse(value, out quickDimensioningCircuits)) {
					quickDimensioningCircuitsAsString = null;
				} else {
					quickDimensioningCircuitsAsString = value; 
				}
				this.CheckPlannedCircuits();
			}
		}

		public bool CanHeat {
			get { return canHeat; }
			set { canHeat = value; }
		}

		public bool CanCool {
			get { return canCool; }
			set { canCool = value; }
		}

		[XmlIgnore]
		public Room AssociatedRoom {
			get { return associatedRoom; }
			set { 
				associatedRoom = value;
				if (associatedRoom != null && !associatedRoom.UsedProductsForQuickDimensioning.Contains(this)) {
					associatedRoom.UsedProductsForQuickDimensioning.Add(this);
				}			
			}
		}

		public SerializableDictionary<string, int> QuickDimensioningConnectedDistributors {
			get { return quickDimensioningConnectedDistributors; }
			set { quickDimensioningConnectedDistributors = value; }
		}

		protected void CheckPlannedCircuits() {
			int plannedCircuits = 0;
			string distributor = null;
			foreach (KeyValuePair<string, int> connectedDist in this.quickDimensioningConnectedDistributors) {
				plannedCircuits += connectedDist.Value;
				distributor = connectedDist.Key;
			}
			if (plannedCircuits > this.QuickDimensioningCircuits) {
				this.quickDimensioningConnectedDistributors[distributor] -= (plannedCircuits - this.QuickDimensioningCircuits);
			}
		}

		public abstract string Name {
			get;
		}

		public abstract string QuickDimensioningName {
			get;
		}

		public abstract string FullName {
			get;
		}

		public abstract ProductType Type {
			get;
		}

		public string Comment {
			get { return this.comment; }
			set { this.comment = value; }
		}

		public float AvailableFloorArea {
			get {
				Room room = this.AssociatedRoom;
				float area = room.Area;
				foreach (PlannedProduct product in room.PlannedProducts) {
					if (product.Product != this) {
						area -= product.Product.PlannedFloorArea;
					}
				}
				if (area < 0) {
					area = 0;
				}
				return area;
			}
		}

		public abstract float PlannedFloorArea {
			get;
			set;
		}

		public abstract float PlannedRoofArea {
			get;
			set;
		}

		public abstract float PlannedWallArea {
			get;
			set;
		}

		public float TotalPlannedArea {
			get { return this.PlannedFloorArea + this.PlannedRoofArea + this.PlannedWallArea; }
		}

		public abstract double PlannedHeatLoad {
			get;
		}

		public abstract double PlannedCoolLoad {
			get;
		}

		public ProductConnection PlannedConnectionVorlauf {
			get { return this.plannedConnectionVorlauf; }
			set { this.plannedConnectionVorlauf = value; }
		}

		public ProductConnection PlannedConnectionRuecklauf {
			get { return this.plannedConnectionRuecklauf; }
			set { this.plannedConnectionRuecklauf = value; }
		}

		public void GetHeatFlow(out double vorlauf, out double ruecklauf) {
			List<Product> vorlaufProducts = new List<Product>();
			List<Product> ruecklaufProducts = new List<Product>();
			Distributor distributor = null;
			Product curP = this;
			bool allEuroval = this is EurovalProduct; // TODO Temporary check if all products are euroval until we know how to calculate heatflow for different products
			double eurovalPipeLengthBefore = 0; // Temporary sum of euroval pipe length
			double eurovalPipeLengthAfter = 0; // Temporary sum of euroval pipe length
			bool connected = true;
			while (connected && curP != null) {
				if (curP.PlannedConnectionVorlauf == null) {
					connected = false;
					continue;
				}
				switch (curP.PlannedConnectionVorlauf.ConnectionType) {
					case ProductConnection.ConnectionTypeEnum.DISTRIBUTOR:
						distributor = curP.PlannedConnectionVorlauf.Distributor;
						curP = null;
						break;
					case ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT:
						curP = curP.PlannedConnectionVorlauf.OtherProduct.Product;
						vorlaufProducts.Insert(0, curP);
						allEuroval = allEuroval & curP is EurovalProduct; // Temporary check if all products are euroval until we know how to calculate heatflow for different products
						eurovalPipeLengthBefore += curP is EurovalProduct ? (curP as EurovalProduct).PlannedPipeLength : 0; // Temporary sum of euroval pipe length
						break;
					default:
						connected = false;
						break;
				}
			}
			if (distributor == null) {
				connected = false;
			}
			curP = this;
			while (connected && curP != null) {
				switch (curP.PlannedConnectionRuecklauf.ConnectionType) {
					case ProductConnection.ConnectionTypeEnum.DISTRIBUTOR:
						if (!distributor.Equals(curP.PlannedConnectionRuecklauf.Distributor)) {
							connected = false;
						}
						distributor = curP.PlannedConnectionRuecklauf.Distributor;
						curP = null;
						break;
					case ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT:
						curP = curP.PlannedConnectionRuecklauf.OtherProduct.Product;
						ruecklaufProducts.Insert(0, curP);
						allEuroval = allEuroval & curP is EurovalProduct; // Temporary check if all products are euroval until we know how to calculate heatflow for different products
						eurovalPipeLengthAfter += curP is EurovalProduct ? (curP as EurovalProduct).PlannedPipeLength : 0; // Temporary sum of euroval pipe length
						break;
					default:
						connected = false;
						break;
				}
			}

			double verteilerVorlauf = connected ? distributor.RegulatorCircuit.HeatFlowTemperature : 35;
			double gesamtSpreizung = EN1264.Instance.DefaultSpreizung(verteilerVorlauf);
			vorlauf = verteilerVorlauf;
			ruecklauf = verteilerVorlauf - gesamtSpreizung;
			if (connected && allEuroval) {
				vorlauf = verteilerVorlauf - gesamtSpreizung * eurovalPipeLengthBefore / (eurovalPipeLengthBefore + (this as EurovalProduct).PlannedPipeLength + eurovalPipeLengthAfter);
				ruecklauf = verteilerVorlauf - gesamtSpreizung * (eurovalPipeLengthBefore + (this as EurovalProduct).PlannedPipeLength) / (eurovalPipeLengthBefore + (this as EurovalProduct).PlannedPipeLength + eurovalPipeLengthAfter);
			}
		}

		public void GetCoolFlow(out double vorlauf, out double ruecklauf) {
			List<Product> vorlaufProducts = new List<Product>();
			List<Product> ruecklaufProducts = new List<Product>();
			Distributor distributor = null;
			Product curP = this;
			bool allEuroval = this is EurovalProduct; // TODO Temporary check if all products are euroval until we know how to calculate heatflow for different products
			double eurovalPipeLengthBefore = 0; // Temporary sum of euroval pipe length
			double eurovalPipeLengthAfter = 0; // Temporary sum of euroval pipe length
			bool connected = true;
			while (connected && curP != null) {
				if (curP.PlannedConnectionVorlauf == null) {
					connected = false;
					continue;
				}
				switch (curP.PlannedConnectionVorlauf.ConnectionType) {
					case ProductConnection.ConnectionTypeEnum.DISTRIBUTOR:
						distributor = curP.PlannedConnectionVorlauf.Distributor;
						curP = null;
						break;
					case ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT:
						curP = curP.PlannedConnectionVorlauf.OtherProduct.Product;
						vorlaufProducts.Insert(0, curP);
						allEuroval = allEuroval & curP is EurovalProduct; // Temporary check if all products are euroval until we know how to calculate heatflow for different products
						eurovalPipeLengthBefore += curP is EurovalProduct ? (curP as EurovalProduct).PlannedPipeLength : 0; // Temporary sum of euroval pipe length
						break;
					default:
						connected = false;
						break;
				}
			}
			if (distributor == null) {
				connected = false;
			}
			curP = this;
			while (connected && curP != null) {
				switch (curP.PlannedConnectionRuecklauf.ConnectionType) {
					case ProductConnection.ConnectionTypeEnum.DISTRIBUTOR:
						if (!distributor.Equals(curP.PlannedConnectionRuecklauf.Distributor)) {
							connected = false;
						}
						distributor = curP.PlannedConnectionRuecklauf.Distributor;
						curP = null;
						break;
					case ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT:
						curP = curP.PlannedConnectionRuecklauf.OtherProduct.Product;
						ruecklaufProducts.Insert(0, curP);
						allEuroval = allEuroval & curP is EurovalProduct; // Temporary check if all products are euroval until we know how to calculate heatflow for different products
						eurovalPipeLengthAfter += curP is EurovalProduct ? (curP as EurovalProduct).PlannedPipeLength : 0; // Temporary sum of euroval pipe length
						break;
					default:
						connected = false;
						break;
				}
			}

			double verteilerVorlauf = connected ? distributor.RegulatorCircuit.CoolFlowTemperature : 16;
			double gesamtSpreizung = 6; // TODO
			vorlauf = verteilerVorlauf;
			ruecklauf = verteilerVorlauf + gesamtSpreizung;
			if (connected && allEuroval) {
				vorlauf = verteilerVorlauf + gesamtSpreizung * eurovalPipeLengthBefore / (eurovalPipeLengthBefore + (this as EurovalProduct).PlannedPipeLength + eurovalPipeLengthAfter);
				ruecklauf = verteilerVorlauf + gesamtSpreizung * (eurovalPipeLengthBefore + (this as EurovalProduct).PlannedPipeLength) / (eurovalPipeLengthBefore + (this as EurovalProduct).PlannedPipeLength + eurovalPipeLengthAfter);
			}
		}

		public abstract bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg);

		internal virtual void FinalizeLoading() {
			// nothing todo
		}
	}
}
