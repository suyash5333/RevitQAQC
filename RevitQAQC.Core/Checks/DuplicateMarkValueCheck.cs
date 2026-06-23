using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using System.Collections.Generic;
using RevitQAQC.Shared.Models;

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
            int duplicateCount = 0;

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
                    duplicateCount++;
                }
                else
                {
                    usedMarks.Add(markValue);
                }
            }

            return new CheckResult
            {
                CheckName = CheckName,
                IsPass = duplicateCount == 0,
                Message = duplicateCount == 0
                    ? "No duplicate Mark values found."
                    : $"{duplicateCount} duplicate Mark values found.",
                IssueCount = duplicateCount
            };
        }
    }
}