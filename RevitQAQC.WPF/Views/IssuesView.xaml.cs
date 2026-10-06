using RevitQAQC.Core.Services;
using RevitQAQC.Shared.Models;
using System.Collections.Generic;
using System;
using RevitQAQC.Interfaces.Services;
using System.Linq;
using System.Windows.Controls;

namespace RevitQAQC.WPF.Views
{
    public partial class IssuesView : UserControl
    {
        private readonly ReportModel _report;
        private readonly ElementSelectionService _selectionService;
        private readonly List<IssueDisplayModel> _issues;
        private readonly Action<ReportModel> _reportUpdated;

        public IssuesView(
    ReportModel report,
    ElementSelectionService selectionService,
    Action<ReportModel> reportUpdated)

        {
            InitializeComponent();

            _report = report;
            _selectionService = selectionService;
            _reportUpdated = reportUpdated;

            _issues = report.CheckResults
                .SelectMany(result => result.Issues.Select(issue => new IssueDisplayModel
                {
                    Check = result.CheckName,
                    ElementId = issue.ElementId,
                    Severity = issue.Severity,
                    Category = issue.Category,
                    CurrentValue = issue.CurrentValue,
                    Problem = issue.Problem,
                    SuggestedValue = issue.SuggestedValue
                }))
                .ToList();

            IssuesDataGrid.ItemsSource = _issues;

            TotalIssuesText.Text =
                _issues.Count.ToString();

            CriticalText.Text =
                _issues.Count(x => x.Severity == "Critical").ToString();

            MajorText.Text =
                _issues.Count(x => x.Severity == "Major").ToString();

            WarningText.Text =
                _issues.Count(x => x.Severity == "Warning").ToString();



        }

        public void UpdateReport(ReportModel newReport)
        {
            var issues = newReport.CheckResults
                .SelectMany(result =>
                    result.Issues.Select(issue => new IssueDisplayModel
                    {
                        Check = result.CheckName,
                        ElementId = issue.ElementId,
                        Severity = issue.Severity,
                        Category = issue.Category,
                        CurrentValue = issue.CurrentValue,
                        Problem = issue.Problem,
                        SuggestedValue = issue.SuggestedValue
                    }))
                .ToList();

            IssuesDataGrid.ItemsSource = issues;

            TotalIssuesText.Text =
                issues.Count.ToString();

            CriticalText.Text =
                issues.Count(x =>
                    x.Severity == "Critical").ToString();

            MajorText.Text =
                issues.Count(x =>
                    x.Severity == "Major").ToString();

            WarningText.Text =
                issues.Count(x =>
                    x.Severity == "Warning").ToString();
        }

        private void SelectElement_Click(
            object sender,
            System.Windows.RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.DataContext is IssueDisplayModel issue)
            {
                _selectionService.SelectElement(
                    issue.ElementId);
            }
        }
    }

    public class IssueDisplayModel
    {
        public string Check { get; set; } = "";
        public long ElementId { get; set; }
        public string Severity { get; set; } = "";
        public string Category { get; set; } = "";
        public string CurrentValue { get; set; } = "";
        public string Problem { get; set; } = "";
        public string SuggestedValue { get; set; } = "";
    }




}