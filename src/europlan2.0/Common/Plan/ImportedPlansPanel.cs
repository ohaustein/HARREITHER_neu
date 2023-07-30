using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using log4net;

namespace Europlan.Common {
	public partial class ImportedPlansPanel : UserControl, IEditorUserControl, ISaveRequest {

#if PDF
		private class PreviewConverterArguments {
			private ProgressForm progressForm;
			private NewPlanForm newPlanForm;
			private string fileName;
			private string dir;
			private string subDir;
			private string extension;
			private string tmpFileName;

			public PreviewConverterArguments(ProgressForm progressForm, NewPlanForm newPlanform, string fileName, /*List<SolidFramework.Pdf.Plumbing.PdfPage> Pages,*/ string dir, string subDir, string extension) {
				this.progressForm = progressForm;
				this.newPlanForm = newPlanform;
				this.fileName = fileName;
				this.dir = dir;
				this.subDir = subDir;
				this.extension = extension;
			}

			public ProgressForm ProgressForm {
				get { return this.progressForm; }
			}

			public NewPlanForm NewPlanForm {
				get { return this.newPlanForm; }
			}

			public string FileName {
				get { return this.fileName; }
			}

			public string Dir {
				get { return this.dir; }
			}

			public string SubDir {
				get { return this.subDir; }
			}

			public string Extension {
				get { return this.extension; }
			}

			public string TmpFileName {
				get { return this.tmpFileName; }
				set { this.tmpFileName = value; }
			}
		}

		private class FinalConverterArguments {
			private ProgressForm progressForm;
			private NewPlanForm newPlanForm;
			private string fileName;
			private string dir;
			private string subDir;
			private string extension;
			private double top, left, bottom, right;
			private int dpi;

			public FinalConverterArguments(ProgressForm progressForm, NewPlanForm newPlanform, string fileName, string dir, string subDir, string extension, double top, double left, double bottom, double right, int dpi) {
				this.progressForm = progressForm;
				this.newPlanForm = newPlanform;
				this.fileName = fileName;
				this.dir = dir;
				this.subDir = subDir;
				this.extension = extension;
				this.top = top;
				this.left = left;
				this.bottom = bottom;
				this.right = right;
				this.dpi = dpi;
			}

			public ProgressForm ProgressForm {
				get { return this.progressForm; }
			}

			public NewPlanForm NewPlanForm {
				get { return this.newPlanForm; }
			}

			public string FileName {
				get { return this.fileName; }
			}

			public string Dir {
				get { return this.dir; }
			}

			public string SubDir {
				get { return this.subDir; }
			}

			public string Extension {
				get { return this.extension; }
			}

			public double Top {
				get { return this.top; }
			}

			public double Left {
				get { return this.left; }
			}

			public double Bottom {
				get { return this.bottom; }
			}

			public double Right {
				get { return this.right; }
			}

			public int Dpi {
				get { return this.dpi; }
			}
		}
#endif

		private static readonly ILog log = LogManager.GetLogger(typeof(ImportedPlansPanel));

        private event ProjectStructureChangedHandler projectStructureChanged;
        public event ProjectStructureChangedHandler ProjectStructureChanged {
            add { this.projectStructureChanged += value; }
            remove { this.projectStructureChanged -= value; }
        }
        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }
        private event TreeSelectionRequestedHandler treeSelectionRequested;
        public event TreeSelectionRequestedHandler TreeSelectionRequested {
            add { this.treeSelectionRequested += value; }
            remove { this.treeSelectionRequested -= value; }
        }
        public event ProjectSaveRequestHandler ProjectSaveRequest;

#if PDF
		private System.ComponentModel.BackgroundWorker backgroundSaver;
		private System.ComponentModel.BackgroundWorker backgroundSaver2;
#endif

		public ImportedPlansPanel() {
			InitializeComponent();
#if PDF
			this.backgroundSaver = new BackgroundWorker();
			this.backgroundSaver.DoWork += new DoWorkEventHandler(backgroundSaver_DoWork);
			this.backgroundSaver.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundSaver_RunWorkerCompleted);
			this.backgroundSaver2 = new BackgroundWorker();
			this.backgroundSaver2.DoWork += new DoWorkEventHandler(backgroundSaver2_DoWork);
			this.backgroundSaver2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundSaver2_RunWorkerCompleted);
