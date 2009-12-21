using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Hitherm® Compact", "Hitherm® Compact Klimawand")]
	public class HithermCompactProduct : Product {

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 100;
		private static int quickDimensioningCoolPowerPerSquareMeter = 100;
		private static bool canHeat = true;
		private static bool canCool = false;

		// planning
		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double verbindeLeitungInnenquerschnitt = 0.000179071;
		private static double verbindeLeitungInnendurchmesser = 0.015099678;
		private static double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static double v = 0.00000101; /* m²/s ... kinematische Viskosität */

		private static double spreizungHeizMin = 4;
		private static double spreizungHeizMax = 12;
		private static double spreizungKühlMin = 2;
		private static double spreizungKühlMax = 5;

		private static int maxPressureLost = 15000;
		private static int maxDurchfluss = 240;

		// Hitherm(r) Hochleistungs-Klimawandregister (RA 5) Heizleistung qW in W/m²
		private static double[][] regHeizleistung = {
			//  tHm (°C)  30.0  32.5  35.0  37.5  40.0  42.5  45.0  47.5  50.0
			new double[] { 105,  120,  140,  155,  175,  190,  210,  225,  240}, // ti=15°C
			new double[] {  85,  100,  120,  135,  155,  170,  185,  205,  220}, // ti=18°C
			new double[] {  70,   85,  105,  120,  140,  155,  175,  190,  210}, // ti=20°C
			new double[] {  55,   70,   90,  105,  125,  140,  160,  175,  195}, // ti=22°C
			new double[] {  45,   60,   80,   95,  115,  130,  145,  165,  180}  // ti=24°C
		};

		//     Diffenz Raumtemp - Kuehlmitteltemp (K):  0   2   3   4   5   6   7   9
		private static double[] regKuehlleistung = { 0, 13, 20, 25, 33, 40, 45, 60 };

		private static double[] beplankungRWerte = { 0, 0.01, 0.02, 0.1 };
		private static double[] beplankungFaktoren = { 1, 0.95, 0.91, 0.66 };

		private static double alphaBoden = 10.8;
		private static double alphaWand = 8;
		private static double alphaDecke = 6.5;

		private static double defaultDaemmung = 2.5;

		private static bool usePlus = false;

		private Dictionary<HithermRegister, int> registerCircuits = new Dictionary<HithermRegister, int>();
		private Dictionary<int, HithermCircuit> circuitIds = new Dictionary<int, HithermCircuit>();

		private ProductType hithermCompactType = ProductType.WH;
		private float plannedFloorArea = 0;
		private float plannedCeilingArea = 0;
		private float plannedFloorOrCeilingArea = 0;

		public HithermCompactProduct() {
		}

		protected HithermCompactProduct(HithermCompactProduct product) : base(product) {
		}

		public override void Initialize() {
		}

		public override void StaticInitialize() {
			quickDimensioningHeatPowerPerSquareMeter = 100;
			quickDimensioningCoolPowerPerSquareMeter = 100;
			canHeat = true;
			canCool = false;
		}

		public override Product Clone(Room room) {
			HithermCompactProduct product = new HithermCompactProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		#region Product Parameters
		[ProductParameter]
		public static bool ConfigQuickDimensioningCanHeat {
			get { return canHeat; }
			set { canHeat = value; }
		}
		public override bool QuickDimensioningCanHeat {
			get { return canCool; }
		}

		[ProductParameter]
		public static bool ConfigQuickDimensioningCanCool {
			get { return canCool; }
			set { canCool = value; }
		}
		public override bool QuickDimensioningCanCool {
			get { return canCool; }
		}

		[ProductParameter]
		public static int ConfigQuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
			set { quickDimensioningHeatPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
		}

		[ProductParameter]
		
		public static int ConfigQuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
			set { quickDimensioningCoolPowerPerSquareMeter = value; }
		}
		
		public override int QuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
		}

		[ProductParameter]
		public static double ConfigC {
			get { return c; }
			set { c = value; }
		}

		[ProductParameter]
		public static double ConfigVerbindeLeitungInnendurchmesser {
			get { return verbindeLeitungInnendurchmesser; }
			set { verbindeLeitungInnendurchmesser = value; }
		}

		[ProductParameter]
		public static double ConfigVerbindeLeitungInnenquerschnitt {
			get { return verbindeLeitungInnenquerschnitt; }
			set { verbindeLeitungInnenquerschnitt = value; }
		}

		[ProductParameter]
		public static double ConfigRho {
			get { return rho; }
			set { rho = value; }
		}

		[ProductParameter]
		public static double ConfigV {
			get { return v; }
			set { v = value; }
		}

		[ProductParameter]
		public static int ConfigMaxPressureLost {
			get { return maxPressureLost; }
			set { maxPressureLost = value; }
		}

		[ProductParameter]
		public static int ConfigMaxDurchfluss {
			get { return maxDurchfluss; }
			set { maxDurchfluss = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistungString {
			get {
				return ConvertArrayToString2(regHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung {
			get { return regHeizleistung; }
			set { regHeizleistung = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegKuehlleistungString {
			get {
				return ConvertArrayToString(regKuehlleistung);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					regKuehlleistung = array;
				}
			}
		}
		public static double[] ConfigHlRegKuehlleistung {
			get { return regKuehlleistung; }
			set { regKuehlleistung = value; }
		}

		[ProductParameter]
		public static string ConfigBeplankungRWerteString {
			get {
				return ConvertArrayToString(beplankungRWerte);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					beplankungRWerte = array;
				}
			}
		}
		public static double[] ConfigBeplankungRWerte {
			get { return beplankungRWerte; }
			set { beplankungRWerte = value; }
		}

		[ProductParameter]
		public static string ConfigBeplankungFaktorenString {
			get {
				return ConvertArrayToString(beplankungFaktoren);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					beplankungFaktoren = array;
				}
			}
		}
		public static double[] ConfigBeplankungFaktoren {
			get { return beplankungFaktoren; }
			set { beplankungFaktoren = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaBoden {
			get { return alphaBoden; }
			set { alphaBoden = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaWand {
			get { return alphaWand; }
			set { alphaWand = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaDecke {
			get { return alphaDecke; }
			set { alphaDecke = value; }
		}

		[ProductParameter]
		public static double ConfigDefaultDaemmung {
			get { return defaultDaemmung; }
			set { defaultDaemmung = value; }
		}

		[ProductParameter]
		public static bool ConfigUsePlus {
			get { return usePlus; }
			set { usePlus = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungHeizMin {
			get { return spreizungHeizMin; }
			set { spreizungHeizMin = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungHeizMax {
			get { return spreizungHeizMax; }
			set { spreizungHeizMax = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungKühlMin {
			get { return spreizungKühlMin; }
			set { spreizungKühlMin = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungKühlMax {
			get { return spreizungKühlMax; }
			set { spreizungKühlMax = value; }
		}
		#endregion Product Parameters

		public override int GetDefaultQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 10);
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			return 0;
		}

		public override float QuickDimensioningMaximumArea {
			get { return Int32.MaxValue; }
		}

		public override string QuickDimensioningName {
			get { return "Hitherm®\nCompact\n(m²)"; }
		}

		public override ProductType Type {
			get { return this.hithermCompactType; }
		}

		public ProductType HithermType {
			get { return this.hithermCompactType; }
			set { this.hithermCompactType = value; }
		}

		public override void CalculateHeatAndCoolFlow() {
			base.CalculateHeatAndCoolFlow();
			double spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			double spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			if (spreizungHeat > HithermProduct.ConfigSpreizungHeizMax) {
				spreizungHeat = HithermProduct.ConfigSpreizungHeizMax;
			}
			if (spreizungHeat < HithermProduct.ConfigSpreizungHeizMin) {
				spreizungHeat = HithermProduct.ConfigSpreizungHeizMin;
			}
			if (spreizungCool > HithermProduct.ConfigSpreizungKühlMax) {
				spreizungCool = HithermProduct.ConfigSpreizungKühlMax;
			}
			if (spreizungCool < HithermProduct.ConfigSpreizungKühlMin) {
				spreizungCool = HithermProduct.ConfigSpreizungKühlMin;
			}
			this.plannedRuecklaufTempHeat = this.plannedVorlaufTempHeat - spreizungHeat;
			this.plannedRuecklaufTempCool = this.plannedVorlaufTempCool + spreizungCool;
			if (this.plannedRuecklaufTempHeat - this.associatedRoom.RoomHeatTemperature < 3) {
				this.plannedRuecklaufTempHeat = this.associatedRoom.RoomHeatTemperature + 3;
			}
			if (this.associatedRoom.RoomCoolTemperature - this.plannedRuecklaufTempCool < 3) {
				this.plannedRuecklaufTempCool = this.associatedRoom.RoomCoolTemperature - 3;
			}
		}

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung) {
			// TODO
			this.lastErrorMsg = "Noch nicht implementiert";
			return false;
		}

		public override float PlannedFloorArea {
			get { return 0; }
			set { }
		}

		public override float PlannedWallArea {
			get { return 0; }
			set { }
		}

		public override float PlannedCeilingArea {
			get { return 0; }
			set { }
		}

		public override float PlannedRoofArea {
			get { return 0; }
			set { }
		}

		public override double PlannedCoolLoad {
			get { return 0; }
		}

		public override double PlannedHeatLoad {
			get { return 0; }
		}

		public override float PlannedNetArea {
			get { return 0; }
		}

		/*public override int GetIndexOfCircuit(Circuit c) {
			return -1;
		}*/

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_21MM; }
		}

		[XmlIgnore]
		public override float PlannedInsideConstructionRValue {
			get { return 0; /*TODO*/ }
		}

		[XmlIgnore]
		public override bool HasInsideConstruction {
			get { return false; }
		}

		[XmlIgnore]
		public override Construction PlannedInsideConstruction {
			get { return null; }
		}

		[XmlIgnore]
		public override float PlannedOutsideConstructionRValue {
			get { return 0; /*TODO*/ }
		}

		[XmlIgnore]
		public override bool HasOutsideConstruction {
			get { return false; }
		}

		[XmlIgnore]
		public override Construction PlannedOutsideConstruction {
			get { return null; }
		}

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {

		}

		public HithermCircuit GetCircuitForRegister(HithermCompactRegister register) {
			/*if (!this.registerCircuits.ContainsKey(register)) {
				return null;
			}
			if (!this.circuitIds.ContainsKey(this.registerCircuits[register])) {
				return null;
			}
			return this.circuitIds[this.registerCircuits[register]];*/
			// TODO
			return null;
		}

		internal void AddRegisterToCircuit(HithermCompactRegister register, int circuitId) {
			/*this.registerCircuits[register] = circuitId;
			if (!this.circuitIds.ContainsKey(circuitId)) {
				HithermCircuit hc = new HithermCircuit();
				this.circuits.Add(hc);
				this.circuitIds[circuitId] = hc;
			}
			this.circuitIds[circuitId].Registers.Add(register);*/
			// TODO
		}

		internal void MoveRegisterToCircuit(HithermCompactRegister register, int circuitId) {
			/*if (this.registerCircuits.ContainsKey(register)) {
				HithermCircuit hc = this.circuitIds[this.registerCircuits[register]];
				hc.Registers.Remove(register);
				if (hc.Registers.Count == 0) {
					this.circuits.Remove(hc);
					this.circuitIds.Remove(this.registerCircuits[register]);
				}
				this.registerCircuits[register] = circuitId;
				if (!this.circuitIds.ContainsKey(circuitId)) {
					hc = new HithermCircuit();
					this.circuits.Add(hc);
					this.circuitIds[circuitId] = hc;
				}
				this.circuitIds[circuitId].Registers.Add(register);
			}*/
			// TODO
		}

		internal void RemoveRegisterFromCircuit(HithermCompactRegister register) {
			/*if (this.registerCircuits.ContainsKey(register)) {
				HithermCircuit hc = this.circuitIds[this.registerCircuits[register]];
				hc.Registers.Remove(register);
				if (hc.Registers.Count == 0) {
					this.circuits.Remove(hc);
					this.circuitIds.Remove(this.registerCircuits[register]);
				}
				this.registerCircuits.Remove(register);
			}*/
			// TODO
		}

		internal int GetRegisterCircuitId(HithermCompactRegister register) {
			/*if (this.registerCircuits.ContainsKey(register)) {
				return this.registerCircuits[register];
			}*/
			// TODO
			return 0;
		}

	}
	
}
