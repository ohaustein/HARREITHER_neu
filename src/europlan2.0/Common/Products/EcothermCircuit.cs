using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class EcothermCircuit : Circuit {
		[XmlIgnore]
		public EcothermProduct EcothermProduct {
			get { return this.PlannedProduct.Product as EcothermProduct; }
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

		private double areaReduced;
		[XmlIgnore]
		public double AreaReduced {
			get { return this.areaReduced; }
			set { this.areaReduced = value; }
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

		private double rimLength;
		[XmlIgnore]
		public double RimLength {
			get { return this.rimLength; }
			set { this.rimLength = value; }
		}

		private double rimCorners;
		[XmlIgnore]
		public double RimCorners {
			get { return this.rimCorners; }
			set { this.rimCorners = value; }
		}
		#endregion Area

		private double c_areaAz;
		private double c_areaRz;
		private double c_pipeLengthAz;
		private double c_pipeLengthRz;

		private double c_qAzHeatPerSqm;
		private double c_qRzHeatPerSqm;

		private double c_qAzCoolPerSqm;
		private double c_qRzCoolPerSqm;

		private double c_floorTempAzHeat;
		[XmlIgnore]
		public double C_FloorTempAzHeat {
			get { return this.c_floorTempAzHeat; }
		}

		private double c_floorTempRzHeat;
		[XmlIgnore]
		public double C_FloorTempRzHeat {
			get { return this.c_floorTempRzHeat; }
		}

		private double c_floorTempAzCool;
		[XmlIgnore]
		public double C_FloorTempAzCool {
			get { return this.c_floorTempAzCool; }
		}

		private double c_floorTempRzCool;
		[XmlIgnore]
		public double C_FloorTempRzCool {
			get { return this.c_floorTempRzCool; }
		}

		private double c_thetaVRzHeat;
		private double c_thetaRRzHeat;
		private double c_thetaVAzHeat;
		private double c_thetaRAzHeat;

		private double c_thetaVRzCool;
		private double c_thetaRRzCool;
		private double c_thetaVAzCool;
		private double c_thetaRAzCool;



		[XmlIgnore]
		public override double PipeLengthWithoutConnections {
			get { return this.c_pipeLengthAz + this.c_pipeLengthRz; }
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
		public double QAzHeat {
			get { return this.c_qAzHeatPerSqm * this.c_areaAz; }
		}

		[XmlIgnore]
		public double QRzHeat {
			get { return this.c_qRzHeatPerSqm * this.c_areaRz; }
		}

		[XmlIgnore]
		public double QAzCool {
			get { return -this.c_qAzCoolPerSqm * this.c_areaAz; }
		}

		[XmlIgnore]
		public double QRzCool {
			get { return -this.c_qRzCoolPerSqm * this.c_areaRz; }
		}

		[XmlIgnore]
		public double QFbhTotalHeat {
			get { return this.QAzHeat + this.QRzHeat; }
		}

		[XmlIgnore]
		public double QFbhTotalCool {
			get { return this.QAzCool + this.QRzCool; }
		}

		public double GetAreaRim(Nullable<Europlan.Common.EcothermProduct.EcothermRimType> rimType) {
			if (!rimType.HasValue) {
				return 0;
			}

			double bRz = ((float)EcothermProduct.GetRimWidth(rimType.Value)) / 100;
			double lRz = this.rimLength + this.rimCorners * bRz;
			lRz = lRz < 0 ? 0 : lRz;
			
			return lRz * bRz;
		}

		public void Calculate(Europlan.Common.EcothermProduct.EcothermLayDistance layDistance, Nullable<Europlan.Common.EcothermProduct.EcothermRimType> rimType) {

			if (layDistance == EcothermProduct.EcothermLayDistance.NONE) {
				c_Qh2oHeat = 0;

				c_Qh2oCool = 0;

				c_druckverlustHeat = 0;
				c_flussGeschwindigkeitHeat = 0;

				c_massenstromHeat = 0;

				c_druckverlustCool = 0;

				c_massenstromCool = 0;
				c_flussGeschwindigkeitCool = 0;

				c_areaAz = 0;
				c_areaRz = 0;
				c_pipeLengthAz = 0;
				c_pipeLengthRz = 0;

				c_qAzHeatPerSqm = 0;
				c_qRzHeatPerSqm = 0;

				c_qAzCoolPerSqm = 0;
				c_qRzCoolPerSqm = 0;

				c_floorTempAzHeat = 0;

				c_floorTempRzHeat = 0;

				c_floorTempAzCool = 0;

				c_floorTempRzCool = 0;

				c_thetaVRzHeat = 0;
				c_thetaRRzHeat = 0;
				c_thetaVAzHeat = 0;
				c_thetaRAzHeat = 0;

				c_thetaVRzCool = 0;
				c_thetaRRzCool = 0;
				c_thetaVAzCool = 0;
				c_thetaRAzCool = 0;

				return;
			}

			EN1264 en1264 = EN1264.Instance;

			double c = EcothermProduct.ConfigC;
			double v = EcothermProduct.ConfigV;
			double rho = EcothermProduct.ConfigRho;
			double ag = EcothermProduct.ConfigAgActivated ? EcothermProduct.ConfigAg : 1;
			double sr0 = EcothermProduct.ConfigSr0;
			double sr = EcothermProduct.ConfigSr;
			double alpha0 = EcothermProduct.ConfigAlpha0;
			double alphaFbh = EcothermProduct.ConfigAlphaFbh;
			double alphaFbk = EcothermProduct.ConfigAlphaFbk;
			double su0 = EcothermProduct.ConfigSu0;
			double su = EcothermProduct.ConfigSu;
			double lambdaR0 = EcothermProduct.ConfigLambdaR0;
			double lambdaR = EcothermProduct.ConfigLambdaR;
			double lambdaU0 = EcothermProduct.ConfigLambdaU0;
			double lambdaU = EcothermProduct.ConfigLambdaU;
			double lambdaE = EcothermProduct.ConfigLambdaE;
			double rLambdaDecke = EcothermProduct.ConfigRLambdaDecke;
			double rLambdaPutz = EcothermProduct.ConfigRLambdaPutz;
			double rAlphaDeckeFbh = 1 / Product.ConfigAlphaDeckeHeat; /* Wärmeübergang Decke bei Heizung */
			double rAlphaDeckeFbk = 1 / Product.ConfigAlphaDeckeCool; /* Wärmeübergang Decke bei Kühlung */
			double rohrAussenD = EcothermProduct.ConfigRohrAussenD;
			double rohrInnenD = EcothermProduct.ConfigRohrInnenD;
			double rohrInnenA = EcothermProduct.ConfigRohrInnenA;

			double rLambdaB = this.EcothermProduct.PlannedInsideConstructionRValue;
			double rLambdaIns = this.EcothermProduct.PlannedOutsideConstructionRValue;

			double factor = (this.EcothermProduct.PlannedFloorConstruction != null && (this.EcothermProduct.PlannedFloorConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TROCKEN) || this.EcothermProduct.PlannedFloorConstruction.Type == ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TROCKEN))) ? EcothermProduct.ConfigFaktorTrockenkonstruktion : 1;

			// Aufteilung RZ - AZ
			double aFbh = this.areaTotal - this.areaReduced / 2- this.areaUnheated - this.areaRemovedDueConnection;	// wirksam beheizte Fläche

			bool calculateWithRim = rimType.HasValue && (rimLength - this.rimCorners * EcothermProduct.GetRimWidth(rimType.Value) / 100 > 0);
			this.c_areaRz = 0;
			this.c_pipeLengthRz = 0;
			if (calculateWithRim) {
				this.c_areaRz = this.GetAreaRim(rimType);
				this.c_pipeLengthRz = this.c_areaRz * EcothermProduct.GetPipeLengthPerSqm(EcothermProduct.GetRimLayDistance(rimType.Value));  // Rohrlänge der Randzone berechnen
			}
			this.c_areaAz = aFbh - this.c_areaRz;                                                      // Fläche der Aufenthaltszone berechnen
			this.c_pipeLengthAz = (this.c_areaAz + this.areaReduced / 2) * EcothermProduct.GetPipeLengthPerSqm(layDistance);                           // Rohlänge der Aufenthaltszone berechnen

			{ // Heizlastberechnung
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.EcothermProduct.GetHeatFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
				this.c_thetaVRzHeat = distributorVorlaufTemp;
				this.c_thetaRAzHeat = distributorRuecklaufTemp;
				this.c_thetaVRzHeat = this.c_thetaVRzHeat - (this.c_thetaVRzHeat - this.c_thetaRAzHeat) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRAzHeat = this.c_thetaRAzHeat + (this.c_thetaVRzHeat - this.c_thetaRAzHeat) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRRzHeat = this.c_thetaVRzHeat;
				this.c_thetaVAzHeat = this.c_thetaVRzHeat;

				double dThetaRz = 0;

				if (calculateWithRim) {
					this.c_thetaRRzHeat = this.c_thetaVRzHeat - (this.c_thetaVRzHeat - this.c_thetaRAzHeat) * this.c_pipeLengthRz / this.PipeLengthWithoutConnections;
					this.c_thetaVAzHeat = this.c_thetaRRzHeat;
					dThetaRz = en1264.Heizmitteluebertemperatur(this.c_thetaVRzHeat, this.c_thetaRRzHeat, this.EcothermProduct.AssociatedRoom.RoomHeatTemperature);
					//                                                                        // Heizmittelübertemperatur der Randzone berechnen
				}
				double dThetaAz = en1264.Heizmitteluebertemperatur(this.c_thetaVAzHeat, this.c_thetaRAzHeat, this.EcothermProduct.AssociatedRoom.RoomHeatTemperature);
				//                                                                            // Heizmittelübertemperatur der Aufenthaltszone berechnen

				double tRz = 0;
				double ppRz = 0;
				double bgRz = 0;
				double khRz = 0;
				if (calculateWithRim) {
					tRz = EcothermProduct.GetTeilung(EcothermProduct.GetRimLayDistance(rimType.Value));    // Teilung der Randzone
					ppRz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, ag);
					//                                                                        // Potenzprodukt der Randzone berechnen
					bgRz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, ag, sr,sr0, lambdaR, lambdaR0);
					//                                                                        // systemabhängigen Koeffizienten der Randzone berechnen
					khRz = en1264.WaermedurchgangsKoeffizientRohr(bgRz, ppRz);                // Wärmedurchgangskoeffizient der Randzone berechnen
					this.c_qRzHeatPerSqm = en1264.WaermestromDichteRohr(khRz, dThetaRz) * factor;                       // in den Raum abgegebene Wärmeleistung der Randzone berechnen
				}

				double tAz = EcothermProduct.GetTeilung(layDistance);                             // Teilung der Aufenthaltszone
				double ppAz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, ag);
				//                                                                            // Potenzprodukt der Aufenthaltszone berechnen
				double bgAz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbh, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, ag, sr, sr0, lambdaR, lambdaR0);
				//                                                                            // systemabhängigen Koeffizienten der Aufenthaltszone berechnen
				double khAz = en1264.WaermedurchgangsKoeffizientRohr(bgAz, ppAz);             // Wärmedurchgangskoeffizient der Aufenthaltszone berechnen
				this.c_qAzHeatPerSqm = en1264.WaermestromDichteRohr(khAz, dThetaAz) * factor;                    // in den Raum abgegebene Wärmeleistung der Aufenthaltszone berechnen

				double qAverage = this.QFbhTotalHeat / this.AreaWithoutConnections;
				double qU = en1264.WaermeverlustAussen(alphaFbh, rLambdaB, su, lambdaU, rAlphaDeckeFbh, rLambdaIns, rLambdaDecke, rLambdaPutz, qAverage, this.EcothermProduct.AssociatedRoom.RoomHeatTemperature, this.EcothermProduct.PlannedRoomTemperatureBelowHeat);

				// hydraulische Berechnung
				this.c_Qh2oHeat = (qAverage + qU) * this.AreaWithoutConnections;            // gesamte aufgenommene Leistung berechnen
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

				this.c_massenstromHeat = en1264.Massenstrom(totalQh2o, c, distributorVorlaufTemp - distributorRuecklaufTemp);

				this.c_druckverlustHeat = en1264.DruckverlustRohr(this.c_massenstromHeat, rohrInnenA, rho, rohrInnenD, v, 0.000004, this.PipeLengthWithoutConnections);
				this.c_flussGeschwindigkeitHeat = en1264.FlussGeschwindigkeit(C_DurchflussHeat, rohrInnenA);
				foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
					if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
						this.c_druckverlustHeat += cp.CalculateDruckverlust(this.c_massenstromHeat);
					}
				}

				this.c_floorTempAzHeat = en1264.OberflaechenTemperatur(this.c_qAzHeatPerSqm, alphaFbh, this.EcothermProduct.AssociatedRoom.RoomHeatTemperature);
				if (calculateWithRim) {
					this.c_floorTempRzHeat = en1264.OberflaechenTemperatur(this.c_qRzHeatPerSqm, alphaFbh, this.EcothermProduct.AssociatedRoom.RoomHeatTemperature);
				} else {
					this.c_floorTempRzHeat = 0;
				}
			}
			{ // Kühllastberechnung
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.EcothermProduct.GetCoolFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
				this.c_thetaVRzCool = distributorVorlaufTemp;
				this.c_thetaRAzCool = distributorRuecklaufTemp;
				this.c_thetaVRzCool = this.c_thetaVRzCool - (this.c_thetaVRzCool - this.c_thetaRAzCool) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRAzCool = this.c_thetaRAzCool + (this.c_thetaVRzCool - this.c_thetaRAzCool) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRRzCool = this.c_thetaVRzCool;
				this.c_thetaVAzCool = this.c_thetaVRzCool;

				double dThetaRz = 0;

				if (calculateWithRim) {
					this.c_thetaRRzCool = this.c_thetaVRzCool - (this.c_thetaVRzCool - this.c_thetaRAzCool) * this.c_pipeLengthRz / this.PipeLengthWithoutConnections;
					this.c_thetaVAzCool = this.c_thetaRRzCool;
					dThetaRz = en1264.Heizmitteluebertemperatur(this.c_thetaVRzCool, this.c_thetaRRzCool, this.EcothermProduct.AssociatedRoom.RoomCoolTemperature);
					//                                                                        // Heizmittelübertemperatur der Randzone berechnen
				}
				double dThetaAz = en1264.Heizmitteluebertemperatur(this.c_thetaVAzCool, this.c_thetaRAzCool, this.EcothermProduct.AssociatedRoom.RoomCoolTemperature);
				//                                                                            // Heizmittelübertemperatur der Aufenthaltszone berechnen

				double tRz = 0;
				double ppRz = 0;
				double bgRz = 0;
				double khRz = 0;
				if (calculateWithRim) {
					tRz = EcothermProduct.GetTeilung(EcothermProduct.GetRimLayDistance(rimType.Value));    // Teilung der Randzone
					ppRz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, ag);
					//                                                                        // Potenzprodukt der Randzone berechnen
					bgRz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tRz, su, rohrAussenD, ag, sr, sr0, lambdaR, lambdaR0);
					//                                                                        // systemabhängigen Koeffizienten der Randzone berechnen
					khRz = en1264.WaermedurchgangsKoeffizientRohr(bgRz, ppRz);                // Wärmedurchgangskoeffizient der Randzone berechnen
					this.c_qRzCoolPerSqm = en1264.WaermestromDichteRohr(khRz, dThetaRz) * factor;                       // in den Raum abgegebene Wärmeleistung der Randzone berechnen
				}

				double tAz = EcothermProduct.GetTeilung(layDistance);                             // Teilung der Aufenthaltszone
				double ppAz = en1264.PotenzProduktFussbodenGeometrie(alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, ag);
				//                                                                            // Potenzprodukt der Aufenthaltszone berechnen
				double bgAz = en1264.SystemabhaengigerKoeffizientGeometrie(6.7, alpha0, alphaFbk, su0, lambdaU0, lambdaE, rLambdaB, tAz, su, rohrAussenD, ag, sr, sr0, lambdaR, lambdaR0);
				//                                                                            // systemabhängigen Koeffizienten der Aufenthaltszone berechnen
				double khAz = en1264.WaermedurchgangsKoeffizientRohr(bgAz, ppAz);             // Wärmedurchgangskoeffizient der Aufenthaltszone berechnen
				this.c_qAzCoolPerSqm = en1264.WaermestromDichteRohr(khAz, dThetaAz) * factor;                    // in den Raum abgegebene Wärmeleistung der Aufenthaltszone berechnen

				double qAverage = -this.QFbhTotalCool / this.AreaWithoutConnections;
				double qU = en1264.WaermeverlustAussen(alphaFbk, rLambdaB, su, lambdaU, rAlphaDeckeFbk, rLambdaIns, rLambdaDecke, rLambdaPutz, qAverage, this.EcothermProduct.AssociatedRoom.RoomCoolTemperature, this.EcothermProduct.PlannedRoomTemperatureBelowCool);

				// hydraulische Berechnung
				//this.c_Qh2oCool = (qAverage + qU) * this.AreaWithoutConnections;            // gesamte aufgenommene Leistung berechnen
				/*double deltaT = this.c_thetaVRzCool - this.c_thetaRAzCool;                                          // gesamte Spreizung
				this.c_durchflussCool = en1264.Durchfluss(this.c_Qh2oCool, EurovalProduct.ConfigC, deltaT);
				this.c_druckverlustCool = en1264.DruckverlustRohr(this.c_durchflussCool, EurovalProduct.ConfigRohrInnenA, EurovalProduct.ConfigRho, EurovalProduct.ConfigRohrInnenD, EurovalProduct.ConfigV, 0.000004, this.PipeLengthWithAllConnections);*/
				//                                                                           // gesamten Druckverlust berechnen


				this.c_Qh2oCool = (qAverage + qU) * this.AreaWithoutConnections;            // gesamte aufgenommene Leistung berechnen
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

				this.c_massenstromCool = en1264.Massenstrom(totalQh2o, c, distributorVorlaufTemp - distributorRuecklaufTemp);

				this.c_druckverlustCool = en1264.DruckverlustRohr(this.c_massenstromCool, rohrInnenA, rho, rohrInnenD, v, 0.000004, this.PipeLengthWithoutConnections);
				this.c_flussGeschwindigkeitCool = en1264.FlussGeschwindigkeit(C_DurchflussCool, rohrInnenA);
				foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
					if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
						this.c_druckverlustCool += cp.CalculateDruckverlust(this.c_massenstromCool);
					}
				}

				this.c_floorTempAzCool = en1264.OberflaechenTemperatur(this.c_qAzCoolPerSqm, alphaFbk, this.EcothermProduct.AssociatedRoom.RoomCoolTemperature);
				if (calculateWithRim) {
					this.c_floorTempRzCool = en1264.OberflaechenTemperatur(this.c_qRzCoolPerSqm, alphaFbk, this.EcothermProduct.AssociatedRoom.RoomCoolTemperature);
				} else {
					this.c_floorTempRzCool = 0;
				}
			}
		}
	}
}
