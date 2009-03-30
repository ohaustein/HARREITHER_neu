using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Application {
	public partial class ProjectSummaryPanel : UserControl, IEditorUserControl {
		public ProjectSummaryPanel() {
			InitializeComponent();
			UpdateControl();
		}

		public void UpdateControl() {
			Project project = Project.Instance;
			txtProjectName.Text = project.ProjectName;
			txtContact.Text = project.ProjectContact;
			txtNotes.Text = project.ProjectNotes;
			dateCreated.Value = project.ProjectCreated;
			dateChanged.Value = project.ProjectLastChanged;
			txtEditor.Text = project.ProjectEditor;
		}

		public bool AllowLeave() {
			return true;
		}

	}
}
