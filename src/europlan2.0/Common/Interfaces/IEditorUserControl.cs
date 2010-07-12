using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public delegate void ProjectStructureChangedHandler(object sender);
	public delegate void ProjectChangedHandler(object sender);
	public delegate void ProjectSaveRequestHandler(object sender);
	public delegate void TreeSelectionRequestedHandler(object sender, object requestedItem);
	
	public interface IEditorUserControl {

		event ProjectStructureChangedHandler ProjectStructureChanged;
		event ProjectChangedHandler ProjectChanged;
		event TreeSelectionRequestedHandler TreeSelectionRequested;

		void UpdateControl(bool resetUserInterface);
		bool AllowLeave();
	}

	public interface ISaveRequest {
		event ProjectSaveRequestHandler ProjectSaveRequest;
	}
}
