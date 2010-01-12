using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Modul Klima-Boden")]
	public class ModulKlimaBodenProduct : Product {

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 50;
		private static int quickDimensioningCoolPowerPerSquareMeter = 50;
		private static bool canHeat = true;
		private static bool canCool = false;

		// planning
		private static double su0 = 0.045; /* Mindestüberdeckung fix */
		private static double alpha0 = 10.8; /* Fixwert für FBH fix */
		private static double lambdaU0 = 1; /* fix */
		private static double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
		private static double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */
		private static double atmt = 1.06; /* Fixwert laut Norm */
		private static double b = 6.5; /* Fixwert laut Norm */

		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static double v = 0.00000101; /* m²/s ... kinematische Viskosität */

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
		private static int maxModulesInCircuit = 40;
		private static double spreizungHeizMin = 4;
		private static double spreizungHeizMax = 12;
		private static double spreizungKuehlMin = 2;
		private static double spreizungKuehlMax = 5;

		private static double[] druckverlustModul_100_40 = { 0.2, 0.35, 0.65, 0.9, 1.25, 1.5, 1.8, 2.2, 2.6, 3, 3.6, 4.5, 5.4, 6.3, 7.2, 8.1, 9.1, 10, 11, 12, 13, 14, 15, 16.5, 17.8, 19, 20, 21.5, 23, 25 };

		private Nullable<int> requestedCircuits = null;
		private int requestedModulesDicht = 0;
		private int requestedModulesModulierend = 0;
		private int requestedModulesSonstige = 0;
		private double requestedSonstigeVerbindeLeitung = 0;

		public ModulKlimaBodenProduct() {

		}

		protected ModulKlimaBodenProduct(ModulKlimaBodenProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public new static void StaticInitialize() {
			Configuration userConfig = Configuration.UserTemplate;
			quickDimensioningHeatPowerPerSquareMeter = userConfig.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 50);
			quickDimensioningCoolPowerPerSquareMeter = userConfig.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 50);
			canHeat = userConfig.GetProductParameterAsBool<ModulKlimaBodenProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = userConfig.GetProductParameterAsBool<ModulKlimaBodenProduct>("ConfigQuickDimensioningCanCool", false);
			useHarreitherNorm = userConfig.GetProductParameterAsBool<ModulKlimaBodenProduct>("ConfigUseHarreitherNorm", true);
			maxPressureLost = userConfig.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigMaxPressureLost", 15000);
			maxDurchfluss = userConfig.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigMaxDurchfluss", 240);
			maxModulesInCircuit = userConfig.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigModulesInCircuit", 40);
			spreizungHeizMin = userConfig.GetProductParameterAsDouble<ModulKlimaBodenProduct>("ConfigSpreizungHeizMin", 4);
			spreizungHeizMax = userConfig.GetProductParameterAsDouble<ModulKlimaBodenProduct>("ConfigSpreizungHeizMax", 12);
			spreizungKuehlMin = userConfig.GetProductParameterAsDouble<ModulKlimaBodenProduct>("ConfigSpreizungKuehlMin", 2);
			spreizungKuehlMax = userConfig.GetProductParameterAsDouble<ModulKlimaBodenProduct>("ConfigSpreizungKuehlMax", 5);
		}

		public override Product Clone(Room room) {
			ModulKlimaBodenProduct product = new ModulKlimaBodenProduct(this);
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

		public static double ConfigAlphaFbk {
			get { return Product.ConfigAlphaDecke; }
		}

		public static double ConfigAlphaFbh {
			get { return Product.ConfigAlphaBoden; }
		}

		[ProductParameter]
		public static double ConfigLambdaU0 {
			get { return lambdaU0; }
			set { lambdaU0 = value; }
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
		public static bool ConfigUseHarreitherNorm {
			get { return useHarreitherNorm; }
			set { useHarreitherNorm = value; }
		}

		[ProductParameter]
		public static double ConfigMaxFloorTempHarreither {
			get { return maxFloorTempHarreither; }
			set { maxFloorTempHarreither = value; }
		}

		[ProductParameter]
		public static double ConfigMaxFloorTempEn1264 {
			get { return maxFloorTempEn1264; }
			set { maxFloorTempEn1264 = value; }
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
		public static double ConfigMaxMassenstrom {
			get { return maxDurchfluss; }
		}

		[ProductParameter]
		public static int ConfigModulesInCircuit {
			get { return maxModulesInCircuit; }
			set { maxModulesInCircuit = value; }
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
		public static double ConfigSpreizungKuehlMin {
			get { return spreizungKuehlMin; }
			set { spreizungKuehlMin = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungKuehlMax {
			get { return spreizungKuehlMax; }
			set { spreizungKuehlMax = value; }
		}

		[ProductParameter]
		public static double ConfigRho {
			get { return rho; }
			set { rho = value; }
		}

		[ProductParameter]
		public static double ConfigC {
			get { return c; }
			set { c = value; }
		}

		[ProductParameter]
		public static double ConfigV {
			get { return v; }
			set { v = value; }
		}

		[ProductParameter]
		public static string ConfigDruckverlustModul_100_40String {
			get {
				return ConvertArrayToString(druckverlustModul_100_40);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					druckverlustModul_100_40 = array;
				}
			}
		}
		public static double[] ConfigDruckverlustModul_100_40 {
			get { return druckverlustModul_100_40; }
			set { druckverlustModul_100_40 = value; }
		}
		#endregion Product Parameters

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

		public override string QuickDimensioningName {
			get { return QuickDimensioningNameStatic; }
		}

		public static string QuickDimensioningNameStatic {
			get { return "Modul\nKlima-\nBoden\n(m²)"; }
		}

		public override ProductType Type {
			get { return ProductType.FBH; }
		}

		/// <summary>
		/// The number of circuits the user requested for this product in the planning.
		/// If this property is null the optimal number of circuits will be calculated.
		/// </summary>
		public Nullable<int> RequestedCircuits {
			get { return this.requestedCircuits; }
			set {
				if (this.plannedConnection != null && this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					this.requestedCircuits = value.HasValue ? value.Value : 1;
				} else {
					this.requestedCircuits = value;
				}
			}
		}

		public int RequestedModulesDicht {
			get { return this.requestedModulesDicht; }
			set { this.requestedModulesDicht = value; }
		}

		public int RequestedModulesModulierend {
			get { return this.requestedModulesModulierend; }
			set { this.requestedModulesModulierend = value; }
		}

		public int RequestedModulesSonstige {
			get { return this.requestedModulesSonstige; }
			set { this.requestedModulesSonstige = value; }
		}

		public double RequestedSonstigeVerbindeLeitung {
			get { return this.requestedSonstigeVerbindeLeitung; }
			set { this.requestedSonstigeVerbindeLeitung = value; }
		}

		public int RequestedModulesTotal {
			get { return this.requestedModulesDicht + this.requestedModulesModulierend + this.requestedModulesSonstige; }
		}

		public override void CalculateHeatAndCoolFlow() {
			base.CalculateHeatAndCoolFlow();
			double spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			double spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			if (spreizungHeat > ModulKlimaBodenProduct.ConfigSpreizungHeizMax) {
				spreizungHeat = ModulKlimaBodenProduct.ConfigSpreizungHeizMax;
			}
			if (spreizungHeat < ModulKlimaBodenProduct.ConfigSpreizungHeizMin) {
				spreizungHeat = ModulKlimaBodenProduct.ConfigSpreizungHeizMin;
			}
			if (spreizungCool > ModulKlimaBodenProduct.ConfigSpreizungKuehlMax) {
				spreizungCool = ModulKlimaBodenProduct.ConfigSpreizungKuehlMax;
			}
			if (spreizungCool < ModulKlimaBodenProduct.ConfigSpreizungKuehlMin) {
				spreizungCool = ModulKlimaBodenProduct.ConfigSpreizungKuehlMin;
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
			this.incompleteCalculation = false;
			if (this.plannedFloorConstruction == null || this.plannedInsulationConstruction == null || this.plannedConnection == null) {
				this.lastErrorMsg = "Fehlende Eingaben: ";
				if (plannedFloorConstruction == null) {
					this.lastErrorMsg += "Fußbodenkonstruktion, ";
				}
				if (plannedInsulationConstruction == null) {
					this.lastErrorMsg += "Wärmedämmkonstruktion, ";
				}
				if (PlannedConnection == null) {
					this.lastErrorMsg += "Heizkreisanschluß, ";
				}
				this.lastErrorMsg = this.lastErrorMsg.Substring(0, this.lastErrorMsg.Length - 2);
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

			int cCount = this.RequestedCircuits.HasValue ? this.RequestedCircuits.Value : this.RequestedModulesTotal / 40;
			if (cCount > 12) {
				cCount = 12;
			}
			if (cCount <= 0) {
				cCount = 1;
			}
			this.CalculateHeatAndCoolFlow();
			bool found = false;
			while (!found) {
				int modulesPerCircuit = this.RequestedModulesTotal / cCount;
				int additionalModules = this.RequestedModulesTotal - modulesPerCircuit * cCount;

				int langeFittingePerCircuit = this.requestedModulesModulierend / cCount;
				int additionalLangeFittinge = this.requestedModulesModulierend - langeFittingePerCircuit * cCount;

				this.CorrectCircuits(cCount, false);

				int curCNr = 0;
				foreach (ModulBodenCircuit c in this.circuits) {
					c.Row.List.Clear();
					for (int i = 0; i < modulesPerCircuit + (curCNr < additionalModules ? 1 : 0); i++ ) {
						KlimaFlaechenModul m = new KlimaFlaechenModul(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40, i % 2 == 0 ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
						c.Row.List.Add(m);
					}
					c.ModulKlimaBodenProduct = this;
					c.NrOfCircuit = curCNr;
					c.PipeLengthVorlaufTotal = vorlaufTotal[curCNr];
					c.PipeLengthVorlaufNotIsolated = vorlaufNotIsolated[curCNr];
					c.PipeLengthRuecklaufTotal = ruecklaufTotal[curCNr];
					c.PipeLengthRuecklaufNotIsolated = ruecklaufNotIsolated[curCNr];
					c.PipeLengthVorlaufWithoutOtherProductTotal = vorlaufWithoutOtherProductTotal[curCNr];
					c.PipeLengthVorlaufWithoutOtherProductNotIsolated = vorlaufWithoutOtherProductNotIsolated[curCNr];
					c.PipeLengthRuecklaufWithoutOtherProductTotal = ruecklaufWithoutOtherProductTotal[curCNr];
					c.PipeLengthRuecklaufWithoutOtherProductNotIsolated = ruecklaufWithoutOtherProductNotIsolated[curCNr];
					c.LangeFittinge = langeFittingePerCircuit + (cCount - curCNr - 1 < additionalLangeFittinge ? 1 : 0);
					c.SonstigeVerbindeleitung = this.requestedSonstigeVerbindeLeitung / cCount;
					c.Calculate();
					curCNr++;
				}

				found = true;
				if (this.PlannedDeltaRhoHeat > ModulKlimaBodenProduct.ConfigMaxPressureLost / 100) {
					found = false;
				}
				if (this.PlannedDeltaRhoCool > ModulKlimaBodenProduct.ConfigMaxPressureLost / 100) {
					found = false;
				}
				if (this.PlannedMaxMhHeat > ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					found = false;
				}
				if (this.PlannedMaxMhCool > ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					found = false;
				}

				found = this.RequestedCircuits.HasValue || cCount >= 12 || found;

				if (!found) {
					cCount++;
				}
			}

			if (variableSpreizung && this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
				// Heizleistung veringern
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < ModulKlimaBodenProduct.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad) {
					this.plannedRuecklaufTempHeat -= 0.1;
					foreach (ModulBodenCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempHeat += 0.1;
				// Heizleistung erhöhen
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > ModulKlimaBodenProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < ModulKlimaBodenProduct.ConfigMaxPressureLost / 100 && this.PlannedMaxMhHeat < ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					this.plannedRuecklaufTempHeat += 0.1;
					foreach (ModulBodenCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				// Kühlleistung verringern
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < ModulKlimaBodenProduct.ConfigSpreizungKuehlMax && this.PlannedCoolLoad > requestedCoolLoad) {
					this.plannedRuecklaufTempCool += 0.1;
					foreach (ModulBodenCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempCool -= 0.1;
				// Kühlleistung erhöhen
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > ModulKlimaBodenProduct.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < ModulKlimaBodenProduct.ConfigMaxPressureLost / 100 && this.PlannedMaxMhCool < ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					this.plannedRuecklaufTempCool -= 0.1;
					foreach (ModulBodenCircuit c in this.circuits) {
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
			//foreach (ModulBodenCircuit c in this.circuits) {
			//    int moduleCount = 0;
			//    foreach (KlimaFlaechenList row in c.Rows) {
			//        moduleCount += row.List.Count;
			//    }
			//    if (moduleCount > ModulKlimaBodenProduct.ConfigModulesInCircuit) {
			//        //errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält mehr als 50 Module\n";
			//        errorMsg += "Der Heizkreis HK" + (c.NrOfCircuit + 1).ToString() + " enthält zu viele Module (" + moduleCount + " > " + ModulKlimaBodenProduct.ConfigModulesInCircuit.ToString() + ")\n";
			//    }
			//}

			if (this.PlannedModulArea > this.PlannedNetArea) {
				this.lastErrorMsg += "Die verplanten Module nehmen mehr Fläche in Anspruch als für dieses System zur Verfügung steht (" + Math.Round(this.PlannedModulArea, 1).ToString() + "m² > " + Math.Round(this.PlannedNetArea, 1).ToString() + "m²)\n";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeat, 1) > (ModulKlimaBodenProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264)) {
				this.lastErrorMsg += "Oberflächentemperatur zu groß (" + Math.Round(this.PlannedFloorTemperatureHeat, 1) + "°C > " + Math.Round((ModulKlimaBodenProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264), 1) + "°C)\n";
			}
			if (this.PlannedMaxMhHeat >= this.PlannedMaxMhCool) {
			    if (Math.Round(this.PlannedMaxMhHeat, 1) > ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					this.lastErrorMsg += "Durchfluß bei Heizung zu groß (" + Math.Round(this.PlannedMaxMhHeat, 1).ToString() + "kg/h > " + ModulKlimaBodenProduct.ConfigMaxMassenstrom.ToString() + "kg/h)\n";
			    }
			} else {
			    if (Math.Round(this.PlannedMaxMhCool, 1) > ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					this.lastErrorMsg += "Durchfluß bei Kühlung zu groß (" + Math.Round(this.PlannedMaxMhCool, 1).ToString() + "kg/h > " + ModulKlimaBodenProduct.ConfigMaxMassenstrom.ToString() + "kg/h)\n";
			    }
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool) {
			    if (Math.Round(this.PlannedDeltaRhoHeat, 2) > ModulKlimaBodenProduct.ConfigMaxPressureLost / 100) {
					this.lastErrorMsg += "Druckverlust bei Heizung zu groß (" + Math.Round(this.PlannedDeltaRhoHeat, 2).ToString() + "mbar > " + (ModulKlimaBodenProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
			    }
			} else {
			    if (Math.Round(this.PlannedDeltaRhoCool, 2) > ModulKlimaBodenProduct.ConfigMaxPressureLost / 100) {
					this.lastErrorMsg += "Druckverlust bei Kühlung zu groß (" + Math.Round(this.PlannedDeltaRhoCool, 1).ToString() + "mbar > " + (ModulKlimaBodenProduct.ConfigMaxPressureLost / 100).ToString() + "mbar)\n";
			    }
			}
			if (this.lastErrorMsg.Length == 0) {
				this.lastErrorMsg = null;
			}

			return true;
		}

		private string CorrectCircuits(int circuitCount, bool cleanupConnected) {
			if (this.circuits.Count > circuitCount) {
				this.circuits.RemoveRange(circuitCount, this.circuits.Count - circuitCount);
			}
			List<KeyValuePair<int, Circuit.CircuitConnection>> remove = new List<KeyValuePair<int, Circuit.CircuitConnection>>();
			if (cleanupConnected) {
				foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.connectedCircuits) {
					if (kvp.Key >= circuitCount) {
						remove.Add(kvp);
					}
				}
				foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in remove) {
					this.connectedCircuits.Remove(kvp.Key);
					foreach (KeyValuePair<int, Circuit.CircuitConnection> otherKvp in kvp.Value.OtherProduct.InverseConnectedCircuits) {
						if (otherKvp.Value.OtherCircuitId == kvp.Key) {
							kvp.Value.OtherProduct.InverseConnectedCircuits.Remove(otherKvp.Key);
							break;
						}
					}
				}
			}
			remove.Clear();
			foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.inverseConnectedCircuits) {
				if (kvp.Key >= circuitCount) {
					remove.Add(kvp);
				}
			}
			foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in remove) {
				this.inverseConnectedCircuits.Remove(kvp.Key);
				foreach (KeyValuePair<int, Circuit.CircuitConnection> otherKvp in kvp.Value.OtherProduct.ConnectedCircuits) {
					if (otherKvp.Value.OtherProduct == this && otherKvp.Value.OtherCircuitId == kvp.Key) {
						kvp.Value.OtherProduct.ConnectedCircuits.Remove(otherKvp.Key);
						break;
					}
				}
			}
			while (this.circuits.Count < circuitCount) {
				ModulBodenCircuit ec = new ModulBodenCircuit();
				ec.ModulKlimaBodenProduct = this;
				ec.NrOfCircuit = this.circuits.Count;
				if (this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					int j = 0;
					bool found = false;
					while (!found && j < this.plannedConnection.OtherProduct.Product.PlannedCircuitCount) {
						found = !this.plannedConnection.OtherProduct.Product.ConnectedCircuits.ContainsKey(j);
						j++;
					}
					if (!found) {
						return "Es sind nicht alle Heizkreise dieses Systems angeschloßen";
					}
					j--;
					this.plannedConnection.OtherProduct.Product.ConnectedCircuits.Add(j, new Circuit.CircuitConnection(this.plannedConnection.CircuitConnectionType, ec));
					this.inverseConnectedCircuits.Add(ec.NrOfCircuit, new Circuit.CircuitConnection(this.plannedConnection.CircuitConnectionType, this.plannedConnection.OtherProduct.Product.PlannedCircuits[j]));
				}
				this.circuits.Add(ec);
			}

			if (this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT && this.inverseConnectedCircuits.Count < this.circuits.Count) {
				for (int i = 0; i < this.circuits.Count; i++) {
					if (!this.inverseConnectedCircuits.ContainsKey(i)) {
						Circuit c = null;
						foreach (Circuit oc in this.plannedConnection.OtherProduct.Product.PlannedCircuits) {
							if (!this.plannedConnection.OtherProduct.Product.ConnectedCircuits.ContainsKey(oc.NrOfCircuit)) {
								c = oc;
								break;
							}
						}
						if (c != null) {
							this.inverseConnectedCircuits.Add(i, new Circuit.CircuitConnection(this.plannedConnection.CircuitConnectionType, c));
							this.plannedConnection.OtherProduct.Product.ConnectedCircuits.Add(c.NrOfCircuit, new Circuit.CircuitConnection(this.plannedConnection.CircuitConnectionType, this.circuits[i]));
						}
					}
				}
			}
			return null;
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

		public override float PlannedCeilingArea {
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

		[XmlIgnore]
		public double PlannedModulArea {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (ModulBodenCircuit c in this.circuits) {
					value += c.ModulArea;
				}
				return value;
			}
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

		[XmlIgnore]
		public override Construction PlannedInsideConstruction {
			get { return this.plannedFloorConstruction; }
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

		[XmlIgnore]
		public override Construction PlannedOutsideConstruction {
			get { return this.plannedInsulationConstruction; }
		}

		[XmlIgnore]
		public double PlannedHeatLoadPerSqM {
			get { return this.PlannedHeatLoad / this.PlannedNetArea; }
		}

		[XmlIgnore]
		public double PlannedCoolLoadPerSqM {
			get { return this.PlannedCoolLoad / this.PlannedNetArea; }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeat {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = this.circuits.Count == 0 ? 0 : double.MinValue;
				foreach (ModulBodenCircuit c in this.circuits) {
					if (c.C_FloorTempHeat > value) {
						value = c.C_FloorTempHeat;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureCool {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = this.circuits.Count == 0 ? 0 : double.MaxValue;
				foreach (ModulBodenCircuit c in this.circuits) {
					if (c.C_FloorTempCool < value) {
						value = c.C_FloorTempCool;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public override double WasserInhalt {
			get {
				// Euroval Anbindung
				// 21mm Anbindung
				double pipeEurovalLength = 0;
				double pipe21mmLength = 0;
				foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
					if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
						if (pipe.OnlyFirst) {
							pipe21mmLength += (pipe.Vorlauf + pipe.Ruecklauf);
						} else {
							pipe21mmLength += ((pipe.Vorlauf + pipe.Ruecklauf) * this.PlannedCircuitCount);
						}
					} else {
						if (pipe.OnlyFirst) {
							pipeEurovalLength += (pipe.Vorlauf + pipe.Ruecklauf);
						} else {
							pipeEurovalLength += ((pipe.Vorlauf + pipe.Ruecklauf) * this.PlannedCircuitCount);
						}
					}
				}

				double wasserInhalt = 0;
				foreach (ModulBodenCircuit c in this.circuits) {
					foreach (KlimaFlaechenModul modul in c.Row.List) {
						wasserInhalt += modul.WasserInhalt;
					}
				}

				return wasserInhalt + (pipeEurovalLength * EurovalProduct.rohrInnenA * 1000) + (pipe21mmLength * Product.rundrohr21mmInnenA * 1000);
			}
		}

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {

			// Modul
			Project.Instance.AddRequiredMaterial(requiredMaterial, "MK01", this.RequestedModulesTotal);

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
			pipe21mmLength += this.RequestedSonstigeVerbindeLeitung;
			Project.Instance.AddRequiredMaterial(requiredMaterial, "EV01", pipeEurovalLength);
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI51", pipe21mmLength);

			// Muffe
			if (circuit21mmOnlyFirstLength > 0) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", (circuit21mmOnlyFirstLength + circuit21mmAllLength) * 0.3);
				if (this.PlannedCircuitCount > 1) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", circuit21mmAllLength * 0.3 * (this.PlannedCircuitCount - 1));
				}
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", circuit21mmAllLength * 0.3 * this.PlannedCircuitCount);
			}

			// Winkel 90°
			if (circuit21mmOnlyFirstLength > 0) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI56", (circuit21mmOnlyFirstLength + circuit21mmAllLength) * 0.8);
				if (this.PlannedCircuitCount > 1) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HI56", circuit21mmAllLength * 0.8 * (this.PlannedCircuitCount - 1));
				}
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI56", circuit21mmAllLength * 0.8 * this.PlannedCircuitCount);
			}

			// Winkel 45°
			if (circuit21mmOnlyFirstLength > 0) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", GetWinkel45PerLfm(circuit21mmOnlyFirstLength + circuit21mmAllLength));
				if (this.PlannedCircuitCount > 1) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", GetWinkel45PerLfm(circuit21mmAllLength) * (this.PlannedCircuitCount - 1));
				}
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", GetWinkel45PerLfm(circuit21mmAllLength) * this.PlannedCircuitCount);
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", this.PlannedCircuitCount * 2);
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", this.RequestedModulesSonstige * 2);

			// Modulbögen
			Project.Instance.AddRequiredMaterial(requiredMaterial, "MK10", this.RequestedModulesDicht - 1);
			Project.Instance.AddRequiredMaterial(requiredMaterial, "MK11", this.RequestedModulesModulierend - 1);

			// Modulstreifen
			double streifen = Math.Ceiling(this.RequestedModulesModulierend * 1.5);
			Project.Instance.AddRequiredMaterial(requiredMaterial, "MK04", streifen);

			// nur bei Estrichkonstruktion
			if (this.HasInsideConstruction) {
				if (this.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_STAHL) ||
					this.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_STAHL)) {
					// Stahlbleche
					Project.Instance.AddRequiredMaterial(requiredMaterial, "MK21", Double.NegativeInfinity);
					Project.Instance.AddRequiredMaterial(requiredMaterial, "MK22", Double.NegativeInfinity);
				}
			}

			// Rohrführungsplatte
			if (this.RequestedSonstigeVerbindeLeitung > 0) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK05", Math.Ceiling(this.RequestedSonstigeVerbindeLeitung / 8));
			}

			// Modulniveauplatten
			double area = this.PlannedFloorArea - this.PlannedModulArea - (streifen * (0.945 * 0.096));
			Project.Instance.AddRequiredMaterial(requiredMaterial, "MK03", Math.Ceiling(area * 2));
			
		}

		private double GetWinkel45PerLfm(double lfm) {
			if (lfm < 20) {
				return 0;
			} else {
				return ((lfm / 10) - 1) * 2;
			}
		}

		public static void ReviseRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {

			// same amount left and right
			if (requiredMaterial.ContainsKey("MK01")) {
				int amount = (int)requiredMaterial["MK01"];
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK01", -1 * amount);
				if (amount % 2 != 0) {
					amount++;
				}
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK01", amount / 2);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK02", amount / 2);
			}

			// Modul-Kappe/T-Stück (manuell)
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI58", -2);
			Project.Instance.AddRequiredMaterial(requiredMaterial, "MK20", -2);

		}

		public override double Dichte {
			get { return ModulKlimaBodenProduct.ConfigRho; }
		}

		public override double Waermekapazitaet {
			get { return ModulKlimaBodenProduct.ConfigC; }
		}

		public override double Viskositaet {
			get { return ModulKlimaBodenProduct.ConfigV; }
		}
	}
	
}
