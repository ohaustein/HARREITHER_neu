using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Threading;
using Europlan.Licensing;
using WW.Math;
using WW.Math.Geometry;

namespace Europlan.Common {

	public class ModulKlimaBodenConnection : Connection {
		private KlimaFlaechenModul modul;

		internal ModulKlimaBodenConnection() {
		}

		public ModulKlimaBodenConnection(Distributor distributor, Product product, Circuit circuit, KlimaFlaechenModul modul, bool vorlauf) : base(distributor, product, circuit, vorlauf) {
			this.modul = modul;
		}
	}

	[Serializable()]
	[ProductName("Product_ModulKlimBodenName", "Product_ModulKlimBodenFullName")]
	public class ModulKlimaBodenProduct : Product, ProductWithInsulationConstruction {

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
		private float plannedAreaReduced = 0;
		private float plannedAreaUnheated = 0;
		private Construction plannedFloorConstruction = null;
		private Construction plannedInsulationConstruction = null;
		private string plannedFloorConstructionId = null;
		private string plannedInsulationConstructionId = null;

		//  !!!!!!!!!!! changes must be also applied in SystemParametersPanel.cs !!!!!!!!!!!
		private static bool useHarreitherNorm = true;
		private static double maxFloorTempHarreither = 27;
		private static double maxFloorTempEn1264 = 29;
		private static double maxNassraumTemp = 33;

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

		private ModulKlimaBodenConstruction graphConstruction = null;

