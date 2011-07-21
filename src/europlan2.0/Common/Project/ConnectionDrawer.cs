using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using WW.Math;
using WW.Math.Geometry;
using System.Xml.Serialization;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	public partial class ConnectionDrawer : Component {

		public ConnectionDrawer() {
			InitializeComponent();
		}

		public ConnectionDrawer(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private List<Product> productsInFloor = new List<Product>();
		//private Dictionary<Distributor, DistributorPositioner> distributorsInFloor = new Dictionary<Distributor, DistributorPositioner>();
		private Product product = null;
		private Floor floor = null;
		private bool planFloor = true;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Product Product {
			get { return this.product; }
			set {
				this.product = value;
				this.Floor = (this.product != null && this.product.AssociatedRoom != null) ? this.product.AssociatedRoom.AssociatedFloor : null;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private Floor Floor {
			get { return this.floor; }
			set {
				this.floor = value;
				//this.distributorsInFloor.Clear();
				if (this.floor == null) {
					this.Plan = null;
				} else {
					this.Plan = floor.AssociatedPlan;
					/*DistributorPositioner positioner;
					foreach (Distributor d in this.GetAllDistributors()) {
						positioner = new DistributorPositioner();
						positioner.Floor = floor;
						positioner.Distributor = d;
						this.distributorsInFloor.Add(d, positioner);
					}*/
				}
			}
		}

		public bool PlanFloor {
			get { return this.planFloor; }
			set {
				if (this.planFloor != value) {
					this.planFloor = value;
					// quick workaround to get affected products
					//this.Plan = this.Plan;
				}
			}
		}

		public bool PlanCeiling {
			get { return !this.planFloor; }
			set {
				if (this.planFloor == value) {
					this.planFloor = !value;
					// quick workaround to get affected products
					//this.Plan = this.Plan;
				}
			}
		}

		private Plan tmpPlan = null;
		private Plan Plan {
			get { return this.tmpPlan; }
			set {
				this.tmpPlan = value;
				this.productsInFloor.Clear();
				if (this.Plan != null) {
					ResetProducts();
				}
			}
		}

		private void ResetProducts() {
			this.productsInFloor = this.GetAllProducts();
			/*this.productsInFloor.Clear();
			foreach (Product p in this.GetAllProducts()) {
				// TODO add other products
				//if (this.planFloor) {
					if (p is ModulKlimaBodenProduct) {
						productsInFloor.Add(p);
					} else if (p is EurovalProduct) {
						productsInFloor.Add(p);
					} else if (p is EcothermProduct) {
						productsInFloor.Add(p);
					} else if (p is HithermProduct) {
						productsInFloor.Add(p);
					} else
				//} else {
					if (p is ModulKlimaDeckeProduct) {
						productsInFloor.Add(p);
					}
				//}
			}*/
		}

		private List<Floor> GetAllFloors() {
			List<Floor> floors = new List<Floor>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.AssociatedPlanId == this.Plan.Id) {
						floors.Add(floor);
					}
				}
			}
			return floors;
		}

		private List<Room> GetAllRooms() {
			List<Room> rooms = new List<Room>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.AssociatedPlanId == this.Plan.Id) {
						foreach (Room room in floor.Rooms) {
							rooms.Add(room);
						}
					}
				}
			}
			return rooms;
		}

		private List<Product> GetAllProducts() {
			List<Product> products = new List<Product>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.AssociatedPlanId == this.Plan.Id) {
						foreach (Room room in floor.Rooms) {
							foreach (PlannedProduct product in room.PlannedProducts) {
								//if ((this.PlanFloor && product.Product.Type == Product.ProductType.FBH) ||
									//(this.PlanCeiling && product.Product.Type == Product.ProductType.DH)) {
									products.Add(product.Product);
								//}
							}
						}
					}
				}
			}
			return products;
		}

		/*private List<Distributor> GetAllDistributors() {
			List<Distributor> distributors = new List<Distributor>();
			if (this.Plan != null) {
				foreach (Floor floor in Project.Instance.Floors) {
					distributors.AddRange(floor.GetAllAvailableDistributors());
				}
			}
			return distributors;
		}*/

		public void Paint(Graphics g, Matrix4D additionalTransformation) {
			if (this.Plan != null) {
				Region clip = g.Clip;
				/*foreach (KeyValuePair<Distributor, DistributorPositioner> kvp in this.distributorsInFloor) {
					kvp.Value.PaintAfterPlanPannel(g, additionalTransformation, new Point2D(), new Point());
				}*/
				g.Clip = clip;
				foreach (Product product in this.productsInFloor) {
					foreach (GraphicalProductConnection connection in product.Connections) {
						if ((connection.ConnectionType == Product.ProductType.FBH && this.PlanFloor) || (connection.ConnectionType == Product.ProductType.DH && this.PlanCeiling)) {
							connection.Draw(g, additionalTransformation, this.Plan.Measure.Value, false, this.product != product);
						}
					}
				}
			}
		}
	}
}
