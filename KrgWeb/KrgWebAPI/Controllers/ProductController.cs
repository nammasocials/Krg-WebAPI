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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService iProductService)
        {
            _productService = iProductService;
        }

        [HttpGet("getAllProductList")]
        public async Task<IActionResult> getAllProductList()
        {
            var result = await _productService.fetchProductList();

            return StatusCode(200, new ApiResponse<List<Vproduct>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] VProductInput productInput)
        {
            var product = ProductMapper.ToEntity(productInput);
            if (productInput.ProductLogo != null)
            {
                using (var ms = new MemoryStream())
                {
                    await productInput.ProductLogo.CopyToAsync(ms);
                    product.ProductLogo = ms.ToArray();  // ✅ convert to byte[]
                }
            }

            var result = await _productService.AddOrEditProduct(product, false);

            return StatusCode(200, new ApiResponse<Vproduct>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.ProductCode} records",
                Data = result
            });
        }
        [HttpPost("EditProduct")]
        public async Task<IActionResult> EditProduct([FromForm] VProductInput productInput)
        {
            var product = ProductMapper.ToEntity(productInput);
            if (productInput.ProductLogo != null)
            {
                using (var ms = new MemoryStream())
                {
                    await productInput.ProductLogo.CopyToAsync(ms);
                    product.ProductLogo = ms.ToArray();  // ✅ convert to byte[]
                }
            }

            var result = await _productService.AddOrEditProduct(product, true);

            return StatusCode(200, new ApiResponse<Vproduct>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.ProductCode} records",
                Data = result
            });
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProductDetails(Guid id)
        {
            var result = await _productService.deleteProduct(id);
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
        [HttpPost("AddStockEntry")]
        public async Task<IActionResult> AddStockEntry(Guid id)
        {
            var result = await _productService.deleteProduct(id);
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
