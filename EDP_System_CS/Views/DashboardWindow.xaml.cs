using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using EDPSystem.Models;
using EDPSystem.Services;
using EDPSystem.ViewModels;

namespace EDPSystem.Views
{
    public partial class DashboardWindow : Window
    {
        private DashboardViewModel viewModel;
        private User? _currentUser;

        public DashboardWindow() : this(null) { }

        public DashboardWindow(User? currentUser = null)
        {
            _currentUser = currentUser;
            AvaloniaXamlLoader.Load(this);
            viewModel = new DashboardViewModel();
            this.DataContext = viewModel;
            this.Opened += DashboardWindow_Opened;
        }

        private async void DashboardWindow_Opened(object? sender, EventArgs e)
        {
            await viewModel.LoadDataAsync();
        }

        private void DashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            var dashboardBtn = this.FindControl<Button>("DashboardBtn");
            var reportsBtn = this.FindControl<Button>("ReportsBtn");
            var aboutBtn = this.FindControl<Button>("AboutBtn");
            
            if (dashboardBtn != null)
                dashboardBtn.Foreground = new SolidColorBrush(Color.Parse("#00F3FF"));
            if (reportsBtn != null)
                reportsBtn.Foreground = new SolidColorBrush(Color.Parse("SlateGray"));
            if (aboutBtn != null)
                aboutBtn.Foreground = new SolidColorBrush(Color.Parse("SlateGray"));
        }

        private void ReportsBtn_Click(object sender, RoutedEventArgs e)
        {
            ReportsWindow reportsWindow = new ReportsWindow();
            reportsWindow.Show();
        }

        private void TransactionsBtn_Click(object sender, RoutedEventArgs e)
        {
            TransactionsWindow transactionsWindow = new TransactionsWindow(_currentUser);
            transactionsWindow.Show();
        }

        private void UserMgmtBtn_Click(object sender, RoutedEventArgs e)
        {
            var databaseService = new DatabaseService();
            UserManagementWindow userMgmtWindow = new UserManagementWindow(databaseService);
            userMgmtWindow.Show();
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            // Simple message box
            var dialog = new Window
            {
                Title = "System Information",
                Width = 400,
                Height = 200,
                Content = new TextBlock
                {
                    Text = "EDP Intelligence System v1.0\n\nA cyberpunk-themed neural interface for data processing and intelligence analysis.\n\nPowered by .NET 8.0 and Avalonia",
                    Margin = new Avalonia.Thickness(20),
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                    Foreground = new SolidColorBrush(Color.Parse("White"))
                }
            };
            dialog.ShowDialog(this);
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}

