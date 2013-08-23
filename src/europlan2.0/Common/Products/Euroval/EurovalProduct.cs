using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using log4net;
using System.Threading;
using Europlan.Licensing;
using WW.Math.Geometry;
using WW.Math;
using System.Drawing;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Product_EurovalName", "Product_EurovalFullName")]
	public class EurovalProduct : Product, ProductWithInsulationConstruction, IPipeProduct<EurovalProduct.EurovalLayDistance, EurovalProduct.EurovalRimType> {

		private static readonly ILog log = LogManager.GetLogger(typeof(EurovalProduct));

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 50;
		private static int quickDimensioningCoolPowerPerSquareMeter = 50;
		private static bool canHeat = true;
		private static bool canCool = false;

		// planning
		private static double su0 = 0.045; /* Mindestüberdeckung fix */
		private static double alpha0 = 10.8; /* Fixwert für FBH fix */
		private static double lambdaR0 = 0.35; /* fix */
		private static double lambdaR = 0.22; /* für PP Rohr laut Tabelle A.13 fix */
		private static double lambdaU0 = 1; /* fix */
		private static double lambdaE = 1.2; /* Estrichleitfähigkeit, fix */
		private static double su = 0.035; /* Estrichüberdeckung; Annahme ECO30;  fix*/
		private static double lambdaU = 1.2; /* Wärmeleitfähigkeit der Überdeckung */
		private static double rohrAussenD = 0.0206505; /* Aussendurchmesser Euroval Rohr */
		private static double rohrInnenD = 0.0153; /* Rohrinnendurchmesser */
		public static double rohrInnenA = 0.000183783; /* Rohrinnenquerschnitt */
		private static double ag = 1.2125; /* Ovalrohr Geometriefaktor für Euroval */
		private static double sr0 = 0.002; /* fix ??? */
		private static double sr = 0.00238; /* Aus Euroval Normprüfdaten */
		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static double v = 0.00000101; /* m²/s ... kinematische Viskosität */
		private static bool agActivated = true;
		private static double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
		private static double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */

		private static double faktorTrockenkonstruktion = 0.45;

		private static double maxResidenceTempHarreither = 27;
		private static double maxRimTempHarreither = 33;
		private static double maxResidenceTempEn1264 = 29;
		private static double maxRimTempEn1264 = 35;
		private static double maxNassraumTemp = 33;

		//  !!!!!!!!!!! changes must be also applied in SystemParametersPanel.cs !!!!!!!!!!!
		private static bool useHarreitherNorm = true;
		private static double maxCircuitLength = 100.0;
		private static int maxPressureLost = 15000;
		private static int maxDurchfluss = 240;
		private static double spreizungHeizMin = 4;
		private static double spreizungHeizMax = 12;
		private static double spreizungKuehlMin = 2;
		private static double spreizungKuehlMax = 5;

		protected float plannedArea = 0;
		private float plannedAreaReduced = 0;
		private float plannedAreaUnheated = 0;
		private float plannedRimLength = 0;
		private int plannedRimCorners = 0;
		private Construction plannedFloorConstruction = null;
		private Construction plannedInsulationConstruction = null;
		private string plannedFloorConstructionId = null;
		private string plannedInsulationConstructionId = null;

		private Nullable<EurovalLayDistance> requestedLayDistance = null;
		private Nullable<EurovalRimType> requestedRimType = null;
		private Nullable<int> requestedCircuits = null;

		private Nullable<EurovalLayDistance> plannedLayDistance = null;
		private Nullable<EurovalRimType> plannedRimType = null;

		private List<ExtendedCorrections> plannedCorrectionList = new List<ExtendedCorrections>();
		private List<Segment2D> plannedRimSegments = new List<Segment2D>();
		private List<Point2D> plannedAreaGraphical = new List<Point2D>();
		private List<List<Point2D>> plannedReducedAreas = new List<List<Point2D>>();
		private Point2D textBoxPosition = Point2D.Zero;
		private Nullable<float> textBoxFontSize = null;
        private float textBoxRotation = 0;

		private bool clipSchieneKlebeband = false;
		private bool anhydritEstrich = false;

        private Color productColor = Color.Red;

		public class LayDistanceConverter : System.ComponentModel.TypeConverter {
			private static readonly string A5 = EuroplanRes.EurovalProduct_A5; //"A5"
			private static readonly string EV5 = EuroplanRes.EurovalProduct_EV5; //"EV5"
			private static readonly string EV10 = EuroplanRes.EurovalProduct_EV10; //"EV10"
			private static readonly string EV15 = EuroplanRes.EurovalProduct_EV15; //"EV15"
			private static readonly string EV20 = EuroplanRes.EurovalProduct_EV20; //"EV20"
			private static readonly string EV25 = EuroplanRes.EurovalProduct_EV25; //"EV25"
			private static readonly string EV30 = EuroplanRes.EurovalProduct_EV30; //"EV30"
			private static readonly string EV35 = EuroplanRes.EurovalProduct_EV35; //"EV35"

			private Dictionary<string, EurovalLayDistance> mappingFromString = new Dictionary<string, EurovalLayDistance>();
			private Dictionary<EurovalLayDistance, string> mappingToString = new Dictionary<EurovalLayDistance, string>();

			public LayDistanceConverter() {
				mappingFromString.Add(A5, EurovalLayDistance.A5);
				mappingFromString.Add(EV5, EurovalLayDistance.EV5);
				mappingFromString.Add(EV10, EurovalLayDistance.EV10);
				mappingFromString.Add(EV15, EurovalLayDistance.EV15);
				mappingFromString.Add(EV20, EurovalLayDistance.EV20);
				mappingFromString.Add(EV25, EurovalLayDistance.EV25);
				mappingFromString.Add(EV30, EurovalLayDistance.EV30);
				mappingFromString.Add(EV35, EurovalLayDistance.EV35);
				mappingToString.Add(EurovalLayDistance.A5, A5);
				mappingToString.Add(EurovalLayDistance.EV5, EV5);
				mappingToString.Add(EurovalLayDistance.EV10, EV10);
				mappingToString.Add(EurovalLayDistance.EV15, EV15);
				mappingToString.Add(EurovalLayDistance.EV20, EV20);
				mappingToString.Add(EurovalLayDistance.EV25, EV25);
				mappingToString.Add(EurovalLayDistance.EV30, EV30);
				mappingToString.Add(EurovalLayDistance.EV35, EV35);
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
				if (value is EurovalLayDistance && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((EurovalLayDistance)value)) {
						return mappingToString[(EurovalLayDistance)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		public class RimTypeConverter : System.ComponentModel.TypeConverter {
			private static readonly string EV5_40 = EuroplanRes.EurovalProduct_EV5_40;
			private static readonly string EV5_80 = EuroplanRes.EurovalProduct_EV5_80;
			private static readonly string EV5_120 = EuroplanRes.EurovalProduct_EV5_120;
			private static readonly string EV10_55 = EuroplanRes.EurovalProduct_EV10_55;
			private static readonly string EV10_110 = EuroplanRes.EurovalProduct_EV10_110;
			private static readonly string EV10_165 = EuroplanRes.EurovalProduct_EV10_165;
			private static readonly string EV15_60 = EuroplanRes.EurovalProduct_EV15_60;
			private static readonly string EV15_120 = EuroplanRes.EurovalProduct_EV15_120;
			private static readonly string EV15_180 = EuroplanRes.EurovalProduct_EV15_180;

			private Dictionary<string, EurovalRimType> mappingFromString = new Dictionary<string, EurovalRimType>();
			private Dictionary<EurovalRimType, string> mappingToString = new Dictionary<EurovalRimType, string>();

			public RimTypeConverter() {
				mappingFromString.Add(EV5_40, EurovalRimType.EV5_40);
				mappingFromString.Add(EV5_80, EurovalRimType.EV5_80);
				mappingFromString.Add(EV5_120, EurovalRimType.EV5_120);
				mappingFromString.Add(EV10_55, EurovalRimType.EV10_55);
				mappingFromString.Add(EV10_110, EurovalRimType.EV10_110);
				mappingFromString.Add(EV10_165, EurovalRimType.EV10_165);
				mappingFromString.Add(EV15_60, EurovalRimType.EV15_60);
				mappingFromString.Add(EV15_120, EurovalRimType.EV15_120);
				mappingFromString.Add(EV15_180, EurovalRimType.EV15_180);
				mappingToString.Add(EurovalRimType.EV5_40, EV5_40);
				mappingToString.Add(EurovalRimType.EV5_80, EV5_80);
				mappingToString.Add(EurovalRimType.EV5_120, EV5_120);
				mappingToString.Add(EurovalRimType.EV10_55, EV10_55);
				mappingToString.Add(EurovalRimType.EV10_110, EV10_110);
				mappingToString.Add(EurovalRimType.EV10_165, EV10_165);
				mappingToString.Add(EurovalRimType.EV15_60, EV15_60);
				mappingToString.Add(EurovalRimType.EV15_120, EV15_120);
				mappingToString.Add(EurovalRimType.EV15_180, EV15_180);
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
				if (value is EurovalRimType && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((EurovalRimType)value)) {
						return mappingToString[(EurovalRimType)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(LayDistanceConverter))]
		public enum EurovalLayDistance {
			A5 = 0,
			EV5 = 1,
			EV10 = 2,
			EV15 = 3,
			EV20 = 4,
			EV25 = 5,
			EV30 = 6,
			EV35 = 7,
			NONE = -1
		}

		[System.ComponentModel.TypeConverter(typeof(RimTypeConverter))]
		public enum EurovalRimType {
			EV15_60,
			EV15_120,
			EV15_180,
			EV10_55,
			EV10_110,
			EV10_165,
			EV5_40,
			EV5_80,
			EV5_120
		}

		public EurovalProduct(){
			if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEuroval)) {
				throw new ProductNotLicensedException(this.GetType());
			}
		}

		protected EurovalProduct(EurovalProduct product) : base(product) {
		}

		public override void ClearGraphicalRepresentation() {
			base.ClearGraphicalRepresentation();
			plannedRimSegments = new List<Segment2D>();
			plannedAreaGraphical = new List<Point2D>();
			plannedReducedAreas = new List<List<Point2D>>();
			textBoxPosition = Point2D.Zero;
			textBoxFontSize = 5.0f;
		}

		public override void Initialize() {
		}

		public override Product.CalculateModeEnum DefaultCalculateMode {
			get { return CalculateModeEnum.HEAT; }
		}

		public override string ImageKey {
            get { return "Fußbodenheizung.png"; }
		}

		public override string SelectedImageKey {
            get { return "Fußbodenheizung.png"; }
		}

		[XmlIgnore]
		public override bool AllowToSwitchMode {
			get {
				return (PlannedFloorArea == 0 || PlannedFloorArea == this.associatedRoom.Area) &&
						PlannedAreaReduced == 0 &&
						PlannedAreaUnheated == 0 &&
						PlannedRimLength == 0 &&
						PlannedRimCorners == 0 &&
						RequestedLayDistance == null &&
						RequestedRimType == null &&
						RequestedCircuits == null;
			}
		}

		public new static void StaticInitialize(Configuration config) {
			Product.StaticInitialize<EurovalProduct>(config);
		}

		public void ResetProduct() {
			PlannedFloorArea = 0;
			PlannedAreaReduced = 0;
			PlannedAreaUnheated = 0;
			PlannedRimLength = 0;
			plannedRimCorners = 0;
			RequestedLayDistance = null;
			RequestedRimType = null;
			RequestedCircuits = null;
		}

		public static string GlobalNotificationMessage {
			get {
				string message = null;
				Configuration userConfig = Configuration.UserTemplate;

				double defaultAg = userConfig.GetProductParameterAsBool<EurovalProduct>("ConfigAgActivated") ? userConfig.GetProductParameterAsDouble<EurovalProduct>("ConfigAg") : 1.0;
				double actualAg = agActivated ? ag : 1.0;
				if (actualAg != defaultAg) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_Geometriefaktor;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(actualAg, 4).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultAg, 4).ToString());
					message += newMsg;
				}

				double defaultSu0 = userConfig.GetProductParameterAsDouble<EurovalProduct>("ConfigSu0");
				if (su0 != defaultSu0) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_Mindestueberdeckung;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(su0, 3).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultSu0, 3).ToString());
					message += newMsg;
				}

				double defaultSu = userConfig.GetProductParameterAsDouble<EurovalProduct>("ConfigSu");
				if (su != defaultSu) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_Estrichueberdeckung;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(su, 3).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultSu, 3).ToString());
					message += newMsg;
				}

				double defaultC = userConfig.GetProductParameterAsDouble<EurovalProduct>("ConfigC");
				if (c != defaultC) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_Waermekapazitaet;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(c, 2).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultC, 2).ToString());
					message += newMsg;
				}

				double defaultRho = userConfig.GetProductParameterAsDouble<EurovalProduct>("ConfigRho");
				if (rho != defaultRho) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_Dichte;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(rho, 2).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultRho, 2).ToString());
					message += newMsg;
				}

				double defaultV = userConfig.GetProductParameterAsDouble<EurovalProduct>("ConfigV");
				if (v != defaultV) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_Viskositaet;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(v, 10).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultV, 10).ToString());
					message += newMsg;
				}

				if (message != null) {
					message = EuroplanRes.EurovalProduct_NotificationParameter + /*"Ecotherm-Systeme werden mit veränderten Paramtern berechnet. Folgende Parameter weichen von den Standardwerten ab:\n" */
						"\n" + message;
				}

				return message;
			}
		}

		public override Product Clone(Room room) {
			EurovalProduct product = new EurovalProduct(this);
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

		[DoubleProductParameter(0.35)]
		public static double ConfigLambdaR0 {
			get { return lambdaR0; }
			set { lambdaR0 = value; }
		}

		[DoubleProductParameter(0.22)]
		public static double ConfigLambdaR {
			get { return lambdaR; }
			set { lambdaR = value; }
		}

		[DoubleProductParameter(1)]
		public static double ConfigLambdaU0 {
			get { return lambdaU0; }
			set { lambdaU0 = value; }
		}

		[DoubleProductParameter(1.2)]
		public static double ConfigLambdaE {
			get { return lambdaE; }
			set { lambdaE = value; }
		}

		[DoubleProductParameter(0.035)]
		public static double ConfigSu {
			get { return su; }
			set { su = value; }
		}

		[DoubleProductParameter(1.2)]
		public static double ConfigLambdaU {
			get { return lambdaU; }
			set { lambdaU = value; }
		}

		[DoubleProductParameter(0.0206505)]
		public static double ConfigRohrAussenD {
			get { return rohrAussenD; }
			set { rohrAussenD = value; }
		}

		[DoubleProductParameter(0.0153)]
		public static double ConfigRohrInnenD {
			get { return rohrInnenD; }
			set { rohrInnenD = value; }
		}

		[DoubleProductParameter(0.000183783)]
		public static double ConfigRohrInnenA {
			get { return rohrInnenA; }
			set { rohrInnenA = value; }
		}

		[DoubleProductParameter(1.2125)]
		public static double ConfigAg {
			get { return ag; }
			set { ag = value; }
		}

		[DoubleProductParameter(0.002)]
		public static double ConfigSr0 {
			get { return sr0; }
			set { sr0 = value; }
		}

		[DoubleProductParameter(0.00238)]
		public static double ConfigSr {
			get { return sr; }
			set { sr = value; }
		}

		[DoubleProductParameter(4.19)]
		public static double ConfigC {
			get { return c; }
			set { c = value; }
		}

		[DoubleProductParameter(1000)]
		public static double ConfigRho {
			get { return rho; }
			set { rho = value; }
		}

		[DoubleProductParameter(0.00000101)]
		public static double ConfigV {
			get { return v; }
			set { v = value; }
		}

		[BoolProductParameter(true)]
		public static bool ConfigAgActivated {
			get { return agActivated; }
			set { agActivated = value; }
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

		[DoubleProductParameter(0.45)]
		public static double ConfigFaktorTrockenkonstruktion {
			get { return faktorTrockenkonstruktion; }
			set { faktorTrockenkonstruktion = value; }
		}

		[DoubleProductParameter(27)]
		public static double ConfigMaxResidenceTempHarreither {
			get { return maxResidenceTempHarreither; }
			set { maxResidenceTempHarreither = value; }
		}

		[DoubleProductParameter(33)]
		public static double ConfigMaxRimTempHarreither {
			get { return maxRimTempHarreither; }
			set { maxRimTempHarreither = value; }
		}

		[DoubleProductParameter(29)]
		public static double ConfigMaxResidenceTempEn1264 {
			get { return maxResidenceTempEn1264; }
			set { maxResidenceTempEn1264 = value; }
		}

		[DoubleProductParameter(35)]
		public static double ConfigMaxRimTempEn1264 {
			get { return maxRimTempEn1264; }
			set { maxRimTempEn1264 = value; }
		}

		[DoubleProductParameter(33)]
		public static double ConfigMaxNassraumTemp {
			get { return maxNassraumTemp; }
			set { maxNassraumTemp = value; }
		}

		[BoolProductParameter(true)]
		public static bool ConfigUseHarreitherNorm {
			get { return useHarreitherNorm; }
			set { useHarreitherNorm = value; }
		}

		[DoubleProductParameter(100)]
		public static double ConfigMaxCircuitLength {
			get { return maxCircuitLength; }
			set { maxCircuitLength = value; }
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
		#endregion Product Parameters

        public double MaxDurchfluss {
            get {
                if (this.PlannedConnection == null || this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.NONE) {
                    return EurovalProduct.ConfigMaxDurchfluss;
                }
                Product p = this;
                while (p != null && p.PlannedConnection != null && p.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT && p.PlannedConnection.OtherProduct != null) {
                    p = p.PlannedConnection.OtherProduct.Product;
                }
                if (p != null && p.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR && p.PlannedConnection.Distributor != null) {
                    return p.PlannedConnection.Distributor.MaxDurchfluss;
                }
                return EurovalProduct.ConfigMaxDurchfluss;
            }
        }
        public double MaxMassenstrom {
            get { return MaxDurchfluss * EurovalProduct.ConfigRho / 1000; }
        }

		/// <summary>
		/// Returns the default number of circuit for the planned area (for quick dimensioning)
		/// </summary>
		public override int GetDefaultQuickDimensioningCircuits() {
			EurovalLayDistance distance = Project.Instance.QuickDimensioning.EurovalLayDistance;
			return (int)Math.Ceiling(quickDimensioningPlannedArea / (100/GetPipeLengthPerSqm(distance)));
		}

		/// <summary>
		/// Gets the default area that should be planned for this product in quick dimensioning
		/// </summary>
		public override float GetDefaultQuickDimensioningPlannedArea() {
			if (this.AssociatedRoom != null) {
				return this.AssociatedRoom.Area;
			}
			return 0;
		}

		/// <summary>
		/// Returns the pipe length in m per m² for the specified laydistance 
		/// </summary>
		public static double GetPipeLengthPerSqm(EurovalLayDistance distance) {
			switch (distance) {
				case EurovalLayDistance.A5:
					return 20;
				case EurovalLayDistance.EV5:
					return 10;
				case EurovalLayDistance.EV10:
					return 7.5;
				case EurovalLayDistance.EV15:
					return 6.7;
				case EurovalLayDistance.EV20:
					return 5;
				case EurovalLayDistance.EV25:
					return 4;
				case EurovalLayDistance.EV30:
					return 3.5;
				case EurovalLayDistance.EV35:
					return 3;
				case EurovalLayDistance.NONE:
					return 0;
				default:
					throw new Exception("Unknown Laydistance");
			}
		}

		/// <summary>
		/// Returns the number of clipschiene in m per m² for the specified laydistance and estrich
		/// </summary>
		public static double GetClipschienePerSqm(EurovalLayDistance distance, bool anhydritEstrich) {
			if (anhydritEstrich) {
				return 2;
			} else {
				switch (distance) {
					case EurovalLayDistance.A5:
						return 2;
					case EurovalLayDistance.EV5:
						return 1.8;
					case EurovalLayDistance.EV10:
						return 1.6;
					case EurovalLayDistance.EV15:
						return 1.5;
					case EurovalLayDistance.EV20:
						return 1.4;
					case EurovalLayDistance.EV25:
						return 1.3;
					case EurovalLayDistance.EV30:
						return 1.2;
					case EurovalLayDistance.EV35:
						return 1.2;
					case EurovalLayDistance.NONE:
						return 0;
					default:
						throw new Exception("Unknown Laydistance");
				}
			}
		}

		public static double GetOvalmuffePerSqm(EurovalLayDistance layDistance) {
			switch (layDistance) {
				case EurovalLayDistance.A5:
					return 0.1;
				case EurovalLayDistance.EV5:
					return 0.07;
				case EurovalLayDistance.EV10:
					return 0.06;
				case EurovalLayDistance.EV15:
					return 0.05;
				case EurovalLayDistance.EV20:
					return 0.04;
				case EurovalLayDistance.EV25:
					return 0.03;
				case EurovalLayDistance.EV30:
					return 0.02;
				case EurovalLayDistance.EV35:
					return 0.02;
				case EurovalLayDistance.NONE:
					return 0;
				default:
					throw new Exception("Unknown Laydistance");
			}
		}

		/// <summary>
		/// Returns distance between two pipes in m for specified laydistance 
		/// </summary>
		public static double GetTeilung(EurovalLayDistance distance) {
			switch (distance) {
				case EurovalLayDistance.A5:
					return 0.05;
				case EurovalLayDistance.EV5:
					return 0.1;
				case EurovalLayDistance.EV10:
					return 0.125;
				case EurovalLayDistance.EV15:
					return 0.15;
				case EurovalLayDistance.EV20:
					return 0.2;
				case EurovalLayDistance.EV25:
					return 0.25;
				case EurovalLayDistance.EV30:
					return 0.3;
				case EurovalLayDistance.EV35:
					return 0.35;
				case EurovalLayDistance.NONE:
					return double.MaxValue;
				default:
					throw new Exception("Unknwon LayDistance");
			}
		}

		/// <summary>
		/// Returns the laydistance for the specified rimtype
		/// </summary>
		public static EurovalLayDistance GetRimLayDistance(EurovalRimType rimType) {
			switch (rimType) {
				case EurovalRimType.EV15_60:
				case EurovalRimType.EV15_120:
				case EurovalRimType.EV15_180:
					return EurovalLayDistance.EV15;
				case EurovalRimType.EV10_55:
				case EurovalRimType.EV10_110:
				case EurovalRimType.EV10_165:
					return EurovalLayDistance.EV10;
				case EurovalRimType.EV5_40:
				case EurovalRimType.EV5_80:
				case EurovalRimType.EV5_120:
					return EurovalLayDistance.EV5;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		/// <summary>
		/// Returns the width of the rim in cm for the specified rimtype
		/// </summary>
		public static int GetRimWidth(EurovalRimType rimType) {
			switch (rimType) {
				case EurovalRimType.EV5_40:
					return 40;
				case EurovalRimType.EV10_55:
					return 55;
				case EurovalRimType.EV15_60:
					return 60;
				case EurovalRimType.EV5_80:
					return 80;
				case EurovalRimType.EV10_110:
					return 110;
				case EurovalRimType.EV15_120:
				case EurovalRimType.EV5_120:
					return 120;
				case EurovalRimType.EV10_165:
					return 165;
				case EurovalRimType.EV15_180:
					return 180;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		public bool UseClipSchieneKlebeband {
			get { return clipSchieneKlebeband; }
			set { clipSchieneKlebeband = value; }
		}

		public bool UseAnhydritEstrich {
			get { return anhydritEstrich; }
			set { anhydritEstrich = value; }
		}

		#region QuickDimensioning
		/// <summary>
		/// The maximum area that this product can use in quick dimensioning
		/// </summary>
		public override float QuickDimensioningMaximumArea {
			get {
				if (this.AssociatedRoom != null) {
					return this.AssociatedRoom.Area;
				}
				return 0;
			}
		}

		/// <summary>
		/// The name of this product that is shown in quick dimensioning
		/// </summary>
		public override string QuickDimensioningName {
			get { return QuickDimensioningNameStatic; }
		}

		public static string QuickDimensioningNameStatic {
			get { return "Euroval®\n(m²)"; }
		}
		#endregion QuickDimensioning

		/// <summary>
		/// The type of this product
		/// </summary>
		public override ProductType Type {
			get { return ProductType.FBH; }
		}

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_EUROVAL; }
		}

		#region Auslegung
		public override float PlannedFloorArea {
			get { return this.plannedArea; }
			set { this.plannedArea = value; }
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

		public override float PlannedNetArea {
			get { return this.PlannedFloorArea - this.PlannedAreaReduced / 2 - this.PlannedAreaUnheated; }
		}

		/// <summary>
		/// The lay distance that the user requested for this product in the planning.
		/// If this is property is null the optimal lay distance will be calculated.
		/// </summary>
		public Nullable<EurovalLayDistance> RequestedLayDistance {
			get { return this.requestedLayDistance; }
			set { this.requestedLayDistance = value; }
		}

		/// <summary>
		/// The rim type the user requested for this product in the planning.
		/// If this property is null the optimal rim type will be calculated.
		/// </summary>
		public Nullable<EurovalRimType> RequestedRimType {
			get { return this.requestedRimType; }
			set { this.requestedRimType = value; }
		}

		/// <summary>
		/// The number of circuits the user requested for this product in the planning.
		/// If this property is null the optimal number of circuits will be calculated.
		/// </summary>
		public Nullable<int> RequestedCircuits {
			get { return this.requestedCircuits; }
			set {
				if ((this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) ||
					this.PlannedCorrections) {
					this.requestedCircuits = value.HasValue ? value.Value : 1;
					if (PlannedCorrections) {
						while (this.plannedCorrectionList.Count < this.requestedCircuits.Value) {
							this.plannedCorrectionList.Add(new ExtendedCorrections(this.plannedCorrectionList.Count + 1, this));
						}
						if (this.plannedCorrectionList.Count > this.requestedCircuits.Value) {
							this.plannedCorrectionList.RemoveRange(this.requestedCircuits.Value, this.plannedCorrectionList.Count - this.requestedCircuits.Value);
						}
					}
				} else {
					this.requestedCircuits = value;
				}
			}
		}

		/// <summary>
		/// The length of the rim the user planned.
		/// </summary>
		public float PlannedRimLength {
			get { return this.plannedRimLength; }
			set { this.plannedRimLength = value; }
		}

		/// <summary>
		/// The rim segments planned in the graphical mode
		/// </summary>
		public List<Segment2D> PlannedRimSegments {
			get { return this.plannedRimSegments; }
			set { this.plannedRimSegments = value; }
		}

		/// <summary>
		/// The graphical representation of the area
		/// </summary>
		public List<Point2D> PlannedAreaGraphical {
			get { return this.plannedAreaGraphical; }
			set { this.plannedAreaGraphical = value; }
		}

		/// <summary>
		/// The graphical representation of the areas 
		/// </summary>
		public List<List<Point2D>> PlannedReducedAreas {
			get { return this.plannedReducedAreas; }
			set { this.plannedReducedAreas = value; }
		}

		/// <summary>
		/// The number of corners in the rim the user planned.
		/// </summary>
		public int PlannedRimCorners {
			get { return this.plannedRimCorners; }
			set { this.plannedRimCorners = value; }
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

		/// <summary>
		/// The area which is planned reduced (50%).
		/// Half of this area is subtracted from the planned area for calculation.
		/// </summary>
		public float PlannedAreaReduced {
			get { return this.plannedAreaReduced; }
			set { this.plannedAreaReduced = value; }
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
		/// The area that is occupied by the planned rim
		/// </summary>
		[XmlIgnore]
		public float PlannedAreaRim {
			get {
				if (this.plannedRimType.HasValue) {
					float rimWidth = ((float)GetRimWidth(this.plannedRimType.Value)) / 100.0f;
					float realRimLength = this.plannedRimLength + rimWidth * this.plannedRimCorners;
					return realRimLength * rimWidth;
				} else {
					return 0;
				}
			}
		}

		/// <summary>
		/// The area that is occupied by the residence area.
		/// This area is again divided into reduced (<seealso cref="PlannedAreaReduced">PlannedAreaReduced</seealso>), 
		/// unheated (<seealso cref="PlannedAreaUnheated">PlannedAreaUnheated</seealso>) and normal parts.
		/// </summary>
		[XmlIgnore]
		public float PlannedAreaResidence {
			get {
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					double area = (ec.AreaTotal - ec.GetAreaRim(this.plannedRimType) - ec.AreaRemovedDueConnection);
					if (area < 0) {
						area = 0;
					}
					value += area;
				}
				return (float)value;
			}
		}

        /// <summary>
        /// Actual part of the residence area that is heated/cooled.
        /// </summary>
        [XmlIgnore]
        public float PlannedAreaResidenceHeated {
            get {
                return this.PlannedAreaResidence - this.PlannedAreaUnheated;
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
		#endregion Auslegung

		#region Auslegung calculated values
		/// <summary>
		/// The lay distance used for calculation. This is either the lay distance the user requested
		/// or if the user did not request any specific lay distance the optimal lay distance is
		/// calculated.
		/// </summary>
		[XmlIgnore]
		public Nullable<EurovalLayDistance> PlannedLayDistance {
			get { return this.plannedLayDistance; }
			set { this.plannedLayDistance = value; }
		}

		/// <summary>
		/// The rim type used for calculation. This is either the rim type the user requested
		/// or if the user did not request any specific rim type the optimal rim type is
		/// calculated.
		/// </summary>
		[XmlIgnore]
		public Nullable<EurovalRimType> PlannedRimType {
			get { return this.plannedRimType; }
			set { this.plannedRimType = value; }
		}

		/// <summary>
		/// The lay distance of the rim type used for calculation.
		/// </summary>
		[XmlIgnore]
		public Nullable<EurovalLayDistance> PlannedRimLayDistance {
			get { return this.plannedRimType == null ? (Nullable<EurovalLayDistance>)null : (Nullable<EurovalLayDistance>)GetRimLayDistance(this.plannedRimType.Value); }
		}

		/// <summary>
		/// The width of the rim type used for calculation.
		/// </summary>
		[XmlIgnore]
		public int PlannedRimWidth {
			get { return this.plannedRimType == null ? 0 : GetRimWidth(this.plannedRimType.Value); }
		}

		#region Heat Load
		/// <summary>
		/// The heat load per m² that results of the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadPerSqM {
			get {
				return this.plannedArea == 0 ? 0 : this.PlannedHeatLoad / this.plannedArea;
			}
		}

		/// <summary>
		/// The heat load per m² that is emmited by the rim area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadPerSqMRim {
			get {
				double area = this.PlannedAreaRim;
				return area == 0 ? 0 :this.PlannedHeatLoadRim / area;
			}
		}

		/// <summary>
		/// The heat load per m² that is emmited by the residence area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadPerSqMResidence {
			get {
				double area = this.PlannedAreaResidenceHeated;
				return area == 0 ? 0 : this.PlannedHeatLoadResidence / area;
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
				foreach (EurovalCircuit ec in this.circuits) {
					value += ec.QFbhTotalHeat;
				}
				return value;
			}
		}

		/// <summary>
		/// The total heat load that is emmited by the rim area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadRim {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					value += ec.QRzHeat;
				}
				return value;
			}
		}

		/// <summary>
		/// The total heat load that is emmited by the residence area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadResidence {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					value += ec.QAzHeat;
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeatRim {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_FloorTempRzHeat > value) {
						value = ec.C_FloorTempRzHeat;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeatResidence {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_FloorTempAzHeat > value) {
						value = ec.C_FloorTempAzHeat;
					}
				}
				return value;
			}
		}
		#endregion Heat Load

		#region Cool Load
		/// <summary>
		/// The cool load per m² that results of the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadPerSqM {
			get { 
				return this.plannedArea == 0 ? 0 : this.PlannedCoolLoad / this.plannedArea;
			}
		}

		/// <summary>
		/// The cool load per m² that is emmited by the rim area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadPerSqMRim {
			get {
				double area = this.PlannedAreaRim;
				return area == 0 ? 0 : this.PlannedCoolLoadRim / area;
			}
		}

		/// <summary>
		/// The cool load per m² that is emmited by the residence area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadPerSqMResidence {
			get {
				double area = this.PlannedAreaResidenceHeated;
				return area == 0 ? 0 : this.PlannedCoolLoadResidence / area;
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
				foreach (EurovalCircuit ec in this.circuits) {
					value += ec.QFbhTotalCool;
				}
				return value;
			}
		}

		/// <summary>
		/// The total cool load that is emmited by the rim area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadRim {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					value += ec.QRzCool;
				}
				return value;
			}
		}

		/// <summary>
		/// The total cool load that is emmited by the residence area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadResidence {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					value += ec.QAzCool;
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureCoolRim {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = double.MaxValue;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_FloorTempRzCool < value) {
						value = ec.C_FloorTempRzCool;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureCoolResidence {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = double.MaxValue;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_FloorTempAzCool < value) {
						value = ec.C_FloorTempAzCool;
					}
				}
				return value;
			}
		}
		#endregion Cool Load

		[XmlIgnore]
		public double PlannedPipeLengthPerCircuit {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.PipeLengthWithoutConnections > value) {
						value = ec.PipeLengthWithoutConnections;
					}
				}
				return value;
			}
		}

        [XmlIgnore]
        public double LongestVlPerCircuit {
            get {
                if (this.incompleteCalculation) {
                    return 0;
                }
                double value = 0;
                foreach (EurovalCircuit ec in this.circuits) {
                    if (ec.PipeLengthVorlaufTotal > value) {
                        value = ec.PipeLengthVorlaufTotal;
                    }
                }
                return value;
            }
        }

        [XmlIgnore]
        public double LongestRlPerCircuit {
            get {
                if (this.incompleteCalculation) {
                    return 0;
                }
                double value = 0;
                foreach (EurovalCircuit ec in this.circuits) {
                    if (ec.PipeLengthRuecklaufTotal > value) {
                        value = ec.PipeLengthRuecklaufTotal;
                    }
                }
                return value;
            }
        }

		[XmlIgnore]
		public double LongestPipeLengthPerCircuitWithAllConnections {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.PipeLengthWithAllConnections > value) {
						value = ec.PipeLengthWithAllConnections;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PipeLengthWithoutConnectionsOfLongestPipeWithConnections {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double longest = 0;
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.PipeLengthWithAllConnections > longest) {
						longest = ec.PipeLengthWithAllConnections;
						value = ec.PipeLengthWithoutConnections;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double ConnectionLengthOfLongestPipeWithConnections {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double longest = 0;
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.PipeLengthWithAllConnections > longest) {
						longest = ec.PipeLengthWithAllConnections;
						value = ec.PipeLengthWithAllConnections - ec.PipeLengthWithoutConnections;
					}
				}
				return value;
			}
		}
		#endregion Auslegung calculated values

		private bool CheckHardParameters(double floorTempHeatRim, double floorTempHeatRes, double pressureLossHeat,
			double floorTempCoolRim, double floorTempCoolRes, double pressureLossCool, 
			double circuitLength, bool checkHeat, bool checkCool, bool ignoreResidence, bool ignoreRim, bool ignoreCircuitLength) {

			if (checkHeat && !ignoreRim && (floorTempHeatRim > this.MaxRimTemp)) {
				return false;
			}
			if (checkHeat && !ignoreResidence && (floorTempHeatRes > this.MaxResidenceTemp)) {
				return false;
			}
			if (checkHeat && (pressureLossHeat > maxPressureLost) && !ignoreCircuitLength) {
				return false;
			}
			if (circuitLength > maxCircuitLength && !ignoreCircuitLength) {
				return false;
			}
			return true;
		}

		private bool CoversLoads(double heatLoad, double coolLoad, double requestedHeatLoad, double requestedCoolLoad) {
			return heatLoad >= requestedHeatLoad && coolLoad >= requestedCoolLoad;
		}

		/// <summary>
		/// Returns true if the new parameters should be used, false if the old parameters should be used.
		/// </summary>
		private bool CompareParameters(double oldFloorTempHeatRim, double oldFloorTempHeatRes, double oldHeatLoad, double oldPressureLossHeat,
			double oldFloorTempCoolRim, double oldFloorTempCoolRes, double oldCoolLoad, double oldPressureLossCool, 
			double oldCircuitLength, double oldAreaRim, double oldAreaResidence,
			double newFloorTempHeatRim, double newFloorTempHeatRes, double newHeatLoad, double newPressureLossHeat,
			double newFloorTempCoolRim, double newFloorTempCoolRes, double newCoolLoad, double newPressureLossCool, 
			double newCircuitLength, double newAreaRim, double newAreaResidence,
			double requestedHeatLoad, double requestedCoolLoad, bool checkHeat, bool checkCool, bool ignoreResidence, bool ignoreRim, bool ignoreCircuits) {

			bool oldOk = CheckHardParameters(oldFloorTempHeatRim, oldFloorTempHeatRes, oldPressureLossHeat, 
				oldFloorTempCoolRim, oldFloorTempCoolRes, oldPressureLossHeat, 
				oldCircuitLength, checkHeat, checkCool, ignoreResidence, ignoreRim, ignoreCircuits);
			bool newOk = CheckHardParameters(newFloorTempHeatRim, newFloorTempHeatRes, newPressureLossHeat, 
				newFloorTempCoolRim, newFloorTempCoolRes, newPressureLossHeat,
				newCircuitLength, checkHeat, checkCool, ignoreResidence, ignoreRim, ignoreCircuits);
			if (oldOk != newOk) {
				return newOk;
			}
			bool oldCovers = CoversLoads(oldHeatLoad, oldCoolLoad, checkHeat ? requestedHeatLoad : 0, checkCool ? requestedCoolLoad : 0);
			bool newCovers = CoversLoads(newHeatLoad, newCoolLoad, checkHeat ? requestedHeatLoad : 0, checkCool ? requestedCoolLoad : 0);
			if (oldCovers != newCovers) {
				return newCovers;
			}
			bool oldRimWidthOk = oldAreaRim * 2 <= oldAreaResidence;
			bool newRimWidthOk = newAreaRim * 2 <= newAreaResidence;
			if (oldRimWidthOk != newRimWidthOk) {
				return newRimWidthOk;
			}
			if (oldCovers) {
				if (!oldRimWidthOk && newAreaRim != oldAreaRim) {
					return newAreaRim < oldAreaRim;
				}
				return newFloorTempHeatRes <= oldFloorTempHeatRes;
			} else {
				return newHeatLoad > oldHeatLoad;
			}
		}

		public override ProductConnection PlannedConnection {
			get { return base.PlannedConnection; }
			set {
				if (value != null && value.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					this.requestedCircuits = this.PlannedCircuitCount > 0 ? this.PlannedCircuitCount : 1;
					this.requestedLayDistance = this.plannedLayDistance.HasValue ? this.plannedLayDistance.Value : EurovalLayDistance.EV35;
					this.requestedRimType = this.plannedRimType;
				}
				this.plannedConnection = value;

			}
		}

		public override void CalculateHeatAndCoolFlow() {
			base.CalculateHeatAndCoolFlow();
			double spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			double spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			if (spreizungHeat > EurovalProduct.ConfigSpreizungHeizMax) {
				spreizungHeat = EurovalProduct.ConfigSpreizungHeizMax;
			}
			if (spreizungHeat < EurovalProduct.ConfigSpreizungHeizMin) {
				spreizungHeat = EurovalProduct.ConfigSpreizungHeizMin;
			}
			if (spreizungCool > EurovalProduct.ConfigSpreizungKuehlMax) {
				spreizungCool = EurovalProduct.ConfigSpreizungKuehlMax;
			}
			if (spreizungCool < EurovalProduct.ConfigSpreizungKuehlMin) {
				spreizungCool = EurovalProduct.ConfigSpreizungKuehlMin;
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

		bool secondConfig = false;

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung) {
			this.requestedHeatLoad = requestedHeatLoad;
			this.requestedCoolLoad = requestedCoolLoad;
			this.incompleteCalculation = false;
			if (this.PlannedFloorConstruction == null || this.PlannedInsulationConstruction == null || (this.PlannedConnection == null && !this.plannedProductIsConnection)) {
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

			if (this.plannedProductIsConnection) {
				this.plannedLayDistance = null;
				this.plannedRimType = null;
				this.circuits.Clear();
				this.lastErrorMsg = null;
				return true;
			}

            // calculate unheated area for graphical mode
            if (this.GraphicalMode.HasValue && this.GraphicalMode.Value) {
                double unheatedSum = 0;
                if (this.GraphicalArea != null && this.GraphicalArea.Count > 0 && this.AssociatedRoom != null && this.AssociatedRoom.RoomUnusedAreaCoordinates != null && this.AssociatedRoom.RoomUnusedAreaCoordinates.Count > 0 && this.AssociatedRoom.AssociatedPlan != null && this.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
                    List<Polygon2D> product = new List<Polygon2D>();
                    Polygon2D tmp = new Polygon2D(this.GraphicalArea);
                    if (tmp.IsClockwise()) {
                        tmp.Reverse();
                    }
                    product.Add(tmp);
                    foreach (List<Point2D> p in this.AssociatedRoom.RoomUnusedAreaCoordinates) {
                        Polygon2D unheated = new Polygon2D(p);
                        if (unheated.IsClockwise()) {
                            unheated.Reverse();
                        }
                        List<Polygon2D> unheatedList = new List<Polygon2D>();
                        unheatedList.Add(unheated);
                        IList<Polygon2D> unheatedInProduct = null;
                        try {
                            unheatedInProduct = Polygon2D.GetIntersection(unheatedList, product);
                            foreach (Polygon2D tmp2 in unheatedInProduct) {
                                unheatedSum += Math.Abs(tmp2.GetArea());
                            }
                        } catch {
                            // nothing to do
                        }
                    }
                    unheatedSum = Math.Round(unheatedSum / this.AssociatedRoom.AssociatedPlan.Measure.Value / this.AssociatedRoom.AssociatedPlan.Measure.Value, 2);
                }
                this.PlannedAreaUnheated = (float)unheatedSum;
            }

			if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				int c = this.PlannedConnection.OtherProduct.Product.PlannedCircuits.Count - this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Count;
				foreach (Circuit.CircuitConnection cc in this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Values) {
					if (cc.OtherProduct == this) {
						c++;
					}
				}
				this.CorrectCircuits(this.requestedCircuits.Value, false);
				if (!this.requestedCircuits.HasValue || c < this.requestedCircuits.Value) {
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

			// determine laydistance/rimtype combinations to calculate
			Dictionary<EurovalLayDistance, Nullable<EurovalRimType>[]> teilungen = new Dictionary<EurovalLayDistance, EurovalRimType?[]>();
			if (this.plannedRimLength > 0) {
				teilungen.Add(EurovalLayDistance.NONE, new Nullable<EurovalRimType>[] { null });
				teilungen.Add(EurovalLayDistance.EV35, new Nullable<EurovalRimType>[] { EurovalRimType.EV15_60, EurovalRimType.EV15_120, EurovalRimType.EV15_180 });
				teilungen.Add(EurovalLayDistance.EV30, new Nullable<EurovalRimType>[] { EurovalRimType.EV15_60, EurovalRimType.EV15_120, EurovalRimType.EV15_180 });
				teilungen.Add(EurovalLayDistance.EV25, new Nullable<EurovalRimType>[] { EurovalRimType.EV15_60, EurovalRimType.EV15_120, EurovalRimType.EV15_180 });
				teilungen.Add(EurovalLayDistance.EV20, new Nullable<EurovalRimType>[] { EurovalRimType.EV10_55, EurovalRimType.EV10_110, EurovalRimType.EV10_165 });
				teilungen.Add(EurovalLayDistance.EV15, new Nullable<EurovalRimType>[] { EurovalRimType.EV5_40, EurovalRimType.EV5_80, EurovalRimType.EV5_120 });
				teilungen.Add(EurovalLayDistance.EV10, new Nullable<EurovalRimType>[] { EurovalRimType.EV5_40, EurovalRimType.EV5_80, EurovalRimType.EV5_120 });
				teilungen.Add(EurovalLayDistance.EV5, new Nullable<EurovalRimType>[] { null });
			} else {
				teilungen.Add(EurovalLayDistance.NONE, new Nullable<EurovalRimType>[] { null });
				teilungen.Add(EurovalLayDistance.EV35, new Nullable<EurovalRimType>[] { null });
				teilungen.Add(EurovalLayDistance.EV30, new Nullable<EurovalRimType>[] { null });
				teilungen.Add(EurovalLayDistance.EV25, new Nullable<EurovalRimType>[] { null });
				teilungen.Add(EurovalLayDistance.EV20, new Nullable<EurovalRimType>[] { null });
				teilungen.Add(EurovalLayDistance.EV15, new Nullable<EurovalRimType>[] { null });
				teilungen.Add(EurovalLayDistance.EV10, new Nullable<EurovalRimType>[] { null });
				teilungen.Add(EurovalLayDistance.EV5, new Nullable<EurovalRimType>[] { null });
			}
			if (this.requestedLayDistance != null) {
				EurovalLayDistance[] distances = new EurovalLayDistance[teilungen.Keys.Count];
				teilungen.Keys.CopyTo(distances, 0);
				foreach (EurovalLayDistance distance in distances) {
					if (distance != this.requestedLayDistance) {
						teilungen.Remove(distance);
					}
				}
			}
			if (this.requestedRimType != null) {
				EurovalLayDistance[] distances = new EurovalLayDistance[teilungen.Keys.Count];
				teilungen.Keys.CopyTo(distances, 0);
				foreach (EurovalLayDistance distance in distances) {
					teilungen[distance] = new Nullable<EurovalRimType>[] { this.requestedRimType };
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
			this.CalculateVorlaufRuecklauf(out vorlaufTotal, out vorlaufNotIsolated, out ruecklaufTotal, out ruecklaufNotIsolated, out vorlaufWithoutOtherProductTotal, out vorlaufWithoutOtherProductNotIsolated, out ruecklaufWithoutOtherProductTotal, out ruecklaufWithoutOtherProductNotIsolated, out longestVorlaufTotal, out longestRuecklaufTotal, 12);

			Nullable<EurovalLayDistance> bestLaydistance = null;
			Nullable<EurovalRimType> bestRimType = null;
			int bestCircuits = int.MaxValue;
			double bestPipeLength = double.MaxValue;
			double bestFloorTempRimHeat = double.MaxValue;
			double bestFloorTempResidenceHeat = double.MaxValue;
			double bestPressureLossHeat = double.MaxValue;
			double bestFloorTempRimCool = double.MaxValue;
			double bestFloorTempResidenceCool = double.MaxValue;
			double bestPressureLossCool = double.MaxValue;
			double bestHeatLoad = 0;
			double bestCoolLoad = 0;
			double bestAreaRim = 0;
			double bestAreaResidence = 0;

			this.CalculateHeatAndCoolFlow();
			foreach (EurovalLayDistance ld in teilungen.Keys) {
				foreach (Nullable<EurovalRimType> rt in teilungen[ld]) {
					this.PlannedRimType = rt;
					this.PlannedLayDistance = ld;
					bool tryCalc = true;
					int circuitCount = 1;
					if (this.requestedCircuits.HasValue) {
						circuitCount = this.requestedCircuits.Value;
					} else {
						double pl = (this.plannedArea - this.plannedAreaUnheated - areaRemovedDueConnection - this.PlannedAreaRim) * EurovalProduct.GetPipeLengthPerSqm(ld);
						if (rt.HasValue) {
							pl += this.PlannedAreaRim * EurovalProduct.GetPipeLengthPerSqm(EurovalProduct.GetRimLayDistance(rt.Value));
						} else {
							pl += this.PlannedAreaRim * EurovalProduct.GetPipeLengthPerSqm(ld);
						}
						circuitCount = (int)Math.Ceiling(pl / (100 - longestVorlaufTotal - longestRuecklaufTotal));
						if (this.connectedCircuits.Count > circuitCount) {
							circuitCount = this.connectedCircuits.Count;
						}
					}
					circuitCount = circuitCount < 1 ? 1 : circuitCount;
					circuitCount = circuitCount > 12 ? 12 : circuitCount;
					while (tryCalc) {
						this.lastErrorMsg = this.CorrectCircuits(circuitCount, false);
						if (this.lastErrorMsg != null) {
							this.incompleteCalculation = true;
							return false;
						}
						int i = 0;
						foreach (EurovalCircuit ec in this.circuits) {
							ec.EurovalProduct = this;
							ec.NrOfCircuit = i;
							ec.AreaTotal = this.plannedArea / circuitCount;
							ec.AreaReduced = this.plannedAreaReduced / circuitCount;
							ec.AreaUnheated = this.plannedAreaUnheated / circuitCount;
							ec.AreaRemovedDueConnection = areaRemovedDueConnection / circuitCount;
							ec.RimLength = this.plannedRimLength / circuitCount;
							ec.RimCorners = ((double)this.plannedRimCorners) / circuitCount;
							ec.PipeLengthVorlaufTotal = vorlaufTotal[i];
							ec.PipeLengthVorlaufNotIsolated = vorlaufNotIsolated[i];
							ec.PipeLengthRuecklaufTotal = ruecklaufTotal[i];
							ec.PipeLengthRuecklaufNotIsolated = ruecklaufNotIsolated[i];
							ec.PipeLengthVorlaufWithoutOtherProductTotal = vorlaufWithoutOtherProductTotal[i];
							ec.PipeLengthVorlaufWithoutOtherProductNotIsolated = vorlaufWithoutOtherProductNotIsolated[i];
							ec.PipeLengthRuecklaufWithoutOtherProductTotal = ruecklaufWithoutOtherProductTotal[i];
							ec.PipeLengthRuecklaufWithoutOtherProductNotIsolated = ruecklaufWithoutOtherProductNotIsolated[i];
							ec.Calculate(ld, rt);
							i++;
						}
						tryCalc = false;
						if (this.PlannedDeltaRhoHeat > EurovalProduct.ConfigMaxPressureLost / 100.0) {
							tryCalc = true;
						}
						if (this.PlannedDeltaRhoCool > EurovalProduct.ConfigMaxPressureLost / 100.0) {
							tryCalc = true;
						}
						if (this.PlannedMaxMhHeat > MaxMassenstrom) {
							tryCalc = true;
						}
						if (this.PlannedMaxMhCool > MaxMassenstrom) {
							tryCalc = true;
						}
						tryCalc = tryCalc && !this.requestedCircuits.HasValue;
						tryCalc = tryCalc && circuitCount < 12;
						tryCalc = tryCalc && this.PlannedConnectedProducts.Count == 0;
						if (tryCalc) {
							circuitCount++;
						}
					}
					bool useNew = !bestLaydistance.HasValue || bestLaydistance.Value == EurovalLayDistance.NONE ||
						this.CompareParameters(bestFloorTempRimHeat, bestFloorTempResidenceHeat, bestHeatLoad, bestPressureLossHeat,
							bestFloorTempRimCool, bestFloorTempResidenceCool, bestCoolLoad, bestPressureLossCool,
							bestPipeLength, bestAreaRim, bestAreaResidence,
							this.PlannedFloorTemperatureHeatRim, this.PlannedFloorTemperatureHeatResidence, this.PlannedHeatLoad, this.PlannedDeltaRhoHeat,
							this.PlannedFloorTemperatureCoolRim, this.PlannedFloorTemperatureCoolResidence, this.PlannedCoolLoad, this.PlannedDeltaRhoCool,
							this.PlannedPipeLengthPerCircuit, this.PlannedAreaRim, this.PlannedAreaResidence,
							requestedHeatLoad - this.PlannedHeatLoadAnbindung, requestedCoolLoad - this.PlannedCoolLoadAnbindung, calculateHeat, calculateCool, this.requestedLayDistance.HasValue, this.requestedRimType.HasValue, this.requestedCircuits.HasValue);
					if (useNew) {
						bestLaydistance = ld;
						bestRimType = rt;
						bestCircuits = circuitCount;
						bestPipeLength = this.PlannedPipeLengthPerCircuit;
						bestFloorTempRimHeat = this.PlannedFloorTemperatureHeatRim;
						bestFloorTempResidenceHeat = this.PlannedFloorTemperatureHeatResidence;
						bestHeatLoad = this.PlannedHeatLoad;
						bestPressureLossHeat = this.PlannedDeltaRhoHeat;
						bestFloorTempRimCool = this.PlannedFloorTemperatureCoolRim;
						bestFloorTempResidenceCool = this.PlannedFloorTemperatureCoolResidence;
						bestCoolLoad = this.PlannedCoolLoad;
						bestPressureLossCool = this.PlannedDeltaRhoCool;
						bestAreaRim = this.PlannedAreaRim;
						bestAreaResidence = this.PlannedAreaResidence;
					}
				}
			}

			if (!bestLaydistance.HasValue) {
				this.circuits.Clear();
				this.lastErrorMsg = EuroplanRes.ErrorMessage_KeineAutomatischeAuslegung; //"Keine Automatische Auslegung möglich"
				this.incompleteCalculation = true;
				return false;
			}

			{ // calculate best choice again
				this.plannedLayDistance = bestLaydistance;
				this.plannedRimType = bestRimType;
				this.CorrectCircuits(bestCircuits, true);

				int i = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					ec.EurovalProduct = this;
					ec.NrOfCircuit = i;
					if (this.PlannedCorrections) {
						ec.AreaTotal = this.plannedCorrectionList[i].AreaValue;
						ec.AreaReduced = this.plannedCorrectionList[i].AreaReducedValue;
						ec.AreaUnheated = this.plannedCorrectionList[i].AreaUnheatedValue;
						ec.AreaRemovedDueConnection = this.plannedCorrectionList[i].ConnectionsValue;
						ec.RimLength = this.plannedCorrectionList[i].RimLengthValue;
						ec.RimCorners = this.plannedCorrectionList[i].RimCornersValue;
					} else {
						ec.AreaTotal = this.plannedArea / bestCircuits;
						ec.AreaReduced = this.plannedAreaReduced / bestCircuits;
						ec.AreaUnheated = this.plannedAreaUnheated / bestCircuits;
						ec.AreaRemovedDueConnection = areaRemovedDueConnection / bestCircuits;
						ec.RimLength = this.plannedRimLength / bestCircuits;
						ec.RimCorners = ((double)this.plannedRimCorners) / bestCircuits;
					}
					ec.PipeLengthVorlaufTotal = vorlaufTotal[i];
					ec.PipeLengthVorlaufNotIsolated = vorlaufNotIsolated[i];
					ec.PipeLengthRuecklaufTotal = ruecklaufTotal[i];
					ec.PipeLengthRuecklaufNotIsolated = ruecklaufNotIsolated[i];
					ec.PipeLengthVorlaufWithoutOtherProductTotal = vorlaufWithoutOtherProductTotal[i];
					ec.PipeLengthVorlaufWithoutOtherProductNotIsolated = vorlaufWithoutOtherProductNotIsolated[i];
					ec.PipeLengthRuecklaufWithoutOtherProductTotal = ruecklaufWithoutOtherProductTotal[i];
					ec.PipeLengthRuecklaufWithoutOtherProductNotIsolated = ruecklaufWithoutOtherProductNotIsolated[i];
					ec.Calculate(bestLaydistance.Value, bestRimType);
					i++;
				}

				if (variableSpreizung && this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
					double defSpreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
					double defSpreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
					// Heizleistung veringern
					while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < EurovalProduct.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad && this.PlannedSpreizungHeat < 1.2 * defSpreizungHeat) {
						this.plannedRuecklaufTempHeat -= 0.1;
						foreach (EurovalCircuit ec in this.circuits) {
							ec.Calculate(bestLaydistance.Value, bestRimType);
						}
					}
					this.plannedRuecklaufTempHeat += 0.1;
					// Heizleistung erhöhen
					while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > EurovalProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < EurovalProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhHeat < MaxMassenstrom && this.PlannedSpreizungHeat > 0.8 * defSpreizungHeat) {
						this.plannedRuecklaufTempHeat += 0.1;
						foreach (EurovalCircuit ec in this.circuits) {
							ec.Calculate(bestLaydistance.Value, bestRimType);
						}
					}
					// Kühlleistung verringern
					while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < EurovalProduct.ConfigSpreizungKuehlMax && this.PlannedCoolLoad > requestedCoolLoad && this.PlannedSpreizungCool < 1.2 * defSpreizungCool) {
						this.plannedRuecklaufTempCool += 0.1;
						i = 0;
						foreach (EurovalCircuit ec in this.circuits) {
							ec.Calculate(bestLaydistance.Value, bestRimType);
						}
					}
					this.plannedRuecklaufTempCool -= 0.1;
					// Kühlleistung erhöhen
					while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > EurovalProduct.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < EurovalProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhCool < MaxMassenstrom && this.PlannedSpreizungCool > 0.8 * defSpreizungCool) {
						this.plannedRuecklaufTempCool -= 0.1;
						i = 0;
						foreach (EurovalCircuit ec in this.circuits) {
							ec.Calculate(bestLaydistance.Value, bestRimType);
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
			}

			if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				if (!this.secondConfig) {
					this.secondConfig = true;
					this.PlannedConnection.OtherProduct.ConfigureProduct(variableSpreizung);
					bool ok = this.ConfigureProduct(requestedHeatLoad, requestedCoolLoad, canHeat, canCool, false);
					this.secondConfig = false;
					this.incompleteCalculation = !ok;
					return ok;
				}
			}

			if (areaRemovedDueConnection > this.plannedArea - this.plannedAreaUnheated) {
				this.incompleteCalculation = true;
				this.lastErrorMsg = EuroplanRes.ErrorMessage_Anbindeleitung;
				this.lastErrorMsg = this.lastErrorMsg.Replace("%VALUE%", Math.Round(this.PlannedRemoveArea, 1).ToString());
				this.lastErrorMsg = this.lastErrorMsg.Replace("%MAXIMUM%", Math.Round(this.AvailableFloorArea, 1).ToString());
				return false;
			}

			this.lastErrorMsg = "";
			string newMsg;
			if (this.LongestPipeLengthPerCircuitWithAllConnections > EurovalProduct.ConfigMaxCircuitLength) {
				newMsg = EuroplanRes.ErrorMessage_Rohrlaenge;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PipeLengthWithoutConnectionsOfLongestPipeWithConnections, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(EurovalProduct.ConfigMaxCircuitLength - this.ConnectionLengthOfLongestPipeWithConnections, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeatResidence, 1) > this.MaxResidenceTemp && this.requestedHeatLoad > 0) {
				newMsg = EuroplanRes.ErrorMessage_TemperaturAz;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedFloorTemperatureHeatResidence, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.MaxResidenceTemp, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeatRim, 1) > this.MaxRimTemp && this.requestedHeatLoad > 0) {
				newMsg = EuroplanRes.ErrorMessage_TemperaturRz;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedFloorTemperatureHeatRim, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.MaxRimTemp, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
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
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > Math.Round(EurovalProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoHeat, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(EurovalProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > Math.Round(EurovalProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoCool, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(EurovalProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.lastErrorMsg.Length == 0) {
				this.lastErrorMsg = null;
			}

			return true;
		}

		private string CorrectCircuits(int circuitCount, bool cleanupConnected) {
			string error = null;
			if (this.circuits.Count > circuitCount) {
				this.circuits.RemoveRange(circuitCount, this.circuits.Count - circuitCount);
			}
			List<KeyValuePair<int, Circuit.CircuitConnection>> remove = new List<KeyValuePair<int, Circuit.CircuitConnection>>();
			if (cleanupConnected) {
				foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.connectedCircuits) {
					if (kvp.Key >= circuitCount && !kvp.Value.UserDefined) {
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
				EurovalCircuit ec = new EurovalCircuit();
				ec.EurovalProduct = this;
				ec.NrOfCircuit = this.circuits.Count;
				if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					int j = 0;
					bool found = false;
					while (!found && j < this.PlannedConnection.OtherProduct.Product.PlannedCircuitCount) {
						found = !this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.ContainsKey(j);
						j++;
					}
					if (!found) {
						error = EuroplanRes.ErrorMessage_HkAnschluss; // "Es sind nicht alle Heizkreise dieses Systems angeschloßen"
					} else {
						j--;
						this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Add(j, new Circuit.CircuitConnection(this.PlannedConnection.CircuitConnectionType, ec, false));
						if (this.PlannedConnection.OtherProduct.Product.PlannedCircuits.Count > j) {
							this.inverseConnectedCircuits.Add(ec.NrOfCircuit, new Circuit.CircuitConnection(this.PlannedConnection.CircuitConnectionType, this.PlannedConnection.OtherProduct.Product.PlannedCircuits[j], this.PlannedConnection.UserDefined));
						} else {
							this.inverseConnectedCircuits.Add(ec.NrOfCircuit, new Circuit.CircuitConnection(this.PlannedConnection.CircuitConnectionType, this.PlannedConnection.OtherProduct, j, this.PlannedConnection.UserDefined));
						}
					}					
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
			return error;
		}

		private double MaxRimTemp {
			get {
				double maxTemp = ConfigUseHarreitherNorm ? ConfigMaxRimTempHarreither : ConfigMaxRimTempEn1264;
				return AssociatedRoom.IsNassraum ? Math.Max(maxTemp, ConfigMaxNassraumTemp) : maxTemp;
			}
		}

		private double MaxResidenceTemp {
			get {
				double maxTemp = ConfigUseHarreitherNorm ? ConfigMaxResidenceTempHarreither : ConfigMaxResidenceTempEn1264;
				return AssociatedRoom.IsNassraum ? Math.Max(maxTemp, ConfigMaxNassraumTemp) : maxTemp;
			}
		}

		[XmlIgnore]
		public override double WasserInhalt {
			get {
				double length = 0;
				foreach (EurovalCircuit c in this.circuits) {
					length += c.PipeLengthWithoutOtherProduct;
				}
				return rohrInnenA * length * 1000;
			}
		}

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {

			// Anbindeleitungen
			this.AddRequiredMaterialForConnections(requiredMaterial, false, 0, true);

			// Euroval Rohr
			double length = 0;
			foreach (EurovalCircuit c in this.circuits) {
				length += c.PipeLengthWithoutConnections;
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, "EV01", length);

			// Clipschiene
			string clipschiene = clipSchieneKlebeband ? "EV16" : "EV15";
			double amount = 0;
			if (this.PlannedLayDistance.HasValue) {
				amount += this.PlannedAreaResidenceHeated * GetClipschienePerSqm(this.PlannedLayDistance.Value, anhydritEstrich);
			}
			if (this.PlannedRimType.HasValue) {
				amount += this.PlannedAreaRim * GetClipschienePerSqm(GetRimLayDistance(this.PlannedRimType.Value), anhydritEstrich);
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, clipschiene, amount);

			// Ovalmuffe
			amount = 0;
			if (this.PlannedLayDistance.HasValue) {
				amount += this.PlannedAreaResidenceHeated * GetOvalmuffePerSqm(this.PlannedLayDistance.Value);
			}
			if (this.PlannedRimType.HasValue) {
				amount += this.PlannedAreaRim * GetOvalmuffePerSqm(GetRimLayDistance(this.PlannedRimType.Value));
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, "EV10", amount);

			//Verteileranschlußbögen
			if (this.PlannedConnection != null && this.PlannedConnection.Distributor != null) {
				string verteilerAnschluß = this.PlannedConnection.Distributor.LangeAnschlussboegen ? "EV21" : "EV20";

				Project.Instance.AddRequiredMaterial(requiredMaterial, verteilerAnschluß, this.circuits.Count * 2);
			}

			// nur bei Estrichkonstruktion
			if (this.HasInsideConstruction) {
				if (this.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH) ||
					this.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH)) {
					// Eco 30
					if (!anhydritEstrich) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "EV34", (this.plannedArea) * 0.2);
					}
					// Randdämmstreifen
					Project.Instance.AddRequiredMaterial(requiredMaterial, "EV30", (this.plannedArea));
					// PE Folie
					Project.Instance.AddRequiredMaterial(requiredMaterial, "EV31", (this.plannedArea) * 1.1);
				}
			}

			// Dämmung
			if (this.HasOutsideConstruction) {
				foreach (ConstructionLayer layer in this.PlannedOutsideConstruction.Layers) {
					if (layer.LayerMaterial != null) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, layer.LayerMaterial.Id, this.plannedArea);
					}
				}
			}

			// unknown amount
			Project.Instance.AddRequiredMaterial(requiredMaterial, "EV11", Double.NegativeInfinity);
			Project.Instance.AddRequiredMaterial(requiredMaterial, "EV12", Double.NegativeInfinity);
		}

		public override double Dichte {
			get { return EurovalProduct.ConfigRho; }
		}

		public override double Waermekapazitaet {
			get { return EurovalProduct.ConfigC; }
		}

		public override double Viskositaet {
			get { return EurovalProduct.ConfigV; }
		}

		[XmlIgnore]
		public bool PlannedCorrections {
			get{ return this.plannedCorrectionList.Count > 0; }
			set {
				if (this.PlannedCorrections != value) {
					this.plannedCorrectionList.Clear();
					if (value) {
						this.requestedCircuits = this.PlannedCircuitCount;
						this.requestedLayDistance = this.PlannedLayDistance;
						this.requestedRimType = this.PlannedRimType;
					}
					if (value && this.requestedCircuits != null && this.requestedLayDistance != null && (this.plannedRimLength == 0 || this.requestedRimType != null)) {
						for (int i = 0; i < this.requestedCircuits.Value; i++ ) {
							this.plannedCorrectionList.Add(new ExtendedCorrections(i + 1, this));
						}
					}
				}
			}
		}

		public List<ExtendedCorrections> PlannedCorrectionList {
			get { return this.plannedCorrectionList; }
			set {
				this.plannedCorrectionList = (value == null) ? new List<ExtendedCorrections>() : value;
			}
		}

		internal override void FinalizeLoading(PlannedProduct pp) {
			base.FinalizeLoading(pp);
			foreach (EurovalCircuit c in this.circuits) {
				c.EurovalProduct = this;
			}
			if (this.PlannedCorrections) {
				int i = 1;
				foreach (ExtendedCorrections ec in this.PlannedCorrectionList) {
					ec.EurovalProduct = this;
					ec.CircuitNr = i++;
				}
				foreach (EurovalCircuit ec in this.circuits) {
					ec.EurovalProduct = this;
				}
			}
		}

		public override bool ManualMode {
			get {
				return (this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT); // ||
			}
		}

		[XmlIgnore]
		public override WW.Math.Geometry.Polygon2D GraphicalArea {
			get { return new Polygon2D(this.plannedAreaGraphical); }
		}

		public Point2D TextBoxPosition {
			get { return this.textBoxPosition; }
			set { this.textBoxPosition = value;	}
		}


		public Nullable<float> TextBoxFontSize {
			get { return textBoxFontSize; }
			set { textBoxFontSize = value; }
		}

        public float TextBoxFontSizeForUse {
            get { return this.TextBoxFontSize.HasValue ? this.TextBoxFontSize.Value : (float)Product.ConfigBoxFontSize; }
        }

        public float TextBoxRotation {
            get { return this.textBoxRotation; }
            set { this.textBoxRotation = value; }
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
				polygon.Add(bestConnectionPoint + (v * width / 2) + (v2 * 0.05 * measure));
				polygon.Add(bestConnectionPoint - (v * width / 2) + (v2 * 0.05 * measure));
				polygon.Add(bestConnectionPoint - (v * width / 2));

				double angle = -Math.Atan2(v.X, v.Y) * 180.0 / Math.PI;

				possibleConnection = new PossibleProductConnection(bestConnectionPoint, polygon, input, output, angle, this, firstCircuit, otherCircuits);
			}
			return possibleConnection;
		}

        public string PipeLengthText {
            get {
                double vl = this.LongestVlPerCircuit;
                double rl = this.LongestRlPerCircuit;
                if (vl > 0 || rl > 0) {
                    return Math.Round(this.PlannedPipeLengthPerCircuit + vl + rl, 1).ToString() + " (" + Math.Ceiling(vl) + "+" + Math.Ceiling(rl) + ")";
                } else {
                    return Math.Round(this.PlannedPipeLengthPerCircuit, 1).ToString();
                }
            }
        }

        [XmlIgnore]
        public Color ProductColor {
            get { return this.productColor; }
            set { this.productColor = value; }
        }

        // Color cannot be serialized!!!
        // quick workaround to serialize it nevertheless
        public int ProductColorR {
            get { return this.productColor.R; }
            set { this.productColor = Color.FromArgb(value, this.productColor.G, this.productColor.B); }
        }
        public int ProductColorG {
            get { return this.productColor.G; }
            set { this.productColor = Color.FromArgb(this.productColor.R, value, this.productColor.B); }
        }
        public int ProductColorB {
            get { return this.productColor.B; }
            set { this.productColor = Color.FromArgb(this.productColor.R, this.productColor.G, value); }
        }
    }
}
