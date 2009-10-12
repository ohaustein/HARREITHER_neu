using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Euroval®")]
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

		protected float plannedArea = 0;
		private float plannedAreaReduced = 0;
		private float plannedAreaUnheated = 0;
		private float plannedRimLength = 0;
		private int plannedRimCorners = 0;
		private float plannedRoomTemperatureBelowHeat = 10;
		private float plannedRoomTemperatureBelowCool = 30;
		private Construction plannedFloorConstruction = null;
		private Construction plannedInsulationConstruction = null;

		private Nullable<LayDistance> requestedLayDistance = null;
		private Nullable<RimType> requestedRimType = null;

		private Nullable<LayDistance> plannedLayDistance = null;
		private Nullable<RimType> plannedRimType = null;

		private double plannedQHeat = 0;
		private double plannedQHeatRim = 0;
		private double plannedQHeatResidence = 0;
		private double plannedQHeatU = 0;
		private double plannedDeltaRhoHeat = 0;
		private double plannedSpreizungHeat = 0;
		private double plannedQCool = 0;
		private double plannedQCoolRim = 0;
		private double plannedQCoolResidence = 0;
		private double plannedQCoolU = 0;
		private double plannedDeltaRhoCool = 0;
		private double plannedSpreizungCool = 0;

		private double plannedPipeLength = 0;

		public enum LayDistance {
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
		private static double GetPipeLengthPerSqm(LayDistance distance) {
			switch (distance) {
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
		private static double GetTeilung(LayDistance distance) {
			switch (distance) {
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
		private static LayDistance GetRimLayDistance(RimType rimType) {
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
		private static int GetRimWidth(RimType rimType) {
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
			get { return this.PlannedFloorArea - this.PlannedAreaRim; }
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
			get { return Math.Round(plannedQHeat * PlannedFloorArea, 1); }
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
		public double PlannedSpreizungHeat {
			get { return this.plannedSpreizungHeat; }
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
			get { return Math.Round(plannedQCool * PlannedFloorArea, 1); }
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
		public double PlannedSpreizungCool {
			get { return this.plannedSpreizungCool; }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeatRim {
			get { return this.plannedQHeatRim / alphaFbh + this.AssociatedRoom.RoomHeatTemperature; }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureCoolRim {
			get { return this.plannedQCoolRim / alphaFbk + this.AssociatedRoom.RoomHeatTemperature; }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureHeatResidence {
			get { return this.plannedQHeatResidence / alphaFbh + this.AssociatedRoom.RoomHeatTemperature; }
		}

		[XmlIgnore]
		public double PlannedFloorTemperatureCoolResidence {
			get { return this.plannedQCoolResidence / alphaFbk + this.AssociatedRoom.RoomHeatTemperature; }
		}
		#endregion Cool Load

		[XmlIgnore]
		public double PlannedPipeLength {
			get { return this.plannedPipeLength; }
		}
		#endregion Auslegung calculated values

		private void CalculateQForLayDistance(LayDistance distance, Nullable<RimType> distanceRim,
				out double qHeatU, out double qHeat, out double qHeatRim, out double qHeatResidence, out double deltaRhoHeat, out double spreizungHeat,
				out double qCoolU, out double qCool, out double qCoolRim, out double qCoolResidence, out double deltaRhoCool, out double spreizungCool,
				out double pipeLength) {
			if (this.plannedArea == 0) {
				qHeatU = 0;
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
				pipeLength = 0;
				return;
			}

			EN1264 en1264 = EN1264.Instance;

			double su = 0.035; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
			double lambdaU = 1.2; /* Estrich??? */
			double rLambdaB = this.plannedFloorConstruction == null ? 0 : this.plannedFloorConstruction.RValue;  //0.1; /* Annahme Parkett mit 0.1 m²K/W; durch echte Konstruktion ersetzen! */
			double rLambdaIns = this.plannedInsulationConstruction == null ? 0 : this.plannedInsulationConstruction.RValue;
			double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
			double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */
			double rAlphaDecke = 0.17; /* Wärmeübergang Decke; fix??? */

			// Aufteilung RZ - AZ
			double aGes = this.PlannedFloorArea;	// gesamte Fläche
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
			double lRlGes = lRlRz + lRlAz;
			double originalPipeLength = this.plannedPipeLength;
			this.plannedPipeLength = lRlGes;

			{ // Heizlastberechnung
				double thetaVrz = 35;
				double thetaRaz = 30;
				this.GetHeatFlow(out thetaVrz, out thetaRaz);
				double thetaRrz = thetaVrz;
				double thetaVaz = thetaVrz;
				spreizungHeat = thetaVrz - thetaRaz;

				double dThetaRz = 0;

				if (calculateWithRim) {
					thetaRrz = thetaVrz - (thetaVrz - thetaRaz) * lRlRz / lRlGes;
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

				qHeat = QFbh / aGes;
				qHeatRim = qRz;
				qHeatResidence = qAz;
				qHeatU = en1264.WaermeverlustUnten(alphaFbh, rLambdaB, su, lambdaU, rAlphaDecke, rLambdaIns, rLambdaDecke, rLambdaPutz, qHeat, this.AssociatedRoom.RoomHeatTemperature, this.PlannedRoomTemperatureBelowHeat);
				//                                                                           // Wärmeverlust nach unten berechnen

				// hydraulische Berechnung
				double qH2o = (qHeat + qHeatU) * aGes;                                        // gesamte aufgenommene Leistung berechnen
				double deltaT = thetaVrz - thetaRaz;                                          // gesamte Spreizung
				deltaRhoHeat = en1264.DruckverlustRohr(qH2o, c, deltaT, rohrInnenA, rho, rohrInnenD, v, 0.000004, lRlGes);
				//                                                                           // gesamten Druckverlust berechnen
			}

			{ // Kühllastberechnung
				double thetaVrz = 16;
				double thetaRaz = 22;
				this.GetCoolFlow(out thetaVrz, out thetaRaz);
				double thetaRrz = thetaVrz;
				double thetaVaz = thetaVrz;
				spreizungCool = thetaRaz - thetaVrz;

				double dThetaRz = 0;

				if (calculateWithRim) {
					thetaRrz = thetaVrz - (thetaVrz - thetaRaz) * lRlRz / lRlGes;
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

				qCool = QFbk / aGes;
				qCoolRim = qRz;
				qCoolResidence = qAz;
				qCoolU = en1264.WaermeverlustUnten(alphaFbk, rLambdaB, su, lambdaU, rAlphaDecke, rLambdaIns, rLambdaDecke, rLambdaPutz, qCool, this.AssociatedRoom.RoomCoolTemperature, this.PlannedRoomTemperatureBelowCool);
				//                                                                           // Kühlverlust nach unten berechnen

				// hydraulische Berechnung
				double qH2o = (qCool + qCoolU) * aGes;                                        // gesamte aufgenommene Leistung berechnen
				double deltaT = thetaVrz - thetaRaz;                                          // gesamte Spreizung
				deltaRhoCool = en1264.DruckverlustRohr(qH2o, c, deltaT, rohrInnenA, rho, rohrInnenD, v, 0.000004, lRlGes);
				//                                                                           // gesamten Druckverlust berechnen
			}
			pipeLength = lRlGes;                                                          // gesamte Rohrlänge
			this.plannedPipeLength = originalPipeLength;
		}

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg) {
			if (this.plannedFloorConstruction == null || this.plannedInsulationConstruction == null || this.PlannedConnectionVorlauf == null || this.PlannedConnectionRuecklauf == null) {
				errorMsg = "Fehlende Eingaben: ";
				if (plannedFloorConstruction == null) {
					errorMsg += "Fußbodenkonstruktion, ";
				}
				if (plannedInsulationConstruction == null) {
					errorMsg += "Wärmedämmkonstruktion, ";
				}
				if (PlannedConnectionVorlauf == null || PlannedConnectionRuecklauf == null) {
					errorMsg += "Heizkreisanschluß, ";
				}
				errorMsg = errorMsg.Substring(0, errorMsg.Length - 2);
				return false;
			}
			LayDistance[] teilungen =
				(this.requestedLayDistance == null ?
				new LayDistance[] { LayDistance.EV35, LayDistance.EV30, LayDistance.EV25, LayDistance.EV20, LayDistance.EV15, LayDistance.EV10, LayDistance.EV5 } :
				new LayDistance[] { this.requestedLayDistance.Value });
			Nullable<RimType>[] randzonen =
				(this.requestedRimType == null ?
					(this.plannedRimLength > 0 ? new Nullable<RimType>[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 } :
					new Nullable<RimType>[] { null }) :
				new Nullable<RimType>[] { this.requestedRimType.Value });
			bool found = false;
			int i = 0;
			int j = 0;
			double qHeat = 0;
			double qHeatU = 0;
			double qHeatRim = 0;
			double qHeatResidence = 0;
			double deltaRhoHeat = 0;
			double spreizungHeat = 0;
			double qCool = 0;
			double qCoolU = 0;
			double qCoolRim = 0;
			double qCoolResidence = 0;
			double deltaRhoCool = 0;
			double spreizungCool = 0;
			double pipeLength = 0;
			Nullable<RimType> distanceRim = this.plannedRimLength > 0 ? (Nullable<RimType>)RimType.EV15_60 : (Nullable<RimType>)null;
			while (!found && i < teilungen.Length) {
				j = 0;
				while (!found && j < randzonen.Length) {
					this.CalculateQForLayDistance(teilungen[i], randzonen[j],
						out qHeatU, out qHeat, out qHeatRim, out qHeatResidence, out deltaRhoHeat, out spreizungHeat,
						out qCoolU, out qCool, out qCoolRim, out qCoolResidence, out deltaRhoCool, out spreizungCool,
						out pipeLength);
					if ((!calculateHeat || qHeat * this.PlannedFloorArea >= requestedHeatLoad) &&
							(!calculateCool || -qCool * this.PlannedFloorArea >= requestedCoolLoad)) {
						found = true;
					} else {
						j++;
					}
				}
				if (!found) {
					i++;
				}
			}
			if (!found) {
				i--;
				j--;
			}
			this.PlannedLayDistance = teilungen[i];
			this.PlannedRimType = randzonen[j];
			if (requestedHeatLoad > 0) {
				this.plannedQHeat = qHeat;
				this.plannedQHeatU = qHeatU;
				this.plannedQHeatRim = qHeatRim;
				this.plannedQHeatResidence = qHeatResidence;
				this.plannedDeltaRhoHeat = deltaRhoHeat;
				this.plannedSpreizungHeat = spreizungHeat;
			} else {
				this.plannedQHeat = 0;
				this.plannedQHeatU = 0;
				this.plannedQHeatRim = 0;
				this.plannedQHeatResidence = 0;
				this.plannedDeltaRhoHeat = 0;
				this.plannedSpreizungHeat = 0;
			}
			if (requestedCoolLoad > 0) {
				this.plannedQCool = -qCool;
				this.plannedQCoolU = -qCoolU;
				this.plannedQCoolRim = -qCoolRim;
				this.plannedQCoolResidence = -qCoolResidence;
				this.plannedDeltaRhoCool = deltaRhoCool;
				this.plannedSpreizungCool = spreizungCool;
			} else {
				this.plannedQCool = 0;
				this.plannedQCoolU = 0;
				this.plannedQCoolRim = 0;
				this.plannedQCoolResidence = 0;
				this.plannedDeltaRhoCool = 0;
				this.plannedSpreizungCool = 0;
			}
			this.plannedPipeLength = pipeLength;
			errorMsg = null;
			return true;
		}
	}
	
}
