using System;
using System.Drawing;
using System.IO;
using PdfiumViewer;

namespace Europlan.Common.PDF
{
    internal class PdfHelper : IDisposable
    {


        private const Int32 PDF_PT_PER_INCH = 72;


        internal static void DrawToHDC(String pdfFilePath, Int32 pageIndex, Graphics graphics, Int32 dpi)
        {
            try
            {
                using (var pdfDocument = PdfDocument.Load(pdfFilePath))
                {
                    var image = pdfDocument.Render(pageIndex, dpi, dpi, PdfRenderFlags.None);
                    graphics.DrawImage(image, 0, 0);
                }
            }
            catch (Exception ex) { throw new ExceptionPdfHelperDrawToHDC(ex, pdfFilePath, pageIndex); }
        }

        internal static Bitmap GetPageBitmap(String pdfFilePath, Int32 pageIndex, Int32 dpi)
        {
            try
            {
                using (var pdfDocument = PdfDocument.Load(pdfFilePath))
                {
                    var image = pdfDocument.Render(pageIndex, dpi, dpi, PdfRenderFlags.None);
                    var bitmap = new Bitmap(image);
                    return bitmap;
                }
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageBitmap(ex, pdfFilePath, pageIndex); }
        }

        internal static Bitmap GetPageBitmap(String pdfFilePath, Int32 pageIndex, Int32 pageWidth, Int32 pageHeight, Int32 dpi)
        {
            try
            {
                using (var pdfDocument = PdfDocument.Load(pdfFilePath))
                {
                    var image = pdfDocument.Render(pageIndex, pageWidth, pageHeight, dpi, dpi, PdfRenderFlags.None);
                    var bitmap = new Bitmap(image);
                    return bitmap;
                }
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageBitmap(ex, pdfFilePath, pageIndex); }
        }

        internal static Int32 GetPageCount(String pdfFilePath)
        {
            try
            {
                using (var pdfDocument = PdfDocument.Load(pdfFilePath))
                {
                    var pageCount = pdfDocument.PageCount;
                    return pageCount;
                }
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageCount(ex, pdfFilePath); }
        }


        private PdfDocument PdfDocument;
        private String PdfFilePath;


        public PdfHelper(String pdfFilePath)
        {
            PdfDocument = PdfDocument.Load(pdfFilePath);
            PdfFilePath = pdfFilePath;
        }


        public void Dispose()
        {
            PdfDocument.Dispose();
        }

        internal void DrawToHDC(Int32 pageIndex, Graphics graphics, Int32 dpi)
        {
            try
            {
                var image = PdfDocument.Render(pageIndex, dpi, dpi, PdfRenderFlags.None);
                graphics.DrawImage(image, 0, 0);
            }
            catch (Exception ex) { throw new ExceptionPdfHelperDrawToHDC(ex, PdfFilePath, pageIndex); }
        }

        internal Bitmap GetPageBitmap(Int32 pageIndex, Int32 dpi)
        {
            try
            {
                var image = PdfDocument.Render(pageIndex, dpi, dpi, PdfRenderFlags.None);
                var bitmap = new Bitmap(image);
                return bitmap;
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageBitmap(ex, PdfFilePath, pageIndex); }
        }

        internal Bitmap GetPageBitmap(Int32 pageIndex, Int32 pageWidth, Int32 pageHeight, Int32 dpi)
        {
            try
            {
                var image = PdfDocument.Render(pageIndex, pageWidth, pageHeight, dpi, dpi, PdfRenderFlags.None);
                var bitmap = new Bitmap(image);
                return bitmap;
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageBitmap(ex, PdfFilePath, pageIndex); }
        }

        internal Int32 GetPageCount()
        {
            try
            {
                var pageCount = PdfDocument.PageCount;
                return pageCount;
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageCount(ex, PdfFilePath); }
        }

        internal Rectangle GetPageSize(Int32 pageIndex)
        {
            try
            {
                var pdfPage = PdfDocument.PageSizes[pageIndex];
                var rectangle = new System.Drawing.Rectangle(0, 0, Convert.ToInt32(pdfPage.Width), Convert.ToInt32(pdfPage.Height));
                return rectangle;
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageBitmap(ex, PdfFilePath, pageIndex); }
        }

