using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ProductNameAttribute : Attribute {
		public string name;

		public ProductNameAttribute(string name) {
			this.name = name;
		}

		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}
		
	}
}
