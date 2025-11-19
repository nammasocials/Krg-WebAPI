using DBLayer.Models;
using DBLayer.Service;
using DBLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KrgWebAPI.Controllers
{
    [Route("KrgWebAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService iInvoiceService)
        {
            _invoiceService = iInvoiceService;
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
    }
}
