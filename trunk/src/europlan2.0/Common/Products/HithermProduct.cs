using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.Windows.Forms;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Hitherm®", "Hitherm® Klimawand")]
	public class HithermProduct : Product {

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
		private static double[][] hlRegHeizleistung = {
			//  tHm (°C)  30.0  32.5  35.0  37.5  40.0  42.5  45.0  47.5  50.0
			new double[] { 105,  120,  140,  155,  175,  190,  210,  225,  240}, // ti=15°C
			new double[] {  85,  100,  120,  135,  155,  170,  185,  205,  220}, // ti=18°C
			new double[] {  70,   85,  105,  120,  140,  155,  175,  190,  210}, // ti=20°C
			new double[] {  55,   70,   90,  105,  125,  140,  160,  175,  195}, // ti=22°C
			new double[] {  45,   60,   80,   95,  115,  130,  145,  165,  180}  // ti=24°C
		};

		// Hitherm(r) Standard-Klimawandregister (RA 10) Heizleistung qW in W/m²
		private static double[][] stdRegHeizleistung = {
			//  tHm (°C)  30.0  32.5  35.0  37.5  40.0  42.5  45.0  47.5  50.0
			new double[] {  70,   85,   95,  110,  120,  130,  145,  155,  170}, // ti=15°C
			new double[] {  55,   70,   80,   95,  105,  120,  130,  145,  155}, // ti=18°C
			new double[] {  50,   60,   75,   85,  100,  110,  125,  135,  150}, // ti=20°C
			new double[] {  40,   50,   65,   75,   90,  100,  115,  125,  140}, // ti=22°C
			new double[] {  30,   40,   55,   65,   80,   90,  100,  115,  130}  // ti=24°C
		};

		/*private static double[][] hlRegKuehlleistung = {
			//  tHm (°C) 16.0 18.0 20.0 22.0 25.0
			new double[] { 13,   0},               // ti=18°C
			new double[] { 25,  13,   0},          // ti=20°C
			new double[] { 40,  25,  13,   0},     // ti=22°C
			new double[] { 60,  45,  33,  20,   0} // ti=25°C
		};*/

		//     Diffenz Raumtemp - Kuehlmitteltemp (K):  0   2   3   4   5   6   7   9
		private static double[] hlRegKuehlleistung  = { 0, 13, 20, 25, 33, 40, 45, 60 };
		private static double[] stdRegKuehlleistung = { 0,  9, 14, 18, 24, 29, 32, 43 };

		private static double[] beplankungRWerte = { 0, 0.01, 0.02, 0.1 };
		private static double[] beplankungFaktoren = { 1, 0.95, 0.91, 0.66 };

		private static double alphaBoden = 10.8;
		private static double alphaWand = 8;
		private static double alphaDecke = 6.5;

		private static double defaultDaemmung = 2.5;

		private static bool usePlus = false;

		/*private static double factorSpezialputz = 1.15;
		private static double factorMaschinenputz = 1.0;
		private static double factorLehmputz = 0.95;
		private static double factorGkpHohlraum = 0.69;
		private static double factorHolzHohlraum = 0.62;*/

		private Dictionary<HithermRegister, int> registerCircuits = new Dictionary<HithermRegister, int>();
		private Dictionary<int, HithermCircuit> circuitIds = new Dictionary<int, HithermCircuit>();

		private ProductType hithermType = ProductType.WH;
		private float plannedFloorArea = 0;
		private float plannedCeilingArea = 0;
		private float plannedFloorOrCeilingArea = 0;

		public HithermProduct() {

		}

		protected HithermProduct(HithermProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public override void StaticInitialize() {
			quickDimensioningHeatPowerPerSquareMeter = 100;
			quickDimensioningCoolPowerPerSquareMeter = 100;
			canHeat = true;
			canCool = false;
			usePlus = false;
			maxPressureLost = 15000;
			maxDurchfluss = 240;
		}

		public override Product Clone(Room room) {
			HithermProduct product = new HithermProduct(this);
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
				return ConvertArrayToString2(hlRegHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					hlRegHeizleistung = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung {
			get { return hlRegHeizleistung; }
			set { hlRegHeizleistung = value; }
		}

		[ProductParameter]
		public static string ConfigStdRegHeizleistungString {
			get {
				return ConvertArrayToString2(stdRegHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					stdRegHeizleistung = array;
				}
			}
		}
		public static double[][] ConfigStdRegHeizleistung {
			get { return stdRegHeizleistung; }
			set { stdRegHeizleistung = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegKuehlleistungString {
			get {
				return ConvertArrayToString(hlRegKuehlleistung);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					hlRegKuehlleistung = array;
				}
			}
		}
		public static double[] ConfigHlRegKuehlleistung {
			get { return hlRegKuehlleistung; }
			set { hlRegKuehlleistung = value; }
		}

		[ProductParameter]
		public static string ConfigStdRegKuehlleistungString {
			get {
				return ConvertArrayToString(stdRegKuehlleistung);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					stdRegKuehlleistung = array;
				}
			}
		}
		public static double[] ConfigStdRegKuehlleistung {
			get { return stdRegKuehlleistung; }
			set { stdRegKuehlleistung = value; }
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

		/*[ProductParameter]
		public static double ConfigFactorSpezialputz {
			get { return factorSpezialputz; }
			set { factorSpezialputz = value; }
		}

		[ProductParameter]
		public static double ConfigFactorMaschinenputz {
			get { return factorMaschinenputz; }
			set { factorMaschinenputz = value; }
		}

		[ProductParameter]
		public static double ConfigFactorLehmputz {
			get { return factorLehmputz; }
			set { factorLehmputz = value; }
		}

		[ProductParameter]
		public static double ConfigFactorGkpHohlraum {
			get { return factorGkpHohlraum; }
			set { factorGkpHohlraum = value; }
		}

		[ProductParameter]
		public static double ConfigFactorHolzHohlraum {
			get { return factorHolzHohlraum; }
			set { factorHolzHohlraum = value; }
		}*/

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
			get { return "Hitherm®\n(m²)"; }
		}

		public override ProductType Type {
			get { return this.hithermType; }
		}

		public ProductType HithermType {
			get { return this.hithermType; }
			set { this.hithermType = value; }
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
			this.incompleteCalculation = false;
			if (this.PlannedConnection == null) {
				this.lastErrorMsg = "Fehlende Eingaben: ";
				if (PlannedConnection == null) {
					this.lastErrorMsg += "Heizkreisanschluß, ";
				}
				this.lastErrorMsg = this.lastErrorMsg.Substring(0, this.lastErrorMsg.Length - 2);
				this.incompleteCalculation = true;
				return false;
			}

			double[] vorlaufTotal;
			double[] vorlaufNotIsolated;
			double[] ruecklaufTotal;
			double[] ruecklaufNotIsolated;
			double[] vorlaufWithoutOtherProductTotal;
			double[] vorlaufWithoutOtherProductNotIsolated;
			double[] ruecklaufWithoutOtherProductTotal;
			double[] ruecklaufWithoutOtherProductNotIsolated;
			double longestVorlaufTotal;
			double longestRuecklaufTotal;
			this.CalculateVorlaufRuecklauf(out vorlaufTotal, out vorlaufNotIsolated, out ruecklaufTotal, out ruecklaufNotIsolated, out vorlaufWithoutOtherProductTotal, out vorlaufWithoutOtherProductNotIsolated, out ruecklaufWithoutOtherProductTotal, out ruecklaufWithoutOtherProductNotIsolated, out longestVorlaufTotal, out longestRuecklaufTotal);

			this.CalculateHeatAndCoolFlow();
			int i = 0;
			foreach (HithermCircuit hc in this.circuits) {
				hc.HithermProduct = this;
				hc.NrOfCircuit = i;
				hc.PipeLengthVorlaufTotal = vorlaufTotal[i];
				hc.PipeLengthVorlaufNotIsolated = vorlaufNotIsolated[i];
				hc.PipeLengthRuecklaufTotal = ruecklaufTotal[i];
				hc.PipeLengthRuecklaufNotIsolated = ruecklaufNotIsolated[i];
				hc.PipeLengthVorlaufWithoutOtherProductTotal = vorlaufWithoutOtherProductTotal[i];
				hc.PipeLengthVorlaufWithoutOtherProductNotIsolated = vorlaufWithoutOtherProductNotIsolated[i];
				hc.PipeLengthRuecklaufWithoutOtherProductTotal = ruecklaufWithoutOtherProductTotal[i];
				hc.PipeLengthRuecklaufWithoutOtherProductNotIsolated = ruecklaufWithoutOtherProductNotIsolated[i];
				hc.Calculate();
			}

			if (variableSpreizung && this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
				// Heizleistung veringern
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < HithermProduct.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad) {
					this.plannedRuecklaufTempHeat -= 0.1;
					foreach (HithermCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempHeat += 0.1;
				// Heizleistung erhöhen
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > HithermProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < HithermProduct.ConfigMaxPressureLost / 100 && this.PlannedMaxMhHeat < HithermProduct.ConfigMaxDurchfluss) {
					this.plannedRuecklaufTempHeat += 0.1;
					foreach (HithermCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				// Kühlleistung verringern
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < HithermProduct.ConfigSpreizungKühlMax && this.PlannedCoolLoad > requestedCoolLoad) {
					this.plannedRuecklaufTempCool += 0.1;
					foreach (HithermCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempCool -= 0.1;
				// Kühlleistung erhöhen
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > HithermProduct.ConfigSpreizungKühlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < HithermProduct.ConfigMaxPressureLost / 100 && this.PlannedMaxMhCool < HithermProduct.ConfigMaxDurchfluss) {
					this.plannedRuecklaufTempCool -= 0.1;
					foreach (HithermCircuit c in this.circuits) {
						c.Calculate();
					}
				}

				// Calculate variable spreizung for connected products
				foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.connectedCircuits) {
					if (kvp.Value != null) {
						kvp.Value.OtherProduct.CalculateHeatAndCoolFlow();
						PlannedProduct pp = Project.Instance.GetPlannedProduct(kvp.Value.OtherProduct);
						if (pp != null) {
							pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, true);
						}
					}
				}
			}

			this.lastErrorMsg = "";
			if (this.PlannedMaxMhHeat >= this.PlannedMaxMhCool) {
				if (Math.Round(this.PlannedMaxMhHeat, 1) > HithermProduct.ConfigMaxDurchfluss) {
					this.lastErrorMsg += "Durchfluß bei Heizung zu groß (" + Math.Round(this.PlannedMaxMhHeat, 1).ToString() + "kg/h > " + HithermProduct.ConfigMaxDurchfluss.ToString() + "kg/h)\n";
				}
			} else {
				if (Math.Round(this.PlannedMaxMhCool, 1) > HithermProduct.ConfigMaxDurchfluss) {
					this.lastErrorMsg += "Durchfluß bei Kühlung zu groß (" + Math.Round(this.PlannedMaxMhCool, 1).ToString() + "kg/h > " + HithermProduct.ConfigMaxDurchfluss.ToString() + "kg/h)\n";
				}
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > HithermProduct.ConfigMaxPressureLost / 100) {
					this.lastErrorMsg += "Druckverlust bei Heizung zu groß (" + Math.Round(this.PlannedDeltaRhoHeat, 2).ToString() + "mbar > " + (HithermProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
				}
			} else {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > HithermProduct.ConfigMaxPressureLost / 100) {
					this.lastErrorMsg += "Druckverlust bei Kühlung zu groß (" + Math.Round(this.PlannedDeltaRhoCool, 1).ToString() + "mbar > " + (HithermProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
				}
			}
			if (this.hithermType == Product.ProductType.FBH && this.PlannedRegisterArea > this.PlannedFloorArea) {
				this.lastErrorMsg += "Die verplanten Register nehmen mehr Fläche in Anspruch als für dieses System zur Verfügung steht (" + Math.Round(this.PlannedRegisterArea, 1).ToString() + "m² > " + Math.Round(this.PlannedFloorArea, 1).ToString() + "m²)\n";
			} else if (this.hithermType == Product.ProductType.DH && this.PlannedRegisterArea > this.PlannedCeilingArea) {
				this.lastErrorMsg += "Die verplanten Register nehmen mehr Fläche in Anspruch als für dieses System zur Verfügung steht (" + Math.Round(this.PlannedRegisterArea, 1).ToString() + "m² > " + Math.Round(this.PlannedCeilingArea, 1).ToString() + "m²)\n";
			}
			if (this.lastErrorMsg.Length == 0) {
				this.lastErrorMsg = null;
			}

			return true;
		}

		public override float PlannedFloorArea {
			get {
				if (this.hithermType == ProductType.FBH) {
					return this.plannedFloorArea;
				}
				return 0;
			}
			set {
				if (this.hithermType == ProductType.FBH) {
					this.plannedFloorArea = value;
				}
			}
		}

		public override float PlannedWallArea {
			get {
				if (this.hithermType == ProductType.WH) {
					return this.PlannedNetArea;
				}
				return 0;
			}
			set { }
		}

		public override float PlannedCeilingArea {
			get {
				if (this.hithermType == ProductType.DH) {
					return this.plannedCeilingArea;
				}
				return 0;
			}
			set {
				if (this.hithermType == ProductType.DH) {
					this.plannedCeilingArea = value;
				}
			}
		}

		public override float PlannedRoofArea {
			get { return 0; }
			set { }
		}

		// Not to be used in code! This property is only intended to be used for (de)serializing
		public float PlannedFloorOrCeilingArea {
			get {
				if (this.hithermType == ProductType.DH) {
					return this.plannedCeilingArea;
				}
				if (this.hithermType == ProductType.FBH) {
					return this.plannedFloorArea;
				}
				return 0;
			}
			set {
				this.plannedFloorOrCeilingArea = value;
			}
		}

		public override double PlannedCoolLoad {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (HithermCircuit c in this.circuits) {
					if (!c.QFbhTotalCool.Equals(double.NaN)) {
						value += c.QFbhTotalCool;
					}
				}
				return value;
			}
		}

		public override double PlannedHeatLoad {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (HithermCircuit c in this.circuits) {
					if (!c.QFbhTotalHeat.Equals(double.NaN)) {
						value += c.QFbhTotalHeat;
					}
				}
				return value;
			}
		}

		public override float PlannedNetArea {
			get {
				double area = 0;
				foreach (HithermCircuit hc in this.circuits) {
					area += hc.RegisterArea;
				}
				return (float)area;
			}
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

		public HithermCircuit GetCircuitForRegister(HithermRegister register) {
			if (!this.registerCircuits.ContainsKey(register)) {
				return null;
			}
			if (!this.circuitIds.ContainsKey(this.registerCircuits[register])) {
				return null;
			}
			return this.circuitIds[this.registerCircuits[register]];
		}

		internal void AddRegisterToCircuit(HithermRegister register, int circuitId) {
			this.registerCircuits[register] = circuitId;
			if (!this.circuitIds.ContainsKey(circuitId)) {
				HithermCircuit hc = new HithermCircuit();
				hc.HithermProduct = this;
				this.circuits.Add(hc);
				this.circuitIds[circuitId] = hc;
			}
			this.circuitIds[circuitId].Registers.Add(register);
		}

		internal void MoveRegisterToCircuit(HithermRegister register, int circuitId) {
			if (this.registerCircuits.ContainsKey(register)) {
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
			}
		}

		internal void RemoveRegisterFromCircuit(HithermRegister register) {
			if (this.registerCircuits.ContainsKey(register)) {
				HithermCircuit hc = this.circuitIds[this.registerCircuits[register]];
				hc.Registers.Remove(register);
				if (hc.Registers.Count == 0) {
					this.circuits.Remove(hc);
					this.circuitIds.Remove(this.registerCircuits[register]);
				}
				this.registerCircuits.Remove(register);
			}
		}

		internal int GetRegisterCircuitId(HithermRegister register) {
			if (this.registerCircuits.ContainsKey(register)) {
				return this.registerCircuits[register];
			}
			return 0;
		}

		internal override void FinalizeLoading(PlannedProduct pp) {
			base.FinalizeLoading(pp);
			switch (this.hithermType) {
				case ProductType.FBH:
					this.plannedFloorArea = this.plannedFloorOrCeilingArea;
					this.plannedFloorOrCeilingArea = 0;
					this.plannedCeilingArea = 0;
					break;

				case ProductType.DH:
					this.plannedCeilingArea = this.plannedFloorOrCeilingArea;
					this.plannedFloorOrCeilingArea = 0;
					this.plannedFloorArea = 0;
					break;

				default:
					this.plannedCeilingArea = 0;
					this.plannedFloorOrCeilingArea = 0;
					this.plannedFloorArea = 0;
					break;
			}
			if (pp != null) {
				int i = 1;
				foreach (HithermCircuit hc in this.circuits) {
					this.circuitIds[i] = hc;
					foreach (HithermRegister hr in hc.Registers) {
						this.registerCircuits[hr] = i;
						hr.PlannedProduct = pp;
					}
					i++;
				}
			}
		}

		[XmlIgnore]
		public override double PlannedHeizlastBereinigung {
			get {
				double bereinigung = 0;
				foreach (HithermCircuit hc in this.circuits) {
					bereinigung += hc.HeizleistungBereinigung;
				}
				return bereinigung;
			}
		}

		[XmlIgnore]
		public override double PlannedKuehllastBereinigung {
			get {
				double bereinigung = 0;
				foreach (HithermCircuit hc in this.circuits) {
					bereinigung += hc.KuehlleistungBereinigung;
				}
				return bereinigung;
			}
		}

		/// <summary>
		/// The percentage of the total room area that is occupied by the planned area.
		/// </summary>
		[XmlIgnore]
		public float PlannedFloorAreaPercentage {
			get {
				if (this.hithermType != ProductType.FBH) {
					return 0;
				}
				return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AssociatedRoom.Area);
			}
			set {
				if (this.hithermType == ProductType.FBH) {
					this.PlannedFloorArea = (float)(this.AssociatedRoom.Area * value / 100);
				}
			}
		}

		/// <summary>
		/// The percentage of the total room area that is occupied by the planned area.
		/// </summary>
		[XmlIgnore]
		public float PlannedCeilingAreaPercentage {
			get {
				if (this.hithermType != ProductType.DH) {
					return 0;
				}
				return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedCeilingArea * 100 / this.AssociatedRoom.Area);
			}
			set {
				if (this.hithermType == ProductType.DH) {
					this.PlannedCeilingArea = (float)(this.AssociatedRoom.Area * value / 100);
				}
			}
		}

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {

			// Euroval Anbindung
			// 21mm Anbindung
			double pipeEurovalLength = 0;
			double pipe21mmLength = 0;
			double circuit21mmOnlyFirstLength = 0;
			double circuit21mmAllLength = 0;
			foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
				if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
					if (pipe.OnlyFirst) {
						pipe21mmLength += (pipe.Vorlauf + pipe.Ruecklauf);
						circuit21mmOnlyFirstLength += (pipe.Vorlauf + pipe.Ruecklauf);
					} else {
						pipe21mmLength += ((pipe.Vorlauf + pipe.Ruecklauf) * this.PlannedCircuitCount);
						circuit21mmAllLength += (pipe.Vorlauf + pipe.Ruecklauf);
					}

				} else {
					if (pipe.OnlyFirst) {
						pipeEurovalLength += (pipe.Vorlauf + pipe.Ruecklauf);
					} else {
						pipeEurovalLength += ((pipe.Vorlauf + pipe.Ruecklauf) * this.PlannedCircuitCount);
					}
				}
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, "EV01", pipeEurovalLength);
			if (ConfigUsePlus) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR51", pipe21mmLength);
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI51", pipe21mmLength);
			}
			
			
			double verbindeLength = 0;
			int teilflaechen = 0;
			double registerCount = 0;
			foreach (HithermCircuit c in this.circuits) {
				foreach (HithermRegister register in c.Registers) {
					teilflaechen++;
					registerCount += register.RegisterCount;

					// Register
					if (register.PartNumber != "") {
						Project.Instance.AddRequiredMaterial(requiredMaterial, register.PartNumber, register.RegisterCount);
					}
#if DEBUG
					else {
						MessageBox.Show("Hitherm Product not found.");
					}
#endif

					// Ovalschweißmuffen bei Hitherm+
					if (ConfigUsePlus && register.RegisterCount > 1) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "EV10", 2 * (register.RegisterCount - 1));
					}

					// Ovalendkappen
					if (ConfigUsePlus) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HR65", 2);
					} else {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HI65", 2);
					}

					//Wandwinkel
					int amount = 2;
					if (register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_HORIZONTAL) {
						amount = 4;
					}
					if (ConfigUsePlus) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HR66", amount);
					} else {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HI66", amount);
					}

					verbindeLength += register.PipeHorizontal + register.PipeVertical;

				}
				// Bodenwinkel
				if (ConfigUsePlus) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HR68", 2);
				} else {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HI68", 2);
				}

			}

			// Ovalmuffen
			if (verbindeLength > 0) {
				if (ConfigUsePlus) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HR55", verbindeLength / 2);
				} else {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", verbindeLength / 2);
				}
			}

			// Ovalrohr
			if (ConfigUsePlus) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR60", verbindeLength);
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI60", verbindeLength);
			}

			// Dübelhaken
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI40", (registerCount * 2) + verbindeLength);

			// unknown amount
			if (ConfigUsePlus) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR67", Double.NegativeInfinity);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR70", Double.NegativeInfinity);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR69", Double.NegativeInfinity);

			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI67", Double.NegativeInfinity);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI70", Double.NegativeInfinity);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI71", Double.NegativeInfinity);
			}
		}

		public double PlannedRegisterArea {
			get {
				double area = 0;
				foreach (HithermCircuit hc in this.PlannedCircuits) {
					area += hc.RegisterArea;
				}
				return area;
			}
		}
	}
	
}
