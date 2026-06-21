namespace RevitQAQC.Interfaces.Checks;

public interface IQACheck
{
    string CheckName { get; }

    string Description { get; }

    bool Execute();
}