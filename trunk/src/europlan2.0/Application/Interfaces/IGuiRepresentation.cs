using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Europlan.Application {

	interface IGuiRepresentation {

		Type AssociatedPanelType {
			get;
		}

		Icon AssociatedIcon {
			get;
		}

	}

}
