using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Euroval")]
	public class EurovalProduct : Product {

		private static readonly double su0 = 0.045; /* Mindestüberdeckung fix??? */
		private static readonly double alpha0 = 10.8; /* Fixwert für FBH fix??? */
		private static readonly double alphaFbk = 6.5; //6.5; /* für FBK fix??? */
		private static readonly double alphaFbh = 10.8; /* für FBH fix??? */
		private static readonly double lambdaR0 = 0.35; /* fix ??? */
		private static readonly double lambdaR = 0.22; /* für PP Rohr laut Tabelle A.13 fix??? */
		private static readonly double lambdaU0 = 1; /* fix??? */
		private static readonly double lambdaE = 1.2; /* Estrichleitfähigkeit, fix */
		private static readonly double rohrAussenD = 0.0206505; /* Aussendurchmesser Euroval Rohr */
		private static readonly double rohrInnenD = 0.0153; /* Rohrinnendurchmesser */
		private static readonly double rohrInnenA = 0.000183783; /* Rohrinnenquerschnitt */
		private static readonly double ag = 1.1034; /* Ovalrohr Geometriefaktor für Euroval */
		private static readonly double sr0 = 0.002; /* fix ??? */
		private static readonly double sr = 0.00238; /* Aus Euroval Normprüfdaten */
		private static readonly double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static readonly double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static readonly double v = 0.00000101; /* m²/s ... kinematische Viskosität */

		private Nullable<LayDistance> plannedLayDistance = null;
		private Nullable<LayDistance> requestedLayDistance = null;
		private double plannedQ = 0;
		private double plannedQRim = 0;
		private double plannedQResidence = 0;
		private double plannedQU = 0;
		private double plannedDeltaRho = 0;
		private double plannedPipeLength = 0;

		private float plannedAreaReduced = 0;
		private float plannedAreaUnheated = 0;

		private float plannedRim = 0;
		private Nullable<RimType> plannedRimType = null;
		private Nullable<RimType> requestedRimType = null;
		private int plannedCornersRim = 0;

		private float plannedRoomTemperatureBelow = 10;

		private Construction floorConstruction = null;
		private Construction insulationConstruction = null;

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

		public override int GetDefaultQuickDimensioningCircuits() {
			LayDistance distance = Project.Instance.QuickDimensioning.LayDistance;
			return (int)Math.Ceiling(quickDimensioningPlannedArea / (100/GetPipeLengthPerSqm(distance)));
		}

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

		private static double GetTeilung(LayDistance distance) {
			switch (distance) {
				case LayDistance.EV5:
					return 0.05;
				case LayDistance.EV10:
					return 0.1;
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

		private static LayDistance GetLayDistanceRim(RimType rimType) {
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

		private static int GetWidthRim(RimType rimType) {
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
			get { return "Euroval"; }
		}

		public override string QuickDimensioningName {
			get { return "Euroval\n(m²)"; }
		}

		public override ProductType Type {
			get { return ProductType.FBH; }
		}

		public Nullable<LayDistance> PlannedLayDistance {
			get { return this.plannedLayDistance; }
			set {
				//
				this.plannedLayDistance = value;
			}
		}

		public Nullable<LayDistance> RequestedLayDistance {
			get { return this.requestedLayDistance; }
			set { this.requestedLayDistance = value; }
		}

		private double CalculateQForLayDistance(LayDistance distance, Nullable<RimType> distanceRim, out double qU, out double q, out double qRim, out double qResidence, out double deltaRho, out double pipeLength) {
			if (this.plannedFloorArea == 0) {
				qU = 0;
				q = 0;
				qRim = 0;
				qResidence = 0;
				deltaRho = 0;
				pipeLength = 0;
				return 0;
			}

			EN1264 en1264 = EN1264.Instance;

			double su = 0.045; /* Estrichüberdeckung; Annahme ECO30; durch echte Konstruktion ersetzen! */
			double lambdaU = 1.2; /* Estrich??? */
			double rLambdaB = this.floorConstruction == null ? 0 : this.floorConstruction.RValue;  //0.1; /* Annahme Parkett mit 0.1 m²K/W; durch echte Konstruktion ersetzen! */
			double rLambdaIns = this.insulationConstruction == null ? 0 : this.insulationConstruction.RValue;
			double rLambdaDecke = 0.11; /* Fußbodenbelag 25cm Stahlbeton; durch echte Konstruktion ersetzen! */
			double rLambdaPutz = 0.02; /* Fußbodenbelag 1.5cm Putz; durch echte Konstruktion ersetzen! */
			double rAlphaDecke = 0.17; /* Wärmeübergang Decke; fix??? */

			// Aufteilung RZ - AZ
			double aGes = this.PlannedFloorArea;	// gesamte Fläche
			double aRed = this.PlannedAreaReduced;	// Fläche mit reduzierter Heizleistung
			double aUnb = this.PlannedAreaUnheated;	// unbeheizte Fläche
			double aFbh = aGes - aRed / 2 - aUnb;	// wirksam beheizte Fläche

			bool calculateWithRim = distanceRim.HasValue && (this.plannedRim - this.plannedCornersRim * GetWidthRim(distanceRim.Value) / 100 > 0);
			double lRz = 0;
			double bRz = 0;
			double aRz = 0;
			double lRlRz = 0;
			if (calculateWithRim) {
				lRz = this.plannedRim - this.plannedCornersRim * bRz;			// Länge der Randzone
				bRz = ((float)GetWidthRim(distanceRim.Value)) / 100;
				lRz = lRz < 0 ? 0 : lRz;
				aRz = lRz * bRz;

				lRlRz = aRz * GetPipeLengthPerSqm(GetLayDistanceRim(distanceRim.Value));
			}
			double aAz = aFbh - aRz;
			double lRlAz = aAz * GetPipeLengthPerSqm(distance);
			double lRlGes = lRlRz + lRlAz;

			double thetaVrz = 35;
			double thetaRrz = 35;
			double thetaVaz = 35;
			double thetaRaz = 30;

			double dThetaRz = 0;

			if (calculateWithRim) {
				thetaRrz = thetaVrz - (thetaVrz - thetaRaz) * lRlRz / lRlGes;
				thetaVaz = thetaRrz;
				dThetaRz = en1264.Heizmitteluebertemperatur(thetaVrz, thetaRrz, this.AssociatedRoom.RoomTemperature);
			}
			double dThetaAz = en1264.Heizmitteluebertemperatur(thetaVaz, thetaRaz, this.AssociatedRoom.RoomTemperature);

			double tRz = 0;
			double ppRz = 0;
			double bgRz = 0;
			double khRz = 0;
			double qRz = 0;
			if (calculateWithRim) {
				tRz = EurovalProduct.GetTeilung(GetLayDistanceRim(distanceRim.Value));
				ppRz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, ag);
				bgRz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, ag, sr, sr0, lambdaR, lambdaR0);
				khRz = en1264.WaermedurchgangsKoeffizientRohr(bgRz, ppRz);
				qRz = en1264.WaermestromDichteRohr(khRz, dThetaRz);
			}

			double tAz = EurovalProduct.GetTeilung(distance);
			double ppAz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, ag);
			double bgAz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, ag, sr, sr0, lambdaR, lambdaR0);
			double khAz = en1264.WaermedurchgangsKoeffizientRohr(bgAz, ppAz);
			double qAz = en1264.WaermestromDichteRohr(khAz, dThetaAz);

			double qFbh = aRz * qRz + aAz * qAz;
			q = qFbh / aGes;

			qU = en1264.WaermeverlustUnten(alphaFbh, rLambdaB, su, lambdaU, rAlphaDecke, rLambdaIns, rLambdaDecke, rLambdaPutz, q, this.AssociatedRoom.RoomTemperature, this.PlannedRoomTemperatureBelow);

			qRim = qRz;
			qResidence = qAz;

			// hydraulische Berechnung
			double qH2o = (q + qU) * aGes;
			double deltaT = thetaVrz - thetaRaz;
			pipeLength = lRlGes;
			deltaRho = en1264.Druckverlust(qH2o, c, deltaT, rohrInnenA, rho, rohrInnenD, v, 0.000004, lRlGes);

			return q;
		}

		public override void ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad) {

			LayDistance[] teilungen =
				(this.requestedLayDistance == null ?
				new LayDistance[] { LayDistance.EV35, LayDistance.EV30, LayDistance.EV25, LayDistance.EV20, LayDistance.EV15, LayDistance.EV10, LayDistance.EV5 } :
				new LayDistance[] { this.requestedLayDistance.Value });
			RimType[] randzonen =
				(this.requestedRimType == null ?
				new RimType[] { RimType.EV15_60, RimType.EV15_120, RimType.EV15_180, RimType.EV10_55, RimType.EV10_110, RimType.EV10_165, RimType.EV5_40, RimType.EV5_80, RimType.EV5_120 } :
				new RimType[] { this.requestedRimType.Value });
			bool found = false;
			int i = 0;
			int j = 0;
			double q = 0;
			double qU = 0;
			double qRim = 0;
			double qResidence = 0;
			double deltaRho = 0;
			double pipeLength = 0;
			Nullable<RimType> distanceRim = this.plannedRim > 0 ? (Nullable<RimType>)RimType.EV15_60 : (Nullable<RimType>)null;
			while (!found && i < teilungen.Length) {
				j = 0;
				while (!found && j < randzonen.Length) {
					this.CalculateQForLayDistance(teilungen[i], randzonen[j], out qU, out q, out qRim, out qResidence, out deltaRho, out pipeLength);
					if (q * this.PlannedFloorArea >= requestedHeatLoad) {
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
			this.plannedQ = q;
			this.plannedQU = qU;
			this.plannedQRim = qRim;
			this.plannedQResidence = qResidence;
			this.plannedPipeLength = pipeLength;
			this.plannedDeltaRho = deltaRho;
		}

		public double PlannedHeatLoadPerSqM {
			get { return this.plannedQ; }
		}

		public double PlannedHeatLoadPerSqMBelow {
			get { return this.plannedQU; }
		}

		public double PlannedHeatLoadPerSqMH20 {
			get { return this.plannedQ + this.plannedQU; }
		}

		public double PlannedHeatLoadPerSqMRim {
			get { return this.plannedQRim; }
		}

		public double PlannedHeatLoadPerSqMResidence {
			get { return this.plannedQResidence; }
		}

		public override double PlannedHeatLoad {
			get { return Math.Round(plannedQ * PlannedFloorArea, 1); }
		}

		public double PlannedHeatLoadRim {
			get { return Math.Round(this.PlannedAreaRim * this.plannedQRim, 0); }
		}

		public double PlannedHeatLoadResidence {
			get { return Math.Round(this.PlannedAreaResidence * this.plannedQResidence, 0); }
		}

		public override double PlannedCoolLoad {
			get { return 0; }
		}

		[XmlIgnore]
		public Construction PlannedFloorConstruction {
			get { return this.floorConstruction; }
			set { this.floorConstruction = value;  }
		}

		[XmlIgnore]
		public Construction PlannedInsulationConstruction {
			get { return this.insulationConstruction; }
			set { this.insulationConstruction = value; }
		}

		public string PlannedFloorConstructionId {
			get { return (this.floorConstruction == null ? "" : this.floorConstruction.Id); }
			set { this.floorConstruction = Project.Instance.Config.GetConstruction(value); }
		}

		public string PlannedInsulationConstructionId {
			get { return (this.insulationConstruction == null ? "" : this.insulationConstruction.Id); }
			set { this.insulationConstruction = Project.Instance.Config.GetConstruction(value); }
		}

		public float PlannedFloorConstructionRValue {
			get { return (this.floorConstruction == null ? 0 : this.floorConstruction.RValue); }
		}

		public float PlannedInsulationConstructionRValue {
			get { return (this.insulationConstruction == null ? 0 : this.insulationConstruction.RValue); }
		}

		[XmlIgnore]
		public float PlannedFloorAreaPercentage {
			get {
				return (this.AvailableFloorArea <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AvailableFloorArea);
			}
			set {
				this.PlannedFloorArea = (float)(this.AvailableFloorArea * value / 100);
			}
		}

		public float AvailableFloorArea {
			get {
				Room room = this.AssociatedRoom;
				float area = room.Area;
				foreach (PlannedProduct product in room.PlannedProducts) {
					if (product.Product != this) {
						area -= product.Product.PlannedFloorArea;
					}
				}
				if (area < 0) {
					area = 0;
				}
				return area;
			}
		}

		public float PlannedAreaReduced {
			get { return this.plannedAreaReduced; }
			set { this.plannedAreaReduced = value; }
		}

		public float PlannedAreaUnheated {
			get { return this.plannedAreaUnheated; }
			set { this.plannedAreaUnheated = value; }
		}

		public float PlannedAreaRim {
			get {
				if (this.plannedRimType.HasValue) {
					float rimWidth = ((float)GetWidthRim(this.plannedRimType.Value)) / 100.0f;
					float realRimLength = this.plannedRim - rimWidth * this.plannedCornersRim;
					return realRimLength * rimWidth; 
				} else {
					return 0;
				}
			}
		}

		public float PlannedAreaResidence {
			get { return this.PlannedFloorArea - this.PlannedAreaRim; }
		}

		public Nullable<RimType> PlannedRimType {
			get { return this.plannedRimType; }
			set { this.plannedRimType = value; }
		}

		public Nullable<RimType> RequestedRimType {
			get { return this.requestedRimType; }
			set { this.requestedRimType = value; }
		}

		public Nullable<LayDistance> PlannedLayDistanceRim {
			get { return this.plannedRimType == null ? (Nullable<LayDistance>)null : (Nullable<LayDistance>)GetLayDistanceRim(this.plannedRimType.Value); }
		}

		public int PlannedRimWidth {
			get { return this.plannedRimType == null ? 0 : GetWidthRim(this.plannedRimType.Value); }
		}

		public float PlannedRim {
			get { return this.plannedRim; }
			set { this.plannedRim = value; }
		}

		public int PlannedCornersRim {
			get { return this.plannedCornersRim; }
			set { this.plannedCornersRim = value; }
		}

		public float PlannedRoomTemperatureBelow {
			get { return this.plannedRoomTemperatureBelow; }
			set { this.plannedRoomTemperatureBelow = value; }
		}

		public double PlannedDeltaRho {
			get { return this.plannedDeltaRho; }
		}

		public double PlannedPipeLength {
			get { return this.plannedPipeLength; }
		}
	}
	
}
