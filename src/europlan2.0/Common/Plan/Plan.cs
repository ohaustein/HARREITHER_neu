using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Europlan.Common;

namespace Europlan.Common {

	[Serializable()]
	[XmlInclude(typeof(ImagePlan))]
	[XmlInclude(typeof(CadPlan))]
	public abstract class Plan {

		private string name;
		private string relativeFileName;
		private Nullable<float> scale = null;
		
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

		public Nullable<float> Scale {
			get { return scale; }
			set { scale = value; }
		}

	}

}
