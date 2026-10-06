using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using RevitQAQC.Core.Checks;
using RevitQAQC.Engine.Processors;
using RevitQAQC.Engine.Services;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Interfaces.Services;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Addin.Services
{
    public class QAScanService : IQAScanService
    {
        private readonly QAScanHandler _handler;
        private readonly ExternalEvent _externalEvent;

        public QAScanService()
        {
            _handler = new QAScanHandler();
            _externalEvent = ExternalEvent.Create(_handler);
        }

        public void RunScan(Action<ReportModel> completionCallback)
        {
            _handler.CompletionCallback = completionCallback;
            _externalEvent.Raise();
        }

        public void Dispose()
        {
            _externalEvent?.Dispose();
        }
    }

    public class QAScanHandler : IExternalEventHandler
    {
        public Action<ReportModel> CompletionCallback { get; set; }

        public void Execute(UIApplication app)
        {
            try
            {
                UIDocument uiDoc = app.ActiveUIDocument;

                if (uiDoc == null)
                    return;

                Document doc = uiDoc.Document;

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

                CompletionCallback?.Invoke(report);
            }
            catch (Exception ex)
            {
                TaskDialog.Show(
                    "QAQC Scan Error",
                    $"Unable to run QAQC scan.\n\n{ex.Message}");
            }
        }

        public string GetName()
        {
            return "Run QAQC Scan";
        }
    }
}