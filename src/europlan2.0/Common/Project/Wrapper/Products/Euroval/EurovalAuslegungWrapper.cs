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
		private string teilSystem;
		private string insideConstruction;
		private double insideRValue;
		private string outsideConstruction;
		private double outsideRValue;
		private int circuits;
		private string rzLayDistance;
		private int rzWidth;
		private double rzArea;
		private string azLayDistance;
		private double azArea;
		private double connectionArea;
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

		public string TeilSystem {
			get { return teilSystem; }
			set { teilSystem = value; }
		}

		public string InsideConstruction {
			get { return insideConstruction; }
			set { insideConstruction = value; }
		}

		public double InsideRValue {
			get { return insideRValue; }
			set { insideRValue = value; }
		}

		public string OutsideConstruction {
			get { return outsideConstruction; }
			set { outsideConstruction = value; }
		}

		public double OutsideRValue {
			get { return outsideRValue; }
			set { outsideRValue = value; }
		}
		public int Circuits {
			get { return circuits; }
			set { circuits = value; }
		}

		public string RzLayDistance {
			get { return rzLayDistance; }
			set { rzLayDistance = value; }
		}

		public int RzWidth {
			get { return rzWidth; }
			set { rzWidth = value; }
		}

		public double RzArea {
			get { return rzArea; }
			set { rzArea = value; }
		}

		public string AzLayDistance {
			get { return azLayDistance; }
			set { azLayDistance = value; }
		}

		public double AzArea {
			get { return azArea; }
			set { azArea = value; }
		}

		public double ConnectionArea {
			get { return connectionArea; }
			set { connectionArea = value; }
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
