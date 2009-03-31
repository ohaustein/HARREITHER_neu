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
			txtProjectName.Lines = project.ProjectName;
			txtContact.Lines = project.ProjectContact;
			txtNotes.Lines = project.ProjectNotes;
			dateCreated.Value = project.ProjectCreated;
			dateChanged.Value = project.ProjectLastChanged;
			txtEditor.Text = project.ProjectEditor;
		}

		public bool AllowLeave() {
			return true;
		}

		private void txtProjectName_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectName = txtProjectName.Lines;
		}

		private void txtEditor_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectEditor = txtEditor.Text;
		}

		private void txtNotes_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectNotes = txtNotes.Lines;
		}

		private void txtContact_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectContact = txtContact.Lines;
		}

	}
}
