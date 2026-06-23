using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Core.Checks
{
    public class MissingCommentsCheck : IQACheck
    {
        public string CheckName => "Missing Comments";

        public string Description => "Checks elements with empty Comments.";

        public CheckResult Execute(Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            int missingCommentsCount = 0;

            foreach (var element in elements)
            {
                Parameter commentsParam =
                    element.LookupParameter("Comments");

                if (commentsParam == null)
                {
                    missingCommentsCount++;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(commentsParam.AsString()))
                {
                    missingCommentsCount++;
                }
            }

            return new CheckResult
            {
                CheckName = CheckName,
                IsPass = missingCommentsCount == 0,
                Message = missingCommentsCount == 0
                    ? "All elements have valid Comments values."
                    : $"{missingCommentsCount} elements have missing or empty Comments.",
                IssueCount = missingCommentsCount
            };
        }
    }
}