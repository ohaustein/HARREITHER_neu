using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class FloorConstruction : Construction {
		private float floorThickness;

		public FloorConstruction() : base() {
			this.floorThickness = 0;
		}

		public FloorConstruction(string id, string name, ConstructionType type, float floorThickness) : base(id, name, type) {
			this.floorThickness = floorThickness;
		}

		public float FloorThickness {
			get { return floorThickness; }
			set { floorThickness = value; }
		}
	}
}
