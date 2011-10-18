using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	public class HithermCircuit : Circuit, IWallCircuit<HithermCircuit, GraphicalHithermVerbindung, HithermRegister> {

		private List<HithermRegister> registers = new List<HithermRegister>();
		private List<GraphicalHithermVerbindung> links = new List<GraphicalHithermVerbindung>();

		private double graphicalAdditionalVl = 0;
		private double graphicalAdditionalRl = 0;

		public HithermCircuit() {

		}

		public double GraphicalAdditionalVl {
			get { return this.graphicalAdditionalVl; }
			set { this.graphicalAdditionalVl = value; }
		}

		public double GraphicalAdditionalRl {
			get { return this.graphicalAdditionalRl; }
			set { this.graphicalAdditionalRl = value; }
		}

		public List<HithermRegister> Registers {
			get { return this.registers; }
			set { this.registers = value; }
		}

		public List<GraphicalHithermVerbindung> Links {
			get { return this.links; }
			set { this.links = value; }
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

		[XmlIgnore]
		public int HkLabelNr {
			get {
				return this.registers.Count == 0 ? -1 : this.registers[0].Heizkreis;
			}
		}

		#region Area
		/// <summary>
		/// Summe der Flächen der einzelnen Register
		/// </summary>
		[XmlIgnore]
		public double CoveredArea {
			get {
				double area = 0;
				foreach (HithermRegister register in this.registers) {
					area += register.CoveredArea;
				}
				return area;
			}
		}

		[XmlIgnore]
		public double HeatArea {
			get {
				double area = 0;
				foreach (HithermRegister register in this.registers) {
					area += register.HeatArea;
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

		[XmlIgnore]
		public double C_QHeatPerSqm {
			get { return c_qHeatPerSqm; }
			//set { c_qHeatPerSqm = value; }
		}

		[XmlIgnore]
		public double C_QCoolPerSqm {
			get { return c_qCoolPerSqm; }
			//set { c_qCoolPerSqm = value; }
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
				return this.PipeLengthWithoutConnections + this.vorlaufTotal + this.ruecklaufTotal + length + this.graphicalAdditionalVl + this.graphicalAdditionalRl;
			}
		}

		[XmlIgnore]
		public override double PipeLengthWithUnisolatedConnections {
			get { return this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated; }
		}

		[XmlIgnore]
		public double QHeat {
			get { return this.c_qHeatPerSqm * this.HeatArea; }
		}

		[XmlIgnore]
		public double QCool {
			get { return -this.c_qCoolPerSqm * this.HeatArea; }
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

			double alphaInnenHeat = 8;
			double alphaAussenHeat = 8;
			double alphaInnenCool = 8;
			double alphaAussenCool = 8;
			switch (this.HithermProduct.HithermType) {
				case Product.ProductType.FBH:
					alphaInnenHeat = Product.ConfigAlphaBodenHeat;
					alphaAussenHeat = Product.ConfigAlphaDeckeHeat;
					alphaInnenCool = Product.ConfigAlphaBodenCool;
					alphaAussenCool = Product.ConfigAlphaDeckeCool;
					break;

				case Product.ProductType.DH:
					alphaInnenHeat = Product.ConfigAlphaDeckeHeat;
					alphaAussenHeat = Product.ConfigAlphaBodenHeat;
					alphaInnenCool = Product.ConfigAlphaDeckeCool;
					alphaAussenCool = Product.ConfigAlphaBodenCool;
					break;

				default:
					alphaInnenHeat = Product.ConfigAlphaWandHeat;
					alphaAussenHeat = Product.ConfigAlphaWandHeat;
					alphaInnenCool = Product.ConfigAlphaWandCool;
					alphaAussenCool = Product.ConfigAlphaWandCool;
					break;
			}

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
					this.c_massenstromHeat = 0;
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
						double heatLoad = reg.Heizleistung(heizmittelTemp, this.HithermProduct.AssociatedRoom.RoomHeatTemperature, alphaInnenHeat);
						heatLoadRegisters += heatLoad;
						qU += reg.WaermeverlustAussen(heatLoad, this.HithermProduct.AssociatedRoom.RoomCoolTemperature, alphaAussenHeat, alphaInnenHeat);
					}
					this.c_qHeatPerSqm = heatLoadRegisters / this.HeatArea;
					qU = qU / this.HeatArea;

					// hydraulische Berechnung
					this.c_Qh2oHeat = (this.c_qHeatPerSqm + qU) * this.HeatArea;            // gesamte aufgenommene Leistung berechnen
					//                                                                           // gesamten Druckverlust berechnen

					foreach (ConnectionPipe cp in this.plannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							double heatLoad;
							double qH2o;
							cp.CalculateHeatLoad(out heatLoad, out qH2o, this.NrOfCircuit);
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

					this.c_massenstromHeat = en1264.Massenstrom(totalQh2o, Europlan.Common.HithermProduct.ConfigC, distributorVorlaufTemp - distributorRuecklaufTemp);
					this.c_flussGeschwindigkeitHeat = en1264.FlussGeschwindigkeit(C_DurchflussHeat, Product.rundrohr21mmInnenA);
					this.c_druckverlustHeat = 0;
					foreach (HithermRegister reg in this.registers) {
						this.c_druckverlustHeat += reg.Druckverlust(this.c_massenstromHeat);
					}
					foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							this.c_druckverlustHeat += cp.CalculateDruckverlust(this.c_massenstromHeat);
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
					this.c_massenstromCool = 0;
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
						double coolLoad = reg.Kuehlleistung(kuehlmittelTemp, this.HithermProduct.AssociatedRoom.RoomCoolTemperature, alphaInnenCool);
						coolLoadRegisters += coolLoad;
						qU += reg.KaelteverlustHinten(coolLoad, this.HithermProduct.AssociatedRoom.RoomCoolTemperature, alphaAussenCool, alphaInnenCool);
					}
					this.c_qCoolPerSqm = coolLoadRegisters / this.HeatArea;
					qU = qU / this.HeatArea;

					// hydraulische Berechnung
					this.c_Qh2oCool = (this.c_qCoolPerSqm + qU) * this.HeatArea;            // gesamte aufgenommene Leistung berechnen
					//                                                                           // gesamten Druckverlust berechnen

					foreach (ConnectionPipe cp in this.plannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							double coolLoad;
							double qH2o;
							cp.CalculateCoolLoad(out coolLoad, out qH2o, this.NrOfCircuit);
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

					this.c_massenstromCool = en1264.Massenstrom(totalQh2o, Europlan.Common.HithermProduct.ConfigC, distributorVorlaufTemp - distributorRuecklaufTemp);
					this.c_flussGeschwindigkeitCool = en1264.FlussGeschwindigkeit(C_DurchflussCool, Product.rundrohr21mmInnenA);

					this.c_druckverlustCool = 0;
					foreach (HithermRegister reg in this.registers) {
						this.c_druckverlustCool += reg.Druckverlust(this.c_massenstromCool);
					}
					foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							this.c_druckverlustCool += cp.CalculateDruckverlust(this.c_massenstromCool);
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
					bereinigung += reg.HeizleistungBereinigung(this.HithermProduct.AssociatedRoom.RoomHeatTemperature);
				}
				return bereinigung;
			}
		}

		[XmlIgnore]
		public double KuehlleistungBereinigung {
			get {
				double bereinigung = 0;
				foreach (HithermRegister reg in this.registers) {
					bereinigung += reg.KuehlleistungBereinigung(this.HithermProduct.AssociatedRoom.RoomCoolTemperature);
				}
				return bereinigung;
			}
		}

		public override double CircuitArea {
			get { return this.CoveredArea; }
		}

		public bool IsConnectionAvailable(HithermRegister register, bool input) {
			foreach (GraphicalHithermVerbindung link in this.links) {
				if (input) {
					if (link.End == register) {
						return false;
					}
				} else {
					if (link.Start == register) {
						return false;
					}
				}
			}
			return true;
		}

		public List<HithermRegister> GetAllConnectedRegisters(HithermRegister register) {
			List<HithermRegister> connectedRegisters = new List<HithermRegister>();
			HithermRegister cur = register;
			while (cur != null) {
				connectedRegisters.Add(cur);
				cur = GetNextConnectedRegister(cur);
			}
			cur = GetPreviousConnectedRegister(cur);
			while (cur != null) {
				connectedRegisters.Add(cur);
				cur = GetPreviousConnectedRegister(cur);
			}
			return connectedRegisters;
		}

		public HithermRegister GetNextConnectedRegister(HithermRegister register) {
			GraphicalHithermVerbindung link = this.GetOutputLink(register);
			if (link != null) {
				return link.End;
			}
			return null;
		}

		public HithermRegister GetPreviousConnectedRegister(HithermRegister register) {
			GraphicalHithermVerbindung link = this.GetInputLink(register);
			if (link != null) {
				return link.Start;
			}
			return null;
		}

		public GraphicalHithermVerbindung GetOutputLink(HithermRegister register) {
			if (register == null) {
				return null;
			}
			foreach (GraphicalHithermVerbindung link in this.Links) {
				if (link.Start == register) {
					return link;
				}
			}
			return null;
		}

		public GraphicalHithermVerbindung GetInputLink(HithermRegister register) {
			if (register == null) {
				return null;
			}
			foreach (GraphicalHithermVerbindung link in this.Links) {
				if (link.End == register) {
					return link;
				}
			}
			return null;
		}

		public bool IsConnectedToGround(bool checkVorlauf, bool checkRuecklauf) {
			bool vorlaufConnected = false;
			bool ruecklaufConnected = false;
			foreach (GraphicalHithermVerbindung link in this.Links) {
				if (!link.HasStart) {
					ruecklaufConnected = true;
					if (vorlaufConnected) {
						break;
					}
				}
				if (!link.HasEnd) {
					vorlaufConnected = true;
					if (ruecklaufConnected) {
						break;
					}
				}
			}
			if (!checkVorlauf && !checkRuecklauf) {
				return vorlaufConnected || ruecklaufConnected;
			}
			return (vorlaufConnected || !checkVorlauf) && (ruecklaufConnected || !checkRuecklauf);
		}
	}
}
