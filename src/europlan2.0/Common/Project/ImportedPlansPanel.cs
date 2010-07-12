using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Europlan.Common {
	public partial class ImportedPlansPanel : UserControl, IEditorUserControl, ISaveRequest {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;
		public event ProjectSaveRequestHandler ProjectSaveRequest;

		public ImportedPlansPanel() {
			InitializeComponent();

			this.SetLanguage();
		}

		private void SetLanguage() {
			this.lblImportedPlans.Text = EuroplanRes.ImportedPlansPanel_ImportiertePlaene; //"Importierte Pläne";
			this.btnImport.Text = EuroplanRes.ImportedPlansPanel_PlanImportieren; //"Plan importieren";
			this.btnDelete.Text = EuroplanRes.ImportedPlansPanel_PlanEntfernen; //"Plan entfernen";
		}

		public void UpdateControl(bool resetUserInterface) {
			planSource.DataSource = Project.Instance.ImportedPlans;
			planSource.ResetBindings(false);
		}

		public bool AllowLeave() {
			return true;
		}

		private void btnImport_Click(object sender, EventArgs e) {
			if (ProjectSaveRequest != null) {
				ProjectSaveRequest(this);
			}
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.DefaultExt = "dxf";
			dialog.Filter = EuroplanRes.ImportedPlansPanel_DxfFilter + "|*.dxf";
			dialog.Filter +=  "|" + EuroplanRes.ImportedPlansPanel_ImageFilter + "|*.jpg;*.png;*.bmp";
			dialog.Multiselect = false;
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				List<Plan> plans = Project.Instance.ImportedPlans;
				foreach (Plan plan in plans) {
					if (Path.GetFileName(plan.RelativeFileName).Equals(Path.GetFileName(dialog.FileName))) {
						result = MessageBox.Show("Gibts schon...");
						return;
					}
				}
				NewPlanForm newPlanForm = new NewPlanForm();
				result = newPlanForm.ShowDialog();
				if (result == DialogResult.OK) {
					string dir = Path.GetDirectoryName(Project.Instance.ProjectFileName);
					string subDir = Path.GetFileNameWithoutExtension(Project.Instance.ProjectFileName) + "_plans";
					dir = Path.Combine(dir, subDir);
					if (!Directory.Exists(dir)) {
						Directory.CreateDirectory(dir);
					}
					string newFileName = Path.Combine(dir, Path.GetFileName(dialog.FileName));
					File.Copy(dialog.FileName, newFileName, true);				
					Plan plan = new Plan();
					plan.Name = newPlanForm.PlanName;
					plan.RelativeFileName = Path.Combine(subDir, Path.GetFileName(dialog.FileName));
					plans.Add(plan);
					if (ProjectChanged != null) {
						ProjectChanged(this);
					}
					UpdateControl(false);
				}
				newPlanForm.Dispose();
			}
			dialog.Dispose();
		}

		private void btnDelete_Click(object sender, EventArgs e) {

		}

		private void dgvPlans_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(this);
			}
		}


	}
}
