using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EDPSystem.Models;
using EDPSystem.Services;

namespace EDPSystem.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<IntelligenceBatch> _reports = new();
        private string _selectedPriority = "ALL";
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private bool _isLoading;

        public ObservableCollection<IntelligenceBatch> Reports
        {
            get => _reports;
            set => SetProperty(ref _reports, value);
        }

        public string SelectedPriority
        {
            get => _selectedPriority;
            set => SetProperty(ref _selectedPriority, value);
        }

        public DateTime DateFrom
        {
            get => _dateFrom;
            set => SetProperty(ref _dateFrom, value);
        }

        public DateTime DateTo
        {
            get => _dateTo;
            set => SetProperty(ref _dateTo, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ReportsViewModel(DatabaseService? databaseService = null)
        {
            _databaseService = databaseService ?? new DatabaseService();
            Reports = new ObservableCollection<IntelligenceBatch>();
            DateFrom = DateTime.Now.AddDays(-30);
            DateTo = DateTime.Now;
        }

        public async Task LoadReportsAsync()
        {
            IsLoading = true;
            try
            {
                var priority = SelectedPriority == "ALL" ? null : SelectedPriority;
                var reports = await _databaseService.GetReportsFilteredAsync(priority, DateFrom, DateTo);
                
                Reports.Clear();
                foreach (var report in reports)
                    Reports.Add(report);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task RefreshAsync()
        {
            await LoadReportsAsync();
        }

        public void ExportAsJson()
        {
            // TODO: Implement JSON export
        }

        public void ExportAsCsv()
        {
            // TODO: Implement CSV export
        }
    }
}
