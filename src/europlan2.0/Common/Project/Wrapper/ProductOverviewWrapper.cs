using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class ProductOverviewWrapper {

		private PlannedProduct plannedProduct;
		
		public ProductOverviewWrapper(PlannedProduct pp) {
			plannedProduct = pp;
		}

		public PlannedProduct PlannedProduct {
			get { return plannedProduct; }
		}
		
		public string RoomId {
			get { return plannedProduct.Product.AssociatedRoom.Id; }
		}

		public string RoomName {
			get { return plannedProduct.Product.AssociatedRoom.Name; }
		}

		public string TeilSystem {
			get { return plannedProduct.InternalName; }
		}

		public string SystemName {
			get { return plannedProduct.Product.FullName; }
		}

		public Nullable<int> NrOfCircuits {
			get {
				if (plannedProduct.Product.PlannedCircuitCount == 0) {
					return null;
				} else {
					return plannedProduct.Product.PlannedCircuitCount;
				}
			}
			set {
				if (plannedProduct.Product is EurovalProduct) {
					(plannedProduct.Product as EurovalProduct).RequestedCircuits = value;
					plannedProduct.ConfigureProduct(false);
				} else if (plannedProduct.Product is EcothermProduct) {
					(plannedProduct.Product as EcothermProduct).RequestedCircuits = value;
					plannedProduct.ConfigureProduct(false);
				} else if (plannedProduct.Product is ModulKlimaBodenProduct) {
					(plannedProduct.Product as ModulKlimaBodenProduct).RequestedCircuits = value;
					plannedProduct.ConfigureProduct(false);
				}
			}
		}

		public bool NrOfCircuitsEditable {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					return !(plannedProduct.Product as EurovalProduct).PlannedProductIsConnection;
				} else if (plannedProduct.Product is EcothermProduct) {
					return !(plannedProduct.Product as EcothermProduct).PlannedProductIsConnection;
				} else if (plannedProduct.Product is ModulKlimaBodenProduct) {
					return true;
				}

				return false;
			}
		}

		public bool NrOfCircuitsModified {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					return (plannedProduct.Product as EurovalProduct).RequestedCircuits != null;
				} else if (plannedProduct.Product is EcothermProduct) {
					return (plannedProduct.Product as EcothermProduct).RequestedCircuits != null;
				} else if (plannedProduct.Product is ModulKlimaBodenProduct) {
					return (plannedProduct.Product as ModulKlimaBodenProduct).RequestedCircuits != null;
				}
				return false;
			}
		}

		public Nullable<double> PipeLength {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					if ((plannedProduct.Product as EurovalProduct).PlannedPipeLengthPerCircuit > 0) {
						return (plannedProduct.Product as EurovalProduct).PlannedPipeLengthPerCircuit;
					} 
				} else if (plannedProduct.Product is EcothermProduct) {
					if ((plannedProduct.Product as EcothermProduct).PlannedPipeLengthPerCircuit > 0) {
						return (plannedProduct.Product as EcothermProduct).PlannedPipeLengthPerCircuit;
					}
				}
				return null;
			}
		}

		public Nullable<double> TotalArea {
			get { return plannedProduct.PlannedArea; }
		}

		public Nullable<double> DruckverlustHeat {
			get {
				if (plannedProduct.Product.PlannedMhHeat > 0) {
					return plannedProduct.Product.PlannedMhHeat; 
				} else {
					return null;
				}
			}
		}

		public double HeatNetLoad {
			get { return plannedProduct.RequestedHeatLoad; }
		}

		public double HeatPower {
			get { return plannedProduct.PlannedHeatLoad; }
		}

		public Nullable<double> HeatRest {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					if ((plannedProduct.Product as EurovalProduct).PlannedProductIsConnection) {
						return null;
					}
				} else if (plannedProduct.Product is EcothermProduct) {
					if ((plannedProduct.Product as EcothermProduct).PlannedProductIsConnection) {
						return null;
					}
				} 
				return -1 * (HeatNetLoad - HeatPower); 
			}
		}

		public Nullable<double> DruckverlustCool {
			get {
				if (plannedProduct.Product.PlannedMhCool > 0) {
					return plannedProduct.Product.PlannedMhCool;
				} else {
					return null;
				}
			}
		}

		public double CoolNetLoad {
			get { return plannedProduct.RequestedCoolLoad; }
		}
		
		public double CoolPower {
			get { return plannedProduct.PlannedCoolLoad; }
		}

		public Nullable<double> CoolRest {
			get {
				if (plannedProduct.Product is EurovalProduct) {
					if ((plannedProduct.Product as EurovalProduct).PlannedProductIsConnection) {
						return null;
					}
				} else if (plannedProduct.Product is EcothermProduct) {
					if ((plannedProduct.Product as EcothermProduct).PlannedProductIsConnection) {
						return null;
					}
				} 
				return -1 * (CoolNetLoad - CoolPower); 
			}
		}

		public bool Ok {
			get { return plannedProduct.Product.LastErrorMessage == null; }
		}

	}

}
