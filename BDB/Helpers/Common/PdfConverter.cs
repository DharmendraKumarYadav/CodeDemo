using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;

namespace BDB.Helpers.Common
{
    public interface IPdfConverterService
    {
        void CreatePdf(string htmlString);
    }
    public class PdfConverterService : IPdfConverterService
    {
        private IConverter _converter;
        public PdfConverterService(IConverter converter)
        {
            _converter = converter;
        }

        public void CreatePdf(string htmlString)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = "./wwwroot/Booking/Order.pdf"

            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            _converter.Convert(pdf);
        }
    }
}
