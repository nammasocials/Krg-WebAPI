using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.Service;
using DBLayer.ViewModels;
using KrgWebAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UtilityLayer;

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
        [HttpGet("getRecentlyAddedCustomers")]
        public async Task<IActionResult> getRecentlyAddedCustomers()
        {
            var customerList = await _customerService.fetchCustomerList();
            var result = customerList.Where(C => C.CreatedOn.Date >= DateTime.Today).Take(5).ToList();

            return StatusCode(200, new ApiResponse<List<Vcustomer>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }
        [HttpGet("getCustomerStats")]
        public async Task<IActionResult> getCustomerStatsForDashboard()
        {
            var result = await _customerService.fetchDashboardCustomerStats();

            return StatusCode(200, new ApiResponse<VDashboardStats>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.OverallCount} records",
                Data = result
            });
        }

        [HttpGet("getAllCustomerDetails/{customerCode}")]
        public async Task<IActionResult> fetchCustomerDetails(Guid customerCode)
        {
            var result = await _customerService.fetchCustomerDetails(customerCode);

            return StatusCode(200, new ApiResponse<Vcustomer>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.CustomerCode} records",
                Data = result
            });
        }

        [HttpGet("getCustomerPhoto/{customerCode}")]
        public async Task<IActionResult> GetCustomerPhoto(Guid customerCode)
        {
            var (imageBytes, mimeType) = await _customerService.fetchCustomerImageAsync(customerCode);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                // Return 200 OK with empty string as body
                return Content(string.Empty, "text/plain");
            }

            // Serve bytes directly with appropriate mime type (jpeg, png, etc)
            return File(imageBytes, mimeType);
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
                    var imageBytes = ms.ToArray();
                    using (var img = Image.FromStream(new MemoryStream(imageBytes)))
                    {
                        string mimeType = ImageReader.GetMimeType(img.RawFormat);
                        Console.WriteLine($"Detected MIME type: {mimeType}");

                        // Save both imageBytes and mimeType to your database/entity
                        customer.CustomerLogo = imageBytes;
                        customer.CustomerLogoMime = mimeType;
                    }
                }
            }

            var result = await _customerService.AddOrEditCustomer(customer, false);

            return StatusCode(200, new ApiResponse<Vcustomer>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.CustomerCode} records",
                Data = result
            });
        }

        [HttpPost("EditCustomer")]
        public async Task<IActionResult> EditCustomer([FromForm] VCustomerInput customerInput)
        {
            var customer = CustomerMapper.ToEntity(customerInput);
            if (customerInput.CompanyLogo != null)
            {
                using (var ms = new MemoryStream())
                {
                    await customerInput.CompanyLogo.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();
                    using (var img = Image.FromStream(new MemoryStream(imageBytes)))
                    {
                        string mimeType = ImageReader.GetMimeType(img.RawFormat);
                        Console.WriteLine($"Detected MIME type: {mimeType}");

                        // Save both imageBytes and mimeType to your database/entity
                        customer.CustomerLogo = imageBytes;
                        customer.CustomerLogoMime = mimeType;
                    }
                }
            }

            var result = await _customerService.AddOrEditCustomer(customer, true);

            return StatusCode(200, new ApiResponse<Vcustomer>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.CustomerCode} records",
                Data = result
            });
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomerDetails(Guid id)
        {
            var result = await _customerService.deleteCustomer(id);
            if (!result)
            {
                return StatusCode(200, new ApiResponse<bool>
                {
                    Code = 500,
                    Message = $"Removing Customer Failed for {id}",
                    Data = result
                });
            }
            return StatusCode(200, new ApiResponse<bool>
            {
                Code = 200,
                Message = $"Removing Customer Succeed for {id}",
                Data = result
            });
        }
    }
}
