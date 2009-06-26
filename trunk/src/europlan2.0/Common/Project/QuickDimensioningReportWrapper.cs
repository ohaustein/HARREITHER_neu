using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class QuickDimensioningReportWrapper {

		private Room room;
		private Floor floor;

		public QuickDimensioningReportWrapper(Room room, Floor floor) {
			this.room = room;
			this.floor = floor;
		}

		public string RoomId {
			get { return this.room.Id; }
		}

		public string RoomName {
			get { return this.room.Name; }
		}

		public string FloorName {
			get { return this.floor.Name; }
		}

		public int RoomTemperature {
			get { return this.room.QuickDimensioningRoomTemperature; }
		}

		public float RoomArea {
			get { return this.room.Area; }
		}

		public int RoomHeatLoad {
			get { return this.room.QuickDimensioningHeatLoad; }
		}

		public int RoomCoolLoad {
			get { return this.room.QuickDimensioningCoolLoad; }
		}

		public int RoomNrOfServos {
			get { return this.room.QuickDimensioningNrOfServos; }
		}

		public string RoomController {
			get { return this.room.QuickDimensioningRoomController.ToString(); }
		}

		public string RoomComments {
			get { return this.room.QuickDimensioningComments; }
		}

		public float EurovalArea {
			get { return 0; }
		}

		public int EurovalCircuits {
			get { return 0; }
		}

	}
}
