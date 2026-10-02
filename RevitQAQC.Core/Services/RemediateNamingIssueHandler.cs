using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitQAQC.Core.Services
{
    public class RemediateNamingIssueHandler : IExternalEventHandler
    {
        public long ElementId { get; set; }

        public string SuggestedValue { get; set; } = "";

        public Action<bool> CompletionCallback { get; set; }

        public void Execute(UIApplication app)
        {
            try
            {
                UIDocument uiDoc = app.ActiveUIDocument;

                if (uiDoc == null)
                    return;

                Document doc = uiDoc.Document;

                ElementId id = new ElementId(ElementId);

                Element element = doc.GetElement(id);

                if (element == null)
                {
                    TaskDialog.Show(
                        "Element Not Found",
                        $"Element {ElementId} could not be found.");

                    return;
                }

                Parameter markParam =
                    element.LookupParameter("Mark");

                if (markParam == null)
                {
                    TaskDialog.Show(
                        "Remediation Failed",
                        $"Element {ElementId} does not have a Mark parameter.");

                    return;
                }

                if (markParam.IsReadOnly)
                {
                    TaskDialog.Show(
                        "Remediation Failed",
                        $"The Mark parameter of element {ElementId} is read-only.");

                    return;
                }

                if (string.IsNullOrWhiteSpace(SuggestedValue))
                {
                    TaskDialog.Show(
                        "Remediation Failed",
                        "No suggested naming value was provided.");

                    return;
                }

                using (Transaction transaction =
                    new Transaction(
                        doc,
                        "QAQC - Remediate Naming Convention"))
                {
                    transaction.Start();

                    markParam.Set(SuggestedValue);

                    transaction.Commit();

                    CompletionCallback?.Invoke(true);
                }

                TaskDialog.Show(
                    "Remediation Successful",
                    $"Element {ElementId} has been renamed to:\n\n{SuggestedValue}");
            }
            catch (Exception ex)
            {
                CompletionCallback?.Invoke(false);
                TaskDialog.Show(
                    "Remediation Error",
                    $"Unable to remediate element.\n\n{ex.Message}");
            }
        }


        public string GetName()
        {
            return "Remediate QAQC Naming Issue";
        }
    }
}