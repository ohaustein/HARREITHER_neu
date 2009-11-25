using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class RequiredMaterialWrapper {

		private Material material;

		public RequiredMaterialWrapper(Material material) {
			this.material = material;
		}

		public string PartNumber {
			get { return material.PartNumber; }
		}

		public string Name {
			get { return material.Name; }
		}

		public double CalculatedAmount {
			get { return 0; }
		}

		public double RequiredAmount {
			get { return 0; }
			set { }
		}

		public string Unit {
			get { return material.Unit; }
		}

		public Category Category {
			get {
				return material.Category;	
			}
		}

	}

}
