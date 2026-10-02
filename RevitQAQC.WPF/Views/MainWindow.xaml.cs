using RevitQAQC.Shared.Models;
using System.Windows;

namespace RevitQAQC.WPF.Views
{
    public partial class MainWindow : Window

    {
        private readonly ReportModel _report;

        public MainWindow(ReportModel report)
        {
            InitializeComponent();

            _report = report;

            ContentFrame.Content = new DashboardView(_report);
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new DashboardView(_report);
        }
        private void QAChecks_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new QAChecksView();
        }
        private void Issues_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new IssuesView();
        }
        private void Remediation_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new RemediationView();
        }
        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new ReportsView();
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new SettingsView();
        }
    }
}