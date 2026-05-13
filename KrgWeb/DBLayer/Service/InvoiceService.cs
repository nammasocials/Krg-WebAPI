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
<<<<<<< Updated upstream

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
=======
        public InvoiceService(NsinvoiceBillingContext context, 
            IUserClaimsService iUserClaimsService,
            ICustomerService customerService,
            IProductService productService)
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
            _customerService = customerService;
            _productService = productService;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            var customer = await _customerService.fetchCustomerDetails(invoiceEntity.CustomerCode);
=======
            var customer = await _customerService.fetchCustomerDetails(invoice.CustomerCode);
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            await AddInvoiceItemsAsync(invoiceEntity.InvoiceCode,invoiceEntity.InvoiceNo, invoice.InvoiceItems);
=======
            await AddInvoiceItemsAsync(invoiceEntity, invoice.InvoiceItems);
>>>>>>> Stashed changes

            return await _context.Vinvoices.Where(C => C.InvoiceCode == invoiceEntity.InvoiceCode).FirstOrDefaultAsync();
        }

<<<<<<< Updated upstream
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
=======
        public async Task<List<VinvoiceDetail>> AddInvoiceItemsAsync(InvInvoice invoice ,List<VInvoiceProductsInput> items)
        {
            var claims = _userClaimsService.GetUserClaims();
            var itemEntities = InvoiceItemsMapper.ToEntity(items);
            foreach (var item in itemEntities)
            {
                item.InvoiceCode = invoice.InvoiceCode;
                item.InvoiceNo = invoice.InvoiceNo;
                var productDetails = await _productService.fetchProductDetails(item.ProductCode);
                item.Hsncode = productDetails.Hsncode;
                item.UnitCost = productDetails.UnitCost;
                item.CentralGst = productDetails.CentralGstPer;
                item.StateGst = productDetails.CentralGstPer;
                item.Cost = item.Quantity * productDetails.UnitCost;
                item.CentralGstAmount = (productDetails.CentralGstPer * item.Cost) / 100;
                item.StateGstAmount = (productDetails.StateGstPer * item.Cost) / 100;
                item.NetProductAmount = item.Cost + item.CentralGstAmount + item.StateGstAmount;
>>>>>>> Stashed changes
            }
            _context.InvInvoiceItems.AddRange(itemEntities);
            await _context.SaveChangesAsync();

            foreach (var item in itemEntities)
            {
                await _productService.UpdateStock(item.ProductCode, item.Quantity);
            }
            
            return await _context.VinvoiceDetails.Where(C => C.InvoiceCode == invoice.InvoiceCode).ToListAsync();
        }
    }
}
