using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.Service.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface ICustomerService
    {
        public Task<List<Vcustomer>> fetchCustomerList();
        public Task<Vcustomer> fetchCustomerDetails(int customerCode);
        public Task<(byte[] ImageData, string MimeType)> fetchCustomerImageAsync(int customerCode);
        public Task<bool> deleteCustomer(int customerId);
        public Task<Vcustomer> AddOrEditCustomer(InvCustomer customer, bool isEdit);
    }
    public class CustomerService : ICustomerService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;
        private readonly IRecentActivityService _recentActivityService;
        public CustomerService(NsinvoiceBillingContext context, IUserClaimsService iUserClaimsService
            IRecentActivityService iRecentActivityService) 
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
            _recentActivityService = iRecentActivityService;
        }
        public async Task<List<Vcustomer>> fetchCustomerList()
        {
            var customers = await _context.Vcustomers.ToListAsync();
            return customers;
        }
        public async Task<Vcustomer> fetchCustomerDetails(int customerCode)
        {
            var customers = await _context.Vcustomers.Where(C => C.CustomerCode == customerCode).FirstOrDefaultAsync();
            return customers;
        }
        public async Task<(byte[] ImageData, string MimeType)> fetchCustomerImageAsync(int customerCode)
        {
            var customerPhoto = await _context.InvCustomers
                .Where(c => c.CustomerCode == customerCode)
                .Select(c => new { c.CustomerLogo, c.CustomerLogoMime })  // Assuming you store MIME type
                .FirstOrDefaultAsync();

            if (customerPhoto == null || customerPhoto.CustomerLogo == null)
                return (null, null);

            return (customerPhoto.CustomerLogo, customerPhoto.CustomerLogoMime ?? "image/jpeg");
        }


        public async Task<Vcustomer> AddOrEditCustomer(InvCustomer customer, bool isEdit)
        {
            var claims = _userClaimsService.GetUserClaims();
            if (isEdit)
            {
                var customerToEdit = await _context.InvCustomers
                    .FirstOrDefaultAsync(c => c.CustomerCode == customer.CustomerCode);

                if (customerToEdit != null)
                {
                    var createdOn = customerToEdit.CreatedOn;

                    

                    _context.Entry(customerToEdit).CurrentValues.SetValues(customer);
                    customerToEdit.CreatedOn = createdOn;
                    customerToEdit.ModifiedOn = DateTime.Now;
                    customerToEdit.ModifiedBy = claims.UserCode;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                customer.CreatedOn = DateTime.Now;
                customer.CreatedBy = claims.UserCode;
                await _context.InvCustomers.AddAsync(customer);
                await _context.SaveChangesAsync();
            }

            return await _context.Vcustomers.Where(C => C.CustomerCode == customer.CustomerCode).FirstOrDefaultAsync();
        }

        public async Task<bool> deleteCustomer(int customerId)
        {
            var customerToDelete = await _context.InvCustomers
                    .FirstOrDefaultAsync(c => c.CustomerCode == customerId);

            if (customerToDelete != null)
            {
                _context.InvCustomers.Remove(customerToDelete);
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
