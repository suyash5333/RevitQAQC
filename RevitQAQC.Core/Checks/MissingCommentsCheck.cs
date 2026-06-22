using RevitQAQC.Interfaces.Checks;
using Autodesk.Revit.DB;

namespace RevitQAQC.Core.Checks
{
    public class MissingCommentsCheck : IQACheck
    {
        public string CheckName => "Missing Comments";

        public string Description => "Checks elements with empty Comments.";

        public bool Execute(Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToElements();

            foreach (var element in elements)
            {
                Parameter commentsParam =
                    element.LookupParameter("Comments");

                if (commentsParam == null)
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(commentsParam.AsString()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}