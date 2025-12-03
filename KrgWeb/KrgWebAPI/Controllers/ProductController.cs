using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.Service;
using DBLayer.ViewModels;
using KrgWebAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Threading.Tasks;
using UtilityLayer;

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
        [HttpGet("getProductPhoto/{productCode}")]
        public async Task<IActionResult> GetProductPhoto(Guid productCode)
        {
            var (imageBytes, mimeType) = await _productService.fetchProductImageAsync(productCode);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                // Return 200 OK with empty string as body
                return Content(string.Empty, "text/plain");
            }

            // Serve bytes directly with appropriate mime type (jpeg, png, etc)
            return File(imageBytes, mimeType);
        }
        [HttpGet("getProductDetails/{productCode}")]
        public async Task<IActionResult> fetchProductDetails(Guid productCode)
        {
            var result = await _productService.fetchProductDetails(productCode);

            return StatusCode(200, new ApiResponse<Vproduct>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.ProductCode} records",
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
                    var imageBytes = ms.ToArray();
                    using (var img = Image.FromStream(new MemoryStream(imageBytes)))
                    {
                        string mimeType = ImageReader.GetMimeType(img.RawFormat);
                        Console.WriteLine($"Detected MIME type: {mimeType}");

                        // Save both imageBytes and mimeType to your database/entity
                        product.ProductLogo = imageBytes;
                        product.ProductLogoMime = mimeType;
                    }
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
        [HttpGet("fetchStockList/{productCode}")]
        public async Task<IActionResult> fetchStockList(Guid productCode)
        {
            var result = await _productService.fetchProductStockHistoryAsync(productCode);

            return StatusCode(200, new ApiResponse<List<Vstock>>
            {
                Code = 200,
                Message = $"Successfully Fetched {result.Count} records",
                Data = result
            });
        }
        [HttpPost("AddStockEntry")]
        public async Task<IActionResult> AddStockEntry([FromForm] VProductAddStock vProductAddStock)
        {
            var result = await _productService.AddStock(vProductAddStock.ProductCode, vProductAddStock.Quantity);
            if (!result)
            {
                return StatusCode(200, new ApiResponse<Guid>
                {
                    Code = 500,
                    Message = $"Adding Stock for {vProductAddStock.ProductCode}",
                    Data = vProductAddStock.ProductCode
                });
            }
            return StatusCode(200, new ApiResponse<bool>
            {
                Code = 200,
                Message = $"Removing Customer Succeed for {result}",
                Data = result
            });
        }
        //[HttpGet("{productCode}/stock")]
        //public async Task<IActionResult> FetchStockDetailsByProductCode(Guid productCode)
        //{
        //    var result = await _productService.FetchStockDetailsAsync(productCode);
        //    if (!result)
        //    {
        //        return StatusCode(200, new ApiResponse<decimal>
        //        {
        //            Code = 500,
        //            Message = $"Adding Stock for {vProductAddStock.ProductCode}",
        //            Data = vProductAddStock.ProductCode
        //        });
        //    }
        //    return StatusCode(200, new ApiResponse<bool>
        //    {
        //        Code = 200,
        //        Message = $"Removing Customer Succeed for {result}",
        //        Data = result
        //    });
        //}
    }
}
