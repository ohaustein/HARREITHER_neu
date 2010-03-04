using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ProductNameAttribute : Attribute {
		public string name;
		public string fullName;

		public ProductNameAttribute(string name) {
			this.name = name;
			this.fullName = name;
		}

		public ProductNameAttribute(string name, string fullName) {
			this.name = name;
			this.fullName = fullName;
		}

		public string Name {
			get { return EuroplanRes.ResourceManager.GetString(this.name); }
		}

		public string FullName {
			get { return EuroplanRes.ResourceManager.GetString(this.fullName); }
		}
	}
}
