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
	}

}
