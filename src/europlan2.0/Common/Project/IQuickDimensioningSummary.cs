using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public interface IQuickDimensioningSummary {
		string Name {
			get;
		}

		float Area {
			get;
		}

		int HeatLoad {
			get;
		}

		int CoolLoad {
			get;
		}

		float EurovalArea {
			get;
		}

		int EurovalCircuits {
			get;
		}

		float ConcreteActivationArea {
			get;
		}

		int ConcreteActivationCircuits {
			get;
		}

		float HithermArea {
			get;
		}

		int HithermCircuits {
			get;
		}

		float HithermCompactArea {
			get;
		}

		int HithermCompactCircuits {
			get;
		}

		float HithermCompactRoofArea {
			get;
		}

		int HithermCompactRoofCircuits {
			get;
		}

		float ModulKlimaBodenArea {
			get;
		}

		int ModulKlimaBodenCircuits {
			get;
		}

		float ModulKlimaDeckeArea {
			get;
		}

		int ModulKlimaDeckeCircuits {
			get;
		}

		int NrOfServos {
			get;
		}

		string RoomControllers {
			get;
		}
	}
}
