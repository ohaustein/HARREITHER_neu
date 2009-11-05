using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Modul Klima-Decke")]
	public class ModulKlimaDeckeProduct : Product {

		//private static double module_100_40_area = 0.9925 * 0.4;
		private static double module_100_30_area = 0.9925 * 0.295;
		private static double module_120_30_area = 1.194 * 0.295;
		private static double module_80_30_area = 0.791 * 0.295;

		private List<KlimaFlaechenModul> modules_100_30L = new List<KlimaFlaechenModul>();
		private List<KlimaFlaechenModul> modules_120_30L = new List<KlimaFlaechenModul>();
		private List<KlimaFlaechenModul> modules_80_30L = new List<KlimaFlaechenModul>();
		private List<KlimaFlaechenModul> modules_100_30R = new List<KlimaFlaechenModul>();
		private List<KlimaFlaechenModul> modules_120_30R = new List<KlimaFlaechenModul>();
		private List<KlimaFlaechenModul> modules_80_30R = new List<KlimaFlaechenModul>();

		public ModulKlimaDeckeProduct() {

		}

		protected ModulKlimaDeckeProduct(ModulKlimaDeckeProduct product) : base(product) {

		}

		public override void Initialize() {
			quickDimensioningHeatPowerPerSquareMeter = 80;
			quickDimensioningCoolPowerPerSquareMeter = 80;
			canHeat = true;
			canCool = true;
		}

		public override void StaticInitialize() {

		}

		public override Product Clone(Room room) {
			ModulKlimaDeckeProduct product = new ModulKlimaDeckeProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		public override int GetDefaultQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 18);
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			if (this.AssociatedRoom != null) {
				return this.AssociatedRoom.Area * Project.Instance.QuickDimensioning.CeilingAllocation / 100;
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
			get { return "Modul Klima-Decke"; }
		}

		public override string QuickDimensioningName {
			get { return "Modul\nKlima\nDecke\n(m²)"; }
		}

		/// <summary>
		/// The full name of this product
		/// </summary>
		public override string FullName {
			get { return Name; }
		}

		public override ProductType Type {
			get { return ProductType.DH; }
		}

		public int ModuleCount_100_30L {
			get { return this.modules_100_30L.Count; }
			set {
				while (value > this.modules_100_30L.Count) {
					this.modules_100_30L.Add(new KlimaFlaechenModul(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30, KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT));
				}
				if (value < this.modules_100_30L.Count) {
					this.modules_100_30L.RemoveRange(value, this.modules_100_30L.Count - value);
				}
			}
		}

		public int ModuleCount_120_30L {
			get { return this.modules_100_30L.Count; }
			set {
				while (value > this.modules_120_30L.Count) {
					this.modules_120_30L.Add(new KlimaFlaechenModul(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30, KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT));
				}
				if (value < this.modules_120_30L.Count) {
					this.modules_120_30L.RemoveRange(value, this.modules_120_30L.Count - value);
				}
			}
		}

		public int ModuleCount_80_30L {
			get { return this.modules_80_30L.Count; }
			set {
				while (value > this.modules_80_30L.Count) {
					this.modules_80_30L.Add(new KlimaFlaechenModul(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30, KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT));
				}
				if (value < this.modules_100_30L.Count) {
					this.modules_80_30L.RemoveRange(value, this.modules_80_30L.Count - value);
				}
			}
		}

		public int ModuleCount_100_30R {
			get { return this.modules_100_30R.Count; }
			set {
				while (value > this.modules_100_30R.Count) {
					this.modules_100_30R.Add(new KlimaFlaechenModul(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30, KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT));
				}
				if (value < this.modules_100_30L.Count) {
					this.modules_100_30R.RemoveRange(value, this.modules_100_30R.Count - value);
				}
			}
		}

		public int ModuleCount_120_30R {
			get { return this.modules_100_30R.Count; }
			set {
				while (value > this.modules_120_30R.Count) {
					this.modules_120_30R.Add(new KlimaFlaechenModul(KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30, KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT));
				}
				if (value < this.modules_120_30R.Count) {
					this.modules_120_30R.RemoveRange(value, this.modules_120_30R.Count - value);
				}
			}
		}

		public int ModuleCount_80_30R {
			get { return this.modules_80_30R.Count; }
			set {
				while (value > this.modules_80_30R.Count) {
					this.modules_80_30R.Add(new KlimaFlaechenModul(KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30, KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT));
				}
				if (value < this.modules_100_30R.Count) {
					this.modules_80_30R.RemoveRange(value, this.modules_80_30R.Count - value);
				}
			}
		}

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, out string errorMsg) {
			// TODO
			double thetaV = 35;
			double thetaR = 30;

			EN1264 en1264 = EN1264.Instance;

			double deltaThetaH = en1264.Heizmitteluebertemperatur(thetaV, thetaR, this.AssociatedRoom.RoomHeatTemperature);

			//en1264.WaermestromDichteFlaeche(
			errorMsg = "Noch nicht implementiert";
			return false;
		}

		public override float PlannedFloorArea {
			get { return 0; }
			set { }
		}

		public override float PlannedWallArea {
			get { return 0; }
			set { }
		}

		public override float PlannedRoofArea {
			get {
				return (float)((this.modules_100_30L.Count + this.modules_100_30R.Count) * module_100_30_area + 
					(this.modules_120_30L.Count + this.modules_120_30R.Count) * module_120_30_area + 
					(this.modules_80_30L.Count + this.modules_80_30R.Count) * module_80_30_area);
			}
			set { }
		}

		public override double PlannedCoolLoad {
			get { return 0; }
		}

		public override double PlannedHeatLoad {
			get { return 0; }
		}

		public override float PlannedNetArea {
			get { return 0; }
		}

		/*public override int GetIndexOfCircuit(Circuit c) {
			return -1;
		}*/

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_21MM; }
		}

		[XmlIgnore]
		public override float PlannedInsideConstructionRValue {
			get { return 0; /*TODO*/ }
		}

		[XmlIgnore]
		public override bool HasInsideConstruction {
			get { return false; }
		}

		[XmlIgnore]
		public override float PlannedOutsideConstructionRValue {
			get { return 0; /*TODO*/ }
		}

		[XmlIgnore]
		public override bool HasOutsideConstruction {
			get { return false; }
		}
	}
	
}
