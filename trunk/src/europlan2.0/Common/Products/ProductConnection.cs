using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class ProductConnection {
		public enum ConnectionTypeEnum {
			DISTRIBUTOR,
			OTHER_PRODUCT,
			NONE
		}

		private Distributor distributor = null;
		private PlannedProduct otherProduct = null;
		private string distributorId = null;
		private string otherProductId = null;

		//private Distributor ruecklaufDistributor = null;
		//private PlannedProduct product = null;
		//private string ruecklaufDistributorId = null;
		//private string productId = null;

		public ProductConnection() {
		}

		public ProductConnection(Distributor distributor/*, Distributor ruecklaufDistributor*/) {
			this.distributor = distributor;
			//this.ruecklaufDistributor = ruecklaufDistributor;
		}

		/*public ProductConnection(Distributor vorlaufDistributor, PlannedProduct ruecklaufOtherProduct) {
			this.distributor = vorlaufDistributor;
			this.product = ruecklaufOtherProduct;
		}*/

		public ProductConnection(PlannedProduct otherProduct/*, Distributor ruecklaufDistributor*/) {
			this.otherProduct = otherProduct;
			//this.ruecklaufDistributor = ruecklaufDistributor;
		}

		/*public ProductConnection(PlannedProduct vorlaufOtherProduct, PlannedProduct ruecklaufOtherProduct) {
			this.otherProduct = vorlaufOtherProduct;
			this.product = ruecklaufOtherProduct;
		}*/

		#region Anbindung
		[XmlIgnore]
		public ConnectionTypeEnum ConnectionType {
			get { return (this.distributor != null || this.distributorId != null) ? ConnectionTypeEnum.DISTRIBUTOR : (this.otherProduct != null || this.otherProductId != null ? ConnectionTypeEnum.OTHER_PRODUCT : ConnectionTypeEnum.NONE); }
		}

		[XmlIgnore]
		public Distributor Distributor {
			get {
				if (this.distributorId != null) {
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Distributor d in f.Distributors) {
							if (d.Id == this.distributorId) {
								this.distributor = d;
							}
						}
					}
					this.distributorId = null;
				}
				return this.distributor;
			}
			set {
				this.distributor = value;
				this.distributorId = null;
				this.otherProduct = null;
				this.otherProductId = null;
			}
		}

		[XmlIgnore]
		public PlannedProduct OtherProduct {
			get {
				if (this.otherProductId != null) {
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct pp in r.PlannedProducts) {
								if (pp.Id == this.otherProductId) {
									this.otherProduct = pp;
								}
							}
						}
					}
					this.otherProductId = null;
				}
				return this.otherProduct;
			}
			set {
				this.otherProduct = value;
				this.otherProductId = null;
				this.distributor = null;
				this.distributorId = null;
			}
		}


		public string DistributorId {
			get { return this.Distributor == null ? null : this.Distributor.Id; }
			set {
				this.distributorId = value;
				this.otherProduct = null;
				this.otherProductId = null;
			}
		}

		public string OtherProductId {
			get { return this.OtherProduct == null ? null : this.OtherProduct.Id; }
			set {
				this.otherProductId = value;
				this.distributor = null;
				this.distributorId = null;
			}
		}

		[XmlIgnore]
		public object Connection {
			get { return this.distributor != null ? (object)this.distributor : (object)this.otherProduct; }
		}
		#endregion Anbindung

		#region Produkt
		/*[XmlIgnore]
		public PlannedProduct Product {
			get {
				if (this.productId != null) {
					foreach (Floor f in Project.Instance.Floors) {
						foreach (Room r in f.Rooms) {
							foreach (PlannedProduct pp in r.PlannedProducts) {
								if (pp.Id == this.productId) {
									this.product = pp;
								}
							}
						}
					}
					this.productId = null;
				}
				return this.product;
			}
			set {
				this.product = value;
				this.productId = null;
			}
		}

		public string ProductId {
			get { return this.Product == null ? null : this.Product.Id; }
			set { this.productId = value; }
		}*/
		#endregion Produkt

		public override string ToString() {
			if (this.Distributor != null) {
				return this.Distributor.Id + ": " + this.Distributor.Name;
			}
			if (this.OtherProduct != null) {
				return "Anschluﬂ an " + this.OtherProduct.System + " in <TODO: Add Floor>";
			}
			return "";
		}

	}
}
