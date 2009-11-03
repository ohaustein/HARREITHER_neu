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

		public void Calculate(int maxModulCountInRow, double lengthVerbindeleitungen) {
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
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.ModulKlimaBodenProduct.GetHeatFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
				this.c_thetaVHeat = distributorVorlaufTemp;
				this.c_thetaRHeat = distributorRuecklaufTemp;
				this.c_thetaVHeat = this.c_thetaVHeat - (this.c_thetaVHeat - this.c_thetaRHeat) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRHeat = this.c_thetaRHeat + (this.c_thetaVHeat - this.c_thetaRHeat) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				
				double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVHeat, this.c_thetaRHeat, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature);

				double au = en1264.auFlaeche(ModulKlimaBodenProduct.ConfigAlpha0, ModulKlimaBodenProduct.ConfigAlphaFbh, su0, lambdaU0, ModulKlimaBodenProduct.ConfigSu, lambdaE);
				double ab = en1264.abFlaeche(B, au, atmt, rLambdaB);
				this.c_qHeatPerSqm = en1264.WaermestromDichteFlaeche(B, ab, atmt, au, dTheta);

				double qAverage = this.QHeat / this.AreaWithoutConnections;
				double qU = en1264.WaermeverlustUnten(ModulKlimaBodenProduct.ConfigAlphaFbh, rLambdaB, ModulKlimaBodenProduct.ConfigSu, lambdaU, rAlphaDeckeFbh, rLambdaIns, ModulKlimaBodenProduct.ConfigRLambdaDecke, ModulKlimaBodenProduct.ConfigRLambdaPutz, qAverage, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaBodenProduct.PlannedRoomTemperatureBelowHeat);

				// hydraulische Berechnung
				this.c_Qh2oHeat = (qAverage + qU) * this.AreaWithoutConnections;            // gesamte aufgenommene Leistung berechnen
				//                                                                           // gesamten Druckverlust berechnen

				foreach (ConnectionPipe cp in this.plannedProduct.Product.PlannedConnectionPipes) {
					if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
						double heatLoad;
						double qH2o;
						cp.CalculateHeatLoad(out heatLoad, out qH2o);
						this.c_Qh2oHeat += qH2o;
					}
				}
				double totalQh2o = this.c_Qh2oHeat;
				CircuitConnection cc = this.plannedProduct.Product.GetCircuitConnected(this.nrOfCircuit);
				if (cc != null) {
					totalQh2o += cc.OtherCircuit.C_Qh2oHeat;
				}
				cc = this.plannedProduct.Product.GetCircuitInverseConnected(this.nrOfCircuit);
				if (cc != null) {
					totalQh2o += cc.OtherCircuit.C_Qh2oHeat;
				}

				this.c_durchflussHeat = en1264.Durchfluss(totalQh2o, ModulKlimaBodenProduct.ConfigC, distributorVorlaufTemp - distributorRuecklaufTemp);

				this.c_druckverlustHeat = en1264.DruckverlustModul_100_40(maxModulCountInRow, this.c_durchflussHeat);
				this.c_druckverlustHeat += en1264.DruckverlustRohr(this.c_durchflussHeat, Product.rundrohr21mmInnenA, EurovalProduct.ConfigRho, Product.rundrohr21mmInnenD, EurovalProduct.ConfigV, 0.000004, lengthVerbindeleitungen);
				foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
					if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
						this.c_druckverlustHeat += cp.CalculateDruckverlust(this.c_durchflussHeat);
					}
				}

				this.c_floorTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, ModulKlimaBodenProduct.ConfigAlphaFbh, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature);
				
			}
			{ // Kühllastberechnung
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.ModulKlimaBodenProduct.GetCoolFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
				this.c_thetaVHeat = distributorVorlaufTemp;
				this.c_thetaRHeat = distributorRuecklaufTemp;
				double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVCool, this.c_thetaRCool, this.ModulKlimaBodenProduct.AssociatedRoom.RoomCoolTemperature);

				double au = en1264.auFlaeche(ModulKlimaBodenProduct.ConfigAlpha0, ModulKlimaBodenProduct.ConfigAlphaFbk, su0, lambdaU0, ModulKlimaBodenProduct.ConfigSu, lambdaE);
				double ab = en1264.abFlaeche(B, au, atmt, rLambdaB);
				this.c_qCoolPerSqm = en1264.WaermestromDichteFlaeche(B, ab, atmt, au, dTheta);

				double qAverage = this.QCool / this.AreaWithoutConnections;
				double qU = en1264.WaermeverlustUnten(ModulKlimaBodenProduct.ConfigAlphaFbh, rLambdaB, ModulKlimaBodenProduct.ConfigSu, lambdaU, rAlphaDeckeFbh, rLambdaIns, ModulKlimaBodenProduct.ConfigRLambdaDecke, ModulKlimaBodenProduct.ConfigRLambdaPutz, qAverage, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaBodenProduct.PlannedRoomTemperatureBelowHeat);

				// hydraulische Berechnung
				this.c_Qh2oCool = (qAverage + qU) * this.AreaWithoutConnections;            // gesamte aufgenommene Leistung berechnen
				//                                                                           // gesamten Druckverlust berechnen

				foreach (ConnectionPipe cp in this.plannedProduct.Product.PlannedConnectionPipes) {
					if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
						double coolLoad;
						double qH2o;
						cp.CalculateCoolLoad(out coolLoad, out qH2o);
						this.c_Qh2oCool -= qH2o;
					}
				}
				double totalQh2o = this.c_Qh2oCool;
				CircuitConnection cc = this.plannedProduct.Product.GetCircuitConnected(this.nrOfCircuit);
				if (cc != null) {
					totalQh2o += cc.OtherCircuit.C_Qh2oCool;
				}
				cc = this.plannedProduct.Product.GetCircuitInverseConnected(this.nrOfCircuit);
				if (cc != null) {
					totalQh2o += cc.OtherCircuit.C_Qh2oCool;
				}

				this.c_durchflussCool = en1264.Durchfluss(totalQh2o, ModulKlimaBodenProduct.ConfigC, distributorVorlaufTemp - distributorRuecklaufTemp);

				this.c_druckverlustCool = en1264.DruckverlustModul_100_40(maxModulCountInRow, this.c_durchflussCool);
				this.c_druckverlustCool += en1264.DruckverlustRohr(this.c_durchflussCool, Product.rundrohr21mmInnenA, EurovalProduct.ConfigRho, Product.rundrohr21mmInnenD, EurovalProduct.ConfigV, 0.000004, lengthVerbindeleitungen);
				foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
					if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
						this.c_druckverlustCool += cp.CalculateDruckverlust(this.c_durchflussCool);
					}
				}

				this.c_floorTempCool = en1264.OberflaechenTemperatur(this.c_qCoolPerSqm, ModulKlimaBodenProduct.ConfigAlphaFbk, this.ModulKlimaBodenProduct.AssociatedRoom.RoomCoolTemperature);
		
			}
		}
	}
}
