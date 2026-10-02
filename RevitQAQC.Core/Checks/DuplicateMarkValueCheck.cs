using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;
using System.Collections.Generic;

namespace RevitQAQC.Core.Checks
{
    public class DuplicateMarkValueCheck : IQACheck
    {
        public string CheckName => "Duplicate Mark Values";

        public string Description => "Checks for duplicate Mark parameter values.";

        public CheckResult Execute(Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            var usedMarks = new HashSet<string>();
            var issues = new List<QAIssue>();

            foreach (var element in elements)
            {
                Parameter markParam = element.LookupParameter("Mark");

                if (markParam == null)
                    continue;

                string markValue = markParam.AsString();

                if (string.IsNullOrWhiteSpace(markValue))
                    continue;

                if (usedMarks.Contains(markValue))
                {
                    issues.Add(new QAIssue
                    {
                        ElementId = element.Id.Value,
                        Category = element.Category?.Name ?? "Unknown",
                        CurrentValue = markValue,
                        Problem = "Duplicate Mark value",
                        SuggestedValue = markValue,
                        Severity = "Major"
                    });
                }
                else
                {
                    usedMarks.Add(markValue);
                }
            }

            return new CheckResult
            {
                CheckName = CheckName,
                IsPass = issues.Count == 0,
                Message = issues.Count == 0
                    ? "No duplicate Mark values found."
                    : $"{issues.Count} duplicate Mark values found.",
                IssueCount = issues.Count,
                Issues = issues
            };
        }
    }
}