using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using EDPSystem.Models;
using EDPSystem.Services;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System.Drawing;

namespace EDPSystem.ViewModels
{
    public class TransactionsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<BookBorrow> _bookBorrows = new();
        private ObservableCollection<SalesTransaction> _salesTransactions = new();
        private ObservableCollection<InventoryCount> _inventoryCounts = new();
        private string _selectedTab = "BookBorrows";
        private bool _isLoading;
        private User? _currentUser = null;

        // Company Configuration
        private const string COMPANY_NAME = "EDP INTELLIGENCE SYSTEM";
        private const string COMPANY_ADDRESS = "123 Business Street, Suite 100, New York, NY 10001";
        private const string COMPANY_PHONE = "(555) 123-4567";
        private const string COMPANY_EMAIL = "reports@edpsystem.local";

        // Book Borrow Properties
        private string _borrowerName = string.Empty;
        private string _bookTitle = string.Empty;
        private string _isbn = string.Empty;
        private DateTime _borrowDate = DateTime.Now;
        private DateTime _dueDate = DateTime.Now.AddDays(14);

        // Sales Transaction Properties
        private string _invoiceNumber = string.Empty;
        private string _productName = string.Empty;
        private string _customerName = string.Empty;
        private int _quantity = 1;
        private decimal _unitPrice = 0;
        private string _paymentMethod = "Cash";

        // Inventory Count Properties
        private string _itemCode = string.Empty;
        private string _itemName = string.Empty;
        private int _physicalCount = 0;
        private int _systemCount = 0;
        private string _countedBy = string.Empty;
        private string _location = string.Empty;

        public ObservableCollection<BookBorrow> BookBorrows
        {
            get => _bookBorrows;
            set => SetProperty(ref _bookBorrows, value);
        }

        public ObservableCollection<SalesTransaction> SalesTransactions
        {
            get => _salesTransactions;
            set => SetProperty(ref _salesTransactions, value);
        }

        public ObservableCollection<InventoryCount> InventoryCounts
        {
            get => _inventoryCounts;
            set => SetProperty(ref _inventoryCounts, value);
        }

        public string SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
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

        // Book Borrow Properties
        public string BorrowerName
        {
            get => _borrowerName;
            set => SetProperty(ref _borrowerName, value);
        }

        public string BookTitle
        {
            get => _bookTitle;
            set => SetProperty(ref _bookTitle, value);
        }

        public string ISBN
        {
            get => _isbn;
            set => SetProperty(ref _isbn, value);
        }

