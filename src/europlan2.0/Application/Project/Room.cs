using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Europlan.Application {
	
	
	public class Room {

		private string name;
		private string id;
		private int temperature;
		private float area;
		private int heatPower;
		private int coolPower;
		private int normalizedHeatPower;
		private int normalizedCoolPower;

		private static readonly ILog log = LogManager.GetLogger(typeof(Room));

		public Room() {
			InitializeRoom();
		}

		public Room(string name) {
			InitializeRoom();
			this.name = name;
		}

		private void InitializeRoom() {
			name = "";
			temperature = 0;
			area = 0;
			heatPower = 0;
			coolPower = 0;
			normalizedHeatPower = 0;
			normalizedCoolPower = 0;
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public int Temperature {
			get { return temperature; }
			set { temperature = value; }
		}

		public float Area {
			get { return area; }
			set { area = value; }
		}

		public int HeatPower {
			get { return heatPower; }
			set { heatPower = value; }
		}

		public int CoolPower {
			get { return coolPower; }
			set { coolPower = value; }
		}

		public int NormalizedHeatPower {
			get { return normalizedHeatPower; }
			set { normalizedHeatPower = value; }
		}

		public int NormalizedCoolPower {
			get { return normalizedCoolPower; }
			set { normalizedCoolPower = value; }
		}


	}

}
