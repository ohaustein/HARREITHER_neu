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

		public PossibleConnection() {
		}

		public PossibleConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, double rotation, bool invertColors, bool connectHorizontal, bool connectVertical) {
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
	}

	public class PossibleHithermRegisterConnection : PossibleConnection {
		private HithermRegister register;

		public PossibleHithermRegisterConnection() {
		}

		public PossibleHithermRegisterConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, HithermProduct product, HithermCircuit circuit, HithermRegister register, bool connectHorizontal, bool connectVertical)
			: base(connectionPoint, connectionArea, possibleInput, possibleOutput, product, circuit, 0, 0, connectHorizontal, connectVertical) {
			this.register = register;
		}

		public HithermRegister Register {
			get { return this.register; }
		}
	}
}
