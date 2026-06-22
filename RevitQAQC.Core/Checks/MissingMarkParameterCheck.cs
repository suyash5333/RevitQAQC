using RevitQAQC.Interfaces.Checks;
using Autodesk.Revit.DB;

namespace RevitQAQC.Core.Checks
{
    public class MissingMarkParameterCheck : IQACheck
    {
        public string CheckName => "Missing Mark Parameter";

        public string Description => "Checks elements that have empty Mark values.";

        public bool Execute(Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            foreach (var element in elements)
            {
                Parameter markParam =
                    element.LookupParameter("Mark");

                if (markParam == null)
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(markParam.AsString()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}