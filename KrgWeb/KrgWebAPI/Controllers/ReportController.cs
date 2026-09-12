using DBLayer.Service;
using DBLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KrgWebAPI.Controllers
{
    [Route("KrgWebAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService iReportService)
        {
            _reportService = iReportService;
        }

        /// <summary>Invoices in the range with their tax split. Basis for GST filing.</summary>
        [HttpGet("salesRegister")]
        public async Task<IActionResult> GetSalesRegister([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _reportService.fetchSalesRegister(Range(fromDate, toDate));

            return StatusCode(200, new ApiResponse<VSalesRegisterReport>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.InvoiceCount} invoices",
                Data = result
            });
        }

        /// <summary>Stock movement per product, with opening and closing balances.</summary>
        [HttpGet("stockLedger")]
        public async Task<IActionResult> GetStockLedger([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] Guid? productCode)
        {
            var result = await _reportService.fetchStockLedger(Range(fromDate, toDate), productCode);

            return StatusCode(200, new ApiResponse<VStockLedgerReport>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Products.Count} products",
                Data = result
            });
        }

        /// <summary>Expenses grouped by category, split normal against invoice based.</summary>
        [HttpGet("expenseReport")]
        public async Task<IActionResult> GetExpenseReport([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _reportService.fetchExpenseReport(Range(fromDate, toDate));

            return StatusCode(200, new ApiResponse<VExpenseReport>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Rows.Count} categories",
                Data = result
            });
        }

        /// <summary>Revenue less expenses, by month.</summary>
        [HttpGet("profitSummary")]
        public async Task<IActionResult> GetProfitSummary([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _reportService.fetchProfitSummary(Range(fromDate, toDate));

            return StatusCode(200, new ApiResponse<VProfitSummaryReport>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Rows.Count} months",
                Data = result
            });
        }

        /// <summary>Tiles for the landing dashboard.</summary>
        [HttpGet("dashboardSummary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var result = await _reportService.fetchDashboardSummary();

            return StatusCode(200, new ApiResponse<VDashboardSummary>
            {
                Code = 200,
                Message = "Successfully Fetched dashboard summary",
                Data = result
            });
        }

        /// <summary>
        /// An omitted range means "everything up to today", which each report
        /// resolves the same way.
        /// </summary>
        private static VDateRangeFilter Range(DateTime? fromDate, DateTime? toDate)
            => new VDateRangeFilter { FromDate = fromDate, ToDate = toDate };
    }
}
