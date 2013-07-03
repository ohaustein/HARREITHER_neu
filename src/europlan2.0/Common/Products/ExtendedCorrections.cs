using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

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

		private bool sum = false;

		private EurovalProduct evProduct;
		private EcothermProduct ecProduct;
		private JumbovalProduct jvProduct;

		private ExtendedCorrections() {
			this.circuitNr = 0;
		}

		public ExtendedCorrections(EurovalProduct product) {
			this.sum = true;
			this.evProduct = product;
		}

		public ExtendedCorrections(EcothermProduct product) {
			this.sum = true;
			this.ecProduct = product;
		}

		public ExtendedCorrections(JumbovalProduct product) {
			this.sum = true;
			this.jvProduct = product;
		}

		public ExtendedCorrections(int circuitNr, EurovalProduct product) {
			this.circuitNr = circuitNr;
			this.evProduct = product;
		}

		public ExtendedCorrections(int circuitNr, EcothermProduct product) {
			this.circuitNr = circuitNr;
			this.ecProduct = product;
		}

		public ExtendedCorrections(int circuitNr, JumbovalProduct product) {
			this.circuitNr = circuitNr;
			this.jvProduct = product;
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

		public ExtendedCorrections(int ciruitNr, Nullable<double> area, Nullable<double> rimLength, Nullable<int> rimCorners, Nullable<double> connections, EcothermProduct product) {
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
			this.ecProduct = product;
		}

		public ExtendedCorrections(int ciruitNr, Nullable<double> area, Nullable<double> rimLength, Nullable<int> rimCorners, Nullable<double> connections, JumbovalProduct product) {
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
			this.jvProduct = product;
		}

		public bool CorrectArea {
			get { return this.correctArea; }
			set {
				if ((this.evProduct != null || this.ecProduct != null || this.jvProduct != null) && !this.correctArea && value) {
					this.AreaPercentage = this.AreaPercentage;
				}
				this.correctArea = value;
			}
		}

		public double AreaPercentage {
			get {
				if (sum) {
					double perc = 0.0;
					if (this.evProduct != null) {
						foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
							perc += ec.AreaPercentage;
						}
					} else if (this.ecProduct != null) {
						foreach (ExtendedCorrections ec in this.ecProduct.PlannedCorrectionList) {
							perc += ec.AreaPercentage;
						}
					} else if (this.jvProduct != null) {
						foreach (ExtendedCorrections ec in this.jvProduct.PlannedCorrectionList) {
							perc += ec.AreaPercentage;
						}
					}
					return perc;
				}
				if (this.correctArea) {
					return this.areaPercentage;
				} else {
					double perc = 100.0;
					double circuits = 0;
					if (this.evProduct != null) {
						foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
							if (ec.CorrectArea) {
								perc -= ec.AreaPercentage;
							} else {
								circuits++;
							}
						}
					} else if (this.ecProduct != null) {
						foreach (ExtendedCorrections ec in this.ecProduct.PlannedCorrectionList) {
							if (ec.CorrectArea) {
								perc -= ec.AreaPercentage;
							} else {
								circuits++;
							}
						}
					} else if (this.jvProduct != null) {
						foreach (ExtendedCorrections ec in this.jvProduct.PlannedCorrectionList) {
							if (ec.CorrectArea) {
								perc -= ec.AreaPercentage;
							} else {
								circuits++;
							}
						}
					}
					if (circuits < 1) {
						circuits = 1;
					}
					if (perc < 0) {
						perc = 0;
					}
					return perc / circuits;
				}
			}
			set { this.areaPercentage = value; }
		}

		[XmlIgnore]
		public double AreaValue {
			get {
				if (this.evProduct != null) {
					return this.evProduct.PlannedFloorArea * this.AreaPercentage / 100;
				} else if (this.ecProduct != null) {
					return this.ecProduct.PlannedFloorArea * this.AreaPercentage / 100;
				} else if (this.jvProduct != null) {
					return this.jvProduct.PlannedFloorArea * this.AreaPercentage / 100;
				} else {
					return 0;
				}
			}
			set {
				if (this.evProduct != null) {
					this.areaPercentage = value * 100 / this.evProduct.PlannedFloorArea;
				} else if (this.ecProduct != null) {
					this.areaPercentage = value * 100 / this.ecProduct.PlannedFloorArea;
				} else if (this.jvProduct != null) {
					this.areaPercentage = value * 100 / this.jvProduct.PlannedFloorArea;
				}
			}
		}

		[XmlIgnore]
		public double AreaReducedValue {
			get {
				if (this.evProduct != null) {
					return this.evProduct.PlannedAreaReduced * this.AreaPercentage / 100;
				} else if (this.ecProduct != null) {
					return this.ecProduct.PlannedAreaReduced * this.AreaPercentage / 100;
				} else if (this.jvProduct != null) {
					return this.jvProduct.PlannedAreaReduced * this.AreaPercentage / 100;
				} else {
					return 0;
				}
			}
		}

		[XmlIgnore]
		public double AreaUnheatedValue {
			get {
				if (this.evProduct != null) {
					return this.evProduct.PlannedAreaUnheated * this.AreaPercentage / 100;
				} else if (this.ecProduct != null) {
					return this.ecProduct.PlannedAreaUnheated * this.AreaPercentage / 100;
				} else if (this.jvProduct != null) {
					return this.jvProduct.PlannedAreaUnheated * this.AreaPercentage / 100;
				} else {
					return 0;
				}
			}
		}

		public bool CorrectRim {
			get { return this.correctRim; }
			set {
				if ((this.evProduct != null || this.ecProduct != null || this.jvProduct != null) && !this.correctRim && value) {
					this.RimPercentage = this.RimPercentage;
					this.RimCornersValue = this.RimCornersValue;
				}
				this.correctRim = value;
			}
		}

		public double RimPercentage {
			get {
				if (sum) {
					double perc = 0.0;
					if (this.evProduct != null) {
						foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
							perc += ec.RimPercentage;
						}
					} else if (this.ecProduct != null) {
						foreach (ExtendedCorrections ec in this.ecProduct.PlannedCorrectionList) {
							perc += ec.RimPercentage;
						}
					} else if (this.jvProduct != null) {
						foreach (ExtendedCorrections ec in this.jvProduct.PlannedCorrectionList) {
							perc += ec.RimPercentage;
						}
					} 
					return perc;
				}
				if (this.correctRim) {
					return this.rimLengthPercentage;
				} else {
					double perc = 100.0;
					double circuits = 0;
					if (this.evProduct != null) {
						foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
							if (ec.CorrectRim) {
								perc -= ec.RimPercentage;
							} else {
								circuits++;
							}
						}
					} else if (this.ecProduct != null) {
						foreach (ExtendedCorrections ec in this.ecProduct.PlannedCorrectionList) {
							if (ec.CorrectRim) {
								perc -= ec.RimPercentage;
							} else {
								circuits++;
							}
						}
					} else if (this.jvProduct != null) {
						foreach (ExtendedCorrections ec in this.jvProduct.PlannedCorrectionList) {
							if (ec.CorrectRim) {
								perc -= ec.RimPercentage;
							} else {
								circuits++;
							}
						}
					}
					if (perc < 0) {
						perc = 0;
					}
					if (circuits < 1) {
						circuits = 1;
					}
					return perc / circuits;
				}
			}
			set { this.rimLengthPercentage = value; }
		}

		[XmlIgnore]
		public double RimLengthValue {
			get {
				if (this.evProduct != null) {
					return this.evProduct.PlannedRimLength * this.RimPercentage / 100;
				} else if (this.ecProduct != null) {
					return this.ecProduct.PlannedRimLength * this.RimPercentage / 100;
				}  else if (this.jvProduct != null) {
					return this.jvProduct.PlannedRimLength * this.RimPercentage / 100;
				} else {
					return 0;
				}
			}
			set {
				if (this.evProduct != null) {
					this.rimLengthPercentage = value * 100 / this.evProduct.PlannedRimLength;
				} else if (this.ecProduct != null) {
					this.rimLengthPercentage = value * 100 / this.ecProduct.PlannedRimLength;
				} else if (this.jvProduct != null) {
					this.rimLengthPercentage = value * 100 / this.jvProduct.PlannedRimLength;
				}
			}
		}

		public int RimCornersValue {
			get {
				if (sum) {
					int corners = 0;
					if (this.evProduct != null) {
						foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
							corners += ec.RimCornersValue;
						}
					} else if (this.ecProduct != null) {
						foreach (ExtendedCorrections ec in this.ecProduct.PlannedCorrectionList) {
							corners += ec.RimCornersValue;
						}
					} else if (this.jvProduct != null) {
						foreach (ExtendedCorrections ec in this.jvProduct.PlannedCorrectionList) {
							corners += ec.RimCornersValue;
						}
					}
					return corners;
				}
				if (this.correctRim) {
					return this.rimCornersValue;
				} else {
					int corners = 0;
					if (this.evProduct != null) {
						corners = this.evProduct.PlannedRimCorners;
					} else if (this.ecProduct != null) {
						corners = this.ecProduct.PlannedRimCorners;
					} else if (this.jvProduct != null) {
						corners = this.jvProduct.PlannedRimCorners;
					}
					int circuits = 0;
					int circuitsBefore = 0;
					if (this.evProduct != null) {
						foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
							if (ec.CorrectRim) {
								corners -= ec.RimCornersValue;
							} else {
								if (ec.CircuitNr < this.circuitNr) {
									circuitsBefore++;
								}
								circuits++;
							}
						}
					} else if (this.ecProduct != null) {
						foreach (ExtendedCorrections ec in this.ecProduct.PlannedCorrectionList) {
							if (ec.CorrectRim) {
								corners -= ec.RimCornersValue;
							} else {
								if (ec.CircuitNr < this.circuitNr) {
									circuitsBefore++;
								}
								circuits++;
							}
						}
					} else if (this.jvProduct != null) {
						foreach (ExtendedCorrections ec in this.jvProduct.PlannedCorrectionList) {
							if (ec.CorrectRim) {
								corners -= ec.RimCornersValue;
							} else {
								if (ec.CircuitNr < this.circuitNr) {
									circuitsBefore++;
								}
								circuits++;
							}
						}
					}
					if (circuits < 1) {
						circuits = 1;
					}
					if (corners >= 0) {
						return corners / circuits + (corners % circuits > circuitsBefore ? 1 : 0);
					} else {
						corners = -corners;
						return -(corners / circuits + (corners % circuits > circuitsBefore ? 1 : 0));
					}
				}
			}
			set { this.rimCornersValue = value; }
		}

		[XmlIgnore]
		public string RimCornersString {
			get {
				if (sum) {
					if (this.evProduct != null) {
						return RimCornersValue.ToString() + " (" + this.evProduct.PlannedRimCorners.ToString() + ")";
					} else if (this.ecProduct != null) {
						return RimCornersValue.ToString() + " (" + this.ecProduct.PlannedRimCorners.ToString() + ")";
					}  else if (this.jvProduct != null) {
						return RimCornersValue.ToString() + " (" + this.jvProduct.PlannedRimCorners.ToString() + ")";
					} else {
						return RimCornersValue.ToString() + " (0)";
					}
				} else {
					return RimCornersValue.ToString();
				}
			}
			set {
				int val;
				if (Int32.TryParse(value, out val)) {
					this.RimCornersValue = val;
				}
			}
		}

		public bool CorrectConnections {
			get { return this.correctConnections; }
			set {
				if ((this.evProduct != null || this.ecProduct != null || this.jvProduct != null) && !this.correctConnections && value) {
					this.ConnectionsPercentage = this.ConnectionsPercentage;
				}
				this.correctConnections = value; 
			}
		}

		public double ConnectionsPercentage {
			get {
				if (sum) {
					double perc = 0.0;
					if (this.evProduct != null) {
						foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
							perc += ec.ConnectionsPercentage;
						}
					} else if (this.ecProduct != null) {
						foreach (ExtendedCorrections ec in this.ecProduct.PlannedCorrectionList) {
							perc += ec.ConnectionsPercentage;
						}
					} else if (this.jvProduct != null) {
						foreach (ExtendedCorrections ec in this.jvProduct.PlannedCorrectionList) {
							perc += ec.ConnectionsPercentage;
						}
					}
					return perc;
				}
				if (this.correctConnections) {
					return this.connectionsPercentage;
				} else {
					double perc = 100.0;
					double circuits = 0;
					if (this.evProduct != null) {
						foreach (ExtendedCorrections ec in this.evProduct.PlannedCorrectionList) {
							if (ec.CorrectConnections) {
								perc -= ec.ConnectionsPercentage;
							} else {
								circuits++;
							}
						}
					} else if (this.ecProduct != null) {
						foreach (ExtendedCorrections ec in this.ecProduct.PlannedCorrectionList) {
							if (ec.CorrectConnections) {
								perc -= ec.ConnectionsPercentage;
							} else {
								circuits++;
							}
						}
					} else if (this.jvProduct != null) {
						foreach (ExtendedCorrections ec in this.jvProduct.PlannedCorrectionList) {
							if (ec.CorrectConnections) {
								perc -= ec.ConnectionsPercentage;
							} else {
								circuits++;
							}
						}
					}
					if (perc < 0) {
						perc = 0;
					}
					if (circuits < 1) {
						circuits = 1;
					}
					return perc / circuits;
				}
			}
			set { this.connectionsPercentage = value; }
		}

		[XmlIgnore]
		public double ConnectionsValue {
			get {
				if (this.evProduct != null) {
					return this.evProduct.PlannedRemoveArea * this.ConnectionsPercentage / 100;
				} else if (this.ecProduct != null) {
					return this.ecProduct.PlannedRemoveArea * this.ConnectionsPercentage / 100;
				}  else if (this.jvProduct != null) {
					return this.jvProduct.PlannedRemoveArea * this.ConnectionsPercentage / 100;
				} else {
					return 0;
				}
			}
			set {
				if (this.evProduct != null) {
					this.connectionsPercentage = value * 100 / this.evProduct.PlannedRemoveArea;
				} else if (this.ecProduct != null) {
					this.connectionsPercentage = value * 100 / this.ecProduct.PlannedRemoveArea;
				} else if (this.jvProduct != null) {
					this.connectionsPercentage = value * 100 / this.jvProduct.PlannedRemoveArea;
				}
			}
		}

		[XmlIgnore]
		public int CircuitNr {
			get { return this.circuitNr; }
			set { this.circuitNr = value; }
		}

		[XmlIgnore]
		public EurovalProduct EurovalProduct {
			set { this.evProduct = value; }
		}

		[XmlIgnore]
		public EcothermProduct EcothermProduct {
			set { this.ecProduct = value; }
		}

		[XmlIgnore]
		public JumbovalProduct JumbovalProduct {
			set { this.jvProduct = value; }
		}

		[XmlIgnore]
		public bool Sum {
			get { return sum; }
		}
	}
}
