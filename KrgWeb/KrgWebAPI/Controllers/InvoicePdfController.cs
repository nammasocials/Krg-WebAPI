using Microsoft.AspNetCore.Mvc;
using KrgWebAPI.Services;

namespace KrgWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicePdfController : ControllerBase
    {
        private readonly PdfService _PdfService;

        public InvoicePdfController(PdfService PdfService)
        {
            _PdfService = PdfService;
        }

        [HttpGet("download")]
        public IActionResult DownloadInvoice()
        {
            var pdfBytes = _PdfService.GenerateInvoicePdf();

            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }
    }
}