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
		protected bool usedForQuickDimensioning = true;
		protected Room associatedRoom = null;
		protected SerializableDictionary<string, int> quickDimensioningConnectedDistributors = new SerializableDictionary<string, int>();
		protected ProductConnection plannedConnection = null;
		protected SerializableDictionary<int, PlannedProduct> plannedConnectedProducts = new SerializableDictionary<int, PlannedProduct>();
		protected List<ConnectionPipe> plannedConnectionPipes = new List<ConnectionPipe>();

		//protected int plannedCircuits = 1;
		protected List<Circuit> circuits = new List<Circuit>();

		protected string comment = null;

		protected SerializableDictionary<int, Circuit.CircuitConnection> connectedCircuits = new SerializableDictionary<int, Circuit.CircuitConnection>();
		public SerializableDictionary<int, Circuit.CircuitConnection> ConnectedCircuits {
			get { return this.connectedCircuits; }
			set { this.connectedCircuits = value; }
		}

		protected SerializableDictionary<int, Circuit.CircuitConnection> inverseConnectedCircuits = new SerializableDictionary<int, Circuit.CircuitConnection>();
		public SerializableDictionary<int, Circuit.CircuitConnection> InverseConnectedCircuits {
			get { return this.inverseConnectedCircuits; }
			set { this.inverseConnectedCircuits = value; }
		}

		public Circuit.CircuitConnection GetCircuitConnected(int thisCircuit) {
			if (this.connectedCircuits.ContainsKey(thisCircuit)) {
				return this.connectedCircuits[thisCircuit];
			}
			return null;
		}

		public Circuit.CircuitConnection GetCircuitInverseConnected(int thisCircuit) {
			if (this.inverseConnectedCircuits.ContainsKey(thisCircuit)) {
				return this.inverseConnectedCircuits[thisCircuit];
			}
			return null;
		}

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

		#region Quick Dimensioning
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

		public bool UsedForQuickDimensioning {
			get { return usedForQuickDimensioning; }
			set { usedForQuickDimensioning = value; }
		}
		#endregion Quick Dimensioning

		[XmlIgnore]
		public Room AssociatedRoom {
			get { return associatedRoom; }
			set { 
				associatedRoom = value;
				if (this.usedForQuickDimensioning && associatedRoom != null && !associatedRoom.UsedProductsForQuickDimensioning.Contains(this)) {
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

		public abstract float PlannedNetArea {
			get;
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

		[XmlIgnore]
		public double PlannedHeatLoadAnbindung {
			get {
				double value = 0;
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
								if (cp.ConnectionThrough != null && cp.ConnectionThrough.Product != null && cp.ConnectionThrough.Product == this) {
									value += cp.HeatLoadTotal;
								}
							}
						}
					}
				}
				return value;
			}
		}

		public double PlannedHeatLoadIncludingConnectionsThrough {
			get { return this.PlannedHeatLoad + this.PlannedHeatLoadAnbindung; }
		}

		public abstract double PlannedCoolLoad {
			get;
		}

		[XmlIgnore]
		public double PlannedCoolLoadAnbindung {
			get {
				double value = 0;
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
								if (cp.ConnectionThrough != null && cp.ConnectionThrough.Product != null && cp.ConnectionThrough.Product == this) {
									value += cp.CoolLoadTotal;
								}
							}
						}
					}
				}
				return value;
			}
		}

		public double PlannedCoolLoadIncludingConnectionsThrough {
			get { return this.PlannedCoolLoad + this.PlannedCoolLoadAnbindung; }
		}

		public List<ConnectionPipe> PlannedConnectionPipes {
			get { return this.plannedConnectionPipes; }
			set { this.plannedConnectionPipes = value; }
		}

		[XmlIgnore]
		public List<ConnectionPipe> PlannedConnectionPipesThroughThisProduct {
			get {
				List<ConnectionPipe> cps = new List<ConnectionPipe>();
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
								if (cp.ConnectionThrough != null && cp.ConnectionThrough.Product == this) {
									cps.Add(cp);
								}
							}
						}
					}
				}
				return cps;
			}
		}

		[XmlIgnore]
		public int PlannedCircuitCount {
			get { return this.circuits.Count; }
		}

		[XmlIgnore]
		public abstract ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get;
		}

		public void GetHeatFlow(out double vorlauf, out double ruecklauf) {
			if (this.plannedConnection == null) {
				vorlauf = 0;
				ruecklauf = 0;
				return;
			}
			if (this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				this.plannedConnection.OtherProduct.Product.GetHeatFlow(out vorlauf, out ruecklauf);
				return;
			}
			vorlauf = this.plannedConnection.Distributor.RegulatorCircuit.HeatFlowTemperature;
			ruecklauf = vorlauf - EN1264.Instance.DefaultSpreizung(vorlauf);
		}

		public void GetCoolFlow(out double vorlauf, out double ruecklauf) {
			if (this.plannedConnection == null) {
				vorlauf = 0;
				ruecklauf = 0;
				return;
			}
			if (this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				this.plannedConnection.OtherProduct.Product.GetCoolFlow(out vorlauf, out ruecklauf);
				return;
			}
			vorlauf = this.plannedConnection.Distributor.RegulatorCircuit.CoolFlowTemperature;
			ruecklauf = vorlauf + 3;
		}

		public virtual ProductConnection PlannedConnection {
			get { return this.plannedConnection; }
			set { this.plannedConnection = value; }
		}

		public SerializableDictionary<int, string> PlannedConnectedProductIds {
			get {
				return null;
				// TODO
			}
			set {
				// TODO
			}
		}

		public List<Circuit> PlannedCircuits {
			get {
				return this.circuits;
			}
			set {
				this.circuits = value;
			}
		}

		//public abstract int GetIndexOfCircuit(Circuit c);

		public abstract Circuit GetCircuit(int index);

		[XmlIgnore]
		public SerializableDictionary<int, PlannedProduct> PlannedConnectedProducts {
			get { return this.plannedConnectedProducts; }
		}

		public abstract bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg);

		internal virtual void FinalizeLoading() {
			// nothing todo
		}
	}
}
