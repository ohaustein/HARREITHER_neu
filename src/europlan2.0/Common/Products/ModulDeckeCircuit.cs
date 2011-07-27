using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Europlan.Common {

	public class ModulDeckeCircuit : Circuit {

		//private List<KlimaFlaechenList> rows = new List<KlimaFlaechenList>();
		private List<ModulDeckeSubArea> subAreas = new List<ModulDeckeSubArea>();
		private Color circuitColor = Color.FromArgb(0, 128, 0);

		private List<KlimaFlaechenSubAreaVerbindung> verbindungen = null;

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
		public double CoveredArea {
			get {
				double area = 0;
				foreach (ModulDeckeSubArea subArea in subAreas) {
					area += subArea.CoveredArea;
				}
				return area;
			}
		}

		[XmlIgnore]
		public double HeatArea {
			get {
				double area = 0;
				foreach (ModulDeckeSubArea subArea in subAreas) {
					area += subArea.HeatArea;
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

		private double c_ceilingTempHeat;
		[XmlIgnore]
		public double C_CeilingTempHeat {
			get { return this.c_ceilingTempHeat; }
		}

		private double c_ceilingTempCool;
		[XmlIgnore]
		public double C_CeilingTempCool {
			get { return this.c_ceilingTempCool; }
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

			double alphaInnenHeat = 8;
			double alphaAussenHeat = 8;
			double alphaInnenCool = 8;
			double alphaAussenCool = 8;
			switch (this.ModulKlimaDeckeProduct.ModulType) {
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

			double atmt = ModulKlimaDeckeProduct.ConfigAtmt;
			double b = ModulKlimaDeckeProduct.ConfigB;
			double c = ModulKlimaDeckeProduct.ConfigC;
			double alpha0 = ModulKlimaDeckeProduct.ConfigAlpha0;
			/*double alphaDecke = ModulKlimaDeckeProduct.ConfigAlphaDecke;
			double alphaBoden = ModulKlimaDeckeProduct.ConfigAlphaBoden;*/
			double su0 = ModulKlimaDeckeProduct.ConfigSu0;
			//double su = ModulKlimaDeckeProduct.ConfigSu;
			double lambdaU0 = ModulKlimaDeckeProduct.ConfigLambdaU0;
			//double lambdaU = ModulKlimaDeckeProduct.ConfigLambdaU;
			//double lambdaE = ModulKlimaDeckeProduct.ConfigLambdaE;
			double rLambdaDecke = ModulKlimaDeckeProduct.ConfigRLambdaDecke;
			double rLambdaDach = ModulKlimaDeckeProduct.ConfigRLambdaDach;
			double rAlphaDeckeDh = 1 / alphaAussenHeat; /* Wärmeübergang Decke bei Heizung */
			double rAlphaDeckeDk = 1 / alphaAussenCool; /* Wärmeübergang Decke bei Kühlung */

			double rLambdaB = 0;
			double rLambdaIns = this.ModulKlimaDeckeProduct.PlannedInsulationConstruction == null ? 0 : this.ModulKlimaDeckeProduct.PlannedInsulationConstruction.RValue;

			double su = this.ModulKlimaDeckeProduct.PlannedCeilingConstruction == null ? 0 : this.ModulKlimaDeckeProduct.PlannedCeilingConstruction.Thickness / 1000;
			double lambdaE = this.ModulKlimaDeckeProduct.PlannedCeilingConstruction == null ? 0 : this.ModulKlimaDeckeProduct.PlannedCeilingConstruction.LambdaValue;
			double lambdaU = lambdaE;

			{ // Heizlastberechnung
				double leistungsFaktor = ModulKlimaDeckeProduct.ConfigLeistungsFaktorHeizen;
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

					double au = en1264.auFlaeche(alpha0, alphaInnenHeat, su0, lambdaU0, su, lambdaE);
					double ab = en1264.abFlaeche(b, au, atmt, rLambdaB);
					this.c_qHeatPerSqm = en1264.WaermestromDichteFlaeche(b, ab, atmt, au, dTheta) * leistungsFaktor;

					double qU = en1264.WaermeverlustAussen(alphaInnenHeat, rLambdaB, su, lambdaU, rAlphaDeckeDh, rLambdaIns, rLambdaDecke, rLambdaDach, this.c_qHeatPerSqm, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaDeckeProduct.PlannedRoomTemperatureBelowHeat);

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

					this.c_massenstromHeat = en1264.Massenstrom(totalQh2o, c, distributorVorlaufTemp - distributorRuecklaufTemp);
					this.c_flussGeschwindigkeitHeat = en1264.FlussGeschwindigkeit(C_DurchflussHeat, Product.rundrohr21mmInnenA);

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

					this.c_ceilingTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, alphaInnenHeat, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomHeatTemperature);

					//this.c_floorTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, ModulKlimaDeckeProduct.ConfigAlphaFbh, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomHeatTemperature);
				}
			}
			{ // Kühllastberechnung
				double leistungsFaktor = ModulKlimaDeckeProduct.ConfigLeistungsFaktorKuehlen;
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

					double au = en1264.auFlaeche(alpha0, alphaInnenCool, su0, lambdaU0, su, lambdaE);
					double ab = en1264.abFlaeche(b, au, atmt, rLambdaB);
					this.c_qCoolPerSqm = en1264.WaermestromDichteFlaeche(b, ab, atmt, au, dTheta) * leistungsFaktor;

					double qU = en1264.WaermeverlustAussen(alphaInnenCool, rLambdaB, su, lambdaU, rAlphaDeckeDk, rLambdaIns, rLambdaDecke, rLambdaDach, this.c_qCoolPerSqm, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaDeckeProduct.PlannedRoomTemperatureBelowHeat);

					// hydraulische Berechnung
					this.c_Qh2oCool = (this.c_qCoolPerSqm + qU) * this.HeatArea;            // gesamte aufgenommene Leistung berechnen
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

					this.c_ceilingTempCool = en1264.OberflaechenTemperatur(this.c_qCoolPerSqm, alphaInnenCool, this.ModulKlimaDeckeProduct.AssociatedRoom.RoomCoolTemperature);

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

		public override double CircuitArea {
			get { return this.CoveredArea; }
		}

		public bool ContainsModul(KlimaFlaechenModul modul) {
			if (this.subAreas != null) {
				foreach (ModulDeckeSubArea subArea in this.subAreas) {
					if (subArea.ContainsModul(modul)) {
						return true;
					}
				}
			}
			return false;
		}

		public int CountModules() {
			int count = 0;
			foreach (ModulDeckeSubArea subArea in this.subAreas) {
				count += subArea.CountModules();
			}
			return count;
		}

		public ModulDeckeSubArea GetSubareaForModul(KlimaFlaechenModul modul, out int index) {
			index = 0;
			foreach (ModulDeckeSubArea sa in this.subAreas) {
				if (sa.ContainsModul(modul)) {
					return sa;
				}
				index++;
			}
			index = -1;
			return null;
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

		public List<KlimaFlaechenSubAreaVerbindung> Links {
			get { return this.verbindungen; }
			set { this.verbindungen = value; }
		}

		public List<KlimaFlaechenModul> GetAllLinkedModules(KlimaFlaechenModul referenceModul) {
			List<KlimaFlaechenModul> linkedModules = new List<KlimaFlaechenModul>();
			linkedModules.Add(referenceModul);
			List<KlimaFlaechenModul> nextModules = this.GetNextLinkedModules(referenceModul);
			nextModules.AddRange(this.GetPreviousLinkedModules(referenceModul));
			while (nextModules.Count > 0) {
				KlimaFlaechenModul nextModule = nextModules[0];
				nextModules.RemoveAt(0);
				if (!linkedModules.Contains(nextModule)) {
					linkedModules.Add(nextModule);
					nextModules.AddRange(this.GetNextLinkedModules(nextModule));
					nextModules.AddRange(this.GetPreviousLinkedModules(nextModule));
				}
			}
			return linkedModules;
		}

		public IKlimaFlaechenVerbindung GetNextLink(KlimaFlaechenModul modul) {
			if (this.verbindungen != null) {
				foreach (KlimaFlaechenSubAreaVerbindung link in this.verbindungen) {
					if (link.Start.Contains(modul)) {
						return link;
					}
				}
			}

			foreach (ModulDeckeSubArea sa in this.SubAreas) {
				foreach (KlimaFlaechenList row in sa.Rows) {
					foreach (KlimaFlaechenModulVerbindung link in row.Links) {
						if (link.Start == modul) {
							return link;
						}
					}
				}
			}
			return null;
		}

		public IKlimaFlaechenVerbindung GetPreviousLink(KlimaFlaechenModul modul) {
			if (this.verbindungen != null) {
				foreach (KlimaFlaechenSubAreaVerbindung link in this.verbindungen) {
					if (link.End.Contains(modul)) {
						return link;
					}
				}
			}

			foreach (ModulDeckeSubArea sa in this.SubAreas) {
				foreach (KlimaFlaechenList row in sa.Rows) {
					foreach (KlimaFlaechenModulVerbindung link in row.Links) {
						if (link.End == modul) {
							return link;
						}
					}
				}
			}
			return null;
		}

		private List<KlimaFlaechenModul> GetNextLinkedModules(KlimaFlaechenModul referenceModul) {
			List<KlimaFlaechenModul> nextModules = new List<KlimaFlaechenModul>();
			foreach (ModulDeckeSubArea sa in this.subAreas) {
				foreach (KlimaFlaechenList row in sa.Rows) {
					if (row.Links != null) {
						foreach (KlimaFlaechenModulVerbindung link in row.Links) {
							if (link.Start == referenceModul) {
								nextModules.Add(link.End);
							}
						}
					}
				}
			}

			if (this.Links != null) {
				foreach (KlimaFlaechenSubAreaVerbindung saLink in this.Links) {
					if (saLink.Start.Contains(referenceModul)) {
						nextModules.AddRange(saLink.End);
					}
				}
			}

			return nextModules;
		}

		private List<KlimaFlaechenModul> GetPreviousLinkedModules(KlimaFlaechenModul referenceModul) {
			List<KlimaFlaechenModul> prevModules = new List<KlimaFlaechenModul>();
			foreach (ModulDeckeSubArea sa in this.subAreas) {
				foreach (KlimaFlaechenList row in sa.Rows) {
					if (row.Links != null) {
						foreach (KlimaFlaechenModulVerbindung link in row.Links) {
							if (link.End == referenceModul) {
								prevModules.Add(link.Start);
							}
						}
					}
				}
			}

			if (this.Links != null) {
				foreach (KlimaFlaechenSubAreaVerbindung saLink in this.Links) {
					if (saLink.End.Contains(referenceModul)) {
						prevModules.AddRange(saLink.Start);
					}
				}
			}

			return prevModules;
		}
	}
}
