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
        public Task<bool> deleteCustomer(int customerId);
        public Task<Vcustomer> AddOrEditCustomer(InvCustomer customer, bool isEdit);
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
        public async Task<Vcustomer> AddOrEditCustomer(InvCustomer customer, bool isEdit)
        {
            if (isEdit)
            {
                var customerToEdit = await _context.InvCustomers
                    .FirstOrDefaultAsync(c => c.CustomerCode == customer.CustomerCode);

                if (customerToEdit != null)
                {
                    var createdOn = customerToEdit.CreatedOn;
                    _context.Entry(customerToEdit).CurrentValues.SetValues(customer);
                    customerToEdit.CreatedOn = createdOn;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
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
