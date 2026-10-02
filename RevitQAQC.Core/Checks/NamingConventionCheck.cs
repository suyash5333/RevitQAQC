using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;
using RevitQAQC.Core.Services;
using System.Collections.Generic;

namespace RevitQAQC.Core.Checks
{
    public class NamingConventionCheck : IQACheck
    {
        public string CheckName => "Naming Convention Check";

        public string Description =>
            "Checks wall element Marks against the configured naming convention.";

        public CheckResult Execute(Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            var issues = new List<QAIssue>();

            var standard = new NamingStandard
            {
                Category = "Walls",
                Prefix = "WALL",
                LevelPrefix = "L",
                LevelDigits = 2,
                NumberDigits = 3,
                Separator = "-"
            };

            var validator = new NamingConventionValidator();

            // First collect existing Marks
            var usedMarks = new HashSet<string>();

            foreach (var element in elements)
            {
                if (element.Category == null)
                    continue;

                if (element.Category.Name != standard.Category)
                    continue;

                Parameter markParam =
                    element.LookupParameter("Mark");

                string mark =
                    markParam?.AsString() ?? "";

                if (!string.IsNullOrWhiteSpace(mark))
                {
                    usedMarks.Add(mark);
                }
            }

            // Now validate each wall
            foreach (var element in elements)
            {
                if (element.Category == null)
                    continue;

                if (element.Category.Name != standard.Category)
                    continue;

                Parameter markParam =
                    element.LookupParameter("Mark");

                string mark =
                    markParam?.AsString() ?? "";

                Level level =
                    doc.GetElement(element.LevelId) as Level;

                string levelName =
                    level?.Name ?? "Level 1";

                // Temporarily remove the current Mark
                // so it doesn't get detected as its own duplicate.
                if (!string.IsNullOrWhiteSpace(mark))
                {
                    usedMarks.Remove(mark);
                }

                QAIssue? issue = validator.Validate(
                    element.Id.Value,
                    element.Category.Name,
                    mark,
                    levelName,
                    standard,
                    usedMarks);

                if (issue != null)
                {
                    issues.Add(issue);
                }

                // Add the current Mark back
                if (!string.IsNullOrWhiteSpace(mark))
                {
                    usedMarks.Add(mark);
                }
            }

            return new CheckResult
            {
                CheckName = CheckName,
                IsPass = issues.Count == 0,
                Message = issues.Count == 0
                    ? "All wall naming conventions passed."
                    : $"{issues.Count} wall naming convention issues found.",
                IssueCount = issues.Count,
                Issues = issues
            };
        }
    }
}