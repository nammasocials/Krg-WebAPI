using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.Service.Authentication;
using DBLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLayer;

namespace DBLayer.Service
{
    public interface IInvoiceService
    {
        public Task<List<Vinvoice>> fetchInvoiceList();
        public Task<Vinvoice> AddInvoiceAsync(VInvoiceInput invoice);
        public Task<VinvoiceDetail?> fetchInvoiceDetails(Guid invoiceCode);
    }
    public class InvoiceService : IInvoiceService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public InvoiceService(
            NsinvoiceBillingContext context, 
            IUserClaimsService iUserClaimsService, 
            IProductService iProductService,
            ICustomerService iCustomerService)
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
            _customerService = iCustomerService;
            _productService = iProductService;
        }
        public async Task<List<Vinvoice>> fetchInvoiceList()
        {
            var invoiceList = await _context.Vinvoices.ToListAsync();
            return invoiceList;
        }
        public async Task<VinvoiceDetail?> fetchInvoiceDetails(Guid invoiceCode)
        {
            var invoiceDetails = await _context.VinvoiceDetails.
                Where(I => I.InvoiceCode == invoiceCode).FirstOrDefaultAsync();
            return invoiceDetails;
        }
        public async Task<Vinvoice> AddInvoiceAsync(VInvoiceInput invoice)
        {

            var claims = _userClaimsService.GetUserClaims();
            var invoiceEntity = InvoiceMapper.ToEntity(invoice);
            invoiceEntity.CreatedBy = claims.UserCode;
            var customer = await _customerService.fetchCustomerDetails(invoiceEntity.CustomerCode);
            invoiceEntity.Gst = customer.Gst;
            if (invoiceEntity.EwayBillLogo != null)
            {
                using (var ms = new MemoryStream())
                {
                    await invoice.EwayBillLogo.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();
                    using (var img = Image.FromStream(new MemoryStream(imageBytes)))
                    {
                        string mimeType = ImageReader.GetMimeType(img.RawFormat);
                        Console.WriteLine($"Detected MIME type: {mimeType}");

                        // Save both imageBytes and mimeType to your database/entity
                        invoiceEntity.EwayBillLogo = imageBytes;
                        invoiceEntity.EwayBillLogoMime = mimeType;
                    }
                }
            }

            await _context.InvInvoices.AddAsync(invoiceEntity);
            await _context.SaveChangesAsync();

            await AddInvoiceItemsAsync(invoiceEntity.InvoiceCode,invoiceEntity.InvoiceNo, invoice.InvoiceItems);

            return await _context.Vinvoices.Where(C => C.InvoiceCode == invoiceEntity.InvoiceCode).FirstOrDefaultAsync();
        }

        public async Task<List<VinvoiceDetail>> AddInvoiceItemsAsync(Guid invoiceCode , string invoiceNo,List<VInvoiceProductsInput> items)
        {
            var claims = _userClaimsService.GetUserClaims();

            var itemEntities = InvoiceItemsMapper.ToEntity(items);

            foreach (var item in itemEntities)
            {
                item.InvoiceCode = invoiceCode;
                var product = await _productService.fetchProductEntityByCodeAsync(item.ProductCode);
                if (product != null)
                {
                    item.InvoiceNo = invoiceNo;
                    item.Hsncode = product.Hsncode;
                    item.UnitCost = product.UnitCost;
                    item.Cost = product.UnitCost * item.Quantity;
                    item.StateGst = product.StateGstPer;
                    item.CentralGst = product.CentralGstPer;
                }
            }
            _context.InvInvoiceItems.AddRange(itemEntities);
            await _context.SaveChangesAsync();

            return await _context.VinvoiceDetails.Where(C => C.InvoiceCode == invoiceCode).ToListAsync();
        }
    }
}
