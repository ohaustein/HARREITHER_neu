using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ConstructionType {
		private string id;
		private string name;
		private ConstructionScopeEnum scope;
		private bool userDefined;

		public ConstructionType() {
			this.name = "";
			this.scope = ConstructionScopeEnum.FloorConstruction;
			this.userDefined = false;
		}

		public ConstructionType(string id, string name, ConstructionScopeEnum scope, bool userDefined) {
			this.id = id;
			this.name = name;
			this.scope = scope;
			this.userDefined = userDefined;
		}

		public String Id {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public ConstructionScopeEnum Scope {
			get { return scope; }
			set { scope = value; }
		}

		public bool UserDefined {
			get { return userDefined; }
			set { userDefined = value; }
		}

	}
}
