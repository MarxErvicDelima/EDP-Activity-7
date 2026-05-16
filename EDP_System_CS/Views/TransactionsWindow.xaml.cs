using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using EDPSystem.Models;
using EDPSystem.Services;
using EDPSystem.ViewModels;

namespace EDPSystem.Views
{
    public partial class TransactionsWindow : Window
    {
        private TransactionsViewModel viewModel;

        public TransactionsWindow() : this(null) { }

        public TransactionsWindow(User? currentUser = null)
        {
            AvaloniaXamlLoader.Load(this);
            viewModel = new TransactionsViewModel(currentUser: currentUser);
            this.DataContext = viewModel;
            this.Opened += TransactionsWindow_Opened;
        }

        private async void TransactionsWindow_Opened(object? sender, EventArgs e)
        {
            await viewModel.LoadAllTransactionsAsync();
            UpdateRecordCounts();
        }

        // Book Borrowing Events
        private async void AddBookBorrow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await viewModel.AddBookBorrowAsync();
                UpdateRecordCounts();
                ShowMessage("Book borrow record added successfully!");
            }
            catch (Exception ex)
            {
                ShowError($"Error adding record: {ex.Message}");
            }
        }

        private async void ExportBookBorrows_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                             $"BookBorrows_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                await viewModel.ExportBookBorrowsAsync(filePath);
                ShowMessage($"Book borrows report exported to:\n{filePath}");
            }
            catch (Exception ex)
            {
                ShowError($"Export failed: {ex.Message}");
            }
        }

        // Sales Transactions Events
        private async void AddSalesTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var paymentCombo = this.FindControl<ComboBox>("PaymentCombo");
                if (paymentCombo?.SelectedItem is ComboBoxItem item)
                    viewModel.PaymentMethod = item.Content?.ToString() ?? "Cash";

                await viewModel.AddSalesTransactionAsync();
                UpdateRecordCounts();
                ShowMessage("Sales transaction added successfully!");
            }
            catch (Exception ex)
            {
                ShowError($"Error adding record: {ex.Message}");
            }
        }

        private async void ExportSalesTransactions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                             $"SalesTransactions_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                await viewModel.ExportSalesTransactionsAsync(filePath);
                ShowMessage($"Sales transactions report exported to:\n{filePath}");
            }
            catch (Exception ex)
            {
                ShowError($"Export failed: {ex.Message}");
            }
        }

        // Inventory Count Events
        private async void AddInventoryCount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await viewModel.AddInventoryCountAsync();
                UpdateRecordCounts();
                ShowMessage("Inventory count record added successfully!");
            }
            catch (Exception ex)
            {
                ShowError($"Error adding record: {ex.Message}");
            }
        }

        private async void ExportInventoryCounts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                             $"InventoryCounts_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                await viewModel.ExportInventoryCountsAsync(filePath);
                ShowMessage($"Inventory counts report exported to:\n{filePath}");
            }
            catch (Exception ex)
            {
                ShowError($"Export failed: {ex.Message}");
            }
        }

        // Helper Methods
        private void UpdateRecordCounts()
        {
            var bookBorrowText = this.FindControl<TextBlock>("BookBorrowCountText");
            if (bookBorrowText != null)
                bookBorrowText.Text = $"{viewModel.BookBorrows.Count} records";

            var salesText = this.FindControl<TextBlock>("SalesCountText");
            if (salesText != null)
                salesText.Text = $"{viewModel.SalesTransactions.Count} records";

            var inventoryText = this.FindControl<TextBlock>("InventoryCountText");
            if (inventoryText != null)
                inventoryText.Text = $"{viewModel.InventoryCounts.Count} records";
        }

        private void ShowMessage(string message)
        {
            var dialog = new Window
            {
                Title = "Success",
                Width = 500,
                Height = 200,
                Background = new SolidColorBrush(Color.Parse("#020617")),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var stackPanel = new StackPanel { Margin = new Avalonia.Thickness(20) };
            var textBlock = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush(Color.Parse("White")),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Avalonia.Thickness(0, 0, 0, 20)
            };
            var button = new Button
            {
                Content = "OK",
                Width = 100,
                Background = new SolidColorBrush(Color.Parse("#00F3FF")),
                Foreground = new SolidColorBrush(Color.Parse("#020617"))
            };
            button.Click += (s, e) => dialog.Close();

            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(button);
            dialog.Content = stackPanel;
            dialog.ShowDialog(this);
        }

        private void ShowError(string message)
        {
            var dialog = new Window
            {
                Title = "Error",
                Width = 500,
                Height = 200,
                Background = new SolidColorBrush(Color.Parse("#020617")),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var stackPanel = new StackPanel { Margin = new Avalonia.Thickness(20) };
            var textBlock = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush(Color.Parse("#FF006E")),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Avalonia.Thickness(0, 0, 0, 20)
            };
            var button = new Button
            {
                Content = "OK",
                Width = 100,
                Background = new SolidColorBrush(Color.Parse("#FF006E")),
                Foreground = new SolidColorBrush(Color.Parse("White"))
            };
            button.Click += (s, e) => dialog.Close();

            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(button);
            dialog.Content = stackPanel;
            dialog.ShowDialog(this);
        }
    }
}
