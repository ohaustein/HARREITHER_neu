using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class RoomType {

		private string id;
		private string name = "";
		private int heatPowerPerSquareMeter = 0;
		private int coolPowerPerSquareMeter = 0;
		private bool userDefined = false;

		public RoomType() {
			id = System.Guid.NewGuid().ToString();
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public int HeatPowerPerSquareMeter {
			get { return heatPowerPerSquareMeter; }
			set { heatPowerPerSquareMeter = value; }
		}

		public int CoolPowerPerSquareMeter {
			get { return coolPowerPerSquareMeter; }
			set { coolPowerPerSquareMeter = value; }
		}

		public bool UserDefined {
			get { return userDefined; }
			set { userDefined = value; }
		}

	}
}
