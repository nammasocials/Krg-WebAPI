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
    }
    public class InvoiceService : IInvoiceService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;
        public InvoiceService(NsinvoiceBillingContext context, IUserClaimsService iUserClaimsService)
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
        }
        public async Task<List<Vinvoice>> fetchInvoiceList()
        {
            var invoiceList = await _context.Vinvoices.ToListAsync();
            return invoiceList;
        }
        public async Task<Vinvoice> AddInvoiceAsync(VInvoiceInput invoice)
        {

            var claims = _userClaimsService.GetUserClaims();
            var invoiceEntity = InvoiceMapper.ToEntity(invoice);
            invoiceEntity.CreatedBy = claims.UserCode;
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

            await AddInvoiceItemsAsync(invoiceEntity.InvoiceCode, invoice.InvoiceItems);

            return await _context.Vinvoices.Where(C => C.InvoiceCode == invoiceEntity.InvoiceCode).FirstOrDefaultAsync();
        }

        public async Task<List<VinvoiceDetail>> AddInvoiceItemsAsync(Guid invoiceCode ,List<VInvoiceProductsInput> items)
        {
            var claims = _userClaimsService.GetUserClaims();

            foreach (var item in items)
            {
                item.InvoiceCode = invoiceCode;
            }
            var itemEntities = InvoiceItemsMapper.ToEntity(items);
            _context.InvInvoiceItems.AddRange(itemEntities);
            await _context.SaveChangesAsync();

            return await _context.VinvoiceDetails.Where(C => C.InvoiceCode == invoiceCode).ToListAsync();
        }
    }
}
