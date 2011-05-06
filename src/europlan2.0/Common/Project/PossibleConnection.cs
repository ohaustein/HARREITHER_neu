using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;
using WW.Math.Geometry;

namespace Europlan.Common {
	public class PossibleConnection {
		private Point2D connectionPoint;
		private Polygon2D connectionArea;
		private bool possibleInput;
		private bool possibleOutput;
		private Product product;
		private Circuit circuit;
		private Distributor distributor;
		private int distributorPosition;
		private double rotation;

		public PossibleConnection() {
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
			//set { connectionPoint = value; }
		}

		public Polygon2D ConnectionArea {
			get { return connectionArea; }
			//set { connectionArea = value; }
		}

		public bool PossibleInput {
			get { return possibleInput; }
			//set { possibleInput = value; }
		}

		public bool PossibleOutput {
			get { return possibleOutput; }
			//set { possibleOutput = value; }
		}

		public Product Product {
			get { return this.product; }
			//set { this.product = value; }
		}

		public Circuit Circuit {
			get { return this.circuit; }
			//set { this.circuit = value; }
		}

		public Distributor Distributor {
			get { return this.distributor; }
		}

		public int DistributorIndex {
			get { return this.distributorPosition; }
		}

		public double Rotation {
			get { return this.rotation; }
		}
	}

	public class PossibleHithermRegisterConnection : PossibleConnection {
		private HithermRegister register;

		public PossibleHithermRegisterConnection() {
		}

		public PossibleHithermRegisterConnection(Point2D connectionPoint, Polygon2D connectionArea, bool possibleInput, bool possibleOutput, HithermProduct product, HithermCircuit circuit, HithermRegister register)
			: base(connectionPoint, connectionArea, possibleInput, possibleOutput, product, circuit, 0, 0) {
			this.register = register;
		}

		public HithermRegister Register {
			get { return this.register; }
		}
	}
}
