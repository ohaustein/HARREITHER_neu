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
			this.lblImportedPlans.Text = EuroplanRes.ImportedPlansPanel_ImportiertePlaene; //"Planverwaltung"
			this.btnImport.Text = EuroplanRes.ImportedPlansPanel_PlanImportieren; //"Plan importieren"
			this.btnDelete.Text = EuroplanRes.ImportedPlansPanel_PlanEntfernen; //"Plan entfernen"
			this.btnExport.Text = EuroplanRes.ImportedPlansPanel_PlanExportieren; //"Plan exportieren"
			this.nameDataGridViewTextBoxColumn.Name = EuroplanRes.ImportedPlansPanel_PlanName; //"Name"
			this.RelativeFileName.Name = EuroplanRes.ImportedPlansPanel_DateiPfad; //"Pfad"
			this.colOptions.Name = EuroplanRes.ImportedPlansPanel_Optionen; //"Optionen"
		}

		public void UpdateControl(bool resetUserInterface) {
			btnDelete.Enabled = Project.Instance.ImportedPlans.Count > 0;
			btnExport.Enabled = Project.Instance.ImportedPlans.Count > 0;
			planSource.DataSource = Project.Instance.ImportedPlans;
			planSource.ResetBindings(false);
		}

		public bool AllowLeave() {
			return true;
		}

		private void btnImport_Click(object sender, EventArgs e) {
			bool saved = true;
			if (ProjectSaveRequest != null) {
				ProjectSaveRequest(this, true, out saved);
			}
			if (saved) {
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.CheckFileExists = true;
				dialog.CheckPathExists = true;
				dialog.DefaultExt = "dxf";
				dialog.Filter = "alle Pläne|*.dxf;*.dwg;*.jpg;*.png;*.bmp";
				dialog.Filter += "|" + EuroplanRes.ImportedPlansPanel_DxfFilter + "|*.dxf;*.dwg";
				dialog.Filter += "|" + EuroplanRes.ImportedPlansPanel_ImageFilter + "|*.jpg;*.png;*.bmp";
				dialog.Multiselect = false;
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					List<Plan> plans = Project.Instance.ImportedPlans;
					foreach (Plan plan in plans) {
						if (Path.GetFileName(plan.RelativeFileName).Equals(Path.GetFileName(dialog.FileName))) {
							result = MessageBox.Show(EuroplanRes.ImportedPlansPanel_PlanSchonVorhanden);
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
						if (!dialog.FileName.Equals(newFileName)) {
							File.Copy(dialog.FileName, newFileName, true);
						}
						string extension = Path.GetExtension(dialog.FileName);
						Plan plan = null;
						if (isImage(extension)) {
							plan = new ImagePlan();
						} else if (isCad(extension)) {
							plan = new CadPlan();
						}
						// TODO assure that plan is not null
						plan.Name = newPlanForm.PlanName;
						plan.RelativeFileName = Path.Combine(subDir, Path.GetFileName(dialog.FileName));
						plans.Add(plan);
						if (ProjectChanged != null) {
							ProjectChanged(this);
						}
						UpdateControl(false);
						openPlanOptions(plan);
					}
					newPlanForm.Dispose();
				}
				dialog.Dispose();
			}
		}

		private bool isImage(string extension) {

			return string.Compare(".jpg", extension, true) == 0 ||
				string.Compare(".bmp", extension, true) == 0 ||
				string.Compare(".png", extension, true) == 0;
		}
		
		private bool isCad(string extension) {
			return string.Compare(".dxf", extension, true) == 0 ||
				string.Compare(".dwg", extension, true) == 0;
		}

		private void btnDelete_Click(object sender, EventArgs e) {
			// TODO - check if plan is alerady used
			// if (alreadyused) { ....
			DialogResult result = MessageBox.Show(EuroplanRes.ImportedPlansPanel_WirklichLoeschenMessage, EuroplanRes.ImportedPlansPanel_WirklichLoeschenTitle, MessageBoxButtons.YesNo);
			if (result.Equals(DialogResult.Yes)) {
				if (dgvPlans.SelectedRows[0] != null) {
					Plan plan = dgvPlans.SelectedRows[0].DataBoundItem as Plan;
					deletePlan(plan);
				}
			}
		}

		private void btnExport_Click(object sender, EventArgs e) {
			if (dgvPlans.SelectedRows[0] != null) {
				Plan plan = dgvPlans.SelectedRows[0].DataBoundItem as Plan;
				ExportPlanForm form = new ExportPlanForm(plan);
				form.ShowDialog();
				form.Dispose();
			}
		}

		private void dgvPlans_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (ProjectChanged != null) {
				ProjectChanged(this);
			}
		}

		private void dgvPlans_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvPlans.Columns.Count &&
				this.dgvPlans.Columns[e.ColumnIndex] == this.colOptions &&
				e.RowIndex >= 0 && e.RowIndex < this.dgvPlans.Rows.Count) {
				Plan plan = this.dgvPlans.Rows[e.RowIndex].DataBoundItem as Plan;
				if (plan != null) {
					openPlanOptions(plan);
				}
			}
		}

		private void openPlanOptions(Plan plan) {
			DialogResult result = DialogResult.OK;
			if (plan != null) {

				if (plan is ImagePlan) {
					ImagePlanOptionsForm ipoForm = new ImagePlanOptionsForm(plan as ImagePlan);
					result = ipoForm.ShowDialog();
					if (ipoForm.UnsavedChanges) {
						if (ProjectChanged != null) {
							ProjectChanged(this);
						}
					}
					ipoForm.Dispose();
				} else if (plan is CadPlan) {
					CadPlanOptionsForm cpoForm = null;
					try {
						cpoForm = new CadPlanOptionsForm(plan as CadPlan);
						result = cpoForm.ShowDialog();
						if (cpoForm.UnsavedChanges) {
							if (ProjectChanged != null) {
								ProjectChanged(this);
							}
						}
					} catch (Exception) {
						MessageBox.Show("Beim Einlesen des Plans ist leider ein Fehler aufgetreten. Bitte versuchen Sie, falls möglich, den Plan mit einem CAD-Programm erneut abzuspeichern und nochmals zu importieren.", "Fehler beim Einlesen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						deletePlan(plan);
					} finally {
						if (cpoForm != null) {
							cpoForm.Dispose();
						}
					}
				}
				if (result == DialogResult.Cancel) {
					deletePlan(plan);
				}
			}
		}

		private void deletePlan(Plan plan) {
			if (plan != null) {
				string fileName = plan.AbsoluteFileName;
				if (File.Exists(fileName)) {
					File.Delete(fileName);
				}
				Project.Instance.ImportedPlans.Remove(plan);
				if (ProjectChanged != null) {
					ProjectChanged(this);
				}
				UpdateControl(false);
			}
		}

	}
}
