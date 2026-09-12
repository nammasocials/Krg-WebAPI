using DBLayer.Models;
using DBLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface IReportService
    {
        public Task<VSalesRegisterReport> fetchSalesRegister(VDateRangeFilter filter);
        public Task<VStockLedgerReport> fetchStockLedger(VDateRangeFilter filter, Guid? productCode = null);
        public Task<VExpenseReport> fetchExpenseReport(VDateRangeFilter filter);
        public Task<VProfitSummaryReport> fetchProfitSummary(VDateRangeFilter filter);
        public Task<VDashboardSummary> fetchDashboardSummary();
    }

    public class ReportService : IReportService
    {
        /// <summary>
        /// Stock movements are labelled by the [dbo].[InvConstant] name the ledger
        /// row points at; anything that is not an inward movement counts as outward.
        /// </summary>
        private const string StockInTransactionType = "Stock-In";

        private readonly NsinvoiceBillingContext _context;

        public ReportService(NsinvoiceBillingContext context)
        {
            _context = context;
        }

        public async Task<VSalesRegisterReport> fetchSalesRegister(VDateRangeFilter filter)
        {
            var (from, to) = filter.Resolve();

            var invoices = await _context.Vinvoices
                .Where(i => i.CreatedOn >= from && i.CreatedOn <= to)
                .ToListAsync();

            var codes = invoices.Select(i => i.InvoiceCode).ToList();

            // One pass over the lines, then grouped in memory: the tax split lives on
            // the line rows, and an invoice's line count is wanted on the register too.
            var lines = await _context.VinvoiceDetails
                .Where(d => codes.Contains(d.InvoiceCode))
                .ToListAsync();

            var linesByInvoice = lines
                .GroupBy(d => d.InvoiceCode)
                .ToDictionary(g => g.Key, g => g.ToList());

            var rows = invoices
                .OrderBy(i => i.CreatedOn)
                .Select(i =>
                {
                    linesByInvoice.TryGetValue(i.InvoiceCode, out var invoiceLines);
                    invoiceLines ??= new List<VinvoiceDetail>();

                    var taxable = invoiceLines.Sum(l => l.Cost);
                    var cgst = invoiceLines.Sum(l => l.CentralGstAmount);
                    var sgst = invoiceLines.Sum(l => l.StateGstAmount);

                    return new VSalesRegisterRow
                    {
                        InvoiceCode = i.InvoiceCode,
                        InvoiceNo = i.InvoiceNo,
                        InvoiceDate = i.CreatedOn,
                        CustomerName = i.CustomerName,
                        CustomerGst = i.Gstnumber,
                        TaxableValue = taxable,
                        CentralGstAmount = cgst,
                        StateGstAmount = sgst,
                        TotalTax = cgst + sgst,
                        InvoiceTotal = i.TotalCost,
                        LineCount = invoiceLines.Count
                    };
                })
                .ToList();

            return new VSalesRegisterReport
            {
                FromDate = from,
                ToDate = to,
                Rows = rows,
                TotalTaxableValue = rows.Sum(r => r.TaxableValue),
                TotalCentralGst = rows.Sum(r => r.CentralGstAmount),
                TotalStateGst = rows.Sum(r => r.StateGstAmount),
                TotalTax = rows.Sum(r => r.TotalTax),
                GrandTotal = rows.Sum(r => r.InvoiceTotal),
                InvoiceCount = rows.Count
            };
        }

        public async Task<VStockLedgerReport> fetchStockLedger(VDateRangeFilter filter, Guid? productCode = null)
        {
            var (from, to) = filter.Resolve();

            var movementQuery = _context.Vstocks.AsQueryable();
            if (productCode.HasValue)
            {
                movementQuery = movementQuery.Where(s => s.ProductCode == productCode.Value);
            }

            var movements = await movementQuery
                .Where(s => s.CreatedOn <= to)
                .ToListAsync();

            var productQuery = _context.Vproducts.AsQueryable();
            if (productCode.HasValue)
            {
                productQuery = productQuery.Where(p => p.ProductCode == productCode.Value);
            }
            var products = await productQuery.ToListAsync();

            var byProduct = movements
                .GroupBy(m => m.ProductCode)
                .ToDictionary(g => g.Key, g => g.OrderBy(m => m.CreatedOn).ToList());

            var result = new List<VStockLedgerProduct>();

            foreach (var product in products.OrderBy(p => p.ProductName))
            {
                byProduct.TryGetValue(product.ProductCode, out var productMovements);
                productMovements ??= new List<Vstock>();

                // Everything before the window is the opening balance; the rest is
                // the ledger itself, with the balance carried down each row.
                var opening = productMovements
                    .Where(m => m.CreatedOn < from)
                    .Sum(m => SignedQuantity(m));

                var running = opening;
                var rows = new List<VStockLedgerRow>();
                var stockIn = 0;
                var stockOut = 0;

                foreach (var movement in productMovements.Where(m => m.CreatedOn >= from))
                {
                    var signed = SignedQuantity(movement);
                    running += signed;

                    if (signed >= 0)
                    {
                        stockIn += signed;
                    }
                    else
                    {
                        stockOut += -signed;
                    }

                    rows.Add(new VStockLedgerRow
                    {
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        TransactionDate = movement.CreatedOn,
                        TransactionType = movement.TransactionType,
                        Quantity = movement.Quantity,
                        Balance = running
                    });
                }

                result.Add(new VStockLedgerProduct
                {
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    OpeningBalance = opening,
                    StockIn = stockIn,
                    StockOut = stockOut,
                    ClosingBalance = running,
                    CurrentStock = product.CurrentStock,
                    Movements = rows
                });
            }

            return new VStockLedgerReport
            {
                FromDate = from,
                ToDate = to,
                Products = result,
                TotalStockIn = result.Sum(p => p.StockIn),
                TotalStockOut = result.Sum(p => p.StockOut),
                TotalCurrentStock = result.Sum(p => p.CurrentStock)
            };
        }

        public async Task<VExpenseReport> fetchExpenseReport(VDateRangeFilter filter)
        {
            var (from, to) = filter.Resolve();

            var expenses = await _context.Vexpenses
                .Where(e => e.IsActive && e.ExpenseDate >= from && e.ExpenseDate <= to)
                .ToListAsync();

            var rows = expenses
                .GroupBy(e => new { e.ExpenseType, e.ExpenseTypeName })
                .Select(g => new VExpenseReportRow
                {
                    ExpenseType = g.Key.ExpenseType,
                    ExpenseTypeName = g.Key.ExpenseTypeName,
                    NormalAmount = g.Where(e => e.ExpenseNature == "Normal").Sum(e => e.Amount),
                    InvoiceBasedAmount = g.Where(e => e.ExpenseNature != "Normal").Sum(e => e.Amount),
                    TotalAmount = g.Sum(e => e.Amount),
                    EntryCount = g.Count()
                })
                .OrderByDescending(r => r.TotalAmount)
                .ToList();

            return new VExpenseReport
            {
                FromDate = from,
                ToDate = to,
                Rows = rows,
                TotalNormal = rows.Sum(r => r.NormalAmount),
                TotalInvoiceBased = rows.Sum(r => r.InvoiceBasedAmount),
                GrandTotal = rows.Sum(r => r.TotalAmount),
                EntryCount = expenses.Count
            };
        }

        public async Task<VProfitSummaryReport> fetchProfitSummary(VDateRangeFilter filter)
        {
            var (from, to) = filter.Resolve();

            var invoices = await _context.Vinvoices
                .Where(i => i.CreatedOn >= from && i.CreatedOn <= to)
                .Select(i => new { i.CreatedOn, i.TotalCost })
                .ToListAsync();

            var expenses = await _context.Vexpenses
                .Where(e => e.IsActive && e.ExpenseDate >= from && e.ExpenseDate <= to)
                .Select(e => new { e.ExpenseDate, e.Amount })
                .ToListAsync();

            // A month with revenue but no expenses (or the reverse) still deserves a
            // row, so the two sides are unioned on their month key rather than joined.
            var months = invoices.Select(i => (i.CreatedOn.Year, i.CreatedOn.Month))
                .Concat(expenses.Select(e => (e.ExpenseDate.Year, e.ExpenseDate.Month)))
                .Distinct()
                .OrderBy(m => m.Year).ThenBy(m => m.Month)
                .ToList();

            var rows = months.Select(m =>
            {
                var revenue = invoices
                    .Where(i => i.CreatedOn.Year == m.Year && i.CreatedOn.Month == m.Month)
                    .Sum(i => i.TotalCost);
                var spend = expenses
                    .Where(e => e.ExpenseDate.Year == m.Year && e.ExpenseDate.Month == m.Month)
                    .Sum(e => e.Amount);

                return new VProfitSummaryRow
                {
                    Year = m.Year,
                    Month = m.Month,
                    MonthName = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m.Month)} {m.Year}",
                    Revenue = revenue,
                    Expenses = spend,
                    Profit = revenue - spend
                };
            }).ToList();

            var totalRevenue = rows.Sum(r => r.Revenue);
            var totalExpenses = rows.Sum(r => r.Expenses);

            return new VProfitSummaryReport
            {
                FromDate = from,
                ToDate = to,
                Rows = rows,
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses,
                TotalProfit = totalRevenue - totalExpenses,
                ProfitMarginPercent = totalRevenue == 0
                    ? 0
                    : Math.Round((totalRevenue - totalExpenses) / totalRevenue * 100, 2)
            };
        }

        public async Task<VDashboardSummary> fetchDashboardSummary()
        {
            var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            return new VDashboardSummary
            {
                ActiveCustomers = await _context.Vcustomers.CountAsync(),
                TotalInvoices = await _context.Vinvoices.CountAsync(),
                InvoicesThisMonth = await _context.Vinvoices.CountAsync(i => i.CreatedOn >= monthStart),
                TotalAvailableStock = await _context.Vproducts
                    .Where(p => p.IsActive)
                    .SumAsync(p => (int?)p.CurrentStock) ?? 0,
                ActiveProducts = await _context.Vproducts.CountAsync(p => p.IsActive),
                TotalRevenue = await _context.Vinvoices.SumAsync(i => (decimal?)i.TotalCost) ?? 0m,
                TotalExpenses = await _context.Vexpenses
                    .Where(e => e.IsActive)
                    .SumAsync(e => (decimal?)e.Amount) ?? 0m
            };
        }

        /// <summary>
        /// The ledger stores quantities unsigned, with direction in the transaction
        /// type, so outward movements are negated before any balance is taken.
        /// </summary>
        private static int SignedQuantity(Vstock movement)
            => movement.TransactionType == StockInTransactionType
                ? Math.Abs(movement.Quantity)
                : -Math.Abs(movement.Quantity);
    }
}
