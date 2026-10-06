using RevitQAQC.Core.Services;
using RevitQAQC.Shared.Models;
using System.Linq;
using System.Windows;
using RevitQAQC.Interfaces.Services;
using System.Windows.Controls;
using System;

namespace RevitQAQC.WPF.Views
{
    public partial class RemediationView : UserControl
    {
        private readonly ReportModel _report;

        private readonly ElementSelectionService _selectionService;

        private readonly IQAScanService _scanService;
        private readonly ElementRemediationService _remediationService;

        private readonly Action<ReportModel> _reportUpdated;

        public RemediationView(
     ReportModel report,
     ElementRemediationService remediationService,
     IQAScanService scanService,
     Action<ReportModel> reportUpdated,
     ElementSelectionService selectionService)
        {
            InitializeComponent();

            _report = report;
            _selectionService = selectionService;
            _remediationService = remediationService;
            _scanService = scanService;
            _reportUpdated = reportUpdated;

            var issues = report.CheckResults
                .Where(result =>
                    result.CheckName == "Naming Convention Check")
                .SelectMany(result => result.Issues)
                .ToList();

            RemediationDataGrid.ItemsSource = issues;
        }

        private void RemediationDataGrid_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (RemediationDataGrid.SelectedItem is QAIssue issue)
            {
                SelectedElementText.Text =
                    issue.ElementId.ToString();

                RecommendedActionText.Text =
                    $"Change Mark from \"{issue.CurrentValue}\" " +
                    $"to \"{issue.SuggestedValue}\".";
            }
        }

        private void FixSelected_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (RemediationDataGrid.SelectedItem is not QAIssue issue)
            {
                MessageBox.Show(
                    "Please select an issue first.",
                    "QAQC Remediation");

                return;
            }

            if (string.IsNullOrWhiteSpace(issue.SuggestedValue))
            {
                MessageBox.Show(
                    "This issue does not have a suggested value.",
                    "QAQC Remediation");

                return;
            }

            if (issue.CurrentValue == issue.SuggestedValue)
            {
                MessageBox.Show(
                    "The current value is already the suggested value. No change is required.",
                    "QAQC Remediation");

                return;
            }

            _remediationService.RemediateNamingIssue(
                issue.ElementId,
                issue.SuggestedValue,
                success => { });
        }

        private void FixAll_Click(
    object sender,
    RoutedEventArgs e)
        {
            var issues = RemediationDataGrid.Items
                .OfType<QAIssue>()
                .Where(issue =>
                    !string.IsNullOrWhiteSpace(issue.SuggestedValue))
                .ToList();

            if (issues.Count == 0)
            {
                MessageBox.Show(
                    "There are no naming convention issues available for automatic remediation.",
                    "QAQC Remediation");

                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"You are about to fix {issues.Count} naming convention issue(s).\n\n" +
                "The changes will be applied to the Revit model.\n\n" +
                "Do you want to continue?",
                "Confirm Fix All",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            var elementIds = issues
                .Select(issue => issue.ElementId)
                .ToList();

            var suggestedValues = issues
                .Select(issue => issue.SuggestedValue)
                .ToList();

            _remediationService.RemediateAllNamingIssues(
                elementIds,
                suggestedValues,
                fixedCount =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            $"{fixedCount} naming convention issue(s) were fixed.",
                            "QAQC Remediation");
                    });
                });
        }

        private void SelectInRevit_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (RemediationDataGrid.SelectedItem is not QAIssue issue)
            {
                MessageBox.Show(
                    "Please select an issue first.",
                    "QAQC Remediation");

                return;
            }

            _selectionService.SelectElement(issue.ElementId);
        }


        public void UpdateReport(ReportModel newReport)
        {
            var issues = newReport.CheckResults
                .Where(result =>
                    result.CheckName == "Naming Convention Check")
                .SelectMany(result => result.Issues)
                .ToList();

            RemediationDataGrid.ItemsSource = issues;

            SelectedElementText.Text = "—";
            RecommendedActionText.Text = "—";
        }

        private void Rescan_Click(
    object sender,
    RoutedEventArgs e)
        {
            _scanService.RunScan(report =>
            {
                Dispatcher.Invoke(() =>
                {
                    _reportUpdated(report);

                    MessageBox.Show(
                        $"Re-scan completed.\n\n" +
                        $"Issues found: {report.CheckResults.Sum(r => r.IssueCount)}",
                        "QAQC Re-scan");
                });
            });
        }
        private void PreviewFix_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (RemediationDataGrid.SelectedItem is not QAIssue issue)
            {
                MessageBox.Show(
                    "Please select an issue first.",
                    "QAQC Remediation");

                return;
            }

            string currentValue =
                string.IsNullOrWhiteSpace(issue.CurrentValue)
                    ? "(empty)"
                    : issue.CurrentValue;

            string suggestedValue =
                string.IsNullOrWhiteSpace(issue.SuggestedValue)
                    ? "(none)"
                    : issue.SuggestedValue;

            MessageBox.Show(
                $"Element ID: {issue.ElementId}\n\n" +
                $"Current Mark:\n{currentValue}\n\n" +
                $"Proposed Mark:\n{suggestedValue}\n\n" +
                "No changes have been applied to the Revit model.",
                "Preview Fix");
        }
    }
}