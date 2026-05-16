-- EDP System Database Schema
-- Run this script to set up the database for the C# WPF application

USE mysql;

-- Create database if not exists
CREATE DATABASE IF NOT EXISTS edp_system;
USE edp_system;

-- Users Table (for authentication and user management)
CREATE TABLE IF NOT EXISTS users (
  UserID INT PRIMARY KEY AUTO_INCREMENT,
  Username VARCHAR(50) NOT NULL UNIQUE,
  FirstName VARCHAR(50) NOT NULL,
  LastName VARCHAR(50) NOT NULL,
  Email VARCHAR(100) UNIQUE,
  PasswordHash VARCHAR(255) NOT NULL,
  IsActive TINYINT(1) DEFAULT 1,
  CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  LastLogin DATETIME,
  ResetToken VARCHAR(255),
  ResetTokenExpiry DATETIME
);

-- System Stats Table
CREATE TABLE IF NOT EXISTS system_stats (
  id INT PRIMARY KEY AUTO_INCREMENT,
  stat_name VARCHAR(100) NOT NULL,
  stat_value VARCHAR(50) NOT NULL,
  stat_trend VARCHAR(20) NOT NULL,
  stat_icon VARCHAR(50),
  stat_color VARCHAR(20)
);

-- Throughput Logs Table
CREATE TABLE IF NOT EXISTS throughput_logs (
  id INT PRIMARY KEY AUTO_INCREMENT,
  log_time TIME NOT NULL,
  data_rate DECIMAL(10,2) NOT NULL
);

