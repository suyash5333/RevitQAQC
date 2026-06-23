using PdfSharp.Drawing;
using PdfSharp.Pdf;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Engine.Reports
{
    public class PdfReportGenerator
    {
        public void Generate(ReportModel report, string filePath)
        {
            PdfDocument document = new PdfDocument();

            document.Info.Title = "Revit QAQC Report";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new XFont("Arial", 18);
            XFont headingFont = new XFont("Arial", 14);
            XFont bodyFont = new XFont("Arial", 10);

            double y = 40;

            gfx.DrawString(
                "Revit QAQC Report",
                titleFont,
                XBrushes.Black,
                new XPoint(40, y));

            y += 40;

            gfx.DrawString(
                $"Project: {report.ProjectName}",
                bodyFont,
                XBrushes.Black,
                new XPoint(40, y));

            y += 20;

            gfx.DrawString(
                $"Generated On: {report.GeneratedOn}",
                bodyFont,
                XBrushes.Black,
                new XPoint(40, y));

            y += 30;

            gfx.DrawString(
                "Health Score",
                headingFont,
                XBrushes.Black,
                new XPoint(40, y));

            y += 25;

            gfx.DrawString(
                $"Score: {report.HealthScore.Score:F2}",
                bodyFont,
                XBrushes.Black,
                new XPoint(50, y));

            y += 20;

            gfx.DrawString(
                $"Grade: {report.HealthScore.Grade}",
                bodyFont,
                XBrushes.Black,
                new XPoint(50, y));

            y += 35;

            gfx.DrawString(
                "Check Results",
                headingFont,
                XBrushes.Black,
                new XPoint(40, y));

            y += 25;

            foreach (var result in report.CheckResults)
            {
                string status = result.IsPass ? "PASS" : "FAIL";

                gfx.DrawString(
                    $"{result.CheckName} - {status}",
                    bodyFont,
                    XBrushes.Black,
                    new XPoint(50, y));

                y += 15;

                gfx.DrawString(
                    $"Issues: {result.IssueCount}",
                    bodyFont,
                    XBrushes.Black,
                    new XPoint(70, y));

                y += 15;

                gfx.DrawString(
                    result.Message,
                    bodyFont,
                    XBrushes.Black,
                    new XPoint(70, y));

                y += 25;
            }

            document.Save(filePath);
        }
    }
}