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
			get {
				if (Project.Instance.RequiredMaterialCalculated.ContainsKey(material.Id)) {
					return Project.Instance.RequiredMaterialCalculated[material.Id];
				}
				return 0; 
			}
		}

		public Nullable<double> RequiredAmount {
			get {
				if (Project.Instance != null) {
					if (Project.Instance.RequiredMaterialOverrides.ContainsKey(material.Id)) {
						return Project.Instance.RequiredMaterialOverrides[material.Id];
					}
				}
				return null; 
			}
			set {
				if (Project.Instance != null) {
					if (Project.Instance.RequiredMaterialOverrides.ContainsKey(material.Id)) {
						if (value != null) {
							Project.Instance.RequiredMaterialOverrides[material.Id] = (double)value;
						} else {
							Project.Instance.RequiredMaterialOverrides.Remove(material.Id);
						}
					} else {
						if (value != null) {
							Project.Instance.RequiredMaterialOverrides.Add(material.Id, (double)value);
						}
					}
				}

			}
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
