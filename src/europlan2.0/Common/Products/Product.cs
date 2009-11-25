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

		public static readonly double rundrohr21mmAussenD = 0.021;
		public static readonly double rundrohr21mmInnenD = 0.0162;
		public static readonly double rundrohr21mmInnenA = (rundrohr21mmInnenD / 2) * (rundrohr21mmInnenD / 2) * Math.PI;

		public enum ProductType {
			FBH,
			WH,
			DH,
			REST
		}

		protected int quickDimensioningCircuits = 0;
		protected string quickDimensioningCircuitsAsString = null;
		protected float quickDimensioningPlannedArea = 0;
		protected bool usedForQuickDimensioning = true;
		protected Room associatedRoom = null;
		protected SerializableDictionary<string, int> quickDimensioningConnectedDistributors = new SerializableDictionary<string, int>();
		protected ProductConnection plannedConnection = null;
		protected SerializableDictionary<int, PlannedProduct> plannedConnectedProducts = new SerializableDictionary<int, PlannedProduct>();
		protected List<ConnectionPipe> plannedConnectionPipes = new List<ConnectionPipe>();

		protected float plannedRoomTemperatureBelowHeat = 18;
		protected float plannedRoomTemperatureBelowCool = 22;

		protected bool incompleteCalculation = true;

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
			this.quickDimensioningCircuits = product.quickDimensioningCircuits;
			this.quickDimensioningCircuitsAsString = product.quickDimensioningCircuitsAsString;
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
		public abstract bool QuickDimensioningCanHeat {
			get;
		}

		public abstract bool QuickDimensioningCanCool {
			get;
		}

		public abstract int QuickDimensioningHeatPowerPerSquareMeter {
			get;
		}

		public abstract int QuickDimensioningCoolPowerPerSquareMeter {
			get;
		}

		public int QuickDimensioningHeatPower {
			get {
				if (QuickDimensioningCanHeat) {
					return (int)(QuickDimensioningPlannedArea * QuickDimensioningHeatPowerPerSquareMeter);
				}
				return 0; 
			}
		}
		
		public int QuickDimensioningCoolPower {
			get {
				if (QuickDimensioningCanCool) {
					return (int)(QuickDimensioningPlannedArea * QuickDimensioningCoolPowerPerSquareMeter);
				}
				return 0; 
			}
		}

		//public int QuickDimensioningHeatPowerPerSquareMeter {
		//    get { return quickDimensioningHeatPowerPerSquareMeter; }
		//    set { quickDimensioningHeatPowerPerSquareMeter = value; }
		//}

		//public int QuickDimensioningCoolPowerPerSquareMeter {
		//    get { return quickDimensioningCoolPowerPerSquareMeter; }
		//    set { quickDimensioningCoolPowerPerSquareMeter = value; }
		//}

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

		//public bool CanHeat {
		//    get { return canHeat; }
		//    set { canHeat = value; }
		//}

		//public bool CanCool {
		//    get { return canCool; }
		//    set { canCool = value; }
		//}

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

		public float AvailableCeilingArea {
			get {
				Room room = this.AssociatedRoom;
				float area = room.Area;
				foreach (PlannedProduct product in room.PlannedProducts) {
					if (product.Product != this) {
						area -= product.Product.PlannedCeilingArea;
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

		public abstract float PlannedCeilingArea {
			get;
			set;
		}

		public abstract float PlannedWallArea {
			get;
			set;
		}

		public float TotalPlannedArea {
			get { return this.PlannedFloorArea + this.PlannedCeilingArea + this.PlannedWallArea; }
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

		[XmlIgnore]
		public double PlannedRemoveArea {
			get {
				double value = 0;
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
								if (cp.ConnectionThrough != null && cp.ConnectionThrough.Product != null && cp.ConnectionThrough.Product == this) {
									value += cp.AreaTotal;
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

		[XmlIgnore]
		public SerializableDictionary<int, PlannedProduct> PlannedConnectedProducts {
			get { return this.plannedConnectedProducts; }
		}

		[XmlIgnore]
		public bool PlannedCalculationComplete {
			get { return !this.incompleteCalculation; }
		}

		[XmlIgnore]
		public double PlannedSpreizungHeat {
			get {
				if (this.PlannedConnection == null) {
					return 0;
				}
				if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
					return this.PlannedConnection.Distributor.RegulatorCircuit == null ? 0 : EN1264.Instance.DefaultSpreizung(this.PlannedConnection.Distributor.RegulatorCircuit.HeatFlowTemperature);
				} else if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					return this.PlannedConnection.OtherProduct.Product.PlannedSpreizungHeat;
				}
				return (this.PlannedConnection == null || this.PlannedConnection.Distributor == null || this.PlannedConnection.Distributor.RegulatorCircuit == null) ? 0 : EN1264.Instance.DefaultSpreizung(this.PlannedConnection.Distributor.RegulatorCircuit.HeatFlowTemperature);
			}
		}

		[XmlIgnore]
		public double PlannedSpreizungCool {
			get {
				return 3; // TODO
			}
		}
		
		/// <summary>
		/// The pressure loss for heating, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedDeltaRhoHeat {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					double druckverlust = c.C_DruckverlustHeat;
					Circuit.CircuitConnection cc = this.GetCircuitConnected(c.NrOfCircuit);
					if (cc != null) {
						druckverlust += cc.OtherCircuit.C_DruckverlustHeat;
					}
					cc = this.GetCircuitInverseConnected(c.NrOfCircuit);
					if (cc != null) {
						druckverlust += cc.OtherCircuit.C_DruckverlustHeat;
					}
					if (druckverlust > value) {
						value = druckverlust;
					}
				}
				return value;
			}
		}

		/// <summary>
		/// The pressure loss for cooling, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedDeltaRhoCool {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					double druckverlust = c.C_DruckverlustCool;
					Circuit.CircuitConnection cc = this.GetCircuitConnected(c.NrOfCircuit);
					if (cc != null) {
						druckverlust += cc.OtherCircuit.C_DruckverlustCool;
					}
					cc = this.GetCircuitInverseConnected(c.NrOfCircuit);
					if (cc != null) {
						druckverlust += cc.OtherCircuit.C_DruckverlustCool;
					}
					if (druckverlust > value) {
						value = druckverlust;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedMhHeat {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					if (c.C_DurchflussHeat > value) {
						value = c.C_DurchflussHeat;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedMhCool {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					if (c.C_DurchflussCool > value) {
						value = c.C_DurchflussCool;
					}
				}
				return value;
			}
		}

		public abstract bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg);

		internal virtual void FinalizeLoading() {
			// nothing todo
		}

		protected void CalculateVorlaufRuecklauf(out double[] vorlaufTotal, out double[] vorlaufNotIsolated, out double[] ruecklaufTotal, out double[] ruecklaufNotIsolated, out double[] vorlaufWithoutOtherProductTotal, out double[] vorlaufWithoutOtherProductNotIsolated, out double[] ruecklaufWithoutOtherProductTotal, out double[] ruecklaufWithoutOtherProductNotIsolated, out double longestVorlaufTotal, out double longestRuecklaufTotal) {
			// Get Vorlauf and Ruecklauf of the defined connection pipes
			double vorlaufTotalFirst = 0;
			double vorlaufNotIsolatedFirst = 0;
			double ruecklaufTotalFirst = 0;
			double ruecklaufNotIsolatedFirst = 0;
			double vorlaufTotalOthers = 0;
			double vorlaufNotIsolatedOthers = 0;
			double ruecklaufTotalOthers = 0;
			double ruecklaufNotIsolatedOthers = 0;
			foreach (ConnectionPipe cp in this.PlannedConnectionPipes) {
				vorlaufTotalFirst += cp.Vorlauf;
				ruecklaufTotalFirst += cp.Ruecklauf;
				if (cp.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) {
					vorlaufNotIsolatedFirst += cp.Vorlauf;
				}
				if (cp.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) {
					ruecklaufNotIsolatedFirst += cp.Ruecklauf;
				}
				if (!cp.OnlyFirst) {
					vorlaufTotalOthers += cp.Vorlauf;
					ruecklaufTotalOthers += cp.Ruecklauf;
					if (cp.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) {
						vorlaufNotIsolatedOthers += cp.Vorlauf;
					}
					if (cp.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) {
						ruecklaufNotIsolatedOthers += cp.Ruecklauf;
					}
				}
			}

			// add connected products to Vorlauf and Ruecklauf
			vorlaufTotal = new double[] { vorlaufTotalFirst, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers };
			vorlaufNotIsolated = new double[] { vorlaufNotIsolatedFirst, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers };
			ruecklaufTotal = new double[] { ruecklaufTotalFirst, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers };
			ruecklaufNotIsolated = new double[] { ruecklaufNotIsolatedFirst, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers };
			vorlaufWithoutOtherProductTotal = new double[] { vorlaufTotalFirst, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers };
			vorlaufWithoutOtherProductNotIsolated = new double[] { vorlaufNotIsolatedFirst, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers };
			ruecklaufWithoutOtherProductTotal = new double[] { ruecklaufTotalFirst, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers };
			ruecklaufWithoutOtherProductNotIsolated = new double[] { ruecklaufNotIsolatedFirst, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers };

			foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.connectedCircuits) {
				if (kvp.Value != null) {
					if (kvp.Value.CircuitConnectionType == Circuit.CircuitConnectionTypeEnum.VORLAUF) {
						vorlaufTotal[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthWithoutOtherProduct;
						vorlaufNotIsolated[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthWithoutOtherProductNotIsolated;
					} else {
						ruecklaufTotal[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthWithoutOtherProduct;
						ruecklaufNotIsolated[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthWithoutOtherProductNotIsolated;
					}
				}
			}

			foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.inverseConnectedCircuits) {
				if (kvp.Value != null) {
					if (kvp.Value.CircuitConnectionType == Circuit.CircuitConnectionTypeEnum.VORLAUF) {
						ruecklaufTotal[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthWithoutOtherProduct - kvp.Value.OtherCircuit.PipeLengthVorlaufWithoutOtherProductTotal;
						ruecklaufNotIsolated[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthWithoutOtherProductNotIsolated - kvp.Value.OtherCircuit.PipeLengthVorlaufWithoutOtherProductNotIsolated;
						vorlaufTotal[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthVorlaufWithoutOtherProductTotal;
						vorlaufNotIsolated[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthVorlaufWithoutOtherProductNotIsolated;
					} else {
						vorlaufTotal[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthWithoutOtherProduct - kvp.Value.OtherCircuit.PipeLengthRuecklaufWithoutOtherProductTotal;
						vorlaufNotIsolated[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthWithoutOtherProductNotIsolated - kvp.Value.OtherCircuit.PipeLengthRuecklaufWithoutOtherProductNotIsolated;
						ruecklaufTotal[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthRuecklaufWithoutOtherProductTotal;
						ruecklaufNotIsolated[kvp.Key] += kvp.Value.OtherCircuit.PipeLengthRuecklaufWithoutOtherProductNotIsolated;
					}
				}
			}

			longestVorlaufTotal = vorlaufTotal[0];
			longestRuecklaufTotal = ruecklaufTotal[0];
			for (int i = 1; i < 12; i++) {
				if (vorlaufTotal[i] > longestVorlaufTotal) {
					longestVorlaufTotal = vorlaufTotal[i];
				}
				if (ruecklaufTotal[i] > longestRuecklaufTotal) {
					longestRuecklaufTotal = ruecklaufTotal[i];
				}
			}
		}

		public Circuit GetCircuit(int index) {
			if (index < this.circuits.Count) {
				return this.circuits[index];
			}
			return null;
		}

		/// <summary>
		/// The r-value of the planned construction to the inside of the room
		/// </summary>
		[XmlIgnore]
		public abstract float PlannedInsideConstructionRValue {
			get;
		}

		[XmlIgnore]
		public abstract bool HasInsideConstruction {
			get;
		}

		[XmlIgnore]
		public abstract Construction PlannedInsideConstruction {
			get;
		}

		/// <summary>
		/// The r-value of the planned construction to the outside of the room
		/// </summary>
		[XmlIgnore]
		public abstract float PlannedOutsideConstructionRValue {
			get;
		}

		[XmlIgnore]
		public abstract bool HasOutsideConstruction {
			get;
		}

		[XmlIgnore]
		public abstract Construction PlannedOutsideConstruction {
			get;
		}

		/// <summary>
		/// The temperature of the room below used for the heating calcuation
		/// </summary>
		public float PlannedRoomTemperatureBelowHeat {
			get { return this.plannedRoomTemperatureBelowHeat; }
			set { this.plannedRoomTemperatureBelowHeat = value; }
		}

		/// <summary>
		/// The temperature of the room below used for the cooling calculation
		/// </summary>
		public float PlannedRoomTemperatureBelowCool {
			get { return this.plannedRoomTemperatureBelowCool; }
			set { this.plannedRoomTemperatureBelowCool = value; }
		}
	}
}
