using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Star.SettingsXpress;
using WW.Math;

namespace Europlan.Common
{

    public partial class PdfRegionPickerForm : Form
    {


        private PDF.PdfHelper PdfHelper;
        private Int32? PdfPageCount;


        public Nullable<Point2D> BottomRight => this.pdfRegionPicker.BottomRight;

        public Int32 Dpi
        {
            get
            {
                switch (this.sliderSize.Value)
                {
                    case 0:
                        return 54;
                    case 1:
                        return 72;
                    case 2:
                        return 96; // default
                    case 3:
                        return 120;
                    case 4:
                        return 150;
                    default:
                        return 96;
                }
            }
        }

        public int PixelHeight
        {
            get
            {
                if (!this.pdfRegionPicker.TopLeft.HasValue || !this.pdfRegionPicker.BottomRight.HasValue)
                {
                    return 0;
                }
                double height = this.pdfRegionPicker.BottomRight.Value.Y - this.pdfRegionPicker.TopLeft.Value.Y;
                return (int)(height * this.Dpi / ImportedPlansPanel.PDF_PT_PER_INCH);
            }
        }

        public int PixelWidth
        {
            get
            {
                if (!this.pdfRegionPicker.TopLeft.HasValue || !this.pdfRegionPicker.BottomRight.HasValue)
                {
                    return 0;
                }
                double width = this.pdfRegionPicker.BottomRight.Value.X - this.pdfRegionPicker.TopLeft.Value.X;
                return (int)(width * this.Dpi / ImportedPlansPanel.PDF_PT_PER_INCH);
            }
        }

        public Int32? SelectedPageIndex { get; private set; }

        public Nullable<Point2D> TopLeft => this.pdfRegionPicker.TopLeft;


        internal PdfRegionPickerForm(PDF.PdfHelper pdfHelper)
        {
            PdfHelper = pdfHelper;
            PdfPageCount = pdfHelper.GetPageCount();
            SelectedPageIndex = null;
            InitializeComponent();
            this.SetLanguage();
        }


        private void LoadPageImage()
        {
            if (SelectedPageIndex.HasValue)
            {
                TempImagePlan plan = new TempImagePlan();
                plan.Name = "(neu)";
                var tempPath = System.IO.Path.GetTempFileName();
                using (var pageBitmap = PdfHelper.GetPageBitmap(SelectedPageIndex.Value))
                {
                    pageBitmap.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
                    plan.SetAbsoluteFilename(tempPath);
                    this.picturePanel.Plan = plan;
                    this.picturePanel.ProductPlanner = this.pdfRegionPicker;
                }
            }
        }

        private void SetLanguage()
        {
            this.Text = EuroplanRes.PdfRegionPickerForm_BereichAuswaehlen;
            this.btnZoomIn.Text = EuroplanRes.Plan_Heranzoomen;
            this.btnZoomIn.ToolTipText = EuroplanRes.Plan_Heranzoomen;
            this.btnZoomOut.Text = EuroplanRes.Plan_Herauszoomen;
            this.btnZoomOut.ToolTipText = EuroplanRes.Plan_Herauszoomen;
            this.btnMove.Text = EuroplanRes.Plan_Verschieben;
            this.btnMove.ToolTipText = EuroplanRes.Plan_Verschieben;
            this.btnSelectRegion.Text = EuroplanRes.PdfRegionPickerForm_Auswaehlen;
            this.btnSelectRegion.ToolTipText = EuroplanRes.PdfRegionPickerForm_Auswaehlen;
            this.lblSize.Text = EuroplanRes.PdfRegionPickerForm_Bildgroesse;
            this.lblSmall.Text = EuroplanRes.PdfRegionPickerForm_Klein;
            this.lblMedium.Text = EuroplanRes.PdfRegionPickerForm_Normal;
            this.lblLarge.Text = EuroplanRes.PdfRegionPickerForm_Gross;
            this.btnOk.Text = EuroplanRes.General_Uebernehmen;
        }


        private void Form_Load(Object sender, EventArgs e)
        {
            try
            {
                SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
                this.Location = settings.GetPoint("Location", this.Location);
                this.Size = settings.GetSize("Size", this.Size);
                this.ProgressLoad.Maximum = PdfPageCount.HasValue ? PdfPageCount.Value : 0;
                this.ProgressLoad.Minimum = 0;
            }
            catch (Exception) { }
        }

        private void Form_Shown(Object sender, EventArgs e)
        {
            try
            {
                this.BackgroundWorkerPdfLoading.RunWorkerAsync();
            }
            catch (Exception) { }
        }

