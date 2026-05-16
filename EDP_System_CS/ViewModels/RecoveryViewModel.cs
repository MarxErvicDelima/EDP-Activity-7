using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EDPSystem.ViewModels
{
    public class RecoveryViewModel : INotifyPropertyChanged
    {
        private string _email = string.Empty;
        private string _verificationCode = string.Empty;
        private string _newPassword = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _errorMessage = string.Empty;
        private string _statusMessage = string.Empty;
        private bool _isStep1 = true;
        private bool _isStep2 = false;
        private bool _isStep3 = false;
        private bool _isProcessing = false;

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string VerificationCode
        {
            get => _verificationCode;
            set
            {
                if (_verificationCode != value)
                {
                    _verificationCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStep1
        {
            get => _isStep1;
            set
            {
                if (_isStep1 != value)
                {
                    _isStep1 = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStep2
        {
            get => _isStep2;
            set
            {
                if (_isStep2 != value)
                {
                    _isStep2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStep3
        {
            get => _isStep3;
            set
            {
                if (_isStep3 != value)
                {
                    _isStep3 = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                if (_isProcessing != value)
                {
                    _isProcessing = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task<bool> SendVerificationCodeAsync()
        {
            ErrorMessage = string.Empty;
            
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Please enter your email address";
                return false;
            }

            IsProcessing = true;

            try
            {
                // Simulate API call
                await Task.Delay(1500);

                // For demo: accept any email containing '@'
                if (!Email.Contains("@"))
                {
                    ErrorMessage = "Invalid email format";
                    IsProcessing = false;
                    return false;
                }

                // Generate demo verification code
                StatusMessage = "Verification code sent to your email (Demo: 123456)";
                IsStep1 = false;
                IsStep2 = true;
                ErrorMessage = string.Empty;

                IsProcessing = false;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                IsProcessing = false;
                return false;
            }
        }

        public async Task<bool> VerifyCodeAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                ErrorMessage = "Please enter the verification code";
                return false;
            }

            IsProcessing = true;

            try
            {
                // Simulate API call
                await Task.Delay(1000);

                // For demo: accept code "123456"
                if (VerificationCode != "123456")
                {
                    ErrorMessage = "Invalid verification code";
                    IsProcessing = false;
                    return false;
                }

                StatusMessage = "Code verified! Please enter your new password";
                IsStep2 = false;
                IsStep3 = true;
                ErrorMessage = string.Empty;

                IsProcessing = false;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                IsProcessing = false;
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                ErrorMessage = "Please enter a new password";
                return false;
            }

            if (NewPassword.Length < 6)
            {
                ErrorMessage = "Password must be at least 6 characters";
                return false;
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match";
                return false;
            }

            IsProcessing = true;

            try
            {
                // Simulate API call to reset password
                await Task.Delay(1500);

                StatusMessage = "Password reset successful! Redirecting to login...";
                ErrorMessage = string.Empty;

                IsProcessing = false;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                IsProcessing = false;
                return false;
            }
        }

        public void Reset()
        {
            Email = string.Empty;
            VerificationCode = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
            ErrorMessage = string.Empty;
            StatusMessage = string.Empty;
            IsStep1 = true;
            IsStep2 = false;
            IsStep3 = false;
            IsProcessing = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
