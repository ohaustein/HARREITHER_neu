using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;

namespace Europlan.Common {

	[XmlInclude(typeof(EurovalProduct))]
	[XmlInclude(typeof(EcothermProduct))]
	[XmlInclude(typeof(ConcreteActivationProduct))]
	[XmlInclude(typeof(HithermProduct))]
	[XmlInclude(typeof(HithermCompactProduct))]
	[XmlInclude(typeof(ModulKlimaBodenProduct))]
	[XmlInclude(typeof(ModulKlimaDeckeProduct))]
	[Serializable()]
	public abstract class Product : IRequiredMaterial {

		public static readonly double rundrohr21mmAussenD = 0.021;
		public static readonly double rundrohr21mmInnenD = 0.0162;
		public static readonly double rundrohr21mmInnenA = (rundrohr21mmInnenD / 2) * (rundrohr21mmInnenD / 2) * Math.PI;

		private static double alphaDeckeHeat = 6.5;
		private static double alphaBodenHeat = 10.8;
		private static double alphaWandHeat = 8.0;
		private static double alphaDeckeCool = 10.8;
		private static double alphaBodenCool = 6.5;
		private static double alphaWandCool = 8.0;

		protected double requestedHeatLoad = 0;
		protected double requestedCoolLoad = 0;

