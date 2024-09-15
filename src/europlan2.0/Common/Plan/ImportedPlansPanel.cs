using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using log4net;

namespace Europlan.Common
{
    public partial class ImportedPlansPanel : UserControl, IEditorUserControl, ISaveRequest
    {


        internal const Int32 PDF_PREVIEW_DPI = 72;
        internal const Int32 PDF_PT_PER_INCH = 72;



#if PDF
        private class PreviewConverterArguments
        {
            private ProgressForm progressForm;
            private NewPlanForm newPlanForm;
            private string fileName;
            private string dir;
            private string subDir;
            private string extension;
            private string tmpFileName;

            public PreviewConverterArguments(ProgressForm progressForm, NewPlanForm newPlanform, string fileName, /*List<SolidFramework.Pdf.Plumbing.PdfPage> Pages,*/ string dir, string subDir, string extension)
            {
                this.progressForm = progressForm;
                this.newPlanForm = newPlanform;
                this.fileName = fileName;
                this.dir = dir;
                this.subDir = subDir;
                this.extension = extension;
            }

            public ProgressForm ProgressForm
            {
                get { return this.progressForm; }
            }

            public NewPlanForm NewPlanForm
            {
                get { return this.newPlanForm; }
            }

            public string FileName
            {
                get { return this.fileName; }
            }

            public string Dir
            {
                get { return this.dir; }
            }

            public string SubDir
            {
                get { return this.subDir; }
            }

            public string Extension
            {
                get { return this.extension; }
            }

            public string TmpFileName
            {
                get { return this.tmpFileName; }
                set { this.tmpFileName = value; }
            }
        }

        private class FinalConverterArguments
        {
            private ProgressForm progressForm;
            private NewPlanForm newPlanForm;
            private string fileName;
            private string dir;
            private string subDir;
            private string extension;
            private double top, left, bottom, right;
            private int dpi;

            public FinalConverterArguments(ProgressForm progressForm, NewPlanForm newPlanform, string fileName, string dir, string subDir, string extension, double top, double left, double bottom, double right, int dpi)
            {
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

            public ProgressForm ProgressForm
            {
                get { return this.progressForm; }
            }

            public NewPlanForm NewPlanForm
            {
                get { return this.newPlanForm; }
            }

            public string FileName
            {
                get { return this.fileName; }
            }

            public string Dir
            {
                get { return this.dir; }
            }

            public string SubDir
            {
                get { return this.subDir; }
            }

            public string Extension
            {
                get { return this.extension; }
            }

            public double Top
            {
                get { return this.top; }
            }

            public double Left
            {
                get { return this.left; }
            }

            public double Bottom
            {
                get { return this.bottom; }
            }

            public double Right
            {
                get { return this.right; }
            }

            public int Dpi
            {
                get { return this.dpi; }
            }
        }
#endif

        private static readonly ILog log = LogManager.GetLogger(typeof(ImportedPlansPanel));

        private event ProjectStructureChangedHandler projectStructureChanged;
        public event ProjectStructureChangedHandler ProjectStructureChanged
        {
            add { this.projectStructureChanged += value; }
            remove { this.projectStructureChanged -= value; }
        }
        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged
        {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }
        private event TreeSelectionRequestedHandler treeSelectionRequested;
        public event TreeSelectionRequestedHandler TreeSelectionRequested
        {
            add { this.treeSelectionRequested += value; }
            remove { this.treeSelectionRequested -= value; }
        }
        public event ProjectSaveRequestHandler ProjectSaveRequest;


        public ImportedPlansPanel()
        {
            InitializeComponent();
            this.SetLanguage();
        }


        private Rectangle GetPlanRegion(PDF.PdfHelper pdfHelper, Int32 pageIndex, WW.Math.Point2D? pointTopLeft, WW.Math.Point2D? pointBottomRight)
        {
            if (pointTopLeft.HasValue && pointBottomRight.HasValue)
            {
                var top = Convert.ToInt32(pointTopLeft.Value.Y * PDF_PT_PER_INCH / PDF_PREVIEW_DPI);
                var left = Convert.ToInt32(pointTopLeft.Value.X * PDF_PT_PER_INCH / PDF_PREVIEW_DPI);
                var bottom = Convert.ToInt32(pointBottomRight.Value.Y * PDF_PT_PER_INCH / PDF_PREVIEW_DPI);
                var right = Convert.ToInt32(pointBottomRight.Value.X * PDF_PT_PER_INCH / PDF_PREVIEW_DPI);
                var width = right - left;
                var height = bottom - top;
                var rectangle = new Rectangle(left, top, width, height);
                return rectangle;
            }
            else
            {
                var pageSize = pdfHelper.GetPageSize(pageIndex);
                var width = pageSize.Right - pageSize.Left;
                var height = pageSize.Bottom - pageSize.Top;
                var rectangle = new Rectangle(pageSize.Left, pageSize.Top, width, height);
                return rectangle;
            }
        }

