using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class InsulationConstruction : Construction {
		private bool peFoil;

		public InsulationConstruction() : base() {
			this.peFoil = false;
		}

		public InsulationConstruction(string id, string name, ConstructionType type, bool peFoil) : base(id, name, type) {
			this.peFoil = peFoil;
		}

		public bool PeFoil {
			get { return peFoil; }
			set { peFoil = value; }
		}
	}
}
