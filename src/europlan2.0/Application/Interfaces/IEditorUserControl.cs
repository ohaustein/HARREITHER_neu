using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {

	public delegate void ProjectStructureChangedHandler(object sender);
	public delegate void ProjectChangedHandler(object sender);
	public delegate void TreeSelectionRequestedHandler(object sender, object requestedItem);
	
	interface IEditorUserControl {

		event ProjectStructureChangedHandler ProjectStructureChanged;
		event ProjectChangedHandler ProjectChanged;
		event TreeSelectionRequestedHandler TreeSelectionRequested;

		void UpdateControl();
		bool AllowLeave();
	}
}
