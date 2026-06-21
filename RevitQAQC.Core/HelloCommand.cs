using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitQAQC.Core
{
    [Transaction(TransactionMode.Manual)]
    public class HelloCommand : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            TaskDialog.Show("Revit QAQC", "Hello from Revit 2026!");

            return Result.Succeeded;
        }
    }
}