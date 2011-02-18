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

		public string Id {
			get { return material.Id; }
		}

		public string Name {
			get { return material.LocalizedName; }
		}

		public double CalculatedAmount {
			get {
				if (Project.Instance.RequiredMaterialCalculated.ContainsKey(material.Id)) {
					if (Project.Instance.RequiredMaterialCalculated[material.Id] >= 0) {
						return Math.Ceiling(Project.Instance.RequiredMaterialCalculated[material.Id]);
					} else if (Project.Instance.RequiredMaterialCalculated[material.Id] == Double.NegativeInfinity) {
						return 0;
					} else {
						return Math.Ceiling(Math.Abs(Project.Instance.RequiredMaterialCalculated[material.Id]));
					}
				}
				return 0; 
			}
		}

		public bool CanBeCalculated {
			get {
				if (Project.Instance.RequiredMaterialCalculated.ContainsKey(material.Id)) {
					if (Project.Instance.RequiredMaterialCalculated[material.Id] < 0) {
						return false;
					}
				}
				return true;
			}
		}

		public Nullable<double> RequiredAmount {
			get {
				if (Project.Instance != null) {
					if (Project.Instance.RequiredMaterialOverrides.ContainsKey(material.Id)) {
						return CalculatedAmount + Project.Instance.RequiredMaterialOverrides[material.Id];
					} else {
						return CalculatedAmount;
					}
				}
				return null; 
			}
			set {
				if (Project.Instance != null) {
					if (Project.Instance.RequiredMaterialOverrides.ContainsKey(material.Id)) {
						if (value != null) {
							Project.Instance.RequiredMaterialOverrides[material.Id] = (double)value - CalculatedAmount;
						} else {
							Project.Instance.RequiredMaterialOverrides.Remove(material.Id);
						}
					} else {
						if (value != null) {
							Project.Instance.RequiredMaterialOverrides.Add(material.Id, (double)value - CalculatedAmount);
						}
					}
				}

			}
		}

		public Nullable<double> RecommendedAmount {
			get {
				if (RequiredAmount.HasValue && RequiredAmount.Value > 0) {
					if (material.Denomination.HasValue) {
						return Math.Ceiling(RequiredAmount.Value / material.Denomination.Value) * material.Denomination.Value;
					} else {
						return RequiredAmount;
					}
				} else {
					return null;
				}
			}
		}

		public double Price {
			get {
				if (RequiredAmount.HasValue && RequiredAmount.Value > 0 && material.Denomination.HasValue && material.Denomination.Value > 0) {
					return material.PricePerUnit * RequiredAmount.Value;
				} else {
					return 0.0;
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

		public CategoryType CategoryType {
			get {
				return material.Category.Type;
			}
		}

		public string CategoryTypeName {
			get {
				return new CategoryTypeEnumConverter().ConvertToString(material.Category.Type);
			}
		}

	}

}
