using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {
	interface IEditorUserControl {
		void UpdateControl();
		bool AllowLeave();
	}
}
