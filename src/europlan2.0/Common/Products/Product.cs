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
		private Distributor plannedConnectedDistributor = null;
		private string plannedConnectedDistributorId = null;

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

		public string PlannedConnectedDistributorId {
			get {
				if (this.PlannedConnectedDistributor == null) {
					return null;
				}
				return this.plannedConnectedDistributor.Id;
			}
			set { this.plannedConnectedDistributorId = value; }
		}

		[XmlIgnore]
		public Distributor PlannedConnectedDistributor {
			get {
				if (this.plannedConnectedDistributorId != null) {
					// find correct distributor first
					this.plannedConnectedDistributor = Project.Instance.GetDistributor(this.plannedConnectedDistributorId);
					this.plannedConnectedDistributorId = null;
				}
				return this.plannedConnectedDistributor;
			}
			set {
				if (this.plannedConnectedDistributor != null && this.plannedConnectedDistributor.PlannedConnectedProducts.Contains(this)) {
					this.plannedConnectedDistributor.PlannedConnectedProducts.Remove(this);
				}
				this.plannedConnectedDistributor = value;
				this.plannedConnectedDistributorId = null;
				if (this.plannedConnectedDistributor != null && !this.plannedConnectedDistributor.PlannedConnectedProducts.Contains(this)) {
					this.plannedConnectedDistributor.PlannedConnectedProducts.Add(this);
				}
			}
		}

		public abstract void ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool);

		internal virtual void FinalizeLoading() {
			if (this.plannedConnectedDistributorId != null) {
				this.PlannedConnectedDistributor = Project.Instance.GetDistributor(this.plannedConnectedDistributorId);
			}
		}
	}
}
