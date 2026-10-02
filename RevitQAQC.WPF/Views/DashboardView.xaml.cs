using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Win32;

using RevitQAQC.Engine.Reports;
using RevitQAQC.Shared.Models;
using RevitQAQC.WPF.ViewModels;

namespace RevitQAQC.WPF.Views
{
    public partial class DashboardView : UserControl
    {
        private ICollectionView _resultsView;
        private readonly ReportModel _report;

        public DashboardView(ReportModel report)
        {
            InitializeComponent();

            try
            {
                _report = report;

                DashboardViewModel viewModel = new DashboardViewModel(report);

                DataContext = viewModel;

                _resultsView =
                    CollectionViewSource.GetDefaultView(
                        viewModel.Report.CheckResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to initialize dashboard.\n\nReason:\n{ex.Message}",
                    "Initialization Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Window.GetWindow(this)?.Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private void ExportPdfButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "PDF Files (*.pdf)|*.pdf";
            dialog.FileName = "QAQC_Report.pdf";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    PdfReportGenerator generator =
                        new PdfReportGenerator();

                    generator.Generate(
                        _report,
                        dialog.FileName);

                    MessageBox.Show(
                        "PDF exported successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Unable to export the PDF.\n\nReason:\n{ex.Message}",
                        "PDF Export Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void ExportJsonButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "JSON Files (*.json)|*.json";
            dialog.FileName = "QAQC_Report.json";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    JsonReportGenerator generator =
                        new JsonReportGenerator();

                    generator.GenerateReport(
                        _report,
                        dialog.FileName);

                    MessageBox.Show(
                        "JSON exported successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Unable to export the JSON file.\n\nReason:\n{ex.Message}",
                        "JSON Export Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (_resultsView == null)
                return;

            ApplyFilters();
        }

        private void StatusFilter_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_resultsView == null)
                return;

            string searchText =
                SearchBox.Text.Trim().ToLower();

            string status =
                (StatusFilter.SelectedItem as ComboBoxItem)
                ?.Content?.ToString() ?? "All";

            _resultsView.Filter = item =>
            {
                if (item is not CheckResult result)
                    return false;

                bool matchesSearch =
                    string.IsNullOrWhiteSpace(searchText) ||
                    (result.CheckName?.ToLower()
                        .Contains(searchText) ?? false) ||
                    (result.Message?.ToLower()
                        .Contains(searchText) ?? false);

                bool matchesStatus = status switch
                {
                    "Pass" => result.IsPass,
                    "Fail" => !result.IsPass,
                    _ => true
                };

                return matchesSearch && matchesStatus;
            };

            _resultsView.Refresh();
        }
        private void ChecksDataGrid_MouseDoubleClick(
    object sender,
    System.Windows.Input.MouseButtonEventArgs e)
        {
            // Element selection/remediation will be connected here later.
        }
    }
}