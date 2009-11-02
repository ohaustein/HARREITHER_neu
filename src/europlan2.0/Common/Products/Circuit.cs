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
			private CircuitConnectionTypeEnum type;

			public CircuitConnectionTypeEnum CircuitConnectionType {
				get { return type; }
				set { type = value; }
			}

			private Circuit otherCircuit;

			public Circuit OtherCircuit {
				get { return otherCircuit; }
				set { otherCircuit = value; }
			}

			public CircuitConnection() {
			}

			public CircuitConnection(CircuitConnectionTypeEnum type, Circuit otherCircuit) {
				this.type = type;
				this.otherCircuit = otherCircuit;
			}
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
		public double PipeLengthWithoutOtherProduct {
			get {
				return this.PipeLengthWithoutConnections + this.PipeLengthVorlaufWithoutOtherProductTotal + this.PipeLengthRuecklaufWithoutOtherProductTotal;
			}
		}

		[XmlIgnore]
		public double PipeLengthWithoutOtherProductNotIsolated {
			get {
				return this.PipeLengthWithoutConnections + this.PipeLengthVorlaufWithoutOtherProductNotIsolated + this.PipeLengthRuecklaufWithoutOtherProductNotIsolated;
			}
		}

		/*[XmlIgnore]
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
		}*/

		#region Anbindung
		protected double vorlaufTotal;
		[XmlIgnore]
		public double PipeLengthVorlaufTotal {
			get { return this.vorlaufTotal; }
			set { this.vorlaufTotal = value; }
		}

		protected double vorlaufNotIsolated;
		[XmlIgnore]
		public double PipeLengthVorlaufNotIsolated {
			get { return this.vorlaufNotIsolated; }
			set { this.vorlaufNotIsolated = value; }
		}

		protected double ruecklaufTotal;
		[XmlIgnore]
		public double PipeLengthRuecklaufTotal {
			get { return this.ruecklaufTotal; }
			set { this.ruecklaufTotal = value; }
		}

		protected double ruecklaufNotIsolated;
		[XmlIgnore]
		public double PipeLengthRuecklaufNotIsolated {
			get { return this.ruecklaufNotIsolated; }
			set { this.ruecklaufNotIsolated = value; }
		}

		protected double vorlaufWithoutOtherProductTotal;
		[XmlIgnore]
		public double PipeLengthVorlaufWithoutOtherProductTotal {
			get { return this.vorlaufWithoutOtherProductTotal; }
			set { this.vorlaufWithoutOtherProductTotal = value; }
		}

		protected double vorlaufWithoutOtherProductNotIsolated;
		[XmlIgnore]
		public double PipeLengthVorlaufWithoutOtherProductNotIsolated {
			get { return this.vorlaufWithoutOtherProductNotIsolated; }
			set { this.vorlaufWithoutOtherProductNotIsolated = value; }
		}

		protected double ruecklaufWithoutOtherProductTotal;
		[XmlIgnore]
		public double PipeLengthRuecklaufWithoutOtherProductTotal {
			get { return this.ruecklaufWithoutOtherProductTotal; }
			set { this.ruecklaufWithoutOtherProductTotal = value; }
		}

		protected double ruecklaufWithoutOtherProductNotIsolated;
		[XmlIgnore]
		public double PipeLengthRuecklaufWithoutOtherProductNotIsolated {
			get { return this.ruecklaufWithoutOtherProductNotIsolated; }
			set { this.ruecklaufWithoutOtherProductNotIsolated = value; }
		}


		#endregion Anbindung
	}
}
