using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Core.Checks
{
    public class WrongLevelAssignmentCheck : IQACheck
    {
        public string CheckName => "Level Assignment Check";

        public string Description =>
            "Checks whether elements have a valid level assignment.";

        public CheckResult Execute(Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            int invalidLevelCount = 0;

            foreach (var element in elements)
            {
                Level level = doc.GetElement(element.LevelId) as Level;

                if (level == null)
                {
                    invalidLevelCount++;
                }
            }

            return new CheckResult
            {
                CheckName = CheckName,
                IsPass = invalidLevelCount == 0,
                Message = invalidLevelCount == 0
                    ? "All elements have valid level assignments."
                    : $"{invalidLevelCount} elements have invalid level assignments.",
                IssueCount = invalidLevelCount
            };
        }
    }
}