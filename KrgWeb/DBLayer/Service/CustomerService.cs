using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.Service.Authentication;
using DBLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLayer;

namespace DBLayer.Service
{
    public interface ICustomerService
    {
        public Task<List<Vcustomer>> fetchCustomerList();
        public Task<Vcustomer> fetchCustomerDetails(Guid customerCode);
        public Task<(byte[] ImageData, string MimeType)> fetchCustomerImageAsync(Guid customerCode);
        public Task<bool> deleteCustomer(Guid customerId);
        public Task<Vcustomer> AddOrEditCustomer(InvCustomer customer, bool isEdit);
        public Task<VDashboardStats> fetchDashboardCustomerStats();
    }
    public class CustomerService : ICustomerService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;
        private readonly ICustomerCacheService _cacheService;
        private readonly IRecentActivityService _recentActivityService;
        public CustomerService(NsinvoiceBillingContext context, IUserClaimsService iUserClaimsService,
            IRecentActivityService iRecentActivityService, ICustomerCacheService customerCache) 
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
            _recentActivityService = iRecentActivityService;
            _cacheService = customerCache;
        }
        public async Task<List<Vcustomer>> fetchCustomerList()
        {
            var customers = await _cacheService.GetCurrentCustomersAsync();
            if (customers == null || customers.Count <= 0)
            {
                customers = await _context.Vcustomers.ToListAsync();

            }
            return customers;
        }
        public async Task<VDashboardStats> fetchDashboardCustomerStats()
        {
            var result = new VDashboardStats();
            result.OverallCount = await _context.Vcustomers.CountAsync();
            result.RecentAddedCount = await _context.Vcustomers.
                Where(R => R.CreatedOn.Date == DateTime.Today).CountAsync();
            return result;
        }
        public async Task<Vcustomer> fetchCustomerDetails(Guid customerCode)
        {
            var customers = await _context.Vcustomers.Where(C => C.CustomerCode == customerCode).FirstOrDefaultAsync();
            return customers;
        }
        public async Task<(byte[] ImageData, string MimeType)> fetchCustomerImageAsync(Guid customerCode)
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
                    var ignoredColumns = new List<string>{ "ModifiedOn", "ModifiedBy", "CreatedOn", "CreatedBy" };
                    var modifiedColumns = DifferenceFinder.GetDifferentProperties<InvCustomer>(customerToEdit, customer, ignoredColumns);

                    _context.Entry(customerToEdit).CurrentValues.SetValues(customer);
                    customerToEdit.CreatedOn = createdOn;
                    customerToEdit.ModifiedOn = DateTime.Now;
                    customerToEdit.ModifiedBy = claims.UserCode;

                    await _context.SaveChangesAsync();
                    var description = string.Join(", ", modifiedColumns) + $" For Customer {customerToEdit.CustomerName}";

                    await _recentActivityService.AddActivityLogAsync("Customer", customerToEdit.CustomerCode
                        , "Update", description);
                }
            }
            else
            {
                customer.CreatedOn = DateTime.Now;
                customer.CreatedBy = claims.UserCode;
                await _context.InvCustomers.AddAsync(customer);
                await _context.SaveChangesAsync();
                await _recentActivityService.AddActivityLogAsync("Customer", customer.CustomerCode
                    , "Insert", $"New Customer - Name : {customer.CustomerName} has been added");
            }

            return await _context.Vcustomers.Where(C => C.CustomerCode == customer.CustomerCode).FirstOrDefaultAsync();
        }

        public async Task<bool> deleteCustomer(Guid customerId)
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
