using Autodesk.Revit.DB;
using RevitQAQC.Shared.Models;
namespace RevitQAQC.Interfaces.Checks
{
    public interface IQACheck
    {
        string CheckName { get; }

        string Description { get; }

        CheckResult Execute(Document doc);
    }
}