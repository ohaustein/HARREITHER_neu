using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class CeilingConstruction : Construction {
		public CeilingConstruction() : base() {
		}

		public CeilingConstruction(string id, string name, ConstructionType type)
			: base(id, name, type) {
		}

		public override Construction Clone() {
			CeilingConstruction construction = new CeilingConstruction(this.Id, this.Name, this.Type);
			return construction;
		}
	}
}
