using System;

namespace EDPSystem.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string ResetToken { get; set; } = string.Empty;
        public DateTime? ResetTokenExpiry { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string Status => IsActive ? "Active" : "Inactive";
    }

    public class SystemStat
    {
        public int Id { get; set; }
        public string StatName { get; set; } = string.Empty;
        public string StatValue { get; set; } = string.Empty;
        public string StatTrend { get; set; } = string.Empty;
        public string StatIcon { get; set; } = string.Empty;
        public string StatColor { get; set; } = string.Empty;
    }

    public class ThroughputLog
    {
        public int Id { get; set; }
        public TimeSpan LogTime { get; set; }
        public decimal DataRate { get; set; }
    }

    public class SystemLog
    {
        public int Id { get; set; }
        public string LogTitle { get; set; } = string.Empty;
        public string LogDesc { get; set; } = string.Empty;
        public string LogType { get; set; } = string.Empty;
        public DateTime LogTime { get; set; }
    }

    public class IntelligenceBatch
    {
        public int Id { get; set; }
        public string BatchId { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    // Transaction Models for Activity 5
    
    public class BookBorrow
    {
        public int BorrowID { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty; // Active, Returned, Overdue
        public decimal LateFee { get; set; }

        public string StatusDisplay => Status ?? "Active";
        public bool IsOverdue => ReturnDate == null && DateTime.Now > DueDate;
    }

    public class SalesTransaction
    {
        public int TransactionID { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Card, Cheque
        public string Status { get; set; } = string.Empty; // Completed, Pending, Cancelled

        public string StatusDisplay => Status ?? "Pending";
    }

    public class InventoryCount
    {
        public int CountID { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int PhysicalCount { get; set; }
        public int SystemCount { get; set; }
        public int Variance { get; set; }
        public DateTime CountDate { get; set; }
        public string CountedBy { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public bool HasVariance => Variance != 0;
        public decimal VariancePercentage => SystemCount > 0 ? (decimal)Variance / SystemCount * 100 : 0;
    }
}
