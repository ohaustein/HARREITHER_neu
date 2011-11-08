using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Europlan.Common {

	[Serializable()]
	public class TempImagePlan : ImagePlan {
		
		private string absoluteFileName;

		public override string AbsoluteFileName {
			get { return this.absoluteFileName; }
		}

		public void SetAbsoluteFilename(string filename) {
			this.absoluteFileName = filename;
		}

		public WW.Math.Point2D GetImageSize() {
			if (!string.IsNullOrEmpty(this.AbsoluteFileName)) {
				try {
					Image image = Image.FromFile(this.AbsoluteFileName);
					int width = image.Width;
					int height = image.Height;
					image.Dispose();
					return new WW.Math.Point2D(width, height);
				} catch {
					return new WW.Math.Point2D(0, 0);
				}
			} else {
				return new WW.Math.Point2D(0, 0);
			}
		}
	}

}
