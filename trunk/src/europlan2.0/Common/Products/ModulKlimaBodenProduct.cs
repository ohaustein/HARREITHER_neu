using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Modul Klima-Boden")]
	public class ModulKlimaBodenProduct : Product {

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
			get { return "Modul\nKlima-\nBoden\n(m²)"; }
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
			// TODO
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
			get { return 0; }
			set { }
		}

		public override double PlannedCoolLoad {
			get { return 0; }
		}

		public override double PlannedHeatLoad {
			get { return 0; }
		}

		public override double PlannedPipeLength {
			get { return 0; }
		}

		public override float PlannedNetArea {
			get { return 0; }
		}

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_21MM; }
		}
	}
	
}
