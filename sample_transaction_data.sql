-- ============================================
-- EDP SYSTEM - SAMPLE TRANSACTION DATA
-- ============================================
-- This script inserts sample data into the three transaction tables
-- for testing and demonstration purposes

-- ============================================
-- BOOK BORROWS TABLE - Sample Data
-- ============================================
INSERT INTO book_borrows (book_title, isbn, borrower_name, borrow_date, due_date, return_date, status, late_fee) VALUES
('The Art of Computer Programming', '978-0-201-61915-7', 'John Smith', '2026-05-01', '2026-05-15', NULL, 'Active', 0.00),
('Clean Code: A Handbook of Agile Software', '978-0-132-35088-2', 'Sarah Johnson', '2026-04-20', '2026-05-04', '2026-05-05', 'Overdue', 5.50),
('Design Patterns: Elements of Reusable Object-Oriented Software', '978-0-201-63361-0', 'Michael Chen', '2026-04-15', '2026-04-29', '2026-04-28', 'Returned', 0.00),
('The Pragmatic Programmer', '978-0-135-95705-9', 'Emily Rodriguez', '2026-05-03', '2026-05-17', NULL, 'Active', 0.00),
('Introduction to Algorithms', '978-0-262-03384-8', 'David Thompson', '2026-04-10', '2026-04-24', '2026-04-23', 'Returned', 0.00),
('Code Complete: A Practical Handbook', '978-0-735-61966-8', 'Lisa Anderson', '2026-04-25', '2026-05-09', NULL, 'Active', 0.00),
('Refactoring: Improving the Design of Existing Code', '978-0-201-48567-7', 'James Wilson', '2026-04-18', '2026-05-02', '2026-05-08', 'Overdue', 15.75),
('The C Programming Language', '978-0-131-10362-7', 'Patricia Garcia', '2026-05-02', '2026-05-16', NULL, 'Active', 0.00),
('Java: The Complete Reference', '978-0-078-22229-4', 'Robert Martinez', '2026-04-28', '2026-05-12', NULL, 'Active', 0.00),
('Cracking the Coding Interview', '978-0-984-78208-4', 'Jennifer Lee', '2026-04-05', '2026-04-19', '2026-04-21', 'Returned', 0.00);

-- ============================================
-- SALES TRANSACTIONS TABLE - Sample Data
-- ============================================
INSERT INTO sales_transactions (invoice_number, product_name, customer_name, quantity, unit_price, total_amount, transaction_date, payment_method, status) VALUES
('INV-2026-0001', 'Enterprise Server License', 'Tech Solutions Inc.', 5, 2500.00, 12500.00, '2026-05-08', 'Bank Transfer', 'Completed'),
('INV-2026-0002', 'Software Development Kit', 'StartUp Ventures LLC', 10, 499.99, 4999.90, '2026-05-07', 'Credit Card', 'Completed'),
('INV-2026-0003', 'Database Management System', 'Corporate Analytics Ltd.', 3, 3500.00, 10500.00, '2026-05-06', 'Bank Transfer', 'Completed'),
('INV-2026-0004', 'Cloud Storage Package', 'Digital Marketing Agency', 20, 150.00, 3000.00, '2026-05-05', 'Credit Card', 'Completed'),
('INV-2026-0005', 'Security Software Suite', 'Financial Services Corp.', 7, 1200.00, 8400.00, '2026-05-04', 'Bank Transfer', 'Completed'),
('INV-2026-0006', 'API Integration Service', 'E-Commerce Platform', 2, 2000.00, 4000.00, '2026-05-03', 'Credit Card', 'Pending'),
('INV-2026-0007', 'Mobile Application Framework', 'App Development Studio', 15, 399.99, 5999.85, '2026-05-02', 'Bank Transfer', 'Completed'),
('INV-2026-0008', 'Data Analytics Dashboard', 'Business Intelligence Group', 1, 5000.00, 5000.00, '2026-05-01', 'Credit Card', 'Pending'),
('INV-2026-0009', 'Monitoring & Logging Platform', 'DevOps Solutions Inc.', 4, 1500.00, 6000.00, '2026-04-30', 'Bank Transfer', 'Completed'),
('INV-2026-0010', 'AI Machine Learning Toolkit', 'Research Institute', 6, 3000.00, 18000.00, '2026-04-29', 'Credit Card', 'Completed'),
('INV-2026-0011', 'Backup & Recovery System', 'Data Center Operations', 2, 4500.00, 9000.00, '2026-04-28', 'Bank Transfer', 'Completed'),
('INV-2026-0012', 'User Management System', 'HR Tech Solutions', 8, 800.00, 6400.00, '2026-04-27', 'Credit Card', 'Completed');

