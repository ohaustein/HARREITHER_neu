using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Threading;
using Europlan.Licensing;
using WW.Math.Geometry;
using WW.Math;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Product_ModulKlimBoden20Name", "Product_ModulKlimBoden20FullName")]
	public class ModulKlimaBoden20Product : Product, ProductWithInsulationConstruction {

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

		private static double leistungsFaktorHeizen = 1.0;
		private static double leistungsFaktorKuehlen = 1.0;

		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static double v = 0.00000101; /* m²/s ... kinematische Viskosität */

        private static double[] druckverlustModul_100_40 = { 0.2, 0.35, 0.65, 0.9, 1.25, 1.5, 1.8, 2.2, 2.6, 3, 3.6, 4.5, 5.4, 6.3, 7.2, 8.1, 9.1, 10, 11, 12, 13, 14, 15, 16.5, 17.8, 19, 20, 21.5, 23, 25 };

        private float plannedAreaReduced = 0;
		private float plannedAreaUnheated = 0;
		private Construction plannedFloorConstruction = null;
		private Construction plannedInsulationConstruction = null;
		private string plannedFloorConstructionId = null;
		private string plannedInsulationConstructionId = null;

		//  !!!!!!!!!!! changes must be also applied in SystemParametersPanel.cs !!!!!!!!!!!
		private static int maxPressureLost = 15000;
		private static int maxDurchfluss = 240;
		private static int maxModulesInRow = 20;
		private static int maxModulesInParallel = 3;
		private static int maxModulesInCircuit = 40;
		private static double spreizungHeizMin = 4;
		private static double spreizungHeizMax = 12;
		private static double spreizungKuehlMin = 2;
		private static double spreizungKuehlMax = 5;

        //  !!!!!!!!!!! changes must be also applied in SystemParametersPanel.cs !!!!!!!!!!!
        private static bool useHarreitherNorm = true;
        private static double maxFloorTempHarreither = 27;
        private static double maxFloorTempEn1264 = 29;
        private static double maxNassraumTemp = 33;

		private ProductType modulType = ProductType.FBH;
		private float plannedFloorArea = 0;
		private float plannedCeilingArea = 0;
		private float plannedFloorOrCeilingArea = 0;

		private ModulKlimaBoden20Construction graphConstruction = null;

		public ModulKlimaBoden20Product() {
			if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaBoden)) {
				throw new ProductNotLicensedException(this.GetType());
			}
		}

        protected ModulKlimaBoden20Product(ModulKlimaBoden20Product product)
            : base(product)
        {

		}

		public override void InitializeNewProduct() {
			this.PlannedCircuits.Add(new ModulKlimaBoden20Circuit(this));
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
			Product.StaticInitialize<ModulKlimaBoden20Product>(config);
		}

		public static string GlobalNotificationMessage {
			get {
				string message = null;
				Configuration userConfig = Configuration.UserTemplate;

				double defaultLeistungsFaktorHeizen = userConfig.GetProductParameterAsDouble<ModulKlimaBoden20Product>("ConfigLeistungsFaktorHeizen");
				if (leistungsFaktorHeizen != defaultLeistungsFaktorHeizen) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_LeistungsfaktorHeizen;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(leistungsFaktorHeizen, 3).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultLeistungsFaktorHeizen, 3).ToString());
					message += newMsg;
				}

				double defaultLeistungsFaktorKuehlen = userConfig.GetProductParameterAsDouble<ModulKlimaBoden20Product>("ConfigLeistungsFaktorKuehlen");
				if (leistungsFaktorKuehlen != defaultLeistungsFaktorKuehlen) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_LeistungsfaktorKuehlen;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(leistungsFaktorKuehlen, 3).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultLeistungsFaktorKuehlen, 3).ToString());
					message += newMsg;
				}

				if (message != null) {
					message = EuroplanRes.ModulKlimaBoden20Product_NotificationParameter + /*"Hitherm-Systeme werden mit veränderten Paramtern berechnet. Folgende Parameter weichen von den Standardwerten ab:\n" */
						"\n" + message;
				}

				return message;
			}
		}

		public override Product Clone(Room room) {
            ModulKlimaBoden20Product product = new ModulKlimaBoden20Product(this);
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

		[BoolProductParameter(true)]
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

		[DoubleProductParameter(1)]
		public static double ConfigLeistungsFaktorHeizen {
			get { return leistungsFaktorHeizen; }
			set { leistungsFaktorHeizen = value; }
		}

		[DoubleProductParameter(1)]
		public static double ConfigLeistungsFaktorKuehlen {
			get { return leistungsFaktorKuehlen; }
			set { leistungsFaktorKuehlen = value; }
		}

		[IntProductParameter(18000)]
		public static int ConfigMaxPressureLost {
			get { return ModulKlimaBoden20Product.maxPressureLost; }
            set { ModulKlimaBoden20Product.maxPressureLost = value; }
		}

		[IntProductParameter(240)]
		public static int ConfigMaxDurchfluss {
            get { return ModulKlimaBoden20Product.maxDurchfluss; }
            set { ModulKlimaBoden20Product.maxDurchfluss = value; }
		}

		[IntProductParameter(20)]
		public static int ConfigMaxModulesInRow {
            get { return ModulKlimaBoden20Product.maxModulesInRow; }
            set { ModulKlimaBoden20Product.maxModulesInRow = value; }
		}

		[IntProductParameter(3)]
		public static int ConfigMaxModulesInParallel {
            get { return ModulKlimaBoden20Product.maxModulesInParallel; }
            set { ModulKlimaBoden20Product.maxModulesInParallel = value; }
		}

		[IntProductParameter(40)]
		public static int ConfigModulesInCircuit {
            get { return ModulKlimaBoden20Product.maxModulesInCircuit; }
            set { ModulKlimaBoden20Product.maxModulesInCircuit = value; }
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
        public static string ConfigDruckverlustModul_100_40String
        {
            get
            {
                return ConvertArrayToString(druckverlustModul_100_40);
            }
            set
            {
                double[] array = ConvertStringToArray(value);
                if (array != null)
                {
                    druckverlustModul_100_40 = array;
                }
            }
        }
        public static double[] ConfigDruckverlustModul_100_40
        {
            get { return druckverlustModul_100_40; }
            set { druckverlustModul_100_40 = value; }
        }

        [DoubleProductParameter(27)]
        public static double ConfigMaxFloorTempHarreither
        {
            get { return maxFloorTempHarreither; }
            set { maxFloorTempHarreither = value; }
        }

        [BoolProductParameter(true)]
        public static bool ConfigUseHarreitherNorm
        {
            get { return useHarreitherNorm; }
            set { useHarreitherNorm = value; }
        }

        [DoubleProductParameter(29)]
        public static double ConfigMaxFloorTempEn1264
        {
            get { return maxFloorTempEn1264; }
            set { maxFloorTempEn1264 = value; }
        }

        [DoubleProductParameter(33)]
        public static double ConfigMaxNassraumTemp
        {
            get { return maxNassraumTemp; }
            set { maxNassraumTemp = value; }
        }

		#endregion Product Parameters

        public double MaxDurchfluss {
            get {
                if (this.PlannedConnection == null || this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.NONE) {
                    return ModulKlimaBoden20Product.ConfigMaxDurchfluss;
                }
                Product p = this;
                while (p != null && p.PlannedConnection != null && p.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT && p.PlannedConnection.OtherProduct != null) {
                    p = p.PlannedConnection.OtherProduct.Product;
                }
                if (p != null && p.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR && p.PlannedConnection.Distributor != null) {
                    return p.PlannedConnection.Distributor.MaxDurchfluss;
                }
                return ModulKlimaBoden20Product.ConfigMaxDurchfluss;
            }
        }
        public double MaxMassenstrom {
            get { return MaxDurchfluss * ModulKlimaBoden20Product.ConfigRho / 1000; }
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

		public override string QuickDimensioningName {
			get { return QuickDimensioningNameStatic; }
		}

		public static string QuickDimensioningNameStatic {
			get { return "Modul\nKlima\nBoden 20\n(m²)"; }
		}

		public override ProductType Type {
			get { return this.modulType; }
		}

		public ProductType ModulType {
			get { return this.modulType; }
			set { this.modulType = value; }
		}

		public override void CalculateHeatAndCoolFlow() {
			base.CalculateHeatAndCoolFlow();
			double spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			double spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			if (spreizungHeat > ModulKlimaBoden20Product.ConfigSpreizungHeizMax) {
				spreizungHeat = ModulKlimaBoden20Product.ConfigSpreizungHeizMax;
			}
			if (spreizungHeat < ModulKlimaBoden20Product.ConfigSpreizungHeizMin) {
				spreizungHeat = ModulKlimaBoden20Product.ConfigSpreizungHeizMin;
			}
			if (spreizungCool > ModulKlimaBoden20Product.ConfigSpreizungKuehlMax) {
				spreizungCool = ModulKlimaBoden20Product.ConfigSpreizungKuehlMax;
			}
			if (spreizungCool < ModulKlimaBoden20Product.ConfigSpreizungKuehlMin) {
				spreizungCool = ModulKlimaBoden20Product.ConfigSpreizungKuehlMin;
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
				this.lastErrorMsg = EuroplanRes.ErrorMessage_FehlendeEingaben + " "; //"Fehlende Eingaben: "
				if (PlannedFloorConstruction == null) {
					this.lastErrorMsg += EuroplanRes.ErrorMessage_FehlendeEingabenFussboden + ", "; //"Fußbodenkonstruktion, "
				}
				if (PlannedInsulationConstruction == null) {
					this.lastErrorMsg += EuroplanRes.ErrorMessage_FehlendeEingabenDaemmung + ", "; //"Wärmedämmkonstruktion, "
				}
				if (PlannedConnection == null) {
					this.lastErrorMsg += EuroplanRes.ErrorMessage_FehlendeEingabenHkAnschluss + ", "; //"Heizkreisanschluß, "
				}
				this.lastErrorMsg = this.lastErrorMsg.Substring(0, this.lastErrorMsg.Length - 2);
				this.incompleteCalculation = true;
				return false;
			}

			if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {

				int c = this.PlannedConnection.OtherProduct.Product.PlannedCircuits.Count - this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Count;
				foreach (Circuit.CircuitConnection cc in this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Values) {
					if (cc.OtherProduct == this) {
						c++;
					}
				}
				if (c < this.circuits.Count) {
					this.lastErrorMsg = EuroplanRes.ErrorMessage_HkAnschluss; //"Es sind nicht alle Heizkreise dieses Systems angeschloßen"
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
					this.lastErrorMsg = EuroplanRes.ErrorMessage_HkAnschluss; //"Es sind nicht alle Heizkreise dieses Systems angeschloßen"
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
			this.CalculateVorlaufRuecklauf(out vorlaufTotal, out vorlaufNotIsolated, out ruecklaufTotal, out ruecklaufNotIsolated, out vorlaufWithoutOtherProductTotal, out vorlaufWithoutOtherProductNotIsolated, out ruecklaufWithoutOtherProductTotal, out ruecklaufWithoutOtherProductNotIsolated, out longestVorlaufTotal, out longestRuecklaufTotal, Math.Max(PlannedCircuitCount, 12));

			this.CalculateHeatAndCoolFlow();
			bool graphical = (this.GraphicalMode.HasValue && this.GraphicalMode.Value);
			double measure = 1;
			if (this.AssociatedRoom != null && this.AssociatedRoom.AssociatedPlan != null && this.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
				measure = this.AssociatedRoom.AssociatedPlan.Measure.Value;
			}

			int i = 0;
			foreach (ModulKlimaBoden20Circuit mc in this.circuits) {
				mc.NrOfCircuit = i;
				mc.ModulKlimaBoden20Product = this;
				mc.PipeLengthVorlaufTotal = vorlaufTotal[i];
				mc.PipeLengthVorlaufNotIsolated = vorlaufNotIsolated[i];
				mc.PipeLengthRuecklaufTotal = ruecklaufTotal[i];
				mc.PipeLengthRuecklaufNotIsolated = ruecklaufNotIsolated[i];
				mc.PipeLengthVorlaufWithoutOtherProductTotal = vorlaufWithoutOtherProductTotal[i];
				mc.PipeLengthVorlaufWithoutOtherProductNotIsolated = vorlaufWithoutOtherProductNotIsolated[i];
				mc.PipeLengthRuecklaufWithoutOtherProductTotal = ruecklaufWithoutOtherProductTotal[i];
				mc.PipeLengthRuecklaufWithoutOtherProductNotIsolated = ruecklaufWithoutOtherProductNotIsolated[i];
                mc.ReducedArea = this.PlannedAreaReduced / this.circuits.Count;

                if (graphical)
                {
                    foreach (ModulKlimaBoden20SubArea sa in mc.SubAreas)
                    {
                        foreach (KlimaFlaechenList row in sa.Rows)
                        {
                            double verbindeleitung = 0;
                            if (row.Links != null)
                            {
                                foreach (KlimaFlaechenModulVerbindung link in row.Links)
                                {
                                    verbindeleitung += link.GetLength(measure);
                                }
                            }
                            row.LengthVerbindeleitungen = verbindeleitung;
                        }
                    }
                }
                else
                {
                    foreach (ModulKlimaBoden20SubArea sa in mc.SubAreas)
                    {
                        foreach (KlimaFlaechenList row in sa.Rows)
                        {
                            double interModuleConnectionLength = 0;
                            foreach (KlimaFlaechenModul modul in row.List)
                            {
                                interModuleConnectionLength += modul.ModulationLengthValue;
                            }
                            row.LengthVerbindeleitungen = row.SonstigeVerbindeleitung + interModuleConnectionLength;
                        }
                    }
                }
				mc.Calculate();
				i++;
			}

			if (variableSpreizung && this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
				double defSpreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
				double defSpreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
				// Heizleistung veringern
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < ModulKlimaBoden20Product.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad && this.PlannedSpreizungHeat < 1.2 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat -= 0.1;
					foreach (ModulKlimaBoden20Circuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempHeat += 0.1;
				// Heizleistung erhöhen
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > ModulKlimaBoden20Product.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < ModulKlimaBoden20Product.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhHeat < MaxMassenstrom && this.PlannedSpreizungHeat > 0.8 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat += 0.1;
					foreach (ModulKlimaBoden20Circuit c in this.circuits) {
						c.Calculate();
					}
				}
				// Kühlleistung verringern
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < ModulKlimaBoden20Product.ConfigSpreizungKuehlMax && this.PlannedCoolLoad > requestedCoolLoad && this.PlannedSpreizungCool < 1.2 * defSpreizungCool) {
					this.plannedRuecklaufTempCool += 0.1;
					foreach (ModulKlimaBoden20Circuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempCool -= 0.1;
				// Kühlleistung erhöhen
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > ModulKlimaBoden20Product.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < ModulKlimaBoden20Product.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhCool < MaxMassenstrom && this.PlannedSpreizungCool > 0.8 * defSpreizungCool) {
					this.plannedRuecklaufTempCool -= 0.1;
					foreach (ModulKlimaBoden20Circuit c in this.circuits) {
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
			if (this.ModulType == ProductType.DH && Math.Round(this.CoveredArea, 1) > Math.Round(this.PlannedCeilingArea, 1)) {
				newMsg = EuroplanRes.ErrorMessage_Modulflaeche;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.CoveredArea, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.PlannedCeilingArea, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			}
			foreach (ModulKlimaBoden20Circuit c in circuits) {
				int saNr = 1;
				int longestRow = 0;
				int moduleCount = 0;
				foreach (ModulKlimaBoden20SubArea sa in c.SubAreas) {
					if (sa.Rows.Count > 0) {
						if (sa.Rows.Count > ModulKlimaBoden20Product.ConfigMaxModulesInParallel) {
							newMsg = EuroplanRes.ErrorMessage_ModulReihen;
							newMsg = newMsg.Replace("%TEILFL%", saNr.ToString());
							newMsg = newMsg.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
							newMsg = newMsg.Replace("%VALUE%", sa.Rows.Count.ToString());
							newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaBoden20Product.ConfigMaxModulesInParallel.ToString());
							this.lastErrorMsg += newMsg + "\n";
						}
						int maxModules = 0;
						int maxRowNr = 0;
						int minModules = Int32.MaxValue;
						int minRowNr = 0;
						int curRowNr = 1;
						foreach (KlimaFlaechenList row in sa.Rows) {
							moduleCount += row.List.Count;
							if (row.List.Count > maxModules) {
								maxModules = row.List.Count;
								maxRowNr = curRowNr;
							}
							if (row.List.Count < minModules) {
								minModules = row.List.Count;
								minRowNr = curRowNr;
							}
							curRowNr++;
						}
						if (maxModules > minModules + 1) {
							newMsg = EuroplanRes.ErrorMessage_ModulReiheUnterschied;
							newMsg = newMsg.Replace("%REIHELANG%", maxRowNr.ToString());
							newMsg = newMsg.Replace("%REIHEKURZ%", maxRowNr.ToString());
							newMsg = newMsg.Replace("%TEILFL%", saNr.ToString());
							newMsg = newMsg.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
							newMsg = newMsg.Replace("%VALUELANG%", sa.Rows.Count.ToString());
							newMsg = newMsg.Replace("%VALUEKURZ%", ModulKlimaBoden20Product.ConfigMaxModulesInParallel.ToString());
							this.lastErrorMsg += newMsg + "\n";
						}
						longestRow += maxModules;
					}
					saNr++;
				}
				if (longestRow > ModulKlimaBoden20Product.ConfigMaxModulesInRow) {
					newMsg = EuroplanRes.ErrorMessage_ModulReiheLaenge;
					newMsg = newMsg.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
					newMsg = newMsg.Replace("%VALUE%", longestRow.ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaBoden20Product.ConfigMaxModulesInRow.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
				if (moduleCount > ModulKlimaBoden20Product.ConfigModulesInCircuit) {
					newMsg = EuroplanRes.ErrorMessage_ModulAnzahl;
					newMsg = newMsg.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
					newMsg = newMsg.Replace("%VALUE%", moduleCount.ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaBoden20Product.ConfigModulesInCircuit.ToString());
				}
			}
			if (this.PlannedMaxMhHeat >= this.PlannedMaxMhCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedMaxMhHeat, 1) > MaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhHeat, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", MaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedMaxMhCool, 1) > MaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhCool, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", MaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > Math.Round(ModulKlimaBoden20Product.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoHeat, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ModulKlimaBoden20Product.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > Math.Round(ModulKlimaBoden20Product.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoCool, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ModulKlimaBoden20Product.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
            if (Math.Round(this.PlannedFloorTemperatureHeat, 1) > this.MaxFloorTemp && this.requestedHeatLoad > 0)
            {
                newMsg = EuroplanRes.ErrorMessage_Oberflaechentemperatur;
                newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedFloorTemperatureHeat, 1).ToString());
                newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.MaxFloorTemp, 1).ToString());
                this.lastErrorMsg += newMsg + "\n";
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

        private double MaxFloorTemp
        {
            get
            {
                double maxTemp = ConfigUseHarreitherNorm ? ConfigMaxFloorTempHarreither : ConfigMaxFloorTempEn1264;
                return AssociatedRoom.IsNassraum ? Math.Max(maxTemp, ConfigMaxNassraumTemp) : maxTemp;
            }
        }

		[XmlIgnore]
		public double CoveredArea {
			get {
				double area = 0;
				foreach (ModulKlimaBoden20Circuit mc in this.circuits) {
					area += mc.CoveredArea;
				}
				return area;
			}

		}

        [XmlIgnore]
        public double PlannedModulArea
        {
            get
            {
                if (this.incompleteCalculation)
                {
                    return 0;
                }
                double value = 0;
                foreach (ModulKlimaBoden20Circuit c in this.circuits)
                {
                    value += c.HeatArea;
                }
                return value;
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
        public float PlannedAreaReduced
        {
            get { return this.plannedAreaReduced; }
            set { this.plannedAreaReduced = value; }
        }

		public override float PlannedFloorArea {
			get {
				if (this.modulType == ProductType.FBH) {
					return this.plannedFloorArea;
				}
				return 0;
			}
			set {
				if (this.modulType == ProductType.FBH) {
					this.plannedFloorArea = value;
				}
			}
		}

		public override float PlannedWallArea {
			get {
				if (this.modulType == ProductType.WH) {
					return this.PlannedNetArea;
				}
				return 0;
			}
			set { }
		}

		public override float PlannedCeilingArea {
			get {
				if (this.modulType == ProductType.DH) {
					return this.plannedCeilingArea;
				}
				return 0;
			}
			set {
				if (this.modulType == ProductType.DH) {
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
				if (this.modulType == ProductType.DH) {
					return this.plannedCeilingArea;
				}
				if (this.modulType == ProductType.FBH) {
					return this.plannedFloorArea;
				}
				return 0;
			}
			set {
				this.plannedFloorOrCeilingArea = value;
			}
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
				foreach (ModulKlimaBoden20Circuit mbc in this.circuits) {
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
				foreach (ModulKlimaBoden20Circuit c in this.circuits) {
					if (!c.QFbhTotalHeat.Equals(double.NaN)) {
						value += c.QFbhTotalHeat;
					}
				}
				return value;
			}
		}

        [XmlIgnore]
        public double PlannedHeatLoadPerSqM
        {
            get { return this.PlannedHeatLoad / this.PlannedNetArea; }
        }

        [XmlIgnore]
        public double PlannedCoolLoadPerSqM
        {
            get { return this.PlannedCoolLoad / this.PlannedNetArea; }
        }

		public override float PlannedNetArea {
			get {
                float area = ModulType == ProductType.FBH ? this.PlannedFloorArea : this.PlannedCeilingArea;
                return area - this.PlannedAreaUnheated;
			}
		}

		/// <summary>
		/// The id of the planned ceiling construction for serialization
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
		/// The planned ceiling contruction
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
		/// The r-value of the planned ceiling construction
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
		/// The r-value of the planned ceiling construction
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
				foreach (ModulKlimaBoden20Circuit c in this.circuits) {
					foreach (ModulKlimaBoden20SubArea subArea in c.SubAreas) {
						foreach (KlimaFlaechenList row in subArea.Rows) {
							foreach (KlimaFlaechenModul modul in row.List) {
								wasserInhalt += modul.WasserInhalt;
							}
						}
					}
				}

				return wasserInhalt + (pipeEurovalLength * EurovalProduct.rohrInnenA * 1000) + (pipe21mmLength * Product.rundrohr21mmInnenA * 1000);
			}
		}

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {
			bool graphical = this.GraphicalMode.HasValue && this.GraphicalMode.Value;

			if (!graphical) {
				double additional21mm = 0;
				int nrOfElements = 0;

				int rows = 0;
				int subAreas = 0;
				double modulArea = 0;
				foreach (ModulKlimaBoden20Circuit c in this.circuits) {
					foreach (ModulKlimaBoden20SubArea subArea in c.SubAreas) {
						subAreas++;
						foreach (KlimaFlaechenList row in subArea.Rows) {
							rows++;

							// TO BE CLARIFIED
							additional21mm += row.LengthVerbindeleitungen;
							additional21mm += 1.4;

							foreach (KlimaFlaechenModul modul in row.List) {
								// Modul
								Project.Instance.AddRequiredMaterial(requiredMaterial, modul.PartNumber, 1);
								nrOfElements++;
								modulArea += modul.GetHeatArea(false);
							}
						}
					}
				}

				this.AddRequiredMaterialForConnections(requiredMaterial, false, additional21mm, true);

			} else {
				// TODO graphical 
			}
		}

		public override double Dichte {
			get { return ModulKlimaBoden20Product.ConfigRho; }
		}

		public override double Waermekapazitaet {
			get { return ModulKlimaBoden20Product.ConfigC; }
		}

		public override double Viskositaet {
			get { return ModulKlimaBoden20Product.ConfigV; }
		}

		public override string NotificationMessage {
			get {
				string notification = base.NotificationMessage;
				if (this.ModulType == ProductType.DH && Math.Round(this.CoveredArea, 1) > Math.Round(this.PlannedCeilingArea * 3 / 4, 1) && Math.Round(this.CoveredArea, 1) <= Math.Round(this.PlannedCeilingArea, 1)) {
					string newNotification = EuroplanRes.ErrorMessage_ModulBelegung;
					newNotification = newNotification.Replace("%VALUE%", Math.Round(this.CoveredArea, 1).ToString());
					newNotification = newNotification.Replace("%MAXIMUM%", Math.Round(this.PlannedCeilingArea * 3 / 4, 1).ToString());
					if (notification == null) {
						notification = newNotification;

					} else {
						notification = notification + "\n" + newNotification;
					}
				}                
                if (this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.TICHELMANN) {
                    if (notification == null) {
                        notification = "";
                    } else {
                        notification += "\n";
                    }
                    notification += EuroplanRes.ModulKlimaBoden20Product_NotificationTichelmann;
                }
                return notification;
			}
		}

		internal override void FinalizeLoading(PlannedProduct pp) {
			base.FinalizeLoading(pp);
			foreach (ModulKlimaBoden20Circuit c in this.circuits) {
				c.ModulKlimaBoden20Product = this;
			}
			switch (this.modulType) {
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
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeat {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (ModulKlimaBoden20Circuit mc in this.circuits) {
					if (mc.C_FloorTempHeat > value) {
						value = mc.C_FloorTempHeat;
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
				double value = Double.MaxValue;
				foreach (ModulKlimaBoden20Circuit mc in this.circuits) {
					if (mc.C_FloorTempCool < value) {
						value = mc.C_FloorTempCool;
					}
				}
				return value;
			}
		}

		public ModulKlimaBoden20Construction GraphConstruction {
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
				foreach (ModulKlimaBoden20Circuit c in this.circuits) {
					foreach (ModulKlimaBoden20SubArea subArea in c.SubAreas) {
						foreach (KlimaFlaechenList row in subArea.Rows) {
							if (row.List.Count > 0) {
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		public List<KlimaFlaechenModul> GetModulesInLane(int lane) {
			List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
			foreach (ModulKlimaBoden20Circuit c in this.circuits) {
				foreach (ModulKlimaBoden20SubArea subArea in c.SubAreas) {
					foreach (KlimaFlaechenList row in subArea.Rows) {
						foreach (KlimaFlaechenModul modul in row.List) {
							if (modul.GraphLane == lane) {
								modules.Add(modul);
							}
						}
					}
				}
			}
			return modules;
		}

		public List<KlimaFlaechenModulWithRowAndCircuit> GetModulesInLaneWithRowAndCircuit(int lane) {
			List<KlimaFlaechenModulWithRowAndCircuit> modules = new List<KlimaFlaechenModulWithRowAndCircuit>();
			foreach (ModulKlimaBoden20Circuit circuit in this.circuits) {
				foreach (ModulKlimaBoden20SubArea subArea in circuit.SubAreas) {
					foreach (KlimaFlaechenList row in subArea.Rows) {
						foreach (KlimaFlaechenModul modul in row.List) {
							if (modul.GraphLane == lane) {
								modules.Add(new KlimaFlaechenModulWithRowAndCircuit(modul, row, circuit));
							}
						}
					}
				}
			}
			return modules;
		}

		public int CountModules() {
			int count = 0;
			foreach (ModulKlimaBoden20Circuit circuit in this.circuits) {
				count += circuit.CountModules();
			}
			return count;
		}

		public ModulKlimaBoden20Circuit GetCircuitForModul(KlimaFlaechenModul modul, out int index) {
			index = 0;
			foreach (ModulKlimaBoden20Circuit c in this.circuits) {
				if (c.ContainsModul(modul)) {
					return c;
				}
				index++;
			}
			index = -1;
			return null;
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
					if (distance < bestDistance) {
						bestDistance = distance;
						bestSegment = segment;
						bestConnectionPoint = newConnectionPoint;
					}
				}
				lastPoint = point;
			}
			if (bestDistance < 10) {
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
								GraphicalProductConnection gpc = new GraphicalProductConnection(Project.Instance.GetPlannedProduct(this), d, new Point2D[] { firstPoint, secondPoint }, true, true, ppc.DistributorStartPosition, true, true, ProductType.DH);
								gpc.Automatic = true;
								anbindungsPunkte.AddRange(gpc.GetAnbindungsPunkte(measure, input, ppc.DistributorStartPosition, ignoreDistributorIndices, true));
							}
						}
					}
				}
			}
			return anbindungsPunkte;
		}

		[XmlIgnore]
		public override WW.Math.Geometry.Polygon2D GraphicalArea {
			get { return (this.GraphicalMode.HasValue && this.GraphicalMode.Value == true) ? new Polygon2D(this.AssociatedRoom.CeilingCoordinatesToUse) : null; }
		}

		public override void DeleteConnection(GraphicalProductConnection connection) {
			base.DeleteConnection(connection);
			foreach (ModulKlimaBoden20Circuit c in this.PlannedCircuits) {
				List<KlimaFlaechenSubAreaVerbindung> saLinksToDelete = new List<KlimaFlaechenSubAreaVerbindung>();
				foreach (KlimaFlaechenSubAreaVerbindung link in c.Links) {
					if (link.EndConnectedToAnbindung || link.StartConnectedToAnbindung) {
						saLinksToDelete.Add(link);
					}
				}
				foreach (KlimaFlaechenSubAreaVerbindung link in saLinksToDelete) {
					c.Links.Remove(link);
				}
				foreach (ModulKlimaBoden20SubArea sa in c.SubAreas) {
					foreach (KlimaFlaechenList row in sa.Rows) {
						List<KlimaFlaechenModulVerbindung> linksToDelete = new List<KlimaFlaechenModulVerbindung>();
						foreach (KlimaFlaechenModulVerbindung link in row.Links) {
							if (link.EndConnectedToAnbindung || link.StartConnectedToAnbindung) {
								linksToDelete.Add(link);
							}
						}
						foreach (KlimaFlaechenModulVerbindung link in linksToDelete) {
							row.Links.Remove(link);
						}
					}
				}
			}
		}		

		

		public override void ClearGraphicalRepresentation() {
			base.ClearGraphicalRepresentation();
			this.GraphConstruction = null;
			foreach (ModulKlimaBoden20Circuit c in this.PlannedCircuits) {
				c.Links = new List<KlimaFlaechenSubAreaVerbindung>();
				foreach (ModulKlimaBoden20SubArea subArea in c.SubAreas) {
					foreach (KlimaFlaechenList list in subArea.Rows) {
						list.Links = new List<KlimaFlaechenModulVerbindung>();
						foreach (KlimaFlaechenModul modul in list.List) {
							modul.ClearGraphicalRepresentation();
						}
					}
				}
			}
		}

		public KlimaFlaechenModul GetModuleAtPos(int lane, double posInLane, double measure, double tolerance) {
			foreach (ModulKlimaBoden20Circuit c in this.PlannedCircuits) {
				foreach (ModulKlimaBoden20SubArea sa in c.SubAreas) {
					foreach (KlimaFlaechenList row in sa.Rows) {
						foreach (KlimaFlaechenModul m in row.List) {
							if (m.GraphLane == lane) {
								if (Math.Abs(posInLane - m.GraphPositionInLan) / measure < tolerance) {
									return m;
								}
							}
						}
					}
				}
			}
			return null;
		}

        public struct KlimaFlaechenModulWithRowAndCircuit
        {
            public KlimaFlaechenModul modul;
            public KlimaFlaechenList row;
            public ModulKlimaBoden20Circuit circuit;

            public KlimaFlaechenModulWithRowAndCircuit(KlimaFlaechenModul modul, KlimaFlaechenList row, ModulKlimaBoden20Circuit circuit)
            {
                this.modul = modul;
                this.row = row;
                this.circuit = circuit;
            }
        }

    }

}
