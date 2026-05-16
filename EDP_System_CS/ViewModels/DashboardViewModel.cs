using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EDPSystem.Models;
using EDPSystem.Services;

namespace EDPSystem.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<SystemStat> _stats = new();
        private ObservableCollection<SystemLog> _systemLogs = new();
        private ObservableCollection<ThroughputLog> _throughputLogs = new();
        private bool _isLoading;

        public ObservableCollection<SystemStat> Stats
        {
            get => _stats;
            set => SetProperty(ref _stats, value);
        }

        public ObservableCollection<SystemLog> SystemLogs
        {
            get => _systemLogs;
            set => SetProperty(ref _systemLogs, value);
        }

        public ObservableCollection<ThroughputLog> ThroughputLogs
        {
            get => _throughputLogs;
            set => SetProperty(ref _throughputLogs, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public DashboardViewModel(DatabaseService? databaseService = null)
        {
            _databaseService = databaseService ?? new DatabaseService();
            Stats = new ObservableCollection<SystemStat>();
            SystemLogs = new ObservableCollection<SystemLog>();
            ThroughputLogs = new ObservableCollection<ThroughputLog>();
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                // Load stats
                var stats = await _databaseService.GetStatsAsync();
                Stats.Clear();
                foreach (var stat in stats)
                    Stats.Add(stat);

                // Load system logs
                var logs = await _databaseService.GetSystemLogsAsync();
                SystemLogs.Clear();
                foreach (var log in logs)
                    SystemLogs.Add(log);

                // Load throughput logs
                var throughput = await _databaseService.GetThroughputLogsAsync();
                ThroughputLogs.Clear();
                foreach (var log in throughput)
                    ThroughputLogs.Add(log);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
