using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using EDPSystem.Models;

namespace EDPSystem.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string server = "localhost", string database = "edp_system", string user = "root", string password = "")
        {
            _connectionString = $"Server={server};Port=3308;Database={database};Uid={user};Pwd={password};";
            // Initialize users in background - don't block UI thread
            _ = InitializeDefaultUsersAsync();
        }

        /// <summary>
        /// Initializes default test users if the users table is empty
        /// </summary>
        private async Task InitializeDefaultUsersAsync()
        {
            try
            {
                var users = await GetAllUsersAsync();
                if (users.Count == 0)
                {
                    // Create default test users
                    await CreateUserAsync("admin", "Admin", "User", "admin@edp.local", "admin");
                    await CreateUserAsync("user1", "John", "Doe", "john@edp.local", "password1");
                    await CreateUserAsync("user2", "Jane", "Smith", "jane@edp.local", "password1");
                }
            }
            catch
            {
                // Silently fail - users table might not exist yet
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<SystemStat>> GetStatsAsync()
        {
            try
            {
                var stats = new List<SystemStat>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("SELECT id, stat_name, stat_value, stat_trend, stat_icon, stat_color FROM system_stats", conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    stats.Add(new SystemStat
                    {
                        Id = reader.GetInt32("id"),
                        StatName = reader.GetString("stat_name"),
                        StatValue = reader.GetString("stat_value"),
                        StatTrend = reader.GetString("stat_trend"),
                        StatIcon = reader.GetString("stat_icon"),
                        StatColor = reader.GetString("stat_color")
                    });
                }
                return stats;
            }
            catch
            {
                return GetMockStats();
            }
        }

        public async Task<List<ThroughputLog>> GetThroughputLogsAsync()
        {
            try
            {
                var logs = new List<ThroughputLog>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("SELECT id, log_time, data_rate FROM throughput_logs ORDER BY log_time ASC", conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    logs.Add(new ThroughputLog
                    {
                        Id = reader.GetInt32("id"),
                        LogTime = reader.GetTimeSpan("log_time"),
                        DataRate = reader.GetDecimal("data_rate")
                    });
                }
                return logs;
            }
            catch
            {
                return GetMockThroughputLogs();
            }
        }

        public async Task<List<SystemLog>> GetSystemLogsAsync()
        {
            try
            {
                var logs = new List<SystemLog>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("SELECT id, log_title, log_desc, log_type, log_time FROM system_logs ORDER BY log_time DESC LIMIT 50", conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    logs.Add(new SystemLog
                    {
                        Id = reader.GetInt32("id"),
                        LogTitle = reader.GetString("log_title"),
                        LogDesc = reader.GetString("log_desc"),
                        LogType = reader.GetString("log_type"),
                        LogTime = reader.GetDateTime("log_time")
                    });
                }
                return logs;
            }
            catch
            {
                return GetMockSystemLogs();
            }
        }

        public async Task<List<IntelligenceBatch>> GetReportsAsync()
        {
            try
            {
                var reports = new List<IntelligenceBatch>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("SELECT id, batch_id, source, priority, created_at FROM intelligence_batches ORDER BY created_at DESC", conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    reports.Add(new IntelligenceBatch
                    {
                        Id = reader.GetInt32("id"),
                        BatchId = reader.GetString("batch_id"),
                        Source = reader.GetString("source"),
                        Priority = reader.GetString("priority"),
                        CreatedAt = reader.GetDateTime("created_at")
                    });
                }
                return reports;
            }
            catch
            {
                return GetMockReports();
            }
        }

        public async Task<List<IntelligenceBatch>> GetReportsFilteredAsync(string priority = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            try
            {
                var reports = new List<IntelligenceBatch>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                string query = "SELECT id, batch_id, source, priority, created_at FROM intelligence_batches WHERE 1=1";
                if (!string.IsNullOrEmpty(priority))
                    query += $" AND priority = '{priority}'";
                if (dateFrom.HasValue)
                    query += $" AND created_at >= '{dateFrom:yyyy-MM-dd}'";
                if (dateTo.HasValue)
                    query += $" AND created_at <= '{dateTo:yyyy-MM-dd}'";
                query += " ORDER BY created_at DESC";

                using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    reports.Add(new IntelligenceBatch
                    {
                        Id = reader.GetInt32("id"),
                        BatchId = reader.GetString("batch_id"),
                        Source = reader.GetString("source"),
                        Priority = reader.GetString("priority"),
                        CreatedAt = reader.GetDateTime("created_at")
                    });
                }
                return reports;
            }
            catch
            {
                return GetMockReports();
            }
        }

        // Mock Data Methods (Fallback)
        private List<SystemStat> GetMockStats()
        {
            return new List<SystemStat>
            {
                new SystemStat { Id = 1, StatName = "Data Packets", StatValue = "4.2 PB", StatTrend = "+12%", StatIcon = "📦", StatColor = "#00F3FF" },
                new SystemStat { Id = 2, StatName = "Active Links", StatValue = "89", StatTrend = "+5%", StatIcon = "🔗", StatColor = "#BC13FE" },
                new SystemStat { Id = 3, StatName = "Breach Intel", StatValue = "2", StatTrend = "+1%", StatIcon = "⚠️", StatColor = "#FF006E" }
            };
        }

        private List<ThroughputLog> GetMockThroughputLogs()
        {
            var logs = new List<ThroughputLog>();
            var random = new Random();
            for (int i = 0; i < 24; i++)
            {
                logs.Add(new ThroughputLog
                {
                    Id = i,
                    LogTime = TimeSpan.FromHours(i),
                    DataRate = (decimal)(random.NextDouble() * 50 + 20) // 20-70 units
                });
            }
            return logs;
        }

        private List<SystemLog> GetMockSystemLogs()
        {
            return new List<SystemLog>
            {
                new SystemLog { Id = 1, LogTitle = "System Sync", LogDesc = "Neural link synchronized", LogType = "sync", LogTime = DateTime.Now.AddMinutes(-5) },
                new SystemLog { Id = 2, LogTitle = "Data Report", LogDesc = "Weekly report generated", LogType = "report", LogTime = DateTime.Now.AddHours(-2) },
                new SystemLog { Id = 3, LogTitle = "Backup Complete", LogDesc = "Encrypted backup finished", LogType = "backup", LogTime = DateTime.Now.AddDays(-1) }
            };
        }

        private List<IntelligenceBatch> GetMockReports()
        {
            return new List<IntelligenceBatch>
            {
                new IntelligenceBatch { Id = 1, BatchId = "BATCH-001", Source = "Neural Net Alpha", Priority = "CRITICAL", CreatedAt = DateTime.Now.AddDays(-1) },
                new IntelligenceBatch { Id = 2, BatchId = "BATCH-002", Source = "Signal Processor", Priority = "HIGH", CreatedAt = DateTime.Now.AddDays(-2) },
                new IntelligenceBatch { Id = 3, BatchId = "BATCH-003", Source = "Data Center Beta", Priority = "MEDIUM", CreatedAt = DateTime.Now.AddDays(-3) },
                new IntelligenceBatch { Id = 4, BatchId = "BATCH-004", Source = "Archive System", Priority = "LOW", CreatedAt = DateTime.Now.AddDays(-4) }
            };
        }

        // User Authentication & Management Methods
        
        /// <summary>
        /// Authenticates a user with username and password
        /// </summary>
        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand(
                    "SELECT UserID, Username, FirstName, LastName, Email, PasswordHash, IsActive, CreatedAt, UpdatedAt, LastLogin FROM users WHERE Username = @username",
                    conn);
                cmd.Parameters.AddWithValue("@username", username);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var passwordHash = reader.GetString("PasswordHash");
                    if (VerifyPassword(password, passwordHash))
                    {
                        var user = new User
                        {
                            UserID = reader.GetInt32("UserID"),
                            Username = reader.GetString("Username"),
                            FirstName = reader.GetString("FirstName"),
                            LastName = reader.GetString("LastName"),
                            Email = reader.GetString("Email"),
                            IsActive = reader.GetBoolean("IsActive"),
                            CreatedAt = reader.GetDateTime("CreatedAt"),
                            UpdatedAt = reader.GetDateTime("UpdatedAt"),
                            LastLogin = reader.IsDBNull("LastLogin") ? null : reader.GetDateTime("LastLogin")
                        };

                        // Update last login
                        await UpdateLastLoginAsync(user.UserID);
                        return user;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var users = new List<User>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand(
                    "SELECT UserID, Username, FirstName, LastName, Email, IsActive, CreatedAt, UpdatedAt, LastLogin FROM users ORDER BY CreatedAt DESC",
                    conn);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new User
                    {
                        UserID = reader.GetInt32("UserID"),
                        Username = reader.GetString("Username"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        IsActive = reader.GetBoolean("IsActive"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        UpdatedAt = reader.GetDateTime("UpdatedAt"),
                        LastLogin = reader.IsDBNull("LastLogin") ? null : reader.GetDateTime("LastLogin")
                    });
                }
                return users;
            }
            catch
            {
                return new List<User>();
            }
        }

        /// <summary>
        /// Searches users by username, first name, or last name
        /// </summary>
        public async Task<List<User>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                var users = new List<User>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand(
                    "SELECT UserID, Username, FirstName, LastName, Email, IsActive, CreatedAt, UpdatedAt, LastLogin FROM users " +
                    "WHERE Username LIKE @search OR FirstName LIKE @search OR LastName LIKE @search OR Email LIKE @search " +
                    "ORDER BY CreatedAt DESC",
                    conn);
                cmd.Parameters.AddWithValue("@search", $"%{searchTerm}%");

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new User
                    {
                        UserID = reader.GetInt32("UserID"),
                        Username = reader.GetString("Username"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        IsActive = reader.GetBoolean("IsActive"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        UpdatedAt = reader.GetDateTime("UpdatedAt"),
                        LastLogin = reader.IsDBNull("LastLogin") ? null : reader.GetDateTime("LastLogin")
                    });
                }
                return users;
            }
            catch
            {
                return new List<User>();
            }
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand(
                    "SELECT UserID, Username, FirstName, LastName, Email, IsActive, CreatedAt, UpdatedAt, LastLogin FROM users WHERE UserID = @id",
                    conn);
                cmd.Parameters.AddWithValue("@id", userId);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new User
                    {
                        UserID = reader.GetInt32("UserID"),
                        Username = reader.GetString("Username"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        IsActive = reader.GetBoolean("IsActive"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        UpdatedAt = reader.GetDateTime("UpdatedAt"),
                        LastLogin = reader.IsDBNull("LastLogin") ? null : reader.GetDateTime("LastLogin")
                    };
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        public async Task<bool> CreateUserAsync(string username, string firstName, string lastName, string email, string password)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                var passwordHash = HashPassword(password);

                using var cmd = new MySqlCommand(
                    "INSERT INTO users (Username, FirstName, LastName, Email, PasswordHash, IsActive) " +
                    "VALUES (@username, @firstName, @lastName, @email, @passwordHash, 1)",
                    conn);

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@email", email ?? "");
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        public async Task<bool> UpdateUserAsync(int userId, string firstName, string lastName, string email)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand(
                    "UPDATE users SET FirstName = @firstName, LastName = @lastName, Email = @email, UpdatedAt = NOW() WHERE UserID = @userId",
                    conn);

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@email", email ?? "");

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Activates or deactivates a user
        /// </summary>
        public async Task<bool> SetUserActiveStatusAsync(int userId, bool isActive)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand(
                    "UPDATE users SET IsActive = @isActive, UpdatedAt = NOW() WHERE UserID = @userId",
                    conn);

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@isActive", isActive ? 1 : 0);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand("DELETE FROM users WHERE UserID = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Changes user password
        /// </summary>
        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand("SELECT PasswordHash FROM users WHERE UserID = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                var passwordHash = await cmd.ExecuteScalarAsync() as string;
                if (!VerifyPassword(oldPassword, passwordHash))
                    return false;

                var newPasswordHash = HashPassword(newPassword);

                using var updateCmd = new MySqlCommand(
                    "UPDATE users SET PasswordHash = @passwordHash, UpdatedAt = NOW() WHERE UserID = @userId",
                    conn);

                updateCmd.Parameters.AddWithValue("@userId", userId);
                updateCmd.Parameters.AddWithValue("@passwordHash", newPasswordHash);

                await updateCmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Requests a password reset with a token
        /// </summary>
        public async Task<string> RequestPasswordResetAsync(string username)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                var resetToken = GenerateResetToken();
                var tokenExpiry = DateTime.Now.AddHours(24);

                using var cmd = new MySqlCommand(
                    "UPDATE users SET ResetToken = @token, ResetTokenExpiry = @expiry WHERE Username = @username",
                    conn);

                cmd.Parameters.AddWithValue("@token", resetToken);
                cmd.Parameters.AddWithValue("@expiry", tokenExpiry);
                cmd.Parameters.AddWithValue("@username", username);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0 ? resetToken : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Resets user password using reset token
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string username, string resetToken, string newPassword)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                // Verify token and check expiry
                using var cmd = new MySqlCommand(
                    "SELECT ResetTokenExpiry FROM users WHERE Username = @username AND ResetToken = @token",
                    conn);

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@token", resetToken);

                var expiry = await cmd.ExecuteScalarAsync();
                if (expiry == null || Convert.ToDateTime(expiry) < DateTime.Now)
                    return false;

                // Update password and clear reset token
                var newPasswordHash = HashPassword(newPassword);

                using var updateCmd = new MySqlCommand(
                    "UPDATE users SET PasswordHash = @passwordHash, ResetToken = NULL, ResetTokenExpiry = NULL, UpdatedAt = NOW() " +
                    "WHERE Username = @username AND ResetToken = @token",
                    conn);

                updateCmd.Parameters.AddWithValue("@username", username);
                updateCmd.Parameters.AddWithValue("@token", resetToken);
                updateCmd.Parameters.AddWithValue("@passwordHash", newPasswordHash);

                await updateCmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Helper Methods for Password Hashing
        
        /// <summary>
        /// Hashes a password using SHA256
        /// </summary>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(hashedBytes);
            }
        }

        /// <summary>
        /// Verifies a password against a hash
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Generates a reset token
        /// </summary>
        private string GenerateResetToken()
        {
            var randomNumber = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Updates the LastLogin timestamp for a user
        /// </summary>
        private async Task UpdateLastLoginAsync(int userId)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new MySqlCommand("UPDATE users SET LastLogin = NOW() WHERE UserID = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch { }
        }

        // ============ TRANSACTION METHODS (ACTIVITY 5) ============

        /// <summary>
        /// Gets all book borrowing records
        /// </summary>
        public async Task<List<BookBorrow>> GetAllBookBorrowsAsync()
        {
            try
            {
                var borrows = new List<BookBorrow>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand("SELECT BorrowID, BookTitle, ISBN, BorrowerName, BorrowDate, DueDate, ReturnDate, Status, LateFee FROM book_borrows ORDER BY BorrowDate DESC", conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    borrows.Add(new BookBorrow
                    {
                        BorrowID = reader.GetInt32("BorrowID"),
                        BookTitle = reader.GetString("BookTitle"),
                        ISBN = reader.GetString("ISBN"),
                        BorrowerName = reader.GetString("BorrowerName"),
                        BorrowDate = reader.GetDateTime("BorrowDate"),
                        DueDate = reader.GetDateTime("DueDate"),
                        ReturnDate = reader.IsDBNull("ReturnDate") ? null : reader.GetDateTime("ReturnDate"),
                        Status = reader.GetString("Status"),
                        LateFee = reader.GetDecimal("LateFee")
                    });
                }
                return borrows;
            }
            catch
            {
                return GetMockBookBorrows();
            }
        }

        /// <summary>
        /// Adds a new book borrow record
        /// </summary>
        public async Task<bool> AddBookBorrowAsync(BookBorrow borrow)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand(
                    "INSERT INTO book_borrows (BookTitle, ISBN, BorrowerName, BorrowDate, DueDate, Status) VALUES (@title, @isbn, @borrower, @borrowDate, @dueDate, @status)",
                    conn);
                cmd.Parameters.AddWithValue("@title", borrow.BookTitle);
                cmd.Parameters.AddWithValue("@isbn", borrow.ISBN);
                cmd.Parameters.AddWithValue("@borrower", borrow.BorrowerName);
                cmd.Parameters.AddWithValue("@borrowDate", borrow.BorrowDate);
                cmd.Parameters.AddWithValue("@dueDate", borrow.DueDate);
                cmd.Parameters.AddWithValue("@status", borrow.Status ?? "Active");
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Gets all sales transactions
        /// </summary>
        public async Task<List<SalesTransaction>> GetAllSalesTransactionsAsync()
        {
            try
            {
                var transactions = new List<SalesTransaction>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand(
                    "SELECT TransactionID, InvoiceNumber, ProductName, CustomerName, Quantity, UnitPrice, TotalAmount, TransactionDate, PaymentMethod, Status FROM sales_transactions ORDER BY TransactionDate DESC",
                    conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    transactions.Add(new SalesTransaction
                    {
                        TransactionID = reader.GetInt32("TransactionID"),
                        InvoiceNumber = reader.GetString("InvoiceNumber"),
                        ProductName = reader.GetString("ProductName"),
                        CustomerName = reader.GetString("CustomerName"),
                        Quantity = reader.GetInt32("Quantity"),
                        UnitPrice = reader.GetDecimal("UnitPrice"),
                        TotalAmount = reader.GetDecimal("TotalAmount"),
                        TransactionDate = reader.GetDateTime("TransactionDate"),
                        PaymentMethod = reader.GetString("PaymentMethod"),
                        Status = reader.GetString("Status")
                    });
                }
                return transactions;
            }
            catch
            {
                return GetMockSalesTransactions();
            }
        }

        /// <summary>
        /// Adds a new sales transaction
        /// </summary>
        public async Task<bool> AddSalesTransactionAsync(SalesTransaction transaction)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand(
                    "INSERT INTO sales_transactions (InvoiceNumber, ProductName, CustomerName, Quantity, UnitPrice, TotalAmount, TransactionDate, PaymentMethod, Status) " +
                    "VALUES (@invoice, @product, @customer, @qty, @price, @total, @date, @payment, @status)",
                    conn);
                cmd.Parameters.AddWithValue("@invoice", transaction.InvoiceNumber);
                cmd.Parameters.AddWithValue("@product", transaction.ProductName);
                cmd.Parameters.AddWithValue("@customer", transaction.CustomerName);
                cmd.Parameters.AddWithValue("@qty", transaction.Quantity);
                cmd.Parameters.AddWithValue("@price", transaction.UnitPrice);
                cmd.Parameters.AddWithValue("@total", transaction.TotalAmount);
                cmd.Parameters.AddWithValue("@date", transaction.TransactionDate);
                cmd.Parameters.AddWithValue("@payment", transaction.PaymentMethod);
                cmd.Parameters.AddWithValue("@status", transaction.Status ?? "Completed");
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Gets all inventory count records
        /// </summary>
        public async Task<List<InventoryCount>> GetAllInventoryCountsAsync()
        {
            try
            {
                var counts = new List<InventoryCount>();
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand(
                    "SELECT CountID, ItemCode, ItemName, PhysicalCount, SystemCount, Variance, CountDate, CountedBy, Remarks, Location FROM inventory_counts ORDER BY CountDate DESC",
                    conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    counts.Add(new InventoryCount
                    {
                        CountID = reader.GetInt32("CountID"),
                        ItemCode = reader.GetString("ItemCode"),
                        ItemName = reader.GetString("ItemName"),
                        PhysicalCount = reader.GetInt32("PhysicalCount"),
                        SystemCount = reader.GetInt32("SystemCount"),
                        Variance = reader.GetInt32("Variance"),
                        CountDate = reader.GetDateTime("CountDate"),
                        CountedBy = reader.GetString("CountedBy"),
                        Remarks = reader.IsDBNull("Remarks") ? string.Empty : reader.GetString("Remarks"),
                        Location = reader.IsDBNull("Location") ? string.Empty : reader.GetString("Location")
                    });
                }
                return counts;
            }
            catch
            {
                return GetMockInventoryCounts();
            }
        }

        /// <summary>
        /// Adds a new inventory count record
        /// </summary>
        public async Task<bool> AddInventoryCountAsync(InventoryCount count)
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new MySqlCommand(
                    "INSERT INTO inventory_counts (ItemCode, ItemName, PhysicalCount, SystemCount, Variance, CountDate, CountedBy, Remarks, Location) " +
                    "VALUES (@code, @name, @physical, @system, @variance, @date, @counted, @remarks, @location)",
                    conn);
                cmd.Parameters.AddWithValue("@code", count.ItemCode);
                cmd.Parameters.AddWithValue("@name", count.ItemName);
                cmd.Parameters.AddWithValue("@physical", count.PhysicalCount);
                cmd.Parameters.AddWithValue("@system", count.SystemCount);
                cmd.Parameters.AddWithValue("@variance", count.Variance);
                cmd.Parameters.AddWithValue("@date", count.CountDate);
                cmd.Parameters.AddWithValue("@counted", count.CountedBy);
                cmd.Parameters.AddWithValue("@remarks", count.Remarks ?? string.Empty);
                cmd.Parameters.AddWithValue("@location", count.Location ?? string.Empty);
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch { return false; }
        }

        // Mock Data Methods for Transactions
        private List<BookBorrow> GetMockBookBorrows()
        {
            return new List<BookBorrow>
            {
                new BookBorrow { BorrowID = 1, BookTitle = "The Great Gatsby", ISBN = "9780743273565", BorrowerName = "John Smith", BorrowDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(-8), ReturnDate = DateTime.Now.AddDays(-7), Status = "Returned", LateFee = 0.00M },
                new BookBorrow { BorrowID = 2, BookTitle = "To Kill a Mockingbird", ISBN = "9780060935467", BorrowerName = "Maria Garcia", BorrowDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(-3), ReturnDate = null, Status = "Overdue", LateFee = 5.00M },
                new BookBorrow { BorrowID = 3, BookTitle = "1984", ISBN = "9780451524935", BorrowerName = "Alex Johnson", BorrowDate = DateTime.Now.AddDays(-5), DueDate = DateTime.Now.AddDays(2), ReturnDate = null, Status = "Active", LateFee = 0.00M }
            };
        }

        private List<SalesTransaction> GetMockSalesTransactions()
        {
            return new List<SalesTransaction>
            {
                new SalesTransaction { TransactionID = 1, InvoiceNumber = "INV-001", ProductName = "Laptop Pro 15", CustomerName = "Tech Solutions Inc", Quantity = 2, UnitPrice = 1299.99M, TotalAmount = 2599.98M, TransactionDate = DateTime.Now.AddDays(-10), PaymentMethod = "Card", Status = "Completed" },
                new SalesTransaction { TransactionID = 2, InvoiceNumber = "INV-002", ProductName = "Office Chair", CustomerName = "John Enterprises", Quantity = 5, UnitPrice = 199.99M, TotalAmount = 999.95M, TransactionDate = DateTime.Now.AddDays(-8), PaymentMethod = "Cash", Status = "Completed" },
                new SalesTransaction { TransactionID = 3, InvoiceNumber = "INV-003", ProductName = "Desk Monitor 27\"", CustomerName = "Design Studio LLC", Quantity = 3, UnitPrice = 349.99M, TotalAmount = 1049.97M, TransactionDate = DateTime.Now.AddDays(-5), PaymentMethod = "Card", Status = "Completed" }
            };
        }

        private List<InventoryCount> GetMockInventoryCounts()
        {
            return new List<InventoryCount>
            {
                new InventoryCount { CountID = 1, ItemCode = "SKU-001", ItemName = "Widget A", PhysicalCount = 95, SystemCount = 100, Variance = -5, CountDate = DateTime.Now.AddDays(-7), CountedBy = "admin", Remarks = "Shelf check complete", Location = "Warehouse A" },
                new InventoryCount { CountID = 2, ItemCode = "SKU-002", ItemName = "Widget B", PhysicalCount = 48, SystemCount = 50, Variance = -2, CountDate = DateTime.Now.AddDays(-7), CountedBy = "admin", Remarks = "Stock verified", Location = "Warehouse A" },
                new InventoryCount { CountID = 3, ItemCode = "SKU-003", ItemName = "Gadget X", PhysicalCount = 157, SystemCount = 150, Variance = 7, CountDate = DateTime.Now.AddDays(-7), CountedBy = "admin", Remarks = "Overstock found", Location = "Warehouse B" }
            };
        }
    }
}
