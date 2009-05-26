using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class ConstructionLayer {
		private string name;
		private float lambdaValue;
		private float thickness;
		private string materialId = "";
		private Material layerMaterial = null;

		public override bool Equals(object obj) {
			if (obj is ConstructionLayer) {
				ConstructionLayer layer = obj as ConstructionLayer;
				if ((this.name == layer.name) &&
					(this.lambdaValue == layer.lambdaValue) &&
					(this.thickness == layer.thickness) &&
					(this.materialId == layer.materialId)) {
					return true;
				}
			}
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public float LambdaValue {
			get { return lambdaValue; }
			set { lambdaValue = value; }
		}

		public float Thickness {
			get { return thickness; }
			set { thickness = value; }
		}

		[XmlIgnore]
		public float RValue {
			get {
				if (lambdaValue == 0) {
					return 0;
				} else {
					return thickness / 1000 / lambdaValue;
				}
			}
			set {
				if (value == 0) {
					lambdaValue = 0;
				} else {
					lambdaValue = thickness / 1000 / value;
				}
			}
		}

		public string MaterialId {
			get { return this.materialId; }
			set {
				this.materialId = value;
				foreach (Material material in Configuration.UserTemplate.Materials) {
					if (material.Id == this.materialId) {
						this.layerMaterial = material;
					}
				}
			}
		}

		[XmlIgnore]
		public Material LayerMaterial {
			get {
				if (layerMaterial == null && this.materialId != "") {
					foreach (Material material in Configuration.UserTemplate.Materials) {
						if (material.Id == this.materialId) {
							this.layerMaterial = material;
						}
					}
				}
				return layerMaterial; 
			}
			set { 
				layerMaterial = value;
				this.materialId = layerMaterial.Id;
			}
		}
	}
}
