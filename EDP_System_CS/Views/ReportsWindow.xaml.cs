using System;
using System.IO;
using System.Text.Json;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using EDPSystem.Services;
using EDPSystem.ViewModels;

namespace EDPSystem.Views
{
    public partial class ReportsWindow : Window
    {
        private ReportsViewModel viewModel;

        public ReportsWindow()
        {
            AvaloniaXamlLoader.Load(this);
            viewModel = new ReportsViewModel();
            this.DataContext = viewModel;
            this.Opened += ReportsWindow_Opened;
        }

        private async void ReportsWindow_Opened(object? sender, EventArgs e)
        {
            var dateFromPicker = this.FindControl<CalendarDatePicker>("DateFromPicker");
            var dateToPicker = this.FindControl<CalendarDatePicker>("DateToPicker");
            
            if (dateFromPicker != null)
                dateFromPicker.SelectedDate = DateTime.Now.AddDays(-30);
            if (dateToPicker != null)
                dateToPicker.SelectedDate = DateTime.Now;

            await viewModel.LoadReportsAsync();
            UpdateReportCount();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var priorityCombo = this.FindControl<ComboBox>("PriorityCombo");
            var dateFromPicker = this.FindControl<CalendarDatePicker>("DateFromPicker");
            var dateToPicker = this.FindControl<CalendarDatePicker>("DateToPicker");
            
            if (priorityCombo?.SelectedItem is ComboBoxItem item)
                viewModel.SelectedPriority = item.Content?.ToString() ?? "ALL";

            viewModel.DateFrom = dateFromPicker?.SelectedDate ?? DateTime.Now.AddDays(-30);
            viewModel.DateTo = dateToPicker?.SelectedDate ?? DateTime.Now;

            await viewModel.LoadReportsAsync();
            UpdateReportCount();
        }

        private void UpdateReportCount()
        {
            var reportCountText = this.FindControl<TextBlock>("ReportCountText");
            if (reportCountText != null)
                reportCountText.Text = $"{viewModel.Reports.Count} reports";
        }

        private void ExportJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var json = JsonSerializer.Serialize(viewModel.Reports, new JsonSerializerOptions { WriteIndented = true });
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                             $"reports_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                File.WriteAllText(filePath, json);

                var dialog = new Window
                {
                    Title = "Export Successful",
                    Width = 400,
                    Height = 150,
                    Content = new TextBlock
                    {
                        Text = $"Reports exported to:\n{filePath}",
                        Margin = new Avalonia.Thickness(20),
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Foreground = new SolidColorBrush(Color.Parse("White"))
                    }
                };
                dialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                var dialog = new Window
                {
                    Title = "Export Error",
                    Width = 400,
                    Height = 150,
                    Content = new TextBlock
                    {
                        Text = $"Export failed: {ex.Message}",
                        Margin = new Avalonia.Thickness(20),
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Foreground = new SolidColorBrush(Color.Parse("#FF006E"))
                    }
                };
                dialog.ShowDialog(this);
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var csv = "BatchID,Source,Priority,CreatedAt\n";
                csv += string.Join("\n", viewModel.Reports.Select(r =>
                    $"{r.BatchId},{r.Source},{r.Priority},{r.CreatedAt:yyyy-MM-dd HH:mm}"));

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                             $"reports_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                File.WriteAllText(filePath, csv);

                var dialog = new Window
                {
                    Title = "Export Successful",
                    Width = 400,
                    Height = 150,
                    Content = new TextBlock
                    {
                        Text = $"Reports exported to:\n{filePath}",
                        Margin = new Avalonia.Thickness(20),
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Foreground = new SolidColorBrush(Color.Parse("White"))
                    }
                };
                dialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                var dialog = new Window
                {
                    Title = "Export Error",
                    Width = 400,
                    Height = 150,
                    Content = new TextBlock
                    {
                        Text = $"Export failed: {ex.Message}",
                        Margin = new Avalonia.Thickness(20),
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                        Foreground = new SolidColorBrush(Color.Parse("#FF006E"))
                    }
                };
                dialog.ShowDialog(this);
            }
        }
    }
}

