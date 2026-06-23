using Autodesk.Revit.DB;
using RevitQAQC.Interfaces;
using RevitQAQC.Interfaces.Checks;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Engine.Services
{
    public class QAEngine
    {
        private readonly List<IQACheck> _checks;

        public QAEngine(List<IQACheck> checks)
        {
            _checks = checks;
        }

        public List<CheckResult> Run(Document document)
        {
            List<CheckResult> results = new();

            foreach (var check in _checks)
            {
                CheckResult result = check.Execute(document);
                results.Add(result);
            }

            return results;
        }
    }
}