using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EDPSystem.ViewModels;

namespace EDPSystem.Views
{
    public partial class RecoveryWindow : Window
    {
        private RecoveryViewModel viewModel;

        public RecoveryWindow()
        {
            AvaloniaXamlLoader.Load(this);
            viewModel = new RecoveryViewModel();
            this.DataContext = viewModel;

            var sendCodeButton = this.FindControl<Button>("SendCodeButton");
            if (sendCodeButton != null)
                sendCodeButton.Click += SendCodeButton_Click;

            var verifyCodeButton = this.FindControl<Button>("VerifyCodeButton");
            if (verifyCodeButton != null)
                verifyCodeButton.Click += VerifyCodeButton_Click;

            var resetPasswordButton = this.FindControl<Button>("ResetPasswordButton");
            if (resetPasswordButton != null)
                resetPasswordButton.Click += ResetPasswordButton_Click;

            var cancelButton = this.FindControl<Button>("CancelButton");
            if (cancelButton != null)
                cancelButton.Click += CancelButton_Click;
        }

        private async void SendCodeButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var result = await viewModel.SendVerificationCodeAsync();
            if (result)
            {
                // Move to Step 2
            }
        }

        private async void VerifyCodeButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var result = await viewModel.VerifyCodeAsync();
            if (result)
            {
                // Move to Step 3
            }
        }

        private async void ResetPasswordButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var result = await viewModel.ResetPasswordAsync();
            if (result)
            {
                // Show success and close after delay
                await Task.Delay(2000);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            viewModel.Reset();
            this.Close();
        }
    }
}
