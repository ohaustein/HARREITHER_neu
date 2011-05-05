using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Europlan.Common {

	public class ModulBodenCircuit : Circuit {

		private KlimaFlaechenList row = new KlimaFlaechenList();

		private int langeFittinge;
		private double sonstigeVerbindeleitung;
		private int sonstigeModule;
		private double reducedArea = 0;
		private Color circuitColor = Color.FromArgb(0, 128, 0);

		private List<KlimaFlaechenModulVerbindung> verbindungen = null;

		public ModulBodenCircuit() {

		}
	
		public KlimaFlaechenList Row {
			get { return row; }
			set { row = value; }
		}

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

		public int LangeFittinge {
			get { return this.langeFittinge; }
			set { this.langeFittinge = value; }
		}

		public double SonstigeVerbindeleitung {
			get { return this.sonstigeVerbindeleitung; }
			set { this.sonstigeVerbindeleitung = value; }
		}

		public double ReducedArea {
			get { return this.reducedArea; }
			set { reducedArea = value; }
		}

		#region Area
		/// <summary>
		/// Summe der Flächen der einzelnen Module
		/// </summary>
		[XmlIgnore]
		public double HeatArea {
			get { return row.GetHeatArea(true) + 0.02 * this.SonstigeVerbindeleitung; }
		}

		[XmlIgnore]
		private double HeatAreaForCalculation {
			get {
				if (this.reducedArea > this.HeatArea) {
					return this.HeatArea / 2;
				}
				return this.HeatArea - this.reducedArea / 2;
			}
		}

		[XmlIgnore]
		public double CoveredArea {
			get { return row.GetCoveredArea(true) + this.LangeFittinge * 0.15 + 0.055 * this.SonstigeVerbindeleitung; }
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
			get { return row.GetEquivalentPipeLength(true); }
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
			get { return this.c_qHeatPerSqm * this.HeatAreaForCalculation; }
		}

		[XmlIgnore]
		public double QCool {
			get { return -this.c_qCoolPerSqm * this.HeatAreaForCalculation; }
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

			double atmt = ModulKlimaBodenProduct.ConfigAtmt;
			double b = ModulKlimaBodenProduct.ConfigB;
			double c = ModulKlimaBodenProduct.ConfigC;
			double alpha0 = ModulKlimaBodenProduct.ConfigAlpha0;
			double alphaFbh = ModulKlimaBodenProduct.ConfigAlphaFbh;
			double alphaFbk = ModulKlimaBodenProduct.ConfigAlphaFbk;
			double su0 = ModulKlimaBodenProduct.ConfigSu0;
			double lambdaU0 = ModulKlimaBodenProduct.ConfigLambdaU0;
			double rLambdaDecke = ModulKlimaBodenProduct.ConfigRLambdaDecke;
			double rLambdaPutz = ModulKlimaBodenProduct.ConfigRLambdaPutz;
			double rAlphaDeckeFbh = 1 / Product.ConfigAlphaDeckeHeat; /* Wärmeübergang Decke bei Heizung */
			double rAlphaDeckeFbk = 1 / Product.ConfigAlphaDeckeCool; /* Wärmeübergang Decke bei Kühlung */

			double su = 0.002;
			double lambdaE = 60;
			if (this.ModulKlimaBodenProduct.PlannedFloorConstruction != null) {
				ConstructionTypeManager ctm = ConstructionTypeManager.Instance;
				ConstructionType ctEstrichS = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH);
				ConstructionType ctEstrichU = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH);
				ConstructionType ctStahlS = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_STD_STAHL);
				ConstructionType ctStahlU = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_USER_STAHL);
				ConstructionType ctTrkEstrS = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TRK_ESTRICH);
				ConstructionType ctTrkEstrU = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TRK_ESTRICH);

				ConstructionType ct = this.ModulKlimaBodenProduct.PlannedFloorConstruction.Type;
				if (ct == ctEstrichS || ct == ctEstrichU) {
					su = 0.03;
					lambdaE = 1.2;
				} else if (ct == ctTrkEstrS || ct == ctTrkEstrU) {
					su = 0.02;
					lambdaE = 0.33;
				} else if (ct == ctStahlS || ct == ctStahlU) {
					su = 0.002;
					lambdaE = 60;
				}

			
			}
			double lambdaU = lambdaE;

			double rLambdaB = this.ModulKlimaBodenProduct.PlannedFloorConstruction == null ? 0 : this.ModulKlimaBodenProduct.PlannedFloorConstruction.RValue;
			double rLambdaIns = this.ModulKlimaBodenProduct.PlannedInsulationConstruction == null ? 0 : this.ModulKlimaBodenProduct.PlannedInsulationConstruction.RValue;

			this.row.LengthVerbindeleitungen = this.SonstigeVerbindeleitung + this.LangeFittinge * 0.1;

			{ // Heizlastberechnung
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.ModulKlimaBodenProduct.GetHeatFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
				this.c_thetaVHeat = distributorVorlaufTemp;
				this.c_thetaRHeat = distributorRuecklaufTemp;
				this.c_thetaVHeat = this.c_thetaVHeat - (this.c_thetaVHeat - this.c_thetaRHeat) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRHeat = this.c_thetaRHeat + (this.c_thetaVHeat - this.c_thetaRHeat) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				if (c_thetaVHeat.Equals(double.NaN) || c_thetaRHeat.Equals(double.NaN)) {
					this.c_qHeatPerSqm = 0;
					this.c_massenstromHeat = 0;
					this.c_druckverlustHeat = 0;
					this.c_floorTempHeat = 0;
				} else {

					double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVHeat, this.c_thetaRHeat, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature);

					double au = en1264.auFlaeche(alpha0, alphaFbh, su0, lambdaU0, su, lambdaE);
					double ab = en1264.abFlaeche(b, au, atmt, rLambdaB);
					this.c_qHeatPerSqm = en1264.WaermestromDichteFlaeche(b, ab, atmt, au, dTheta);

					double qU = en1264.WaermeverlustAussen(alphaFbh, rLambdaB, su, lambdaU, rAlphaDeckeFbh, rLambdaIns, rLambdaDecke, rLambdaPutz, this.c_qHeatPerSqm, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaBodenProduct.PlannedRoomTemperatureBelowHeat);

					// hydraulische Berechnung
					this.c_Qh2oHeat = (this.c_qHeatPerSqm + qU) * this.HeatAreaForCalculation;            // gesamte aufgenommene Leistung berechnen
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

					this.c_massenstromHeat = en1264.Massenstrom(totalQh2o, c, distributorVorlaufTemp - distributorRuecklaufTemp);
					this.c_flussGeschwindigkeitHeat = en1264.FlussGeschwindigkeit(C_DurchflussHeat, Product.rundrohr21mmInnenA);

					this.c_druckverlustHeat = row.Druckverlust(this.c_massenstromHeat);
					foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							this.c_druckverlustHeat += cp.CalculateDruckverlust(this.c_massenstromHeat);
						}
					}

					this.c_floorTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, ModulKlimaBodenProduct.ConfigAlphaFbh, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature);
				}
			}
			{ // Kühllastberechnung
				double distributorVorlaufTemp;
				double distributorRuecklaufTemp;
				this.ModulKlimaBodenProduct.GetCoolFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
				this.c_thetaVCool = distributorVorlaufTemp;
				this.c_thetaRCool = distributorRuecklaufTemp;
				this.c_thetaVCool = this.c_thetaVCool - (this.c_thetaVCool - this.c_thetaRCool) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				this.c_thetaRCool = this.c_thetaRCool + (this.c_thetaVCool - this.c_thetaRCool) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
				if (c_thetaVCool.Equals(double.NaN) || c_thetaRCool.Equals(double.NaN)) {
					this.c_qCoolPerSqm = 0;
					this.c_massenstromCool = 0;
					this.c_druckverlustCool = 0;
					this.c_floorTempCool = 0;
				} else {

					double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVCool, this.c_thetaRCool, this.ModulKlimaBodenProduct.AssociatedRoom.RoomCoolTemperature);

					double au = en1264.auFlaeche(alpha0, alphaFbk, su0, lambdaU0,su, lambdaE);
					double ab = en1264.abFlaeche(b, au, atmt, rLambdaB);
					this.c_qCoolPerSqm = en1264.WaermestromDichteFlaeche(b, ab, atmt, au, dTheta);

					double qU = en1264.WaermeverlustAussen(alphaFbk, rLambdaB, su, lambdaU, rAlphaDeckeFbk, rLambdaIns, rLambdaDecke, rLambdaPutz, this.c_qCoolPerSqm, this.ModulKlimaBodenProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaBodenProduct.PlannedRoomTemperatureBelowHeat);

					// hydraulische Berechnung
					this.c_Qh2oCool = (this.c_qCoolPerSqm + qU) * this.HeatAreaForCalculation;            // gesamte aufgenommene Leistung berechnen
					//                                                                           // gesamten Druckverlust berechnen

					foreach (ConnectionPipe cp in this.plannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							double coolLoad;
							double qH2o;
							cp.CalculateCoolLoad(out coolLoad, out qH2o, this.NrOfCircuit);
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
					this.c_flussGeschwindigkeitCool = en1264.FlussGeschwindigkeit(C_DurchflussCool, Product.rundrohr21mmInnenA);

					this.c_druckverlustCool = row.Druckverlust(this.c_massenstromCool);
					foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
						if (this.nrOfCircuit == 0 || !cp.OnlyFirst) {
							this.c_druckverlustCool += cp.CalculateDruckverlust(this.c_massenstromCool);
						}
					}

					this.c_floorTempCool = en1264.OberflaechenTemperatur(this.c_qCoolPerSqm, ModulKlimaBodenProduct.ConfigAlphaFbk, this.ModulKlimaBodenProduct.AssociatedRoom.RoomCoolTemperature);
				}
			}
		}

		public int ModuleTotal {
			get { return this.row.List.Count; }
		}

		public int DichteModule {
			get { return this.ModuleTotal - this.ModulierendeModule - this.SonstigeModule; }
		}

		public int ModulierendeModule {
			get { return this.LangeFittinge; }
		}

		public int SonstigeModule {
			get { return this.sonstigeModule; }
			set { this.sonstigeModule = value; }
		}

		public override double CircuitArea {
			get { return this.CoveredArea; }
		}

		[XmlIgnore]
		public Color CircuitColor {
			get { return this.circuitColor; }
			set { this.circuitColor = value; }
		}

		// Color cannot be serialized!!!
		// quick workaround to serialize it nevertheless
		public int CircuitColorR {
			get { return this.circuitColor.R; }
			set { this.circuitColor = Color.FromArgb(value, this.circuitColor.G, this.circuitColor.B); }
		}
		public int CircuitColorG {
			get { return this.circuitColor.G; }
			set { this.circuitColor = Color.FromArgb(this.circuitColor.R, value, this.circuitColor.B); }
		}
		public int CircuitColorB {
			get { return this.circuitColor.B; }
			set { this.circuitColor = Color.FromArgb(this.circuitColor.R, this.circuitColor.G, value); }
		}

		public List<KlimaFlaechenModulVerbindung> Links {
			get { return this.verbindungen; }
			set { this.verbindungen = value; }
		}

		/// <summary>
		/// Returns all modules that are already connected to this module via links
		/// </summary>
		/// <param name="referenceModul"></param>
		/// <returns></returns>
		public List<KlimaFlaechenModul> GetAllLinkedModules(KlimaFlaechenModul referenceModul) {
			List<KlimaFlaechenModul> linkedModules = new List<KlimaFlaechenModul>();
			linkedModules.Add(referenceModul);
			KlimaFlaechenModul nextModul = this.GetNextLinkedModule(referenceModul);
			while (nextModul != null && !linkedModules.Contains(nextModul)) {
				linkedModules.Add(nextModul);
				nextModul = this.GetNextLinkedModule(nextModul);
			}
			KlimaFlaechenModul previousModul = this.GetPreviousLinkedModule(referenceModul);
			while (previousModul != null && !linkedModules.Contains(previousModul)) {
				linkedModules.Add(previousModul);
				previousModul = this.GetPreviousLinkedModule(previousModul);
			}
			return linkedModules;
		}

		private KlimaFlaechenModul GetNextLinkedModule(KlimaFlaechenModul referenceModul) {
			if (this.verbindungen != null) {
				foreach (KlimaFlaechenModulVerbindung link in this.verbindungen) {
					if (link.Start == referenceModul) {
						return link.End;
					}
				}
			}
			return null;
		}

		private KlimaFlaechenModul GetPreviousLinkedModule(KlimaFlaechenModul referenceModul) {
			if (this.verbindungen != null) {
				foreach (KlimaFlaechenModulVerbindung link in this.verbindungen) {
					if (link.End == referenceModul) {
						return link.Start;
					}
				}
			}
			return null;
		}
	}
}
