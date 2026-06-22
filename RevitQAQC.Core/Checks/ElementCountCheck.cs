using RevitQAQC.Interfaces.Checks;
using Autodesk.Revit.DB;

namespace RevitQAQC.Core.Checks
{
    public class ElementCountCheck : IQACheck
    {
        public string CheckName => "Element Count";

        public string Description => "Counts model elements.";

        public bool Execute(Document doc)
        {
            int elementCount = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .GetElementCount();

            return elementCount > 0;
        }
    }
}