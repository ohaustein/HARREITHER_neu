using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class EurovalAuslegungWrapper {

		private string roomId;
		private string roomName;
		private string floorId;
		private string floorName;
		private string heatOrCool;
		private int circuits;
		private double vorlaufTemp;
		private double ruecklaufTemp;
		private double wassermenge;

		public string RoomId {
			get { return roomId; }
			set { roomId = value; }
		}

		public string RoomName {
			get { return roomName; }
			set { roomName = value; }
		}

		public string FloorId {
			get { return floorId; }
			set { floorId = value; }
		}

		public string FloorName {
			get { return floorName; }
			set { floorName = value; }
		}

		public string HeatOrCool {
			get { return heatOrCool; }
			set { heatOrCool = value; }
		}

		public int Circuits {
			get { return circuits; }
			set { circuits = value; }
		}

		public double VorlaufTemp {
			get { return vorlaufTemp; }
			set { vorlaufTemp = value; }
		}

		public double RuecklaufTemp {
			get { return ruecklaufTemp; }
			set { ruecklaufTemp = value; }
		}

		public double Wassermenge {
			get { return wassermenge; }
			set { wassermenge = value; }
		}

	}

}
