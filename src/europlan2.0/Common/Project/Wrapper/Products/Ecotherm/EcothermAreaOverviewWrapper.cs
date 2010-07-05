using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class EcothermAreaOverviewWrapper {

		private string layDistance;
		private double azArea;
		private double rzArea;
		private double connectingArea;

		public string LayDistance {
			get { return layDistance; }
			set { layDistance = value; }
		}

		public double TotalArea {
			get { return azArea + rzArea + connectingArea; }
		}

		public double AzArea {
			get { return azArea; }
			set { azArea = value; }
		}

		public double RzArea {
			get { return rzArea; }
			set { rzArea = value; }
		}
		
		public double ConnectingArea {
			get { return connectingArea; }
			set { connectingArea = value; }
		}

	}

}