-- ============================================
-- INVENTORY COUNTS TABLE - Sample Data
-- ============================================
INSERT INTO inventory_counts (item_code, item_name, physical_count, system_count, variance, count_date, counted_by, location) VALUES
('PRD-001', 'Server Hardware Units', 45, 45, 0, '2026-05-08', 'John Smith', 'Warehouse A - Section 1'),
('PRD-002', 'Network Equipment Kits', 32, 35, -3, '2026-05-08', 'Maria Garcia', 'Warehouse A - Section 2'),
('PRD-003', 'Database Licenses', 120, 120, 0, '2026-05-08', 'David Lee', 'Storage Room B'),
('PRD-004', 'Software Installation Packs', 67, 65, 2, '2026-05-07', 'Emma White', 'Warehouse C - Section 1'),
('PRD-005', 'Development Tools Bundle', 23, 25, -2, '2026-05-07', 'James Brown', 'Office Supply - Room 3'),
('PRD-006', 'Security Module Components', 89, 90, -1, '2026-05-07', 'Sarah Miller', 'Warehouse A - Section 3'),
('PRD-007', 'API Integration Modules', 15, 15, 0, '2026-05-06', 'Mike Johnson', 'Technical Lab - Rack 5'),
('PRD-008', 'Mobile Framework Packages', 42, 40, 2, '2026-05-06', 'Rachel Davis', 'Warehouse B - Section 2'),
('PRD-009', 'Analytics Dashboard Units', 8, 8, 0, '2026-05-06', 'Chris Wilson', 'High-Value Storage - Safe A'),
('PRD-010', 'Monitoring Probe Devices', 56, 54, 2, '2026-05-05', 'Patricia Adams', 'Warehouse C - Section 2'),
('PRD-011', 'AI Toolkit Licenses', 33, 35, -2, '2026-05-05', 'Kevin Martin', 'Digital License Vault'),
('PRD-012', 'Backup System Components', 27, 27, 0, '2026-05-05', 'Lisa Thompson', 'Warehouse D - Section 1'),
('PRD-013', 'User Management Tokens', 150, 148, 2, '2026-05-04', 'Robert Taylor', 'Authentication Server Room'),
('PRD-014', 'Cloud Storage Modules', 78, 80, -2, '2026-05-04', 'Jennifer White', 'Warehouse B - Section 3'),
('PRD-015', 'Enterprise License Keys', 12, 12, 0, '2026-05-04', 'Daniel Harris', 'Executive Vault - Safe B');

-- ============================================
-- SUMMARY
-- ============================================
-- Book Borrows: 10 records
--   - 5 Active loans
--   - 2 Overdue loans
--   - 3 Returned books
--
-- Sales Transactions: 12 records
--   - 10 Completed transactions (total: $94,899.75)
--   - 2 Pending transactions (total: $9,000.00)
--
-- Inventory Counts: 15 records
--   - 6 items with zero variance (perfect accuracy)
--   - 9 items with variance (1-3 unit discrepancies)
--   - Total variance: 13 units across all items
--   - Accuracy rate: 60% (9/15 items have variance)
-- ============================================
