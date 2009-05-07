using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ConstructionLayer {
		private string name;
		private int lambdaValue;
		private int thickness;
		private Material layerMaterial;

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public int LambdaValue {
			get { return lambdaValue; }
			set { lambdaValue = value; }
		}

		public int Thickness {
			get { return thickness; }
			set { thickness = value; }
		}

		public int RValue {
			get { return thickness / lambdaValue; }
			set { lambdaValue = thickness / value; }
		}

		public Material LayerMaterial {
			get { return layerMaterial; }
			set { layerMaterial = value; }
		}
	}
}
