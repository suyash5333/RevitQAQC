using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;
using RevitQAQC.Core.Services;
using System.Collections.Generic;

namespace RevitQAQC.Core.Checks
{
    public class NamingConventionCheck : IQACheck
    {
        public string CheckName =>
            "Naming Convention Check";

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

            // Store every existing Mark exactly as it exists in the model.
            var usedMarks = new HashSet<string>();

            foreach (var element in elements)
            {
                if (element.Category == null)
                    continue;

                if (element.Category.Name != standard.Category)
                    continue;

                Parameter markParameter =
                    element.LookupParameter("Mark");

                string mark =
                    markParameter?.AsString() ?? "";

                if (!string.IsNullOrWhiteSpace(mark))
                {
                    usedMarks.Add(mark);
                }
            }

            // Validate each wall.
            foreach (var element in elements)
            {
                if (element.Category == null)
                    continue;

                if (element.Category.Name != standard.Category)
                    continue;

                Parameter markParameter =
                    element.LookupParameter("Mark");

                string mark =
                    markParameter?.AsString() ?? "";

                Level level =
                    doc.GetElement(element.LevelId) as Level;

                string levelName =
                    level?.Name ?? "Level 1";

                // Create a copy so validation of this element
                // cannot corrupt the master list of existing Marks.
                var marksForValidation =
                    new HashSet<string>(usedMarks);

                // The element's own Mark must not be considered
                // a duplicate of itself.
                if (!string.IsNullOrWhiteSpace(mark))
                {
                    marksForValidation.Remove(mark);
                }

                QAIssue? issue =
                    validator.Validate(
                        element.Id.Value,
                        element.Category.Name,
                        mark,
                        levelName,
                        standard,
                        marksForValidation);

                if (issue != null)
                {
                    issues.Add(issue);

                    // Reserve the suggested Mark so future
                    // suggestions don't reuse it.
                    if (!string.IsNullOrWhiteSpace(
                        issue.SuggestedValue))
                    {
                        usedMarks.Add(
                            issue.SuggestedValue);
                    }
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