using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Modul Klima-Decke")]
	public class ModulKlimaDeckeProduct : Product {

		public class ModulCeilingConstructionEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string kassettenDecke = "Kassettendecke";
			private static readonly string c_profil = "C-Profil";
			private static readonly string holzStaffel = "Holzstaffel";

			private Dictionary<string, ModulCeilingConstructionEnum> mappingFromString = new Dictionary<string, ModulCeilingConstructionEnum>();
			private Dictionary<ModulCeilingConstructionEnum, string> mappingToString = new Dictionary<ModulCeilingConstructionEnum, string>();

			public ModulCeilingConstructionEnumConverter() {
				mappingFromString.Add(kassettenDecke, ModulCeilingConstructionEnum.KASSETTENDECKE);
				mappingFromString.Add(c_profil, ModulCeilingConstructionEnum.C_PROFIL);
				mappingFromString.Add(holzStaffel, ModulCeilingConstructionEnum.HOLZSTAFFEL);
				mappingToString.Add(ModulCeilingConstructionEnum.KASSETTENDECKE, kassettenDecke);
				mappingToString.Add(ModulCeilingConstructionEnum.C_PROFIL, c_profil);
				mappingToString.Add(ModulCeilingConstructionEnum.HOLZSTAFFEL, holzStaffel);
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
				if (value is ModulCeilingConstructionEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((ModulCeilingConstructionEnum)value)) {
						return mappingToString[(ModulCeilingConstructionEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(ModulCeilingConstructionEnumConverter))]
		public enum ModulCeilingConstructionEnum {
			KASSETTENDECKE,
			C_PROFIL,
			HOLZSTAFFEL
		}

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 80;
		private static int quickDimensioningCoolPowerPerSquareMeter = 80;
		private static bool canHeat = true;
		private static bool canCool = true;

		// planning
		private static double su0 = 0.045; /* Mindestüberdeckung fix */
		private static double alpha0 = 10.8; /* Fixwert für FBH fix */
		private static double lambdaU0 = 1; /* fix */
		private static double rLambdaDecke = 0.11; /* Deckenschicht 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
		private static double rLambdaDach = 0.0; /* Deckenschicht; durch echte Konstruktion ersetzen! */
		private static double atmt = 1.06; /* Fixwert laut Norm */
		private static double b = 6.5; /* Fixwert laut Norm */
		private static double leistungsFaktor = 0.77;

		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static double v = 0.00000101; /* m²/s ... kinematische Viskosität */

		private static double[] druckverlustModul_120_30 = { 0.23, 0.47, 0.82, 1.05, 1.5, 1.75, 2.1, 2.6, 3, 3.5, 4.2, 5.25, 6.3, 7.3, 8.4, 9.4, 10.6, 11.7, 12.8, 14, 15.1, 16.3, 17.5, 19.2, 20.7, 22.1, 23.3, 25, 26.8, 29.1 };
		private static double[] druckverlustModul_100_30 = { 0.19, 0.37, 0.65, 0.84, 1.2, 1.4, 1.7, 2.1, 2.4, 2.8, 3.3, 4.2, 5, 5.9, 7, 7.5, 8.5, 9.3, 10.3, 11.2, 12.1, 13, 14, 15.4, 16.6, 17.7, 18.6, 20, 21.4, 23.3 };
		private static double[] druckverlustModul_80_30 = { 0.17, 0.34, 0.6, 0.77, 1.1, 1.3, 1.5, 1.9, 2.2, 2.6, 3.1, 3.8, 4.6, 5.4, 6.1, 6.9, 7.7, 8.5, 9.4, 10.2, 11.1, 11.9, 12.8, 14, 15.1, 16.2, 17, 18.3, 19.6, 21.3 };

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
		private static double spreizungHeizMin = 4;
		private static double spreizungHeizMax = 12;
		private static double spreizungKuehlMin = 2;
		private static double spreizungKuehlMax = 5;
		private static ModulCeilingConstructionEnum construction = ModulCeilingConstructionEnum.C_PROFIL;

		public ModulKlimaDeckeProduct() {

		}

		protected ModulKlimaDeckeProduct(ModulKlimaDeckeProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public new static void StaticInitialize() {
			Configuration userConfig = Configuration.UserTemplate;
			quickDimensioningHeatPowerPerSquareMeter = userConfig.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 80);
			quickDimensioningCoolPowerPerSquareMeter = userConfig.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 80);
			canHeat = userConfig.GetProductParameterAsBool<ModulKlimaDeckeProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = userConfig.GetProductParameterAsBool<ModulKlimaDeckeProduct>("ConfigQuickDimensioningCanCool", true);
			//useHarreitherNorm = true;
			maxPressureLost = userConfig.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigMaxPressureLost", 15000);
			maxDurchfluss = userConfig.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigMaxDurchfluss", 240);
			maxModulesInRow = userConfig.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigMaxModulesInRow", 20);
			maxModulesInParallel = userConfig.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigMaxModulesInParallel", 6);
			maxModulesInCircuit = userConfig.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigModulesInCircuit", 50);
			spreizungHeizMin = userConfig.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigSpreizungHeizMin", 4);
			spreizungHeizMax = userConfig.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigSpreizungHeizMax", 12);
			spreizungKuehlMin = userConfig.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigSpreizungKuehlMin", 2);
			spreizungKuehlMax = userConfig.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigSpreizungKuehlMax", 5);
			construction = userConfig.GetProductParameterAsEnum<ModulKlimaDeckeProduct, ModulCeilingConstructionEnum>("ConfigModulCeilingConstruction", ModulCeilingConstructionEnum.C_PROFIL);
			leistungsFaktor = userConfig.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigLeistungsFaktor", 0.77);
		}

		public static string GlobalNotificationMessage {
			get {
				string message = null;
				Configuration userConfig = Configuration.UserTemplate;

				double defaultLeistungsFaktor = userConfig.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigLeistungsFaktor", 0.77);
				if (leistungsFaktor != defaultLeistungsFaktor) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					message += "  Leistungsfaktor: " + Math.Round(leistungsFaktor, 3).ToString() + " (Standardwert: " + Math.Round(defaultLeistungsFaktor, 3).ToString() + ")";
				}

				if (message != null) {
					message = "Modul Klimadecken-Systeme werden mit veränderten Paramtern berechnet. Folgende Parameter weichen von den Standardwerten ab:\n" + message;
				}

				return message;
			}
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

		public static double ConfigAlphaDk {
			get { return Product.ConfigAlphaBoden; }
		}

		public static double ConfigAlphaDh {
			get { return Product.ConfigAlphaDecke; }
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
		public static double ConfigRLambdaDach {
			get { return rLambdaDach; }
			set { rLambdaDach = value; }
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
		public static double ConfigLeistungsFaktor {
			get { return leistungsFaktor; }
			set { leistungsFaktor = value; }
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
		public static double ConfigMaxMassenstrom {
			get { return maxDurchfluss; }
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
		public static int ConfigModulCeilingConstruction {
			get { return (int)ModulKlimaDeckeProduct.construction; }
			set { ModulKlimaDeckeProduct.construction = (ModulCeilingConstructionEnum)value; }
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
		public static string ConfigDruckverlustModul_120_30String {
			get {
				return ConvertArrayToString(druckverlustModul_120_30);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					druckverlustModul_120_30 = array;
				}
			}
		}
		public static double[] ConfigDruckverlustModul_120_30 {
			get { return druckverlustModul_120_30; }
			set { druckverlustModul_120_30 = value; }
		}

		[ProductParameter]
		public static string ConfigDruckverlustModul_100_30String {
			get {
				return ConvertArrayToString(druckverlustModul_100_30);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					druckverlustModul_100_30 = array;
				}
			}
		}
		public static double[] ConfigDruckverlustModul_100_30 {
			get { return druckverlustModul_100_30; }
			set { druckverlustModul_100_30 = value; }
		}

		[ProductParameter]
		public static string ConfigDruckverlustModul_80_30String {
			get {
				return ConvertArrayToString(druckverlustModul_80_30);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					druckverlustModul_80_30 = array;
				}
			}
		}
		public static double[] ConfigDruckverlustModul_80_30 {
			get { return druckverlustModul_80_30; }
			set { druckverlustModul_80_30 = value; }
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

		public override string QuickDimensioningName {
			get { return QuickDimensioningNameStatic; }
		}

		public static string QuickDimensioningNameStatic {
			get { return "Modul\nKlima\nDecke\n(m²)"; }
		}

		public override ProductType Type {
			get { return ProductType.DH; }
		}

		public override void CalculateHeatAndCoolFlow() {
			base.CalculateHeatAndCoolFlow();
			double spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			double spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			if (spreizungHeat > ModulKlimaDeckeProduct.ConfigSpreizungHeizMax) {
				spreizungHeat = ModulKlimaDeckeProduct.ConfigSpreizungHeizMax;
			}
			if (spreizungHeat < ModulKlimaDeckeProduct.ConfigSpreizungHeizMin) {
				spreizungHeat = ModulKlimaDeckeProduct.ConfigSpreizungHeizMin;
			}
			if (spreizungCool > ModulKlimaDeckeProduct.ConfigSpreizungKuehlMax) {
				spreizungCool = ModulKlimaDeckeProduct.ConfigSpreizungKuehlMax;
			}
			if (spreizungCool < ModulKlimaDeckeProduct.ConfigSpreizungKuehlMin) {
				spreizungCool = ModulKlimaDeckeProduct.ConfigSpreizungKuehlMin;
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
			if (this.plannedCeilingConstruction == null || this.plannedInsulationConstruction == null || this.PlannedConnection == null) {
				this.lastErrorMsg = "Fehlende Eingaben: ";
				if (plannedCeilingConstruction == null) {
					this.lastErrorMsg += "Deckenkonstruktion, ";
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

			this.CalculateHeatAndCoolFlow();
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

			if (variableSpreizung && this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
				double defSpreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
				double defSpreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
				// Heizleistung veringern
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < ModulKlimaDeckeProduct.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad && this.PlannedSpreizungHeat < 1.2 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat -= 0.1;
					foreach (ModulDeckeCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempHeat += 0.1;
				// Heizleistung erhöhen
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > ModulKlimaDeckeProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < ModulKlimaDeckeProduct.ConfigMaxPressureLost / 100 && this.PlannedMaxMhHeat < ModulKlimaDeckeProduct.ConfigMaxMassenstrom && this.PlannedSpreizungHeat > 0.8 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat += 0.1;
					foreach (ModulDeckeCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				// Kühlleistung verringern
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < ModulKlimaDeckeProduct.ConfigSpreizungKuehlMax && this.PlannedCoolLoad > requestedCoolLoad && this.PlannedSpreizungCool < 1.2 * defSpreizungCool) {
					this.plannedRuecklaufTempCool += 0.1;
					foreach (ModulDeckeCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempCool -= 0.1;
				// Kühlleistung erhöhen
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > ModulKlimaDeckeProduct.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < ModulKlimaDeckeProduct.ConfigMaxPressureLost / 100 && this.PlannedMaxMhCool < ModulKlimaDeckeProduct.ConfigMaxMassenstrom && this.PlannedSpreizungCool > 0.8 * defSpreizungCool) {
					this.plannedRuecklaufTempCool -= 0.1;
					foreach (ModulDeckeCircuit c in this.circuits) {
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
			if (Math.Round(this.CoveredCeilingArea, 1) > Math.Round(this.PlannedCeilingArea, 1)) {
				this.lastErrorMsg += "Die Gesamtfläche der Module ist größer als die zur Verfügung stehende Fläche (" + Math.Round(this.CoveredCeilingArea, 1).ToString() + " > " + Math.Round(this.PlannedCeilingArea, 1).ToString() + ")\n";
			}
			// TODO
			/*foreach (ModulDeckeCircuit c in this.circuits) {
				int moduleCount = 0;
				foreach (KlimaFlaechenList row in c.Rows) {
					moduleCount += row.List.Count;
				}
				if (moduleCount > ModulKlimaBodenProduct.ConfigModulesInCircuit) {
					//errorMsg += "Der Heizkreis HK" + c.NrOfCircuit.ToString() + " enthält mehr als 50 Module\n";
					errorMsg += "Der Heizkreis HK" + (c.NrOfCircuit + 1).ToString() + " enthält zu viele Module (" + moduleCount + " > " + ModulKlimaBodenProduct.ConfigModulesInCircuit.ToString() + ")\n";
				}
			}*/
			//double maxTemp = double.MinValue;
			//foreach (ModulDeckeCircuit c in this.circuits) {
			//    if (c.C_FloorTempHeat > maxTemp) {
			//        maxTemp = c.C_FloorTempHeat;
			//    }
			//}
			//if (Math.Round(maxTemp, 1) > (ModulKlimaBodenProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264)) {
			//    errorMsg += "Oberflächentemperatur zu groß (" + Math.Round(maxTemp, 1) + "°C > " + Math.Round((EurovalProduct.ConfigUseHarreitherNorm ? ModulKlimaBodenProduct.ConfigMaxFloorTempHarreither : ModulKlimaBodenProduct.ConfigMaxFloorTempEn1264), 1) + "°C)\n";
			//}
			foreach (ModulDeckeCircuit c in circuits) {
				int saNr = 1;
				int longestRow = 0;
				int moduleCount = 0;
				foreach (ModulDeckeSubArea sa in c.SubAreas) {
					if (sa.Rows.Count > 0) {
						if (sa.Rows.Count > ModulKlimaDeckeProduct.ConfigMaxModulesInParallel) {
							this.lastErrorMsg += "Die Teilfläche " + saNr.ToString() + " im Heizkreis HK" + (c.NrOfCircuit + 1).ToString() + " enthält zu viele parallele Reihen (" + sa.Rows.Count.ToString() + " > " + ModulKlimaDeckeProduct.ConfigMaxModulesInParallel.ToString() + ")\n";
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
							this.lastErrorMsg += "Die Reihe " + maxRowNr.ToString() + " in der Teilfläche " + saNr.ToString() + " im Heizkreis HK" + (c.NrOfCircuit + 1).ToString() + " ist um mehr als 1 Modul länger als die Reihe " + minRowNr.ToString() + " (" + maxModules.ToString() + ", " + minModules.ToString() + ")\n";
						}
						longestRow += maxModules;
					}
					saNr++;
				}
				if (longestRow > ModulKlimaDeckeProduct.ConfigMaxModulesInRow) {
					this.lastErrorMsg += "Der Heizkreis HK" + (c.NrOfCircuit + 1).ToString() + " enthält zu viele Module in Serie (" + longestRow.ToString() + " > " + ModulKlimaDeckeProduct.ConfigMaxModulesInRow.ToString() + ")\n";
				}
				if (moduleCount > ModulKlimaDeckeProduct.ConfigModulesInCircuit) {
					this.lastErrorMsg += "Der Heizkreis HK" + (c.NrOfCircuit + 1).ToString() + " enthält zu viele Module (" + moduleCount.ToString() + " > " + ModulKlimaDeckeProduct.ConfigModulesInCircuit.ToString() + ")\n";
				}
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

		[XmlIgnore]
		public override Construction PlannedInsideConstruction {
			get { return this.plannedCeilingConstruction; }
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
				foreach (ModulDeckeCircuit c in this.circuits) {
					foreach (ModulDeckeSubArea subArea in c.SubAreas) {
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

			int nrOfElements = 0;
			int nrOfOtherElements = 0;
			int rows = 0;
			int subAreas = 0;
			double modulArea = 0;
			foreach (ModulDeckeCircuit c in this.circuits) {
				foreach (ModulDeckeSubArea subArea in c.SubAreas) {
					subAreas++;
					foreach (KlimaFlaechenList row in subArea.Rows) {
						rows++;
						pipe21mmLength += row.LengthVerbindeleitungen;
						pipe21mmLength += 1.4;
						foreach (KlimaFlaechenModul modul in row.List) {
							// Modul
							Project.Instance.AddRequiredMaterial(requiredMaterial, modul.PartNumber, 1);
							nrOfElements++;
							modulArea += modul.Area;
							if (modul.ModulType == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || modul.ModulType == KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) {
								nrOfOtherElements++;
							}
						}
					}
				}
			}

			Project.Instance.AddRequiredMaterial(requiredMaterial, "EV01", pipeEurovalLength);
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI51", pipe21mmLength);

			// Muffe
			if (circuit21mmOnlyFirstLength > 0) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", (circuit21mmOnlyFirstLength + circuit21mmAllLength) / 2);
				if (this.PlannedCircuitCount > 1) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", circuit21mmAllLength * (this.PlannedCircuitCount - 1) / 2);
				}
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", circuit21mmAllLength * this.PlannedCircuitCount / 2);
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", subAreas + rows);

			// T-Stück
			Project.Instance.AddRequiredMaterial(requiredMaterial, "MK20", (rows - 1) * 2);

			if (ConfigModulCeilingConstruction == (int)ModulCeilingConstructionEnum.HOLZSTAFFEL) {
				// Winkel 90°
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI56", subAreas * 2);

				// Holzstaffel
				if (modulArea > 0) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "MK51", modulArea * 3);
				}
			} else {
				// Winkel 90°
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI56", (rows + 1) * 2);

				// Einhängebügel
				Project.Instance.AddRequiredMaterial(requiredMaterial, "MK50", (nrOfElements - nrOfOtherElements) * 4);
				if (nrOfOtherElements > 0) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "MK49", nrOfOtherElements * 4);
				}
			}

			// Winkel 45° in Wand
			if (this.Type == ProductType.WH) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI57", nrOfElements * 2);
			}
			
		}

		public override double Dichte {
			get { return ModulKlimaDeckeProduct.ConfigRho; }
		}

		public override double Waermekapazitaet {
			get { return ModulKlimaDeckeProduct.ConfigC; }
		}

		public override double Viskositaet {
			get { return ModulKlimaDeckeProduct.ConfigV; }
		}

		public override string NotificationMessage {
			get {
				string notification = base.NotificationMessage;
				if (Math.Round(this.CoveredCeilingArea, 1) > Math.Round(this.PlannedCeilingArea * 3 / 4, 1) && Math.Round(this.CoveredCeilingArea, 1) <= Math.Round(this.PlannedCeilingArea, 1)) {
					string newNotification = "Es sind mehr als 75% der Gesamtfläche mit Modulen belegt (" + Math.Round(this.CoveredCeilingArea, 1).ToString() + " > " + Math.Round(this.PlannedCeilingArea * 3 / 4, 1).ToString() + ")";
					if (notification == null) {
						notification = newNotification;
					} else {
						notification = notification + "\n" + newNotification;
					}
				}
				return notification;
			}
		}
	}
	
}
