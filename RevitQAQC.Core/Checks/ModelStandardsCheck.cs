using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;

namespace RevitQAQC.Core.Checks
{
    public class ModelStandardsCheck : IQACheck
    {
        public string CheckName => "Model Standards Check";

        public string Description =>
            "Checks whether the model contains required levels.";

        public bool Execute(Document doc)
        {
            int levelCount = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .GetElementCount();

            return levelCount > 0;
        }
    }
}