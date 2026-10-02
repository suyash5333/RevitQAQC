using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace RevitQAQC.Core.Services
{
    public class SelectElementHandler : IExternalEventHandler
    {
        public long ElementId { get; set; }

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

                uiDoc.Selection.SetElementIds(
                    new[] { id });
            }
            catch (Exception ex)
            {
                TaskDialog.Show(
                    "Selection Error",
                    $"Unable to select element.\n\n{ex.Message}");
            }
        }

        public string GetName()
        {
            return "Select QAQC Element";
        }
    }
}