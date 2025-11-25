using DBLayer.Models;
using DBLayer.Service.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface IInvoiceService
    {
        public Task<List<Vinvoice>> fetchInvoiceList();
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
        public async Task<Vinvoice> AddInvoiceAsync(InvInvoice invoiceEntity, List<InvInvoiceItem> products)
        {
            var claims = _userClaimsService.GetUserClaims();
            invoiceEntity.CreatedBy = claims.UserCode;

            await _context.InvInvoices.AddAsync(invoiceEntity);
            await _context.SaveChangesAsync();



            return await _context.Vinvoices.Where(C => C.InvoiceCode == invoiceEntity.InvoiceCode).FirstOrDefaultAsync();
        }
    }
}
