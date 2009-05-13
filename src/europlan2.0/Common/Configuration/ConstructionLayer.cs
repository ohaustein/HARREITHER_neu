using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ConstructionLayer {
		private string name;
		private float lambdaValue;
		private float thickness;
		private Material layerMaterial;

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

		public Material LayerMaterial {
			get { return layerMaterial; }
			set { layerMaterial = value; }
		}
	}
}
