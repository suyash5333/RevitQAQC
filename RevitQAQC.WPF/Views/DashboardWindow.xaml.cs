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

namespace RevitQAQC.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        private readonly ReportModel _report;
        public DashboardWindow(ReportModel report)
        {
            InitializeComponent();
            _report = report;
            DataContext = new DashboardViewModel(report);
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
                PdfReportGenerator generator = new PdfReportGenerator();

                generator.Generate(_report, dialog.FileName);

                MessageBox.Show(
                    "PDF exported successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void ExportJsonButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "JSON Files (*.json)|*.json";
            dialog.FileName = "QAQC_Report.json";

            if (dialog.ShowDialog() == true)
            {
                JsonReportGenerator generator = new JsonReportGenerator();

                generator.GenerateReport(_report, dialog.FileName);

                MessageBox.Show(
                    "JSON exported successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

    }
}