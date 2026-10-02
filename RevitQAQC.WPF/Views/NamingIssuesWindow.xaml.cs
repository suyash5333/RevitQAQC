using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using RevitQAQC.Core.Services;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.WPF.Views
{
    public partial class NamingIssuesWindow : Window
    {
        private readonly ElementSelectionService _selectionService;
        private readonly ElementRemediationService _remediationService;

        public NamingIssuesWindow(
            List<QAIssue> issues,
            ElementSelectionService selectionService,
            ElementRemediationService remediationService)
        {
            InitializeComponent();

            _selectionService = selectionService;
            _remediationService = remediationService;

            IssuesDataGrid.ItemsSource = issues;
        }

        private void IssuesDataGrid_MouseDoubleClick(
            object sender,
            MouseButtonEventArgs e)
        {
            if (IssuesDataGrid.SelectedItem is not QAIssue issue)
                return;

            _selectionService.SelectElement(
                issue.ElementId);
        }


        private void RemediateButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (IssuesDataGrid.SelectedItem is not QAIssue issue)
            {
                MessageBox.Show(
                    "Please select a naming issue first.",
                    "Remediate Naming Issue",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(issue.SuggestedValue))
            {
                MessageBox.Show(
                    "There is no suggested value for this issue.",
                    "Remediation Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmation =
                MessageBox.Show(
                    $"Remediate this issue?\n\n" +
                    $"Element ID: {issue.ElementId}\n" +
                    $"Current Value: {issue.CurrentValue}\n" +
                    $"New Value: {issue.SuggestedValue}",
                    "Confirm Remediation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
                return;

            _remediationService.RemediateNamingIssue(
      issue.ElementId,
      issue.SuggestedValue,
      success =>
      {
          if (!success)
              return;

          Dispatcher.Invoke(() =>
          {
              issue.CurrentValue = issue.SuggestedValue;
              issue.SuggestedValue = "";
              issue.Problem = "Remediated successfully.";
              issue.Severity = "Resolved";

              IssuesDataGrid.Items.Refresh();
          });
      });
        }

        private void CloseButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}