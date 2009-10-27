using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	[XmlInclude(typeof(EurovalCircuit))]
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
		public abstract PlannedProduct PlannedProduct {
			get;
		}

		/*public abstract string PlannedProductId {
			get;
			set;
		}*/

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


		[XmlIgnore]
		public abstract double PipeLengthWithAllConnections {
			get;
		}

		[XmlIgnore]
		public abstract double PipeLengthWithUnisolatedConnections {
			get;
		}

		[XmlIgnore]
		public abstract double PipeLengthWithoutConnections {
			get;
		}

		[XmlIgnore]
		public double PipeLengthVorlauf {
			get {
				double value = 0;
				foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
					value += cp.Vorlauf;
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PipeLengthVorlaufNotIsolated {
			get {
				double value = 0;
				foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
					if (cp.Insulation == ConnectionPipe.InsulationEnum.IN_NONE) {
						value += cp.Vorlauf;
					}
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PipeLengthRuecklauf {
			get {
				double value = 0;
				foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
					value += cp.Ruecklauf;
				}
				return value;
			}
		}

		[XmlIgnore]
		public double PipeLengthRuecklaufNotIsolated {
			get {
				double value = 0;
				foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes) {
					if (cp.Insulation != ConnectionPipe.InsulationEnum.IN_VL_RL) {
						value += cp.Ruecklauf;
					}
				}
				return value;
			}
		}
	}
}
