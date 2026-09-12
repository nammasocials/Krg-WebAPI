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
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService iExpenseService)
        {
            _expenseService = iExpenseService;
        }

        /// <summary>Every filed expense, newest first. The date range is optional.</summary>
        [HttpGet("fetchExpenseList")]
        public async Task<IActionResult> GetExpenseList([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = fromDate.HasValue || toDate.HasValue
                ? new VDateRangeFilter { FromDate = fromDate, ToDate = toDate }
                : null;

            var result = await _expenseService.fetchExpenseList(filter);

            return StatusCode(200, new ApiResponse<List<Vexpense>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }

        [HttpGet("fetchExpenseDetails/{expenseCode}")]
        public async Task<IActionResult> GetExpenseDetails(Guid expenseCode)
        {
            var result = await _expenseService.fetchExpenseDetails(expenseCode);

            if (result == null)
            {
                return StatusCode(404, new ApiResponse<Vexpense>
                {
                    Code = 404,
                    Message = $"No expense found for {expenseCode}",
                    Data = null
                });
            }

            return StatusCode(200, new ApiResponse<Vexpense>
            {
                Code = 200,
                Message = "Successfully Fetched 1 record",
                Data = result
            });
        }

        /// <summary>Expense categories, from [dbo].[InvConstant]. Drives the form dropdown.</summary>
        [HttpGet("fetchExpenseTypes")]
        public async Task<IActionResult> GetExpenseTypes()
        {
            var result = await _expenseService.fetchExpenseTypes();

            return StatusCode(200, new ApiResponse<List<Vconstant>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }

        [HttpPost("AddExpense")]
        public async Task<IActionResult> AddExpense([FromBody] VExpenseInput expense)
        {
            var result = await _expenseService.AddOrEditExpense(expense, isEdit: false);

            return StatusCode(200, new ApiResponse<Vexpense>
            {
                Code = 200,
                Message = "Successfully Created expense record",
                Data = result
            });
        }

        [HttpPut("EditExpense")]
        public async Task<IActionResult> EditExpense([FromBody] VExpenseInput expense)
        {
            if (expense.ExpenseCode == null || expense.ExpenseCode == Guid.Empty)
            {
                return StatusCode(400, new ApiResponse<Vexpense>
                {
                    Code = 400,
                    Message = "ExpenseCode is required to edit an expense",
                    Data = null
                });
            }

            var result = await _expenseService.AddOrEditExpense(expense, isEdit: true);

            return StatusCode(200, new ApiResponse<Vexpense>
            {
                Code = 200,
                Message = "Successfully Updated expense record",
                Data = result
            });
        }

        /// <summary>Soft delete: the row stays for the audit trail and past reports.</summary>
        [HttpDelete("DeleteExpense/{expenseCode}")]
        public async Task<IActionResult> DeleteExpense(Guid expenseCode)
        {
            var deleted = await _expenseService.deleteExpense(expenseCode);

            return StatusCode(deleted ? 200 : 404, new ApiResponse<bool>
            {
                Code = deleted ? 200 : 404,
                Message = deleted ? "Successfully Deleted expense record" : $"No expense found for {expenseCode}",
                Data = deleted
            });
        }
    }
}
