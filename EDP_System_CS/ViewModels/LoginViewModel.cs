using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EDPSystem.Models;
using EDPSystem.Services;

namespace EDPSystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading;
        private User? _currentUser = null;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public User? CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public LoginViewModel(DatabaseService? databaseService = null)
        {
            _databaseService = databaseService ?? new DatabaseService();
        }

        public async Task<bool> LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username and password are required.";
                return false;
            }

            IsLoading = true;
            try
            {
                // Authenticate against the database
                var user = await _databaseService.AuthenticateUserAsync(Username, Password);
                
                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        ErrorMessage = "Your account is inactive. Please contact an administrator.";
                        return false;
                    }

                    CurrentUser = user;
                    ErrorMessage = string.Empty;
                    return true;
                }
                else
                {
                    ErrorMessage = "Invalid username or password.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login error: {ex.Message}";
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> TestDatabaseConnectionAsync()
        {
            return await _databaseService.TestConnectionAsync();
        }
    }
}
