
using System.Linq;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Engine.Processors
{
    public class ResultsProcessor
    {
        public HealthScoreResult Calculate(List<CheckResult> results)
        {
            int totalIssues = results.Sum(r => r.IssueCount);

            int totalElements = results
                .FirstOrDefault(r => r.CheckName == "Element Count")
                ?.value ?? 0;

            double score = 100;

            if (totalElements > 0)
            {
                score = ((double)(totalElements - totalIssues)
                         / totalElements) * 100;
            }

            score = Math.Max(0, score);

            string grade;

            if (score >= 90)
                grade = "Excellent";
            else if (score >= 75)
                grade = "Good";
            else if (score >= 50)
                grade = "Fair";
            else
                grade = "Poor";

            return new HealthScoreResult
            {
                Score = Math.Round(score, 2),
                Grade = grade
            };
        }
        public ReportModel CreateReport(string projectName,List<CheckResult> results)
        {
            return new ReportModel
            {
                ProjectName = projectName,
                GeneratedOn = DateTime.Now,
                CheckResults = results,
                HealthScore = Calculate(results)
            };
        }
    }
}