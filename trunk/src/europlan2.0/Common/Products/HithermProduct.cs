using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

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

		private static double factorSpezialputz = 1.15;
		private static double factorMaschinenputz = 1.0;
		private static double factorLehmputz = 0.95;
		private static double factorGkpHohlraum = 0.69;
		private static double factorHolzHohlraum = 0.62;

		private Dictionary<HithermRegister, int> registerCircuits = new Dictionary<HithermRegister, int>();
		private Dictionary<int, HithermCircuit> circuitIds = new Dictionary<int, HithermCircuit>();

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

		private static double[][] ConvertStringToArray(string value) {
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
					if (!double.TryParse(strValue.Trim(), out doubleValue)) {
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

		public static string ConvertArrayToString(double[][] array) {
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

		[ProductParameter]
		public static string ConfigHlRegHeizleistungString {
			get {
				return ConvertArrayToString(hlRegHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray(value);
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
				return ConvertArrayToString(stdRegHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray(value);
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
			get { return "Hitherm®\n(m²)"; }
		}

		public override ProductType Type {
			get { return ProductType.WH; }
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
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > HithermProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < HithermProduct.ConfigMaxPressureLost / 100 && this.PlannedMhHeat < HithermProduct.ConfigMaxDurchfluss) {
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
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > HithermProduct.ConfigSpreizungKühlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < HithermProduct.ConfigMaxPressureLost / 100 && this.PlannedMhCool < HithermProduct.ConfigMaxDurchfluss) {
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
			//if (this.PlannedMhHeat >= this.PlannedMhCool) {
				if (Math.Round(this.PlannedMhHeat, 1) > HithermProduct.ConfigMaxDurchfluss) {
					//errorMsg += "Durchfluß bei Heizung zu groß (" + Math.Round(this.PlannedMhHeat, 1).ToString() + "kg/h > " + HithermProduct.ConfigMaxDurchfluss.ToString() + "kg/h)\n";
					this.lastErrorMsg += "Durchfluß bei zu groß (" + Math.Round(this.PlannedMhHeat, 1).ToString() + "kg/h > " + HithermProduct.ConfigMaxDurchfluss.ToString() + "kg/h)\n";
				}
				/*} else {
					if (Math.Round(this.PlannedMhCool, 1) > HithermProduct.ConfigMaxDurchfluss) {
						errorMsg += "Durchfluß bei Kühlung zu groß (" + Math.Round(this.PlannedMhCool, 1).ToString() + "kg/h > " + HithermProduct.ConfigMaxDurchfluss.ToString() + "kg/h)\n";
					}
				}*/
				//if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > HithermProduct.ConfigMaxPressureLost / 100) {
					//errorMsg += "Druckverlust bei Heizung zu groß (" + Math.Round(this.PlannedDeltaRhoHeat, 2).ToString() + "mbar > " + (HithermProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
					this.lastErrorMsg += "Druckverlust zu groß (" + Math.Round(this.PlannedDeltaRhoHeat, 2).ToString() + "mbar > " + (HithermProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
				}
				/*} else {
					if (Math.Round(this.PlannedDeltaRhoCool, 2) > HithermProduct.ConfigMaxPressureLost / 100) {
						errorMsg += "Druckverlust bei Kühlung zu groß (" + Math.Round(this.PlannedDeltaRhoCool, 1).ToString() + "mbar > " + (HithermProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
					}
				}*/
			if (this.lastErrorMsg.Length == 0) {
				this.lastErrorMsg = null;
			}

			return true;
		}

		public override float PlannedFloorArea {
			get { return 0; }
			set { }
		}

		public override float PlannedWallArea {
			get { return this.PlannedNetArea; }
			set { }
		}

		public override float PlannedCeilingArea {
			get { return 0; }
			set { }
		}

		public override double PlannedCoolLoad {
			get { return 0; }
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

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {

		}
	}
	
}
