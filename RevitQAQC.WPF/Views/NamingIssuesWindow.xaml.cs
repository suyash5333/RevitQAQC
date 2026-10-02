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

        public NamingIssuesWindow(
            List<QAIssue> issues,
            ElementSelectionService selectionService)
        {
            InitializeComponent();

            _selectionService = selectionService;

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

        private void CloseButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}