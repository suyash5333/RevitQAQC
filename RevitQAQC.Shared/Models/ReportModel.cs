using System.Collections.Generic;

namespace RevitQAQC.Shared.Models;

public class ReportModel
{
    public string ProjectName { get; set; } = "";

    public DateTime GeneratedOn { get; set; }

    public List<CheckResult> CheckResults { get; set; } = new();

    public HealthScoreResult HealthScore { get; set; } = new();
}