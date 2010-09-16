using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Europlan.Common {

	[Serializable()]
	public class ImagePlan : Plan {

		private float angle = 0;
		private float xPos = 0;
		private float yPos = 0;
		private Nullable<float> scale = null;

		public float Angle {
			get { return angle; }
			set { angle = value; }
		}

		public float XPos {
			get { return xPos; }
			set { xPos = value; }
		}

		public float YPos {
			get { return yPos; }
			set { yPos = value; }
		}

		public Nullable<float> Scale {
			get { return scale; }
			set { scale = value; }
		}

	}

}
