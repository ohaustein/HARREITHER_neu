using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Modul Klima-Boden")]
	public class ModulKlimaBodenProduct : Product {

		private static double lambdaE = 60.0;
		private static double su = 0.002; /* Estrichüberdeckung bzw. Überdeckung Lastausgleich */
		private static double alpha0 = 10.8; /* Fixwert für FBH fix??? */
		private static double alphaFbk = 6.5; //6.5; /* für FBK fix??? */
		private static double alphaFbh = 10.8; /* für FBH fix??? */
		private static double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
		private static double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */
		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */

		private float plannedArea = 0;
		private float plannedFloorArea = 0;
		private float plannedAreaUnheated = 0;
		private Construction plannedFloorConstruction = null;
		private Construction plannedInsulationConstruction = null;

		//  !!!!!!!!!!! changes must be also applied in SystemParametersPanel.cs !!!!!!!!!!!
		private static bool useHarreitherNorm = true;
		private static double maxFloorTempHarreither = 27;
		private static double maxFloorTempEn1264 = 29;
		private static int maxPressureLost = 15000;
		private static int maxDurchfluss = 240;
		private static int maxModulesInRow = 20;
		private static int maxModulesInParallel = 6;
		private static int maxModulesInCircuit = 50;


		public ModulKlimaBodenProduct() {

		}

		protected ModulKlimaBodenProduct(ModulKlimaBodenProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 50;
			quickDimensioningCoolPowerPerSquareMeter = 50;
			canHeat = true;
			canCool = false;
		}

		public override void StaticInitialize() {
			useHarreitherNorm = true;
			maxPressureLost = 15000;
			maxDurchfluss = 240;
			maxModulesInRow = 20;
			maxModulesInParallel = 6;
			maxModulesInCircuit = 50;
		}

		public override Product Clone(Room room) {
			ModulKlimaBodenProduct product = new ModulKlimaBodenProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override int GetDefaultQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 18);
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			if (this.AssociatedRoom != null) {
				return this.AssociatedRoom.Area;
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
			get { return "Modul Klima-Boden"; }
		}

		public override string QuickDimensioningName {
			get { return "Modul\nKlima-\nBoden\n(m²)"; }
		}

		/// <summary>
		/// The full name of this product
		/// </summary>
		public override string FullName {
			get { return Name; }
		}

		public override ProductType Type {
			get { return ProductType.FBH; }
		}

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg) {
			this.incompleteCalculation = false;
			if (this.plannedFloorConstruction == null || this.plannedInsulationConstruction == null) {
				errorMsg = "Fehlende Eingaben: ";
				if (plannedFloorConstruction == null) {
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
			foreach (ModulBodenCircuit mc in this.circuits) {
				mc.NrOfCircuit = i;
				mc.ModulKlimaBodenProduct = this;
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
			foreach (ModulBodenCircuit c in this.circuits) {
				if (c.Rows.Count > ModulKlimaBodenProduct.ConfigMaxModulesInParallel) {
					//errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält mehr als 6 parallele Modulreihen\n";
					errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält zu viele parallele Modulreihen (" + c.Rows.Count.ToString() + " > " + ModulKlimaBodenProduct.ConfigMaxModulesInParallel.ToString() + ")\n";
				}
			}
			foreach (ModulBodenCircuit c in this.circuits) {
				int maxModuleCount = 0;
				foreach (KlimaFlaechenList row in c.Rows) {
					//if (row.List.Count > 20) {
					//    errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält mehr als 20 Module in Serie\n";
					//    break;
					//}
					if (row.List.Count > maxModuleCount) {
						maxModuleCount = row.List.Count;
					}
				}
				if (maxModuleCount > ModulKlimaBodenProduct.ConfigMaxModulesInRow) {
					errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält zu viele Module in Serien (" + maxModuleCount + " > " + ModulKlimaBodenProduct.ConfigMaxModulesInRow.ToString() + ")\n";
				}
			}
			foreach (ModulBodenCircuit c in this.circuits) {
				int moduleCount = 0;
				foreach (KlimaFlaechenList row in c.Rows) {
					moduleCount += row.List.Count;
				}
				if (moduleCount > ModulKlimaBodenProduct.ConfigModulesInCircuit) {
					//errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält mehr als 50 Module\n";
					errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält zu viele Module (" + moduleCount + " > " + ModulKlimaBodenProduct.ConfigModulesInCircuit.ToString() + ")\n";
				}
			}
			double maxTemp = double.MinValue;
			foreach (ModulBodenCircuit c in this.circuits) {
				if (c.C_FloorTempHeat > maxTemp) {
					maxTemp = c.C_FloorTempHeat;
				}
			}
			if (Math.Round(maxTemp, 1) > (ModulKlimaBodenProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264)) {
				errorMsg += "Oberflächentemperatur zu groß (" + Math.Round(maxTemp, 1) + "°C > " + Math.Round((EurovalProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264), 1) + "°C)\n";
			}
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

		public override float PlannedFloorArea {
			get { return this.plannedArea; }
			set { this.plannedArea = value; }
		}

		[XmlIgnore]
		public double CoveredFloorArea {
			get {
				double area = 0;
				foreach (ModulBodenCircuit mc in this.circuits) {
					area += mc.ModulArea;
				}
				return area;
			}

		}

		/// <summary>
		/// The percentage of the total room area that is occupied by the planned area.
		/// </summary>
		[XmlIgnore]
		public float PlannedFloorAreaPercentage {
			/*get { return (this.AvailableFloorArea <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AvailableFloorArea); }
			set { this.PlannedFloorArea = (float)(this.AvailableFloorArea * value / 100); }*/
			get { return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AssociatedRoom.Area); }
			set { this.PlannedFloorArea = (float)(this.AssociatedRoom.Area * value / 100); }
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

		public override float PlannedRoofArea {
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
				foreach (ModulBodenCircuit mbc in this.circuits) {
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
				foreach (ModulBodenCircuit c in this.circuits) {
					if (!c.QFbhTotalHeat.Equals(double.NaN)) {
						value += c.QFbhTotalHeat;
					}
				}
				return value;
			}
		}

		public override float PlannedNetArea {
			get { return this.PlannedFloorArea - this.PlannedAreaUnheated; }
		}

		/// <summary>
		/// The id of the planned floor construction for serialization
		/// </summary>
		public string PlannedFloorConstructionId {
			get { return (this.plannedFloorConstruction == null ? "" : this.plannedFloorConstruction.Id); }
			set { this.plannedFloorConstruction = Project.Instance.Config.GetConstruction(value); }
		}

		/// <summary>
		/// The id of the planned insulation construction for serialization
		/// </summary>
		public string PlannedInsulationConstructionId {
			get { return (this.plannedInsulationConstruction == null ? "" : this.plannedInsulationConstruction.Id); }
			set { this.plannedInsulationConstruction = Project.Instance.Config.GetConstruction(value); }
		}

		/// <summary>
		/// The planned floor contruction
		/// </summary>
		[XmlIgnore]
		public Construction PlannedFloorConstruction {
			get { return this.plannedFloorConstruction; }
			set { this.plannedFloorConstruction = value; }
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
		/// The r-value of the planned floor construction
		/// </summary>
		[XmlIgnore]
		public float PlannedFloorConstructionRValue {
			get { return (this.plannedFloorConstruction == null ? 0 : this.plannedFloorConstruction.RValue); }
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

		[ProductParameter]
		public static bool ConfigUseHarreitherNorm {
			get { return ModulKlimaBodenProduct.useHarreitherNorm; }
			set { ModulKlimaBodenProduct.useHarreitherNorm = value; }
		}

		[ProductParameter]
		public static double ConfigMaxFloorTempHarreither {
			get { return ModulKlimaBodenProduct.maxFloorTempHarreither; }
			set { ModulKlimaBodenProduct.maxFloorTempHarreither = value; }
		}

		[ProductParameter]
		public static double ConfigMaxFloorTempEn1264 {
			get { return ModulKlimaBodenProduct.maxFloorTempEn1264; }
			set { ModulKlimaBodenProduct.maxFloorTempEn1264 = value; }
		}

		[ProductParameter]
		public static int ConfigMaxPressureLost {
			get { return ModulKlimaBodenProduct.maxPressureLost; }
			set { ModulKlimaBodenProduct.maxPressureLost = value; }
		}

		[ProductParameter]
		public static int ConfigMaxDurchfluss {
			get { return ModulKlimaBodenProduct.maxDurchfluss; }
			set { ModulKlimaBodenProduct.maxDurchfluss = value; }
		}

		[ProductParameter]
		public static int ConfigMaxModulesInRow {
			get { return ModulKlimaBodenProduct.maxModulesInRow; }
			set { ModulKlimaBodenProduct.maxModulesInRow = value; }
		}

		[ProductParameter]
		public static int ConfigMaxModulesInParallel {
			get { return ModulKlimaBodenProduct.maxModulesInParallel; }
			set { ModulKlimaBodenProduct.maxModulesInParallel = value; }
		}

		[ProductParameter]
		public static int ConfigModulesInCircuit {
			get { return ModulKlimaBodenProduct.maxModulesInCircuit; }
			set { ModulKlimaBodenProduct.maxModulesInCircuit = value; }
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
		public static double ConfigAlpha0 {
			get { return alpha0; }
			set { alpha0 = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaFbk {
			get { return alphaFbk; }
			set { alphaFbk = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaFbh {
			get { return alphaFbh; }
			set { alphaFbh = value; }
		}

		[ProductParameter]
		public static double ConfigRLambdaDecke {
			get { return rLambdaDecke; }
			set { rLambdaDecke = value; }
		}

		[ProductParameter]
		public static double ConfigRLambdaPutz {
			get { return rLambdaPutz; }
			set { rLambdaPutz = value; }
		}

		[ProductParameter]
		public static double ConfigC {
			get { return c; }
			set { c = value; }
		}

		/// <summary>
		/// The r-value of the planned floor construction
		/// </summary>
		[XmlIgnore]
		public override float PlannedInsideConstructionRValue {
			get { return (this.plannedFloorConstruction == null ? 0 : this.plannedFloorConstruction.RValue); }
		}

		[XmlIgnore]
		public override bool HasInsideConstruction {
			get { return this.plannedFloorConstruction != null; }
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
