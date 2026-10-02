using RevitQAQC.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace RevitQAQC.WPF.ViewModels
{
    public class DashboardViewModel
    {
        public ReportModel Report { get; private set; }

        public DashboardViewModel(ReportModel report)
        {
            Report = report;
        }

        public int FailedChecks
        {
            get
            {
                return Report.CheckResults.Count(result => !result.IsPass);
            }
        }

        public List<QAIssue> NamingIssues
        {
            get
            {
                var namingCheck = Report.CheckResults
                    .FirstOrDefault(x =>
                        x.CheckName == "Naming Convention Check");

                return namingCheck?.Issues ?? new List<QAIssue>();
            }
        }
    }
}