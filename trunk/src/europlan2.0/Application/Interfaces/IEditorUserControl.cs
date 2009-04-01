using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {

	public delegate void ProjectStructureChangedHandler(object sender);
	public delegate void ProjectChangedHandler(object sender);
	
	interface IEditorUserControl {

		event ProjectStructureChangedHandler ProjectStructureChanged;
		event ProjectChangedHandler ProjectChanged;

		void UpdateControl();
		bool AllowLeave();
	}
}
