using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class QuickDimensioningReportWrapper {

		private Room room;
		private Floor floor;

		private float[] productArea = new float[7];
		private int[] productCircuits = new int[7];		

		public QuickDimensioningReportWrapper(Room room, Floor floor, List<string> productOrder) {
			this.room = room;
			this.floor = floor;
			foreach (Product product in room.UsedProductsForQuickDimensioning) {
				productArea[productOrder.IndexOf(product.QuickDimensioningName)] = product.QuickDimensioningPlannedArea;
				productCircuits[productOrder.IndexOf(product.QuickDimensioningName)] = product.QuickDimensioningCircuits;
			}
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

		public string RoomType {
			get { return this.room.QuickDimensioningRoomType.Name; }
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

		public float Product1Area {
			get { return productArea[0]; }
			set { productArea[0] = value; }
		}

		public int Product1Circuits {
			get { return productCircuits[0]; }
			set { productCircuits[0] = value; }
		}

		public float Product2Area {
			get { return productArea[1]; }
			set { productArea[1] = value; }
		}

		public int Product2Circuits {
			get { return productCircuits[1]; }
			set { productCircuits[1] = value; }
		}

		public float Product3Area {
			get { return productArea[2]; }
			set { productArea[2] = value; }
		}

		public int Product3Circuits {
			get { return productCircuits[2]; }
			set { productCircuits[2] = value; }
		}

		public float Product4Area {
			get { return productArea[3]; }
			set { productArea[3] = value; }
		}

		public int Product4Circuits {
			get { return productCircuits[3]; }
			set { productCircuits[3] = value; }
		}

		public float Product5Area {
			get { return productArea[4]; }
			set { productArea[4] = value; }
		}

		public int Product5Circuits {
			get { return productCircuits[4]; }
			set { productCircuits[4] = value; }
		}

		public float Product6Area {
			get { return productArea[5]; }
			set { productArea[5] = value; }
		}

		public int Product6Circuits {
			get { return productCircuits[5]; }
			set { productCircuits[5] = value; }
		}

		public float Product7Area {
			get { return productArea[6]; }
			set { productArea[6] = value; }
		}

		public int Product7Circuits {
			get { return productCircuits[6]; }
			set { productCircuits[6] = value; }
		}
		
	}
}