		public ModulKlimaBodenProduct() {
			if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaBoden)) {
				throw new ProductNotLicensedException(this.GetType());
			}
		}

		protected ModulKlimaBodenProduct(ModulKlimaBodenProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public override string ImageKey {
            get { return "Fußbodenheizung.png"; }
		}

		public override string SelectedImageKey {
            get { return "Fußbodenheizung.png"; }
		}

		public override Product.CalculateModeEnum DefaultCalculateMode {
			get { return CalculateModeEnum.HEAT; }
		}

		public new static void StaticInitialize(Configuration config) {
			/*quickDimensioningHeatPowerPerSquareMeter = config.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 50);
			quickDimensioningCoolPowerPerSquareMeter = config.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 50);
			canHeat = config.GetProductParameterAsBool<ModulKlimaBodenProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = config.GetProductParameterAsBool<ModulKlimaBodenProduct>("ConfigQuickDimensioningCanCool", false);
			useHarreitherNorm = config.GetProductParameterAsBool<ModulKlimaBodenProduct>("ConfigUseHarreitherNorm", true);
			maxPressureLost = config.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigMaxPressureLost", 15000);
			maxDurchfluss = config.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigMaxDurchfluss", 240);
			maxModulesInCircuit = config.GetProductParameterAsInt<ModulKlimaBodenProduct>("ConfigModulesInCircuit", 40);
			spreizungHeizMin = config.GetProductParameterAsDouble<ModulKlimaBodenProduct>("ConfigSpreizungHeizMin", 4);
			spreizungHeizMax = config.GetProductParameterAsDouble<ModulKlimaBodenProduct>("ConfigSpreizungHeizMax", 12);
			spreizungKuehlMin = config.GetProductParameterAsDouble<ModulKlimaBodenProduct>("ConfigSpreizungKuehlMin", 2);
			spreizungKuehlMax = config.GetProductParameterAsDouble<ModulKlimaBodenProduct>("ConfigSpreizungKuehlMax", 5);*/
			Product.StaticInitialize<ModulKlimaBodenProduct>(config);
		}

		public static string GlobalNotificationMessage {
			get {
				return null;
			}
		}

		public override Product Clone(Room room) {
			ModulKlimaBodenProduct product = new ModulKlimaBodenProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		#region Product Parameters
		[BoolProductParameter(true)]
		public static bool ConfigQuickDimensioningCanHeat {
			get { return canHeat; }
			set { canHeat = value; }
		}
		public override bool QuickDimensioningCanHeat {
			get { return canHeat; }
		}

		[BoolProductParameter(false)]
		public static bool ConfigQuickDimensioningCanCool {
			get { return canCool; }
			set { canCool = value; }
		}
		public override bool QuickDimensioningCanCool {
			get { return canCool; }
		}

		[IntProductParameter(50)]
		public static int ConfigQuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
			set { quickDimensioningHeatPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
		}

		[IntProductParameter(50)]
		public static int ConfigQuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
			set { quickDimensioningCoolPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
		}

		[DoubleProductParameter(0.045)]
		public static double ConfigSu0 {
			get { return su0; }
			set { su0 = value; }
		}

		[DoubleProductParameter(10.8)]
		public static double ConfigAlpha0 {
			get { return alpha0; }
			set { alpha0 = value; }
		}

		public static double ConfigAlphaFbk {
			get { return Product.ConfigAlphaBodenCool; }
		}

		public static double ConfigAlphaFbh {
			get { return Product.ConfigAlphaBodenHeat; }
		}

		[DoubleProductParameter(1)]
		public static double ConfigLambdaU0 {
			get { return lambdaU0; }
			set { lambdaU0 = value; }
		}

		[DoubleProductParameter(0.11)]
		public static double ConfigRLambdaDecke {
			get { return rLambdaDecke; }
			set { rLambdaDecke = value; }
		}

		[DoubleProductParameter(0.02)]
		public static double ConfigRLambdaPutz {
			get { return rLambdaPutz; }
			set { rLambdaPutz = value; }
		}

		[DoubleProductParameter(1.06)]
		public static double ConfigAtmt {
			get { return atmt; }
			set { atmt = value; }
		}

		[DoubleProductParameter(6.5)]
		public static double ConfigB {
			get { return b; }
			set { b = value; }
		}
		
		[BoolProductParameter(true)]
		public static bool ConfigUseHarreitherNorm {
			get { return useHarreitherNorm; }
			set { useHarreitherNorm = value; }
		}

		[DoubleProductParameter(27)]
		public static double ConfigMaxFloorTempHarreither {
			get { return maxFloorTempHarreither; }
			set { maxFloorTempHarreither = value; }
		}

		[DoubleProductParameter(29)]
		public static double ConfigMaxFloorTempEn1264 {
			get { return maxFloorTempEn1264; }
			set { maxFloorTempEn1264 = value; }
		}

		[DoubleProductParameter(33)]
		public static double ConfigMaxNassraumTemp {
			get { return maxNassraumTemp; }
			set { maxNassraumTemp = value; }
		}

		[IntProductParameter(15000)]
		public static int ConfigMaxPressureLost {
			get { return maxPressureLost; }
			set { maxPressureLost = value; }
		}

		[IntProductParameter(240)]
		public static int ConfigMaxDurchfluss {
			get { return maxDurchfluss; }
			set { maxDurchfluss = value; }
		}
		public static double ConfigMaxMassenstrom {
			get { return maxDurchfluss; }
		}

		[IntProductParameter(40)]
		public static int ConfigModulesInCircuit {
			get { return maxModulesInCircuit; }
			set { maxModulesInCircuit = value; }
		}

		[DoubleProductParameter(4)]
		public static double ConfigSpreizungHeizMin {
			get { return spreizungHeizMin; }
			set { spreizungHeizMin = value; }
		}

		[DoubleProductParameter(12)]
		public static double ConfigSpreizungHeizMax {
			get { return spreizungHeizMax; }
			set { spreizungHeizMax = value; }
		}

		[DoubleProductParameter(2)]
		public static double ConfigSpreizungKuehlMin {
			get { return spreizungKuehlMin; }
			set { spreizungKuehlMin = value; }
		}

		[DoubleProductParameter(5)]
		public static double ConfigSpreizungKuehlMax {
			get { return spreizungKuehlMax; }
			set { spreizungKuehlMax = value; }
		}

		[DoubleProductParameter(1000)]
		public static double ConfigRho {
			get { return rho; }
			set { rho = value; }
		}

		[DoubleProductParameter(4.19)]
		public static double ConfigC {
			get { return c; }
			set { c = value; }
		}

		[DoubleProductParameter(0.00000101)]
		public static double ConfigV {
			get { return v; }
			set { v = value; }
		}

		[StringProductParameter("{0.2, 0.35, 0.65, 0.9, 1.25, 1.5, 1.8, 2.2, 2.6, 3, 3.6, 4.5, 5.4, 6.3, 7.2, 8.1, 9.1, 10, 11, 12, 13, 14, 15, 16.5, 17.8, 19, 20, 21.5, 23, 25}")]
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
				if (this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
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

		public double RequestedModulesDichtArea {
			get { return this.requestedModulesDicht * 0.4; }
		}

		public int RequestedModulesModulierend {
			get { return this.requestedModulesModulierend; }
			set { this.requestedModulesModulierend = value; }
		}

		public double RequestedModulesModulierendArea {
			get { return this.requestedModulesModulierend * 0.55; }
		}

		public int RequestedModulesSonstige {
			get { return this.requestedModulesSonstige; }
			set { this.requestedModulesSonstige = value; }
		}

		// TODO
		public double RequestedModulesSonstigeArea {
			get { return this.requestedModulesSonstige * 0.4 + this.requestedSonstigeVerbindeLeitung * 0.055; }
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
			this.requestedHeatLoad = requestedHeatLoad;
			this.requestedCoolLoad = requestedCoolLoad;
			this.incompleteCalculation = false;
			if (this.PlannedFloorConstruction == null || this.PlannedInsulationConstruction == null || this.PlannedConnection == null) {
				this.lastErrorMsg = EuroplanRes.ErrorMessage_FehlendeEingaben + " "; //"Fehlende Eingaben: ";
				if (PlannedFloorConstruction == null) {
					this.lastErrorMsg += EuroplanRes.ErrorMessage_FehlendeEingabenFussboden + ", "; //"Fußbodenkonstruktion, ";
				}
				if (PlannedInsulationConstruction == null) {
					this.lastErrorMsg += EuroplanRes.ErrorMessage_FehlendeEingabenDaemmung + ", "; //"Wärmedämmkonstruktion, ";
				}
				if (PlannedConnection == null) {
					this.lastErrorMsg += EuroplanRes.ErrorMessage_FehlendeEingabenHkAnschluss + ", "; //"Heizkreisanschluß, ";
				}
				this.lastErrorMsg = this.lastErrorMsg.Substring(0, this.lastErrorMsg.Length - 2);
				this.incompleteCalculation = true;
				return false;
			}

			if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				// TODO connect all circuits

				int c = this.PlannedConnection.OtherProduct.Product.PlannedCircuits.Count - this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Count;
				foreach (Circuit.CircuitConnection cc in this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Values) {
					if (cc.OtherProduct == this) {
						c++;
					}
				}
				if (c < this.circuits.Count) {
					/*if (this.requestedCircuits.HasValue) {
						// TODO reset circuits
					} else {*/
					//this.circuits.Clear();
					/*}*/
					this.lastErrorMsg = EuroplanRes.ErrorMessage_HkAnschluss; //"Es sind nicht alle Heizkreise dieses Systems angeschloßen";
					this.incompleteCalculation = true;
					return false;
				}
				bool userDefinedOk = true;
				foreach (Circuit.CircuitConnection cc in this.inverseConnectedCircuits.Values) {
					if (cc.OtherCircuit == null) {
						userDefinedOk = false;
					}
				}
				if (!userDefinedOk) {
					this.lastErrorMsg = EuroplanRes.ErrorMessage_HkAnschluss; //"Es sind nicht alle Heizkreise dieses Systems angeschloßen";
					this.incompleteCalculation = true;
					return false;
				}
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

			if (this.connectedCircuits.Count > cCount) {
				cCount = this.connectedCircuits.Count;
			}
			
			if (v > 12) {
				cCount = 12;
			}
			if (cCount <= 0) {
				cCount = 1;
			}
			this.CalculateHeatAndCoolFlow();
			bool graphical = (this.GraphicalMode.HasValue && this.GraphicalMode.Value);
			bool found = false;
			double measure = 1;
			if (this.AssociatedRoom != null && this.AssociatedRoom.AssociatedPlan != null && this.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
				measure = this.AssociatedRoom.AssociatedPlan.Measure.Value;
			}
			while (!found) {
				int modulesPerCircuit = this.RequestedModulesTotal / cCount;
				int additionalModules = this.RequestedModulesTotal - modulesPerCircuit * cCount;

				int langeFittingePerCircuit = this.requestedModulesModulierend / cCount;
				int additionalLangeFittinge = this.requestedModulesModulierend - langeFittingePerCircuit * cCount;

				int sonstigeModulePerCircuit = this.RequestedModulesSonstige / cCount;
				int additionalSonstigeModule = this.RequestedModulesSonstige - sonstigeModulePerCircuit * cCount;

				if (!graphical) {
					this.CorrectCircuits(cCount, false);
				}

				int curCNr = 0;
				foreach (ModulBodenCircuit c in this.circuits) {
					if (!graphical) {
						c.Row.List.Clear();
						for (int i = 0; i < modulesPerCircuit + (curCNr < additionalModules ? 1 : 0); i++) {
							KlimaFlaechenModul m = new KlimaFlaechenModul(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40, i % 2 == 0 ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
							c.Row.List.Add(m);
						}
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
					if (!graphical) {
						c.LangeFittinge = langeFittingePerCircuit + (cCount - curCNr - 1 < additionalLangeFittinge ? 1 : 0);
						c.SonstigeModule = sonstigeModulePerCircuit + (cCount - curCNr - 1 < additionalSonstigeModule ? 1 : 0);
						c.SonstigeVerbindeleitung = this.requestedSonstigeVerbindeLeitung / cCount;
					} else {
						int langeFittinge = 0;
						double verbindeleitung = 0;
						if (c.Links != null) {
							foreach (KlimaFlaechenModulVerbindung link in c.Links) {
								if (link.IsLangerFitting(measure)) {
									langeFittinge++;
								} else {
									verbindeleitung += link.GetLength(measure);
								}
							}
						}
						c.LangeFittinge = langeFittinge;
						c.SonstigeVerbindeleitung = verbindeleitung;
					}
					c.ReducedArea = this.PlannedAreaReduced / cCount;
					c.Calculate();
					curCNr++;
				}

				found = true;
				if (this.PlannedDeltaRhoHeat > ModulKlimaBodenProduct.ConfigMaxPressureLost / 100.0) {
					found = false;
				}
				if (this.PlannedDeltaRhoCool > ModulKlimaBodenProduct.ConfigMaxPressureLost / 100.0) {
					found = false;
				}
				if (this.PlannedMaxMhHeat > ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					found = false;
				}
				if (this.PlannedMaxMhCool > ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					found = false;
				}

				found = this.RequestedCircuits.HasValue || cCount >= 12 || this.PlannedConnectedProducts.Count > 0 || found;

				if (!found) {
					cCount++;
				}
			}

			if (variableSpreizung && this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
				double defSpreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
				double defSpreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
				// Heizleistung veringern
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < ModulKlimaBodenProduct.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad && this.PlannedSpreizungHeat < 1.2 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat -= 0.1;
					foreach (ModulBodenCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempHeat += 0.1;
				// Heizleistung erhöhen
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > ModulKlimaBodenProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < ModulKlimaBodenProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhHeat < ModulKlimaBodenProduct.ConfigMaxMassenstrom && this.PlannedSpreizungHeat > 0.8 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat += 0.1;
					foreach (ModulBodenCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				// Kühlleistung verringern
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < ModulKlimaBodenProduct.ConfigSpreizungKuehlMax && this.PlannedCoolLoad > requestedCoolLoad && this.PlannedSpreizungCool < 1.2 * defSpreizungCool) {
					this.plannedRuecklaufTempCool += 0.1;
					foreach (ModulBodenCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempCool -= 0.1;
				// Kühlleistung erhöhen
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > ModulKlimaBodenProduct.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < ModulKlimaBodenProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhCool < ModulKlimaBodenProduct.ConfigMaxMassenstrom && this.PlannedSpreizungCool > 0.8 * defSpreizungCool) {
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
			string newMsg;
			foreach (ModulBodenCircuit c in this.circuits) {
			    int moduleCount = 0;
                moduleCount += c.ModuleTotal;
			    if (moduleCount > ModulKlimaBodenProduct.ConfigModulesInCircuit) {
                    newMsg = EuroplanRes.ErrorMessage_ModulAnzahl;
                    newMsg = newMsg.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
                    newMsg = newMsg.Replace("%VALUE%", moduleCount.ToString());
                    newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaBodenProduct.ConfigModulesInCircuit.ToString());
                    this.lastErrorMsg += newMsg + "\n";
                }
			}

			if (this.CoveredFloorArea > this.PlannedNetArea) {
				newMsg = EuroplanRes.ErrorMessage_Modulflaeche;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.CoveredFloorArea, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.PlannedNetArea, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeat, 1) > this.MaxFloorTemp && this.requestedHeatLoad > 0) {
				newMsg = EuroplanRes.ErrorMessage_Oberflaechentemperatur;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedFloorTemperatureHeat, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.MaxFloorTemp, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			}
			if (this.PlannedMaxMhHeat >= this.PlannedMaxMhCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedMaxMhHeat, 1) > ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhHeat, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaBodenProduct.ConfigMaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedMaxMhCool, 1) > ModulKlimaBodenProduct.ConfigMaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhCool, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaBodenProduct.ConfigMaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > Math.Round(ModulKlimaBodenProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoHeat, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ModulKlimaBodenProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > Math.Round(ModulKlimaBodenProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoCool, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ModulKlimaBodenProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.PlannedRemoveArea > this.AvailableFloorArea) {
				newMsg = EuroplanRes.ErrorMessage_Anbindeleitung;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedRemoveArea, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.AvailableFloorArea, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
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
				if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					int j = 0;
					bool found = false;
					while (!found && j < this.PlannedConnection.OtherProduct.Product.PlannedCircuitCount) {
						found = !this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.ContainsKey(j);
						j++;
					}
					if (!found) {
						return EuroplanRes.ErrorMessage_HkAnschluss; // "Es sind nicht alle Heizkreise dieses Systems angeschloßen";
					}
					j--;
					this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Add(j, new Circuit.CircuitConnection(this.PlannedConnection.CircuitConnectionType, ec, false));
					this.inverseConnectedCircuits.Add(ec.NrOfCircuit, new Circuit.CircuitConnection(this.PlannedConnection.CircuitConnectionType, this.PlannedConnection.OtherProduct.Product.PlannedCircuits[j], false));
				}
				this.circuits.Add(ec);
			}

			if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT && this.inverseConnectedCircuits.Count < this.circuits.Count) {
				for (int i = 0; i < this.circuits.Count; i++) {
					if (!this.inverseConnectedCircuits.ContainsKey(i)) {
						Circuit c = null;
						foreach (Circuit oc in this.PlannedConnection.OtherProduct.Product.PlannedCircuits) {
							if (!this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.ContainsKey(oc.NrOfCircuit)) {
								c = oc;
								break;
							}
						}
						if (c != null) {
							this.inverseConnectedCircuits.Add(i, new Circuit.CircuitConnection(this.PlannedConnection.CircuitConnectionType, c, false));
							this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Add(c.NrOfCircuit, new Circuit.CircuitConnection(this.PlannedConnection.CircuitConnectionType, this.circuits[i], false));
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
					area += mc.CoveredArea;
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

		/// <summary>
		/// The area which is planned reduced (50%).
		/// Half of this area is subtracted from the planned area for calculation.
		/// </summary>
		public float PlannedAreaReduced {
			get { return this.plannedAreaReduced; }
			set { this.plannedAreaReduced = value; }
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
				if (this.incompleteCalculation || this.requestedCoolLoad == 0) {
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
				if (this.incompleteCalculation || this.requestedHeatLoad == 0) {
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
					value += c.HeatArea;
				}
				return value;
			}
		}

		/// <summary>
		/// The id of the planned floor construction for serialization
		/// </summary>
		public string PlannedFloorConstructionId {
			get { return this.PlannedFloorConstruction == null ? this.plannedFloorConstructionId : this.PlannedFloorConstruction.Id; }
			set {
				this.plannedFloorConstructionId = value;
				this.plannedFloorConstruction = null;
			}
		}

		/// <summary>
		/// The id of the planned insulation construction for serialization
		/// </summary>
		public string PlannedInsulationConstructionId {
			get { return this.PlannedInsulationConstruction == null ? this.plannedInsulationConstructionId : this.PlannedInsulationConstruction.Id; }
			set {
				this.plannedInsulationConstructionId = value;
				this.plannedInsulationConstruction = null;
			}
		}

		/// <summary>
		/// The planned floor contruction
		/// </summary>
		[XmlIgnore]
		public Construction PlannedFloorConstruction {
			get {
				if (this.plannedFloorConstructionId != null) {
					this.plannedFloorConstruction = Project.Instance.Config.GetConstruction(this.plannedFloorConstructionId);
					this.plannedFloorConstructionId = null;
				}
				return this.plannedFloorConstruction;
			}
			set {
				this.plannedFloorConstruction = value;
				this.plannedFloorConstructionId = null;
			}
		}

		/// <summary>
		/// The planned insulation construction
		/// </summary>
		[XmlIgnore]
		public Construction PlannedInsulationConstruction {
			get {
				if (this.plannedInsulationConstructionId != null) {
					this.plannedInsulationConstruction = Project.Instance.Config.GetConstruction(this.plannedInsulationConstructionId);
					this.plannedInsulationConstructionId = null;
				}
				return this.plannedInsulationConstruction;
			}
			set {
				this.plannedInsulationConstruction = value;
				this.plannedInsulationConstructionId = null;
			}
		}

		/// <summary>
		/// The r-value of the planned floor construction
		/// </summary>
		[XmlIgnore]
		public float PlannedFloorConstructionRValue {
			get { return (this.PlannedFloorConstruction == null ? 0 : this.PlannedFloorConstruction.RValue); }
		}

		/// <summary>
		/// The r-value of the planned insulation construction
		/// </summary>
		[XmlIgnore]
		public float PlannedInsulationConstructionRValue {
			get { return (this.PlannedInsulationConstruction == null ? 0 : this.PlannedInsulationConstruction.RValue); }
		}

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_21MM; }
		}

		/// <summary>
		/// The r-value of the planned floor construction
		/// </summary>
		[XmlIgnore]
		public override float PlannedInsideConstructionRValue {
			get { return (this.PlannedFloorConstruction == null ? 0 : this.PlannedFloorConstruction.RValue); }
		}

		[XmlIgnore]
		public override bool HasInsideConstruction {
			get { return this.PlannedFloorConstruction != null; }
		}

		[XmlIgnore]
		public override Construction PlannedInsideConstruction {
			get { return this.PlannedFloorConstruction; }
		}

		/// <summary>
		/// The r-value of the planned insulation construction
		/// </summary>
		[XmlIgnore]
		public override float PlannedOutsideConstructionRValue {
			get { return (this.PlannedInsulationConstruction == null ? 0 : this.PlannedInsulationConstruction.RValue); }
		}

		[XmlIgnore]
		public override bool HasOutsideConstruction {
			get { return this.PlannedInsulationConstruction != null; }
		}

		[XmlIgnore]
		public override Construction PlannedOutsideConstruction {
			get { return this.PlannedInsulationConstruction; }
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

		private double MaxFloorTemp {
			get {
				double maxTemp = ConfigUseHarreitherNorm ? ConfigMaxFloorTempHarreither : ConfigMaxFloorTempEn1264;
				return AssociatedRoom.IsNassraum ? Math.Max(maxTemp, ConfigMaxNassraumTemp) : maxTemp;
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

		public const string PLACEHOLDER_MK01_02 = "PLACEHOLDER_MK01/02";

        private struct ModulePosForCalc {
            public ModulePosForCalc(double x, double y, double height, double width) {
                this.x = x;
                this.y = y;
                this.height = height;
                this.width = width;
            }

            public double x;
            public double y;
            public double height;
            public double width;
        }

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {
			bool graphical = this.GraphicalMode.HasValue && this.GraphicalMode.Value;

			// Anbindeleitungen
			this.AddRequiredMaterialForConnections(requiredMaterial, false, graphical ? 0 : this.RequestedSonstigeVerbindeLeitung, !graphical);

			double graphVerbindung = 0;
			int graphWinkel = 0;
			int graphWinkel45 = 0;
			int graphBoegenKurz = 0;
			int graphBoegenLang = 0;

			if (!graphical) {
				// Modul
				Project.Instance.AddRequiredMaterial(requiredMaterial, ModulKlimaBodenProduct.PLACEHOLDER_MK01_02, this.RequestedModulesTotal);
			}else {
				double measure = this.AssociatedRoom.AssociatedPlan.Measure.Value;
				foreach (ModulBodenCircuit c in this.PlannedCircuits) {
					foreach (KlimaFlaechenModul modul in c.Row.List) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, modul.PartNumber, 1);
					}
					foreach (KlimaFlaechenModulVerbindung link in c.Links) {
						if (link.IsKurzerFitting(measure)) {
							graphBoegenKurz++;
						} else if (link.IsLangerFitting(measure)) {
							graphBoegenLang++;
						} else {
							graphVerbindung += link.GetLength(measure);
							if (link.Start != null && link.End != null) {
								graphWinkel45 += 2;
							} else {
								graphWinkel45++;
							}
						}
						graphWinkel += link.GetRequiredWinkel();
					}
				}
			}

			if (graphical) {
				// 90° Winkel
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI56", graphWinkel);
				// Verbindeleitungen
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI51", graphVerbindung);
			}

			// 45° Winkel
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", this.PlannedCircuitCount * 2);
			if (graphical) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", graphWinkel45);
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", this.RequestedModulesSonstige * 2);
			}

			// Modulbögen
			if (graphical) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK10", graphBoegenKurz);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK11", graphBoegenLang);
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK10", this.RequestedModulesDicht - 1);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK11", this.RequestedModulesModulierend - 1);
			}

			// Modulstreifen
            double streifen = 0;
            if (graphical && this.AssociatedRoom != null && this.AssociatedRoom.AssociatedPlan != null && this.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
                // TODO for graphical!!!
                double measure = this.AssociatedRoom.AssociatedPlan.Measure.Value;
                Dictionary<double, List<KlimaFlaechenModul>> moduleByRotation = new Dictionary<double,List<KlimaFlaechenModul>>();
                foreach (ModulBodenCircuit c in this.PlannedCircuits) {
                    foreach (KlimaFlaechenModul modul in c.Row.List) {
                        double roundedRotation = Math.Round(modul.GraphRotation, 1);
                        if (!moduleByRotation.ContainsKey(roundedRotation)) {
                            moduleByRotation.Add(roundedRotation, new List<KlimaFlaechenModul>());
                        }
                        moduleByRotation[roundedRotation].Add(modul);
                    }
                }
                double modulStreifenLength = 0;
                foreach (KeyValuePair<double, List<KlimaFlaechenModul>> kvp in moduleByRotation) {
                    List<ModulePosForCalc> modulePos = new List<ModulePosForCalc>();
                    Matrix3D rotate = Transformation3D.Rotate(-kvp.Key * Math.PI / 180.0);
                    foreach (KlimaFlaechenModul m in kvp.Value) {
                        Point2D rotatedPos = rotate.Transform(new Point2D(m.GraphPosX, m.GraphPosY));
                        modulePos.Add(new ModulePosForCalc(rotatedPos.X / measure, rotatedPos.Y / measure, KlimaFlaechenModul.GetModuleHeight(m.ModulType), KlimaFlaechenModul.GetModuleWidth(m.ModulType)));
                    }
                    for (int i = 0; i < modulePos.Count - 1; i++) {
                        for (int j = i + 1; j < modulePos.Count; j++) {
                            int xStreifenWidth = 0;
                            double xOverlap = 0;
                            int yStreifenWidth = 0;
                            double yOverlap = 0;

                            // check distance of these 2 modules in x
                            if (modulePos[i].x < modulePos[j].x) {
                                double distX = modulePos[j].x - modulePos[i].x - modulePos[i].width;
                                // possible modulstreifen found
                                if (Math.Abs(distX - 0.1) < 0.005) {
                                    xStreifenWidth = 1;
                                } else if (Math.Abs(distX - 0.2) < 0.005) {
                                    xStreifenWidth = 2;
                                }
                                if (xStreifenWidth > 0) {
                                    xOverlap = Math.Min(modulePos[i].y + modulePos[i].height, modulePos[j].y + modulePos[j].height) - Math.Max(modulePos[i].y, modulePos[j].y);
                                }
                            } else {
                                double distX = modulePos[i].x - modulePos[j].x - modulePos[j].width;
                                if (Math.Abs(distX - 0.1) < 0.005) {
                                    xStreifenWidth = 1;
                                } else if (Math.Abs(distX - 0.2) < 0.005) {
                                    xStreifenWidth = 2;
                                }
                                if (xStreifenWidth > 0) {
                                    // possible modulstreifen found
                                    xOverlap = Math.Min(modulePos[j].y + modulePos[j].height, modulePos[i].y + modulePos[i].height) - Math.Max(modulePos[j].y, modulePos[i].y);
                                }
                            }

                            // check distance of these 2 modules in y
                            if (modulePos[i].y < modulePos[j].y) {
                                double distY = modulePos[j].y - modulePos[i].y - modulePos[i].height;
                                // possible modulstreifen found
                                if (Math.Abs(distY - 0.1) < 0.005) {
                                    yStreifenWidth = 1;
                                } else if (Math.Abs(distY - 0.2) < 0.005) {
                                    yStreifenWidth = 2;
                                }
                                if (yStreifenWidth > 0) {
                                    yOverlap = Math.Min(modulePos[i].x + modulePos[i].width, modulePos[j].x + modulePos[j].width) - Math.Max(modulePos[i].x, modulePos[j].x);
                                }
                            } else {
                                double distY = modulePos[i].y - modulePos[j].y - modulePos[j].height;
                                if (Math.Abs(distY - 0.1) < 0.005) {
                                    yStreifenWidth = 1;
                                } else if (Math.Abs(distY - 0.2) < 0.005) {
                                    yStreifenWidth = 2;
                                }
                                if (yStreifenWidth > 0) {
                                    // possible modulstreifen found
                                    yOverlap = Math.Min(modulePos[j].x + modulePos[j].width, modulePos[i].x + modulePos[i].width) - Math.Max(modulePos[j].x, modulePos[i].x);
                                }
                            }

                            if (xOverlap > 0 && xStreifenWidth > 0) {
                                modulStreifenLength += xOverlap * xStreifenWidth;
                            }
                            if (yOverlap > 0 && yStreifenWidth > 0) {
                                modulStreifenLength += yOverlap * yStreifenWidth;
                            }
                            if (xOverlap > 0 && xStreifenWidth > 0 && yOverlap > 0 && yStreifenWidth > 0) {
                                modulStreifenLength += xStreifenWidth / 10 * yStreifenWidth;
                            }
                        }
                    }
                }

                streifen = Math.Ceiling(modulStreifenLength);
            } else {
                streifen = Math.Ceiling(this.RequestedModulesModulierend * 1.5);
            }

            if (streifen == 0) {
                Project.Instance.AddRequiredMaterial(requiredMaterial, "MK04", double.NegativeInfinity);
            } else {
                Project.Instance.AddRequiredMaterial(requiredMaterial, "MK04", -streifen);
            }

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
			if (graphical) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK05", Math.Ceiling(graphVerbindung / 8));
			} else {
				if (this.RequestedSonstigeVerbindeLeitung > 0) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "MK05", Math.Ceiling(this.RequestedSonstigeVerbindeLeitung / 8));
				}
			}

			// Modulniveauplatten
			double area = this.PlannedFloorArea - this.PlannedModulArea - (streifen * (0.945 * 0.096));
			Project.Instance.AddRequiredMaterial(requiredMaterial, "MK03", -Math.Ceiling(area * 2));
		}

		public static void ReviseRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {

			// same amount left and right
			if (requiredMaterial.ContainsKey(ModulKlimaBodenProduct.PLACEHOLDER_MK01_02)) {
				int amount = (int)requiredMaterial[ModulKlimaBodenProduct.PLACEHOLDER_MK01_02];
				Project.Instance.AddRequiredMaterial(requiredMaterial, ModulKlimaBodenProduct.PLACEHOLDER_MK01_02, -1 * amount);
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

		public ModulKlimaBodenConstruction GraphConstruction {
			get { return this.graphConstruction; }
			set { this.graphConstruction = value; }
		}

		[XmlIgnore]
		public override bool AllowToSwitchMode {
			get { return !this.ContainsModules; }
		}

		[XmlIgnore]
		public bool ContainsModules {
			get {
				foreach (ModulBodenCircuit c in this.circuits) {
					if (c.ModuleTotal > 0) {
						return true;
					}
				}
				return false;
			}
		}

		internal ModulBodenCircuit GetCircuitForModul(KlimaFlaechenModul modul, out int index) {
			index = 0;
			foreach (ModulBodenCircuit c in this.circuits) {
				if (c.Row.List.Contains(modul)) {
					return c;
				}
				index++;
			}
			index = -1;
			return null;
		}

		/*public override List<PossibleConnection> GetPossibleConnections(bool input, bool output, double measure, bool invertYAxis, Point2D currentMousePoint, Distributor distributor, Nullable<int> nr) {
			if (!Polygon2D.IsInside(currentMousePoint, this.AssociatedRoom.RoomCoordinates)) {
				return new List<PossibleConnection>();
			}
			List<PossibleConnection> possibleConnections = new List<PossibleConnection>();
			Point2D input12D, input22D, input32D, input42D;
			Point2D output12D, output22D, output32D, output42D;

			// TODO wenn distributor bereit gesetzt ist dürfen nicht alle zurückgegeben werden
			List<ModulBodenCircuit> openInputs = this.GetOpenInputs();
			List<ModulBodenCircuit> openOutputs = this.GetOpenOutputs();

			foreach (ModulBodenCircuit c in this.PlannedCircuits) {
				foreach (KlimaFlaechenModul modul in c.Row.List) {
					Matrix3D transformation = Matrix3D.Identity;
					transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
					transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);

					double height = KlimaFlaechenModul.GetModuleHeight(modul.ModulType) * measure;
					double width = KlimaFlaechenModul.GetModuleWidth(modul.ModulType) * measure;

					if (invertYAxis == modul.GraphBottomUp) {
						if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							if (output && modul.GetOutputLink(c, invertYAxis) == null && openOutputs.Contains(c)) {
								output12D = transformation.Transform(new Point2D(0, 0));
								output22D = transformation.Transform(new Point2D(0, 0.1 * measure));
								output32D = transformation.Transform(new Point2D(0.1 * measure, 0.1 * measure));
								output42D = transformation.Transform(new Point2D(0.1 * measure, 0));
								possibleConnections.Add(new PossibleConnection(modul.GetOutputConnection(measure, invertYAxis, this), new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }), false, true, this, c, modul.GraphRotation, 0));
							}
							if (input && modul.GetInputLink(c, invertYAxis) == null && openInputs.Contains(c)) {
								input12D = transformation.Transform(new Point2D(width, height));
								input22D = transformation.Transform(new Point2D(width, height - 0.1 * measure));
								input32D = transformation.Transform(new Point2D(width - 0.1 * measure, height - 0.1 * measure));
								input42D = transformation.Transform(new Point2D(width - 0.1 * measure, height));
								possibleConnections.Add(new PossibleConnection(modul.GetInputConnection(measure, invertYAxis, this), new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }), true, false, this, c, modul.GraphRotation, 0));
							}
						} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							if (output && modul.GetOutputLink(c, invertYAxis) == null && openOutputs.Contains(c)) {
								output12D = transformation.Transform(new Point2D(width, 0));
								output22D = transformation.Transform(new Point2D(width, 0.1 * measure));
								output32D = transformation.Transform(new Point2D(width - 0.1 * measure, 0.1 * measure));
								output42D = transformation.Transform(new Point2D(width - 0.1 * measure, 0));
								possibleConnections.Add(new PossibleConnection(modul.GetOutputConnection(measure, invertYAxis, this), new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }), false, true, this, c, modul.GraphRotation, 0));
							}
							if (input && modul.GetInputLink(c, invertYAxis) == null && openInputs.Contains(c)) {
								input12D = transformation.Transform(new Point2D(0, height));
								input22D = transformation.Transform(new Point2D(0, height - 0.1 * measure));
								input32D = transformation.Transform(new Point2D(0.1 * measure, height - 0.1 * measure));
								input42D = transformation.Transform(new Point2D(0.1 * measure, height));
								possibleConnections.Add(new PossibleConnection(modul.GetInputConnection(measure, invertYAxis, this), new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }), true, false, this, c, modul.GraphRotation, 0));
							}
						}
					} else {
						if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							if (input && modul.GetInputLink(c, invertYAxis) == null && openInputs.Contains(c)) {
								input12D = transformation.Transform(new Point2D(0, 0));
								input22D = transformation.Transform(new Point2D(0, 0.1 * measure));
								input32D = transformation.Transform(new Point2D(0.1 * measure, 0.1 * measure));
								input42D = transformation.Transform(new Point2D(0.1 * measure, 0));
								possibleConnections.Add(new PossibleConnection(modul.GetInputConnection(measure, invertYAxis, this), new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }), true, false, this, c, modul.GraphRotation, 0));
							}
							if (output && modul.GetOutputLink(c, invertYAxis) == null && openOutputs.Contains(c)) {
								output12D = transformation.Transform(new Point2D(width, height));
								output22D = transformation.Transform(new Point2D(width, height - 0.1 * measure));
								output32D = transformation.Transform(new Point2D(width - 0.1 * measure, height - 0.1 * measure));
								output42D = transformation.Transform(new Point2D(width - 0.1 * measure, height));
								possibleConnections.Add(new PossibleConnection(modul.GetOutputConnection(measure, invertYAxis, this), new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }), false, true, this, c, modul.GraphRotation, 0));
							}
						} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							if (input && modul.GetInputLink(c, invertYAxis) == null && openInputs.Contains(c)) {
								input12D = transformation.Transform(new Point2D(width, 0));
								input22D = transformation.Transform(new Point2D(width, 0.1 * measure));
								input32D = transformation.Transform(new Point2D(width - 0.1 * measure, 0.1 * measure));
								input42D = transformation.Transform(new Point2D(width - 0.1 * measure, 0));
								possibleConnections.Add(new PossibleConnection(modul.GetInputConnection(measure, invertYAxis, this), new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }), true, false, this, c, modul.GraphRotation, 0));
							}
							if (output && modul.GetOutputLink(c, invertYAxis) == null && openOutputs.Contains(c)) {
								output12D = transformation.Transform(new Point2D(0, height));
								output22D = transformation.Transform(new Point2D(0, height - 0.1 * measure));
								output32D = transformation.Transform(new Point2D(0.1 * measure, height - 0.1 * measure));
								output42D = transformation.Transform(new Point2D(0.1 * measure, height));
								possibleConnections.Add(new PossibleConnection(modul.GetOutputConnection(measure, invertYAxis, this), new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }), false, true, this, c, modul.GraphRotation, 0));
							}
						}
					}
				}
			}

			return possibleConnections;
		}*/

		/*private List<ModulBodenCircuit> GetOpenInputs() {
			List<ModulBodenCircuit> openInputs = new List<ModulBodenCircuit>();
			foreach (ModulBodenCircuit c in this.PlannedCircuits) {
				openInputs.Add(c);
			}
			foreach (GraphicalProductConnection conn in this.Connections) {
				if (conn.Vorlauf) {
					foreach (ModulBodenCircuit c in conn.ProductCircuits) {
						openInputs.Remove(c);
					}
				}
			}
			return openInputs;
		}

		private List<ModulBodenCircuit> GetOpenOutputs() {
			List<ModulBodenCircuit> openOutputs = new List<ModulBodenCircuit>();
			foreach (ModulBodenCircuit c in this.PlannedCircuits) {
				openOutputs.Add(c);
			}
			foreach (GraphicalProductConnection conn in this.Connections) {
				if (!conn.Vorlauf) {
					foreach (ModulBodenCircuit c in conn.ProductCircuits) {
						openOutputs.Remove(c);
					}
				}
			}
			return openOutputs;
		}*/

		private bool IsFirstCircuitConnected() {
			foreach (GraphicalProductConnection conn in this.Connections) {
				if (conn.FirstCircuit) {
					return true;
				}
			}
			return false;
		}

		private bool AreOtherCircuitsConnected() {
			foreach (GraphicalProductConnection conn in this.Connections) {
				if (conn.OtherCircuits) {
					return true;
				}
			}
			return false;
		}

		[XmlIgnore]
		public override WW.Math.Geometry.Polygon2D GraphicalArea {
			get { return (this.GraphicalMode.HasValue && this.GraphicalMode.Value == true) ? new Polygon2D(this.AssociatedRoom.RoomCoordinates) : null; }
		}

		public override PossibleProductConnection GetPossibleProductConnection(bool input, bool output, bool firstCircuit, bool otherCircuits, double measure, bool invertYAxis, Point2D currentMousePoint) {
			if (this.AssociatedRoom.RoomCoordinates.Count < 3 || !Polygon2D.IsInside(currentMousePoint, this.AssociatedRoom.RoomCoordinates) || (!firstCircuit && !otherCircuits) || (!input && !output) || this.circuits == null || this.circuits.Count < 1) {
				return null;
			}

			PossibleProductConnection possibleConnection = null;

			foreach (GraphicalProductConnection connection in this.Connections) {
				if (connection.FirstCircuit && ((input && connection.Vorlauf) || (output && connection.Ruecklauf))) {
					firstCircuit = false;
				}
				if (connection.OtherCircuits && ((input && connection.Vorlauf) || (output && connection.Ruecklauf))) {
					otherCircuits = false;
				}
			}


			int connectionsCount = 0;
			if (firstCircuit) {
				connectionsCount++;
			}
			if (otherCircuits) {
				connectionsCount += this.circuits.Count - 1;
			}
			if (input && output) {
				connectionsCount = connectionsCount * 2;
			}

			if (connectionsCount == 0) {
				return null;
			}

			double width = connectionsCount * 0.05 * measure;

			Segment2D segment;
			double bestDistance = double.MaxValue;
			Segment2D bestSegment = new Segment2D();
			Polygon2D room = new Polygon2D(this.AssociatedRoom.RoomCoordinates);
			if (room.IsClockwise()) {
				room.Reverse();
			}
			Point2D lastPoint = room[room.Count - 1];
			Point2D bestConnectionPoint = new Point2D();
			foreach (Point2D point in room) {
				segment = new Segment2D(lastPoint, point);
				if (segment.GetLength() >= width) {
					Point2D newConnectionPoint = segment.GetClosestPoint(currentMousePoint);
					if ((segment.Start - newConnectionPoint).GetLength() < width / 2) {
						Vector2D v = segment.End - segment.Start;
						v.Normalize();
						newConnectionPoint = segment.Start + v * (width / 2);
					}
					if ((segment.End - newConnectionPoint).GetLength() < width / 2) {
						Vector2D v = (segment.Start - segment.End);
						v.Normalize();
						newConnectionPoint = segment.End + v * (width / 2);
					}
					double distance = segment.GetDistance(currentMousePoint);
					//double distance = (newConnectionPoint - currentMousePoint).GetLength();
					if (distance < bestDistance) {
						bestDistance = distance;
						bestSegment = segment;
						bestConnectionPoint = newConnectionPoint;
					}
				}
				lastPoint = point;
			}
			if (bestDistance < 10) {
				//Point2D connectionPoint = bestSegment.GetClosestPoint(currentMousePoint);
				//if ((connectionPoint - bestSegment.Start).GetLength() >= width / 2 && (connectionPoint - bestSegment.End).GetLength() >= width / 2) {
				Polygon2D polygon = new Polygon2D();
				Vector2D v = bestSegment.End - bestSegment.Start;
				v.Normalize();
				Vector2D v2 = new Vector2D(-v.Y, v.X);
				polygon.Add(bestConnectionPoint + (v * width / 2));
				polygon.Add(bestConnectionPoint + (v * width / 2) + (v2 * 0.1 * measure));
				polygon.Add(bestConnectionPoint - (v * width / 2) + (v2 * 0.1 * measure));
				polygon.Add(bestConnectionPoint - (v * width / 2));

				double angle = -Math.Atan2(v.X, v.Y) * 180.0 / Math.PI;

				possibleConnection = new PossibleProductConnection(bestConnectionPoint, polygon, input, output, angle, this, firstCircuit, otherCircuits);
				//}
			}
			return possibleConnection;
		}

		public List<GraphicalConnectionAnbindungsPunkt> GetAnbindungsPunkte(double measure, bool invertYAxis, bool input, int distributorIndex, List<int> ignoreDistributorIndices, Nullable<Point2D> mousePoint) {
			List<GraphicalConnectionAnbindungsPunkt> anbindungsPunkte = new List<GraphicalConnectionAnbindungsPunkt>();
			if (this.connections != null) {
				foreach (GraphicalProductConnection connection in this.connections) {
					anbindungsPunkte.AddRange(connection.GetAnbindungsPunkte(measure, input, distributorIndex, ignoreDistributorIndices, false));
				}
			}
			if (mousePoint.HasValue && (this.connections == null || this.connections.Count == 0)) {
				double planRotation = this.associatedRoom.AssociatedPlan.Rotation;
				foreach (Distributor d in this.associatedRoom.AssociatedFloor.GetAllAvailableDistributors(true)) {
					if (d.IsInsideProduct(this)) {
						if (d.IsPointInside(mousePoint.Value, this.AssociatedRoom.AssociatedFloor, measure, invertYAxis)) {
							PossibleProductConnection ppc = d.GetPossibleProductConnections(true, true, measure, invertYAxis, mousePoint.Value, this, this.AssociatedRoom.AssociatedFloor, this.PlannedCircuitCount, true, input ? -0.055 / 4.0 : 0.055 / 4.0);
							Vector2D vector = new Vector2D(0.01 * measure * Math.Sin((-ppc.Rotation + 0) * Math.PI / 180.0), 0.01 * measure * Math.Cos((-ppc.Rotation + 0) * Math.PI / 180.0));
							Point2D firstPoint = ppc.ConnectionPoint + vector;
							Point2D secondPoint = ppc.ConnectionPoint - vector;
							if (ppc != null) {
								GraphicalProductConnection gpc = new GraphicalProductConnection(Project.Instance.GetPlannedProduct(this), d, new Point2D[] { firstPoint, secondPoint }, true, true, ppc.DistributorStartPosition, true, true, ProductType.FBH);
								gpc.Automatic = true;
								anbindungsPunkte.AddRange(gpc.GetAnbindungsPunkte(measure, input, ppc.DistributorStartPosition, ignoreDistributorIndices, true));
							}
						}
					}
				}
			}
			return anbindungsPunkte;
		}

		public override void DeleteConnection(GraphicalProductConnection connection) {
			base.DeleteConnection(connection);
			foreach (ModulBodenCircuit c in this.PlannedCircuits) {
				List<KlimaFlaechenModulVerbindung> linksToDelete = new List<KlimaFlaechenModulVerbindung>();
				foreach (KlimaFlaechenModulVerbindung link in c.Links) {
					if (link.EndConnectedToAnbindung || link.StartConnectedToAnbindung) {
						linksToDelete.Add(link);
					}
				}
				foreach (KlimaFlaechenModulVerbindung link in linksToDelete) {
					c.Links.Remove(link);
				}
			}
		}

		public override void ClearGraphicalRepresentation() {
			base.ClearGraphicalRepresentation();
			this.GraphConstruction = null;
			foreach (ModulBodenCircuit c in this.PlannedCircuits) {
				c.Links = null;
				c.Row.Links = null;
				foreach (KlimaFlaechenModul modul in c.Row.List) {
					modul.ClearGraphicalRepresentation();
				}
			}
		}
	}
}
