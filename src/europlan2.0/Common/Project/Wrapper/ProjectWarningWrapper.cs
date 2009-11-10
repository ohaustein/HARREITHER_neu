using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class ProjectWarningWrapper {

		private string floorId;
		private string floorName;
		private string warning;

		public string FloorId {
			get { return floorId; }
			set { floorId = value; }
		}

		public string FloorName {
			get { return floorName; }
			set { floorName = value; }
		}

		public string Warning {
			get { return warning; }
			set { warning = value; }
		}
	
	}

}
