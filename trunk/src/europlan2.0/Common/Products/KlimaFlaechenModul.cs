using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class KlimaFlaechenModul {
		private static double module_100_40_area = 0.9925 * 0.4;
		private static double module_100_30_area = 0.9925 * 0.295;
		private static double module_120_30_area = 1.194 * 0.295;
		private static double module_80_30_area = 0.791 * 0.295;

		public enum ModulTypeEnum {
			MODUL_100_40,
			MODUL_100_30,
			MODUL_120_30,
			MODUL_80_30
		}

		public enum ModulOrientationEnum {
			ORIENTATION_LEFT,
			ORIENTATION_RIGHT
		}

		private ModulTypeEnum modulType;
		private ModulOrientationEnum orientation;

		/*private Nullable<Point> origin = null;*/

		public KlimaFlaechenModul(ModulTypeEnum modulType, ModulOrientationEnum orientation)  {
			this.modulType = modulType;
			this.orientation = orientation;
		}

		public ModulTypeEnum ModulType {
			get { return this.modulType; }
			set { this.modulType = value; }
		}

		public ModulOrientationEnum Orientation {
			get { return this.orientation; }
			set { this.orientation = value; }
		}

		/*public Nullable<Point> Origin {
			get { return this.origin; }
			set { this.origin = value; }
		}*/

		[XmlIgnore]
		public double Area {
			get {
				switch (this.modulType) {
					case ModulTypeEnum.MODUL_100_40:
						return KlimaFlaechenModul.module_100_40_area;

					case ModulTypeEnum.MODUL_80_30:
						return KlimaFlaechenModul.module_80_30_area;

					case ModulTypeEnum.MODUL_100_30:
						return KlimaFlaechenModul.module_100_30_area;

					case ModulTypeEnum.MODUL_120_30:
						return KlimaFlaechenModul.module_120_30_area;

					default:
						return 0;
				}
			}
		}

		public double Druckverlust(double durchfluss) {
			switch (this.modulType) {
				case ModulTypeEnum.MODUL_100_40:
					return EN1264.Instance.DruckverlustModul_100_40(1, durchfluss);

				case ModulTypeEnum.MODUL_80_30:
					return EN1264.Instance.DruckverlustModul_80_30(1, durchfluss);

				case ModulTypeEnum.MODUL_100_30:
					return EN1264.Instance.DruckverlustModul_100_30(1, durchfluss);

				case ModulTypeEnum.MODUL_120_30:
					return EN1264.Instance.DruckverlustModul_120_30(1, durchfluss);

				default:
					return 0;
			}
		}
	}
}
