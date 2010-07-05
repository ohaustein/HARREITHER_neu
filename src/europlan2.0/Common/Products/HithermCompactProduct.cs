using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;
using Europlan.Licensing;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Product_HithermCompactName", "Product_HithermCompactFullName")]
	public class HithermCompactProduct : Product {

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
		private static double spreizungKuehlMin = 2;
		private static double spreizungKuehlMax = 5;

		private static int maxPressureLost = 15000;
		private static int maxDurchfluss = 240;

		private static double maxRegisterArea = 10.0;

		//     Diffenz Raumtemp - Kuehlmitteltemp (K):  0   2   3   4   5   6   7   9
		//private static double[] regKuehlleistung = { 0, 13, 20, 25, 33, 40, 45, 60 };

		private static double[][] regHeizleistung2500Std = {
			//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
			new double[] { 180,  205,  230,  260,  285,  310}, // ti=15°C
			new double[] { 150,  175,  200,  225,  250,  280}, // ti=18°C
			new double[] { 130,  155,  180,  205,  230,  260}, // ti=20°C
			new double[] { 110,  135,  160,  185,  210,  235}, // ti=22°C
			new double[] {  90,  115,  140,  165,  190,  215}  // ti=24°C
		};

		private static double[][] regHeizleistung2000Std = {
			//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
			new double[] { 145,  165,  185,  210,  230,  250}, // ti=15°C
			new double[] { 120,  140,  160,  185,  205,  225}, // ti=18°C
			new double[] { 105,  125,  145,  165,  185,  210}, // ti=20°C
			new double[] {  85,  110,  130,  150,  170,  190}, // ti=22°C
			new double[] {  75,   90,  110,  135,  155,  175}  // ti=24°C
		};

		private static double[][] regHeizleistung1500Std = {
			//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
			new double[] { 110,  130,  145,  160,  175,  190}, // ti=15°C
			new double[] {  95,  110,  125,  140,  155,  175}, // ti=18°C
			new double[] {  80,   95,  110,  130,  145,  160}, // ti=20°C
			new double[] {  65,   85,  100,  115,  130,  145}, // ti=22°C
			new double[] {  55,   70,   85,  100,  120,  135}  // ti=24°C
		};

		private static double[][] regHeizleistung1000Std = {
			//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
			new double[] {  75,   85,  100,  110,  120,  130}, // ti=15°C
			new double[] {  65,   75,   85,   95,  105,  120}, // ti=18°C
			new double[] {  55,   65,   75,   85,  100,  110}, // ti=20°C
			new double[] {  45,   55,   65,   80,   90,  100}, // ti=22°C
			new double[] {  35,   50,   60,   70,   80,   90}  // ti=24°C
		};

		private static double[][] regHeizleistung620Std = {
			//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
			new double[] {37.5, 42.5,   50,   55,   60,   65}, // ti=15°C
			new double[] {32.5, 37.5, 42.5, 47.5, 52.5,   60}, // ti=18°C
			new double[] {27.5, 32.5, 37.5, 42.5,   50,   55}, // ti=20°C
			new double[] {22.5, 27.5, 32.5,   40,   45,   50}, // ti=22°C
			new double[] {17.5,   25,   30,   35,   40,   45}  // ti=24°C
		};

		private static double[][] regHeizleistung2000Par = {
			//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
			new double[] { 160,  180,  205,  225,  250,  275}, // ti=15°C
			new double[] { 130,  155,  175,  200,  220,  245}, // ti=18°C
			new double[] { 115,  135,  160,  180,  205,  225}, // ti=20°C
			new double[] {  95,  120,  140,  165,  185,  210}, // ti=22°C
			new double[] {  75,  100,  125,  145,  170,  190}  // ti=24°C
		};

		private static double[][] regHeizleistung1500Par = {
			//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
			new double[] { 120,  135,  155,  170,  190,  205}, // ti=15°C
			new double[] { 100,  115,  135,  150,  170,  185}, // ti=18°C
			new double[] {  85,  105,  120,  135,  155,  170}, // ti=20°C
			new double[] {  70,   90,  105,  125,  140,  160}, // ti=22°C
			new double[] {  60,   75,   90,  110,  125,  145}  // ti=24°C
		};

		private static double[][] regHeizleistung1000Par = {
			//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
			new double[] {  80,   90,  100,  115,  125,  135}, // ti=15°C
			new double[] {  65,   75,   90,  100,  110,  125}, // ti=18°C
			new double[] {  55,   70,   80,   90,  100,  115}, // ti=20°C
			new double[] {  50,   60,   70,   80,   95,  105}, // ti=22°C
			new double[] {  40,   50,   60,   75,   85,   95}  // ti=24°C
		};

		private static double[][] regKuehlleistungProQm = {
			//  tKm (°C)  16.0  18.0  20.0  22.0
			new double[] {12.5,  0.0            }, // ti=18°C
			new double[] {24.0, 12.5,  0.0      }, // ti=20°C
			new double[] {39.0, 25.0, 12.5,  0.0}, // ti=22°C
			new double[] {58.0, 44.0, 32.0, 19.5}  // ti=25°C
		};

		/*private static double[] druckverlustHITC_620 = { 0.1, 0.3, 0.4, 0.6, 0.8, 0.9, 1.1, 1.3, 1.6, 1.8, 2.3, 2.8, 3.4, 4.0, 4.7, 5.4, 6.2, 7.0, 7.9, 8.8, 9.7, 10.7, 11.8, 12.8, 14.0 };
		private static double[] druckverlustHITC_1000 = { 0.2, 0.4, 0.7, 0.9, 1.2, 1.4, 1.7, 2.0, 2.3, 2.7, 3.3, 4.1, 4.9, 5.7, 6.6, 7.5, 8.5, 9.5, 10.6, 11.7, 12.9, 14.2, 15.5, 16.8, 18.2 };
		private static double[] druckverlustHITC_1500 = { 0.1, 0.3, 0.5, 0.7, 0.9, 1.1, 1.4, 1.7, 2.0, 2.3, 3.0, 3.9, 4.7, 5.7, 6.8, 7.9, 9.2, 10.5, 11.9, 13.4, 15.0, 16.7, 18.4, 20.3, 22.2 };
		private static double[] druckverlustHITC_2000 = { 0.2, 0.4, 0.7, 0.9, 1.2, 1.5, 1.9, 2.2, 2.6, 3.0, 3.8, 4.8, 5.8, 6.9, 8.1, 9.3, 10.7, 12.1, 13.7, 15.3, 17.0, 18.7, 20.6, 22.6, 24.6 };
		private static double[] druckverlustHITC_2500 = { 0.25, 0.5, 0.875, 1.125, 1.5, 1.875, 2.375, 2.75, 3.25, 3.75, 4.75, 6.0, 7.25, 8.625, 10.125, 11.625, 13.375, 15.125, 17.125, 19.125, 21.25, 23.375, 25.75, 28.25, 30.75 };*/

		private static double[] beplankungRWerte = { 0, 0.01, 0.02, 0.1 };
		private static double[] beplankungFaktoren = { 1, 0.95, 0.91, 0.66 };

		private static double defaultDaemmung = 2.5;

		private static bool usePlus = false;

		private static double leistungsFaktorKuehlen = 1.0;
		private static double leistungsFaktorHeizen = 1.0;

		private Dictionary<HithermCompactRegister, int> registerCircuits = new Dictionary<HithermCompactRegister, int>();
		private Dictionary<int, HithermCompactCircuit> circuitIds = new Dictionary<int, HithermCompactCircuit>();

		private ProductType hithermCompactType = ProductType.WH;
		private float plannedFloorArea = 0;
		private float plannedCeilingArea = 0;
		private float plannedRoofArea = 0;
		private float plannedFloorCeilingRoofArea = 0;

		public HithermCompactProduct() {
			if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHithermCompact)) {
				throw new ProductNotLicensedException(this.GetType());
			}
		}

		protected HithermCompactProduct(HithermCompactProduct product) : base(product) {
		}

		public override void Initialize() {
		}

		public new static void StaticInitialize(Configuration config) {
			quickDimensioningHeatPowerPerSquareMeter = config.GetProductParameterAsInt<HithermCompactProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 100);
			quickDimensioningCoolPowerPerSquareMeter = config.GetProductParameterAsInt<HithermCompactProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 100);
			canHeat = config.GetProductParameterAsBool<HithermCompactProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = config.GetProductParameterAsBool<HithermCompactProduct>("ConfigQuickDimensioningCanCool", false);
			usePlus = config.GetProductParameterAsBool<HithermCompactProduct>("ConfigUsePlus", false);
			maxPressureLost = config.GetProductParameterAsInt<HithermCompactProduct>("ConfigMaxPressureLost", 15000);
			maxDurchfluss = config.GetProductParameterAsInt<HithermCompactProduct>("ConfigMaxDurchfluss", 240);
			maxRegisterArea = config.GetProductParameterAsDouble<HithermCompactProduct>("ConfigMaxRegisterArea", 10.0);
			leistungsFaktorHeizen = config.GetProductParameterAsDouble<HithermCompactProduct>("ConfigLeistungsFaktorHeizen", 1.0);
			leistungsFaktorKuehlen = config.GetProductParameterAsDouble<HithermCompactProduct>("ConfigLeistungsFaktorKuehlen", 1.0);
		}

		public static string GlobalNotificationMessage {
			get {
				string message = null;
				Configuration userConfig = Configuration.UserTemplate;

				double defaultLeistungsFaktorHeizen = userConfig.GetProductParameterAsDouble<HithermCompactProduct>("ConfigLeistungsFaktorHeizen", 1.0);
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

				double defaultLeistungsFaktorKuehlen = userConfig.GetProductParameterAsDouble<HithermCompactProduct>("ConfigLeistungsFaktorKuehlen", 1.0);
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
					message = EuroplanRes.HithermCompactProduct_NotificationParameter + /*"Hitherm Compact-Systeme werden mit veränderten Paramtern berechnet. Folgende Parameter weichen von den Standardwerten ab:\n" */
						"\n" + message;
				}

				return message;
			}
		}

		public override Product Clone(Room room) {
			HithermCompactProduct product = new HithermCompactProduct(this);
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
			get { return canHeat; }
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
		public static double ConfigMaxMassenstrom {
			get { return maxDurchfluss * rho / 1000; }
		}

		[ProductParameter]
		public static double ConfigMaxRegisterArea {
			get { return maxRegisterArea; }
			set { maxRegisterArea = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistung2500StdString {
			get {
				return ConvertArrayToString2(regHeizleistung2500Std);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung2500Std = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung2500Std {
			get { return regHeizleistung2500Std; }
			set { regHeizleistung2500Std = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistung2000StdString {
			get {
				return ConvertArrayToString2(regHeizleistung2000Std);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung2000Std = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung2000Std {
			get { return regHeizleistung2000Std; }
			set { regHeizleistung2000Std = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistung1500StdString {
			get {
				return ConvertArrayToString2(regHeizleistung1500Std);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung1500Std = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung1500Std {
			get { return regHeizleistung1500Std; }
			set { regHeizleistung1500Std = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistung1000StdString {
			get {
				return ConvertArrayToString2(regHeizleistung1000Std);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung1000Std = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung1000Std {
			get { return regHeizleistung1000Std; }
			set { regHeizleistung1000Std = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistung620StdString {
			get {
				return ConvertArrayToString2(regHeizleistung620Std);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung620Std = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung620Std {
			get { return regHeizleistung620Std; }
			set { regHeizleistung620Std = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistung2000ParString {
			get {
				return ConvertArrayToString2(regHeizleistung2000Par);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung2000Par = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung2000Par {
			get { return regHeizleistung2000Par; }
			set { regHeizleistung2000Par = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistung1500ParString {
			get {
				return ConvertArrayToString2(regHeizleistung1500Par);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung1500Par = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung1500Par {
			get { return regHeizleistung1500Par; }
			set { regHeizleistung1500Par = value; }
		}

		[ProductParameter]
		public static string ConfigHlRegHeizleistung1000ParString {
			get {
				return ConvertArrayToString2(regHeizleistung1000Par);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regHeizleistung1000Par = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung1000Par {
			get { return regHeizleistung1000Par; }
			set { regHeizleistung1000Par = value; }
		}


		[ProductParameter]
		public static string ConfigHlRegKuehlleistungProQmString {
			get {
				return ConvertArrayToString2(regKuehlleistungProQm);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					regKuehlleistungProQm = array;
				}
			}
		}
		public static double[][] ConfigHlRegKuehlleistungProQm {
			get { return regKuehlleistungProQm; }
			set { regKuehlleistungProQm = value; }
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
		public static double ConfigDefaultDaemmung {
			get { return defaultDaemmung; }
			set { defaultDaemmung = value; }
		}

		[ProductParameter]
		public static bool ConfigUsePlus {
			get { return usePlus; }
			set { usePlus = value; }
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
		public static double ConfigLeistungsFaktorKuehlen {
			get { return leistungsFaktorKuehlen; }
			set { leistungsFaktorKuehlen = value; }
		}

		[ProductParameter]
		public static double ConfigLeistungsFaktorHeizen {
			get { return leistungsFaktorHeizen; }
			set { leistungsFaktorHeizen = value; }
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
			get { return QuickDimensioningNameStatic; }
		}

		public static string QuickDimensioningNameStatic {
			get { return "Hitherm®\nCompact\n(m²)"; }
		}

		public override ProductType Type {
			get { return this.hithermCompactType; }
		}

		public ProductType HithermCompactType {
			get { return this.hithermCompactType; }
			set { this.hithermCompactType = value; }
		}

		public override void CalculateHeatAndCoolFlow() {
			base.CalculateHeatAndCoolFlow();
			double spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			double spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			if (spreizungHeat > HithermCompactProduct.ConfigSpreizungHeizMax) {
				spreizungHeat = HithermCompactProduct.ConfigSpreizungHeizMax;
			}
			if (spreizungHeat < HithermCompactProduct.ConfigSpreizungHeizMin) {
				spreizungHeat = HithermCompactProduct.ConfigSpreizungHeizMin;
			}
			if (spreizungCool > HithermCompactProduct.ConfigSpreizungKuehlMax) {
				spreizungCool = HithermCompactProduct.ConfigSpreizungKuehlMax;
			}
			if (spreizungCool < HithermCompactProduct.ConfigSpreizungKuehlMin) {
				spreizungCool = HithermCompactProduct.ConfigSpreizungKuehlMin;
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
			if (this.PlannedConnection == null) {
				this.lastErrorMsg = EuroplanRes.ErrorMessage_FehlendeEingaben + " "; //"Fehlende Eingaben: ";
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
			foreach (HithermCompactCircuit hc in this.circuits) {
				hc.HithermCompactProduct = this;
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
				i++;
			}

			if (variableSpreizung && this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
				double defSpreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
				double defSpreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
				// Heizleistung veringern
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < HithermCompactProduct.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad && this.PlannedSpreizungHeat < 1.2 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat -= 0.1;
					foreach (HithermCompactCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempHeat += 0.1;
				// Heizleistung erhöhen
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > HithermCompactProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < HithermCompactProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhHeat < HithermCompactProduct.ConfigMaxMassenstrom && this.PlannedSpreizungHeat > 0.8 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat += 0.1;
					foreach (HithermCompactCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				// Kühlleistung verringern
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < HithermCompactProduct.ConfigSpreizungKuehlMax && this.PlannedCoolLoad > requestedCoolLoad && this.PlannedSpreizungCool < 1.2 * defSpreizungCool) {
					this.plannedRuecklaufTempCool += 0.1;
					foreach (HithermCompactCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempCool -= 0.1;
				// Kühlleistung erhöhen
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > HithermCompactProduct.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < HithermCompactProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhCool < HithermCompactProduct.ConfigMaxMassenstrom && this.PlannedSpreizungCool > 0.8 * defSpreizungCool) {
					this.plannedRuecklaufTempCool -= 0.1;
					foreach (HithermCompactCircuit c in this.circuits) {
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
			foreach (HithermCompactCircuit hc in this.circuits) {
				if (Math.Round(hc.CoveredArea, 1) > Math.Round(ConfigMaxRegisterArea, 1)) {
					newMsg = EuroplanRes.ErrorMessage_BelegteFlaeche;
					newMsg = newMsg.Replace("%HK%", (hc.NrOfCircuit + 1).ToString());
					newMsg = newMsg.Replace("%VALUE%", Math.Round(hc.CoveredArea, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ConfigMaxRegisterArea, 1).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.PlannedMaxMhHeat >= this.PlannedMaxMhCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedMaxMhHeat, 1) > HithermCompactProduct.ConfigMaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhHeat, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", HithermCompactProduct.ConfigMaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedMaxMhCool, 1) > HithermCompactProduct.ConfigMaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhCool, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", HithermCompactProduct.ConfigMaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > Math.Round(HithermCompactProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoHeat, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(HithermCompactProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > Math.Round(HithermCompactProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoCool, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(HithermCompactProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.hithermCompactType == Product.ProductType.FBH && this.PlannedRegisterArea > this.PlannedFloorArea) {
				newMsg = EuroplanRes.ErrorMessage_Registerflaeche2;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedRegisterArea, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.PlannedFloorArea, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			} else if (this.hithermCompactType == Product.ProductType.DH && this.PlannedRegisterArea > this.PlannedCeilingArea) {
				newMsg = EuroplanRes.ErrorMessage_Registerflaeche2;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedRegisterArea, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.PlannedCeilingArea, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			}
			if (this.lastErrorMsg.Length == 0) {
				this.lastErrorMsg = null;
			}

			return true;
		}

		public override float PlannedFloorArea {
			get {
				if (this.hithermCompactType == ProductType.FBH) {
					return this.plannedFloorArea;
				}
				return 0;
			}
			set {
				if (this.hithermCompactType == ProductType.FBH) {
					this.plannedFloorArea = value;
				}
			}
		}

		public override float PlannedWallArea {
			get {
				if (this.hithermCompactType == ProductType.WH) {
					return this.PlannedNetArea;
				}
				return 0;
			}
			set { }
		}

		public override float PlannedCeilingArea {
			get {
				if (this.hithermCompactType == ProductType.DH) {
					return this.plannedCeilingArea;
				}
				return 0;
			}
			set {
				if (this.hithermCompactType == ProductType.DH) {
					this.plannedCeilingArea = value;
				}
			}
		}

		public override float PlannedRoofArea {
			get {
				if (this.hithermCompactType == ProductType.DSH) {
					return this.plannedRoofArea;
				}
				return 0;
			}
			set {
				if (this.hithermCompactType == ProductType.DSH) {
					this.plannedRoofArea = value;
				}
			}
		}

		// Not to be used in code! This property is only intended to be used for (de)serializing
		public float PlannedFloorCeilingRoofArea {
			get {
				if (this.hithermCompactType == ProductType.DH) {
					return this.plannedCeilingArea;
				}
				if (this.hithermCompactType == ProductType.FBH) {
					return this.plannedFloorArea;
				}
				if (this.hithermCompactType == ProductType.DSH) {
					return this.plannedRoofArea;
				}
				return 0;
			}
			set {
				this.plannedFloorCeilingRoofArea = value;
			}
		}

		public override double PlannedCoolLoad {
			get {
				if (this.incompleteCalculation || this.requestedCoolLoad == 0) {
					return 0;
				}
				double value = 0;
				foreach (HithermCompactCircuit c in this.circuits) {
					if (!c.QFbhTotalCool.Equals(double.NaN)) {
						value += c.QFbhTotalCool;
					}
				}
				return value;
			}
		}

		public override double PlannedHeatLoad {
			get {
				if (this.incompleteCalculation || this.requestedHeatLoad == 0) {
					return 0;
				}
				double value = 0;
				foreach (HithermCompactCircuit c in this.circuits) {
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
				foreach (HithermCompactCircuit hc in this.circuits) {
					area += hc.CoveredArea;
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
			foreach (HithermCompactCircuit c in this.circuits) {
				foreach (HithermCompactRegister register in c.Registers) {
					teilflaechen++;
					registerCount += register.RegisterCount;

					// Register
					if (register.PartNumber != "") {
						Project.Instance.AddRequiredMaterial(requiredMaterial, register.PartNumber, register.RegisterCount);
					}
#if DEBUG
					else {
						MessageBox.Show("Hitherm Compact Product not found.");
					}
#endif
					//Wandwinkel
					if (ConfigUsePlus) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HR66", 1);
					} else {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HI66", 1);
					}

					// Bodenwinkel
					if (ConfigUsePlus) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HR69", 2);
					} else {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HI68", 2);
					}

					verbindeLength += register.PipeHorizontal + register.PipeVertical;

				}

			}
			
			// Ovalmuffen
			double amount = teilflaechen;
			if (verbindeLength > 0) {
				amount += verbindeLength / 2;
			}
			if (ConfigUsePlus) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR55", amount);
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", amount);
			}

			// Ovalrohr
			if (ConfigUsePlus) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR60", verbindeLength);
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI60", verbindeLength);
			}

			// Kleber
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HC41", PlannedNetArea / 15);
			
			// Schrauben
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HC42", PlannedNetArea * 13);

		}

		public static void ReviseRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {
			if (requiredMaterial.ContainsKey("HC42")) {
				double amount = requiredMaterial["HC42"];
				if (amount > 1000) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HC42", (-1) * (amount - (amount % 1000)));
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HC43", amount - (amount % 1000));
				}
			}
		} 

		public HithermCompactCircuit GetCircuitForRegister(HithermCompactRegister register) {
			if (!this.registerCircuits.ContainsKey(register)) {
				return null;
			}
			if (!this.circuitIds.ContainsKey(this.registerCircuits[register])) {
				return null;
			}
			return this.circuitIds[this.registerCircuits[register]];
		}

		internal void AddRegisterToCircuit(HithermCompactRegister register, int circuitId) {
			this.registerCircuits[register] = circuitId;
			if (!this.circuitIds.ContainsKey(circuitId)) {
				HithermCompactCircuit hc = new HithermCompactCircuit();
				hc.HithermCompactProduct = this;
				this.circuits.Add(hc);
				this.circuitIds[circuitId] = hc;
			}
			this.circuitIds[circuitId].Registers.Add(register);
		}

		internal void MoveRegisterToCircuit(HithermCompactRegister register, int circuitId) {
			if (this.registerCircuits.ContainsKey(register)) {
				HithermCompactCircuit hc = this.circuitIds[this.registerCircuits[register]];
				hc.Registers.Remove(register);
				if (hc.Registers.Count == 0) {
					this.circuits.Remove(hc);
					this.circuitIds.Remove(this.registerCircuits[register]);
				}
				this.registerCircuits[register] = circuitId;
				if (!this.circuitIds.ContainsKey(circuitId)) {
					hc = new HithermCompactCircuit();
					this.circuits.Add(hc);
					this.circuitIds[circuitId] = hc;
				}
				this.circuitIds[circuitId].Registers.Add(register);
			}
		}

		internal void RemoveRegisterFromCircuit(HithermCompactRegister register) {
			if (this.registerCircuits.ContainsKey(register)) {
				HithermCompactCircuit hc = this.circuitIds[this.registerCircuits[register]];
				hc.Registers.Remove(register);
				if (hc.Registers.Count == 0) {
					this.circuits.Remove(hc);
					this.circuitIds.Remove(this.registerCircuits[register]);
				}
				this.registerCircuits.Remove(register);
			}
		}

		internal int GetRegisterCircuitId(HithermCompactRegister register) {
			if (this.registerCircuits.ContainsKey(register)) {
				return this.registerCircuits[register];
			}
			return 0;
		}

		internal override void FinalizeLoading(PlannedProduct pp) {
			base.FinalizeLoading(pp);
			switch (this.hithermCompactType) {
				case ProductType.FBH:
					this.plannedFloorArea = this.plannedFloorCeilingRoofArea;
					this.plannedFloorCeilingRoofArea = 0;
					this.plannedCeilingArea = 0;
					break;

				case ProductType.DH:
					this.plannedCeilingArea = this.plannedFloorCeilingRoofArea;
					this.plannedFloorCeilingRoofArea = 0;
					this.plannedFloorArea = 0;
					break;

				default:
					this.plannedCeilingArea = 0;
					this.plannedFloorCeilingRoofArea = 0;
					this.plannedFloorArea = 0;
					break;
			}
			if (pp != null) {
				int i = 1;
				foreach (HithermCompactCircuit hc in this.circuits) {
					this.circuitIds[i] = hc;
					foreach (HithermCompactRegister hr in hc.Registers) {
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
				foreach (HithermCompactCircuit hc in this.circuits) {
					bereinigung += hc.HeizleistungBereinigung;
				}
				return bereinigung;
			}
		}

		[XmlIgnore]
		public override double PlannedKuehllastBereinigung {
			get {
				double bereinigung = 0;
				foreach (HithermCompactCircuit hc in this.circuits) {
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
				if (this.hithermCompactType != ProductType.FBH) {
					return 0;
				}
				return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AssociatedRoom.Area);
			}
			set {
				if (this.hithermCompactType == ProductType.FBH) {
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
				if (this.hithermCompactType != ProductType.DH) {
					return 0;
				}
				return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedCeilingArea * 100 / this.AssociatedRoom.Area);
			}
			set {
				if (this.hithermCompactType == ProductType.DH) {
					this.PlannedCeilingArea = (float)(this.AssociatedRoom.Area * value / 100);
				}
			}
		}

		public double PlannedRegisterArea {
			get {
				double area = 0;
				foreach (HithermCompactCircuit hcc in this.PlannedCircuits) {
					area += hcc.CoveredArea;
				}
				return area;
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
				foreach (HithermCompactCircuit c in this.circuits) {
					foreach (HithermCompactRegister register in c.Registers) {
						wasserInhalt += register.WasserInhalt * register.RegisterCount;
					}
				}

				return wasserInhalt + (pipeEurovalLength * EurovalProduct.rohrInnenA * 1000) + (pipe21mmLength * Product.rundrohr21mmInnenA * 1000);
			}
		}

		public override double Dichte {
			get { return HithermCompactProduct.ConfigRho; }
		}

		public override double Waermekapazitaet {
			get { return HithermCompactProduct.ConfigC; }
		}

		public override double Viskositaet {
			get { return HithermCompactProduct.ConfigV; }
		}
	}
	
}