        private void SetLanguage()
        {
            this.lblImportedPlans.Text = EuroplanRes.ImportedPlansPanel_ImportiertePlaene; //"Planverwaltung"
            this.btnImport.Text = EuroplanRes.ImportedPlansPanel_PlanImportieren; //"Plan importieren"
            this.btnDelete.Text = EuroplanRes.ImportedPlansPanel_PlanEntfernen; //"Plan entfernen"
            this.btnExport.Text = EuroplanRes.ImportedPlansPanel_PlanExportieren; //"Plan exportieren"
            this.nameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ImportedPlansPanel_PlanName; //"Name"
            this.RelativeFileName.HeaderText = EuroplanRes.ImportedPlansPanel_DateiPfad; //"Pfad"
            this.dataGridViewTextBoxColumn1.HeaderText = EuroplanRes.ImportedPlansPanel_DateiPfad;
            this.colOptions.HeaderText = EuroplanRes.ImportedPlansPanel_Optionen; //"Optionen"
        }

        public void UpdateControl(bool resetUserInterface)
        {
            btnDelete.Enabled = Project.Instance.ImportedPlans.Count > 0;
            btnExport.Enabled = Project.Instance.ImportedPlans.Count > 0;
            planSource.DataSource = Project.Instance.ImportedPlans;
            planSource.ResetBindings(false);
        }

        public bool AllowLeave()
        {
            return true;
        }

        private void CheckIfPlanAlreadyImported(OpenFileDialogResult openFileDialogResult)
        {
            var fileNameToLookFor = openFileDialogResult.FileNameWithoutExtension + (openFileDialogResult.IsPdf ? ".png" : openFileDialogResult.Extension);
            var isPlanAlreadyImported = Project.Instance.ImportedPlans
                    .Select(plan => Path.GetFileName(plan.RelativeFileName))
                    .Any(planFileName => String.Equals(fileNameToLookFor, planFileName, StringComparison.OrdinalIgnoreCase));
            if (isPlanAlreadyImported)
            {
                if (openFileDialogResult.IsPdf)
                {
                    // Bei einem PDF erhält der Nutzer einen Hinweis darauf und kann wählen, ob er abermals importieren möchte. Grund dafür ist, dass man ggf. mehrere Teile aus einem PDF importieren möchte
                    var dialogResult = MessageBox.Show(EuroplanRes.ImportedPlansPanel_PdfSchonImportiertMeldung, EuroplanRes.ImportedPlansPanel_PdfSchonImportiertTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        throw new ExceptionImportAborted();
                    }
                }
                else
                {
                    MessageBox.Show(EuroplanRes.ImportedPlansPanel_PlanSchonVorhanden, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    throw new ExceptionImportAborted();
                }
            }
        }

        private Plan CreatePlanInstance(OpenFileDialogResult openFileDialogResult)
        {
            if (openFileDialogResult.IsImage) return new ImagePlan();
            if (openFileDialogResult.IsCad) return new CadPlan();
            throw new ExceptionInvalidExtension(openFileDialogResult.Extension);
        }

        private OpenFileDialogResult DetermineFileToImport()
        {
            var fileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "dxf",
                Filter = EuroplanRes.ImportedPlansPanel_AlleFilter + "|*.dxf;*.dwg;*.pdf;*.jpg;*.png;*.bmp" +
                        "|" + EuroplanRes.ImportedPlansPanel_DxfFilter + "|*.dxf;*.dwg" +
                        "|" + EuroplanRes.ImportedPlansPanel_PdfFilter + "|*.pdf" +
                        "|" + EuroplanRes.ImportedPlansPanel_ImageFilter + "|*.jpg;*.png;*.bmp",
                Multiselect = false
            };
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                return new OpenFileDialogResult(fileDialog);
            }
            else
            {
                return null;
            }
        }

