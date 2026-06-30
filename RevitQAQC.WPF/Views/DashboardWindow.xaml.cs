using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RevitQAQC.Shared.Models;
using RevitQAQC.WPF.ViewModels;
using Microsoft.Win32;
using RevitQAQC.Engine.Reports;
using System.ComponentModel;

namespace RevitQAQC.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        private ICollectionView _resultsView;
        private readonly ReportModel _report;
        public DashboardWindow(ReportModel report)
        {
            InitializeComponent();

            try
            {
                _report = report;

                DashboardViewModel viewModel = new DashboardViewModel(report);

                DataContext = viewModel;

                _resultsView = CollectionViewSource.GetDefaultView(viewModel.Report.CheckResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to initialize dashboard.\n\nReason:\n{ex.Message}",
                    "Initialization Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Close();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
                    PdfReportGenerator generator = new PdfReportGenerator();

                    generator.Generate(_report, dialog.FileName);

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
                    JsonReportGenerator generator = new JsonReportGenerator();

                    generator.GenerateReport(_report, dialog.FileName);

                    MessageBox.Show(
                        "JSON exported successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to export the JSON file.\n\nReason:\n{ex.Message}",
                    "JSON Export Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                }
            }
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_resultsView == null)
                return;

            string searchText = SearchBox.Text.Trim().ToLower();

            _resultsView.Filter = item =>
            {
                if (item is CheckResult result)
                {
                    if (string.IsNullOrEmpty(searchText))
                        return true;

                    return result.CheckName.ToLower().Contains(searchText)
                        || result.Message.ToLower().Contains(searchText);
                }

                return false;
            };

            ApplyFilters();
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_resultsView == null)
                return;

            string searchText = SearchBox.Text.Trim().ToLower();

            string status = (StatusFilter.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All";

            _resultsView.Filter = item =>
            {
                if (item is not CheckResult result)
                    return false;

                bool matchesSearch =
                string.IsNullOrWhiteSpace(searchText) ||
                (result.CheckName?.ToLower().Contains(searchText) ?? false) ||
                (result.Message?.ToLower().Contains(searchText) ?? false);

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
    }
}