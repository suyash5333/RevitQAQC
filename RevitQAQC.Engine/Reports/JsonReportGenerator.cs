using System.IO;
using Newtonsoft.Json;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Engine.Reports
{
    public class JsonReportGenerator
    {
        public void GenerateReport(ReportModel report, string filePath)
        {
            string json = JsonConvert.SerializeObject(
                report,
                Formatting.Indented);

            File.WriteAllText(filePath, json);
        }
    }
}