        private void openPlanOptions(Plan plan)
        {
            DialogResult result = DialogResult.OK;
            if (plan != null)
            {

                if (plan is ImagePlan)
                {
                    ImagePlanOptionsForm ipoForm = new ImagePlanOptionsForm(plan as ImagePlan);
                    result = ipoForm.ShowDialog();
                    if (ipoForm.UnsavedChanges)
                    {
                        if (this.projectChanged != null)
                        {
                            this.projectChanged(this);
                        }
                    }
                    ipoForm.Dispose();
                }
                else if (plan is CadPlan)
                {
                    CadPlanOptionsForm cpoForm = null;
                    try
                    {
                        cpoForm = new CadPlanOptionsForm(plan as CadPlan);
                        result = cpoForm.ShowDialog();
                        if (cpoForm.UnsavedChanges)
                        {
                            if (this.projectChanged != null)
                            {
                                this.projectChanged(this);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(EuroplanRes.ImportedPlansPanel_CadFehlerText, EuroplanRes.ImportedPlansPanel_CadFehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        deletePlan(plan);
                    }
                    finally
                    {
                        if (cpoForm != null)
                        {
                            cpoForm.Dispose();
                        }
                    }
                }
                if (result == DialogResult.Cancel)
                {
                    deletePlan(plan);
                }
            }
        }

        private void deletePlan(Plan plan)
        {
            if (plan != null)
            {
                string fileName = plan.AbsoluteFileName;
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                Project.Instance.ImportedPlans.Remove(plan);
                if (this.projectChanged != null)
                {
                    this.projectChanged(this);
                }
                UpdateControl(false);
            }
        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Ob das Projekt gespeichert ist, wird über ein Ereignis ausgelöst; dieses liefert im out-Parameter auch zurück, ob das Projekt gespeichert wurde
                var isProjectSaved = true;
                ProjectSaveRequest?.Invoke(this, true, out isProjectSaved);

                if (isProjectSaved)
                {
                    var openFileDialogResult = DetermineFileToImport();
                    if (openFileDialogResult != null)
                    {
                        try
                        {
                            CheckIfPlanAlreadyImported(openFileDialogResult);
                            var projectPlansDirectoryName = Project.Instance.GetProjectPlansDirectoryName();
                            var projectPlansSubdirectoryName = Project.Instance.GetProjectPlansSubdirectoryName();
                            if (!Directory.Exists(projectPlansDirectoryName))
                            {
                                Directory.CreateDirectory(projectPlansDirectoryName);
                            }

                            using (var newPlanForm = new NewPlanForm())
                            {
                                if (newPlanForm.ShowDialog() == DialogResult.OK)
                                {
                                    if (openFileDialogResult.IsPdf)
                                    {
                                        var pdfHelper = new PDF.PdfHelper(openFileDialogResult.FilePath);
                                        using (var pdfRegionPicketForm = new PdfRegionPickerForm(pdfHelper))
                                        {
                                            if (pdfRegionPicketForm.ShowDialog() == DialogResult.OK)
                                            {
                                                if (pdfRegionPicketForm.SelectedPageIndex.HasValue)
                                                {
                                                    var selectedPageIndex = pdfRegionPicketForm.SelectedPageIndex.Value;
                                                    var planRegion = GetPlanRegion(pdfHelper, selectedPageIndex, pdfRegionPicketForm.TopLeft, pdfRegionPicketForm.BottomRight);
                                                    var newPlanAbsolutPath = PlanFileHandling.Default.GetLocalFilePath(openFileDialogResult.FilePath, projectPlansDirectoryName, isPdf: true);
                                                    var newPlanRelativePath = Path.Combine(projectPlansSubdirectoryName, Path.GetFileName(newPlanAbsolutPath));
                                                    pdfHelper.SaveToPng(pdfRegionPicketForm.SelectedPageIndex.Value, newPlanAbsolutPath, planRegion, pdfRegionPicketForm.Dpi);

                                                    var newPlan = new ImagePlan();
                                                    newPlan.Name = newPlanForm.PlanName;
                                                    newPlan.RelativeFileName = newPlanRelativePath;
                                                    Project.Instance.ImportedPlans.Add(newPlan);
                                                    if (this.projectChanged != null) this.projectChanged(this);
                                                    UpdateControl(false);
                                                    openPlanOptions(newPlan);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var newPlanAbsolutPath = PlanFileHandling.Default.GetLocalFilePath(openFileDialogResult.FilePath, projectPlansDirectoryName, isPdf: false);
                                        var newPlanRelativePath = Path.Combine(projectPlansSubdirectoryName, Path.GetFileName(newPlanAbsolutPath));
                                        var newPlan = CreatePlanInstance(openFileDialogResult);
                                        newPlan.Name = newPlanForm.PlanName;
                                        newPlan.RelativeFileName = newPlanRelativePath;
                                        if (!String.Equals(openFileDialogResult.FilePath, newPlanAbsolutPath, StringComparison.OrdinalIgnoreCase))
                                        {
                                            File.Copy(openFileDialogResult.FilePath, newPlanAbsolutPath, overwrite: true);
                                        }

                                        var import = true;
                                        if (openFileDialogResult.IsDwg)
                                        {
                                            MessageBox.Show(EuroplanRes.ImportedPlansPanel_DwgWarnung, EuroplanRes.ImportedPlansPanel_DwgWarnungTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                        if (newPlan.IsLargePlan)
                                        {
                                            import = MessageBox.Show(EuroplanRes.ImportedPlansPanel_GrosserPlanText, EuroplanRes.ImportedPlansPanel_GrosserPlanTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                                        }
                                        if (import)
                                        {
                                            Project.Instance.ImportedPlans.Add(newPlan);
                                            if (projectChanged != null) projectChanged(this);
                                            UpdateControl(false);
                                            openPlanOptions(newPlan);
                                        }
                                    }
                                }
                            }
                        }
                        catch (ExceptionImportAborted) { return; }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // check if plan is already used
            // if (alreadyused) { ....
            DialogResult result = MessageBox.Show(EuroplanRes.ImportedPlansPanel_WirklichLoeschenMessage, EuroplanRes.ImportedPlansPanel_WirklichLoeschenTitle, MessageBoxButtons.YesNo);
            if (result.Equals(DialogResult.Yes))
            {
                if (dgvPlans.SelectedRows[0] != null)
                {
                    Plan plan = dgvPlans.SelectedRows[0].DataBoundItem as Plan;
                    deletePlan(plan);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvPlans.SelectedRows[0] != null)
            {
                Plan plan = dgvPlans.SelectedRows[0].DataBoundItem as Plan;
                ExportPlanForm form = new ExportPlanForm(plan);
                form.ShowDialog();
                form.Dispose();
            }
        }

        private void dgvPlans_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (projectChanged != null)
            {
                projectChanged(this);
            }
        }

        private void dgvPlans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < this.dgvPlans.Columns.Count &&
                this.dgvPlans.Columns[e.ColumnIndex] == this.colOptions &&
                e.RowIndex >= 0 && e.RowIndex < this.dgvPlans.Rows.Count)
            {
                Plan plan = this.dgvPlans.Rows[e.RowIndex].DataBoundItem as Plan;
                if (plan != null)
                {
                    openPlanOptions(plan);
                }
            }
        }


        private class OpenFileDialogResult
        {

            private readonly String[] EXTENSION_FOR_CAD = { "DWG", "DXF" };
            private readonly String[] EXTENSION_FOR_DWG = { "DWG" };
            private readonly String[] EXTENSION_FOR_IMG = { "BMP", "JPG", "PNG" };
            private readonly String[] EXTENSION_FOR_PDF = { "PDF" };


            public String Extension { get; private set; }

            public String FileName { get; private set; }

            public String FileNameWithoutExtension { get; private set; }

            public String FilePath { get; private set; }

            public Boolean IsCad { get; private set; }

            public Boolean IsDwg { get; private set; }

            public Boolean IsImage { get; private set; }

            public Boolean IsPdf { get; private set; }


            internal OpenFileDialogResult(OpenFileDialog openFileDialog)
            {
                Extension = Path.GetExtension(openFileDialog.FileName);
                FilePath = openFileDialog.FileName;
                FileName = Path.GetFileName(openFileDialog.FileName);
                FileNameWithoutExtension = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                IsImage = EXTENSION_FOR_IMG.Any(extension => String.Equals(extension, Extension.TrimStart('.'), StringComparison.OrdinalIgnoreCase));
                IsCad = EXTENSION_FOR_CAD.Any(extension => String.Equals(extension, Extension.TrimStart('.'), StringComparison.OrdinalIgnoreCase));
                IsPdf = EXTENSION_FOR_PDF.Any(extension => String.Equals(extension, Extension.TrimStart('.'), StringComparison.OrdinalIgnoreCase));
                IsDwg = EXTENSION_FOR_DWG.Any(extension => String.Equals(extension, Extension.TrimStart('.'), StringComparison.OrdinalIgnoreCase));
            }

        }


        private class ExceptionImportAborted : Exception { }

        private class ExceptionInvalidExtension : Exception
        {

            private const String MESSAGE = "Diese Datei kann nicht importiert werden.";

            public String Extension { get; private set; }

            internal ExceptionInvalidExtension(String extension)
            : base(MESSAGE)
            {
                Extension = extension;
            }

        }

    }
}