        private void Form_FormClosing(Object sender, FormClosingEventArgs e)
        {
            bool import = true;
            if (this.PixelHeight * this.PixelWidth > ImagePlan.largePlanSize)
            {
                DialogResult dr = MessageBox.Show(EuroplanRes.PdfRegionPickerForm_GrosserPlanText, EuroplanRes.PdfRegionPickerForm_GrosserPlanTitel, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                e.Cancel = (dr == DialogResult.Cancel);
                if (e.Cancel)
                {
                    return;
                }
                else
                {
                    import = (dr == DialogResult.Yes);
                }
            }

            SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
            settings.StorePoint("Location", this.Location);
            settings.StoreSize("Size", this.Size);
            if (import)
            {
                this.picturePanel.ApplyChangesToPlan();
            }
            SettingsFile.Update();

            this.DialogResult = import ? DialogResult.OK : DialogResult.Cancel;
        }

        private void BackgroundWorkerPdfLoading_DoWork(Object sender, DoWorkEventArgs e)
        {
            if (sender is BackgroundWorker backgroundWorker)
            {
                if (PdfPageCount.HasValue)
                {
                    for (var pageIndex = 0; pageIndex < PdfPageCount.Value; pageIndex++)
                    {
                        try
                        {
                            var pageSize = PdfHelper.GetPageSize(pageIndex);
                            var originalPageWidth = pageSize.Width;
                            var pageScale = 200.0 / originalPageWidth;
                            var previewWidth = Convert.ToInt32(pageSize.Width * pageScale);
                            var previewHeight = Convert.ToInt32(pageSize.Height * pageScale);
                            var previreBitmap = PdfHelper.GetPageBitmap(pageIndex, previewWidth, previewHeight);
                            var pictureBoxPreview = new PictureBox()
                            {
                                BorderStyle = BorderStyle.FixedSingle,
                                Cursor = Cursors.Hand,
                                Image = Image.FromHbitmap(previreBitmap.GetHbitmap()),
                                Margin = new Padding(8, 8, 8, 16),
                                SizeMode = PictureBoxSizeMode.CenterImage,
                                Size = new Size(previewWidth, previewHeight),
                                Tag = pageIndex
                            };
                            pictureBoxPreview.Click += PictureBoxPreview_Click;
                            backgroundWorker.ReportProgress(pageIndex, pictureBoxPreview);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private void BackgroundWorkerPdfLoading_ProgressChanged(Object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is PictureBox pictureBox)
            {
                this.Invoke(
                        new Action(
                                delegate ()
                                {
                                    this.ProgressLoad.Value = e.ProgressPercentage + 1;
                                    this.FlowPanelPreview.Controls.Add(pictureBox);

                                }
                            )
                    );
            }
        }

        private void BackgroundWorkerPdfLoading_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            this.ProgressLoad.Visible = false;
        }

        private void PictureBoxPreview_Click(Object sender, EventArgs e)
        {
            try
            {
                if (sender is PictureBox pictureBox)
                {
                    if (pictureBox.Tag is Int32 pageIndex)
                    {
                        SelectedPageIndex = pageIndex;
                        LoadPageImage();
                    }
                }
            }
            catch (Exception) { }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            picturePanel.AddScale(1.1, null);
            picturePanel.Invalidate();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            picturePanel.AddScale(0.9, null);
            picturePanel.Invalidate();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            picturePanel.Cursor = Cursors.Hand;
            btnMove.Checked = true;
            btnSelectRegion.Checked = false;
            picturePanel.Mode = PlanMode.PM_MOVE;
            pdfRegionPicker.Mode = PdfRegionPicker.PdfRegionPickerMode.DPM_NONE;
            picturePanel.StartPoint = null;
            picturePanel.EndPoint = null;
            picturePanel.Invalidate();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelectRegion_Click(object sender, EventArgs e)
        {
            btnMove.Checked = false;
            btnSelectRegion.Checked = true;
            picturePanel.Mode = PlanMode.PM_PLANNER_DRAG;
            pdfRegionPicker.Mode = PdfRegionPicker.PdfRegionPickerMode.DPM_PICK_REGION;
            picturePanel.Invalidate();
        }

        private void pdfRegionPicker_RegionPicked(object sender, EventArgs e)
        {
            this.lblActualSize.Text = this.PixelWidth + " x " + this.PixelHeight + " px";
        }

        private void sliderSize_Scroll(object sender, EventArgs e)
        {
            this.lblActualSize.Text = this.PixelWidth + " x " + this.PixelHeight + " px";
        }


    }
}