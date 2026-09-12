using DBLayer.Models;
using DBLayer.Service.Authentication;
using DBLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface IExpenseService
    {
        public Task<List<Vexpense>> fetchExpenseList(VDateRangeFilter? filter = null);
        public Task<Vexpense?> fetchExpenseDetails(Guid expenseCode);
        public Task<List<Vconstant>> fetchExpenseTypes();
        public Task<Vexpense?> AddOrEditExpense(VExpenseInput expense, bool isEdit);
        public Task<bool> deleteExpense(Guid expenseCode);
    }

    public class ExpenseService : IExpenseService
    {
        /// <summary>Matches the seeded [dbo].[InvConstant] rows for expense types.</summary>
        private const string ExpenseEntityId = "InvExpenses";
        private const string ExpenseTypeCategory = "ExpenseType";

        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;

        public ExpenseService(NsinvoiceBillingContext context, IUserClaimsService iUserClaimsService)
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
        }

        public async Task<List<Vexpense>> fetchExpenseList(VDateRangeFilter? filter = null)
        {
            var query = _context.Vexpenses.Where(e => e.IsActive);

            if (filter != null)
            {
                var (from, to) = filter.Resolve();
                query = query.Where(e => e.ExpenseDate >= from && e.ExpenseDate <= to);
            }

            return await query.OrderByDescending(e => e.ExpenseDate).ToListAsync();
        }

        public async Task<Vexpense?> fetchExpenseDetails(Guid expenseCode)
        {
            return await _context.Vexpenses
                .Where(e => e.ExpenseCode == expenseCode)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Vconstant>> fetchExpenseTypes()
        {
            return await _context.Vconstants
                .Where(c => c.EntityId == ExpenseEntityId
                         && c.Category == ExpenseTypeCategory
                         && c.IsActive)
                .OrderBy(c => c.Key)
                .ToListAsync();
        }

        public async Task<Vexpense?> AddOrEditExpense(VExpenseInput expense, bool isEdit)
        {
            var claims = _userClaimsService.GetUserClaims();

            InvExpense entity;
            if (isEdit)
            {
                entity = await _context.InvExpenses
                    .Where(e => e.ExpenseCode == expense.ExpenseCode)
                    .FirstOrDefaultAsync()
                    ?? throw new InvalidOperationException($"Expense {expense.ExpenseCode} was not found.");

                entity.ModifiedBy = claims.UserCode;
                entity.ModifiedOn = DateTime.Now;
            }
            else
            {
                entity = new InvExpense
                {
                    ExpenseCode = Guid.NewGuid(),
                    IsActive = true,
                    CreatedBy = claims.UserCode,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = claims.UserCode,
                    ModifiedOn = DateTime.Now
                };
                await _context.InvExpenses.AddAsync(entity);
            }

            entity.ExpenseType = expense.ExpenseType;
            entity.ExpenseDate = expense.ExpenseDate == default ? DateTime.Now : expense.ExpenseDate;
            entity.Amount = expense.Amount;
            entity.Description = expense.Description;

            // Both links are optional and independent: an expense can belong to one of
            // our invoices, arrive on a vendor bill, both, or neither.
            entity.InvoiceCode = expense.InvoiceCode;
            entity.VendorName = Blank(expense.VendorName);
            entity.VendorInvoiceNo = Blank(expense.VendorInvoiceNo);
            entity.VendorInvoiceDate = expense.VendorInvoiceDate;

            await _context.SaveChangesAsync();

            return await fetchExpenseDetails(entity.ExpenseCode);
        }

        /// <summary>Soft delete, so the audit trail and any report history stay intact.</summary>
        public async Task<bool> deleteExpense(Guid expenseCode)
        {
            var claims = _userClaimsService.GetUserClaims();

            var entity = await _context.InvExpenses
                .Where(e => e.ExpenseCode == expenseCode)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.ModifiedBy = claims.UserCode;
            entity.ModifiedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>An empty box on the form means "no vendor bill", not an empty string.</summary>
        private static string? Blank(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
