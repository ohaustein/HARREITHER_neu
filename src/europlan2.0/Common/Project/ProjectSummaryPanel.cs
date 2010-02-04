using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class ProjectSummaryPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		public ProjectSummaryPanel() {
			InitializeComponent();
			UpdateControl(true);
		}

		public void UpdateControl(bool resetUserInterface) {
			Project project = Project.Instance;
			txtNumber.Text = project.ProjectNumber;
			txtProjectName.Lines = project.ProjectName;
			txtContact.Lines = project.ProjectContact;
			txtNotes.Lines = project.ProjectNotes;
			txtCreated.Text = project.ProjectCreated.ToShortDateString(); ;
			txtChanged.Text = project.ProjectLastChanged.ToShortDateString();
			txtEditor.Text = project.ProjectEditor;
		}

		public bool AllowLeave() {
			return true;
		}

		private void txtProjectName_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectName = txtProjectName.Lines;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void txtEditor_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectEditor = txtEditor.Text;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void txtNotes_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectNotes = txtNotes.Lines;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void txtContact_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectContact = txtContact.Lines;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void txtNumber_TextChanged(object sender, EventArgs e) {
			Project.Instance.ProjectNumber = txtNumber.Text;
			if (ProjectChanged != null) {
				ProjectChanged(null);
			}
		}

		private void btnNext_Click(object sender, EventArgs e) {
			if (TreeSelectionRequested != null) {
				TreeSelectionRequested(this, typeof(FacilityDetailsSummaryPanel));
			}
		}

	}
}
