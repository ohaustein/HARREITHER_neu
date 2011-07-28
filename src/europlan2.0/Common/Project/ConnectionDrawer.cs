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
		private List<Distributor> distributorsInFloor = new List<Distributor>();
		private Product product = null;
		private Floor floor = null;
		private bool planFloor = true;
		private bool planCeiling = false;

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
				if (this.floor == null) {
					this.distributorsInFloor.Clear();
					this.Plan = null;
				} else {
					this.Plan = this.floor.AssociatedPlan;
					this.distributorsInFloor = this.floor.GetAllAvailableDistributors(true);
				}
			}
		}

		public bool PlanFloor {
			get { return this.planFloor; }
			set { this.planFloor = value; }
		}

		public bool PlanCeiling {
			get { return !this.planFloor; }
			set { this.planCeiling = value; }
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
			this.distributorsInFloor = this.floor != null ? this.floor.GetAllAvailableDistributors(true) : new List<Distributor>();
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
								products.Add(product.Product);
							}
						}
					}
				}
			}
			return products;
		}

		public void Paint(Graphics g, Matrix4D additionalTransformation) {
			if (this.Plan != null && this.floor != null && this.Plan.Measure.HasValue) {
				Region clip = g.Clip;
				foreach (Distributor distributor in this.distributorsInFloor) {
					distributor.Draw(g, additionalTransformation, this.Plan.Measure.Value, this.Plan.InvertYAxis, this.floor);
				}
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
