using System;

namespace DBLayer.ViewModels
{
    /// <summary>
    /// One filed expense. A normal expense sends only the first four fields; an
    /// invoice based one additionally sets <see cref="InvoiceCode"/> (our own
    /// invoice), the vendor bill fields, or both.
    /// </summary>
    public class VExpenseInput
    {
        public Guid? ExpenseCode { get; set; }

        public int ExpenseType { get; set; }

        public DateTime ExpenseDate { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public Guid? InvoiceCode { get; set; }

        public string? VendorName { get; set; }

        public string? VendorInvoiceNo { get; set; }

        public DateTime? VendorInvoiceDate { get; set; }
    }

    /// <summary>Date-range filter shared by the expense list and the reports.</summary>
    public class VDateRangeFilter
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        /// <summary>
        /// The lower bound for an open-ended "from". DateTime.MinValue cannot be
        /// used: SQL Server's DATETIME starts at 1753-01-01 and anything earlier
        /// fails the parameter conversion with SqlDateTime overflow.
        /// </summary>
        private static readonly DateTime SqlDateTimeMin = new DateTime(1753, 1, 1);

        /// <summary>
        /// Resolves the range to concrete bounds. An open start reaches back to the
        /// earliest date SQL Server can hold; an open end becomes the end of today.
        /// The upper bound is pushed to the end of its day so a same-day filter
        /// includes everything filed that day.
        /// </summary>
        public (DateTime From, DateTime To) Resolve()
        {
            var from = FromDate?.Date ?? SqlDateTimeMin;
            if (from < SqlDateTimeMin)
            {
                from = SqlDateTimeMin;
            }

            var to = (ToDate?.Date ?? DateTime.Today).AddDays(1).AddTicks(-1);
            return (from, to);
        }
    }
}
