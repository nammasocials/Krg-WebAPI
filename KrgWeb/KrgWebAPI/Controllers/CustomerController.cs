using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.Service;
using DBLayer.ViewModels;
using KrgWebAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KrgWebAPI.Controllers
{
    [Route("KrgWebAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService iCustomerService)
        {
            _customerService = iCustomerService;
        }

        [HttpGet("getAllCustomerList")]
        public async Task<IActionResult> getAllCustomerList()
        {
            var result = await _customerService.fetchCustomerList();

            return StatusCode(200, new ApiResponse<List<Vcustomer>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }
        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer([FromForm] VCustomerInput customerInput)
        {
            var customer = CustomerMapper.ToEntity(customerInput);
            if (customerInput.CompanyLogo != null)
            {
                using (var ms = new MemoryStream())
                {
                    await customerInput.CompanyLogo.CopyToAsync(ms);
                    customer.CustomerLogo = ms.ToArray();  // ✅ convert to byte[]
                }
            }

            var result = await _customerService.AddCustomer(customer);

            return StatusCode(200, new ApiResponse<Vcustomer>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.CustomerCode} records",
                Data = result
            });
        }
    }
}
