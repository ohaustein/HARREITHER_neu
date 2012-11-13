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
		private Circuit.CircuitConnectionTypeEnum circuitConnectionType = Circuit.CircuitConnectionTypeEnum.VORLAUF;
		private bool userDefined = false;

		public Circuit.CircuitConnectionTypeEnum CircuitConnectionType {
			get { return this.circuitConnectionType; }
			set { this.circuitConnectionType = value; }
		}


		public ProductConnection() {
		}

		public ProductConnection(Distributor distributor) {
			this.distributor = distributor;
		}

		public ProductConnection(PlannedProduct otherProduct, Circuit.CircuitConnectionTypeEnum circuitConnectionType) {
			this.otherProduct = otherProduct;
			this.circuitConnectionType = circuitConnectionType;
		}

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
		public Distributor DirectOrIndirectDistributor {
			get {
				if (this.distributor != null) {
					return this.distributor;
				}
				if (this.otherProduct != null && this.otherProduct.Product != null && this.otherProduct.Product.PlannedConnection != null) {
					return this.otherProduct.Product.PlannedConnection.DirectOrIndirectDistributor;
				}
				return null;
			}
		}

		[XmlIgnore]
		public object Connection {
			get { return this.distributor != null ? (object)this.distributor : (object)this.otherProduct; }
		}
		#endregion Anbindung

		public override string ToString() {
			if (this.Distributor != null) {
				return this.Distributor.Id + ": " + this.Distributor.Name;
			}
			if (this.OtherProduct != null) {
				string connTo = EuroplanRes.ProductConnection_AnschlussAn;
				connTo = connTo.Replace("%SYSTEM%", this.OtherProduct.System);
				connTo = connTo.Replace("%RAUMID%", this.OtherProduct.Product.AssociatedRoom.Id);
				connTo = connTo.Replace("%RAUMNAME%", this.OtherProduct.Product.AssociatedRoom.Name);
				return connTo;
			}
			return "";
		}

		public bool UserDefined {
			get { return this.userDefined; }
			set { this.userDefined = value; }
		}

	}
}
