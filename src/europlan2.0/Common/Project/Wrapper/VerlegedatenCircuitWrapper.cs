using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class VerlegedatenCircuitWrapper {

		private string distributor;
        private int startCircuitNumber = 0;
        private int endCircuitNumber = 0;
		private string name;
		private double durchfluss;
		private string area;

		public string Distributor {
			get { return distributor; }
			set { distributor = value; }
		}

		public string CircuitNumber {
			get {
                if (startCircuitNumber == endCircuitNumber) {
                    return startCircuitNumber.ToString();
                } else {
                    return startCircuitNumber.ToString() + "-" + endCircuitNumber.ToString();
                }
            }
		}

        public int StartCircuitNumber {
            get { return startCircuitNumber; }
            set { startCircuitNumber = value; }
        }

        public int EndCircuitNumber {
            get { return endCircuitNumber; }
            set { endCircuitNumber = value; }
        }

        public string Name {
			get { return name; }
			set { name = value; }
		}

		public double Durchfluss {
			get { return durchfluss; }
			set { durchfluss = value; }
		}

		public string Area {
			get { return area; }
			set { area = value; }
		}

	}

}
