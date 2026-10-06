using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RevitQAQC.Core.Checks;
using RevitQAQC.Core.Services;
using RevitQAQC.Engine.Processors;
using RevitQAQC.Engine.Reports;
using RevitQAQC.Engine.Services;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;
using RevitQAQC.WPF.Views;
using RevitQAQC.Addin.Services;

using System.Collections.Generic;

namespace RevitQAQC.Addin.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class RunQAQCCommand : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            Document doc =
                commandData.Application.ActiveUIDocument.Document;

            var checks = new List<IQACheck>()
            {
                new MissingMarkParameterCheck(),
                new MissingCommentsCheck(),
                new ElementCountCheck(),
                new DuplicateMarkValueCheck(),
                new WrongLevelAssignmentCheck(),
                new ModelStandardsCheck(),
                new NamingConventionCheck()
            };

            var engine = new QAEngine(checks);

            var results = engine.Run(doc);

            var processor = new ResultsProcessor();

            ReportModel report =
                processor.CreateReport(
                    doc.Title,
                    results);

            var jsonGenerator =
                new JsonReportGenerator();

            jsonGenerator.GenerateReport(
                report,
                @"C:\Temp\QAQC_Report.json");

            var pdfGenerator =
                new PdfReportGenerator();

            pdfGenerator.Generate(
                report,
                @"C:\Temp\QAQC_Report.pdf");

            // =====================================================
            // CREATE SELECTION SERVICE WHILE INSIDE REVIT
            // =====================================================

            ElementSelectionService selectionService =
            new ElementSelectionService();

            ElementRemediationService remediationService =
                new ElementRemediationService();

            QAScanService scanService =
                new QAScanService();

            MainWindow mainWindow = new MainWindow(
                report,
                selectionService,
                remediationService,
                scanService);

            mainWindow.Show();

            return Result.Succeeded;
        }
    }
}