
using System.Linq;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Engine.Processors
{
    public class ResultsProcessor
    {
        public HealthScoreResult Calculate(List<CheckResult> results)
        {
            int totalElements = results.FirstOrDefault(r => r.CheckName == "Element Count")
             ?.value ?? 0;

            double score = 0;

            if (totalElements > 0)
            {
                var weights = new Dictionary<string, double>
            {
                { "Missing Mark Parameter", 30 },
                { "Missing Comments", 20 },
                { "Duplicate Mark Values", 10 },
                { "Level Assignment Check", 25 },
                { "Model Standards Check", 15 }
            };

                foreach (var result in results)
                {
                    if (weights.ContainsKey(result.CheckName))
                    {
                        double checkScore = 100 -
                            ((double)result.IssueCount / totalElements * 100);

                        checkScore = Math.Max(0, checkScore);

                        score += checkScore * (weights[result.CheckName] / 100.0);
                    }
                }
            }

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