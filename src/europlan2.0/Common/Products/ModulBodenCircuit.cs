using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	public class ModulBodenCircuit : Circuit {

		[XmlIgnore]
		public ModulKlimaBodenProduct ModulKlimaBodenProduct {
			get { return this.PlannedProduct.Product as ModulKlimaBodenProduct; }
			set {
				foreach (Floor f in Project.Instance.Floors) {
					foreach (Room r in f.Rooms) {
						foreach (PlannedProduct pp in r.PlannedProducts) {
							if (pp.Product == value) {
								this.plannedProduct = pp;
							}
						}
					}
				}
			}
		}

		private PlannedProduct plannedProduct = null;

		[XmlIgnore]
		public override PlannedProduct PlannedProduct {
			get {
				return this.plannedProduct;
			}
		}

		#region Area
		private double areaTotal;
		[XmlIgnore]
		public double AreaTotal {
			get { return this.areaTotal; }
			set { this.areaTotal = value; }
		}

		private double areaUnheated;
		[XmlIgnore]
		public double AreaUnheated {
			get { return this.areaUnheated; }
			set { this.areaUnheated = value; }
		}

		private double areaRemovedDueConnection;
		[XmlIgnore]
		public double AreaRemovedDueConnection {
			get { return this.areaRemovedDueConnection; }
			set { this.areaRemovedDueConnection = value; }
		}

		[XmlIgnore]
		public double AreaWithoutConnections {
			get { return this.areaTotal - this.areaRemovedDueConnection; }
		}

		#endregion Area

		private double c_area;
		private double c_pipeLength;

		private double c_qHeatPerSqm;
		private double c_qCoolPerSqm;

		private double c_floorTempHeat;
		[XmlIgnore]
		public double C_FloorTempHeat {
			get { return this.c_floorTempHeat; }
		}

		private double c_floorTempCool;
		[XmlIgnore]
		public double C_FloorTempCool {
			get { return this.c_floorTempCool; }
		}

		private double c_druckverlustHeat;
		[XmlIgnore]
		public double C_DruckverlustHeat {
			get { return this.c_druckverlustHeat; }
		}

		private double c_durchflussHeat;
		[XmlIgnore]
		public double C_DurchflussHeat {
			get { return this.c_durchflussHeat; }
		}

		private double c_druckverlustCool;
		[XmlIgnore]
		public double C_DruckverlustCool {
			get { return this.c_druckverlustCool; }
		}

		private double c_durchflussCool;
		[XmlIgnore]
		public double C_DurchflussCool {
			get { return this.c_durchflussCool; }
		}

		private double c_thetaVHeat;
		private double c_thetaRHeat;

		private double c_thetaVCool;
		private double c_thetaRCool;

		[XmlIgnore]
		public override double PipeLengthWithoutConnections {
			get { return this.c_pipeLength; }
		}

		[XmlIgnore]
		public override double PipeLengthWithAllConnections {
			get { return this.PipeLengthWithoutConnections + this.vorlaufTotal + this.ruecklaufTotal; }
		}

		[XmlIgnore]
		public override double PipeLengthWithUnisolatedConnections {
			get { return this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated; }
		}

		[XmlIgnore]
		public double QHeat {
			get { return this.c_qHeatPerSqm * this.c_area; }
		}

		[XmlIgnore]
		public double QCool {
			get { return -this.c_qCoolPerSqm * this.c_area; }
		}

		[XmlIgnore]
		public double QFbhTotalHeat {
			get { return this.QHeat; }
		}

		[XmlIgnore]
		public double QFbhTotalCool {
			get { return this.QCool; }
		}

		public void Calculate(int modulCount, double lengthVerbindeleitungen) {
			EN1264 en1264 = EN1264.Instance;

			double su0 = 0.045; 
			double lambdaU0 = 1;
			double atmt = 1.06;
			double lambdaU = 60;
			double lambdaE = ModulKlimaBodenProduct.ConfigLambdaE;
			double rLambdaB = this.ModulKlimaBodenProduct.PlannedFloorConstruction == null ? 0 : this.ModulKlimaBodenProduct.PlannedFloorConstruction.RValue;
			double rLambdaIns = this.ModulKlimaBodenProduct.PlannedInsulationConstruction == null ? 0 : this.ModulKlimaBodenProduct.PlannedInsulationConstruction.RValue;
			double B = 6.5;

			double rAlphaDeckeFbh = 1 / EurovalProduct.ConfigAlphaFbk; /* Wärmeübergang Decke bei Heizung */
			double rAlphaDeckeFbk = 1 / EurovalProduct.ConfigAlphaFbh; /* Wärmeübergang Decke bei Kühlung */

			{ // Heizlastberechnung
				this.c_thetaVHeat = 35;
				this.c_thetaRHeat = 30;
				this.ModulKlimaBodenProduct.GetHeatFlow(out this.c_thetaVHeat, out this.c_thetaRHeat);
				this.c_thetaVHeat = this.c_thetaVHeat - (this.c_thetaVHeat - this.c_thetaRHeat) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRHeat = this.c_thetaRHeat + (this.c_thetaVHeat - this.c_thetaRHeat) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				
				double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVHeat, this.c_thetaRHeat, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature);

				double au = en1264.auFlaeche(ModulKlimaBodenProduct.ConfigAlpha0, ModulKlimaBodenProduct.ConfigAlphaFbh, su0, lambdaU0, ModulKlimaBodenProduct.ConfigSu, lambdaE);
				double ab = en1264.abFlaeche(B, au, atmt, rLambdaB);
				this.c_qHeatPerSqm = en1264.WaermestromDichteFlaeche(B, ab, atmt, au, dTheta);

				double qAverage = this.QHeat / this.AreaWithoutConnections;
				double qU = en1264.WaermeverlustUnten(ModulKlimaBodenProduct.ConfigAlphaFbh, rLambdaB, ModulKlimaBodenProduct.ConfigSu, lambdaU, rAlphaDeckeFbh, rLambdaIns, ModulKlimaBodenProduct.ConfigRLambdaDecke, ModulKlimaBodenProduct.ConfigRLambdaPutz, qAverage, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaBodenProduct.PlannedRoomTemperatureBelowHeat);

				// hydraulische Berechnung
				double qH2o = (qAverage + qU) * this.AreaWithoutConnections;            // gesamte aufgenommene Leistung berechnen
				double deltaT = this.c_thetaVHeat - this.c_thetaRHeat;                                          // gesamte Spreizung
				this.c_durchflussHeat = en1264.Durchfluss(qH2o, ModulKlimaBodenProduct.ConfigC, deltaT);
				this.c_druckverlustHeat = en1264.DruckverlustModul_100_40(modulCount, this.c_durchflussHeat);
				//                                                                           // gesamten Druckverlust berechnen

				this.c_floorTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, ModulKlimaBodenProduct.ConfigAlphaFbh, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature);
				
			}
			{ // Kühllastberechnung
				this.c_thetaVCool = 35;
				this.c_thetaRCool = 30;
				this.ModulKlimaBodenProduct.GetCoolFlow(out this.c_thetaVCool, out this.c_thetaRCool);
				this.c_thetaVCool = this.c_thetaVCool - (this.c_thetaVCool - this.c_thetaRCool) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRCool = this.c_thetaRCool + (this.c_thetaVCool - this.c_thetaRCool) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);

				double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVCool, this.c_thetaRCool, this.ModulKlimaBodenProduct.AssociatedRoom.RoomCoolTemperature);

				double au = en1264.auFlaeche(ModulKlimaBodenProduct.ConfigAlpha0, ModulKlimaBodenProduct.ConfigAlphaFbk, su0, lambdaU0, ModulKlimaBodenProduct.ConfigSu, lambdaE);
				double ab = en1264.abFlaeche(B, au, atmt, rLambdaB);
				this.c_qCoolPerSqm = en1264.WaermestromDichteFlaeche(B, ab, atmt, au, dTheta);

				double qAverage = this.QCool / this.AreaWithoutConnections;
				double qU = en1264.WaermeverlustUnten(ModulKlimaBodenProduct.ConfigAlphaFbh, rLambdaB, ModulKlimaBodenProduct.ConfigSu, lambdaU, rAlphaDeckeFbh, rLambdaIns, ModulKlimaBodenProduct.ConfigRLambdaDecke, ModulKlimaBodenProduct.ConfigRLambdaPutz, qAverage, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaBodenProduct.PlannedRoomTemperatureBelowHeat);

				// hydraulische Berechnung
				double qH2o = (qAverage + qU) * this.AreaWithoutConnections;            // gesamte aufgenommene Leistung berechnen
				double deltaT = this.c_thetaVCool - this.c_thetaRCool;                                          // gesamte Spreizung
				this.c_durchflussCool = en1264.Durchfluss(qH2o, ModulKlimaBodenProduct.ConfigC, deltaT);
				this.c_druckverlustCool = en1264.DruckverlustModul_100_40(modulCount, this.c_durchflussCool);
				//                                                                           // gesamten Druckverlust berechnen

				this.c_floorTempCool = en1264.OberflaechenTemperatur(this.c_qCoolPerSqm, ModulKlimaBodenProduct.ConfigAlphaFbk, this.ModulKlimaBodenProduct.AssociatedRoom.RoomCoolTemperature);
		
			}
		}
	}
}
