using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitQAQC.Core.Services
{
    public class RemediateAllNamingIssuesHandler : IExternalEventHandler
    {
        public List<long> ElementIds { get; set; } = new();

        public List<string> SuggestedValues { get; set; } = new();

        public Action<int> CompletionCallback { get; set; }

        public void Execute(UIApplication app)
        {
            try
            {
                UIDocument uiDoc = app.ActiveUIDocument;

                if (uiDoc == null)
                    return;

                Document doc = uiDoc.Document;

                int fixedCount = 0;

                using (Transaction transaction =
                    new Transaction(
                        doc,
                        "QAQC - Remediate All Naming Issues"))
                {
                    transaction.Start();

                    for (int i = 0;
                         i < ElementIds.Count && i < SuggestedValues.Count;
                         i++)
                    {
                        if (string.IsNullOrWhiteSpace(
                            SuggestedValues[i]))
                        {
                            continue;
                        }

                        ElementId id =
                            new ElementId(ElementIds[i]);

                        Element element =
                            doc.GetElement(id);

                        if (element == null)
                            continue;

                        Parameter markParam =
                            element.LookupParameter("Mark");

                        if (markParam == null ||
                            markParam.IsReadOnly)
                        {
                            continue;
                        }

                        markParam.Set(
                            SuggestedValues[i]);

                        fixedCount++;
                    }

                    transaction.Commit();
                }

                CompletionCallback?.Invoke(fixedCount);

                TaskDialog.Show(
                    "QAQC Remediation",
                    $"{fixedCount} naming convention issue(s) fixed.");
            }
            catch (Exception ex)
            {
                TaskDialog.Show(
                    "Remediation Error",
                    $"Unable to fix naming convention issues.\n\n{ex.Message}");

                CompletionCallback?.Invoke(0);
            }
        }

        public string GetName()
        {
            return "Remediate All QAQC Naming Issues";
        }
    }
}