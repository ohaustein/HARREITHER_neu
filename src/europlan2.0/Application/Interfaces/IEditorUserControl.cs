using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {

	public delegate void ProjectStructureChangedHandler(object sender);
	
	interface IEditorUserControl {

		event ProjectStructureChangedHandler ProjectStructureChanged;

		void UpdateControl();
		bool AllowLeave();
	}
}
