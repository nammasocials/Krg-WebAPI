using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.Service;
using DBLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using UtilityLayer;

namespace KrgWebAPI.Controllers
{
    [Route("KrgWebAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceReportService _invoiceReportService;
        public InvoiceController(IInvoiceService iInvoiceService, IInvoiceReportService iInvoiceReportService)
        {
            _invoiceService = iInvoiceService;
            _invoiceReportService = iInvoiceReportService;
        }
        [HttpGet("fetchInvoiceList")]
        public async Task<IActionResult> getAllInvioiceAsync()
        {
            var result = await _invoiceService.fetchInvoiceList();

            return StatusCode(200, new ApiResponse<List<Vinvoice>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }
        [HttpGet("fetchInvoiceDetails/{invoiceDetails}")]
        public async Task<IActionResult> getIvioiceDetailsAsync(Guid invoiceDetails)
        {
            var result = await _invoiceService.fetchInvoiceDetails(invoiceDetails);

            return StatusCode(200, new ApiResponse<VinvoiceDetail>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.InvoiceNo} records",
                Data = result
            });
        }
        [HttpPost("AddInvoice")]
        public async Task<IActionResult> AddInvoice([FromForm] VInvoiceInput invoiceInput)
        {
            

            var result = await _invoiceService.AddInvoiceAsync(invoiceInput);

            return StatusCode(200, new ApiResponse<Vinvoice>
            {
                Code = 200,
                Message = $"Successfully Created {result.InvoiceNo} record",
                Data = result
            });
        }
        /// <summary>
        /// Renders the invoice through Reports\Invoice.rdlc and returns it as a PDF
        /// download. One call per invoice; the browser gets the file directly.
        /// </summary>
        [HttpGet("exportInvoicePdf/{invoiceCode}")]
        public async Task<IActionResult> ExportInvoicePdf(Guid invoiceCode)
        {
            var export = await _invoiceReportService.ExportInvoicePdfAsync(invoiceCode);

            if (export == null)
            {
                return StatusCode(404, new ApiResponse<string>
                {
                    Code = 404,
                    Message = $"No invoice found for {invoiceCode}",
                    Data = null
                });
            }

            return File(export.Value.Pdf, "application/pdf", export.Value.FileName);
        }

        [HttpGet("getEwayBillPhoto/{invoiceCode}")]
        public async Task<IActionResult> GetEwayBillPhoto(Guid invoiceCode)
        {
            var (imageBytes, mimeType) = await _invoiceService.fetchEwaybillLogoIfAvailableAsync(invoiceCode);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                // Return 200 OK with empty string as body
                return Content(string.Empty, "text/plain");
            }

            // Serve bytes directly with appropriate mime type (jpeg, png, etc)
            return File(imageBytes, mimeType);
        }
    }
}
