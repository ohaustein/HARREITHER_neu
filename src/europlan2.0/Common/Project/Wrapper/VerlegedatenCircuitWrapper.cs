using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class VerlegedatenCircuitWrapper {

		private string distributor;
		public int circuitNumber;
		public string name;
		public double durchfluss;
		public double area;

		public string Distributor {
			get { return distributor; }
			set { distributor = value; }
		}

		public int CircuitNumber {
			get { return circuitNumber; }
			set { circuitNumber = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public double Durchfluss {
			get { return durchfluss; }
			set { durchfluss = value; }
		}

		public double Area {
			get { return area; }
			set { area = value; }
		}

	}

}
