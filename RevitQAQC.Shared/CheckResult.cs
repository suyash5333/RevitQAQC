namespace RevitQAQC.Shared;

public class CheckResult
{
    public string CheckName { get; set; } = "";

    public bool IsPass { get; set; }

    public string Message { get; set; } = "";
}