        internal void SaveToPng(Int32 pageIndex, String filePath, Rectangle region, Int32 dpi)
        {
            var originalBitmap = PdfDocument.Render(pageIndex, dpi, dpi, PdfRenderFlags.CorrectFromDpi);
            var transformedRegionLeft = Convert.ToInt32(region.Left * dpi / PDF_PT_PER_INCH);
            var transformedRegionTop = Convert.ToInt32(region.Top * dpi / PDF_PT_PER_INCH);
            var transformedRegionWidth = Convert.ToInt32(region.Width * dpi / PDF_PT_PER_INCH);
            var transformedRegionHeight = Convert.ToInt32(region.Height * dpi / PDF_PT_PER_INCH);
            var transformedRegion = new Rectangle(transformedRegionLeft, transformedRegionTop, transformedRegionWidth, transformedRegionHeight);
            using (var transformedBitmap = new Bitmap(transformedRegion.Width, transformedRegion.Height))
            {
                using (var graphics = Graphics.FromImage(transformedBitmap))
                {
                    graphics.DrawImage(originalBitmap, 0, 0, transformedRegion, GraphicsUnit.Pixel);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    var directoryName = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    transformedBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }


        internal abstract class ExceptionPdfHelper : Exception
        {

            public String PdfFilePath { get; private set; }

            protected ExceptionPdfHelper(String pdfFilePath, String messageTemplate, params Object[] parameters) : base(String.Format(messageTemplate, parameters))
            {
                PdfFilePath = pdfFilePath;
            }

            protected ExceptionPdfHelper(Exception innerException, String pdfFilePath, String messageTemplate, params Object[] parameters) : base(String.Format(messageTemplate, parameters), innerException)
            {
                PdfFilePath = pdfFilePath;
            }

        }

        internal abstract class ExceptionPdfHelperPage : ExceptionPdfHelper
        {

            public Int32 PageNumber { get; private set; }

            protected ExceptionPdfHelperPage(String pdfFilePath, Int32 pageIndex, String messageTemplate, params Object[] parameters) : base(pdfFilePath, String.Format(messageTemplate, parameters))
            {
                PageNumber = pageIndex + 1;
            }

            protected ExceptionPdfHelperPage(Exception innerException, String pdfFilePath, Int32 pageIndex, String messageTemplate, params Object[] parameters) : base(pdfFilePath, String.Format(messageTemplate, parameters), innerException)
            {
                PageNumber = pageIndex + 1;
            }

        }

        internal class ExceptionPdfHelperGetPageBitmap : ExceptionPdfHelperPage
        {

            private const String MESSAGE_TEMPLATE = "Beim Umwandeln der PDF-Seite {0} in eine Grafik ist ein Fehler aufgetreten";

            internal ExceptionPdfHelperGetPageBitmap(Exception innerException, String pdfFilePath, Int32 pageIndex)
                : base(innerException, pdfFilePath, pageIndex, String.Format(MESSAGE_TEMPLATE, pageIndex + 1), innerException)
            {
            }

        }

        internal class ExceptionPdfHelperGetPageCount : ExceptionPdfHelper
        {

            private const String MESSAGE_TEMPLATE = "Beim Ermitteln der Anzahl der Seites der PDF-Datei ist ein Fehler aufgetreten";

            internal ExceptionPdfHelperGetPageCount(Exception innerException, String pdfFilePath)
                : base(innerException, pdfFilePath, String.Format(MESSAGE_TEMPLATE), innerException)
            {
            }

        }

        internal class ExceptionPdfHelperDrawToHDC : ExceptionPdfHelperPage
        {

            private const String MESSAGE_TEMPLATE = "Beim Übertragen der PDF-Seite {0} in eine Grafik ist ein Fehler aufgetreten";

            internal ExceptionPdfHelperDrawToHDC(Exception innerException, String pdfFilePath, Int32 pageIndex)
                : base(innerException, pdfFilePath, pageIndex, String.Format(MESSAGE_TEMPLATE, pageIndex + 1), innerException)
            {
            }

        }


    }
}
