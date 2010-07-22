using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Europlan.Common {
	
	public class Plan {

		private string name;
		private string relativeFileName;
		private double scale = 1.0;
		
		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string RelativeFileName {
			get { return relativeFileName; }
			set { relativeFileName = value; }
		}

		[XmlIgnore]
		public string AbsoluteFileName {
			get {
				string projectDir = Path.GetDirectoryName(Project.Instance.ProjectFileName);
				return Path.Combine(projectDir, RelativeFileName);
			}
		}

		public double Scale {
			get { return scale; }
			set { scale = value; }
		}

	}

}
