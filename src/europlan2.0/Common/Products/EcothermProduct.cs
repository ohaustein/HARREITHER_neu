using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Ecotherm®", "Ecotherm® Fußbodenheizung")]
	public class EcothermProduct : Product {

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
		private static double rohrAussenD = 0.017; /* Aussendurchmesser Ecotherm Rohr */
		private static double rohrInnenD = 0.013; /* Rohrinnendurchmesser */
		private static double rohrInnenA = (rohrInnenD / 2) * (rohrInnenD / 2) * Math.PI; /* Rohrinnenquerschnitt */
		private static double ag = 1.1034; /* Ovalrohr Geometriefaktor für Euroval */
		private static double sr0 = 0.002; /* fix ??? */
		private static double sr = 0.00238; /* Aus Euroval Normprüfdaten */
		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static double v = 0.00000101; /* m²/s ... kinematische Viskosität */
		private static bool agActivated = false;
		private static double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
		private static double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */

		private static double faktorTrockenkonstruktion = 0.45;

		private static double maxResidenceTempHarreither = 27;
		private static double maxRimTempHarreither = 33;
		private static double maxResidenceTempEn1264 = 29;
		private static double maxRimTempEn1264 = 35;

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

		private Nullable<EcothermLayDistance> requestedLayDistance = null;
		private Nullable<EcothermRimType> requestedRimType = null;
		private Nullable<int> requestedCircuits = null;

		private Nullable<EcothermLayDistance> plannedLayDistance = null;
		private Nullable<EcothermRimType> plannedRimType = null;

		private List<ExtendedCorrections> plannedCorrectionList = new List<ExtendedCorrections>();

		/*public override int GetIndexOfCircuit(Circuit c) {
			int i = 0;
			foreach (EurovalCircuit ec in this.circuits) {
				if (ec == c) {
					return i;
				}
				i++;
			}
			return -1;
		}*/

		private bool clipSchieneKlebeband = false;
		private bool anhydritEstrich = false;

		public class LayDistanceConverter : System.ComponentModel.TypeConverter {
			private static readonly string A5 = "A5";
			private static readonly string EV5 = "EV5";
			private static readonly string EV10 = "EV10";
			private static readonly string EV15 = "EV15";
			private static readonly string EV20 = "EV20";
			private static readonly string EV25 = "EV25";
			private static readonly string EV30 = "EV30";
			private static readonly string EV35 = "EV35";

			private Dictionary<string, EcothermLayDistance> mappingFromString = new Dictionary<string, EcothermLayDistance>();
			private Dictionary<EcothermLayDistance, string> mappingToString = new Dictionary<EcothermLayDistance, string>();

			public LayDistanceConverter() {
				mappingFromString.Add(A5, EcothermLayDistance.A5);
				mappingFromString.Add(EV5, EcothermLayDistance.EV5);
				mappingFromString.Add(EV10, EcothermLayDistance.EV10);
				mappingFromString.Add(EV15, EcothermLayDistance.EV15);
				mappingFromString.Add(EV20, EcothermLayDistance.EV20);
				mappingFromString.Add(EV25, EcothermLayDistance.EV25);
				mappingFromString.Add(EV30, EcothermLayDistance.EV30);
				mappingFromString.Add(EV35, EcothermLayDistance.EV35);
				mappingToString.Add(EcothermLayDistance.A5, A5);
				mappingToString.Add(EcothermLayDistance.EV5, EV5);
				mappingToString.Add(EcothermLayDistance.EV10, EV10);
				mappingToString.Add(EcothermLayDistance.EV15, EV15);
				mappingToString.Add(EcothermLayDistance.EV20, EV20);
				mappingToString.Add(EcothermLayDistance.EV25, EV25);
				mappingToString.Add(EcothermLayDistance.EV30, EV30);
				mappingToString.Add(EcothermLayDistance.EV35, EV35);
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
				if (value is EcothermLayDistance && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((EcothermLayDistance)value)) {
						return mappingToString[(EcothermLayDistance)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(LayDistanceConverter))]

		public enum EcothermLayDistance {
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

		public enum EcothermRimType {
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

		public EcothermProduct(){

		}

		protected EcothermProduct(EcothermProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public new static void StaticInitialize(Configuration config) {
			quickDimensioningHeatPowerPerSquareMeter = config.GetProductParameterAsInt<EcothermProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 50);
			quickDimensioningCoolPowerPerSquareMeter = config.GetProductParameterAsInt<EcothermProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 50);
			canHeat = config.GetProductParameterAsBool<EcothermProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = config.GetProductParameterAsBool<EcothermProduct>("ConfigQuickDimensioningCanCool", false);
			useHarreitherNorm = config.GetProductParameterAsBool<EcothermProduct>("ConfigUseHarreitherNorm", true);
			maxCircuitLength = config.GetProductParameterAsDouble<EcothermProduct>("ConfigMaxCircuitLength", 100.0);
			maxPressureLost = config.GetProductParameterAsInt<EcothermProduct>("ConfigMaxPressureLost", 15000);
			maxDurchfluss = config.GetProductParameterAsInt<EcothermProduct>("ConfigMaxDurchfluss", 240);
			spreizungHeizMin = config.GetProductParameterAsDouble<EcothermProduct>("ConfigSpreizungHeizMin", 4);
			spreizungHeizMax = config.GetProductParameterAsDouble<EcothermProduct>("ConfigSpreizungHeizMax", 12);
			spreizungKuehlMin = config.GetProductParameterAsDouble<EcothermProduct>("ConfigSpreizungKuehlMin", 2);
			spreizungKuehlMax = config.GetProductParameterAsDouble<EcothermProduct>("ConfigSpreizungKuehlMax", 5);
			su0 = config.GetProductParameterAsDouble<EcothermProduct>("ConfigSu0", 0.045);
			su = config.GetProductParameterAsDouble<EcothermProduct>("ConfigSu", 0.035);
		}

		public static string GlobalNotificationMessage {
			get {
				string message = null;
				Configuration userConfig = Configuration.UserTemplate;

				double defaultSu0 = userConfig.GetProductParameterAsDouble<EcothermProduct>("ConfigSu0", 0.045);
				if (su0 != defaultSu0) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					message += "  Mindestüberdeckung: " + Math.Round(su0, 3).ToString() + " (Standardwert: " + Math.Round(defaultSu0, 3).ToString() + ")";
				}

				double defaultSu = userConfig.GetProductParameterAsDouble<EcothermProduct>("ConfigSu", 0.035);
				if (su != defaultSu) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					message += "  Estrichüberdeckung: " + Math.Round(su, 3).ToString() + " (Standardwert: " + Math.Round(defaultSu, 3).ToString() + ")";
				}

				if (message != null) {
					message = "Ecotherm-Systeme werden mit veränderten Paramtern berechnet. Folgende Parameter weichen von den Standardwerten ab:\n" + message;
				}

				return message;
			}
		}

		public override Product Clone(Room room) {
			EcothermProduct product = new EcothermProduct(this);
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
			get { return Product.ConfigAlphaBodenCool; }
		}

		public static double ConfigAlphaFbh {
			get { return Product.ConfigAlphaBodenHeat; }
		}

		[ProductParameter]
		public static double ConfigLambdaR0 {
			get { return lambdaR0; }
			set { lambdaR0 = value; }
		}

		[ProductParameter]
		public static double ConfigLambdaR {
			get { return lambdaR; }
			set { lambdaR = value; }
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
		public static double ConfigRohrAussenD {
			get { return rohrAussenD; }
			set { rohrAussenD = value; }
		}

		[ProductParameter]
		public static double ConfigRohrInnenD {
			get { return rohrInnenD; }
			set { rohrInnenD = value; }
		}

		[ProductParameter]
		public static double ConfigRohrInnenA {
			get { return rohrInnenA; }
			set { rohrInnenA = value; }
		}

		[ProductParameter]
		public static double ConfigAg {
			get { return ag; }
			set { ag = value; }
		}

		[ProductParameter]
		public static double ConfigSr0 {
			get { return sr0; }
			set { sr0 = value; }
		}

		[ProductParameter]
		public static double ConfigSr {
			get { return sr; }
			set { sr = value; }
		}

		[ProductParameter]
		public static double ConfigC {
			get { return c; }
			set { c = value; }
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
		public static bool ConfigAgActivated {
			get { return agActivated; }
			set { agActivated = value; }
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
		public static double ConfigFaktorTrockenkonstruktion {
			get { return faktorTrockenkonstruktion; }
			set { faktorTrockenkonstruktion = value; }
		}

		[ProductParameter]
		public static double ConfigMaxResidenceTempHarreither {
			get { return maxResidenceTempHarreither; }
			set { maxResidenceTempHarreither = value; }
		}

		[ProductParameter]
		public static double ConfigMaxRimTempHarreither {
			get { return maxRimTempHarreither; }
			set { maxRimTempHarreither = value; }
		}

		[ProductParameter]
		public static double ConfigMaxResidenceTempEn1264 {
			get { return maxResidenceTempEn1264; }
			set { maxResidenceTempEn1264 = value; }
		}

		[ProductParameter]
		public static double ConfigMaxRimTempEn1264 {
			get { return maxRimTempEn1264; }
			set { maxRimTempEn1264 = value; }
		}

		[ProductParameter]
		public static bool ConfigUseHarreitherNorm {
			get { return useHarreitherNorm; }
			set { useHarreitherNorm = value; }
		}

		[ProductParameter]
		public static double ConfigMaxCircuitLength {
			get { return maxCircuitLength; }
			set { maxCircuitLength = value; }
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
		#endregion Product Parameters

		/// <summary>
		/// Returns the default number of circuit for the planned area (for quick dimensioning)
		/// </summary>
		public override int GetDefaultQuickDimensioningCircuits() {
			EcothermLayDistance distance = (EcothermProduct.EcothermLayDistance)Project.Instance.QuickDimensioning.LayDistance;
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
		public static double GetPipeLengthPerSqm(EcothermLayDistance distance) {
			switch (distance) {
				case EcothermLayDistance.A5:
					return 20;
				case EcothermLayDistance.EV5:
					return 10;
				case EcothermLayDistance.EV10:
					return 7.5;
				case EcothermLayDistance.EV15:
					return 6.7;
				case EcothermLayDistance.EV20:
					return 5;
				case EcothermLayDistance.EV25:
					return 4;
				case EcothermLayDistance.EV30:
					return 3.5;
				case EcothermLayDistance.EV35:
					return 3;
				case EcothermLayDistance.NONE:
					return 0;
				default:
					throw new Exception("Unknown Laydistance");
			}
		}

		/// <summary>
		/// Returns the number of clipschiene in m per m² for the specified laydistance and estrich
		/// </summary>
		public static double GetClipschienePerSqm(EcothermLayDistance distance, bool anhydritEstrich) {
			if (anhydritEstrich) {
				return 2;
			} else {
				switch (distance) {
					case EcothermLayDistance.A5:
						return 2.5;
					case EcothermLayDistance.EV5:
						return 2;
					case EcothermLayDistance.EV10:
						return 1.8;
					case EcothermLayDistance.EV15:
						return 1.7;
					case EcothermLayDistance.EV20:
						return 1.6;
					case EcothermLayDistance.EV25:
						return 1.5;
					case EcothermLayDistance.EV30:
						return 1.4;
					case EcothermLayDistance.EV35:
						return 1.4;
					case EcothermLayDistance.NONE:
						return 0;
					default:
						throw new Exception("Unknown Laydistance");
				}
			}
		}

		private double GetMuffePerSqm(EcothermLayDistance layDistance) {
			switch (layDistance) {
				case EcothermLayDistance.A5:
					return 0.1;
				case EcothermLayDistance.EV5:
					return 0.07;
				case EcothermLayDistance.EV10:
					return 0.06;
				case EcothermLayDistance.EV15:
					return 0.05;
				case EcothermLayDistance.EV20:
					return 0.04;
				case EcothermLayDistance.EV25:
					return 0.03;
				case EcothermLayDistance.EV30:
					return 0.02;
				case EcothermLayDistance.EV35:
					return 0.02;
				case EcothermLayDistance.NONE:
					return 0;
				default:
					throw new Exception("Unknown Laydistance");
			}
		}

		/// <summary>
		/// Returns distance between two pipes in m for specified laydistance 
		/// </summary>
		public static double GetTeilung(EcothermLayDistance distance) {
			switch (distance) {
				case EcothermLayDistance.A5:
					return 0.05;
				case EcothermLayDistance.EV5:
					return 0.1;
				case EcothermLayDistance.EV10:
					return 0.125;
				case EcothermLayDistance.EV15:
					return 0.15;
				case EcothermLayDistance.EV20:
					return 0.2;
				case EcothermLayDistance.EV25:
					return 0.25;
				case EcothermLayDistance.EV30:
					return 0.3;
				case EcothermLayDistance.EV35:
					return 0.35;
				case EcothermLayDistance.NONE:
					return double.MaxValue;
				default:
					throw new Exception("Unknwon LayDistance");
			}
		}

		/// <summary>
		/// Returns the laydistance for the specified rimtype
		/// </summary>
		public static EcothermLayDistance GetRimLayDistance(EcothermRimType rimType) {
			switch (rimType) {
				case EcothermRimType.EV15_60:
				case EcothermRimType.EV15_120:
				case EcothermRimType.EV15_180:
					return EcothermLayDistance.EV15;
				case EcothermRimType.EV10_55:
				case EcothermRimType.EV10_110:
				case EcothermRimType.EV10_165:
					return EcothermLayDistance.EV10;
				case EcothermRimType.EV5_40:
				case EcothermRimType.EV5_80:
				case EcothermRimType.EV5_120:
					return EcothermLayDistance.EV5;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		/// <summary>
		/// Returns the width of the rim in cm for the specified rimtype
		/// </summary>
		public static int GetRimWidth(EcothermRimType rimType) {
			switch (rimType) {
				case EcothermRimType.EV5_40:
					return 40;
				case EcothermRimType.EV10_55:
					return 55;
				case EcothermRimType.EV15_60:
					return 60;
				case EcothermRimType.EV5_80:
					return 80;
				case EcothermRimType.EV10_110:
					return 110;
				case EcothermRimType.EV15_120:
				case EcothermRimType.EV5_120:
					return 120;
				case EcothermRimType.EV10_165:
					return 165;
				case EcothermRimType.EV15_180:
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
			get { return "Ecotherm®\n(m²)"; }
		}
		#endregion QuickDimensioning

		/// <summary>
		/// The type of this product
		/// </summary>
		public override ProductType Type {
			get { return ProductType.FBH; }
		}

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_ECOTHERM; }
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
		public Nullable<EcothermLayDistance> RequestedLayDistance {
			get { return this.requestedLayDistance; }
			set { this.requestedLayDistance = value; }
		}

		/// <summary>
		/// The rim type the user requested for this product in the planning.
		/// If this property is null the optimal rim type will be calculated.
		/// </summary>
		public Nullable<EcothermRimType> RequestedRimType {
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
				if ((this.plannedConnection != null && this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) ||
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
				foreach (EcothermCircuit ec in this.circuits) {
					value += (ec.AreaTotal - ec.GetAreaRim(this.plannedRimType) - ec.AreaRemovedDueConnection);
				}
				return (float)value;
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
		public Nullable<EcothermLayDistance> PlannedLayDistance {
			get { return this.plannedLayDistance; }
			set { this.plannedLayDistance = value; }
		}

		/// <summary>
		/// The rim type used for calculation. This is either the rim type the user requested
		/// or if the user did not request any specific rim type the optimal rim type is
		/// calculated.
		/// </summary>
		[XmlIgnore]
		public Nullable<EcothermRimType> PlannedRimType {
			get { return this.plannedRimType; }
			set { this.plannedRimType = value; }
		}

		/// <summary>
		/// The lay distance of the rim type used for calculation.
		/// </summary>
		[XmlIgnore]
		public Nullable<EcothermLayDistance> PlannedRimLayDistance {
			get { return this.plannedRimType == null ? (Nullable<EcothermLayDistance>)null : (Nullable<EcothermLayDistance>)GetRimLayDistance(this.plannedRimType.Value); }
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
				double area = this.PlannedAreaResidence;
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				double area = this.PlannedAreaResidence;
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				double value = 0;
				foreach (EcothermCircuit ec in this.circuits) {
					if (ec.C_FloorTempRzCool > value) {
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
				double value = 0;
				foreach (EcothermCircuit ec in this.circuits) {
					if (ec.C_FloorTempAzCool > value) {
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
				foreach (EcothermCircuit ec in this.circuits) {
					if (ec.PipeLengthWithoutConnections > value) {
						value = ec.PipeLengthWithoutConnections;
					}
				}
				return value;
				//return this.plannedPipeLength / this.PlannedCircuits; 
			}
		}

		[XmlIgnore]
		public double LongestPipeLengthPerCircuitWithAllConnections {
			get {
				if (this.incompleteCalculation) {
					return 0;
				}
				double value = 0;
				foreach (EcothermCircuit ec in this.circuits) {
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
				foreach (EcothermCircuit ec in this.circuits) {
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
				foreach (EcothermCircuit ec in this.circuits) {
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

			if (checkHeat && (useHarreitherNorm && ((floorTempHeatRim > maxRimTempHarreither && !ignoreRim) || (floorTempHeatRes > maxResidenceTempHarreither && !ignoreResidence)))) {
				return false;
			}
			if (checkHeat && ((floorTempHeatRim > maxRimTempEn1264 && !ignoreRim) || (floorTempHeatRes > maxResidenceTempEn1264 && !ignoreResidence))) {
				return false;
			}
			if (checkHeat && (pressureLossHeat > maxPressureLost) && !ignoreCircuitLength) {
				return false;
			}
			// TODO checkCool
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
			double oldCircuitLength,
			double newFloorTempHeatRim, double newFloorTempHeatRes, double newHeatLoad, double newPressureLossHeat,
			double newFloorTempCoolRim, double newFloorTempCoolRes, double newCoolLoad, double newPressureLossCool, 
			double newCircuitLength,
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
			if (oldOk) {
				bool oldCovers = CoversLoads(oldHeatLoad, oldCoolLoad, checkHeat ? requestedHeatLoad : 0, checkCool ? requestedCoolLoad : 0);
				bool newCovers = CoversLoads(newHeatLoad, newCoolLoad, checkHeat ? requestedHeatLoad : 0, checkCool ? requestedCoolLoad : 0);
				if (oldCovers != newCovers) {
					return newCovers;
				}
				if (oldCovers) {
					// TODO implement better decisison which parameters should be used
					if (checkCool) {
						return newFloorTempCoolRes >= oldFloorTempCoolRes;
					}
					return newFloorTempHeatRes <= oldFloorTempHeatRes;
				} else {
					if (checkCool) {
						return newCoolLoad > oldCoolLoad;
					}
					return newHeatLoad > oldHeatLoad;
				}
			} else {

			}
			return true;
		}

		public override ProductConnection PlannedConnection {
			get { return this.plannedConnection; }
			set {
				if (value != null && value.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
					this.requestedCircuits = this.PlannedCircuitCount > 0 ? this.PlannedCircuitCount : 1;
					this.requestedLayDistance = this.plannedLayDistance.HasValue ? this.plannedLayDistance.Value : EcothermLayDistance.EV35;
					this.requestedRimType = this.plannedRimType.HasValue ? this.plannedRimType.Value : EcothermRimType.EV15_60;
				}
				this.plannedConnection = value;

			}
		}

		public override void CalculateHeatAndCoolFlow() {
			base.CalculateHeatAndCoolFlow();
			double spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			double spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			if (spreizungHeat > EcothermProduct.ConfigSpreizungHeizMax) {
				spreizungHeat = EcothermProduct.ConfigSpreizungHeizMax;
			}
			if (spreizungHeat < EcothermProduct.ConfigSpreizungHeizMin) {
				spreizungHeat = EcothermProduct.ConfigSpreizungHeizMin;
			}
			if (spreizungCool > EcothermProduct.ConfigSpreizungKuehlMax) {
				spreizungCool = EcothermProduct.ConfigSpreizungKuehlMax;
			}
			if (spreizungCool < EcothermProduct.ConfigSpreizungKuehlMin) {
				spreizungCool = EcothermProduct.ConfigSpreizungKuehlMin;
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
			if (this.plannedFloorConstruction == null || this.plannedInsulationConstruction == null || (this.PlannedConnection == null && !this.plannedProductIsConnection)) {
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

			if (this.plannedProductIsConnection) {
				this.plannedLayDistance = null;
				this.plannedRimType = null;
				this.circuits.Clear();
				this.lastErrorMsg = null;
				return true;
			}

			if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				// TODO connect all circuits

				int c = this.PlannedConnection.OtherProduct.Product.PlannedCircuits.Count - this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Count;
				foreach (Circuit.CircuitConnection cc in this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Values) {
					if (cc.OtherProduct == this) {
						c++;
					}
				}
				if (!this.requestedCircuits.HasValue || c < this.requestedCircuits.Value) {
					/*if (this.requestedCircuits.HasValue) {
						// TODO reset circuits
					} else {*/
						//this.circuits.Clear();
					/*}*/
					this.lastErrorMsg = "Es sind nicht alle Heizkreise dieses Systems angeschloßen";
					this.incompleteCalculation = true;
					return false;
				}
				bool userDefinedOk = true;
				foreach (Circuit.CircuitConnection cc in this.inverseConnectedCircuits.Values) {
					if (cc.OtherCircuit == null) {
						userDefinedOk = false;
					}
				}
				this.CorrectCircuits(this.requestedCircuits.Value, false);
				if (!userDefinedOk) {
					this.lastErrorMsg = "Es sind nicht alle Heizkreise dieses Systems angeschloßen";
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
			Dictionary<EcothermLayDistance, Nullable<EcothermRimType>[]> teilungen = new Dictionary<EcothermLayDistance, EcothermRimType?[]>();
			if (this.plannedRimLength > 0) {
				teilungen.Add(EcothermLayDistance.NONE, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV35, new Nullable<EcothermRimType>[] { EcothermRimType.EV15_60, EcothermRimType.EV15_120, EcothermRimType.EV15_180, EcothermRimType.EV10_55, EcothermRimType.EV10_110, EcothermRimType.EV10_165, EcothermRimType.EV5_40, EcothermRimType.EV5_80, EcothermRimType.EV5_120 });
				teilungen.Add(EcothermLayDistance.EV30, new Nullable<EcothermRimType>[] { EcothermRimType.EV15_60, EcothermRimType.EV15_120, EcothermRimType.EV15_180, EcothermRimType.EV10_55, EcothermRimType.EV10_110, EcothermRimType.EV10_165, EcothermRimType.EV5_40, EcothermRimType.EV5_80, EcothermRimType.EV5_120 });
				teilungen.Add(EcothermLayDistance.EV25, new Nullable<EcothermRimType>[] { EcothermRimType.EV15_60, EcothermRimType.EV15_120, EcothermRimType.EV15_180, EcothermRimType.EV10_55, EcothermRimType.EV10_110, EcothermRimType.EV10_165, EcothermRimType.EV5_40, EcothermRimType.EV5_80, EcothermRimType.EV5_120 });
				teilungen.Add(EcothermLayDistance.EV20, new Nullable<EcothermRimType>[] { EcothermRimType.EV10_55, EcothermRimType.EV10_110, EcothermRimType.EV10_165, EcothermRimType.EV5_40, EcothermRimType.EV5_80, EcothermRimType.EV5_120 });
				teilungen.Add(EcothermLayDistance.EV15, new Nullable<EcothermRimType>[] { EcothermRimType.EV5_40, EcothermRimType.EV5_80, EcothermRimType.EV5_120 });
				teilungen.Add(EcothermLayDistance.EV10, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV5, new Nullable<EcothermRimType>[] { null });
			} else {
				teilungen.Add(EcothermLayDistance.NONE, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV35, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV30, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV25, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV20, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV15, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV10, new Nullable<EcothermRimType>[] { null });
				teilungen.Add(EcothermLayDistance.EV5, new Nullable<EcothermRimType>[] { null });
			}
			if (this.requestedLayDistance != null) {
				EcothermLayDistance[] distances = new EcothermLayDistance[teilungen.Keys.Count];
				teilungen.Keys.CopyTo(distances, 0);
				foreach (EcothermLayDistance distance in distances) {
					if (distance != this.requestedLayDistance) {
						teilungen.Remove(distance);
					}
				}
			}
			if (this.requestedRimType != null) {
				EcothermLayDistance[] distances = new EcothermLayDistance[teilungen.Keys.Count];
				teilungen.Keys.CopyTo(distances, 0);
				foreach (EcothermLayDistance distance in distances) {
					teilungen[distance] = new Nullable<EcothermRimType>[] { this.requestedRimType };
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

			Nullable<EcothermLayDistance> bestLaydistance = null;
			Nullable<EcothermRimType> bestRimType = null;
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

			this.CalculateHeatAndCoolFlow();
			foreach (EcothermLayDistance ld in teilungen.Keys) {
				foreach (Nullable<EcothermRimType> rt in teilungen[ld]) {
					bool tryCalc = true;
					int circuitCount = 1;
					if (this.requestedCircuits.HasValue) {
						circuitCount = this.requestedCircuits.Value;
					} else {
						double pl = (this.plannedArea - this.plannedAreaUnheated - areaRemovedDueConnection - this.PlannedAreaRim) * EcothermProduct.GetPipeLengthPerSqm(ld);
						if (rt.HasValue) {
							pl += this.PlannedAreaRim * EcothermProduct.GetPipeLengthPerSqm(EcothermProduct.GetRimLayDistance(rt.Value));
						} else {
							pl += this.PlannedAreaRim * EcothermProduct.GetPipeLengthPerSqm(ld);
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
							/*if (this.requestedCircuits.HasValue) {
								// TODO reset circuits
							} else {*/
								//this.circuits.Clear();
							/*}*/
							this.incompleteCalculation = true;
							return false;
						}
						int i = 0;
						foreach (EcothermCircuit ec in this.circuits) {
							ec.EcothermProduct = this;
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
						if (this.PlannedDeltaRhoHeat > EcothermProduct.ConfigMaxPressureLost / 100.0) {
							tryCalc = true;
						}
						if (this.PlannedDeltaRhoCool > EcothermProduct.ConfigMaxPressureLost / 100.0) {
							tryCalc = true;
						}
						if (this.PlannedMaxMhHeat > EcothermProduct.ConfigMaxMassenstrom) {
							tryCalc = true;
						}
						if (this.PlannedMaxMhCool > EcothermProduct.ConfigMaxMassenstrom) {
							tryCalc = true;
						}
						tryCalc = tryCalc && !this.requestedCircuits.HasValue;
						tryCalc = tryCalc && circuitCount < 12;
						tryCalc = tryCalc && this.PlannedConnectedProducts.Count == 0;
						if (tryCalc) {
							circuitCount++;
						}
					}
					bool useNew = !bestLaydistance.HasValue || bestLaydistance.Value == EcothermLayDistance.NONE ||
						this.CompareParameters(bestFloorTempRimHeat, bestFloorTempResidenceHeat, bestHeatLoad, bestPressureLossHeat,
							bestFloorTempRimCool, bestFloorTempResidenceCool, bestCoolLoad, bestPressureLossCool,
							bestPipeLength,
							this.PlannedFloorTemperatureHeatRim, this.PlannedFloorTemperatureHeatResidence, this.PlannedHeatLoad, this.PlannedDeltaRhoHeat,
							this.PlannedFloorTemperatureCoolRim, this.PlannedFloorTemperatureCoolResidence, this.PlannedCoolLoad, this.PlannedDeltaRhoCool,
							this.PlannedPipeLengthPerCircuit,
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
					}
				}
			}

			if (!bestLaydistance.HasValue) {
				/*if (this.requestedCircuits.HasValue) {
					// TODO reset circuits
				} else {*/
					this.circuits.Clear();
				/*}*/
					this.lastErrorMsg = "Keine Automatische Auslegung möglich";
				this.incompleteCalculation = true;
				return false;
			}

			{ // calculate best choice again
				this.plannedLayDistance = bestLaydistance;
				this.plannedRimType = bestRimType;
				this.CorrectCircuits(bestCircuits, true);

				int i = 0;
				foreach (EcothermCircuit ec in this.circuits) {
					ec.EcothermProduct = this;
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
					while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < EcothermProduct.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad && this.PlannedSpreizungHeat < 1.2 * defSpreizungHeat) {
						this.plannedRuecklaufTempHeat -= 0.1;
						foreach (EcothermCircuit ec in this.circuits) {
							ec.Calculate(bestLaydistance.Value, bestRimType);
						}
					}
					this.plannedRuecklaufTempHeat += 0.1;
					// Heizleistung erhöhen
					while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > EcothermProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < EcothermProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhHeat < EcothermProduct.ConfigMaxMassenstrom && this.PlannedSpreizungHeat > 0.8 * defSpreizungHeat) {
						this.plannedRuecklaufTempHeat += 0.1;
						foreach (EcothermCircuit ec in this.circuits) {
							ec.Calculate(bestLaydistance.Value, bestRimType);
						}
					}
					// Kühlleistung verringern
					while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < EcothermProduct.ConfigSpreizungKuehlMax && this.PlannedCoolLoad > requestedCoolLoad && this.PlannedSpreizungCool < 1.2 * defSpreizungCool) {
						this.plannedRuecklaufTempCool += 0.1;
						i = 0;
						foreach (EcothermCircuit ec in this.circuits) {
							ec.Calculate(bestLaydistance.Value, bestRimType);
						}
					}
					this.plannedRuecklaufTempCool -= 0.1;
					// Kühlleistung erhöhen
					while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > EcothermProduct.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < EcothermProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhCool < EcothermProduct.ConfigMaxMassenstrom && this.PlannedSpreizungCool > 0.8 * defSpreizungCool) {
						this.plannedRuecklaufTempCool -= 0.1;
						i = 0;
						foreach (EcothermCircuit ec in this.circuits) {
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

			if (this.plannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				if (!this.secondConfig) {
					this.secondConfig = true;
					this.plannedConnection.OtherProduct.ConfigureProductDefault();
					bool ok = this.ConfigureProduct(requestedHeatLoad, requestedCoolLoad, canHeat, canCool, false);
					this.secondConfig = false;
					this.incompleteCalculation = !ok;
					return ok;
				}
			}

			this.lastErrorMsg = "";
			if (this.LongestPipeLengthPerCircuitWithAllConnections > EcothermProduct.ConfigMaxCircuitLength) {
				this.lastErrorMsg += "Rohrlänge zu groß (" + Math.Round(this.PipeLengthWithoutConnectionsOfLongestPipeWithConnections, 1) + "m > " + Math.Round(EcothermProduct.ConfigMaxCircuitLength - this.ConnectionLengthOfLongestPipeWithConnections, 1) + "m)\n";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeatResidence, 1) > (EcothermProduct.ConfigUseHarreitherNorm ? EcothermProduct.ConfigMaxResidenceTempHarreither : EcothermProduct.ConfigMaxResidenceTempEn1264)) {
				this.lastErrorMsg += "Oberflächentemperatur in der Aufenthaltszone zu groß (" + Math.Round(this.PlannedFloorTemperatureHeatResidence, 1) + "°C > " + Math.Round((EcothermProduct.ConfigUseHarreitherNorm ? EcothermProduct.ConfigMaxResidenceTempHarreither : EcothermProduct.ConfigMaxResidenceTempEn1264), 1) + "°C)\n";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeatRim, 1) > (EcothermProduct.ConfigUseHarreitherNorm ? EcothermProduct.ConfigMaxRimTempHarreither : EcothermProduct.ConfigMaxRimTempEn1264)) {
				this.lastErrorMsg += "Oberflächentemperatur in der Randzone zu groß (" + Math.Round(this.PlannedFloorTemperatureHeatRim, 1) + "°C > " + Math.Round((EcothermProduct.ConfigUseHarreitherNorm ? EcothermProduct.ConfigMaxRimTempHarreither : EcothermProduct.ConfigMaxRimTempEn1264), 1) + "°C)\n";
			}
			if (this.PlannedMaxMhHeat >= this.PlannedMaxMhCool) {
				if (Math.Round(this.PlannedMaxMhHeat, 1) > EcothermProduct.ConfigMaxMassenstrom) {
					this.lastErrorMsg += "Durchfluß bei Heizung zu groß (" + Math.Round(this.PlannedMaxMhHeat, 1).ToString() + "kg/h > " + EcothermProduct.ConfigMaxMassenstrom.ToString() + "kg/h)\n";
				}
			} else {
				if (Math.Round(this.PlannedMaxMhCool, 1) > EcothermProduct.ConfigMaxMassenstrom) {
					this.lastErrorMsg += "Durchfluß bei Kühlung zu groß (" + Math.Round(this.PlannedMaxMhCool, 1).ToString() + "kg/h > " + EcothermProduct.ConfigMaxMassenstrom.ToString() + "kg/h)\n";
				}
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > Math.Round(EcothermProduct.ConfigMaxPressureLost/ 100.0, 2)) {
					this.lastErrorMsg += "Druckverlust bei Heizung zu groß (" + Math.Round(this.PlannedDeltaRhoHeat, 2).ToString() + "mbar > " + Math.Round(EcothermProduct.ConfigMaxPressureLost / 100.0, 2).ToString() + "mbar)\n";
				}
			} else {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > Math.Round(EcothermProduct.ConfigMaxPressureLost / 100.0, 2)) {
					this.lastErrorMsg += "Druckverlust bei Kühlung zu groß (" + Math.Round(this.PlannedDeltaRhoCool, 2).ToString() + "mbar > " + Math.Round(EcothermProduct.ConfigMaxPressureLost / 100.0, 2).ToString() + "mbar)\n";
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
				if (kvp.Key >= circuitCount && !kvp.Value.UserDefined) {
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
				EcothermCircuit ec = new EcothermCircuit();
				ec.EcothermProduct = this;
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
					this.plannedConnection.OtherProduct.Product.ConnectedCircuits.Add(j, new Circuit.CircuitConnection(this.plannedConnection.CircuitConnectionType, ec, false));
					this.inverseConnectedCircuits.Add(ec.NrOfCircuit, new Circuit.CircuitConnection(this.plannedConnection.CircuitConnectionType, this.plannedConnection.OtherProduct.Product.PlannedCircuits[j], false));
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
							this.inverseConnectedCircuits.Add(i, new Circuit.CircuitConnection(this.plannedConnection.CircuitConnectionType, c, false));
							this.plannedConnection.OtherProduct.Product.ConnectedCircuits.Add(c.NrOfCircuit, new Circuit.CircuitConnection(this.plannedConnection.CircuitConnectionType, this.circuits[i], false));
						}
					}
				}
			}
			return null;
		}

		[XmlIgnore]
		public override double WasserInhalt {
			get {
				double length = 0;
				foreach (EcothermCircuit c in this.circuits) {
					length += c.PipeLengthWithoutOtherProduct;
				}
				return rohrInnenA * length * 1000;
			}
		}

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {
			
			double connectionPipeArea = 0;
			foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
				connectionPipeArea += pipe.AreaTotal;
			}

			double totalArea = this.PlannedAreaResidence + this.PlannedAreaRim + connectionPipeArea;
			
			// Ecotherm Rohr
			double length = 0;
			foreach (EcothermCircuit c in this.circuits) {
				length += c.PipeLengthWithoutOtherProduct;
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, "EC01", length);

			// Clipschiene
			string clipschiene = "EC02";
			double amount = 0;
			if (this.PlannedLayDistance.HasValue) {
			    amount += this.PlannedAreaResidence * GetClipschienePerSqm(this.PlannedLayDistance.Value, anhydritEstrich);
			}
			if (this.PlannedRimType.HasValue) {
			    amount += this.PlannedAreaRim * GetClipschienePerSqm(GetRimLayDistance(this.PlannedRimType.Value), anhydritEstrich);
			}
			foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
				if (pipe.Verlegeart != ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH) {
					amount += pipe.AreaTotal * GetClipschienePerSqm(ConnectionPipe.GetEcothermLayDistance(pipe.Verlegeart), anhydritEstrich);
				}
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, clipschiene, amount);

			// Muffe
			amount = 0;
			if (this.PlannedLayDistance.HasValue) {
				amount += this.PlannedAreaResidence * GetMuffePerSqm(this.PlannedLayDistance.Value);
			}
			if (this.PlannedRimType.HasValue) {
				amount += this.PlannedAreaRim * GetMuffePerSqm(GetRimLayDistance(this.PlannedRimType.Value));
			}
			foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
				if (pipe.Verlegeart != ConnectionPipe.VerlegeartEnum.VA_UNTER_ESTRICH) {
					amount += pipe.AreaTotal * GetMuffePerSqm(ConnectionPipe.GetEcothermLayDistance(pipe.Verlegeart));
				}
			}
			Project.Instance.AddRequiredMaterial(requiredMaterial, "EC06", amount);

			//Verteileranschlußbögen
			if (this.PlannedConnection != null && this.PlannedConnection.Distributor != null) {
				string verteilerAnschluß = this.PlannedConnection.Distributor.LangeAnschlussboegen ? "EC05" : "EC04";

				Project.Instance.AddRequiredMaterial(requiredMaterial, verteilerAnschluß, this.circuits.Count * 2);
			}

			// nur bei Estrichkonstruktion
			if (this.HasInsideConstruction) {
				if (this.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH) ||
					this.PlannedInsideConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH)) {

					// EZ 70
					if (!anhydritEstrich) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "EC08", (totalArea + this.PlannedAreaUnheated) * 0.2);
					}
				}
			}

			// Dämmung
			if (this.HasOutsideConstruction) {
				foreach (ConstructionLayer layer in this.PlannedOutsideConstruction.Layers) {
					if (layer.LayerMaterial != null) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, layer.LayerMaterial.Id, this.PlannedFloorArea + this.PlannedAreaUnheated);
					}
				}
			}

		}

		public override double Dichte {
			get { return EcothermProduct.ConfigRho; }
		}

		public override double Waermekapazitaet {
			get { return EcothermProduct.ConfigC; }
		}

		public override double Viskositaet {
			get { return EcothermProduct.ConfigV; }
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
/*		public bool PlannedCorrections {
			get { return this.plannedCorrections; }
			set {
				if (this.plannedCorrections != value) {
					this.plannedCorrections = value;
					if (this.plannedCorrections) {
						if (this.requestedCircuits == null || this.requestedLayDistance == null || (this.requestedRimType == null && this.plannedRimLength > 0)) {
							this.PlannedCorrections = false;
						} else {
							this.plannedCorrectionList = new List<ExtendedCorrections>();
							for (int i = 0; i < this.requestedCircuits.Value; i++ ) {
								this.plannedCorrectionList.Add(new ExtendedCorrections(i + 1, this));
							}
						}
					} else {
						this.plannedCorrectionList = null;
					}
				}
			}
		}*/

		public List<ExtendedCorrections> PlannedCorrectionList {
			get { return this.plannedCorrectionList; }
			set {
				//this.plannedCorrections = (value != null && value.Count > 0);
				this.plannedCorrectionList = (value == null) ? new List<ExtendedCorrections>() : value;
			}
		}

		internal override void FinalizeLoading(PlannedProduct pp) {
			base.FinalizeLoading(pp);
			if (this.PlannedCorrections) {
				int i = 1;
				foreach (ExtendedCorrections ec in this.PlannedCorrectionList) {
					ec.EcothermProduct = this;
					ec.CircuitNr = i++;
				}
				foreach (EcothermCircuit ec in this.circuits) {
					ec.EcothermProduct = this;
				}
			}
		}

		public override bool ManualMode {
			get {
				return (this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT); // ||
					//this.PlannedCorrections;
			}
		}

	}
}
