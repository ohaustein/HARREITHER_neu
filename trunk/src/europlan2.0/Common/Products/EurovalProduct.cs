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
		//private static double defaultThetaVHeat = 35;
		//private static double defaultThetaRHeat = 30;
		//private static double defaultThetaVCool = 17;
		//private static double defaultThetaRCool = 20;
		private static bool agActivated = true;

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
		private int plannedCircuits = 1;

		private double plannedQHeat = 0;
		private double plannedQHeatRim = 0;
		private double plannedQHeatResidence = 0;
		private double plannedQHeatU = 0;
		private double plannedDeltaRhoHeat = 0;
		private double plannedMhHeat = 0;
		private double plannedSpreizungHeat = 0;
		private double plannedHeatLoad = 0;
		private double plannedHeatLoadAnbindung = 0;
		private double plannedQCool = 0;
		private double plannedQCoolRim = 0;
		private double plannedQCoolResidence = 0;
		private double plannedQCoolU = 0;
		private double plannedDeltaRhoCool = 0;
		private double plannedSpreizungCool = 0;
		private double plannedMhCool = 0;
		private double plannedCoolLoad = 0;
		private double plannedCoolLoadAnbindung = 0;

		private double plannedPipeLength = 0;
		private double plannedRemoveArea = 0;
		private double plannedRemoveHeatLoad = 0;
		private double plannedRemoveCoolLoad = 0;

		public enum LayDistance {
			A5,
			EV5,
			EV10,
			EV15,
			EV20,
			EV25,
			EV30,
			EV35
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

		/*[ProductParameter]
		public static double ConfigDefaultThetaVHeat {
			get { return EurovalProduct.defaultThetaVHeat; }
			set { EurovalProduct.defaultThetaVHeat = value; }
		}

		[ProductParameter]
		public static double ConfigDefaultThetaRHeat {
			get { return EurovalProduct.defaultThetaRHeat; }
			set { EurovalProduct.defaultThetaRHeat = value; }
		}

		[ProductParameter]
		public static double ConfigDefaultThetaVCool {
			get { return EurovalProduct.defaultThetaVCool; }
			set { EurovalProduct.defaultThetaVCool = value; }
		}

		[ProductParameter]
		public static double ConfigDefaultThetaRCool {
			get { return EurovalProduct.defaultThetaRCool; }
			set { EurovalProduct.defaultThetaRCool = value; }
		}*/

		[ProductParameter(overrideableInPlanning=true)]
		public static bool ConfigAgActivated {
			get { return EurovalProduct.agActivated; }
			set { EurovalProduct.agActivated = value; }
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
			get { return this.PlannedFloorArea - this.PlannedAreaRim - (float)this.plannedRemoveArea; }
		}

		/// <summary>
		/// The percentage of the total room area that is occupied by the planned area.
		/// </summary>
		[XmlIgnore]
		public float PlannedFloorAreaPercentage {
			get { return (this.AvailableFloorArea <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AvailableFloorArea); }
			set { this.PlannedFloorArea = (float)(this.AvailableFloorArea * value / 100); }
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
			get { return this.plannedQHeat; }
		}

		/// <summary>
		/// The heat load per m² that is emmited in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadPerSqMBelow {
			get { return this.plannedQHeatU; }
		}

		/// <summary>
		/// The heat load per m² that is emmited outside of the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadPerSqMH20 {
			get { return this.plannedQHeat + this.plannedQHeatU; }
		}

		/// <summary>
		/// The heat load per m² that is emmited by the rim area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadPerSqMRim {
			get { return this.plannedQHeatRim; }
		}

		/// <summary>
		/// The heat load per m² that is emmited by the residence area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadPerSqMResidence {
			get { return this.plannedQHeatResidence; }
		}

		/// <summary>
		/// The total heat load that is emmited in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public override double PlannedHeatLoad {
			get { return Math.Round(this.plannedHeatLoad, 1); }
		}

		[XmlIgnore]
		public double PlannedHeatLoadAnbindung {
			get { return Math.Round(this.plannedHeatLoadAnbindung, 1); }
		}

		/// <summary>
		/// The total heat load that is emmited by the rim area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadRim {
			get { return Math.Round(this.PlannedAreaRim * this.plannedQHeatRim, 0); }
		}

		/// <summary>
		/// The total heat load that is emmited by the residence area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedHeatLoadResidence {
			get { return Math.Round(this.PlannedAreaResidence * this.plannedQHeatResidence, 0); }
		}

		/// <summary>
		/// The pressure loss for heating, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedDeltaRhoHeat {
			get { return this.plannedDeltaRhoHeat; }
		}

		[XmlIgnore]
		public double PlannedMhHeat {
			get { return this.plannedMhHeat; }
		}

		[XmlIgnore]
		public double PlannedSpreizungHeat {
			get { return this.plannedSpreizungHeat; }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeatRim {
			get { return EN1264.Instance.OberflaechenTemperatur(this.plannedQHeatRim, alphaFbh, this.AssociatedRoom.RoomHeatTemperature); }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeatResidence {
			get { return EN1264.Instance.OberflaechenTemperatur(this.plannedQHeatResidence, alphaFbh, this.AssociatedRoom.RoomHeatTemperature); }
		}
		#endregion Heat Load

		#region Cool Load
		/// <summary>
		/// The cool load per m² that results of the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadPerSqM {
			get { return this.plannedQCool; }
		}

		/// <summary>
		/// The heat load per m² that is emmited in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadPerSqMBelow {
			get { return this.plannedQCoolU; }
		}

		/// <summary>
		/// The cool load per m² that is emmited outside of the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadPerSqMH20 {
			get { return this.plannedQCool + this.plannedQCoolU; }
		}

		/// <summary>
		/// The cool load per m² that is emmited by the rim area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadPerSqMRim {
			get { return this.plannedQCoolRim; }
		}

		/// <summary>
		/// The cool load per m² that is emmited by the residence area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadPerSqMResidence {
			get { return this.plannedQCoolResidence; }
		}

		/// <summary>
		/// The total cool load that is emmited in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public override double PlannedCoolLoad {
			get { return Math.Round(this.plannedCoolLoad, 1); }
		}

		[XmlIgnore]
		public double PlannedCoolLoadAnbindung {
			get { return Math.Round(this.plannedCoolLoadAnbindung, 1); }
		}

		/// <summary>
		/// The total cool load that is emmited by the rim area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadRim {
			get { return Math.Round(this.PlannedAreaRim * this.plannedQCoolRim, 0); }
		}

		/// <summary>
		/// The total cool load that is emmited by the residence area in the room, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedCoolLoadResidence {
			get { return Math.Round(this.PlannedAreaResidence * this.plannedQCoolResidence, 0); }
		}

		/// <summary>
		/// The pressure loss for cooling, based on the current calculation.
		/// </summary>
		[XmlIgnore]
		public double PlannedDeltaRhoCool {
			get { return this.plannedDeltaRhoCool; }
		}

		[XmlIgnore]
		public double PlannedMhCool {
			get { return this.plannedMhCool; }
		}

		[XmlIgnore]
		public double PlannedSpreizungCool {
			get { return this.plannedSpreizungCool; }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureCoolRim {
			get { return EN1264.Instance.OberflaechenTemperatur(-this.plannedQCoolRim, alphaFbk, this.AssociatedRoom.RoomCoolTemperature); }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureCoolResidence {
			get { return EN1264.Instance.OberflaechenTemperatur(-this.plannedQCoolResidence, alphaFbk, this.AssociatedRoom.RoomCoolTemperature); }
		}
		#endregion Cool Load

		[XmlIgnore]
		public override double PlannedPipeLength {
			get { return this.plannedPipeLength; }
		}

		[XmlIgnore]
		public int PlannedCircuits {
			//get { return this.plannedPipeLength <= 0 ? 1 : (int)Math.Ceiling(this.plannedPipeLength / maxCircuitLength); }
			get { return this.plannedCircuits; }
			set { this.plannedCircuits = value; }
		}

		[XmlIgnore]
		public double PlannedRemoveArea {
			get { return this.plannedRemoveArea; }
		}

		[XmlIgnore]
		public double PlannedRemoveHeatLoad {
			get { return this.plannedRemoveHeatLoad; }
		}

		[XmlIgnore]
		public double PlannedRemoveCoolLoad {
			get { return this.plannedRemoveCoolLoad; }
		}

		[XmlIgnore]
		public double PlannedPipeLengthPerCircuit {
			get { return this.plannedPipeLength / this.PlannedCircuits; }
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
		private void CalculateQForLayDistance(LayDistance distance, Nullable<RimType> distanceRim, Nullable<int> circuits/*,
				out double qHeatU, out double qHeat, out double qHeatRim, out double qHeatResidence, out double deltaRhoHeat, out double spreizungHeat,
				out double qCoolU, out double qCool, out double qCoolRim, out double qCoolResidence, out double deltaRhoCool, out double spreizungCool,
				out double pipeLength*/) {
			if (this.plannedArea == 0) {
				/*qHeatU = 0;
				qHeat = 0;
				qHeatRim = 0;
				qHeatResidence = 0;
				deltaRhoHeat = 0;
				spreizungHeat = 0;
				qCoolU = 0;
				qCool = 0;
				qCoolRim = 0;
				qCoolResidence = 0;
				deltaRhoCool = 0;
				spreizungCool = 0;
				pipeLength = 0;*/
				return;
			}

			EN1264 en1264 = EN1264.Instance;

			// Rohrlänge Anbindeleitungen
			double vorlaufTotal = 0;
			double ruecklaufTotal = 0;
			double vorlaufNotIsolated = 0;
			double ruecklaufNotIsolated = 0;
			double anbindungHeatLoad = 0;
			double anbindungCoolLoad = 0;
			foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
				vorlaufNotIsolated += (pipe.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) ? pipe.Vorlauf : 0;
				ruecklaufNotIsolated += (pipe.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) ? pipe.Ruecklauf : 0;
				vorlaufTotal += pipe.Vorlauf;
				ruecklaufTotal += pipe.Ruecklauf;
				//anbindungHeatLoad += pipe.HeatLoad;
				//anbindungCoolLoad += pipe.CoolLoad;
			}

			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
							if (cp.PlannedProduct != null && cp.PlannedProduct.Product == this) {
								anbindungHeatLoad += cp.HeatLoad;
								anbindungCoolLoad += cp.CoolLoad;
							}
						}
					}
				}
			}

			double su = 0.035; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
			double lambdaU = 1.2; /* Estrich??? */
			double rLambdaB = this.plannedFloorConstruction == null ? 0 : this.plannedFloorConstruction.RValue;  //0.1; /* Annahme Parkett mit 0.1 m²K/W; durch echte Konstruktion ersetzen! */
			double rLambdaIns = this.plannedInsulationConstruction == null ? 0 : this.plannedInsulationConstruction.RValue;
			double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
			double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */
			double rAlphaDecke = 0.17; /* Wärmeübergang Decke; fix??? */

			this.plannedLayDistance = distance;
			this.plannedRimType = distanceRim;

			// Aufteilung RZ - AZ
			double aGes = this.PlannedFloorArea - this.plannedRemoveArea;	// gesamte Fläche
			double aRed = this.PlannedAreaReduced;	// Fläche mit reduzierter Heizleistung
			double aUnb = this.PlannedAreaUnheated;	// unbeheizte Fläche
			double aFbh = aGes - aRed / 2 - aUnb;	// wirksam beheizte Fläche

			bool calculateWithRim = distanceRim.HasValue && (this.plannedRimLength - this.plannedRimCorners * GetRimWidth(distanceRim.Value) / 100 > 0);
			double lRz = 0;
			double bRz = 0;
			double aRz = 0;
			double lRlRz = 0;
			if (calculateWithRim) {
				lRz = this.plannedRimLength - this.plannedRimCorners * bRz;			      // Tatsächliche länge der Randzone berechnen
				bRz = ((float)GetRimWidth(distanceRim.Value)) / 100;
				lRz = lRz < 0 ? 0 : lRz;
				aRz = lRz * bRz;                                                          // Fläche der Randzone berechnen
				lRlRz = aRz * GetPipeLengthPerSqm(GetRimLayDistance(distanceRim.Value));  // Rohrlänge der Randzone berechnen
			}
			double aAz = aFbh - aRz;                                                      // Fläche der Aufenthaltszone berechnen
			double lRlAz = aAz * GetPipeLengthPerSqm(distance);                           // Rohlänge der Aufenthaltszone berechnen
			this.plannedPipeLength = lRlRz + lRlAz;
			lRlRz = lRlRz / this.PlannedCircuits;
			lRlAz = lRlAz / this.PlannedCircuits;

			this.plannedCircuits = circuits.HasValue ? circuits.Value : (this.plannedPipeLength <= 0 ? 1 : (int)Math.Ceiling(this.plannedPipeLength / maxCircuitLength));

			{ // Heizlastberechnung
				double thetaVrz = 35;
				double thetaRaz = 30;
				this.GetHeatFlow(out thetaVrz, out thetaRaz);
				this.plannedSpreizungHeat = thetaVrz - thetaRaz;
				thetaVrz = thetaVrz - (thetaVrz - thetaRaz) * vorlaufNotIsolated / (this.PlannedPipeLengthPerCircuit + vorlaufNotIsolated + ruecklaufNotIsolated);
				thetaRaz = thetaRaz + (thetaVrz - thetaRaz) * ruecklaufNotIsolated / (this.PlannedPipeLengthPerCircuit + vorlaufNotIsolated + ruecklaufNotIsolated);
				double thetaRrz = thetaVrz;
				double thetaVaz = thetaVrz;

				double dThetaRz = 0;

				if (calculateWithRim) {
					thetaRrz = thetaVrz - (thetaVrz - thetaRaz) * lRlRz / this.PlannedPipeLengthPerCircuit;
					thetaVaz = thetaRrz;
					dThetaRz = en1264.Heizmitteluebertemperatur(thetaVrz, thetaRrz, this.AssociatedRoom.RoomHeatTemperature);
					//                                                                        // Heizmittelübertemperatur der Randzone berechnen
				}
				double dThetaAz = en1264.Heizmitteluebertemperatur(thetaVaz, thetaRaz, this.AssociatedRoom.RoomHeatTemperature);
				//                                                                            // Heizmittelübertemperatur der Aufenthaltszone berechnen

				double tRz = 0;
				double ppRz = 0;
				double bgRz = 0;
				double khRz = 0;
				double qRz = 0;
				if (calculateWithRim) {
					tRz = EurovalProduct.GetTeilung(GetRimLayDistance(distanceRim.Value));    // Teilung der Randzone
					ppRz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, (agActivated ? ag : 1));
					//                                                                        // Potenzprodukt der Randzone berechnen
					bgRz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, (agActivated ? ag : 1), sr, sr0, lambdaR, lambdaR0);
					//                                                                        // systemabhängigen Koeffizienten der Randzone berechnen
					khRz = en1264.WaermedurchgangsKoeffizientRohr(bgRz, ppRz);                // Wärmedurchgangskoeffizient der Randzone berechnen
					qRz = en1264.WaermestromDichteRohr(khRz, dThetaRz);                       // in den Raum abgegebene Wärmeleistung der Randzone berechnen
				}

				double tAz = EurovalProduct.GetTeilung(distance);                             // Teilung der Aufenthaltszone
				double ppAz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, (agActivated ? ag : 1));
				//                                                                            // Potenzprodukt der Aufenthaltszone berechnen
				double bgAz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, (agActivated ? ag : 1), sr, sr0, lambdaR, lambdaR0);
				//                                                                            // systemabhängigen Koeffizienten der Aufenthaltszone berechnen
				double khAz = en1264.WaermedurchgangsKoeffizientRohr(bgAz, ppAz);             // Wärmedurchgangskoeffizient der Aufenthaltszone berechnen
				double qAz = en1264.WaermestromDichteRohr(khAz, dThetaAz);                    // in den Raum abgegebene Wärmeleistung der Aufenthaltszone berechnen

				double QFbh = aRz * qRz + aAz * qAz;                                          // gesamte in den Raum abgegebene Wärme

				this.plannedHeatLoad = QFbh + anbindungHeatLoad;
				this.plannedHeatLoadAnbindung = anbindungHeatLoad;
				this.plannedQHeat = QFbh / aGes;
				this.plannedQHeatRim = qRz;
				this.plannedQHeatResidence = qAz;
				this.plannedQHeatU = en1264.WaermeverlustUnten(alphaFbh, rLambdaB, su, lambdaU, rAlphaDecke, rLambdaIns, rLambdaDecke, rLambdaPutz, this.plannedQHeat, this.AssociatedRoom.RoomHeatTemperature, this.PlannedRoomTemperatureBelowHeat);
				//                                                                           // Wärmeverlust nach unten berechnen

				// hydraulische Berechnung
				double qH2o = (this.plannedQHeat + this.plannedQHeatU) * aGes / this.PlannedCircuits;// gesamte aufgenommene Leistung berechnen
				double deltaT = thetaVrz - thetaRaz;                                          // gesamte Spreizung
				this.plannedDeltaRhoHeat = en1264.DruckverlustRohr(qH2o, c, deltaT, rohrInnenA, rho, rohrInnenD, v, 0.000004, this.PlannedPipeLengthPerCircuit + vorlaufTotal + ruecklaufTotal);
				//                                                                           // gesamten Druckverlust berechnen
				this.plannedMhHeat = en1264.Durchfluss(qH2o, EurovalProduct.c, deltaT);
			}

			{ // Kühllastberechnung
				double thetaVrz = 16;
				double thetaRaz = 22;
				this.GetCoolFlow(out thetaVrz, out thetaRaz);
				double thetaRrz = thetaVrz;
				double thetaVaz = thetaVrz;
				this.plannedSpreizungCool = thetaRaz - thetaVrz;

				double dThetaRz = 0;

				if (calculateWithRim) {
					thetaRrz = thetaVrz - (thetaVrz - thetaRaz) * lRlRz / this.PlannedPipeLengthPerCircuit;
					thetaVaz = thetaRrz;
					dThetaRz = en1264.Heizmitteluebertemperatur(thetaVrz, thetaRrz, this.AssociatedRoom.RoomCoolTemperature);
					//                                                                        // Heizmittelübertemperatur der Randzone berechnen
				}
				double dThetaAz = en1264.Heizmitteluebertemperatur(thetaVaz, thetaRaz, this.AssociatedRoom.RoomCoolTemperature);
				//                                                                            // Heizmittelübertemperatur der Aufenthaltszone berechnen

				double tRz = 0;
				double ppRz = 0;
				double bgRz = 0;
				double khRz = 0;
				double qRz = 0;
				if (calculateWithRim) {
					tRz = EurovalProduct.GetTeilung(GetRimLayDistance(distanceRim.Value));    // Teilung der Randzone
					ppRz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, (agActivated ? ag : 1));
					//                                                                        // Potenzprodukt der Randzone berechnen
					bgRz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, (agActivated ? ag : 1), sr, sr0, lambdaR, lambdaR0);
					//                                                                        // systemabhängigen Koeffizienten der Randzone berechnen
					khRz = en1264.WaermedurchgangsKoeffizientRohr(bgRz, ppRz);                // Wärmedurchgangskoeffizient der Randzone berechnen
					qRz = en1264.WaermestromDichteRohr(khRz, dThetaRz);                       // in den Raum abgegebene Wärmeleistung der Randzone berechnen
				}

				double tAz = EurovalProduct.GetTeilung(distance);                             // Teilung der Aufenthaltszone
				double ppAz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, (agActivated ? ag : 1));
				//                                                                            // Potenzprodukt der Aufenthaltszone berechnen
				double bgAz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, (agActivated ? ag : 1), sr, sr0, lambdaR, lambdaR0);
				//                                                                            // systemabhängigen Koeffizienten der Aufenthaltszone berechnen
				double khAz = en1264.WaermedurchgangsKoeffizientRohr(bgAz, ppAz);             // Wärmedurchgangskoeffizient der Aufenthaltszone berechnen
				double qAz = en1264.WaermestromDichteRohr(khAz, dThetaAz);                    // in den Raum abgegebene Wärmeleistung der Aufenthaltszone berechnen

				double QFbk = aRz * qRz + aAz * qAz;                                          // gesamte in den Raum abgegebene Wärme

				this.plannedCoolLoad = QFbk + anbindungCoolLoad;
				this.plannedCoolLoadAnbindung = anbindungCoolLoad;
				this.plannedQCool = -QFbk / aGes;
				this.plannedQCoolRim = -qRz;
				this.plannedQCoolResidence = -qAz;
				this.plannedQCoolU = -en1264.WaermeverlustUnten(alphaFbk, rLambdaB, su, lambdaU, rAlphaDecke, rLambdaIns, rLambdaDecke, rLambdaPutz, this.plannedQCool, this.AssociatedRoom.RoomCoolTemperature, this.PlannedRoomTemperatureBelowCool);
				//                                                                           // Kühlverlust nach unten berechnen

				// hydraulische Berechnung
				double qH2o = (this.plannedQCool + this.plannedQCoolU) * aGes;                                        // gesamte aufgenommene Leistung berechnen
				double deltaT = thetaVrz - thetaRaz;                                          // gesamte Spreizung
				this.plannedDeltaRhoCool = en1264.DruckverlustRohr(qH2o, c, deltaT, rohrInnenA, rho, rohrInnenD, v, 0.000004, this.PlannedPipeLengthPerCircuit);
				//                                                                            // gesamten Druckverlust berechnen
				this.plannedMhCool = en1264.Durchfluss(qH2o, EurovalProduct.c, deltaT);
			}
		}

		private bool CheckHardParameters(double floorTempHeatRim, double floorTempHeatRes, double pressureLossHeat,
			double floorTempCoolRim, double floorTempCoolRes, double pressureLossCool, 
			double circuitLength, bool checkHeat, bool checkCool) {

			if (checkHeat && (useHarreitherNorm && (floorTempHeatRim > maxRimTempHarreither || floorTempHeatRes > maxResidenceTempHarreither))) {
				return false;
			}
			if (checkHeat && (floorTempHeatRim > maxRimTempEn1264 || floorTempHeatRes > maxResidenceTempEn1264)) {
				return false;
			}
			if (checkHeat && (pressureLossHeat > maxPressureLost)) {
				return false;
			}
			// TODO checkCool
			if (circuitLength > maxCircuitLength) {
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
			double requestedHeatLoad, double requestedCoolLoad, bool checkHeat, bool checkCool) {

			bool oldOk = CheckHardParameters(oldFloorTempHeatRim, oldFloorTempHeatRes, oldPressureLossHeat, 
				oldFloorTempCoolRim, oldFloorTempCoolRes, oldPressureLossHeat, 
				oldCircuitLength, checkHeat, checkCool);
			bool newOk = CheckHardParameters(newFloorTempHeatRim, newFloorTempHeatRes, newPressureLossHeat, 
				newFloorTempCoolRim, newFloorTempCoolRes, newPressureLossHeat, newCircuitLength, checkHeat, checkCool);
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
			if (this.plannedFloorConstruction == null || this.plannedInsulationConstruction == null || this.PlannedConnection == null) {
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

			this.plannedRemoveArea = 0;
			this.plannedRemoveHeatLoad = 0;
			this.plannedRemoveCoolLoad = 0;
			List<ConnectionPipe> connectionPipes = new List<ConnectionPipe>();
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
							if (cp != null && cp.PlannedProduct != null && cp.PlannedProduct.Product == this) {
								connectionPipes.Add(cp);
								this.plannedRemoveArea += cp.Area;
							}
						}
					}
				}
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
					this.CalculateQForLayDistance(curLaydistance.Value, curRimtype, this.requestedCircuits);
					if (!this.requestedCircuits.HasValue) {
						// if pressure loss is to large increase circuits until pressure loss is within the valid range
						while (this.plannedCircuits < 12 &&
								(calculateHeat && this.PlannedDeltaRhoHeat > EurovalProduct.maxPressureLost / 100) ||
								(calculateCool && this.PlannedDeltaRhoCool > EurovalProduct.maxPressureLost / 100)) {
							this.CalculateQForLayDistance(curLaydistance.Value, curRimtype, this.plannedCircuits + 1);
						}
					}
					// check if new parameters are better than the old ones
					if ((!bestLaydistance.HasValue && this.CheckHardParameters(this.PlannedFloorTemperatureHeatRim, this.PlannedFloorTemperatureHeatResidence, this.PlannedDeltaRhoHeat,
							this.PlannedFloorTemperatureCoolRim, this.PlannedFloorTemperatureCoolResidence, this.PlannedDeltaRhoCool,
							this.PlannedPipeLengthPerCircuit, calculateHeat, calculateCool)) || 
							( bestLaydistance.HasValue &&
							this.CompareParameters(bestFloorTempRimHeat, bestFloorTempResidenceHeat, bestHeatLoad, bestPressureLossHeat,
							bestFloorTempRimCool, bestFloorTempResidenceCool, bestCoolLoad, bestPressureLossCool,
							bestPipeLength,
							this.PlannedFloorTemperatureHeatRim, this.PlannedFloorTemperatureHeatResidence, this.PlannedHeatLoad, this.PlannedDeltaRhoHeat,
							this.PlannedFloorTemperatureCoolRim, this.PlannedFloorTemperatureCoolResidence, this.PlannedCoolLoad, this.PlannedDeltaRhoCool,
							this.PlannedPipeLengthPerCircuit, requestedHeatLoad, requestedCoolLoad, calculateHeat, calculateCool))) {

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
			this.CalculateQForLayDistance(bestLaydistance.Value, bestRimType, bestCircuits);
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
			errorMsg = null;
			return true;
		}
	}
	
}
