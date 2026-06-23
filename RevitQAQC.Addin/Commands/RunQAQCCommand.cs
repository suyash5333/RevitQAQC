using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

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
            TaskDialog.Show(
                "QAQC",
                "RunQAQCCommand executed successfully.");

            return Result.Succeeded;
        }
    }
}