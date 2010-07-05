using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Licensing {
	public class ProductNotLicensedException : Exception {
		public Type productType = null;

		public ProductNotLicensedException(Type productType) : base() {
			this.productType = productType;
		}

		public ProductNotLicensedException(Type productType, string message) : base(message) {
			this.productType = productType;
		}

		public Type ProductType {
			get { return this.productType; }
			set { this.productType = value; }
		}
	}
}
