using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public enum MaterialTypeEnum {
		UnknownMaterial = 0,
		Euroval = 1,
		Modul = 2,
		Hitherm = 4,
		Distributor = 8,	// Verteiler
		Insulation = 16,		// Dämmung
		General = 32,
		All = 63
	}
}
