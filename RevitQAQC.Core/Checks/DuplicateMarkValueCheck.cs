using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using System.Collections.Generic;

namespace RevitQAQC.Core.Checks
{
    public class DuplicateMarkValueCheck : IQACheck
    {
        public string CheckName => "Duplicate Mark Values";

        public string Description => "Checks for duplicate Mark parameter values.";

        public bool Execute(Document doc)
        {
            var usedMarks = new HashSet<string>();

            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            foreach (var element in elements)
            {
                Parameter markParam = element.LookupParameter("Mark");

                if (markParam == null)
                    continue;

                string markValue = markParam.AsString();

                if (string.IsNullOrWhiteSpace(markValue))
                    continue;

                if (usedMarks.Contains(markValue))
                    return false;

                usedMarks.Add(markValue);
            }

            return true;
        }
    }
}