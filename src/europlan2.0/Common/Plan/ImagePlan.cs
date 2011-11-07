using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

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

		[XmlIgnore]
		public override double Rotation {
			get { return angle; }
		}

		[XmlIgnore]
		public override bool InvertYAxis {
			get { return false; }
		}

		public double GetPlanExportMeasureFactor() {
			if (this.Measure.Value < 200) {
				Image image = Image.FromFile(this.AbsoluteFileName);
				float factor = 200.0f / this.Measure.Value;
				if (image.Width * factor * image.Height * factor > 10000 * 5000) {
					factor = (float)Math.Sqrt(10000.0f * 5000.0f / image.Width / image.Height);
				}
				image.Dispose();
				return factor;
			} else {
				return 1.0;
			}
		}

		public bool IsExportMeasureOk() {
			return this.Measure.Value * this.GetPlanExportMeasureFactor() >= 199;
		}
	}

}
