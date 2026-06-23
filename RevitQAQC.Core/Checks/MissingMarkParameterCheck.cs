using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Core.Checks
{
    public class MissingMarkParameterCheck : IQACheck
    {
        public string CheckName => "Missing Mark Parameter";

        public string Description => "Checks elements that have empty Mark values.";

        public CheckResult Execute(Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            int missingMarkCount = 0;

            foreach (var element in elements)
            {
                Parameter markParam =
                    element.LookupParameter("Mark");

                if (markParam == null)
                {
                    missingMarkCount++;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(markParam.AsString()))
                {
                    missingMarkCount++;
                }
            }

            return new CheckResult
            {
                CheckName = CheckName,
                IsPass = missingMarkCount == 0,
                Message = missingMarkCount == 0
                    ? "All elements have valid Mark values."
                    : $"{missingMarkCount} elements have missing or empty Mark values.",
                IssueCount = missingMarkCount
            };
        }
    }
}