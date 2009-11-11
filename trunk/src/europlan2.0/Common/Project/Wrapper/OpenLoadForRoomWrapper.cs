using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common
{
	public class OpenLoadForRoomWrapper {

		private string roomId;
		private string roomName;
		private double requiredLoad;
		private double netLoad;
		private double power;


		public string RoomId {
			get { return roomId; }
			set { roomId = value; }
		}

		public string RoomName {
			get { return roomName; }
			set { roomName = value; }
		}

		public double RequiredLoad {
			get { return requiredLoad; }
			set { requiredLoad = value; }
		}

		public double NetLoad {
			get { return netLoad; }
			set { netLoad = value; }
		}

		public double Power {
			get { return power; }
			set { power = value; }
		}

		public double Rest {
			get { return NetLoad - Power; }
		}

		public double Rate {
			get {
				if (NetLoad > 0) {
					return 100.0 / NetLoad * Power;
				} else {
					return 0;
				}
			}
		}

	}

}
