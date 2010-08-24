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

		private string id = Guid.NewGuid().ToString();
		private string name;
		private string relativeFileName;
		private Nullable<float> measure = null;
		
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

		public Nullable<float> Measure {
			get { return measure; }
			set { measure = value; }
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}
	}

}
