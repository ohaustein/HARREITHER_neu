using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Euroval® Fußbodenheizung")]
	public class EurovalProduct : Product {

		private static double su0 = 0.045; /* Mindestüberdeckung fix??? */
		private static double alpha0 = 10.8; /* Fixwert für FBH fix??? */
		private static double alphaFbk = 6.5; //6.5; /* für FBK fix??? */
		private static double alphaFbh = 10.8; /* für FBH fix??? */
		private static double lambdaR0 = 0.35; /* fix ??? */
		private static double lambdaR = 0.22; /* für PP Rohr laut Tabelle A.13 fix??? */
		private static double lambdaU0 = 1; /* fix??? */
		private static double lambdaE = 1.2; /* Estrichleitfähigkeit, fix */
		private static double rohrAussenD = 0.0206505; /* Aussendurchmesser Euroval Rohr */
		private static double rohrInnenD = 0.0153; /* Rohrinnendurchmesser */
		private static double rohrInnenA = 0.000183783; /* Rohrinnenquerschnitt */
		private static double ag = 1.1034; /* Ovalrohr Geometriefaktor für Euroval */
		private static double sr0 = 0.002; /* fix ??? */
		private static double sr = 0.00238; /* Aus Euroval Normprüfdaten */
		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static double v = 0.00000101; /* m²/s ... kinematische Viskosität */
		private static bool agActivated = true;
		private static double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
		private static double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */

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
		private static double spreizungKühlMin = 2;
		private static double spreizungKühlMax = 6;

		protected float plannedArea = 0;
		private float plannedAreaReduced = 0;
		private float plannedAreaUnheated = 0;
		private float plannedRimLength = 0;
		private int plannedRimCorners = 0;
		private float plannedRoomTemperatureBelowHeat = 18;
		private float plannedRoomTemperatureBelowCool = 22;
		private Construction plannedFloorConstruction = null;
		private Construction plannedInsulationConstruction = null;

		private Nullable<LayDistance> requestedLayDistance = null;
		private Nullable<RimType> requestedRimType = null;
		private Nullable<int> requestedCircuits = null;

		private Nullable<LayDistance> plannedLayDistance = null;
		private Nullable<RimType> plannedRimType = null;

		private bool plannedProductIsConnection = false;

		private bool plannedCorrections = false;

		private List<EurovalCircuit> circuits = new List<EurovalCircuit>();

		public override Circuit GetCircuit(int index) {
			if (index < this.circuits.Count) {
				return this.circuits[index];
			}
			return null;
		}

		private bool clipSchiene = false;
		private bool anhydritEstrich = false;

		public enum LayDistance {
			A5 = 0,
			EV5 = 1,
			EV10 = 2,
			EV15 = 3,
			EV20 = 4,
			EV25 = 5,
			EV30 = 6,
			EV35 = 7
		}

		public enum RimType {
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

		}

		protected EurovalProduct(EurovalProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 50;
			quickDimensioningCoolPowerPerSquareMeter = 50;
			canHeat = true;
			canCool = false;
		}

		public override void StaticInitialize() {
			useHarreitherNorm = true;
			maxCircuitLength = 100.0;
			maxPressureLost = 15000;
			maxDurchfluss = 240;
			spreizungHeizMin = 4;
			spreizungHeizMax = 12;
			spreizungKühlMin = 2;
			spreizungKühlMax = 6;
		}

		public override Product Clone(Room room) {
			EurovalProduct product = new EurovalProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		#region Product Parameters
		[ProductParameter]
		public static double ConfigSu0 {
			get { return su0; }
			set { su0 = value; }
		}

		[ProductParameter]
		public static double ConfigAlpha0 {
			get { return EurovalProduct.alpha0; }
			set { EurovalProduct.alpha0 = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaFbk {
			get { return EurovalProduct.alphaFbk; }
			set { EurovalProduct.alphaFbk = value; }
		}

		[ProductParameter]
		public static double ConfigAlphaFbh {
			get { return EurovalProduct.alphaFbh; }
			set { EurovalProduct.alphaFbh = value; }
		}

		[ProductParameter]
		public static double ConfigLambdaR0 {
			get { return EurovalProduct.lambdaR0; }
			set { EurovalProduct.lambdaR0 = value; }
		}

		[ProductParameter]
		public static double ConfigLambdaR {
			get { return EurovalProduct.lambdaR; }
			set { EurovalProduct.lambdaR = value; }
		}

		[ProductParameter]
		public static double ConfigLambdaU0 {
			get { return EurovalProduct.lambdaU0; }
			set { EurovalProduct.lambdaU0 = value; }
		}

		[ProductParameter]
		public static double ConfigLambdaE {
			get { return EurovalProduct.lambdaE; }
			set { EurovalProduct.lambdaE = value; }
		}

		[ProductParameter]
		public static double ConfigRohrAussenD {
			get { return EurovalProduct.rohrAussenD; }
			set { EurovalProduct.rohrAussenD = value; }
		}

		[ProductParameter]
		public static double ConfigRohrInnenD {
			get { return EurovalProduct.rohrInnenD; }
			set { EurovalProduct.rohrInnenD = value; }
		}

		[ProductParameter]
		public static double ConfigRohrInnenA {
			get { return EurovalProduct.rohrInnenA; }
			set { EurovalProduct.rohrInnenA = value; }
		}

		[ProductParameter]
		public static double ConfigAg {
			get { return EurovalProduct.ag; }
			set { EurovalProduct.ag = value; }
		}

		[ProductParameter]
		public static double ConfigSr0 {
			get { return EurovalProduct.sr0; }
			set { EurovalProduct.sr0 = value; }
		}

		[ProductParameter]
		public static double ConfigSr {
			get { return EurovalProduct.sr; }
			set { EurovalProduct.sr = value; }
		}

		[ProductParameter]
		public static double ConfigC {
			get { return EurovalProduct.c; }
			set { EurovalProduct.c = value; }
		}

		[ProductParameter]
		public static double ConfigRho {
			get { return EurovalProduct.rho; }
			set { EurovalProduct.rho = value; }
		}

		[ProductParameter]
		public static double ConfigV {
			get { return EurovalProduct.v; }
			set { EurovalProduct.v = value; }
		}

		[ProductParameter]
		public static bool ConfigAgActivated {
			get { return EurovalProduct.agActivated; }
			set { EurovalProduct.agActivated = value; }
		}

		[ProductParameter]
		public static double ConfigRLambdaDecke {
			get { return EurovalProduct.rLambdaDecke; }
			set { EurovalProduct.rLambdaDecke = value; }
		}

		[ProductParameter]
		public static double ConfigRLambdaPutz {
			get { return EurovalProduct.rLambdaPutz; }
			set { EurovalProduct.rLambdaPutz = value; }
		}

		[ProductParameter]
		public static double ConfigMaxResidenceTempHarreither {
			get { return EurovalProduct.maxResidenceTempHarreither; }
			set { EurovalProduct.maxResidenceTempHarreither = value; }
		}

		[ProductParameter]
		public static double ConfigMaxRimTempHarreither {
			get { return EurovalProduct.maxRimTempHarreither; }
			set { EurovalProduct.maxRimTempHarreither = value; }
		}

		[ProductParameter]
		public static double ConfigMaxResidenceTempEn1264 {
			get { return EurovalProduct.maxResidenceTempEn1264; }
			set { EurovalProduct.maxResidenceTempEn1264 = value; }
		}

		[ProductParameter]
		public static double ConfigMaxRimTempEn1264 {
			get { return EurovalProduct.maxRimTempEn1264; }
			set { EurovalProduct.maxRimTempEn1264 = value; }
		}

		[ProductParameter]
		public static bool ConfigUseHarreitherNorm {
			get { return EurovalProduct.useHarreitherNorm; }
			set { EurovalProduct.useHarreitherNorm = value; }
		}

		[ProductParameter]
		public static double ConfigMaxCircuitLength {
			get { return EurovalProduct.maxCircuitLength; }
			set { EurovalProduct.maxCircuitLength = value; }
		}

		[ProductParameter]
		public static int ConfigMaxPressureLost {
			get { return EurovalProduct.maxPressureLost; }
			set { EurovalProduct.maxPressureLost = value; }
		}

		[ProductParameter]
		public static int ConfigMaxDurchfluss {
			get { return EurovalProduct.maxDurchfluss; }
			set { EurovalProduct.maxDurchfluss = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungHeizMin {
			get { return EurovalProduct.spreizungHeizMin; }
			set { EurovalProduct.spreizungHeizMin = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungHeizMax {
			get { return EurovalProduct.spreizungHeizMax; }
			set { EurovalProduct.spreizungHeizMax = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungKühlMin {
			get { return EurovalProduct.spreizungKühlMin; }
			set { EurovalProduct.spreizungKühlMin = value; }
		}

		[ProductParameter]
		public static double ConfigSpreizungKühlMax {
			get { return EurovalProduct.spreizungKühlMax; }
			set { EurovalProduct.spreizungKühlMax = value; }
		}
		#endregion Product Parameters

		/// <summary>
		/// Returns the default number of circuit for the planned area (for quick dimensioning)
		/// </summary>
		public override int GetDefaultQuickDimensioningCircuits() {
			LayDistance distance = Project.Instance.QuickDimensioning.LayDistance;
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
		public static double GetPipeLengthPerSqm(LayDistance distance) {
			switch (distance) {
				case LayDistance.A5:
					return 20;
				case LayDistance.EV5:
					return 10;
				case LayDistance.EV10:
					return 7.5;
				case LayDistance.EV15:
					return 6.7;
				case LayDistance.EV20:
					return 5;
				case LayDistance.EV25:
					return 4;
				case LayDistance.EV30:
					return 3.5;
				case LayDistance.EV35:
					return 3;
				default:
					throw new Exception("Unknown Laydistance");
			}
		}

		/// <summary>
		/// Returns distance between two pipes in m for specified laydistance 
		/// </summary>
		public static double GetTeilung(LayDistance distance) {
			switch (distance) {
				case LayDistance.A5:
					return 0.05;
				case LayDistance.EV5:
					return 0.1;
				case LayDistance.EV10:
					return 0.125;
				case LayDistance.EV15:
					return 0.15;
				case LayDistance.EV20:
					return 0.2;
				case LayDistance.EV25:
					return 0.25;
				case LayDistance.EV30:
					return 0.3;
				case LayDistance.EV35:
					return 0.35;
				default:
					throw new Exception("Unknwon LayDistance");
			}
		}

		/// <summary>
		/// Returns the laydistance for the specified rimtype
		/// </summary>
		public static LayDistance GetRimLayDistance(RimType rimType) {
			switch (rimType) {
				case RimType.EV15_60:
				case RimType.EV15_120:
				case RimType.EV15_180:
					return LayDistance.EV15;
				case RimType.EV10_55:
				case RimType.EV10_110:
				case RimType.EV10_165:
					return LayDistance.EV10;
				case RimType.EV5_40:
				case RimType.EV5_80:
				case RimType.EV5_120:
					return LayDistance.EV5;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		/// <summary>
		/// Returns the width of the rim in cm for the specified rimtype
		/// </summary>
		public static int GetRimWidth(RimType rimType) {
			switch (rimType) {
				case RimType.EV5_40:
					return 40;
				case RimType.EV10_55:
					return 55;
				case RimType.EV15_60:
					return 60;
				case RimType.EV5_80:
					return 80;
				case RimType.EV10_110:
					return 110;
				case RimType.EV15_120:
				case RimType.EV5_120:
					return 120;
				case RimType.EV10_165:
					return 165;
				case RimType.EV15_180:
					return 180;
				default:
					throw new Exception("Unknwon RimType");
			}
		}

		public bool UseClipSchiene {
			get { return clipSchiene; }
			set { clipSchiene = value; }
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
			get { return "Euroval®\n(m²)"; }
		}
		#endregion QuickDimensioning

		/// <summary>
		/// The name of this product
		/// </summary>
		public override string Name {
			get { return "Euroval®"; }
		}

		/// <summary>
		/// The full name of this product
		/// </summary>
		public override string FullName {
			get { return "Euroval® Fußbodenheizung"; }
		}

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
		public Nullable<LayDistance> RequestedLayDistance {
			get { return this.requestedLayDistance; }
			set { this.requestedLayDistance = value; }
		}

		/// <summary>
		/// The rim type the user requested for this product in the planning.
		/// If this property is null the optimal rim type will be calculated.
		/// </summary>
		public Nullable<RimType> RequestedRimType {
			get { return this.requestedRimType; }
			set { this.requestedRimType = value; }
		}

		/// <summary>
		/// The number of circuits the user requested for this product in the planning.
		/// If this property is null the optimal number of circuits will be calculated.
		/// </summary>
		public Nullable<int> RequestedCircuits {
			get { return this.requestedCircuits; }
			set { this.requestedCircuits = value; }
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
		/// The temperature of the room below used for the heating calcuation
		/// </summary>
		public float PlannedRoomTemperatureBelowHeat {
			get { return this.plannedRoomTemperatureBelowHeat; }
			set { this.plannedRoomTemperatureBelowHeat = value; }
		}

		/// <summary>
		/// The temperature of the room below used for the cooling calculation
		/// </summary>
		public float PlannedRoomTemperatureBelowCool {
			get { return this.plannedRoomTemperatureBelowCool; }
			set { this.plannedRoomTemperatureBelowCool = value; }
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
					float realRimLength = this.plannedRimLength - rimWidth * this.plannedRimCorners;
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
			/*get { return (this.AvailableFloorArea <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AvailableFloorArea); }
			set { this.PlannedFloorArea = (float)(this.AvailableFloorArea * value / 100); }*/
			get { return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AssociatedRoom.Area); }
			set { this.PlannedFloorArea = (float)(this.AssociatedRoom.Area * value / 100); }
		}

		public bool PlannedProductIsConnection {
			get { return this.plannedProductIsConnection; }
			set { this.plannedProductIsConnection = value; }
		}
		#endregion Auslegung

		#region Auslegung calculated values
		/// <summary>
		/// The lay distance used for calculation. This is either the lay distance the user requested
		/// or if the user did not request any specific lay distance the optimal lay distance is
		/// calculated.
		/// </summary>
		[XmlIgnore]
		public Nullable<LayDistance> PlannedLayDistance {
			get { return this.plannedLayDistance; }
			set { this.plannedLayDistance = value; }
		}

		/// <summary>
		/// The rim type used for calculation. This is either the rim type the user requested
		/// or if the user did not request any specific rim type the optimal rim type is
		/// calculated.
		/// </summary>
		[XmlIgnore]
		public Nullable<RimType> PlannedRimType {
			get { return this.plannedRimType; }
			set { this.plannedRimType = value; }
		}

		/// <summary>
		/// The lay distance of the rim type used for calculation.
		/// </summary>
		[XmlIgnore]
		public Nullable<LayDistance> PlannedRimLayDistance {
			get { return this.plannedRimType == null ? (Nullable<LayDistance>)null : (Nullable<LayDistance>)GetRimLayDistance(this.plannedRimType.Value); }
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
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					value += ec.QAzHeat;
				}
				return value;
			}
		}

		/// <summary>
		/// The pressure loss for heating, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedDeltaRhoHeat {
			get {
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_DruckverlustHeat > value) {
						value = ec.C_DruckverlustHeat;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedMhHeat {
			get {
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_DurchflussHeat > value) {
						value = ec.C_DurchflussHeat;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedSpreizungHeat {
			get {
				return (this.PlannedConnection == null || this.PlannedConnection.Distributor == null || this.PlannedConnection.Distributor.RegulatorCircuit == null) ? 0 : EN1264.Instance.DefaultSpreizung(this.PlannedConnection.Distributor.RegulatorCircuit.HeatFlowTemperature);
			}
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeatRim {
			get {
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
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					value += ec.QAzCool;
				}
				return value;
			}
		}

		/// <summary>
		/// The pressure loss for cooling, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedDeltaRhoCool {
			get {
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_DruckverlustCool > value) {
						value = ec.C_DruckverlustCool;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedMhCool {
			get {
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_DurchflussCool > value) {
						value = ec.C_DurchflussCool;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedSpreizungCool {
			get {
				return 3; // TODO
			}
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureCoolRim {
			get {
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
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
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.C_FloorTempAzCool > value) {
						value = ec.C_FloorTempAzCool;
					}
				}
				return value;
			}
		}
		#endregion Cool Load

		[XmlIgnore]
		public double PlannedRemoveArea {
			get {
				double value = 0;
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
								if (cp.ConnectionThrough != null && cp.ConnectionThrough.Product != null && cp.ConnectionThrough.Product == this) {
									value += cp.AreaTotal;
								}
							}
						}
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PlannedPipeLengthPerCircuit {
			get {
				double value = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					if (ec.PipeLengthWithoutConnections > value) {
						value = ec.PipeLengthWithoutConnections;
					}
				}
				return value;
				//return this.plannedPipeLength / this.PlannedCircuits; 
			}
		}

		public override int PlannedCircuits {
			get { return this.circuits.Count; }
		}
		#endregion Auslegung calculated values

		/// <summary>
		/// Calculates parameters for the given laydistance and rimtype and sets the following fields:
		/// - plannedLayDistance
		/// - plannedRimType
		/// - plannedPipeLength
		/// - plannedQHeat
		/// - plannedQHeatRim
		/// - plannedQHeatResidence
		/// - plannedQHeatU
		/// - plannedDeltaRhoHeat
		/// - plannedQCool
		/// - plannedQCoolRim
		/// - plannedQCoolResidence
		/// - plannedQCoolU
		/// - plannedDeltaRhoCool
		/// </summary>
		/// <param name="distance"></param>
		/// <param name="distanceRim"></param>
		//private void CalculateQForLayDistance(LayDistance distance, Nullable<RimType> distanceRim, Nullable<int> circuits,
		//        double vorlaufTotal, double ruecklaufTotal, double vorlaufNotIsolated, double ruecklaufNotIsolated) {
		//    if (this.plannedArea == 0) {
		//        return;
		//    }

		//    EN1264 en1264 = EN1264.Instance;

		//    /*// Rohrlänge Anbindeleitungen
		//    double vorlaufTotal = 0;
		//    double ruecklaufTotal = 0;
		//    double vorlaufNotIsolated = 0;
		//    double ruecklaufNotIsolated = 0;
		//    foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
		//        vorlaufNotIsolated += (pipe.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) ? pipe.Vorlauf : 0;
		//        ruecklaufNotIsolated += (pipe.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) ? pipe.Ruecklauf : 0;
		//        vorlaufTotal += pipe.Vorlauf;
		//        ruecklaufTotal += pipe.Ruecklauf;
		//    }*/

		//    double su = 0.035; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
		//    double lambdaU = 1.2; /* Estrich??? */
		//    double rLambdaB = this.plannedFloorConstruction == null ? 0 : this.plannedFloorConstruction.RValue;  //0.1; /* Annahme Parkett mit 0.1 m²K/W; durch echte Konstruktion ersetzen! */
		//    double rLambdaIns = this.plannedInsulationConstruction == null ? 0 : this.plannedInsulationConstruction.RValue;
		//    double rAlphaDeckeFbh = 1 / alphaFbh; /* Wärmeübergang Decke bei Heizung */
		//    double rAlphaDeckeFbk = 1 / alphaFbk; /* Wärmeübergang Decke bei Kühlung */

		//    this.plannedLayDistance = distance;
		//    this.plannedRimType = distanceRim;

		//    // Aufteilung RZ - AZ
		//    double aGes = this.PlannedFloorArea - this.plannedRemoveArea;	// gesamte Fläche
		//    double aRed = this.PlannedAreaReduced;	// Fläche mit reduzierter Heizleistung
		//    double aUnb = this.PlannedAreaUnheated;	// unbeheizte Fläche
		//    double aFbh = aGes - aRed / 2 - aUnb;	// wirksam beheizte Fläche

		//    bool calculateWithRim = distanceRim.HasValue && (this.plannedRimLength - this.plannedRimCorners * GetRimWidth(distanceRim.Value) / 100 > 0);
		//    double lRz = 0;
		//    double bRz = 0;
		//    double aRz = 0;
		//    double lRlRz = 0;
		//    if (calculateWithRim) {
		//        lRz = this.plannedRimLength - this.plannedRimCorners * bRz;			      // Tatsächliche länge der Randzone berechnen
		//        bRz = ((float)GetRimWidth(distanceRim.Value)) / 100;
		//        lRz = lRz < 0 ? 0 : lRz;
		//        aRz = lRz * bRz;                                                          // Fläche der Randzone berechnen
		//        lRlRz = aRz * GetPipeLengthPerSqm(GetRimLayDistance(distanceRim.Value));  // Rohrlänge der Randzone berechnen
		//    }
		//    double aAz = aFbh - aRz;                                                      // Fläche der Aufenthaltszone berechnen
		//    double lRlAz = aAz * GetPipeLengthPerSqm(distance);                           // Rohlänge der Aufenthaltszone berechnen
		//    this.plannedPipeLength = lRlRz + lRlAz;
		//    lRlRz = lRlRz / this.PlannedCircuits;
		//    lRlAz = lRlAz / this.PlannedCircuits;

		//    this.plannedCircuits = circuits.HasValue ? circuits.Value : (this.plannedPipeLength <= 0 ? 1 : (int)Math.Ceiling(this.plannedPipeLength / (maxCircuitLength - vorlaufTotal - ruecklaufTotal)));

		//    { // Heizlastberechnung
		//        double QFbhFirst = 0;
		//        { // 1. Heizkreis
		//            double thetaVrz = 35;
		//            double thetaRaz = 30;
		//            this.GetHeatFlow(out thetaVrz, out thetaRaz);
		//            this.plannedSpreizungHeat = thetaVrz - thetaRaz;
		//            thetaVrz = thetaVrz - (thetaVrz - thetaRaz) * vorlaufNotIsolated / (this.PlannedPipeLengthPerCircuit + vorlaufNotIsolated + ruecklaufNotIsolated);
		//            thetaRaz = thetaRaz + (thetaVrz - thetaRaz) * ruecklaufNotIsolated / (this.PlannedPipeLengthPerCircuit + vorlaufNotIsolated + ruecklaufNotIsolated);
		//            double thetaRrz = thetaVrz;
		//            double thetaVaz = thetaVrz;

		//            double dThetaRz = 0;

		//            if (calculateWithRim) {
		//                thetaRrz = thetaVrz - (thetaVrz - thetaRaz) * lRlRz / this.PlannedPipeLengthPerCircuit;
		//                thetaVaz = thetaRrz;
		//                dThetaRz = en1264.Heizmitteluebertemperatur(thetaVrz, thetaRrz, this.AssociatedRoom.RoomHeatTemperature);
		//                //                                                                        // Heizmittelübertemperatur der Randzone berechnen
		//            }
		//            double dThetaAz = en1264.Heizmitteluebertemperatur(thetaVaz, thetaRaz, this.AssociatedRoom.RoomHeatTemperature);
		//            //                                                                            // Heizmittelübertemperatur der Aufenthaltszone berechnen

		//            double tRz = 0;
		//            double ppRz = 0;
		//            double bgRz = 0;
		//            double khRz = 0;
		//            double qRz = 0;
		//            if (calculateWithRim) {
		//                tRz = EurovalProduct.GetTeilung(GetRimLayDistance(distanceRim.Value));    // Teilung der Randzone
		//                ppRz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, (agActivated ? ag : 1));
		//                //                                                                        // Potenzprodukt der Randzone berechnen
		//                bgRz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, (agActivated ? ag : 1), sr, sr0, lambdaR, lambdaR0);
		//                //                                                                        // systemabhängigen Koeffizienten der Randzone berechnen
		//                khRz = en1264.WaermedurchgangsKoeffizientRohr(bgRz, ppRz);                // Wärmedurchgangskoeffizient der Randzone berechnen
		//                qRz = en1264.WaermestromDichteRohr(khRz, dThetaRz);                       // in den Raum abgegebene Wärmeleistung der Randzone berechnen
		//            }

		//            double tAz = EurovalProduct.GetTeilung(distance);                             // Teilung der Aufenthaltszone
		//            double ppAz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, (agActivated ? ag : 1));
		//            //                                                                            // Potenzprodukt der Aufenthaltszone berechnen
		//            double bgAz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, (agActivated ? ag : 1), sr, sr0, lambdaR, lambdaR0);
		//            //                                                                            // systemabhängigen Koeffizienten der Aufenthaltszone berechnen
		//            double khAz = en1264.WaermedurchgangsKoeffizientRohr(bgAz, ppAz);             // Wärmedurchgangskoeffizient der Aufenthaltszone berechnen
		//            double qAz = en1264.WaermestromDichteRohr(khAz, dThetaAz);                    // in den Raum abgegebene Wärmeleistung der Aufenthaltszone berechnen

		//            QFbhFirst = (aRz * qRz + aAz * qAz) / this.PlannedCircuits;                   // gesamte in den Raum abgegebene Wärme

		//            double qFbhFirst = QFbhFirst / aGes * this.PlannedCircuits;
		//            double qUFirst = en1264.WaermeverlustUnten(alphaFbh, rLambdaB, su, lambdaU, rAlphaDeckeFbh, rLambdaIns, EurovalProduct.rLambdaDecke, EurovalProduct.rLambdaPutz, qFbhFirst, this.AssociatedRoom.RoomHeatTemperature, this.PlannedRoomTemperatureBelowHeat);
		//            //this.plannedHeatLoad = QFbh + this.plannedHeatLoadAnbindung;
		//            //this.plannedQHeat = QFbh / aGes;
		//            //this.plannedQHeatRim = qRz;
		//            //this.plannedQHeatResidence = qAz;
		//            //this.plannedQHeatU = en1264.WaermeverlustUnten(alphaFbh, rLambdaB, su, lambdaU, rAlphaDeckeFbh, rLambdaIns, EurovalProduct.rLambdaDecke, EurovalProduct.rLambdaPutz, this.plannedQHeat, this.AssociatedRoom.RoomHeatTemperature, this.PlannedRoomTemperatureBelowHeat);
		//            //                                                                           // Wärmeverlust nach unten berechnen

		//            // hydraulische Berechnung
		//            double qH2o = (qFbhFirst + qUFirst) * aGes / this.PlannedCircuits;            // gesamte aufgenommene Leistung berechnen
		//            // TODO a
		//            double deltaT = thetaVrz - thetaRaz;                                          // gesamte Spreizung
		//            this.plannedDeltaRhoHeat = en1264.DruckverlustRohr(qH2o, c, deltaT, rohrInnenA, rho, rohrInnenD, v, 0.000004, this.PlannedPipeLengthPerCircuit + vorlaufTotal + ruecklaufTotal);
		//            //                                                                           // gesamten Druckverlust berechnen
		//            this.plannedMhHeat = en1264.Durchfluss(qH2o, EurovalProduct.c, deltaT);
		//        }
		//        //this.plannedHeatLoad = QFbh + this.plannedHeatLoadAnbindung;
		//        //this.plannedQHeat = QFbh / aGes;
		//        //this.plannedQHeatRim = qRz;
		//        //this.plannedQHeatResidence = qAz;
		//        //this.plannedQHeatU = en1264.WaermeverlustUnten(alphaFbh, rLambdaB, su, lambdaU, rAlphaDeckeFbh, rLambdaIns, EurovalProduct.rLambdaDecke, EurovalProduct.rLambdaPutz, this.plannedQHeat, this.AssociatedRoom.RoomHeatTemperature, this.PlannedRoomTemperatureBelowHeat);
		//    }

		//    { // Kühllastberechnung
		//        double thetaVrz = 16;
		//        double thetaRaz = 22;
		//        this.GetCoolFlow(out thetaVrz, out thetaRaz);
		//        double thetaRrz = thetaVrz;
		//        double thetaVaz = thetaVrz;
		//        this.plannedSpreizungCool = thetaRaz - thetaVrz;

		//        double dThetaRz = 0;

		//        if (calculateWithRim) {
		//            thetaRrz = thetaVrz - (thetaVrz - thetaRaz) * lRlRz / this.PlannedPipeLengthPerCircuit;
		//            thetaVaz = thetaRrz;
		//            dThetaRz = en1264.Heizmitteluebertemperatur(thetaVrz, thetaRrz, this.AssociatedRoom.RoomCoolTemperature);
		//            //                                                                        // Heizmittelübertemperatur der Randzone berechnen
		//        }
		//        double dThetaAz = en1264.Heizmitteluebertemperatur(thetaVaz, thetaRaz, this.AssociatedRoom.RoomCoolTemperature);
		//        //                                                                            // Heizmittelübertemperatur der Aufenthaltszone berechnen

		//        double tRz = 0;
		//        double ppRz = 0;
		//        double bgRz = 0;
		//        double khRz = 0;
		//        double qRz = 0;
		//        if (calculateWithRim) {
		//            tRz = EurovalProduct.GetTeilung(GetRimLayDistance(distanceRim.Value));    // Teilung der Randzone
		//            ppRz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, (agActivated ? ag : 1));
		//            //                                                                        // Potenzprodukt der Randzone berechnen
		//            bgRz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, (agActivated ? ag : 1), sr, sr0, lambdaR, lambdaR0);
		//            //                                                                        // systemabhängigen Koeffizienten der Randzone berechnen
		//            khRz = en1264.WaermedurchgangsKoeffizientRohr(bgRz, ppRz);                // Wärmedurchgangskoeffizient der Randzone berechnen
		//            qRz = en1264.WaermestromDichteRohr(khRz, dThetaRz);                       // in den Raum abgegebene Wärmeleistung der Randzone berechnen
		//        }

		//        double tAz = EurovalProduct.GetTeilung(distance);                             // Teilung der Aufenthaltszone
		//        double ppAz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, (agActivated ? ag : 1));
		//        //                                                                            // Potenzprodukt der Aufenthaltszone berechnen
		//        double bgAz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, (agActivated ? ag : 1), sr, sr0, lambdaR, lambdaR0);
		//        //                                                                            // systemabhängigen Koeffizienten der Aufenthaltszone berechnen
		//        double khAz = en1264.WaermedurchgangsKoeffizientRohr(bgAz, ppAz);             // Wärmedurchgangskoeffizient der Aufenthaltszone berechnen
		//        double qAz = en1264.WaermestromDichteRohr(khAz, dThetaAz);                    // in den Raum abgegebene Wärmeleistung der Aufenthaltszone berechnen

		//        double QFbk = aRz * qRz + aAz * qAz;                                          // gesamte in den Raum abgegebene Wärme

		//        this.plannedCoolLoad = -QFbk + this.plannedCoolLoadAnbindung;
		//        this.plannedQCool = -QFbk / aGes;
		//        this.plannedQCoolRim = -qRz;
		//        this.plannedQCoolResidence = -qAz;
		//        this.plannedQCoolU = -en1264.WaermeverlustUnten(alphaFbk, rLambdaB, su, lambdaU, rAlphaDeckeFbk, rLambdaIns, EurovalProduct.rLambdaDecke, EurovalProduct.rLambdaPutz, this.plannedQCool, this.AssociatedRoom.RoomCoolTemperature, this.PlannedRoomTemperatureBelowCool);
		//        //                                                                           // Kühlverlust nach unten berechnen

		//        // hydraulische Berechnung
		//        double qH2o = (-this.plannedQCool - this.plannedQCoolU) * aGes;                                        // gesamte aufgenommene Leistung berechnen
		//        double deltaT = thetaVrz - thetaRaz;                                          // gesamte Spreizung
		//        this.plannedDeltaRhoCool = en1264.DruckverlustRohr(qH2o, c, deltaT, rohrInnenA, rho, rohrInnenD, v, 0.000004, this.PlannedPipeLengthPerCircuit + vorlaufTotal + ruecklaufTotal);
		//        //                                                                            // gesamten Druckverlust berechnen
		//        this.plannedMhCool = en1264.Durchfluss(qH2o, EurovalProduct.c, deltaT);
		//    }
		//}

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

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg) {
			if (this.plannedFloorConstruction == null || this.plannedInsulationConstruction == null || (this.PlannedConnection == null && !this.plannedProductIsConnection)) {
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
				return false;
			}

			if (this.plannedProductIsConnection) {
				this.plannedLayDistance = null;
				this.plannedRimType = null;
				this.circuits.Clear();
				errorMsg = null;
				return true;
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
			Dictionary<LayDistance, Nullable<RimType>[]> teilungen = new Dictionary<LayDistance, RimType?[]>();
			if (this.plannedRimLength > 0) {
				teilungen.Add(LayDistance.EV35, new Nullable<RimType>[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV30, new Nullable<RimType>[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV25, new Nullable<RimType>[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV20, new Nullable<RimType>[] { RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV15, new Nullable<RimType>[] { RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV10, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV5, new Nullable<RimType>[] { null });
			} else {
				teilungen.Add(LayDistance.EV35, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV30, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV25, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV20, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV15, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV10, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV5, new Nullable<RimType>[] { null });
			}
			if (this.requestedLayDistance != null) {
				LayDistance[] distances = new LayDistance[teilungen.Keys.Count];
				teilungen.Keys.CopyTo(distances, 0);
				foreach (LayDistance distance in distances) {
					if (distance != this.requestedLayDistance) {
						teilungen.Remove(distance);
					}
				}
			}
			if (this.requestedRimType != null) {
				LayDistance[] distances = new LayDistance[teilungen.Keys.Count];
				teilungen.Keys.CopyTo(distances, 0);
				foreach (LayDistance distance in distances) {
					teilungen[distance] = new Nullable<RimType>[] { this.requestedRimType };
				}
			}

			// Get Vorlauf and Ruecklauf of the defined connection pipes
			double vorlaufTotalFirst = 0;
			double vorlaufNotIsolatedFirst = 0;
			double ruecklaufTotalFirst = 0;
			double ruecklaufNotIsolatedFirst = 0;
			double vorlaufTotalOthers = 0;
			double vorlaufNotIsolatedOthers = 0;
			double ruecklaufTotalOthers = 0;
			double ruecklaufNotIsolatedOthers = 0;
			foreach (ConnectionPipe cp in this.PlannedConnectionPipes) {
				vorlaufTotalFirst += cp.Vorlauf;
				ruecklaufTotalFirst += cp.Ruecklauf;
				if (cp.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) {
					vorlaufNotIsolatedFirst += cp.Vorlauf;
				}
				if (cp.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) {
					ruecklaufNotIsolatedFirst += cp.Ruecklauf;
				}
				if (!cp.OnlyFirst) {
					vorlaufTotalOthers += cp.Vorlauf;
					ruecklaufTotalOthers += cp.Ruecklauf;
					if (cp.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) {
						vorlaufNotIsolatedOthers += cp.Vorlauf;
					}
					if (cp.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) {
						ruecklaufNotIsolatedOthers += cp.Ruecklauf;
					}
				}
			}

			// add connected products to Vorlauf and Ruecklauf
			double[] vorlaufTotal = new double[] { vorlaufTotalFirst, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers };
			double[] vorlaufNotIsolated = new double[] { vorlaufNotIsolatedFirst, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers };
			double[] ruecklaufTotal = new double[] { ruecklaufTotalFirst, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers };
			double[] ruecklaufNotIsolated = new double[] { ruecklaufNotIsolatedFirst, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers };

			foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.connectedCircuits) {
				if (kvp.Value != null) {
					if (kvp.Value.type == Circuit.CircuitConnectionTypeEnum.VORLAUF) {
						vorlaufTotal[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithAllConnections;
						vorlaufNotIsolated[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithUnisolatedConnections;
					} else {
						ruecklaufTotal[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithAllConnections;
						ruecklaufNotIsolated[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithUnisolatedConnections;
					}
				}
			}

			double longestVorlaufTotal = vorlaufTotal[0];
			double longestRuecklaufTotal = ruecklaufTotal[0];
			for (int i = 1; i < 12; i++) {
				if (vorlaufTotal[i] > longestVorlaufTotal) {
					longestVorlaufTotal = vorlaufTotal[i];
				}
				if (ruecklaufTotal[i] > longestRuecklaufTotal) {
					longestRuecklaufTotal = ruecklaufTotal[i];
				}
			}

			Nullable<LayDistance> bestLaydistance = null;
			Nullable<RimType> bestRimType = null;
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

			foreach (LayDistance ld in teilungen.Keys) {
				foreach (Nullable<RimType> rt in teilungen[ld]) {
					bool tryCalc = true;
					int circuitCount = 1;
					if (this.requestedCircuits.HasValue) {
						circuitCount = this.requestedCircuits.Value;
					} else {
						circuitCount = (int)Math.Ceiling((this.plannedArea - this.plannedAreaReduced / 2 - this.plannedAreaUnheated - areaRemovedDueConnection) * EurovalProduct.GetPipeLengthPerSqm(ld) / (100 - longestVorlaufTotal - longestRuecklaufTotal));
					}
					circuitCount = circuitCount < 1 ? 1 : circuitCount;
					while (tryCalc) {
						if (this.circuits.Count > circuitCount) {
							// TODO check if other products are connected to the circuits that are removed
							this.circuits.RemoveRange(circuitCount, this.circuits.Count - circuitCount);
						}
						while (this.circuits.Count < circuitCount) {
							this.circuits.Add(new EurovalCircuit());
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
							ec.VorlaufTotal = vorlaufTotal[i];
							ec.VorlaufNotIsolated = vorlaufNotIsolated[i];
							ec.RuecklaufTotal = ruecklaufNotIsolated[i];
							ec.RuecklaufNotIsolated = ruecklaufNotIsolated[i];
							ec.Calculate(ld, rt);
							i++;
						}
						tryCalc = false;
						if (this.PlannedDeltaRhoHeat > EurovalProduct.ConfigMaxPressureLost) {
							tryCalc = true;
						}
						if (this.PlannedDeltaRhoCool > EurovalProduct.ConfigMaxPressureLost) {
							tryCalc = true;
						}
						if (this.PlannedMhHeat > EurovalProduct.ConfigMaxDurchfluss) {
							tryCalc = true;
						}
						if (this.PlannedMhCool > EurovalProduct.ConfigMaxDurchfluss) {
							tryCalc = true;
						}
						tryCalc = tryCalc && !this.requestedCircuits.HasValue;
						tryCalc = tryCalc && circuitCount < 12;
						if (tryCalc) {
							circuitCount++;
						}
					}
					bool useNew = !bestLaydistance.HasValue ||
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
				if (this.requestedCircuits.HasValue) {
					// TODO reset circuits
				} else {
					this.circuits.Clear();
				}
				errorMsg = "Keine Automatische Auslegung möglich";
				return false;
			}

			{ // calculate best choice again
				this.plannedLayDistance = bestLaydistance;
				this.plannedRimType = bestRimType;
				if (this.circuits.Count > bestCircuits) {
					// TODO check if other products are connected to the circuits that are removed
					this.circuits.RemoveRange(bestCircuits, this.circuits.Count - bestCircuits);
				}
				while (this.circuits.Count < bestCircuits) {
					this.circuits.Add(new EurovalCircuit());
				}
				int i = 0;
				foreach (EurovalCircuit ec in this.circuits) {
					ec.EurovalProduct = this;
					ec.NrOfCircuit = i;
					ec.AreaTotal = this.plannedArea / bestCircuits;
					ec.AreaReduced = this.plannedAreaReduced / bestCircuits;
					ec.AreaUnheated = this.plannedAreaUnheated / bestCircuits;
					ec.AreaRemovedDueConnection = areaRemovedDueConnection / bestCircuits;
					ec.RimLength = this.plannedRimLength / bestCircuits;
					ec.RimCorners = ((double)this.plannedRimCorners) / bestCircuits;
					ec.VorlaufTotal = vorlaufTotal[i];
					ec.VorlaufNotIsolated = vorlaufNotIsolated[i];
					ec.RuecklaufTotal = ruecklaufNotIsolated[i];
					ec.RuecklaufNotIsolated = ruecklaufNotIsolated[i];
					ec.Calculate(bestLaydistance.Value, bestRimType);
					i++;
				}
			}

			errorMsg = null;
			return true;


			/*this.plannedRemoveArea = 0;
			this.plannedHeatLoadAnbindung = 0;
			this.plannedCoolLoadAnbindung = 0;
			List<ConnectionPipe> connectionPipes = new List<ConnectionPipe>();
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
							if (cp != null && cp.ConnectionThrough != null && cp.ConnectionThrough.Product == this) {
								connectionPipes.Add(cp);
								this.plannedRemoveArea += cp.AreaTotal;
								this.plannedHeatLoadAnbindung += cp.HeatLoadTotal;
								this.plannedCoolLoadAnbindung += cp.CoolLoadTotal;
							}
						}
					}
				}
			}

			if (this.plannedProductIsConnection) {
				this.plannedHeatLoad = this.plannedHeatLoadAnbindung;
				this.plannedCoolLoad = this.plannedCoolLoadAnbindung;
				errorMsg = null;
				return true;
			}

			// Rohrlänge Anbindeleitungen
			double vorlaufTotal = 0;
			double ruecklaufTotal = 0;
			double vorlaufNotIsolated = 0;
			double ruecklaufNotIsolated = 0;
			foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
				vorlaufNotIsolated += (pipe.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) ? pipe.Vorlauf : 0;
				ruecklaufNotIsolated += (pipe.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) ? pipe.Ruecklauf : 0;
				vorlaufTotal += pipe.Vorlauf;
				ruecklaufTotal += pipe.Ruecklauf;
			}

			Dictionary<LayDistance, Nullable<RimType>[]> teilungen = new Dictionary<LayDistance, RimType?[]>();
			if (this.plannedRimLength > 0) {
				teilungen.Add(LayDistance.EV35, new Nullable<RimType>[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV30, new Nullable<RimType>[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV25, new Nullable<RimType>[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV20, new Nullable<RimType>[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV15, new Nullable<RimType>[] { RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV10, new Nullable<RimType>[] { RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 });
				teilungen.Add(LayDistance.EV5, new Nullable<RimType>[] { RimType.EV5_40 });
			} else {
				teilungen.Add(LayDistance.EV35, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV30, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV25, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV20, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV15, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV10, new Nullable<RimType>[] { null });
				teilungen.Add(LayDistance.EV5, new Nullable<RimType>[] { null });
			}
			if (this.requestedLayDistance != null) {
				LayDistance[] distances = new LayDistance[teilungen.Keys.Count];
				teilungen.Keys.CopyTo(distances, 0);
				foreach (LayDistance distance in distances) {
					if (distance != this.requestedLayDistance) {
						teilungen.Remove(distance);
					}
				}
			}
			if (this.requestedRimType != null) {
				LayDistance[] distances = new LayDistance[teilungen.Keys.Count];
				teilungen.Keys.CopyTo(distances, 0);
				foreach (LayDistance distance in distances) {
					teilungen[distance] = new Nullable<RimType>[] { this.requestedRimType };
				}
			}

			Nullable<LayDistance> curLaydistance = null;
			Nullable<RimType> curRimtype = null;
			Nullable<RimType> distanceRim = this.plannedRimLength > 0 ? (Nullable<RimType>)RimType.EV15_60 : (Nullable<RimType>)null;
			Dictionary<LayDistance, Nullable<RimType>[]>.KeyCollection.Enumerator ldEnumerator = teilungen.Keys.GetEnumerator();
			Nullable<LayDistance> bestLaydistance = null;
			Nullable<RimType> bestRimType = null;
			double bestPipeLength = double.MaxValue;
			int bestCircuits = int.MaxValue;
			double bestFloorTempRimHeat = double.MaxValue;
			double bestFloorTempResidenceHeat = double.MaxValue;
			double bestPressureLossHeat = double.MaxValue;
			double bestFloorTempRimCool = double.MaxValue;
			double bestFloorTempResidenceCool = double.MaxValue;
			double bestPressureLossCool = double.MaxValue;
			double bestHeatLoad = 0;
			double bestCoolLoad = 0;
			while (ldEnumerator.MoveNext()) {
				curLaydistance = ldEnumerator.Current;
				IEnumerator rtEnumerator = teilungen[curLaydistance.Value].GetEnumerator();
				while (rtEnumerator.MoveNext()) {
					curRimtype = rtEnumerator.Current as Nullable<RimType>;
					this.CalculateQForLayDistance(curLaydistance.Value, curRimtype, this.requestedCircuits, vorlaufTotal, vorlaufNotIsolated, ruecklaufTotal, ruecklaufNotIsolated);
					if (!this.requestedCircuits.HasValue) {
						// if pressure loss is to large increase circuits until pressure loss is within the valid range
						while (this.plannedCircuits < 12 &&
								(calculateHeat && this.PlannedDeltaRhoHeat > EurovalProduct.maxPressureLost / 100) ||
								(calculateCool && this.PlannedDeltaRhoCool > EurovalProduct.maxPressureLost / 100)) {
							this.CalculateQForLayDistance(curLaydistance.Value, curRimtype, this.plannedCircuits + 1, vorlaufTotal, vorlaufNotIsolated, ruecklaufTotal, ruecklaufNotIsolated);
						}
					}
					// check if new parameters are better than the old ones
					if ((!bestLaydistance.HasValue && this.CheckHardParameters(this.PlannedFloorTemperatureHeatRim, this.PlannedFloorTemperatureHeatResidence, this.PlannedDeltaRhoHeat,
							this.PlannedFloorTemperatureCoolRim, this.PlannedFloorTemperatureCoolResidence, this.PlannedDeltaRhoCool,
							this.PlannedPipeLengthPerCircuit, calculateHeat, calculateCool, this.requestedLayDistance.HasValue, this.requestedRimType.HasValue, this.requestedCircuits.HasValue)) || 
							( bestLaydistance.HasValue &&
							this.CompareParameters(bestFloorTempRimHeat, bestFloorTempResidenceHeat, bestHeatLoad, bestPressureLossHeat,
							bestFloorTempRimCool, bestFloorTempResidenceCool, bestCoolLoad, bestPressureLossCool,
							bestPipeLength,
							this.PlannedFloorTemperatureHeatRim, this.PlannedFloorTemperatureHeatResidence, this.PlannedHeatLoad, this.PlannedDeltaRhoHeat,
							this.PlannedFloorTemperatureCoolRim, this.PlannedFloorTemperatureCoolResidence, this.PlannedCoolLoad, this.PlannedDeltaRhoCool,
							this.PlannedPipeLengthPerCircuit, requestedHeatLoad, requestedCoolLoad, calculateHeat, calculateCool, this.requestedLayDistance.HasValue, this.requestedRimType.HasValue, this.requestedCircuits.HasValue))) {

						bestLaydistance = this.PlannedLayDistance;
						bestRimType = this.PlannedRimType;
						bestPipeLength = this.PlannedPipeLengthPerCircuit;
						bestCircuits = this.PlannedCircuits;
						bestFloorTempRimHeat = this.PlannedFloorTemperatureHeatRim;
						bestFloorTempResidenceHeat = this.PlannedFloorTemperatureHeatResidence;
						bestPressureLossHeat = this.PlannedDeltaRhoHeat;
						bestFloorTempRimCool = this.PlannedFloorTemperatureCoolRim;
						bestFloorTempResidenceCool = this.PlannedFloorTemperatureCoolResidence;
						bestPressureLossCool = this.PlannedDeltaRhoCool;
						bestHeatLoad = this.PlannedHeatLoad;
						bestCoolLoad = this.PlannedCoolLoad;
					}
				}
			}
			if (!bestLaydistance.HasValue) {
				this.plannedLayDistance = null;
				this.plannedRimType = null;
				this.plannedHeatLoad = 0;
				this.plannedHeatLoadAnbindung = 0;
				this.plannedQHeat = 0;
				this.plannedQHeatU = 0;
				this.plannedQHeatRim = 0;
				this.plannedQHeatResidence = 0;
				this.plannedDeltaRhoHeat = 0;
				this.plannedSpreizungHeat = 0;
				this.plannedMhHeat = 0;
				this.plannedCoolLoad = 0;
				this.plannedCoolLoadAnbindung = 0;
				this.plannedQCool = 0;
				this.plannedQCoolU = 0;
				this.plannedQCoolRim = 0;
				this.plannedQCoolResidence = 0;
				this.plannedDeltaRhoCool = 0;
				this.plannedSpreizungCool = 0;
				this.plannedMhCool = 0;
				this.plannedPipeLength = 0;
				this.plannedCircuits = 1;
				errorMsg = "Keine Automatische Auslegung möglich";
				return false;
			}
			this.CalculateQForLayDistance(bestLaydistance.Value, bestRimType, bestCircuits, vorlaufTotal, vorlaufNotIsolated, ruecklaufTotal, ruecklaufNotIsolated);
			if (requestedHeatLoad <= 0) {
				this.plannedHeatLoad = 0;
				this.plannedHeatLoadAnbindung = 0;
				this.plannedQHeat = 0;
				this.plannedQHeatU = 0;
				this.plannedQHeatRim = 0;
				this.plannedQHeatResidence = 0;
				this.plannedDeltaRhoHeat = 0;
				this.plannedSpreizungHeat = 0;
			}
			if (requestedCoolLoad <= 0) {
				this.plannedCoolLoad = 0;
				this.plannedCoolLoadAnbindung = 0;
				this.plannedQCool = 0;
				this.plannedQCoolU = 0;
				this.plannedQCoolRim = 0;
				this.plannedQCoolResidence = 0;
				this.plannedDeltaRhoCool = 0;
				this.plannedSpreizungCool = 0;
			}
			errorMsg = "";
			if (this.PlannedPipeLengthPerCircuit > EurovalProduct.ConfigMaxCircuitLength - vorlaufTotal - ruecklaufTotal) {
				errorMsg += "Rohrlänge zu groß (" + Math.Round(this.PlannedPipeLengthPerCircuit, 1) + "m > " + Math.Round(EurovalProduct.ConfigMaxCircuitLength - vorlaufTotal - ruecklaufTotal, 1) + "m)\n";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeatResidence, 1) > (EurovalProduct.ConfigUseHarreitherNorm ? EurovalProduct.ConfigMaxResidenceTempHarreither : EurovalProduct.ConfigMaxResidenceTempEn1264)) {
				errorMsg += "Oberflächentemperatur in der Aufenthaltszone zu groß (" + Math.Round(this.PlannedFloorTemperatureHeatResidence, 1) + "°C > " + Math.Round((EurovalProduct.ConfigUseHarreitherNorm ? EurovalProduct.ConfigMaxResidenceTempHarreither : EurovalProduct.ConfigMaxResidenceTempEn1264), 1) + "°C)";
			}
			if (Math.Round(this.PlannedFloorTemperatureHeatRim, 1) > (EurovalProduct.ConfigUseHarreitherNorm ? EurovalProduct.ConfigMaxRimTempHarreither : EurovalProduct.ConfigMaxRimTempEn1264)) {
				errorMsg += "Oberflächentemperatur in der Randzone zu groß (" + Math.Round(this.PlannedFloorTemperatureHeatRim, 1) + "°C > " + Math.Round((EurovalProduct.ConfigUseHarreitherNorm ? EurovalProduct.ConfigMaxRimTempHarreither : EurovalProduct.ConfigMaxRimTempEn1264), 1) + "°C)";
			}
			return true;*/
		}
	}
	
}
