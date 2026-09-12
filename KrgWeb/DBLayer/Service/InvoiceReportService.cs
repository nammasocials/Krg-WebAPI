using DBLayer.Models;
using DBLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface IInvoiceReportService
    {
        /// <summary>Builds the report data for one invoice, or null when it does not exist.</summary>
        Task<VInvoiceReport?> BuildInvoiceReportAsync(Guid invoiceCode);

        /// <summary>Renders one invoice to a PDF, or null when the invoice does not exist.</summary>
        Task<(byte[] Pdf, string FileName)?> ExportInvoicePdfAsync(Guid invoiceCode);
    }

    /// <summary>
    /// Renders Reports\Invoice.rdlc to PDF via the RDLC engine
    /// (ReportViewerCore.NETCore, the .NET port of the ReportViewer local mode).
    /// </summary>
    public class InvoiceReportService : IInvoiceReportService
    {
        private const string ReportRelativePath = "Reports/Invoice.rdlc";
        private const string ItemsDataSetName = "InvoiceItems";

        private readonly NsinvoiceBillingContext _context;
        private readonly IConfiguration _configuration;

        public InvoiceReportService(NsinvoiceBillingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<VInvoiceReport?> BuildInvoiceReportAsync(Guid invoiceCode)
        {
            var invoice = await _context.Vinvoices
                .Where(i => i.InvoiceCode == invoiceCode)
                .FirstOrDefaultAsync();

            if (invoice == null)
            {
                return null;
            }

            // Every line of the invoice, not just the first row [dbo].[VInvoiceDetail] returns.
            var details = await _context.VinvoiceDetails
                .Where(d => d.InvoiceCode == invoiceCode)
                .OrderBy(d => d.CreatedOn)
                .ToListAsync();

            var report = new VInvoiceReport
            {
                InvoiceNo = invoice.InvoiceNo,
                InvoiceDate = invoice.CreatedOn,
                CustomerName = invoice.CustomerName,
                CustomerEmail = invoice.CustomerEmail,
                CustomerGst = invoice.Gstnumber,
                CompanyName = _configuration["InvoiceReport:CompanyName"] ?? "Namma Socials",
                CompanyAddress = _configuration["InvoiceReport:CompanyAddress"] ?? string.Empty,
                CompanyGst = _configuration["InvoiceReport:CompanyGst"] ?? string.Empty,
                TotalCost = invoice.TotalCost,
                Items = details.Select((d, index) => new VInvoiceReportItem
                {
                    SlNo = index + 1,
                    ProductName = d.ProductName,
                    HsnCode = d.Hsncode,
                    Quantity = d.Quantity,
                    UnitCost = d.UnitCost,
                    Cost = d.Cost,
                    CentralGst = d.CentralGst,
                    CentralGstAmount = d.CentralGstAmount,
                    StateGst = d.StateGst,
                    StateGstAmount = d.StateGstAmount,
                    // [dbo].[VInvoiceDetail] does not project [NetProductAmount], so it is
                    // recomputed here exactly as [dbo].[usp_CalculateAndUpdateGST] stores it.
                    NetProductAmount = d.Cost + d.CentralGstAmount + d.StateGstAmount
                }).ToList()
            };

            return report;
        }

        public async Task<(byte[] Pdf, string FileName)?> ExportInvoicePdfAsync(Guid invoiceCode)
        {
            var report = await BuildInvoiceReportAsync(invoiceCode);
            if (report == null)
            {
                return null;
            }

            var pdf = Render(report);
            return (pdf, BuildFileName(report.InvoiceNo));
        }

        /// <summary>
        /// Renders report data through the RDLC. Static and dependency-free so the
        /// report layout can be exercised without a database.
        /// </summary>
        public static byte[] Render(VInvoiceReport report)
        {
            var reportPath = Path.Combine(AppContext.BaseDirectory, ReportRelativePath);
            if (!File.Exists(reportPath))
            {
                throw new FileNotFoundException(
                    $"Invoice report definition not found at '{reportPath}'. " +
                    "Confirm Reports\\Invoice.rdlc is copied to the output directory.",
                    reportPath);
            }

            using var localReport = new LocalReport();
            using (var definition = File.OpenRead(reportPath))
            {
                localReport.LoadReportDefinition(definition);
            }

            localReport.DataSources.Add(new ReportDataSource(ItemsDataSetName, report.Items));

            // Amounts are formatted in the report itself; these are the header values only.
            localReport.SetParameters(new[]
            {
                new ReportParameter("InvoiceNo", report.InvoiceNo),
                new ReportParameter("InvoiceDate", report.InvoiceDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)),
                new ReportParameter("CustomerName", report.CustomerName),
                new ReportParameter("CustomerEmail", report.CustomerEmail ?? string.Empty),
                new ReportParameter("CustomerGst", report.CustomerGst ?? string.Empty),
                new ReportParameter("CompanyName", report.CompanyName),
                new ReportParameter("CompanyAddress", report.CompanyAddress ?? string.Empty),
                new ReportParameter("CompanyGst", report.CompanyGst ?? string.Empty),
                new ReportParameter("TotalCost", report.TotalCost.ToString("N2", CultureInfo.InvariantCulture))
            });

            return localReport.Render("PDF");
        }

        /// <summary>
        /// Invoice numbers are free text, so strip anything a filename cannot carry.
        /// </summary>
        private static string BuildFileName(string invoiceNo)
        {
            var safe = new string((invoiceNo ?? string.Empty)
                .Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c)
                .ToArray())
                .Trim();

            if (string.IsNullOrWhiteSpace(safe))
            {
                safe = "Invoice";
            }

            return $"Invoice_{safe}.pdf";
        }
    }
}
