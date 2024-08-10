using System;
using Patagames.Pdf.Net;

namespace Europlan.Common.PDF
{
    internal static class PdfHelper
    {


        internal static void DrawToHDC(String pdfFilePath, Int32 pageIndex,System.Drawing.Graphics graphics)
        {
            try
            {
                using (var pdfDocument = PdfDocument.Load(pdfFilePath))
                {
                    var pdfPage = pdfDocument.Pages[pageIndex];
                    var pdfPageWidth = Convert.ToInt32(pdfPage.Width);
                    var pdfPageHeight = Convert.ToInt32(pdfPage.Height);
                    var bitmap = new System.Drawing.Bitmap(pdfPageWidth, pdfPageHeight);
                    var rect = new System.Drawing.Rectangle(0, 0, pdfPageWidth, pdfPageHeight);
                    var rotate = Patagames.Pdf.Enums.PageRotate.Normal;
                    var renderFlags = Patagames.Pdf.Enums.RenderFlags.FPDF_NONE;
                    pdfPage.Render(graphics, rect, rotate, renderFlags);
                }
            }
            catch (Exception ex) { throw new __ex3(ex, pdfFilePath, pageIndex); }
        }

        internal static System.Drawing.Bitmap GetPageBitmap(String pdfFilePath, Int32 pageIndex)
        {
            try
            {
                using (var pdfDocument = PdfDocument.Load(pdfFilePath))
                {
                    var pdfPage = pdfDocument.Pages[pageIndex];
                    var pdfPageWidth = Convert.ToInt32(pdfPage.Width);
                    var pdfPageHeight = Convert.ToInt32(pdfPage.Height);
                    var bitmap = new System.Drawing.Bitmap(pdfPageWidth, pdfPageHeight);
                    var rect = new System.Drawing.Rectangle(0, 0, pdfPageWidth, pdfPageHeight);
                    var graphics = System.Drawing.Graphics.FromImage(bitmap);
                    var rotate = Patagames.Pdf.Enums.PageRotate.Normal;
                    var renderFlags = Patagames.Pdf.Enums.RenderFlags.FPDF_NONE;
                    pdfPage.Render(graphics, rect, rotate, renderFlags);
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
                    var pageCount = pdfDocument.Pages.Count;
                    return pageCount;
                }
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageCount(ex, pdfFilePath); }
        }

        internal static System.Drawing.Rectangle GetPageTrimBox(String pdfFilePath, Int32 pageIndex)
        {
            try
            {
                using (var pdfDocument = PdfDocument.Load(pdfFilePath))
                {
                    var pdfPage = pdfDocument.Pages[pageIndex];
                    var pdfTrimBox = pdfPage.TrimBox;
                    var rectangle = new System.Drawing.Rectangle(Convert.ToInt32(pdfTrimBox.left), Convert.ToInt32(pdfTrimBox.top), Convert.ToInt32(pdfTrimBox.Width), Convert.ToInt32(pdfTrimBox.Height));
                    return rectangle;
                }
            }
            catch (Exception ex) { throw new ExceptionPdfHelperGetPageBitmap(ex, pdfFilePath, pageIndex); }
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

        internal class __ex3 : ExceptionPdfHelperPage
        {

            private const String MESSAGE_TEMPLATE = "Beim Übertragen der PDF-Seite {0} in eine Grafik ist ein Fehler aufgetreten";

            internal __ex3(Exception innerException, String pdfFilePath, Int32 pageIndex)
                : base(innerException, pdfFilePath, pageIndex, String.Format(MESSAGE_TEMPLATE, pageIndex + 1), innerException)
            {
            }

        }


    }
}
