using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestPDF.Models;

namespace TestPDF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> MergePDF()
        {
            // Get some file names
            string[] files = new string[2];
            files[0] = "/WORK-20200630T070648Z-001/PDFSHARP/code/TestPDF/wwwroot/pdf/1.pdf";
            files[1] = "/WORK-20200630T070648Z-001/PDFSHARP/code/TestPDF/wwwroot/pdf/2.pdf";
            PdfDocument PDFJudul = PdfReader.Open(files[0], PdfDocumentOpenMode.Import);
            PdfDocument PDFBab1 = PdfReader.Open(files[1], PdfDocumentOpenMode.Import);
            // Open the output document
            PdfDocument outputDocument = new PdfDocument();

            // For checking the file size uncomment next line.
            //outputDocument.Options.CompressContentStreams = true;


            // Iterate files
            foreach (string file in files)
            {
                // Open the document to import pages from it.
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
               
                // Show consecutive pages facing. Requires Acrobat 5 or higher.
                outputDocument.PageLayout = PdfPageLayout.TwoColumnLeft;
                var MSAddOn = new MemoryStream();
                //Add Page Untuk Keterangan Judul
                if (file==files[0])
                {
                    // Create a new PDF document
                    PdfDocument document = new PdfDocument();
                    document.Info.Title = "Judul Laporan AKhir Mahasiswa XXX";

                    // Create an empty page
                    PdfPage PageJudul = document.AddPage();

                    // Get an XGraphics object for drawing
                    XGraphics gfx = XGraphics.FromPdfPage(PageJudul);

                    // Create a font
                    XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

                    // Draw the text
                    gfx.DrawString("Bagian Judul Laporan Akhir Mahasiswa XXX", font, XBrushes.Black,
                      new XRect(0, 0, PageJudul.Width, PageJudul.Height),
                      XStringFormats.Center);

                    document.Save(MSAddOn);

                    PdfDocument AddDocument = PdfReader.Open(MSAddOn, PdfDocumentOpenMode.Import);
                    outputDocument.AddPage(AddDocument.Pages[0]);
                }
                //Add Page Untuk Keterangan Bab1
                if (file == files[1])
                {
                    // Create a new PDF document
                    PdfDocument document = new PdfDocument();
                    document.Info.Title = "Bagian Bab 1 Laporan Akhir Mahasiswa XXX";

                    // Create an empty page
                    PdfPage PageBab1 = document.AddPage();

                    // Get an XGraphics object for drawing
                    XGraphics gfx = XGraphics.FromPdfPage(PageBab1);

                    // Create a font
                    XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

                    // Draw the text
                    gfx.DrawString("Bagian Bab 1 Laporan Akhir Mahasiswa XXX", font, XBrushes.Black,
                      new XRect(0, 0, PageBab1.Width, PageBab1.Height),
                      XStringFormats.Center);
                    document.Save(MSAddOn);

                    PdfDocument AddDocument = PdfReader.Open(MSAddOn, PdfDocumentOpenMode.Import);
                    outputDocument.AddPage(AddDocument.Pages[0]);
                }
                // Iterate pages
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfPage page = inputDocument.Pages[idx];
                    outputDocument.AddPage(page);
                }
                int totalPages = outputDocument.Pages.Count;
                int currentPage = 0;
                foreach (PdfPage page in outputDocument.Pages)
                {
                    ++currentPage;
                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                        XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

                        // Draw the text
                        gfx.DrawString((currentPage).ToString(), font, XBrushes.Black,
                          new XRect(0, 0, page.Width, page.Height),
                          XStringFormats.BottomRight);
                        // Todo: write pagenumber using currentPage and totalPages
                    }
                }
            }
            var ms = new MemoryStream();
            outputDocument.Save(ms,true);

            return File(ms.ToArray(), "application/pdf","export.pdf");
        }
        public async Task<IActionResult> HelloWorld()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.BottomRight);

            // Save the document...
            const string filename = "HelloWorld.pdf";
            var ms = new MemoryStream();
            document.Save(ms, true);

            return File(ms.ToArray(), "application/pdf", "export.pdf");
        }
        //void DrawText(XGraphics gfx, int number)
        //{
        //    BeginBox(gfx, number, "Text Styles");

        //    const string facename = "Times New Roman";

        //    //XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
        //    XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.WinAnsi, PdfFontEmbedding.Default);

        //    XFont fontRegular = new XFont(facename, 20, XFontStyle.Regular, options);
        //    XFont fontBold = new XFont(facename, 20, XFontStyle.Bold, options);
        //    XFont fontItalic = new XFont(facename, 20, XFontStyle.Italic, options);
        //    XFont fontBoldItalic = new XFont(facename, 20, XFontStyle.BoldItalic, options);

        //    // The default alignment is baseline left (that differs from GDI+)
        //    gfx.DrawString("Times (regular)", fontRegular, XBrushes.DarkSlateGray, 0, 30);
        //    gfx.DrawString("Times (bold)", fontBold, XBrushes.DarkSlateGray, 0, 65);
        //    gfx.DrawString("Times (italic)", fontItalic, XBrushes.DarkSlateGray, 0, 100);
        //    gfx.DrawString("Times (bold italic)", fontBoldItalic, XBrushes.DarkSlateGray, 0, 135);

        //    EndBox(gfx);
        //}
        //public void BeginBox(XGraphics gfx, int number, string title)
        //{
        //    const int dEllipse = 15;
        //    XRect rect = new XRect(0, 20, 300, 200);
        //    if (number % 2 == 0)
        //        rect.X = 300 - 5;
        //    rect.Y = 40 + ((number - 1) / 2) * (200 - 5);
        //    rect.Inflate(-10, -10);
        //    XRect rect2 = rect;
        //    rect2.Offset(this.borderWidth, this.borderWidth);
        //    gfx.DrawRoundedRectangle(new XSolidBrush(this.shadowColor), rect2, new XSize(dEllipse + 8, dEllipse + 8));
        //    XLinearGradientBrush brush = new XLinearGradientBrush(rect, this.backColor, this.backColor2, XLinearGradientMode.Vertical);
        //    gfx.DrawRoundedRectangle(this.borderPen, brush, rect, new XSize(dEllipse, dEllipse));
        //    rect.Inflate(-5, -5);

        //    XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
        //    gfx.DrawString(title, font, XBrushes.Navy, rect, XStringFormats.TopCenter);

        //    rect.Inflate(-10, -5);
        //    rect.Y += 20;
        //    rect.Height -= 20;

        //    this.state = gfx.Save();
        //    gfx.TranslateTransform(rect.X, rect.Y);
        //}

        //public void EndBox(XGraphics gfx)
        //{
        //    gfx.Restore(this.state);
        //}
    }
}
