using System;
using System.IO;
using SelectPdf;

namespace LogItUpApi.Shared
{
    public class HtmlToPdfGenerator
    {
        public MemoryStream GeneratePdf(string url, HtmlToPdfConfig config)
        {
            MemoryStream result = new MemoryStream();

            HtmlToPdf converter = new HtmlToPdf();

            SetConfig(config, ref converter);

            PdfDocument doc = converter.ConvertUrl(url);

            doc.Save(result);

            doc.Close();

            return result;
        }

        private void SetConfig(HtmlToPdfConfig config, ref HtmlToPdf converter)
        {
            converter.Options.PdfPageSize = config.PdfPageSize;
            converter.Options.PdfPageOrientation = config.PdfPageOrientation;
            converter.Options.WebPageWidth = config.WebPageWidth;
            converter.Options.WebPageHeight = config.WebPageHeight;
            converter.Options.MarginBottom = config.MarginBottom;
            converter.Options.MarginTop = config.MarginTop;
            converter.Options.MarginLeft = config.MarginLeft;
            converter.Options.MarginRight = config.MarginRight;
            converter.Options.AutoFitWidth = config.AutoFitWidth;
        }
    }

    public class HtmlToPdfConfig
    {
        public PdfPageSize PdfPageSize { get; set; }
        public PdfPageOrientation PdfPageOrientation { get; set; }
        public HtmlToPdfPageFitMode AutoFitWidth { get; set; }
        public int WebPageWidth { get; set; }
        public int WebPageHeight { get; set; }
        public int MarginBottom { get; set; }
        public int MarginTop { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }

        public HtmlToPdfConfig()
        {
            PdfPageSize = PdfPageSize.A4;
            PdfPageOrientation = PdfPageOrientation.Portrait;
            WebPageWidth = 0;
            WebPageHeight = 0;
            MarginBottom = 10;
            MarginTop = 10;
            MarginLeft = 10;
            MarginRight = 10;
            AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
        }
    }

}