-- System Logs Table
CREATE TABLE IF NOT EXISTS system_logs (
  id INT PRIMARY KEY AUTO_INCREMENT,
  log_title VARCHAR(255) NOT NULL,
  log_desc VARCHAR(255),
  log_type VARCHAR(50),
  log_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Intelligence Batches Table
CREATE TABLE IF NOT EXISTS intelligence_batches (
  id INT PRIMARY KEY AUTO_INCREMENT,
  batch_id VARCHAR(20) UNIQUE NOT NULL,
  source VARCHAR(100) NOT NULL,
  priority VARCHAR(20),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Book Borrowing Transactions Table
CREATE TABLE IF NOT EXISTS book_borrows (
  BorrowID INT PRIMARY KEY AUTO_INCREMENT,
  BookTitle VARCHAR(255) NOT NULL,
  ISBN VARCHAR(20),
  BorrowerName VARCHAR(100) NOT NULL,
  BorrowDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  DueDate DATETIME NOT NULL,
  ReturnDate DATETIME,
  Status VARCHAR(20) DEFAULT 'Active',
  LateFee DECIMAL(10, 2) DEFAULT 0.00,
  CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Sales Transactions Table
CREATE TABLE IF NOT EXISTS sales_transactions (
  TransactionID INT PRIMARY KEY AUTO_INCREMENT,
  InvoiceNumber VARCHAR(50) UNIQUE NOT NULL,
  ProductName VARCHAR(255) NOT NULL,
  CustomerName VARCHAR(100) NOT NULL,
  Quantity INT NOT NULL,
  UnitPrice DECIMAL(10, 2) NOT NULL,
  TotalAmount DECIMAL(10, 2) NOT NULL,
  TransactionDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PaymentMethod VARCHAR(50),
  Status VARCHAR(20) DEFAULT 'Completed',
  CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Inventory Count Table
CREATE TABLE IF NOT EXISTS inventory_counts (
  CountID INT PRIMARY KEY AUTO_INCREMENT,
  ItemCode VARCHAR(50) NOT NULL,
  ItemName VARCHAR(255) NOT NULL,
  PhysicalCount INT NOT NULL,
  SystemCount INT NOT NULL,
  Variance INT NOT NULL,
  CountDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CountedBy VARCHAR(100) NOT NULL,
  Remarks TEXT,
  Location VARCHAR(100),
  CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insert Mock Data

-- Insert sample users (passwords are hashed with SHA256)
-- admin/admin, user1/password1, user2/password2
INSERT IGNORE INTO users (Username, FirstName, LastName, Email, PasswordHash, IsActive) VALUES
('admin', 'Admin', 'User', 'admin@edp.local', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 1),
('user1', 'John', 'Doe', 'john@edp.local', '5f4dcc3b5aa765d61d8327deb882cf99', 1),
('user2', 'Jane', 'Smith', 'jane@edp.local', '5f4dcc3b5aa765d61d8327deb882cf99', 1);

INSERT IGNORE INTO system_stats (stat_name, stat_value, stat_trend, stat_icon, stat_color) VALUES
('Data Packets', '4.2 PB', '+12%', '📦', '#00F3FF'),
('Active Links', '89', '+5%', '🔗', '#BC13FE'),
('Breach Intel', '2', '+1%', '⚠️', '#FF006E');

INSERT IGNORE INTO throughput_logs (log_time, data_rate) VALUES
('00:00:00', 25.5),
('01:00:00', 32.8),
('02:00:00', 28.3),
('03:00:00', 35.6),
('04:00:00', 22.1),
('05:00:00', 38.9),
('06:00:00', 31.2),
('07:00:00', 41.5),
('08:00:00', 35.8),
('09:00:00', 44.2),
('10:00:00', 39.7),
('11:00:00', 33.5),
('12:00:00', 46.8),
('13:00:00', 42.1),
('14:00:00', 37.6),
('15:00:00', 50.3),
('16:00:00', 45.9),
('17:00:00', 40.2),
('18:00:00', 48.5),
('19:00:00', 43.7),
('20:00:00', 38.1),
('21:00:00', 36.4),
('22:00:00', 29.8),
('23:00:00', 26.5);

INSERT IGNORE INTO system_logs (log_title, log_desc, log_type, log_time) VALUES
('System Sync', 'Neural link synchronized with all nodes', 'sync', NOW()),
('Data Batch Complete', 'Weekly intelligence report compiled', 'report', DATE_SUB(NOW(), INTERVAL 2 HOUR)),
('Backup Finished', 'Encrypted data backup completed successfully', 'backup', DATE_SUB(NOW(), INTERVAL 1 DAY));

INSERT IGNORE INTO intelligence_batches (batch_id, source, priority, created_at) VALUES
('BATCH-001', 'Neural Net Alpha', 'CRITICAL', DATE_SUB(NOW(), INTERVAL 1 DAY)),
('BATCH-002', 'Signal Processor', 'HIGH', DATE_SUB(NOW(), INTERVAL 2 DAY)),
('BATCH-003', 'Data Center Beta', 'MEDIUM', DATE_SUB(NOW(), INTERVAL 3 DAY)),
('BATCH-004', 'Archive System', 'LOW', DATE_SUB(NOW(), INTERVAL 4 DAY)),
('BATCH-005', 'Neural Net Gamma', 'HIGH', DATE_SUB(NOW(), INTERVAL 5 DAY)),
('BATCH-006', 'Signal Processor', 'CRITICAL', DATE_SUB(NOW(), INTERVAL 6 DAY)),
('BATCH-007', 'Data Center Omega', 'MEDIUM', DATE_SUB(NOW(), INTERVAL 7 DAY)),
('BATCH-008', 'Archive System', 'LOW', DATE_SUB(NOW(), INTERVAL 8 DAY));

-- Sample Book Borrowing Data
INSERT IGNORE INTO book_borrows (BookTitle, ISBN, BorrowerName, BorrowDate, DueDate, ReturnDate, Status, LateFee) VALUES
('The Great Gatsby', '9780743273565', 'John Smith', DATE_SUB(NOW(), INTERVAL 15 DAY), DATE_SUB(NOW(), INTERVAL 8 DAY), DATE_SUB(NOW(), INTERVAL 7 DAY), 'Returned', 0.00),
('To Kill a Mockingbird', '9780060935467', 'Maria Garcia', DATE_SUB(NOW(), INTERVAL 10 DAY), DATE_SUB(NOW(), INTERVAL 3 DAY), NULL, 'Overdue', 5.00),
('1984', '9780451524935', 'Alex Johnson', DATE_SUB(NOW(), INTERVAL 5 DAY), DATE_ADD(NOW(), INTERVAL 2 DAY), NULL, 'Active', 0.00),
('Pride and Prejudice', '9780141040349', 'Sarah Wilson', DATE_SUB(NOW(), INTERVAL 20 DAY), DATE_SUB(NOW(), INTERVAL 13 DAY), DATE_SUB(NOW(), INTERVAL 12 DAY), 'Returned', 0.00),
('The Catcher in the Rye', '9780316769174', 'Michael Brown', DATE_SUB(NOW(), INTERVAL 2 DAY), DATE_ADD(NOW(), INTERVAL 5 DAY), NULL, 'Active', 0.00);

-- Sample Sales Transactions
INSERT IGNORE INTO sales_transactions (InvoiceNumber, ProductName, CustomerName, Quantity, UnitPrice, TotalAmount, TransactionDate, PaymentMethod, Status) VALUES
('INV-001', 'Laptop Pro 15', 'Tech Solutions Inc', 2, 1299.99, 2599.98, DATE_SUB(NOW(), INTERVAL 10 DAY), 'Card', 'Completed'),
('INV-002', 'Office Chair', 'John Enterprises', 5, 199.99, 999.95, DATE_SUB(NOW(), INTERVAL 8 DAY), 'Cash', 'Completed'),
('INV-003', 'Desk Monitor 27"', 'Design Studio LLC', 3, 349.99, 1049.97, DATE_SUB(NOW(), INTERVAL 5 DAY), 'Card', 'Completed'),
('INV-004', 'USB-C Cable 2m', 'Electronics Plus', 50, 12.99, 649.50, DATE_SUB(NOW(), INTERVAL 3 DAY), 'Card', 'Completed'),
('INV-005', 'Wireless Mouse', 'Office Supplies Co', 25, 39.99, 999.75, DATE_SUB(NOW(), INTERVAL 1 DAY), 'Cheque', 'Pending'),
('INV-006', 'Mechanical Keyboard', 'Gaming Zone', 10, 129.99, 1299.90, NOW(), 'Card', 'Completed');

-- Sample Inventory Counts
INSERT IGNORE INTO inventory_counts (ItemCode, ItemName, PhysicalCount, SystemCount, Variance, CountDate, CountedBy, Remarks, Location) VALUES
('SKU-001', 'Widget A', 95, 100, -5, DATE_SUB(NOW(), INTERVAL 7 DAY), 'admin', 'Shelf check complete', 'Warehouse A'),
('SKU-002', 'Widget B', 48, 50, -2, DATE_SUB(NOW(), INTERVAL 7 DAY), 'admin', 'Stock verified', 'Warehouse A'),
('SKU-003', 'Gadget X', 157, 150, 7, DATE_SUB(NOW(), INTERVAL 7 DAY), 'admin', 'Overstock found', 'Warehouse B'),
('SKU-004', 'Gadget Y', 72, 75, -3, DATE_SUB(NOW(), INTERVAL 5 DAY), 'user1', 'Minor discrepancy', 'Warehouse B'),
('SKU-005', 'Component Z', 200, 200, 0, DATE_SUB(NOW(), INTERVAL 5 DAY), 'user1', 'Perfect match', 'Warehouse C'),
('SKU-006', 'Part Q', 43, 50, -7, DATE_SUB(NOW(), INTERVAL 2 DAY), 'user1', 'Under investigation', 'Warehouse A');

