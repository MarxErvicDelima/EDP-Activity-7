using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EDPSystem.Models;
using EDPSystem.Services;

namespace EDPSystem.ViewModels
{
    public class UserManagementViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<User> _users = new();
        private User? _selectedUser;
        private string _searchTerm = string.Empty;
        private string _statusMessage = string.Empty;
        private bool _isLoading;
        private User? _currentUser;

        // Form fields
        private string _newUsername = string.Empty;
        private string _newFirstName = string.Empty;
        private string _newLastName = string.Empty;
        private string _newEmail = string.Empty;
        private string _newPassword = string.Empty;
        private string _editFirstName = string.Empty;
        private string _editLastName = string.Empty;
        private string _editEmail = string.Empty;

        // Password recovery fields
        private string _recoveryUsername = string.Empty;
        private string _resetToken = string.Empty;
        private string _newPasswordReset = string.Empty;
        private string _confirmPasswordReset = string.Empty;

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
                if (value != null)
                {
                    EditFirstName = value.FirstName;
                    EditLastName = value.LastName;
                    EditEmail = value.Email;
                }
            }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
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

        // Form properties
        public string NewUsername
        {
            get => _newUsername;
            set => SetProperty(ref _newUsername, value);
        }

        public string NewFirstName
        {
            get => _newFirstName;
            set => SetProperty(ref _newFirstName, value);
        }

        public string NewLastName
        {
            get => _newLastName;
            set => SetProperty(ref _newLastName, value);
        }

        public string NewEmail
        {
            get => _newEmail;
            set => SetProperty(ref _newEmail, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string EditFirstName
        {
            get => _editFirstName;
            set => SetProperty(ref _editFirstName, value);
        }

        public string EditLastName
        {
            get => _editLastName;
            set => SetProperty(ref _editLastName, value);
        }

        public string EditEmail
        {
            get => _editEmail;
            set => SetProperty(ref _editEmail, value);
        }

        // Password recovery properties
        public string RecoveryUsername
        {
            get => _recoveryUsername;
            set => SetProperty(ref _recoveryUsername, value);
        }

        public string ResetToken
        {
            get => _resetToken;
            set => SetProperty(ref _resetToken, value);
        }

        public string NewPasswordReset
        {
            get => _newPasswordReset;
            set => SetProperty(ref _newPasswordReset, value);
        }

        public string ConfirmPasswordReset
        {
            get => _confirmPasswordReset;
            set => SetProperty(ref _confirmPasswordReset, value);
        }

        public UserManagementViewModel(DatabaseService? databaseService = null, User? currentUser = null)
        {
            _databaseService = databaseService ?? new DatabaseService();
            _currentUser = currentUser;
            _users = new ObservableCollection<User>();
        }

        public async Task LoadUsersAsync()
        {
            IsLoading = true;
            try
            {
                var users = await _databaseService.GetAllUsersAsync();
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
                StatusMessage = $"Loaded {users.Count} user(s)";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading users: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SearchUsersAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                await LoadUsersAsync();
                return;
            }

            IsLoading = true;
            try
            {
                var users = await _databaseService.SearchUsersAsync(SearchTerm);
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
                StatusMessage = $"Found {users.Count} user(s) matching '{SearchTerm}'";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error searching users: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task AddUserAsync()
        {
            if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(NewPassword))
            {
                StatusMessage = "Username and password are required.";
                return;
            }

            IsLoading = true;
            try
            {
                var success = await _databaseService.CreateUserAsync(
                    NewUsername, NewFirstName, NewLastName, NewEmail, NewPassword);

                if (success)
                {
                    StatusMessage = "User added successfully!";
                    NewUsername = string.Empty;
                    NewFirstName = string.Empty;
                    NewLastName = string.Empty;
                    NewEmail = string.Empty;
                    NewPassword = string.Empty;
                    await LoadUsersAsync();
                }
                else
                {
                    StatusMessage = "Failed to add user. Username may already exist.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error adding user: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task UpdateUserAsync()
        {
            if (SelectedUser == null)
            {
                StatusMessage = "Please select a user to update.";
                return;
            }

            IsLoading = true;
            try
            {
                var success = await _databaseService.UpdateUserAsync(
                    SelectedUser.UserID, EditFirstName, EditLastName, EditEmail);

                if (success)
                {
                    StatusMessage = "User updated successfully!";
                    await LoadUsersAsync();
                    SelectedUser = null;
                }
                else
                {
                    StatusMessage = "Failed to update user.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating user: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task ActivateUserAsync()
        {
            if (SelectedUser == null)
            {
                StatusMessage = "Please select a user.";
                return;
            }

            IsLoading = true;
            try
            {
                var success = await _databaseService.SetUserActiveStatusAsync(SelectedUser.UserID, true);
                if (success)
                {
                    StatusMessage = $"User {SelectedUser.Username} activated!";
                    await LoadUsersAsync();
                }
                else
                {
                    StatusMessage = "Failed to activate user.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task DeactivateUserAsync()
        {
            if (SelectedUser == null)
            {
                StatusMessage = "Please select a user.";
                return;
            }

            IsLoading = true;
            try
            {
                var success = await _databaseService.SetUserActiveStatusAsync(SelectedUser.UserID, false);
                if (success)
                {
                    StatusMessage = $"User {SelectedUser.Username} deactivated!";
                    await LoadUsersAsync();
                }
                else
                {
                    StatusMessage = "Failed to deactivate user.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task DeleteUserAsync()
        {
            if (SelectedUser == null)
            {
                StatusMessage = "Please select a user to delete.";
                return;
            }

            IsLoading = true;
            try
            {
                var success = await _databaseService.DeleteUserAsync(SelectedUser.UserID);
                if (success)
                {
                    StatusMessage = $"User {SelectedUser.Username} deleted!";
                    await LoadUsersAsync();
                    SelectedUser = null;
                }
                else
                {
                    StatusMessage = "Failed to delete user.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task RequestPasswordResetAsync()
        {
            if (string.IsNullOrWhiteSpace(RecoveryUsername))
            {
                StatusMessage = "Please enter a username.";
                return;
            }

            IsLoading = true;
            try
            {
                var token = await _databaseService.RequestPasswordResetAsync(RecoveryUsername);
                if (token != null)
                {
                    ResetToken = token;
                    StatusMessage = $"Reset token generated. Share this token with the user: {token.Substring(0, 20)}...";
                }
                else
                {
                    StatusMessage = "User not found or reset failed.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task ResetPasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(RecoveryUsername) || string.IsNullOrWhiteSpace(ResetToken) ||
                string.IsNullOrWhiteSpace(NewPasswordReset) || string.IsNullOrWhiteSpace(ConfirmPasswordReset))
            {
                StatusMessage = "Please fill in all fields.";
                return;
            }

            if (NewPasswordReset != ConfirmPasswordReset)
            {
                StatusMessage = "Passwords do not match.";
                return;
            }

            IsLoading = true;
            try
            {
                var success = await _databaseService.ResetPasswordAsync(RecoveryUsername, ResetToken, NewPasswordReset);
                if (success)
                {
                    StatusMessage = "Password reset successfully!";
                    RecoveryUsername = string.Empty;
                    ResetToken = string.Empty;
                    NewPasswordReset = string.Empty;
                    ConfirmPasswordReset = string.Empty;
                }
                else
                {
                    StatusMessage = "Invalid token or token has expired.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
