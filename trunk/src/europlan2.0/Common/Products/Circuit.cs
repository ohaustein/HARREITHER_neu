using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public abstract class Circuit {

		public enum CircuitConnectionTypeEnum {
			VORLAUF,
			RUECKLAUF
		}

		public class CircuitConnection {
			public CircuitConnectionTypeEnum type;
			public Circuit otherCircuit;
		}

		protected string id;

		public Circuit() {
			this.id = System.Guid.NewGuid().ToString();
		}

		[XmlIgnore]
		public abstract Product Product {
			get;
		}

		protected bool corrections = false;
		public bool Corrections {
			get { return this.corrections; }
			set { this.corrections = value; }
		}

		protected int nrOfCircuit = 0;
		[XmlIgnore]
		public int NrOfCircuit {
			get { return this.nrOfCircuit; }
			set { this.nrOfCircuit = value; }
		}



		public abstract double PipeLengthWithAllConnections {
			get;
		}

		public abstract double PipeLengthWithUnisolatedConnections {
			get;
		}

		public abstract double PipeLengthWithoutConnections {
			get;
		}

		public double PipeLengthVorlauf {
			get {
				double value = 0;
				foreach (ConnectionPipe cp in this.Product.PlannedConnectionPipes) {
					value += cp.Vorlauf;
				}
				return value;
			}
		}

		public double PipeLengthVorlaufNotIsolated {
			get {
				double value = 0;
				foreach (ConnectionPipe cp in this.Product.PlannedConnectionPipes) {
					if (cp.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) {
						value += cp.Vorlauf;
					}
				}
				return value;
			}
		}

		public double PipeLengthRuecklauf {
			get {
				double value = 0;
				foreach (ConnectionPipe cp in this.Product.PlannedConnectionPipes) {
					value += cp.Ruecklauf;
				}
				return value;
			}
		}

		public double PipeLengthRuecklaufNotIsolated {
			get {
				double value = 0;
				foreach (ConnectionPipe cp in this.Product.PlannedConnectionPipes) {
					if (cp.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) {
						value += cp.Ruecklauf;
					}
				}
				return value;
			}
		}
	}
}
