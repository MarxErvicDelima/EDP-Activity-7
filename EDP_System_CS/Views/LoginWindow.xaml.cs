using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EDPSystem.Services;
using EDPSystem.ViewModels;

namespace EDPSystem.Views
{
    public partial class LoginWindow : Window
    {
        private LoginViewModel viewModel;

        public LoginWindow()
        {
            AvaloniaXamlLoader.Load(this);
            viewModel = new LoginViewModel();
            this.DataContext = viewModel;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Get password from TextBox
            var passwordBox = this.FindControl<TextBox>("PasswordBox");
            if (passwordBox != null)
            {
                viewModel.Password = passwordBox.Text ?? "";
            }

            if (await viewModel.LoginAsync())
            {
                // Navigation to dashboard window with current user
                DashboardWindow dashboardWindow = new DashboardWindow(viewModel.CurrentUser);
                dashboardWindow.Show();
                this.Close();
            }
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // Open password recovery window
            RecoveryWindow recoveryWindow = new RecoveryWindow();
            recoveryWindow.ShowDialog(this);
        }
    }
}

