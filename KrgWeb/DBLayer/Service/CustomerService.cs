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
        public Task<Vcustomer> AddCustomer(InvCustomer customer);
    }
    public class CustomerService : ICustomerService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;
        public CustomerService(NsinvoiceBillingContext context, IUserClaimsService iUserClaimsService) 
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
        }
        public async Task<List<Vcustomer>> fetchCustomerList()
        {
            var customers = await _context.Vcustomers.ToListAsync();
            return customers;
        }
        public async Task<Vcustomer> AddCustomer(InvCustomer customer)
        { 
            await _context.InvCustomers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return await _context.Vcustomers.Where(C => C.CustomerCode == customer.CustomerCode).FirstOrDefaultAsync();
        }
    }
}
