using Autodesk.Revit.DB;

namespace RevitQAQC.Interfaces.Checks
{
    public interface IQACheck
    {
        string CheckName { get; }

        string Description { get; }

        bool Execute(Document doc);
    }
}