		#region Product Parameters
		[ProductParameter]
		public static double ConfigAlphaDeckeHeat {
			get { return alphaDeckeHeat; }
			set { alphaDeckeHeat = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaBodenHeat {
			get { return alphaBodenHeat; }
			set { alphaBodenHeat = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaWandHeat {
			get { return alphaWandHeat; }
			set { alphaWandHeat = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaDeckeCool {
			get { return alphaDeckeCool; }
			set { alphaDeckeCool = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaBodenCool {
			get { return alphaBodenCool; }
			set { alphaBodenCool = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaWandCool {
			get { return alphaWandCool; }
			set { alphaWandCool = value; }
		}
		#endregion Product Parameters

		public class ProductTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string dh = "Decke";
			private static readonly string fbh = "Boden";
			private static readonly string rest = "Rest";
			private static readonly string wh = "Wand";
			private static readonly string dsh = "Dachschräge";

			private Dictionary<string, ProductType> mappingFromString = new Dictionary<string, ProductType>();
			private Dictionary<ProductType, string> mappingToString = new Dictionary<ProductType, string>();

			public ProductTypeEnumConverter() {
				mappingFromString.Add(dh, ProductType.DH);
				mappingFromString.Add(fbh, ProductType.FBH);
				mappingFromString.Add(rest, ProductType.REST);
				mappingFromString.Add(wh, ProductType.WH);
				mappingFromString.Add(dsh, ProductType.DSH);
				mappingToString.Add(ProductType.DH, dh);
				mappingToString.Add(ProductType.FBH, fbh);
				mappingToString.Add(ProductType.REST, rest);
				mappingToString.Add(ProductType.WH, wh);
				mappingToString.Add(ProductType.DSH, dsh);
			}

			public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) {
				return sourceType == typeof(string);
			}

			public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType) {
				return destinationType == typeof(string);
			}

			public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
				if (value is string) {
					if (mappingFromString.ContainsKey((string)value)) {
						return mappingFromString[(string)value];
					}
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
				if (value is ProductType && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((ProductType)value)) {
						return mappingToString[(ProductType)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(ProductTypeEnumConverter))]
		public enum ProductType {
			FBH,
			WH,
			DH,
			DSH,
			REST
		}

		protected int quickDimensioningCircuits = 0;
		protected string quickDimensioningCircuitsAsString = null;
		protected float quickDimensioningPlannedArea = 0;
		protected bool usedForQuickDimensioning = true;
		protected Room associatedRoom = null;
		protected SerializableDictionary<string, int> quickDimensioningConnectedDistributors = new SerializableDictionary<string, int>();
		protected ProductConnection plannedConnection = null;
		//protected SerializableDictionary<int, PlannedProduct> plannedConnectedProducts = new SerializableDictionary<int, PlannedProduct>();
		protected List<ConnectionPipe> plannedConnectionPipes = new List<ConnectionPipe>();

		protected float plannedRoomTemperatureBelowHeat = 18;
		protected float plannedRoomTemperatureBelowCool = 28;

		protected double plannedVorlaufTempHeat = double.MinValue;
		protected double plannedRuecklaufTempHeat = double.MinValue;
		protected double plannedVorlaufTempCool = double.MinValue;
		protected double plannedRuecklaufTempCool = double.MinValue;

		protected bool plannedProductIsConnection = false;

		protected bool incompleteCalculation = true;

		protected string lastErrorMsg = null;

		//protected int plannedCircuits = 1;
		protected List<Circuit> circuits = new List<Circuit>();

		protected bool stellMotore = false;

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

		public static void StaticInitialize() {
			Configuration userConfig = Configuration.UserTemplate;
			Product.alphaBodenHeat = userConfig.GetProductParameterAsDouble<Product>("ConfigAlphaBodenHeat", 10.8);
			Product.alphaDeckeHeat = userConfig.GetProductParameterAsDouble<Product>("ConfigAlphaDeckeHeat", 6.5);
			Product.alphaWandHeat = userConfig.GetProductParameterAsDouble<Product>("ConfigAlphaWandHeat", 8.0);
			Product.alphaBodenCool = userConfig.GetProductParameterAsDouble<Product>("ConfigAlphaBodenCool", 6.5);
			Product.alphaDeckeCool = userConfig.GetProductParameterAsDouble<Product>("ConfigAlphaDeckeCool", 10.8);
			Product.alphaWandCool = userConfig.GetProductParameterAsDouble<Product>("ConfigAlphaWandCool", 8.0);
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
		//public abstract void StaticInitialize();
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

		public string Name {
			get {
				object[] attributes = this.GetType().GetCustomAttributes(typeof(ProductNameAttribute), true);

				if (attributes.Length > 0) {
					return (attributes[0] as ProductNameAttribute).Name;
				} else {
					return this.GetType().Name;
				}
			}
		}

		public abstract string QuickDimensioningName {
			get;
		}

		public string FullName {
			get {
				object[] attributes = this.GetType().GetCustomAttributes(typeof(ProductNameAttribute), true);

				if (attributes.Length > 0) {
					return (attributes[0] as ProductNameAttribute).FullName;
				} else {
					return this.GetType().Name;
				}
			}
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

		public abstract double Dichte {
			get;
		}

		public abstract double Waermekapazitaet {
			get;
		}

		public abstract double Viskositaet {
			get;
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

		public abstract float PlannedRoofArea {
			get;
			set;
		}

		public float TotalPlannedArea {
			get { return this.PlannedFloorArea + this.PlannedCeilingArea + this.PlannedWallArea + this.PlannedRoofArea; }
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

		[XmlIgnore]
		public double PlannedVorlaufTempHeat {
			get { return this.plannedVorlaufTempHeat; }
		}

		[XmlIgnore]
		public double PlannedRuecklaufTempHeat {
			get { return this.plannedRuecklaufTempHeat; }
		}

		[XmlIgnore]
		public double PlannedVorlaufTempCool {
			get { return this.plannedVorlaufTempCool; }
		}

		[XmlIgnore]
		public double PlannedruecklaufTempCool {
			get { return this.plannedRuecklaufTempCool; }
		}

		public virtual void CalculateHeatAndCoolFlow() {
			if (this.plannedConnection == null) {
				this.plannedVorlaufTempHeat = 0;
				this.plannedRuecklaufTempHeat = 0;
				this.plannedVorlaufTempCool = 0;
				this.plannedRuecklaufTempCool = 0;
				return;
			}
			double spreizungHeat = 0;
			double spreizungCool = 0;
			if (this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				this.plannedConnection.OtherProduct.Product.CalculateHeatAndCoolFlow();
				this.plannedConnection.OtherProduct.Product.GetHeatFlow(out this.plannedVorlaufTempHeat, out this.plannedRuecklaufTempHeat);
				spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
				this.plannedConnection.OtherProduct.Product.GetCoolFlow(out this.plannedVorlaufTempCool, out this.plannedRuecklaufTempCool);
				spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			} else {
				this.plannedVorlaufTempHeat = this.plannedConnection.Distributor.RegulatorCircuit.HeatFlowTemperature;
				spreizungHeat = EN1264.Instance.DefaultSpreizung(this.plannedVorlaufTempHeat);
				this.plannedRuecklaufTempHeat = this.plannedVorlaufTempHeat - spreizungHeat;
				this.plannedVorlaufTempCool = this.plannedConnection.Distributor.RegulatorCircuit.CoolFlowTemperature;
				spreizungCool = 4;
				this.plannedRuecklaufTempCool = this.plannedVorlaufTempCool + spreizungCool;
			}
		}

		public void GetHeatFlow(out double vorlauf, out double ruecklauf) {
			/*if (this.plannedConnection == null) {
				vorlauf = 0;
				ruecklauf = 0;
				return;
			}
			if (this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				this.plannedConnection.OtherProduct.Product.GetHeatFlow(out vorlauf, out ruecklauf);
				return;
			}
			vorlauf = this.plannedConnection.Distributor.RegulatorCircuit.HeatFlowTemperature;
			ruecklauf = vorlauf - EN1264.Instance.DefaultSpreizung(vorlauf);*/
			vorlauf = this.plannedVorlaufTempHeat;
			ruecklauf = this.plannedRuecklaufTempHeat;
		}

		public void GetCoolFlow(out double vorlauf, out double ruecklauf) {
			/*if (this.plannedConnection == null) {
				vorlauf = 0;
				ruecklauf = 0;
				return;
			}
			if (this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				this.plannedConnection.OtherProduct.Product.GetCoolFlow(out vorlauf, out ruecklauf);
				return;
			}
			vorlauf = this.plannedConnection.Distributor.RegulatorCircuit.CoolFlowTemperature;
			ruecklauf = vorlauf + 3;*/
			vorlauf = this.plannedVorlaufTempCool;
			ruecklauf = this.plannedRuecklaufTempCool;
		}

		public virtual ProductConnection PlannedConnection {
			get { return this.plannedConnection; }
			set { this.plannedConnection = value; }
		}

		/*public SerializableDictionary<int, string> PlannedConnectedProductIds {
			get {
				return null;
				// TODO
			}
			set {
				// TODO
			}
		}*/

		public List<Circuit> PlannedCircuits {
			get {
				return this.circuits;
			}
			set {
				this.circuits = value;
			}
		}

		[XmlIgnore]
		public List<Product> PlannedConnectedProducts {
			get {
				List<Product> pp = new List<Product>();
				foreach (ConnectionPipe cp in this.plannedConnectionPipes) {
					if (cp.ConnectionThrough != null && cp.ConnectionThrough.Product != null && cp.ConnectionThrough.Product.PlannedProductIsConnection && !pp.Contains(cp.ConnectionThrough.Product)) {
						pp.Add(cp.ConnectionThrough.Product);
					}
				}
				foreach (Circuit.CircuitConnection cc in this.connectedCircuits.Values) {
					if (!pp.Contains(cc.OtherProduct)) {
						pp.Add(cc.OtherProduct);
					}
				}
				return pp;
			}
		}

		[XmlIgnore]
		public bool PlannedCalculationComplete {
			get { return !this.incompleteCalculation; }
		}

		[XmlIgnore]
		public double PlannedSpreizungHeat {
			get {
				/*if (this.PlannedConnection == null) {
					return 0;
				}
				if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
					return this.PlannedConnection.Distributor.RegulatorCircuit == null ? 0 : EN1264.Instance.DefaultSpreizung(this.PlannedConnection.Distributor.RegulatorCircuit.HeatFlowTemperature);
				} else if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					return this.PlannedConnection.OtherProduct.Product.PlannedSpreizungHeat;
				}
				return (this.PlannedConnection == null || this.PlannedConnection.Distributor == null || this.PlannedConnection.Distributor.RegulatorCircuit == null) ? 0 : EN1264.Instance.DefaultSpreizung(this.PlannedConnection.Distributor.RegulatorCircuit.HeatFlowTemperature);*/
				return this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			}
		}

		[XmlIgnore]
		public double PlannedSpreizungCool {
			get {
				//return 3; // TODO
				return this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
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
		/// The pressure loss for heating at the distributor, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedDeltaRhoDistributorHeat {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					double druckverlust = c.C_DruckverlustDistributorHeat;
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

		/// <summary>
		/// The pressure loss for cooling at the distributor, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedDeltaRhoDistributorCool {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					double druckverlust = c.C_DruckverlustDistributorCool;
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
					value += c.C_MassenstromHeat;
				}
				return value;
			}
		}
		public double PlannedDurchflussHeat {
			get { return PlannedMaxMhHeat * 1000 / Dichte; }
		}

		[XmlIgnore]
		public double PlannedMhCool {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					value += c.C_MassenstromCool;
				}
				return value;
			}
		}
		public double PlannedDurchflussCool {
			get { return PlannedMhCool * 1000 / Dichte; }
		}

		[XmlIgnore]
		public double PlannedMaxMhHeat {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					if (c.C_MassenstromHeat > value) {
						value = c.C_MassenstromHeat;
					}
				}
				return value;
			}
		}
		public double PlannedMaxDurchflussHeat {
			get { return PlannedMaxMhHeat * 1000 / Dichte; }
		}

		[XmlIgnore]
		public double PlannedMaxMhCool {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (Circuit c in this.circuits) {
					if (c.C_MassenstromCool > value) {
						value = c.C_MassenstromCool;
					}
				}
				return value;
			}
		}
		public double PlannedMaxDurchflussCool {
			get { return PlannedMaxMhCool * 1000 / Dichte; }
		}

		public abstract bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung);

		internal virtual void FinalizeLoading(PlannedProduct pp) {
			foreach (Circuit c in this.circuits) {
				c.FinalizeLoading();
			}
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

		public bool StellMotore {
			get { return stellMotore; }
			set { stellMotore = value; }
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
		public abstract double WasserInhalt {
			get;
		}

		[XmlIgnore]
		public abstract Construction PlannedOutsideConstruction {
			get;
		}

		private double QH2OHeat {
			get {
				double qH2OHeat = 0;
				foreach (Circuit c in this.circuits) {
					qH2OHeat += Math.Abs(c.C_Qh2oHeat);
				}
				return qH2OHeat;
			}
		}

		private double QH2OCool {
			get {
				double qH2OCool = 0;
				foreach (Circuit c in this.circuits) {
					qH2OCool += Math.Abs(c.C_Qh2oCool);
				}
				return qH2OCool;
			}
		}

		public double TransmissionFloorHeat {
			get {
				if (this.Type == ProductType.FBH) {
					return QH2OHeat - this.PlannedHeatLoad;
				}
				return 0; 
			}
		}

		public double TransmissionWallHeat {
			get {
				if (this.Type == ProductType.WH) {
					return QH2OHeat - this.PlannedHeatLoad;
				}
				return 0;
			}
		}

		public double TransmissionCeilingHeat {
			get {
				if (this.Type == ProductType.DH) {
					return QH2OHeat - this.PlannedHeatLoad;
				}
				return 0;
			}
		}

		public double TransmissionRoofHeat {
			get {
				if (this.Type == ProductType.DSH) {
					return QH2OHeat - this.PlannedHeatLoad;
				}
				return 0;
			}
		}

		public double TransmissionFloorCool {
			get {
				if (this.Type == ProductType.FBH) {
					return QH2OCool - this.PlannedCoolLoad;
				}
				return 0;
			}
		}

		public double TransmissionWallCool {
			get {
				if (this.Type == ProductType.WH) {
					return QH2OCool - this.PlannedCoolLoad;
				}
				return 0;
			}
		}

		public double TransmissionCeilingCool {
			get {
				if (this.Type == ProductType.DH) {
					return QH2OCool - this.PlannedCoolLoad;
				}
				return 0;
			}
		}

		public double TransmissionRoofCool {
			get {
				if (this.Type == ProductType.DSH) {
					return QH2OCool - this.PlannedCoolLoad;
				}
				return 0;
			}
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

		public abstract void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial);

		[XmlIgnore]
		public string LastErrorMessage {
			get { return this.lastErrorMsg; }
		}

		[XmlIgnore]
		public bool IsOtherProductConnected {
			get {
				foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.connectedCircuits) {
					if (kvp.Value != null) {
						return true;
					}
				}
				return false;
			}
		}

		public bool PlannedProductIsConnection {
			get { return this.plannedProductIsConnection; }
			set { this.plannedProductIsConnection = value; }
		}

		[XmlIgnore]
		public virtual double PlannedHeizlastBereinigung {
			get { return 0; }
		}

		[XmlIgnore]
		public virtual double PlannedKuehllastBereinigung {
			get { return 0; }
		}

		protected static double[] ConvertStringToArray(string value) {
			string str = value.Trim();
			if (!str.StartsWith("{") || !str.EndsWith("}")) {
				// log warning
				return null;
			}
			List<double> list = new List<double>();
			string[] strValues = str.Substring(1, str.Length - 2).Trim().Split(',');
			foreach (string strValue in strValues) {
				double doubleValue;
				if (!double.TryParse(strValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out doubleValue)) {
					// log warning
					return null;
				}
				list.Add(doubleValue);
			}
			double[] array = new double[list.Count];
			int j = 0;
			foreach (double doubleValue in list) {
				array[j] = doubleValue;
				j++;
			}
			return array;
		}

		protected static string ConvertArrayToString(double[] array) {
			string str = "";
			foreach (double val in array) {
				str += ", " + val.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
			}
			str = str.Substring(2);
			string rtn = "{" + str + "}";
			return rtn;
		}

		protected static double[][] ConvertStringToArray2(string value) {
			string str = value.Trim();
			if (!str.StartsWith("{") || !str.EndsWith("}")) {
				// log warning
				return null;
			}
			str = str.Substring(1, str.Length - 2).Trim();
			List<List<double>> list = new List<List<double>>();
			while (str.Length > 0) {
				int end = str.IndexOf('}');
				if (str[0] != '{' || end < 0) {
					// log warning
					return null;
				}
				List<double> curList = new List<double>();
				list.Add(curList);
				string[] strValues = str.Substring(1, end - 1).Trim().Split(',');
				foreach (string strValue in strValues) {
					double doubleValue;
					if (!double.TryParse(strValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out doubleValue)) {
						// log warning
						return null;
					}
					curList.Add(doubleValue);
				}
				str = str.Substring(end + 1).Trim();
				if (str.Length != 0) {
					if (str[0] != ',') {
						// log warning
						return null;
					}
					str = str.Substring(1);
				}
			}
			double[][] array = new double[list.Count][];
			int i = 0;
			foreach (List<double> curList in list) {
				array[i] = new double[curList.Count];
				int j = 0;
				foreach (double doubleValue in curList) {
					array[i][j] = doubleValue;
					j++;
				}
				i++;
			}
			return array;
		}

		protected static string ConvertArrayToString2(double[][] array) {
			string rtn = "";
			foreach (double[] row in array) {
				string rowStr = "";
				foreach (double val in row) {
					rowStr += ", " + val.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
				}
				rowStr = rowStr.Substring(2);
				rtn += " ,{" + rowStr + "}";
			}
			rtn = "{" + rtn.Substring(2) + "}";
			return rtn;
		}

		public virtual string NotificationMessage {
			get {
				double coolLoad = Math.Round(this.PlannedCoolLoad / this.PlannedNetArea, 1);
				if (coolLoad > 75) {
					return "Bei der aktuell berechneten Betriebsweise wird eine Entfeuchtung empfohlen, da die Kühlleistung 75W/m² übersteigt.";
				}
				return null;
			}
		}
	}
}
