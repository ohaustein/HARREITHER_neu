using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	public class HithermCircuit : Circuit {

		private List<HithermRegister> registers = new List<HithermRegister>();

		public HithermCircuit() {

		}
	
		public List<HithermRegister> Registers {
			get { return this.registers; }
			set { this.registers = value; }
		}

		[XmlIgnore]
		public HithermProduct HithermProduct {
			get { return this.PlannedProduct.Product as HithermProduct; }
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
		/// <summary>
		/// Summe der Flächen der einzelnen Register
		/// </summary>
		[XmlIgnore]
		public double RegisterArea {
			get {
				double area = 0;
				foreach (HithermRegister register in this.registers) {
					area += register.Area;
				}
				return area;
			}
		}

		//private double areaTotal;
		//[XmlIgnore]
		//public double AreaTotal {
		//    get { return this.areaTotal; }
		//    set { this.areaTotal = value; }
		//}

		//private double areaUnheated;
		//[XmlIgnore]
		//public double AreaUnheated {
		//    get { return this.areaUnheated; }
		//    set { this.areaUnheated = value; }
		//}

		//private double areaRemovedDueConnection;
		//[XmlIgnore]
		//public double AreaRemovedDueConnection {
		//    get { return this.areaRemovedDueConnection; }
		//    set { this.areaRemovedDueConnection = value; }
		//}

		//[XmlIgnore]
		//public double AreaWithoutConnections {
		//    get { return this.areaTotal - this.areaRemovedDueConnection; }
		//}

		#endregion Area

		//private double c_area;
		//private double c_pipeLength;

		private double c_qHeatPerSqm;
		private double c_qCoolPerSqm;

		public double C_QHeatPerSqm {
			get { return c_qHeatPerSqm; }
			set { c_qHeatPerSqm = value; }
		}

		public double C_QCoolPerSqm {
			get { return c_qCoolPerSqm; }
			set { c_qCoolPerSqm = value; }
		}

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
			get {
				double length = 0;
				foreach (HithermRegister reg in this.registers) {
					length += reg.EquivalentPipeLengthUnisolated;
				}
				return length;
			}
		}

		[XmlIgnore]
		public override double PipeLengthWithAllConnections {
			get {
				double length = 0;
				foreach (HithermRegister reg in this.registers) {
					length += reg.PipeVertical + reg.PipeHorizontal;
				}
				return this.PipeLengthWithoutConnections + this.vorlaufTotal + this.ruecklaufTotal + length;
			}
		}

		[XmlIgnore]
		public override double PipeLengthWithUnisolatedConnections {
			get { return this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated; }
		}

		[XmlIgnore]
		public double QHeat {
			get { return this.c_qHeatPerSqm * this.RegisterArea; }
		}

		[XmlIgnore]
		public double QCool {
			get { return -this.c_qCoolPerSqm * this.RegisterArea; }
		}

		[XmlIgnore]
		public double QFbhTotalHeat {
			get { return this.QHeat; }
		}

		[XmlIgnore]
		public double QFbhTotalCool {
			get { return this.QCool; }
		}

		public void Calculate() {
			EN1264 en1264 = EN1264.Instance;

			//double su0 = 0.045; 
			//double lambdaU0 = 1;
			//double atmt = 1.06;
			//double lambdaU = 60;
			//double lambdaE = ModulKlimaBodenProduct.ConfigLambdaE;
			//double rLambdaB = this.ModulKlimaBodenProduct.PlannedFloorConstruction == null ? 0 : this.ModulKlimaBodenProduct.PlannedFloorConstruction.RValue;
			//double rLambdaIns = this.ModulKlimaBodenProduct.PlannedInsulationConstruction == null ? 0 : this.ModulKlimaBodenProduct.PlannedInsulationConstruction.RValue;
			//double B = 6.5;

			//double rAlphaDeckeFbh = 1 / EurovalProduct.ConfigAlphaFbk; /* Wärmeübergang Decke bei Heizung */
			//double rAlphaDeckeFbk = 1 / EurovalProduct.ConfigAlphaFbh; /* Wärmeübergang Decke bei Kühlung */

			{ // Heizlastberechnung
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.HithermProduct.GetHeatFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
				this.c_thetaVHeat = distributorVorlaufTemp;
				this.c_thetaRHeat = distributorRuecklaufTemp;
				this.c_thetaVHeat = this.c_thetaVHeat - (this.c_thetaVHeat - this.c_thetaRHeat) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRHeat = this.c_thetaRHeat + (this.c_thetaVHeat - this.c_thetaRHeat) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				if (c_thetaVHeat.Equals(double.NaN) || c_thetaRHeat.Equals(double.NaN)) {
					this.c_qHeatPerSqm = 0;
					this.c_durchflussHeat = 0;
					this.c_druckverlustHeat = 0;
					//this.c_floorTempHeat = 0;
				} else {

					//        double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVHeat, this.c_thetaRHeat, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature);

					//        double au = en1264.auFlaeche(ModulKlimaBodenProduct.ConfigAlpha0, ModulKlimaBodenProduct.ConfigAlphaFbh, su0, lambdaU0, ModulKlimaBodenProduct.ConfigSu, lambdaE);
					//        double ab = en1264.abFlaeche(B, au, atmt, rLambdaB);
					//        this.c_qHeatPerSqm = en1264.WaermestromDichteFlaeche(B, ab, atmt, au, dTheta);
					double heizmittelTemp = (this.c_thetaVHeat + this.c_thetaRHeat) / 2;
					double heatLoadRegisters = 0;
					double qU = 0; // TODO
					foreach (HithermRegister reg in this.registers) {
						double heatLoad = reg.Heizleistung(heizmittelTemp, this.HithermProduct.AssociatedRoom.RoomHeatTemperature);
						heatLoadRegisters += heatLoad;
						qU += reg.WaermeverlustHinten(heatLoad, this.HithermProduct.AssociatedRoom.RoomCoolTemperature);
					}
					this.c_qHeatPerSqm = heatLoadRegisters / this.RegisterArea;
					qU = qU / this.RegisterArea;

					// hydraulische Berechnung
					this.c_Qh2oHeat = (this.c_qHeatPerSqm + qU) * this.RegisterArea;            // gesamte aufgenommene Leistung berechnen
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

					this.c_durchflussHeat = en1264.Durchfluss(totalQh2o, Europlan.Common.HithermProduct.ConfigC, distributorVorlaufTemp - distributorRuecklaufTemp);

					this.c_druckverlustHeat = 0;
					foreach (HithermRegister reg in this.registers) {
						this.c_druckverlustHeat += reg.Druckverlust(this.c_durchflussHeat);
					}
					foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							this.c_druckverlustHeat += cp.CalculateDruckverlust(this.c_durchflussHeat);
						}
					}

					//this.c_floorTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, Europlan.Common.HithermProduct.ConfigAlphaFbh, this.HithermProduct.AssociatedRoom.RoomHeatTemperature);
				}
			}
			{
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.HithermProduct.GetCoolFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
				this.c_thetaVCool = distributorVorlaufTemp;
				this.c_thetaRCool = distributorRuecklaufTemp;
				this.c_thetaVCool = this.c_thetaVCool - (this.c_thetaVCool - this.c_thetaRCool) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRCool = this.c_thetaRCool + (this.c_thetaVCool - this.c_thetaRCool) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				if (c_thetaVCool.Equals(double.NaN) || c_thetaRCool.Equals(double.NaN)) {
					this.c_qCoolPerSqm = 0;
					this.c_durchflussCool = 0;
					this.c_druckverlustCool = 0;
					//this.c_floorTempCool = 0;
				} else {

					//        double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVHeat, this.c_thetaRHeat, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature);

					//        double au = en1264.auFlaeche(ModulKlimaBodenProduct.ConfigAlpha0, ModulKlimaBodenProduct.ConfigAlphaFbh, su0, lambdaU0, ModulKlimaBodenProduct.ConfigSu, lambdaE);
					//        double ab = en1264.abFlaeche(B, au, atmt, rLambdaB);
					//        this.c_qHeatPerSqm = en1264.WaermestromDichteFlaeche(B, ab, atmt, au, dTheta);
					double kuehlmittelTemp = (this.c_thetaVCool + this.c_thetaRCool) / 2;
					double coolLoadRegisters = 0;
					double qU = 0;
					foreach (HithermRegister reg in this.registers) {
						double coolLoad = reg.Kuehlleistung(kuehlmittelTemp, this.HithermProduct.AssociatedRoom.RoomCoolTemperature);
						coolLoadRegisters += coolLoad;
						qU += reg.KaelteverlustHinten(coolLoad, this.HithermProduct.AssociatedRoom.RoomCoolTemperature);
					}
					this.c_qCoolPerSqm = coolLoadRegisters / this.RegisterArea;
					qU = qU / this.RegisterArea;

					// hydraulische Berechnung
					this.c_Qh2oCool = (this.c_qCoolPerSqm + qU) * this.RegisterArea;            // gesamte aufgenommene Leistung berechnen
					//                                                                           // gesamten Druckverlust berechnen

					foreach (ConnectionPipe cp in this.plannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							double coolLoad;
							double qH2o;
							cp.CalculateCoolLoad(out coolLoad, out qH2o);
							this.c_Qh2oCool += qH2o;
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

					this.c_durchflussCool = en1264.Durchfluss(totalQh2o, Europlan.Common.HithermProduct.ConfigC, distributorVorlaufTemp - distributorRuecklaufTemp);

					this.c_druckverlustCool = 0;
					foreach (HithermRegister reg in this.registers) {
						this.c_druckverlustCool += reg.Druckverlust(this.c_durchflussCool);
					}
					foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							this.c_druckverlustCool += cp.CalculateDruckverlust(this.c_durchflussCool);
						}
					}

					//this.c_floorTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, Europlan.Common.HithermProduct.ConfigAlphaFbh, this.HithermProduct.AssociatedRoom.RoomHeatTemperature);
				}
			}
		}

		[XmlIgnore]
		public double HeizleistungBereinigung {
			get {
				double bereinigung = 0;
				foreach (HithermRegister reg in this.registers) {
					bereinigung += reg.HeizleistungBereinigung();
				}
				return bereinigung;
			}
		}

		[XmlIgnore]
		public double KuehlleistungBereinigung {
			get {
				double bereinigung = 0;
				foreach (HithermRegister reg in this.registers) {
					bereinigung += reg.KuehlleistungBereinigung();
				}
				return bereinigung;
			}
		}
	}
}
