using System;
using System.Drawing;
using PdfiumViewer;

namespace Europlan.Common.PDF
{
    internal static class PdfHelper
    {


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

        internal static System.Drawing.Bitmap GetPageBitmap(String pdfFilePath, Int32 pageIndex, Int32 dpi)
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

        internal static Rectangle GetPageTrimBox(String pdfFilePath, Int32 pageIndex)
        {

            try
            {
                using (var pdfDocument = PdfDocument.Load(pdfFilePath))
                {
                    var pdfPageSize = pdfDocument.PageSizes[pageIndex];
                    var rectangle = new Rectangle(0, 0, Convert.ToInt32(pdfPageSize.Width), Convert.ToInt32(pdfPageSize.Height));
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
