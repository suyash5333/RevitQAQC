using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Core.Checks
{
    public class ModelStandardsCheck : IQACheck
    {
        public string CheckName => "Model Standards Check";

        public string Description =>
            "Checks whether the model contains required levels.";

        public CheckResult Execute(Document doc)
        {
            int levelCount = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .GetElementCount();

            return new CheckResult
            {
                CheckName = CheckName,
                IsPass = levelCount > 0,
                Message = levelCount > 0
                ? $"Model contains {levelCount} levels."
                : "No levels found in model.",
                IssueCount = levelCount > 0 ? 0 : 1
            };
        }
    }
}