        public DateTime BorrowDate
        {
            get => _borrowDate;
            set => SetProperty(ref _borrowDate, value);
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value);
        }

        // Sales Transaction Properties
        public string InvoiceNumber
        {
            get => _invoiceNumber;
            set => SetProperty(ref _invoiceNumber, value);
        }

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value);
        }

        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set => SetProperty(ref _unitPrice, value);
        }

        public string PaymentMethod
        {
            get => _paymentMethod;
            set => SetProperty(ref _paymentMethod, value);
        }

        // Inventory Count Properties
        public string ItemCode
        {
            get => _itemCode;
            set => SetProperty(ref _itemCode, value);
        }

        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        public int PhysicalCount
        {
            get => _physicalCount;
            set => SetProperty(ref _physicalCount, value);
        }

        public int SystemCount
        {
            get => _systemCount;
            set => SetProperty(ref _systemCount, value);
        }

        public string CountedBy
        {
            get => _countedBy;
            set => SetProperty(ref _countedBy, value);
        }

        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        public TransactionsViewModel(DatabaseService? databaseService = null, User? currentUser = null)
        {
            _databaseService = databaseService ?? new DatabaseService();
            _currentUser = currentUser;
            BookBorrows = new ObservableCollection<BookBorrow>();
            SalesTransactions = new ObservableCollection<SalesTransaction>();
            InventoryCounts = new ObservableCollection<InventoryCount>();
        }

        public async Task LoadAllTransactionsAsync()
        {
            IsLoading = true;
            try
            {
                await LoadBookBorrowsAsync();
                await LoadSalesTransactionsAsync();
                await LoadInventoryCountsAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadBookBorrowsAsync()
        {
            try
            {
                var borrows = await _databaseService.GetAllBookBorrowsAsync();
                BookBorrows.Clear();
                foreach (var borrow in borrows)
                    BookBorrows.Add(borrow);
            }
            catch { }
        }

        public async Task LoadSalesTransactionsAsync()
        {
            try
            {
                var transactions = await _databaseService.GetAllSalesTransactionsAsync();
                SalesTransactions.Clear();
                foreach (var transaction in transactions)
                    SalesTransactions.Add(transaction);
            }
            catch { }
        }

        public async Task LoadInventoryCountsAsync()
        {
            try
            {
                var counts = await _databaseService.GetAllInventoryCountsAsync();
                InventoryCounts.Clear();
                foreach (var count in counts)
                    InventoryCounts.Add(count);
            }
            catch { }
        }

        // Add Book Borrow
        public async Task AddBookBorrowAsync()
        {
            if (string.IsNullOrWhiteSpace(BorrowerName) || string.IsNullOrWhiteSpace(BookTitle))
                return;

            var borrow = new BookBorrow
            {
                BookTitle = BookTitle,
                ISBN = ISBN,
                BorrowerName = BorrowerName,
                BorrowDate = BorrowDate,
                DueDate = DueDate,
                Status = "Active",
                LateFee = 0
            };

            if (await _databaseService.AddBookBorrowAsync(borrow))
            {
                await LoadBookBorrowsAsync();
                ClearBookBorrowFields();
            }
        }

        // Add Sales Transaction
        public async Task AddSalesTransactionAsync()
        {
            if (string.IsNullOrWhiteSpace(InvoiceNumber) || string.IsNullOrWhiteSpace(ProductName))
                return;

            var transaction = new SalesTransaction
            {
                InvoiceNumber = InvoiceNumber,
                ProductName = ProductName,
                CustomerName = CustomerName,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                TotalAmount = Quantity * UnitPrice,
                TransactionDate = DateTime.Now,
                PaymentMethod = PaymentMethod,
                Status = "Completed"
            };

            if (await _databaseService.AddSalesTransactionAsync(transaction))
            {
                await LoadSalesTransactionsAsync();
                ClearSalesTransactionFields();
            }
        }

        // Add Inventory Count
        public async Task AddInventoryCountAsync()
        {
            if (string.IsNullOrWhiteSpace(ItemCode) || string.IsNullOrWhiteSpace(ItemName))
                return;

            var count = new InventoryCount
            {
                ItemCode = ItemCode,
                ItemName = ItemName,
                PhysicalCount = PhysicalCount,
                SystemCount = SystemCount,
                Variance = PhysicalCount - SystemCount,
                CountDate = DateTime.Now,
                CountedBy = CountedBy,
                Location = Location
            };

            if (await _databaseService.AddInventoryCountAsync(count))
            {
                await LoadInventoryCountsAsync();
                ClearInventoryCountFields();
            }
        }

        // Clear Input Fields
        private void ClearBookBorrowFields()
        {
            BorrowerName = string.Empty;
            BookTitle = string.Empty;
            ISBN = string.Empty;
            BorrowDate = DateTime.Now;
            DueDate = DateTime.Now.AddDays(14);
        }

        private void ClearSalesTransactionFields()
        {
            InvoiceNumber = string.Empty;
            ProductName = string.Empty;
            CustomerName = string.Empty;
            Quantity = 1;
            UnitPrice = 0;
            PaymentMethod = "Cash";
        }

        private void ClearInventoryCountFields()
        {
            ItemCode = string.Empty;
            ItemName = string.Empty;
            PhysicalCount = 0;
            SystemCount = 0;
            CountedBy = string.Empty;
            Location = string.Empty;
        }

        // Export Methods
        public async Task ExportBookBorrowsAsync(string filePath)
        {
            try
            {
                await Task.Run(() =>
                {
                    using var package = new ExcelPackage();
                    CreateBookBorrowsReport(package);
                    var file = new FileInfo(filePath);
                    package.SaveAs(file);
                });
            }
            catch { }
        }

        public async Task ExportSalesTransactionsAsync(string filePath)
        {
            try
            {
                await Task.Run(() =>
                {
                    using var package = new ExcelPackage();
                    CreateSalesTransactionReport(package);
                    var file = new FileInfo(filePath);
                    package.SaveAs(file);
                });
            }
            catch { }
        }

        public async Task ExportInventoryCountsAsync(string filePath)
        {
            try
            {
                await Task.Run(() =>
                {
                    using var package = new ExcelPackage();
                    CreateInventoryCountReport(package);
                    var file = new FileInfo(filePath);
                    package.SaveAs(file);
                });
            }
            catch { }
        }

        // Report Creation Methods
        private void CreateBookBorrowsReport(ExcelPackage package)
        {
            var workbook = package;
            
            // Sheet 1: Data
            var wsData = workbook.Workbook.Worksheets.Add("Book Borrows");
            AddBookBorrowsDataSheet(wsData);
            
            // Sheet 2: Summary & Graph
            var wsSummary = workbook.Workbook.Worksheets.Add("Summary");
            AddBookBorrowsSummarySheet(wsSummary);
        }

        private void CreateSalesTransactionReport(ExcelPackage package)
        {
            var workbook = package;
            
            // Sheet 1: Data
            var wsData = workbook.Workbook.Worksheets.Add("Sales Transactions");
            AddSalesTransactionsDataSheet(wsData);
            
            // Sheet 2: Summary & Graph
            var wsSummary = workbook.Workbook.Worksheets.Add("Summary");
            AddSalesTransactionsSummarySheet(wsSummary);
        }

        private void CreateInventoryCountReport(ExcelPackage package)
        {
            var workbook = package;
            
            // Sheet 1: Data
            var wsData = workbook.Workbook.Worksheets.Add("Inventory Counts");
            AddInventoryCountsDataSheet(wsData);
            
            // Sheet 2: Summary & Graph
            var wsSummary = workbook.Workbook.Worksheets.Add("Summary");
            AddInventoryCountsSummarySheet(wsSummary);
        }

        // Data Sheet Methods
        private void AddBookBorrowsDataSheet(ExcelWorksheet ws)
        {
            // Set column widths
            ws.Column(1).Width = 12;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 15;
            ws.Column(4).Width = 20;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 15;
            ws.Column(7).Width = 15;
            ws.Column(8).Width = 12;

            // Company Header
            int currentRow = 1;
            ws.Cells[currentRow, 1, currentRow, 8].Merge = true;
            ws.Cells[currentRow, 1].Value = COMPANY_NAME;
            ws.Cells[currentRow, 1].Style.Font.Bold = true;
            ws.Cells[currentRow, 1].Style.Font.Size = 16;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 8].Merge = true;
            ws.Cells[currentRow, 1].Value = COMPANY_ADDRESS;
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 4].Merge = true;
            ws.Cells[currentRow, 1].Value = $"Phone: {COMPANY_PHONE}";
            ws.Cells[currentRow, 1].Style.Font.Size = 9;
            ws.Cells[currentRow, 5, currentRow, 8].Merge = true;
            ws.Cells[currentRow, 5].Value = $"Email: {COMPANY_EMAIL}";
            ws.Cells[currentRow, 5].Style.Font.Size = 9;
            currentRow++;

            // Add spacing
            currentRow++;

            // Report Title
            ws.Cells[currentRow, 1, currentRow, 8].Merge = true;
            ws.Cells[currentRow, 1].Value = "BOOK BORROWING REPORT";
            ws.Cells[currentRow, 1].Style.Font.Bold = true;
            ws.Cells[currentRow, 1].Style.Font.Size = 14;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 8].Merge = true;
            ws.Cells[currentRow, 1].Value = "Report Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            // Signature area
            string signedByName = _currentUser?.FullName ?? "[User Name]";
            ws.Cells[currentRow, 1, currentRow, 8].Merge = true;
            ws.Cells[currentRow, 1].Value = $"Authorized By: {signedByName}";
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 4].Merge = true;
            ws.Cells[currentRow, 1].Value = "Signature: ________________________";
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 5, currentRow, 8].Merge = true;
            ws.Cells[currentRow, 5].Value = $"Date: {DateTime.Now:yyyy-MM-dd}";
            ws.Cells[currentRow, 5].Style.Font.Size = 10;
            currentRow++;

            // Add spacing
            currentRow++;

            // Column Headers
            int headerRow = currentRow;
            ws.Cells[headerRow, 1].Value = "Borrow ID";
            ws.Cells[headerRow, 2].Value = "Book Title";
            ws.Cells[headerRow, 3].Value = "ISBN";
            ws.Cells[headerRow, 4].Value = "Borrower Name";
            ws.Cells[headerRow, 5].Value = "Borrow Date";
            ws.Cells[headerRow, 6].Value = "Due Date";
            ws.Cells[headerRow, 7].Value = "Return Date";
            ws.Cells[headerRow, 8].Value = "Status";

            // Apply header formatting
            for (int i = 1; i <= 8; i++)
            {
                ws.Cells[headerRow, i].Style.Font.Bold = true;
                ws.Cells[headerRow, i].Style.Font.Color.SetColor(Color.White);
                ws.Cells[headerRow, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRow, i].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                ws.Cells[headerRow, i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            // Data
            currentRow = headerRow + 1;
            foreach (var borrow in BookBorrows)
            {
                ws.Cells[currentRow, 1].Value = borrow.BorrowID;
                ws.Cells[currentRow, 2].Value = borrow.BookTitle;
                ws.Cells[currentRow, 3].Value = borrow.ISBN;
                ws.Cells[currentRow, 4].Value = borrow.BorrowerName;
                ws.Cells[currentRow, 5].Value = borrow.BorrowDate.ToString("yyyy-MM-dd");
                ws.Cells[currentRow, 6].Value = borrow.DueDate.ToString("yyyy-MM-dd");
                ws.Cells[currentRow, 7].Value = borrow.ReturnDate?.ToString("yyyy-MM-dd") ?? "Not Returned";
                ws.Cells[currentRow, 8].Value = borrow.Status;
                currentRow++;
            }

            // Auto-fit columns (already set width above, but good to have)
            for (int i = 1; i <= 8; i++)
                ws.Cells[1, i].Style.WrapText = true;
        }

        private void AddSalesTransactionsDataSheet(ExcelWorksheet ws)
        {
            // Set column widths
            ws.Column(1).Width = 12;
            ws.Column(2).Width = 12;
            ws.Column(3).Width = 20;
            ws.Column(4).Width = 18;
            ws.Column(5).Width = 12;
            ws.Column(6).Width = 12;
            ws.Column(7).Width = 15;
            ws.Column(8).Width = 15;
            ws.Column(9).Width = 16;
            ws.Column(10).Width = 12;

            // Company Header
            int currentRow = 1;
            ws.Cells[currentRow, 1, currentRow, 10].Merge = true;
            ws.Cells[currentRow, 1].Value = COMPANY_NAME;
            ws.Cells[currentRow, 1].Style.Font.Bold = true;
            ws.Cells[currentRow, 1].Style.Font.Size = 16;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 10].Merge = true;
            ws.Cells[currentRow, 1].Value = COMPANY_ADDRESS;
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
            ws.Cells[currentRow, 1].Value = $"Phone: {COMPANY_PHONE}";
            ws.Cells[currentRow, 1].Style.Font.Size = 9;
            ws.Cells[currentRow, 6, currentRow, 10].Merge = true;
            ws.Cells[currentRow, 6].Value = $"Email: {COMPANY_EMAIL}";
            ws.Cells[currentRow, 6].Style.Font.Size = 9;
            currentRow++;

            // Add spacing
            currentRow++;

            // Report Title
            ws.Cells[currentRow, 1, currentRow, 10].Merge = true;
            ws.Cells[currentRow, 1].Value = "SALES TRANSACTION REPORT";
            ws.Cells[currentRow, 1].Style.Font.Bold = true;
            ws.Cells[currentRow, 1].Style.Font.Size = 14;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 10].Merge = true;
            ws.Cells[currentRow, 1].Value = "Report Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            // Signature area
            string signedByName = _currentUser?.FullName ?? "[User Name]";
            ws.Cells[currentRow, 1, currentRow, 10].Merge = true;
            ws.Cells[currentRow, 1].Value = $"Authorized By: {signedByName}";
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
            ws.Cells[currentRow, 1].Value = "Signature: ________________________";
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 6, currentRow, 10].Merge = true;
            ws.Cells[currentRow, 6].Value = $"Date: {DateTime.Now:yyyy-MM-dd}";
            ws.Cells[currentRow, 6].Style.Font.Size = 10;
            currentRow++;

            // Add spacing
            currentRow++;

            // Column Headers
            int headerRow = currentRow;
            ws.Cells[headerRow, 1].Value = "Transaction ID";
            ws.Cells[headerRow, 2].Value = "Invoice #";
            ws.Cells[headerRow, 3].Value = "Product Name";
            ws.Cells[headerRow, 4].Value = "Customer Name";
            ws.Cells[headerRow, 5].Value = "Quantity";
            ws.Cells[headerRow, 6].Value = "Unit Price";
            ws.Cells[headerRow, 7].Value = "Total Amount";
            ws.Cells[headerRow, 8].Value = "Transaction Date";
            ws.Cells[headerRow, 9].Value = "Payment Method";
            ws.Cells[headerRow, 10].Value = "Status";

            // Apply header formatting
            for (int i = 1; i <= 10; i++)
            {
                ws.Cells[headerRow, i].Style.Font.Bold = true;
                ws.Cells[headerRow, i].Style.Font.Color.SetColor(Color.White);
                ws.Cells[headerRow, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRow, i].Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);
                ws.Cells[headerRow, i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            // Data
            currentRow = headerRow + 1;
            foreach (var transaction in SalesTransactions)
            {
                ws.Cells[currentRow, 1].Value = transaction.TransactionID;
                ws.Cells[currentRow, 2].Value = transaction.InvoiceNumber;
                ws.Cells[currentRow, 3].Value = transaction.ProductName;
                ws.Cells[currentRow, 4].Value = transaction.CustomerName;
                ws.Cells[currentRow, 5].Value = transaction.Quantity;
                ws.Cells[currentRow, 6].Value = transaction.UnitPrice.ToString("C");
                ws.Cells[currentRow, 7].Value = transaction.TotalAmount.ToString("C");
                ws.Cells[currentRow, 8].Value = transaction.TransactionDate.ToString("yyyy-MM-dd");
                ws.Cells[currentRow, 9].Value = transaction.PaymentMethod;
                ws.Cells[currentRow, 10].Value = transaction.Status;
                currentRow++;
            }

            // Auto-fit columns
            for (int i = 1; i <= 10; i++)
                ws.Cells[1, i].Style.WrapText = true;
        }

        private void AddInventoryCountsDataSheet(ExcelWorksheet ws)
        {
            // Set column widths
            ws.Column(1).Width = 12;
            ws.Column(2).Width = 12;
            ws.Column(3).Width = 18;
            ws.Column(4).Width = 15;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 12;
            ws.Column(7).Width = 15;
            ws.Column(8).Width = 15;
            ws.Column(9).Width = 15;

            // Company Header
            int currentRow = 1;
            ws.Cells[currentRow, 1, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 1].Value = COMPANY_NAME;
            ws.Cells[currentRow, 1].Style.Font.Bold = true;
            ws.Cells[currentRow, 1].Style.Font.Size = 16;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 1].Value = COMPANY_ADDRESS;
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
            ws.Cells[currentRow, 1].Value = $"Phone: {COMPANY_PHONE}";
            ws.Cells[currentRow, 1].Style.Font.Size = 9;
            ws.Cells[currentRow, 6, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 6].Value = $"Email: {COMPANY_EMAIL}";
            ws.Cells[currentRow, 6].Style.Font.Size = 9;
            currentRow++;

            // Add spacing
            currentRow++;

            // Report Title
            ws.Cells[currentRow, 1, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 1].Value = "INVENTORY COUNT REPORT";
            ws.Cells[currentRow, 1].Style.Font.Bold = true;
            ws.Cells[currentRow, 1].Style.Font.Size = 14;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 1].Value = "Report Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;

            // Signature area
            string signedByName = _currentUser?.FullName ?? "[User Name]";
            ws.Cells[currentRow, 1, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 1].Value = $"Authorized By: {signedByName}";
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            currentRow++;

            ws.Cells[currentRow, 1, currentRow, 5].Merge = true;
            ws.Cells[currentRow, 1].Value = "Signature: ________________________";
            ws.Cells[currentRow, 1].Style.Font.Size = 10;
            ws.Cells[currentRow, 6, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 6].Value = $"Date: {DateTime.Now:yyyy-MM-dd}";
            ws.Cells[currentRow, 6].Style.Font.Size = 10;
            currentRow++;

            // Add spacing
            currentRow++;

            // Column Headers
            int headerRow = currentRow;
            ws.Cells[headerRow, 1].Value = "Count ID";
            ws.Cells[headerRow, 2].Value = "Item Code";
            ws.Cells[headerRow, 3].Value = "Item Name";
            ws.Cells[headerRow, 4].Value = "Physical Count";
            ws.Cells[headerRow, 5].Value = "System Count";
            ws.Cells[headerRow, 6].Value = "Variance";
            ws.Cells[headerRow, 7].Value = "Count Date";
            ws.Cells[headerRow, 8].Value = "Counted By";
            ws.Cells[headerRow, 9].Value = "Location";

            // Apply header formatting
            for (int i = 1; i <= 9; i++)
            {
                ws.Cells[headerRow, i].Style.Font.Bold = true;
                ws.Cells[headerRow, i].Style.Font.Color.SetColor(Color.White);
                ws.Cells[headerRow, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells[headerRow, i].Style.Fill.BackgroundColor.SetColor(Color.DarkOrange);
                ws.Cells[headerRow, i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            // Data
            currentRow = headerRow + 1;
            foreach (var count in InventoryCounts)
            {
                ws.Cells[currentRow, 1].Value = count.CountID;
                ws.Cells[currentRow, 2].Value = count.ItemCode;
                ws.Cells[currentRow, 3].Value = count.ItemName;
                ws.Cells[currentRow, 4].Value = count.PhysicalCount;
                ws.Cells[currentRow, 5].Value = count.SystemCount;
                ws.Cells[currentRow, 6].Value = count.Variance;
                ws.Cells[currentRow, 7].Value = count.CountDate.ToString("yyyy-MM-dd");
                ws.Cells[currentRow, 8].Value = count.CountedBy;
                ws.Cells[currentRow, 9].Value = count.Location;
                currentRow++;
            }

            // Auto-fit columns
            for (int i = 1; i <= 9; i++)
                ws.Cells[1, i].Style.WrapText = true;
        }

        // Summary Sheet Methods
        private void AddBookBorrowsSummarySheet(ExcelWorksheet ws)
        {
            // Title
            ws.Cells[1, 1].Value = "BOOK BORROWING ANALYSIS";
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 1].Style.Font.Size = 16;
            ws.Cells[1, 1, 1, 4].Merge = true;
            ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            int activeCount = 0, returnedCount = 0, overdueCount = 0;
            foreach (var borrow in BookBorrows)
            {
                if (borrow.Status == "Active") activeCount++;
                else if (borrow.Status == "Returned") returnedCount++;
                else if (borrow.Status == "Overdue") overdueCount++;
            }

            // Create chart data in column E-F (hidden from view)
            int chartDataRow = 3;
            ws.Cells[chartDataRow, 5].Value = "Status";
            ws.Cells[chartDataRow, 6].Value = "Count";
            
            ws.Cells[chartDataRow + 1, 5].Value = "Active";
            ws.Cells[chartDataRow + 1, 6].Value = activeCount;
            
            ws.Cells[chartDataRow + 2, 5].Value = "Returned";
            ws.Cells[chartDataRow + 2, 6].Value = returnedCount;
            
            ws.Cells[chartDataRow + 3, 5].Value = "Overdue";
            ws.Cells[chartDataRow + 3, 6].Value = overdueCount;

            // Create Pie Chart
            if (BookBorrows.Count > 0)
            {
                try
                {
                    var pieChart = ws.Drawings.AddChart("BookBorrowsPie", eChartType.Pie3D) as ExcelPieChart;
                    if (pieChart != null)
                    {
                        pieChart.Title.Text = "Book Borrows by Status";
                        pieChart.Title.Font.Size = 14;
                        pieChart.Title.Font.Bold = true;
                        
                        // Add series with proper reference
                        pieChart.Series.Add(ws.Cells[$"F{chartDataRow + 1}:F{chartDataRow + 3}"], ws.Cells[$"E{chartDataRow + 1}:E{chartDataRow + 3}"]);
                        
                        // Position chart at A3 with size 500x300
                        pieChart.SetPosition(2, 0, 0, 0);
                        pieChart.SetSize(500, 300);
                    }
                }
                catch (Exception ex)
                {
                    ws.Cells[3, 1].Value = $"Chart Error: {ex.Message}";
                }
            }

            // Add summary statistics below the chart
            int statsRow = 20;
            ws.Cells[statsRow, 1].Value = "Summary Statistics";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 1].Style.Font.Size = 12;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Total Borrows:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = BookBorrows.Count;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Active:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = activeCount;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Returned:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = returnedCount;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Overdue:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = overdueCount;

            ws.Column(1).AutoFit();
            ws.Column(2).AutoFit();
        }

        private void AddSalesTransactionsSummarySheet(ExcelWorksheet ws)
        {
            // Title
            ws.Cells[1, 1].Value = "SALES TRANSACTION ANALYSIS";
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 1].Style.Font.Size = 16;
            ws.Cells[1, 1, 1, 4].Merge = true;
            ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            decimal totalAmount = 0;
            int completedCount = 0;
            int pendingCount = 0;
            foreach (var transaction in SalesTransactions)
            {
                totalAmount += transaction.TotalAmount;
                if (transaction.Status == "Completed") completedCount++;
                else pendingCount++;
            }

            // Create chart data in column E-F
            int chartDataRow = 3;
            ws.Cells[chartDataRow, 5].Value = "Status";
            ws.Cells[chartDataRow, 6].Value = "Count";

            ws.Cells[chartDataRow + 1, 5].Value = "Completed";
            ws.Cells[chartDataRow + 1, 6].Value = completedCount;
            
            ws.Cells[chartDataRow + 2, 5].Value = "Pending";
            ws.Cells[chartDataRow + 2, 6].Value = pendingCount;

            // Create Column Chart
            if (SalesTransactions.Count > 0)
            {
                try
                {
                    var columnChart = ws.Drawings.AddChart("SalesStatusChart", eChartType.ColumnClustered) as ExcelBarChart;
                    if (columnChart != null)
                    {
                        columnChart.Title.Text = "Sales Transactions by Status";
                        columnChart.Title.Font.Size = 14;
                        columnChart.Title.Font.Bold = true;
                        columnChart.Series.Add(ws.Cells[$"F{chartDataRow + 1}:F{chartDataRow + 2}"], ws.Cells[$"E{chartDataRow + 1}:E{chartDataRow + 2}"]);
                        
                        // Position chart at A3 with size 500x300
                        columnChart.SetPosition(2, 0, 0, 0);
                        columnChart.SetSize(500, 300);
                    }
                }
                catch (Exception ex)
                {
                    ws.Cells[3, 1].Value = $"Chart Error: {ex.Message}";
                }
            }

            // Add summary statistics below the chart
            int statsRow = 20;
            ws.Cells[statsRow, 1].Value = "Summary Statistics";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 1].Style.Font.Size = 12;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Total Transactions:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = SalesTransactions.Count;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Total Sales Amount:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = totalAmount;
            ws.Cells[statsRow, 2].Style.Numberformat.Format = "$#,##0.00";
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Completed:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = completedCount;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Pending:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = pendingCount;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Average Transaction:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = SalesTransactions.Count > 0 ? totalAmount / SalesTransactions.Count : 0;
            ws.Cells[statsRow, 2].Style.Numberformat.Format = "$#,##0.00";

            ws.Column(1).AutoFit();
            ws.Column(2).AutoFit();
        }

        private void AddInventoryCountsSummarySheet(ExcelWorksheet ws)
        {
            // Title
            ws.Cells[1, 1].Value = "INVENTORY COUNT ANALYSIS";
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 1].Style.Font.Size = 16;
            ws.Cells[1, 1, 1, 4].Merge = true;
            ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            int varianceCount = 0;
            int totalVariance = 0;
            int accurateCount = 0;
            foreach (var count in InventoryCounts)
            {
                if (count.Variance != 0) varianceCount++;
                else accurateCount++;
                totalVariance += Math.Abs(count.Variance);
            }

            // Create chart data in column E-F
            int chartDataRow = 3;
            ws.Cells[chartDataRow, 5].Value = "Status";
            ws.Cells[chartDataRow, 6].Value = "Count";

            ws.Cells[chartDataRow + 1, 5].Value = "Accurate";
            ws.Cells[chartDataRow + 1, 6].Value = accurateCount;
            
            ws.Cells[chartDataRow + 2, 5].Value = "Variance";
            ws.Cells[chartDataRow + 2, 6].Value = varianceCount;

            // Create Pie Chart
            if (InventoryCounts.Count > 0)
            {
                try
                {
                    var pieChart = ws.Drawings.AddChart("InventoryAccuracyPie", eChartType.Pie3D) as ExcelPieChart;
                    if (pieChart != null)
                    {
                        pieChart.Title.Text = "Inventory Count Accuracy";
                        pieChart.Title.Font.Size = 14;
                        pieChart.Title.Font.Bold = true;
                        pieChart.Series.Add(ws.Cells[$"F{chartDataRow + 1}:F{chartDataRow + 2}"], ws.Cells[$"E{chartDataRow + 1}:E{chartDataRow + 2}"]);
                        
                        // Position chart at A3 with size 500x300
                        pieChart.SetPosition(2, 0, 0, 0);
                        pieChart.SetSize(500, 300);
                    }
                }
                catch (Exception ex)
                {
                    ws.Cells[3, 1].Value = $"Chart Error: {ex.Message}";
                }
            }

            // Add summary statistics below the chart
            int statsRow = 20;
            ws.Cells[statsRow, 1].Value = "Summary Statistics";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 1].Style.Font.Size = 12;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Total Items Counted:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = InventoryCounts.Count;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Accurate Items:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = accurateCount;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Items with Variance:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = varianceCount;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Total Variance:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            ws.Cells[statsRow, 2].Value = totalVariance;
            statsRow++;

            ws.Cells[statsRow, 1].Value = "Accuracy Rate:";
            ws.Cells[statsRow, 1].Style.Font.Bold = true;
            decimal accuracyRate = InventoryCounts.Count > 0 ? (accurateCount / (decimal)InventoryCounts.Count) * 100 : 0;
            ws.Cells[statsRow, 2].Value = accuracyRate / 100;
            ws.Cells[statsRow, 2].Style.Numberformat.Format = "0.00%";

            ws.Column(1).AutoFit();
            ws.Column(2).AutoFit();
        }
    }
}
