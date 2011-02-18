using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Threading;
using Europlan.Licensing;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Product_ModulKlimDeckeName", "Product_ModulKlimDeckeFullName")]
	public class ModulKlimaDeckeProduct : Product, ProductWithInsulationConstruction {

		public class ModulCeilingConstructionEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string kassettenDecke = EuroplanRes.ModulKlimaDeckeProduct_Kassettendecke; //"Kassettendecke";
			private static readonly string c_profil = EuroplanRes.ModulKlimaDeckeProduct_CProfil; //"C-Profil";
			private static readonly string holzStaffel = EuroplanRes.ModulKlimaDeckeProduct_Holzstaffel; //"Holzstaffel";

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
		private static double leistungsFaktorHeizen = 0.95;
		private static double leistungsFaktorKuehlen = 0.95;

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
		private string plannedCeilingConstructionId = null;
		private string plannedInsulationConstructionId = null;

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
		private static ModulKlimaDeckeConstructionKassette.RasterMass rasterMass = ModulKlimaDeckeConstructionKassette.RasterMass.Raster_1050_450;

		private static double maxCeilingTempHeat = 29.0;

		private ProductType modulType = ProductType.DH;
		private float plannedFloorArea = 0;
		private float plannedCeilingArea = 0;
		private float plannedFloorOrCeilingArea = 0;

		private ModulKlimaDeckeConstruction graphConstruction = null;

		public ModulKlimaDeckeProduct() {
			if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdModulKlimaDecke)) {
				throw new ProductNotLicensedException(this.GetType());
			}
		}

		protected ModulKlimaDeckeProduct(ModulKlimaDeckeProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public override string ImageKey {
            get { return "Klimadecke.png"; }
		}

		public override string SelectedImageKey {
            get { return "Klimadecke.png"; }
		}

		public override Product.CalculateModeEnum DefaultCalculateMode {
			get { return CalculateModeEnum.COOL; }
		}

		public new static void StaticInitialize(Configuration config) {
			/*quickDimensioningHeatPowerPerSquareMeter = config.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 80);
			quickDimensioningCoolPowerPerSquareMeter = config.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 80);
			canHeat = config.GetProductParameterAsBool<ModulKlimaDeckeProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = config.GetProductParameterAsBool<ModulKlimaDeckeProduct>("ConfigQuickDimensioningCanCool", true);
			//useHarreitherNorm = true;
			maxPressureLost = config.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigMaxPressureLost", 15000);
			maxDurchfluss = config.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigMaxDurchfluss", 240);
			maxModulesInRow = config.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigMaxModulesInRow", 20);
			maxModulesInParallel = config.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigMaxModulesInParallel", 6);
			maxModulesInCircuit = config.GetProductParameterAsInt<ModulKlimaDeckeProduct>("ConfigModulesInCircuit", 50);
			spreizungHeizMin = config.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigSpreizungHeizMin", 4);
			spreizungHeizMax = config.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigSpreizungHeizMax", 12);
			spreizungKuehlMin = config.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigSpreizungKuehlMin", 2);
			spreizungKuehlMax = config.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigSpreizungKuehlMax", 5);
			construction = config.GetProductParameterAsEnum<ModulKlimaDeckeProduct, ModulCeilingConstructionEnum>("ConfigModulCeilingConstruction", ModulCeilingConstructionEnum.C_PROFIL);
			leistungsFaktorHeizen = config.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigLeistungsFaktorHeizen", 0.95);
			leistungsFaktorKuehlen = config.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigLeistungsFaktorKuehlen", 0.95);*/
			Product.StaticInitialize<ModulKlimaDeckeProduct>(config);
		}

		public static string GlobalNotificationMessage {
			get {
				string message = null;
				Configuration userConfig = Configuration.UserTemplate;

				double defaultLeistungsFaktorHeizen = userConfig.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigLeistungsFaktorHeizen");
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

				double defaultLeistungsFaktorKuehlen = userConfig.GetProductParameterAsDouble<ModulKlimaDeckeProduct>("ConfigLeistungsFaktorKuehlen");
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
					message = EuroplanRes.ModulKlimaDeckeProduct_NotificationParameter + /*"Hitherm-Systeme werden mit veränderten Paramtern berechnet. Folgende Parameter weichen von den Standardwerten ab:\n" */
						"\n" + message;
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

		[IntProductParameter(80)]
		public static int ConfigQuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
			set { quickDimensioningHeatPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
		}

		[IntProductParameter(80)]
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

		//public static double ConfigAlphaDk {
		//    get { return Product.ConfigAlphaBoden; }
		//}

		//public static double ConfigAlphaDh {
		//    get { return Product.ConfigAlphaDecke; }
		//}

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

		[DoubleProductParameter(0)]
		public static double ConfigRLambdaDach {
			get { return rLambdaDach; }
			set { rLambdaDach = value; }
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

		[DoubleProductParameter(0.95)]
		public static double ConfigLeistungsFaktorHeizen {
			get { return leistungsFaktorHeizen; }
			set { leistungsFaktorHeizen = value; }
		}

		[DoubleProductParameter(0.95)]
		public static double ConfigLeistungsFaktorKuehlen {
			get { return leistungsFaktorKuehlen; }
			set { leistungsFaktorKuehlen = value; }
		}

		[IntProductParameter(15000)]
		public static int ConfigMaxPressureLost {
			get { return ModulKlimaDeckeProduct.maxPressureLost; }
			set { ModulKlimaDeckeProduct.maxPressureLost = value; }
		}

		[IntProductParameter(240)]
		public static int ConfigMaxDurchfluss {
			get { return ModulKlimaDeckeProduct.maxDurchfluss; }
			set { ModulKlimaDeckeProduct.maxDurchfluss = value; }
		}
		public static double ConfigMaxMassenstrom {
			get { return maxDurchfluss; }
		}

		[IntProductParameter(20)]
		public static int ConfigMaxModulesInRow {
			get { return ModulKlimaDeckeProduct.maxModulesInRow; }
			set { ModulKlimaDeckeProduct.maxModulesInRow = value; }
		}

		[IntProductParameter(6)]
		public static int ConfigMaxModulesInParallel {
			get { return ModulKlimaDeckeProduct.maxModulesInParallel; }
			set { ModulKlimaDeckeProduct.maxModulesInParallel = value; }
		}

		[IntProductParameter(50)]
		public static int ConfigModulesInCircuit {
			get { return ModulKlimaDeckeProduct.maxModulesInCircuit; }
			set { ModulKlimaDeckeProduct.maxModulesInCircuit = value; }
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

		[IntProductParameter(1)]
		public static int ConfigModulCeilingConstruction {
			get { return (int)ModulKlimaDeckeProduct.construction; }
			set { ModulKlimaDeckeProduct.construction = (ModulCeilingConstructionEnum)value; }
		}

		[IntProductParameter(0)]
		public static int ConfigModulCeilingConstructionKassetteRasterMass {
			get { return (int)ModulKlimaDeckeProduct.rasterMass; }
			set { ModulKlimaDeckeProduct.rasterMass = (ModulKlimaDeckeConstructionKassette.RasterMass)value; }
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

		[StringProductParameter("{0.23, 0.47, 0.82, 1.05, 1.5, 1.75, 2.1, 2.6, 3, 3.5, 4.2, 5.25, 6.3, 7.3, 8.4, 9.4, 10.6, 11.7, 12.8, 14, 15.1, 16.3, 17.5, 19.2, 20.7, 22.1, 23.3, 25, 26.8, 29.1}")]
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

		[StringProductParameter("{0.19, 0.37, 0.65, 0.84, 1.2, 1.4, 1.7, 2.1, 2.4, 2.8, 3.3, 4.2, 5, 5.9, 7, 7.5, 8.5, 9.3, 10.3, 11.2, 12.1, 13, 14, 15.4, 16.6, 17.7, 18.6, 20, 21.4, 23.3}")]
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

		[StringProductParameter("{0.17, 0.34, 0.6, 0.77, 1.1, 1.3, 1.5, 1.9, 2.2, 2.6, 3.1, 3.8, 4.6, 5.4, 6.1, 6.9, 7.7, 8.5, 9.4, 10.2, 11.1, 11.9, 12.8, 14, 15.1, 16.2, 17, 18.3, 19.6, 21.3}")]
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

		[DoubleProductParameter(29)]
		public static double ConfigMaxCeilingTempHeat {
			get { return maxCeilingTempHeat; }
			set { maxCeilingTempHeat = value; }
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
			this.requestedHeatLoad = requestedHeatLoad;
			this.requestedCoolLoad = requestedCoolLoad;
			this.incompleteCalculation = false;
			if (this.PlannedCeilingConstruction == null || this.PlannedInsulationConstruction == null || this.PlannedConnection == null) {
				this.lastErrorMsg = EuroplanRes.ErrorMessage_FehlendeEingaben + " "; //"Fehlende Eingaben: ";
				if (PlannedCeilingConstruction == null) {
					this.lastErrorMsg += EuroplanRes.ErrorMessage_FehlendeEingabenDecke + ", "; //"Fußbodenkonstruktion, ";
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
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > ModulKlimaDeckeProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < ModulKlimaDeckeProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhHeat < ModulKlimaDeckeProduct.ConfigMaxMassenstrom && this.PlannedSpreizungHeat > 0.8 * defSpreizungHeat) {
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
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > ModulKlimaDeckeProduct.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < ModulKlimaDeckeProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhCool < ModulKlimaDeckeProduct.ConfigMaxMassenstrom && this.PlannedSpreizungCool > 0.8 * defSpreizungCool) {
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
			string newMsg;
			if (this.ModulType == ProductType.DH && Math.Round(this.CoveredArea, 1) > Math.Round(this.PlannedCeilingArea, 1)) {
				newMsg = EuroplanRes.ErrorMessage_Modulflaeche;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.CoveredArea, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.PlannedCeilingArea, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
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
							newMsg = EuroplanRes.ErrorMessage_ModulReihen;
							newMsg = newMsg.Replace("%TEILFL%", saNr.ToString());
							newMsg = newMsg.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
							newMsg = newMsg.Replace("%VALUE%", sa.Rows.Count.ToString());
							newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaDeckeProduct.ConfigMaxModulesInParallel.ToString());
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
							newMsg = newMsg.Replace("%VALUEKURZ%", ModulKlimaDeckeProduct.ConfigMaxModulesInParallel.ToString());
							this.lastErrorMsg += newMsg + "\n";
						}
						longestRow += maxModules;
					}
					saNr++;
				}
				if (longestRow > ModulKlimaDeckeProduct.ConfigMaxModulesInRow) {
					newMsg = EuroplanRes.ErrorMessage_ModulReiheLaenge;
					newMsg = newMsg.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
					newMsg = newMsg.Replace("%VALUE%", longestRow.ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaDeckeProduct.ConfigMaxModulesInRow.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
				if (moduleCount > ModulKlimaDeckeProduct.ConfigModulesInCircuit) {
					newMsg = EuroplanRes.ErrorMessage_ModulAnzahl;
					newMsg = newMsg.Replace("%HK%", (c.NrOfCircuit + 1).ToString());
					newMsg = newMsg.Replace("%VALUE%", moduleCount.ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaDeckeProduct.ConfigModulesInCircuit.ToString());
				}
			}
			if (this.PlannedMaxMhHeat >= this.PlannedMaxMhCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedMaxMhHeat, 1) > ModulKlimaDeckeProduct.ConfigMaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhHeat, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaDeckeProduct.ConfigMaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedMaxMhCool, 1) > ModulKlimaDeckeProduct.ConfigMaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhCool, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", ModulKlimaDeckeProduct.ConfigMaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > Math.Round(ModulKlimaDeckeProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoHeat, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ModulKlimaDeckeProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > Math.Round(ModulKlimaDeckeProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoCool, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ModulKlimaDeckeProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (Math.Round(this.PlannedCeilingTemperatureHeat, 1) > Math.Round(ModulKlimaDeckeProduct.ConfigMaxCeilingTempHeat, 1) && this.requestedHeatLoad > 0) {
				newMsg = EuroplanRes.ErrorMessage_DeckentemperaturHeat;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedCeilingTemperatureHeat, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ModulKlimaDeckeProduct.ConfigMaxCeilingTempHeat, 1).ToString());
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

		/*public override float PlannedCeilingArea {
			get { return this.plannedArea; }
			set { this.plannedArea = value; }
		}*/

		[XmlIgnore]
		public double CoveredArea {
			get {
				double area = 0;
				foreach (ModulDeckeCircuit mc in this.circuits) {
					area += mc.CoveredArea;
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
				if (this.incompleteCalculation || this.requestedHeatLoad == 0) {
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
			get {
				double area = 0;
				foreach (ModulDeckeCircuit hc in this.circuits) {
					area += hc.CoveredArea;
				}
				return (float)area;
				/*switch (this.ModulType) {
					case ProductType.DH:
						return this.PlannedCeilingArea - this.PlannedAreaUnheated;
					case ProductType.WH:
						return this.PlannedWallArea;
					case ProductType.FBH:
						return this.PlannedFloorArea;
					default:
						return 0;
				}*/
			}
		}

		/// <summary>
		/// The id of the planned ceiling construction for serialization
		/// </summary>
		public string PlannedCeilingConstructionId {
			get { return this.PlannedCeilingConstruction == null ? this.plannedCeilingConstructionId : this.PlannedCeilingConstruction.Id; }
			set {
				this.plannedCeilingConstructionId = value;
				this.plannedCeilingConstruction = null;
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
		public Construction PlannedCeilingConstruction {
			get {
				if (this.plannedCeilingConstructionId != null) {
					this.plannedCeilingConstruction = Project.Instance.Config.GetConstruction(this.plannedCeilingConstructionId);
					this.plannedCeilingConstructionId = null;
				}
				return this.plannedCeilingConstruction;
			}
			set {
				this.plannedCeilingConstruction = value;
				this.plannedCeilingConstructionId = null;
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
		public float PlannedCeilingConstructionRValue {
			get { return (this.PlannedCeilingConstruction == null ? 0 : this.PlannedCeilingConstruction.RValue); }
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
			get { return (this.PlannedCeilingConstruction == null ? 0 : this.PlannedCeilingConstruction.RValue); }
		}

		[XmlIgnore]
		public override bool HasInsideConstruction {
			get { return this.PlannedCeilingConstruction != null; }
		}

		[XmlIgnore]
		public override Construction PlannedInsideConstruction {
			get { return this.PlannedCeilingConstruction; }
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

			double additional21mm = 0;
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
						additional21mm += row.LengthVerbindeleitungen;
						additional21mm += 1.4;
						foreach (KlimaFlaechenModul modul in row.List) {
							// Modul
							Project.Instance.AddRequiredMaterial(requiredMaterial, modul.PartNumber, 1);
							nrOfElements++;
							modulArea += modul.GetHeatArea(false);
							if (modul.ModulType == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 ||
								modul.ModulType == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B ||
								modul.ModulType == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C ||
								modul.ModulType == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D || 
								modul.ModulType == KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40) {
								nrOfOtherElements++;
							}
						}
					}
				}
			}

			this.AddRequiredMaterialForConnections(requiredMaterial, false, additional21mm);

			// Muffe
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
				return notification;
			}
		}

		internal override void FinalizeLoading(PlannedProduct pp) {
			base.FinalizeLoading(pp);
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
		public double PlannedCeilingTemperatureHeat {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (ModulDeckeCircuit mc in this.circuits) {
					if (mc.C_CeilingTempHeat > value) {
						value = mc.C_CeilingTempHeat;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedCeilingTemperatureCool {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = Double.MaxValue;
				foreach (ModulDeckeCircuit mc in this.circuits) {
					if (mc.C_CeilingTempCool < value) {
						value = mc.C_CeilingTempCool;
					}
				}
				return value;
			}
		}

		public ModulKlimaDeckeConstruction GraphConstruction {
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
				foreach (ModulDeckeCircuit c in this.circuits) {
					foreach (ModulDeckeSubArea subArea in c.SubAreas) {
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
			foreach (ModulDeckeCircuit c in this.circuits) {
				foreach (ModulDeckeSubArea subArea in c.SubAreas) {
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
			foreach (ModulDeckeCircuit circuit in this.circuits) {
				foreach (ModulDeckeSubArea subArea in circuit.SubAreas) {
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
			foreach (ModulDeckeCircuit circuit in this.circuits) {
				count += circuit.CountModules();
			}
			return count;
		}

		public ModulDeckeCircuit GetCircuitForModul(KlimaFlaechenModul modul, out int index) {
			index = 0;
			foreach (ModulDeckeCircuit c in this.circuits) {
				if (c.ContainsModul(modul)) {
					return c;
				}
				index++;
			}
			index = -1;
			return null;
		}
	}

	public struct KlimaFlaechenModulWithRowAndCircuit {
		public KlimaFlaechenModul modul;
		public KlimaFlaechenList row;
		public ModulDeckeCircuit circuit;

		public KlimaFlaechenModulWithRowAndCircuit(KlimaFlaechenModul modul, KlimaFlaechenList row, ModulDeckeCircuit circuit) {
			this.modul = modul;
			this.row = row;
			this.circuit = circuit;
		}
	}
}
