using System;
using System.Collections.Generic;

namespace DBLayer.ViewModels
{
    /// <summary>One invoice on the sales register, with its tax split.</summary>
    public class VSalesRegisterRow
    {
        public Guid InvoiceCode { get; set; }

        public string InvoiceNo { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string? CustomerGst { get; set; }

        /// <summary>Value before tax.</summary>
        public decimal TaxableValue { get; set; }

        public decimal CentralGstAmount { get; set; }

        public decimal StateGstAmount { get; set; }

        public decimal TotalTax { get; set; }

        public decimal InvoiceTotal { get; set; }

        public int LineCount { get; set; }
    }

    public class VSalesRegisterReport
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<VSalesRegisterRow> Rows { get; set; } = new List<VSalesRegisterRow>();

        public decimal TotalTaxableValue { get; set; }

        public decimal TotalCentralGst { get; set; }

        public decimal TotalStateGst { get; set; }

        public decimal TotalTax { get; set; }

        public decimal GrandTotal { get; set; }

        public int InvoiceCount { get; set; }
    }

    /// <summary>One stock movement, carrying the running balance after it.</summary>
    public class VStockLedgerRow
    {
        public Guid ProductCode { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public int Balance { get; set; }
    }

    public class VStockLedgerProduct
    {
        public Guid ProductCode { get; set; }

        public string ProductName { get; set; } = string.Empty;

        /// <summary>Balance carried into the range, i.e. all movement before FromDate.</summary>
        public int OpeningBalance { get; set; }

        public int StockIn { get; set; }

        public int StockOut { get; set; }

        public int ClosingBalance { get; set; }

        /// <summary>Live [dbo].[InvProducts].[CurrentStock], for comparison with the ledger.</summary>
        public int CurrentStock { get; set; }

        public List<VStockLedgerRow> Movements { get; set; } = new List<VStockLedgerRow>();
    }

    public class VStockLedgerReport
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<VStockLedgerProduct> Products { get; set; } = new List<VStockLedgerProduct>();

        public int TotalStockIn { get; set; }

        public int TotalStockOut { get; set; }

        public int TotalCurrentStock { get; set; }
    }

    public class VExpenseReportRow
    {
        public int ExpenseType { get; set; }

        public string ExpenseTypeName { get; set; } = string.Empty;

        public decimal NormalAmount { get; set; }

        public decimal InvoiceBasedAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public int EntryCount { get; set; }
    }

    public class VExpenseReport
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<VExpenseReportRow> Rows { get; set; } = new List<VExpenseReportRow>();

        public decimal TotalNormal { get; set; }

        public decimal TotalInvoiceBased { get; set; }

        public decimal GrandTotal { get; set; }

        public int EntryCount { get; set; }
    }

    /// <summary>Revenue less expenses for one calendar month.</summary>
    public class VProfitSummaryRow
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public string MonthName { get; set; } = string.Empty;

        public decimal Revenue { get; set; }

        public decimal Expenses { get; set; }

        public decimal Profit { get; set; }
    }

    public class VProfitSummaryReport
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<VProfitSummaryRow> Rows { get; set; } = new List<VProfitSummaryRow>();

        public decimal TotalRevenue { get; set; }

        public decimal TotalExpenses { get; set; }

        public decimal TotalProfit { get; set; }

        /// <summary>Profit as a percentage of revenue; zero when there is no revenue.</summary>
        public decimal ProfitMarginPercent { get; set; }
    }

    /// <summary>Tiles on the landing dashboard.</summary>
    public class VDashboardSummary
    {
        public int ActiveCustomers { get; set; }

        public int TotalInvoices { get; set; }

        public int InvoicesThisMonth { get; set; }

        public int TotalAvailableStock { get; set; }

        public int ActiveProducts { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TotalExpenses { get; set; }
    }
}
