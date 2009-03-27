using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Europlan.Application {
	

	public class Floor {

		private string name;
		private IList<Room> rooms;

		private static readonly ILog log = LogManager.GetLogger(typeof(Floor));

		public Floor() {
			InitializeFloor();
		}

		public Floor(string name) {
			InitializeFloor();
			this.name = name;
		}

		private void InitializeFloor() {
			name = "";
			rooms = new List<Room>();
		}
	
	}

}

