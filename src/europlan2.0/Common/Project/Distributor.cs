using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	public class Distributor : IGuiRepresentation, IRequiredMaterial {

#region enums

		//public class DistributorTypeEnumConverter : System.ComponentModel.TypeConverter {
	
		//    private static readonly string durchflussmengenregler = "Harreither Systemverteiler mit Durchflußmengenregler";
		//    private static readonly string ruecklaufventil = "Harreither Systemverteiler mit Rücklaufventil";

		//    private Dictionary<string, DistributorTypeEnum> mappingFromString = new Dictionary<string, DistributorTypeEnum>();
		//    private Dictionary<DistributorTypeEnum, string> mappingToString = new Dictionary<DistributorTypeEnum, string>();

		//    public DistributorTypeEnumConverter() {
		//        mappingFromString.Add(durchflussmengenregler, DistributorTypeEnum.Durchflussmengenregler);
		//        mappingFromString.Add(ruecklaufventil, DistributorTypeEnum.Ruecklaufventil);
		//        mappingToString.Add(DistributorTypeEnum.Durchflussmengenregler, durchflussmengenregler);
		//        mappingToString.Add(DistributorTypeEnum.Ruecklaufventil, ruecklaufventil);
		//    }

		//    public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) {
		//        return sourceType == typeof(string);
		//    }

		//    public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType) {
		//        return destinationType == typeof(string);
		//    }

		//    public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
		//        if (value is string) {
		//            if (mappingFromString.ContainsKey((string)value)) {
		//                return mappingFromString[(string)value];
		//            }
		//        }
		//        return base.ConvertFrom(context, culture, value);
		//    }

		//    public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
		//        if (value is DistributorTypeEnum && destinationType == typeof(string)) {
		//            if (mappingToString.ContainsKey((DistributorTypeEnum)value)) {
		//                return mappingToString[(DistributorTypeEnum)value];
		//            }
		//        }
		//        return base.ConvertTo(context, culture, value, destinationType);
		//    }
		//}

		//[System.ComponentModel.TypeConverter(typeof(DistributorTypeEnumConverter))]
		//public enum DistributorTypeEnum {
		//    Durchflussmengenregler,
		//    Ruecklaufventil
		//}

		public class AnschlussHollaenderEnumConverter : System.ComponentModel.TypeConverter {

			private static readonly string kein = EuroplanRes.Distributor_KeinHollaender;
			private static readonly string hollaender32 = EuroplanRes.Distributor_Hollaender32mm;
			private static readonly string hollaenderIG = EuroplanRes.Distributor_HollaenderIg;
			//private static readonly string hollaenderAG = "Anschlußholländer mit Anschlußstück 1\" AG";

			private Dictionary<string,  AnschlussHollaenderEnum> mappingFromString = new Dictionary<string,  AnschlussHollaenderEnum>();
			private Dictionary< AnschlussHollaenderEnum, string> mappingToString = new Dictionary< AnschlussHollaenderEnum, string>();

			public AnschlussHollaenderEnumConverter() {
				mappingFromString.Add(kein, AnschlussHollaenderEnum.Kein);
				mappingFromString.Add(hollaender32, AnschlussHollaenderEnum.hollaender32);
				mappingFromString.Add(hollaenderIG, AnschlussHollaenderEnum.hollaenderIG);
				//mappingFromString.Add(hollaenderAG, AnschlussHollaenderEnum.hollaenderAG);
				mappingToString.Add(AnschlussHollaenderEnum.Kein, kein);
				mappingToString.Add(AnschlussHollaenderEnum.hollaender32, hollaender32);
				mappingToString.Add(AnschlussHollaenderEnum.hollaenderIG, hollaenderIG);
				//mappingToString.Add(AnschlussHollaenderEnum.hollaenderAG, hollaenderAG);
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
				if (value is  AnschlussHollaenderEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey(( AnschlussHollaenderEnum)value)) {
						return mappingToString[( AnschlussHollaenderEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(AnschlussHollaenderEnumConverter))]
		public enum AnschlussHollaenderEnum {
			Kein,
			hollaender32,
			hollaenderIG
			//hollaenderAG
		}

#endregion

		private string id;
		private string name;
		private string regulatorCircuitId = null;
		private RegulatorCircuit regulatorCircuit = null;
		//private DistributorTypeEnum distributorType;
		private AnschlussHollaenderEnum anschlussHollaender;
		private bool langeAnschlussboegen;
		private int maxCircuits;
		private int additionalCircuits;
		private int zusaetzlicheStellantriebe;
		private bool flanschKugelHaehne;
		private bool einbauSchrank;
		private bool useForFloor;
		private bool useForWall;
		private bool useForCeiling;

		private List<string> additionalFloors;

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(Distributor));

		private TreeNode distributorNode = new TreeNode();

		public Distributor() {
			InitializeDistributor();
		}

		public override string ToString() {
			return this.id + ": " + this.name;
		}

		private void InitializeDistributor() {
			id = "";
			name = "";
			regulatorCircuitId = "";
			//distributorType = DistributorTypeEnum.Durchflussmengenregler;
			anschlussHollaender = AnschlussHollaenderEnum.Kein;
			langeAnschlussboegen = false;
			maxCircuits = 12;
			additionalCircuits = 0;
			zusaetzlicheStellantriebe = 0;
			flanschKugelHaehne = true;
			einbauSchrank = true;
			additionalFloors = new List<string>();
			distributorNode.Tag = this;
			useForFloor = true;
			useForWall = true;
			useForCeiling = true;
		}

		public string Id {
			get { return id; }
			set {
				id = value;
				if (distributorNode != null) {
					distributorNode.Text = (String.IsNullOrEmpty(name) ? EuroplanRes.Distributor_Unbenannt : id + ": " + name);
				}
			}
		}

		public string Name {
			get { return name; }
			set {
				name = value;
				if (distributorNode != null) {
					distributorNode.Text = (String.IsNullOrEmpty(name) ? EuroplanRes.Distributor_Unbenannt : id + ": " + name);
				}
			}
		}

		//public DistributorTypeEnum DistributorType {
		//    get { return distributorType; }
		//    set { distributorType = value; }
		//}

		public AnschlussHollaenderEnum AnschlussHollaender {
			get { return anschlussHollaender; }
			set { anschlussHollaender = value; }
		}

		[XmlIgnore]
		public List<Floor> AdditionalFloors {
			get {
				List<Floor> floors = new List<Floor>();
				foreach (string floorId in additionalFloors) {
					foreach (Floor floor in Project.Instance.Floors) {
						if (floor.Id == floorId) {
							floors.Add(floor);
							continue;
						}
					}
				}
				return floors;
			}
			set {
				foreach (Floor floor in value) {
					if (!additionalFloors.Contains(floor.Id)) {
						additionalFloors.Add(floor.Id);
					}
				}
			}
		}

		[XmlIgnore]
		public Floor AssociatedFloor {
			get {
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.Distributors.Contains(this)) {
						return floor;
					}
				}
				return null;
			}
		}

		public int MaxCircuits {
			get { return maxCircuits; }
			set { maxCircuits = value; }
		}

		public int AdditionalCircuits {
			get { return additionalCircuits; }
			set { additionalCircuits = value; }
		}

		public int ZusaetzlicheStellantriebe {
			get { return zusaetzlicheStellantriebe; }
			set { zusaetzlicheStellantriebe = value; }
		}

		public bool FlanschKugelHaehne {
			get { return flanschKugelHaehne; }
			set { flanschKugelHaehne = value; }
		}

		public bool EinbauSchrank {
			get { return einbauSchrank; }
			set { einbauSchrank = value; }
		}

		public bool LangeAnschlussboegen {
			get { return langeAnschlussboegen; }
			set { langeAnschlussboegen = value; }
		}

		public bool UseForFloor {
			get { return useForFloor; }
			set { useForFloor = value; }
		}

		public bool UseForWall {
			get { return useForWall; }
			set { useForWall = value; }
		}

		public bool UseForCeiling {
			get { return useForCeiling; }
			set { useForCeiling = value; }
		}

		internal TreeNode Node {
			get { return this.distributorNode; }
		}

		public List<string> AdditionalFloorIds {
			get { return additionalFloors; }
			set { additionalFloors = value; }
		}

		[XmlIgnore]
		public RegulatorCircuit RegulatorCircuit {
			get {
				if (this.regulatorCircuitId != null) {
					if (Project.Instance != null) {
						foreach (RegulatorCircuit c in Project.Instance.RegulatorCircuits) {
							if (c.Id == regulatorCircuitId) {
								this.regulatorCircuit = c;
								continue;
							}
						}
					}
					this.regulatorCircuitId = null;
				}
				return this.regulatorCircuit;
			}
			set {
				this.regulatorCircuit = value;
				this.regulatorCircuitId = null;
			}
		}

		public string RegulatorCircuitId {
			get {
				if (this.RegulatorCircuit == null) {
					return null;
				}
				return this.regulatorCircuit.Id;
			}
			set { this.regulatorCircuitId = value; }
		}

		public TreeNode FindNode(object element) {
			if (element == this) {
				return distributorNode;
			}
			return null;
		}
		
		public Type AssociatedPanelType {
			get {
				return typeof(DistributorPanel);
			}
		}

		public System.Drawing.Icon AssociatedIcon {
			get {
				return null;
			}
		}

		[XmlIgnore]
		public List<PlannedProduct> PlannedConnectedProducts {
			get {
				List<PlannedProduct> products = new List<PlannedProduct>();
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct product in room.PlannedProducts) {
							if (product.Product.PlannedConnection != null && product.Product.PlannedConnection.Distributor != null) {
								if (product.Product.PlannedConnection.Distributor == this) {
									products.Add(product);
								}
							}
						}
					}
				}
				return products;
			}
		}

		public int PlannedCircuits {
			get {
				int plannedCircuits = 0;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct pp in room.PlannedProducts) {
							if (pp.Product.PlannedConnection != null && pp.Product.PlannedConnection.DistributorId == this.Id) {
								plannedCircuits += pp.Product.PlannedCircuitCount;
							}
						}
					}
				}
				return plannedCircuits;
			}
		}

		public int PlannedStellAntriebe {
			get {
				int plannedStellAntriebe = 0;
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Room room in floor.Rooms) {
						foreach (PlannedProduct pp in room.PlannedProducts) {
							if (pp.Product.PlannedConnection != null && pp.Product.PlannedConnection.DistributorId == this.Id) {
								if (pp.Product.StellMotore) {
									plannedStellAntriebe += pp.Product.PlannedCircuitCount;
								}
							}
						}
					}
				}
				return plannedStellAntriebe;
			}
		}

		public double GetDruckverlust(double durchfluss) {
			double ratio = 0;
			switch (PlannedCircuits) {
				case 1:
				case 2:
					ratio = 200.0 / 700.0;
					break;
				case 3:
					ratio = 200.0 / 1000.0;
					break;
				case 4:
					ratio = 200.0 / 1300.0;
					break;
				case 5:
					ratio = 200.0 / 1600.0;
					break;
				case 6:
					ratio = 200.0 / 1900.0;
					break;
				case 7:
					ratio = 180.0 / 2000.0;
					break;
				case 8:
					ratio = 160.0 / 2000.0;
					break;
				case 9:
					ratio = 140.0 / 2000.0;
					break;
				case 10:
					ratio = 120.0 / 2000.0;
					break;
				case 11:
					ratio = 100.0 / 2000.0;
					break;
				case 12:
					ratio = 80.0 / 2000.0;
					break;
			}

			return durchfluss * ratio;
		}

		public double MaxDruckverlustVerteilerHeat {
			get {
				double maxDruckverlustInCircuit = 0;
				double durchfluss = 0;
				foreach (PlannedProduct pp in PlannedConnectedProducts) {
					maxDruckverlustInCircuit = maxDruckverlustInCircuit < pp.Product.PlannedDeltaRhoDistributorHeat ? pp.Product.PlannedDeltaRhoDistributorHeat : maxDruckverlustInCircuit;
					durchfluss += pp.Product.PlannedMhHeat;
				}
				return maxDruckverlustInCircuit + GetDruckverlust(durchfluss);
			}
		}

		public double MaxDruckverlustVerteilerCool {
			get {
				double maxDruckverlustInCircuit = 0;
				double durchfluss = 0;
				foreach (PlannedProduct pp in PlannedConnectedProducts) {
					maxDruckverlustInCircuit = maxDruckverlustInCircuit < pp.Product.PlannedDeltaRhoDistributorCool ? pp.Product.PlannedDeltaRhoDistributorCool : maxDruckverlustInCircuit;
					durchfluss += pp.Product.PlannedMhCool;
				}
				return maxDruckverlustInCircuit + GetDruckverlust(durchfluss);
			}
		}


		public void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {
			int totalCircuits = PlannedCircuits + AdditionalCircuits;
			int totalStellantriebe = PlannedStellAntriebe + ZusaetzlicheStellantriebe;

			string partNr = "VO";
			int circuits = totalCircuits > 2 ? totalCircuits : 2;
			partNr += String.Format("{0:00}", circuits);
			Project.Instance.AddRequiredMaterial(requiredMaterial, partNr, 1);

			if (this.FlanschKugelHaehne) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "VT35", 2);
			}
			if (this.AnschlussHollaender == AnschlussHollaenderEnum.hollaender32) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "VT21", 2);
			} else if (this.AnschlussHollaender == AnschlussHollaenderEnum.hollaenderIG) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "VT20", 2);
			}
			string einbauSchrank = "";
			if (this.EinbauSchrank) {
				if (totalCircuits <= 4) {
					einbauSchrank = "VO60";
				} else if (totalCircuits > 4 && totalCircuits <= 9) {
					einbauSchrank = "VO61";
				} else {
					einbauSchrank = "VO62";
				}
				Project.Instance.AddRequiredMaterial(requiredMaterial, einbauSchrank, 1);
			}
			if (totalStellantriebe > 0) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "VO44", totalStellantriebe);
			}
		}

	}

}
