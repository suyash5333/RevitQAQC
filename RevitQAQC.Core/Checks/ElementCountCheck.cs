using RevitQAQC.Interfaces.Checks;
using Autodesk.Revit.DB;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Core.Checks
{
    public class ElementCountCheck : IQACheck
    {
        public string CheckName => "Element Count";

        public string Description => "Counts model elements.";

        public CheckResult Execute(Document doc)
        {
            int elementCount = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .GetElementCount();

            return new CheckResult
            {
                CheckName = CheckName,
                IsPass = elementCount > 0,
                Message = $"Model contains {elementCount} elements.",
                IssueCount = 0,
                value = elementCount
            };
        }
    }
}