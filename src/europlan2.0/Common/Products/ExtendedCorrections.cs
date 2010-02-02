using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ExtendedCorrections {
		private int circuitNr;
		private bool correctArea = false;
		private double areaPercentage = -1.0;
		private bool correctRim = false;
		private double rimLengthPercentage = -1.0;
		private int rimCornersValue = 0;
		private bool correctConnections = false;
		private double connectionsPercentage = -1.0;

		private EurovalProduct evProduct;

		public ExtendedCorrections() {
			this.circuitNr = 0;
		}

		public ExtendedCorrections(int circuitNr) {
			this.circuitNr = circuitNr;
		}

		public ExtendedCorrections(int ciruitNr, Nullable<double> area, Nullable<double> rimLength, Nullable<int> rimCorners, Nullable<double> connections, EurovalProduct product) {
			circuitNr = 0;
			correctArea = area.HasValue;
			if (correctArea) {
				this.areaPercentage = area.Value;
			}
			correctRim = rimLength.HasValue;
			if (correctRim) {
				this.rimLengthPercentage = rimLength.Value;
				this.rimCornersValue = rimCorners.HasValue ? rimCorners.Value : 0;
			}
			correctConnections = connections.HasValue;
			if (correctConnections) {
				this.connectionsPercentage = connections.Value;
			}
			this.evProduct = product;
		}

		public bool CorrectArea {
			get { return this.correctArea; }
			set { this.correctArea = value; }
		}

		public double AreaPercentage {
			get {
				if (this.correctArea) {
					return this.areaPercentage;
				} else {
					return this.AreaValue * 100 / this.evProduct.PlannedFloorArea;
				}
			}
			set { this.areaPercentage = value; }
		}

		public double AreaValue {
			get {
				if (this.correctArea) {
					return this.evProduct.PlannedFloorArea * this.areaPercentage / 100;
				} else {
					double area = this.evProduct.PlannedFloorArea;
					double circuits = 0;
					foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
						if (ec.CorrectArea) {
							area -= ec.AreaValue;
						} else {
							circuits++;
						}
					}
					if (area < 0) {
						area = 0;
					}
					return area / circuits;
				}
			}
			set {
				this.areaPercentage = value * 100 / this.evProduct.PlannedFloorArea;
			}
		}

		public double AreaReducedValue {
			get { return this.evProduct.PlannedAreaReduced * this.AreaPercentage / 100; }
		}

		public double AreaUnheatedValue {
			get { return this.evProduct.PlannedAreaUnheated * this.AreaPercentage / 100; }
		}

		public bool CorrectRim {
			get { return this.correctRim; }
			set { this.correctRim = value; }
		}

		public double RimPercentage {
			get { return this.rimLengthPercentage; }
			set { this.rimLengthPercentage = value; }
		}

		public double RimLengthValue {
			get {
				if (this.correctRim) {
					return this.evProduct.PlannedRimLength * this.rimLengthPercentage / 100;
				} else {
					double rim = this.evProduct.PlannedRimLength;
					double circuits = 0;
					foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
						if (ec.CorrectRim) {
							rim -= ec.RimLengthValue;
						} else {
							circuits++;
						}
					}
					if (rim < 0) {
						rim = 0;
					}
					return rim / circuits;
				}
			}
			set {
				this.rimLengthPercentage = value * 100 / this.evProduct.PlannedRimLength;
			}
		}

		public int RimCornersValue {
			get {
				if (this.correctRim) {
					return this.rimCornersValue;
				} else {
					int corners = this.evProduct.PlannedRimCorners;
					int circuits = 0;
					foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
						if (ec.CorrectRim) {
							corners -= ec.RimCornersValue;
						} else {
							circuits++;
						}
					}
					return corners / circuits;
				}
			}
		}

		public bool CorrectConnections {
			get { return this.correctConnections; }
			set { this.correctConnections = value; }
		}

		public double ConnectionsPercentage {
			get { return this.connectionsPercentage; }
			set { this.connectionsPercentage = value; }
		}

		public double ConnectionsValue {
			get {
				if (this.correctConnections) {
					return this.evProduct.PlannedRemoveArea * this.connectionsPercentage / 100;
				} else {
					double connections = this.evProduct.PlannedRemoveArea;
					double circuits = 0;
					foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
						if (ec.CorrectConnections) {
							connections -= ec.ConnectionsValue;
						} else {
							circuits++;
						}
					}
					if (connections < 0) {
						connections = 0;
					}
					return connections / circuits;
				}
			}
			set {
				this.connectionsPercentage = value * 100 / this.evProduct.PlannedRemoveArea;
			}
		}

		public int CircuitNr {
			get { return this.circuitNr; }
			set { this.circuitNr = value; }
		}
	}
}