#endif
			this.SetLanguage();
		}

		private void SetLanguage() {
			this.lblImportedPlans.Text = EuroplanRes.ImportedPlansPanel_ImportiertePlaene; //"Planverwaltung"
			this.btnImport.Text = EuroplanRes.ImportedPlansPanel_PlanImportieren; //"Plan importieren"
			this.btnDelete.Text = EuroplanRes.ImportedPlansPanel_PlanEntfernen; //"Plan entfernen"
			this.btnExport.Text = EuroplanRes.ImportedPlansPanel_PlanExportieren; //"Plan exportieren"
			this.nameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ImportedPlansPanel_PlanName; //"Name"
			this.RelativeFileName.HeaderText = EuroplanRes.ImportedPlansPanel_DateiPfad; //"Pfad"
			this.dataGridViewTextBoxColumn1.HeaderText = EuroplanRes.ImportedPlansPanel_DateiPfad;
			this.colOptions.HeaderText = EuroplanRes.ImportedPlansPanel_Optionen; //"Optionen"
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

		[Obsolete]
		private List<SolidFramework.Pdf.Plumbing.PdfPage> GetPdfPages(string filename) {
			List<SolidFramework.Pdf.Plumbing.PdfPage> pageList = null;
			SolidFramework.Pdf.Catalog catalog = null;
			SolidFramework.Pdf.Plumbing.PdfPages pages = null;
			SolidFramework.Pdf.PdfDocument doc = null;
			
			doc = new SolidFramework.Pdf.PdfDocument(filename);
			doc.Open();
			// Get our pages.
			pageList = new List<SolidFramework.Pdf.Plumbing.PdfPage>(doc.Catalog.Pages.PageCount);
			catalog = (SolidFramework.Pdf.Catalog)SolidFramework.Pdf.Catalog.Create(doc);
			pages = (SolidFramework.Pdf.Plumbing.PdfPages)catalog.Pages;
			ProcessPages(ref pages, ref pageList);

			return pageList;
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
#if PDF
				dialog.Filter = EuroplanRes.ImportedPlansPanel_AlleFilter + "|*.dxf;*.dwg;*.pdf;*.jpg;*.png;*.bmp";
#else
				dialog.Filter = EuroplanRes.ImportedPlansPanel_AlleFilter + "|*.dxf;*.dwg;*.jpg;*.png;*.bmp";
#endif
				dialog.Filter += "|" + EuroplanRes.ImportedPlansPanel_DxfFilter + "|*.dxf;*.dwg";
#if PDF
				dialog.Filter += "|" + EuroplanRes.ImportedPlansPanel_PdfFilter + "|*.pdf";
#endif
				dialog.Filter += "|" + EuroplanRes.ImportedPlansPanel_ImageFilter + "|*.jpg;*.png;*.bmp";
				dialog.Multiselect = false;
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					List<Plan> plans = Project.Instance.ImportedPlans;
					string extension = Path.GetExtension(dialog.FileName);
					string file = isPdf(extension) ? Path.GetFileNameWithoutExtension(dialog.FileName) + ".png" : Path.GetFileName(dialog.FileName);
					foreach (Plan plan in plans) {
						if (Path.GetFileName(plan.RelativeFileName).Equals(file)) {
							result = MessageBox.Show(EuroplanRes.ImportedPlansPanel_PlanSchonVorhanden);
							return;
						}
					}
					
					NewPlanForm newPlanForm = null;

#if PDF
#if !DEBUG
					// Solid Framework license
					SolidFramework.LicenseCollection.Instance.Clear();
					SolidFramework.License.Import("Christian Neudorfer", "christian.neudorfer@bluesource.at", "bluesource - mobile solutions gmbh", "CXZC");
#endif

					List<SolidFramework.Pdf.Plumbing.PdfPage> pages = null;
#endif

					if (isPdf(extension)) {
#if PDF
						pages = this.GetPdfPages(dialog.FileName);
						newPlanForm = new NewPlanForm(true);
						newPlanForm.NumOfPages = pages.Count;
#endif
					} else {
						newPlanForm = new NewPlanForm(false);
					}
					result = newPlanForm.ShowDialog();
					if (result == DialogResult.OK) {
						Plan plan = null;
						string dir = Path.GetDirectoryName(Project.Instance.ProjectFileName);
						string subDir = Path.GetFileNameWithoutExtension(Project.Instance.ProjectFileName) + "_plans";
						dir = Path.Combine(dir, subDir);
						if (!Directory.Exists(dir)) {
							Directory.CreateDirectory(dir);
						}
						
						if (isPdf(extension)) {
#if PDF
							previewSemaphore = new Semaphore(0, 1);
							ProgressForm progressForm = new ProgressForm(previewSemaphore);
							PreviewConverterArguments args = new PreviewConverterArguments(progressForm, newPlanForm, dialog.FileName, /*pages,*/ dir, subDir, extension);
							this.backgroundSaver.RunWorkerAsync(args);

							progressForm.ShowDialog();
							dialog.Dispose();
#endif
						} else {
							if (isImage(extension)) {
								plan = new ImagePlan();
							} else if (isCad(extension)) {
								plan = new CadPlan();
							}

							
							string newFileName = Path.Combine(dir, Path.GetFileName(dialog.FileName));
							if (!dialog.FileName.Equals(newFileName)) {
								File.Copy(dialog.FileName, newFileName, true);
							}

							plan.Name = newPlanForm.PlanName;
							plan.RelativeFileName = Path.Combine(subDir, isPdf(extension) ? Path.GetFileNameWithoutExtension(dialog.FileName) + ".png" : Path.GetFileName(dialog.FileName));
							bool import = true;
                            if (isDwg(extension)) {
                                MessageBox.Show(EuroplanRes.ImportedPlansPanel_DwgWarnung, EuroplanRes.ImportedPlansPanel_DwgWarnungTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
							if (plan.IsLargePlan) {
								import = MessageBox.Show(EuroplanRes.ImportedPlansPanel_GrosserPlanText, EuroplanRes.ImportedPlansPanel_GrosserPlanTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
							}
							if (import) {
								plans.Add(plan);
								if (projectChanged != null) {
									projectChanged(this);
								}
								UpdateControl(false);
							}

							newPlanForm.Dispose();
							dialog.Dispose();
							if (import) {
								openPlanOptions(plan);
							}
						}						
					}
				}
			}
		}

		private static readonly int PDF_PREVIEW_DPI = 72;
		public static readonly int PDF_PT_PER_INCH = 72;

		private static Semaphore previewSemaphore;
		private static Semaphore finalSemaphore;

#if PDF
		private void backgroundSaver_DoWork(object sender, DoWorkEventArgs e) {
			PreviewConverterArguments args = e.Argument as PreviewConverterArguments;

			List<SolidFramework.Pdf.Plumbing.PdfPage> pages = this.GetPdfPages(args.FileName);

			// Create a bitmap from the page with set dpi.
			Bitmap bm = pages[args.NewPlanForm.PageNumber - 1].DrawBitmap(PDF_PREVIEW_DPI);

			// Setup the filename.
			args.TmpFileName = Path.GetTempFileName();

			// If the file exits already, delete it. I.E. Overwrite it.
			if (File.Exists(args.TmpFileName))
				File.Delete(args.TmpFileName);

			// Save the file.
			bm.Save(args.TmpFileName, System.Drawing.Imaging.ImageFormat.Png);

			// Cleanup.
			bm.Dispose();

			e.Result = args;
		}

		private void backgroundSaver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			previewSemaphore.WaitOne();
			PreviewConverterArguments args = e.Result as PreviewConverterArguments;
			args.ProgressForm.Close();
			TempImagePlan plan = new TempImagePlan();
			plan.Name = args.NewPlanForm.PlanName;
			plan.SetAbsoluteFilename(args.TmpFileName);

			PdfRegionPickerForm regionPickerForm = new PdfRegionPickerForm(plan);
			DialogResult dr = regionPickerForm.ShowDialog();
			if (dr == DialogResult.Cancel) {
				return;
			}
			int dpi = regionPickerForm.Dpi;

			List<SolidFramework.Pdf.Plumbing.PdfPage> pages = this.GetPdfPages(args.FileName);
			SolidFramework.Pdf.Plumbing.PdfPage page = pages[args.NewPlanForm.PageNumber - 1];

			double top = 0, left = 0, bottom = 0, right = 0;
			
			if (regionPickerForm.TopLeft.HasValue && regionPickerForm.BottomRight.HasValue) {
				top = regionPickerForm.TopLeft.Value.Y * PDF_PT_PER_INCH / PDF_PREVIEW_DPI;
				left = regionPickerForm.TopLeft.Value.X * PDF_PT_PER_INCH / PDF_PREVIEW_DPI;
				bottom = regionPickerForm.BottomRight.Value.Y * PDF_PT_PER_INCH / PDF_PREVIEW_DPI;
				right = regionPickerForm.BottomRight.Value.X * PDF_PT_PER_INCH / PDF_PREVIEW_DPI;
			} else {
				bottom = page.TrimBox.Bottom - page.TrimBox.Top;
				right = page.TrimBox.Right - page.TrimBox.Left;
			}

			finalSemaphore = new Semaphore(0, 1);
			ProgressForm progressForm = new ProgressForm(finalSemaphore);
			FinalConverterArguments fArgs = new FinalConverterArguments(progressForm, args.NewPlanForm, args.FileName, args.Dir, args.SubDir, args.Extension, top, left, bottom, right, dpi);
			this.backgroundSaver2.RunWorkerAsync(fArgs);
			progressForm.ShowDialog();
		}

		private void backgroundSaver2_DoWork(object sender, DoWorkEventArgs e) {
			FinalConverterArguments args = e.Argument as FinalConverterArguments;

			double dpi = args.Dpi;

			double width = args.Right - args.Left;
			double height = args.Bottom - args.Top;

			Bitmap bm = new Bitmap((int)(width * dpi / PDF_PT_PER_INCH), (int)(height * dpi / PDF_PT_PER_INCH));
			Graphics g = Graphics.FromImage(bm);
			System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
			m.Translate((float)(-args.Left * dpi / PDF_PT_PER_INCH), (float)(-args.Top * dpi / PDF_PT_PER_INCH));
			m.Scale((float)(dpi / PDF_PT_PER_INCH), (float)(dpi / PDF_PT_PER_INCH));
			g.Transform = m;
			List<SolidFramework.Pdf.Plumbing.PdfPage> pages = this.GetPdfPages(args.FileName);
			pages[args.NewPlanForm.PageNumber - 1].DrawToHDC(ref g);

			// Setup the filename.
			string newFileName = Path.Combine(args.Dir, Path.GetFileNameWithoutExtension(args.FileName) + ".png");

			// If the file exits already, delete it. I.E. Overwrite it.
			if (File.Exists(newFileName))
				File.Delete(newFileName);

			// Save the file.
			bm.Save(newFileName, System.Drawing.Imaging.ImageFormat.Png);

			// Cleanup.
			bm.Dispose();

			e.Result = args;

		}

		private void backgroundSaver2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			finalSemaphore.WaitOne();

			FinalConverterArguments args = e.Result as FinalConverterArguments;

			args.ProgressForm.Close();
			ImagePlan plan = new ImagePlan();
			plan.Name = args.NewPlanForm.PlanName;
			plan.RelativeFileName = Path.Combine(args.SubDir, isPdf(args.Extension) ? Path.GetFileNameWithoutExtension(args.FileName) + ".png" : Path.GetFileName(args.FileName));

			Project.Instance.ImportedPlans.Add(plan);
            if (this.projectChanged != null) {
                this.projectChanged(this);
			}
			UpdateControl(false);

			args.NewPlanForm.Dispose();
			openPlanOptions(plan);
		}

