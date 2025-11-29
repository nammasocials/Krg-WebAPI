using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.ViewModels
{
    public class VInvoiceInput
    {
        public string InvoiceNo { get; set; } = null!;

        public Guid CustomerCode { get; set; }

        public bool IsEwayBillAvailable { get; set; }

        public IFormFile? EwayBillLogo { get; set; }
        public string? EwayBillLogoMime { get; set; }
        public List<VInvoiceProductsInput> InvoiceItems { get; set; }
    }
    public class VInvoiceProductsInput
    {
        public Guid InvoiceCode { get; set; }

        public Guid ProductCode { get; set; }

        public int Quantity { get; set; }
    }
}
