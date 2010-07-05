using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class BilanzWrapper {

		private string description;
		private string heatValue;
		private string heatUnit;
		private string coolValue;
		private string coolUnit;

		public string Description {
			get { return description; }
			set { description = value; }
		}

		public string HeatValue {
			get { return heatValue; }
			set { heatValue = value; }
		}

		public string HeatUnit {
			get { return heatUnit; }
			set { heatUnit = value; }
		}

		public string CoolValue {
			get { return coolValue; }
			set { coolValue = value; }
		}
		
		public string CoolUnit {
			get { return coolUnit; }
			set { coolUnit = value; }
		}

	}

}
