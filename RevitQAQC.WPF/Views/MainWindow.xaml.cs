using RevitQAQC.Core.Services;
using RevitQAQC.Shared.Models;
using System.Windows;
using RevitQAQC.Interfaces.Services;

namespace RevitQAQC.WPF.Views
{
    public partial class MainWindow : Window
    {
        private ReportModel _report;

        private IssuesView _issuesView;

        private RemediationView _remediationView;

        private DashboardView _dashboardView;

        private readonly IQAScanService _scanService;
        private readonly ElementSelectionService _selectionService;
        private readonly ElementRemediationService _remediationService;

        public MainWindow(
            ReportModel report,
            ElementSelectionService selectionService,
            ElementRemediationService remediationService,
            IQAScanService scanService)
        {
            InitializeComponent();

            _report = report;
            _selectionService = selectionService;
            _remediationService = remediationService;
            _scanService = scanService;

            _dashboardView =
    new DashboardView(_report);

            ContentFrame.Content =
                _dashboardView;
        }

        private void Dashboard_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (_dashboardView == null)
            {
                _dashboardView =
                    new DashboardView(_report);
            }

            ContentFrame.Content =
                _dashboardView;
        }

        private void QAChecks_Click(
            object sender,
            RoutedEventArgs e)
        {
            ContentFrame.Content =
                new QAChecksView();
        }

        private void Issues_Click(
            object sender,
            RoutedEventArgs e)
        {
            _issuesView = new IssuesView(
            _report,
            _selectionService,
            UpdateReport);

            ContentFrame.Content = _issuesView;
        }

        private void Remediation_Click(
            object sender,
            RoutedEventArgs e)
        {
            _remediationView = new RemediationView(
            _report,
            _remediationService,
            _scanService,
            UpdateReport,
            _selectionService);

            ContentFrame.Content = _remediationView;
        }

        private void Reports_Click(
            object sender,
            RoutedEventArgs e)
        {
            ContentFrame.Content =
                new ReportsView();
        }

        private void Settings_Click(
            object sender,
            RoutedEventArgs e)
        {
            ContentFrame.Content =
                new SettingsView();
        }
        public void UpdateReport(ReportModel newReport)
        {
            _report = newReport;

            if (_dashboardView != null)
            {
                _dashboardView.UpdateReport(_report);
            }

            if (_issuesView != null)
            {
                _issuesView.UpdateReport(_report);
            }

            if (_remediationView != null)
            {
                _remediationView.UpdateReport(_report);
            }
        }
    }
}