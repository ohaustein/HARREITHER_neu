using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	[XmlInclude(typeof(EurovalCircuit))]
	[XmlInclude(typeof(EcothermCircuit))]
	[XmlInclude(typeof(ModulDeckeCircuit))]
	[XmlInclude(typeof(ModulBodenCircuit))]
	[XmlInclude(typeof(HithermCircuit))]
	[XmlInclude(typeof(HithermCompactCircuit))]
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

			[XmlIgnore]
			public Circuit OtherCircuit {
				get { return OtherProduct == null ? null : OtherProduct.GetCircuit(otherCircuitId); }
				set {
					otherProduct = value.PlannedProduct;
					otherCircuitId = value.NrOfCircuit;
				}
			}

			private PlannedProduct otherProduct;
			[XmlIgnore]
			public Product OtherProduct {
				get {
					if (this.otherProductId != null) {
						this.otherProduct = null;
						foreach (Floor f in Project.Instance.Floors) {
							foreach (Room r in f.Rooms) {
								foreach (PlannedProduct pp in r.PlannedProducts) {
									if (pp.Id == this.otherProductId) {
										this.otherProduct = pp;
										this.otherProductId = null;
										break;
									}
								}
								if (this.otherProductId == null) {
									break;
								}
							}
							if (this.otherProductId == null) {
								break;
							}
						}
						this.otherProductId = null;
					}
					return otherProduct.Product;
				}
				/*set { otherProduct = value; }*/
			}

			private string otherProductId;
			public string OtherProductId {
				get { return this.otherProductId != null ? this.otherProductId : (this.otherProduct == null ? null : this.otherProduct.Id); }
				set { this.otherProductId = value; }
			}
			
			private int otherCircuitId;
			public int OtherCircuitId {
				get { return otherCircuitId; }
				set { otherCircuitId = value; }
			}

			public CircuitConnection() {
			}

			public CircuitConnection(CircuitConnectionTypeEnum type, Circuit otherCircuit) {
				this.type = type;
				this.OtherCircuit = otherCircuit;
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

		protected double c_Qh2oHeat;
		[XmlIgnore]
		public double C_Qh2oHeat {
			get { return this.c_Qh2oHeat; }
			set { this.c_Qh2oHeat = value; }
		}

		protected double c_Qh2oCool;
		[XmlIgnore]
		public double C_Qh2oCool {
			get { return this.c_Qh2oCool; }
			set { this.c_Qh2oCool = value; }
		}

		protected double c_druckverlustHeat;
		[XmlIgnore]
		public double C_DruckverlustHeat {
			get { return this.c_druckverlustHeat; }
		}

		[XmlIgnore]
		public double C_DruckverlustDistributorHeat {
			get {
				if (c_durchflussHeat > 0) {
					double druckverlust = Math.Pow((c_durchflussHeat / 1000) / EN1264.KVSValue, 2) * 1000;
					druckverlust = druckverlust < 1.2 ? 1.2 : druckverlust;
					druckverlust = druckverlust > 20 ? 20 : druckverlust;
					return druckverlust;
				}
				return 0;
			}
		}

		protected double c_durchflussHeat;
		[XmlIgnore]
		public double C_DurchflussHeat {
			get { return this.c_durchflussHeat; }
		}

		protected double c_flussGeschwindigkeitHeat;
		[XmlIgnore]
		public double C_FlussGeschwindigkeitHeat {
			get { return this.c_flussGeschwindigkeitHeat; }
		}

		protected double c_druckverlustCool;
		[XmlIgnore]
		public double C_DruckverlustCool {
			get { return this.c_druckverlustCool; }
		}

		[XmlIgnore]
		public double C_DruckverlustDistributorCool {
			get {
				if (c_durchflussCool > 0) {
					double druckverlust = Math.Pow((c_durchflussCool / 1000) / EN1264.KVSValue, 2) * 1000;
					//druckverlust = druckverlust < 1.2 ? 1.2 : druckverlust;
					//druckverlust = druckverlust > 20 ? 20 : druckverlust;
					return druckverlust;
				}
				return 0;
			}
		}

		protected double c_durchflussCool;
		[XmlIgnore]
		public double C_DurchflussCool {
			get { return this.c_durchflussCool; }
		}

		protected double c_flussGeschwindigkeitCool;
		[XmlIgnore]
		public double C_FlussGeschwindigkeitCool {
			get { return this.c_flussGeschwindigkeitCool; }
		}

		#endregion Anbindung

		internal virtual void FinalizeLoading() {
			// nothing to do
		}
	}
}
