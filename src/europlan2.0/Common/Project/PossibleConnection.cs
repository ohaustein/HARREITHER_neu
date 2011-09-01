using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class PossibleConnection {
		protected Point2D connectionPoint;
		protected Polygon2D connectionArea;
		protected bool possibleInput;
		protected bool possibleOutput;
		protected Product product;
		protected Circuit circuit;
		protected Distributor distributor;
		protected int distributorPosition;
		protected double rotation;
		protected bool invertColors = false;
		protected bool connectHorizontal = true;
		protected bool connectVertical = true;
		protected Nullable<Vector2D> preferredStartVector = null;

		public PossibleConnection() {
		}

		public PossibleConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, double rotation, bool invertColors, bool connectHorizontal, bool connectVertical, Vector2D preferredStartVector) {
			this.connectionPoint = connectionPoint;
			this.connectionArea = connectionArea;
			this.possibleInput = possibleInput;
			this.possibleOutput = possibleOutput;
			this.product = null;
			this.circuit = null;
			this.distributor = null;
			this.distributorPosition = -1;
			this.rotation = rotation;
			this.invertColors = true;
			this.connectHorizontal = connectHorizontal;
			this.connectVertical = connectVertical;
			this.preferredStartVector = preferredStartVector;
		}

		public PossibleConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, Product product, Circuit circuit, double rotation, int i, bool connectHorizontal, bool connectVertical) {
			this.connectionPoint = connectionPoint;
			this.connectionArea = connectionArea;
			this.possibleInput = possibleInput;
			this.possibleOutput = possibleOutput;
			this.product = product;
			this.circuit = circuit;
			this.distributor = null;
			this.distributorPosition = -1;
			this.rotation = rotation;
			this.connectHorizontal = connectHorizontal;
			this.connectVertical = connectVertical;
		}

		public PossibleConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, Product product, Circuit circuit, double rotation, int i, bool connectHorizontal, bool connectVertical, Vector2D preferredStartVector) {
			this.connectionPoint = connectionPoint;
			this.connectionArea = connectionArea;
			this.possibleInput = possibleInput;
			this.possibleOutput = possibleOutput;
			this.product = product;
			this.circuit = circuit;
			this.distributor = null;
			this.distributorPosition = -1;
			this.rotation = rotation;
			this.connectHorizontal = connectHorizontal;
			this.connectVertical = connectVertical;
			this.preferredStartVector = preferredStartVector;
		}

		public PossibleConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, Product product, Circuit circuit, double rotation, int i) {
			this.connectionPoint = connectionPoint;
			this.connectionArea = connectionArea;
			this.possibleInput = possibleInput;
			this.possibleOutput = possibleOutput;
			this.product = product;
			this.circuit = circuit;
			this.distributor = null;
			this.distributorPosition = -1;
			this.rotation = rotation;
		}

		public PossibleConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, Distributor distributor, int position, double rotation, int i) {
			this.connectionPoint = connectionPoint;
			this.connectionArea = connectionArea;
			this.possibleInput = possibleInput;
			this.possibleOutput = possibleOutput;
			this.product = null;
			this.circuit = null;
			this.distributor = distributor;
			this.distributorPosition = position;
			this.rotation = rotation;
		}

		public Point2D ConnectionPoint {
			get { return connectionPoint; }
			set { connectionPoint = value; }
		}

		public Polygon2D ConnectionArea {
			get { return connectionArea; }
			set { connectionArea = value; }
		}

		public bool PossibleInput {
			get { return possibleInput; }
			set { possibleInput = value; }
		}

		public bool PossibleOutput {
			get { return possibleOutput; }
			set { possibleOutput = value; }
		}

		public Product Product {
			get { return this.product; }
			set { this.product = value; }
		}

		public Circuit Circuit {
			get { return this.circuit; }
			set { this.circuit = value; }
		}

		public Distributor Distributor {
			get { return this.distributor; }
			set { this.distributor = value; }
		}

		public int DistributorIndex {
			get { return this.distributorPosition; }
			set { this.distributorPosition = value; }
		}

		public double Rotation {
			get { return this.rotation; }
			set { this.rotation = value; }
		}

		public bool InvertColors {
			get { return this.invertColors; }
			set { this.invertColors = value; }
		}

		public bool ConnectHorizontal {
			get { return this.connectHorizontal; }
			set { this.connectHorizontal = value; }
		}

		public bool ConnectVertical {
			get { return this.connectVertical; }
			set { this.connectVertical = value; }
		}

		public Nullable<Vector2D> PreferredStartVector {
			get { return this.preferredStartVector; }
		}
	}

	public class PossibleHithermRegisterConnection : PossibleConnection {
		private HithermRegister register;

		public PossibleHithermRegisterConnection() {
		}

		public PossibleHithermRegisterConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, HithermProduct product, HithermCircuit circuit, HithermRegister register, bool connectHorizontal, bool connectVertical)
			: base(connectionPoint, connectionArea, possibleInput, possibleOutput, product, circuit, 0, 0, connectHorizontal, connectVertical) {
			this.register = register;
		}

		public PossibleHithermRegisterConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, HithermProduct product, HithermCircuit circuit, HithermRegister register, bool connectHorizontal, bool connectVertical, Vector2D preferredStartVector)
			: base(connectionPoint, connectionArea, possibleInput, possibleOutput, product, circuit, 0, 0, connectHorizontal, connectVertical, preferredStartVector) {
			this.register = register;
		}

		public HithermRegister Register {
			get { return this.register; }
		}
	}

	public class PossibleHithermCompactRegisterConnection : PossibleConnection {
		private HithermCompactRegister register;

		public PossibleHithermCompactRegisterConnection() {
		}

		public PossibleHithermCompactRegisterConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, HithermCompactProduct product, HithermCompactCircuit circuit, HithermCompactRegister register, bool connectHorizontal, bool connectVertical)
			: base(connectionPoint, connectionArea, possibleInput, possibleOutput, product, circuit, 0, 0, connectHorizontal, connectVertical) {
			this.register = register;
		}

		public PossibleHithermCompactRegisterConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, HithermCompactProduct product, HithermCompactCircuit circuit, HithermCompactRegister register, bool connectHorizontal, bool connectVertical, Vector2D preferredStartVector)
			: base(connectionPoint, connectionArea, possibleInput, possibleOutput, product, circuit, 0, 0, connectHorizontal, connectVertical, preferredStartVector) {
			this.register = register;
		}

		public HithermCompactRegister Register {
			get { return this.register; }
		}
	}

	public class PossibleProductConnection {
		protected Point2D connectionPoint;
		protected Polygon2D connectionArea;
		protected bool possibleInput;
		protected bool possibleOutput;
		protected Product product;
		protected Distributor distributor;
		protected int distributorStartPosition;
		protected int distributorCircuitCount;
		protected double rotation;
		protected bool firstCircuit;
		protected bool otherCircuits;

		protected GraphicalProductConnection productConnection;
		protected int segmentId = -1;
		protected double distFromSegmentStart = -1;

		public PossibleProductConnection() {
		}

		public PossibleProductConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, double rotation, Product product, bool firstCircuit, bool otherCircuits) {
			this.connectionPoint = connectionPoint;
			this.connectionArea = connectionArea;
			this.possibleInput = possibleInput;
			this.possibleOutput = possibleOutput;
			this.product = product;
			this.rotation = rotation;
			this.firstCircuit = firstCircuit;
			this.otherCircuits = otherCircuits;
			this.distributor = null;
			this.distributorStartPosition = -1;
			this.distributorCircuitCount = 0;
		}

		public PossibleProductConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, double rotation, Distributor distributor, int distributorStartPosition, int distributorCircuitCount) {
			this.connectionPoint = connectionPoint;
			this.connectionArea = connectionArea;
			this.possibleInput = possibleInput;
			this.possibleOutput = possibleOutput;
			this.product = null;
			this.rotation = rotation;
			this.firstCircuit = false;
			this.otherCircuits = false;
			this.distributor = distributor;
			this.distributorStartPosition = distributorStartPosition;
			this.distributorCircuitCount = distributorCircuitCount;
		}

		public PossibleProductConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, double rotation, GraphicalProductConnection productConnection, int segmentId, double distFromSegmentStart) {
			this.connectionPoint = connectionPoint;
			this.connectionArea = connectionArea;
			this.possibleInput = possibleInput;
			this.possibleOutput = possibleOutput;
			this.product = null;
			this.rotation = rotation;
			this.firstCircuit = false;
			this.otherCircuits = false;
			this.productConnection = productConnection;
			this.segmentId = segmentId;
			this.distFromSegmentStart = distFromSegmentStart;
		}

		public Point2D ConnectionPoint {
			get { return connectionPoint; }
			set { connectionPoint = value; }
		}

		public Polygon2D ConnectionArea {
			get { return connectionArea; }
			set { connectionArea = value; }
		}

		public bool PossibleInput {
			get { return possibleInput; }
			set { possibleInput = value; }
		}

		public bool PossibleOutput {
			get { return possibleOutput; }
			set { possibleOutput = value; }
		}

		public Product Product {
			get { return this.product; }
			set { this.product = value; }
		}

		public GraphicalProductConnection ProductConnection {
			get { return this.productConnection; }
			set { this.productConnection = value; }
		}

		/*public List<Circuit> Circuits {
			get {
				List<Circuit> circuits = new List<Circuit>();
				if (this.firstCircuit) {
					circuits.Add(this.product.PlannedCircuits[0]);
				}
				if (this.otherCircuits) {
					for (int i = 1; i < this.product.PlannedCircuits.Count; i++) {
						circuits.Add(this.product.PlannedCircuits[i]);
					}
				}
				foreach (GraphicalProductConnection conn in this.product.Connections) {
					foreach (Circuit c in conn.ProductCircuits) {
						if (circuits.Contains(c)) {
							circuits.Remove(c);
						}
					}
				}
				return circuits;
			}
			//set { this.circuits = value; }
		}*/

		public int ProductCircuitCount {
			get {
				int circuitCount = 0;
				if (this.product != null) {
					if (this.firstCircuit && this.product.PlannedCircuits.Count > 0) {
						circuitCount++;
					}
					if (this.otherCircuits && this.product.PlannedCircuits.Count > 0) {
						circuitCount += this.product.PlannedCircuits.Count - 1;
					}
				}
				return circuitCount;
			}
		}

		public bool ProductFirstCircuit {
			get { return this.firstCircuit; }
			set { this.firstCircuit = value; }
		}

		public bool ProductOtherCircuits {
			get { return this.otherCircuits; }
			set { this.otherCircuits = value; }
		}

		public double Rotation {
			get { return this.rotation; }
			set { this.rotation = value; }
		}

		public Distributor Distributor {
			get { return this.distributor; }
			set { this.distributor = value; }
		}

		public int DistributorStartPosition {
			get { return this.distributorStartPosition; }
			set { this.distributorStartPosition = value; }
		}

		public int DistributorCircuitCount {
			get { return this.distributorCircuitCount; }
			set { this.distributorCircuitCount = value; }
		}

		public int SegmentId {
			get { return this.segmentId; }
			set { this.segmentId = value; }
		}

		public double DistFromSegmentStart {
			get { return this.distFromSegmentStart; }
			set { this.distFromSegmentStart = value; }
		}
		/*public List<int> DistributorIndices {
			// TODO
			get { return new List<int>(); }
			//set { }
		}*/
	}
}