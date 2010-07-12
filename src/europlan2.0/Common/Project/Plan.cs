using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	
	public class Plan {

		private string name;
		private string relativeFileName;

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string RelativeFileName {
			get { return relativeFileName; }
			set { relativeFileName = value; }
		}

	}

}
