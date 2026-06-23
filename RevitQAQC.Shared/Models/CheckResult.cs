namespace RevitQAQC.Shared.Models;

public class CheckResult
{
    public string CheckName { get; set; } = "";

    public bool IsPass { get; set; }

    public string Message { get; set; } = "";

    public int IssueCount { get; set; }

    public int value { get; set; }
}