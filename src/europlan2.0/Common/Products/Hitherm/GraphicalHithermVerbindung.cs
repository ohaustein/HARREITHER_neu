using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using System.Drawing;
using System.Xml.Serialization;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;

namespace Europlan.Common {
	public class GraphicalHithermVerbindung : GenericGraphicalWallVerbindungImplementation<HithermProduct, HithermCircuit, HithermRegister, GraphicalHithermRegisterWrapper, GraphicalHithermVerbindung> {

		internal GraphicalHithermVerbindung()
			: base() {
		}

		internal GraphicalHithermVerbindung(bool finished)
			: base(finished) {
		}

		public GraphicalHithermVerbindung(HithermRegister start, HithermRegister end, IEnumerable<Point2D> vertices, HithermCircuit circuit, PlannedProduct product)
			: base (start, end, vertices, circuit, product) {
		}

		/*private HithermRegister start;
		private HithermRegister end;
		private HithermCircuit circuit;

		internal GraphicalHithermVerbindung() : base() {
		}

		internal GraphicalHithermVerbindung(bool finished) : base(finished) {
		}

		public GraphicalHithermVerbindung(HithermRegister start, HithermRegister end, IEnumerable<Point2D> vertices, HithermCircuit circuit, PlannedProduct product) {
			this.start = start;
			this.end = end;
			this.InitializeVertices(vertices);
			this.circuit = circuit;
			this.product = product;
		}

		public void FinalizeLoading() {
			PlannedProduct tmpProduct = this.Product;
			HithermCircuit tmpCircuit = this.Circuit;
			HithermRegister tmpRegister = this.End;
			tmpRegister = this.Start;
		}

		[XmlIgnore]
		public HithermRegister Start {
			get {
				if (this.startIndex >= 0) {
					if (this.Circuit is HithermCircuit) {
						HithermCircuit hc = this.Circuit as HithermCircuit;
						this.start = hc.Registers[this.startIndex];
						this.startIndex = -1;
					}
				}
				return this.start;
			}
		}

		[XmlIgnore]
		public HithermRegister End {
			get {
				if (this.endIndex >= 0) {
					if (this.Circuit is HithermCircuit) {
						HithermCircuit hc = this.Circuit as HithermCircuit;
						this.end = hc.Registers[this.endIndex];
						this.endIndex = -1;
					}
				}
				return this.end;
			}
		}

		public override int StartIndex {
			get {
				if (this.startIndex >= 0) {
					return this.startIndex;
				}
				int index = -1;
				if (this.Circuit is HithermCircuit) {
					HithermCircuit hc = this.Circuit as HithermCircuit;
					int i = 0;
					foreach (HithermRegister r in hc.Registers) {
						if (r == this.start) {
							index = i;
							break;
						}
						i++;
					}
				}
				return index;
			}
			set { this.startIndex = value; }
		}

		public override int EndIndex {
			get {
				if (this.endIndex >= 0) {
					return this.endIndex;
				}
				int index = -1;
				if (this.Circuit is HithermCircuit) {
					HithermCircuit hc = this.Circuit as HithermCircuit;
					int i = 0;
					foreach (HithermRegister r in hc.Registers) {
						if (r == this.end) {
							index = i;
							break;
						}
						i++;
					}
				}
				return index;
			}
			set { this.endIndex = value; }
		}

		[XmlIgnore]
		public HithermCircuit Circuit {
			get {
				if (this.circuitIndex >= 0) {
					this.circuit = this.Product.Product.PlannedCircuits[this.circuitIndex] as HithermCircuit;
					this.circuitIndex = -1;
				}
				return this.circuit;
			}
			set {
				this.circuit = value;
				this.circuitIndex = -1;
			}
		}

		public override int CircuitIndex {
			get {
				if (this.circuitIndex >= 0) {
					return this.circuitIndex;
				}
				int index = -1;
				int i = 0;
				foreach (Circuit c in this.Product.Product.PlannedCircuits) {
					if (c == this.circuit) {
						index = i;
						break;
					}
					i++;
				}
				return index;
			}
			set { this.circuitIndex = value; }
		}

		[XmlIgnore]
		public override bool HasStart {
			get {
				return this.start != null;
			}
		}

		[XmlIgnore]
		public override bool HasEnd {
			get {
				return this.end != null;
			}
		}

		[XmlIgnore]
		public override int HkId {
			get {
				if (this.circuit != null && this.circuit.Registers.Count > 0) {
					return this.circuit.Registers[0].Heizkreis;
				}
				return -1;
			}
		}*/
	}
}
