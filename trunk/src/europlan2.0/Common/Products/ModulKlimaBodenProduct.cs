using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Modul Klima-Boden")]
	public class ModulKlimaBodenProduct : Product {

		private float plannedArea = 0;
		private float plannedAreaUnheated = 0;
		private float plannedRoomTemperatureBelowHeat = 18;
		private float plannedRoomTemperatureBelowCool = 22;
		private Construction plannedFloorConstruction = null;
		private Construction plannedInsulationConstruction = null;

		//  !!!!!!!!!!! changes must be also applied in SystemParametersPanel.cs !!!!!!!!!!!
		private static bool useHarreitherNorm = true;
		private static int maxPressureLost = 15000;
		private static int maxDurchfluss = 240;
		private static int maxModulesInRow = 20;
		private static int maxModulesInParallel = 6;
		private static int maxModulesInCircuit = 50;


		public ModulKlimaBodenProduct() {

		}

		protected ModulKlimaBodenProduct(ModulKlimaBodenProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 50;
			quickDimensioningCoolPowerPerSquareMeter = 50;
			canHeat = true;
			canCool = false;
		}

		public override void StaticInitialize() {
			useHarreitherNorm = true;
			maxPressureLost = 15000;
			maxDurchfluss = 240;
			maxModulesInRow = 20;
			maxModulesInParallel = 6;
			maxModulesInCircuit = 50;
		}

		public override Product Clone(Room room) {
			ModulKlimaBodenProduct product = new ModulKlimaBodenProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override int GetDefaultQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 18);
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
			get { return "Modul Klima-Boden"; }
		}

		public override string QuickDimensioningName {
			get { return "Modul\nKlima-\nBoden\n(m≤)"; }
		}

		/// <summary>
		/// The full name of this product
		/// </summary>
		public override string FullName {
			get { return Name; }
		}

		public override ProductType Type {
			get { return ProductType.FBH; }
		}

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg) {
			if (this.plannedFloorConstruction == null || this.plannedInsulationConstruction == null) {
				errorMsg = "Fehlende Eingaben: ";
				if (plannedFloorConstruction == null) {
					errorMsg += "Fuﬂbodenkonstruktion, ";
				}
				if (plannedInsulationConstruction == null) {
					errorMsg += "W‰rmed‰mmkonstruktion, ";
				}
				if (PlannedConnection == null) {
					errorMsg += "Heizkreisanschluﬂ, ";
				}
				errorMsg = errorMsg.Substring(0, errorMsg.Length - 2);
				return false;
			}


			double areaRemovedDueConnection = 0;
			double heatLoadRemovedDueConnection = 0;
			double coolLoadRemovedDueConnection = 0;
			List<ConnectionPipe> connectionPipes = new List<ConnectionPipe>();
			foreach (Floor f in Project.Instance.Floors) {
				foreach (Room r in f.Rooms) {
					foreach (PlannedProduct pp in r.PlannedProducts) {
						foreach (ConnectionPipe cp in pp.Product.PlannedConnectionPipes) {
							if (cp != null && cp.ConnectionThrough != null && cp.ConnectionThrough.Product == this) {
								connectionPipes.Add(cp);
								areaRemovedDueConnection += cp.AreaTotal;
								heatLoadRemovedDueConnection += cp.HeatLoadTotal;
								coolLoadRemovedDueConnection += cp.CoolLoadTotal;
							}
						}
					}
				}
			}

			// Get Vorlauf and Ruecklauf of the defined connection pipes
			double vorlaufTotalFirst = 0;
			double vorlaufNotIsolatedFirst = 0;
			double ruecklaufTotalFirst = 0;
			double ruecklaufNotIsolatedFirst = 0;
			double vorlaufTotalOthers = 0;
			double vorlaufNotIsolatedOthers = 0;
			double ruecklaufTotalOthers = 0;
			double ruecklaufNotIsolatedOthers = 0;
			foreach (ConnectionPipe cp in this.PlannedConnectionPipes) {
				vorlaufTotalFirst += cp.Vorlauf;
				ruecklaufTotalFirst += cp.Ruecklauf;
				if (cp.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) {
					vorlaufNotIsolatedFirst += cp.Vorlauf;
				}
				if (cp.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) {
					ruecklaufNotIsolatedFirst += cp.Ruecklauf;
				}
				if (!cp.OnlyFirst) {
					vorlaufTotalOthers += cp.Vorlauf;
					ruecklaufTotalOthers += cp.Ruecklauf;
					if (cp.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) {
						vorlaufNotIsolatedOthers += cp.Vorlauf;
					}
					if (cp.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) {
						ruecklaufNotIsolatedOthers += cp.Ruecklauf;
					}
				}
			}

			// add connected products to Vorlauf and Ruecklauf
			double[] vorlaufTotal = new double[] { vorlaufTotalFirst, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers };
			double[] vorlaufNotIsolated = new double[] { vorlaufNotIsolatedFirst, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers };
			double[] ruecklaufTotal = new double[] { ruecklaufTotalFirst, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers };
			double[] ruecklaufNotIsolated = new double[] { ruecklaufNotIsolatedFirst, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers };
			double[] vorlaufWithoutOtherProductTotal = new double[] { vorlaufTotalFirst, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers, vorlaufTotalOthers };
			double[] vorlaufWithoutOtherProductNotIsolated = new double[] { vorlaufNotIsolatedFirst, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers, vorlaufNotIsolatedOthers };
			double[] ruecklaufWithoutOtherProductTotal = new double[] { ruecklaufTotalFirst, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers, ruecklaufTotalOthers };
			double[] ruecklaufWithoutOtherProductNotIsolated = new double[] { ruecklaufNotIsolatedFirst, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers, ruecklaufNotIsolatedOthers };

			foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.connectedCircuits) {
				if (kvp.Value != null) {
					if (kvp.Value.type == Circuit.CircuitConnectionTypeEnum.VORLAUF) {
						vorlaufTotal[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithoutOtherProduct;
						vorlaufNotIsolated[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithoutOtherProductNotIsolated;
					} else {
						ruecklaufTotal[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithoutOtherProduct;
						ruecklaufNotIsolated[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithoutOtherProductNotIsolated;
					}
				}
			}

			foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.inverseConnectedCircuits) {
				if (kvp.Value != null) {
					if (kvp.Value.type == Circuit.CircuitConnectionTypeEnum.VORLAUF) {
						ruecklaufTotal[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithoutOtherProduct - kvp.Value.otherCircuit.PipeLengthVorlaufWithoutOtherProductTotal;
						ruecklaufNotIsolated[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithoutOtherProductNotIsolated - kvp.Value.otherCircuit.PipeLengthVorlaufWithoutOtherProductNotIsolated;
						vorlaufTotal[kvp.Key] += kvp.Value.otherCircuit.PipeLengthVorlaufWithoutOtherProductTotal;
						vorlaufNotIsolated[kvp.Key] += kvp.Value.otherCircuit.PipeLengthVorlaufWithoutOtherProductNotIsolated;
					} else {
						vorlaufTotal[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithoutOtherProduct - kvp.Value.otherCircuit.PipeLengthRuecklaufWithoutOtherProductTotal;
						vorlaufNotIsolated[kvp.Key] += kvp.Value.otherCircuit.PipeLengthWithoutOtherProductNotIsolated - kvp.Value.otherCircuit.PipeLengthRuecklaufWithoutOtherProductNotIsolated;
						ruecklaufTotal[kvp.Key] += kvp.Value.otherCircuit.PipeLengthRuecklaufWithoutOtherProductTotal;
						ruecklaufNotIsolated[kvp.Key] += kvp.Value.otherCircuit.PipeLengthRuecklaufWithoutOtherProductNotIsolated;
					}
				}
			}

			double longestVorlaufTotal = vorlaufTotal[0];
			double longestRuecklaufTotal = ruecklaufTotal[0];
			for (int i = 1; i < 12; i++) {
				if (vorlaufTotal[i] > longestVorlaufTotal) {
					longestVorlaufTotal = vorlaufTotal[i];
				}
				if (ruecklaufTotal[i] > longestRuecklaufTotal) {
					longestRuecklaufTotal = ruecklaufTotal[i];
				}
			}

			errorMsg = "";
			return false;
		}

		public override float PlannedFloorArea {
			get { return this.plannedArea; }
			set { this.plannedArea = value; }
		}

		/// <summary>
		/// The percentage of the total room area that is occupied by the planned area.
		/// </summary>
		[XmlIgnore]
		public float PlannedFloorAreaPercentage {
			/*get { return (this.AvailableFloorArea <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AvailableFloorArea); }
			set { this.PlannedFloorArea = (float)(this.AvailableFloorArea * value / 100); }*/
			get { return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AssociatedRoom.Area); }
			set { this.PlannedFloorArea = (float)(this.AssociatedRoom.Area * value / 100); }
		}

		/// <summary>
		/// The area which is planned unheated.
		/// This area is subtracted from the planned area for calculation.
		/// </summary>
		public float PlannedAreaUnheated {
			get { return this.plannedAreaUnheated; }
			set { this.plannedAreaUnheated = value; }
		}

		public override float PlannedWallArea {
			get { return 0; }
			set { }
		}

		public override float PlannedRoofArea {
			get { return 0; }
			set { }
		}

		public override double PlannedCoolLoad {
			get { return 0; }
		}

		public override double PlannedHeatLoad {
			get { return 0; }
		}

		public override float PlannedNetArea {
			get { return this.PlannedFloorArea - this.PlannedAreaUnheated; }
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


		public override Circuit GetCircuit(int index) {
			return null;
		}

		/*public override int GetIndexOfCircuit(Circuit c) {
			return -1;
		}*/

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_21MM; }
		}

		[ProductParameter]
		public static bool ConfigUseHarreitherNorm {
			get { return ModulKlimaBodenProduct.useHarreitherNorm; }
			set { ModulKlimaBodenProduct.useHarreitherNorm = value; }
		}

		[ProductParameter]
		public static int ConfigMaxPressureLost {
			get { return ModulKlimaBodenProduct.maxPressureLost; }
			set { ModulKlimaBodenProduct.maxPressureLost = value; }
		}

		[ProductParameter]
		public static int ConfigMaxDurchfluss {
			get { return ModulKlimaBodenProduct.maxDurchfluss; }
			set { ModulKlimaBodenProduct.maxDurchfluss = value; }
		}

		[ProductParameter]
		public static int ConfigMaxModulesInRow {
			get { return ModulKlimaBodenProduct.maxModulesInRow; }
			set { ModulKlimaBodenProduct.maxModulesInRow = value; }
		}

		[ProductParameter]
		public static int ConfigMaxModulesInParallel {
			get { return ModulKlimaBodenProduct.maxModulesInParallel; }
			set { ModulKlimaBodenProduct.maxModulesInParallel = value; }
		}

		[ProductParameter]
		public static int ConfigModulesInCircuit {
			get { return ModulKlimaBodenProduct.maxModulesInCircuit; }
			set { ModulKlimaBodenProduct.maxModulesInCircuit = value; }
		}
	}
	
}
