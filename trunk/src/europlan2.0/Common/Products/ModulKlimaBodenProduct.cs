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
		private static double alphaFbk = 6.5; /* für FBK fix */
		private static double alphaFbh = 10.8; /* für FBH fix */
		private static double lambdaU0 = 1; /* fix */
		private static double lambdaE = 60.0; /* Estrichleitfähigkeit bzw Leitfähigkeit Lastausgleichsschicht, fix */
		private static double su = 0.002; /* Estrichüberdeckung bzw. Überdeckung Lastausgleich */
		private static double lambdaU = 60; /* Wärmeleitfähigkeit der Überdeckung */
		private static double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
		private static double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */
		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double atmt = 1.06; /* Fixwert laut Norm */
		private static double b = 6.5; /* Fixwert laut Norm */

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

		public override void StaticInitialize() {
			quickDimensioningHeatPowerPerSquareMeter = 50;
			quickDimensioningCoolPowerPerSquareMeter = 50;
			canHeat = true;
			canCool = false;
			useHarreitherNorm = true;
			maxPressureLost = 15000;
			maxDurchfluss = 240;
			maxModulesInCircuit = 50;
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
		public static double ConfigRLambdaPutz {
			get { return rLambdaPutz; }
			set { rLambdaPutz = value; }
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

		[ProductParameter]
		public static int ConfigModulesInCircuit {
			get { return maxModulesInCircuit; }
			set { maxModulesInCircuit = value; }
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

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg) {
			this.incompleteCalculation = false;
			if (this.plannedFloorConstruction == null || this.plannedInsulationConstruction == null || this.plannedConnection == null) {
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

			int cCount = this.RequestedCircuits.HasValue ? this.RequestedCircuits.Value : this.RequestedModulesTotal / 40;
			if (cCount > 12) {
				cCount = 12;
			}
			if (cCount <= 0) {
				cCount = 1;
			}
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
				if (this.PlannedMhHeat > ModulKlimaBodenProduct.ConfigMaxDurchfluss) {
					found = false;
				}
				if (this.PlannedMhCool > ModulKlimaBodenProduct.ConfigMaxDurchfluss) {
					found = false;
				}

				found = this.RequestedCircuits.HasValue || cCount >= 12 || found;

				if (!found) {
					cCount++;
				}
			}

			errorMsg = "";
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

			if (this.PlannedModulArea > this.plannedArea - this.plannedAreaUnheated) {
				errorMsg += "Die verplanten Module nehmen mehr Fläche in Anspruch als für dieses System zu Verfügung steht (" + this.PlannedModulArea.ToString() + "m² > " + (this.plannedArea - this.plannedAreaUnheated).ToString() + "m²)\n";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeat, 1) > (ModulKlimaBodenProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264)) {
				errorMsg += "Oberflächentemperatur zu groß (" + Math.Round(this.PlannedFloorTemperatureHeat, 1) + "°C > " + Math.Round((ModulKlimaBodenProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264), 1) + "°C)\n";
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

		public override float PlannedCeilingArea {
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
	}
	
}
