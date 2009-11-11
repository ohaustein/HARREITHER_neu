using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class RoomOverviewWrapper {

		private string id;
		private string name;
		private int heatTemperature;
		private double heatPower;
		private int heatNetLoad;
		private int coolTemperature;
		private double coolPower;
		private int coolNetLoad;
		private float area;
		private string floorId;
		private string floorName;

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public int HeatTemperature {
			get { return heatTemperature; }
			set { heatTemperature = value; }
		}

		public int HeatNetLoad {
			get { return heatNetLoad; }
			set { heatNetLoad = value; }
		}

		public double HeatPower {
			get { return heatPower; }
			set { heatPower = value; }
		}

		public double HeatRest {
			get { return -1 * (HeatNetLoad - HeatPower); }
		}

		public int CoolTemperature {
			get { return coolTemperature; }
			set { coolTemperature = value; }
		}
		
		public int CoolNetLoad {
			get { return coolNetLoad; }
			set { coolNetLoad = value; }
		}
		
		public double CoolPower {
			get { return coolPower; }
			set { coolPower = value; }
		}

		public double CoolRest {
			get { return -1 * (CoolNetLoad - CoolPower); }
		}

		public float Area {
			get { return area; }
			set { area = value; }
		}

		public string FloorId {
			get { return floorId; }
			set { floorId = value; }
		}

		public string FloorName {
			get { return floorName; }
			set { floorName = value; }
		}

	}

}
