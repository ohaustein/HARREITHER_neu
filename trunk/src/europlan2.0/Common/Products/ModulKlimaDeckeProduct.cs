using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Modul Klima-Decke")]
	public class ModulKlimaDeckeProduct : Product {

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 80;
		private static int quickDimensioningCoolPowerPerSquareMeter = 80;
		private static bool canHeat = true;
		private static bool canCool = true;

		// planning
		private static double su0 = 0.045; /* Mindestüberdeckung fix */
		private static double alpha0 = 10.8; /* Fixwert für FBH fix */
		private static double alphaDh = 6.5; /* für FBK fix */
		private static double alphaDk = 10.8; /* für FBH fix */
		private static double lambdaU0 = 1; /* fix */
		private static double lambdaE = 0.32; /* Estrichleitfähigkeit bzw Leitfähigkeit Lastausgleichsschicht, fix */
		private static double su = 0.01; /* Überdeckung Beplankung */
		private static double lambdaU = 0.32; /* Wärmeleitfähigkeit der Überdeckung */
		private static double rLambdaDecke = 0.11; /* Deckenschicht 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
		private static double rLambdaDach = 0.0; /* Deckenschicht; durch echte Konstruktion ersetzen! */
		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double atmt = 1.06; /* Fixwert laut Norm */
		private static double b = 6.5; /* Fixwert laut Norm */

		private float plannedArea = 0;
		//private float plannedFloorArea = 0;
		private float plannedAreaUnheated = 0;
		private Construction plannedCeilingConstruction = null;
		private Construction plannedInsulationConstruction = null;

		//  !!!!!!!!!!! changes must be also applied in SystemParametersPanel.cs !!!!!!!!!!!
		//private static bool useHarreitherNorm = true;
		//private static double maxFloorTempHarreither = 27;
		//private static double maxFloorTempEn1264 = 29;
		private static int maxPressureLost = 15000;
		private static int maxDurchfluss = 240;
		private static int maxModulesInRow = 20;
		private static int maxModulesInParallel = 6;
		private static int maxModulesInCircuit = 50;


		public ModulKlimaDeckeProduct() {

		}

		protected ModulKlimaDeckeProduct(ModulKlimaDeckeProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public override void StaticInitialize() {
			quickDimensioningHeatPowerPerSquareMeter = 80;
			quickDimensioningCoolPowerPerSquareMeter = 80;
			canHeat = true;
			canCool = true;
			//useHarreitherNorm = true;
			maxPressureLost = 15000;
			maxDurchfluss = 240;
			maxModulesInRow = 20;
			maxModulesInParallel = 6;
			maxModulesInCircuit = 50;
		}

		public override Product Clone(Room room) {
			ModulKlimaDeckeProduct product = new ModulKlimaDeckeProduct(this);
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
		public static double ConfigSu0 {
			get { return su0; }
			set { su0 = value; }
		}

		[ProductParameter]
		public static double ConfigAlpha0 {
			get { return alpha0; }
			set { alpha0 = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaDk {
			get { return alphaDk; }
			set { alphaDk = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaDh {
			get { return alphaDh; }
			set { alphaDh = value; }
		}

		[ProductParameter]
		public static double ConfigLambdaU0 {
			get { return lambdaU0; }
			set { lambdaU0 = value; }
		}

		[ProductParameter]
		public static double ConfigLambdaE {
			get { return lambdaE; }
			set { lambdaE = value; }
		}

		[ProductParameter]
		public static double ConfigSu {
			get { return su; }
			set { su = value; }
		}

		[ProductParameter]
		public static double ConfigLambdaU {
			get { return lambdaU; }
			set { lambdaU = value; }
		}

		[ProductParameter]
		public static double ConfigRLambdaDecke {
			get { return rLambdaDecke; }
			set { rLambdaDecke = value; }
		}

		[ProductParameter]
		public static double ConfigRLambdaDach {
			get { return rLambdaDach; }
			set { rLambdaDach = value; }
		}

		[ProductParameter]
		public static double ConfigC {
			get { return c; }
			set { c = value; }
		}

		[ProductParameter]
		public static double ConfigAtmt {
			get { return atmt; }
			set { atmt = value; }
		}

		[ProductParameter]
		public static double ConfigB {
			get { return b; }
			set { b = value; }
		}

		[ProductParameter]
		public static int ConfigMaxPressureLost {
			get { return ModulKlimaDeckeProduct.maxPressureLost; }
			set { ModulKlimaDeckeProduct.maxPressureLost = value; }
		}

		[ProductParameter]
		public static int ConfigMaxDurchfluss {
			get { return ModulKlimaDeckeProduct.maxDurchfluss; }
			set { ModulKlimaDeckeProduct.maxDurchfluss = value; }
		}

		[ProductParameter]
		public static int ConfigMaxModulesInRow {
			get { return ModulKlimaDeckeProduct.maxModulesInRow; }
			set { ModulKlimaDeckeProduct.maxModulesInRow = value; }
		}

		[ProductParameter]
		public static int ConfigMaxModulesInParallel {
			get { return ModulKlimaDeckeProduct.maxModulesInParallel; }
			set { ModulKlimaDeckeProduct.maxModulesInParallel = value; }
		}

		[ProductParameter]
		public static int ConfigModulesInCircuit {
			get { return ModulKlimaDeckeProduct.maxModulesInCircuit; }
			set { ModulKlimaDeckeProduct.maxModulesInCircuit = value; }
		}
		#endregion Product Parameters

		public override int GetDefaultQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 18);
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			if (this.AssociatedRoom != null) {
				return this.AssociatedRoom.Area * Project.Instance.QuickDimensioning.CeilingAllocation / 100;
			}
			return 0;
		}

		public override float QuickDimensioningMaximumArea {
			get {
				if (this.AssociatedRoom != null) {
					return this.AssociatedRoom.Area;
				}
				return 0;
			}
		}

		public override string Name {
			get { return "Modul Klima-Decke"; }
		}

		public override string QuickDimensioningName {
			get { return "Modul\nKlima\nDecke\n(m²)"; }
		}

		/// <summary>
		/// The full name of this product
		/// </summary>
		public override string FullName {
			get { return Name; }
		}

		public override ProductType Type {
			get { return ProductType.DH; }
		}

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg) {
			this.incompleteCalculation = false;
			if (this.plannedCeilingConstruction == null || this.plannedInsulationConstruction == null) {
				errorMsg = "Fehlende Eingaben: ";
				if (plannedCeilingConstruction == null) {
					errorMsg += "Fußbodenkonstruktion, ";
				}
				if (plannedInsulationConstruction == null) {
					errorMsg += "Wärmedämmkonstruktion, ";
				}
				if (PlannedConnection == null) {
					errorMsg += "Heizkreisanschluß, ";
				}
				errorMsg = errorMsg.Substring(0, errorMsg.Length - 2);
				this.incompleteCalculation = true;
				return false;
			}


			double areaRemovedDueConnection = 0;
			double heatLoadRemovedDueConnection = 0;
			double coolLoadRemovedDueConnection = 0;
			List<ConnectionPipe> connectionPipes = new List<ConnectionPipe>();
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
							if (cp != null && cp.ConnectionThrough != null && cp.ConnectionThrough.Product == this) {
								connectionPipes.Add(cp);
								areaRemovedDueConnection += cp.AreaTotal;
								heatLoadRemovedDueConnection += cp.HeatLoadTotal;
								coolLoadRemovedDueConnection += cp.CoolLoadTotal;
							}
						}
					}
				}
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

			int i = 0;
			foreach (ModulDeckeCircuit mc in this.circuits) {
				mc.NrOfCircuit = i;
				mc.ModulKlimaDeckeProduct = this;
				mc.PipeLengthVorlaufTotal = vorlaufTotal[i];
				mc.PipeLengthVorlaufNotIsolated = vorlaufNotIsolated[i];
				mc.PipeLengthRuecklaufTotal = ruecklaufTotal[i];
				mc.PipeLengthRuecklaufNotIsolated = ruecklaufNotIsolated[i];
				mc.PipeLengthVorlaufWithoutOtherProductTotal = vorlaufWithoutOtherProductTotal[i];
				mc.PipeLengthVorlaufWithoutOtherProductNotIsolated = vorlaufWithoutOtherProductNotIsolated[i];
				mc.PipeLengthRuecklaufWithoutOtherProductTotal = ruecklaufWithoutOtherProductTotal[i];
				mc.PipeLengthRuecklaufWithoutOtherProductNotIsolated = ruecklaufWithoutOtherProductNotIsolated[i];
				mc.Calculate();
				i++;
			}

			errorMsg = "";
			foreach (ModulDeckeCircuit c in this.circuits) {
				int moduleCount = 0;
				foreach (KlimaFlaechenList row in c.Rows) {
					moduleCount += row.List.Count;
				}
				if (moduleCount > ModulKlimaBodenProduct.ConfigModulesInCircuit) {
					//errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält mehr als 50 Module\n";
					errorMsg += "Der Heizkreis HK" + (c.NrOfCircuit + 1).ToString() + " enthält zu viele Module (" + moduleCount + " > " + ModulKlimaBodenProduct.ConfigModulesInCircuit.ToString() + ")\n";
				}
			}
			//double maxTemp = double.MinValue;
			//foreach (ModulDeckeCircuit c in this.circuits) {
			//    if (c.C_FloorTempHeat > maxTemp) {
			//        maxTemp = c.C_FloorTempHeat;
			//    }
			//}
			//if (Math.Round(maxTemp, 1) > (ModulKlimaBodenProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264)) {
			//    errorMsg += "Oberflächentemperatur zu groß (" + Math.Round(maxTemp, 1) + "°C > " + Math.Round((EurovalProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264), 1) + "°C)\n";
			//}
			if (this.PlannedMhHeat >= this.PlannedMhCool) {
				if (Math.Round(this.PlannedMhHeat, 1) > ModulKlimaBodenProduct.ConfigMaxDurchfluss) {
					errorMsg += "Durchfluß bei Heizung zu groß (" + Math.Round(this.PlannedMhHeat, 1).ToString() + "kg/h > " + ModulKlimaBodenProduct.ConfigMaxDurchfluss.ToString() + "kg/h)\n";
				}
			} else {
				if (Math.Round(this.PlannedMhCool, 1) > ModulKlimaBodenProduct.ConfigMaxDurchfluss) {
					errorMsg += "Durchfluß bei Kühlung zu groß (" + Math.Round(this.PlannedMhCool, 1).ToString() + "kg/h > " + ModulKlimaBodenProduct.ConfigMaxDurchfluss.ToString() + "kg/h)\n";
				}
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > ModulKlimaBodenProduct.ConfigMaxPressureLost / 100) {
					errorMsg += "Druckverlust bei Heizung zu groß (" + Math.Round(this.PlannedDeltaRhoHeat, 2).ToString() + "mbar > " + (ModulKlimaBodenProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
				}
			} else {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > ModulKlimaBodenProduct.ConfigMaxPressureLost / 100) {
					errorMsg += "Druckverlust bei Kühlung zu groß (" + Math.Round(this.PlannedDeltaRhoCool, 1).ToString() + "mbar > " + (ModulKlimaBodenProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
				}
			}
			if (errorMsg.Length == 0) {
				errorMsg = null;
			}

			return true;
		}

		public override float PlannedCeilingArea {
			get { return this.plannedArea; }
			set { this.plannedArea = value; }
		}

		[XmlIgnore]
		public double CoveredCeilingArea {
			get {
				double area = 0;
				foreach (ModulDeckeCircuit mc in this.circuits) {
					area += mc.ModulArea;
				}
				return area;
			}

		}

		/// <summary>
		/// The percentage of the total room area that is occupied by the planned area.
		/// </summary>
		[XmlIgnore]
		public float PlannedCeilingAreaPercentage {
			get { return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedCeilingArea * 100 / this.AssociatedRoom.Area); }
			set { this.PlannedCeilingArea = (float)(this.AssociatedRoom.Area * value / 100); }
		}

		/// <summary>
		/// The area which is planned unheated.
		/// This area is subtracted from the planned area for calculation.
		/// </summary>
		public float PlannedAreaUnheated {
			get { return this.plannedAreaUnheated; }
			set { this.plannedAreaUnheated = value; }
		}

		public override float PlannedWallArea {
			get { return 0; }
			set { }
		}

		public override float PlannedFloorArea {
			get { return 0; }
			set { }
		}

		/// <summary>
		/// The total cool load that is emmited in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public override double PlannedCoolLoad {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (ModulDeckeCircuit mbc in this.circuits) {
					if (!mbc.QFbhTotalCool.Equals(double.NaN)) {
						value += mbc.QFbhTotalCool;
					}
				}
				return value;
			}
		}

		/// <summary>
		/// The total heat load that is emmited in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public override double PlannedHeatLoad {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (ModulDeckeCircuit c in this.circuits) {
					if (!c.QFbhTotalHeat.Equals(double.NaN)) {
						value += c.QFbhTotalHeat;
					}
				}
				return value;
			}
		}

		public override float PlannedNetArea {
			get { return this.PlannedCeilingArea - this.PlannedAreaUnheated; }
		}

		/// <summary>
		/// The id of the planned floor construction for serialization
		/// </summary>
		public string PlannedCeilingConstructionId {
			get { return (this.plannedCeilingConstruction == null ? "" : this.plannedCeilingConstruction.Id); }
			set { this.plannedCeilingConstruction = Project.Instance.Config.GetConstruction(value); }
		}

		/// <summary>
		/// The id of the planned insulation construction for serialization
		/// </summary>
		public string PlannedInsulationConstructionId {
			get { return (this.plannedInsulationConstruction == null ? "" : this.plannedInsulationConstruction.Id); }
			set { this.plannedInsulationConstruction = Project.Instance.Config.GetConstruction(value); }
		}

		/// <summary>
		/// The planned ceiling contruction
		/// </summary>
		[XmlIgnore]
		public Construction PlannedCeilingConstruction {
			get { return this.plannedCeilingConstruction; }
			set { this.plannedCeilingConstruction = value; }
		}

		/// <summary>
		/// The planned insulation construction
		/// </summary>
		[XmlIgnore]
		public Construction PlannedInsulationConstruction {
			get { return this.plannedInsulationConstruction; }
			set { this.plannedInsulationConstruction = value; }
		}

		/// <summary>
		/// The r-value of the planned ceiling construction
		/// </summary>
		[XmlIgnore]
		public float PlannedCeilingConstructionRValue {
			get { return (this.plannedCeilingConstruction == null ? 0 : this.plannedCeilingConstruction.RValue); }
		}

		/// <summary>
		/// The r-value of the planned insulation construction
		/// </summary>
		[XmlIgnore]
		public float PlannedInsulationConstructionRValue {
			get { return (this.plannedInsulationConstruction == null ? 0 : this.plannedInsulationConstruction.RValue); }
		}

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_21MM; }
		}

		/// <summary>
		/// The r-value of the planned ceiling construction
		/// </summary>
		[XmlIgnore]
		public override float PlannedInsideConstructionRValue {
			get { return (this.plannedCeilingConstruction == null ? 0 : this.plannedCeilingConstruction.RValue); }
		}

		[XmlIgnore]
		public override bool HasInsideConstruction {
			get { return this.plannedCeilingConstruction != null; }
		}

		/// <summary>
		/// The r-value of the planned insulation construction
		/// </summary>
		[XmlIgnore]
		public override float PlannedOutsideConstructionRValue {
			get { return (this.plannedInsulationConstruction == null ? 0 : this.plannedInsulationConstruction.RValue); }
		}

		[XmlIgnore]
		public override bool HasOutsideConstruction {
			get { return this.plannedInsulationConstruction != null; }
		}
	}
	
}
