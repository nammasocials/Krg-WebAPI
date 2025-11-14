using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLayer.Models;
using DBLayer.Service.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Service
{
    public interface IProductService
    {
        public Task<List<Vproduct>> fetchProductList();
        public Task<(byte[] ImageData, string MimeType)> fetchProductImageAsync(Guid productCode);
        public Task<Vproduct> fetchProductDetails(Guid productCode);
        public Task<bool> deleteProduct(Guid productId);
        public Task<Vproduct> AddOrEditProduct(InvProduct product, bool isEdit);
    }
    public class ProductService : IProductService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;
        public ProductService(NsinvoiceBillingContext context, IUserClaimsService iUserClaimsService)
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
        }
        public async Task<List<Vproduct>> fetchProductList()
        {
            var products = await _context.Vproducts.ToListAsync();
            return products;
        }
        public async Task<(byte[] ImageData, string MimeType)> fetchProductImageAsync(Guid productCode)
        {
            var productPhoto = await _context.InvProducts
                .Where(c => c.ProductCode == productCode)
                .Select(c => new { c.ProductLogo, c.ProductLogoMime })   // Assuming you store MIME type
                .FirstOrDefaultAsync();

            if (productPhoto == null || productPhoto.ProductLogo == null)
                return (null, null);

            return (productPhoto.ProductLogo, productPhoto.ProductLogoMime ?? "image/jpeg");
        }
        public async Task<Vproduct> fetchProductDetails(Guid productCode)
        {
            var product = await _context.Vproducts.Where(C => C.ProductCode == productCode).FirstOrDefaultAsync();
            return product;
        }
        public async Task<Vproduct> AddOrEditProduct(InvProduct product, bool isEdit)
        {
            if (isEdit)
            {
                var productToEdit = await _context.InvProducts
                    .FirstOrDefaultAsync(c => c.ProductCode == product.ProductCode);

                if (productToEdit != null)
                {
                    var createdOn = productToEdit.CreatedOn;
                    _context.Entry(productToEdit).CurrentValues.SetValues(product);
                    productToEdit.CreatedOn = createdOn;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                await _context.InvProducts.AddAsync(product);
                await _context.SaveChangesAsync();
            }

            return await _context.Vproducts.Where(C => C.ProductCode == product.ProductCode).FirstOrDefaultAsync();
        }
        public async Task<bool> deleteProduct(Guid productId)
        {
            var productToDelete = await _context.InvProducts
                    .FirstOrDefaultAsync(c => c.ProductCode == productId);

            if (productToDelete != null)
            {
                _context.InvProducts.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
