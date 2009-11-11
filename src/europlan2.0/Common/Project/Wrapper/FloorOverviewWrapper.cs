using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class FloorOverviewWrapper {

		private string heatOrCool;
		private string floorName;
		private double floorArea;
		private double qH2o;
		private double q;
		private double transmissionFloor;
		private double transmissionWall;
		private double transmissionCeiling;

		public string HeatOrCool {
			get { return heatOrCool; }
			set { heatOrCool = value; }
		}

		public string FloorName {
			get { return floorName; }
			set { floorName = value; }
		}

		public double FloorArea {
			get { return floorArea; }
			set { floorArea = value; }
		}

		public double QH2o {
			get { return qH2o; }
			set { qH2o = value; }
		}

		public double Q {
			get { return q; }
			set { q = value; }
		}

		public double TransmissionFloor {
			get { return transmissionFloor; }
			set { transmissionFloor = value; }
		}

		public double TransmissionWall {
			get { return transmissionWall; }
			set { transmissionWall = value; }
		}

		public double TransmissionCeiling {
			get { return transmissionCeiling; }
			set { transmissionCeiling = value; }
		}
	}

}
