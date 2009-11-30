using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Europlan.Common {
	[Serializable()]
	public class PlannedProduct : IGuiRepresentation {

		private Product plannedProduct;
		private Room room;
		private TreeNode productNode = null;

		private bool coverHeatLoad = true;
		private bool coverCoolLoad = true;
		private double requestedHeatLoad;
		private double requestedCoolLoad;
		private bool calculateHeat = false;
		private bool calculateCool = false;
		private string internalName;

		private string id;

		internal PlannedProduct() {
			this.id = Guid.NewGuid().ToString();
			this.plannedProduct = null;
			this.room = null;
		}

		public PlannedProduct(Product plannedProduct) {
			this.id = Guid.NewGuid().ToString();
			this.productNode = new TreeNode();
			this.productNode.Tag = this;
			this.Product = plannedProduct;
		}

		public PlannedProduct(Room room) {
			this.id = Guid.NewGuid().ToString();
			this.plannedProduct = null;
			this.room = room;
		}

		public string Id {
			get { return this.id; }
			set { this.id = value; }
		}

		public string InternalName {
			get { return this.internalName; }
		}

		public override string ToString() {
			return this.productNode.Text;
		}

		[XmlIgnore]
		public Product.ProductType PlannedProductType {
			get {
				if (plannedProduct == null) {
					return Product.ProductType.REST;
				} else {
					return plannedProduct.Type;
				}
			}
		}

		[XmlIgnore]
		public string System {
			get {
				if (plannedProduct == null) {
					return null;
				} else {
					return plannedProduct.Name;
				}
			}
		}

		[XmlIgnore]
		public string Comment {
			get {
				if (plannedProduct == null) {
					return "Restposition";
				} else {
					return plannedProduct.Comment;
				}
			}
			set {
				if (plannedProduct != null) {
					plannedProduct.Comment = value;
				}
			}
		}

		[XmlIgnore]
		public float FloorArea {
			get {
				if (plannedProduct == null) {
					Room room = this.room;
					float area = room.Area;
					foreach (PlannedProduct product in room.PlannedProducts) {
						if (product != this) {
							area -= product.Product.PlannedFloorArea;
						}
					}
					if (area < 0) {
						area = 0;
					}
					return area;
				} else {
					return plannedProduct.PlannedFloorArea;
				}
			}
		}

		[XmlIgnore]
		public Nullable<float> PlannedArea {
			get {
				if (plannedProduct == null) {
					return null;
				} else {
					return plannedProduct.PlannedFloorArea + plannedProduct.PlannedCeilingArea + plannedProduct.PlannedWallArea;
				}
			}
		}

		/*[System.Xml.Serialization.XmlIgnore]
		public Nullable<float> PlannedAreaPercentage {
			get {
				if (plannedProduct == null) {
					return null;
				} else {
					return plannedProduct.PlannedFloorArea * 100 / this.AvailableFloorArea;
				}
			}
			set {
				if (plannedProduct != null) {
					this.plannedProduct.PlannedFloorArea = (float)(this.AvailableFloorArea * value / 100);
				}
			}
		}*/

		[XmlIgnore]
		public double PlannedHeatLoad {
			get {
				if (plannedProduct == null) {
					double heatLoad = this.room.NormalizedHeatLoad;
					foreach (PlannedProduct pp in this.room.PlannedProducts) {
						heatLoad -= pp.PlannedHeatLoad;
					}
					return Math.Round(-heatLoad, 0);
				} else {
					return Math.Round(plannedProduct.PlannedHeatLoadIncludingConnectionsThrough, 0);
				}
			}
		}

		[XmlIgnore]
		public string PlannedHeatLoadString {
			get {
				double plannedHeatLoad = this.PlannedHeatLoad;
				if (plannedProduct == null) {
					return (plannedHeatLoad >= 0 ? "+" : "") + plannedHeatLoad.ToString();
				}
				return plannedHeatLoad.ToString();
			}
		}

		[XmlIgnore]
		public double PlannedCoolLoad {
			get {
				if (plannedProduct == null) {
					double coolLoad = this.room.NormalizedCoolLoad;
					foreach (PlannedProduct pp in this.room.PlannedProducts) {
						coolLoad -= pp.PlannedCoolLoad;
					}
					return Math.Round(-coolLoad, 0);
				} else {
					return Math.Round(plannedProduct.PlannedCoolLoadIncludingConnectionsThrough, 0);
				}
			}
		}

		[XmlIgnore]
		public string PlannedCoolLoadString {
			get {
				double plannedCoolLoad = this.PlannedCoolLoad;
				if (plannedProduct == null) {
					return (plannedCoolLoad >= 0 ? "+" : "") + plannedCoolLoad.ToString();
				}
				return plannedCoolLoad.ToString();
			}
		}

		public bool CalculateHeat {
			get { return this.calculateHeat || !this.calculateCool; }
			set { this.calculateHeat = value; }
		}

		public bool CalculateCool {
			get { return this.calculateCool; }
			set { this.calculateCool = value; }
		}

		public bool CoverHeatLoad {
			get { return this.coverHeatLoad; }
			set { this.coverHeatLoad = value; }
		}

		public bool CoverCoolLoad {
			get { return this.coverCoolLoad; }
			set { this.coverCoolLoad = value; }
		}

		public double RequestedHeatLoad {
			get {
				if (this.coverHeatLoad) {
					return this.NecessaryHeatLoad;
				} else {
					return this.requestedHeatLoad;
				}
			}
			set { this.requestedHeatLoad = Math.Round(value, 1); }
		}

		public double RequestedCoolLoad {
			get {
				if (this.coverCoolLoad) {
					return this.NecessaryCoolLoad;
				} else {
					return this.requestedCoolLoad;
				}
			}
			set { this.requestedCoolLoad = Math.Round(value, 1); }
		}

		[System.Xml.Serialization.XmlIgnore]
		public float RequestedHeatLoadPercentage {
			get {
				if (this.Product.AssociatedRoom.HeatLoad == 0) {
					return 0;
				}
				if (this.coverHeatLoad) {
					return (float)this.NecessaryHeatLoad * 100 / this.Product.AssociatedRoom.NormalizedHeatLoad;
				}
				return (float)Math.Round(this.requestedHeatLoad * 100 / this.Product.AssociatedRoom.NormalizedHeatLoad, 1);
			}
			set {
				if (this.Product.AssociatedRoom.HeatLoad != 0) {
					this.requestedHeatLoad = this.Product.AssociatedRoom.HeatLoad * value / 100;
				}
			}
		}

		[System.Xml.Serialization.XmlIgnore]
		public float RequestedCoolLoadPercentage {
			get {
				if (this.Product.AssociatedRoom.CoolLoad == 0) {
					return 0;
				}
				if (this.coverCoolLoad) {
					return (float)this.NecessaryCoolLoad * 100 / this.Product.AssociatedRoom.NormalizedCoolLoad;
				}
				return (float)(this.requestedCoolLoad * 100 / this.Product.AssociatedRoom.NormalizedCoolLoad);
			}
			set {
				if (this.NecessaryCoolLoad != 0) {
					this.requestedCoolLoad = this.NecessaryCoolLoad * value / 100;
				}
			}
		}

		public double RequestedHeatLoadPerSqM {
			get { return (this.PlannedArea.HasValue ? this.RequestedHeatLoad / this.PlannedArea.Value : 0); }
		}

		public Product Product {
			get { return this.plannedProduct; }
			set {
				this.plannedProduct = value;
				if (this.plannedProduct != null) {
					this.room = null;
					if (this.productNode == null) {
						this.productNode = new TreeNode();
						this.productNode.Tag = this;
					}
					if (this.plannedProduct.AssociatedRoom != null) {
						List<PlannedProduct> products = this.plannedProduct.AssociatedRoom.PlannedProducts;
						Dictionary<Product.ProductType, int> productCounter = new Dictionary<Product.ProductType, int>();
						foreach (PlannedProduct p in products) {
							if (!productCounter.ContainsKey(p.PlannedProductType)) {
								productCounter.Add(p.PlannedProductType, 1);
							} else {
								productCounter[p.PlannedProductType] = productCounter[p.PlannedProductType] + 1;
							}
							internalName = p.PlannedProductType.ToString() + productCounter[p.PlannedProductType];
							p.productNode.Text = internalName + ": " + p.System;
						}
						if (productCounter.ContainsKey(this.PlannedProductType)) {
							internalName = this.PlannedProductType.ToString() + (productCounter[this.PlannedProductType] + 1);
							this.productNode.Text = internalName + ": " + this.System;
						} else {
							internalName = this.PlannedProductType.ToString() + "1";
							this.productNode.Text = internalName + ": " + this.System;
						}
					} else {
						internalName = this.PlannedProductType.ToString();
						this.productNode.Text = internalName + ": " + this.System;
					}
				} else {
					this.productNode = null;
				}
			}
		}

		public TreeNode Node {
			get { return this.productNode; }
		}

		public double NecessaryHeatLoad {
			get {
				if (this.plannedProduct.AssociatedRoom == null) {
					return 0;
				}
				double heatLoad = this.plannedProduct.AssociatedRoom.NormalizedHeatLoad;
				bool selfFound = false;
				foreach (PlannedProduct product in this.plannedProduct.AssociatedRoom.PlannedProducts) {
					if (product != this) {
						//heatLoad -= product.Product.PlannedHeatLoad;
						if (product.CoverHeatLoad) {
							if (!selfFound) {
								heatLoad = 0;
								break;
							}
						} else {
							heatLoad -= product.RequestedHeatLoad;
						}
					} else {
						//break;
						selfFound = true;
					}
				}
				if (heatLoad < 0) {
					heatLoad = 0;
				}
				return heatLoad;
			}
		}

		public double NecessaryCoolLoad {
			get {
				if (this.plannedProduct.AssociatedRoom == null) {
					return 0;
				}
				double coolLoad = this.plannedProduct.AssociatedRoom.NormalizedCoolLoad;
				bool selfFound = false;
				foreach (PlannedProduct product in this.plannedProduct.AssociatedRoom.PlannedProducts) {
					if (product != this) {
						//heatLoad -= product.Product.PlannedHeatLoad;
						if (product.CoverCoolLoad) {
							if (!selfFound) {
								coolLoad = 0;
								break;
							}
						} else {
							coolLoad -= product.RequestedCoolLoad;
						}
					} else {
						//break;
						selfFound = true;
					}
				}
				if (coolLoad < 0) {
					coolLoad = 0;
				}
				return coolLoad;
			}
		}

		/*public double NecessaryCoolLoad {
			get {
				if (this.plannedProduct.AssociatedRoom == null) {
					return 0;
				}
				double coolLoad = this.plannedProduct.AssociatedRoom.NormalizedCoolLoad;
				foreach (PlannedProduct product in this.plannedProduct.AssociatedRoom.PlannedProducts) {
					if (product != this) {
						coolLoad -= product.Product.PlannedCoolLoad;
					}
				}
				if (coolLoad < 0) {
					coolLoad = 0;
				}
				return coolLoad;
			}
		}*/

		public void ConfigureProductDefault() {
			this.requestedCoolLoad = this.NecessaryCoolLoad;
			this.requestedHeatLoad = this.NecessaryHeatLoad;
			this.calculateHeat = this.requestedHeatLoad > 0;
			this.calculateCool = this.requestedCoolLoad > 0;
			string errorMsg;
			this.plannedProduct.ConfigureProduct(this.requestedHeatLoad, this.requestedCoolLoad, this.calculateHeat, this.calculateCool, out errorMsg);
		}

		#region IGuiRepresentation Members

		public Type AssociatedPanelType {
			get {
				if (this.plannedProduct is EurovalProduct) {
					return typeof(PlannedEurovalProductPanel);
				} else if (this.plannedProduct is ModulKlimaDeckeProduct) {
					return typeof(PlannedModulKlimaDeckeProductPanel);
				} else if (this.plannedProduct is ModulKlimaBodenProduct) {
					return typeof(PlannedModulKlimaBodenProductPanel);
				} else if (this.plannedProduct is HithermProduct) {
					return typeof(PlannedHithermProductPanel);
				}
				return null;
			}
		}

		public System.Drawing.Icon AssociatedIcon {
			get { return null; }
		}

		#endregion

		internal void FinalizeLoading() {
			this.plannedProduct.FinalizeLoading(this);
			string errorMsg;

			List<PlannedProduct> products = this.plannedProduct.AssociatedRoom.PlannedProducts;
			Dictionary<Product.ProductType, int> productCounter = new Dictionary<Product.ProductType, int>();
			foreach (PlannedProduct p in products) {
				if (!productCounter.ContainsKey(p.PlannedProductType)) {
					productCounter.Add(p.PlannedProductType, 1);
				} else {
					productCounter[p.PlannedProductType] = productCounter[p.PlannedProductType] + 1;
				}
				internalName = p.PlannedProductType.ToString() + productCounter[p.PlannedProductType];
				p.productNode.Text = internalName + ": " + p.System;
			}

			this.plannedProduct.ConfigureProduct(this.requestedHeatLoad, this.requestedCoolLoad, this.calculateHeat, this.calculateCool, out errorMsg);

		}

		public TreeNode FindNode(object element) {
			if (element == this) {
				return this.Node;
			}
			return null;
		}
	}
}
