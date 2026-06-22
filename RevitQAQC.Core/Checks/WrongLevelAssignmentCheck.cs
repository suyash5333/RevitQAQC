using Autodesk.Revit.DB;
using RevitQAQC.Interfaces.Checks;

namespace RevitQAQC.Core.Checks
{
    public class WrongLevelAssignmentCheck : IQACheck
    {
        public string CheckName => "Level Assignment Check";

        public string Description =>
            "Checks whether elements have a valid level assignment.";

        public bool Execute(Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            foreach (var element in elements)
            {
                ElementId levelId = element.LevelId;

                if (levelId == ElementId.InvalidElementId)
                {
                    return false;
                }

                Level level = doc.GetElement(levelId) as Level;

                if (level == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}