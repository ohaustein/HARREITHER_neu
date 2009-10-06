using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Europlan.Common {
	public class KlimaFlaechenModul {
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

		private Nullable<Point> origin = null;

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

		public Nullable<Point> Origin {
			get { return this.origin; }
			set { this.origin = value; }
		}
	}
}
