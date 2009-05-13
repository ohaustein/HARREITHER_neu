using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public enum ConstructionScopeEnum {
		UnknownConstruction = 0,
		FloorConstruction = 1,
		InsulationConstruction = 2,
		WallConstruction = 4,
		CeilingConstruction = 8,
		All = 15
	}
}
