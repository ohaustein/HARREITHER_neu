using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public delegate void ProjectStructureChangedHandler(object sender);
	public delegate void ProjectChangedHandler(object sender);
	/// <summary>
	/// Requests the project to be saved
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="firstSave">only make sure the project has been saved once. Don't prompt fror save if there are unsaved changes.</param>
	/// <param name="saved">returns weather the project has been saved</param>
	public delegate void ProjectSaveRequestHandler(object sender, bool firstSave, out bool saved);
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