#endif

		private bool isImage(string extension) {
			return string.Compare(".jpg", extension, true) == 0 ||
				string.Compare(".bmp", extension, true) == 0 ||
				string.Compare(".png", extension, true) == 0;
		}
		
		private bool isCad(string extension) {
			return string.Compare(".dxf", extension, true) == 0 ||
				string.Compare(".dwg", extension, true) == 0;
		}

		private bool isPdf(string extension) {
			return string.Compare(".pdf", extension, true) == 0;
		}

        private bool isDwg(string extension) {
            return string.Compare(".dwg", extension, true) == 0;
        }

		private void btnDelete_Click(object sender, EventArgs e) {
			// check if plan is already used
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
			if (projectChanged != null) {
				projectChanged(this);
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
                        if (this.projectChanged != null) {
                            this.projectChanged(this);
						}
					}
					ipoForm.Dispose();
				} else if (plan is CadPlan) {
					CadPlanOptionsForm cpoForm = null;
					try {
						cpoForm = new CadPlanOptionsForm(plan as CadPlan);
						result = cpoForm.ShowDialog();
						if (cpoForm.UnsavedChanges) {
                            if (this.projectChanged != null) {
                                this.projectChanged(this);
							}
						}
					} catch (Exception) {
						MessageBox.Show(EuroplanRes.ImportedPlansPanel_CadFehlerText, EuroplanRes.ImportedPlansPanel_CadFehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (this.projectChanged != null) {
                    this.projectChanged(this);
				}
				UpdateControl(false);
			}
		}

#if PDF
		private static void ProcessPages(ref SolidFramework.Pdf.Plumbing.PdfPages pages,
			ref List<SolidFramework.Pdf.Plumbing.PdfPage> listPages) {
			// Walk the Pages catalog and get all the page objects.  This will follow 
			// the references and get the actual object that we can work 
			// with recursively.
			foreach (SolidFramework.Pdf.Plumbing.PdfItem pdfItem in pages.Kids) {
				SolidFramework.Pdf.Plumbing.PdfDictionary dictionary =
					(SolidFramework.Pdf.Plumbing.PdfDictionary)
					SolidFramework.Pdf.Plumbing.PdfItem.GetIndirectionItem(pdfItem);
				if (dictionary.Type == "Pages") {
					SolidFramework.Pdf.Plumbing.PdfPages nodePages =
						(SolidFramework.Pdf.Plumbing.PdfPages)dictionary;
					ProcessPages(ref nodePages, ref listPages);
				} else if (dictionary.Type == "Page") {
					SolidFramework.Pdf.Plumbing.PdfPage page =
						(SolidFramework.Pdf.Plumbing.PdfPage)dictionary;
					listPages.Add(page);
				}
			}
		}
#endif

	}
}
