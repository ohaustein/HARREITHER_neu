using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	public class ModulDeckeCircuit : Circuit {

		//private List<KlimaFlaechenList> rows = new List<KlimaFlaechenList>();
		private List<ModulDeckeSubArea> subAreas = new List<ModulDeckeSubArea>();

		public ModulDeckeCircuit() {
			// A circuit needs to have at least one subarea so add this subarea by default,
			// if this circuit is deserialized this subarea will be deleted again in FinalizeLoading
			this.subAreas.Add(new ModulDeckeSubArea());
		}
	
		//public List<KlimaFlaechenList> Rows {
		//	get { return rows; }
		//	set { rows = value; }
		//}

		public List<ModulDeckeSubArea> SubAreas {
			get { return this.subAreas; }
			set { this.subAreas = value; }
		}

		[XmlIgnore]
		public ModulKlimaDeckeProduct ModulKlimaDeckeProduct {
			get { return this.PlannedProduct.Product as ModulKlimaDeckeProduct; }
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
		/// Summe der Flächen der einzelnen Module
		/// </summary>
		[XmlIgnore]
		public double ModulArea {
			get {
				double area = 0;
				foreach (ModulDeckeSubArea subArea in subAreas) {
					area += subArea.ModulArea;
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

		//private double c_floorTempHeat;
		//[XmlIgnore]
		//public double C_FloorTempHeat {
		//    get { return this.c_floorTempHeat; }
		//}

		//private double c_floorTempCool;
		//[XmlIgnore]
		//public double C_FloorTempCool {
		//    get { return this.c_floorTempCool; }
		//}

		private double c_thetaVHeat;
		private double c_thetaRHeat;

		private double c_thetaVCool;
		private double c_thetaRCool;

		[XmlIgnore]
		public override double PipeLengthWithoutConnections {
			get {
				double length = 0;
				foreach (ModulDeckeSubArea subArea in this.subAreas) {
					length += subArea.EquivalentPipeLength;
				}
				return length;
			}
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
			get { return this.c_qHeatPerSqm * this.ModulArea; }
		}

		[XmlIgnore]
		public double QCool {
			get { return -this.c_qCoolPerSqm * this.ModulArea; }
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

			double atmt = ModulKlimaDeckeProduct.ConfigAtmt;
			double b = ModulKlimaDeckeProduct.ConfigB;
			double c = ModulKlimaDeckeProduct.ConfigC;
			double alpha0 = ModulKlimaDeckeProduct.ConfigAlpha0;
			double alphaDh = ModulKlimaDeckeProduct.ConfigAlphaDh;
			double alphaDk = ModulKlimaDeckeProduct.ConfigAlphaDk;
			double su0 = ModulKlimaDeckeProduct.ConfigSu0;
			//double su = ModulKlimaDeckeProduct.ConfigSu;
			double lambdaU0 = ModulKlimaDeckeProduct.ConfigLambdaU0;
			//double lambdaU = ModulKlimaDeckeProduct.ConfigLambdaU;
			//double lambdaE = ModulKlimaDeckeProduct.ConfigLambdaE;
			double rLambdaDecke = ModulKlimaDeckeProduct.ConfigRLambdaDecke;
			double rLambdaDach = ModulKlimaDeckeProduct.ConfigRLambdaDach;
			double rAlphaDeckeDh = 1 / alphaDk; /* Wärmeübergang Decke bei Heizung */
			double rAlphaDeckeDk = 1 / alphaDh; /* Wärmeübergang Decke bei Kühlung */

			double leistungsFaktor = ModulKlimaDeckeProduct.ConfigLeistungsFaktor;

			double rLambdaB = 0;
			double rLambdaIns = this.ModulKlimaDeckeProduct.PlannedInsulationConstruction == null ? 0 : this.ModulKlimaDeckeProduct.PlannedInsulationConstruction.RValue;

			double su = this.ModulKlimaDeckeProduct.PlannedCeilingConstruction == null ? 0 : this.ModulKlimaDeckeProduct.PlannedCeilingConstruction.Thickness / 1000;
			double lambdaE = this.ModulKlimaDeckeProduct.PlannedCeilingConstruction == null ? 0 : this.ModulKlimaDeckeProduct.PlannedCeilingConstruction.LambdaValue;
			double lambdaU = lambdaE;

			{ // Heizlastberechnung
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.ModulKlimaDeckeProduct.GetHeatFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
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

					double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVHeat, this.c_thetaRHeat, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomHeatTemperature);

					double au = en1264.auFlaeche(alpha0, alphaDh, su0, lambdaU0, su, lambdaE);
					double ab = en1264.abFlaeche(b, au, atmt, rLambdaB);
					this.c_qHeatPerSqm = en1264.WaermestromDichteFlaeche(b, ab, atmt, au, dTheta) * leistungsFaktor;

					double qU = en1264.WaermeverlustAussen(alphaDh, rLambdaB, su, lambdaU, rAlphaDeckeDh, rLambdaIns, rLambdaDecke, rLambdaDach, this.c_qHeatPerSqm, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaDeckeProduct.PlannedRoomTemperatureBelowHeat);

					// hydraulische Berechnung
					this.c_Qh2oHeat = (this.c_qHeatPerSqm + qU) * this.ModulArea;            // gesamte aufgenommene Leistung berechnen
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

					this.c_massenstromHeat = en1264.Massenstrom(totalQh2o, c, distributorVorlaufTemp - distributorRuecklaufTemp);

					this.c_druckverlustHeat = 0;
					foreach (ModulDeckeSubArea subArea in this.subAreas) {
						this.c_druckverlustHeat += subArea.Druckverlust(this.c_massenstromHeat);
					}
					/*foreach (KlimaFlaechenList row in rows) {
						double rowDruckverlust = row.Druckverlust(this.c_durchflussHeat / rows.Count);
						if (rowDruckverlust > this.c_druckverlustHeat) {
							this.c_druckverlustHeat = rowDruckverlust;
						}
					}*/
					foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							this.c_druckverlustHeat += cp.CalculateDruckverlust(this.c_massenstromHeat);
						}
					}

					//this.c_floorTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, ModulKlimaDeckeProduct.ConfigAlphaFbh, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomHeatTemperature);
				}
			}
			{ // Kühllastberechnung
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.ModulKlimaDeckeProduct.GetCoolFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
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

					double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVCool, this.c_thetaRCool, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomCoolTemperature);

					double au = en1264.auFlaeche(alpha0, alphaDk, su0, lambdaU0, su, lambdaE);
					double ab = en1264.abFlaeche(b, au, atmt, rLambdaB);
					this.c_qCoolPerSqm = en1264.WaermestromDichteFlaeche(b, ab, atmt, au, dTheta) * leistungsFaktor;

					double qU = en1264.WaermeverlustAussen(alphaDh, rLambdaB, su, lambdaU, rAlphaDeckeDh, rLambdaIns, rLambdaDecke, rLambdaDach, this.c_qCoolPerSqm, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaDeckeProduct.PlannedRoomTemperatureBelowHeat);

					// hydraulische Berechnung
					this.c_Qh2oCool = (this.c_qCoolPerSqm + qU) * this.ModulArea;            // gesamte aufgenommene Leistung berechnen
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

					this.c_massenstromCool = en1264.Massenstrom(totalQh2o, c, distributorVorlaufTemp - distributorRuecklaufTemp);

					this.c_druckverlustCool = 0;
					foreach (ModulDeckeSubArea subArea in this.subAreas) {
						this.c_druckverlustCool += subArea.Druckverlust(this.c_massenstromCool);
					}
					/*foreach (KlimaFlaechenList row in rows) {
						double rowDruckverlust = row.Druckverlust(this.c_durchflussCool / rows.Count);
						if (rowDruckverlust > this.c_druckverlustCool) {
							this.c_druckverlustCool = rowDruckverlust;
						}
					}*/
					foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							this.c_druckverlustCool += cp.CalculateDruckverlust(this.c_massenstromCool);
						}
					}

					//this.c_floorTempCool = en1264.OberflaechenTemperatur(this.c_qCoolPerSqm, ModulKlimaDeckeProduct.ConfigAlphaFbk, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomCoolTemperature);
				}
			}
		}

		internal override void FinalizeLoading() {
			base.FinalizeLoading();
			// If this circuit is deserialized remove the subarea that was added by default
			if (this.SubAreas.Count > 0) {
				this.SubAreas.RemoveAt(0);
			}
			foreach (ModulDeckeSubArea sa in this.SubAreas) {
				sa.FinalizeLoading();
			}
		}
	}
}
