using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EDPSystem.ViewModels;
using EDPSystem.Services;
using EDPSystem.Models;

namespace EDPSystem.Views
{
    public partial class UserManagementWindow : Window
    {
        private UserManagementViewModel _viewModel;

        public UserManagementWindow(DatabaseService databaseService = null, User currentUser = null)
        {
            AvaloniaXamlLoader.Load(this);
            _viewModel = new UserManagementViewModel(databaseService, currentUser);
            DataContext = _viewModel;
            this.Opened += async (s, e) => await _viewModel.LoadUsersAsync();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SearchUsersAsync();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadUsersAsync();
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.UpdateUserAsync();
        }

        private async void ActivateButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.ActivateUserAsync();
        }

        private async void DeactivateButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.DeactivateUserAsync();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedUser != null)
            {
                var messageBox = new Window
                {
                    Title = "Confirm Delete",
                    Width = 400,
                    Height = 150,
                    Content = new StackPanel
                    {
                        Margin = new Avalonia.Thickness(20),
                        Children =
                        {
                            new TextBlock
                            {
                                Text = $"Are you sure you want to delete user '{_viewModel.SelectedUser.Username}'?",
                                Foreground = Avalonia.Media.Brush.Parse("#e0e0e0"),
                                Margin = new Avalonia.Thickness(0, 0, 0, 20),
                                TextWrapping = Avalonia.Media.TextWrapping.Wrap
                            },
                            new StackPanel
                            {
                                Orientation = Avalonia.Layout.Orientation.Horizontal,
                                Spacing = 10,
                                Children =
                                {
                                    new Button
                                    {
                                        Content = "Yes, Delete",
                                        Background = Avalonia.Media.Brush.Parse("#FF0000"),
                                        Foreground = Avalonia.Media.Brush.Parse("White"),
                                        Padding = new Avalonia.Thickness(15, 8),
                                        Tag = "delete"
                                    },
                                    new Button
                                    {
                                        Content = "Cancel",
                                        Background = Avalonia.Media.Brush.Parse("#00F3FF"),
                                        Foreground = Avalonia.Media.Brush.Parse("#0a0e27"),
                                        Padding = new Avalonia.Thickness(15, 8),
                                        Tag = "cancel"
                                    }
                                }
                            }
                        }
                    },
                    Background = Avalonia.Media.Brush.Parse("#1a1f3a")
                };
                
                await messageBox.ShowDialog(this);
            }
        }

        private async void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            var passwordBox = this.FindControl<TextBox>("PasswordBox");
            _viewModel.NewPassword = passwordBox?.Text ?? "";
            await _viewModel.AddUserAsync();
            if (passwordBox != null) passwordBox.Text = "";
        }

        private async void RequestResetButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.RequestPasswordResetAsync();
        }

        private async void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var resetPasswordBox = this.FindControl<TextBox>("ResetPasswordBox");
            var confirmPasswordBox = this.FindControl<TextBox>("ConfirmResetPasswordBox");
            
            _viewModel.NewPasswordReset = resetPasswordBox?.Text ?? "";
            _viewModel.ConfirmPasswordReset = confirmPasswordBox?.Text ?? "";
            
            await _viewModel.ResetPasswordAsync();
            
            if (resetPasswordBox != null) resetPasswordBox.Text = "";
            if (confirmPasswordBox != null) confirmPasswordBox.Text = "";
        }
    }
}
